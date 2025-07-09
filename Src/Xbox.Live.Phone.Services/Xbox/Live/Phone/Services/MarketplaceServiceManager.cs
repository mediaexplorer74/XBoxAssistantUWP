// *********************************************************
// Type: Xbox.Live.Phone.Services.MarketplaceServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Microsoft.Phone.Marketplace;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using Xbox.Live.Phone.Utils;
using Xbox.Live.Phone.Utils.Cache;


namespace Xbox.Live.Phone.Services
{
  public sealed class MarketplaceServiceManager : IMarketplaceServiceManager, IDisposable
  {
    public const string AvatarStorePrefix = "AvatarStore";
    private const string ComponentName = "MarketPlaceServiceManager";
    private const string StoreUri = "https://epix-ssl{1}.xbox.com/epix/{0}/avatarstore.xml";
    private const string StorePrefix = "store:";
    private const string GamePrefix = "title:urn:uuid:";
    private AvatarOfferEnumerationRequest offerEnumRequest;
    private GameEnumerationRequest gameEnumRequest;
    private CategoryEnumerationRequest categoryEnumRequest;
    private AvatarPurchaseRequest purchaseRequest;
    private AvatarPurchaseHistoryRequest purchaseHistoryRequest;
    private GamerContextEx gamerContextEx;
    private ManualResetEvent offerEvent;
    private ManualResetEvent gamesEvent;
    private ManualResetEvent categoryEvent;
    private string currentEnvironmentPrefix = string.Empty;

    public event EventHandler<EventArgs> EventScreenShown;

    public event EventHandler<EventArgs> EventScreenHidden;

    public event EventHandler<ServiceProxyEventArgs<List<StoreData>>> EventGetStoreDataCompleted;

    public event EventHandler<ServiceProxyEventArgs<ProductResponse<Offer>>> EventGetAvatarItemsCompleted;

    public event EventHandler<ServiceProxyEventArgs<ProductResponse<Game>>> EventGetGameListCompleted;

    public event EventHandler<ServiceProxyEventArgs<ProductResponse<Category>>> EventGetCategoryListCompleted;

    public event EventHandler<ServiceProxyEventArgs<ProductResponse<Offer>>> EventGetOfferCompleted;

    public event EventHandler<ServiceProxyEventArgs<PurchaseReceipt>> EventPurchaseCompleted;

    public event EventHandler<ServiceProxyEventArgs<PurchaseReceipt>> EventGetPurchaseHistoryCompleted;

    public event EventHandler EventPurchaseCancelled;

    public event EventHandler<ServiceProxyEventArgs<bool>> EventGetGamerContextCompleted;

    public event EventHandler<ServiceProxyEventArgs<Guid>> EventUpdatedTransactionIdCompleted;

    public static List<StoreData> GetAvatarStoreList(XContainer doc)
    {
      List<StoreData> avatarStoreList = new List<StoreData>();
      if (doc != null)
      {
        avatarStoreList = doc.Descendants((XName) "slot").Where<XElement>((Func<XElement, bool>) (slot => slot.Element((XName) "metadata") != null && ((XElement) slot.Element((XName) "metadata").Elements((XName) "property").Where<XElement>((Func<XElement, bool>) (a => a.Element((XName) "name").Value == "phone")).Nodes<XElement>().ElementAt<XNode>(1)).Value == "true" && slot.Element((XName) "name") != null && slot.Element((XName) "description") != null && slot.Element((XName) "shallowimg") != null)).OrderBy<XElement, string>((Func<XElement, string>) (slot => ((XElement) slot.Element((XName) "metadata").Elements((XName) "property").Where<XElement>((Func<XElement, bool>) (a => a.Element((XName) "name").Value == "index")).Nodes<XElement>().ElementAt<XNode>(1)).Value)).Select<XElement, StoreData>((Func<XElement, StoreData>) (slot => new StoreData()
        {
          Name = slot.Element((XName) "name").Value,
          Description = slot.Element((XName) "description").Value,
          Image = slot.Element((XName) "shallowimg").Value,
          Parameter2 = slot.Descendants((XName) "param2").First<XElement>().Value,
          IsGameStore = slot.Descendants((XName) "param2").First<XElement>().Value.StartsWith("title:urn:uuid:", StringComparison.OrdinalIgnoreCase),
          Parameter2Value = MarketplaceServiceManager.GetParameter2Value(slot.Descendants((XName) "param2").First<XElement>().Value)
        })).Take<StoreData>(4).ToList<StoreData>();
        if (avatarStoreList.Count < 4)
        {
          List<StoreData> list = doc.Descendants((XName) "slot").Where<XElement>((Func<XElement, bool>) (slot =>
          {
            if (slot.Element((XName) "metadata") != null)
              return false;
            return slot.Descendants((XName) "param2").First<XElement>().Value.StartsWith("title:urn:uuid:", StringComparison.OrdinalIgnoreCase) || slot.Descendants((XName) "param2").First<XElement>().Value.StartsWith("store:", StringComparison.OrdinalIgnoreCase);
          })).Select<XElement, StoreData>((Func<XElement, StoreData>) (slot => new StoreData()
          {
            Name = slot.Element((XName) "name").Value,
            Description = slot.Element((XName) "description").Value,
            Image = slot.Element((XName) "shallowimg").Value,
            Parameter2 = slot.Descendants((XName) "param2").First<XElement>().Value,
            IsGameStore = slot.Descendants((XName) "param2").First<XElement>().Value.StartsWith("title:urn:uuid:", StringComparison.OrdinalIgnoreCase),
            Parameter2Value = MarketplaceServiceManager.GetParameter2Value(slot.Descendants((XName) "param2").First<XElement>().Value)
          })).Take<StoreData>(4 - avatarStoreList.Count).ToList<StoreData>();
          avatarStoreList.AddRange((IEnumerable<StoreData>) list);
        }
      }
      return avatarStoreList;
    }

