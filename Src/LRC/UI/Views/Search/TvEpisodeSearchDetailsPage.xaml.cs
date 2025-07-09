// *********************************************************
// Type: LRC.TvEpisodeSearchDetailsPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

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
  public class TvEpisodeSearchDetailsPage : SearchDetailsPageBase
  {
    private const string ComponentName = "TvEpisodeSearchDetailsPage";
    public static readonly DependencyProperty IsRelatedAvailableProperty = DependencyProperty.Register("IsRelatedAvailable", typeof (bool), typeof (TvEpisodeSearchDetailsPage), new PropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((TvEpisodeSearchDetailsPage) d).AddRelatedPivotItem(e))));
    public static readonly DependencyProperty IsCastAndCrewAvailableProperty = DependencyProperty.Register("IsCastAndCrewAvailable", typeof (bool), typeof (TvEpisodeSearchDetailsPage), new PropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((TvEpisodeSearchDetailsPage) d).AddCastAndCrewPivotItem(e))));
    internal BusyIndicator BusyIndicator;
    internal Grid LayoutRoot;
    internal PivotItem related;
    internal RelatedItems RelatedItems;
    internal PivotItem castandcrew;
    internal BusyIndicator CastAndCrewLoadingIndicator;
    internal CastAndCrew TvEpisodeCastAndCrew;
    internal XLPivot TvEpisodeSearchDetailsControl;
    internal PivotItem overview;
    internal PlayNow PlayNowTvEpisode;
    internal TextBlock OverviewErrorText;
    internal BusyIndicator OverviewLoadingIndicator;
    internal MediaBar MediaControls;
    private bool _contentLoaded;

    public TvEpisodeSearchDetailsPage()
    {
      this.InitializeComponent();
      this.NeedToSaveViewModel = true;
      this.AllowMultipleInstances = true;
      this.Persistent = false;
      Binding binding1 = new Binding("SearchItem.RelatedItemsViewModel.IsRelatedAvailable")
      {
        Source = this.DataContext
      };
      BindingOperations.SetBinding((DependencyObject) this, TvEpisodeSearchDetailsPage.IsRelatedAvailableProperty, (BindingBase) binding1);
      Binding binding2 = new Binding("SearchItem.IsCastAndCrewAvailable")
      {
        Source = this.DataContext
      };
      BindingOperations.SetBinding((DependencyObject) this, TvEpisodeSearchDetailsPage.IsCastAndCrewAvailableProperty, (BindingBase) binding2);
      this.OmniturePageName = "wp:lrc:search:tvepisodedetail";
      this.OmnitureChannelName = "wp:lrc:search";
    }

    protected override Pivot Pivot => (Pivot) this.TvEpisodeSearchDetailsControl;

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

    private void AddRelatedPivotItem(DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
        return;
      ((PresentationFrameworkCollection<UIElement>) ((Panel) this.LayoutRoot).Children).Remove((UIElement) this.related);
      ((UIElement) this.related).Visibility = (Visibility) 0;
      ((PresentationFrameworkCollection<object>) this.Pivot.Items).Add((object) this.related);
    }

    private void AddCastAndCrewPivotItem(DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
        return;
      ((PresentationFrameworkCollection<UIElement>) ((Panel) this.LayoutRoot).Children).Remove((UIElement) this.castandcrew);
      ((UIElement) this.castandcrew).Visibility = (Visibility) 0;
      ((PresentationFrameworkCollection<object>) this.Pivot.Items).Add((object) this.castandcrew);
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
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Search/TvEpisodeSearchDetailsPage.xaml", UriKind.Relative));
      this.BusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("BusyIndicator");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.related = (PivotItem) ((FrameworkElement) this).FindName("related");
      this.RelatedItems = (RelatedItems) ((FrameworkElement) this).FindName("RelatedItems");
      this.castandcrew = (PivotItem) ((FrameworkElement) this).FindName("castandcrew");
      this.CastAndCrewLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("CastAndCrewLoadingIndicator");
      this.TvEpisodeCastAndCrew = (CastAndCrew) ((FrameworkElement) this).FindName("TvEpisodeCastAndCrew");
      this.TvEpisodeSearchDetailsControl = (XLPivot) ((FrameworkElement) this).FindName("TvEpisodeSearchDetailsControl");
      this.overview = (PivotItem) ((FrameworkElement) this).FindName("overview");
      this.PlayNowTvEpisode = (PlayNow) ((FrameworkElement) this).FindName("PlayNowTvEpisode");
      this.OverviewErrorText = (TextBlock) ((FrameworkElement) this).FindName("OverviewErrorText");
      this.OverviewLoadingIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("OverviewLoadingIndicator");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
