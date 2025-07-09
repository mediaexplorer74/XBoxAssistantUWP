// *********************************************************
// Type: Xbox.Live.Phone.Services.TitleHistory.TitleHistoryServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.Silverlight.XLiveWeb;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services.TitleHistory
{
  public sealed class TitleHistoryServiceManager : ITitleHistoryServiceManager
  {
    private const string ComponentName = "TitleHistoryServiceManager";
    private const string GetTitleHistoryUriTemplate = "https://services{0}.xboxlive.com/users/xuid({1})/history/titles";
    private string currentEnvironmentPrefix = string.Empty;

    public event EventHandler<ServiceProxyEventArgs<List<TitleHistoryInfo>>> EventGetTitleHistoryCompleted;

    private static string PartnerToken => XboxLiveGamer.GetPartnerToken("http://xboxlive.com");

    public void Initialize(ServiceCommon.Environment environmentName)
    {
      this.currentEnvironmentPrefix = ServiceCommon.GetEnvironmentUrlStringPrefix(environmentName);
    }

    public void GetTitleHistory()
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://services{0}.xboxlive.com/users/xuid({1})/history/titles", new object[2]
        {
          (object) this.currentEnvironmentPrefix,
          (object) XboxLiveGamer.CurrentGamer.XboxUserId
        }), UriKind.Absolute));
        httpClient.Headers = TitleHistoryServiceManager.GetWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GeTitleHistoryCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    private static WebHeaderCollection GetWebHeaders()
    {
      WebHeaderCollection webHeaders = new WebHeaderCollection();
      webHeaders["Authorization"] = "XBL2.0 x=" + TitleHistoryServiceManager.PartnerToken;
      webHeaders["Accept-Language"] = CultureInfo.CurrentUICulture.Name;
      webHeaders["x-xbl-contract-version"] = "1";
      webHeaders["Cache-Control"] = "no-store, no-cache, must-revalidate";
      webHeaders["PRAGMA"] = "no-cache";
      return webHeaders;
    }

    private void GeTitleHistoryCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GeTitleHistoryCompleted);
      if (e.Error == null)
      {
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          List<TitleHistoryInfo> listTitleHistory = TitleHistoryResponseParser.ParseGetTitleHistory(e.Result as string);
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            EventHandler<ServiceProxyEventArgs<List<TitleHistoryInfo>>> historyCompleted = this.EventGetTitleHistoryCompleted;
            if (historyCompleted == null)
              return;
            historyCompleted((object) this, new ServiceProxyEventArgs<List<TitleHistoryInfo>>((object) listTitleHistory, (Exception) null, false, (object) null));
          }, (object) this);
        });
      }
      else
      {
        EventHandler<ServiceProxyEventArgs<List<TitleHistoryInfo>>> historyCompleted = this.EventGetTitleHistoryCompleted;
        if (historyCompleted == null)
          return;
        historyCompleted((object) this, new ServiceProxyEventArgs<List<TitleHistoryInfo>>((object) null, e.Error, false, (object) null));
      }
    }
  }
}
