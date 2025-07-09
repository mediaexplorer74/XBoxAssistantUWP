// *********************************************************
// Type: Microsoft.Phone.Marketplace.IPurchaseRequestUI
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;


namespace Microsoft.Phone.Marketplace
{
  internal interface IPurchaseRequestUI
  {
    Offer PurchaseOffer { get; set; }

    BusyMode BusyMode { get; set; }

    int? CurrentPointsBalance { get; set; }

    bool IsActive { get; }

    void Show(PurchaseUIMode mode);

    void Remove();

    void DelayOperation(Action action, int frames);

    void LoadResourceStrings();

    event EventHandler<EventArgs> RedeemPressed;

    event EventHandler<EventArgs> TermsOfUsePressed;

    event EventHandler<EventArgs> BuyPressed;

    event EventHandler<EventArgs> AddPointsPressed;

    event EventHandler<EventArgs> CancelPressed;

    event EventHandler<EventArgs> ScreenShown;

    event EventHandler<EventArgs> ScreenHidden;
  }
}
