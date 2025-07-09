// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.ReceivedXmediaMessage
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B6D5468-129C-4117-AE93-587315C3ADB1
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.Interface.dll


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public class ReceivedXmediaMessage : IXmediaMessageHeaders
  {
    public byte[] Contents;
    public byte[] RawMessageBuffer;

    internal ReceivedXmediaMessage(
      uint id,
      uint responseTo,
      XmediaMessageType type,
      XmediaMessageKind kind,
      byte[] contents)
    {
      this.Id = id;
      this.ResponseTo = responseTo;
      this.Type = type;
      this.Kind = kind;
      this.Contents = contents;
    }

    public uint Id { get; private set; }

    public uint ResponseTo { get; private set; }

    public XmediaMessageType Type { get; private set; }

    public XmediaMessageKind Kind { get; private set; }
  }
}
