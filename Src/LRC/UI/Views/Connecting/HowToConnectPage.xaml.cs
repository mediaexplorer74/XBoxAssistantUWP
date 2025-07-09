// *********************************************************
// Type: LRC.HowToConnectPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace LRC
{
  public class HowToConnectPage : LrcPage
  {
Grid LayoutRoot;
TextBlock Header;
ScrollViewer ScrollViewerWrapper;
StackPanel InformationalPanel;
TextBlock InformationalText1;
TextBlock BulletHeader;
TextBlock Bullet1;
TextBlock Bullet2;
Button ContinueButton;
Ellipse shadow;
    

    public HowToConnectPage()
    {
      this.InitializeComponent();
      this.AllowMultipleInstances = false;
      this.NeedToSaveViewModel = false;
      this.Persistent = false;
      this.OmniturePageName = "wp:lrc:connecting:welcome";
      this.OmnitureChannelName = "wp:lrc:connecting";
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (App.GlobalData.HideIntroductoryScreens)
        NavHelper.GotoLookForXboxPage((Page) this, false);
      else
        this.OmnitureTrackPageVisit();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      NavHelper.RemoveBackEntryIfNavForward(e);
      base.OnNavigatedFrom(e);
    }

    private void ContinueButton_Click(object sender, RoutedEventArgs e)
    {
      NavHelper.GotoLookForXboxPage((Page) this, false);
      OmnitureAppMeasurement.Instance.TrackEvent("event13", "welcome next button clicked");
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Connecting/HowToConnectPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.Header = (TextBlock) ((FrameworkElement) this).FindName("Header");
      this.ScrollViewerWrapper = (ScrollViewer) ((FrameworkElement) this).FindName("ScrollViewerWrapper");
      this.InformationalPanel = (StackPanel) ((FrameworkElement) this).FindName("InformationalPanel");
      this.InformationalText1 = (TextBlock) ((FrameworkElement) this).FindName("InformationalText1");
      this.BulletHeader = (TextBlock) ((FrameworkElement) this).FindName("BulletHeader");
      this.Bullet1 = (TextBlock) ((FrameworkElement) this).FindName("Bullet1");
      this.Bullet2 = (TextBlock) ((FrameworkElement) this).FindName("Bullet2");
      this.ContinueButton = (Button) ((FrameworkElement) this).FindName("ContinueButton");
      this.shadow = (Ellipse) ((FrameworkElement) this).FindName("shadow");
    }
  }
}
