// *********************************************************
// Type: Microsoft.XMedia.EventLink.EventLinkMessageSerializer
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.Collections.Generic;
using System.IO;


namespace Microsoft.XMedia.EventLink
{
  public static class EventLinkMessageSerializer
  {
    private const uint CurrentProtocolVersion = 1;
    private const uint HeaderSignature = 3202006746;
    private const uint EmptyHMACLength = 20;
    internal const uint FixedHeadersLength = 48;
    private const uint ResponseHeadersLength = 8;
    private static readonly byte[] EmptyHMAC = new byte[new IntPtr(20)];

    public static int MaxAllowedMessageSize { get; set; }

    public static void SerializeMessages(
      Stream outputStream,
      IEnumerable<EventLinkMessage> messages,
      EventLinkSerializationFormat format = EventLinkSerializationFormat.Binary)
    {
      switch (format)
      {
        case EventLinkSerializationFormat.Text:
          using (DelimitedTextStreamWriter writer = new DelimitedTextStreamWriter(outputStream))
          {
            using (IEnumerator<EventLinkMessage> enumerator = messages.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                EventLinkMessage current = enumerator.Current;
                EventLinkMessageSerializer.SerializeMessage(writer, current);
              }
              break;
            }
          }
        case EventLinkSerializationFormat.Binary:
          using (BinaryMessageStreamWriter writer = new BinaryMessageStreamWriter(outputStream))
          {
            using (IEnumerator<EventLinkMessage> enumerator = messages.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                EventLinkMessage current = enumerator.Current;
                EventLinkMessageSerializer.SerializeMessage(writer, current);
              }
              break;
            }
          }
        default:
          throw new ArgumentException("Unsupported serialization format value", nameof (format));
      }
    }

    public static IList<EventLinkMessage> DeserializeMessages(
      Stream inputStream,
      long contentLength,
      uint from = 4294967295,
      EventLinkSerializationFormat format = EventLinkSerializationFormat.Binary)
    {
      List<EventLinkMessage> eventLinkMessageList = new List<EventLinkMessage>();
      switch (format)
      {
        case EventLinkSerializationFormat.Text:
          using (DelimitedTextStreamReader reader = new DelimitedTextStreamReader(inputStream))
          {
            while (!reader.EndOfStream)
              eventLinkMessageList.Add(EventLinkMessageSerializer.DeserializeMessage(reader, from));
            break;
          }
        case EventLinkSerializationFormat.Binary:
          using (BinaryMessageStreamReader reader = new BinaryMessageStreamReader(inputStream))
          {
            long bytesRead;
            for (; contentLength > 0L; contentLength -= bytesRead)
              eventLinkMessageList.Add(EventLinkMessageSerializer.DeserializeMessage(reader, from, out bytesRead));
            break;
          }
        default:
          throw new ArgumentException("Unsupported serialization format value", nameof (format));
      }
      return (IList<EventLinkMessage>) eventLinkMessageList;
    }

    private static EventLinkMessage DeserializeMessage(DelimitedTextStreamReader reader, uint from)
    {
      EventLinkMessage eventLinkMessage = new EventLinkMessage();
      eventLinkMessage.SequenceNumber = reader.ReadUInt32();
      eventLinkMessage.To = reader.ReadUInt32() == 1U ? reader.ReadUInt32() : throw new InvalidDataException("Unsupported protocol version");
      eventLinkMessage.From = reader.ReadUInt32();
      eventLinkMessage.MessageKind = reader.ReadUInt32();
      eventLinkMessage.MessageType = reader.ReadUInt32();
      eventLinkMessage.ResponseTo = reader.ReadUInt32();
      eventLinkMessage.ResponseCode = reader.ReadUInt32();
      eventLinkMessage.Tag = reader.ReadString();
      eventLinkMessage.Content = reader.ReadString();
      if (eventLinkMessage.From != uint.MaxValue)
        eventLinkMessage.From = from;
      return eventLinkMessage;
    }

    private static EventLinkMessage DeserializeMessage(
      BinaryMessageStreamReader reader,
      uint from,
      out long bytesRead)
    {
      try
      {
        EventLinkMessage eventLinkMessage = new EventLinkMessage();
        uint headerSignature = reader.ReadUInt32();
        if (headerSignature != 3202006746U)
          throw new InvalidDataException("Invalid header signature");
        uint messageLength = reader.ReadUInt32();
        if (EventLinkMessageSerializer.MaxAllowedMessageSize != 0 && (long) messageLength > (long) EventLinkMessageSerializer.MaxAllowedMessageSize)
          throw new InvalidDataException("Invalid message length");
        DuplicatingBinaryMessageStreamReader messageStreamReader = new DuplicatingBinaryMessageStreamReader(reader, headerSignature, messageLength);
        eventLinkMessage.SequenceNumber = messageStreamReader.ReadUInt32();
        eventLinkMessage.To = messageStreamReader.ReadUInt32() == 1U ? messageStreamReader.ReadUInt32() : throw new InvalidDataException("Unsupported protocol version");
        eventLinkMessage.From = messageStreamReader.ReadUInt32();
        eventLinkMessage.MessageKind = messageStreamReader.ReadUInt32();
        eventLinkMessage.MessageType = messageStreamReader.ReadUInt32();
        uint num = 48;
        if (eventLinkMessage.MessageKind == 2U)
        {
          eventLinkMessage.ResponseTo = messageStreamReader.ReadUInt32();
          eventLinkMessage.ResponseCode = messageStreamReader.ReadUInt32();
          num += 8U;
        }
        int length = (int) messageLength - (int) num;
        eventLinkMessage.ContentBytes = length >= 0 ? messageStreamReader.ReadBytes(length) : throw new InvalidDataException("Error reading message stream - bad message length");
        messageStreamReader.ReadBytes(EventLinkMessageSerializer.EmptyHMAC.Length);
        if (from != uint.MaxValue)
          eventLinkMessage.From = from;
        bytesRead = (long) (messageLength + 4U);
        eventLinkMessage.RawMessageBuffer = messageStreamReader.GetDuplicatedBuffer();
        return eventLinkMessage;
      }
      catch (EndOfStreamException ex)
      {
        throw new InvalidDataException("Error reading message stream", (Exception) ex);
      }
    }

    private static void SerializeMessage(DelimitedTextStreamWriter writer, EventLinkMessage message)
    {
      writer.Write(message.SequenceNumber);
      writer.Write(1U);
      writer.Write(message.To);
      writer.Write(message.From);
      writer.Write(message.MessageKind);
      writer.Write(message.MessageType);
      writer.Write(message.ResponseTo);
      writer.Write(message.ResponseCode);
      writer.Write(message.Tag);
      writer.Write(message.Content);
    }

    private static void SerializeMessage(BinaryMessageStreamWriter writer, EventLinkMessage message)
    {
      if (message.RawMessageBuffer != null && message.RawMessageBuffer.Length > 0)
      {
        writer.Write(message.RawMessageBuffer, 0, message.RawMessageBuffer.Length);
      }
      else
      {
        writer.Write(3202006746U);
        uint num = 48;
        if (message.MessageKind == 2U)
          num += 8U;
        writer.Write(num + (uint) message.ContentBytes.Length);
        writer.Write(message.SequenceNumber);
        writer.Write(1U);
        writer.Write(message.To);
        writer.Write(message.From);
        writer.Write(message.MessageKind);
        writer.Write(message.MessageType);
        if (message.MessageKind == 2U)
        {
          writer.Write(message.ResponseTo);
          writer.Write(message.ResponseCode);
        }
        writer.Write(message.ContentBytes, 0, message.ContentBytes.Length);
        writer.Write(EventLinkMessageSerializer.EmptyHMAC, 0, EventLinkMessageSerializer.EmptyHMAC.Length);
      }
    }
  }
}
