// *********************************************************
// Type: Microsoft.XMedia.EventLink.Client.EventLinkClient
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using Microsoft.XMedia.Client;
using Microsoft.XMedia.Client.Tracing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;


namespace Microsoft.XMedia.EventLink.Client
{
  public class EventLinkClient : IEventLinkClient
  {
    private EventLinkClient.PublishClient publishClient;
    private EventLinkClient.SubscribeClient subscribeClient;
    private EventLinkClientStatus status;
    private ObservableCollection<PresenceInfo> presenceCollection;

    private IClientTracer Tracer { get; set; }

    private Uri BaseUri { get; set; }

    private string SessionId { get; set; }

    private ShouldRetry RetryPolicy { get; set; }

    private Dispatcher Dispatcher { get; set; }

    public EventLinkClient(Uri baseUri, string activityId, IClientTracer tracer)
      : this(baseUri, ((DependencyObject) Deployment.Current).Dispatcher, activityId, tracer)
    {
    }

    public EventLinkClient(
      Uri baseUri,
      Dispatcher dispatcher,
      string activityId,
      IClientTracer tracer)
    {
      if (baseUri == (Uri) null)
        throw new ArgumentNullException(nameof (baseUri));
      if (dispatcher == null)
        throw new ArgumentNullException(nameof (dispatcher));
      if (tracer == null)
        tracer = (IClientTracer) new NullClientTracer();
      this.Tracer = tracer;
      this.BaseUri = baseUri;
      this.Dispatcher = dispatcher;
      this.RetryPolicy = RetryPolicies.FastBackoff;
      this.status = EventLinkClientStatus.Disconnected;
      TimeSpan httpTimeout = TimeSpan.FromMinutes(2.0);
      this.publishClient = new EventLinkClient.PublishClient(this, dispatcher, httpTimeout, activityId);
      this.subscribeClient = new EventLinkClient.SubscribeClient(this, dispatcher, httpTimeout, new Action<IEnumerable<EventLinkMessage>>(this.OnSystemMessagesReceived), activityId);
      this.presenceCollection = new ObservableCollection<PresenceInfo>();
      this.Presence = new ReadOnlyObservableCollection<PresenceInfo>(this.presenceCollection);
    }

    public event EventHandler<EventLinkMessageEventArgs> MessagesReceived
    {
      add => this.subscribeClient.MessagesReceived += value;
      remove => this.subscribeClient.MessagesReceived -= value;
    }

    public event EventHandler<EventLinkStatusEventArgs> StatusChanged;

    public uint ClientId { get; private set; }

    public EventLinkClientStatus Status
    {
      get => this.status;
      private set
      {
        if (this.status == value)
          return;
        this.Tracer.WriteInfo("EventLinkClient: status changing to {0}", (object) this.status);
        this.status = value;
        if (this.StatusChanged == null)
          return;
        this.StatusChanged((object) this, new EventLinkStatusEventArgs()
        {
          Status = this.status
        });
      }
    }

    public ReadOnlyObservableCollection<PresenceInfo> Presence { get; private set; }

    public string AuthorizationHeader
    {
      get => this.subscribeClient.AuthHeader;
      set
      {
        this.subscribeClient.AuthHeader = value;
        this.publishClient.AuthHeader = value;
      }
    }

    public void Connect(string sessionId, Action<Exception> callback)
    {
      if (string.IsNullOrEmpty(sessionId))
        throw new ArgumentException("Invalid sessionId", nameof (sessionId));
      if (callback == null)
        throw new ArgumentNullException(nameof (callback));
      this.subscribeClient.Connect(sessionId, callback);
    }

    public void Disconnect(Action<Exception> callback) => this.subscribeClient.Disconnect(callback);

    public void Publish(EventLinkMessage message)
    {
      if (message == null)
        throw new ArgumentNullException(nameof (message));
      this.Publish((IEnumerable<EventLinkMessage>) new EventLinkMessage[1]
      {
        message
      });
    }

    public void Publish(IEnumerable<EventLinkMessage> messages)
    {
      if (messages == null)
        throw new ArgumentNullException(nameof (messages));
      if (this.status != EventLinkClientStatus.Connected)
        throw new InvalidOperationException("EventLinkClient is not connected to a session");
      this.publishClient.Publish(messages);
    }

    private void SubscribeErrorStateChanged(EventLinkClientErrorState state)
    {
      if (state != EventLinkClientErrorState.Permanent)
        return;
      this.publishClient.PermanentError();
      this.Status = EventLinkClientStatus.Faulted;
    }

