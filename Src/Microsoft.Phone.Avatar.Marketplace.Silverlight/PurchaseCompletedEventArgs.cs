// *********************************************************
// Type: Microsoft.Phone.Marketplace.PurchaseCompletedEventArgs
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.ComponentModel;


namespace Microsoft.Phone.Marketplace
{
  public class PurchaseCompletedEventArgs : AsyncCompletedEventArgs
  {
    internal PurchaseCompletedEventArgs(Exception error, bool cancelled, object userState)
      : base(error, cancelled, userState)
    {
    }

    public PurchaseReceipt PurchaseReceipt { get; internal set; }

    public Offer PurchaseOffer { get; internal set; }

    public Guid TransactionId { get; internal set; }

    internal bool CanceledByUser { get; set; }
  }
}
