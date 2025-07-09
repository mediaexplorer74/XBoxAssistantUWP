// *********************************************************
// Type: LRC.LrcPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Resources;
using LRC.Service.Omniture;
using LRC.Session;
using LRC.ViewModel;
//using Microsoft.Phone.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
using Windows.UI.Xaml;

//using System.Windows.Controls;
//using System.Windows.Markup;
//using System.Windows.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Xbox.Live.Phone;
using Xbox.Live.Phone.Utils;


namespace LRC
{
  public class LrcPage : /*PhoneApplication*/Page
  {
    private bool connectionFailHandled;

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "These are debug strings.")]
    public LrcPage()
    {
      if (string.Equals(Thread.CurrentThread.CurrentUICulture.Name, "ja", StringComparison.OrdinalIgnoreCase) || string.Equals(Thread.CurrentThread.CurrentUICulture.Name, "ja-jp", StringComparison.OrdinalIgnoreCase))
        ((FrameworkElement) this).Language = XmlLanguage.GetLanguage("ja-jp");
      ((FrameworkElement) this).Loaded += (RoutedEventHandler) ((sender, args) => { });
      this.OmniturePageName = "wp:lrc:page";
      this.OmnitureChannelName = "wp:lrc:channel";
      this.OmnitureEntryPointName = App.GlobalData.OmnitureEntryPoint;
    }

    public static ErrorManager LrcPageErrorManager => ErrorManager.Instance;

    public object DataContext
    {
      get => ((FrameworkElement) this).DataContext;
      set
      {
        if (((FrameworkElement) this).DataContext is ViewModelBase dataContext)
        {
          dataContext.EventOnAsyncOperationError -= new EventHandler<LRCAsyncCompletedEventArgs>(this.OnError);
          dataContext.EventLaunchTitleCompleted -= new EventHandler<LRCAsyncCompletedEventArgs>(this.OnLaunchCompleted);
        }
        ((FrameworkElement) this).DataContext = value;
        if (!(value is ViewModelBase viewModelBase))
          return;
        viewModelBase.EventOnAsyncOperationError += new EventHandler<LRCAsyncCompletedEventArgs>(this.OnError);
        viewModelBase.EventLaunchTitleCompleted += new EventHandler<LRCAsyncCompletedEventArgs>(this.OnLaunchCompleted);
      }
    }

    public bool NeedToSaveViewModel { get; set; }

    public bool AllowMultipleInstances { get; set; }

    public bool Persistent { get; set; }

    public string OmniturePageName { get; set; }

    public string OmnitureChannelName { get; set; }

    public string OmnitureEntryPointName { get; set; }

    public static LrcPage GetLrcPageParent(FrameworkElement childElement)
    {
      if (childElement == null)
        throw new ArgumentNullException(nameof (childElement));
      lrcPageParent = (LrcPage) null;
      FrameworkElement parent = childElement.Parent as FrameworkElement;
      while (lrcPageParent == null && parent != null)
      {
        if (!(parent is LrcPage lrcPageParent))
          parent = parent.Parent as FrameworkElement;
      }
      return lrcPageParent;
    }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "These are debug strings.")]
    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e != null)
      {
        NavigationMode navigationMode = e.NavigationMode;
      }
      ((Page) this).OnNavigatedTo(e);
      if (e == null)
        return;
      if (this.DataContext == null && e.NavigationMode != null)
      {
        string modelKeyForRestore = LrcNavigation.Instance.GetViewModelKeyForRestore(this, this.AllowMultipleInstances);
        if (LrcNavigation.Instance.Locator.ContainsKey(modelKeyForRestore))
        {
          this.DataContext = (object) LrcNavigation.Instance.Locator.Locate(modelKeyForRestore);
          if (!this.Persistent)
            LrcNavigation.Instance.Locator.Remove(modelKeyForRestore);
        }
      }
      if (this.DataContext is ViewModelBase dataContext)
      {
        dataContext.EventOnAsyncOperationError += new EventHandler<LRCAsyncCompletedEventArgs>(this.OnError);
        dataContext.EventLaunchTitleCompleted += new EventHandler<LRCAsyncCompletedEventArgs>(this.OnLaunchCompleted);
      }
      this.connectionFailHandled = false;
      App.GlobalData.EventConnectionFailed += new EventHandler<ConnectionEventArgs>(this.OnConnectionFailed);
      if (App.GlobalData.ShouldLookForXbox)
      {
        App.GlobalData.ShouldLookForXbox = false;
        this.OnConnectionFailed((object) this, (ConnectionEventArgs) null);
      }
      FindChildrenUtil.FindFirstChildOfType<MediaBar>((DependencyObject) this)?.RefreshState();
    }

    protected virtual void OnNavigatedFrom(NavigationEventArgs e)
    {
      ((Page) this).OnNavigatedFrom(e);
      if (e == null)
        return;
      if (this.DataContext != null && e.NavigationMode == null && this.NeedToSaveViewModel)
        LrcNavigation.Instance.Locator.AddOrSet(LrcNavigation.Instance.GetViewModelKeyForSave(this, this.AllowMultipleInstances, true), (ViewModelBase) this.DataContext);
      if (this.DataContext is ViewModelBase dataContext)
      {
        dataContext.EventOnAsyncOperationError -= new EventHandler<LRCAsyncCompletedEventArgs>(this.OnError);
        dataContext.EventLaunchTitleCompleted -= new EventHandler<LRCAsyncCompletedEventArgs>(this.OnLaunchCompleted);
      }
      App.GlobalData.EventConnectionFailed -= new EventHandler<ConnectionEventArgs>(this.OnConnectionFailed);
      if (e.NavigationMode != 1)
        return;
      LrcNavigation.Instance.Locator.Remove(LrcNavigation.Instance.GetViewModelKeyForSave(this, this.AllowMultipleInstances, false));
    }

    protected virtual void OnError(object sender, LRCAsyncCompletedEventArgs eventArgs)
    {
    }

    protected virtual void OnLaunchCompleted(object sender, LRCAsyncCompletedEventArgs eventArgs)
    {
      if (eventArgs == null)
        return;
      if (!eventArgs.IsError)
      {
        if (eventArgs.UserState is LaunchUserState userState && userState.HideDpad)
          return;
        App.GlobalData.ExpandMediaBar();
      }
      else if (eventArgs.StatusCode == ErrorCodeEnum.WillTerminateCurrentTitle)
        new OkCancelDialog(Resource.Warning_Title, Resource.Launch_DifferentTitle_Warning_Message, Resource.OK_Text, Resource.Cancel_Text, new SendOrPostCallback(this.OnOkPressed), new SendOrPostCallback(this.OnCancelPressed)).Start();
      else
        this.OnError(sender, eventArgs);
    }

    protected virtual void OmnitureTrackPageVisit()
    {
      OmnitureAppMeasurement.Instance.TrackVisit(this.OmniturePageName, this.OmnitureChannelName);
    }

    protected virtual void OnBackKeyPress(CancelEventArgs e)
    {
      if (App.GlobalData.IsMediaBarExpanded)
      {
        App.GlobalData.FlipMediaBarStatus();
        if (e != null)
          e.Cancel = true;
      }
      base.OnBackKeyPress(e);
    }

    protected void OnConnectionFailed(object sender, ConnectionEventArgs e)
    {
      if (this.connectionFailHandled)
        return;
      this.connectionFailHandled = true;
      NavHelper.GotoLookForXboxPage((Page) this, true);
    }

    private void OnOkPressed(object state)
    {
      if (!(this.DataContext is ViewModelBase dataContext))
        return;
      dataContext.RetryLastLaunchCommand(state);
      OmnitureAppMeasurement.Instance.TrackEvent("event18", "launch on xbox exit current session event");
    }

    private void OnCancelPressed(object state)
    {
      OmnitureAppMeasurement.Instance.TrackEvent("event19", "launch on xbox cancel event");
    }
  }
}
