// *********************************************************
// Type: Xbox.Live.Phone.Utils.AsyncWithResult
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;


namespace Xbox.Live.Phone.Utils
{
  public class AsyncWithResult(AsyncCallback asyncCallback, object state) : AsyncResultNoResult(asyncCallback, state)
  {
    private object result;

    public object AsyncResult => this.result;

    public void SetAsCompleted(object result, Exception ex, bool completedSynchronously)
    {
      this.result = result;
      this.SetAsCompleted(ex, completedSynchronously);
    }

    public object EndInvoke()
    {
      base.EndInvoke();
      return this.result;
    }
  }
}
