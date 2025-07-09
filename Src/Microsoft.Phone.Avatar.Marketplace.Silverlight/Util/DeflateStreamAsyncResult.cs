// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.DeflateStreamAsyncResult
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.Threading;


namespace Microsoft.Phone.Marketplace.Util
{
  internal class DeflateStreamAsyncResult : IAsyncResult
  {
    public byte[] buffer;
    public int offset;
    public int count;
    public bool isWrite;
    private object m_AsyncObject;
    private object m_AsyncState;
    private AsyncCallback m_AsyncCallback;
    private object m_Result;
    internal bool m_CompletedSynchronously;
    private int m_InvokedCallback;
    private int m_Completed;
    private object m_Event;

    public DeflateStreamAsyncResult(
      object asyncObject,
      object asyncState,
      AsyncCallback asyncCallback,
      byte[] buffer,
      int offset,
      int count)
    {
      this.buffer = buffer;
      this.offset = offset;
      this.count = count;
      this.m_CompletedSynchronously = true;
      this.m_AsyncObject = asyncObject;
      this.m_AsyncState = asyncState;
      this.m_AsyncCallback = asyncCallback;
    }

    public object AsyncState => this.m_AsyncState;

    public WaitHandle AsyncWaitHandle
    {
      get
      {
        int completed = this.m_Completed;
        if (this.m_Event == null)
          Interlocked.CompareExchange(ref this.m_Event, (object) new ManualResetEvent(completed != 0), (object) null);
        ManualResetEvent asyncWaitHandle = (ManualResetEvent) this.m_Event;
        if (completed == 0 && this.m_Completed != 0)
          asyncWaitHandle.Set();
        return (WaitHandle) asyncWaitHandle;
      }
    }

    public bool CompletedSynchronously => this.m_CompletedSynchronously;

    public bool IsCompleted => this.m_Completed != 0;

    internal object Result => this.m_Result;

    internal void Close()
    {
      if (this.m_Event == null)
        return;
      ((WaitHandle) this.m_Event).Close();
    }

    internal void InvokeCallback(bool completedSynchronously, object result)
    {
      this.Complete(completedSynchronously, result);
    }

    internal void InvokeCallback(object result) => this.Complete(result);

    private void Complete(bool completedSynchronously, object result)
    {
      this.m_CompletedSynchronously = completedSynchronously;
      this.Complete(result);
    }

    private void Complete(object result)
    {
      this.m_Result = result;
      Interlocked.Increment(ref this.m_Completed);
      if (this.m_Event != null)
        ((EventWaitHandle) this.m_Event).Set();
      if (Interlocked.Increment(ref this.m_InvokedCallback) != 1 || this.m_AsyncCallback == null)
        return;
      this.m_AsyncCallback((IAsyncResult) this);
    }
  }
}
