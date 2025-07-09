// *********************************************************
// Type: Microsoft.Phone.Marketplace.AvatarRedeemRequest
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using System;
using System.Collections.Generic;
using System.IO;


namespace Microsoft.Phone.Marketplace
{
  public class AvatarRedeemRequest : RedeemRequest
  {
    private Guid _avatarAssetId;
    private int _priceInPoints;
    private MarketplaceAvatarBodyType _avatarBodyType;

    public static List<Guid> Blacklist { get; set; }

    internal MarketplaceAvatarBodyType AvatarBodyType
    {
      get => this._avatarBodyType;
      set => this.DoMutableAction((Action) (() => this._avatarBodyType = value));
    }

    protected override OfferEnumerationRequest CreateOfferRequestObject()
    {
      return (OfferEnumerationRequest) new AvatarOfferEnumerationRequest();
    }

    protected override string CreateUriFromRedeemCode(string redeemCode)
    {
      return string.Format("{0}?billingToken={1}&storeId={2}", (object) "verifytoken", (object) redeemCode, (object) 1);
    }

    protected override bool CheckTitleId(Offer offer, uint titleId, string redeemCode) => true;

    private void HandleOfferInvalidBodyType(
      string redeemCode,
      string offerBodyType,
      string gamerBodyType)
    {
      this.PromptForRedeemCode(string.Format(AvatarUIResources.RedeemCodeBodyTypeNotMatch, (object) offerBodyType, (object) gamerBodyType), redeemCode);
    }

    private void HandleBlackListedOffer(string redeemCode)
    {
      this.PromptForRedeemCode(UIResources.RedeemOfferNotFound, redeemCode);
    }

    private string GetBodyTypeString(MarketplaceAvatarBodyType bodyType)
    {
      switch (bodyType)
      {
        case MarketplaceAvatarBodyType.Male:
          return AvatarUIResources.BodyTypeMale;
        case MarketplaceAvatarBodyType.Female:
          return AvatarUIResources.BodyTypeFemale;
        default:
          return (string) null;
      }
    }

    protected override void ReturnExistingAssetIdAndPointsInReceipt()
    {
      this._avatarAssetId = this.ExistingOffer.AvatarAssetId;
      this._priceInPoints = this.ExistingOffer.PointsPrice;
    }

    protected override void PresentOfferToUser(Offer offer, string redeemCode)
    {
      this._avatarAssetId = offer.AvatarAssetId;
      this._priceInPoints = offer.PointsPrice;
      if (offer.BodyType == MarketplaceAvatarBodyType.Unknown)
        this.HandleInvalidRedeem(redeemCode);
      else if (offer.BodyType != this._avatarBodyType && offer.BodyType != MarketplaceAvatarBodyType.Both)
        this.HandleOfferInvalidBodyType(redeemCode, this.GetBodyTypeString(offer.BodyType), this.GetBodyTypeString(this._avatarBodyType));
      else if (AvatarRedeemRequest.Blacklist != null && AvatarRedeemRequest.Blacklist.Contains(offer.AvatarAssetId))
        this.HandleBlackListedOffer(redeemCode);
      else
        this.PresentRedeemOffer(offer, redeemCode);
    }

    protected override void EnumerateTokenOfferByOfferId(int titleId, Guid offerId)
    {
      ((AvatarOfferEnumerationRequest) this._offerRequest).EnumerateTokenOffersByOfferIdAsync(titleId, 1, 1, this._avatarBodyType, (IEnumerable<Guid>) new Guid[1]
      {
        offerId
      });
    }

    protected override string GetOfferString(Offer offer)
    {
      return string.Format(UIResources.RedeemOfferMessage, (object) offer.OfferName, (object) string.Empty);
    }

    protected override PurchaseReceipt ParseReceipt(Stream response)
    {
      PurchaseReceipt receipt = new PurchaseReceiptParser().Parse(response);
      receipt.AvatarAssetIdForRedeemToken = this._avatarAssetId;
      receipt.AvatarAssetPriceInPointsForRedeemToken = this._priceInPoints;
      return receipt;
    }

    protected override string GetRedeemInputSubtitle()
    {
      return AvatarUIResources.AvatarRedeemInputSubtitle;
    }
  }
}
