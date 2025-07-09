// *********************************************************
// Type: LRC.Service.Search.EdsResponseParser
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xbox.Live.Phone.Utils.Linq;
using Xbox.Live.Phone.Utils.Serialization;


namespace LRC.Service.Search
{
  public static class EdsResponseParser
  {
    private const string ComponentName = "EdsResponseParser";

    public static SearchResults ParseSearchResponse(string serviceResponse)
    {
      if (string.IsNullOrEmpty(serviceResponse))
        return (SearchResults) null;
      SearchResults searchResponse = new SearchResults();
      XElement xelement = LinqHelper.SafeParseXElement(serviceResponse);
      if (xelement != null)
      {
        XElement itemsElement = xelement.Element(EdsConstants.Items);
        if (itemsElement != null)
          searchResponse.Items = EdsResponseParser.GetSearchDataItems(itemsElement);
        XElement resultsElement = xelement.Element(EdsConstants.TotalResultsCounters);
        if (resultsElement != null)
          searchResponse.TotalResultsCounter = EdsResponseParser.GetResultsCounters(resultsElement);
        searchResponse.ContinuationToken = LinqHelper.TryGetStringValue(xelement.Element(EdsConstants.ContinuationToken));
      }
      return searchResponse;
    }

    public static List<string> ParseSearchTermsResponse(string serviceResponse)
    {
      if (!string.IsNullOrEmpty(serviceResponse))
      {
        XElement xelement1 = LinqHelper.SafeParseXElement(serviceResponse);
        if (xelement1 != null)
        {
          XElement xelement2 = xelement1.Element(EdsConstants.SearchTerms);
          if (xelement2 != null)
          {
            IEnumerable<XElement> source = xelement2.Elements(EdsConstants.SearchTerm);
            if (source != null)
            {
              IEnumerable<string> collection = source.Select<XElement, string>((Func<XElement, string>) (searchTerm => LinqHelper.TryGetElementValue(searchTerm, EdsConstants.Value)));
              if (collection != null)
                return new List<string>(collection);
            }
          }
        }
      }
      return new List<string>();
    }

    public static SearchData ParseItemDetailsResponse(string serviceResponse)
    {
      if (!string.IsNullOrEmpty(serviceResponse))
      {
        XElement xelement1 = LinqHelper.SafeParseXElement(serviceResponse);
        if (xelement1 != null)
        {
          XElement xelement2 = xelement1.Element(EdsConstants.Item);
          if (xelement2 != null)
            return EdsResponseParser.CreateSearchData(xelement2);
        }
      }
      return (SearchData) null;
    }

    public static List<SearchData> ParseRelatedItemsResponse(string serviceResponse)
    {
      if (!string.IsNullOrEmpty(serviceResponse))
      {
        XElement xelement = LinqHelper.SafeParseXElement(serviceResponse);
        if (xelement != null)
        {
          XElement itemsElement = xelement.Element(EdsConstants.Items);
          if (itemsElement != null)
            return EdsResponseParser.GetSearchDataItems(itemsElement);
        }
      }
      return new List<SearchData>();
    }

    public static MovieData ParseMovieDetailsResponse(string serviceResponse)
    {
      if (!string.IsNullOrEmpty(serviceResponse))
      {
        XElement xelement1 = LinqHelper.SafeParseXElement(serviceResponse);
        if (xelement1 != null)
        {
          XElement xelement2 = xelement1.Element(EdsConstants.Item);
          if (xelement2 != null)
          {
            SearchData searchData = EdsResponseParser.CreateSearchData(xelement2);
            if (searchData != null)
            {
              MovieData movieDetailsResponse = new MovieData(searchData);
              movieDetailsResponse.Duration = LinqHelper.TryGetTimeSpanValue(xelement2.Element(EdsConstants.Duration));
              XElement xelement3 = xelement2.Element(EdsConstants.Details);
              if (xelement3 != null)
              {
                movieDetailsResponse.Description = LinqHelper.TryGetScrubbedStringValue(xelement3.Element(EdsConstants.Description));
                movieDetailsResponse.Actors = LinqHelper.TryGetStringList(xelement3.Element(EdsConstants.Actors), EdsConstants.Person);
                movieDetailsResponse.Directors = LinqHelper.TryGetStringList(xelement3.Element(EdsConstants.Directors), EdsConstants.Person);
                movieDetailsResponse.Writers = LinqHelper.TryGetStringList(xelement3.Element(EdsConstants.Writers), EdsConstants.Person);
              }
              movieDetailsResponse.Providers = EdsResponseParser.GetProviders(xelement2);
              return movieDetailsResponse;
            }
          }
        }
      }
      return (MovieData) null;
    }

