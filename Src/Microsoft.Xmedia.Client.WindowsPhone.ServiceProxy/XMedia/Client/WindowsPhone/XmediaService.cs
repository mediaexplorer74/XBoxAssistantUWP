// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.XmediaService
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using Microsoft.Phone.Info;
using Microsoft.XMedia;
using Microsoft.XMedia.Client;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public class XmediaService : IXmediaService
  {
    private const string AudienceUri = "http://xlink.xboxlive.com";
    private const int TokenRefreshRetryPeriod = 120000;
    private static readonly object singletonLock = new object();
    private static volatile XmediaService instance;
    private readonly object objectLock;
    private IClientTracer tracer;
    private XmediaService.SignInState signInState;
    private TokenManager tokenManager;
    private readonly XmediaServiceRestApi xmediaServiceRestApi;
    private System.Threading.Timer tokenRefreshTimer;
    private List<XmediaSession> sessions;

    public static XmediaService Instance
    {
      get
      {
        if (XmediaService.instance == null)
        {
          lock (XmediaService.singletonLock)
          {
            if (XmediaService.instance == null)
              XmediaService.instance = new XmediaService();
          }
        }
        return XmediaService.instance;
      }
    }

    private XmediaService()
    {
      this.objectLock = new object();
      this.tracer = (IClientTracer) new IsolatedStorageClientTracer();
      this.tracer.WriteInfo("XmediaService initializing");
      this.signInState = XmediaService.SignInState.NotStarted;
      this.xmediaServiceRestApi = new XmediaServiceRestApi(this.tracer);
      this.tokenManager = new TokenManager();
      this.tokenRefreshTimer = new System.Threading.Timer(new TimerCallback(this.RefreshCompanionToken));
      this.sessions = new List<XmediaSession>();
    }

    public void SetXboxLiveEnvironment(string envName)
    {
      this.xmediaServiceRestApi.SetXboxLiveEnvironment(envName);
    }

    public void SetXboxLiveEnvironmentUri(Uri environment)
    {
      this.xmediaServiceRestApi.SetXboxLiveEnvironmentUri(environment);
    }

    public IAsyncResult BeginCompanionLogin(AsyncCallback callback, object asyncState)
    {
      lock (this.objectLock)
        this.signInState = this.signInState != XmediaService.SignInState.InProgress ? XmediaService.SignInState.InProgress : throw XmediaService.CreateInvalidOperationExceptionForError(ErrorCode.CompanionNotSignedIn);
      bool flag = true;
      try
      {
        AsyncResultNoResult ar = new AsyncResultNoResult(callback, asyncState);
        if (this.tokenManager.HasPartnerToken)
        {
          this.ContinueCompanionLoginWithPartnerToken(ar);
          flag = false;
          return (IAsyncResult) ar;
        }
        this.tracer.WriteInfo("Getting partner token");
        Gamer.BeginGetPartnerToken("http://xlink.xboxlive.com", (AsyncCallback) (getPartnerTokenAsyncResult =>
        {
          try
          {
            string partnerToken = Gamer.EndGetPartnerToken(getPartnerTokenAsyncResult);
            if (string.IsNullOrWhiteSpace(partnerToken))
            {
              this.tracer.WriteError("Unable to retrieve partner token");
              throw new Exception("Unable to retrieve a partner token from Xbox Live");
            }
            this.tokenManager.PartnerToken = "<t>" + partnerToken + "</t>";
            this.tracer.WriteInfo("Got partner token: {0}", (object) this.tokenManager.PartnerToken);
            this.ContinueCompanionLoginWithPartnerToken(ar);
          }
          catch (Exception ex)
          {
            lock (this.objectLock)
              this.signInState = XmediaService.SignInState.NotStarted;
            ar.SetAsCompleted(ex, false);
          }
        }), (object) null);
        flag = false;
        return (IAsyncResult) ar;
      }
      finally
      {
        if (flag)
        {
          lock (this.objectLock)
            this.signInState = XmediaService.SignInState.NotStarted;
        }
      }
    }

    private void ContinueCompanionLoginWithPartnerToken(AsyncResultNoResult ar)
    {
      this.xmediaServiceRestApi.CompanionLogin(this.tokenManager, string.Join("", ((IEnumerable<byte>) (byte[]) DeviceExtendedProperties.GetValue("DeviceUniqueId")).Select<byte, string>((Func<byte, string>) (b => string.Format("{0:x2}", (object) b)))), (CompanionLoginDelegate) ((companionToken, tokenRefreshPeriod, userId, deviceId, e) =>
      {
        if (e == null)
        {
          this.tokenManager.CompanionToken = companionToken;
          this.tokenManager.UserId = userId;
          this.tokenManager.DeviceId = deviceId;
          this.ScheduleTokenRefresh(tokenRefreshPeriod);
          lock (this.objectLock)
            this.signInState = XmediaService.SignInState.SignedIn;
          ar.SetAsCompleted((Exception) null, false);
        }
        else
        {
          lock (this.objectLock)
            this.signInState = XmediaService.SignInState.NotStarted;
          ar.SetAsCompleted(e, false);
        }
      }));
    }

    public void EndCompanionLogin(IAsyncResult ar)
    {
      if (!(ar is AsyncResultNoResult asyncResultNoResult))
        throw new ArgumentException("Incorrect async result type", nameof (ar));
      asyncResultNoResult.EndInvoke();
    }

    public IAsyncResult BeginCompanionLogout(AsyncCallback callback, object asyncState)
    {
      lock (this.objectLock)
      {
        if (this.signInState == XmediaService.SignInState.InProgress)
          throw XmediaService.CreateInvalidOperationExceptionForError(ErrorCode.CompanionSigninInProgress);
        this.signInState = this.signInState != XmediaService.SignInState.NotStarted ? XmediaService.SignInState.NotStarted : throw XmediaService.CreateInvalidOperationExceptionForError(ErrorCode.CompanionNotSignedIn);
      }
      AsyncResultNoResult asyncResult = new AsyncResultNoResult(callback, asyncState);
      this.ScheduleTokenRefresh(0);
      this.xmediaServiceRestApi.CompanionLogout(this.tokenManager, (CompanionLogoutDelegate) (e =>
      {
        List<XmediaSession> sessions;
        lock (this.objectLock)
        {
          sessions = this.sessions;
          this.sessions = new List<XmediaSession>();
        }
        this.tracer.WriteInfo("Logout is closing {0} open sessions", (object) sessions.Count);
        Action onSessionsClosed = (Action) (() =>
        {
          this.tokenManager = new TokenManager();
          asyncResult.SetAsCompleted((Exception) null, false);
        });
        if (sessions.Count == 0)
        {
          onSessionsClosed();
        }
        else
        {
          int remainingSessions = sessions.Count;
          using (List<XmediaSession>.Enumerator enumerator = sessions.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              XmediaSession session = enumerator.Current;
              try
              {
                session.BeginLeaveSession((AsyncCallback) (ar =>
                {
                  try
                  {
                    session.EndLeaveSession(ar);
                  }
                  catch (Exception ex)
                  {
                  }
                  if (Interlocked.Decrement(ref remainingSessions) != 0)
                    return;
                  onSessionsClosed();
                }), (object) null);
              }
              catch (Exception ex)
              {
                if (Interlocked.Decrement(ref remainingSessions) == 0)
                  onSessionsClosed();
              }
            }
          }
        }
      }));
      return (IAsyncResult) asyncResult;
    }

    public void EndCompanionLogout(IAsyncResult ar)
    {
      if (!(ar is AsyncResultNoResult asyncResultNoResult))
        throw new ArgumentException("Incorrect async result type", nameof (ar));
      asyncResultNoResult.EndInvoke();
    }

    public IAsyncResult BeginGetServiceStatus(AsyncCallback callback, object asyncState)
    {
      AsyncResult<bool> ar = new AsyncResult<bool>(callback, asyncState);
      this.xmediaServiceRestApi.GetServiceStatus((GetStatusDelegate) ((online, e) =>
      {
        if (e != null)
          ar.SetAsCompleted(e, false);
        else
          ar.SetAsCompleted(online, false);
      }));
      return (IAsyncResult) ar;
    }

    public bool EndGetServiceStatus(IAsyncResult ar)
    {
      return ar is AsyncResult<bool> asyncResult ? asyncResult.EndInvoke() : throw new ArgumentException("Incorrect async result type", nameof (ar));
    }

    public IAsyncResult BeginGetPairedConsoles(AsyncCallback callback, object asyncState)
    {
      if (this.signInState != XmediaService.SignInState.SignedIn)
        throw XmediaService.CreateInvalidOperationExceptionForError(ErrorCode.CompanionNotSignedIn);
      AsyncResult<IEnumerable<DeviceInfo>> ar = new AsyncResult<IEnumerable<DeviceInfo>>(callback, asyncState);
      this.xmediaServiceRestApi.GetPairedConsoles(this.tokenManager, (GetPairedConsolesDelegate) ((consoles, e) =>
      {
        if (e != null)
          ar.SetAsCompleted(e, false);
        else
          ar.SetAsCompleted(consoles, false);
      }));
      return (IAsyncResult) ar;
    }

    public IEnumerable<DeviceInfo> EndGetPairedConsoles(IAsyncResult ar)
    {
      return ar is AsyncResult<IEnumerable<DeviceInfo>> asyncResult ? asyncResult.EndInvoke() : throw new ArgumentException("Incorrect async result type", nameof (ar));
    }

    public IAsyncResult BeginGetUserSessions(AsyncCallback callback, object asyncState)
    {
      if (this.signInState != XmediaService.SignInState.SignedIn)
        throw XmediaService.CreateInvalidOperationExceptionForError(ErrorCode.CompanionNotSignedIn);
      AsyncResult<IEnumerable<SessionInfo>> ar = new AsyncResult<IEnumerable<SessionInfo>>(callback, asyncState);
      this.xmediaServiceRestApi.GetUserSessions(this.tokenManager, (GetUserSessionsDelegate) ((sessions, e) =>
      {
        if (e != null)
          ar.SetAsCompleted(e, false);
        else
          ar.SetAsCompleted(sessions, false);
      }));
      return (IAsyncResult) ar;
    }

    public IEnumerable<SessionInfo> EndGetUserSessions(IAsyncResult ar)
    {
      return ar is AsyncResult<IEnumerable<SessionInfo>> asyncResult ? asyncResult.EndInvoke() : throw new ArgumentException("Incorrect async result type", nameof (ar));
    }

    public IAsyncResult BeginGetUserInfo(string userId, AsyncCallback callback, object asyncState)
    {
      if (this.signInState != XmediaService.SignInState.SignedIn)
        throw XmediaService.CreateInvalidOperationExceptionForError(ErrorCode.CompanionNotSignedIn);
      AsyncResult<UserInfo> ar = new AsyncResult<UserInfo>(callback, asyncState);
      this.xmediaServiceRestApi.GetUserInfo(this.tokenManager, userId, (GetUserInfoDelegate) ((user, e) =>
      {
        if (e != null)
          ar.SetAsCompleted(e, false);
        else
          ar.SetAsCompleted(user, false);
      }));
      return (IAsyncResult) ar;
    }

    public UserInfo EndGetUserInfo(IAsyncResult ar)
    {
      return ar is AsyncResult<UserInfo> asyncResult ? asyncResult.EndInvoke() : throw new ArgumentException("Incorrect async result type", nameof (ar));
    }

    public IAsyncResult BeginGetDeviceInfo(
      string deviceId,
      AsyncCallback callback,
      object asyncState)
    {
      if (this.signInState != XmediaService.SignInState.SignedIn)
        throw XmediaService.CreateInvalidOperationExceptionForError(ErrorCode.CompanionNotSignedIn);
      AsyncResult<DeviceInfo> ar = new AsyncResult<DeviceInfo>(callback, asyncState);
      this.xmediaServiceRestApi.GetDeviceInfo(this.tokenManager, deviceId, (GetDeviceInfoDelegate) ((device, e) =>
      {
        if (e != null)
          ar.SetAsCompleted(e, false);
        else
          ar.SetAsCompleted(device, false);
      }));
      return (IAsyncResult) ar;
    }

    public DeviceInfo EndGetDeviceInfo(IAsyncResult ar)
    {
      return ar is AsyncResult<DeviceInfo> asyncResult ? asyncResult.EndInvoke() : throw new ArgumentException("Incorrect async result type", nameof (ar));
    }

    public IAsyncResult BeginConnectToSession(
      SessionInfo sessionInfo,
      AsyncCallback callback,
      object asyncState)
    {
      if (this.signInState != XmediaService.SignInState.SignedIn)
        throw XmediaService.CreateInvalidOperationExceptionForError(ErrorCode.CompanionNotSignedIn);
      this.tracer.WriteInfo("Connecting to session {0}", (object) sessionInfo.SessionId);
      AsyncResult<XmediaSession> ar = new AsyncResult<XmediaSession>(callback, asyncState);
      XmediaSession session = new XmediaSession(this, new Uri(sessionInfo.BaseUri), ((DependencyObject) Deployment.Current).Dispatcher, this.xmediaServiceRestApi.ActivityId, this.tracer);
      session.SetAuthorizationHeader(this.tokenManager.CompanionToken);
      session.Connect(sessionInfo.SessionId, (Action<Exception>) (e =>
      {
        if (e == null)
        {
          lock (this.objectLock)
            this.sessions.Add(session);
          this.tracer.WriteInfo("Connected to session {0}. Caching base URI: {1}", (object) sessionInfo.SessionId, (object) sessionInfo.BaseUri);
          this.xmediaServiceRestApi.SetXboxLiveEnvironmentUri(new Uri(sessionInfo.BaseUri));
          ar.SetAsCompleted(session, false);
        }
        else
        {
          if (e is WebException && ((WebException) e).Response is HttpWebResponse)
          {
            HttpWebResponse response = (HttpWebResponse) ((WebException) e).Response;
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
              this.tracer.WriteError("ConnectToSession failed: Unauthorized");
              e = (Exception) new UnauthorizedAccessException("Unauthorized access", e);
              response.Close();
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
              using (Stream responseStream = response.GetResponseStream())
              {
                try
                {
                  JsonObject jsonObject = (JsonObject) JsonValue.Load(responseStream);
                  if ((int) jsonObject["error_code"] == 27)
                  {
                    this.tracer.WriteError("ConnectToSession failed: TooManyClients");
                    e = (Exception) XmediaService.CreateInvalidOperationExceptionForError(ErrorCode.TooManyClients, e);
                  }
                  else
                    this.tracer.WriteError("ConnectToSession failed with error_code: {0}", (object) (int) jsonObject["error_code"]);
                }
                catch (Exception ex)
                {
                  this.tracer.WriteError("Failed to parse server response: {0}", (object) ex);
                }
              }
            }
            else
              this.tracer.WriteError("ConnectToSession failed with status code: {0}", (object) response.StatusCode);
          }
          ar.SetAsCompleted(e, false);
        }
      }));
      return (IAsyncResult) ar;
    }

    public XmediaSession EndConnectToSession(IAsyncResult ar)
    {
      return ar is AsyncResult<XmediaSession> asyncResult ? asyncResult.EndInvoke() : throw new ArgumentException("Incorrect async result type", nameof (ar));
    }

    internal void RemoveSession(XmediaSession session)
    {
      lock (this.objectLock)
        this.sessions.Remove(session);
    }

    internal XmediaService.SignInState Test_SignInState
    {
      get => this.signInState;
      set => this.signInState = value;
    }

    internal TokenManager Test_TokenManager => this.tokenManager;

    internal XmediaServiceRestApi Test_XmediaServiceRestApi => this.xmediaServiceRestApi;

    internal bool Test_FastAuthRenewal { get; set; }

    public void Test_SetPartnerToken(string partnerToken)
    {
      this.tokenManager.PartnerToken = partnerToken;
    }

    internal static void Test_Reset()
    {
      lock (XmediaService.singletonLock)
      {
        if (XmediaService.instance == null)
          return;
        XmediaService.instance.Test_Shutdown();
        XmediaService.instance = (XmediaService) null;
      }
    }

    private void Test_Shutdown() => this.tokenRefreshTimer.Dispose();

    private void RefreshCompanionToken(object state)
    {
      bool flag1 = true;
      try
      {
        this.xmediaServiceRestApi.RefreshCompanionToken(this.tokenManager, (RefreshCompanionTokenDelegate) ((xtvToken, tokenRefreshPeriod, e) =>
        {
          if (e == null)
          {
            this.tokenManager.CompanionToken = xtvToken;
            lock (this.objectLock)
            {
              this.tracer.WriteInfo("Updating companion token for {0} sessions", (object) this.sessions.Count);
              foreach (XmediaSession session in this.sessions)
                session.SetAuthorizationHeader(xtvToken);
            }
            this.ScheduleTokenRefresh(tokenRefreshPeriod);
          }
          else
          {
            bool flag2 = true;
            if (e is UnauthorizedAccessException)
            {
              this.tracer.WriteError("Refresh returned Unauthorized. Not rescheduling.");
              flag2 = false;
            }
            if (!flag2)
              return;
            this.tracer.WriteError("Rescheduling token refresh due to error: {0}", (object) e);
            this.ScheduleTokenRefreshRetry();
          }
        }));
        flag1 = false;
      }
      finally
      {
        if (flag1)
        {
          this.tracer.WriteError("Rescheduling token refresh due to error initiating request");
          this.ScheduleTokenRefreshRetry();
        }
      }
    }

    private void ScheduleTokenRefresh(int timeoutInMin)
    {
      if (timeoutInMin == 0)
      {
        this.tokenRefreshTimer.Change(-1, -1);
      }
      else
      {
        int dueTime = timeoutInMin * 60 * 1000;
        if (this.Test_FastAuthRenewal)
          dueTime = timeoutInMin * 1000;
        this.tokenRefreshTimer.Change(dueTime, -1);
      }
    }

    private void ScheduleTokenRefreshRetry() => this.tokenRefreshTimer.Change(120000, -1);

    private static InvalidOperationException CreateInvalidOperationExceptionForError(
      ErrorCode errorCode,
      Exception innerException = null)
    {
      InvalidOperationException exceptionForError = new InvalidOperationException(ServiceMessages.GetServiceMessage(errorCode), innerException);
      exceptionForError.Data[(object) "ErrorCode"] = (object) (int) errorCode;
      return exceptionForError;
    }

    internal enum SignInState
    {
      NotStarted,
      InProgress,
      SignedIn,
    }
  }
}
