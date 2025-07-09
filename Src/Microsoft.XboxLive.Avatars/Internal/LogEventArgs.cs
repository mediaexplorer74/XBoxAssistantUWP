// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.LogEventArgs
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class LogEventArgs : EventArgs
  {
    public Log Result { get; set; }

    public LogEventArgs(Log result) => this.Result = result;
  }
}
