// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.DataManagerBase
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.Collections.Generic;
using System.Threading;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public abstract class DataManagerBase
  {
    protected int m_numProcessingRequests;
    private Thread m_workerThread;
    private bool m_quitWorkerThread;
    private AutoResetEvent m_newRequestEvent = new AutoResetEvent(false);
    private AutoResetEvent m_workerWaitExitEvent = new AutoResetEvent(false);
    private readonly object m_requestQueueLock = new object();
    private Queue<DataRequest> m_qNormalPriorityRequests = new Queue<DataRequest>();
    private Queue<DataRequest> m_qHighPriorityRequests = new Queue<DataRequest>();
    private List<DataRequest> m_processingRequests = new List<DataRequest>();

    public event DownloadProgressEventHandler DownloadProgress;

    protected int NumberFilesToDownload => this.m_numProcessingRequests;

    protected void InitializeThread() => this.CreateWorkerThread();

    public void EnqueueRequest(DataRequest request, DownloadPriority priority)
    {
      if (request == null)
        throw new ArgumentNullException(nameof (request));
      if (request.DataSource == null || request.Handler == null)
        throw new ArgumentException(nameof (request), "Invalid request parameter");
      lock (this.m_requestQueueLock)
      {
        if (priority == DownloadPriority.High)
          this.m_qHighPriorityRequests.Enqueue(request);
        else
          this.m_qNormalPriorityRequests.Enqueue(request);
      }
      this.PropagateProgressEvent();
      this.m_newRequestEvent.Set();
    }

    private DataRequest DequeueRequest()
    {
      DataRequest dataRequest = (DataRequest) null;
      lock (this.m_requestQueueLock)
      {
        if (this.m_qHighPriorityRequests.Count > 0)
          dataRequest = this.m_qHighPriorityRequests.Dequeue();
        else if (this.m_qNormalPriorityRequests.Count > 0)
          dataRequest = this.m_qNormalPriorityRequests.Dequeue();
      }
      return dataRequest;
    }

    private bool IsRequest()
    {
      bool flag = false;
      lock (this.m_requestQueueLock)
      {
        if (this.m_qHighPriorityRequests.Count > 0)
          flag = true;
        else if (this.m_qNormalPriorityRequests.Count > 0)
          flag = true;
      }
      return flag;
    }

    private bool IsRequestProcessing(DataRequest dataRequest)
    {
      bool flag = false;
      lock (this.m_requestQueueLock)
      {
        for (int index = 0; index < this.m_processingRequests.Count; ++index)
        {
          if (this.m_processingRequests[index].Equals((object) dataRequest))
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    private void ProcessRequest(DataRequest request)
    {
      lock (this.m_requestQueueLock)
      {
        if (!this.IsRequestProcessing(request))
          this.m_processingRequests.Add(request);
      }
      request.DataSource.ProccessRequest(request, this);
    }

    internal void RequestProcessed(
      DataRequestCompletedEventArgs eventArguments,
      DataRequest dataRequest)
    {
      lock (this.m_requestQueueLock)
      {
        for (int index = 0; index < this.m_processingRequests.Count; ++index)
        {
          if (this.m_processingRequests[index].Equals((object) dataRequest))
          {
            this.m_processingRequests.RemoveAt(index);
            break;
          }
        }
      }
      this.OnFinishRequest(eventArguments, dataRequest);
      this.PropagateProgressEvent();
    }

    public abstract void OnFinishRequest(
      DataRequestCompletedEventArgs eventArguments,
      DataRequest dataRequest);

    private void DataManagerThreadProc()
    {
      while (!this.m_quitWorkerThread)
      {
        this.m_newRequestEvent.WaitOne();
        if (!this.m_quitWorkerThread)
        {
          DataRequest request = this.DequeueRequest();
          if (request != null)
            this.ProcessRequest(request);
          lock (this.m_requestQueueLock)
          {
            if (this.IsRequest())
              this.m_newRequestEvent.Set();
          }
        }
        else
          break;
      }
      this.CancelAllRequests();
    }

    private void CreateWorkerThread()
    {
      if (this.m_workerThread != null)
        return;
      this.m_quitWorkerThread = false;
      this.m_workerThread = new Thread(new ThreadStart(this.DataManagerThreadProc));
      this.m_workerThread.IsBackground = true;
      this.m_workerThread.Name = "DataManagerWorker";
      this.m_workerThread.Start();
    }

    private void PropagateProgressEvent()
    {
      DownloadProgressEventHandler downloadProgress = this.DownloadProgress;
      if (downloadProgress == null)
        return;
      DownloadProgressEventArgs e = new DownloadProgressEventArgs();
      lock (this.m_requestQueueLock)
      {
        e.NumberOfDownloadingFiles = this.m_processingRequests.Count;
        e.TotalNumberOfFilesToDownload = this.m_qHighPriorityRequests.Count + this.m_qNormalPriorityRequests.Count;
      }
      downloadProgress((object) this, e);
    }

    private void CancelAllRequests()
    {
      lock (this.m_requestQueueLock)
      {
        foreach (DataRequest highPriorityRequest in this.m_qHighPriorityRequests)
        {
          if (highPriorityRequest.Handler != null)
            highPriorityRequest.Handler((object) null, new DataRequestCompletedEventArgs((Exception) null, true, highPriorityRequest.Context));
        }
        this.m_qHighPriorityRequests.Clear();
        foreach (DataRequest normalPriorityRequest in this.m_qNormalPriorityRequests)
        {
          if (normalPriorityRequest.Handler != null)
            normalPriorityRequest.Handler((object) null, new DataRequestCompletedEventArgs((Exception) null, true, normalPriorityRequest.Context));
        }
        this.m_qNormalPriorityRequests.Clear();
        foreach (DataRequest processingRequest in this.m_processingRequests)
        {
          processingRequest.DataSource.CancelRequest();
          if (processingRequest.Handler != null)
            processingRequest.Handler((object) null, new DataRequestCompletedEventArgs((Exception) null, true, processingRequest.Context));
        }
        this.m_processingRequests.Clear();
      }
    }

    private void QuitWorkerThread()
    {
      this.m_quitWorkerThread = true;
      this.m_newRequestEvent.Set();
      if (this.m_workerThread != null && this.m_workerThread.IsAlive)
        this.m_workerThread.Join();
      this.m_workerThread = (Thread) null;
    }

    public void Cleanup() => this.QuitWorkerThread();
  }
}
