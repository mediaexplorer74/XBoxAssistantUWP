// *********************************************************
// Type: Xbox.Live.Phone.Services.Marketplace
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Microsoft.Phone.Marketplace;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xbox.Live.Phone.Utils;
using Xbox.Live.Phone.Utils.Cache;


namespace Xbox.Live.Phone.Services
{
  public sealed class Marketplace : IDisposable
  {
    private const int TitleId = 1297290138;
    private const int PageSize = 24;
    private const string MarketPlaceCategoryStorageKey = "MPC";
    private const string MarketPlaceGamesStorageKey = "MPG";
    private static Xbox.Live.Phone.Services.Marketplace staticInstance;
    private IMarketplaceServiceManager marketplaceServiceManager;
    private MarketPlacePage<Game> games;
    private MarketPlacePage<Offer> offers;
    private MarketPlacePage<Category> categories;
    private MarketPlaceCategory subcategories;
    private PromotedCategoryUtility promotedCategories;
    private bool isPreviousPageAvailable;
    private bool isNextPageAvailable;
    private ManualResetEvent offerEvent;
    private ManualResetEvent purchaseHistoryEvent;
    private Offer currentOffer;
    private PurchaseReceipt currentPurchaseReceipt;
    private OfferUserState currentRequestedOfferUserState;
    private CategoryUserState currentRequestedCategoryUserState;
    private ProductUserState currentRequestedGameUserState;

    private Marketplace()
    {
      this.marketplaceServiceManager = ServiceManagerFactory.CreateMarketplaceServiceManager();
      this.marketplaceServiceManager.Initialize(EnvironmentState.Instance.Environment);
      this.marketplaceServiceManager.EventGetGameListCompleted += new EventHandler<ServiceProxyEventArgs<ProductResponse<Game>>>(this.MarketPlace_EventGetGameListCompleted);
      this.marketplaceServiceManager.EventGetAvatarItemsCompleted += new EventHandler<ServiceProxyEventArgs<ProductResponse<Offer>>>(this.MarketPlace_EventGetAvatarItemsCompleted);
      this.marketplaceServiceManager.EventGetCategoryListCompleted += new EventHandler<ServiceProxyEventArgs<ProductResponse<Category>>>(this.MarketPlace_EventGetCategoryListCompleted);
      this.marketplaceServiceManager.EventGetOfferCompleted += new EventHandler<ServiceProxyEventArgs<ProductResponse<Offer>>>(this.EventGetOfferCompleted);
      this.marketplaceServiceManager.EventPurchaseCompleted += new EventHandler<ServiceProxyEventArgs<PurchaseReceipt>>(this.MarketPlace_EventPurchaseCompleted);
      this.marketplaceServiceManager.EventPurchaseCancelled += new EventHandler(this.MarketPlace_EventPurchaseCancelled);
      this.marketplaceServiceManager.EventGetPurchaseHistoryCompleted += new EventHandler<ServiceProxyEventArgs<PurchaseReceipt>>(this.MarketPlace_EventGetPurchaseHistoryCompleted);
      this.marketplaceServiceManager.EventGetGamerContextCompleted += new EventHandler<ServiceProxyEventArgs<bool>>(this.MarketPlace_EventGetGamerContextCompleted);
      this.marketplaceServiceManager.EventUpdatedTransactionIdCompleted += new EventHandler<ServiceProxyEventArgs<Guid>>(this.MarketPlace_EventUpdatedTransactionIdCompleted);
      this.marketplaceServiceManager.EventScreenShown += new EventHandler<EventArgs>(this.MarketplaceServiceManager_EventScreenShown);
      this.marketplaceServiceManager.EventScreenHidden += new EventHandler<EventArgs>(this.MarketplaceServiceManager_EventScreenHidden);
      this.categories = new MarketPlacePage<Category>("MPC");
      this.subcategories = new MarketPlaceCategory(this.categories);
      this.promotedCategories = new PromotedCategoryUtility();
      this.games = new MarketPlacePage<Game>("MPG");
      this.offers = new MarketPlacePage<Offer>();
      this.offerEvent = new ManualResetEvent(false);
      this.purchaseHistoryEvent = new ManualResetEvent(false);
    }

