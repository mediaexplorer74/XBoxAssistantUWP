// *********************************************************
// Type: LRC.GameContentSearchDetailsPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using LRC.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using XLToolKit;


namespace LRC
{
  public class GameContentSearchDetailsPage : LrcPivotPage
  {
    private const string ComponentName = "GameContentSearchDetailsPage";
BusyIndicator BusyIndicator;
VisualStateGroup VisualStateGroup;
VisualState Collapsed;
VisualState Expanded;
Grid LayoutRoot;
XLPivot SearchDetailsControl;
PivotItem overview;
ScrollViewer OverviewScroll;
Grid SearchItemRootPanel;
Grid MovieGrid;
Image BoxArtImage;
TextBlock DetailsName;
TextBlock PublisherName;
StarRatingControl StarRating;
TextBlock GameDetailsRelease;
StackPanel GameContentDescriptionPanel;
TextBlock DescriptionTitle;
TextBlock Description;
Button MoreButton;
Button LessButton;
TextBlock playOnXboxTitle;
Button DownloadOnConsole;
TextBlock Rating;
Image RatingIcon;
ItemsControl RatingDescriptorControl;
TextBlock OverviewErrorText;
BusyIndicator OverviewLoadingIndicator;
MediaBar MediaControls;
    

    public GameContentSearchDetailsPage()
    {
      this.InitializeComponent();
      this.NeedToSaveViewModel = true;
      this.AllowMultipleInstances = true;
      this.Persistent = false;
      this.OmniturePageName = "wp:lrc:search:gamecontentdetail";
      this.OmnitureChannelName = "wp:lrc:search";
    }

    protected override Pivot Pivot => (Pivot) this.SearchDetailsControl;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (this.DataContext is SearchDetailsViewModel dataContext)
      {
        dataContext.Load();
      }
      else
      {
        if (App.GlobalData.SelectedSearchItem == null)
          return;
        SearchDetailsViewModel selectedSearchItem = App.GlobalData.SelectedSearchItem;
        this.DataContext = (object) selectedSearchItem;
        selectedSearchItem.Load();
        App.GlobalData.SelectedSearchItem = (SearchDetailsViewModel) null;
      }
    }

    private void DownloadOnConsole_Click(object sender, RoutedEventArgs e)
    {
      if (!(this.DataContext is SearchDetailsViewModel dataContext) || dataContext.SearchItem == null)
        return;
      dataContext.LaunchContent(dataContext.SearchItem.Id, dataContext.SearchItem.ParentId, dataContext.SearchItem.ProductType, (object) null);
      OmnitureAppMeasurement.Instance.TrackPlayOnConsoleEvent("played on Xbox", this.OmnitureEntryPointName, new OmnitureMediaContentTrackInfo()
      {
        Title = dataContext.SearchItem.Title,
        MediaType = "gamecontent",
        MediaId = dataContext.SearchItem.Id,
        Rank = -1,
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

    private void MoreButton_Click(object sender, RoutedEventArgs e)
    {
      VisualStateManager.GoToState((Control) this, "Expanded", true);
    }

    private void LessButton_Click(object sender, RoutedEventArgs e)
    {
      VisualStateManager.GoToState((Control) this, "Collapsed", true);
    }

    private void Description_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (((FrameworkElement) this.Description).ActualHeight <= ((FrameworkElement) this.Description).MaxHeight)
        ((UIElement) this.MoreButton).Visibility = (Visibility) 1;
      else
        ((UIElement) this.MoreButton).Visibility = (Visibility) 0;
    }

    private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.OmnitureTrackPageVisit();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Search/GameContentSearchDetailsPage.xaml", UriKind.Relative));
      this.BusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("BusyIndicator");
      this.VisualStateGroup = (VisualStateGroup) ((FrameworkElement) this).FindName("VisualStateGroup");
      this.Collapsed = (VisualState) ((FrameworkElement) this).FindName("Collapsed");
      this.Expanded = (VisualState) ((FrameworkElement) this).FindName("Expanded");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.SearchDetailsControl = (XLPivot) ((FrameworkElement) this).FindName("SearchDetailsControl");
      this.overview = (PivotItem) ((FrameworkElement) this).FindName("overview");
      this.OverviewScroll = (ScrollViewer) ((FrameworkElement) this).FindName("OverviewScroll");
      this.SearchItemRootPanel = (Grid) ((FrameworkElement) this).FindName("SearchItemRootPanel");
      this.MovieGrid = (Grid) ((FrameworkElement) this).FindName("MovieGrid");
      this.BoxArtImage = (Image) ((FrameworkElement) this).FindName("BoxArtImage");
      this.DetailsName = (TextBlock) ((FrameworkElement) this).FindName("DetailsName");
      this.PublisherName = (TextBlock) ((FrameworkElement) this).FindName("PublisherName");
      this.StarRating = (StarRatingControl) ((FrameworkElement) this).FindName("StarRating");
      this.GameDetailsRelease = (TextBlock) ((FrameworkElement) this).FindName("GameDetailsRelease");
      this.GameContentDescriptionPanel = (StackPanel) ((FrameworkElement) this).FindName("GameContentDescriptionPanel");
      this.DescriptionTitle = (TextBlock) ((FrameworkElement) this).FindName("DescriptionTitle");
      this.Description = (TextBlock) ((FrameworkElement) this).FindName("Description");
      this.MoreButton = (Button) ((FrameworkElement) this).FindName("MoreButton");
      this.LessButton = (Button) ((FrameworkElement) this).FindName("LessButton");
      this.playOnXboxTitle = (TextBlock) ((FrameworkElement) this).FindName("playOnXboxTitle");
      this.DownloadOnConsole = (Button) ((FrameworkElement) this).FindName("DownloadOnConsole");
      this.Rating = (TextBlock) ((FrameworkElement) this).FindName("Rating");
      this.RatingIcon = (Image) ((FrameworkElement) this).FindName("RatingIcon");
      this.RatingDescriptorControl = (ItemsControl) ((FrameworkElement) this).FindName("RatingDescriptorControl");
      this.OverviewErrorText = (TextBlock) ((FrameworkElement) this).FindName("OverviewErrorText");
      this.OverviewLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("OverviewLoadingIndicator");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