    private void PublishErrorStateChanged(EventLinkClientErrorState state)
    {
      if (state != EventLinkClientErrorState.Permanent)
        return;
      this.subscribeClient.PermanentError();
      this.Status = EventLinkClientStatus.Faulted;
    }

    private void OnClientConnected(ClientConnectedMessage message)
    {
      PresenceInfo presenceInfo = new PresenceInfo()
      {
        ClientId = message.ClientId,
        DeviceType = message.ClientDeviceType,
        UserDisplayNames = (IEnumerable<string>) message.UserDisplayNames
      };
      for (int index = 0; index < this.presenceCollection.Count; ++index)
      {
        if ((int) this.presenceCollection[index].ClientId == (int) presenceInfo.ClientId)
        {
          this.presenceCollection[index] = presenceInfo;
          return;
        }
      }
      this.Tracer.WriteInfo("EventLinkClient: client ID {0} connected", (object) presenceInfo.ClientId);
      this.presenceCollection.Add(presenceInfo);
    }

    private void OnClientDisconnected(ClientDisconnectedMessage message)
    {
      for (int index = 0; index < this.presenceCollection.Count; ++index)
      {
        if ((int) this.presenceCollection[index].ClientId == (int) message.ClientId)
        {
          this.Tracer.WriteInfo("EventLinkClient: client ID {0} disconnected", (object) this.presenceCollection[index].ClientId);
          this.presenceCollection.RemoveAt(index);
          break;
        }
      }
    }

    private void OnSystemMessagesReceived(IEnumerable<EventLinkMessage> messages)
    {
      foreach (EventLinkMessage message in messages)
      {
        if (message.MessageKind == 2048U)
        {
          switch (message.MessageType)
          {
            case 1:
              this.OnClientConnected(BinarySerializable.FromByteArray<ClientConnectedMessage>(message.ContentBytes));
              continue;
            case 2:
              this.OnClientDisconnected(BinarySerializable.FromByteArray<ClientDisconnectedMessage>(message.ContentBytes));
              continue;
            default:
              continue;
          }
        }
      }
    }

    internal class CommonClient
    {
      protected EventLinkClient eventLinkClient;
      protected int retryCount;
      protected HttpAsyncClient httpClient;
      private System.Threading.Timer retryTimer;

      public CommonClient(
        EventLinkClient eventLinkClient,
        Dispatcher dispatcher,
        TimeSpan httpTimeout,
        string activityId)
      {
        this.eventLinkClient = eventLinkClient;
        this.httpClient = new HttpAsyncClient(httpTimeout, dispatcher, activityId);
      }

      public string AuthHeader
      {
        get => this.httpClient.AuthHeader;
        set => this.httpClient.AuthHeader = value;
      }

      protected Uri CreateMessagesUri(params string[] path)
      {
        return this.CreateServiceUri("Messages", path);
      }

      protected Uri CreateServiceUri(string service, params string[] path)
      {
        StringBuilder stringBuilder = new StringBuilder(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/", new object[1]
        {
          (object) service
        }));
        if (path != null && path.Length > 0)
          stringBuilder.Append(string.Join("/", path));
        return new Uri(this.eventLinkClient.BaseUri, stringBuilder.ToString());
      }

      protected bool RetryRequest(Action OnRetry)
      {
        TimeSpan delay;
        if (!this.eventLinkClient.RetryPolicy(this.retryCount, out delay))
          return false;
        this.retryTimer = new System.Threading.Timer((TimerCallback) (_ =>
        {
          this.retryTimer.Dispose();
          this.retryTimer = (System.Threading.Timer) null;
          this.eventLinkClient.Dispatcher.BeginInvoke(OnRetry);
        }), (object) null, -1, -1);
        ++this.retryCount;
        this.retryTimer.Change(delay, TimeSpan.FromMilliseconds(-1.0));
        return true;
      }
    }

    internal class SubscribeClient : EventLinkClient.CommonClient
    {
      private EventLinkClient.SubscribeClient.SubscribeState state;
      private bool lastRequestCanceled;
      public EventHandler<EventLinkMessageEventArgs> MessagesReceived;
      private string etag;
      private Action<IEnumerable<EventLinkMessage>> systemMessagesCallback;

