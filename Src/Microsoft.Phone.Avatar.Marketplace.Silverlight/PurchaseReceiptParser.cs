// *********************************************************
// Type: Microsoft.Phone.Marketplace.PurchaseReceiptParser
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  internal class PurchaseReceiptParser
  {
    internal PurchaseReceipt Parse(Stream stream) => this.ParseReceipt(XElement.Load(stream));

    internal PurchaseReceipt ParseReceipt(XElement root)
    {
      PurchaseReceipt receipt = new PurchaseReceipt();
      XNamespace pdlcNs = XmlNamespaces.PdlcNs;
      receipt.TransactionId = root.Element(pdlcNs + "TransactionId").ParseGuid();
      receipt.OfferId = root.Element(pdlcNs + "OfferId").ParseGuid();
      XElement e = root.Element(pdlcNs + "Title");
      if (e.IsNotNull())
        receipt.OfferName = (string) e;
      receipt.PurchaseDate = DateTime.Parse((string) root.Element(pdlcNs + "PurchaseDate"), (IFormatProvider) null, DateTimeStyles.AdjustToUniversal);
      receipt.DownloadUrls = new ReadOnlyCollection<Uri>((IList<Uri>) root.Element(pdlcNs + "MediaInstanceURLs").Elements(XmlNamespaces.ArraysNs + "string").Where<XElement>((Func<XElement, bool>) (url => DownloadVerifier.IsValidDownloadUri(new Uri((string) url)))).Select<XElement, Uri>((Func<XElement, Uri>) (url => new Uri((string) url))).ToList<Uri>());
      XElement xelement = root.Element(pdlcNs + "AssetBalance");
      if (xelement.IsNotNull())
      {
        AssetCountParser assetCountParser = new AssetCountParser();
        receipt.AssetBalance = assetCountParser.ParseAsset(xelement);
        if (receipt.AssetBalance != null && (receipt.AssetBalance.DownloadUrls == null || receipt.AssetBalance.DownloadUrls.Count == 0))
          receipt.AssetBalance.DownloadUrls = new ReadOnlyCollection<Uri>((IList<Uri>) receipt.DownloadUrls);
      }
      return receipt;
    }
  }
}
