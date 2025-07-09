// *********************************************************
// Type: LRC.ZuneHomePage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service;
using LRC.Service.Omniture;
using LRC.UI.Omniture;
using LRC.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;


namespace LRC
{
  public class ZuneHomePage : LrcPanoramaPage
  {
    internal Grid LayoutRoot;
    internal Panorama ZuneMarketplacePanorama;
    internal PanoramaItem picksforme;
    internal ListBox PicksForYouListBox;
    internal PanoramaItem spotlight;
    internal ListBox FeaturedListBox;
    internal PanoramaItem movies;
    internal ListBox MovieListBox;
    internal PanoramaItem tvs;
    internal ListBox TvListBox;
    internal MediaBar MediaControls;
    private bool _contentLoaded;

    public ZuneHomePage()
    {
      this.InitializeComponent();
      this.NeedToSaveViewModel = false;
      this.AllowMultipleInstances = false;
      this.Persistent = true;
      this.DataContext = (object) App.GlobalData.ZuneMarketplace;
      this.OmniturePageName = "wp:lrc:zunemarketplace:home";
      this.OmnitureChannelName = "wp:lrc:zunemarketplace";
    }

    protected override Panorama Panorama => this.ZuneMarketplacePanorama;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      ZuneHomePage.Load();
      if (this.ZuneMarketplacePanorama.SelectedIndex != -1)
        return;
      OmnitureAppMeasurement.Instance.TrackVisit(OmnitureViewConstants.JoinNames(this.OmniturePageName, ((FrameworkElement) ((PresentationFrameworkCollection<object>) this.ZuneMarketplacePanorama.Items)[0]).Name), this.OmnitureChannelName);
    }

    private static void Load() => App.GlobalData.ZuneMarketplace.Load();

    private void Advert_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(sender is ListBox listBox) || ((Selector) listBox).SelectedIndex == -1 || !(((Selector) listBox).SelectedItem is MediaItem selectedItem))
        return;
      OmnitureMediaContentTrackInfo mediaContent = new OmnitureMediaContentTrackInfo();
      switch (selectedItem.MediaType)
      {
        case MediaType.movie:
          MainViewModel globalData1 = App.GlobalData;
          MovieMediaItem movieMediaItem1 = new MovieMediaItem();
          movieMediaItem1.Id = selectedItem.Id;
          movieMediaItem1.Title = selectedItem.Title;
          MovieMediaItem movieMediaItem2 = movieMediaItem1;
          globalData1.SelectedMediaDetails = (ViewModelBase) movieMediaItem2;
          mediaContent.MediaType = "movie";
          mediaContent.Studio = selectedItem.Studio;
          break;
        case MediaType.tv_series:
          MainViewModel globalData2 = App.GlobalData;
          TVMediaItem tvMediaItem1 = new TVMediaItem();
          tvMediaItem1.Id = selectedItem.Id;
          tvMediaItem1.Title = selectedItem.Title;
          TVMediaItem tvMediaItem2 = tvMediaItem1;
          globalData2.SelectedMediaDetails = (ViewModelBase) tvMediaItem2;
          mediaContent.MediaType = "tv";
          mediaContent.Studio = "na";
          break;
        default:
          App.GlobalData.SelectedMediaDetails = (ViewModelBase) null;
          break;
      }
      if (App.GlobalData.SelectedMediaDetails != null)
      {
        NavHelper.SafeNavigate((Page) this, NavHelper.GetZuneVideoDetailsPage(((MediaItem) App.GlobalData.SelectedMediaDetails).MediaType));
        mediaContent.Title = selectedItem.Title;
        mediaContent.MediaId = selectedItem.Id;
        mediaContent.Rank = ((Selector) listBox).SelectedIndex;
        mediaContent.Genre = selectedItem.Genre;
        mediaContent.Category = "na";
        mediaContent.Network = "na";
        App.GlobalData.OmnitureEntryPoint = ((FrameworkElement) this.ZuneMarketplacePanorama.SelectedItem).Name;
        OmnitureAppMeasurement.Instance.MediaItemClickedEvent("media selected", ((FrameworkElement) this.ZuneMarketplacePanorama.SelectedItem).Name, mediaContent);
      }
      ((Selector) listBox).SelectedIndex = -1;
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(sender is ListBox listBox) || ((Selector) listBox).SelectedIndex == -1)
        return;
      MediaItemList selectedItem = ((Selector) listBox).SelectedItem as MediaItemList;
      App.GlobalData.SelectedMediaDetails = (ViewModelBase) selectedItem;
      NavHelper.SafeNavigate((Page) this, new Uri("/UI/Views/ZuneMarketplace/ZuneCategoriesPage.xaml", UriKind.Relative));
      ((Selector) listBox).SelectedIndex = -1;
      if (selectedItem == null)
        return;
      string mediaType = "na";
      switch (selectedItem.Type)
      {
        case MediaType.movie:
          mediaType = "movie";
          break;
        case MediaType.tv_series:
          mediaType = "tv";
          break;
      }
      OmnitureAppMeasurement.Instance.TrackZuneBrowseEvent("zune marketplace browsing event", "browse", mediaType, selectedItem.Title);
    }

    private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.OmnitureTrackPageVisit();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/ZuneMarketplace/ZuneHomePage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.ZuneMarketplacePanorama = (Panorama) ((FrameworkElement) this).FindName("ZuneMarketplacePanorama");
      this.picksforme = (PanoramaItem) ((FrameworkElement) this).FindName("picksforme");
      this.PicksForYouListBox = (ListBox) ((FrameworkElement) this).FindName("PicksForYouListBox");
      this.spotlight = (PanoramaItem) ((FrameworkElement) this).FindName("spotlight");
      this.FeaturedListBox = (ListBox) ((FrameworkElement) this).FindName("FeaturedListBox");
      this.movies = (PanoramaItem) ((FrameworkElement) this).FindName("movies");
      this.MovieListBox = (ListBox) ((FrameworkElement) this).FindName("MovieListBox");
      this.tvs = (PanoramaItem) ((FrameworkElement) this).FindName("tvs");
      this.TvListBox = (ListBox) ((FrameworkElement) this).FindName("TvListBox");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
