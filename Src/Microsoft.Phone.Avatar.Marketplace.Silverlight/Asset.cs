// *********************************************************
// Type: Microsoft.Phone.Marketplace.Asset
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Collections.ObjectModel;


namespace Microsoft.Phone.Marketplace
{
  public class Asset
  {
    public Asset(int assetId, int quanity)
    {
      this.AssetId = assetId;
      this.Quantity = quanity;
    }

    public int AssetId { get; private set; }

    public int Quantity { get; private set; }

    public ReadOnlyCollection<Uri> DownloadUrls { get; internal set; }
  }
}
