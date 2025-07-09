// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AvatarAssetCacheManager
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Version1;
using System.Collections.Generic;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class AvatarAssetCacheManager
  {
    private Dictionary<int, AvatarAssetCache> m_AvatarAssetCache = new Dictionary<int, AvatarAssetCache>();
    private LinkedList<CachedAsset> assetCache = new LinkedList<CachedAsset>();
    private Dictionary<AssetCacheKey, LinkedListNode<CachedAsset>> assetCacheDistionary = new Dictionary<AssetCacheKey, LinkedListNode<CachedAsset>>();
    private int estimatedMemoryUsage;
    private int minCachedAssets = 2;

    public int MaximalMemoryUsage { get; set; }

    public AvatarAssetCacheManager(int cacheSize) => this.MaximalMemoryUsage = cacheSize;

    public void SetCacheSize(int cacheSize)
    {
      lock (this.m_AvatarAssetCache)
      {
        if (this.MaximalMemoryUsage > cacheSize)
        {
          this.MaximalMemoryUsage = cacheSize;
          this.CleanupCache();
        }
        else
          this.MaximalMemoryUsage = cacheSize;
      }
    }

    public AvatarAssetCache GetAssetCache(int version)
    {
      AvatarAssetCache assetCache;
      lock (this.m_AvatarAssetCache)
      {
        this.m_AvatarAssetCache.TryGetValue(version, out assetCache);
        if (assetCache == null)
        {
          if (version != 1)
            return (AvatarAssetCache) null;
          assetCache = (AvatarAssetCache) new AvatarAssetCacheV1();
          this.m_AvatarAssetCache.Add(version, assetCache);
        }
      }
      return assetCache;
    }

    private void CleanupCache()
    {
      int maximalMemoryUsage = this.MaximalMemoryUsage;
      while (this.assetCacheDistionary.Count > this.minCachedAssets && this.estimatedMemoryUsage > maximalMemoryUsage)
      {
        CachedAsset cachedAsset = this.assetCache.Last.Value;
        this.assetCacheDistionary.Remove(cachedAsset.AssetKey);
        this.estimatedMemoryUsage -= cachedAsset.MemoryUsage;
        this.assetCache.RemoveLast();
      }
    }

    public void AddToCache(CachedAsset asset)
    {
      lock (this.assetCache)
      {
        this.estimatedMemoryUsage += asset.MemoryUsage;
        this.CleanupCache();
        LinkedListNode<CachedAsset> node = new LinkedListNode<CachedAsset>(asset);
        this.assetCache.AddFirst(node);
        if (this.assetCacheDistionary.ContainsKey(asset.AssetKey))
          return;
        this.assetCacheDistionary.Add(asset.AssetKey, node);
      }
    }

    public LinkedListNode<CachedAsset> GetCachedAsset(AssetCacheKey key)
    {
      LinkedListNode<CachedAsset> cachedAsset;
      lock (this.assetCache)
        this.assetCacheDistionary.TryGetValue(key, out cachedAsset);
      return cachedAsset;
    }

    public void Renew(CachedAsset asset)
    {
      lock (this.assetCache)
      {
        if (this.assetCache.Count <= 1)
          return;
        LinkedListNode<CachedAsset> node = this.assetCacheDistionary[asset.AssetKey];
        this.assetCache.Remove(node);
        this.assetCache.AddFirst(node);
      }
    }

    public void ReleaseCache()
    {
      lock (this.assetCache)
      {
        this.assetCacheDistionary.Clear();
        this.assetCache.Clear();
        this.estimatedMemoryUsage = 0;
      }
    }

    public int GetEstimatedMemoryUsage() => this.estimatedMemoryUsage;

    public void AddAssetCache(AvatarAssetCache cache)
    {
      lock (this.m_AvatarAssetCache)
        this.m_AvatarAssetCache.Add(cache.GetVersion(), cache);
    }
  }
}
