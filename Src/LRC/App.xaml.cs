// *********************************************************
// Type: LRC.App
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using Delay;
using LRC.Resources;
using LRC.Service.Omniture;
using LRC.ViewModel;
//using Microsoft.Phone.Controls;
//using Microsoft.Phone.Shell;
//using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Markup;
//using System.Windows.Media.Animation;
//using System.Windows.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Markup;
using Xbox.Live.Phone;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Utils;
using Xbox.Live.Phone.Utils.Cache;
using Xbox.Live.Phone.Utils.Instrumentation;
using XLToolKit;


namespace LRC
{
  public partial class App : Application
  {
    private const string ComponentName = "App";
    private const string ViewFilePath = "PhoneState_View.Xml";
    private const string UnhandledExceptionLogFileName = "Unhandled.log";
    private static bool updateRequired;
    private static bool marketplaceLaunched;
    private static BusyIndicator globalBusyIndicator;
    private BusyIndicator connectingBusyIndicator;
    private bool phoneApplicationInitialized;
    private Storyboard connectedStoryboard;
    

    public App()
    {
      this.UnhandledException += new EventHandler<ApplicationUnhandledExceptionEventArgs>(this.Application_UnhandledException);
      if (Debugger.IsAttached)
        Application.Current.Host.Settings.EnableFrameRateCounter = true;
      this.InitializeComponent();
      this.InitializePhoneApplication();
      LrcNavigation.Instance.Initialize(this.RootFrame);
      ThreadManager.UIThread = new ThreadManager.ThreadData(SynchronizationContext.Current, Thread.CurrentThread.ManagedThreadId);
      LowProfileImageLoader.FailedUriSource = new Uri("/UI/Images/DefaultBoxArt/Unknown.png", UriKind.RelativeOrAbsolute);
    }

    public static App CurrentApp => (App) Application.Current;

    public static MainViewModel GlobalData => MainViewModel.Instance;

    public PhoneApplicationFrame RootFrame { get; private set; }

    public SwitchPanel SwitchPanel { get; private set; }

    public static void AppendStringToLogFile(string fileName, string details)
    {
      FileHelper fileHelper = new FileHelper();
      details = "\r\n\r\n" + (object) DateTime.Now.ToLocalTime() + details;
      fileHelper.AppendFile(fileName, details);
    }

    public static void ToggleConnectingBusyIndicator(bool enabled)
    {
      if (App.CurrentApp.connectingBusyIndicator == null)
        return;
      App.CurrentApp.connectingBusyIndicator.IsBusy = enabled;
    }

    public static void ResetRootFrameView()
    {
      if (App.CurrentApp.SwitchPanel == null || App.CurrentApp.RootFrame == null || App.CurrentApp.SwitchPanel.CurrentState == 2)
        return;
      App.CurrentApp.SwitchPanel.CurrentState = 0;
      ((FrameworkElement) App.CurrentApp.RootFrame).DataContext = (object) null;
      App.SetGlobalBusyOverlay(false);
      App.ToggleConnectingBusyIndicator(false);
    }

    public static void ShowRootFrameErrorView()
    {
      if (App.CurrentApp.SwitchPanel == null || App.CurrentApp.RootFrame == null || App.CurrentApp.connectedStoryboard == null)
        return;
      App.SetGlobalBusyOverlay(false);
      App.CurrentApp.connectedStoryboard.Stop();
      App.ToggleConnectingBusyIndicator(false);
      App.CurrentApp.SwitchPanel.CurrentState = 3;
      ((FrameworkElement) App.CurrentApp.RootFrame).DataContext = (object) App.GlobalData;
    }

    public static void ShowtRootFrameProgressBarView()
    {
      if (App.CurrentApp.SwitchPanel == null || App.CurrentApp.RootFrame == null || App.CurrentApp.connectedStoryboard == null)
        return;
      App.SetGlobalBusyOverlay(true);
      App.CurrentApp.connectedStoryboard.Stop();
      App.ToggleConnectingBusyIndicator(false);
      App.CurrentApp.SwitchPanel.CurrentState = 1;
      ((FrameworkElement) App.CurrentApp.RootFrame).DataContext = (object) null;
    }

