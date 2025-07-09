// *********************************************************
// Type: Xbox.Live.Phone.Utils.AsyncResultNoResult
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;


namespace Xbox.Live.Phone.Utils
{
  public class AsyncResultNoResult : IAsyncResult
  {
    private const int StatePending = 0;
    private const int StateCompletedSynchronously = 1;
    private const int StateCompletedAsynchronously = 2;
    private readonly AsyncCallback asyncCallback;
    private readonly object asyncState;
    private volatile int completedState;
    private ManualResetEvent asyncWaitHandle;
    private Exception exception;

    public AsyncResultNoResult(AsyncCallback asyncCallback, object state)
    {
      this.asyncCallback = asyncCallback;
      this.asyncState = state;
    }

    public object AsyncState => this.asyncState;

    public bool CompletedSynchronously => this.completedState == 1;

    [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Event is closed when operation is completed")]
    public WaitHandle AsyncWaitHandle
    {
      get
      {
        if (this.asyncWaitHandle == null)
        {
          bool isCompleted = this.IsCompleted;
          ManualResetEvent manualResetEvent = new ManualResetEvent(isCompleted);
          if (Interlocked.CompareExchange<ManualResetEvent>(ref this.asyncWaitHandle, manualResetEvent, (ManualResetEvent) null) != null)
            manualResetEvent.Close();
          else if (!isCompleted && this.IsCompleted)
            this.asyncWaitHandle.Set();
        }
        return (WaitHandle) this.asyncWaitHandle;
      }
    }

    public bool IsCompleted => this.completedState != 0;

    public void SetAsCompleted(Exception exception, bool completedSynchronously)
    {
      this.exception = exception;
      if (Interlocked.Exchange(ref this.completedState, completedSynchronously ? 1 : 2) != 0)
        throw new InvalidOperationException("You can set a result only once");
      if (this.asyncWaitHandle != null)
        this.asyncWaitHandle.Set();
      if (this.asyncCallback == null)
        return;
      this.asyncCallback((IAsyncResult) this);
    }

    public void EndInvoke()
    {
      if (!this.IsCompleted)
      {
        this.AsyncWaitHandle.WaitOne();
        this.AsyncWaitHandle.Close();
        this.asyncWaitHandle = (ManualResetEvent) null;
      }
      if (this.exception != null)
        throw this.exception;
    }
  }
}
