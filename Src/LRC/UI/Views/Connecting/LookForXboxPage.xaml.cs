// *********************************************************
// Type: LRC.LookForXboxPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using LRC.Session;
using LRC.ViewModel;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xbox.Live.Phone.Services;


namespace LRC
{
  public class LookForXboxPage : LrcPage
  {
    private const string ComponentName = "LookForXboxPage";
    private bool shouldGoBack;
    private BusyIndicator connectingBusyIndicator;
    private bool initialBusyIndicatorState;
Grid LayoutRoot;
VisualStateGroup ConnectingStates;
VisualState Connecting;
VisualState Error;
VisualState Connected;
Image ErrorBackground;
Button RetryButton;
Grid ErrorPane;
StackPanel TextStackPanel_ErrorTitle;
TextBlock ErrorTitle;
ScrollViewer ScrollViewerWrapper;
StackPanel TextStackPanel_Error;
TextBlock ErrorText;
StackPanel stackPanel;
TextBlock ErrorCodeLabel;
TextBlock ErrorCode;
TextBlock LoadingText;
TextBlock VersionText;
BusyIndicator busyIndicator;
Image XboxLiveConnectingTitle;
Ellipse shadow;
Ellipse shadow_Small;
Image XboxLiveSmallTitle;
WelcomeHeaderControl WelcomeHeader;
TextBlock WelcomeText;
Storyboard ConnectingStoryboard;
Storyboard ErrorStoryboard;
Storyboard TryAgainStoryboard;
    

