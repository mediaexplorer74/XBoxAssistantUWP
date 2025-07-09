// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.XmediaSession
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using Microsoft.XMedia.Client;
using Microsoft.XMedia.EventLink;
using Microsoft.XMedia.EventLink.Client;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Threading;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public class XmediaSession : IXmediaTransport
  {
    private IClientTracer tracer;
    private EventLinkClient eventLinkClient;
    private XmediaService xmediaService;

    public event EventHandler<XmediaMessageReceivedEventArgs> MessageReceived;

    public event EventHandler<DeviceChangedEventArgs> DeviceJoined;

    public event EventHandler<DeviceChangedEventArgs> DeviceLeft;

    public event EventHandler<DeviceChangedEventArgs> DeviceUpdated;

    public event EventHandler Faulted;

    internal XmediaSession(
      XmediaService xmediaService,
      Uri uriBase,
      Dispatcher dispatcher,
      string activityId,
      IClientTracer tracer)
    {
      this.xmediaService = xmediaService;
      this.tracer = tracer;
      this.eventLinkClient = new EventLinkClient(uriBase, dispatcher, activityId, this.tracer);
      this.eventLinkClient.MessagesReceived += new EventHandler<EventLinkMessageEventArgs>(this.OnMessageReceived);
      this.eventLinkClient.StatusChanged += new EventHandler<EventLinkStatusEventArgs>(this.OnStatusChanged);
      ((INotifyCollectionChanged) this.eventLinkClient.Presence).CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnPresenceCollectionChanged);
    }

    public uint ClientId => this.eventLinkClient.ClientId;

    public CommunicationCapabilities CommunicationCapabilities
    {
      get
      {
        return this.eventLinkClient.Status != EventLinkClientStatus.Connected ? CommunicationCapabilities.CanNotConnect : CommunicationCapabilities.CanConnectViaCloud;
      }
    }

    public IAsyncResult BeginLeaveSession(AsyncCallback callback, object asyncState)
    {
      AsyncResultNoResult ar = new AsyncResultNoResult(callback, asyncState);
      this.tracer.WriteInfo("LeaveSession starting");
      this.eventLinkClient.Disconnect((Action<Exception>) (e =>
      {
        if (e == null)
        {
          this.xmediaService.RemoveSession(this);
          this.tracer.WriteInfo("LeaveSession completed successfully");
        }
        else
          this.tracer.WriteError("LeaveSession failed: {0}", (object) e);
        ar.SetAsCompleted(e, false);
      }));
      return (IAsyncResult) ar;
    }

    public void EndLeaveSession(IAsyncResult ar)
    {
      if (!(ar is AsyncResultNoResult asyncResultNoResult))
        throw new ArgumentException("Incorrect async result type", nameof (ar));
      asyncResultNoResult.EndInvoke();
    }

    public void SendMessage(IXmediaMessage message)
    {
      EventLinkMessage message1 = new EventLinkMessage();
      message1.ResponseTo = message.ResponseTo;
      message1.MessageKind = (uint) message.Kind;
      message1.MessageType = (uint) message.Type;
      message1.To = uint.MaxValue;
      message1.SequenceNumber = message.Id;
      byte[] buffer = new byte[(IntPtr) message.GetSerializedSize()];
      using (MemoryStream output = new MemoryStream(buffer))
      {
        using (BinaryWriter writer = new BinaryWriter((Stream) output))
          message.Serialize(writer);
      }
      message1.ContentBytes = buffer;
      this.eventLinkClient.Publish(message1);
    }

    public void SendRawMessage(byte[] buffer)
    {
      this.eventLinkClient.Publish(new EventLinkMessage()
      {
        RawMessageBuffer = buffer
      });
    }

    internal void Connect(string sessionId, Action<Exception> callback)
    {
      this.eventLinkClient.Connect(sessionId, callback);
    }

    internal void SetAuthorizationHeader(string authorizationHeader)
    {
      this.eventLinkClient.AuthorizationHeader = authorizationHeader;
    }

    private void OnStatusChanged(object sender, EventLinkStatusEventArgs e)
    {
      if (e.Status != EventLinkClientStatus.Faulted)
        return;
      this.tracer.WriteError("Session faulted");
      this.xmediaService.RemoveSession(this);
      EventHandler faulted = this.Faulted;
      if (faulted == null)
        return;
      faulted((object) this, new EventArgs());
    }

    private void OnMessageReceived(object sender, EventLinkMessageEventArgs e)
    {
      EventHandler<XmediaMessageReceivedEventArgs> messageReceived = this.MessageReceived;
      if (messageReceived == null)
        return;
      foreach (EventLinkMessage message in e.Messages)
      {
        XmediaMessageReceivedEventArgs e1 = new XmediaMessageReceivedEventArgs(new ReceivedXmediaMessage(message.SequenceNumber, message.ResponseTo, (XmediaMessageType) message.MessageType, (XmediaMessageKind) message.MessageKind, message.ContentBytes)
        {
          RawMessageBuffer = message.RawMessageBuffer
        });
        messageReceived((object) this, e1);
      }
    }

    private void OnPresenceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        EventHandler<DeviceChangedEventArgs> deviceJoined = this.DeviceJoined;
        if (deviceJoined == null)
          return;
        PresenceInfo newItem = (PresenceInfo) e.NewItems[0];
        DeviceChangedEventArgs e1 = new DeviceChangedEventArgs(newItem.ClientId, newItem.DeviceType, newItem.UserDisplayNames);
        this.tracer.WriteInfo("PresenceCollectionChanged: client ID {0} joined on device type {1}", (object) newItem.ClientId, (object) newItem.DeviceType);
        deviceJoined((object) this, e1);
      }
      else if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        EventHandler<DeviceChangedEventArgs> deviceLeft = this.DeviceLeft;
        if (deviceLeft == null)
          return;
        PresenceInfo oldItem = (PresenceInfo) e.OldItems[0];
        DeviceChangedEventArgs e2 = new DeviceChangedEventArgs(oldItem.ClientId, oldItem.DeviceType, oldItem.UserDisplayNames);
        this.tracer.WriteInfo("PresenceCollectionChanged: client ID {0} left on device type {1}", (object) oldItem.ClientId, (object) oldItem.DeviceType);
        deviceLeft((object) this, e2);
      }
      else
      {
        if (e.Action != NotifyCollectionChangedAction.Replace)
          return;
        EventHandler<DeviceChangedEventArgs> deviceUpdated = this.DeviceUpdated;
        if (deviceUpdated == null)
          return;
        PresenceInfo newItem = (PresenceInfo) e.NewItems[0];
        DeviceChangedEventArgs e3 = new DeviceChangedEventArgs(newItem.ClientId, newItem.DeviceType, newItem.UserDisplayNames);
        this.tracer.WriteInfo("PresenceCollectionChanged: client ID {0} changed", (object) newItem.ClientId);
        deviceUpdated((object) this, e3);
      }
    }
  }
}
