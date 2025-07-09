// *********************************************************
// Type: Xbox.Live.Phone.Services.AvatarItem
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;


namespace Xbox.Live.Phone.Services
{
  public class AvatarItem : Product
  {
    public Guid AvatarAssetId { get; set; }

    public Guid OfferId { get; set; }

    public Guid ProductId { get; set; }

    public string Description { get; set; }

    public int PriceInPoint { get; set; }
  }
}
