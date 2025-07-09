// *********************************************************
// Type: Xbox.Live.Phone.Services.MessageServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.MessageService.DataContracts;
using Leet.Silverlight.XLiveWeb;
using System;
using System.Globalization;
using System.Net;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services
{
  public sealed class MessageServiceManager : IMessageServiceManager
  {
    private const string ComponentName = "MessageServiceManager";
    private const string GetMessageSummaryUriTemplate = "https://uds-part{0}.xboxlive.com/Message.svc/summarylist?";
    private const string GetMessageSummaryWithHashUriTemplate = "https://uds-part{0}.xboxlive.com/Message.svc/summarylist?hashCode={1}";
    private const string GetMessageDetailsUriTemplate = "https://uds-part{0}.xboxlive.com/Message.svc/messagedetails?messageId={1}";
    private const string SendMessageUriTemplate = "https://uds-part{0}.xboxlive.com/Message.svc/send";
    private const string DeleteMessageUriTemplate = "https://uds-part{0}.xboxlive.com/Message.svc/delete?messageId={1}";
    private const string BlockUserUriTemplate = "https://uds-part{0}.xboxlive.com/Message.svc/block?messageId={1}";
    private static AtomicCounter ongoingRequestCounter = new AtomicCounter(0);
    private string currentEnvironmentPrefix = string.Empty;

    public event EventHandler<ServiceProxyEventArgs<MessageSummariesResponse>> EventGetMessageSummaryListCompleted;

    public event EventHandler<ServiceProxyEventArgs<MessageDetails>> EventGetOneMessageCompleted;

    public event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventDeleteMessageCompleted;

    public event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventSendMessageCompleted;

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

    public void GetMessageListAsync(string hashCode)
    {
      MessageServiceManager.ongoingRequestCounter.Plus(1);
      string fullUriString;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        if (!string.IsNullOrEmpty(hashCode))
          fullUriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Message.svc/summarylist?hashCode={1}", new object[2]
          {
            (object) this.currentEnvironmentPrefix,
            (object) hashCode
          });
        else
          fullUriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Message.svc/summarylist?", new object[1]
          {
            (object) this.currentEnvironmentPrefix
          });
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(fullUriString), HttpStack.PlatformDefault);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + MessageServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnGetMessageSummariesCompleted);
        httpClient.GetDataContractAsync<MessageSummariesResponse>();
      });
    }

    public void GetOneMessageAsync(uint messageId)
    {
      string fullUriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Message.svc/messagedetails?messageId={1}", new object[2]
      {
        (object) this.currentEnvironmentPrefix,
        (object) messageId
      });
      MessageServiceManager.ongoingRequestCounter.Plus(1);
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(fullUriString), HttpStack.PlatformDefault);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + MessageServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnGetMessageDetailsCompleted);
        httpClient.GetDataContractAsync<MessageDetails>();
      });
    }

    public void DeleteMessageAsync(uint messageId, bool blockUser)
    {
      string fullUriString;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        if (blockUser)
          fullUriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Message.svc/block?messageId={1}", new object[2]
          {
            (object) this.currentEnvironmentPrefix,
            (object) messageId
          });
        else
          fullUriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Message.svc/delete?messageId={1}", new object[2]
          {
            (object) this.currentEnvironmentPrefix,
            (object) messageId
          });
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(fullUriString), HttpStack.PlatformDefault);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + MessageServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnDeleteMessageCompleted);
        httpClient.DeleteDataContractAsync<ResponseWithErrorCode>((object) null);
      });
    }

    public void SendMessageAsync(SendMessageRequest messageToSend)
    {
      string fullUriString;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        fullUriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Message.svc/send", new object[1]
        {
          (object) this.currentEnvironmentPrefix
        });
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(fullUriString), HttpStack.PlatformDefault);
        httpClient.ContentType = "application/xml";
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + MessageServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnSendMessageCompleted);
        httpClient.PostDataContractAsync<SendMessageRequest, ResponseWithErrorCode>((object) messageToSend);
      });
    }

    public bool IsLoading() => MessageServiceManager.ongoingRequestCounter.Count > 0;

    private void HttpClient_OnGetMessageSummariesCompleted(
      object sender,
      XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<MessageSummariesResponse>> summaryListCompleted = this.EventGetMessageSummaryListCompleted;
      MessageServiceManager.ongoingRequestCounter.Subtract(1);
      Exception error = e.Error;
      if (summaryListCompleted != null)
      {
        ServiceProxyEventArgs<MessageSummariesResponse> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<MessageSummariesResponse>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToGetMessageSummary, "Failed to get message summary", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<MessageSummariesResponse>(e.Result, (Exception) null, false, (object) null);
        summaryListCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }

    private void HttpClient_OnGetMessageDetailsCompleted(
      object sender,
      XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<MessageDetails>> messageCompleted = this.EventGetOneMessageCompleted;
      MessageServiceManager.ongoingRequestCounter.Subtract(1);
      Exception error = e.Error;
      if (messageCompleted != null)
      {
        ServiceProxyEventArgs<MessageDetails> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<MessageDetails>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToGetMessage, "Failed to get message", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<MessageDetails>(e.Result, (Exception) null, false, (object) null);
        messageCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }

    private void HttpClient_OnSendMessageCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> messageCompleted = this.EventSendMessageCompleted;
      Exception error = e.Error;
      if (messageCompleted != null)
      {
        ServiceProxyEventArgs<ResponseWithErrorCode> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<ResponseWithErrorCode>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToSendMessage, "Failed to send message", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<ResponseWithErrorCode>(e.Result, (Exception) null, false, (object) null);
        messageCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }

    private void HttpClient_OnDeleteMessageCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> messageCompleted = this.EventDeleteMessageCompleted;
      Exception error = e.Error;
      if (messageCompleted != null)
      {
        ServiceProxyEventArgs<ResponseWithErrorCode> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<ResponseWithErrorCode>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToDeleteMessage, "Failed to delete message", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<ResponseWithErrorCode>(e.Result, (Exception) null, false, (object) null);
        messageCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }
  }
}