    public static List<SearchData> ParseRecommendationResponse(string serviceResponse)
    {
      List<SearchData> recommendationResponse = new List<SearchData>();
      if (!string.IsNullOrEmpty(serviceResponse))
      {
        XElement xelement1 = LinqHelper.SafeParseXElement(serviceResponse);
        if (xelement1 != null)
        {
          IEnumerable<XElement> source = xelement1.Descendants(EdsConstants.Item).Where<XElement>((Func<XElement, bool>) (recommendedItem => string.Compare(LinqHelper.TryGetStringValue(recommendedItem.Attribute(EdsConstants.ItemTypeAttribute)), "RecommendedItem", StringComparison.OrdinalIgnoreCase) == 0));
          if (source != null)
          {
            IEnumerable<XElement> xelements = source.Descendants<XElement>(EdsConstants.Item);
            if (xelements != null)
            {
              foreach (XElement xelement2 in xelements)
              {
                SearchData searchData = EdsResponseParser.CreateSearchData(xelement2);
                if (searchData != null)
                  recommendationResponse.Add(searchData);
              }
            }
          }
        }
      }
      return recommendationResponse;
    }

    public static XboxAppData ParseAppDetailsResponse(string serviceResponse)
    {
      if (!string.IsNullOrEmpty(serviceResponse))
      {
        XElement xelement1 = LinqHelper.SafeParseXElement(serviceResponse);
        if (xelement1 != null)
        {
          XElement xelement2 = xelement1.Element(EdsConstants.Item);
          if (xelement2 != null)
          {
            SearchData searchData = EdsResponseParser.CreateSearchData(xelement2);
            if (searchData != null)
            {
              XboxAppData appDetailsResponse = new XboxAppData(searchData);
              appDetailsResponse.TitleId = LinqHelper.TryGetUintTitleIdValue(xelement2.Element(EdsConstants.TitleId));
              XElement xelement3 = xelement2.Element(EdsConstants.Details);
              if (xelement3 != null)
              {
                appDetailsResponse.Description = LinqHelper.TryGetStringValue(xelement3.Element(EdsConstants.Description));
                appDetailsResponse.Genres = EdsResponseParser.GetGenres(xelement3);
              }
              return appDetailsResponse;
            }
          }
        }
      }
      return (XboxAppData) null;
    }

    public static TvSeriesData ParseTvSeriesDetailsResponse(string serviceResponse)
    {
      if (!string.IsNullOrEmpty(serviceResponse))
      {
        XElement xelement = LinqHelper.SafeParseXElement(serviceResponse);
        if (xelement != null)
        {
          TvSeriesData seriesDetailsResponse = new TvSeriesData();
          seriesDetailsResponse.ContinuationToken = LinqHelper.TryGetStringValue(xelement.Element(EdsConstants.ContinuationToken));
          IEnumerable<TvSeasonData> tvSeasonDatas = xelement.Element(EdsConstants.Items).Elements(EdsConstants.Item).Select<XElement, TvSeasonData>((Func<XElement, TvSeasonData>) (item => EdsResponseParser.CreateTvSeasonData(item)));
          seriesDetailsResponse.TvSeasons = new XmlSerializableDictionary<string, TvSeasonData>();
          if (tvSeasonDatas != null)
          {
            foreach (TvSeasonData tvSeasonData in tvSeasonDatas)
              seriesDetailsResponse.TvSeasons.Add(tvSeasonData.Id, tvSeasonData);
          }
          IEnumerable<TvEpisodeData> tvEpisodeDatas = xelement.Element(EdsConstants.Items).Elements(EdsConstants.Item).Select<XElement, TvEpisodeData>((Func<XElement, TvEpisodeData>) (item => EdsResponseParser.CreateTvEpisodeData(item)));
          seriesDetailsResponse.TvEpisodes = new XmlSerializableDictionary<string, TvEpisodeData>();
          if (tvEpisodeDatas != null)
          {
            foreach (TvEpisodeData tvEpisodeData in tvEpisodeDatas)
              seriesDetailsResponse.TvEpisodes.Add(tvEpisodeData.Id, tvEpisodeData);
          }
          seriesDetailsResponse.TvSeriesHasSeasons = new bool?(seriesDetailsResponse.TvSeasons != null && seriesDetailsResponse.TvSeasons.Count > 0);
          return seriesDetailsResponse;
        }
      }
      return (TvSeriesData) null;
    }

