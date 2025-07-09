// *********************************************************
// Type: LRC.SuccessfulConnectionPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace LRC
{
  public class SuccessfulConnectionPage : LrcPage
  {
Grid LayoutRoot;
WelcomeHeaderControl WelcomeHeader;
Ellipse shadow;
Button ContinueButton;
StackPanel InformationalPanel;
TextBlock Header;
TextBlock InformationalText1;
TextBlock InformationalText2;
Storyboard IntroStoryboard;
    

    public SuccessfulConnectionPage()
    {
      this.InitializeComponent();
      this.AllowMultipleInstances = false;
      this.NeedToSaveViewModel = false;
      this.Persistent = false;
      this.OmniturePageName = "wp:lrc:connecting:success";
      this.OmnitureChannelName = "wp:lrc:connecting";
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      this.IntroStoryboard.Begin();
      this.OmnitureTrackPageVisit();
    }

    [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "test code")]
    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);
      NavHelper.RemoveBackEntryIfNavForward(e);
    }

    private void ContinueButton_Click(object sender, RoutedEventArgs e)
    {
      App.GlobalData.HideIntroductoryScreens = true;
      NavHelper.GotoMainPageWithSlowCloudDetection((LrcPage) this);
      OmnitureAppMeasurement.Instance.TrackEvent("event14", "connection success continue button clicked");
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Connecting/SuccessfulConnectionPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.WelcomeHeader = (WelcomeHeaderControl) ((FrameworkElement) this).FindName("WelcomeHeader");
      this.shadow = (Ellipse) ((FrameworkElement) this).FindName("shadow");
      this.ContinueButton = (Button) ((FrameworkElement) this).FindName("ContinueButton");
      this.InformationalPanel = (StackPanel) ((FrameworkElement) this).FindName("InformationalPanel");
      this.Header = (TextBlock) ((FrameworkElement) this).FindName("Header");
      this.InformationalText1 = (TextBlock) ((FrameworkElement) this).FindName("InformationalText1");
      this.InformationalText2 = (TextBlock) ((FrameworkElement) this).FindName("InformationalText2");
      this.IntroStoryboard = (Storyboard) ((FrameworkElement) this).FindName("IntroStoryboard");
    }
  }
}
