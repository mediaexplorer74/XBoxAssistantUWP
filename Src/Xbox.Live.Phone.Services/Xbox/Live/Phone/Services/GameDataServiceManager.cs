// *********************************************************
// Type: Xbox.Live.Phone.Services.GameDataServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Gds.Contracts;
using Leet.Silverlight.XLiveWeb;
using System;
using System.Globalization;
using System.Net;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services
{
  public sealed class GameDataServiceManager : IGameDataServiceManager
  {
    public const uint PageCount = 50;
    private const string GetGamesUriBase = "https://uds-part{0}.xboxlive.com/GameData.svc/games?";
    private const string GetAchievementsUriBase = "https://uds-part{0}.xboxlive.com/GameData.svc/achievements?";
    private const string GamerTagParameter = "&gamertags={0}";
    private const string GamerTagComparisonParameter = "&gamertags={0},{1}";
    private const string GameIdParameter = "&gameId={0}";
    private const string PageParameters = "&pageStart={0}&pageCount={1}";
    private static AtomicCounter ongoingRequestCounter = new AtomicCounter(0);
    private string getGamesUriString;
    private string getAchievementsUriString;
    private string currentEnvironmentPrefix = string.Empty;

    public event EventHandler<ServiceProxyEventArgs<Games>> EventGetGamesCompleted;

    public event EventHandler<ServiceProxyEventArgs<Achievements>> EventGetAchievementsCompleted;

    public static AtomicCounter OngoingRequestCounter
    {
      get => GameDataServiceManager.ongoingRequestCounter;
    }

    public WebHeaderCollection WebHeaders { get; set; }

    private static string PartnerToken
    {
      get => XboxLiveGamer.GetPartnerToken("http://xboxlive.com/userdata");
    }

    public void Initialize(ServiceCommon.Environment environmentName)
    {
      this.currentEnvironmentPrefix = ServiceCommon.GetEnvironmentUrlStringPrefix(environmentName);
      this.getGamesUriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/GameData.svc/games?", new object[1]
      {
        (object) this.currentEnvironmentPrefix
      });
      this.getAchievementsUriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/GameData.svc/achievements?", new object[1]
      {
        (object) this.currentEnvironmentPrefix
      });
    }

    public void GetGamesAsync(string gamerTag, uint pageNumber)
    {
      string fullUriString = this.getGamesUriString;
      GameDataServiceManager.OngoingRequestCounter.Plus(1);
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        if (string.IsNullOrEmpty(gamerTag))
        {
          // ISSUE: reference to a compiler-generated field
          this.fullUriString += string.Format((IFormatProvider) CultureInfo.InvariantCulture, "&gamertags={0}", new object[1]
          {
            (object) XboxLiveGamer.CurrentGamer.GamerTag
          });
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.fullUriString += string.Format((IFormatProvider) CultureInfo.InvariantCulture, "&gamertags={0},{1}", new object[2]
          {
            (object) XboxLiveGamer.CurrentGamer.GamerTag,
            (object) gamerTag
          });
        }
        // ISSUE: reference to a compiler-generated field
        this.fullUriString += string.Format((IFormatProvider) CultureInfo.InvariantCulture, "&pageStart={0}&pageCount={1}", new object[2]
        {
          (object) pageNumber,
          (object) 50U
        });
        GamesResponse gamesResponse = new GamesResponse(pageNumber);
        gamesResponse.EventGetGamesCompleted += this.EventGetGamesCompleted;
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(fullUriString), HttpStack.PlatformDefault);
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(gamesResponse.HttpClient_OnGetGamesCompleted);
        WebHeaderCollection webHeaders = ServiceCommon.CreateWebHeaders();
        webHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + GameDataServiceManager.PartnerToken;
        httpClient.Headers = webHeaders;
        httpClient.GetDataContractAsync<Games>();
      });
    }

    public void GetAchievementsAsync(string gamerTag, uint gameId)
    {
      string fullUriString = this.getAchievementsUriString;
      GameDataServiceManager.OngoingRequestCounter.Plus(1);
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        if (string.IsNullOrEmpty(gamerTag))
        {
          // ISSUE: reference to a compiler-generated field
          this.fullUriString += string.Format((IFormatProvider) CultureInfo.InvariantCulture, "&gamertags={0}", new object[1]
          {
            (object) XboxLiveGamer.CurrentGamer.GamerTag
          });
        }
        else if (gamerTag != XboxLiveGamer.CurrentGamer.GamerTag)
        {
          // ISSUE: reference to a compiler-generated field
          this.fullUriString += string.Format((IFormatProvider) CultureInfo.InvariantCulture, "&gamertags={0},{1}", new object[2]
          {
            (object) XboxLiveGamer.CurrentGamer.GamerTag,
            (object) gamerTag
          });
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.fullUriString += string.Format((IFormatProvider) CultureInfo.InvariantCulture, "&gamertags={0}", new object[1]
          {
            (object) gamerTag
          });
        }
        // ISSUE: reference to a compiler-generated field
        this.fullUriString += string.Format((IFormatProvider) CultureInfo.InvariantCulture, "&gameId={0}", new object[1]
        {
          (object) gameId.ToString((IFormatProvider) CultureInfo.InvariantCulture)
        });
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(fullUriString), HttpStack.PlatformDefault);
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnGetAchievementsCompleted);
        WebHeaderCollection webHeaders = ServiceCommon.CreateWebHeaders();
        webHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + GameDataServiceManager.PartnerToken;
        httpClient.Headers = webHeaders;
        httpClient.GetDataContractAsync<Achievements>();
      });
    }

    public bool IsLoading() => GameDataServiceManager.OngoingRequestCounter.Count > 0;

    private void HttpClient_OnGetAchievementsCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      GameDataServiceManager.OngoingRequestCounter.Subtract(1);
      EventHandler<ServiceProxyEventArgs<Achievements>> achievementsCompleted = this.EventGetAchievementsCompleted;
      if (achievementsCompleted != null)
      {
        ServiceProxyEventArgs<Achievements> e1;
        if (e.ResultAvailable)
        {
          if (!(e.Result is Achievements result) || result.UserAchievementsCollection == null || result.UserAchievementsCollection.Count < 1)
          {
            XLiveMobileException exception = XLiveMobileException.CreateException(5000, "The achievment data response is invalid. ", (string[]) null);
            exception.StatusCode = 20;
            e1 = new ServiceProxyEventArgs<Achievements>((object) null, (Exception) exception, false, (object) null);
          }
          else
            e1 = new ServiceProxyEventArgs<Achievements>((object) result, (Exception) null, false, (object) null);
        }
        else
          e1 = new ServiceProxyEventArgs<Achievements>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToGetAchievement, "Failed to get achievments", (string[]) null), false, (object) null);
        achievementsCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }
  }
}
