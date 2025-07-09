// *********************************************************
// Type: Leet.Silverlight.XLiveWeb.XLiveWebHttpClientEventArgs
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.ComponentModel;


namespace Leet.Silverlight.XLiveWeb
{
  public class XLiveWebHttpClientEventArgs : AsyncCompletedEventArgs
  {
    public XLiveWebHttpClientEventArgs(
      object responseObject,
      Exception exception,
      bool cancelled,
      object userToken)
      : base(exception, cancelled, userToken)
    {
      this.ResponseObject = responseObject;
    }

    public object Result
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return this.ResponseObject;
      }
    }

    public bool ResultAvailable => this.Error == null && !this.Cancelled;

    private object ResponseObject { get; set; }
  }
}
