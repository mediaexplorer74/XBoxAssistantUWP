// *********************************************************
// Type: LRC.SearchResultsPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using LRC.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Navigation;
using XLToolKit;


namespace LRC
{
  public class SearchResultsPage : LrcPage
  {
Grid LayoutRoot;
TextBlock PageTitle;
Grid searchGrid;
WatermarkedTextBox ResultsSearchTextBox;
Button ResultsSearchButton;
Button SelectFilterButton;
BusyIndicator BusyIndicator;
SwitchPanel ResultSwitchPanel;
ListBoxWithCompression SearchResultsListBox;
TextBlock ErrorTextBlock;
TextBlock NoResultsFoundTextBlock;
TextBlock LoadingTextBlock;
MediaBar MediaControls;
    

    public SearchResultsPage()
    {
      this.InitializeComponent();
      this.Persistent = false;
      this.AllowMultipleInstances = false;
      this.NeedToSaveViewModel = true;
      this.OmnitureChannelName = "wp:lrc:search";
      this.OmniturePageName = "wp:lrc:search:results";
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      SearchViewModel searchViewModel = App.GlobalData.CurrentSearch as SearchViewModel;
      App.GlobalData.CurrentSearch = (ViewModelBase) null;
      if (searchViewModel != null)
        this.DataContext = (object) searchViewModel;
      else if (this.DataContext != null)
        searchViewModel = this.DataContext as SearchViewModel;
      if (searchViewModel == null)
        return;
      searchViewModel.EventNewSearchCompleted += new EventHandler<EventArgs>(this.SearchViewModel_EventNewSearchCompleted);
      searchViewModel.Load();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      if (this.DataContext is SearchViewModel dataContext)
        dataContext.EventNewSearchCompleted -= new EventHandler<EventArgs>(this.SearchViewModel_EventNewSearchCompleted);
      base.OnNavigatedFrom(e);
    }

    protected override void OnError(object sender, LRCAsyncCompletedEventArgs eventArgs)
    {
    }

    private void ListBox_BottomReached(object sender, VisualStateChangedEventArgs e)
    {
      ((SearchViewModel) this.DataContext).FetchMoreData();
    }

    private void DoSearch()
    {
      if (this.DataContext == null || string.IsNullOrWhiteSpace(this.ResultsSearchTextBox.Text))
        return;
      SearchViewModel dataContext = (SearchViewModel) this.DataContext;
      dataContext.SearchText = this.ResultsSearchTextBox.Text;
      dataContext.ResetLastRefreshTime();
      dataContext.Load();
      OmnitureAppMeasurement.Instance.TrackSearchUsedEvent("search used", "text entry", this.ResultsSearchTextBox.Text);
    }

    private void SearchViewModel_EventNewSearchCompleted(object sender, EventArgs e)
    {
      this.SearchResultsListBox.ResetPosition();
      if (!(sender is SearchViewModel searchViewModel))
        return;
      string restult = searchViewModel.SearchResults == null || searchViewModel.SearchResults.Count <= 0 ? "no" : "yes";
      OmnitureAppMeasurement.Instance.TrackSearchResultsPageVisit(this.OmniturePageName, this.OmnitureChannelName, searchViewModel.SearchText, restult, searchViewModel.SearchFilterViewModel.SelectedSearchFilter.Name);
    }

