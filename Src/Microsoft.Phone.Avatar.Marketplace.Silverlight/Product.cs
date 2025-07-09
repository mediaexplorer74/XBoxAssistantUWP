// *********************************************************
// Type: Microsoft.Phone.Marketplace.Product
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;


namespace Microsoft.Phone.Marketplace
{
  public class Product
  {
    internal Product()
    {
    }

    public Guid GameId { get; internal set; }

    public string Title { get; internal set; }

    public string ThumbnailUrl { get; internal set; }
  }
}