      public SubscribeClient(
        EventLinkClient eventLinkClient,
        Dispatcher dispatcher,
        TimeSpan httpTimeout,
        Action<IEnumerable<EventLinkMessage>> systemMessagesCallback,
        string activityId)
        : base(eventLinkClient, dispatcher, httpTimeout, activityId)
      {
        this.systemMessagesCallback = systemMessagesCallback;
      }

      public EventLinkClientErrorState ErrorState
      {
        get
        {
          switch (this.state)
          {
            case EventLinkClient.SubscribeClient.SubscribeState.TemporaryError:
              return EventLinkClientErrorState.Temporary;
            case EventLinkClient.SubscribeClient.SubscribeState.PermanentError:
              return EventLinkClientErrorState.Permanent;
            default:
              return EventLinkClientErrorState.None;
          }
        }
      }

      public void PermanentError() => this.OnPermanentError();

      public void Connect(string sessionId, Action<Exception> completionCallback)
      {
        this.eventLinkClient.Tracer.WriteInfo("SubscribeClient: connecting");
        this.OnConnect();
        this.httpClient.PostAsync(this.CreateSubscriptionUri(sessionId), (Action<Stream>) null, (Action<string, Exception>) ((response, error) => this.OnConnectResponse(sessionId, response, error, completionCallback)));
      }

      public void Disconnect(Action<Exception> completionCallback)
      {
        this.OnDisconnect();
        this.httpClient.AbortOutstandingRequests();
        this.OnDisconnectCompleted();
        this.eventLinkClient.Tracer.WriteInfo("SubscribeClient: disconnected");
        if (completionCallback == null)
          return;
        completionCallback((Exception) null);
      }

      private void OnConnectResponse(
        string sessionId,
        string response,
        Exception error,
        Action<Exception> completionCallback)
      {
        this.eventLinkClient.Dispatcher.VerifyAccess();
        if (this.state != EventLinkClient.SubscribeClient.SubscribeState.Connecting)
          return;
        try
        {
          if (error == null)
          {
            try
            {
              this.state = EventLinkClient.SubscribeClient.SubscribeState.Reading;
              this.eventLinkClient.SessionId = sessionId;
              this.eventLinkClient.ClientId = uint.Parse(response, (IFormatProvider) CultureInfo.InvariantCulture);
              this.eventLinkClient.Status = EventLinkClientStatus.Connected;
            }
            catch (FormatException ex)
            {
              error = (Exception) ex;
            }
            catch (OverflowException ex)
            {
              error = (Exception) ex;
            }
          }
          if (error == null)
          {
            this.eventLinkClient.Tracer.WriteInfo("SubscribeClient: connected");
            this.GetMessages();
          }
          else
          {
            this.eventLinkClient.Tracer.WriteInfo("SubscribeClient: connect failed: {0}", (object) error);
            this.OnError();
            this.eventLinkClient.SubscribeErrorStateChanged(this.ErrorState);
          }
        }
        finally
        {
          completionCallback(error);
        }
      }

      private void OnConnect()
      {
        this.state = this.state == EventLinkClient.SubscribeClient.SubscribeState.Initial ? EventLinkClient.SubscribeClient.SubscribeState.Connecting : throw new InvalidOperationException("Subscribe called on EventLinkClient not in the initial state.");
      }

      private void OnGetCompleted()
      {
        switch (this.state)
        {
          case EventLinkClient.SubscribeClient.SubscribeState.Reading:
          case EventLinkClient.SubscribeClient.SubscribeState.TemporaryError:
            this.retryCount = 0;
            this.state = EventLinkClient.SubscribeClient.SubscribeState.Reading;
            break;
          case EventLinkClient.SubscribeClient.SubscribeState.Disconnecting:
            break;
          case EventLinkClient.SubscribeClient.SubscribeState.Disconnected:
            break;
          case EventLinkClient.SubscribeClient.SubscribeState.PermanentError:
            break;
          default:
            throw new InvalidOperationException("OnGetCompleted called on EventLinkClient not in a allowed state.");
        }
      }

      private void OnDisconnect()
      {
        switch (this.state)
        {
          case EventLinkClient.SubscribeClient.SubscribeState.Connecting:
          case EventLinkClient.SubscribeClient.SubscribeState.Reading:
          case EventLinkClient.SubscribeClient.SubscribeState.TemporaryError:
            this.state = EventLinkClient.SubscribeClient.SubscribeState.Disconnecting;
            this.eventLinkClient.Status = EventLinkClientStatus.Disconnecting;
            break;
          case EventLinkClient.SubscribeClient.SubscribeState.PermanentError:
            break;
          default:
            throw new InvalidOperationException("OnDisconnect called on EventLinkClient in an unallowed state.");
        }
      }

