// *********************************************************
// Type: LRC.Service.Search.EdsConstants
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml.Linq;
using Xbox.Live.Phone;
using Xbox.Live.Phone.Services;


namespace LRC.Service.Search
{
  internal static class EdsConstants
  {
    internal const string MSPointIndicator = "MPT";
    internal const string FilterTotal = "TOTAL";
    internal const string FilterApps = "APPITEM";
    internal const string FilterGames = "XBOXGAMEITEM";
    internal const string FilterMovies = "MOVIEITEM";
    internal const string FilterMusic = "MUSICITEM";
    internal const string FilterTV = "TVITEM";
    internal const string FilterNone = "";
    internal const string FilterSeparator = ".";
    internal const string ItemAttributeRecommendedItem = "RecommendedItem";
    internal const string FilterAllItems = "APPITEM.XBOXGAMEITEM.MOVIEITEM.MUSICITEM.TVITEM";
    private const string ComponentName = "EdsConstants";
    private const string EdsBaseUriForINT = "http://eds-beta.xboxlive.com/eds/";
    private const string EdsBaseUriForPROD = "https://services.xboxlive.com/eds/";
    private const string CommonQueryParams = "?clientContext=bing&clientType=WinPhone7&inputMethod=Controller&tier=Silver.Gold&version=1.4";
    internal static readonly XNamespace EdsNamespace = (XNamespace) "http://schemas.datacontract.org/2004/07/Microsoft.EDS.DataModel";
    internal static readonly XNamespace XMLSchemaNamespace = (XNamespace) "http://www.w3.org/2001/XMLSchema-instance";
    internal static readonly XName ContinuationToken = EdsConstants.EdsNamespace + nameof (ContinuationToken);
    internal static readonly XName Items = EdsConstants.EdsNamespace + nameof (Items);
    internal static readonly XName Item = EdsConstants.EdsNamespace + nameof (Item);
    internal static readonly XName TotalResultsCounters = EdsConstants.EdsNamespace + nameof (TotalResultsCounters);
    internal static readonly XName Count = EdsConstants.EdsNamespace + nameof (Count);
    internal static readonly XName ID = EdsConstants.EdsNamespace + nameof (ID);
    internal static readonly XName Image = EdsConstants.EdsNamespace + nameof (Image);
    internal static readonly XName Url = EdsConstants.EdsNamespace + nameof (Url);
    internal static readonly XName Height = EdsConstants.EdsNamespace + nameof (Height);
    internal static readonly XName Width = EdsConstants.EdsNamespace + nameof (Width);
    internal static readonly XName DetailsUrl = EdsConstants.EdsNamespace + nameof (DetailsUrl);
    internal static readonly XName Name = EdsConstants.EdsNamespace + nameof (Name);
    internal static readonly XName ReleaseDate = EdsConstants.EdsNamespace + nameof (ReleaseDate);
    internal static readonly XName ArtistName = EdsConstants.EdsNamespace + nameof (ArtistName);
    internal static readonly XName RecommendedItem = EdsConstants.EdsNamespace + nameof (RecommendedItem);
    internal static readonly XName IsActionable = EdsConstants.EdsNamespace + nameof (IsActionable);
    internal static readonly XName ItemTypeElement = EdsConstants.EdsNamespace + "ItemType";
    internal static readonly XName ItemTypeAttribute = EdsConstants.XMLSchemaNamespace + "type";
    internal static readonly XName ParentItemType = EdsConstants.EdsNamespace + nameof (ParentItemType);
    internal static readonly XName ParentalRating = EdsConstants.EdsNamespace + nameof (ParentalRating);
    internal static readonly XName ParentalRatingSystem = EdsConstants.EdsNamespace + nameof (ParentalRatingSystem);
    internal static readonly XName CriticRating = EdsConstants.EdsNamespace + nameof (CriticRating);
    internal static readonly XName AverageUserRating = EdsConstants.EdsNamespace + nameof (AverageUserRating);
    internal static readonly XName UserRatingCount = EdsConstants.EdsNamespace + nameof (UserRatingCount);
    internal static readonly XName Duration = EdsConstants.EdsNamespace + nameof (Duration);
    internal static readonly XName Details = EdsConstants.EdsNamespace + nameof (Details);
    internal static readonly XName Description = EdsConstants.EdsNamespace + nameof (Description);
    internal static readonly XName Person = EdsConstants.EdsNamespace + nameof (Person);
    internal static readonly XName Actors = EdsConstants.EdsNamespace + nameof (Actors);
    internal static readonly XName Directors = EdsConstants.EdsNamespace + nameof (Directors);
    internal static readonly XName Writers = EdsConstants.EdsNamespace + nameof (Writers);
    internal static readonly XName SeasonNumber = EdsConstants.EdsNamespace + nameof (SeasonNumber);
    internal static readonly XName EpisodeNumber = EdsConstants.EdsNamespace + nameof (EpisodeNumber);
    internal static readonly XName HasSeasons = EdsConstants.EdsNamespace + nameof (HasSeasons);
    internal static readonly XName Genres = EdsConstants.EdsNamespace + nameof (Genres);
    internal static readonly XName Genre = EdsConstants.EdsNamespace + nameof (Genre);
    internal static readonly XName GameGenre = EdsConstants.EdsNamespace + nameof (GameGenre);
    internal static readonly XName NumberOfProviders = EdsConstants.EdsNamespace + nameof (NumberOfProviders);
    internal static readonly XName Providers = EdsConstants.EdsNamespace + nameof (Providers);
    internal static readonly XName Provider = EdsConstants.EdsNamespace + nameof (Provider);
    internal static readonly XName PartnerApplicationLaunchInfos = EdsConstants.EdsNamespace + nameof (PartnerApplicationLaunchInfos);
    internal static readonly XName PartnerApplicationLaunchInfo = EdsConstants.EdsNamespace + nameof (PartnerApplicationLaunchInfo);
    internal static readonly XName ProductId = EdsConstants.EdsNamespace + nameof (ProductId);
    internal static readonly XName ProviderContents = EdsConstants.EdsNamespace + nameof (ProviderContents);
    internal static readonly XName ProviderContent = EdsConstants.EdsNamespace + nameof (ProviderContent);
    internal static readonly XName Device = EdsConstants.EdsNamespace + nameof (Device);
    internal static readonly XName OfferInstances = EdsConstants.EdsNamespace + nameof (OfferInstances);
    internal static readonly XName OfferInstance = EdsConstants.EdsNamespace + nameof (OfferInstance);
    internal static readonly XName DisplayPrice = EdsConstants.EdsNamespace + nameof (DisplayPrice);
    internal static readonly XName OfferType = EdsConstants.EdsNamespace + nameof (OfferType);
    internal static readonly XName Price = EdsConstants.EdsNamespace + nameof (Price);
    internal static readonly XName VideoAttributes = EdsConstants.EdsNamespace + nameof (VideoAttributes);
    internal static readonly XName ClosedCaptioning = EdsConstants.EdsNamespace + nameof (ClosedCaptioning);
    internal static readonly XName ResolutionFormat = EdsConstants.EdsNamespace + nameof (ResolutionFormat);
    internal static readonly XName VideoType = EdsConstants.EdsNamespace + nameof (VideoType);
    internal static readonly XName ClientType = EdsConstants.EdsNamespace + nameof (ClientType);
    internal static readonly XName DeepLinkInfo = EdsConstants.EdsNamespace + nameof (DeepLinkInfo);
    internal static readonly XName TitleId = EdsConstants.EdsNamespace + nameof (TitleId);
    internal static readonly XName SearchTerms = EdsConstants.EdsNamespace + nameof (SearchTerms);
    internal static readonly XName SearchTerm = EdsConstants.EdsNamespace + nameof (SearchTerm);
    internal static readonly XName Value = EdsConstants.EdsNamespace + nameof (Value);
    private static bool isInitialized;

