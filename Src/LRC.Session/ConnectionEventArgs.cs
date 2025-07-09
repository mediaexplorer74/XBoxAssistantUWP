// *********************************************************
// Type: LRC.Session.ConnectionEventArgs
// Assembly: LRC.Session, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D38BC875-BBEE-4348-AA20-8CADF3B9A3EB
// *********************************************************LRC.Session.dll

using System;


namespace LRC.Session
{
  public class ConnectionEventArgs : EventArgs
  {
    public Exception Error { get; set; }
  }
}
