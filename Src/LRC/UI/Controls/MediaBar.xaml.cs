// *********************************************************
// Type: LRC.MediaBar
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using LRC.ViewModel;
using Microsoft.Devices;
using Microsoft.Xmedia.Client.WindowsPhone;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Xbox.Live.Phone.Utils;


namespace LRC
{
  [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Ready", GroupName = "CustomStates")]
  [TemplateVisualState(Name = "Waiting", GroupName = "CustomStates")]
  [TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
  [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Pressed", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Focused", GroupName = "FocusStates")]
  [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
  public partial class MediaBar : UserControl, IDisposable
  {
    private const string ComponentName = "MediaBar";
    private const int PressAndHoldTimerInterval = 500;
    private const int HapticTime = 15;
    private const string SkipBackButtonName = "SkipBackButton";
    private const string RewindButtonName = "RewindButton";
    private const string PlayPauseButtonName = "PlayPauseButton";
    private const string FastForwardButtonName = "FastForwardButton";
    private const string SkipForwardButtonName = "SkipForwardButton";
    private const string ButtonVisualStatePlay = "Play";
    private const string ButtonVisualStatePause = "Pause";
    private const string ButtonVisualStatePlayTapped = "PlayTapped";
    private const string ButtonVisualStatePauseTapped = "PauseTapped";
    private const string ButtonVisualStateTapped = "Waiting";
    private const string ButtonVisualStateReady = "Ready";
    private const string DpadA = "Dpad:A";
    private const string DpadB = "Dpad:B";
    private const string DpadX = "Dpad:X";
    private const string DpadY = "Dpad:Y";
    private const string DpadUp = "Dpad:Up";
    private const string DpadLeft = "Dpad:Left";
    private const string DpadDown = "Dpad:Down";
    private const string DpadRight = "Dpad:Right";
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof (IsExpanded), typeof (bool), typeof (MediaBar), new PropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((MediaBar) d).ChangeVisualState())));
    private NowPlayingMediaState mediaStateBeforeButtonTap;
    private Button clickedButton;
    private Button skipButtonPressed;
    private Button fastForwardButton = new Button();
    private Button rewindButton = new Button();
    private bool hasPressAndHoldTimerFired;
    private Timer pressAndHoldTimer;
    private VibrateController vibrateController = VibrateController.Default;
Grid LayoutRoot;
VisualStateGroup CustomStates;
VisualState Collapsed;
VisualState Expanded;
Grid MediaBarContainer;
ProgressBar MediaProgressBar;
Grid PositionAndDuration;
TextBlock PositionText;
TextBlock DurationText;
Grid MediaControls;
MediaControlButton NowPlayingLink;
MediaControlButton SkipBackButton;
Button PlayPauseButton;
MediaControlButton SkipForwardButton;
MediaControlButton ShowController;
Grid D_PAD_GRID;
Button Up_Btn;
Button Left_Btn;
Button A_Btn;
Button Right_Btn;
Button Down_Btn;
Button Y_Btn;
Button X_Btn;
Button B_Btn;
    

