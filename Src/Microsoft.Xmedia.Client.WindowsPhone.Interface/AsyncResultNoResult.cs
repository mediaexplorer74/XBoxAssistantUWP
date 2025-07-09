// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.AsyncResultNoResult
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B6D5468-129C-4117-AE93-587315C3ADB1
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.Interface.dll

using System;
using System.Threading;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  internal class AsyncResultNoResult : IAsyncResult
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

    public object AsyncState => this.asyncState;

    public bool CompletedSynchronously => this.completedState == 1;

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
  }
}
