// *********************************************************
// Type: LRC.ViewModel.LRCAsyncCompletedEventArgs
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using System;
using System.ComponentModel;


namespace LRC.ViewModel
{
  public class LRCAsyncCompletedEventArgs : AsyncCompletedEventArgs
  {
    public LRCAsyncCompletedEventArgs()
    {
    }

    public LRCAsyncCompletedEventArgs(object userObject)
      : base((Exception) null, false, userObject)
    {
    }

    public ErrorCodeEnum StatusCode { get; internal set; }

    public string MessageTitle { get; internal set; }

    public string MessageBody { get; internal set; }

    public ErrorSeverity Severity
    {
      get
      {
        if (this.StatusCode == ErrorCodeEnum.None)
          return ErrorSeverity.None;
        if (this.StatusCode <= ErrorCodeEnum.Error)
          return ErrorSeverity.Fatal;
        return this.StatusCode <= ErrorCodeEnum.Info ? ErrorSeverity.Error : ErrorSeverity.Info;
      }
    }

    public bool IsError => this.StatusCode != ErrorCodeEnum.None;
  }
}
