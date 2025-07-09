// *********************************************************
// Type: Xbox.Live.Phone.Services.ServiceProxyEventArgs`1
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.ComponentModel;


namespace Xbox.Live.Phone.Services
{
  public class ServiceProxyEventArgs<TReturnType> : AsyncCompletedEventArgs
  {
    private object result;

    public ServiceProxyEventArgs(
      object objectResult,
      Exception exception,
      bool cancelled,
      object userToken)
      : base(exception, cancelled, userToken)
    {
      this.result = objectResult;
    }

    public TReturnType Result
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return this.result != null && (object) this.result.GetType() == (object) typeof (TReturnType) ? (TReturnType) this.result : default (TReturnType);
      }
    }

    public bool ResultAvailable => this.Error == null && !this.Cancelled;
  }
}
