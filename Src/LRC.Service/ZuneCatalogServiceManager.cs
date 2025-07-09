// *********************************************************
// Type: LRC.Service.ZuneCatalogServiceManager
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System;
using System.Net;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace LRC.Service
{
  public class ZuneCatalogServiceManager : IZuneCatalogServiceManager
  {
    private const string ComponentName = "ZuneServiceManager";
    private const string ClientType = "clientType=Xbox360";

    public event EventHandler<ServiceProxyEventArgs<string>> EventGetFeedCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventGetVideoItemCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventGetMediaItemTypeCompleted;

    public void GetFeedList(string path, object userState)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        WebClient webClient = new WebClient();
        webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.DownloadFeedListCompleted);
        if (!string.IsNullOrWhiteSpace(path))
        {
          if (path.IndexOf("?", StringComparison.OrdinalIgnoreCase) != -1)
          {
            if (path.IndexOf("clientType=Xbox360", StringComparison.OrdinalIgnoreCase) == -1)
            {
              if (!path.EndsWith("?", StringComparison.OrdinalIgnoreCase))
              {
                // ISSUE: reference to a compiler-generated field
                this.path += "&";
              }
              // ISSUE: reference to a compiler-generated field
              this.path += "clientType=Xbox360";
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.path += "?clientType=Xbox360";
          }
        }
        string uriString = ZuneServiceUtil.BaseUri + path;
        webClient.DownloadStringAsync(new Uri(uriString), userState);
      });
    }

    public void GetMediaDetail(string mediaId, string mediaType, object userState)
    {
      if (mediaType == null)
        throw new ArgumentNullException(nameof (mediaType));
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        WebClient webClient = new WebClient();
        string uriString = ZuneServiceUtil.BaseUri + ZuneServiceUtil.GetMediaPath(mediaType, mediaId) + "?clientType=Xbox360";
        webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.DownloadVideoItemCompleted);
        webClient.DownloadStringAsync(new Uri(uriString), userState);
      });
    }

    public void GetMediaItemType(string mediaId, object userState)
    {
      if (string.IsNullOrEmpty(mediaId))
        throw new ArgumentNullException(nameof (mediaId));
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        WebClient webClient = new WebClient();
        webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.GetMediaItemTypeCompleted);
        string uriString = ZuneServiceUtil.BaseUri + "media/" + mediaId;
        webClient.DownloadStringAsync(new Uri(uriString), userState);
      });
    }

    private void DownloadFeedListCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is WebClient webClient)
        webClient.DownloadStringCompleted -= new DownloadStringCompletedEventHandler(this.DownloadFeedListCompleted);
      EventHandler<ServiceProxyEventArgs<string>> getFeedCompleted = this.EventGetFeedCompleted;
      if (getFeedCompleted == null || e == null)
        return;
      ServiceProxyEventArgs<string> e1 = e.Error == null ? new ServiceProxyEventArgs<string>((object) e.Result, (Exception) null, false, e.UserState) : new ServiceProxyEventArgs<string>((object) string.Empty, e.Error, false, e.UserState);
      getFeedCompleted((object) this, e1);
    }

    private void DownloadVideoItemCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is WebClient webClient)
        webClient.DownloadStringCompleted -= new DownloadStringCompletedEventHandler(this.DownloadVideoItemCompleted);
      EventHandler<ServiceProxyEventArgs<string>> videoItemCompleted = this.EventGetVideoItemCompleted;
      if (videoItemCompleted == null || e == null)
        return;
      ServiceProxyEventArgs<string> e1 = e.Error == null ? new ServiceProxyEventArgs<string>((object) CleanXmlUtil.TrimPrefixXml(e.Result), (Exception) null, false, e.UserState) : new ServiceProxyEventArgs<string>((object) string.Empty, e.Error, false, e.UserState);
      videoItemCompleted((object) this, e1);
    }

    private void GetMediaItemTypeCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is WebClient webClient)
        webClient.DownloadStringCompleted -= new DownloadStringCompletedEventHandler(this.GetMediaItemTypeCompleted);
      EventHandler<ServiceProxyEventArgs<string>> itemTypeCompleted = this.EventGetMediaItemTypeCompleted;
      if (itemTypeCompleted == null || e == null)
        return;
      ServiceProxyEventArgs<string> e1 = e.Error == null ? new ServiceProxyEventArgs<string>((object) e.Result, (Exception) null, false, e.UserState) : new ServiceProxyEventArgs<string>((object) string.Empty, e.Error, false, e.UserState);
      itemTypeCompleted((object) this, e1);
    }
  }
}
