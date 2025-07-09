// *********************************************************
// Type: Microsoft.Phone.Marketplace.Offer
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Collections.ObjectModel;


namespace Microsoft.Phone.Marketplace
{
  public class Offer
  {
    public Guid AvatarAssetId { get; internal set; }

    public MarketplaceAvatarBodyType BodyType { get; internal set; }

    public bool IsBlocked { get; set; }

    public object OfferImage { get; set; }

    internal Offer()
    {
    }

    public Guid OfferId { get; internal set; }

    public Asset Asset { get; internal set; }

    public int TitleId { get; internal set; }

    public string TitleName { get; internal set; }

    public string OfferName { get; internal set; }

    public string SellText { get; internal set; }

    public int PointsPrice { get; internal set; }

    public long DownloadSize { get; internal set; }

    public DateTime ReleaseDate { get; internal set; }

    public int ContentCategory { get; internal set; }

    public ReadOnlyCollection<Microsoft.Phone.Marketplace.OfferImage> Images { get; internal set; }

    internal ulong LegacyOfferId { get; set; }

    internal int MediaTypeId { get; set; }

    internal int StoreId { get; set; }
  }
}
