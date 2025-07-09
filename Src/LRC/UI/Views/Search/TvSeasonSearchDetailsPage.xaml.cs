// *********************************************************
// Type: LRC.TvSeasonSearchDetailsPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Resources;
using LRC.Service.Search;
using LRC.ViewModel;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;


namespace LRC
{
  public class TvSeasonSearchDetailsPage : LrcPage
  {
    internal Grid LayoutRoot;
    internal Grid PageContents;
    internal TextBlock PageTitle;
    internal TextBlock PageSubTitle;
    internal ListBoxWithCompression EpisodeList;
    internal TextBlock ErrorText;
    internal BusyIndicator BusyIndicator;
    internal MediaBar MediaControls;
    private bool _contentLoaded;

    public TvSeasonSearchDetailsPage()
    {
      this.InitializeComponent();
      this.NeedToSaveViewModel = true;
      this.AllowMultipleInstances = true;
      this.Persistent = false;
      this.OmniturePageName = "wp:lrc:search:tvseasondetail";
      this.OmnitureChannelName = "wp:lrc:search";
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (this.DataContext is SearchDetailsViewModel dataContext)
        dataContext.Load();
      else if (App.GlobalData.SelectedSearchItem != null)
      {
        SearchDetailsViewModel selectedSearchItem = App.GlobalData.SelectedSearchItem;
        this.DataContext = (object) selectedSearchItem;
        selectedSearchItem.Load();
        App.GlobalData.SelectedSearchItem = (SearchDetailsViewModel) null;
      }
      this.OmnitureTrackPageVisit();
    }

    private void Episode_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(sender is ListBox listBox) || ((Selector) listBox).SelectedIndex == -1)
        return;
      if (((Selector) listBox).SelectedItem is TvEpisodeData selectedItem)
      {
        App.GlobalData.SelectedSearchItem = new SearchDetailsViewModel(selectedItem.Id, selectedItem.ItemType, selectedItem.Name, selectedItem.DetailsUrl);
        if (((SearchDetailsViewModel) this.DataContext).SearchItem.TelevisionSeasonNumber.HasValue)
          App.GlobalData.SelectedSearchItem.Title = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.TVDetailsTitle, new object[2]
          {
            (object) ((ViewModelBase) this.DataContext).Title,
            (object) ((SearchDetailsViewModel) this.DataContext).SearchItem.TelevisionSeasonNumber
          });
        else
          App.GlobalData.SelectedSearchItem.Title = ((ViewModelBase) this.DataContext).Title;
        App.GlobalData.SelectedSearchItem.SearchItem.TelevisionEpisodeNumber = selectedItem.EpisodeNumber;
        App.GlobalData.SelectedSearchItem.SearchItem.TelevisionSeasonNumber = ((SearchDetailsViewModel) this.DataContext).SearchItem.TelevisionSeasonNumber;
        if (selectedItem.ImageHeight.HasValue && selectedItem.ImageWidth.HasValue)
          App.GlobalData.SelectedSearchItem.SearchItem.ImageSize = new Size?(new Size(selectedItem.ImageWidth.Value, selectedItem.ImageHeight.Value));
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

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Search/TvSeasonSearchDetailsPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.PageContents = (Grid) ((FrameworkElement) this).FindName("PageContents");
      this.PageTitle = (TextBlock) ((FrameworkElement) this).FindName("PageTitle");
      this.PageSubTitle = (TextBlock) ((FrameworkElement) this).FindName("PageSubTitle");
      this.EpisodeList = (ListBoxWithCompression) ((FrameworkElement) this).FindName("EpisodeList");
      this.ErrorText = (TextBlock) ((FrameworkElement) this).FindName("ErrorText");
      this.BusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("BusyIndicator");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