    public event EventHandler<ServiceProxyEventArgs<List<Offer>>> EventGetAvatarItemsCompleted;

    public event EventHandler<ServiceProxyEventArgs<List<Game>>> EventGetGameListCompleted;

    public event EventHandler<ServiceProxyEventArgs<List<Category>>> EventGetCategoryListCompleted;

    public event EventHandler<ServiceProxyEventArgs<AvatarItem>> EventPurchaseCompleted;

    public event EventHandler EventPurchaseCancelled;

    public event EventHandler<EventArgs> EventScreenShown;

    public event EventHandler<EventArgs> EventScreenHidden;

    public event EventHandler<ServiceProxyEventArgs<bool>> EventGetGamerContextCompleted;

    public event EventHandler<ServiceProxyEventArgs<Guid>> EventGetUpdatedTransactionId;

    public static Xbox.Live.Phone.Services.Marketplace Instance
    {
      get
      {
        if (Xbox.Live.Phone.Services.Marketplace.staticInstance == null)
          Xbox.Live.Phone.Services.Marketplace.staticInstance = new Xbox.Live.Phone.Services.Marketplace();
        return Xbox.Live.Phone.Services.Marketplace.staticInstance;
      }
    }

    public bool IsPreviousPageAvailable
    {
      get => this.isPreviousPageAvailable;
      set => this.isPreviousPageAvailable = value;
    }

    public bool IsNextPageAvailable
    {
      get => this.isNextPageAvailable;
      set => this.isNextPageAvailable = value;
    }

    public IMarketplaceServiceManager MarketplaceServiceManager => this.marketplaceServiceManager;

    public void GetAvatarStoresAsync() => this.marketplaceServiceManager.GetStoresAsync();

