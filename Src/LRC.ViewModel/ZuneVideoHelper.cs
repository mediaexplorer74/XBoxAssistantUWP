// *********************************************************
// Type: LRC.ViewModel.ZuneVideoHelper
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Xbox.Live.Phone.Utils;
using Xbox.Live.Phone.Utils.Linq;


namespace LRC.ViewModel
{
  public static class ZuneVideoHelper
  {
    private const int PhotoWidthThreshold = 400;
    private const string ComponentName = "ZuneVideoHelper";
    private const string LicenseFree = "FREE";
    private const string LicenseRightRent = "RENT";
    private const string LicenseRightRentStream = "RENTSTREAM";
    private const string LicenseRightPurchase = "PURCHASE";
    private const string LicenseRightPurchaseStream = "PURCHASESTREAM";
    private const int MaxPhotosToReturn = 16;

    public static IEnumerable<ViewModelBase> ParseByCategory(
      string result,
      MediaCategory category,
      MediaType type)
    {
      if (!string.IsNullOrEmpty(result))
      {
        switch (type)
        {
          case MediaType.movie:
            switch (category)
            {
              case MediaCategory.NewReleases:
                return ZuneVideoHelper.ParseNewRelease(result);
              case MediaCategory.TopPurchases:
                return ZuneVideoHelper.ParseTopEntries(result, type);
              case MediaCategory.TopRentals:
                return ZuneVideoHelper.ParseTopEntries(result, type);
              case MediaCategory.Genres:
                return ZuneVideoHelper.ParseCommonList(result, type, category);
              case MediaCategory.Studio:
                return ZuneVideoHelper.ParseCommonList(result, type, category);
            }
            break;
          case MediaType.tv_series:
            switch (category)
            {
              case MediaCategory.NewReleases:
                return ZuneVideoHelper.ParseNewRelease(result);
              case MediaCategory.TopPurchases:
                return ZuneVideoHelper.ParseTopEntries(result, type);
              case MediaCategory.Genres:
                return ZuneVideoHelper.ParseCommonList(result, type, category);
              case MediaCategory.Network:
                return ZuneVideoHelper.ParseCommonList(result, type, category);
            }
            break;
        }
      }
      return (IEnumerable<ViewModelBase>) null;
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "prevent random error crashes app")]
    public static XDocument SafeParseDocument(string result)
    {
      XDocument document = (XDocument) null;
      if (!string.IsNullOrWhiteSpace(result))
      {
        try
        {
          document = XDocument.Parse(result);
        }
        catch (XmlException ex)
        {
        }
        catch (Exception ex)
        {
        }
      }
      return document;
    }

    public static IEnumerable<ViewModelBase> ParseNewRelease(string result)
    {
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return (IEnumerable<ViewModelBase>) null;
      List<XElement> xelementList = new List<XElement>();
      IEnumerable<XElement> collection = document.Descendants(ZuneNamespace.Atom + "entry").Select<XElement, XElement>((Func<XElement, XElement>) (entry => entry));
      xelementList.AddRange(collection);
      return xelementList[1].Descendants(ZuneNamespace.ZuneDefaultNamespace + "feature").Select<XElement, ViewModelBase>((Func<XElement, ViewModelBase>) (entry => (ViewModelBase) ZuneVideoHelper.GetMediaItemEntry(entry)));
    }

    public static IEnumerable<ViewModelBase> ParseTopEntries(string result, MediaType type)
    {
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return (IEnumerable<ViewModelBase>) null;
      switch (type)
      {
        case MediaType.movie:
          return document.Descendants(ZuneNamespace.Atom + "entry").Select<XElement, ViewModelBase>((Func<XElement, ViewModelBase>) (entry => ZuneVideoHelper.ParseTopMovies(entry)));
        case MediaType.tv_series:
          return document.Descendants(ZuneNamespace.Atom + "entry").Select<XElement, ViewModelBase>((Func<XElement, ViewModelBase>) (entry => ZuneVideoHelper.ParseTopTvSeries(entry)));
        default:
          return (IEnumerable<ViewModelBase>) null;
      }
    }

    public static IEnumerable<ViewModelBase> ParseCommonList(
      string result,
      MediaType type,
      MediaCategory category)
    {
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      return document == null ? (IEnumerable<ViewModelBase>) null : document.Descendants(ZuneNamespace.Atom + "entry").Select<XElement, ViewModelBase>((Func<XElement, ViewModelBase>) (entry => ZuneVideoHelper.ParseCommonListEntry(entry, type, category)));
    }

    public static ViewModelBase ParseCommonListEntry(
      XElement entry,
      MediaType type,
      MediaCategory category)
    {
      if (entry == null)
        return (ViewModelBase) null;
      MediaItemList commonListEntry = new MediaItemList();
      commonListEntry.Type = type;
      commonListEntry.Title = LinqHelper.GetValue(entry, ZuneNamespace.Atom + "title", string.Empty);
      commonListEntry.Id = LinqHelper.GetValue(entry, ZuneNamespace.Atom + "id", string.Empty);
      commonListEntry.Category = category;
      return (ViewModelBase) commonListEntry;
    }

