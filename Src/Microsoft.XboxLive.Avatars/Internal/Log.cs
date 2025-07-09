// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Log
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public abstract class Log
  {
    public object Sender { get; set; }

    public string Message { get; set; }

    public DateTime TimeStamp { get; private set; }

    public Log() => this.TimeStamp = DateTime.Now;

    public Log(object sender, string message)
    {
      this.TimeStamp = DateTime.Now;
      this.Sender = sender;
      this.Message = message;
    }
  }
}
