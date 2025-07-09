// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.IXmediaMessageHeaders
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B6D5468-129C-4117-AE93-587315C3ADB1
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.Interface.dll


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public interface IXmediaMessageHeaders
  {
    uint Id { get; }

    uint ResponseTo { get; }

    XmediaMessageType Type { get; }

    XmediaMessageKind Kind { get; }
  }
}
