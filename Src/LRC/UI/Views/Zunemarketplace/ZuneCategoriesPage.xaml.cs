// *********************************************************
// Type: LRC.ZuneCategoriesPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service;
using LRC.Service.Omniture;
using LRC.ViewModel;
//using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using Windows.UI.Xaml;

//using System.Windows.Controls;
//using System.Windows.Controls.Primitives;
//using System.Windows.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using XLToolKit;


namespace LRC
{
  public partial class ZuneCategoriesPage : LrcPivotPage
  {
    /*internal LrcPivotPage ZuneCategories;
    internal Grid LayoutRoot;
    internal XLPivot VideoPivot;
    internal MediaBar MediaControls;
    private bool _contentLoaded;*/

    public ZuneCategoriesPage()
    {
      this.InitializeComponent();
      this.Persistent = false;
      this.AllowMultipleInstances = false;
      this.NeedToSaveViewModel = true;
      this.OmniturePageName = "wp:lrc:zunemarketplace:category";
      this.OmnitureChannelName = "wp:lrc:zunemarketplace";
    }

    protected override Pivot Pivot => (Pivot) this.VideoPivot;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (this.DataContext is ZuneCategoriesViewModel)
        return;
      ZuneCategoriesViewModel categoriesViewModel = new ZuneCategoriesViewModel();
      categoriesViewModel.SelectedMediaDetails = App.GlobalData.SelectedMediaDetails as MediaItemList;
      App.GlobalData.SelectedMediaDetails = (ViewModelBase) null;
      categoriesViewModel.LoadPivotItems();
      this.DataContext = (object) categoriesViewModel;
    }

    private void Pivot_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(this.DataContext is ZuneCategoriesViewModel dataContext))
        return;
      dataContext.Load();
    }

    private void Pivot_Unloaded(object sender, RoutedEventArgs e)
    {
      App.GlobalData.SelectedMediaDetails = (ViewModelBase) null;
    }

    private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(e.AddedItems[0] is PivotItem addedItem))
        return;
      if (this.DataContext is ZuneCategoriesViewModel dataContext)
      {
        dataContext.SelectedPivotTag = (int) ((FrameworkElement) addedItem).Tag;
        dataContext.Load();
      }
      this.OmnitureTrackPageVisit();
    }

    private void ListBox_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is ListBoxWithCompression boxWithCompression) || ((FrameworkElement) boxWithCompression).DataContext == null)
        return;
      boxWithCompression.EventListboxBottomReached += new EventHandler<VisualStateChangedEventArgs>(this.ListBox_BottomReached);
      if (!(((FrameworkElement) boxWithCompression).DataContext is VideoItemList dataContext))
        return;
      if (!string.IsNullOrEmpty(dataContext.Style))
        ((FrameworkElement) boxWithCompression).Style = Application.Current.Resources[(object) dataContext.Style] as Style;
      if (string.IsNullOrEmpty(dataContext.Template))
        return;
      ((ItemsControl) boxWithCompression).ItemTemplate = Application.Current.Resources[(object) dataContext.Template] as DataTemplate;
    }

    private void ListBox_BottomReached(object sender, VisualStateChangedEventArgs e)
    {
      ((ZuneCategoriesViewModel) this.DataContext)?.LoadMoreDataOfCurrentPivot();
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(sender is ListBox listBox) || ((Selector) listBox).SelectedIndex == -1)
        return;
      ViewModelBase selectedItem1 = (ViewModelBase) ((Selector) listBox).SelectedItem;
      App.GlobalData.SelectedMediaDetails = selectedItem1;
      if (((Selector) listBox).SelectedItem is MediaItem)
      {
        NavHelper.SafeNavigate((Page) this, NavHelper.GetZuneVideoDetailsPage(((MediaItem) selectedItem1).MediaType));
        OmnitureMediaContentTrackInfo mediaContent = new OmnitureMediaContentTrackInfo()
        {
          Title = ((ViewModelBase) ((Selector) listBox).SelectedItem).Title,
          MediaType = ((MediaItem) ((Selector) listBox).SelectedItem).MediaType == MediaType.movie ? "movie" : "tv",
          MediaId = ((MediaItem) ((Selector) listBox).SelectedItem).Id,
          Genre = ((MediaItem) ((Selector) listBox).SelectedItem).Genre,
          Category = ((PivotItem) this.Pivot.SelectedItem).Header.ToString(),
          Rank = -1,
          Studio = "na",
          Network = "na"
        };
        App.GlobalData.OmnitureEntryPoint = "browse";
        OmnitureAppMeasurement.Instance.MediaItemClickedEvent("media selected", "browse", mediaContent);
      }
      else
      {
        NavHelper.SafeNavigate((Page) this, new Uri("/UI/Views/ZuneMarketplace/ZuneCategoryContentPage.xaml", UriKind.Relative));
        if (((Selector) listBox).SelectedItem is MediaItemList selectedItem2)
        {
          string genre = "na";
          string studio = "na";
          string network = "na";
          switch (selectedItem2.Type)
          {
            case MediaType.movie:
              switch (selectedItem2.Category)
              {
                case MediaCategory.Genres:
                  genre = selectedItem2.Title;
                  break;
                case MediaCategory.Studio:
                  studio = selectedItem2.Title;
                  break;
              }
              break;
            case MediaType.tv_series:
              switch (selectedItem2.Category)
              {
                case MediaCategory.Genres:
                  genre = selectedItem2.Title;
                  break;
                case MediaCategory.Network:
                  network = selectedItem2.Title;
                  break;
              }
              break;
          }
          OmnitureAppMeasurement.Instance.TrackZuneGenreStudioNetworkClickedEvent("zune genre/studio/network selected event", genre, studio, network);
        }
      }
      ((Selector) listBox).SelectedIndex = -1;
    }

    /*[DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/ZuneMarketplace/ZuneCategoriesPage.xaml", UriKind.Relative));
      this.ZuneCategories = (LrcPivotPage) ((FrameworkElement) this).FindName("ZuneCategories");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.VideoPivot = (XLPivot) ((FrameworkElement) this).FindName("VideoPivot");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }*/
  }
}
