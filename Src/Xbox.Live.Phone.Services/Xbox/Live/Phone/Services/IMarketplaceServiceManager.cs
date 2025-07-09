// *********************************************************
// Type: Xbox.Live.Phone.Services.IMarketplaceServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Microsoft.Phone.Marketplace;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;


namespace Xbox.Live.Phone.Services
{
  public interface IMarketplaceServiceManager : IDisposable
  {
    event EventHandler<ServiceProxyEventArgs<List<StoreData>>> EventGetStoreDataCompleted;

    event EventHandler<ServiceProxyEventArgs<ProductResponse<Offer>>> EventGetAvatarItemsCompleted;

    event EventHandler<ServiceProxyEventArgs<ProductResponse<Offer>>> EventGetOfferCompleted;

    event EventHandler<ServiceProxyEventArgs<ProductResponse<Game>>> EventGetGameListCompleted;

    event EventHandler<ServiceProxyEventArgs<ProductResponse<Category>>> EventGetCategoryListCompleted;

    event EventHandler<ServiceProxyEventArgs<PurchaseReceipt>> EventPurchaseCompleted;

    event EventHandler<ServiceProxyEventArgs<PurchaseReceipt>> EventGetPurchaseHistoryCompleted;

    event EventHandler EventPurchaseCancelled;

    event EventHandler<ServiceProxyEventArgs<bool>> EventGetGamerContextCompleted;

    event EventHandler<ServiceProxyEventArgs<Guid>> EventUpdatedTransactionIdCompleted;

    event EventHandler<EventArgs> EventScreenShown;

    event EventHandler<EventArgs> EventScreenHidden;

    void Initialize(ServiceCommon.Environment environmentName);

    void GetStoresAsync();

    void GetCategoryListForLifeStyleAsync(
      int pageSize,
      int pageNumber,
      int categoryId,
      object userState);

    void GetGameListForGameStyleAsync(int pageSize, int pageNumber, object userState);

    void GetAvatarItemsByCategoryIdAsync(
      int titleId,
      int pageSize,
      int pageNumber,
      int categoryId,
      AvatarBodyType bodyType,
      object userState);

    void GetAvatarItemsByGameIdAsync(
      int titleId,
      int pageSize,
      int pageNumber,
      AvatarBodyType bodyType,
      Guid gameId,
      object userState);

    void GetOfferByOfferIdAsync(int titleId, AvatarBodyType bodyType, Guid offerId);

    void GetMostPopularAvatarItemsAsync(
      int titleId,
      int pageSize,
      int pageNumber,
      AvatarBodyType bodyType,
      object userState);

    void PurchaseAvatarItem(int titleId, AvatarBodyType bodyType, Offer offer);

    void GetPurchaseHistory(int titleId, Guid transactionId);

    void InvalidateGamerContext();

    void GetGamerContext();
  }
}
