// *********************************************************
// Type: LRC.TvSeriesSearchDetailsPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Search;
using LRC.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Navigation;
using XLToolKit;


namespace LRC
{
  public class TvSeriesSearchDetailsPage : SearchDetailsPageBase
  {
    public static readonly DependencyProperty IsRelatedAvailableProperty = DependencyProperty.Register("IsRelatedAvailable", typeof (bool), typeof (TvSeriesSearchDetailsPage), new PropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((TvSeriesSearchDetailsPage) d).AddRelatedPivotItem(e))));
    internal Grid LayoutRoot;
    internal PivotItem related;
    internal RelatedItems RelatedItems;
    internal XLPivot TvSeriesSearchDetailsControl;
    internal PivotItem seasons;
    internal ListBoxWithCompression SeasonsListBox;
    internal TextBlock ErrorText;
    internal BusyIndicator BusyIndicator;
    internal MediaBar MediaControls;
    private bool _contentLoaded;

    public TvSeriesSearchDetailsPage()
    {
      this.InitializeComponent();
      this.NeedToSaveViewModel = true;
      this.AllowMultipleInstances = true;
      this.Persistent = false;
      Binding binding = new Binding("SearchItem.RelatedItemsViewModel.IsRelatedAvailable")
      {
        Source = this.DataContext
      };
      BindingOperations.SetBinding((DependencyObject) this, TvSeriesSearchDetailsPage.IsRelatedAvailableProperty, (BindingBase) binding);
      this.OmniturePageName = "wp:lrc:search:tvseriesdetail";
      this.OmnitureChannelName = "wp:lrc:search";
    }

    protected override Pivot Pivot => (Pivot) this.TvSeriesSearchDetailsControl;

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
        App.GlobalData.SelectedSearchItem = (SearchDetailsViewModel) null;
        selectedSearchItem.Load();
      }
    }

    private void Season_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(sender is ListBox listBox) || ((Selector) listBox).SelectedIndex == -1)
        return;
      if (((Selector) listBox).SelectedItem is TvSeasonData selectedItem)
      {
        App.GlobalData.SelectedSearchItem = new SearchDetailsViewModel(selectedItem.Id, selectedItem.ItemType, selectedItem.Name, selectedItem.DetailsUrl);
        App.GlobalData.SelectedSearchItem.Title = ((ViewModelBase) this.DataContext).Title;
        App.GlobalData.SelectedSearchItem.SearchItem.TelevisionSeasonNumber = selectedItem.SeasonNumber;
        NavHelper.SafeNavigate((Page) this, NavHelper.GetSearchDetailsUri(selectedItem.Id, selectedItem.ItemType));
      }
      ((Selector) listBox).SelectedIndex = -1;
    }

    private void ListBox_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is ListBoxWithCompression boxWithCompression))
        return;
      boxWithCompression.EventListboxBottomReached += new EventHandler<VisualStateChangedEventArgs>(this.ListBox_BottomReached);
    }

    private void ListBox_BottomReached(object sender, VisualStateChangedEventArgs e)
    {
      ((SearchDetailsViewModel) this.DataContext).FetchMoreData();
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
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Search/TvSeriesSearchDetailsPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.related = (PivotItem) ((FrameworkElement) this).FindName("related");
      this.RelatedItems = (RelatedItems) ((FrameworkElement) this).FindName("RelatedItems");
      this.TvSeriesSearchDetailsControl = (XLPivot) ((FrameworkElement) this).FindName("TvSeriesSearchDetailsControl");
      this.seasons = (PivotItem) ((FrameworkElement) this).FindName("seasons");
      this.SeasonsListBox = (ListBoxWithCompression) ((FrameworkElement) this).FindName("SeasonsListBox");
      this.ErrorText = (TextBlock) ((FrameworkElement) this).FindName("ErrorText");
      this.BusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("BusyIndicator");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
