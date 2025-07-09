// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.WebClientsManager
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class WebClientsManager : IDisposable
  {
    private const int defaultMaxClients = 1;
    private Queue<WebClient> m_qWebClientsStorage = new Queue<WebClient>();
    private List<WebClient> m_usedWebClientsStorage = new List<WebClient>();
    private int m_maxWebClients;
    private bool m_stopDownloading;
    private readonly object m_mgrLock = new object();
    private AutoResetEvent m_mgrWaitEvent = new AutoResetEvent(false);
    private OpenReadCompletedEventHandler m_downloadOpenReadCompleted;
    private DownloadProgressChangedEventHandler m_downloadProgressEventHandler;
    private bool m_disposed;

    internal WebClientsManager() => this.MaxNumWebClients = 1;

    internal WebClientsManager(
      int maxNumWebClients,
      OpenReadCompletedEventHandler downloadOpenReadCompleted)
    {
      this.MaxNumWebClients = maxNumWebClients;
      this.m_downloadOpenReadCompleted = downloadOpenReadCompleted;
    }

    internal WebClientsManager(
      int maxNumWebClients,
      OpenReadCompletedEventHandler downloadOpenReadCompleted,
      DownloadProgressChangedEventHandler downloadProgressEventHandler)
    {
      this.MaxNumWebClients = maxNumWebClients;
      this.m_downloadOpenReadCompleted = downloadOpenReadCompleted;
      this.m_downloadProgressEventHandler = downloadProgressEventHandler;
    }

    internal int MaxNumWebClients
    {
      set
      {
        lock (this.m_mgrLock)
        {
          this.m_maxWebClients = value;
          while (this.m_usedWebClientsStorage.Count + this.m_qWebClientsStorage.Count > this.m_maxWebClients)
          {
            WebClient webClient = this.m_qWebClientsStorage.Dequeue();
            if (this.m_downloadOpenReadCompleted != null)
              webClient.OpenReadCompleted -= this.m_downloadOpenReadCompleted;
          }
        }
      }
      get => this.m_maxWebClients;
    }

    internal int NumDownloadingFiles => this.m_usedWebClientsStorage.Count;

    internal WebClient GetWebClient(bool waitForFreeClient)
    {
      lock (this.m_mgrLock)
      {
        if (this.m_qWebClientsStorage.Count > 0)
        {
          WebClient webClient = this.m_qWebClientsStorage.Dequeue();
          this.m_usedWebClientsStorage.Add(webClient);
          return webClient;
        }
        if (this.m_usedWebClientsStorage.Count < this.m_maxWebClients)
        {
          WebClient webClient = new WebClient();
          if (webClient != null)
          {
            this.m_usedWebClientsStorage.Add(webClient);
            if (this.m_downloadOpenReadCompleted != null)
              webClient.OpenReadCompleted += this.m_downloadOpenReadCompleted;
            if (this.m_downloadProgressEventHandler != null)
              webClient.DownloadProgressChanged += this.m_downloadProgressEventHandler;
          }
          return webClient;
        }
      }
      if (!waitForFreeClient)
        return (WebClient) null;
      while (!this.m_stopDownloading)
      {
        while (this.m_qWebClientsStorage.Count == 0)
        {
          this.m_mgrWaitEvent.WaitOne();
          if (this.m_stopDownloading)
          {
            this.m_stopDownloading = false;
            return (WebClient) null;
          }
        }
        lock (this.m_mgrLock)
        {
          if (this.m_qWebClientsStorage.Count > 0)
          {
            WebClient webClient = this.m_qWebClientsStorage.Dequeue();
            this.m_usedWebClientsStorage.Add(webClient);
            return webClient;
          }
        }
      }
      return (WebClient) null;
    }

    internal bool ReleaseWebClient(WebClient webClient)
    {
      lock (this.m_mgrLock)
      {
        bool flag = webClient != null ? this.m_usedWebClientsStorage.Remove(webClient) : throw new ArgumentNullException(nameof (webClient));
        if (this.m_usedWebClientsStorage.Count + this.m_qWebClientsStorage.Count > this.m_maxWebClients)
        {
          if (this.m_downloadOpenReadCompleted != null)
            webClient.OpenReadCompleted -= this.m_downloadOpenReadCompleted;
          if (this.m_downloadProgressEventHandler != null)
            webClient.DownloadProgressChanged -= this.m_downloadProgressEventHandler;
          if (this.m_usedWebClientsStorage.Count + this.m_qWebClientsStorage.Count == 0)
          {
            this.m_stopDownloading = true;
            this.m_mgrWaitEvent.Set();
          }
        }
        else
        {
          this.m_qWebClientsStorage.Enqueue(webClient);
          if (this.m_mgrWaitEvent != null)
            this.m_mgrWaitEvent.Set();
        }
        return flag;
      }
    }

    internal void CancelDownloading()
    {
      lock (this.m_mgrWaitEvent)
      {
        foreach (WebClient webClient in this.m_usedWebClientsStorage)
          webClient.CancelAsync();
      }
    }

    internal void StopDownloading()
    {
      this.CancelDownloading();
      this.m_stopDownloading = true;
      this.m_mgrWaitEvent.Set();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public void Dispose(bool disposing)
    {
      if (this.m_disposed)
        return;
      this.StopDownloading();
      if (!disposing)
        return;
      foreach (WebClient webClient in this.m_qWebClientsStorage)
      {
        if (this.m_downloadOpenReadCompleted != null)
          webClient.OpenReadCompleted -= this.m_downloadOpenReadCompleted;
        if (this.m_downloadProgressEventHandler != null)
          webClient.DownloadProgressChanged -= this.m_downloadProgressEventHandler;
      }
      this.m_qWebClientsStorage.Clear();
      foreach (WebClient webClient in this.m_usedWebClientsStorage)
      {
        if (this.m_downloadOpenReadCompleted != null)
          webClient.OpenReadCompleted -= this.m_downloadOpenReadCompleted;
        if (this.m_downloadProgressEventHandler != null)
          webClient.DownloadProgressChanged -= this.m_downloadProgressEventHandler;
      }
      this.m_usedWebClientsStorage.Clear();
      this.m_mgrWaitEvent.Close();
      this.m_mgrWaitEvent = (AutoResetEvent) null;
      this.m_disposed = true;
    }
  }
}
