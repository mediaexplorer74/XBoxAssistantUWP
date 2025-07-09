// *********************************************************
// Type: Microsoft.Phone.Marketplace.PurchaseHistoryParser
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
  internal class PurchaseHistoryParser
  {
    internal ReadOnlyCollection<PurchaseReceipt> Parse(
      Stream responseStream,
      out int totalAvailable)
    {
      totalAvailable = 0;
      PurchaseReceiptParser receiptParser = new PurchaseReceiptParser();
      XElement xelement = XElement.Load(XmlReader.Create(responseStream));
      XNamespace pdlcNs = XmlNamespaces.PdlcNs;
      totalAvailable = (int) xelement.Element(pdlcNs + "TotalItems");
      return new ReadOnlyCollection<PurchaseReceipt>((IList<PurchaseReceipt>) xelement.Element(pdlcNs + "ReceiptsPage").Elements(pdlcNs + "PurchaseReceipt").Select<XElement, PurchaseReceipt>((Func<XElement, PurchaseReceipt>) (e => receiptParser.ParseReceipt(e))).ToList<PurchaseReceipt>());
    }
  }
}
