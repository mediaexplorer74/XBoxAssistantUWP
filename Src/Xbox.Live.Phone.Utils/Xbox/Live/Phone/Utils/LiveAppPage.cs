// *********************************************************
// Type: Xbox.Live.Phone.Utils.LiveAppPage
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using Microsoft.Phone.Marketplace;
using System.Windows.Controls;


namespace Xbox.Live.Phone.Utils
{
  public class LiveAppPage
  {
    public static Page Current { get; set; }

    public static AvatarPurchaseRequest CurrentPurchaseRequest { get; set; }
  }
}
