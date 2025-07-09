// *********************************************************
// Type: Microsoft.XMedia.EventLink.EventLinkMessage
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System.Text;


namespace Microsoft.XMedia.EventLink
{
  public class EventLinkMessage
  {
    public const uint BroadcastId = 4294967295;
    public const uint UnspecifiedId = 4294967295;

    public EventLinkMessage() => this.To = uint.MaxValue;

    public EventLinkMessage(EventLinkMessage other)
    {
      this.From = other.From;
      this.To = other.To;
      this.ResponseTo = other.ResponseTo;
      this.Tag = other.Tag;
      this.Content = other.Content;
    }

    internal uint Id { get; set; }

    internal bool IsSystemMessage => this.MessageKind >= 2048U && this.MessageKind < 4096U;

    public uint SequenceNumber { get; set; }

    public uint To { get; set; }

    public uint From { get; internal set; }

    public uint MessageKind { get; set; }

    public uint MessageType { get; set; }

    public uint ResponseTo { get; set; }

    public uint ResponseCode { get; set; }

    public string Tag { get; set; }

    public string Content
    {
      get => Encoding.UTF8.GetString(this.ContentBytes, 0, this.ContentBytes.Length);
      set => this.ContentBytes = Encoding.UTF8.GetBytes(value);
    }

    public byte[] ContentBytes { get; set; }

    public byte[] RawMessageBuffer { get; set; }
  }
}
