// *********************************************************
// Type: Microsoft.XMedia.EventLink.Client.IEventLinkClient
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.Collections.Generic;


namespace Microsoft.XMedia.EventLink.Client
{
  public interface IEventLinkClient
  {
    void Connect(string sessionId, Action<Exception> callback);

    void Disconnect(Action<Exception> callback);

    void Publish(EventLinkMessage message);

    void Publish(IEnumerable<EventLinkMessage> messages);

    EventLinkClientStatus Status { get; }

    event EventHandler<EventLinkMessageEventArgs> MessagesReceived;

    event EventHandler<EventLinkStatusEventArgs> StatusChanged;
  }
}
