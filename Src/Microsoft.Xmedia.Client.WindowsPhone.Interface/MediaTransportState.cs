// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.MediaTransportState
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B6D5468-129C-4117-AE93-587315C3ADB1
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.Interface.dll


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public enum MediaTransportState
  {
    NoMedia = -1, // 0xFFFFFFFF
    Invalid = 0,
    Stopped = 1,
    Starting = 2,
    Playing = 3,
    Paused = 4,
    Buffering = 5,
  }
}
