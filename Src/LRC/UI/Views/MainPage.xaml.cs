// *********************************************************
// Type: LRC.MainPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Resources;
using LRC.Service;
using LRC.Service.Omniture;
using LRC.UI.Omniture;
using LRC.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
//using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Xbox.Live.Phone.Services.Programming;
using Xbox.Live.Phone.Utils;


namespace LRC
{
  [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Handled manually in Load/Unload events.")]
  public partial class MainPage : LrcPanoramaPage
  {
    private const int SearchTimerInterval = 2000;
    private int nowPlayingMoreInfoCounter;
    private Storyboard searchHelpStoryboard;
    private int searchHelpTextCounter;
    private Timer searchHelpStringsTimer;
    private List<string> searchHelpStrings;
    private bool panoramaManipulationInProgress;

    public MainPage()
    {
      this.InitializeComponent();
      this.NeedToSaveViewModel = false;
      this.AllowMultipleInstances = false;
      this.Persistent = true;
      this.searchHelpStrings = new List<string>();
      this.searchHelpStrings.Add(Resource.SearchHelpText_BasicPrompt);
      this.searchHelpStrings.Add(Resource.SearchHelpText_Prompt1_Category1);
      this.searchHelpStrings.Add(Resource.SearchHelpText_Prompt2_Category1);
      this.searchHelpStrings.Add(Resource.SearchHelpText_BasicPrompt);
      this.searchHelpStrings.Add(Resource.SearchHelpText_Prompt1_Category2);
      this.searchHelpStrings.Add(Resource.SearchHelpText_Prompt2_Category2);
      this.searchHelpStrings.Add(Resource.SearchHelpText_BasicPrompt);
      this.searchHelpStrings.Add(Resource.SearchHelpText_Prompt1_Category3);
      this.searchHelpStrings.Add(Resource.SearchHelpText_Prompt2_Category3);
      this.DataContext = (object) App.GlobalData;
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.MainPage_Loaded);
      ((FrameworkElement) this).Unloaded += new RoutedEventHandler(this.MainPage_Unloaded);
      this.OmniturePageName = "wp:lrc:mainpage";
      this.OmnitureChannelName = "wp:lrc:mainpage";
    }

    protected override Panorama Panorama => this.LRCPanorama;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (App.GlobalData.DirectLaunchViewModel != null && App.GlobalData.DirectLaunchViewModel.NeedToDirectLaunch && App.GlobalData.DirectLaunchViewModel.LaunchItem != null)
      {
        NowPlayingItemViewModel launchItem = App.GlobalData.DirectLaunchViewModel.LaunchItem;
        App.GlobalData.DirectLaunchViewModel.NeedToDirectLaunch = false;
        App.GlobalData.DirectLaunchViewModel.LaunchItem = (NowPlayingItemViewModel) null;
        App.ShowConnectedView();
        NavHelper.SafeNavigateToNowPlayingDetails((Page) this, launchItem);
      }
      else
      {
        base.OnNavigatedTo(e);
        App.ResetRootFrameView();
        if (!App.GlobalData.ShouldResetHomePageIndex)
          return;
        this.LRCPanorama.DefaultItem = ((PresentationFrameworkCollection<object>) this.LRCPanorama.Items)[0];
        App.GlobalData.ShouldResetHomePageIndex = false;
        OmnitureAppMeasurement.Instance.TrackVisit(OmnitureViewConstants.JoinNames(this.OmniturePageName, ((FrameworkElement) this.LRCPanorama.DefaultItem).Name), this.OmnitureChannelName);
      }
    }

    private static string GetOmnitureMediaType(NowPlayingType type)
    {
      string omnitureMediaType = "na";
      switch (type)
      {
        case NowPlayingType.Game:
          omnitureMediaType = "game";
          break;
        case NowPlayingType.Music:
          omnitureMediaType = "music";
          break;
        case NowPlayingType.Movie:
          omnitureMediaType = "movie";
          break;
        case NowPlayingType.MusicVideo:
          omnitureMediaType = "musicvideo";
          break;
        case NowPlayingType.TvEpisode:
          omnitureMediaType = "tv";
          break;
        case NowPlayingType.Application:
          omnitureMediaType = "application";
          break;
      }
      return omnitureMediaType;
    }