    public static void ParseCommonResponse(
      MediaItem item,
      XElement entry,
      XNamespace zuneNamespace)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      if (entry == null)
        throw new ArgumentNullException(nameof (entry));
      XNamespace atom = ZuneNamespace.Atom;
      item.Id = LinqHelper.GetValue(entry, atom + "id", string.Empty).Replace("urn:uuid:", string.Empty);
      item.Rating = LinqHelper.GetValue(entry, zuneNamespace + "rating", "NR");
      item.Description = LinqHelper.GetValue(entry, atom + "content", string.Empty);
      item.DetailTitle = entry.Element(atom + "title").Value;
      if (item.MediaType == MediaType.music_track)
      {
        item.DetailsImageUrl = ZuneServiceUtil.MediaTypeImageBaseUrl + ZuneServiceUtil.GetMediaPath(item.MediaType, item.Id) + "/albumImage?width=100&resize=true";
        item.ImageUrl = item.DetailsImageUrl;
      }
      else
      {
        item.DetailsImageUrl = ZuneServiceUtil.MediaTypeImageBaseUrl + ZuneServiceUtil.GetMediaPath(item.MediaType, item.Id) + "/primaryImage?width=200&resize=true";
        item.ImageUrl = item.DetailsImageUrl;
        item.BackgroundImageUrl = ZuneServiceUtil.MediaTypeImageBaseUrl + ZuneServiceUtil.GetMediaPath(item.MediaType, item.Id) + "/xboxBackgroundImage?height=720&resize=true";
      }
      if (item.IsUpdateable)
        item.Title = item.DetailTitle;
      IEnumerable<XElement> source = entry.Descendants(zuneNamespace + "title").Where<XElement>((Func<XElement, bool>) (el => el.Parent.Name == zuneNamespace + "genre"));
      if (source != null && source.Count<XElement>() > 0)
        item.Genre = (string) source.FirstOrDefault<XElement>();
      item.ReleaseDate = Convert.ToDateTime(LinqHelper.GetValue(entry, zuneNamespace + "releaseDate", DateTime.MinValue.ToString()), (IFormatProvider) CultureInfo.CurrentCulture).ToUniversalTime();
    }

    public static MediaItem GetMediaItemEntry(XElement entry)
    {
      if (entry == null)
        return (MediaItem) null;
      XNamespace defaultNamespace = ZuneNamespace.ZuneDefaultNamespace;
      string str1 = LinqHelper.GetValue(entry, defaultNamespace + "link", defaultNamespace + "type", string.Empty);
      MediaItem mediaItemEntry = (MediaItem) null;
      switch (str1.ToUpperInvariant())
      {
        case "SERIES":
          mediaItemEntry = (MediaItem) new TVMediaItem();
          mediaItemEntry.MediaType = MediaType.tv_series;
          break;
        case "MOVIE":
          mediaItemEntry = (MediaItem) new MovieMediaItem();
          mediaItemEntry.MediaType = MediaType.movie;
          break;
        case "HUB":
          mediaItemEntry = new MediaItem();
          mediaItemEntry.MediaType = MediaType.hub;
          break;
      }
      if (mediaItemEntry == null)
        return (MediaItem) null;
      string str2 = LinqHelper.GetValue(entry, defaultNamespace + "image", defaultNamespace + "id", string.Empty).Replace("urn:uuid:", string.Empty);
      mediaItemEntry.Id = LinqHelper.GetValue(entry, defaultNamespace + "link", defaultNamespace + "target", string.Empty);
      mediaItemEntry.Title = LinqHelper.GetValue(entry, defaultNamespace + "title", string.Empty);
      mediaItemEntry.ImageUrl = ZuneServiceUtil.ImageBaseUrl + str2 + "?width=173&resize=true";
      mediaItemEntry.DetailsImageUrl = ZuneServiceUtil.ImageBaseUrl + str2 + "?width=200&resize=true";
      return mediaItemEntry;
    }

    public static MediaItem GetMediaItem(string response, MediaType mediaType)
    {
      switch (mediaType)
      {
        case MediaType.movie:
          MovieMediaItem mediaItem1 = new MovieMediaItem();
          mediaItem1.IsUpdateable = true;
          ZuneVideoHelper.ParseMovieResponse(response, mediaItem1);
          return (MediaItem) mediaItem1;
        case MediaType.music_track:
          MusicTrackItem mediaItem2 = new MusicTrackItem();
          ZuneVideoHelper.ParseMusicTrackResponse(response, mediaItem2);
          return (MediaItem) mediaItem2;
        case MediaType.tv_episode:
          TVEpisodeItem mediaItem3 = new TVEpisodeItem();
          ZuneVideoHelper.ParseTvEpisodeResponse(response, mediaItem3);
          return (MediaItem) mediaItem3;
        case MediaType.music_musicvideo:
          MusicVideoItem mediaItem4 = new MusicVideoItem();
          ZuneVideoHelper.ParseMusicVideoResponse(response, mediaItem4);
          return (MediaItem) mediaItem4;
        default:
          XDocument document = ZuneVideoHelper.SafeParseDocument(response);
          if (document == null)
            return (MediaItem) null;
          XNamespace atom = ZuneNamespace.Atom;
          XElement entry = document.Element(atom + "entry") ?? document.Element(atom + "feed");
          MediaItem mediaItem5 = new MediaItem();
          mediaItem5.MediaType = mediaType;
          XNamespace defaultNamespace = entry.GetDefaultNamespace();
          mediaItem5.IsUpdateable = true;
          ZuneVideoHelper.ParseCommonResponse(mediaItem5, entry, defaultNamespace);
          return mediaItem5;
      }
    }

    public static ViewModelBase ParseTopMovies(XElement entry)
    {
      if (entry == null)
        return (ViewModelBase) null;
      XNamespace defaultNamespace = entry.GetDefaultNamespace();
      MovieMediaItem topMovies = new MovieMediaItem();
      topMovies.MediaType = MediaType.movie;
      topMovies.IsUpdateable = true;
      ZuneVideoHelper.ParseCommonResponse((MediaItem) topMovies, entry, defaultNamespace);
      topMovies.Studio = LinqHelper.GetValue(entry, defaultNamespace + "studioInfo", defaultNamespace + "name", string.Empty);
      topMovies.Duration = LinqHelper.TryGetTimeSpan(entry, defaultNamespace + "duration");
      topMovies.ReleaseDetails = ZuneVideoHelper.GetReleaseDetails((MediaItem) topMovies);
      return (ViewModelBase) topMovies;
    }

    public static ViewModelBase ParseTopTvSeries(XElement entry)
    {
      if (entry == null)
        return (ViewModelBase) null;
      XNamespace defaultNamespace = entry.GetDefaultNamespace();
      TVMediaItem topTvSeries = new TVMediaItem();
      topTvSeries.MediaType = MediaType.tv_series;
      topTvSeries.IsUpdateable = true;
      ZuneVideoHelper.ParseCommonResponse((MediaItem) topTvSeries, entry, defaultNamespace);
      return (ViewModelBase) topTvSeries;
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "I wish the right is simpler")]
    public static void ParseZuneVideoRights(XElement entry, MediaItem item)
    {
      if (entry == null || item == null)
        return;
      ProviderViewModel providerViewModel = new ProviderViewModel();
      switch (item.MediaType)
      {
        case MediaType.movie:
        case MediaType.tv_episode:
        case MediaType.tv_series:
        case MediaType.hub:
        case MediaType.movietrailer:
          providerViewModel.ImageUrl = "/UI/Images/Tile_Zune_Video.jpg";
          providerViewModel.ProviderName = Resource.ZuneVideo_ProviderName;
          break;
        case MediaType.music_album:
        case MediaType.music_track:
        case MediaType.music_musicvideo:
        case MediaType.music_artist:
        case MediaType.music_playlist:
          providerViewModel.ImageUrl = "/UI/Images/Tile_Zune_Music.jpg";
          providerViewModel.ProviderName = Resource.ZuneMusic_ProviderName;
          break;
        default:
          providerViewModel.ImageUrl = "/UI/Images/Tile_Zune.jpg";
          break;
      }
      ObservableCollection<ProviderViewModel> observableCollection = new ObservableCollection<ProviderViewModel>();
      observableCollection.Add(providerViewModel);
      XNamespace zuneNamespace = entry.GetDefaultNamespace();
      IEnumerable<XElement> xelements = entry.Descendants(zuneNamespace + "right").Where<XElement>((Func<XElement, bool>) (el => ZuneVideoHelper.SearchXBoxClient(el, zuneNamespace)));
      List<string> stringList = new List<string>();
      foreach (XElement entry1 in xelements)
      {
        string upperInvariant = LinqHelper.GetValue(entry1, zuneNamespace + "licenseRight", string.Empty).ToUpperInvariant();
        int? intValue = LinqHelper.TryGetIntValue(LinqHelper.GetValue(entry1, zuneNamespace + "price", string.Empty));
        if (intValue.HasValue && intValue.Value == 0)
          stringList.Add("FREE");
        if (!string.IsNullOrEmpty(upperInvariant) && !stringList.Contains(upperInvariant))
          stringList.Add(upperInvariant);
      }
      if (stringList.Count > 0)
      {
        providerViewModel.OfferDescription = !stringList.Contains("FREE") ? (stringList.Contains("RENT") || stringList.Contains("RENTSTREAM") ? Resource.ProviderOffer_Rent : (stringList.Contains("PURCHASE") || stringList.Contains("PURCHASESTREAM") ? Resource.ProviderOffer_Purchase : (string) null)) : Resource.ProviderOffer_Free;
        item.Providers = observableCollection;
      }
      else
        item.Providers = (ObservableCollection<ProviderViewModel>) null;
    }

    public static IEnumerable<MediaItem> GetMovieList(string result)
    {
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return (IEnumerable<MediaItem>) null;
      List<XElement> xelementList = new List<XElement>();
      IEnumerable<XElement> collection = document.Descendants(ZuneNamespace.Atom + "entry").Select<XElement, XElement>((Func<XElement, XElement>) (entry => entry));
      xelementList.AddRange(collection);
      return xelementList[0].Descendants(ZuneNamespace.ZuneDefaultNamespace + "feature").Select<XElement, MediaItem>((Func<XElement, MediaItem>) (entry => ZuneVideoHelper.GetMediaItemEntry(entry))).Where<MediaItem>((Func<MediaItem, bool>) (item => item != null));
    }

    public static IEnumerable<MediaItem> GetTvepisodeList(string result)
    {
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      return document == null ? (IEnumerable<MediaItem>) null : document.Descendants(ZuneNamespace.Atom + "entry").Select<XElement, MediaItem>((Func<XElement, MediaItem>) (entry => ZuneVideoHelper.GetTvEpisodeEntry(entry))).Where<MediaItem>((Func<MediaItem, bool>) (item => item != null));
    }

    public static string GetNextLinkForFeedList(string response)
    {
      XDocument document = ZuneVideoHelper.SafeParseDocument(response);
      if (document == null)
        return string.Empty;
      string str1 = document.Descendants(ZuneNamespace.Atom + "link").Where<XElement>((Func<XElement, bool>) (link => link.Attribute((XName) "rel").Value == "next")).Select<XElement, string>((Func<XElement, string>) (link => link.Attribute((XName) "href").Value)).FirstOrDefault<string>();
      if (string.IsNullOrEmpty(str1))
        return string.Empty;
      string str2 = str1.Substring(str1.IndexOf('=') + 1);
      return str2.Substring(0, str2.IndexOf('&'));
    }

    public static MediaItem GetMusicTrackItem(XElement entry)
    {
      if (entry == null)
        return (MediaItem) null;
      MusicTrackItem musicTrackItem = new MusicTrackItem();
      musicTrackItem.IsUpdateable = true;
      musicTrackItem.MediaType = MediaType.music_track;
      ZuneVideoHelper.ParseMusicTrackEntry(entry, musicTrackItem);
      return (MediaItem) musicTrackItem;
    }

    public static MediaItem GetAlbumEntry(XElement entry)
    {
      if (entry == null)
        return (MediaItem) null;
      AlbumItem albumItem = new AlbumItem();
      ZuneVideoHelper.ParseAlbumEntry(entry, albumItem);
      return albumItem.IsActionable ? (MediaItem) albumItem : (MediaItem) null;
    }

    public static void ParseAlbumEntry(XElement entry, AlbumItem item)
    {
      if (entry == null || item == null)
        return;
      XNamespace defaultNamespace = entry.GetDefaultNamespace();
      XNamespace atom = ZuneNamespace.Atom;
      item.Id = LinqHelper.GetValue(entry, atom + "id", string.Empty).Replace("urn:uuid:", string.Empty);
      item.DetailTitle = LinqHelper.GetValue(entry, atom + "title", string.Empty);
      item.Title = item.DetailTitle;
      item.IsActionable = Convert.ToBoolean(LinqHelper.GetValue(entry, defaultNamespace + "isActionable", "False"), (IFormatProvider) CultureInfo.InvariantCulture);
      item.Artist = LinqHelper.GetValue(entry, defaultNamespace + "primaryArtist", defaultNamespace + "name", string.Empty);
      item.ArtistId = LinqHelper.GetValue(entry, defaultNamespace + "primaryArtist", defaultNamespace + "id", string.Empty);
      item.Genre = LinqHelper.GetValue(entry, defaultNamespace + "primaryGenre", defaultNamespace + "title", string.Empty);
      IEnumerable<XElement> source = entry.Descendants(defaultNamespace + "imageInstance");
      if (source != null && source.Count<XElement>() > 0)
        item.ImageUrl = LinqHelper.GetValue(source.First<XElement>(), defaultNamespace + "url", string.Empty);
      else
        item.ImageUrl = ZuneServiceUtil.MediaTypeImageBaseUrl + ZuneServiceUtil.GetMediaPath(item.MediaType, item.Id) + "/primaryImage?width=100&resize=true";
    }

    public static MediaItem GetTvEpisodeEntry(XElement entry)
    {
      if (entry == null)
        return (MediaItem) null;
      TVEpisodeItem tvEpisodeEntry = new TVEpisodeItem();
      ZuneVideoHelper.ParseTvEpisodeEntry(entry, tvEpisodeEntry);
      return (MediaItem) tvEpisodeEntry;
    }

    public static void ParseTvEpisodeEntry(XElement entry, TVEpisodeItem item)
    {
      if (item == null || entry == null)
        return;
      item.IsUpdateable = true;
      item.MediaType = MediaType.tv_episode;
      XNamespace defaultNamespace = entry.GetDefaultNamespace();
      ZuneVideoHelper.ParseCommonResponse((MediaItem) item, entry, defaultNamespace);
      string str = LinqHelper.GetValue(entry, defaultNamespace + "episodeNumber", string.Empty);
      if (!string.IsNullOrWhiteSpace(str))
        item.EpisodeNumber = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.TVEpisodeTitle, new object[1]
        {
          (object) str
        });
      item.Studio = LinqHelper.GetValue(entry, defaultNamespace + "network", defaultNamespace + "title", string.Empty);
      item.Duration = LinqHelper.TryGetTimeSpan(entry, defaultNamespace + "length");
      item.ReleaseDetails = ZuneVideoHelper.GetReleaseDetails((MediaItem) item);
      item.SeriesTitle = LinqHelper.GetValue(entry, defaultNamespace + "series", defaultNamespace + "title", string.Empty);
      item.SeriesId = LinqHelper.GetValue(entry, defaultNamespace + "series", defaultNamespace + "id", string.Empty).Replace("urn:uuid:", string.Empty);
      item.SeasonNumber = LinqHelper.GetValue(entry, defaultNamespace + "seasonNumber", string.Empty);
      item.LastRefreshTime = DateTime.UtcNow;
    }

    public static void ParseTvEpisodeDetailResponseEntry(XElement entry, TVEpisodeItem item)
    {
      if (item == null || entry == null)
        return;
      item.IsUpdateable = false;
      item.MediaType = MediaType.tv_episode;
      XNamespace defaultNamespace = entry.GetDefaultNamespace();
      ZuneVideoHelper.ParseCommonResponse((MediaItem) item, entry, defaultNamespace);
      ZuneVideoHelper.ParseZuneVideoRights(entry, (MediaItem) item);
      string str = LinqHelper.GetValue(entry, defaultNamespace + "episodeNumber", string.Empty);
      if (!string.IsNullOrWhiteSpace(str))
        item.EpisodeNumber = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.ZuneVideoMarketPlace_TV_EpisodeNumber, new object[1]
        {
          (object) str
        });
      item.Studio = LinqHelper.GetValue(entry, defaultNamespace + "network", defaultNamespace + "title", string.Empty);
      IEnumerable<XElement> source = entry.Descendants(defaultNamespace + "categories").Select<XElement, XElement>((Func<XElement, XElement>) (node => node));
      item.Genre = LinqHelper.GetValue(source.FirstOrDefault<XElement>(), defaultNamespace + "category", defaultNamespace + "title", string.Empty);
      item.Duration = LinqHelper.TryGetTimeSpan(entry, defaultNamespace + "length");
      item.ReleaseDetails = ZuneVideoHelper.GetReleaseDetails((MediaItem) item);
      item.SeriesTitle = LinqHelper.GetValue(entry, defaultNamespace + "series", defaultNamespace + "title", string.Empty);
      item.SeriesId = LinqHelper.GetValue(entry, defaultNamespace + "series", defaultNamespace + "id", string.Empty).Replace("urn:uuid:", string.Empty);
      item.SeasonNumber = LinqHelper.GetValue(entry, defaultNamespace + "season", defaultNamespace + "number", string.Empty);
      item.LastRefreshTime = DateTime.UtcNow;
      item.Title = ZuneVideoHelper.ConstructTvEpisodePivotTitle(item.SeriesTitle, item.SeasonNumber);
    }

    public static string ConstructTvEpisodePivotTitle(string seriesTitle, string seasonNumber)
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.TVDetailsTitle, new object[2]
      {
        (object) seriesTitle,
        (object) seasonNumber
      });
    }

    public static void ParseMovieResponse(string result, MovieMediaItem mediaItem)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (mediaItem == null)
        return;
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return;
      XElement entry = document.Element(ZuneNamespace.Atom + "entry") ?? document.Element(ZuneNamespace.Atom + "feed");
      XNamespace zuneNamespace = entry.GetDefaultNamespace();
      ZuneVideoHelper.ParseCommonResponse((MediaItem) mediaItem, entry, zuneNamespace);
      mediaItem.Studio = LinqHelper.GetValue(entry, zuneNamespace + "studioInfo", zuneNamespace + "name", string.Empty);
      mediaItem.Duration = LinqHelper.TryGetTimeSpan(entry, zuneNamespace + "duration");
      mediaItem.ReleaseDetails = ZuneVideoHelper.GetReleaseDetails((MediaItem) mediaItem);
      mediaItem.Title = LinqHelper.GetValue(entry, ZuneNamespace.Atom + "title", string.Empty);
      IEnumerable<string> strings1 = entry.Descendants(zuneNamespace + "contributor").Where<XElement>((Func<XElement, bool>) (el => string.Compare(el.Element(zuneNamespace + "roleName").Value, "actor", StringComparison.OrdinalIgnoreCase) == 0)).Select<XElement, string>((Func<XElement, string>) (el => el.Element(zuneNamespace + "name").Value));
      if (strings1 != null && strings1.Count<string>() > 0)
        mediaItem.Actors = new ObservableCollection<string>(strings1);
      IEnumerable<string> strings2 = entry.Descendants(zuneNamespace + "contributor").Where<XElement>((Func<XElement, bool>) (el => string.Compare(el.Element(zuneNamespace + "roleName").Value, "director", StringComparison.OrdinalIgnoreCase) == 0)).Select<XElement, string>((Func<XElement, string>) (el => el.Element(zuneNamespace + "name").Value));
      if (strings2 != null && strings2.Count<string>() > 0)
        mediaItem.Directors = new ObservableCollection<string>(strings2);
      mediaItem.IsCastAndCrewNotFound = (mediaItem.Actors == null || mediaItem.Actors.Count == 0) && (mediaItem.Directors == null || mediaItem.Directors.Count == 0);
      ZuneVideoHelper.ParseZuneVideoRights(entry, (MediaItem) mediaItem);
    }

    public static void ParseTvEpisodeResponse(string result, TVEpisodeItem item)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (item == null)
        return;
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return;
      XNamespace atom = ZuneNamespace.Atom;
      XElement entry = document.Element(atom + "entry") ?? document.Element(atom + "feed");
      if (entry == null)
        return;
      ZuneVideoHelper.ParseTvEpisodeDetailResponseEntry(entry, item);
    }

    public static void ParseTvSeriesResponse(string result, TVMediaItem item)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null || item == null)
        return;
      XNamespace a = ZuneNamespace.Atom;
      XElement entry = document.Element(a + "entry") ?? document.Element(a + "feed");
      if (entry == null)
        return;
      XNamespace zuneNamespace = entry.GetDefaultNamespace();
      ZuneVideoHelper.ParseCommonResponse((MediaItem) item, entry, zuneNamespace);
      IEnumerable<TVMediaItem> source = entry.Descendants(a + "entry").Select<XElement, TVMediaItem>((Func<XElement, TVMediaItem>) (el =>
      {
        return new TVMediaItem()
        {
          ParentId = item.Id,
          Id = LinqHelper.GetValue(el, a + "id", string.Empty),
          Title = el.Element(a + "title").Value,
          Description = LinqHelper.GetValue(el, a + "content", string.Empty),
          ReleaseDate = Convert.ToDateTime(LinqHelper.GetValue(el, zuneNamespace + "releaseDate", DateTime.MinValue.ToString()), (IFormatProvider) CultureInfo.CurrentCulture),
          Rating = LinqHelper.GetValue(el, zuneNamespace + "rating", "NR"),
          ImageUrl = ZuneServiceUtil.ImageBaseUrl + LinqHelper.GetValue(el, zuneNamespace + "image", string.Empty).Replace("urn:uuid:", string.Empty) + "?width=100&height=100",
          DetailsImageUrl = ZuneServiceUtil.ImageBaseUrl + LinqHelper.GetValue(el, zuneNamespace + "image", string.Empty).Replace("urn:uuid:", string.Empty) + "?width=173&height=173",
          MediaType = item.MediaType
        };
      }));
      if (source == null)
        return;
      List<TVMediaItem> list = source.ToList<TVMediaItem>();
      list.Sort(new Comparison<TVMediaItem>(ZuneVideoHelper.CompareSeason));
      ObservableCollection<MediaItem> observableCollection = new ObservableCollection<MediaItem>();
      foreach (MediaItem mediaItem in list)
        observableCollection.Add(mediaItem);
      item.Seasons = observableCollection;
    }

    public static void ParseMusicTrackResponse(string result, MusicTrackItem item)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return;
      XNamespace atom = ZuneNamespace.Atom;
      ZuneVideoHelper.ParseMusicTrackEntry(document.Element(atom + "entry") ?? document.Element(atom + "feed"), item);
    }

    public static void ParseMusicVideoResponse(string result, MusicVideoItem item)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return;
      XNamespace atom = ZuneNamespace.Atom;
      ZuneVideoHelper.ParseMusicVideoEntry(document.Element(atom + "entry") ?? document.Element(atom + "feed"), item);
    }

    public static string ParseAlbumReview(string result)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return string.Empty;
      XNamespace atom = ZuneNamespace.Atom;
      return ZuneVideoHelper.RemoveHtmlTag(LinqHelper.GetValue(document.Element(atom + "entry"), atom + "content", string.Empty));
    }

    public static MediaType GetMediaType(string response)
    {
      XDocument document = ZuneVideoHelper.SafeParseDocument(response);
      if (document == null)
        return MediaType.undefined;
      XElement entry = document.Element(ZuneNamespace.Atom + "entry");
      if (entry == null)
        return MediaType.undefined;
      XNamespace defaultNamespace = entry.GetDefaultNamespace();
      return ZuneServiceUtil.ParseMediaType(LinqHelper.GetValue(entry, defaultNamespace + "type", string.Empty));
    }

    public static string ParseArtistBio(string result)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return string.Empty;
      XNamespace atom = ZuneNamespace.Atom;
      return ZuneVideoHelper.RemoveHtmlTag(LinqHelper.GetValue(document.Element(atom + "entry"), atom + "content", string.Empty));
    }

    public static ObservableCollection<string> GetAlbumPhotos(string result)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return (ObservableCollection<string>) null;
      XNamespace atom = ZuneNamespace.Atom;
      XElement xelement = document.Element(atom + "feed");
      if (xelement == null)
        return (ObservableCollection<string>) null;
      XNamespace defaultNamespace = xelement.GetDefaultNamespace();
      ObservableCollection<string> albumPhotos = new ObservableCollection<string>();
      try
      {
        foreach (string str in xelement.Descendants(atom + "entry").Descendants<XElement>(defaultNamespace + "instances").Select<XElement, string>((Func<XElement, string>) (instance => ZuneVideoHelper.GetImageUrlOfProperSize(instance, 400))))
        {
          if (!string.IsNullOrWhiteSpace(str))
            albumPhotos.Add(str);
          if (albumPhotos.Count >= 16)
            break;
        }
      }
      catch (InvalidOperationException ex)
      {
      }
      catch (NullReferenceException ex)
      {
      }
      return albumPhotos;
    }

    public static ObservableCollection<MediaItem> GetAlbumRelated(string result)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return (ObservableCollection<MediaItem>) null;
      XNamespace atom = ZuneNamespace.Atom;
      XElement xelement = document.Element(atom + "feed");
      if (xelement == null)
        return (ObservableCollection<MediaItem>) null;
      IEnumerable<MediaItem> source = xelement.Descendants(atom + "entry").Select<XElement, MediaItem>((Func<XElement, MediaItem>) (entry => ZuneVideoHelper.GetAlbumEntry(entry)));
      ObservableCollection<MediaItem> albumRelated = new ObservableCollection<MediaItem>();
      if (source == null)
        return (ObservableCollection<MediaItem>) null;
      foreach (MediaItem mediaItem in source.Where<MediaItem>((Func<MediaItem, bool>) (mi => mi != null)).Take<MediaItem>(10))
        albumRelated.Add(mediaItem);
      return albumRelated;
    }

    public static ObservableCollection<MediaItem> GetAlbumsOfSameArtist(string response)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      XDocument document = ZuneVideoHelper.SafeParseDocument(CleanXmlUtil.TrimPrefixXml(response));
      if (document == null)
        return (ObservableCollection<MediaItem>) null;
      XNamespace atom = ZuneNamespace.Atom;
      XElement xelement = document.Element(atom + "feed");
      if (xelement == null)
        return (ObservableCollection<MediaItem>) null;
      IEnumerable<MediaItem> mediaItems = xelement.Descendants(atom + "entry").Select<XElement, MediaItem>((Func<XElement, MediaItem>) (entry => ZuneVideoHelper.GetAlbumEntry(entry)));
      ObservableCollection<MediaItem> albumsOfSameArtist = new ObservableCollection<MediaItem>();
      foreach (MediaItem mediaItem in mediaItems)
      {
        if (mediaItem != null)
          albumsOfSameArtist.Add(mediaItem);
      }
      return albumsOfSameArtist;
    }

    public static void ParseAlbumResponse(string result, AlbumItem item)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (string.IsNullOrEmpty(result))
        return;
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return;
      XNamespace atom = ZuneNamespace.Atom;
      XElement entry = document.Element(atom + "feed");
      if (entry == null)
        return;
      XNamespace defaultNamespace = entry.GetDefaultNamespace();
      ZuneVideoHelper.ParseCommonResponse((MediaItem) item, entry, defaultNamespace);
      item.Label = LinqHelper.GetValue(entry, defaultNamespace + "label", string.Empty);
      item.IsExplicit = Convert.ToBoolean(LinqHelper.GetValue(entry, defaultNamespace + "isExplicit", "False"), (IFormatProvider) CultureInfo.InvariantCulture);
      item.Genre = LinqHelper.GetValue(entry, defaultNamespace + "primaryGenre", defaultNamespace + "title", string.Empty);
      item.Artist = LinqHelper.GetValue(entry, defaultNamespace + "primaryArtist", defaultNamespace + "name", string.Empty);
      item.ArtistId = LinqHelper.GetValue(entry, defaultNamespace + "primaryArtist", defaultNamespace + "id", string.Empty).Replace("urn:uuid:", string.Empty);
      item.ReviewUrl = ZuneServiceUtil.GetRequestUrl(MediaType.music_album, item.Id, InfoType.review);
      item.BioUrl = ZuneServiceUtil.GetRequestUrl(MediaType.music_artist, item.ArtistId, InfoType.biography);
      item.PhotoUrl = ZuneServiceUtil.GetRequestUrl(MediaType.music_artist, item.ArtistId, InfoType.images);
      item.RelatedUrl = ZuneServiceUtil.GetRequestUrl(MediaType.music_album, item.Id, InfoType.relatedAlbums);
      item.AlbumsBySameArtistUrl = ZuneServiceUtil.GetRequestUrl(MediaType.music_artist, item.ArtistId, InfoType.albums);
      item.ReleaseDetails = ZuneVideoHelper.GetAlbumReleaseDetails(item);
      ObservableCollection<ProviderViewModel> observableCollection = new ObservableCollection<ProviderViewModel>();
      ProviderViewModel providerViewModel1 = new ProviderViewModel();
      providerViewModel1.ProviderName = Resource.ZuneMusic_ProviderName;
      providerViewModel1.ImageUrl = "/UI/Images/Tile_Zune_Music.jpg";
      ProviderViewModel providerViewModel2 = providerViewModel1;
      observableCollection.Add(providerViewModel2);
      item.Providers = observableCollection;
      item.BackgroundImageUrl = ZuneServiceUtil.GetRequestUrl(MediaType.music_artist, item.ArtistId, InfoType.backgroundImage);
      IEnumerable<MediaItem> tracks = entry.Descendants(ZuneNamespace.Atom + "entry").Select<XElement, MediaItem>((Func<XElement, MediaItem>) (track => ZuneVideoHelper.GetMusicTrackItem(track)));
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        foreach (ViewModelBase viewModelBase in tracks)
          item.Items.Add(viewModelBase);
      }, (object) null);
    }

    private static int CompareSeason(TVMediaItem item1, TVMediaItem item2)
    {
      if (item1 == null)
        return item2 == null ? 0 : -1;
      if (item2 == null)
        return 1;
      int? intValue1 = LinqHelper.TryGetIntValue(item1.Id);
      int? intValue2 = LinqHelper.TryGetIntValue(item2.Id);
      return !intValue1.HasValue ? (!intValue2.HasValue ? string.Compare(item1.Id, item2.Id, StringComparison.OrdinalIgnoreCase) : 1) : (!intValue2.HasValue ? -1 : intValue2.Value.CompareTo(intValue1.Value));
    }

    private static bool SearchXBoxClient(XElement el, XNamespace zuneNamespace)
    {
      return el.Descendants(zuneNamespace + "clientType").Where<XElement>((Func<XElement, bool>) (clientType => string.Compare(clientType.Value, "XBox360", StringComparison.OrdinalIgnoreCase) == 0)).Count<XElement>() > 0;
    }

    private static void ParseMusicTrackEntry(XElement entry, MusicTrackItem item)
    {
      if (entry == null || item == null)
        return;
      XNamespace defaultNamespace = entry.GetDefaultNamespace();
      ZuneVideoHelper.ParseCommonResponse((MediaItem) item, entry, defaultNamespace);
      item.LabelOwner = LinqHelper.GetValue(entry, defaultNamespace + "labelOwner", string.Empty);
      item.ReleaseDate = Convert.ToDateTime(LinqHelper.GetValue(entry, defaultNamespace + "startDate", DateTime.MinValue.ToString()), (IFormatProvider) CultureInfo.CurrentCulture);
      item.DiskNumber = Convert.ToInt32(LinqHelper.GetValue(entry, defaultNamespace + "diskNumber", "1"), (IFormatProvider) CultureInfo.InvariantCulture);
      item.IsExplicit = Convert.ToBoolean(LinqHelper.GetValue(entry, defaultNamespace + "isExplicit", "False"), (IFormatProvider) CultureInfo.InvariantCulture);
      item.Duration = LinqHelper.TryGetTimeSpan(entry, defaultNamespace + "length");
      item.ReleaseDetails = ZuneVideoHelper.GetReleaseDetails((MediaItem) item);
      item.Album = LinqHelper.GetValue(entry, defaultNamespace + "album", defaultNamespace + "title", string.Empty);
      item.AlbumId = LinqHelper.GetValue(entry, defaultNamespace + "album", defaultNamespace + "id", string.Empty).Replace("urn:uuid:", string.Empty);
      item.Artist = LinqHelper.GetValue(entry, defaultNamespace + "primaryArtist", defaultNamespace + "name", string.Empty);
      item.ArtistId = LinqHelper.GetValue(entry, defaultNamespace + "primaryArtist", defaultNamespace + "id", string.Empty).Replace("urn:uuid:", string.Empty);
      item.Genre = LinqHelper.GetValue(entry, defaultNamespace + "primaryGenre", defaultNamespace + "title", string.Empty);
      string elementValue = LinqHelper.TryGetElementValue(entry, defaultNamespace + "trackNumber");
      if (string.IsNullOrEmpty(elementValue))
      {
        item.TrackNumber = 1;
        item.Title = item.DetailTitle;
      }
      else
      {
        item.TrackNumber = Convert.ToInt32(elementValue, (IFormatProvider) CultureInfo.InvariantCulture);
        item.Title = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Album_DisplayedTrackName, new object[2]
        {
          (object) elementValue,
          (object) item.DetailTitle
        });
      }
    }

    private static void ParseMusicVideoEntry(XElement entry, MusicVideoItem item)
    {
      if (entry == null || item == null)
        return;
      XNamespace defaultNamespace = entry.GetDefaultNamespace();
      ZuneVideoHelper.ParseCommonResponse((MediaItem) item, entry, defaultNamespace);
      item.Title = item.DetailTitle;
      item.LabelOwner = LinqHelper.GetValue(entry, defaultNamespace + "labelOwner", string.Empty);
      item.ReleaseDate = Convert.ToDateTime(LinqHelper.GetValue(entry, defaultNamespace + "startDate", DateTime.MinValue.ToString()), (IFormatProvider) CultureInfo.CurrentCulture);
      item.IsExplicit = Convert.ToBoolean(LinqHelper.GetValue(entry, defaultNamespace + "isExplicit", "False"), (IFormatProvider) CultureInfo.InvariantCulture);
      item.Duration = LinqHelper.TryGetTimeSpan(entry, defaultNamespace + "length");
      item.ReleaseDetails = ZuneVideoHelper.GetReleaseDetails((MediaItem) item);
      item.Album = LinqHelper.GetValue(entry, defaultNamespace + "album", defaultNamespace + "title", string.Empty);
      item.AlbumId = LinqHelper.GetValue(entry, defaultNamespace + "album", defaultNamespace + "id", string.Empty).Replace("urn:uuid:", string.Empty);
      item.Artist = LinqHelper.GetValue(entry, defaultNamespace + "primaryArtist", defaultNamespace + "name", string.Empty);
      item.ArtistId = LinqHelper.GetValue(entry, defaultNamespace + "primaryArtist", defaultNamespace + "id", string.Empty).Replace("urn:uuid:", string.Empty);
      item.TrackId = LinqHelper.GetValue(entry, defaultNamespace + "trackId", string.Empty).Replace("urn:uuid:", string.Empty);
      item.Genre = LinqHelper.GetValue(entry, defaultNamespace + "primaryGenre", defaultNamespace + "title", string.Empty);
    }

    private static string RemoveHtmlTag(string html)
    {
      return string.IsNullOrEmpty(html) ? string.Empty : Regex.Replace(HttpUtility.HtmlDecode(html), "<.+?>|\\r", string.Empty).Replace("&apos;", "'");
    }

    private static string GetReleaseDetails(MediaItem item)
    {
      List<string> values = new List<string>();
      if (item != null)
      {
        if (item.ReleaseDate.Ticks > 0L)
          values.Add(item.ReleaseDate.Year.ToString((IFormatProvider) CultureInfo.CurrentCulture));
        if (!string.IsNullOrEmpty(item.Rating))
          values.Add(item.Rating);
        if (item.Duration.Ticks > 0L)
          values.Add(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.DurationInMinutes, new object[1]
          {
            (object) (int) item.Duration.TotalMinutes
          }));
      }
      return string.Join(Resource.Search_DetailsSeparator, (IEnumerable<string>) values);
    }

    private static string GetAlbumReleaseDetails(AlbumItem item)
    {
      List<string> values = new List<string>();
      if (item != null)
      {
        if (!string.IsNullOrEmpty(item.Artist))
          values.Add(item.Artist);
        if (item.ReleaseDate.Ticks > 0L)
          values.Add(item.ReleaseDate.Year.ToString((IFormatProvider) CultureInfo.CurrentCulture));
      }
      return string.Join(Resource.Search_DetailsSeparator, (IEnumerable<string>) values);
    }

    private static string GetImageUrlOfProperSize(XElement entry, int threshold)
    {
      XNamespace zuneNamespace = entry.GetDefaultNamespace();
      List<ZunePhotoDefinition> list = entry.Descendants(ZuneNamespace.ZuneDefaultNamespace + "imageInstance").Select<XElement, ZunePhotoDefinition>((Func<XElement, ZunePhotoDefinition>) (instance => new ZunePhotoDefinition()
      {
        Url = LinqHelper.GetValue(instance, zuneNamespace + "url", string.Empty),
        Height = LinqHelper.GetIntValue(instance, zuneNamespace + "height", 0),
        Width = LinqHelper.GetIntValue(instance, zuneNamespace + "width", 0)
      })).ToList<ZunePhotoDefinition>();
      list.Sort((Comparison<ZunePhotoDefinition>) ((img1, img2) => img1.Width == img2.Width ? img1.Height.CompareTo(img2.Height) : img1.Width.CompareTo(img2.Width)));
      foreach (ZunePhotoDefinition zunePhotoDefinition in list)
      {
        if (zunePhotoDefinition.Width > threshold && ZuneVideoHelper.Is16By9(zunePhotoDefinition.Width, zunePhotoDefinition.Height))
          return zunePhotoDefinition.Url;
      }
      return (string) null;
    }

    private static bool Is16By9(int width, int height)
    {
      if (height == 0)
        return false;
      float num = (float) width / (float) height;
      return (double) num > 1.7 && (double) num < 1.8;
    }
  }
}
