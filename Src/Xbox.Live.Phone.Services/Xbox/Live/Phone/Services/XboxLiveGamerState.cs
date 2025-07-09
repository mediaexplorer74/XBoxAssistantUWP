// *********************************************************
// Type: Xbox.Live.Phone.Services.XboxLiveGamerState
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services
{
  public class XboxLiveGamerState
  {
    private const string ComponentName = "XboxLiveGamerState";
    private const int SignInTimeOutMilliSeconds = 40000;
    private Timer signInTimer;
    private XboxLiveGamerState.GamerStates gamerState;

    public XboxLiveGamerState() => this.gamerState = XboxLiveGamerState.GamerStates.SignedOut;

    public event EventHandler<ServiceProxyEventArgs<XboxLiveGamerState.GamerStates>> EventGamerStateUpdate;

    public XboxLiveGamerState.GamerStates GamerState
    {
      get => this.gamerState;
      private set
      {
        this.gamerState = value;
        if (this.gamerState == XboxLiveGamerState.GamerStates.SigningIn)
        {
          if (this.signInTimer == null)
            this.signInTimer = new Timer(new TimerCallback(this.SignInTimerCallback), (object) null, 40000, 40000);
        }
        else if (this.signInTimer != null)
        {
          this.signInTimer.Dispose();
          this.signInTimer = (Timer) null;
        }
        if (this.EventGamerStateUpdate == null)
          return;
        this.EventGamerStateUpdate((object) null, new ServiceProxyEventArgs<XboxLiveGamerState.GamerStates>((object) value, (Exception) null, false, (object) null));
      }
    }

    [DefaultValue(false)]
    public bool IsOfflineMessageBoxDisplayed { get; set; }

    public bool IsOnline => this.GamerState == XboxLiveGamerState.GamerStates.SignedIn;

    public void UpdateState(XboxLiveGamerState.GamerStateEvent stateEvent)
    {
      if (!ServiceManagerFactory.IsRunningEmulator && (!NetworkInterface.GetIsNetworkAvailable() || stateEvent == XboxLiveGamerState.GamerStateEvent.NoInternetConnection))
      {
        if (this.GamerState != XboxLiveGamerState.GamerStates.Offline)
          this.IsOfflineMessageBoxDisplayed = false;
        this.GamerState = XboxLiveGamerState.GamerStates.Offline;
      }
      else if (this.GamerState == XboxLiveGamerState.GamerStates.SignedOut)
      {
        if (stateEvent != XboxLiveGamerState.GamerStateEvent.SignInStart)
          return;
        this.GamerState = XboxLiveGamerState.GamerStates.SigningIn;
      }
      else if (this.GamerState == XboxLiveGamerState.GamerStates.SigningIn)
      {
        if (stateEvent == XboxLiveGamerState.GamerStateEvent.SignInSucceed)
        {
          this.GamerState = XboxLiveGamerState.GamerStates.SignedIn;
        }
        else
        {
          if (stateEvent != XboxLiveGamerState.GamerStateEvent.SignInTimeout)
            return;
          this.GamerState = XboxLiveGamerState.GamerStates.Offline;
        }
      }
      else if (this.GamerState == XboxLiveGamerState.GamerStates.SignedIn)
      {
        if (stateEvent != XboxLiveGamerState.GamerStateEvent.SignedOut)
          return;
        this.GamerState = XboxLiveGamerState.GamerStates.SignedOut;
      }
      else
      {
        if (this.GamerState != XboxLiveGamerState.GamerStates.Offline)
          return;
        if (stateEvent == XboxLiveGamerState.GamerStateEvent.SignInStart)
        {
          this.GamerState = XboxLiveGamerState.GamerStates.SigningIn;
        }
        else
        {
          if (stateEvent != XboxLiveGamerState.GamerStateEvent.ConnectionAvailable)
            return;
          this.GamerState = XboxLiveGamerState.GamerStates.SignedOut;
        }
      }
    }

    private void SignInTimerCallback(object state)
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.UpdateState(XboxLiveGamerState.GamerStateEvent.SignInTimeout);
      }, (object) null);
    }

    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Makes for cleaner usage, since it only relates to XboxLiveGamerState.")]
    public enum GamerStates
    {
      SignedOut,
      SigningIn,
      SignedIn,
      Offline,
    }

    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Makes for cleaner usage, since it only relates to XboxLiveGamerState.")]
    public enum GamerStateEvent
    {
      SignInStart,
      SignInSucceed,
      SignInTimeout,
      NoInternetConnection,
      SignedOut,
      ConnectionAvailable,
    }
  }
}