    public void Initialize(ServiceCommon.Environment environmentName)
    {
      this.currentEnvironmentPrefix = ServiceCommon.GetEnvironmentUrlStringPrefix(environmentName);
      this.offerEnumRequest = new AvatarOfferEnumerationRequest();
      this.offerEnumRequest.EnumerateOffersCompleted += new EventHandler<EnumerateOffersCompletedEventArgs>(this.EnumerateOffersCompleted);
      this.gameEnumRequest = new GameEnumerationRequest();
      this.gameEnumRequest.EnumerateGamesCompleted += new EventHandler<EnumerateGamesCompletedEventArgs>(this.EnumerateGamesCompleted);
      this.categoryEnumRequest = new CategoryEnumerationRequest();
      this.categoryEnumRequest.EnumerateCategoriesCompleted += new EventHandler<EnumerateCategoriesCompletedEventArgs>(this.EnumerateCategoriesCompleted);
      this.purchaseHistoryRequest = new AvatarPurchaseHistoryRequest();
      this.purchaseHistoryRequest.EnumeratePurchaseHistoryCompleted += new EventHandler<PurchaseReceiptEventArgs>(this.EnumeratePurchaseHistoryCompleted);
      this.gamerContextEx = new GamerContextEx();
      this.gamerContextEx.GetGamerContextCompleted += new EventHandler<AsyncCompletedEventArgs>(this.GetGamerContextCompleted);
      this.offerEvent = new ManualResetEvent(false);
      this.gamesEvent = new ManualResetEvent(false);
      this.categoryEvent = new ManualResetEvent(false);
    }

    public void GetGamerContext() => this.gamerContextEx.GetGamerContextAsync();

