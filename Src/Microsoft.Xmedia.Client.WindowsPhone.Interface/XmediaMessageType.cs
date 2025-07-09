// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.XmediaMessageType
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B6D5468-129C-4117-AE93-587315C3ADB1
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.Interface.dll


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public enum XmediaMessageType
  {
    JoinSession = -2147483647, // 0x80000001
    LeaveSession = -2147483646, // 0x80000002
    GetActiveTitleId = 1,
    LaunchTitle = 2,
    SendInput = 3,
    GetMediaAndTitleState = 4,
    NonMediaTitleStateNotification = 5,
    MediaTitleStateNotification = 6,
    ApplicationDefined = 7,
  }
}
