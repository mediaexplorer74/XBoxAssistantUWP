// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.XmediaMessageReceivedEventArgs
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B6D5468-129C-4117-AE93-587315C3ADB1
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.Interface.dll

using System;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public class XmediaMessageReceivedEventArgs : EventArgs
  {
    public XmediaMessageReceivedEventArgs(ReceivedXmediaMessage message) => this.Message = message;

    public ReceivedXmediaMessage Message { get; set; }
  }
}