    public static TvSeasonData ParseTvSeasonDetailsResponse(string serviceResponse)
    {
      if (!string.IsNullOrEmpty(serviceResponse))
      {
        XElement xelement = LinqHelper.SafeParseXElement(serviceResponse);
        if (xelement != null)
        {
          IEnumerable<TvEpisodeData> tvEpisodeDatas = xelement.Element(EdsConstants.Items).Elements(EdsConstants.Item).Select<XElement, TvEpisodeData>((Func<XElement, TvEpisodeData>) (item => EdsResponseParser.CreateTvEpisodeData(item)));
          TvSeasonData seasonDetailsResponse = new TvSeasonData();
          seasonDetailsResponse.ContinuationToken = LinqHelper.TryGetStringValue(xelement.Element(EdsConstants.ContinuationToken));
          seasonDetailsResponse.TvEpisodes = new XmlSerializableDictionary<string, TvEpisodeData>();
          if (tvEpisodeDatas != null)
          {
            foreach (TvEpisodeData tvEpisodeData in tvEpisodeDatas)
              seasonDetailsResponse.TvEpisodes.Add(tvEpisodeData.Id, tvEpisodeData);
          }
          return seasonDetailsResponse;
        }
      }
      return (TvSeasonData) null;
    }

    public static TvEpisodeData ParseTvEpisodeDetailsResponse(string serviceResponse)
    {
      if (!string.IsNullOrEmpty(serviceResponse))
      {
        XElement xelement1 = LinqHelper.SafeParseXElement(serviceResponse);
        if (xelement1 != null)
        {
          XElement xelement2 = xelement1.Element(EdsConstants.Item);
          if (xelement2 != null)
          {
            TvEpisodeData episodeDetailsResponse = new TvEpisodeData((TvData) EdsResponseParser.CreateTvEpisodeData(xelement2));
            episodeDetailsResponse.SeasonNumber = LinqHelper.TryGetIntValue(xelement2.Element(EdsConstants.SeasonNumber));
            episodeDetailsResponse.EpisodeNumber = LinqHelper.TryGetIntValue(xelement2.Element(EdsConstants.EpisodeNumber));
            XElement xelement3 = xelement2.Element(EdsConstants.Details);
            if (xelement3 != null)
            {
              episodeDetailsResponse.Description = LinqHelper.TryGetScrubbedStringValue(xelement3.Element(EdsConstants.Description));
              episodeDetailsResponse.Actors = LinqHelper.TryGetStringList(xelement3.Element(EdsConstants.Actors), EdsConstants.Person);
              episodeDetailsResponse.Directors = LinqHelper.TryGetStringList(xelement3.Element(EdsConstants.Directors), EdsConstants.Person);
              episodeDetailsResponse.Writers = LinqHelper.TryGetStringList(xelement3.Element(EdsConstants.Writers), EdsConstants.Person);
            }
            episodeDetailsResponse.Providers = EdsResponseParser.GetProviders(xelement2);
            return episodeDetailsResponse;
          }
        }
      }
      return (TvEpisodeData) null;
    }

