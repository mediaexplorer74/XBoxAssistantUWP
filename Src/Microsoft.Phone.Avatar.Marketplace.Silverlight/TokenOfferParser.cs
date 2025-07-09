// *********************************************************
// Type: Microsoft.Phone.Marketplace.TokenOfferParser
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System.IO;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  internal class TokenOfferParser
  {
    internal TokenOffer Parse(Stream stream)
    {
      XElement xelement = XElement.Load(stream);
      TokenOffer tokenOffer = new TokenOffer();
      XElement e1 = xelement.Element(XmlNamespaces.PdlcNs + "XboxOffer");
      if (e1.IsNotNull())
      {
        tokenOffer.XboxOffer = new XboxOffer();
        tokenOffer.XboxOffer.OfferId = (ulong) e1.Element(XmlNamespaces.PdlcNs + "offerId");
        tokenOffer.XboxOffer.OfferTypeId = (int) e1.Element(XmlNamespaces.PdlcNs + "offerTypeId");
      }
      XElement e2 = xelement.Element(XmlNamespaces.PdlcNs + "EmsOffer");
      if (e2.IsNotNull())
      {
        tokenOffer.EmsOffer = new EmsOffer();
        tokenOffer.EmsOffer.OfferId = e2.Element(XmlNamespaces.PdlcNs + "offerId").ParseGuid();
        tokenOffer.EmsOffer.MediaId = e2.Element(XmlNamespaces.PdlcNs + "mediaId").ParseGuid();
        tokenOffer.EmsOffer.MediaType = (int) e2.Element(XmlNamespaces.PdlcNs + "mediaType");
      }
      return tokenOffer;
    }
  }
}
