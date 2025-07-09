// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.AvatarAssetCacheV1
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class AvatarAssetCacheV1 : AvatarAssetCache
  {
    private Dictionary<AssetCacheKey, AvatarAssetCacheV1.PendingAsset> pendingAssets = new Dictionary<AssetCacheKey, AvatarAssetCacheV1.PendingAsset>();

    internal AvatarAssetCacheV1()
    {
    }

    public override int GetVersion() => 1;

    private void DownloadAssetCompleted(object sender, DataRequestCompletedEventArgs e)
    {
      AvatarAssetCacheV1.CachedAssetsAsyncLoadContext userState = e.UserState as AvatarAssetCacheV1.CachedAssetsAsyncLoadContext;
      lock (userState.common.syncRequestLock)
      {
        --userState.common.numRequests;
        if (e.Error != null)
          userState.common.failed = true;
        else if (e.Cancelled)
          userState.common.failed = true;
        else
          userState.common.requestedAssets[userState.index].Stream = e.Result;
        if (userState.common.numRequests != 0)
          return;
        userState.common.syncEvent.Set();
      }
    }

    internal bool LoadAssets(
      List<BinaryAsset> assets,
      BinaryAssetParseContext parseContext,
      IDataManager dataManager)
    {
      int count1 = assets.Count;
      List<AvatarAssetCacheV1.PendingAsset> pendingAssetList = new List<AvatarAssetCacheV1.PendingAsset>();
      List<AvatarAssetCacheV1.PendingAssetPair> pendingAssetPairList = new List<AvatarAssetCacheV1.PendingAssetPair>();
      bool flag1 = false;
      AvatarAssetCacheManager assetCache = parseContext.m_AssetCache;
      lock (this.pendingAssets)
      {
        for (int index = 0; index < count1; ++index)
        {
          AssetCacheKey assetKey = assets[index].GetAssetKey(assets[index].AssetId, parseContext.m_CoordinateSystem, assets[index].m_SkeletonVersion);
          LinkedListNode<CachedAsset> cachedAsset = assetCache.GetCachedAsset(assetKey);
          if (cachedAsset != null)
          {
            assets[index].m_Cache = cachedAsset.Value as CachedBinaryAsset;
            assetCache.Renew(cachedAsset.Value);
          }
          else
          {
            AvatarAssetCacheV1.PendingAsset pendingAsset;
            this.pendingAssets.TryGetValue(assetKey, out pendingAsset);
            if (pendingAsset != null)
            {
              pendingAsset.AddReference();
              pendingAssetPairList.Add(new AvatarAssetCacheV1.PendingAssetPair(pendingAsset, assets[index]));
            }
            else
            {
              pendingAsset = new AvatarAssetCacheV1.PendingAsset(assets[index], index);
              this.pendingAssets.Add(assetKey, pendingAsset);
              pendingAssetList.Add(pendingAsset);
            }
          }
        }
      }
      int count2 = pendingAssetList.Count;
      if (count2 > 0)
      {
        AvatarAssetCacheV1.CachedAssetsAsyncLoadContextCommon loadContextCommon = new AvatarAssetCacheV1.CachedAssetsAsyncLoadContextCommon();
        loadContextCommon.syncRequestLock = new object();
        using (loadContextCommon.syncEvent = new AutoResetEvent(false))
        {
          loadContextCommon.numRequests = count2;
          loadContextCommon.requestedAssets = assets;
          for (int index = 0; index < count2; ++index)
          {
            BinaryAsset binaryAsset = pendingAssetList[index].m_BinaryAsset;
            CachedBinaryAsset cacheItem = binaryAsset.CreateCacheItem();
            binaryAsset.m_Cache = cacheItem;
            pendingAssetList[index].m_CacheItem = cacheItem;
            dataManager.GetAssetAsync(binaryAsset.AssetId, new DownloadRequestEventHandler(this.DownloadAssetCompleted), (object) new AvatarAssetCacheV1.CachedAssetsAsyncLoadContext()
            {
              index = pendingAssetList[index].m_OriginalIndex,
              common = loadContextCommon
            });
          }
          loadContextCommon.syncEvent.WaitOne();
        }
        flag1 = loadContextCommon.failed;
        for (int index = 0; index < count2; ++index)
        {
          BinaryAsset binaryAsset = pendingAssetList[index].m_BinaryAsset;
          AssetCacheKey assetKey = binaryAsset.GetAssetKey(binaryAsset.AssetId, parseContext.m_CoordinateSystem, binaryAsset.m_SkeletonVersion);
          CachedBinaryAsset cache = binaryAsset.m_Cache;
          if (binaryAsset.Stream == null)
          {
            Logger.Log((Log) new DebugLog((object) this, string.Format("Asset \"{0}\" could not be downloaded", (object) binaryAsset.AssetId)));
            cache.m_AssetState = CachedBinaryAsset.AssetState.Invalid;
            lock (this.pendingAssets)
            {
              pendingAssetList[index].DownloadCompleted(cache);
              this.pendingAssets.Remove(assetKey);
            }
          }
          else
          {
            bool flag2;
            try
            {
              flag2 = cache.Parse(binaryAsset.Stream, parseContext);
            }
            catch (Exception ex)
            {
              Logger.Log((Log) new DebugLog((object) this, string.Format("Error \"{0}\" occured during parsing of asset {1}", (object) ex.Message, (object) binaryAsset.AssetId)));
              flag2 = false;
            }
            if (flag2)
            {
              cache.m_AssetState = CachedBinaryAsset.AssetState.Parsed;
              lock (this.pendingAssets)
              {
                assetCache.AddToCache((CachedAsset) cache);
                pendingAssetList[index].DownloadCompleted(cache);
                this.pendingAssets.Remove(assetKey);
              }
            }
            else
            {
              cache.m_AssetState = CachedBinaryAsset.AssetState.Downloaded;
              lock (this.pendingAssets)
              {
                pendingAssetList[index].DownloadCompleted(cache);
                this.pendingAssets.Remove(assetKey);
              }
              flag1 = true;
            }
            binaryAsset.Stream.Close();
            binaryAsset.Stream = (Stream) null;
          }
        }
      }
      int count3 = pendingAssetPairList.Count;
      if (count3 > 0)
      {
        for (int index = 0; index < count3; ++index)
        {
          AvatarAssetCacheV1.PendingAsset pendingAsset = pendingAssetPairList[index].pendingAsset;
          pendingAsset.m_WaitEvent.WaitOne();
          if (pendingAsset.m_CacheItem.m_AssetState != CachedBinaryAsset.AssetState.Parsed)
            flag1 = true;
          pendingAssetPairList[index].binaryAsset.m_Cache = pendingAsset.m_CacheItem;
          pendingAsset.ReleaseReference();
        }
      }
      return !flag1;
    }

    private class CachedAssetsAsyncLoadContextCommon
    {
      public int numRequests;
      public bool failed;
      public AutoResetEvent syncEvent;
      public object syncRequestLock;
      public List<BinaryAsset> requestedAssets;
    }

    private class CachedAssetsAsyncLoadContext
    {
      public int index;
      public AvatarAssetCacheV1.CachedAssetsAsyncLoadContextCommon common = new AvatarAssetCacheV1.CachedAssetsAsyncLoadContextCommon();
    }

    private class PendingAsset : IDisposable
    {
      public int m_References;
      public CachedBinaryAsset m_CacheItem;
      public int m_OriginalIndex;
      public BinaryAsset m_BinaryAsset;
      public ManualResetEvent m_WaitEvent = new ManualResetEvent(false);

      public PendingAsset(BinaryAsset asset, int originalIndex)
      {
        this.m_References = 1;
        this.m_BinaryAsset = asset;
        this.m_OriginalIndex = originalIndex;
      }

      public void DownloadCompleted(CachedBinaryAsset parsedAsset)
      {
        this.m_CacheItem = parsedAsset;
        this.m_WaitEvent.Set();
        this.ReleaseReference();
      }

      public void ReleaseReference()
      {
        if (Interlocked.Decrement(ref this.m_References) != 0)
          return;
        this.m_WaitEvent.Close();
        this.m_WaitEvent = (ManualResetEvent) null;
        this.m_CacheItem = (CachedBinaryAsset) null;
      }

      public void AddReference() => Interlocked.Increment(ref this.m_References);

      protected virtual void Dispose(bool disposing)
      {
        if (disposing && this.m_WaitEvent != null)
          this.m_WaitEvent.Close();
        this.m_WaitEvent = (ManualResetEvent) null;
      }

      public void Dispose()
      {
        this.Dispose(true);
        GC.SuppressFinalize((object) this);
      }
    }

    private class PendingAssetPair
    {
      public AvatarAssetCacheV1.PendingAsset pendingAsset;
      public BinaryAsset binaryAsset;

      public PendingAssetPair(AvatarAssetCacheV1.PendingAsset pendingAsset, BinaryAsset binaryAsset)
      {
        this.pendingAsset = pendingAsset;
        this.binaryAsset = binaryAsset;
      }
    }
  }
}