    public static Filter ParseFilter(string itemType)
    {
      if (string.IsNullOrEmpty(itemType))
        return Filter.None;
      Filter filter = Filter.None;
      switch (itemType.ToUpperInvariant())
      {
        case "APPITEM":
        case "XBOXAPP":
          filter = Filter.Apps;
          break;
        case "XBOXGAMEITEM":
        case "XBOXARCADEGAME":
        case "XBOXGAMECONSUMABLE":
        case "XBOX360GAME":
        case "XBOX360GAMECONTENT":
        case "XBOX360GAMEDEMO":
        case "XBOXGAMERTILE":
        case "XBOXGAMETRAILER":
        case "XBOXGAMETRIAL":
        case "XBOXGAMEVIDEO":
        case "XBOXORIGINALGAME":
        case "XBOXTHEME":
        case "XBOXXNACOMMUNITYGAME":
          filter = Filter.Games;
          break;
        case "TVITEM":
        case "TVEPISODE":
        case "TVSEASON":
        case "TVSERIES":
        case "TVSHOW":
          filter = Filter.TV;
          break;
        case "MUSICITEM":
        case "MUSICALBUM":
        case "MUSICARTIST":
        case "MUSICVIDEO":
          filter = Filter.Music;
          break;
        case "MOVIEITEM":
        case "MOVIE":
          filter = Filter.Movies;
          break;
        case "TOTAL":
          filter = Filter.All;
          break;
      }
      return filter;
    }

    private static SearchData CreateSearchData(XElement item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      SearchData searchData = (SearchData) null;
      string stringValue = LinqHelper.TryGetStringValue(item.Attribute(EdsConstants.ItemTypeAttribute));
      if (!string.IsNullOrEmpty(stringValue))
      {
        Filter filter = EdsResponseParser.ParseFilter(stringValue);
        switch (filter)
        {
          case Filter.None:
          case Filter.All:
            break;
          default:
            searchData = new SearchData();
            searchData.Id = LinqHelper.TryGetStringValue(item.Element(EdsConstants.ID));
            searchData.DetailsUrl = LinqHelper.TryGetScrubbedStringValue(item.Element(EdsConstants.DetailsUrl));
            searchData.Name = LinqHelper.TryGetStringValue(item.Element(EdsConstants.Name));
            searchData.IsActionable = LinqHelper.TryGetBoolValue(item.Element(EdsConstants.IsActionable));
            searchData.ReleaseDate = LinqHelper.TryGetDateTimeValue(item.Element(EdsConstants.ReleaseDate));
            searchData.ItemType = LinqHelper.TryGetStringValue(item.Attribute(EdsConstants.ItemTypeAttribute));
            searchData.ParentItemType = LinqHelper.TryGetStringValue(item.Element(EdsConstants.ParentItemType));
            searchData.ParentalRating = LinqHelper.TryGetStringValue(item.Element(EdsConstants.ParentalRating));
            searchData.ParentalRatingSystem = LinqHelper.TryGetStringValue(item.Element(EdsConstants.ParentalRatingSystem));
            searchData.CriticRating = LinqHelper.TryGetDoubleValue(item.Element(EdsConstants.CriticRating));
            searchData.AverageUserRating = LinqHelper.TryGetDoubleValue(item.Element(EdsConstants.AverageUserRating));
            searchData.UserRatingCount = LinqHelper.TryGetIntValue(item.Element(EdsConstants.UserRatingCount));
            searchData.TvSeriesHasSeasons = LinqHelper.TryGetBoolValue(item.Element(EdsConstants.HasSeasons));
            searchData.ArtistName = LinqHelper.TryGetStringValue(item.Element(EdsConstants.ArtistName));
            searchData.Filter = filter;
            searchData.Genres = EdsResponseParser.GetGenres(item);
            XElement xelement = item.Element(EdsConstants.Image);
            if (xelement != null)
            {
              searchData.ImageUrl = LinqHelper.TryGetStringValue(xelement.Element(EdsConstants.Url));
              searchData.ImageHeight = LinqHelper.TryGetDoubleValue(xelement.Element(EdsConstants.Height));
              searchData.ImageWidth = LinqHelper.TryGetDoubleValue(xelement.Element(EdsConstants.Width));
              break;
            }
            break;
        }
      }
      return searchData;
    }

    private static TvSeasonData CreateTvSeasonData(XElement item)
    {
      SearchData searchData = item != null ? EdsResponseParser.CreateSearchData(item) : throw new ArgumentNullException(nameof (item));
      if (searchData == null)
        return (TvSeasonData) null;
      return new TvSeasonData(searchData)
      {
        SeasonNumber = LinqHelper.TryGetIntValue(item.Element(EdsConstants.SeasonNumber))
      };
    }