      private void OnDisconnectCompleted()
      {
        switch (this.state)
        {
          case EventLinkClient.SubscribeClient.SubscribeState.Disconnecting:
            this.state = EventLinkClient.SubscribeClient.SubscribeState.Disconnected;
            this.eventLinkClient.Status = EventLinkClientStatus.Disconnected;
            break;
          case EventLinkClient.SubscribeClient.SubscribeState.PermanentError:
            break;
          default:
            throw new InvalidOperationException("OnDisconnectCompleted called on EventLinkClient in an unallowed state.");
        }
      }

      private void OnError()
      {
        switch (this.state)
        {
          case EventLinkClient.SubscribeClient.SubscribeState.Connecting:
          case EventLinkClient.SubscribeClient.SubscribeState.Disconnecting:
            this.eventLinkClient.Tracer.WriteError("SubscribeClient.OnError: error occurred during connect/disconnect.");
            this.state = EventLinkClient.SubscribeClient.SubscribeState.PermanentError;
            break;
          case EventLinkClient.SubscribeClient.SubscribeState.Reading:
          case EventLinkClient.SubscribeClient.SubscribeState.TemporaryError:
            if (this.RetryRequest(new Action(this.RetryRequestCallback)))
            {
              this.eventLinkClient.Tracer.WriteWarning("SubscribeClient.OnError: temporary error, retrying.");
              this.state = EventLinkClient.SubscribeClient.SubscribeState.TemporaryError;
              break;
            }
            this.eventLinkClient.Tracer.WriteError("SubscribeClient.OnError: no longer retrying.");
            this.state = EventLinkClient.SubscribeClient.SubscribeState.PermanentError;
            break;
          default:
            throw new InvalidOperationException("OnError called on EventLinkClient in an unallowed state.");
        }
      }

      private void OnPermanentError()
      {
        this.state = EventLinkClient.SubscribeClient.SubscribeState.PermanentError;
      }

      private Uri CreateSubscriptionUri(string subscriptionId)
      {
        return this.CreateServiceUri("Subscriptions", subscriptionId);
      }

      private void GetMessages()
      {
        if (this.state == EventLinkClient.SubscribeClient.SubscribeState.Reading || this.state == EventLinkClient.SubscribeClient.SubscribeState.TemporaryError)
        {
          this.eventLinkClient.Tracer.WriteVerbose("SubscribeClient.GetMessages: getting messages");
          this.httpClient.LongPollAsync(this.CreateMessagesUri(this.eventLinkClient.SessionId, this.eventLinkClient.ClientId.ToString((IFormatProvider) CultureInfo.InvariantCulture)), this.etag, new Action<Stream, long, string, Exception>(this.OnGetMessagesResponse));
        }
        else
          this.eventLinkClient.Tracer.WriteVerbose("SubscribeClient.GetMessages: not polling because state is {0}", (object) this.state);
      }

