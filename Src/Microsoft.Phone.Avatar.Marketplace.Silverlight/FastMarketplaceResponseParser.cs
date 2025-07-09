// *********************************************************
// Type: Microsoft.Phone.Marketplace.FastMarketplaceResponseParser
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  internal class FastMarketplaceResponseParser
  {
    private static Dictionary<int, OfferImageFormat> SupportedImageFormats = new Dictionary<int, OfferImageFormat>()
    {
      {
        4,
        OfferImageFormat.Jpg
      },
      {
        5,
        OfferImageFormat.Png
      }
    };
    private static Dictionary<int, OfferImageSize> SupportedImageSizes = new Dictionary<int, OfferImageSize>()
    {
      {
        14,
        OfferImageSize.Thumbnail
      },
      {
        26,
        OfferImageSize.LargeThumbnail
      }
    };

    internal ReadOnlyCollection<Offer> Parse(
      Stream responseStream,
      int tier,
      int paymentType,
      out int totalAvailable)
    {
      totalAvailable = 0;
      XElement xelement = XElement.Load(XmlReader.Create(responseStream));
      totalAvailable = (int) xelement.Element(XmlNamespaces.FastNs + "totalItems");
      return new ReadOnlyCollection<Offer>((IList<Offer>) xelement.Elements(XmlNamespaces.AtomNs + "entry").Select<XElement, Offer>((Func<XElement, Offer>) (e => this.CreateOffer(e, tier, paymentType))).Where<Offer>((Func<Offer, bool>) (o => o != null)).ToList<Offer>());
    }

    protected virtual void ParseAssetIdBodyType(XElement e, Offer offer)
    {
    }

    protected virtual int ParseMediaTypeId(XElement e, Offer offer)
    {
      return offer.Asset == null ? 59 : 60;
    }

    protected virtual int ParseStoreId(XElement offerInstance) => 5;

    private Offer CreateOffer(XElement e, int tier, int paymentType)
    {
      Offer offer = new Offer();
      XElement offerInstance = e.Element(XmlNamespaces.FastNs + "offerInstances").Elements(XmlNamespaces.FastNs + "offerInstance").Where<XElement>((Func<XElement, bool>) (o => o.Element(XmlNamespaces.FastNs + "tiers").Elements(XmlNamespaces.FastNs + nameof (tier)).Select<XElement, int>((Func<XElement, int>) (t => (int) t)).Contains<int>(tier) && (int) o.Element(XmlNamespaces.FastNs + nameof (paymentType)) == paymentType)).FirstOrDefault<XElement>();
      if (offerInstance == null)
        return (Offer) null;
      DateTime dateTime1 = DateTime.Parse((string) offerInstance.Element(XmlNamespaces.FastNs + "startDate"), (IFormatProvider) null, DateTimeStyles.AdjustToUniversal);
      offer.ReleaseDate = dateTime1;
      DateTime dateTime2 = DateTime.Parse((string) offerInstance.Element(XmlNamespaces.FastNs + "endDate"), (IFormatProvider) null, DateTimeStyles.AdjustToUniversal);
      DateTime utcNow = DateTime.UtcNow;
      if (utcNow < dateTime1 || utcNow > dateTime2)
        return (Offer) null;
      offer.OfferId = offerInstance.Element(XmlNamespaces.FastNs + "offerId").ParseGuid();
      XElement xelement1 = offerInstance.Element(XmlNamespaces.FastNs + "price");
      if (xelement1 == null)
        return (Offer) null;
      offer.PointsPrice = (int) (float) xelement1;
      XElement xelement2 = e.Element(XmlNamespaces.FastNs + "fullTitle");
      if (xelement2 == null)
        return (Offer) null;
      offer.OfferName = (string) xelement2;
      XElement xelement3 = e.Element(XmlNamespaces.FastNs + "fullDescription");
      if (xelement3 != null)
        offer.SellText = (string) xelement3;
      XElement xelement4 = e.Element(XmlNamespaces.FastNs + "titleId");
      if (xelement4 == null)
        return (Offer) null;
      offer.TitleId = (int) xelement4;
      XElement xelement5 = e.Element(XmlNamespaces.FastNs + "gameReducedTitle");
      if (xelement5 != null)
        offer.TitleName = (string) xelement5;
      this.ParseAssetIdBodyType(e, offer);
      XElement xelement6 = offerInstance.Element(XmlNamespaces.FastNs + "productInstance");
      if (xelement6 != null)
      {
        offer.DownloadSize = (long) xelement6.Element(XmlNamespaces.FastNs + "packageSize");
        XElement assetId = xelement6.Element(XmlNamespaces.FastNs + "assetId");
        XElement quanity = xelement6.Element(XmlNamespaces.FastNs + "purchaseQuantity");
        if (assetId != null && quanity != null)
          offer.Asset = new Asset((int) assetId, (int) quanity);
      }
      XElement xelement7 = offerInstance.Element(XmlNamespaces.FastNs + "hexOfferId");
      if (xelement7 != null)
      {
        string s = (string) xelement7;
        if (s.StartsWith("0x"))
          s = s.Substring(2);
        offer.LegacyOfferId = ulong.Parse(s, NumberStyles.HexNumber);
      }
      XElement xelement8 = e.Element(XmlNamespaces.FastNs + "categories");
      if (xelement8 != null)
      {
        int num = xelement8.Elements(XmlNamespaces.FastNs + "category").Select(category => new
        {
          category = category,
          id = (int) category.Element(XmlNamespaces.FastNs + "categoryId")
        }).Where(_param0 => _param0.id > 12000 & _param0.id < 12033).Select(_param0 => _param0.id - 12000).FirstOrDefault<int>();
        if (num > 0)
          offer.ContentCategory = 1 << num - 1;
      }
      XElement xelement9 = e.Element(XmlNamespaces.FastNs + "images");
      offer.Images = xelement9 == null ? new ReadOnlyCollection<OfferImage>((IList<OfferImage>) new List<OfferImage>()) : new ReadOnlyCollection<OfferImage>((IList<OfferImage>) xelement9.Elements(XmlNamespaces.FastNs + "image").Select<XElement, OfferImage>((Func<XElement, OfferImage>) (image => this.CreateOfferImage(image))).Where<OfferImage>((Func<OfferImage, bool>) (i => i != null)).ToList<OfferImage>());
      offer.MediaTypeId = this.ParseMediaTypeId(e, offer);
      offer.StoreId = this.ParseStoreId(offerInstance);
      return offer;
    }

    protected OfferImage CreateOfferImage(XElement image)
    {
      XElement key1 = image.Element(XmlNamespaces.FastNs + "format");
      XElement key2 = image.Element(XmlNamespaces.FastNs + "size");
      XElement uriString = image.Element(XmlNamespaces.FastNs + "fileUrl");
      if (uriString == null)
        return (OfferImage) null;
      OfferImageFormat offerImageFormat = OfferImageFormat.Png;
      if (key1 == null || !FastMarketplaceResponseParser.SupportedImageFormats.TryGetValue((int) key1, out offerImageFormat))
        return (OfferImage) null;
      OfferImageSize offerImageSize = OfferImageSize.Thumbnail;
      if (key2 == null || !FastMarketplaceResponseParser.SupportedImageSizes.TryGetValue((int) key2, out offerImageSize))
        return (OfferImage) null;
      return new OfferImage()
      {
        Format = offerImageFormat,
        Size = offerImageSize,
        DownloadUrl = new Uri((string) uriString)
      };
    }
  }
}