    public void GetStoresAsync()
    {
      HttpWebRequestRetry httpWebRequestRetry = new HttpWebRequestRetry(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://epix-ssl{1}.xbox.com/epix/{0}/avatarstore.xml", new object[2]
      {
        (object) this.gamerContextEx.LegalLocale,
        (object) this.currentEnvironmentPrefix
      }), UriKind.Absolute));
      httpWebRequestRetry.ResponseAvailable += new EventHandler<HttpWebRequestRetryEventArgs>(this.OnResponseComplete);
      httpWebRequestRetry.BeginGetResponse();
    }

    public void GetCategoryListForLifeStyleAsync(
      int pageSize,
      int pageNumber,
      int categoryId,
      object userState)
    {
      if (this.categoryEnumRequest.IsBusy)
      {
        this.categoryEnumRequest.CancelAsync();
        this.categoryEvent.WaitOne();
      }
      this.categoryEvent.Reset();
      this.categoryEnumRequest.EnumerateCategoriesAsync(pageSize, pageNumber, categoryId, userState);
    }

    public void GetGameListForGameStyleAsync(int pageSize, int pageNumber, object userState)
    {
      if (this.gameEnumRequest.IsBusy)
      {
        this.gameEnumRequest.CancelAsync();
        this.gamesEvent.WaitOne();
      }
      this.gamesEvent.Reset();
      this.gameEnumRequest.EnumerateGamesAsync(pageSize, pageNumber, OrderingOption.SortTitle, OrderingDirection.Ascending, userState);
    }

    public void GetAvatarItemsByCategoryIdAsync(
      int titleId,
      int pageSize,
      int pageNumber,
      int categoryId,
      AvatarBodyType bodyType,
      object userState)
    {
      if (this.offerEnumRequest.IsBusy)
      {
        this.offerEnumRequest.CancelAsync();
        this.offerEvent.WaitOne();
      }
      this.offerEvent.Reset();
      this.offerEnumRequest.EnumerateOffersByCategoryIdAsync(titleId, pageSize, pageNumber, categoryId, MarketplaceServiceManager.ConvertToMarketplaceBodyType(bodyType), OrderingOption.SortTitle, OrderingDirection.Ascending, userState);
    }

    public void GetAvatarItemsByGameIdAsync(
      int titleId,
      int pageSize,
      int pageNumber,
      AvatarBodyType bodyType,
      Guid gameId,
      object userState)
    {
      if (this.offerEnumRequest.IsBusy)
      {
        this.offerEnumRequest.CancelAsync();
        this.offerEvent.WaitOne();
      }
      this.offerEvent.Reset();
      this.offerEnumRequest.EnumerateOffersByGameIdAsync(titleId, pageSize, pageNumber, MarketplaceServiceManager.ConvertToMarketplaceBodyType(bodyType), gameId, OrderingOption.SortTitle, OrderingDirection.Ascending, userState);
    }

    public void GetOfferByOfferIdAsync(int titleId, AvatarBodyType bodyType, Guid offerId)
    {
      if (this.offerEnumRequest.IsBusy)
      {
        this.offerEnumRequest.CancelAsync();
        this.offerEvent.WaitOne();
      }
      this.offerEvent.Reset();
      this.offerEnumRequest.EnumerateOffersByOfferIdAsync(titleId, 1, 1, MarketplaceServiceManager.ConvertToMarketplaceBodyType(bodyType), (IEnumerable<Guid>) new Guid[1]
      {
        offerId
      });
    }

    public void GetMostPopularAvatarItemsAsync(
      int titleId,
      int pageSize,
      int pageNumber,
      AvatarBodyType bodyType,
      object userState)
    {
      if (this.offerEnumRequest.IsBusy)
      {
        this.offerEnumRequest.CancelAsync();
        this.offerEvent.WaitOne();
      }
      this.offerEvent.Reset();
      this.offerEnumRequest.EnumerateOffersAsync(titleId, pageSize, pageNumber, MarketplaceServiceManager.ConvertToMarketplaceBodyType(bodyType), Guid.Empty, new int?(13001), OrderingOption.DownloadCountToday, OrderingDirection.Descending, userState);
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "need to catch all exception so that pdlc does not crash live app")]
    public void PurchaseAvatarItem(int titleId, AvatarBodyType bodyType, Offer offer)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        AvatarRedeemRequest.Blacklist = CatalogBlacklistProvider.Instance.BlacklistedAssetIds;
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          this.purchaseRequest = new AvatarPurchaseRequest();
          this.purchaseRequest.ShowPurchaseCompleted += new EventHandler<PurchaseCompletedEventArgs>(this.ShowPurchaseCompleted);
          this.purchaseRequest.ScreenShown += new EventHandler<EventArgs>(this.PurchaseRequest_ScreenShown);
          this.purchaseRequest.ScreenHidden += new EventHandler<EventArgs>(this.PurchaseRequest_ScreenHidden);
          try
          {
            Guid transactionId = Guid.Empty;
            LiveAppPage.CurrentPurchaseRequest = this.purchaseRequest;
            this.purchaseRequest.ShowPurchaseAsync(titleId, MarketplaceServiceManager.ConvertToMarketplaceBodyType(bodyType), offer, LiveAppPage.Current, out transactionId);
            EventHandler<ServiceProxyEventArgs<Guid>> transactionIdCompleted = this.EventUpdatedTransactionIdCompleted;
            ServiceProxyEventArgs<Guid> e = new ServiceProxyEventArgs<Guid>((object) transactionId, (Exception) null, false, (object) null);
            if (transactionIdCompleted == null)
              return;
            transactionIdCompleted((object) this, e);
          }
          catch (Exception ex)
          {
            if (this.EventPurchaseCompleted == null)
              return;
            this.EventPurchaseCompleted((object) this, new ServiceProxyEventArgs<PurchaseReceipt>((object) null, (Exception) ServiceCommon.HandleServiceException(ex, XLiveMobileExceptionEnum.FailedToPurchaseAvatarItems, "Failed to purchase", (string[]) null), false, (object) null));
          }
        }, (object) this);
      });
    }

    public void GetPurchaseHistory(int titleId, Guid transactionId)
    {
      this.purchaseHistoryRequest.EnumerateAvatarPurchaseHistoryAsync(titleId, 1, 1, (IEnumerable<Guid>) new Guid[1]
      {
        transactionId
      }, new DateTime?(), (object) null);
    }

    public void InvalidateGamerContext() => this.gamerContextEx.InvalidateGamerContext();

    public void Dispose()
    {
      this.offerEvent.Dispose();
      this.gamesEvent.Dispose();
      this.categoryEvent.Dispose();
    }

    private static string GetParameter2Value(string value)
    {
      int num = value.LastIndexOf(':');
      return num >= 0 ? value.Substring(num + 1) : (string) null;
    }

    private static MarketplaceAvatarBodyType ConvertToMarketplaceBodyType(
      AvatarBodyType avatarBodyType)
    {
      if (avatarBodyType == AvatarBodyType.Male)
        return MarketplaceAvatarBodyType.Male;
      return avatarBodyType == AvatarBodyType.Female ? MarketplaceAvatarBodyType.Female : MarketplaceAvatarBodyType.Unknown;
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Almost any type of exception caught here will be handled the same way.")]
    private void OnResponseComplete(object sender, HttpWebRequestRetryEventArgs e)
    {
      if (sender is HttpWebRequestRetry httpWebRequestRetry)
        httpWebRequestRetry.ResponseAvailable -= new EventHandler<HttpWebRequestRetryEventArgs>(this.OnResponseComplete);
      ServiceProxyEventArgs<List<StoreData>> e1 = (ServiceProxyEventArgs<List<StoreData>>) null;
      if (this.EventGetStoreDataCompleted == null)
        return;
      if (e.Response != null)
      {
        AvatarStoreCache avatarStoreCache = CacheManager.Load<AvatarStoreCache>("AvatarStore");
        if (avatarStoreCache == null || string.Compare(avatarStoreCache.LastModified, e.Response.Headers[HttpRequestHeader.LastModified], StringComparison.OrdinalIgnoreCase) != 0)
        {
          string s = CleanXmlUtil.ReadCleanedResponseXmlToString((WebResponse) e.Response);
          if (string.IsNullOrEmpty(s))
            return;
          using (StringReader stringReader = new StringReader(s))
          {
            List<StoreData> avatarStoreList = MarketplaceServiceManager.GetAvatarStoreList((XContainer) XDocument.Load((TextReader) stringReader));
            e1 = new ServiceProxyEventArgs<List<StoreData>>((object) avatarStoreList, (Exception) null, false, (object) null);
            if (avatarStoreCache == null)
              avatarStoreCache = new AvatarStoreCache("AvatarStore");
            avatarStoreCache.LastModified = e.Response.Headers[HttpRequestHeader.LastModified];
            avatarStoreCache.AvatarStores = avatarStoreList;
          }
          CacheManager.Instance.SaveAsync((CacheItem) avatarStoreCache);
        }
        else
          e1 = new ServiceProxyEventArgs<List<StoreData>>((object) avatarStoreCache.AvatarStores, (Exception) null, false, (object) null);
      }
      else
      {
        Exception exception = e.Exception;
        e1 = new ServiceProxyEventArgs<List<StoreData>>((object) null, e.Exception, false, (object) null);
      }
      this.EventGetStoreDataCompleted((object) this, e1);
    }

    private void EnumerateOffersCompleted(object sender, EnumerateOffersCompletedEventArgs e)
    {
      this.offerEvent.Set();
      EventHandler<ServiceProxyEventArgs<ProductResponse<Offer>>> eventHandler = e.UserState == null ? this.EventGetOfferCompleted : this.EventGetAvatarItemsCompleted;
      if (eventHandler == null)
        return;
      if (e != null && e.Error == null)
      {
        if (e.Cancelled)
        {
          ServiceProxyEventArgs<ProductResponse<Offer>> e1 = new ServiceProxyEventArgs<ProductResponse<Offer>>((object) null, (Exception) null, true, e.UserState);
          eventHandler((object) this, e1);
          return;
        }
        if (e.Offers != null)
        {
          ServiceProxyEventArgs<ProductResponse<Offer>> e2 = new ServiceProxyEventArgs<ProductResponse<Offer>>((object) new ProductResponse<Offer>()
          {
            TotalItems = e.TotalAvailable,
            ProductList = e.Offers.ToList<Offer>()
          }, (Exception) null, false, e.UserState);
          eventHandler((object) this, e2);
          return;
        }
      }
      ServiceProxyEventArgs<ProductResponse<Offer>> e3 = new ServiceProxyEventArgs<ProductResponse<Offer>>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToGetAvatarItems, "Failed to get avatar items", (string[]) null), false, (object) null);
      eventHandler((object) this, e3);
    }

    private void EnumerateGamesCompleted(object sender, EnumerateGamesCompletedEventArgs e)
    {
      this.gamesEvent.Set();
      EventHandler<ServiceProxyEventArgs<ProductResponse<Game>>> gameListCompleted = this.EventGetGameListCompleted;
      if (gameListCompleted == null)
        return;
      if (e != null && e.Error == null)
      {
        if (e.Cancelled)
        {
          ServiceProxyEventArgs<ProductResponse<Game>> e1 = new ServiceProxyEventArgs<ProductResponse<Game>>((object) null, (Exception) null, true, e.UserState);
          gameListCompleted((object) this, e1);
          return;
        }
        if (e.Games != null)
        {
          List<Game> gameList = new List<Game>(e.Games.Count);
          List<Game> list = e.Games.Select<Microsoft.Phone.Marketplace.Product, Game>((Func<Microsoft.Phone.Marketplace.Product, Game>) (product =>
          {
            return new Game()
            {
              GameId = product.GameId,
              Title = product.Title,
              ImageUrl = product.ThumbnailUrl
            };
          })).ToList<Game>();
          ServiceProxyEventArgs<ProductResponse<Game>> e2 = new ServiceProxyEventArgs<ProductResponse<Game>>((object) new ProductResponse<Game>()
          {
            TotalItems = e.TotalAvailable,
            ProductList = list
          }, (Exception) null, false, e.UserState);
          gameListCompleted((object) this, e2);
          return;
        }
      }
      ServiceProxyEventArgs<ProductResponse<Game>> e3 = new ServiceProxyEventArgs<ProductResponse<Game>>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToGetGameListForGameStyle, "Failed to get game list for game style", (string[]) null), false, (object) null);
      gameListCompleted((object) this, e3);
    }

    private void EnumerateCategoriesCompleted(
      object sender,
      EnumerateCategoriesCompletedEventArgs e)
    {
      this.categoryEvent.Set();
      EventHandler<ServiceProxyEventArgs<ProductResponse<Category>>> categoryListCompleted = this.EventGetCategoryListCompleted;
      if (categoryListCompleted == null)
        return;
      if (e != null && e.Error == null)
      {
        if (e.Cancelled)
        {
          ServiceProxyEventArgs<ProductResponse<Category>> e1 = new ServiceProxyEventArgs<ProductResponse<Category>>((object) null, (Exception) null, true, e.UserState);
          categoryListCompleted((object) this, e1);
          return;
        }
        if (e.Categories != null)
        {
          List<Category> list = e.Categories.Select<Microsoft.Phone.Marketplace.Category, Category>((Func<Microsoft.Phone.Marketplace.Category, Category>) (category =>
          {
            return new Category()
            {
              CategoryId = category.Id,
              Title = category.Title,
              ImageUrl = category.ThumbnailUrl,
              HasSubCategories = category.HasSubCategory
            };
          })).ToList<Category>();
          ServiceProxyEventArgs<ProductResponse<Category>> e2 = new ServiceProxyEventArgs<ProductResponse<Category>>((object) new ProductResponse<Category>()
          {
            TotalItems = e.TotalAvailable,
            ProductList = list
          }, (Exception) null, false, e.UserState);
          categoryListCompleted((object) this, e2);
          return;
        }
      }
      ServiceProxyEventArgs<ProductResponse<Category>> e3 = new ServiceProxyEventArgs<ProductResponse<Category>>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToGetCategoryListForLifeStyle, "Failed to get category list for life style", (string[]) null), false, (object) null);
      categoryListCompleted((object) this, e3);
    }

    private void ShowPurchaseCompleted(object sender, PurchaseCompletedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      this.purchaseRequest.ScreenShown -= new EventHandler<EventArgs>(this.PurchaseRequest_ScreenShown);
      this.purchaseRequest.ScreenHidden -= new EventHandler<EventArgs>(this.PurchaseRequest_ScreenHidden);
      this.purchaseRequest = (AvatarPurchaseRequest) null;
      if (e.Error == null)
      {
        if (e.Cancelled)
        {
          EventHandler purchaseCancelled = this.EventPurchaseCancelled;
          if (purchaseCancelled != null)
          {
            purchaseCancelled((object) this, EventArgs.Empty);
            return;
          }
        }
        else if (e.PurchaseReceipt != null)
        {
          EventHandler<ServiceProxyEventArgs<PurchaseReceipt>> purchaseCompleted = this.EventPurchaseCompleted;
          if (purchaseCompleted != null)
          {
            ServiceProxyEventArgs<PurchaseReceipt> e1 = new ServiceProxyEventArgs<PurchaseReceipt>((object) e.PurchaseReceipt, (Exception) null, false, (object) null);
            purchaseCompleted((object) this, e1);
            return;
          }
        }
      }
      else if (e.Error is BrowserNavigationException error)
      {
        EventHandler<ServiceProxyEventArgs<PurchaseReceipt>> purchaseCompleted = this.EventPurchaseCompleted;
        ServiceProxyEventArgs<PurchaseReceipt> e2 = new ServiceProxyEventArgs<PurchaseReceipt>((object) null, (Exception) error, false, (object) null);
        if (purchaseCompleted == null)
          return;
        purchaseCompleted((object) this, e2);
        return;
      }
      EventHandler<ServiceProxyEventArgs<PurchaseReceipt>> purchaseCompleted1 = this.EventPurchaseCompleted;
      ServiceProxyEventArgs<PurchaseReceipt> e3 = new ServiceProxyEventArgs<PurchaseReceipt>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToPurchaseAvatarItems, "Failed to purchase an avatar item", (string[]) null), false, (object) null);
      if (purchaseCompleted1 == null)
        return;
      purchaseCompleted1((object) this, e3);
    }

    private void EnumeratePurchaseHistoryCompleted(object sender, PurchaseReceiptEventArgs e)
    {
      if (this.EventGetPurchaseHistoryCompleted == null)
        return;
      this.EventGetPurchaseHistoryCompleted((object) this, e == null || e.Error != null || e.Cancelled || e.Purchases == null ? new ServiceProxyEventArgs<PurchaseReceipt>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToPurchaseAvatarItems, "Failed to purchase an avatar item", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<PurchaseReceipt>(e.Purchases.Count > 0 ? (object) e.Purchases[0] : (object) (PurchaseReceipt) null, (Exception) null, false, (object) null));
    }

    private void GetGamerContextCompleted(object sender, AsyncCompletedEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<bool>> contextCompleted = this.EventGetGamerContextCompleted;
      if (contextCompleted == null)
        return;
      ServiceProxyEventArgs<bool> e1 = new ServiceProxyEventArgs<bool>((object) (bool) (e == null || e.Error != null ? 0 : (!e.Cancelled ? 1 : 0)), (Exception) null, false, (object) null);
      contextCompleted((object) this, e1);
    }

    private void PurchaseRequest_ScreenShown(object sender, EventArgs e)
    {
      EventHandler<EventArgs> eventScreenShown = this.EventScreenShown;
      if (eventScreenShown == null)
        return;
      eventScreenShown((object) this, EventArgs.Empty);
    }

    private void PurchaseRequest_ScreenHidden(object sender, EventArgs e)
    {
      EventHandler<EventArgs> eventScreenHidden = this.EventScreenHidden;
      if (eventScreenHidden == null)
        return;
      eventScreenHidden((object) this, EventArgs.Empty);
    }
  }
}
