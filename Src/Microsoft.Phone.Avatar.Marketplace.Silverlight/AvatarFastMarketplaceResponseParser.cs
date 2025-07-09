// *********************************************************
// Type: Microsoft.Phone.Marketplace.AvatarFastMarketplaceResponseParser
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  internal class AvatarFastMarketplaceResponseParser : FastMarketplaceResponseParser
  {
    protected override void ParseAssetIdBodyType(XElement e, Offer offer)
    {
      XElement guidElement = e.Element(XmlNamespaces.FastNs + "avatarItemAssetId");
      if (guidElement != null)
        offer.AvatarAssetId = guidElement.ParseGuid();
      XElement xelement = e.Element(XmlNamespaces.FastNs + "avatarBodyType");
      if (xelement == null)
        return;
      offer.BodyType = (MarketplaceAvatarBodyType) Enum.Parse(typeof (MarketplaceAvatarBodyType), xelement.Value, true);
    }

    protected override int ParseMediaTypeId(XElement e, Offer offer)
    {
      XElement xelement = e.Element(XmlNamespaces.FastNs + "productType");
      return xelement != null ? int.Parse(xelement.Value) : 47;
    }

    protected override int ParseStoreId(XElement offerInstance)
    {
      XElement xelement = offerInstance.Element(XmlNamespaces.FastNs + "stores").Element(XmlNamespaces.FastNs + "store");
      return xelement != null ? int.Parse(xelement.Value) : 1;
    }

    internal ReadOnlyCollection<Product> ParseGames(Stream responseStream, out int totalAvailable)
    {
      totalAvailable = 0;
      XElement xelement = XElement.Load(XmlReader.Create(responseStream));
      totalAvailable = (int) xelement.Element(XmlNamespaces.FastNs + "totalItems");
      return new ReadOnlyCollection<Product>((IList<Product>) xelement.Elements(XmlNamespaces.AtomNs + "entry").Select<XElement, Product>((Func<XElement, Product>) (e => this.CreateGame(e))).Where<Product>((Func<Product, bool>) (g => g != null)).ToList<Product>());
    }

    private Product CreateGame(XElement e)
    {
      Product game = new Product();
      XElement guidElement = e.Element(XmlNamespaces.AtomNs + "id");
      if (guidElement == null)
        return (Product) null;
      game.GameId = guidElement.ParseGuid();
      XElement xelement1 = e.Element(XmlNamespaces.AtomNs + "title");
      if (xelement1 != null)
        game.Title = (string) xelement1;
      XElement xelement2 = e.Element(XmlNamespaces.FastNs + "images");
      if (xelement2 != null)
      {
        IEnumerable<XElement> source = xelement2.Elements(XmlNamespaces.FastNs + "image").Where<XElement>((Func<XElement, bool>) (element => (int) element.Element(XmlNamespaces.FastNs + "relationshipType") == 33 && (int) element.Element(XmlNamespaces.FastNs + "size") == 23));
        if (source.Count<XElement>() > 0)
          game.ThumbnailUrl = (string) source.First<XElement>().Element(XmlNamespaces.FastNs + "fileUrl");
      }
      return game;
    }
  }
}