    public void LoadFromCache(bool isRestoringTombStonedData)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        if (isRestoringTombStonedData)
        {
          MarketPlacePage<Category> marketPlacePage1 = CacheManager.Load<MarketPlacePage<Category>>("MPC");
          if (marketPlacePage1 != null)
          {
            this.categories = marketPlacePage1;
            this.categories.PageSize = 24;
            this.categories.CurrentDisplayedList = this.categories.CachedItems;
            this.subcategories.ParentCategory = this.categories;
          }
          MarketPlacePage<Game> marketPlacePage2 = CacheManager.Load<MarketPlacePage<Game>>("MPG");
          if (marketPlacePage2 == null)
            return;
          this.games = marketPlacePage2;
          this.games.PageSize = 24;
          this.games.CurrentDisplayedList = this.games.CachedItems;
        }
        else
        {
          this.categories = new MarketPlacePage<Category>("MPC");
          this.subcategories = new MarketPlaceCategory(this.categories);
          this.games = new MarketPlacePage<Game>("MPG");
          CacheManager.Instance.SaveAsync((CacheItem) this.categories);
          CacheManager.Instance.SaveAsync((CacheItem) this.games);
        }
      });
    }

    public void Initialize()
    {
      this.currentRequestedGameUserState = (ProductUserState) null;
      this.currentRequestedCategoryUserState = (CategoryUserState) null;
      this.currentRequestedOfferUserState = (OfferUserState) null;
      this.offers = new MarketPlacePage<Offer>();
      this.ResetPreviousAndNext();
    }

    public void ResetPreviousAndNext()
    {
      this.IsNextPageAvailable = false;
      this.isPreviousPageAvailable = false;
    }

    public void GetCategoryListForLifeStyleAsync(
      int clientPageSize,
      int clientPageNumber,
      int categoryId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        MarketPlacePage<Category> marketPlacePage = categoryId == 14001 ? this.categories : this.promotedCategories.GetPromotedCategory(categoryId);
        marketPlacePage.ClientPageNumber = clientPageNumber;
        marketPlacePage.ClientPageSize = clientPageSize;
        marketPlacePage.PageSize = 24;
        List<Category> cachedCategories = marketPlacePage.CachedItems;
        this.IsPreviousPageAvailable = clientPageNumber > 1;
        CategoryUserState userState = new CategoryUserState();
        if (!marketPlacePage.IsDataAvailable())
        {
          userState.CategoryId = categoryId;
          userState.PageNumber = marketPlacePage.PageNumber;
          userState.ClientPageNumber = clientPageNumber;
          userState.ClientPageSize = clientPageSize;
          this.currentRequestedCategoryUserState = userState;
          this.marketplaceServiceManager.GetCategoryListForLifeStyleAsync(24, marketPlacePage.PageNumber, categoryId, (object) userState);
        }
        else
        {
          this.categories.CurrentDisplayedList = cachedCategories;
          this.IsNextPageAvailable = marketPlacePage.IsNextPageAvailable;
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            ServiceProxyEventArgs<List<Category>> e = new ServiceProxyEventArgs<List<Category>>((object) cachedCategories, (Exception) null, false, (object) null);
            if (this.EventGetCategoryListCompleted == null)
              return;
            this.EventGetCategoryListCompleted((object) this, e);
          }, (object) this);
        }
      });
    }

    public void GetSubCategoryListForLifeStyleAsync(
      int clientPageSize,
      int clientPageNumber,
      int categoryId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        List<Category> subCategoryList = this.subcategories.GetCachedSubCategories(categoryId, clientPageNumber);
        this.IsPreviousPageAvailable = clientPageNumber > 1;
        if (subCategoryList == null)
        {
          CategoryUserState userState = new CategoryUserState();
          userState.ParentCategoryId = categoryId;
          userState.ClientPageNumber = clientPageNumber;
          userState.ClientPageSize = clientPageSize;
          this.currentRequestedCategoryUserState = userState;
          this.marketplaceServiceManager.GetCategoryListForLifeStyleAsync(clientPageSize, clientPageNumber, categoryId, (object) userState);
        }
        else
        {
          this.IsNextPageAvailable = this.subcategories.IsNextPageForSubCategoriesAvailable(categoryId, clientPageSize, clientPageNumber);
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            if (this.EventGetCategoryListCompleted == null)
              return;
            this.EventGetCategoryListCompleted((object) this, new ServiceProxyEventArgs<List<Category>>((object) subCategoryList, (Exception) null, false, (object) null));
          }, (object) this);
        }
      });
    }

    public void GetGameListForGameStyleAsync(int clientPageSize, int clientPageNumber)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        this.games.ClientPageNumber = clientPageNumber;
        this.games.ClientPageSize = clientPageSize;
        this.games.PageSize = 24;
        List<Game> cachedGames = this.games.CachedItems;
        ProductUserState userState = new ProductUserState();
        this.IsPreviousPageAvailable = clientPageNumber > 1;
        if (!this.games.IsDataAvailable())
        {
          userState.PageNumber = this.games.PageNumber;
          userState.ClientPageNumber = clientPageNumber;
          userState.ClientPageSize = clientPageSize;
          this.currentRequestedGameUserState = userState;
          this.marketplaceServiceManager.GetGameListForGameStyleAsync(24, this.games.PageNumber, (object) userState);
        }
        else
        {
          this.games.CurrentDisplayedList = cachedGames;
          this.IsNextPageAvailable = this.games.IsNextPageAvailable;
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            ServiceProxyEventArgs<List<Game>> e = new ServiceProxyEventArgs<List<Game>>((object) cachedGames, (Exception) null, false, (object) null);
            if (this.EventGetGameListCompleted == null)
              return;
            this.EventGetGameListCompleted((object) this, e);
          }, (object) this);
        }
      });
    }

    public void GetAvatarItemsByCategoryIdAsync(
      int clientPageSize,
      int clientPageNumber,
      int categoryId,
      AvatarBodyType bodyType)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        this.SendOfferDataToClient(clientPageSize, clientPageNumber, categoryId, bodyType, Guid.Empty);
      });
    }

    public void GetAvatarItemsByGameIdAsync(
      int clientPageSize,
      int clientPageNumber,
      AvatarBodyType bodyType,
      Guid gameId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        this.SendOfferDataToClient(clientPageSize, clientPageNumber, 0, bodyType, gameId);
      });
    }

    public void GetMostPopularAvatarItemsAsync(
      int clientPageSize,
      int clientPageNumber,
      AvatarBodyType bodyType)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        this.SendOfferDataToClient(clientPageSize, clientPageNumber, 0, bodyType, Guid.Empty);
      });
    }

    public void Purchase(
      AvatarBodyType bodyType,
      Guid offerId,
      Guid transactionId,
      bool isPdlcVisible)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        bool flag = false;
        if (isPdlcVisible)
        {
          this.currentPurchaseReceipt = (PurchaseReceipt) null;
          this.purchaseHistoryEvent.Reset();
          this.marketplaceServiceManager.GetPurchaseHistory(1297290138, transactionId);
          this.purchaseHistoryEvent.WaitOne();
          if (this.currentPurchaseReceipt != null && this.currentPurchaseReceipt.TransactionId != Guid.Empty)
          {
            flag = this.currentPurchaseReceipt.TransactionId != Guid.Empty;
            offerId = this.currentPurchaseReceipt.OfferId;
          }
        }
        Offer offer = (Offer) null;
        if (this.offers != null && this.offers.CurrentDisplayedList != null)
          offer = this.offers.CurrentDisplayedList.Where<Offer>((Func<Offer, bool>) (o => o.OfferId == offerId)).SingleOrDefault<Offer>();
        else if (offerId != Guid.Empty)
        {
          this.currentOffer = (Offer) null;
          this.offerEvent.Reset();
          this.marketplaceServiceManager.GetOfferByOfferIdAsync(1297290138, bodyType, offerId);
          this.offerEvent.WaitOne();
          offer = this.currentOffer;
        }
        if (offer != null && !flag)
        {
          this.PurchaseAvatarItem(bodyType, offer);
        }
        else
        {
          if (this.EventPurchaseCompleted == null)
            return;
          ServiceProxyEventArgs<AvatarItem> e;
          if (offer != null)
            e = new ServiceProxyEventArgs<AvatarItem>((object) new AvatarItem()
            {
              AvatarAssetId = offer.AvatarAssetId,
              OfferId = offer.OfferId,
              Description = offer.OfferName,
              PriceInPoint = offer.PointsPrice
            }, (Exception) null, false, (object) null);
          else
            e = new ServiceProxyEventArgs<AvatarItem>((object) null, (Exception) XLiveMobileException.CreateException((Exception) null, 8103, "failed to purchase avatar item", (string[]) null), false, (object) null);
          this.EventPurchaseCompleted((object) this, e);
        }
      });
    }

    public void GetGamerContextAsync() => this.marketplaceServiceManager.GetGamerContext();

    public void Dispose()
    {
      this.offerEvent.Dispose();
      this.purchaseHistoryEvent.Dispose();
    }

    private void PurchaseAvatarItem(AvatarBodyType bodyType, Offer offer)
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.marketplaceServiceManager.PurchaseAvatarItem(1297290138, bodyType, offer);
      }, (object) this);
    }

    private void MarketPlace_EventGetGameListCompleted(
      object sender,
      ServiceProxyEventArgs<ProductResponse<Game>> e)
    {
      ServiceProxyEventArgs<List<Game>> responseArgs;
      if (e != null && e.Error == null)
      {
        List<Game> objectResult = (List<Game>) null;
        if (e.UserState != null && e.UserState != this.currentRequestedGameUserState)
          return;
        if (!e.Cancelled && e.UserState != null && e.UserState is ProductUserState && e.Result != null)
        {
          ProductUserState userState = (ProductUserState) e.UserState;
          if (e.Result.ProductList != null)
          {
            foreach (Game product in e.Result.ProductList)
              product.IsBlocked = CatalogBlacklistProvider.Instance.BlacklistedGames.Contains(product.GameId);
          }
          this.games.UpdatePage(e.Result.ProductList, e.Result.TotalItems, userState.PageNumber);
          objectResult = this.games.CachedItems;
          CacheManager.Instance.SaveAsync((CacheItem) this.games);
          this.IsNextPageAvailable = this.games.IsNextPageAvailable;
          this.games.CurrentDisplayedList = objectResult;
        }
        responseArgs = new ServiceProxyEventArgs<List<Game>>((object) objectResult, (Exception) null, false, (object) null);
      }
      else
        responseArgs = new ServiceProxyEventArgs<List<Game>>((object) null, (Exception) XLiveMobileException.CreateException(e.Error, 8101, "failed to get game list for game style", (string[]) null), false, (object) null);
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        if (this.EventGetGameListCompleted == null)
          return;
        this.EventGetGameListCompleted((object) this, responseArgs);
      }, (object) this);
    }

    private void MarketPlace_EventGetAvatarItemsCompleted(
      object sender,
      ServiceProxyEventArgs<ProductResponse<Offer>> e)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        if (e != null && e.ResultAvailable && e.UserState != null && e.UserState is OfferUserState)
        {
          if ((OfferUserState) e.UserState != this.currentRequestedOfferUserState)
            return;
          if (e.Result != null && e.Result.ProductList != null)
          {
            List<Guid> blacklistedAssetIds = CatalogBlacklistProvider.Instance.BlacklistedAssetIds;
            foreach (Offer product in e.Result.ProductList)
              product.IsBlocked = blacklistedAssetIds.Contains(product.AvatarAssetId);
          }
        }
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          ServiceProxyEventArgs<List<Offer>> e2;
          if (e != null && e.Error == null)
          {
            List<Offer> objectResult = (List<Offer>) null;
            if (e.UserState != null && e.UserState is OfferUserState)
            {
              OfferUserState userState = (OfferUserState) e.UserState;
              if (!e.Cancelled && e.Result != null)
              {
                this.offers.UpdatePage(e.Result.ProductList, e.Result.TotalItems, userState.PageNumber);
                objectResult = this.offers.CachedItems;
                this.IsNextPageAvailable = this.offers.IsNextPageAvailable;
                this.offers.CurrentDisplayedList = objectResult;
              }
            }
            e2 = new ServiceProxyEventArgs<List<Offer>>((object) objectResult, (Exception) null, e.Cancelled, (object) null);
          }
          else
            e2 = new ServiceProxyEventArgs<List<Offer>>((object) null, (Exception) XLiveMobileException.CreateException(e.Error, 8100, "failed to get avatar items", (string[]) null), false, (object) null);
          if (this.EventGetAvatarItemsCompleted == null)
            return;
          this.EventGetAvatarItemsCompleted((object) this, e2);
        }, (object) this);
      }, (object) this);
    }

    private void MarketPlace_EventGetCategoryListCompleted(
      object sender,
      ServiceProxyEventArgs<ProductResponse<Category>> e)
    {
      ServiceProxyEventArgs<List<Category>> responseArgs;
      if (e != null && e.Error == null)
      {
        List<Category> objectResult = (List<Category>) null;
        if (e.UserState != null && e.UserState is CategoryUserState)
        {
          CategoryUserState userState = (CategoryUserState) e.UserState;
          if (userState != null)
          {
            if (userState != this.currentRequestedCategoryUserState)
              return;
            int parentCategoryId = userState.ParentCategoryId;
            int categoryId = userState.CategoryId;
            int clientPageSize = userState.ClientPageSize;
            int clientPageNumber = userState.ClientPageNumber;
            if (!e.Cancelled && e.Result != null)
            {
              if (e.Result.ProductList != null)
              {
                foreach (Category product in e.Result.ProductList)
                  product.IsBlocked = CatalogBlacklistProvider.Instance.BlacklistedCategories.Contains(product.CategoryId);
              }
              if (parentCategoryId > 0)
              {
                this.subcategories.UpdateSubCategories(parentCategoryId, e.Result.ProductList, e.Result.TotalItems, clientPageNumber);
                objectResult = this.subcategories.GetCachedSubCategories(parentCategoryId, clientPageNumber);
                this.IsNextPageAvailable = this.subcategories.IsNextPageForSubCategoriesAvailable(parentCategoryId, clientPageSize, clientPageNumber);
                CacheManager.Instance.SaveAsync((CacheItem) this.categories);
              }
              else
              {
                MarketPlacePage<Category> marketPlacePage = categoryId != 14001 ? this.promotedCategories.GetPromotedCategory(categoryId) : this.categories;
                marketPlacePage.UpdatePage(e.Result.ProductList, e.Result.TotalItems, userState.PageNumber);
                objectResult = marketPlacePage.CachedItems;
                CacheManager.Instance.SaveAsync((CacheItem) this.categories);
                this.categories.CurrentDisplayedList = objectResult;
                this.IsNextPageAvailable = marketPlacePage.IsNextPageAvailable;
              }
            }
          }
        }
        responseArgs = new ServiceProxyEventArgs<List<Category>>((object) objectResult, (Exception) null, e.Cancelled, (object) null);
      }
      else
        responseArgs = new ServiceProxyEventArgs<List<Category>>((object) null, (Exception) XLiveMobileException.CreateException(e.Error, 8102, "failed to get category list for life style", (string[]) null), false, (object) null);
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        if (this.EventGetCategoryListCompleted == null)
          return;
        this.EventGetCategoryListCompleted((object) this, responseArgs);
      }, (object) this);
    }

    private void EventGetOfferCompleted(
      object sender,
      ServiceProxyEventArgs<ProductResponse<Offer>> e)
    {
      if (e != null && e.ResultAvailable && e.Result != null && e.Result.ProductList != null && e.Result.ProductList.Count == 1)
        this.currentOffer = e.Result.ProductList[0];
      this.offerEvent.Set();
    }

    private void MarketPlace_EventPurchaseCompleted(
      object sender,
      ServiceProxyEventArgs<PurchaseReceipt> e)
    {
      if (this.EventPurchaseCompleted == null)
        return;
      ServiceProxyEventArgs<AvatarItem> e1;
      if (e != null && e.ResultAvailable && e.Result != null)
        e1 = new ServiceProxyEventArgs<AvatarItem>((object) new AvatarItem()
        {
          AvatarAssetId = e.Result.AvatarAssetIdForRedeemToken,
          OfferId = e.Result.OfferId,
          Description = e.Result.OfferName,
          PriceInPoint = e.Result.AvatarAssetPriceInPointsForRedeemToken
        }, (Exception) null, false, (object) null);
      else
        e1 = !(e.Error is BrowserNavigationException error) ? new ServiceProxyEventArgs<AvatarItem>((object) null, (Exception) XLiveMobileException.CreateException(e.Error, 8103, "failed to purchase avatar item", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<AvatarItem>((object) null, (Exception) error, false, (object) null);
      this.EventPurchaseCompleted((object) this, e1);
    }

    private void MarketPlace_EventPurchaseCancelled(object sender, EventArgs e)
    {
      EventHandler tempEvent = this.EventPurchaseCancelled;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        if (tempEvent == null)
          return;
        tempEvent((object) this, EventArgs.Empty);
      }, (object) this);
    }

    private void SendOfferDataToClient(
      int clientPageSize,
      int clientPageNumber,
      int categoryId,
      AvatarBodyType bodyType,
      Guid gameId)
    {
      if (this.currentRequestedOfferUserState != null && (this.currentRequestedOfferUserState.CategoryId != categoryId || this.currentRequestedOfferUserState.GameId != gameId))
        this.offers = new MarketPlacePage<Offer>();
      this.offers.ClientPageNumber = clientPageNumber;
      this.offers.ClientPageSize = clientPageSize;
      this.offers.PageSize = 24;
      List<Offer> cachedOffers = this.offers.CachedItems;
      this.IsPreviousPageAvailable = clientPageNumber > 1;
      OfferUserState userState = new OfferUserState();
      userState.PageSize = 24;
      userState.CategoryId = categoryId;
      userState.GameId = gameId;
      if (!this.offers.IsDataAvailable())
      {
        userState.PageNumber = this.offers.PageNumber;
        userState.ClientPageNumber = clientPageNumber;
        userState.ClientPageSize = clientPageSize;
        this.currentRequestedOfferUserState = userState;
        if (userState.CategoryId != 0)
          this.marketplaceServiceManager.GetAvatarItemsByCategoryIdAsync(1297290138, 24, this.offers.PageNumber, categoryId, bodyType, (object) userState);
        else if (userState.GameId != Guid.Empty)
          this.marketplaceServiceManager.GetAvatarItemsByGameIdAsync(1297290138, 24, this.offers.PageNumber, bodyType, gameId, (object) userState);
        else
          this.marketplaceServiceManager.GetMostPopularAvatarItemsAsync(1297290138, 24, this.offers.PageNumber, bodyType, (object) userState);
      }
      else
      {
        this.offers.CurrentDisplayedList = cachedOffers;
        this.IsNextPageAvailable = this.offers.IsNextPageAvailable;
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          ServiceProxyEventArgs<List<Offer>> e = new ServiceProxyEventArgs<List<Offer>>((object) cachedOffers, (Exception) null, false, (object) null);
          if (this.EventGetAvatarItemsCompleted == null)
            return;
          this.EventGetAvatarItemsCompleted((object) this, e);
        }, (object) this);
      }
    }

    private void MarketPlace_EventGetPurchaseHistoryCompleted(
      object sender,
      ServiceProxyEventArgs<PurchaseReceipt> e)
    {
      if (e != null && e.ResultAvailable && e.Result != null)
        this.currentPurchaseReceipt = e.Result;
      this.purchaseHistoryEvent.Set();
    }

    private void MarketPlace_EventGetGamerContextCompleted(
      object sender,
      ServiceProxyEventArgs<bool> e)
    {
      EventHandler<ServiceProxyEventArgs<bool>> tempEvent = this.EventGetGamerContextCompleted;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        if (tempEvent == null)
          return;
        tempEvent((object) this, e);
      }, (object) this);
    }

    private void MarketPlace_EventUpdatedTransactionIdCompleted(
      object sender,
      ServiceProxyEventArgs<Guid> e)
    {
      EventHandler<ServiceProxyEventArgs<Guid>> updatedTransactionId = this.EventGetUpdatedTransactionId;
      if (updatedTransactionId == null)
        return;
      updatedTransactionId((object) this, e);
    }

    private void MarketplaceServiceManager_EventScreenShown(object sender, EventArgs e)
    {
      EventHandler<EventArgs> eventScreenShown = this.EventScreenShown;
      if (eventScreenShown == null)
        return;
      eventScreenShown((object) this, EventArgs.Empty);
    }

    private void MarketplaceServiceManager_EventScreenHidden(object sender, EventArgs e)
    {
      EventHandler<EventArgs> eventScreenHidden = this.EventScreenHidden;
      if (eventScreenHidden == null)
        return;
      eventScreenHidden((object) this, EventArgs.Empty);
    }
  }
}
