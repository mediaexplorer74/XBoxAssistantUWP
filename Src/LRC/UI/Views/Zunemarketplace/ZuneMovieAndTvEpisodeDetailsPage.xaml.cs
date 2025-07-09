// *********************************************************
// Type: LRC.ZuneMovieAndTvEpisodeDetailsPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service;
using LRC.Service.Omniture;
using LRC.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using XLToolKit;


namespace LRC
{
  public class ZuneMovieAndTvEpisodeDetailsPage : LrcPivotPage
  {
    public static readonly DependencyProperty IsRelatedAvailableProperty = DependencyProperty.Register("IsRelatedAvailable", typeof (bool), typeof (ZuneMovieAndTvEpisodeDetailsPage), new PropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((ZuneMovieAndTvEpisodeDetailsPage) d).AddRelatedPivotItem(e))));
    internal LrcPivotPage VideoDetails;
    internal BusyIndicator BusyIndicator;
    internal VisualStateGroup VisualStateGroup;
    internal VisualState Collapsed;
    internal VisualState Expanded;
    internal Grid LayoutRoot;
    internal PivotItem related;
    internal RelatedItems RelatedItems;
    internal XLPivot VideoDetailsPivot;
    internal PivotItem overview;
    internal ScrollViewer OverviewScroll;
    internal VideoDetailsPanel VideoDetailsPanel;
    internal TextBlock SynopsisLabel;
    internal TextBlock Description;
    internal Button MoreButton;
    internal Button LessButton;
    internal TextBlock playOnXboxTitle;
    internal ItemsControl ProvidersList;
    internal TextBlock OverviewErrorText;
    internal BusyIndicator OverviewLoadingIndicator;
    internal PivotItem castandcrew;
    internal ScrollViewer ScrollViewer;
    internal PersonGroup ActorsPersonGroup;
    internal PersonGroup DirectedByPersonGroup;
    internal TextBlock DetailErrorText;
    internal TextBlock NoCastAndCrewFound;
    internal BusyIndicator DetailLoadingIndicator;
    internal MediaBar MediaControls;
    private bool _contentLoaded;

    public ZuneMovieAndTvEpisodeDetailsPage()
    {
      this.InitializeComponent();
      this.AllowMultipleInstances = false;
      this.NeedToSaveViewModel = true;
      this.Persistent = false;
      Binding binding = new Binding("SelectedMediaDetails.RelatedItemsViewModel.IsRelatedAvailable")
      {
        Source = this.DataContext
      };
      BindingOperations.SetBinding((DependencyObject) this, ZuneMovieAndTvEpisodeDetailsPage.IsRelatedAvailableProperty, (BindingBase) binding);
      this.OmniturePageName = "wp:lrc:zunemarketplace:movie&tvepisodedetails";
      this.OmnitureChannelName = "wp:lrc:zunemarketplace";
    }

    protected override Pivot Pivot => (Pivot) this.VideoDetailsPivot;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (this.DataContext is VideoDetailViewModel dataContext)
      {
        dataContext.Load();
      }
      else
      {
        VideoDetailViewModel videoDetailViewModel = new VideoDetailViewModel();
        MediaItem selectedMediaDetails = App.GlobalData.SelectedMediaDetails as MediaItem;
        videoDetailViewModel.SelectedMediaDetails = MediaItem.CreateMediaItem(selectedMediaDetails.MediaType, selectedMediaDetails.Id);
        if (selectedMediaDetails.MediaType == MediaType.tv_episode)
        {
          if (selectedMediaDetails is TVEpisodeItem tvEpisodeItem)
            videoDetailViewModel.SelectedMediaDetails.Title = ZuneVideoHelper.ConstructTvEpisodePivotTitle(tvEpisodeItem.SeriesTitle, tvEpisodeItem.SeasonNumber);
        }
        else
          videoDetailViewModel.SelectedMediaDetails.Title = selectedMediaDetails.Title;
        App.GlobalData.SelectedMediaDetails = (ViewModelBase) null;
        this.DataContext = (object) videoDetailViewModel;
        videoDetailViewModel.Load();
      }
    }

    private void Purchase_Click(object sender, RoutedEventArgs e)
    {
      if (!(sender is Button button))
        return;
      VideoDetailViewModel dataContext1 = this.DataContext as VideoDetailViewModel;
      dataContext1.LaunchZuneMedia(dataContext1.SelectedMediaDetails.MediaType, dataContext1.SelectedMediaDetails.Id, string.Empty, (object) null);
      if (!(((FrameworkElement) button).DataContext is ProviderViewModel dataContext2))
        return;
      OmnitureAppMeasurement.Instance.TrackZuneBrowsingPlayOnConsoleEvent("played on Xbox", this.OmnitureEntryPointName, new OmnitureMediaContentTrackInfo()
      {
        Title = dataContext1.SelectedMediaDetails.Title,
        MediaType = dataContext1.SelectedMediaDetails.MediaType == MediaType.movie ? "movie" : "tv",
        MediaId = dataContext1.SelectedMediaDetails.Id,
        Rank = -1,
        Genre = dataContext1.SelectedMediaDetails.Genre,
        Category = "na",
        Studio = "na",
        Network = "na"
      }, new OmnitureProviderTrackInfo()
      {
        Title = dataContext2.ProviderName,
        TitleId = dataContext2.TitleId,
        ProductId = dataContext2.ProductId,
        LaunchType = dataContext2.OfferDescription
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

    private void Pivot_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(this.DataContext is VideoDetailViewModel dataContext) || dataContext.SelectedMediaDetails.MediaType != MediaType.tv_episode)
        return;
      ((PresentationFrameworkCollection<object>) this.VideoDetailsPivot.Items).Remove((object) this.castandcrew);
    }

    private void Description_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (((FrameworkElement) this.Description).ActualHeight <= ((FrameworkElement) this.Description).MaxHeight)
        ((UIElement) this.MoreButton).Visibility = (Visibility) 1;
      else
        ((UIElement) this.MoreButton).Visibility = (Visibility) 0;
    }

    private void AddRelatedPivotItem(DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
        return;
      ((PresentationFrameworkCollection<UIElement>) ((Panel) this.LayoutRoot).Children).Remove((UIElement) this.related);
      ((UIElement) this.related).Visibility = (Visibility) 0;
      ((PresentationFrameworkCollection<object>) this.Pivot.Items).Add((object) this.related);
    }

    private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.OmnitureTrackPageVisit();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/ZuneMarketplace/ZuneMovieAndTvEpisodeDetailsPage.xaml", UriKind.Relative));
      this.VideoDetails = (LrcPivotPage) ((FrameworkElement) this).FindName("VideoDetails");
      this.BusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("BusyIndicator");
      this.VisualStateGroup = (VisualStateGroup) ((FrameworkElement) this).FindName("VisualStateGroup");
      this.Collapsed = (VisualState) ((FrameworkElement) this).FindName("Collapsed");
      this.Expanded = (VisualState) ((FrameworkElement) this).FindName("Expanded");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.related = (PivotItem) ((FrameworkElement) this).FindName("related");
      this.RelatedItems = (RelatedItems) ((FrameworkElement) this).FindName("RelatedItems");
      this.VideoDetailsPivot = (XLPivot) ((FrameworkElement) this).FindName("VideoDetailsPivot");
      this.overview = (PivotItem) ((FrameworkElement) this).FindName("overview");
      this.OverviewScroll = (ScrollViewer) ((FrameworkElement) this).FindName("OverviewScroll");
      this.VideoDetailsPanel = (VideoDetailsPanel) ((FrameworkElement) this).FindName("VideoDetailsPanel");
      this.SynopsisLabel = (TextBlock) ((FrameworkElement) this).FindName("SynopsisLabel");
      this.Description = (TextBlock) ((FrameworkElement) this).FindName("Description");
      this.MoreButton = (Button) ((FrameworkElement) this).FindName("MoreButton");
      this.LessButton = (Button) ((FrameworkElement) this).FindName("LessButton");
      this.playOnXboxTitle = (TextBlock) ((FrameworkElement) this).FindName("playOnXboxTitle");
      this.ProvidersList = (ItemsControl) ((FrameworkElement) this).FindName("ProvidersList");
      this.OverviewErrorText = (TextBlock) ((FrameworkElement) this).FindName("OverviewErrorText");
      this.OverviewLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("OverviewLoadingIndicator");
      this.castandcrew = (PivotItem) ((FrameworkElement) this).FindName("castandcrew");
      this.ScrollViewer = (ScrollViewer) ((FrameworkElement) this).FindName("ScrollViewer");
      this.ActorsPersonGroup = (PersonGroup) ((FrameworkElement) this).FindName("ActorsPersonGroup");
      this.DirectedByPersonGroup = (PersonGroup) ((FrameworkElement) this).FindName("DirectedByPersonGroup");
      this.DetailErrorText = (TextBlock) ((FrameworkElement) this).FindName("DetailErrorText");
      this.NoCastAndCrewFound = (TextBlock) ((FrameworkElement) this).FindName("NoCastAndCrewFound");
      this.DetailLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("DetailLoadingIndicator");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
