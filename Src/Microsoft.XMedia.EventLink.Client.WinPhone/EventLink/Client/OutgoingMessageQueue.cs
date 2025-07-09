// *********************************************************
// Type: Microsoft.XMedia.EventLink.Client.OutgoingMessageQueue
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.Collections.Generic;


namespace Microsoft.XMedia.EventLink.Client
{
  internal class OutgoingMessageQueue
  {
    private readonly int maxPayloadSize;
    private int totalPayloadSize;
    private int readPosition;
    private List<EventLinkMessage> pendingMessages;

    public OutgoingMessageQueue(int maxPayloadSize)
    {
      this.maxPayloadSize = maxPayloadSize;
      this.pendingMessages = new List<EventLinkMessage>();
    }

    public int Count => this.pendingMessages.Count - this.readPosition;

    public int MaxPayloadSize => this.maxPayloadSize;

    public int TotalPayloadSize => this.totalPayloadSize;

    public void Enqueue(EventLinkMessage message)
    {
      int num = 1;
      this.pendingMessages.Add(message);
      this.totalPayloadSize += num;
    }

    public EventLinkMessage[] BeginDequeue()
    {
      if (this.readPosition != 0)
        throw new InvalidOperationException("Attempt to dequeue messages while a dequeue is already in progress");
      EventLinkMessage[] eventLinkMessageArray = this.pendingMessages.Count != 0 ? this.pendingMessages.ToArray() : throw new InvalidOperationException("Attempt to dequeue messages when there are none pending");
      this.readPosition = eventLinkMessageArray.Length;
      return eventLinkMessageArray;
    }

    public void CommitDequeue()
    {
      this.pendingMessages.RemoveRange(0, this.readPosition);
      this.readPosition = 0;
    }

    public void AbortDequeue() => this.readPosition = 0;
  }
}
