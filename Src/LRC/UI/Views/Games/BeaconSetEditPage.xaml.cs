// *********************************************************
// Type: LRC.BeaconSetEditPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Resources;
using LRC.ViewModel;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Xbox.Live.Phone.Services;


namespace LRC
{
  public class BeaconSetEditPage : LrcPage
  {
    private BeaconSetEditViewModel beaconSetEditViewModel;
BusyIndicator BusyIndicator;
Grid LayoutRoot;
TextBlock PageTitle;
Grid BeaconEntry;
Image GameImage;
TextBlock GameTitle;
TextBlock FriendsPlayingNowText;
TextBlock FriendsWithBeaconsText;
TextBlock CommentLabel;
TextBox CommentBody;
Button RemoveBeaconButton;
Button SetBeaconButton;
MediaBar MediaControls;
    

    public BeaconSetEditPage()
    {
      this.InitializeComponent();
      this.NeedToSaveViewModel = true;
      this.AllowMultipleInstances = false;
      this.Persistent = false;
      this.OmniturePageName = "wp:lrc:gamedetail:beaconsetedit";
      this.OmnitureChannelName = "wp:lrc:game";
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      this.beaconSetEditViewModel = this.DataContext as BeaconSetEditViewModel;
      if (this.beaconSetEditViewModel == null)
      {
        this.beaconSetEditViewModel = new BeaconSetEditViewModel(App.GlobalData.SelectedMediaDetails as GameItem);
        App.GlobalData.SelectedMediaDetails = (ViewModelBase) null;
      }
      if (this.beaconSetEditViewModel != null)
      {
        this.DataContext = (object) this.beaconSetEditViewModel;
        this.beaconSetEditViewModel.Load();
      }
      this.OmnitureTrackPageVisit();
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      if (e != null)
        e.Cancel = this.beaconSetEditViewModel.IsBlocking;
      base.OnBackKeyPress(e);
    }

    protected override void OnError(object sender, LRCAsyncCompletedEventArgs eventArgs)
    {
      if (eventArgs == null || eventArgs.Severity == ErrorSeverity.None || string.IsNullOrEmpty(eventArgs.MessageBody) || string.IsNullOrEmpty(eventArgs.MessageTitle))
        return;
      LrcPage.LrcPageErrorManager.Nonfatal(eventArgs.MessageTitle, eventArgs.MessageBody);
    }

    private void CommentBody_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.beaconSetEditViewModel.CommentBody = this.CommentBody.Text;
    }

    private void RemoveBeaconButton_Click(object sender, RoutedEventArgs e)
    {
      this.beaconSetEditViewModel.EventRemoveBeaconCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.OnRemoveBeaconCompleted);
      this.beaconSetEditViewModel.CallBeaconService(BeaconServiceCallType.DeleteBeacon);
    }

    private void SetBeaconButton_Click(object sender, RoutedEventArgs e)
    {
      this.beaconSetEditViewModel.EventSetBeaconCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.OnSetBeaconCompleted);
      this.beaconSetEditViewModel.CallBeaconService(BeaconServiceCallType.SetBeacon);
    }

    private void OnRemoveBeaconCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      this.beaconSetEditViewModel.EventRemoveBeaconCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(this.OnRemoveBeaconCompleted);
      if (e.Error != null)
        return;
      NavHelper.SafeGoBack((Page) this);
    }

    private void OnSetBeaconCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      this.beaconSetEditViewModel.EventSetBeaconCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(this.OnSetBeaconCompleted);
      if (e.Error != null)
        return;
      NavHelper.SafeGoBack((Page) this);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Games/BeaconSetEditPage.xaml", UriKind.Relative));
      this.BusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("BusyIndicator");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.PageTitle = (TextBlock) ((FrameworkElement) this).FindName("PageTitle");
      this.BeaconEntry = (Grid) ((FrameworkElement) this).FindName("BeaconEntry");
      this.GameImage = (Image) ((FrameworkElement) this).FindName("GameImage");
      this.GameTitle = (TextBlock) ((FrameworkElement) this).FindName("GameTitle");
      this.FriendsPlayingNowText = (TextBlock) ((FrameworkElement) this).FindName("FriendsPlayingNowText");
      this.FriendsWithBeaconsText = (TextBlock) ((FrameworkElement) this).FindName("FriendsWithBeaconsText");
      this.CommentLabel = (TextBlock) ((FrameworkElement) this).FindName("CommentLabel");
      this.CommentBody = (TextBox) ((FrameworkElement) this).FindName("CommentBody");
      this.RemoveBeaconButton = (Button) ((FrameworkElement) this).FindName("RemoveBeaconButton");
      this.SetBeaconButton = (Button) ((FrameworkElement) this).FindName("SetBeaconButton");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