    internal static string EdsBaseUri { get; set; }

    internal static string SearchTermsUri { get; private set; }

    internal static string MovieRecommendationBaseUri { get; private set; }

    internal static string MovieDetailsBaseUri { get; private set; }

    internal static string AppDetailsBaseUri { get; private set; }

    internal static string TvSeriesDetailsBaseUri { get; private set; }

    internal static string TvSeasonDetailsBaseUri { get; private set; }

    internal static string TvEpisodeDetailsBaseUri { get; private set; }

    internal static string AppsDetailsBaseUri { get; private set; }

    internal static string NowPlayingItemDetailsBaseUri { get; private set; }

    private static string EdsBaseUriWithLocale { get; set; }

    private static string SearchBaseUri { get; set; }

    [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Simpler to read this way and allows us to make sure it happens only once.")]
    internal static void Initialize()
    {
      if (EdsConstants.isInitialized)
        return;
      EdsConstants.isInitialized = true;
      EdsConstants.EdsBaseUri = EnvironmentState.Instance.IsProduction ? "https://services.xboxlive.com/eds/" : "http://eds-beta.xboxlive.com/eds/";
      EdsConstants.EdsBaseUriWithLocale = EdsConstants.EdsBaseUri + XboxLiveGamer.CurrentGamer.LegalLocale;
      EdsConstants.SearchBaseUri = EdsConstants.EdsBaseUriWithLocale + "/items?clientContext=bing&clientType=WinPhone7&inputMethod=Controller&tier=Silver.Gold&version=1.4&isActionable=true&maxItems={0}&q={1}";
      EdsConstants.SearchTermsUri = EdsConstants.EdsBaseUriWithLocale + "/searchTerms?clientContext=bing&clientType=WinPhone7&inputMethod=Controller&tier=Silver.Gold&version=1.4";
      EdsConstants.MovieRecommendationBaseUri = EdsConstants.EdsBaseUriWithLocale + "/recommendations?clientContext=bing&clientType=WinPhone7&inputMethod=Controller&tier=Silver.Gold&version=1.4&itemTypes=movie";
      EdsConstants.MovieDetailsBaseUri = EdsConstants.EdsBaseUriWithLocale + "/movies/{0}?clientContext=bing&clientType=WinPhone7&inputMethod=Controller&tier=Silver.Gold&version=1.4&searchTerm={1}";
      EdsConstants.AppDetailsBaseUri = EdsConstants.EdsBaseUriWithLocale + "/xboxApps/{0}?clientContext=bing&clientType=WinPhone7&inputMethod=Controller&tier=Silver.Gold&version=1.4";
      EdsConstants.TvSeriesDetailsBaseUri = EdsConstants.EdsBaseUriWithLocale + "/tvseries/{0}/seasons?clientContext=bing&clientType=WinPhone7&inputMethod=Controller&tier=Silver.Gold&version=1.4" + EdsConstants.GetContinuationParam("{1}") + EdsConstants.GetMaxItemsParam("{2}");
      EdsConstants.TvSeasonDetailsBaseUri = EdsConstants.EdsBaseUriWithLocale + "/tvSeasons/{0}/episodes?clientContext=bing&clientType=WinPhone7&inputMethod=Controller&tier=Silver.Gold&version=1.4" + EdsConstants.GetContinuationParam("{1}") + EdsConstants.GetMaxItemsParam("{2}");
      EdsConstants.TvEpisodeDetailsBaseUri = EdsConstants.EdsBaseUriWithLocale + "/tvEpisodes/{0}?clientContext=bing&clientType=WinPhone7&inputMethod=Controller&tier=Silver.Gold&version=1.4";
      EdsConstants.NowPlayingItemDetailsBaseUri = EdsConstants.EdsBaseUriWithLocale + "/titles/{0}/content?clientContext=bing&clientType=WinPhone7&inputMethod=Controller&tier=Silver.Gold&version=1.4&contentId={1}";
    }

    internal static string GetFormattedTitleId(uint titleId)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "0x{0:X}", new object[1]
      {
        (object) titleId
      });
    }

