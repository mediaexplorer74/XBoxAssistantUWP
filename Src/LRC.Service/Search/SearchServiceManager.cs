// *********************************************************
// Type: LRC.Service.Search.SearchServiceManager
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using Leet.Silverlight.XLiveWeb;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
using Xbox.Live.Phone;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Utils;


namespace LRC.Service.Search
{
  public class SearchServiceManager : ISearchServiceManager
  {
    private const string ComponentName = "SearchServiceManager";
    private const string AudienceUri = "http://xboxlive.com";

    public SearchServiceManager() => EdsConstants.Initialize();

    public event EventHandler<LRC.Service.ServiceProxyEventArgs<SearchResults>> EventSearchCompleted;

    public event EventHandler<LRC.Service.ServiceProxyEventArgs<List<string>>> EventGetSearchTermsCompleted;

    public event EventHandler<LRC.Service.ServiceProxyEventArgs<List<SearchData>>> EventGetRelatedItemsCompleted;

    public event EventHandler<LRC.Service.ServiceProxyEventArgs<List<SearchData>>> EventGetRecommendedItemsCompleted;

    public event EventHandler<LRC.Service.ServiceProxyEventArgs<MovieData>> EventGetMovieDetailsCompleted;

    public event EventHandler<LRC.Service.ServiceProxyEventArgs<XboxAppData>> EventGetAppDetailsCompleted;

    public event EventHandler<LRC.Service.ServiceProxyEventArgs<TvSeriesData>> EventGetTvSeasonsForSeriesCompleted;

    public event EventHandler<LRC.Service.ServiceProxyEventArgs<TvSeasonData>> EventGetTvEpisodesForSeasonCompleted;

    public event EventHandler<LRC.Service.ServiceProxyEventArgs<TvEpisodeData>> EventGetTvEpisodeDetailsCompleted;

    public event EventHandler<LRC.Service.ServiceProxyEventArgs<XboxGameData>> EventGetNowPlayingAppDetailsCompleted;

    public event EventHandler<LRC.Service.ServiceProxyEventArgs<SearchData>> EventGetNowPlayingItemDetailsCompleted;

    public event EventHandler<LRC.Service.ServiceProxyEventArgs<XboxGameData>> EventGetGameDetailsCompleted;

