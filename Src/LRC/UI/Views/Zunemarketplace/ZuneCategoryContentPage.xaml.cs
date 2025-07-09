// *********************************************************
// Type: LRC.ZuneCategoryContentPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service;
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
  public class ZuneCategoryContentPage : LrcPivotPage
  {
    private const string MovieTemplateName = "FastTwoPerRowTemplate";
    private const string TVTemplateName = "FastTwoPerRowTvTemplate";
    private const string VideoFilteredStyle = "FastListBoxWithWrapping";
    internal LrcPivotPage ZuneCategorieContent;
    internal VideoListBox VideoResource;
    internal Grid LayoutRoot;
    internal XLPivot FilterPivot;
    internal MediaBar MediaControls;
    private bool _contentLoaded;

    public ZuneCategoryContentPage()
    {
      this.InitializeComponent();
      this.AllowMultipleInstances = false;
      this.Persistent = false;
      this.NeedToSaveViewModel = true;
      this.VideoResource.ExecuteAction = (Action<Uri>) (url => NavHelper.SafeNavigate((Page) this, url));
      this.OmniturePageName = "wp:lrc:zunemarketplace:category:content";
      this.OmnitureChannelName = "wp:lrc:zunemarketplace";
    }

    protected override Pivot Pivot => (Pivot) this.FilterPivot;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (this.DataContext is VideoFilteredViewModel)
        return;
      VideoFilteredViewModel filteredViewModel = new VideoFilteredViewModel();
      filteredViewModel.OrderBy = VideoFilterOrder.None;
      filteredViewModel.SelectedMediaDetails = App.GlobalData.SelectedMediaDetails as MediaItemList;
      App.GlobalData.SelectedMediaDetails = (ViewModelBase) null;
      filteredViewModel.LoadPivotItems();
      this.DataContext = (object) filteredViewModel;
    }

    private void Pivot_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(this.DataContext is VideoFilteredViewModel dataContext))
        return;
      dataContext.Load();
    }

    private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(e.AddedItems[0] is PivotItem addedItem))
        return;
      if (this.DataContext is VideoFilteredViewModel dataContext)
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
      ((FrameworkElement) boxWithCompression).Style = Application.Current.Resources[(object) "FastListBoxWithWrapping"] as Style;
      if (this.DataContext is VideoFilteredViewModel dataContext && dataContext.SelectedMediaDetails != null && dataContext.SelectedMediaDetails.Type == MediaType.tv_series)
      {
        DataTemplate dataTemplate = this.VideoResource[(object) "FastTwoPerRowTvTemplate"] as DataTemplate;
        ((ItemsControl) boxWithCompression).ItemTemplate = dataTemplate;
      }
      else
        ((ItemsControl) boxWithCompression).ItemTemplate = this.VideoResource[(object) "FastTwoPerRowTemplate"] as DataTemplate;
      ((Control) boxWithCompression).ApplyTemplate();
      boxWithCompression.EventListboxBottomReached += new EventHandler<VisualStateChangedEventArgs>(this.ListBox_BottomReached);
    }

    private void ListBox_BottomReached(object sender, VisualStateChangedEventArgs e)
    {
      if (!(this.DataContext is VideoFilteredViewModel dataContext))
        return;
      dataContext.FetchMoreDataOfCurrentPivot();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/ZuneMarketplace/ZuneCategoryContentPage.xaml", UriKind.Relative));
      this.ZuneCategorieContent = (LrcPivotPage) ((FrameworkElement) this).FindName("ZuneCategorieContent");
      this.VideoResource = (VideoListBox) ((FrameworkElement) this).FindName("VideoResource");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.FilterPivot = (XLPivot) ((FrameworkElement) this).FindName("FilterPivot");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
