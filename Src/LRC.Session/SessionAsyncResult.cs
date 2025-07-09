// *********************************************************
// Type: LRC.Session.SessionAsyncResult
// Assembly: LRC.Session, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D38BC875-BBEE-4348-AA20-8CADF3B9A3EB
// *********************************************************LRC.Session.dll

using System;
using System.Threading;


namespace LRC.Session
{
  public class SessionAsyncResult : IAsyncResult, IDisposable
  {
    private const string ComponentName = "SessionAsyncResult";
    private AsyncCallback callback;
    private object state;
    private volatile ManualResetEvent waitHandle;
    private object guard = new object();
    private volatile bool complete;

    public SessionAsyncResult(AsyncCallback callback, object state)
    {
      this.callback = callback;
      this.state = state;
      this.waitHandle = new ManualResetEvent(false);
    }

    public object AsyncState => this.state;

    public WaitHandle AsyncWaitHandle
    {
      get
      {
        if (this.waitHandle == null)
        {
          lock (this.guard)
          {
            if (this.waitHandle == null)
            {
              this.waitHandle = new ManualResetEvent(false);
              if (this.complete)
                this.waitHandle.Set();
            }
          }
        }
        return (WaitHandle) this.waitHandle;
      }
    }

    public bool CompletedSynchronously => false;

    public bool IsCompleted => this.complete;

    public object AsyncResult { get; set; }

    public Exception Error { get; set; }

    public void Complete()
    {
      this.complete = true;
      if (this.waitHandle != null)
      {
        try
        {
          this.waitHandle.Set();
        }
        catch (ObjectDisposedException ex)
        {
        }
      }
      if (this.callback == null)
        return;
      this.callback((IAsyncResult) this);
    }

    public void EndInvoke()
    {
      if (!this.IsCompleted)
      {
        this.AsyncWaitHandle.WaitOne();
        this.AsyncWaitHandle.Close();
        this.waitHandle = (ManualResetEvent) null;
      }
      if (this.Error != null)
        throw this.Error;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
      if (!isDisposing || this.waitHandle == null)
        return;
      this.waitHandle.Close();
      this.waitHandle = (ManualResetEvent) null;
    }
  }
}