      private void OnGetMessagesResponse(
        Stream responseStream,
        long contentLength,
        string responseETag,
        Exception error)
      {
        this.eventLinkClient.Dispatcher.VerifyAccess();
        if (error == null)
        {
          this.lastRequestCanceled = false;
          this.OnGetCompleted();
          IList<EventLinkMessage> source1 = EventLinkMessageSerializer.DeserializeMessages(responseStream, contentLength);
          responseStream.Close();
          this.eventLinkClient.Tracer.WriteInfo("SubscribeClient.OnGetMessagesResponse: got {0} messages", (object) source1.Count);
          if (source1.Count > 0)
          {
            this.etag = responseETag;
            IEnumerable<IGrouping<bool, EventLinkMessage>> groupings = source1.GroupBy<EventLinkMessage, bool>((Func<EventLinkMessage, bool>) (m => m.IsSystemMessage));
            EventHandler<EventLinkMessageEventArgs> messagesReceived = this.MessagesReceived;
            foreach (IGrouping<bool, EventLinkMessage> source2 in groupings)
            {
              if (source2.Key)
                this.systemMessagesCallback((IEnumerable<EventLinkMessage>) source2);
              else if (messagesReceived != null && source2.Any<EventLinkMessage>())
                messagesReceived((object) this.eventLinkClient, new EventLinkMessageEventArgs()
                {
                  Messages = (IEnumerable<EventLinkMessage>) source2
                });
            }
          }
          this.GetMessages();
        }
        else
        {
          if (error is WebException webException && webException.Response != null)
            webException.Response.Close();
          if ((this.state == EventLinkClient.SubscribeClient.SubscribeState.Reading || this.state == EventLinkClient.SubscribeClient.SubscribeState.TemporaryError) && webException != null && webException.Status == WebExceptionStatus.RequestCanceled)
          {
            if (this.lastRequestCanceled)
            {
              this.eventLinkClient.Tracer.WriteError("SubscribeClient.OnGetMessagesResponse: two requests cancelled in a row -> permanent error");
              this.OnPermanentError();
              this.eventLinkClient.SubscribeErrorStateChanged(this.ErrorState);
            }
            else
            {
              this.eventLinkClient.Tracer.WriteWarning("SubscribeClient.OnGetMessagesResponse: request was canceled -> retrying immediately");
              this.lastRequestCanceled = true;
              this.OnGetCompleted();
              this.GetMessages();
            }
          }
          else
          {
            if (this.state != EventLinkClient.SubscribeClient.SubscribeState.Reading && this.state != EventLinkClient.SubscribeClient.SubscribeState.TemporaryError)
              return;
            bool flag = false;
            if (webException != null && webException.Response != null)
            {
              HttpWebResponse response = (HttpWebResponse) webException.Response;
              if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Gone)
              {
                this.eventLinkClient.Tracer.WriteError("SubscribeClient.OnGetMessagesResponse: server returned {0}. Forcing permanent error");
                flag = true;
              }
            }
            if (flag)
            {
              this.OnPermanentError();
            }
            else
            {
              this.eventLinkClient.Tracer.WriteError("SubscribeClient.OnGetMessagesResponse: non-permanent error: {0}", (object) webException);
              this.OnError();
            }
            this.eventLinkClient.SubscribeErrorStateChanged(this.ErrorState);
          }
        }
      }

      private void RetryRequestCallback()
      {
        if (this.state != EventLinkClient.SubscribeClient.SubscribeState.TemporaryError)
          return;
        this.eventLinkClient.Tracer.WriteVerbose("SubscribeClient.RetryRequestCallback: retrying get");
        this.GetMessages();
      }

