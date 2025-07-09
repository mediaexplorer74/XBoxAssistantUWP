// *********************************************************
// Type: Xbox.Live.Phone.Services.AvatarStore
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services
{
  public class AvatarStore : INotifyPropertyChanged
  {
    public const int MaxPromotedStores = 4;
    private const int CacheExpirationTimeInSeconds = 600;
    private static AvatarStore staticInstance;
    private List<StoreData> promotedStores;
    private bool isPromotedStoreFrontDisabled;
    private AtomicWrapper<bool> isLoading;
    private IMarketplaceServiceManager marketplaceServiceManager;
    private DateTime lastUpdated;

    private AvatarStore()
    {
      this.isLoading = new AtomicWrapper<bool>(false);
      this.marketplaceServiceManager = Marketplace.Instance.MarketplaceServiceManager;
      this.marketplaceServiceManager.EventGetStoreDataCompleted += new EventHandler<ServiceProxyEventArgs<List<StoreData>>>(this.OnComplete);
      this.promotedStores = new List<StoreData>(4);
      this.IsPromotedStoreFrontDisabled = true;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public event EventHandler EventPopulatePromotedStoreFrontCompleted;

    public static AvatarStore Instance
    {
      get
      {
        if (AvatarStore.staticInstance == null)
          AvatarStore.staticInstance = new AvatarStore();
        return AvatarStore.staticInstance;
      }
    }

    public bool IsPromotedStoreFrontDisabled
    {
      get => this.isPromotedStoreFrontDisabled;
      set
      {
        this.isPromotedStoreFrontDisabled = value;
        this.OnPropertyChanged(nameof (IsPromotedStoreFrontDisabled));
      }
    }

    public List<StoreData> PromotedStores
    {
      get => this.promotedStores;
      set
      {
        this.promotedStores = value;
        this.OnPropertyChanged(nameof (PromotedStores));
      }
    }

    private bool IsExpired => (DateTime.UtcNow - this.lastUpdated).TotalSeconds > 600.0;

    public void PopulatePromotedStores()
    {
      if (!this.isLoading.Value && this.IsExpired)
      {
        this.isLoading.ForceSet(true);
        this.IsPromotedStoreFrontDisabled = true;
        this.marketplaceServiceManager.GetStoresAsync();
      }
      else
      {
        if (this.EventPopulatePromotedStoreFrontCompleted == null)
          return;
        this.EventPopulatePromotedStoreFrontCompleted((object) this, (EventArgs) null);
      }
    }

    private void OnComplete(object sender, ServiceProxyEventArgs<List<StoreData>> e)
    {
      this.lastUpdated = DateTime.UtcNow;
      this.isLoading.ForceSet(false);
      if (e != null && e.ResultAvailable && e.Result != null)
      {
        this.promotedStores = new List<StoreData>(e.Result.Count);
        foreach (StoreData storeData in e.Result)
          this.promotedStores.Add(storeData);
      }
      else
        this.promotedStores = new List<StoreData>(4);
      for (int count = this.promotedStores.Count; count < 4; ++count)
        this.promotedStores.Add(new StoreData());
      this.OnPropertyChanged("PromotedStores");
      this.IsPromotedStoreFrontDisabled = false;
      if (this.EventPopulatePromotedStoreFrontCompleted == null)
        return;
      this.EventPopulatePromotedStoreFrontCompleted((object) this, (EventArgs) null);
    }

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