    private static TvEpisodeData CreateTvEpisodeData(XElement item)
    {
      SearchData searchData = item != null ? EdsResponseParser.CreateSearchData(item) : throw new ArgumentNullException(nameof (item));
      if (searchData == null)
        return (TvEpisodeData) null;
      return new TvEpisodeData(searchData)
      {
        SeasonNumber = LinqHelper.TryGetIntValue(item.Element(EdsConstants.SeasonNumber)),
        EpisodeNumber = LinqHelper.TryGetIntValue(item.Element(EdsConstants.EpisodeNumber))
      };
    }

    private static List<Provider> GetProviders(XElement item)
    {
      XElement xelement = item != null ? item.Element(EdsConstants.Providers) : throw new ArgumentNullException(nameof (item));
      if (xelement != null)
      {
        IEnumerable<Provider> collection = xelement.Elements(EdsConstants.Provider).Select<XElement, Provider>((Func<XElement, Provider>) (provideritem => EdsResponseParser.GetProvider(provideritem)));
        if (collection != null)
          return new List<Provider>(collection);
      }
      return (List<Provider>) null;
    }

    private static Provider GetProvider(XElement item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      return new Provider()
      {
        Id = LinqHelper.TryGetStringValue(item.Element(EdsConstants.ID)),
        ImageUrl = EdsResponseParser.GetImageUrl(item),
        Name = LinqHelper.TryGetStringValue(item.Element(EdsConstants.Name)),
        ProductId = LinqHelper.TryGetStringValue(item.Element(EdsConstants.ProductId)),
        PartnerApplicationLaunchInfos = EdsResponseParser.GetPartnerApplicationLaunchInfos(item),
        ProviderContents = EdsResponseParser.GetProviderContents(item)
      };
    }

    private static List<PartnerApplicationLaunchInfo> GetPartnerApplicationLaunchInfos(XElement item)
    {
      XElement xelement = item != null ? item.Element(EdsConstants.PartnerApplicationLaunchInfos) : throw new ArgumentNullException(nameof (item));
      if (xelement != null)
      {
        IEnumerable<PartnerApplicationLaunchInfo> collection = xelement.Elements(EdsConstants.PartnerApplicationLaunchInfo).Select<XElement, PartnerApplicationLaunchInfo>((Func<XElement, PartnerApplicationLaunchInfo>) (infoItem => EdsResponseParser.GetPartnerApplicationLaunchInfo(infoItem)));
        if (collection != null)
          return new List<PartnerApplicationLaunchInfo>(collection);
      }
      return (List<PartnerApplicationLaunchInfo>) null;
    }

    private static PartnerApplicationLaunchInfo GetPartnerApplicationLaunchInfo(XElement item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      return new PartnerApplicationLaunchInfo()
      {
        ClientType = LinqHelper.TryGetStringValue(item.Element(EdsConstants.ClientType)),
        DeepLinkInfo = LinqHelper.TryGetStringValue(item.Element(EdsConstants.DeepLinkInfo)),
        TitleId = LinqHelper.TryGetHexTitleIdValue(item.Element(EdsConstants.TitleId))
      };
    }

    private static List<ProviderContent> GetProviderContents(XElement item)
    {
      XElement xelement = item != null ? item.Element(EdsConstants.ProviderContents) : throw new ArgumentNullException(nameof (item));
      if (xelement != null)
      {
        IEnumerable<ProviderContent> collection = xelement.Elements(EdsConstants.ProviderContent).Select<XElement, ProviderContent>((Func<XElement, ProviderContent>) (contentItem => EdsResponseParser.GetProviderContent(contentItem)));
        if (collection != null)
          return new List<ProviderContent>(collection);
      }
      return (List<ProviderContent>) null;
    }

    private static ProviderContent GetProviderContent(XElement item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      ProviderContent providerContent = new ProviderContent();
      providerContent.Device = LinqHelper.TryGetStringValue(item.Element(EdsConstants.Device));
      providerContent.OfferInstances = EdsResponseParser.GetOfferInstances(item);
      XElement xelement = item.Element(EdsConstants.VideoAttributes);
      if (xelement != null)
      {
        providerContent.HasClosedCaptioning = LinqHelper.TryGetBoolValue(xelement.Element(EdsConstants.ClosedCaptioning));
        providerContent.ResolutionFormat = LinqHelper.TryGetStringValue(xelement.Element(EdsConstants.ResolutionFormat));
        providerContent.VideoType = LinqHelper.TryGetStringValue(xelement.Element(EdsConstants.VideoType));
      }
      return providerContent;
    }

