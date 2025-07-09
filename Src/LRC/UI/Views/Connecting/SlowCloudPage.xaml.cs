// *********************************************************
// Type: LRC.SlowCloudPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace LRC
{
  public class SlowCloudPage : LrcPage
  {
Storyboard IntroStoryboard;
Grid LayoutRoot;
ScrollViewer scrollViewer;
StackPanel InformationalPanel;
TextBlock Header;
TextBlock Description1;
TextBlock Description2;
WelcomeHeaderControl WelcomeHeader;
Ellipse shadow;
Button ContinueButton;
    

    public SlowCloudPage()
    {
      this.InitializeComponent();
      this.AllowMultipleInstances = false;
      this.NeedToSaveViewModel = false;
      this.Persistent = false;
      this.OmniturePageName = "wp:lrc:connecting:slowcloud";
      this.OmnitureChannelName = "wp:lrc:connecting";
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      this.OmnitureTrackPageVisit();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);
      this.IntroStoryboard.Begin();
      NavHelper.RemoveBackEntryIfNavForward(e);
    }

    private void ContinueButton_Click(object sender, RoutedEventArgs e)
    {
      NavHelper.GotoMainpage((LrcPage) this, App.RootFrameState.ConnectedLoading);
      OmnitureAppMeasurement.Instance.TrackEvent("event16", "slow cloud continue button clicked");
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Connecting/SlowCloudPage.xaml", UriKind.Relative));
      this.IntroStoryboard = (Storyboard) ((FrameworkElement) this).FindName("IntroStoryboard");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.scrollViewer = (ScrollViewer) ((FrameworkElement) this).FindName("scrollViewer");
      this.InformationalPanel = (StackPanel) ((FrameworkElement) this).FindName("InformationalPanel");
      this.Header = (TextBlock) ((FrameworkElement) this).FindName("Header");
      this.Description1 = (TextBlock) ((FrameworkElement) this).FindName("Description1");
      this.Description2 = (TextBlock) ((FrameworkElement) this).FindName("Description2");
      this.WelcomeHeader = (WelcomeHeaderControl) ((FrameworkElement) this).FindName("WelcomeHeader");
      this.shadow = (Ellipse) ((FrameworkElement) this).FindName("shadow");
      this.ContinueButton = (Button) ((FrameworkElement) this).FindName("ContinueButton");
    }
  }
}
