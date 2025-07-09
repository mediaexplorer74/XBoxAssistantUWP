// *********************************************************
// Type: Microsoft.Phone.Marketplace.OfferImage
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;


namespace Microsoft.Phone.Marketplace
{
  public class OfferImage
  {
    internal OfferImage()
    {
    }

    public OfferImageFormat Format { get; internal set; }

    public OfferImageSize Size { get; internal set; }

    public Uri DownloadUrl { get; internal set; }
  }
}
