// *********************************************************
// Type: LRC.SearchSelectFilterPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace LRC
{
  public class SearchSelectFilterPage : LrcPage
  {
    internal Grid LayoutRoot;
    internal Grid ContentPanel;
    internal TextBlock SearchResultFiltersPageTitle;
    internal ListBox SearchResultFiltersListBox;
    internal MediaBar MediaControls;
    private bool _contentLoaded;

    public SearchSelectFilterPage()
    {
      this.InitializeComponent();
      this.Persistent = false;
      this.NeedToSaveViewModel = true;
      this.AllowMultipleInstances = false;
      this.OmnitureChannelName = "wp:lrc:search";
      this.OmniturePageName = "wp:lrc:search:filter";
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      SearchFilterViewModel currentSearch = App.GlobalData.CurrentSearch as SearchFilterViewModel;
      App.GlobalData.CurrentSearch = (ViewModelBase) null;
      if (currentSearch != null)
      {
        this.DataContext = (object) currentSearch;
        currentSearch.Load();
      }
      else if (this.DataContext != null)
        ((ViewModelBase) this.DataContext).Load();
      this.OmnitureTrackPageVisit();
    }

    private void SearchResultFiltersListBox_SelectionChanged(
      object sender,
      SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count <= 0 || !(e.AddedItems[0] is SearchResultFilterItem addedItem))
        return;
      SearchFilterViewModel dataContext = (SearchFilterViewModel) this.DataContext;
      dataContext.SelectedSearchFilter = addedItem;
      App.GlobalData.CurrentSearch = (ViewModelBase) new SearchViewModel(dataContext.SearchText, dataContext.SearchFilter);
      NavHelper.SafeGoBack((Page) this);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/Search/SearchSelectFilterPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.ContentPanel = (Grid) ((FrameworkElement) this).FindName("ContentPanel");
      this.SearchResultFiltersPageTitle = (TextBlock) ((FrameworkElement) this).FindName("SearchResultFiltersPageTitle");
      this.SearchResultFiltersListBox = (ListBox) ((FrameworkElement) this).FindName("SearchResultFiltersListBox");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
