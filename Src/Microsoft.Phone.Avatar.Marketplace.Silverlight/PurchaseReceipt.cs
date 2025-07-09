// *********************************************************
// Type: Microsoft.Phone.Marketplace.PurchaseReceipt
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Collections.ObjectModel;


namespace Microsoft.Phone.Marketplace
{
  public class PurchaseReceipt
  {
    internal PurchaseReceipt()
    {
    }

    public Guid TransactionId { get; internal set; }

    public Guid OfferId { get; internal set; }

    public string OfferName { get; internal set; }

    public DateTime PurchaseDate { get; internal set; }

    public ReadOnlyCollection<Uri> DownloadUrls { get; internal set; }

    public Asset AssetBalance { get; internal set; }

    public Guid AvatarAssetIdForRedeemToken { get; internal set; }

    public int AvatarAssetPriceInPointsForRedeemToken { get; internal set; }
  }
}