      public enum SubscribeState
      {
        Initial,
        Connecting,
        Reading,
        Disconnecting,
        Disconnected,
        TemporaryError,
        PermanentError,
      }
    }

    internal class PublishClient : EventLinkClient.CommonClient
    {
      private EventLinkClient.PublishClient.PublishState state;
      private OutgoingMessageQueue messageQueue;
      private object syncRoot;

      public PublishClient(
        EventLinkClient eventLinkClient,
        Dispatcher dispatcher,
        TimeSpan httpTimeout,
        string activityId)
        : base(eventLinkClient, dispatcher, httpTimeout, activityId)
      {
        this.messageQueue = new OutgoingMessageQueue(-1);
        this.syncRoot = new object();
      }

      public EventLinkClientErrorState ErrorState
      {
        get
        {
          switch (this.state)
          {
            case EventLinkClient.PublishClient.PublishState.TemporaryError:
              return EventLinkClientErrorState.Temporary;
            case EventLinkClient.PublishClient.PublishState.PermanentError:
              return EventLinkClientErrorState.Permanent;
            default:
              return EventLinkClientErrorState.None;
          }
        }
      }

      public void PermanentError() => this.OnPermanentError();

      public void Publish(IEnumerable<EventLinkMessage> messages)
      {
        if (this.state == EventLinkClient.PublishClient.PublishState.PermanentError)
          throw new InvalidOperationException("Publish called on EventLinkClient in a permanent error state.");
        Monitor.Enter(this.syncRoot);
        foreach (EventLinkMessage message in messages)
          this.messageQueue.Enqueue(message);
        Monitor.Exit(this.syncRoot);
        bool flag = this.state == EventLinkClient.PublishClient.PublishState.Idle;
        this.OnPublish();
        this.eventLinkClient.Tracer.WriteInfo("PublishClient.Publish: enqueuing messages. Should publish = {0}", (object) flag);
        if (!flag)
          return;
        this.PublishMessages();
      }

      private void OnPublish()
      {
        switch (this.state)
        {
          case EventLinkClient.PublishClient.PublishState.Idle:
            this.state = EventLinkClient.PublishClient.PublishState.Publishing;
            break;
          case EventLinkClient.PublishClient.PublishState.Publishing:
            break;
          case EventLinkClient.PublishClient.PublishState.TemporaryError:
            break;
          default:
            throw new InvalidOperationException("OnPublish called on EventLinkClient not in a allowed state.");
        }
      }

      private void OnError()
      {
        switch (this.state)
        {
          case EventLinkClient.PublishClient.PublishState.Publishing:
          case EventLinkClient.PublishClient.PublishState.TemporaryError:
            Monitor.Enter(this.syncRoot);
            this.messageQueue.AbortDequeue();
            Monitor.Exit(this.syncRoot);
            if (this.RetryRequest(new Action(this.RetryRequestCallback)))
            {
              this.eventLinkClient.Tracer.WriteWarning("PublishClient.OnError: temporary error, retrying.");
              this.state = EventLinkClient.PublishClient.PublishState.TemporaryError;
              break;
            }
            this.eventLinkClient.Tracer.WriteError("PublishClient.OnError: no longer retrying.");
            this.state = EventLinkClient.PublishClient.PublishState.PermanentError;
            break;
          default:
            throw new InvalidOperationException("OnError called on EventLinkClient in an unallowed state.");
        }
      }

      private void OnPermanentError()
      {
        this.state = EventLinkClient.PublishClient.PublishState.PermanentError;
      }

      private void RetryRequestCallback()
      {
        if (this.state != EventLinkClient.PublishClient.PublishState.TemporaryError)
          return;
        this.eventLinkClient.Tracer.WriteVerbose("PublishClient.RetryRequestCallback: retrying publish");
        this.PublishMessages();
      }

      private void PublishMessages()
      {
        this.eventLinkClient.Tracer.WriteVerbose("PublishClient.PublishMessages: publishing messages");
        this.httpClient.PostAsync(this.CreateMessagesUri(this.eventLinkClient.SessionId, this.eventLinkClient.ClientId.ToString((IFormatProvider) CultureInfo.InvariantCulture)), new Action<Stream>(this.WriteEvents), new Action<Exception>(this.OnPublishCompleted));
      }

      private void OnPublishCompleted(Exception error)
      {
        this.eventLinkClient.Dispatcher.VerifyAccess();
        if (error == null)
        {
          this.retryCount = 0;
          Monitor.Enter(this.syncRoot);
          this.messageQueue.CommitDequeue();
          int count = this.messageQueue.Count;
          Monitor.Exit(this.syncRoot);
          if (count > 0)
          {
            this.eventLinkClient.Tracer.WriteInfo("PublishClient.OnPublishCompleted: more messages in queue");
            this.state = EventLinkClient.PublishClient.PublishState.Publishing;
            this.PublishMessages();
          }
          else
          {
            this.eventLinkClient.Tracer.WriteInfo("PublishClient.OnPublishCompleted: no messages in queue. Now idle.");
            this.state = EventLinkClient.PublishClient.PublishState.Idle;
          }
        }
        else
        {
          if (this.state != EventLinkClient.PublishClient.PublishState.Publishing && this.state != EventLinkClient.PublishClient.PublishState.TemporaryError)
            return;
          bool flag = false;
          if (error is WebException webException && webException.Response != null)
          {
            HttpWebResponse response = (HttpWebResponse) webException.Response;
            if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Gone)
            {
              this.eventLinkClient.Tracer.WriteError("PublishClient.OnPublishCompleted: server returned {0}. Forcing permanent error");
              flag = true;
            }
          }
          if (flag)
          {
            this.OnPermanentError();
          }
          else
          {
            this.eventLinkClient.Tracer.WriteError("PublishClient.OnPublishCompleted: non-permanent error: {0}", (object) webException);
            this.OnError();
          }
          this.eventLinkClient.PublishErrorStateChanged(this.ErrorState);
        }
      }

      private void WriteEvents(Stream stream)
      {
        EventLinkMessage[] messages = (EventLinkMessage[]) null;
        Monitor.Enter(this.syncRoot);
        try
        {
          messages = this.messageQueue.BeginDequeue();
        }
        catch (InvalidOperationException ex)
        {
          this.eventLinkClient.PublishErrorStateChanged(EventLinkClientErrorState.Permanent);
        }
        Monitor.Exit(this.syncRoot);
        if (messages == null)
          return;
        EventLinkMessageSerializer.SerializeMessages(stream, (IEnumerable<EventLinkMessage>) messages);
      }

      public enum PublishState
      {
        Idle,
        Publishing,
        TemporaryError,
        PermanentError,
      }
    }
  }
}
