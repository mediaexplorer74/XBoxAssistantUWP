// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.MediaStateUpdatedEventArgs
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B6D5468-129C-4117-AE93-587315C3ADB1
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.Interface.dll

using System;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public class MediaStateUpdatedEventArgs : EventArgs
  {
    public MediaStateUpdatedEventArgs(MediaState currentMediaState)
    {
      this.CurrentMediaState = currentMediaState;
    }

    public MediaState CurrentMediaState { get; set; }
  }
}
