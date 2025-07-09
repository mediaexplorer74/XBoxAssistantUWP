// *********************************************************
// Type: Xbox.Live.Phone.Services.OfferUserState
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;


namespace Xbox.Live.Phone.Services
{
  public class OfferUserState : ProductUserState
  {
    public int CategoryId { get; set; }

    public Guid GameId { get; set; }
  }
}
