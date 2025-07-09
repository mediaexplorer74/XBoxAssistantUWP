// *********************************************************
// Type: LRC.Session.LRCSession
// Assembly: LRC.Session, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D38BC875-BBEE-4348-AA20-8CADF3B9A3EB
// *********************************************************LRC.Session.dll

using LRC.LocalSubnet;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.XMedia;
using Microsoft.Xmedia.Client.WindowsPhone;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using Xbox.Live.Phone;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Utils;


namespace LRC.Session
{
  [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Necessary")]
  public class LRCSession : IXmediaSession, IDisposable
  {
    public const int RequestTimeOutMilliseconds = 8000;
    public const int WaitForConsoleJoinTimeOutMilliseconds = 10000;
    private const int ConsoleLaunchDelay = 2000;
    private const string ComponentName = "LRCSession";
    private const string TMFErrorCodeString = "ErrorCode";
    private const char Colon = ':';
    private static TimeSpan commandExpirationTime = new TimeSpan(0, 0, 0, 0, 1000);
    private bool disposed;
    private List<SessionInfo> sessionInfoList;
    private LRCLocalTransport localTransport;
    private XmediaSession currentCloudMediaSession;
    private ManualResetEvent consoleJoinedEvent;
    private string consoleIPAddress;
    private int consolePortNumber;
    private uint activeTitleId;
    private MediaState currentMediaState;
    private uint consoleVersion;
    private string consoleLocale;
    private uint consoleLiveTVTitleId;
    private uint notificationSequenceNumber;
    private LRCMessage currentPendingRequest;
    private LRC_RESPONSE currentPendingResponse;
    private ManualResetEvent pendingRequestEvent;
    private ManualResetEvent queueClearEvent;
    private object messageLock = new object();
    private List<LRCSession.RequestEntry> requestQueue = new List<LRCSession.RequestEntry>();
    private bool isSignedInToTMFService;
    private bool isStopping;

    public LRCSession()
    {
      this.isStopping = false;
      this.ForceUsingCloud = false;
      this.localTransport = (LRCLocalTransport) null;
      this.currentCloudMediaSession = (XmediaSession) null;
      this.currentPendingRequest = (LRCMessage) null;
      this.pendingRequestEvent = new ManualResetEvent(false);
      this.consoleJoinedEvent = new ManualResetEvent(false);
      this.queueClearEvent = new ManualResetEvent(true);
      this.isSignedInToTMFService = false;
      this.CommunicationCapabilities = CommunicationCapabilities.CanNotConnect;
    }

    ~LRCSession() => this.Dispose(false);

    public event EventHandler<ConnectionEventArgs> ConnectionFailed
    {
      add
      {
        this.InternalConnectionFailed -= value;
        this.InternalConnectionFailed += value;
      }
      remove => this.InternalConnectionFailed -= value;
    }

    public event EventHandler<MediaStateUpdatedEventArgs> MediaStateUpdated
    {
      add
      {
        this.InternalMediaStateUpdated -= value;
        this.InternalMediaStateUpdated += value;
      }
      remove => this.InternalMediaStateUpdated -= value;
    }

    public event EventHandler<TitleChangedEventArgs> TitleChanged
    {
      add
      {
        this.InternalTitleChanged -= value;
        this.InternalTitleChanged += value;
      }
      remove => this.InternalTitleChanged -= value;
    }

    public event EventHandler<MessageReceivedEventArgs> MessageReceived
    {
      add
      {
      }
      remove
      {
      }
    }

    private event EventHandler<ConnectionEventArgs> InternalConnectionFailed;

    private event EventHandler<MediaStateUpdatedEventArgs> InternalMediaStateUpdated;

    private event EventHandler<TitleChangedEventArgs> InternalTitleChanged;

    public string SessionId { get; set; }

    public CommunicationCapabilities CommunicationCapabilities { get; set; }

    public bool ForceUsingCloud { get; set; }

    public uint ActiveTitleId
    {
      get => this.activeTitleId;
      private set
      {
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          this.activeTitleId = value;
          EventHandler<TitleChangedEventArgs> internalTitleChanged = this.InternalTitleChanged;
          if (internalTitleChanged == null)
            return;
          internalTitleChanged((object) this, new TitleChangedEventArgs(this.activeTitleId));
        }, (object) null);
      }
    }