    private void MainPage_Loaded(object sender, EventArgs e)
    {
      App.GlobalData.Load();
      this.HookUpStoryboards();
      ((UIElement) this.LRCPanorama).ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(this.Panorama_ManipulationDelta);
      ((UIElement) this.LRCPanorama).ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.Panorama_ManipulationCompleted);
    }

    private void MainPage_Unloaded(object sender, EventArgs e)
    {
      ((UIElement) this.LRCPanorama).ManipulationDelta -= new EventHandler<ManipulationDeltaEventArgs>(this.Panorama_ManipulationDelta);
      ((UIElement) this.LRCPanorama).ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(this.Panorama_ManipulationCompleted);
      this.UnhookStoryboards();
    }

    private void HookUpStoryboards()
    {
      this.nowPlayingMoreInfoCounter = 0;
      this.NowPlayingMoreInfoStoryboard.Stop();
      this.NowPlayingMoreInfoStoryboard_Completed((object) null, (EventArgs) null);
      ((Timeline) this.NowPlayingMoreInfoStoryboard).Completed += new EventHandler(this.NowPlayingMoreInfoStoryboard_Completed);
      this.NowPlayingMoreInfoStoryboard.Begin();
      if (this.SearchTextBox.WatermarkHost == null || !((FrameworkElement) this.SearchTextBox.WatermarkHost).Resources.Contains((object) "WatermarkStoryboard"))
        return;
      this.searchHelpStoryboard = (Storyboard) ((FrameworkElement) this.SearchTextBox.WatermarkHost).Resources[(object) "WatermarkStoryboard"];
      if (this.searchHelpStoryboard == null)
        return;
      this.searchHelpStringsTimer = new Timer(new TimerCallback(this.SearchHelpStringsTimerCallback), (object) null, -1, -1);
      this.searchHelpTextCounter = 0;
      this.isSecondSearchCallback = false;
      this.searchHelpStoryboard.Stop();
      this.SearchHelpStoryboard_Completed((object) null, (EventArgs) null);
      ((Timeline) this.searchHelpStoryboard).Completed += new EventHandler(this.SearchHelpStoryboard_Completed);
      this.searchHelpStoryboard.Begin();
    }

    private void UnhookStoryboards()
    {
      if (this.NowPlayingMoreInfoStoryboard != null)
        ((Timeline) this.NowPlayingMoreInfoStoryboard).Completed -= new EventHandler(this.NowPlayingMoreInfoStoryboard_Completed);
      if (this.searchHelpStoryboard != null)
      {
        ((Timeline) this.searchHelpStoryboard).Completed -= new EventHandler(this.SearchHelpStoryboard_Completed);
        this.searchHelpStoryboard = (Storyboard) null;
      }
      if (this.searchHelpStringsTimer == null)
        return;
      this.searchHelpStringsTimer.Dispose();
      this.searchHelpStringsTimer = (Timer) null;
    }

    private void Panorama_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
    {
      this.panoramaManipulationInProgress = true;
    }

    private void Panorama_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      this.panoramaManipulationInProgress = false;
    }

    private void NowPlayingMoreInfoStoryboard_Completed(object sender, EventArgs e)
    {
      this.IncrementNowPlayingString();
      this.NowPlayingMoreInfoStoryboard.Begin();
    }

    private void SearchHelpStoryboard_Completed(object sender, EventArgs e)
    {
      this.IncrementSearchHelpString();
      this.isSecondSearchCallback = false;
      this.searchHelpStringsTimer.Change(2000, 0);
      this.searchHelpStoryboard.Begin();
    }

    private void SearchHelpStringsTimerCallback(object stateInfo)
    {
      this.IncrementSearchHelpString();
      if (this.isSecondSearchCallback)
        return;
      this.searchHelpStringsTimer.Change(2000, 0);
      this.isSecondSearchCallback = true;
    }

    private void IncrementSearchHelpString()
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        if (this.searchHelpTextCounter >= this.searchHelpStrings.Count)
          this.searchHelpTextCounter = 0;
        this.SearchTextBox.Watermark = this.searchHelpStrings[this.searchHelpTextCounter++];
      }, (object) this);
    }

    private void IncrementNowPlayingString()
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        ObservableCollection<string> observableCollection;
        if (App.GlobalData.NowPlayingItem != null && App.GlobalData.NowPlayingItem.MoreInfoDetails != null)
        {
          observableCollection = App.GlobalData.NowPlayingItem.MoreInfoDetails;
        }
        else
        {
          observableCollection = (ObservableCollection<string>) null;
          this.nowPlayingMoreInfoCounter = 0;
        }
        if (observableCollection == null || observableCollection.Count <= 0)
          return;
        if (this.nowPlayingMoreInfoCounter >= observableCollection.Count)
          this.nowPlayingMoreInfoCounter = 0;
        this.NowPlayingMoreInfoDetails.Text = observableCollection[this.nowPlayingMoreInfoCounter++];
      }, (object) this);
    }

    private void SearchTermsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(sender is ListBox listBox) || ((Selector) listBox).SelectedIndex == -1)
        return;
      string selectedItem = ((Selector) listBox).SelectedItem as string;
      if (!string.IsNullOrEmpty(selectedItem))
        this.DoSearch(selectedItem, "popular now");
      ((Selector) listBox).SelectedIndex = -1;
    }

    private void RecentItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(sender is ListBox listBox) || ((Selector) listBox).SelectedIndex == -1)
        return;
      if (((Selector) listBox).SelectedItem is RecentItemViewModel selectedItem && selectedItem.TitleId > 0U && this.DataContext is MainViewModel dataContext)
      {
        if (selectedItem.IsGame)
          dataContext.LaunchGame(selectedItem.TitleId, selectedItem.Id, (object) null);
        else
          dataContext.LaunchApp(selectedItem.TitleId, (string) null, (object) null);
        OmnitureAppMeasurement.Instance.TrackQuickplayLauchOnConsoleEvent("played on Xbox", "quickplay", new OmnitureMediaContentTrackInfo()
        {
          Title = selectedItem.Title,
          MediaId = selectedItem.Id,
          MediaType = selectedItem.IsGame ? "game" : "application",
          Rank = ((Selector) listBox).SelectedIndex,
          Genre = "na",
          Category = "na",
          Studio = "na",
          Network = "na"
        }, new OmnitureProviderTrackInfo()
        {
          Title = "na",
          TitleId = 0U,
          ProductId = "na",
          LaunchType = "plain"
        });
      }
      ((Selector) listBox).SelectedIndex = -1;
    }

    private void PromotedItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(sender is ListBox listBox) || ((Selector) listBox).SelectedIndex == -1 || !(((Selector) listBox).SelectedItem is PromoItem selectedItem))
        return;
      OmnitureMediaContentTrackInfo mediaContent = new OmnitureMediaContentTrackInfo();
      bool flag = true;
      switch (selectedItem.ItemType)
      {
        case PromoItemType.Game:
          App.GlobalData.SelectedMediaDetails = (ViewModelBase) new GameItem(selectedItem.MediaId);
          NavHelper.SafeNavigate((Page) this, NavHelper.GetSearchDetailsUri(selectedItem.MediaId, "XBOX360GAME"));
          mediaContent.MediaType = "game";
          mediaContent.MediaId = selectedItem.MediaId;
          break;
        case PromoItemType.GameContent:
          App.GlobalData.SelectedSearchItem = new SearchDetailsViewModel(selectedItem.ContentMediaId, "XBOX360GAMECONTENT");
          App.GlobalData.SelectedSearchItem.SearchItem.ParentId = selectedItem.MediaId;
          App.GlobalData.SelectedSearchItem.SearchItem.ProductType = selectedItem.ContentMediaTypeId;
          App.GlobalData.SelectedSearchItem.Title = selectedItem.Title;
          NavHelper.SafeNavigate((Page) this, NavHelper.GetSearchDetailsUri(selectedItem.ContentMediaId, "XBOX360GAMECONTENT"));
          mediaContent.MediaType = "gamecontent";
          mediaContent.MediaId = selectedItem.ContentMediaId;
          break;
        case PromoItemType.Album:
          App.GlobalData.SelectedSearchItem = new SearchDetailsViewModel(selectedItem.MediaId, "MUSICALBUM");
          App.GlobalData.SelectedSearchItem.Title = selectedItem.Title;
          NavHelper.SafeNavigate((Page) this, NavHelper.GetSearchDetailsUri(selectedItem.MediaId, "MUSICALBUM"));
          mediaContent.MediaType = "album";
          mediaContent.MediaId = selectedItem.MediaId;
          break;
        case PromoItemType.Artist:
          App.GlobalData.SelectedSearchItem = new SearchDetailsViewModel(selectedItem.MediaId, "MUSICARTIST");
          App.GlobalData.SelectedSearchItem.Title = selectedItem.Title;
          NavHelper.SafeNavigate((Page) this, NavHelper.GetSearchDetailsUri(selectedItem.MediaId, "MUSICARTIST"));
          mediaContent.MediaType = "artist";
          mediaContent.MediaId = selectedItem.MediaId;
          break;
        case PromoItemType.Movie:
          MovieMediaItem movieMediaItem = new MovieMediaItem();
          movieMediaItem.MediaType = MediaType.movie;
          movieMediaItem.Id = selectedItem.MediaId;
          App.GlobalData.SelectedMediaDetails = (ViewModelBase) movieMediaItem;
          App.GlobalData.SelectedMediaDetails.Title = selectedItem.Title;
          NavHelper.SafeNavigate((Page) this, NavHelper.GetZuneVideoDetailsPage(MediaType.movie));
          mediaContent.MediaType = "movie";
          mediaContent.MediaId = selectedItem.MediaId;
          break;
        case PromoItemType.TvSeries:
          TVMediaItem tvMediaItem1 = new TVMediaItem();
          tvMediaItem1.Id = selectedItem.MediaId;
          tvMediaItem1.MediaType = MediaType.tv_series;
          App.GlobalData.SelectedMediaDetails = (ViewModelBase) tvMediaItem1;
          NavHelper.SafeNavigate((Page) this, NavHelper.GetZuneVideoDetailsPage(tvMediaItem1.MediaType));
          mediaContent.MediaType = "tv";
          mediaContent.MediaId = selectedItem.MediaId;
          break;
        case PromoItemType.TvSeason:
          TVMediaItem tvMediaItem2 = new TVMediaItem();
          tvMediaItem2.ParentId = selectedItem.TVSeriesId;
          tvMediaItem2.Id = selectedItem.TVSeasonNumber;
          tvMediaItem2.MediaType = MediaType.tv_series;
          App.GlobalData.SelectedMediaDetails = (ViewModelBase) tvMediaItem2;
          NavHelper.SafeNavigate((Page) this, NavHelper.ZuneTvSeasonDetailsPage);
          mediaContent.MediaType = "tv";
          mediaContent.MediaId = selectedItem.TVSeriesId;
          break;
        case PromoItemType.TvEpisode:
          TVEpisodeItem tvEpisodeItem = new TVEpisodeItem();
          tvEpisodeItem.Id = selectedItem.MediaId;
          tvEpisodeItem.MediaType = MediaType.tv_episode;
          App.GlobalData.SelectedMediaDetails = (ViewModelBase) tvEpisodeItem;
          NavHelper.SafeNavigate((Page) this, NavHelper.GetZuneVideoDetailsPage(MediaType.tv_episode));
          mediaContent.MediaType = "tv";
          mediaContent.MediaId = selectedItem.MediaId;
          break;
        case PromoItemType.Uri:
          NavHelper.SafeNavigate((Page) this, new Uri(selectedItem.DeepLink, UriKind.RelativeOrAbsolute), true);
          flag = false;
          break;
        default:
          FeaturedItemViewModel featuredItemViewModel = new FeaturedItemViewModel(selectedItem.Title, selectedItem.DeepLink, selectedItem.MediaId, selectedItem.Provider);
          featuredItemViewModel.ImageUrl = selectedItem.ImageUrl;
          App.GlobalData.SelectedMediaDetails = (ViewModelBase) featuredItemViewModel;
          NavHelper.SafeNavigate((Page) this, NavHelper.FeaturedItemOverviewPage);
          mediaContent.MediaType = "na";
          mediaContent.MediaId = "na";
          break;
      }
      if (flag)
      {
        mediaContent.Title = selectedItem.Title;
        mediaContent.MediaId = selectedItem.MediaId;
        mediaContent.Rank = this.GetFeaturedItemRank();
        mediaContent.Genre = "na";
        mediaContent.Category = "na";
        mediaContent.Studio = "na";
        mediaContent.Network = "na";
        App.GlobalData.OmnitureEntryPoint = "featured";
        OmnitureAppMeasurement.Instance.MediaItemClickedEvent("media selected", "featured", mediaContent);
      }
      ((Selector) listBox).SelectedIndex = -1;
    }

    private int GetFeaturedItemRank()
    {
      if (((Selector) this.FeaturedListBox).SelectedItem != null)
        return ((Selector) this.FeaturedListBox).SelectedIndex;
      return ((Selector) this.FeaturedListBoxFourByThree).SelectedItem != null ? ((Selector) this.FeaturedListBoxFourByThree).SelectedIndex + ((PresentationFrameworkCollection<object>) ((ItemsControl) this.FeaturedListBox).Items).Count : -1;
    }

    private void NowPlayingItem_Click(object sender, RoutedEventArgs e)
    {
      if (this.panoramaManipulationInProgress || App.GlobalData.NowPlayingItem == null)
        return;
      NavHelper.SafeNavigateToNowPlayingDetails((Page) this, App.GlobalData.NowPlayingItem);
      OmnitureMediaContentTrackInfo mediaContent = new OmnitureMediaContentTrackInfo()
      {
        Title = App.GlobalData.NowPlayingItem.Title,
        MediaType = MainPage.GetOmnitureMediaType(App.GlobalData.NowPlayingItem.ItemType),
        MediaId = App.GlobalData.NowPlayingItem.MediaId,
        Rank = -1,
        Genre = "na",
        Category = "na",
        Studio = "na",
        Network = "na"
      };
      App.GlobalData.OmnitureEntryPoint = "nowplaying";
      OmnitureAppMeasurement.Instance.MediaItemClickedEvent("media selected", "nowplaying", mediaContent);
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
      this.DoSearch(this.SearchTextBox.Text, "text entry");
    }

    private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key != 3)
        return;
      ((Control) this.SearchButton).Focus();
      this.DoSearch(this.SearchTextBox.Text, "text entry");
    }

    private void DoSearch(string searchTerm, string searchType)
    {
      if (string.IsNullOrWhiteSpace(searchTerm))
        return;
      SearchViewModel searchViewModel = new SearchViewModel(searchTerm, LRC.Service.Search.Filter.All);
      App.GlobalData.CurrentSearch = (ViewModelBase) searchViewModel;
      NavHelper.SafeNavigate((Page) this, NavHelper.GetSearchResultsUri(searchViewModel.SearchText, searchViewModel.SearchFilter));
      OmnitureAppMeasurement.Instance.TrackSearchUsedEvent("search used", searchType, searchTerm);
    }

    private void RetryButton_Click(object sender, RoutedEventArgs e)
    {
      NavHelper.GotoLookForXboxPage((Page) this, true);
    }

    private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.OmnitureTrackPageVisit();
    }
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.ErrorPane = (Grid) ((FrameworkElement) this).FindName("ErrorPane");
      this.TextStackPanel_ErrorTitle = (StackPanel) ((FrameworkElement) this).FindName("TextStackPanel_ErrorTitle");
      this.ErrorTitle = (TextBlock) ((FrameworkElement) this).FindName("ErrorTitle");
      this.ScrollViewerWrapper = (ScrollViewer) ((FrameworkElement) this).FindName("ScrollViewerWrapper");
      this.TextStackPanel_Error = (StackPanel) ((FrameworkElement) this).FindName("TextStackPanel_Error");
      this.ErrorText = (TextBlock) ((FrameworkElement) this).FindName("ErrorText");
      this.stackPanel = (StackPanel) ((FrameworkElement) this).FindName("stackPanel");
      this.ErrorCodeLabel = (TextBlock) ((FrameworkElement) this).FindName("ErrorCodeLabel");
      this.ErrorCode = (TextBlock) ((FrameworkElement) this).FindName("ErrorCode");
      this.RetryButton = (Button) ((FrameworkElement) this).FindName("RetryButton");
      this.shadow = (Ellipse) ((FrameworkElement) this).FindName("shadow");
      this.LRCPanorama = (Panorama) ((FrameworkElement) this).FindName("LRCPanorama");
      this.home = (PanoramaItem) ((FrameworkElement) this).FindName("home");
      this.NowPlayingPanel = (StackPanel) ((FrameworkElement) this).FindName("NowPlayingPanel");
      this.NowPlayingButton = (Button) ((FrameworkElement) this).FindName("NowPlayingButton");
      this.NowPlayingButtonRoot = (Grid) ((FrameworkElement) this).FindName("NowPlayingButtonRoot");
      this.ImageRow = (RowDefinition) ((FrameworkElement) this).FindName("ImageRow");
      this.background = (Image) ((FrameworkElement) this).FindName("background");
      this.ImageCtrl = (Image) ((FrameworkElement) this).FindName("ImageCtrl");
      this.ImageText = (TextBlock) ((FrameworkElement) this).FindName("ImageText");
      this.title = (TextBlock) ((FrameworkElement) this).FindName("title");
      this.NowPlayingOnXboxText = (TextBlock) ((FrameworkElement) this).FindName("NowPlayingOnXboxText");
      this.NowPlayingMoreInfoHeader = (TextBlock) ((FrameworkElement) this).FindName("NowPlayingMoreInfoHeader");
      this.NowPlayingMoreInfoDetails = (TextBlock) ((FrameworkElement) this).FindName("NowPlayingMoreInfoDetails");
      this.NowPlayingMoreInfoStoryboard = (Storyboard) ((FrameworkElement) this).FindName("NowPlayingMoreInfoStoryboard");
      this.NowPlayingDashboardText = (TextBlock) ((FrameworkElement) this).FindName("NowPlayingDashboardText");
      this.RecentListBox = (ListBox) ((FrameworkElement) this).FindName("RecentListBox");
      this.RecentsErrorText = (TextBlock) ((FrameworkElement) this).FindName("RecentsErrorText");
      this.featured = (PanoramaItem) ((FrameworkElement) this).FindName("featured");
      this.FeaturedListBox = (ListBox) ((FrameworkElement) this).FindName("FeaturedListBox");
      this.FeaturedListBoxFourByThree = (ListBox) ((FrameworkElement) this).FindName("FeaturedListBoxFourByThree");
      this.search = (PanoramaItem) ((FrameworkElement) this).FindName("search");
      this.searchBox = (Grid) ((FrameworkElement) this).FindName("searchBox");
      this.SearchTextBox = (WatermarkedTextBox) ((FrameworkElement) this).FindName("SearchTextBox");
      this.SearchButton = (Button) ((FrameworkElement) this).FindName("SearchButton");
      this.SearchTermsTitle = (TextBlock) ((FrameworkElement) this).FindName("SearchTermsTitle");
      this.SearchTermsListBox = (ListBox) ((FrameworkElement) this).FindName("SearchTermsListBox");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
