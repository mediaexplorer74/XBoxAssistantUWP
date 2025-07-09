// *********************************************************
// Type: LRC.Service.Search.ISearchServiceManager
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace LRC.Service.Search
{
  public interface ISearchServiceManager
  {
    event EventHandler<ServiceProxyEventArgs<SearchResults>> EventSearchCompleted;

    event EventHandler<ServiceProxyEventArgs<List<string>>> EventGetSearchTermsCompleted;

    event EventHandler<ServiceProxyEventArgs<List<SearchData>>> EventGetRelatedItemsCompleted;

    event EventHandler<ServiceProxyEventArgs<List<SearchData>>> EventGetRecommendedItemsCompleted;

    event EventHandler<ServiceProxyEventArgs<MovieData>> EventGetMovieDetailsCompleted;

    event EventHandler<ServiceProxyEventArgs<XboxAppData>> EventGetAppDetailsCompleted;

    event EventHandler<ServiceProxyEventArgs<TvSeriesData>> EventGetTvSeasonsForSeriesCompleted;

    event EventHandler<ServiceProxyEventArgs<TvSeasonData>> EventGetTvEpisodesForSeasonCompleted;

    event EventHandler<ServiceProxyEventArgs<TvEpisodeData>> EventGetTvEpisodeDetailsCompleted;

    event EventHandler<ServiceProxyEventArgs<SearchData>> EventGetNowPlayingItemDetailsCompleted;

    event EventHandler<ServiceProxyEventArgs<XboxGameData>> EventGetNowPlayingAppDetailsCompleted;

    event EventHandler<ServiceProxyEventArgs<XboxGameData>> EventGetGameDetailsCompleted;

    void Search(
      string searchTerm,
      Filter filters,
      string continuationToken,
      uint resultsPerPage,
      uint consoleLiveTVtitleId,
      object userState);

    void GetSearchTerms(object userState);

    void GetRelatedItems(
      string itemId,
      string itemType,
      bool useZuneOnly,
      uint consoleLiveTVtitleId,
      object userState);

    [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Naming convention matches our data structures and the returning data.")]
    void GetMovieDetails(
      string id,
      string movieName,
      string detailsUrl,
      uint consoleLiveTVtitleId,
      object userState);

    [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Naming convention matches our data structures and the returning data.")]
    void GetAppDetails(string id, string appName, string detailsUrl, object userState);

    [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Naming convention matches our data structures and the returning data.")]
    void GetTvSeasonsForSeries(
      string seriesId,
      string continuationToken,
      uint resultsPerPage,
      string seriesName,
      string detailsUrl,
      uint consoleLiveTVtitleId,
      object userState);

    [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Naming convention matches our data structures and the returning data.")]
    void GetTvEpisodesForSeason(
      string seasonId,
      string continuationToken,
      uint resultsPerPage,
      string seasonName,
      string detailsUrl,
      uint consoleLiveTVtitleId,
      object userState);

    [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Naming convention matches our data structures and the returning data.")]
    void GetTvEpisodeDetails(
      string episodeId,
      string episodeName,
      string detailsUrl,
      uint consoleLiveTVtitleId,
      object userState);

    void GetNowPlayingAppDetails(uint titleId, object userState);

    void GetNowPlayingItemDetails(
      uint titleId,
      string partnerContentId,
      uint consoleLiveTVtitleId,
      object userState);

    void GetGameDetailsByMediaId(string mediaId, object userState);

    void GetGameDetailsByTitleId(uint titleId, object userState);

    void GetRecommendedItems(Filter itemFilter, uint consoleLiveTVtitleId, object userState);
  }
}