    public MediaState CurrentMediaState
    {
      get => this.currentMediaState;
      private set
      {
        if (this.currentMediaState == value)
          return;
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          this.currentMediaState = value;
          if ((int) this.ActiveTitleId != (int) this.currentMediaState.TitleId)
            this.ActiveTitleId = this.currentMediaState.TitleId;
          EventHandler<MediaStateUpdatedEventArgs> mediaStateUpdated = this.InternalMediaStateUpdated;
          if (mediaStateUpdated == null)
            return;
          mediaStateUpdated((object) this, new MediaStateUpdatedEventArgs(this.CurrentMediaState));
        }, (object) null);
      }
    }

    public uint ConsoleLiveTVTitleId
    {
      get => this.consoleLiveTVTitleId;
      private set
      {
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          this.consoleLiveTVTitleId = value;
        }, (object) null);
      }
    }

    public string ConsoleLocale
    {
      get => this.consoleLocale;
      private set
      {
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          this.consoleLocale = value;
        }, (object) null);
      }
    }

    public uint ConsoleVersion
    {
      get => this.consoleVersion;
      private set
      {
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          this.consoleVersion = value;
        }, (object) null);
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Async operation, disposed later")]
    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Trace message")]
    [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "Waiting for official exception")]
    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Can't throw exception in worker thread")]
    public IAsyncResult BeginJoinSession(AsyncCallback callback, object asyncState)
    {
      SessionAsyncResult result = new SessionAsyncResult(callback, asyncState);
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        bool flag = true;
        while (flag)
        {
          flag = this.sessionInfoList != null;
          try
          {
            this.JoinSessionInternal();
            flag = false;
            result.Error = (Exception) null;
          }
          catch (Exception ex)
          {
            result.Error = ex;
            this.sessionInfoList = (List<SessionInfo>) null;
          }
        }
        result.Complete();
      });
      return (IAsyncResult) result;
    }

    public void EndJoinSession(IAsyncResult ar)
    {
      this.InternalEndOperation(LRCSession.Operation.JoinSession, ar);
    }

    public IAsyncResult BeginLeaveSession(AsyncCallback callback, object asyncState)
    {
      throw new NotImplementedException();
    }

    public void EndLeaveSession(IAsyncResult ar) => throw new NotImplementedException();

    public IAsyncResult BeginGetActiveTitleId(AsyncCallback callback, object asyncState)
    {
      return this.InternalBeginOperation(LRCSession.Operation.GetActiveTitleId, callback, asyncState, (object[]) null);
    }

    public uint EndGetActiveTitleId(IAsyncResult ar)
    {
      this.InternalEndOperation(LRCSession.Operation.GetActiveTitleId, ar);
      this.ActiveTitleId = (uint) ((SessionAsyncResult) ar).AsyncResult;
      return this.ActiveTitleId;
    }

    public IAsyncResult BeginGetMediaState(AsyncCallback callback, object asyncState)
    {
      return this.InternalBeginOperation(LRCSession.Operation.GetMediaState, callback, asyncState, (object[]) null);
    }

    public MediaState EndGetMediaState(IAsyncResult ar)
    {
      this.InternalEndOperation(LRCSession.Operation.GetMediaState, ar);
      this.CurrentMediaState = (MediaState) ((SessionAsyncResult) ar).AsyncResult;
      return this.CurrentMediaState;
    }

    public IAsyncResult BeginSendControlCommand(
      ControlKey key,
      AsyncCallback callback,
      object asyncState)
    {
      object[] parameters = new object[1]{ (object) key };
      return this.InternalBeginOperation(LRCSession.Operation.SendControlCommand, callback, asyncState, parameters);
    }

    public void EndSendControlCommand(IAsyncResult ar)
    {
      this.InternalEndOperation(LRCSession.Operation.SendControlCommand, ar);
    }

    public IAsyncResult BeginLaunchTitle(
      uint titleId,
      string parameter,
      AsyncCallback callback,
      object asyncState)
    {
      object[] parameters = new object[2]
      {
        (object) titleId,
        (object) parameter
      };
      return this.InternalBeginOperation(LRCSession.Operation.LaunchTitle, callback, asyncState, parameters);
    }

    public void EndLaunchTitle(IAsyncResult ar)
    {
      this.InternalEndOperation(LRCSession.Operation.LaunchTitle, ar);
    }

    public IAsyncResult BeginSendMessage(string message, AsyncCallback callback, object asyncState)
    {
      throw new NotImplementedException();
    }

    public void EndSendMessage(IAsyncResult ar) => throw new NotImplementedException();

    public IAsyncResult BeginSendSeekControlCommand(
      ulong seekPosition,
      AsyncCallback callback,
      object asyncState)
    {
      throw new NotImplementedException();
    }

    public void EndSendSeekControlCommand(IAsyncResult ar) => throw new NotImplementedException();

    public IAsyncResult BeginGetConsoleSettings(AsyncCallback callback, object asyncState)
    {
      return this.InternalBeginOperation(LRCSession.Operation.GetConsoleSettings, callback, asyncState, (object[]) null);
    }

    public void EndGetConsoleSettings(IAsyncResult ar)
    {
      try
      {
        this.InternalEndOperation(LRCSession.Operation.GetConsoleSettings, ar);
        LRC_GET_CONSOLE_SETTINGS_PARAMS asyncResult = (LRC_GET_CONSOLE_SETTINGS_PARAMS) ((SessionAsyncResult) ar).AsyncResult;
        this.ConsoleLiveTVTitleId = asyncResult.LiveTvProvider;
        this.ConsoleLocale = asyncResult.Locale;
        this.ConsoleVersion = asyncResult.FlashVersion;
      }
      catch (XMobileException ex)
      {
      }
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        if (this.pendingRequestEvent != null)
        {
          this.pendingRequestEvent.Close();
          this.pendingRequestEvent = (ManualResetEvent) null;
        }
        if (this.consoleJoinedEvent != null)
        {
          this.consoleJoinedEvent.Close();
          this.consoleJoinedEvent = (ManualResetEvent) null;
        }
        if (this.queueClearEvent != null)
        {
          this.queueClearEvent.Close();
          this.queueClearEvent = (ManualResetEvent) null;
        }
        if (this.localTransport != null)
        {
          this.localTransport.Dispose();
          this.localTransport = (LRCLocalTransport) null;
        }
      }
      this.disposed = true;
    }

    private static void CheckTMFError(Exception e)
    {
      if (!(e is InvalidOperationException) || !e.Data.Contains((object) "ErrorCode"))
        return;
      object obj = e.Data[(object) "ErrorCode"];
      if (obj != null && obj is int num && num == 27)
        throw new XMobileException(e, 2019, "Too many clients in the session, ConnectToSession failed");
    }

    private static bool HasSuitableLocalNetwork()
    {
      bool flag = false;
      if (1 == Microsoft.Devices.Environment.DeviceType)
        flag = true;
      else if (DeviceNetworkInformation.IsNetworkAvailable)
      {
        using (NetworkInterfaceList networkInterfaceList = new NetworkInterfaceList())
        {
          foreach (NetworkInterfaceInfo networkInterfaceInfo in networkInterfaceList)
          {
            if (1 == networkInterfaceInfo.InterfaceState)
            {
              NetworkInterfaceType interfaceType = networkInterfaceInfo.InterfaceType;
              if (interfaceType == 6 || interfaceType == 71)
              {
                switch (networkInterfaceInfo.InterfaceSubtype - 8)
                {
                  case 0:
                  case 1:
                    flag = true;
                    break;
                }
              }
              if (flag)
                break;
            }
          }
        }
      }
      return flag;
    }

    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "No need as static")]
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Used in debug build")]
    private void InternalEndOperation(LRCSession.Operation op, IAsyncResult ar)
    {
      SessionAsyncResult sessionAsyncResult = (SessionAsyncResult) ar;
      if (ar == null)
        throw new ArgumentNullException(nameof (ar));
      sessionAsyncResult.EndInvoke();
    }

    private void OnResponseReceived(LRC_RESPONSE response)
    {
      if (response == null || this.currentPendingRequest == null || (int) response.Header.MessageType != (int) this.currentPendingRequest.CurrentRequest.Header.MessageType || response.Header.SequenceNumber <= this.currentPendingRequest.CurrentRequest.Header.SequenceNumber)
        return;
      LRCMessage.CurrentSequenceNumber = response.Header.SequenceNumber + 1U;
      this.currentPendingResponse = response;
      this.pendingRequestEvent.Set();
    }

    private void OnSessionJoined(LRC_JOIN_SESSION_RESPONSE joinResponse)
    {
      if (joinResponse == null)
        return;
      LRCMessage.CurrentSequenceNumber = joinResponse.Params.SessionSequenceNumber;
      LRCMessage.ConsoleClientId = joinResponse.Header.FromClientId;
      this.notificationSequenceNumber = joinResponse.Params.NotificationSequenceNumber;
    }

    private void OnNotificationReceived(LRC_NOTIFICATION notification)
    {
      if (notification == null || notification.Header.SequenceNumber < this.notificationSequenceNumber && this.notificationSequenceNumber != 0U)
        return;
      this.notificationSequenceNumber = notification.Header.SequenceNumber + 1U;
      switch (notification)
      {
        case LRC_NON_MEDIA_TITLE_STATE_NOTIFICATION stateNotification1:
          this.OnTitleChanged((object) null, new TitleChangedEventArgs(stateNotification1.Params.TitleId));
          break;
        case LRC_MEDIA_TITLE_STATE_NOTIFICATION stateNotification2:
          this.OnMediaStateUpdated((object) null, new MediaStateUpdatedEventArgs(LRCMessageParser.ConvertMediaStateObject(stateNotification2.Params)));
          break;
      }
    }

    private void OnCloudDeviceJoined(object sender, DeviceChangedEventArgs e)
    {
      if (e.DeviceType != DeviceType.Xbox)
        return;
      LRCMessage.ConsoleClientId = e.ClientId;
      this.consoleJoinedEvent.Set();
    }

    private void OnCloudDeviceLeft(object sender, DeviceChangedEventArgs e)
    {
    }

    private void OnMessageReceived(object sender, XmediaMessageReceivedEventArgs e)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.messageLock)
        {
          if (e.Message == null)
            return;
          bool flag = e.Message is ReceivedLocalSubnetXmediaMessage;
          if (e.Message.RawMessageBuffer == null || e.Message.RawMessageBuffer.Length <= 0)
          {
            if (!flag || this.currentPendingRequest == null)
              return;
            this.pendingRequestEvent.Set();
          }
          else
          {
            List<object> messages = new LRCMessageParser().ParseMessages(e.Message.RawMessageBuffer);
            if (messages == null || messages.Count <= 0)
              return;
            for (int index = 0; index < messages.Count; ++index)
            {
              if (messages[index] is LRC_RESPONSE)
                this.OnResponseReceived((LRC_RESPONSE) messages[index]);
              else
                this.OnNotificationReceived((LRC_NOTIFICATION) messages[index]);
            }
          }
        }
      });
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Error is ignored for local subnet")]
    private void ConnectToConsoleOnLocalSubnet()
    {
      try
      {
        this.localTransport = new LRCLocalTransport(this.consoleIPAddress, this.consolePortNumber);
        this.localTransport.MessageReceived += new EventHandler<XmediaMessageReceivedEventArgs>(this.OnMessageReceived);
        this.CommunicationCapabilities = CommunicationCapabilities.CanConnectViaTcp;
        this.InternalEndOperation(LRCSession.Operation.JoinSession, this.InternalBeginOperation(LRCSession.Operation.JoinSession, (AsyncCallback) null, (object) null, (object[]) null));
        this.localTransport.StartMulticastListener();
      }
      catch (Exception ex)
      {
        this.CommunicationCapabilities = CommunicationCapabilities.CanNotConnect;
        if (!(ex is XMobileException xmobileException) || xmobileException.ErrorCode != 2019)
          return;
        throw;
      }
    }

    private void OnMediaStateUpdated(object sender, MediaStateUpdatedEventArgs e)
    {
      this.CurrentMediaState = e.CurrentMediaState;
    }

    private void OnTitleChanged(object sender, TitleChangedEventArgs e)
    {
      this.ActiveTitleId = e.CurrentRunningTitleId;
    }

    [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Session result disposed later")]
    private IAsyncResult InternalBeginOperation(
      LRCSession.Operation op,
      AsyncCallback callback,
      object asyncState,
      object[] parameters)
    {
      SessionAsyncResult result = new SessionAsyncResult(callback, asyncState);
      lock (this)
      {
        if (this.isStopping)
        {
          result.Error = (Exception) new XMobileException(3001, "Request stopped due to FAS");
          result.Complete();
          return (IAsyncResult) result;
        }
        LRCSession.RequestEntry requestEntry = new LRCSession.RequestEntry();
        requestEntry.Op = op;
        requestEntry.Parameters = parameters;
        requestEntry.Result = result;
        requestEntry.RequestTime = DateTime.UtcNow;
        if (this.requestQueue.Count == 0)
        {
          this.requestQueue.Add(requestEntry);
          this.queueClearEvent.Reset();
          this.InternalBeginOperationWorker(result, op, parameters);
          return (IAsyncResult) result;
        }
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        int num = -1;
        int index1 = this.requestQueue.Count;
        for (int index2 = 0; index2 < this.requestQueue.Count; ++index2)
        {
          switch (this.requestQueue[index2].Op)
          {
            case LRCSession.Operation.GetActiveTitleId:
              flag1 = true;
              break;
            case LRCSession.Operation.GetMediaState:
              flag2 = true;
              break;
            case LRCSession.Operation.SendControlCommand:
            case LRCSession.Operation.SendSeekControlCommand:
              num = index2;
              break;
            case LRCSession.Operation.GetConsoleSettings:
              flag3 = true;
              break;
          }
        }
        if (op == LRCSession.Operation.GetActiveTitleId && flag1 || op == LRCSession.Operation.GetMediaState && flag2 || op == LRCSession.Operation.GetConsoleSettings && flag3)
        {
          result.Error = (Exception) new XMobileException(3001, "Redundant request cancelled");
          result.Complete();
          return (IAsyncResult) result;
        }
        if (op == LRCSession.Operation.SendControlCommand || op == LRCSession.Operation.SendSeekControlCommand)
          index1 = num != -1 ? num + 1 : 1;
        this.requestQueue.Insert(index1, requestEntry);
        this.queueClearEvent.Reset();
      }
      return (IAsyncResult) result;
    }

    private LRCSession.RequestEntry GetNextRequestEntry()
    {
      LRCSession.RequestEntry nextRequestEntry = (LRCSession.RequestEntry) null;
      while (this.requestQueue.Count > 0)
      {
        LRCSession.RequestEntry request = this.requestQueue[0];
        if (request.Op != LRCSession.Operation.SendControlCommand && request.Op != LRCSession.Operation.SendSeekControlCommand || !(DateTime.UtcNow - request.RequestTime > LRCSession.commandExpirationTime))
          return request;
        this.requestQueue.RemoveAt(0);
        request.Result.Error = (Exception) new XMobileException(3001, "Request cancelled due to expiration");
        request.Result.Complete();
        nextRequestEntry = (LRCSession.RequestEntry) null;
      }
      return nextRequestEntry;
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception will be rethrown")]
    private IAsyncResult InternalBeginOperationWorker(
      SessionAsyncResult result,
      LRCSession.Operation op,
      object[] parameters)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        try
        {
          this.ExecuteOperation(op, parameters, result);
        }
        catch (Exception ex)
        {
          Exception innerException = ex;
          if (!(innerException is XMobileException))
            innerException = (Exception) new XMobileException(innerException, 2008, "ExecuteOperation failed");
          result.Error = innerException;
        }
        finally
        {
          bool flag = false;
          if (result.Error != null && (!(result.Error is XMobileException) || ((XMobileException) result.Error).ErrorCode != 2021))
            flag = true;
          result.Complete();
          lock (this)
          {
            this.requestQueue.RemoveAt(0);
            if (this.requestQueue.Count > 0)
            {
              if (!flag)
              {
                LRCSession.RequestEntry nextRequestEntry = this.GetNextRequestEntry();
                if (nextRequestEntry != null)
                  this.InternalBeginOperationWorker(nextRequestEntry.Result, nextRequestEntry.Op, nextRequestEntry.Parameters);
              }
              else
                this.CancelAllPendingRequests();
            }
            if (this.requestQueue.Count <= 0)
              this.queueClearEvent.Set();
          }
          if (flag && op != LRCSession.Operation.JoinSession)
            this.TriggerConnectionFailed(result.Error);
        }
      });
      return (IAsyncResult) result;
    }

    private void WaitForOperationToComplete()
    {
      if (!this.pendingRequestEvent.WaitOne(8000))
      {
        lock (this.messageLock)
          this.ResetPendingRequestResponse();
        throw new XMobileException(2007, "Operation timed out.");
      }
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Used in debug build")]
    [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "Used in debug build")]
    private void ExecuteOperation(
      LRCSession.Operation op,
      object[] parameters,
      SessionAsyncResult result)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      try
      {
        switch (op)
        {
          case LRCSession.Operation.JoinSession:
            this.currentPendingRequest = new LRCMessage(2147483649U, (object[]) null);
            break;
          case LRCSession.Operation.GetActiveTitleId:
            this.currentPendingRequest = new LRCMessage(1U, (object[]) null);
            break;
          case LRCSession.Operation.GetMediaState:
            this.currentPendingRequest = new LRCMessage(4U, (object[]) null);
            break;
          case LRCSession.Operation.LaunchTitle:
            this.currentPendingRequest = new LRCMessage(2U, parameters);
            break;
          case LRCSession.Operation.SendControlCommand:
            this.currentPendingRequest = new LRCMessage(3U, parameters);
            break;
          case LRCSession.Operation.GetConsoleSettings:
            this.currentPendingRequest = new LRCMessage(12U, (object[]) null);
            break;
        }
        if ((this.CommunicationCapabilities & CommunicationCapabilities.CanConnectViaTcp) == CommunicationCapabilities.CanNotConnect && (this.CommunicationCapabilities & CommunicationCapabilities.CanConnectViaCloud) == CommunicationCapabilities.CanNotConnect)
          throw new XMobileException(2010, "Can not connect");
        PerfTracer perfTracer = new PerfTracer();
        bool flag;
        if (this.ForceUsingCloud || (this.CommunicationCapabilities & CommunicationCapabilities.CanConnectViaTcp) == CommunicationCapabilities.CanNotConnect)
        {
          flag = true;
          this.currentCloudMediaSession.SendRawMessage(this.currentPendingRequest.GetUnEncryptedMessageBuffer());
        }
        else
        {
          flag = false;
          this.localTransport.SendTCPCommand(this.currentPendingRequest.GetEncryptedMessageBuffer());
        }
        this.WaitForOperationToComplete();
        if (!flag && this.localTransport.LastError != null)
          throw this.localTransport.LastError;
        if (this.currentPendingResponse == null || this.currentPendingResponse.Header.ResultCode != 0U)
        {
          if (this.currentPendingResponse != null && this.currentPendingResponse.Header.ResultCode == 2147946714U)
            throw new XMobileException(2019, "Too many clients");
          if (this.currentPendingResponse != null)
            throw new XMobileException(2021, "Request failed by console: " + (object) this.currentPendingResponse.Header.ResultCode);
          throw new XMobileException(2008, "Request failed with empty response.");
        }
        switch (op)
        {
          case LRCSession.Operation.JoinSession:
            this.OnSessionJoined(this.currentPendingResponse as LRC_JOIN_SESSION_RESPONSE);
            break;
          case LRCSession.Operation.GetActiveTitleId:
            LRC_GET_ACTIVE_TITLEID_RESPONSE currentPendingResponse1 = (LRC_GET_ACTIVE_TITLEID_RESPONSE) this.currentPendingResponse;
            result.AsyncResult = (object) currentPendingResponse1.Params.TitleId;
            break;
          case LRCSession.Operation.GetMediaState:
            LRC_GET_MEDIA_TITLE_STATE_RESPONSE currentPendingResponse2 = (LRC_GET_MEDIA_TITLE_STATE_RESPONSE) this.currentPendingResponse;
            result.AsyncResult = (object) LRCMessageParser.ConvertMediaStateObject(currentPendingResponse2.Params);
            break;
          case LRCSession.Operation.LaunchTitle:
            Thread.Sleep(2000);
            break;
          case LRCSession.Operation.GetConsoleSettings:
            LRC_GET_CONSOLE_SETTINGS_RESPONSE currentPendingResponse3 = (LRC_GET_CONSOLE_SETTINGS_RESPONSE) this.currentPendingResponse;
            result.AsyncResult = (object) currentPendingResponse3.Params;
            break;
        }
      }
      finally
      {
        this.ResetPendingRequestResponse();
      }
    }

    private void ResetPendingRequestResponse()
    {
      this.currentPendingRequest = (LRCMessage) null;
      this.currentPendingResponse = (LRC_RESPONSE) null;
      this.pendingRequestEvent.Reset();
    }

    private void CurrentCloudMediaSession_Faulted(object sender, EventArgs e)
    {
    }

    private void CancelAllPendingRequests()
    {
      if (this.requestQueue.Count <= 0)
        return;
      for (int index = 0; index < this.requestQueue.Count; ++index)
      {
        SessionAsyncResult result = this.requestQueue[index].Result;
        result.Error = (Exception) new XMobileException(3001, "Request cancelled");
        result.Complete();
      }
      this.requestQueue.Clear();
    }

    private void TriggerConnectionFailed(Exception error)
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        EventHandler<ConnectionEventArgs> connectionFailed = this.InternalConnectionFailed;
        ConnectionEventArgs e = new ConnectionEventArgs()
        {
          Error = error
        };
        if (connectionFailed == null)
          return;
        connectionFailed((object) this, e);
      }, (object) this);
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Can't throw exception in worker thread")]
    [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "Used in debug build")]
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Necessary")]
    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Necessary")]
    private void JoinSessionInternal()
    {
      PerfTracer perfTracer = new PerfTracer();
      this.CommunicationCapabilities = CommunicationCapabilities.CanNotConnect;
      this.queueClearEvent.WaitOne();
      lock (this)
      {
        this.currentPendingRequest = (LRCMessage) null;
        this.currentPendingResponse = (LRC_RESPONSE) null;
      }
      this.ForceUsingCloud = false;
      if (this.sessionInfoList != null)
      {
        if (this.currentCloudMediaSession != null)
        {
          try
          {
            RetryCaller.Retry((SendOrPostCallback) delegate
            {
              this.currentCloudMediaSession.EndLeaveSession(this.currentCloudMediaSession.BeginLeaveSession((AsyncCallback) null, (object) null));
            }, (object) null);
          }
          catch (Exception ex)
          {
          }
        }
        this.currentCloudMediaSession = (XmediaSession) null;
      }
      else
      {
        if (this.isSignedInToTMFService)
        {
          RetryCaller.Retry((SendOrPostCallback) delegate
          {
            XmediaService.Instance.EndCompanionLogout(XmediaService.Instance.BeginCompanionLogout((AsyncCallback) null, (object) null));
          }, (object) null);
          this.isSignedInToTMFService = false;
        }
        int environment = (int) EnvironmentState.Instance.Environment;
        XmediaService.Instance.SetXboxLiveEnvironment(ServiceCommon.GetEnvironmentName(EnvironmentState.Instance.Environment));
        try
        {
          RetryCaller.Retry((SendOrPostCallback) delegate
          {
            XmediaService.Instance.EndCompanionLogin(XmediaService.Instance.BeginCompanionLogin((AsyncCallback) null, (object) null));
          }, (object) null);
          this.isSignedInToTMFService = true;
        }
        catch (Exception ex)
        {
          throw new XMobileException(ex, 1005, "CompanionLogin failed");
        }
        try
        {
          RetryCaller.Retry((SendOrPostCallback) delegate
          {
            this.sessionInfoList = new List<SessionInfo>(XmediaService.Instance.EndGetUserSessions(XmediaService.Instance.BeginGetUserSessions((AsyncCallback) null, (object) null)));
            this.SessionId = this.sessionInfoList.Count > 0 ? this.sessionInfoList[0].SessionId : throw new XMobileException(1012, "Can't find user session");
          }, (object) null);
        }
        catch (Exception ex)
        {
          if (!(ex is XMobileException))
            throw new XMobileException(ex, 1006, "GetUserSession failed");
          throw;
        }
      }
      try
      {
        RetryCaller.Retry((SendOrPostCallback) delegate
        {
          this.currentCloudMediaSession = XmediaService.Instance.EndConnectToSession(XmediaService.Instance.BeginConnectToSession(this.sessionInfoList[0], (AsyncCallback) null, (object) null));
        }, (object) null);
      }
      catch (Exception ex)
      {
        LRCSession.CheckTMFError(ex);
        throw new XMobileException(ex, 1007, "ConnectToSession failed");
      }
      LRCMessage.MyClientId = this.currentCloudMediaSession.ClientId;
      this.currentCloudMediaSession.MessageReceived += new EventHandler<XmediaMessageReceivedEventArgs>(this.OnMessageReceived);
      this.currentCloudMediaSession.DeviceJoined += new EventHandler<DeviceChangedEventArgs>(this.OnCloudDeviceJoined);
      this.currentCloudMediaSession.DeviceLeft += new EventHandler<DeviceChangedEventArgs>(this.OnCloudDeviceLeft);
      string[] strArray = !string.IsNullOrEmpty(this.sessionInfoList[0].LanConnectionIPAddress) ? this.sessionInfoList[0].LanConnectionIPAddress.Split(':') : throw new XMobileException(1008, "No IP address in the session info");
      this.consoleIPAddress = strArray.Length == 2 ? strArray[0] : throw new XMobileException(1009, "Wrong IP address format");
      this.consolePortNumber = Convert.ToInt32(strArray[1], (IFormatProvider) CultureInfo.InvariantCulture);
      LRCCrypto.SetEncryptionKey(LRCCrypto.HexStringToByteArray(this.sessionInfoList[0].LanConnectionKey));
      if (!LRCSession.HasSuitableLocalNetwork())
        this.ForceUsingCloud = true;
      if (!this.ForceUsingCloud)
        this.ConnectToConsoleOnLocalSubnet();
      if ((this.CommunicationCapabilities & CommunicationCapabilities.CanConnectViaTcp) == CommunicationCapabilities.CanNotConnect)
      {
        if (!this.consoleJoinedEvent.WaitOne(10000))
          throw new XMobileException(1010, "Time out waiting for console joined event");
        this.CommunicationCapabilities = CommunicationCapabilities.CanConnectViaCloud;
        try
        {
          this.InternalEndOperation(LRCSession.Operation.JoinSession, this.InternalBeginOperation(LRCSession.Operation.JoinSession, (AsyncCallback) null, (object) null, (object[]) null));
        }
        catch (Exception ex)
        {
          throw new XMobileException(ex, 1011, "Failed to join session over cloud");
        }
      }
      this.currentCloudMediaSession.Faulted -= new EventHandler(this.CurrentCloudMediaSession_Faulted);
      this.currentCloudMediaSession.Faulted += new EventHandler(this.CurrentCloudMediaSession_Faulted);
    }

    [Flags]
    private enum Operation
    {
      JoinSession = 1,
      GetActiveTitleId = 2,
      GetMediaState = 4,
      LaunchTitle = 8,
      SendControlCommand = 16, // 0x00000010
      SendMessage = 32, // 0x00000020
      SendSeekControlCommand = 64, // 0x00000040
      LeaveSession = 128, // 0x00000080
      GetConsoleSettings = 256, // 0x00000100
    }

    private class RequestEntry
    {
      public LRCSession.Operation Op { get; set; }

      public object[] Parameters { get; set; }

      public SessionAsyncResult Result { get; set; }

      public DateTime RequestTime { get; set; }
    }
  }
}
