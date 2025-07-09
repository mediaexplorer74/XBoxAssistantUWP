// *********************************************************
// Type: LRC.NavHelper
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service;
using LRC.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace LRC
{
  public static class NavHelper
  {
    public const string LookForXboxPageUriPath = "/UI/Views/Connecting/LookForXboxPage.xaml";
    public const string HowToConnectPageUriPath = "/UI/Views/Connecting/HowToConnectPage.xaml";
    public const string ShouldGoBackParam = "ShouldGoBack";
    private const string ComponentName = "NavHelper";
    private const string PageCounterParam = "&Counter=";
    private const string LookForXboxPageShouldGoBackUriString = "/UI/Views/Connecting/LookForXboxPage.xaml?ShouldGoBack=true";
    private static int pageCounter;

    public static Uri MainPageUri => new Uri("/UI/Views/MainPage.xaml", UriKind.Relative);

    public static Uri BeaconSetEditPageUri
    {
      get => new Uri("/UI/Views/Games/BeaconSetEditPage.xaml", UriKind.Relative);
    }

    public static Uri AchievementDetailsPage
    {
      get => new Uri("/UI/Views/Games/AchievementDetailsPage.xaml", UriKind.Relative);
    }

    public static Uri ImageViewerUri => new Uri("/UI/Views/ImageViewer.xaml", UriKind.Relative);

    public static Uri ZuneTvSeasonDetailsPage
    {
      get => new Uri("/UI/Views/ZuneMarketplace/ZuneHubPage.xaml", UriKind.Relative);
    }

    public static Uri FeaturedItemOverviewPage
    {
      get => new Uri("/UI/Views/FeaturedItemOverviewPage.xaml", UriKind.Relative);
    }

    public static void SafeNavigate(Page sourcePage, Uri page)
    {
      NavHelper.SafeNavigate(sourcePage, page, false);
    }

    public static void SafeGoBack(Page sourcePage)
    {
      if (sourcePage == null)
        throw new ArgumentNullException(nameof (sourcePage));
      try
      {
        if (!sourcePage.NavigationService.CanGoBack)
          return;
        sourcePage.NavigationService.GoBack();
      }
      catch (InvalidOperationException ex)
      {
      }
    }

    public static void SafeNavigate(Page sourcePage, Uri page, bool enableBusyOverlay)
    {
      if (sourcePage == null)
        throw new ArgumentNullException(nameof (sourcePage));
      if (page == (Uri) null)
        throw new ArgumentNullException(nameof (page));
      if (enableBusyOverlay)
      {
        App.SetGlobalBusyOverlay(true);
        sourcePage.NavigationService.Navigated += new NavigatedEventHandler(NavHelper.NavigationService_Navigated);
      }
      try
      {
        sourcePage.NavigationService.Navigate(page);
      }
      catch (InvalidOperationException ex)
      {
        if (!enableBusyOverlay)
          return;
        App.SetGlobalBusyOverlay(false);
        sourcePage.NavigationService.Navigated -= new NavigatedEventHandler(NavHelper.NavigationService_Navigated);
      }
    }

    public static void NavigationService_Navigated(object sender, EventArgs e)
    {
      if (sender is NavigationService navigationService)
        navigationService.Navigated -= new NavigatedEventHandler(NavHelper.NavigationService_Navigated);
      App.SetGlobalBusyOverlay(false);
    }

    public static void GotoLookForXboxPage(Page sourcePage, bool shouldGoBackWhenSucceed)
    {
      Uri page = !shouldGoBackWhenSucceed ? new Uri("/UI/Views/Connecting/LookForXboxPage.xaml", UriKind.Relative) : new Uri("/UI/Views/Connecting/LookForXboxPage.xaml?ShouldGoBack=true", UriKind.Relative);
      NavHelper.SafeNavigate(sourcePage, page);
    }

    public static void RemoveBackEntryIfNavForward(NavigationEventArgs e)
    {
      if (e == null || e.NavigationMode != null || e.Content == null || string.CompareOrdinal(e.Uri.ToString(), "/UI/Views/Connecting/LookForXboxPage.xaml?ShouldGoBack=true") == 0)
        return;
      LrcNavigation.Instance.RemoveBackEntry();
    }

    public static void GotoMainPageWithSlowCloudDetection(LrcPage parentPage)
    {
      if (App.GlobalData.IsUsingCloud)
        NavHelper.SafeNavigate((Page) parentPage, new Uri("/UI/Views/Connecting/SlowCloudPage.xaml", UriKind.Relative));
      else
        NavHelper.GotoMainpage(parentPage, App.RootFrameState.ConnectedLoading);
    }

    public static void GotoMainpage(LrcPage parentPage, App.RootFrameState progressType)
    {
      MainPage mainPage = parentPage as MainPage;
      if (parentPage == null)
        return;
      if (mainPage == null)
      {
        App.GlobalData.ShouldResetHomePageIndex = true;
        switch (progressType)
        {
          case App.RootFrameState.PageContent:
            App.ResetRootFrameView();
            break;
          case App.RootFrameState.GoHomeLoading:
            App.ShowtRootFrameProgressBarView();
            break;
          case App.RootFrameState.ConnectedLoading:
            App.ShowConnectedView();
            break;
          case App.RootFrameState.ConnectError:
            App.ShowRootFrameErrorView();
            break;
        }
        NavHelper.SafeNavigate((Page) parentPage, NavHelper.MainPageUri);
      }
      else
        mainPage.LRCPanorama.DefaultItem = mainPage.LRCPanorama.DefaultItem;
      App.GlobalData.CollapseMediaBar();
    }

    public static Uri GetZuneVideoDetailsPage(MediaType type)
    {
      string uriString;
      switch (type)
      {
        case MediaType.movie:
          uriString = "/UI/Views/ZuneMarketplace/ZuneMovieAndTvEpisodeDetailsPage.xaml?pageCounter=" + (object) NavHelper.pageCounter++;
          break;
        case MediaType.tv_episode:
          uriString = "/UI/Views/ZuneMarketplace/ZuneMovieAndTvEpisodeDetailsPage.xaml?pageCounter=" + (object) NavHelper.pageCounter++;
          break;
        case MediaType.tv_series:
          uriString = "/UI/Views/ZuneMarketplace/ZuneTvSeriesDetailsPage.xaml?pageCounter=" + (object) NavHelper.pageCounter++;
          break;
        case MediaType.hub:
          uriString = "/UI/Views/ZuneMarketplace/ZuneHubPage.xaml?GlobalData=Hub&pageCounter=" + (object) NavHelper.pageCounter++;
          break;
        default:
          uriString = (string) null;
          break;
      }
      return !string.IsNullOrEmpty(uriString) ? new Uri(uriString, UriKind.Relative) : (Uri) null;
    }

    public static void SafeNavigateToNowPlayingDetails(
      Page sourcePage,
      NowPlayingItemViewModel nowPlayingItem)
    {
      if (nowPlayingItem == null)
        throw new ArgumentNullException(nameof (nowPlayingItem));
      switch (nowPlayingItem.ItemType)
      {
        case NowPlayingType.Game:
          App.GlobalData.SelectedMediaDetails = (ViewModelBase) new GameItem(nowPlayingItem.TitleId);
          NavHelper.SafeNavigate(sourcePage, new Uri("/UI/Views/Games/GameDetailsPage.xaml", UriKind.Relative));
          break;
        case NowPlayingType.Music:
        case NowPlayingType.MusicVideo:
          AlbumItem albumItem = new AlbumItem();
          albumItem.Id = nowPlayingItem.ParentMediaId;
          albumItem.Title = string.Empty;
          App.GlobalData.SelectedMediaDetails = (ViewModelBase) albumItem;
          if (string.IsNullOrEmpty(nowPlayingItem.ParentMediaId))
            break;
          NavHelper.SafeNavigate(sourcePage, new Uri("/UI/Views/Search/AlbumSearchDetailsPage.xaml?item=" + nowPlayingItem.ParentMediaId, UriKind.Relative));
          break;
        case NowPlayingType.Movie:
          if (nowPlayingItem.TitleId == 1481115739U)
          {
            MovieMediaItem movieMediaItem = new MovieMediaItem();
            movieMediaItem.Id = nowPlayingItem.PartnerMediaId;
            movieMediaItem.MediaType = MediaType.movie;
            App.GlobalData.SelectedMediaDetails = (ViewModelBase) movieMediaItem;
            NavHelper.SafeNavigate(sourcePage, NavHelper.GetZuneVideoDetailsPage(movieMediaItem.MediaType));
            break;
          }
          if (string.IsNullOrEmpty(nowPlayingItem.MediaId))
            break;
          SearchDetailsViewModel detailsViewModel1 = new SearchDetailsViewModel(nowPlayingItem.MediaId, "MOVIE", nowPlayingItem.Title, nowPlayingItem.DetailsUrl);
          App.GlobalData.SelectedSearchItem = detailsViewModel1;
          NavHelper.SafeNavigate(sourcePage, NavHelper.GetSearchDetailsUri(detailsViewModel1.SearchItem));
          break;
        case NowPlayingType.TvEpisode:
          if (nowPlayingItem.TitleId == 1481115739U)
          {
            TVEpisodeItem tvEpisodeItem = new TVEpisodeItem();
            tvEpisodeItem.Id = nowPlayingItem.PartnerMediaId;
            tvEpisodeItem.MediaType = MediaType.tv_episode;
            App.GlobalData.SelectedMediaDetails = (ViewModelBase) tvEpisodeItem;
            NavHelper.SafeNavigate(sourcePage, NavHelper.GetZuneVideoDetailsPage(tvEpisodeItem.MediaType));
            break;
          }
          if (string.IsNullOrEmpty(nowPlayingItem.MediaId))
            break;
          SearchDetailsViewModel detailsViewModel2 = new SearchDetailsViewModel(nowPlayingItem.MediaId, "TVEPISODE", nowPlayingItem.Title, nowPlayingItem.DetailsUrl);
          App.GlobalData.SelectedSearchItem = detailsViewModel2;
          NavHelper.SafeNavigate(sourcePage, NavHelper.GetSearchDetailsUri(detailsViewModel2.SearchItem));
          break;
        case NowPlayingType.Application:
          if (string.IsNullOrEmpty(nowPlayingItem.AppId))
            break;
          SearchDetailsViewModel detailsViewModel3 = new SearchDetailsViewModel(nowPlayingItem.AppId, "XBOXAPP", nowPlayingItem.Title, (string) null);
          App.GlobalData.SelectedSearchItem = detailsViewModel3;
          NavHelper.SafeNavigate(sourcePage, NavHelper.GetSearchDetailsUri(detailsViewModel3.SearchItem));
          break;
      }
    }

    public static Uri GetAlbumSearchDetailsUri(string albumId)
    {
      return NavHelper.GetSearchDetailsUri(albumId, "MUSICALBUM");
    }

    public static Uri GetSearchDetailsUri(string itemId, string itemType)
    {
      if (string.IsNullOrEmpty(itemId))
        throw new ArgumentNullException(nameof (itemId));
      if (string.IsNullOrEmpty(itemType))
        throw new ArgumentNullException(nameof (itemType));
      Uri searchDetailsUri = (Uri) null;
      switch (itemType.ToUpperInvariant())
      {
        case "MOVIE":
          searchDetailsUri = new Uri("/UI/Views/Search/MovieSearchDetailsPage.xaml?item=" + itemId + "&Counter=" + (object) NavHelper.pageCounter++, UriKind.Relative);
          break;
        case "TVSERIES":
          searchDetailsUri = new Uri("/UI/Views/Search/TvSeriesSearchDetailsPage.xaml?item=" + itemId + "&Counter=" + (object) NavHelper.pageCounter++, UriKind.Relative);
          break;
        case "TVSEASON":
          searchDetailsUri = new Uri("/UI/Views/Search/TvSeasonSearchDetailsPage.xaml?item=" + itemId + "&Counter=" + (object) NavHelper.pageCounter++, UriKind.Relative);
          break;
        case "TVSHOW":
        case "TVEPISODE":
          searchDetailsUri = new Uri("/UI/Views/Search/TvEpisodeSearchDetailsPage.xaml?item=" + itemId + "&Counter=" + (object) NavHelper.pageCounter++, UriKind.Relative);
          break;
        case "MUSICALBUM":
          searchDetailsUri = new Uri("/UI/Views/Search/AlbumSearchDetailsPage.xaml?item=" + itemId + "&Counter=" + (object) NavHelper.pageCounter++, UriKind.Relative);
          break;
        case "MUSICARTIST":
          searchDetailsUri = new Uri("/UI/Views/Search/ArtistSearchDetailsPage.xaml?item=" + itemId + "&Counter=" + (object) NavHelper.pageCounter++, UriKind.Relative);
          break;
        case "XBOXAPP":
          searchDetailsUri = new Uri("/UI/Views/Search/AppSearchDetailsPage.xaml?item=" + itemId + "&Counter=" + (object) NavHelper.pageCounter++, UriKind.Relative);
          break;
        case "XBOX360GAME":
        case "XBOXARCADEGAME":
        case "XBOXORIGINALGAME":
        case "XBOXGAMETRIAL":
        case "XBOX360GAMEDEMO":
        case "XBOXXNACOMMUNITYGAME":
          searchDetailsUri = new Uri("/UI/Views/Games/GameDetailsPage.xaml?item=" + itemId + "&Counter=" + (object) NavHelper.pageCounter++, UriKind.Relative);
          break;
        case "XBOX360GAMECONTENT":
        case "XBOXGAMECONSUMABLE":
        case "XBOXGAMERTILE":
        case "XBOXGAMETRAILER":
        case "XBOXGAMEVIDEO":
        case "XBOXTHEME":
          searchDetailsUri = new Uri("/UI/Views/Search/GameContentSearchDetailsPage.xaml?item=" + itemId + "&Counter=" + (object) NavHelper.pageCounter++, UriKind.Relative);
          break;
      }
      return searchDetailsUri;
    }

    public static Uri GetSearchDetailsUri(SearchItemViewModel item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      return NavHelper.GetSearchDetailsUri(item.Id, item.ItemType);
    }

    public static Uri GetSearchResultsUri(string searchText, LRC.Service.Search.Filter filter)
    {
      return NavHelper.GetRootSearchPageUri("SearchResultsPage.xaml", searchText, filter);
    }

    public static Uri GetSearchFilterUri(string searchText, LRC.Service.Search.Filter filter)
    {
      return NavHelper.GetRootSearchPageUri("SearchSelectFilterPage.xaml", searchText, filter);
    }

    private static Uri GetRootSearchPageUri(
      string searchPageName,
      string searchText,
      LRC.Service.Search.Filter filter)
    {
      if (string.IsNullOrEmpty(searchPageName))
        throw new ArgumentNullException(nameof (searchPageName));
      if (string.IsNullOrEmpty(searchText))
        throw new ArgumentNullException(nameof (searchText));
      string str = (filter & LRC.Service.Search.Filter.All) == LRC.Service.Search.Filter.All ? "All" : filter.ToString();
      return new Uri("/UI/Views/Search/" + searchPageName + "?SearchText=" + searchText + "&Filter=" + str + "&Counter=" + (object) NavHelper.pageCounter++, UriKind.Relative);
    }
  }
}