    public void Search(
      string searchTerm,
      Filter filters,
      string continuationToken,
      uint resultsPerPage,
      uint consoleLiveTVtitleId,
      object userState)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        string searchUri = EdsConstants.GetSearchUri(searchTerm, filters, continuationToken, resultsPerPage, consoleLiveTVtitleId);
        if (!string.IsNullOrEmpty(searchUri))
        {
          XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(searchUri, UriKind.Absolute), userState);
          httpClient.Headers = SearchServiceManager.GetEdsWebHeaders();
          httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.SearchCompleted);
          httpClient.TimeoutInMilliseconds = 30000;
          httpClient.GetDataContractAsync<string>();
        }
        else
          this.FireEvent<SearchResults>(this.EventSearchCompleted, new SearchResults(), (Exception) null, userState);
      });
    }

    public void GetSearchTerms(object userState)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(EdsConstants.SearchTermsUri, UriKind.Absolute), userState);
        httpClient.Headers = SearchServiceManager.GetEdsWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetSearchTermsCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    public void GetRelatedItems(
      string itemId,
      string itemType,
      bool useZuneOnly,
      uint consoleLiveTVtitleId,
      object userState)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        string relatedItemsUri = EdsConstants.GetRelatedItemsUri(itemId, itemType, useZuneOnly, consoleLiveTVtitleId);
        if (string.IsNullOrEmpty(relatedItemsUri))
        {
          this.FireEvent<List<SearchData>>(this.EventGetRelatedItemsCompleted, new List<SearchData>(), (Exception) null, userState);
        }
        else
        {
          XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(relatedItemsUri, UriKind.Absolute), userState);
          httpClient.Headers = SearchServiceManager.GetEdsWebHeaders();
          httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetRelatedItemsCompleted);
          httpClient.TimeoutInMilliseconds = 30000;
          httpClient.GetDataContractAsync<string>();
        }
      });
    }

    public void GetMovieDetails(
      string id,
      string movieName,
      string detailsUrl,
      uint consoleLiveTVtitleId,
      object userState)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        Uri targetUri = (Uri) null;
        if (!string.IsNullOrEmpty(detailsUrl))
          targetUri = SearchServiceManager.CreateUri(EdsConstants.EdsBaseUri + detailsUrl + EdsConstants.GetConsoleProvidersParam(consoleLiveTVtitleId));
        if (targetUri == (Uri) null)
          targetUri = new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, EdsConstants.MovieDetailsBaseUri, new object[2]
          {
            (object) id,
            (object) movieName
          }) + EdsConstants.GetConsoleProvidersParam(consoleLiveTVtitleId), UriKind.Absolute);
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(targetUri, userState);
        httpClient.Headers = SearchServiceManager.GetEdsWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetMovieDetailsCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    public void GetAppDetails(string id, string appName, string detailsUrl, object userState)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, GameMarketplaceConstants.DetailsByMediaIdBaseUri, new object[2]
        {
          (object) XboxLiveGamer.CurrentGamer.LegalLocale,
          (object) id
        }), UriKind.Absolute), userState);
        httpClient.Headers = SearchServiceManager.GetEdsWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetAppDetailsCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    public void GetTvSeasonsForSeries(
      string seriesId,
      string continuationToken,
      uint resultsPerPage,
      string seriesName,
      string detailsUrl,
      uint consoleLiveTVtitleId,
      object userState)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        Uri targetUri = EdsConstants.GetPaginationUri(detailsUrl, continuationToken, resultsPerPage, consoleLiveTVtitleId);
        if (targetUri == (Uri) null)
          targetUri = new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, EdsConstants.TvSeriesDetailsBaseUri, new object[3]
          {
            (object) seriesId,
            (object) continuationToken,
            (object) resultsPerPage
          }) + EdsConstants.GetConsoleProvidersParam(consoleLiveTVtitleId), UriKind.Absolute);
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(targetUri, userState);
        httpClient.Headers = SearchServiceManager.GetEdsWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetTvSeasonsForSeriesCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    public void GetTvEpisodesForSeason(
      string seasonId,
      string continuationToken,
      uint resultsPerPage,
      string seasonName,
      string detailsUrl,
      uint consoleLiveTVtitleId,
      object userState)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        Uri targetUri = EdsConstants.GetPaginationUri(detailsUrl, continuationToken, resultsPerPage, consoleLiveTVtitleId);
        if (targetUri == (Uri) null)
          targetUri = new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, EdsConstants.TvSeasonDetailsBaseUri, new object[3]
          {
            (object) seasonId,
            (object) continuationToken,
            (object) resultsPerPage
          }) + EdsConstants.GetConsoleProvidersParam(consoleLiveTVtitleId), UriKind.Absolute);
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(targetUri, userState);
        httpClient.Headers = SearchServiceManager.GetEdsWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetTvEpisodesForSeasonCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    public void GetTvEpisodeDetails(
      string episodeId,
      string episodeName,
      string detailsUrl,
      uint consoleLiveTVtitleId,
      object userState)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        Uri targetUri = (Uri) null;
        if (!string.IsNullOrEmpty(detailsUrl))
          targetUri = SearchServiceManager.CreateUri(EdsConstants.EdsBaseUri + detailsUrl + EdsConstants.GetConsoleProvidersParam(consoleLiveTVtitleId));
        if (targetUri == (Uri) null)
          targetUri = new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, EdsConstants.TvEpisodeDetailsBaseUri, new object[2]
          {
            (object) episodeId,
            (object) episodeName
          }) + EdsConstants.GetConsoleProvidersParam(consoleLiveTVtitleId), UriKind.Absolute);
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(targetUri, userState);
        httpClient.Headers = SearchServiceManager.GetEdsWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetTvEpisodeDetailsCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    public void GetNowPlayingAppDetails(uint titleId, object userState)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, GameMarketplaceConstants.GameDetailsByTitleIdBaseUri, new object[2]
        {
          (object) XboxLiveGamer.CurrentGamer.LegalLocale,
          (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, "0x{0:X}", new object[1]
          {
            (object) titleId
          })
        }), UriKind.Absolute), userState);
        httpClient.Headers = SearchServiceManager.GetEdsWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetNowPlayingAppDetailsCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    public void GetNowPlayingItemDetails(
      uint titleId,
      string partnerContentId,
      uint consoleLiveTVtitleId,
      object userState)
    {
      if (string.IsNullOrEmpty(partnerContentId))
        this.FireEvent<SearchData>(this.EventGetNowPlayingItemDetailsCompleted, (SearchData) null, (Exception) new ArgumentNullException(nameof (partnerContentId)), userState);
      else
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, EdsConstants.NowPlayingItemDetailsBaseUri, new object[2]
          {
            (object) EdsConstants.GetFormattedTitleId(titleId),
            (object) Uri.EscapeDataString(Uri.UnescapeDataString(partnerContentId))
          }) + EdsConstants.GetConsoleProvidersParam(consoleLiveTVtitleId), UriKind.Absolute), userState);
          httpClient.Headers = SearchServiceManager.GetEdsWebHeaders();
          httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetNowPlayingItemDetailsCompleted);
          httpClient.TimeoutInMilliseconds = 30000;
          httpClient.GetDataContractAsync<string>();
        });
    }

    public void GetGameDetailsByMediaId(string mediaId, object userState)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, GameMarketplaceConstants.DetailsByMediaIdBaseUri, new object[2]
        {
          (object) XboxLiveGamer.CurrentGamer.LegalLocale,
          (object) mediaId
        }), UriKind.Absolute), userState);
        httpClient.Headers = SearchServiceManager.GetEdsWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetGameDetailCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    public void GetGameDetailsByTitleId(uint titleId, object userState)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, GameMarketplaceConstants.GameDetailsByTitleIdBaseUri, new object[2]
        {
          (object) XboxLiveGamer.CurrentGamer.LegalLocale,
          (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, "0x{0:X}", new object[1]
          {
            (object) titleId
          })
        }), UriKind.Absolute), userState);
        httpClient.Headers = SearchServiceManager.GetEdsWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetGameDetailCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    public void GetRecommendedItems(Filter itemFilter, uint consoleLiveTVtitleId, object userState)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        string uriString = (string) null;
        if (itemFilter == Filter.Movies)
          uriString = EdsConstants.MovieRecommendationBaseUri + EdsConstants.GetConsoleProvidersParam(consoleLiveTVtitleId);
        if (!string.IsNullOrEmpty(uriString))
        {
          XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(uriString, UriKind.Absolute), userState);
          httpClient.Headers = SearchServiceManager.GetEdsWebHeaders();
          httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetRecommendedItemsCompleted);
          httpClient.TimeoutInMilliseconds = 30000;
          httpClient.GetDataContractAsync<string>();
        }
        else
          this.FireEvent<List<SearchData>>(this.EventGetRecommendedItemsCompleted, new List<SearchData>(), (Exception) null, userState);
      });
    }

    private static WebHeaderCollection GetEdsWebHeaders()
    {
      WebHeaderCollection edsWebHeaders = new WebHeaderCollection();
      edsWebHeaders["PRAGMA"] = "no-cache";
      edsWebHeaders["x-xbl-contract-version"] = "1";
      if (EnvironmentState.Instance.IsProduction)
        edsWebHeaders["Authorization"] = "XBL2.0 x=" + XboxLiveGamer.GetPartnerToken("http://xboxlive.com");
      return edsWebHeaders;
    }

    private static Uri CreateUri(string endpointPath)
    {
      Uri result = (Uri) null;
      if (!string.IsNullOrEmpty(endpointPath) && !Uri.TryCreate(endpointPath, UriKind.RelativeOrAbsolute, out result))
        result = (Uri) null;
      return result;
    }

    private void FireEvent<T>(
      EventHandler<LRC.Service.ServiceProxyEventArgs<T>> eventHandler,
      T data,
      Exception exception,
      object userState)
    {
      this.FireEvent<T>(eventHandler, data, exception, userState, true);
    }

    private void FireEvent<T>(
      EventHandler<LRC.Service.ServiceProxyEventArgs<T>> eventHandler,
      T data,
      Exception exception,
      object userState,
      bool useUIThread)
    {
      if (useUIThread)
      {
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          EventHandler<LRC.Service.ServiceProxyEventArgs<T>> eventHandler1 = eventHandler;
          if (eventHandler1 == null)
            return;
          if (exception == null)
            eventHandler1((object) this, new LRC.Service.ServiceProxyEventArgs<T>((object) (T) data, (Exception) null, false, userState));
          else
            eventHandler1((object) this, new LRC.Service.ServiceProxyEventArgs<T>((object) null, exception, false, userState));
        }, (object) this);
      }
      else
      {
        EventHandler<LRC.Service.ServiceProxyEventArgs<T>> eventHandler2 = eventHandler;
        if (eventHandler2 == null)
          return;
        if (exception == null)
          eventHandler2((object) this, new LRC.Service.ServiceProxyEventArgs<T>((object) data, (Exception) null, false, userState));
        else
          eventHandler2((object) this, new LRC.Service.ServiceProxyEventArgs<T>((object) null, exception, false, userState));
      }
    }

    private void SearchCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.SearchCompleted);
      if (e.Error == null)
        this.FireEvent<SearchResults>(this.EventSearchCompleted, EdsResponseParser.ParseSearchResponse(e.Result as string), (Exception) null, e.UserState);
      else
        this.FireEvent<SearchResults>(this.EventSearchCompleted, (SearchResults) null, e.Error, e.UserState);
    }

    private void GetSearchTermsCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetSearchTermsCompleted);
      if (e.Error == null)
        this.FireEvent<List<string>>(this.EventGetSearchTermsCompleted, EdsResponseParser.ParseSearchTermsResponse(e.Result as string), (Exception) null, e.UserState);
      else
        this.FireEvent<List<string>>(this.EventGetSearchTermsCompleted, (List<string>) null, e.Error, e.UserState);
    }

    private void GetRelatedItemsCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetRelatedItemsCompleted);
      if (e.Error == null)
        this.FireEvent<List<SearchData>>(this.EventGetRelatedItemsCompleted, EdsResponseParser.ParseRelatedItemsResponse(e.Result as string), (Exception) null, e.UserState);
      else
        this.FireEvent<List<SearchData>>(this.EventGetRelatedItemsCompleted, (List<SearchData>) null, e.Error, e.UserState);
    }

    private void GetRecommendedItemsCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetRecommendedItemsCompleted);
      if (e.Error == null)
        this.FireEvent<List<SearchData>>(this.EventGetRecommendedItemsCompleted, EdsResponseParser.ParseRecommendationResponse(e.Result as string), (Exception) null, e.UserState, false);
      else
        this.FireEvent<List<SearchData>>(this.EventGetRecommendedItemsCompleted, (List<SearchData>) null, e.Error, e.UserState, false);
    }

    private void GetMovieDetailsCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetMovieDetailsCompleted);
      if (e.Error == null)
        this.FireEvent<MovieData>(this.EventGetMovieDetailsCompleted, EdsResponseParser.ParseMovieDetailsResponse(e.Result as string), (Exception) null, e.UserState);
      else
        this.FireEvent<MovieData>(this.EventGetMovieDetailsCompleted, (MovieData) null, e.Error, e.UserState);
    }

    private void GetAppDetailsCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetAppDetailsCompleted);
      if (e.Error == null)
        this.FireEvent<XboxAppData>(this.EventGetAppDetailsCompleted, new XboxAppData(GameMarketplaceResponseParser.ParseGameDetailsResponse(e.Result as string)), (Exception) null, e.UserState);
      else
        this.FireEvent<XboxAppData>(this.EventGetAppDetailsCompleted, (XboxAppData) null, e.Error, e.UserState);
    }

    private void GetTvSeasonsForSeriesCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetTvSeasonsForSeriesCompleted);
      if (e.Error == null)
        this.FireEvent<TvSeriesData>(this.EventGetTvSeasonsForSeriesCompleted, EdsResponseParser.ParseTvSeriesDetailsResponse(e.Result as string), (Exception) null, e.UserState);
      else
        this.FireEvent<TvSeriesData>(this.EventGetTvSeasonsForSeriesCompleted, (TvSeriesData) null, e.Error, e.UserState);
    }

    private void GetTvEpisodesForSeasonCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetTvEpisodesForSeasonCompleted);
      if (e.Error == null)
        this.FireEvent<TvSeasonData>(this.EventGetTvEpisodesForSeasonCompleted, EdsResponseParser.ParseTvSeasonDetailsResponse(e.Result as string), (Exception) null, e.UserState);
      else
        this.FireEvent<TvSeasonData>(this.EventGetTvEpisodesForSeasonCompleted, (TvSeasonData) null, e.Error, e.UserState);
    }

    private void GetTvEpisodeDetailsCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetTvEpisodeDetailsCompleted);
      if (e.Error == null)
        this.FireEvent<TvEpisodeData>(this.EventGetTvEpisodeDetailsCompleted, EdsResponseParser.ParseTvEpisodeDetailsResponse(e.Result as string), (Exception) null, e.UserState);
      else
        this.FireEvent<TvEpisodeData>(this.EventGetTvEpisodeDetailsCompleted, (TvEpisodeData) null, e.Error, e.UserState);
    }

    private void GetNowPlayingAppDetailsCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetNowPlayingAppDetailsCompleted);
      if (e.Error == null)
        this.FireEvent<XboxGameData>(this.EventGetNowPlayingAppDetailsCompleted, GameMarketplaceResponseParser.ParseGameDetailsResponse(e.Result as string), (Exception) null, e.UserState);
      else
        this.FireEvent<XboxGameData>(this.EventGetNowPlayingAppDetailsCompleted, (XboxGameData) null, e.Error, e.UserState);
    }

    private void GetNowPlayingItemDetailsCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetNowPlayingItemDetailsCompleted);
      if (e.Error == null)
        this.FireEvent<SearchData>(this.EventGetNowPlayingItemDetailsCompleted, EdsResponseParser.ParseItemDetailsResponse(e.Result as string), (Exception) null, e.UserState);
      else
        this.FireEvent<SearchData>(this.EventGetNowPlayingItemDetailsCompleted, (SearchData) null, e.Error, e.UserState);
    }

    private void GetGameDetailCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetGameDetailCompleted);
      if (e.Error == null)
        this.FireEvent<XboxGameData>(this.EventGetGameDetailsCompleted, GameMarketplaceResponseParser.ParseGameDetailsResponse(e.Result as string), (Exception) null, e.UserState);
      else
        this.FireEvent<XboxGameData>(this.EventGetGameDetailsCompleted, (XboxGameData) null, e.Error, e.UserState);
    }
  }
}
