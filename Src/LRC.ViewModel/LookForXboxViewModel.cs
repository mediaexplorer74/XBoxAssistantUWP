// *********************************************************
// Type: LRC.ViewModel.LookForXboxViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  public class LookForXboxViewModel : ViewModelBase, IDisposable
  {
    private const string ComponentName = "LookForXboxViewModel";
    private const int SleepWaitTime = 1000;
    private string loadingText;
    private ManualResetEvent gamerSignInEvent = new ManualResetEvent(false);
    private XboxLiveGamerState.GamerStates lastSeenGamerState = XboxLiveGamerState.GamerStates.SigningIn;
    private int connectErrorCode;

    public event EventHandler<LRCAsyncCompletedEventArgs> EventLookForXboxCompleted;

    public bool IsPairing { get; set; }

    public string LoadingText
    {
      get => this.loadingText;
      set => this.SetPropertyValue<string>(ref this.loadingText, value, nameof (LoadingText));
    }

    public int ErrorCode
    {
      get => this.connectErrorCode;
      private set
      {
        this.SetPropertyValue<int>(ref this.connectErrorCode, value, nameof (ErrorCode));
        switch ((ErrorCodeEnum) this.ErrorCode)
        {
          case ErrorCodeEnum.FailedToSignIn:
          case ErrorCodeEnum.FailedCompanionSignIn:
            this.ErrorHeaderString = Resource.LookForXboxPage_ErrorHeader_CanNotSignIn;
            this.ErrorString = Resource.LookForXboxPage_Error_CanNotSignIn;
            break;
          case ErrorCodeEnum.FailedToConnectToConsole:
          case ErrorCodeEnum.FailedToConnectToSession:
          case ErrorCodeEnum.NoIPAddress:
          case ErrorCodeEnum.WrongIPAddressFormat:
          case ErrorCodeEnum.NoConsoleJoinEvent:
          case ErrorCodeEnum.FailedToJoinSession:
            this.ErrorHeaderString = Resource.LookForXboxPage_ErrorHeader_CanNotConnect;
            this.ErrorString = Resource.LookForXboxPage_Error_CanNotConnect;
            break;
          case ErrorCodeEnum.TooManyClients:
            this.ErrorHeaderString = Resource.LookForXboxPage_ErrorHeader_DeviceLimitReached;
            this.ErrorString = Resource.LookForXboxPage_Error_TooManyClients;
            break;
          default:
            this.ErrorHeaderString = Resource.LookForXboxPage_ErrorHeader_CanNotFind;
            this.ErrorString = Resource.LookForXboxPage_Error_CanNotFind;
            break;
        }
        this.NotifyPropertyChanged("ErrorHeaderString");
        this.NotifyPropertyChanged("ErrorString");
        MainViewModel.Instance.ErrorCode = this.ErrorCode;
        MainViewModel.Instance.ErrorHeaderString = this.ErrorHeaderString;
        MainViewModel.Instance.ErrorString = this.ErrorString;
      }
    }

    public string ErrorHeaderString { get; private set; }

    public string ErrorString { get; private set; }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "This is a debug string")]
    public void Start()
    {
      ErrorCodeEnum errorCode = ErrorCodeEnum.None;
      ThreadManager.UIThread.AssertIsCurrentThread();
      if (this.IsPairing)
        return;
      this.IsPairing = true;
      if (XboxLiveGamer.GamerState.GamerState != XboxLiveGamerState.GamerStates.SignedIn)
      {
        this.LoadingText = Resource.SigningInMessage;
        XboxLiveGamer.EventGamerStateUpdate += new EventHandler<ServiceProxyEventArgs<XboxLiveGamerState.GamerStates>>(this.OnGamerStateUpdated);
        XboxLiveGamer.InitializeCurrentGamer((ManualResetEvent) null);
      }
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        try
        {
          if (XboxLiveGamer.GamerState.GamerState != XboxLiveGamerState.GamerStates.SignedIn)
            this.gamerSignInEvent.WaitOne();
          if (XboxLiveGamer.GamerState.GamerState == XboxLiveGamerState.GamerStates.SignedIn)
          {
            ThreadManager.UIThreadPost((SendOrPostCallback) delegate
            {
              MainViewModel.Instance.WelcomeHeader = XboxLiveGamer.CurrentGamerTag;
              this.LoadingText = Resource.ConnectingMessage;
              MainViewModel.Instance.Load();
            }, (object) null);
            errorCode = MainViewModel.Instance.InitializeSession();
          }
          else
            errorCode = ErrorCodeEnum.FailedToSignIn;
        }
        finally
        {
          if (errorCode != ErrorCodeEnum.None)
          {
            while (this.EventLookForXboxCompleted == null)
              Thread.Sleep(1000);
          }
          this.ErrorCode = (int) errorCode;
          this.NotifyAsyncOperationCompleted(this.EventLookForXboxCompleted, errorCode);
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            this.IsPairing = false;
          }, (object) null);
        }
      });
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected void OnGamerStateUpdated(
      object sender,
      ServiceProxyEventArgs<XboxLiveGamerState.GamerStates> e)
    {
      XboxLiveGamerState.GamerStates gamerStates = e != null ? e.Result : throw new ArgumentNullException(nameof (e));
      if (gamerStates == this.lastSeenGamerState)
        return;
      switch (gamerStates)
      {
        case XboxLiveGamerState.GamerStates.SignedIn:
        case XboxLiveGamerState.GamerStates.Offline:
          this.gamerSignInEvent.Set();
          XboxLiveGamer.EventGamerStateUpdate -= new EventHandler<ServiceProxyEventArgs<XboxLiveGamerState.GamerStates>>(this.OnGamerStateUpdated);
          break;
      }
      this.lastSeenGamerState = gamerStates;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this.gamerSignInEvent == null)
        return;
      this.gamerSignInEvent.Dispose();
      this.gamerSignInEvent = (ManualResetEvent) null;
    }
  }
}
