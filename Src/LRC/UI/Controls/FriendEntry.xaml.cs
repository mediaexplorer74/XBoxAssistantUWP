// *********************************************************
// Type: LRC.FriendEntry
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace LRC
{
  public partial class FriendEntry : Button
  {
Image Gamertile;
StackPanel FriendDetails;
TextBlock GamerTag;
TextBlock PlayingText;
StackPanel BeaconGroup;
Image BeaconIcon;
TextBlock BeaconText;
Image OfflineIcon;
Image OnlineIcon;
    

    public FriendEntry() => this.InitializeComponent();

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/FriendEntry.xaml", UriKind.Relative));
      this.Gamertile = (Image) ((FrameworkElement) this).FindName("Gamertile");
      this.FriendDetails = (StackPanel) ((FrameworkElement) this).FindName("FriendDetails");
      this.GamerTag = (TextBlock) ((FrameworkElement) this).FindName("GamerTag");
      this.PlayingText = (TextBlock) ((FrameworkElement) this).FindName("PlayingText");
      this.BeaconGroup = (StackPanel) ((FrameworkElement) this).FindName("BeaconGroup");
      this.BeaconIcon = (Image) ((FrameworkElement) this).FindName("BeaconIcon");
      this.BeaconText = (TextBlock) ((FrameworkElement) this).FindName("BeaconText");
      this.OfflineIcon = (Image) ((FrameworkElement) this).FindName("OfflineIcon");
      this.OnlineIcon = (Image) ((FrameworkElement) this).FindName("OnlineIcon");
    }
  }
}
