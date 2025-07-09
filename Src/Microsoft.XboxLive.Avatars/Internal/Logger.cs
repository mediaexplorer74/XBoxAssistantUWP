// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Logger
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public static class Logger
  {
    public static event EventHandler<LogEventArgs> LogReceived;

    public static void Log(Microsoft.XboxLive.Avatars.Internal.Log log)
    {
      if (log is DebugLog)
        return;
      Logger.OnLogReceived(log.Sender, log);
    }

    public static void OnLogReceived(object sender, Microsoft.XboxLive.Avatars.Internal.Log log)
    {
      if (Logger.LogReceived == null)
        return;
      Logger.LogReceived(sender, new LogEventArgs(log));
    }
  }
}
