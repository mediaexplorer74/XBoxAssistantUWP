// *********************************************************
// Type: Xbox.Live.Phone.Utils.HttpWebRequestRetryEventArgs
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Net;


namespace Xbox.Live.Phone.Utils
{
  public class HttpWebRequestRetryEventArgs : EventArgs
  {
    public HttpWebRequestRetryEventArgs(HttpWebResponse response, object asyncState)
    {
      this.Response = response;
      this.AsyncState = asyncState;
    }

    public HttpWebRequestRetryEventArgs(Exception exception, object asyncState)
    {
      this.Exception = exception;
      this.AsyncState = asyncState;
    }

    public HttpWebResponse Response { get; set; }

    public Exception Exception { get; set; }

    public object AsyncState { get; set; }
  }
}
