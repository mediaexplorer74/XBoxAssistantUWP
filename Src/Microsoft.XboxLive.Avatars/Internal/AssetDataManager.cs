// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AssetDataManager
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.Collections.Generic;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class AssetDataManager : DataManagerBase, IDataManager
  {
    private AssetStorageDataProvider assetStorageProvider;
    private Queue<IDataProvider> assetDataProviders = new Queue<IDataProvider>();
    private Queue<IDataProvider> manifestDataProviders = new Queue<IDataProvider>();

    public AssetDataManager() => this.InitializeThread();

    public void AddAssetProvider(IDataProvider dataProvider)
    {
      if (dataProvider == null)
        throw new ArgumentNullException(nameof (dataProvider));
      this.assetDataProviders.Enqueue(dataProvider);
    }

    public void AddManifestProvider(IDataProvider dataProvider)
    {
      if (dataProvider == null)
        throw new ArgumentNullException(nameof (dataProvider));
      this.manifestDataProviders.Enqueue(dataProvider);
    }

    public void ClearAssetProviders() => this.assetDataProviders.Clear();

    public void ClearManifestProviders() => this.manifestDataProviders.Clear();

    public bool IsCaching
    {
      get => this.assetStorageProvider != null;
      set
      {
        if (value)
        {
          if (this.assetStorageProvider != null)
            return;
          this.assetStorageProvider = new AssetStorageDataProvider();
          this.assetStorageProvider.LocalCache.IncreaseQuota += new EventHandler<IncreaseQuotaEventArgs>(this.localCache_IncreaseQuota);
        }
        else
        {
          if (this.assetStorageProvider != null)
            this.assetStorageProvider.LocalCache.IncreaseQuota -= new EventHandler<IncreaseQuotaEventArgs>(this.localCache_IncreaseQuota);
          this.assetStorageProvider = (AssetStorageDataProvider) null;
        }
      }
    }

    public event EventHandler<IncreaseQuotaEventArgs> IncreaseQuota;

    private void localCache_IncreaseQuota(object sender, IncreaseQuotaEventArgs e)
    {
      if (this.IncreaseQuota == null)
        return;
      this.IncreaseQuota((object) this, e);
    }

    private void DownloadOpenReadSyncCompleted(object sender, DataRequestCompletedEventArgs e)
    {
      SyncDownloadContext userState = e.UserState as SyncDownloadContext;
      userState.stream = e.Error == null ? (!e.Cancelled ? e.Result : (Stream) null) : (Stream) null;
      userState.syncEvent.Set();
    }

    private static DataProvider GetDataProvider(Queue<IDataProvider> dataProviders)
    {
      return new DataProvider(dataProviders);
    }

    public virtual void GetAssetAsync(
      Guid avatarAsset,
      DownloadRequestEventHandler handler,
      object context)
    {
      DataProvider dataProvider = AssetDataManager.GetDataProvider(this.assetDataProviders);
      this.EnqueueRequest((DataRequest) new DataRequestGuid(avatarAsset, dataProvider, handler, context), DownloadPriority.Normal);
    }

    public virtual void GetAssetAsync(
      string avatarAsset,
      DownloadRequestEventHandler handler,
      object context)
    {
      DataProvider dataProvider = AssetDataManager.GetDataProvider(this.assetDataProviders);
      this.EnqueueRequest((DataRequest) new DataRequestString(avatarAsset, dataProvider, handler, context), DownloadPriority.Normal);
    }

    public Stream DownloadManifest(string gamerTag)
    {
      using (SyncDownloadContext context = new SyncDownloadContext())
      {
        DataProvider dataProvider = AssetDataManager.GetDataProvider(this.manifestDataProviders);
        this.EnqueueRequest((DataRequest) new DataRequestString(gamerTag, dataProvider, new DownloadRequestEventHandler(this.DownloadOpenReadSyncCompleted), (object) context), DownloadPriority.High);
        context.syncEvent.WaitOne();
        return context.DetachStream();
      }
    }

    public void GetManifestAsync(
      string gamerTag,
      DownloadRequestEventHandler handler,
      object context)
    {
      DataProvider dataProvider = AssetDataManager.GetDataProvider(this.manifestDataProviders);
      this.EnqueueRequest((DataRequest) new DataRequestString(gamerTag, dataProvider, handler, context), DownloadPriority.Normal);
    }

    public override void OnFinishRequest(
      DataRequestCompletedEventArgs eventArguments,
      DataRequest dataRequest)
    {
      if (eventArguments.Error != null || eventArguments.Cancelled || !this.IsCaching || dataRequest.CacheWriteDisabled || !(dataRequest.DataSource.ProcessingProvider is NetDataProvider))
        return;
      AssetLocalCache localCache = this.assetStorageProvider.LocalCache;
      NetDataProvider processingProvider = (NetDataProvider) dataRequest.DataSource.ProcessingProvider;
      string assetId = (string) null;
      switch (dataRequest)
      {
        case DataRequestGuid _:
          assetId = ((DataRequestGuid) dataRequest).DataId.ToString();
          break;
        case DataRequestString _:
          assetId = ((DataRequestString) dataRequest).DataId.ToString();
          break;
      }
      string fullIdAddress = AssetStorageDataProvider.GetFullIdAddress(processingProvider.GetAddressFormat(dataRequest), assetId);
      long ticks = DateTime.Now.Ticks;
      localCache.Store(fullIdAddress, eventArguments.Result);
    }

    public void CleanCache() => AssetStorageDataProvider.CleanCache();

    public void IncreaseCacheQuotaTo(long newSize)
    {
      AssetStorageDataProvider.IncreaseCacheQuotaTo(newSize);
    }
  }
}