    internal static Uri GetPaginationUri(
      string detailsUrl,
      string continuationToken,
      uint maxItems,
      uint consoleLiveTVtitleId)
    {
      Uri result = (Uri) null;
      if (!string.IsNullOrEmpty(detailsUrl))
      {
        if (!Uri.TryCreate(EdsConstants.EdsBaseUri + detailsUrl + EdsConstants.GetContinuationParam(continuationToken) + EdsConstants.GetMaxItemsParam(maxItems) + EdsConstants.GetConsoleProvidersParam(consoleLiveTVtitleId), UriKind.Absolute, out result))
          result = (Uri) null;
      }
      return result;
    }

    internal static string GetContinuationParam(string continuationToken)
    {
      return !string.IsNullOrEmpty(continuationToken) ? "&continuationToken=" + continuationToken : (string) null;
    }

    internal static string GetConsoleProvidersParam(uint consoleLiveTVtitleId)
    {
      return consoleLiveTVtitleId > 0U ? "&providers=" + EdsConstants.GetFormattedTitleId(consoleLiveTVtitleId) : (string) null;
    }

    internal static string GetMaxItemsParam(uint maxItems)
    {
      return EdsConstants.GetMaxItemsParam(maxItems.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo));
    }

    internal static string GetMaxItemsParam(string maxItems) => "&maxItems=" + maxItems;

    internal static string GetSearchUri(
      string searchTerm,
      Filter filter,
      string continuationToken,
      uint resultsPerPage,
      uint consoleLiveTVtitleId)
    {
      string str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, EdsConstants.SearchBaseUri, new object[2]
      {
        (object) resultsPerPage,
        (object) Uri.EscapeDataString(searchTerm)
      });
      if (filter != Filter.All)
        str = str + "&itemTypes=" + EdsConstants.GetEdsFilter(filter);
      return str + EdsConstants.GetContinuationParam(continuationToken) + EdsConstants.GetConsoleProvidersParam(consoleLiveTVtitleId);
    }

    internal static string GetRelatedItemsUri(
      string id,
      string itemType,
      bool microsoftContentOnly,
      uint consoleLiveTVtitleId)
    {
      if (string.IsNullOrEmpty(id))
        return (string) null;
      if (string.IsNullOrEmpty(itemType))
        return (string) null;
      Filter filter = EdsResponseParser.ParseFilter(itemType);
      string str1 = (string) null;
      string str2 = (string) null;
      string str3 = microsoftContentOnly ? "microsoft" : "all";
      switch (filter)
      {
        case Filter.Movies:
          str1 = "/movies/";
          str2 = "MOVIE";
          break;
        case Filter.TV:
          if (string.Equals(itemType.ToUpperInvariant(), "TVSERIES", StringComparison.OrdinalIgnoreCase))
          {
            str1 = "/TVSERIES/";
            str2 = "TVSERIES";
            break;
          }
          if (string.Equals(itemType.ToUpperInvariant(), "TVEPISODE", StringComparison.OrdinalIgnoreCase))
          {
            str1 = "/tvEpisodes/";
            str2 = "TVSERIES";
            break;
          }
          if (string.Equals(itemType.ToUpperInvariant(), "TVSHOW", StringComparison.OrdinalIgnoreCase))
          {
            str1 = "/tvShows/";
            str2 = "TVSERIES";
            break;
          }
          break;
        case Filter.Games:
          str1 = "/xbox360games/";
          str2 = "XBOXGAMEITEM";
          break;
      }
      if (string.IsNullOrEmpty(str2))
        return (string) null;
      return EdsConstants.EdsBaseUriWithLocale + str1 + id + "/RelatedItems?clientContext=bing&clientType=WinPhone7&inputMethod=Controller&tier=Silver.Gold&version=1.4&itemTypes=" + str2 + "&idspace=" + str3 + EdsConstants.GetConsoleProvidersParam(consoleLiveTVtitleId);
    }

    private static string GetEdsFilter(Filter filter)
    {
      string edsFilter = "";
      switch (filter)
      {
        case Filter.Movies:
          edsFilter = "MOVIEITEM";
          break;
        case Filter.Music:
          edsFilter = "MUSICITEM";
          break;
        case Filter.TV:
          edsFilter = "TVITEM";
          break;
        case Filter.Games:
          edsFilter = "XBOXGAMEITEM";
          break;
        case Filter.Apps:
          edsFilter = "APPITEM";
          break;
        case Filter.All:
          edsFilter = "APPITEM.XBOXGAMEITEM.MOVIEITEM.MUSICITEM.TVITEM";
          break;
      }
      return edsFilter;
    }
  }
}