    private static List<OfferInstance> GetOfferInstances(XElement item)
    {
      XElement xelement = item != null ? item.Element(EdsConstants.OfferInstances) : throw new ArgumentNullException(nameof (item));
      if (xelement != null)
      {
        IEnumerable<OfferInstance> collection = xelement.Elements(EdsConstants.OfferInstance).Select<XElement, OfferInstance>((Func<XElement, OfferInstance>) (offerInstance => EdsResponseParser.GetOfferInstance(offerInstance)));
        if (collection != null)
          return new List<OfferInstance>(collection);
      }
      return (List<OfferInstance>) null;
    }

    private static OfferInstance GetOfferInstance(XElement item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      OfferInstance offerInstance = new OfferInstance();
      offerInstance.DisplayPrice = LinqHelper.TryGetStringValue(item.Element(EdsConstants.DisplayPrice));
      offerInstance.OfferType = LinqHelper.TryGetStringValue(item.Element(EdsConstants.OfferType));
      offerInstance.Price = LinqHelper.TryGetIntValue(item.Element(EdsConstants.Price));
      float result;
      if (!offerInstance.Price.HasValue && !string.IsNullOrWhiteSpace(offerInstance.DisplayPrice) && offerInstance.DisplayPrice.StartsWith("MPT", StringComparison.OrdinalIgnoreCase) && float.TryParse(offerInstance.DisplayPrice.Replace("MPT", string.Empty), out result))
        offerInstance.Price = new int?((int) result);
      return offerInstance;
    }

    private static List<string> GetGenres(XElement item)
    {
      XElement listElement = item != null ? item.Element(EdsConstants.Genres) : throw new ArgumentNullException(nameof (item));
      if (listElement != null)
      {
        List<string> scrubbedStringList1 = LinqHelper.TryGetScrubbedStringList(listElement, EdsConstants.Genre);
        if (scrubbedStringList1 != null && scrubbedStringList1.Count > 0)
          return scrubbedStringList1;
        List<string> scrubbedStringList2 = LinqHelper.TryGetScrubbedStringList(listElement, EdsConstants.GameGenre);
        if (scrubbedStringList2 != null)
          return scrubbedStringList2;
      }
      XElement xelement = item.Element(EdsConstants.Genre);
      if (xelement == null)
        return new List<string>();
      return new List<string>()
      {
        LinqHelper.TryGetScrubbedStringValue(xelement)
      };
    }

    private static XmlSerializableDictionary<Filter, int> GetResultsCounters(XElement resultsElement)
    {
      if (resultsElement == null)
        throw new ArgumentNullException(nameof (resultsElement));
      XmlSerializableDictionary<Filter, int> resultsCounters = new XmlSerializableDictionary<Filter, int>();
      int? nullable = new int?(0);
      foreach (XElement element in resultsElement.Elements())
      {
        int? intValue = LinqHelper.TryGetIntValue(element.Element(EdsConstants.Count));
        Filter filter = EdsResponseParser.ParseFilter(element.Element(EdsConstants.ItemTypeElement).Value);
        resultsCounters.Add(filter, intValue.HasValue ? intValue.Value : 0);
      }
      return resultsCounters;
    }

    private static string GetImageUrl(XElement item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      string imageUrl = (string) null;
      XElement entry = item.Element(EdsConstants.Image);
      if (entry != null)
        imageUrl = LinqHelper.GetValue(entry, EdsConstants.Url, (string) null);
      return imageUrl;
    }

    private static List<SearchData> GetSearchDataItems(XElement itemsElement)
    {
      List<SearchData> searchDataItems = new List<SearchData>();
      if (itemsElement != null)
      {
        IEnumerable<XElement> xelements = itemsElement.Elements(EdsConstants.Item);
        if (xelements != null)
        {
          foreach (XElement xelement in xelements)
          {
            SearchData searchData = EdsResponseParser.CreateSearchData(xelement);
            if (searchData != null)
              searchDataItems.Add(searchData);
          }
        }
      }
      return searchDataItems;
    }
  }
}
