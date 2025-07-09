// *********************************************************
// Type: Xbox.Live.Phone.Services.Leaderboards.LeaderboardsServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.Silverlight.XLiveWeb;
using System;
using System.Globalization;
using System.Net;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services.Leaderboards
{
  public sealed class LeaderboardsServiceManager : ILeaderboardsServiceManager
  {
    private const string ComponentName = "LeaderboardsServiceManager";
    private const string GetLeaderboardsForTitleUriTemplate = "https://services{0}.xboxlive.com/activity/titles/{1}/leaderboards";
    private const string GetLeaderboardDetailsUriTemplate = "https://services{0}.xboxlive.com/activity/titles/{1}/leaderboards/{2}/users/xuid({3})";
    private string currentEnvironmentPrefix = string.Empty;

    public event EventHandler<ServiceProxyEventArgs<LeaderboardsForTitleData>> EventGetLeaderboardsForTitleCompleted;

    public event EventHandler<ServiceProxyEventArgs<LeaderboardInfo>> EventGetLeaderboardDetailsCompleted;

    private static string PartnerToken => XboxLiveGamer.GetPartnerToken("http://xboxlive.com");

    public void Initialize(ServiceCommon.Environment environmentName)
    {
      this.currentEnvironmentPrefix = ServiceCommon.GetEnvironmentUrlStringPrefix(environmentName);
    }

    public void GetLeaderboardsForTitle(uint gameId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://services{0}.xboxlive.com/activity/titles/{1}/leaderboards", new object[2]
        {
          (object) this.currentEnvironmentPrefix,
          (object) gameId
        }), UriKind.Absolute));
        httpClient.Headers = LeaderboardsServiceManager.GetWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetLeaderboardsForTitleCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    public void GetLeaderboardDetails(uint gameId, uint leaderboardId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://services{0}.xboxlive.com/activity/titles/{1}/leaderboards/{2}/users/xuid({3})", (object) this.currentEnvironmentPrefix, (object) gameId, (object) leaderboardId, (object) XboxLiveGamer.CurrentGamer.XboxUserId), UriKind.Absolute));
        httpClient.Headers = LeaderboardsServiceManager.GetWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetLeaderboardDetailsCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    private static WebHeaderCollection GetWebHeaders()
    {
      WebHeaderCollection webHeaders = new WebHeaderCollection();
      webHeaders["Authorization"] = "XBL2.0 x=" + LeaderboardsServiceManager.PartnerToken;
      webHeaders["Accept-Language"] = CultureInfo.CurrentUICulture.Name;
      webHeaders["Cache-Control"] = "no-store, no-cache, must-revalidate";
      webHeaders["PRAGMA"] = "no-cache";
      return webHeaders;
    }

    private void GetLeaderboardsForTitleCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetLeaderboardsForTitleCompleted);
      if (e.Error == null)
      {
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          LeaderboardsForTitleData leaderboardsForTitleData = LeaderboardsResponseParser.ParseGetLeaderboardsForTitleResponse(e.Result as string);
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            EventHandler<ServiceProxyEventArgs<LeaderboardsForTitleData>> forTitleCompleted = this.EventGetLeaderboardsForTitleCompleted;
            if (forTitleCompleted == null)
              return;
            forTitleCompleted((object) this, new ServiceProxyEventArgs<LeaderboardsForTitleData>((object) leaderboardsForTitleData, (Exception) null, false, (object) null));
          }, (object) this);
        });
      }
      else
      {
        EventHandler<ServiceProxyEventArgs<LeaderboardsForTitleData>> forTitleCompleted = this.EventGetLeaderboardsForTitleCompleted;
        if (forTitleCompleted == null)
          return;
        forTitleCompleted((object) this, new ServiceProxyEventArgs<LeaderboardsForTitleData>((object) null, e.Error, false, (object) null));
      }
    }

    private void GetLeaderboardDetailsCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetLeaderboardDetailsCompleted);
      if (e.Error == null)
      {
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          LeaderboardInfo leaderboardInfo = LeaderboardsResponseParser.ParseGetLeaderboardDetailsResponse(e.Result as string);
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            EventHandler<ServiceProxyEventArgs<LeaderboardInfo>> detailsCompleted = this.EventGetLeaderboardDetailsCompleted;
            if (detailsCompleted == null)
              return;
            detailsCompleted((object) this, new ServiceProxyEventArgs<LeaderboardInfo>((object) leaderboardInfo, (Exception) null, false, (object) null));
          }, (object) this);
        });
      }
      else
      {
        EventHandler<ServiceProxyEventArgs<LeaderboardInfo>> detailsCompleted = this.EventGetLeaderboardDetailsCompleted;
        if (detailsCompleted == null)
          return;
        detailsCompleted((object) this, new ServiceProxyEventArgs<LeaderboardInfo>((object) null, e.Error, false, (object) null));
      }
    }
  }
}
