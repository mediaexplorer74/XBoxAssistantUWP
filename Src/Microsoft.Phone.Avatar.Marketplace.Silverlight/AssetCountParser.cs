// *********************************************************
// Type: Microsoft.Phone.Marketplace.AssetCountParser
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  internal class AssetCountParser
  {
    internal ReadOnlyCollection<Asset> Parse(Stream stream)
    {
      return new ReadOnlyCollection<Asset>((IList<Asset>) XElement.Load(stream).Element(XmlNamespaces.PdlcNs + "MediaAssetsList").Elements(XmlNamespaces.PdlcNs + "MediaAsset").Select<XElement, Asset>(new Func<XElement, Asset>(this.ParseAsset)).ToList<Asset>());
    }

    internal Asset ParseAsset(XElement root)
    {
      XNamespace pdlcNs = XmlNamespaces.PdlcNs;
      Asset asset = new Asset((int) root.Element(pdlcNs + "AssetId"), (int) root.Element(pdlcNs + "Quantity"));
      XElement e = root.Element(pdlcNs + "MediaInstanceURLs");
      if (e.IsNotNull())
        asset.DownloadUrls = new ReadOnlyCollection<Uri>((IList<Uri>) e.Elements(XmlNamespaces.ArraysNs + "string").Select<XElement, Uri>((Func<XElement, Uri>) (url => new Uri((string) url))).ToList<Uri>());
      return asset;
    }
  }
}
