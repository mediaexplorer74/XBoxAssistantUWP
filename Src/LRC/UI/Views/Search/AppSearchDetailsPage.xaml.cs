// *********************************************************
// Type: LRC.AppSearchDetailsPage
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
  public class AppSearchDetailsPage : SearchDetailsPageBase
  {
    private const string ComponentName = "AppSearchDetailsPage";
BusyIndicator BusyIndicator;
Grid LayoutRoot;
XLPivot AppSearchDetailsControl;
PivotItem overview;
Grid AppGrid;
Image AppImage;
TextBlock AppDetailsName;
TextBlock AppDetailsRelease;
StarRatingControl StarRating;
TextBlock XboxTitle;
Button LaunchApp;
TextBlock OverviewErrorText;
BusyIndicator OverviewLoadingIndicator;
MediaBar MediaControls;
    

    public AppSearchDetailsPage()
    {
      this.InitializeComponent();
      this.NeedToSaveViewModel = true;
      this.AllowMultipleInstances = true;
      this.Persistent = false;
      this.OmniturePageName = "wp:lrc:search:appdetail";
      this.OmnitureChannelName = "wp:lrc:search";
    }

    protected override Pivot Pivot => (Pivot) this.AppSearchDetailsControl;

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

    private void LaunchApp_Click(object sender, RoutedEventArgs e)
    {
      if (!(this.DataContext is SearchDetailsViewModel dataContext) || dataContext.SearchItem == null)
        return;
      dataContext.LaunchApp(dataContext.SearchItem.TitleId, (string) null, (object) null);
      OmnitureAppMeasurement.Instance.TrackPlayOnConsoleEvent("played on Xbox", this.OmnitureEntryPointName, new OmnitureMediaContentTrackInfo()
      {
        Title = dataContext.SearchItem.Title,
        MediaType = "application",
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

    private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.OmnitureTrackPageVisit();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Search/AppSearchDetailsPage.xaml", UriKind.Relative));
      this.BusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("BusyIndicator");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.AppSearchDetailsControl = (XLPivot) ((FrameworkElement) this).FindName("AppSearchDetailsControl");
      this.overview = (PivotItem) ((FrameworkElement) this).FindName("overview");
      this.AppGrid = (Grid) ((FrameworkElement) this).FindName("AppGrid");
      this.AppImage = (Image) ((FrameworkElement) this).FindName("AppImage");
      this.AppDetailsName = (TextBlock) ((FrameworkElement) this).FindName("AppDetailsName");
      this.AppDetailsRelease = (TextBlock) ((FrameworkElement) this).FindName("AppDetailsRelease");
      this.StarRating = (StarRatingControl) ((FrameworkElement) this).FindName("StarRating");
      this.XboxTitle = (TextBlock) ((FrameworkElement) this).FindName("XboxTitle");
      this.LaunchApp = (Button) ((FrameworkElement) this).FindName("LaunchApp");
      this.OverviewErrorText = (TextBlock) ((FrameworkElement) this).FindName("OverviewErrorText");
      this.OverviewLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("OverviewLoadingIndicator");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
