// *********************************************************
// Type: LRC.ViewModel.GetMediaInfoUtil
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Service;
using LRC.Service.Search;
using System;
using System.Globalization;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  public class GetMediaInfoUtil
  {
    private const string ComponentName = "GetMediaInfoUtil";
    private const string MediaIdPrefixForTitleId = "66acd000-77fe-1000-9115-D802";

    public event EventHandler<ServiceProxyEventArgs<MediaItem>> EventGetMediaItemCompleted;

    public event EventHandler<ServiceProxyEventArgs<NowPlayingItemViewModel>> EventGetNowPlayingItemCompleted;

    public static uint GetTitleIdFromString(string titleIdString)
    {
      if (string.IsNullOrEmpty(titleIdString))
        return 0;
      if (titleIdString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        titleIdString = titleIdString.Substring(2);
      uint result;
      if (!uint.TryParse(titleIdString, NumberStyles.HexNumber, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result))
        result = 0U;
      return result;
    }

    public static string GetMediaIdFromTitleId(uint titleId)
    {
      return "66acd000-77fe-1000-9115-D802" + titleId.ToString("x", (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public void GetNowPlayingItemDetailsAsync(uint titleId, string mediaId)
    {
      if (string.IsNullOrEmpty(mediaId))
        throw new ArgumentNullException(nameof (mediaId));
      if (titleId == 1481115739U)
        this.GetMediaInfoFromZune(mediaId);
      else
        this.GetMediaInfoFromSearch(titleId, mediaId);
    }

    private static NowPlayingItemViewModel CreateNowPlayingItem(MediaItem mediaItem)
    {
      if (mediaItem == null)
        return (NowPlayingItemViewModel) null;
      NowPlayingItemViewModel nowPlayingItem = new NowPlayingItemViewModel();
      nowPlayingItem.LastRefreshTime = DateTime.UtcNow;
      nowPlayingItem.BackgroundImageUrl = mediaItem.BackgroundImageUrl;
      nowPlayingItem.Description = mediaItem.Description;
      nowPlayingItem.ImageUrl = mediaItem.ImageUrl;
      nowPlayingItem.ItemType = GetMediaInfoUtil.GetItemTypeFromMediaType(mediaItem.MediaType);
      nowPlayingItem.PartnerMediaId = mediaItem.Id;
      nowPlayingItem.Title = mediaItem.Title;
      nowPlayingItem.SubTitle = mediaItem.DetailTitle;
      if (mediaItem is MusicTrackItem musicTrackItem)
        nowPlayingItem.ParentMediaId = musicTrackItem.AlbumId;
      if (mediaItem is MusicVideoItem musicVideoItem)
        nowPlayingItem.ParentMediaId = musicVideoItem.AlbumId;
      return nowPlayingItem;
    }

    private static NowPlayingItemViewModel CreateNowPlayingItem(SearchData searchData)
    {
      if (searchData == null)
        return (NowPlayingItemViewModel) null;
      NowPlayingItemViewModel nowPlayingItem = new NowPlayingItemViewModel();
      nowPlayingItem.LastRefreshTime = DateTime.UtcNow;
      nowPlayingItem.BackgroundImageUrl = searchData.BackgroundImageUrl;
      nowPlayingItem.Description = searchData.Description;
      nowPlayingItem.ImageUrl = searchData.ImageUrl;
      nowPlayingItem.ItemType = GetMediaInfoUtil.GetItemTypeFromFilter(searchData.Filter);
      nowPlayingItem.MediaId = searchData.Id;
      nowPlayingItem.DetailsUrl = searchData.DetailsUrl;
      nowPlayingItem.Title = searchData.Name;
      return nowPlayingItem;
    }

    private static NowPlayingType GetItemTypeFromMediaType(MediaType mediaType)
    {
      NowPlayingType typeFromMediaType = NowPlayingType.Application;
      switch (mediaType)
      {
        case MediaType.movie:
        case MediaType.movietrailer:
          typeFromMediaType = NowPlayingType.Movie;
          break;
        case MediaType.music_track:
          typeFromMediaType = NowPlayingType.Music;
          break;
        case MediaType.tv_episode:
          typeFromMediaType = NowPlayingType.TvEpisode;
          break;
        case MediaType.music_musicvideo:
          typeFromMediaType = NowPlayingType.MusicVideo;
          break;
      }
      return typeFromMediaType;
    }

    private static NowPlayingType GetItemTypeFromFilter(LRC.Service.Search.Filter filter)
    {
      NowPlayingType itemTypeFromFilter = NowPlayingType.Application;
      switch (filter)
      {
        case LRC.Service.Search.Filter.Movies:
          itemTypeFromFilter = NowPlayingType.Movie;
          break;
        case LRC.Service.Search.Filter.Music:
          itemTypeFromFilter = NowPlayingType.Music;
          break;
        case LRC.Service.Search.Filter.TV:
          itemTypeFromFilter = NowPlayingType.TvEpisode;
          break;
        case LRC.Service.Search.Filter.Games:
          itemTypeFromFilter = NowPlayingType.Game;
          break;
        case LRC.Service.Search.Filter.Apps:
          itemTypeFromFilter = NowPlayingType.Application;
          break;
      }
      return itemTypeFromFilter;
    }

    private void GetMediaInfoFromZune(string mediaId)
    {
      IZuneCatalogServiceManager catalogServiceManager = ServiceManagerFactory.CreateZuneCatalogServiceManager();
      catalogServiceManager.EventGetMediaItemTypeCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.ZuneManager_EventGetMediaItemTypeCompleted);
      catalogServiceManager.GetMediaItemType(mediaId, (object) mediaId);
    }

    private void ZuneManager_EventGetMediaItemTypeCompleted(
      object sender,
      ServiceProxyEventArgs<string> e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is IZuneCatalogServiceManager catalogServiceManager1)
        catalogServiceManager1.EventGetMediaItemTypeCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(this.ZuneManager_EventGetMediaItemTypeCompleted);
      bool flag = true;
      if (e.Error == null && !string.IsNullOrEmpty(e.Result))
      {
        string userState = e.UserState as string;
        MediaType mediaType = ZuneVideoHelper.GetMediaType(e.Result);
        switch (mediaType)
        {
          case MediaType.movie:
          case MediaType.music_track:
          case MediaType.tv_episode:
          case MediaType.music_musicvideo:
          case MediaType.movietrailer:
            if (flag)
            {
              IZuneCatalogServiceManager catalogServiceManager2 = ServiceManagerFactory.CreateZuneCatalogServiceManager();
              catalogServiceManager2.EventGetVideoItemCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.ZuneManager_EventGetMediaDetailCompleted);
              catalogServiceManager2.GetMediaDetail(userState, mediaType.ToString(), (object) mediaType);
              break;
            }
            break;
          default:
            flag = false;
            goto case MediaType.movie;
        }
      }
      else
        flag = e.Error == null && false;
      if (flag || this.EventGetMediaItemCompleted == null)
        return;
      this.FireCompletedNotification((MediaItem) null, e.Error);
    }

    private void ZuneManager_EventGetMediaDetailCompleted(
      object sender,
      ServiceProxyEventArgs<string> e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is IZuneCatalogServiceManager catalogServiceManager)
        catalogServiceManager.EventGetVideoItemCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(this.ZuneManager_EventGetMediaDetailCompleted);
      MediaItem mediaItem = (MediaItem) null;
      if (e.Error == null && !string.IsNullOrEmpty(e.Result))
      {
        MediaType userState = (MediaType) e.UserState;
        mediaItem = ZuneVideoHelper.GetMediaItem(e.Result, userState);
      }
      else
      {
        Exception error = e.Error;
      }
      this.FireCompletedNotification(mediaItem, e.Error);
    }

    private void FireCompletedNotification(MediaItem mediaItem, Exception error)
    {
      NowPlayingItemViewModel nowPlayingItem = GetMediaInfoUtil.CreateNowPlayingItem(mediaItem);
      EventHandler<ServiceProxyEventArgs<NowPlayingItemViewModel>> playingItemCompleted = this.EventGetNowPlayingItemCompleted;
      if (playingItemCompleted == null)
        return;
      playingItemCompleted((object) this, new ServiceProxyEventArgs<NowPlayingItemViewModel>((object) nowPlayingItem, error, false, (object) null));
    }

    private void GetMediaInfoFromSearch(uint titleId, string mediaId)
    {
      ISearchServiceManager searchServiceManager = ServiceManagerFactory.CreateSearchServiceManager();
      searchServiceManager.EventGetNowPlayingItemDetailsCompleted += new EventHandler<ServiceProxyEventArgs<SearchData>>(this.SearchServiceManager_EventGetNowPlayingItemDetailsCompleted);
      searchServiceManager.GetNowPlayingItemDetails(titleId, mediaId, MainViewModel.Instance.ConsoleLiveTVProviderTitleId, (object) mediaId);
    }

    private void SearchServiceManager_EventGetNowPlayingItemDetailsCompleted(
      object sender,
      ServiceProxyEventArgs<SearchData> e)
    {
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetNowPlayingItemDetailsCompleted -= new EventHandler<ServiceProxyEventArgs<SearchData>>(this.SearchServiceManager_EventGetNowPlayingItemDetailsCompleted);
      NowPlayingItemViewModel objectResult = (NowPlayingItemViewModel) null;
      if (e.Error == null && e.Result != null)
      {
        objectResult = GetMediaInfoUtil.CreateNowPlayingItem(e.Result);
      }
      else
      {
        Exception error = e.Error;
      }
      EventHandler<ServiceProxyEventArgs<NowPlayingItemViewModel>> playingItemCompleted = this.EventGetNowPlayingItemCompleted;
      if (playingItemCompleted == null)
        return;
      playingItemCompleted((object) this, new ServiceProxyEventArgs<NowPlayingItemViewModel>((object) objectResult, e.Error, false, (object) null));
    }
  }
}
