// *********************************************************
// Type: LRC.Session.LRCMessage
// Assembly: LRC.Session, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D38BC875-BBEE-4348-AA20-8CADF3B9A3EB
// *********************************************************LRC.Session.dll

using LRC.LocalSubnet;
using Microsoft.Xmedia.Client.WindowsPhone;
using System;
using System.IO;


namespace LRC.Session
{
  public class LRCMessage : IXmediaMessage, IXmediaMessageHeaders
  {
    private const string ComponentName = "LRCMessage";
    private byte[] messageBuffer;
    private int messageSize;

    public LRCMessage(byte[] buffer, int size)
    {
      this.messageBuffer = buffer;
      this.messageSize = size;
    }

    public LRCMessage(uint messageType, object[] parameters)
    {
      this.MakeRequest(messageType, parameters);
    }

    public static uint CurrentSequenceNumber { get; set; }

    public static uint ConsoleClientId { get; set; }

    public static uint MyClientId { get; set; }

    public LRC_REQUEST CurrentRequest { get; private set; }

    public uint Id { get; private set; }

    public uint ResponseTo => uint.MaxValue;

    public XmediaMessageType Type => XmediaMessageType.ApplicationDefined;

    public XmediaMessageKind Kind => XmediaMessageKind.Request;

    public void MakeRequest(uint messageType, object[] parameters)
    {
      LRC_REQUEST request;
      switch (messageType)
      {
        case 1:
          request = (LRC_REQUEST) new LRC_GET_ACTIVE_TITLEID_REQUEST();
          break;
        case 2:
          if (parameters == null || parameters.Length != 2 || !(parameters[0] is uint) || parameters[1] != null && !(parameters[1] is string))
            throw new ArgumentException("parameters need two strings");
          LRC_LAUNCH_TITLE_REQUEST launchTitleRequest = new LRC_LAUNCH_TITLE_REQUEST();
          launchTitleRequest.Params.TitleId = (uint) parameters[0];
          if (parameters[1] != null)
          {
            launchTitleRequest.Params.LaunchParameter = (string) parameters[1];
            launchTitleRequest.Params.LaunchParameterLength = (uint) launchTitleRequest.Params.LaunchParameter.Length;
          }
          request = (LRC_REQUEST) launchTitleRequest;
          break;
        case 3:
          request = parameters != null && parameters.Length == 1 && parameters[0] is ControlKey ? (LRC_REQUEST) new LRC_SEND_INPUT_REQUEST()
          {
            Params = {
              VirtualKey = (uint) (ControlKey) parameters[0],
              ValidFields = 1U,
              SeekPos = 0UL
            }
          } : throw new ArgumentException("parameters need ControlKey");
          break;
        case 4:
          request = (LRC_REQUEST) new LRC_GET_MEDIA_TITLE_STATE_REQUEST();
          break;
        case 12:
          request = (LRC_REQUEST) new LRC_GET_CONSOLE_SETTINGS_REQUEST();
          break;
        case 2147483649:
          request = (LRC_REQUEST) new LRC_JOIN_SESSION_REQUEST();
          break;
        case 2147483650:
          request = (LRC_REQUEST) new LRC_LEAVE_SESSION_REQUEST();
          break;
        default:
          throw new ArgumentException("Invalid message type");
      }
      request.Header.SequenceNumber = LRCMessage.CurrentSequenceNumber;
      request.Header.FromClientId = LRCMessage.MyClientId;
      request.Header.ToClientId = LRCMessage.ConsoleClientId;
      request.Header.Signature = 3202006746U;
      this.messageBuffer = LRCMessageParser.CreateBinaryRequest(request);
      this.messageSize = this.messageBuffer.Length;
      this.CurrentRequest = request;
    }

    public byte[] GetUnEncryptedMessageBuffer()
    {
      Array.Resize<byte>(ref this.messageBuffer, (int) LRCCrypto.GetMessageLength(this.messageBuffer) + 4);
      return this.messageBuffer;
    }

    public byte[] GetEncryptedMessageBuffer() => this.messageBuffer;

    public uint GetSerializedSize() => (uint) this.messageSize;

    public void Serialize(BinaryWriter writer)
    {
      if (writer == null)
        throw new ArgumentException("writer is null");
      writer.Write(this.messageBuffer, 0, this.messageSize);
    }
  }
}