    public static void ShowConnectedView()
    {
      if (App.CurrentApp.SwitchPanel == null || App.CurrentApp.RootFrame == null || App.CurrentApp.connectedStoryboard == null)
        return;
      App.SetGlobalBusyOverlay(false);
      App.CurrentApp.SwitchPanel.CurrentState = 2;
      ((FrameworkElement) App.CurrentApp.RootFrame).DataContext = (object) App.GlobalData;
      App.ToggleConnectingBusyIndicator(true);
      App.CurrentApp.connectedStoryboard.Begin();
    }

    public static void SetGlobalBusyOverlay(bool enabled)
    {
      if (App.globalBusyIndicator == null)
        return;
      App.globalBusyIndicator.IsBusy = enabled;
    }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "These are debug strings.")]
    private static DataType LoadDataFromStorage<DataType>(string path)
    {
      DataType dataType = default (DataType);
      if (!string.IsNullOrEmpty(path))
        dataType = CacheManager.Load<DataType>(path);
      return dataType;
    }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "These are debug strings.")]
    private static void SaveDataToStorage(string path, object data)
    {
      if (data == null)
        return;
      CacheManager.Save(path, data);
    }

    private static void InitializeGamerServices()
    {
      if (!ServiceManagerFactory.IsRunningEmulator)
      {
        try
        {
          if (!GamerServicesDispatcher.IsInitialized)
            GamerServicesDispatcher.Initialize((IServiceProvider) null);
        }
        catch (InvalidOperationException ex)
        {
        }
      }
      BackgroundWorker backgroundWorker = new BackgroundWorker();
      backgroundWorker.DoWork += new DoWorkEventHandler(App.UpdateXnaDispatchers);
      backgroundWorker.RunWorkerAsync();
    }

    private static void UpdateXnaDispatchers(object sender, DoWorkEventArgs e)
    {
      while (true)
      {
        try
        {
          do
          {
            do
            {
              Thread.Sleep(33);
            }
            while (ServiceManagerFactory.IsRunningEmulator);
            FrameworkDispatcher.Update();
          }
          while (!GamerServicesDispatcher.IsInitialized);
          GamerServicesDispatcher.Update();
        }
        catch (GameUpdateRequiredException ex)
        {
          if (!App.updateRequired)
          {
            App.updateRequired = true;
            ErrorManager.Instance.Nonfatal(Resource.TitleUpdate_Title, Resource.TitleUpdate_Text, (Action) (() => App.LaunchMarketplace()));
          }
        }
      }
    }

    private static void LaunchMarketplace()
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        try
        {
          if (App.marketplaceLaunched || ServiceManagerFactory.IsRunningEmulator)
            return;
          new MarketplaceDetailTask()
          {
            ContentType = ((MarketplaceContentType) 1)
          }.Show();
          App.marketplaceLaunched = true;
        }
        catch (InvalidOperationException ex)
        {
          if (App.marketplaceLaunched)
            return;
          App.updateRequired = false;
        }
      }, (object) null);
    }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "These are debug strings.")]
    private static void SaveState()
    {
      LrcNavigation.Instance.SaveState("PhoneState_View.Xml");
      MainViewModel.Instance.SaveRecentsData();
      OmnitureAppMeasurement.Instance.SavePersistentData();
    }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "These are debug strings.")]
    private static void RestoreState()
    {
      LrcNavigation.RestoreStaticInstanceState("PhoneState_View.Xml");
    }

    private void Application_Launching(object sender, LaunchingEventArgs e)
    {
      App.InitializeGamerServices();
      OmnitureAppMeasurement.Instance.TrackAppStartEvent("lrc started", "xbox companion", "launch");
    }

    private void Application_Activated(object sender, ActivatedEventArgs e)
    {
      App.updateRequired = false;
      App.marketplaceLaunched = false;
      string launchType;
      if (!e.IsApplicationInstancePreserved)
      {
        App.InitializeGamerServices();
        App.RestoreState();
        App.GlobalData.ShouldLookForXbox = true;
        LrcNavigation.Instance.AdjustStackComingBackFromTombstoning();
        launchType = "rehydrate";
      }
      else
      {
        App.GlobalData.OnAppActivated();
        launchType = "fast app switching";
      }
      OmnitureAppMeasurement.Instance.TrackAppStartEvent("lrc started", "xbox companion", launchType);
    }

    private void Application_Deactivated(object sender, DeactivatedEventArgs e)
    {
      App.SaveState();
      App.GlobalData.OnAppDeactivated();
    }

    private void Application_Closing(object sender, ClosingEventArgs e)
    {
      MainViewModel.Instance.SaveRecentsData();
      OmnitureAppMeasurement.Instance.SavePersistentData();
    }

    private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
      if (!Debugger.IsAttached)
        return;
      Debugger.Break();
    }

    private void Application_UnhandledException(
      object sender,
      ApplicationUnhandledExceptionEventArgs e)
    {
      if (Debugger.IsAttached)
        Debugger.Break();
      App.AppendStringToLogFile("Unhandled.log", "\r\nUnhandled Exception caught:  " + e.ExceptionObject.ToString() + "\r\n\r\n");
      LiveMobileTrace.FlushBuffer();
      OmnitureAppMeasurement.Instance.TrackUnhandledExceptionEvent(e.ExceptionObject.ToString(), "lrc unhandled exception happened");
    }

    private void InitializePhoneApplication()
    {
      if (this.phoneApplicationInitialized)
        return;
      PhoneApplicationFrame applicationFrame = new PhoneApplicationFrame();
      ((FrameworkElement) applicationFrame).Style = (Style) this.Resources[(object) "mainFrameStyle"];
      this.RootFrame = applicationFrame;
      ((Frame) this.RootFrame).UriMapper = (UriMapperBase) new UriMapper();
      ((Frame) this.RootFrame).Navigated += new NavigatedEventHandler(this.CompleteInitializePhoneApplication);
      ((Frame) this.RootFrame).NavigationFailed += new NavigationFailedEventHandler(this.RootFrame_NavigationFailed);
      this.phoneApplicationInitialized = true;
    }

    private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
    {
      if (this.RootVisual != this.RootFrame)
        this.RootVisual = (UIElement) this.RootFrame;
      ((Frame) this.RootFrame).Navigated -= new NavigatedEventHandler(this.CompleteInitializePhoneApplication);
    }

    private void GlobalBusyIndicator_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is BusyIndicator busyIndicator))
        return;
      App.globalBusyIndicator = busyIndicator;
    }

    private void ConnectingBusyIndicator_Loaded(object sender, RoutedEventArgs e)
    {
      this.connectingBusyIndicator = sender as BusyIndicator;
      BusyIndicator connectingBusyIndicator = this.connectingBusyIndicator;
    }

    private void SwitchPanel_Loaded(object sender, RoutedEventArgs e)
    {
      this.SwitchPanel = sender as SwitchPanel;
      if (!string.Equals(Thread.CurrentThread.CurrentUICulture.Name, "ja", StringComparison.OrdinalIgnoreCase) && !string.Equals(Thread.CurrentThread.CurrentUICulture.Name, "ja-jp", StringComparison.OrdinalIgnoreCase))
        return;
      ((FrameworkElement) this.SwitchPanel).Language = XmlLanguage.GetLanguage("ja-jp");
    }

    private void ConnectedGrid_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is Grid grid))
        return;
      this.connectedStoryboard = (Storyboard) ((FrameworkElement) grid).Resources[(object) "ConnectedStoryboard"];
      ((Timeline) this.connectedStoryboard).Completed -= new EventHandler(this.ConnectedStoryboard_Completed);
      ((Timeline) this.connectedStoryboard).Completed += new EventHandler(this.ConnectedStoryboard_Completed);
    }

    private void ConnectedStoryboard_Completed(object sender, EventArgs e)
    {
      App.CurrentApp.SwitchPanel.CurrentState = 0;
      ((FrameworkElement) App.CurrentApp.RootFrame).DataContext = (object) null;
      App.SetGlobalBusyOverlay(false);
      App.ToggleConnectingBusyIndicator(false);
    }

    
    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Part of the RootFrame state")]
    public enum RootFrameState
    {
      PageContent,
      GoHomeLoading,
      ConnectedLoading,
      ConnectError,
    }
  }
}

/*
 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace LRC
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}

 */
