// *********************************************************
// Type: Xbox.Live.Phone.Utils.TraceMessageEventArgs
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;


namespace Xbox.Live.Phone.Utils
{
  public class TraceMessageEventArgs : EventArgs
  {
    public TraceMessageEventArgs(string message) => this.TraceMessage = message;

    public string TraceMessage { get; private set; }
  }
}
