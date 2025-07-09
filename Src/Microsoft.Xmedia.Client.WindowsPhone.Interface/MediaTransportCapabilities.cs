// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.MediaTransportCapabilities
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B6D5468-129C-4117-AE93-587315C3ADB1
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.Interface.dll

using System;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  [Flags]
  public enum MediaTransportCapabilities
  {
    None = 0,
    CanStop = 1,
    CanPause = 2,
    CanRewind = 4,
    CanFastforward = 8,
    CanPlay = 16, // 0x00000010
    CanPlayPause = 32, // 0x00000020
    CanSkipForward = 64, // 0x00000040
    CanSkipBackward = 128, // 0x00000080
    CanSeek = 256, // 0x00000100
    IsLiveTransport = 512, // 0x00000200
  }
}
