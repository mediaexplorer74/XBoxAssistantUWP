// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.AsyncResult`1
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B6D5468-129C-4117-AE93-587315C3ADB1
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.Interface.dll

using System;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  internal class AsyncResult<TResult>(AsyncCallback asyncCallback, object state) : 
    AsyncResultNoResult(asyncCallback, state)
  {
    private TResult result = default (TResult);

    public void SetAsCompleted(TResult result, bool completedSynchronously)
    {
      this.result = result;
      this.SetAsCompleted((Exception) null, completedSynchronously);
    }

    public TResult EndInvoke()
    {
      base.EndInvoke();
      return this.result;
    }
  }
}
