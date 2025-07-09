// *********************************************************
// Type: Microsoft.Phone.Marketplace.EnumerateOffersCompletedEventArgs
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace Microsoft.Phone.Marketplace
{
  public class EnumerateOffersCompletedEventArgs : AsyncCompletedEventArgs
  {
    internal EnumerateOffersCompletedEventArgs(Exception error, bool cancelled, object userState)
      : base(error, cancelled, userState)
    {
    }

    public int TotalAvailable { get; internal set; }

    public ReadOnlyCollection<Offer> Offers { get; internal set; }
  }
}
