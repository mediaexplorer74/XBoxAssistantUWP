// *********************************************************
// Type: LRC.FeaturedItemOverviewPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using LRC.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using Windows.UI.Xaml.Controls;

//using System.Windows.Controls;
//using System.Windows.Navigation;
using XLToolKit;

namespace LRC
{
  public partial class FeaturedItemOverviewPage : LrcPivotPage
  {
    /*internal BusyIndicator BusyIndicator;
Grid LayoutRoot;
XLPivot OverviewPivot;
PivotItem overview;
Image DetailsImage;
TextBlock DetailTitleTextBlock;
TextBlock PlayOnConsoleText;
Button PlayOnConsole;
MediaBar MediaControls;
    */

    public FeaturedItemOverviewPage()
    {
      this.InitializeComponent();
      this.NeedToSaveViewModel = true;
      this.AllowMultipleInstances = false;
      this.Persistent = false;
      this.OmniturePageName = "wp:lrc:mainpage:featured:itemoverview";
      this.OmnitureChannelName = "wp:lrc:advertising";
    }

    protected override Pivot Pivot => (Pivot) this.OverviewPivot;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (this.DataContext != null)
        return;
      this.DataContext = (object) App.GlobalData.SelectedMediaDetails;
      App.GlobalData.SelectedMediaDetails = (ViewModelBase) null;
    }

    private void PlayOnConsole_Click(object sender, RoutedEventArgs e)
    {
      if (!(this.DataContext is FeaturedItemViewModel dataContext))
        return;
      if (dataContext.ProviderTitleId != 0U)
        dataContext.LaunchApp(dataContext.ProviderTitleId, dataContext.DeepLink, (object) null);
      else
        dataContext.LaunchTitle(dataContext.ProviderTitleId, dataContext.DeepLink, (object) null);
      OmnitureAppMeasurement.Instance.TrackPlayOnConsoleEvent("played on Xbox", this.OmnitureEntryPointName, new OmnitureMediaContentTrackInfo()
      {
        Title = dataContext.Title,
        MediaType = dataContext.ProviderTitleId != 0U ? "application" : "hub",
        MediaId = dataContext.MediaId,
        Rank = -1,
        Genre = "na",
        Category = "na",
        Studio = "na",
        Network = "na"
      }, new OmnitureProviderTrackInfo()
      {
        Title = "na",
        TitleId = dataContext.ProviderTitleId,
        ProductId = dataContext.DeepLink,
        LaunchType = dataContext.ProviderTitleId != 0U ? "plain" : "hub"
      });
    }

    private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.OmnitureTrackPageVisit();
    }

   /* [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/FeaturedItemOverviewPage.xaml", UriKind.Relative));
      this.BusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("BusyIndicator");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.OverviewPivot = (XLPivot) ((FrameworkElement) this).FindName("OverviewPivot");
      this.overview = (PivotItem) ((FrameworkElement) this).FindName("overview");
      this.DetailsImage = (Image) ((FrameworkElement) this).FindName("DetailsImage");
      this.DetailTitleTextBlock = (TextBlock) ((FrameworkElement) this).FindName("DetailTitleTextBlock");
      this.PlayOnConsoleText = (TextBlock) ((FrameworkElement) this).FindName("PlayOnConsoleText");
      this.PlayOnConsole = (Button) ((FrameworkElement) this).FindName("PlayOnConsole");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }*/
  }
}