    public LookForXboxPage()
    {
      this.InitializeComponent();
      this.AllowMultipleInstances = false;
      this.NeedToSaveViewModel = false;
      this.Persistent = false;
      this.DataContext = (object) new LookForXboxViewModel();
      object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyFileVersionAttribute), false);
      if (customAttributes.Length != 0)
        this.VersionText.Text = ((AssemblyFileVersionAttribute) customAttributes[0]).Version;
      this.OmniturePageName = "wp:lrc:connecting:lookforxbox";
      this.OmnitureChannelName = "wp:lrc:connecting";
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      if (e == null)
        return;
      LookForXboxViewModel dataContext = (LookForXboxViewModel) this.DataContext;
      if (this.shouldGoBack)
        e.Cancel = true;
      base.OnBackKeyPress(e);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      this.shouldGoBack = ((Page) this).NavigationContext.QueryString.ContainsKey("ShouldGoBack");
      App.GlobalData.ShouldLookForXbox = false;
      this.ConnectToXbox();
      this.UpdateView(LookForXboxPage.ConnectionState.Connecting);
      this.OmnitureTrackPageVisit();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      ((LookForXboxViewModel) this.DataContext).EventLookForXboxCompleted -= new EventHandler<LRCAsyncCompletedEventArgs>(this.OnLookForXboxCompleted);
      NavHelper.RemoveBackEntryIfNavForward(e);
    }

    protected override void OnError(object sender, LRCAsyncCompletedEventArgs eventArgs)
    {
      if (eventArgs == null || !eventArgs.IsError)
        return;
      this.UpdateView(LookForXboxPage.ConnectionState.Error);
    }

    private static void UpdateOmniture(bool error, LookForXboxViewModel viewModel)
    {
      if (viewModel == null)
        return;
      if (error)
      {
        OmnitureAppMeasurement.Instance.TrackLookForXboxErrorEvent("look for xbox error event", viewModel.ErrorCode, viewModel.ErrorString);
      }
      else
      {
        string sessionType = "existing session";
        if (MainViewModel.Instance.CurrentSession is LRCSession && !string.Equals(OmnitureAppMeasurement.Instance.SessionId, ((LRCSession) MainViewModel.Instance.CurrentSession).SessionId, StringComparison.OrdinalIgnoreCase))
        {
          OmnitureAppMeasurement.Instance.SessionId = ((LRCSession) MainViewModel.Instance.CurrentSession).SessionId;
          sessionType = "new session";
          OmnitureAppMeasurement.Instance.ForceOnline();
        }
        OmnitureAppMeasurement.Instance.TrackSessionStartEvent("session start", sessionType);
      }
    }

    private void OnLookForXboxCompleted(object sender, LRCAsyncCompletedEventArgs e)
    {
      LookForXboxViewModel dataContext = (LookForXboxViewModel) this.DataContext;
      if (dataContext != null)
      {
        dataContext.EventLookForXboxCompleted -= new EventHandler<LRCAsyncCompletedEventArgs>(this.OnLookForXboxCompleted);
        LookForXboxPage.UpdateOmniture(e.IsError, dataContext);
      }
      if (!e.IsError)
      {
        DirectLaunchViewModel directLaunchViewModel = App.GlobalData.DirectLaunchViewModel;
        if (directLaunchViewModel != null && directLaunchViewModel.NeedToDirectLaunch && directLaunchViewModel.LaunchItem == null)
        {
          directLaunchViewModel.EventGetLaunchDataCompleted += new EventHandler<LRCAsyncCompletedEventArgs>(this.DirectLaunchViewModel_EventGetLaunchDataCompleted);
          directLaunchViewModel.Load();
        }
        else
          this.UpdateView(LookForXboxPage.ConnectionState.Connected);
      }
      else
        this.UpdateView(LookForXboxPage.ConnectionState.Error);
    }

    private void ConnectToXbox()
    {
      if (!ServiceManagerFactory.IsRunningEmulator && !GamerServicesDispatcher.IsInitialized)
      {
        this.UpdateView(LookForXboxPage.ConnectionState.Connected);
      }
      else
      {
        LookForXboxViewModel dataContext = (LookForXboxViewModel) this.DataContext;
        dataContext.EventLookForXboxCompleted += new EventHandler<LRCAsyncCompletedEventArgs>(this.OnLookForXboxCompleted);
        dataContext.Start();
      }
    }

    private void ConnectingBusyIndicator_Loaded(object sender, RoutedEventArgs e)
    {
      this.connectingBusyIndicator = sender as BusyIndicator;
      this.ToggleBusyIndicator(this.initialBusyIndicatorState);
    }

    private void RetryButton_Click(object sender, RoutedEventArgs e)
    {
      this.ConnectToXbox();
      this.UpdateView(LookForXboxPage.ConnectionState.TryAgain);
      OmnitureAppMeasurement.Instance.TrackEvent("event15", "look for xbox try again button clicked");
    }

    private void ToggleBusyIndicator(bool enabled)
    {
      if (this.connectingBusyIndicator != null)
        this.connectingBusyIndicator.IsBusy = enabled;
      else
        this.initialBusyIndicatorState = enabled;
    }

    private void UpdateView(LookForXboxPage.ConnectionState state)
    {
      this.ConnectingStoryboard.Stop();
      this.ErrorStoryboard.Stop();
      this.TryAgainStoryboard.Stop();
      switch (state)
      {
        case LookForXboxPage.ConnectionState.Connecting:
          this.ToggleBusyIndicator(true);
          ((Timeline) this.ConnectingStoryboard).Completed += new EventHandler(this.ConnectingStoryboard_Completed);
          this.ConnectingStoryboard.Begin();
          break;
        case LookForXboxPage.ConnectionState.Connected:
          this.ToggleBusyIndicator(false);
          this.OnConnected();
          break;
        case LookForXboxPage.ConnectionState.Error:
          this.ToggleBusyIndicator(false);
          ((Timeline) this.ErrorStoryboard).Completed += new EventHandler(this.ErrorStoryboard_Completed);
          this.ErrorStoryboard.Begin();
          break;
        case LookForXboxPage.ConnectionState.TryAgain:
          this.ToggleBusyIndicator(true);
          ((Timeline) this.TryAgainStoryboard).Completed += new EventHandler(this.TryAgainStoryboard_Completed);
          this.TryAgainStoryboard.Begin();
          break;
      }
    }

    private void ConnectingStoryboard_Completed(object sender, EventArgs e)
    {
      ((Timeline) this.ConnectingStoryboard).Completed -= new EventHandler(this.ConnectingStoryboard_Completed);
      VisualStateManager.GoToState((Control) this, "Connecting", true);
    }

    private void OnConnected()
    {
      if (this.shouldGoBack)
        NavHelper.SafeGoBack((Page) this);
      else if (App.GlobalData.HideIntroductoryScreens)
        NavHelper.GotoMainPageWithSlowCloudDetection((LrcPage) this);
      else
        NavHelper.SafeNavigate((Page) this, new Uri("/UI/Views/Connecting/SuccessfulConnectionPage.xaml", UriKind.Relative));
    }

    private void ErrorStoryboard_Completed(object sender, EventArgs e)
    {
      ((Timeline) this.ErrorStoryboard).Completed -= new EventHandler(this.ErrorStoryboard_Completed);
      VisualStateManager.GoToState((Control) this, "Error", true);
      if (!this.shouldGoBack)
        return;
      if (LrcNavigation.Instance.IsPreviousPageMainPage)
        NavHelper.SafeGoBack((Page) this);
      else
        NavHelper.GotoMainpage((LrcPage) this, App.RootFrameState.ConnectError);
    }

    private void TryAgainStoryboard_Completed(object sender, EventArgs e)
    {
      ((Timeline) this.TryAgainStoryboard).Completed -= new EventHandler(this.TryAgainStoryboard_Completed);
      VisualStateManager.GoToState((Control) this, "Connecting", true);
    }

    private void DirectLaunchViewModel_EventGetLaunchDataCompleted(
      object sender,
      LRCAsyncCompletedEventArgs e)
    {
      if (sender is DirectLaunchViewModel directLaunchViewModel)
        directLaunchViewModel.EventGetLaunchDataCompleted -= new EventHandler<LRCAsyncCompletedEventArgs>(this.DirectLaunchViewModel_EventGetLaunchDataCompleted);
      this.UpdateView(LookForXboxPage.ConnectionState.Connected);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Connecting/LookForXboxPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.ConnectingStates = (VisualStateGroup) ((FrameworkElement) this).FindName("ConnectingStates");
      this.Connecting = (VisualState) ((FrameworkElement) this).FindName("Connecting");
      this.Error = (VisualState) ((FrameworkElement) this).FindName("Error");
      this.Connected = (VisualState) ((FrameworkElement) this).FindName("Connected");
      this.ErrorBackground = (Image) ((FrameworkElement) this).FindName("ErrorBackground");
      this.RetryButton = (Button) ((FrameworkElement) this).FindName("RetryButton");
      this.ErrorPane = (Grid) ((FrameworkElement) this).FindName("ErrorPane");
      this.TextStackPanel_ErrorTitle = (StackPanel) ((FrameworkElement) this).FindName("TextStackPanel_ErrorTitle");
      this.ErrorTitle = (TextBlock) ((FrameworkElement) this).FindName("ErrorTitle");
      this.ScrollViewerWrapper = (ScrollViewer) ((FrameworkElement) this).FindName("ScrollViewerWrapper");
      this.TextStackPanel_Error = (StackPanel) ((FrameworkElement) this).FindName("TextStackPanel_Error");
      this.ErrorText = (TextBlock) ((FrameworkElement) this).FindName("ErrorText");
      this.stackPanel = (StackPanel) ((FrameworkElement) this).FindName("stackPanel");
      this.ErrorCodeLabel = (TextBlock) ((FrameworkElement) this).FindName("ErrorCodeLabel");
      this.ErrorCode = (TextBlock) ((FrameworkElement) this).FindName("ErrorCode");
      this.LoadingText = (TextBlock) ((FrameworkElement) this).FindName("LoadingText");
      this.VersionText = (TextBlock) ((FrameworkElement) this).FindName("VersionText");
      this.busyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("busyIndicator");
      this.XboxLiveConnectingTitle = (Image) ((FrameworkElement) this).FindName("XboxLiveConnectingTitle");
      this.shadow = (Ellipse) ((FrameworkElement) this).FindName("shadow");
      this.shadow_Small = (Ellipse) ((FrameworkElement) this).FindName("shadow_Small");
      this.XboxLiveSmallTitle = (Image) ((FrameworkElement) this).FindName("XboxLiveSmallTitle");
      this.WelcomeHeader = (WelcomeHeaderControl) ((FrameworkElement) this).FindName("WelcomeHeader");
      this.WelcomeText = (TextBlock) ((FrameworkElement) this).FindName("WelcomeText");
      this.ConnectingStoryboard = (Storyboard) ((FrameworkElement) this).FindName("ConnectingStoryboard");
      this.ErrorStoryboard = (Storyboard) ((FrameworkElement) this).FindName("ErrorStoryboard");
      this.TryAgainStoryboard = (Storyboard) ((FrameworkElement) this).FindName("TryAgainStoryboard");
    }

    private enum ConnectionState
    {
      Unknown,
      Connecting,
      Connected,
      Error,
      TryAgain,
    }
  }
}