    public MediaBar()
    {
      this.InitializeComponent();
      ((FrameworkElement) this).DataContext = (object) App.GlobalData;
      ((UIElement) this.LayoutRoot).CaptureMouse();
      ((Control) this.PlayPauseButton).ApplyTemplate();
      VisualStateManager.GoToState((Control) this.PlayPauseButton, "Play", false);
      ((FrameworkElement) this.fastForwardButton).Name = "FastForwardButton";
      ((FrameworkElement) this.rewindButton).Name = "RewindButton";
      BindingOperations.SetBinding((DependencyObject) this, MediaBar.IsExpandedProperty, (BindingBase) new Binding("IsMediaBarExpanded")
      {
        Mode = (BindingMode) 1,
        Source = (object) App.GlobalData
      });
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.MediaBar_Loaded);
      ((FrameworkElement) this).Unloaded += new RoutedEventHandler(this.MediaBar_Unloaded);
      this.pressAndHoldTimer = new Timer(new TimerCallback(this.PressAndHoldEvent), (object) null, -1, -1);
    }

    public bool IsExpanded
    {
      get => (bool) ((DependencyObject) this).GetValue(MediaBar.IsExpandedProperty);
      set => ((DependencyObject) this).SetValue(MediaBar.IsExpandedProperty, (object) value);
    }

    public void RefreshState() => this.UpdateVisualStatus();

    public void PressAndHoldEvent(object stateInfo)
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        if (this.skipButtonPressed != null)
        {
          this.hasPressAndHoldTimerFired = true;
          if (((FrameworkElement) this.skipButtonPressed).Name.Equals("SkipForwardButton"))
            this.MediaButton_Click((object) this.fastForwardButton, (RoutedEventArgs) null);
          else
            this.MediaButton_Click((object) this.rewindButton, (RoutedEventArgs) null);
        }
        else
          this.pressAndHoldTimer.Change(-1, -1);
      }, (object) this);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this.pressAndHoldTimer == null)
        return;
      this.pressAndHoldTimer.Dispose();
      this.pressAndHoldTimer = (Timer) null;
    }

    private void MediaBar_Loaded(object sender, RoutedEventArgs e)
    {
      App.GlobalData.EventMediaStateChanged += new EventHandler<LRCAsyncCompletedEventArgs>(this.GlobalData_EventMediaStateChanged);
      this.UpdateVisualStatus();
    }

    private void MediaBar_Unloaded(object sender, RoutedEventArgs e)
    {
      App.GlobalData.EventMediaStateChanged -= new EventHandler<LRCAsyncCompletedEventArgs>(this.GlobalData_EventMediaStateChanged);
    }

    private void GlobalData_EventMediaStateChanged(object sender, LRCAsyncCompletedEventArgs e)
    {
      this.UpdateVisualStatus();
    }

    private void UpdateVisualStatus()
    {
      if (NowPlayingItemViewModel.NowPlayingMediaState == NowPlayingMediaState.Playing)
      {
        VisualStateManager.GoToState((Control) this.PlayPauseButton, "Pause", false);
        App.GlobalData.CollapseMediaBarIfNecessary();
      }
      else
        VisualStateManager.GoToState((Control) this.PlayPauseButton, "Play", false);
      if (App.GlobalData.NowPlayingItem == null)
        return;
      ((RangeBase) this.MediaProgressBar).Value = (double) App.GlobalData.NowPlayingItem.Position;
      ((RangeBase) this.MediaProgressBar).Maximum = (double) App.GlobalData.NowPlayingItem.Duration;
    }

    private void SendCommandCompleted(object sender, LRCAsyncCompletedEventArgs e)
    {
      App.GlobalData.NowPlayingItem.SendCommandCompletedEvent -= new EventHandler<LRCAsyncCompletedEventArgs>(this.SendCommandCompleted);
      if (this.clickedButton != null && e.Error == null)
      {
        switch (((FrameworkElement) this.clickedButton).Name)
        {
          case "PlayPauseButton":
            if (this.mediaStateBeforeButtonTap == NowPlayingMediaState.Playing)
            {
              VisualStateManager.GoToState((Control) this.PlayPauseButton, "Play", false);
              break;
            }
            VisualStateManager.GoToState((Control) this.PlayPauseButton, "Pause", false);
            break;
          case "RewindButton":
          case "FastForwardButton":
            VisualStateManager.GoToState((Control) this.PlayPauseButton, "Play", false);
            VisualStateManager.GoToState((Control) this.clickedButton, "Ready", false);
            break;
          default:
            VisualStateManager.GoToState((Control) this.clickedButton, "Ready", false);
            break;
        }
      }
      this.clickedButton = (Button) null;
    }

    private void MediaButton_Click(object sender, RoutedEventArgs e)
    {
      if (this.clickedButton != null || App.GlobalData.CurrentMediaState == null)
        return;
      this.mediaStateBeforeButtonTap = NowPlayingItemViewModel.NowPlayingMediaState;
      Button button = sender as Button;
      this.clickedButton = button;
      App.GlobalData.NowPlayingItem.SendCommandCompletedEvent += new EventHandler<LRCAsyncCompletedEventArgs>(this.SendCommandCompleted);
      switch (((FrameworkElement) button).Name)
      {
        case "SkipBackButton":
          VisualStateManager.GoToState((Control) button, "Waiting", false);
          App.GlobalData.NowPlayingItem.SendCommand(ControlKey.Replay);
          break;
        case "RewindButton":
          VisualStateManager.GoToState((Control) button, "Waiting", false);
          App.GlobalData.NowPlayingItem.SendCommand(ControlKey.Rewind);
          break;
        case "PlayPauseButton":
          if (this.mediaStateBeforeButtonTap == NowPlayingMediaState.Playing)
          {
            VisualStateManager.GoToState((Control) button, "PauseTapped", false);
            App.GlobalData.NowPlayingItem.SendCommand(ControlKey.Pause);
            break;
          }
          VisualStateManager.GoToState((Control) button, "PlayTapped", false);
          App.GlobalData.NowPlayingItem.SendCommand(ControlKey.Play);
          break;
        case "FastForwardButton":
          VisualStateManager.GoToState((Control) button, "Waiting", false);
          App.GlobalData.NowPlayingItem.SendCommand(ControlKey.FastForward);
          break;
        case "SkipForwardButton":
          VisualStateManager.GoToState((Control) button, "Waiting", false);
          App.GlobalData.NowPlayingItem.SendCommand(ControlKey.Skip);
          break;
      }
      OmnitureAppMeasurement.Instance.TrackPlayControlClickEvent(((FrameworkElement) button).Name, "play control command used");
    }

    private void SkipButton_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
    {
      this.skipButtonPressed = sender as Button;
      this.hasPressAndHoldTimerFired = false;
      this.pressAndHoldTimer.Change(500, -1);
    }

    private void SkipButton_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      this.pressAndHoldTimer.Change(-1, -1);
      if (this.skipButtonPressed == null)
        return;
      if (!this.hasPressAndHoldTimerFired)
        this.MediaButton_Click((object) this.skipButtonPressed, (RoutedEventArgs) null);
      this.skipButtonPressed = (Button) null;
    }

    private void SkipButton_MouseLeave(object sender, MouseEventArgs e)
    {
      this.SkipButton_ManipulationCompleted(sender, (ManipulationCompletedEventArgs) null);
    }

    private void SkipButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      this.SkipButton_ManipulationCompleted(sender, (ManipulationCompletedEventArgs) null);
    }

    private void ExpandButton_Click(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) this).DataContext is MainViewModel dataContext))
        return;
      dataContext.FlipMediaBarStatus();
    }

    private void ChangeVisualState()
    {
      if (!this.IsExpanded)
        VisualStateManager.GoToState((Control) this, "Collapsed", true);
      else
        VisualStateManager.GoToState((Control) this, "Expanded", true);
      this.UpdateVisualStatus();
    }

    private void NowPlayingLink_Click(object sender, RoutedEventArgs e)
    {
      LrcPage lrcPageParent = LrcPage.GetLrcPageParent((FrameworkElement) this);
      if (lrcPageParent == null)
        return;
      NavHelper.GotoMainpage(lrcPageParent, App.RootFrameState.GoHomeLoading);
    }

    private void Left_Click(object sender, RoutedEventArgs e)
    {
      this.vibrateController.Start(new TimeSpan(0, 0, 0, 0, 15));
      MainViewModel.Instance.CurrentSession.BeginSendControlCommand(ControlKey.Left, (AsyncCallback) null, (object) null);
      OmnitureAppMeasurement.Instance.TrackPlayControlClickEvent("Dpad:Left", "play control command used");
    }

    private void Up_Click(object sender, RoutedEventArgs e)
    {
      this.vibrateController.Start(new TimeSpan(0, 0, 0, 0, 15));
      MainViewModel.Instance.CurrentSession.BeginSendControlCommand(ControlKey.Up, (AsyncCallback) null, (object) null);
      OmnitureAppMeasurement.Instance.TrackPlayControlClickEvent("Dpad:Up", "play control command used");
    }

    private void Down_Click(object sender, RoutedEventArgs e)
    {
      this.vibrateController.Start(new TimeSpan(0, 0, 0, 0, 15));
      MainViewModel.Instance.CurrentSession.BeginSendControlCommand(ControlKey.Down, (AsyncCallback) null, (object) null);
      OmnitureAppMeasurement.Instance.TrackPlayControlClickEvent("Dpad:Down", "play control command used");
    }

    private void Right_Click(object sender, RoutedEventArgs e)
    {
      this.vibrateController.Start(new TimeSpan(0, 0, 0, 0, 15));
      MainViewModel.Instance.CurrentSession.BeginSendControlCommand(ControlKey.Right, (AsyncCallback) null, (object) null);
      OmnitureAppMeasurement.Instance.TrackPlayControlClickEvent("Dpad:Right", "play control command used");
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
      this.vibrateController.Start(new TimeSpan(0, 0, 0, 0, 15));
      if (MainViewModel.Instance.IsRunningMediaCenter)
        MainViewModel.Instance.CurrentSession.BeginSendControlCommand(ControlKey.Ok, (AsyncCallback) null, (object) null);
      else
        MainViewModel.Instance.CurrentSession.BeginSendControlCommand(ControlKey.PadA, (AsyncCallback) null, (object) null);
      OmnitureAppMeasurement.Instance.TrackPlayControlClickEvent("Dpad:A", "play control command used");
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
      this.vibrateController.Start(new TimeSpan(0, 0, 0, 0, 15));
      if (MainViewModel.Instance.IsRunningMediaCenter)
        MainViewModel.Instance.CurrentSession.BeginSendControlCommand(ControlKey.Back, (AsyncCallback) null, (object) null);
      else
        MainViewModel.Instance.CurrentSession.BeginSendControlCommand(ControlKey.PadB, (AsyncCallback) null, (object) null);
      OmnitureAppMeasurement.Instance.TrackPlayControlClickEvent("Dpad:B", "play control command used");
    }

    private void X_Click(object sender, RoutedEventArgs e)
    {
      this.vibrateController.Start(new TimeSpan(0, 0, 0, 0, 15));
      if (MainViewModel.Instance.IsRunningMediaCenter)
        MainViewModel.Instance.CurrentSession.BeginSendControlCommand(ControlKey.Info, (AsyncCallback) null, (object) null);
      else
        MainViewModel.Instance.CurrentSession.BeginSendControlCommand(ControlKey.PadX, (AsyncCallback) null, (object) null);
      OmnitureAppMeasurement.Instance.TrackPlayControlClickEvent("Dpad:X", "play control command used");
    }

    private void Y_Click(object sender, RoutedEventArgs e)
    {
      this.vibrateController.Start(new TimeSpan(0, 0, 0, 0, 15));
      MainViewModel.Instance.CurrentSession.BeginSendControlCommand(ControlKey.PadY, (AsyncCallback) null, (object) null);
      OmnitureAppMeasurement.Instance.TrackPlayControlClickEvent("Dpad:Y", "play control command used");
    }

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/MediaBar.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.CustomStates = (VisualStateGroup) ((FrameworkElement) this).FindName("CustomStates");
      this.Collapsed = (VisualState) ((FrameworkElement) this).FindName("Collapsed");
      this.Expanded = (VisualState) ((FrameworkElement) this).FindName("Expanded");
      this.MediaBarContainer = (Grid) ((FrameworkElement) this).FindName("MediaBarContainer");
      this.MediaProgressBar = (ProgressBar) ((FrameworkElement) this).FindName("MediaProgressBar");
      this.PositionAndDuration = (Grid) ((FrameworkElement) this).FindName("PositionAndDuration");
      this.PositionText = (TextBlock) ((FrameworkElement) this).FindName("PositionText");
      this.DurationText = (TextBlock) ((FrameworkElement) this).FindName("DurationText");
      this.MediaControls = (Grid) ((FrameworkElement) this).FindName("MediaControls");
      this.NowPlayingLink = (MediaControlButton) ((FrameworkElement) this).FindName("NowPlayingLink");
      this.SkipBackButton = (MediaControlButton) ((FrameworkElement) this).FindName("SkipBackButton");
      this.PlayPauseButton = (Button) ((FrameworkElement) this).FindName("PlayPauseButton");
      this.SkipForwardButton = (MediaControlButton) ((FrameworkElement) this).FindName("SkipForwardButton");
      this.ShowController = (MediaControlButton) ((FrameworkElement) this).FindName("ShowController");
      this.D_PAD_GRID = (Grid) ((FrameworkElement) this).FindName("D_PAD_GRID");
      this.Up_Btn = (Button) ((FrameworkElement) this).FindName("Up_Btn");
      this.Left_Btn = (Button) ((FrameworkElement) this).FindName("Left_Btn");
      this.A_Btn = (Button) ((FrameworkElement) this).FindName("A_Btn");
      this.Right_Btn = (Button) ((FrameworkElement) this).FindName("Right_Btn");
      this.Down_Btn = (Button) ((FrameworkElement) this).FindName("Down_Btn");
      this.Y_Btn = (Button) ((FrameworkElement) this).FindName("Y_Btn");
      this.X_Btn = (Button) ((FrameworkElement) this).FindName("X_Btn");
      this.B_Btn = (Button) ((FrameworkElement) this).FindName("B_Btn");
    }

    private static class VisualStates
    {
      public const string GroupCommon = "CommonStates";
      public const string GroupCustom = "CustomStates";
      public const string GroupFocus = "FocusStates";
      public const string StateMouseOver = "MouseOver";
      public const string StateDisabled = "Disabled";
      public const string StateNormal = "Normal";
      public const string StatePressed = "Pressed";
      public const string StateReady = "Ready";
      public const string StateWaiting = "Waiting";
      public const string StateFocused = "Focused";
      public const string StateUnfocused = "Unfocused";
    }
  }
}
