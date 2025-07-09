// *********************************************************
// Type: LRC.UI.Omniture.OmnitureViewConstants
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll


namespace LRC.UI.Omniture
{
  public static class OmnitureViewConstants
  {
    public const string AppName = "xbox companion";
    public const string AppStartEventName = "lrc started";
    public const string AppStartTypeLaunch = "launch";
    public const string AppStartTypeRehydrate = "rehydrate";
    public const string AppStartTypeFas = "fast app switching";
    public const string AppUnhandledExceptionEventName = "lrc unhandled exception happened";
    public const string NotAvailable = "na";
    public const int RankNotAvailable = -1;
    public const string PlayedOnXboxEventName = "played on Xbox";
    public const string MediaItemSelectedEventName = "media selected";
    public const string NowPlayingEntryName = "nowplaying";
    public const string QuickplayEntryName = "quickplay";
    public const string FeaturedEntryName = "featured";
    public const string SearchEntryName = "search";
    public const string BrowseEntryName = "browse";
    public const string SpotlightEntryName = "spotlight";
    public const string PicksForMeEntryName = "picks for me";
    public const string RelatedEntryName = "related";
    public const string SessionStartEventName = "session start";
    public const string SessionTypeNew = "new session";
    public const string SessionTypeExisting = "existing session";
    public const string NameSeperator = ":";
    public const string DefaultPageName = "wp:lrc:page";
    public const string DefaultChannelName = "wp:lrc:channel";
    public const string ExitCurrentSessionEventName = "launch on xbox exit current session event";
    public const string ExitCurrentSessionEventVariable = "event18";
    public const string LaunchCanceledEventName = "launch on xbox cancel event";
    public const string LaunchCanceledEventVariable = "event19";
    public const string ConnectingChannelName = "wp:lrc:connecting";
    public const string WelcomePageName = "wp:lrc:connecting:welcome";
    public const string WelcomeNextButtonEventVariable = "event13";
    public const string WelcomeNextButtonEventName = "welcome next button clicked";
    public const string ConnectionSucessPageName = "wp:lrc:connecting:success";
    public const string ConnectionSuccessContinueButtonEventVariable = "event14";
    public const string ConnectionSuccessContinueButtonEventName = "connection success continue button clicked";
    public const string LookForXboxPageName = "wp:lrc:connecting:lookforxbox";
    public const string LookForXboxTryAgainButtonEventVarible = "event15";
    public const string LookForXboxTryAgainButtonEventName = "look for xbox try again button clicked";
    public const string SlowCloudPageName = "wp:lrc:connecting:slowcloud";
    public const string SlowCloudPageContinueButtonEventVariable = "event16";
    public const string SlowCloudPageContinueButtonEventName = "slow cloud continue button clicked";
    public const string LookForXboxErrorEventName = "look for xbox error event";
    public const string GameChannelName = "wp:lrc:game";
    public const string AchievementPageName = "wp:lrc:gamedetail:achievementdetail";
    public const string BeaconPageName = "wp:lrc:gamedetail:beaconsetedit";
    public const string GameDetailPageName = "wp:lrc:gamedetail";
    public const string SearchChannelName = "wp:lrc:search";
    public const string SearchResultsPageName = "wp:lrc:search:results";
    public const string SearchUsedEventName = "search used";
    public const string SearchTypeTextEntry = "text entry";
    public const string SearchTypePopularNow = "popular now";
    public const string SearchResultsYes = "yes";
    public const string SearchResultsNo = "no";
    public const string SearchFilterPageName = "wp:lrc:search:filter";
    public const string AlbumSearchDetailPageName = "wp:lrc:search:albumdetail";
    public const string AppSearchPageName = "wp:lrc:search:appdetail";
    public const string ArtistSearchDetailPageName = "wp:lrc:search:artistdetail";
    public const string GameContentSearchDetailPageName = "wp:lrc:search:gamecontentdetail";
    public const string MovieSearchDetailPageName = "wp:lrc:search:moviedetail";
    public const string TvEpisodeSearchDetailPageName = "wp:lrc:search:tvepisodedetail";
    public const string TvSeasonSearchDetailPageName = "wp:lrc:search:tvseasondetail";
    public const string TvSeriesSearchDetailPageName = "wp:lrc:search:tvseriesdetail";
    public const string ZuneMarketplaceChannelName = "wp:lrc:zunemarketplace";
    public const string ZuneCategoriesPageName = "wp:lrc:zunemarketplace:category";
    public const string ZuneCategoryContentPageName = "wp:lrc:zunemarketplace:category:content";
    public const string ZuneHomePageName = "wp:lrc:zunemarketplace:home";
    public const string ZuneHubPageName = "wp:lrc:zunemarketplace:hub";
    public const string ZuneMovieAndTvEpisodeDetailsPageName = "wp:lrc:zunemarketplace:movie&tvepisodedetails";
    public const string ZuneTvSeriesDetailsPageName = "wp:lrc:zunemarketplace:tvseriesdetails";
    public const string ZuneBrowsingEventName = "zune marketplace browsing event";
    public const string ZuneGenreStutioNetworkSelectedEventName = "zune genre/studio/network selected event";
    public const string AdvertChannelName = "wp:lrc:advertising";
    public const string FeaturedItemOverviewPageName = "wp:lrc:mainpage:featured:itemoverview";
    public const string ImageViewerChannelName = "wp:lrc:image";
    public const string ImageViewerPageName = "wp:lrc:imageviewer";
    public const string MainPageChannelName = "wp:lrc:mainpage";
    public const string MainPageName = "wp:lrc:mainpage";
    public const string NowPlayingClickedEventVariable = "event10";
    public const string NowPlayingClickedEventName = "now playing clicked";
    public const string LaunchTypePlain = "plain";
    public const string MediaTypeGame = "game";
    public const string MediaTypeGameContent = "gamecontent";
    public const string MediaTypeApp = "application";
    public const string MediaTypeHub = "hub";
    public const string MediaTypeMusic = "music";
    public const string MediaTypeMusicVideo = "musicvideo";
    public const string MediaTypeAlbum = "album";
    public const string MediaTypeArtist = "artist";
    public const string MediaTypeMovie = "movie";
    public const string MediaTypeTv = "tv";
    public const string PlayControlClickedEventName = "play control command used";

    public static string JoinNames(params string[] names) => string.Join(":", names);
  }
}
