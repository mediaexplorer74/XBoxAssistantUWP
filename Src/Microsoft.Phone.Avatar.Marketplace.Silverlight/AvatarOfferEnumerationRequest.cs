// *********************************************************
// Type: Microsoft.Phone.Marketplace.AvatarOfferEnumerationRequest
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;


namespace Microsoft.Phone.Marketplace
{
  public class AvatarOfferEnumerationRequest : OfferEnumerationRequest
  {
    private int? _categoryId;
    private Guid _parentProductId;
    private MarketplaceAvatarBodyType _avatarBodyType;

    public void EnumerateOffersAsync(
      int titleId,
      int pageSize,
      int pageNum,
      MarketplaceAvatarBodyType avatarBodyType,
      Guid parentProductId,
      int? categoryId,
      OrderingOption orderBy,
      OrderingDirection orderDirection,
      object userState)
    {
      this._avatarBodyType = avatarBodyType;
      this._parentProductId = parentProductId;
      this._categoryId = categoryId;
      this.EnumerateOffersAsyncCommon(titleId, pageSize, pageNum, (IEnumerable<Guid>) null, new int?(), orderBy, orderDirection, userState, 1);
    }

    public void EnumerateOffersByCategoryIdAsync(
      int titleId,
      int pageSize,
      int pageNum,
      int categoryId,
      MarketplaceAvatarBodyType avatarBodyType,
      OrderingOption orderBy,
      OrderingDirection orderDirection,
      object userState)
    {
      this._avatarBodyType = avatarBodyType;
      this._categoryId = new int?(categoryId);
      this._parentProductId = Guid.Empty;
      this.EnumerateOffersAsyncCommon(titleId, pageSize, pageNum, (IEnumerable<Guid>) null, new int?(), orderBy, orderDirection, userState, 1);
    }

    public void EnumerateOffersByGameIdAsync(
      int titleId,
      int pageSize,
      int pageNum,
      MarketplaceAvatarBodyType avatarBodyType,
      Guid parentProductId,
      OrderingOption orderBy,
      OrderingDirection orderDirection,
      object userState)
    {
      this._avatarBodyType = avatarBodyType;
      this._categoryId = new int?(0);
      this._parentProductId = parentProductId;
      this.EnumerateOffersAsyncCommon(titleId, pageSize, pageNum, (IEnumerable<Guid>) null, new int?(), orderBy, orderDirection, userState, 1);
    }

    public void EnumerateOffersByOfferIdAsync(
      int titleId,
      int pageSize,
      int pageNum,
      MarketplaceAvatarBodyType avatarBodyType,
      IEnumerable<Guid> offerIds)
    {
      this._avatarBodyType = avatarBodyType;
      this.EnumerateOffersAsyncCommon(titleId, pageSize, pageNum, offerIds, new int?(), OrderingOption.SortTitle, OrderingDirection.Ascending, (object) null, 1);
    }

    internal void EnumerateTokenOffersByOfferIdAsync(
      int titleId,
      int pageSize,
      int pageNum,
      MarketplaceAvatarBodyType avatarBodyType,
      IEnumerable<Guid> offerIds)
    {
      this._avatarBodyType = avatarBodyType;
      this.EnumerateOffersAsyncCommon(titleId, pageSize, pageNum, offerIds, new int?(), OrderingOption.SortTitle, OrderingDirection.Ascending, (object) null, 2);
    }

    protected override StringBuilder BuildRelativeUri(
      string locale,
      int tier,
      IEnumerable<Guid> offerIds)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(locale);
      stringBuilder.Append("?");
      if (offerIds == null && this._avatarBodyType > MarketplaceAvatarBodyType.Unknown)
      {
        stringBuilder.Append("bodytypes=");
        stringBuilder.Append((int) this._avatarBodyType);
        stringBuilder.Append(".3");
        stringBuilder.Append("&");
      }
      int? categoryId = this._categoryId;
      if ((categoryId.GetValueOrDefault() <= 0 ? 0 : (categoryId.HasValue ? 1 : 0)) != 0)
      {
        stringBuilder.Append("categories=");
        stringBuilder.Append((object) this._categoryId);
        stringBuilder.Append("&");
      }
      stringBuilder.Append("detailview=detailmobile");
      stringBuilder.Append("&offerfilter=3");
      if (offerIds != null)
      {
        string str = offerIds.Select<Guid, string>((Func<Guid, string>) (id => id.ToString())).Aggregate<string, string>(string.Empty, new Func<string, string, string>(((OfferEnumerationRequest) this).CombineWithOr));
        if (str != string.Empty)
        {
          stringBuilder.Append("&offers=");
          stringBuilder.Append(str);
        }
      }
      if (this._orderBy != OrderingOption.SortTitle)
      {
        stringBuilder.Append("&orderby=");
        stringBuilder.Append(OfferEnumerationRequest.GetFASTOrderingOption(this._orderBy));
      }
      if (this._orderDirection != OrderingDirection.Ascending)
      {
        stringBuilder.Append("&orderdirection=");
        stringBuilder.Append(OfferEnumerationRequest.GetFASTOrderingDirection(this._orderDirection));
      }
      if (this._pageNum != 1)
      {
        stringBuilder.Append("&pagenum=");
        stringBuilder.Append(this._pageNum);
      }
      if (this._pageSize != 100)
      {
        stringBuilder.Append("&pagesize=");
        stringBuilder.Append(this._pageSize);
      }
      if (this._parentProductId != Guid.Empty)
      {
        stringBuilder.Append("&parentproducts=");
        stringBuilder.Append((object) this._parentProductId);
      }
      stringBuilder.Append("&platformtypes=1");
      stringBuilder.Append("&producttypes=");
      stringBuilder.Append(47);
      stringBuilder.Append("&stores=");
      stringBuilder.Append(1);
      stringBuilder.Append("&tiers=");
      stringBuilder.Append(tier);
      return stringBuilder;
    }

    protected override ReadOnlyCollection<Offer> ParseOffer(
      Stream response,
      int tier,
      out int totalAvailable)
    {
      return new AvatarFastMarketplaceResponseParser().Parse(response, tier, this._paymentType, out totalAvailable);
    }
  }
}
