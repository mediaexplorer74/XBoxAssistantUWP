// *********************************************************
// Type: Xbox.Live.Phone.Services.FriendServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.Silverlight.XLiveWeb;
using System;
using System.Globalization;
using System.Net;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services
{
  public sealed class FriendServiceManager : IFriendServiceManager
  {
    private const string ComponentName = "FriendServiceManager";
    private const string AddFriendUri = "https://uds-part{0}.xboxlive.com/Friend.svc/add?gamertag={1}";
    private const string RemoveFriendUri = "https://uds-part{0}.xboxlive.com/Friend.svc/remove?gamertag={1}";
    private const string AcceptFriendREquestUri = "https://uds-part{0}.xboxlive.com/Friend.svc/accept?gamertag={1}";
    private const string DeclineFriendREquestUri = "https://uds-part{0}.xboxlive.com/Friend.svc/decline?gamertag={1}";
    private string currentEnvironmentPrefix = string.Empty;

    public event EventHandler<ServiceProxyEventArgs<string>> EventAddFriendCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventRemoveFriendCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventAcceptFriendRequestCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventDeclineFriendRequestCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventCancelFriendRequestCompleted;

    public WebHeaderCollection WebHeaders { get; set; }

    private static string PartnerToken
    {
      get => XboxLiveGamer.GetPartnerToken("http://xboxlive.com/userdata");
    }

    public void Initialize(ServiceCommon.Environment environmentName)
    {
      this.currentEnvironmentPrefix = ServiceCommon.GetEnvironmentUrlStringPrefix(environmentName);
      this.WebHeaders = ServiceCommon.CreateWebHeaders();
    }

    public void AddFriendAsync(string gamerTag)
    {
      if (string.IsNullOrEmpty(gamerTag))
        throw new ArgumentException("Invalid gamertag. can't be null");
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Friend.svc/add?gamertag={1}", new object[2]
        {
          (object) this.currentEnvironmentPrefix,
          (object) gamerTag
        })), HttpStack.PlatformDefault);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + FriendServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.ContentType = "application/xml";
        string empty = string.Empty;
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnAddFriendCompleted);
        httpClient.PostDataContractAsync<string, string>((object) empty);
      });
    }

    public void RemoveFriendAsync(string gamerTag)
    {
      if (string.IsNullOrEmpty(gamerTag))
        throw new ArgumentException("Invalid gamertag. can't be null");
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Friend.svc/remove?gamertag={1}", new object[2]
        {
          (object) this.currentEnvironmentPrefix,
          (object) gamerTag
        })), HttpStack.PlatformDefault);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + FriendServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.ContentType = "application/xml";
        string empty = string.Empty;
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnRemoveFriendCompleted);
        httpClient.PostDataContractAsync<string, string>((object) empty);
      });
    }

    public void AcceptFriendRequestAsync(string gamerTag)
    {
      if (string.IsNullOrEmpty(gamerTag))
        throw new ArgumentException("Invalid gamertag. can't be null");
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Friend.svc/accept?gamertag={1}", new object[2]
        {
          (object) this.currentEnvironmentPrefix,
          (object) gamerTag
        })), HttpStack.PlatformDefault);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + FriendServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.ContentType = "application/xml";
        string empty = string.Empty;
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnAcceptFriendRequestCompleted);
        httpClient.PostDataContractAsync<string, string>((object) empty);
      });
    }

    public void DeclineFriendRequestAsync(string gamerTag)
    {
      if (string.IsNullOrEmpty(gamerTag))
        throw new ArgumentException("Invalid gamertag. can't be null");
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Friend.svc/decline?gamertag={1}", new object[2]
        {
          (object) this.currentEnvironmentPrefix,
          (object) gamerTag
        })), HttpStack.PlatformDefault);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + FriendServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.ContentType = "application/xml";
        string empty = string.Empty;
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnDeclineFriendRequestCompleted);
        httpClient.PostDataContractAsync<string, string>((object) empty);
      });
    }

    public void CancelFriendRequestAsync(string gamerTag)
    {
      if (string.IsNullOrEmpty(gamerTag))
        throw new ArgumentException("Invalid gamertag. can't be null");
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Friend.svc/remove?gamertag={1}", new object[2]
        {
          (object) this.currentEnvironmentPrefix,
          (object) gamerTag
        })), HttpStack.PlatformDefault);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + FriendServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.ContentType = "application/xml";
        string empty = string.Empty;
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnCancelFriendRequestCompleted);
        httpClient.PostDataContractAsync<string, string>((object) empty);
      });
    }

    private void HttpClient_OnAddFriendCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<string>> addFriendCompleted = this.EventAddFriendCompleted;
      if (addFriendCompleted != null)
      {
        ServiceProxyEventArgs<string> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<string>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToAddFriend, "Failed to add friend", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<string>(e.Result, (Exception) null, false, (object) null);
        addFriendCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }

    private void HttpClient_OnRemoveFriendCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<string>> removeFriendCompleted = this.EventRemoveFriendCompleted;
      if (removeFriendCompleted != null)
      {
        ServiceProxyEventArgs<string> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<string>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToDeleteFriend, "Failed to remove friend", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<string>(e.Result, (Exception) null, false, (object) null);
        removeFriendCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }

    private void HttpClient_OnAcceptFriendRequestCompleted(
      object sender,
      XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<string>> requestCompleted = this.EventAcceptFriendRequestCompleted;
      if (requestCompleted != null)
      {
        ServiceProxyEventArgs<string> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<string>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToAcceptFriendRequest, "Failed to accept friend request", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<string>(e.Result, (Exception) null, false, (object) null);
        requestCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }

    private void HttpClient_OnDeclineFriendRequestCompleted(
      object sender,
      XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<string>> requestCompleted = this.EventDeclineFriendRequestCompleted;
      if (requestCompleted != null)
      {
        ServiceProxyEventArgs<string> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<string>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToDeclineFriendRequest, "Failed to decline friend request", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<string>(e.Result, (Exception) null, false, (object) null);
        requestCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }

    private void HttpClient_OnCancelFriendRequestCompleted(
      object sender,
      XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<string>> requestCompleted = this.EventCancelFriendRequestCompleted;
      if (requestCompleted != null)
      {
        ServiceProxyEventArgs<string> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<string>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToCancelFriendRequest, "Failed to cancel friend request", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<string>(e.Result, (Exception) null, false, (object) null);
        requestCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }
  }
}
