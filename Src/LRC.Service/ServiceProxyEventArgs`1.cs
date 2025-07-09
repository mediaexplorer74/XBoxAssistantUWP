// *********************************************************
// Type: LRC.Service.ServiceProxyEventArgs`1
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System;
using System.ComponentModel;


namespace LRC.Service
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
        return this.result != null && this.result is TReturnType ? (TReturnType) this.result : default (TReturnType);
      }
    }
  }
}
