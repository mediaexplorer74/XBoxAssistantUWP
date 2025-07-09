// *********************************************************
// Type: LRC.ZuneTvSeriesDetailsPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using XLToolKit;


namespace LRC
{
  public class ZuneTvSeriesDetailsPage : LrcPivotPage
  {
    internal Grid LayoutRoot;
    internal XLPivot TvDetailsPivot;
    internal PivotItem seasons;
    internal ListBox SeasonsListBox;
    internal TextBlock OverviewErrorText;
    internal BusyIndicator OverviewLoadingIndicator;
    internal PivotItem description;
    internal ScrollViewer ScrollViewer;
    internal TextBlock DescriptionTextBlock;
    internal TextBlock DescriptionErrorText;
    internal BusyIndicator DescriptionLoadingIndicator;
    internal MediaBar MediaControls;
    private bool _contentLoaded;

    public ZuneTvSeriesDetailsPage()
    {
      this.InitializeComponent();
      this.AllowMultipleInstances = false;
      this.Persistent = false;
      this.NeedToSaveViewModel = true;
      this.OmniturePageName = "wp:lrc:zunemarketplace:tvseriesdetails";
      this.OmnitureChannelName = "wp:lrc:zunemarketplace";
    }

    protected override Pivot Pivot => (Pivot) this.TvDetailsPivot;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (this.DataContext is TvSeriesDetailsViewModel dataContext)
      {
        dataContext.Load();
      }
      else
      {
        TvSeriesDetailsViewModel detailsViewModel = new TvSeriesDetailsViewModel();
        detailsViewModel.SelectedMediaDetails = App.GlobalData.SelectedMediaDetails as MediaItem;
        App.GlobalData.SelectedMediaDetails = (ViewModelBase) null;
        this.DataContext = (object) detailsViewModel;
        detailsViewModel.Load();
      }
    }

    private void Season_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ListBox listBox = sender as ListBox;
      if (((Selector) listBox).SelectedIndex == -1)
        return;
      App.GlobalData.SelectedMediaDetails = (ViewModelBase) (((Selector) listBox).SelectedItem as MediaItem);
      NavHelper.SafeNavigate((Page) this, new Uri("/UI/Views/ZuneMarketplace/ZuneHubPage.xaml", UriKind.Relative));
      ((Selector) listBox).SelectedIndex = -1;
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
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/ZuneMarketplace/ZuneTvSeriesDetailsPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.TvDetailsPivot = (XLPivot) ((FrameworkElement) this).FindName("TvDetailsPivot");
      this.seasons = (PivotItem) ((FrameworkElement) this).FindName("seasons");
      this.SeasonsListBox = (ListBox) ((FrameworkElement) this).FindName("SeasonsListBox");
      this.OverviewErrorText = (TextBlock) ((FrameworkElement) this).FindName("OverviewErrorText");
      this.OverviewLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("OverviewLoadingIndicator");
      this.description = (PivotItem) ((FrameworkElement) this).FindName("description");
      this.ScrollViewer = (ScrollViewer) ((FrameworkElement) this).FindName("ScrollViewer");
      this.DescriptionTextBlock = (TextBlock) ((FrameworkElement) this).FindName("DescriptionTextBlock");
      this.DescriptionErrorText = (TextBlock) ((FrameworkElement) this).FindName("DescriptionErrorText");
      this.DescriptionLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("DescriptionLoadingIndicator");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