    private void SearchResultsListBox_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is ListBoxWithCompression boxWithCompression))
        return;
      boxWithCompression.EventListboxBottomReached += new EventHandler<VisualStateChangedEventArgs>(this.ListBox_BottomReached);
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e) => this.DoSearch();

    private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key != 3)
        return;
      ((Control) this.ResultsSearchButton).Focus();
      this.DoSearch();
    }

    private void SelectFilterButton_Click(object sender, RoutedEventArgs e)
    {
      SearchViewModel dataContext = (SearchViewModel) this.DataContext;
      App.GlobalData.CurrentSearch = (ViewModelBase) new SearchFilterViewModel(dataContext);
      NavHelper.SafeNavigate((Page) this, NavHelper.GetSearchFilterUri(dataContext.SearchText, dataContext.SearchFilter));
    }

    private void SearchResultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (((Selector) this.SearchResultsListBox).SelectedIndex < 0)
        return;
      if (e.AddedItems.Count > 0 && e.AddedItems[0] is SearchItemViewModel addedItem)
      {
        string itemType = !string.Equals(addedItem.ItemType, "TVSERIES", StringComparison.OrdinalIgnoreCase) || addedItem.TelevisionSeriesHasSeasons ? addedItem.ItemType : "TVSEASON";
        Uri searchDetailsUri = NavHelper.GetSearchDetailsUri(addedItem.Id, itemType);
        if (searchDetailsUri != (Uri) null)
        {
          if (addedItem.IsGame)
          {
            App.GlobalData.SelectedMediaDetails = (ViewModelBase) new GameItem(addedItem.Id);
            App.GlobalData.SelectedMediaDetails.Title = addedItem.Title;
          }
          else if (addedItem.IsMusicAlbum)
          {
            AlbumItem albumItem = new AlbumItem();
            albumItem.Title = addedItem.Title;
            App.GlobalData.SelectedMediaDetails = (ViewModelBase) albumItem;
          }
          else
          {
            App.GlobalData.SelectedSearchItem = new SearchDetailsViewModel(addedItem.Id, addedItem.ItemType, addedItem.Title, addedItem.DetailsUrl);
            App.GlobalData.SelectedSearchItem.SearchItem.TelevisionSeriesHasSeasons = addedItem.TelevisionSeriesHasSeasons;
            App.GlobalData.SelectedSearchItem.SearchItem.ImageSize = addedItem.ImageSize;
            App.GlobalData.SelectedSearchItem.Title = addedItem.Title;
          }
          NavHelper.SafeNavigate((Page) this, searchDetailsUri);
          OmnitureMediaContentTrackInfo mediaContent = new OmnitureMediaContentTrackInfo()
          {
            Title = addedItem.Title,
            MediaType = addedItem.ItemType,
            MediaId = addedItem.Id,
            Rank = ((Selector) this.SearchResultsListBox).SelectedIndex,
            Genre = addedItem.Genres == null || addedItem.Genres.Count <= 0 ? "na" : addedItem.Genres[0],
            Category = "na",
            Studio = "na",
            Network = "na"
          };
          App.GlobalData.OmnitureEntryPoint = "search";
          OmnitureAppMeasurement.Instance.MediaItemClickedEvent("media selected", "search", mediaContent);
        }
      }
      ((Selector) this.SearchResultsListBox).SelectedIndex = -1;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Search/SearchResultsPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.PageTitle = (TextBlock) ((FrameworkElement) this).FindName("PageTitle");
      this.searchGrid = (Grid) ((FrameworkElement) this).FindName("searchGrid");
      this.ResultsSearchTextBox = (WatermarkedTextBox) ((FrameworkElement) this).FindName("ResultsSearchTextBox");
      this.ResultsSearchButton = (Button) ((FrameworkElement) this).FindName("ResultsSearchButton");
      this.SelectFilterButton = (Button) ((FrameworkElement) this).FindName("SelectFilterButton");
      this.BusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("BusyIndicator");
      this.ResultSwitchPanel = (SwitchPanel) ((FrameworkElement) this).FindName("ResultSwitchPanel");
      this.SearchResultsListBox = (ListBoxWithCompression) ((FrameworkElement) this).FindName("SearchResultsListBox");
      this.ErrorTextBlock = (TextBlock) ((FrameworkElement) this).FindName("ErrorTextBlock");
      this.NoResultsFoundTextBlock = (TextBlock) ((FrameworkElement) this).FindName("NoResultsFoundTextBlock");
      this.LoadingTextBlock = (TextBlock) ((FrameworkElement) this).FindName("LoadingTextBlock");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
