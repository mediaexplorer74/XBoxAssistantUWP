// *********************************************************
// Type: LRC.ViewModel.SearchViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
using LRC.Service.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class SearchViewModel : ViewModelBase
  {
    private const string ComponentName = "SearchViewModel";
    private const string PerfSearch = "SearchViewModel:Search";
    private const string PerfFetchMore = "SearchViewModel:FetchMore";
    private const string PerfHandleNewSearchResults = "SearchViewModel:HandleNewSearchResults";
    private const int MaxSearchStringLength = 500;
    private const uint MaxSearchResults = 500;
    private const uint ResultsPerSearch = 20;
    private VerticalAlignment busyIndicatorVerticalAlignment;
    private string prevSearchText;
    private SearchFilterViewModel searchFilterViewModel;
    private string continuationToken;
    private ObservableCollection<SearchItemViewModel> searchResults;
    private string searchText;
    private bool isAddingData;

    public SearchViewModel()
    {
      this.IsBusy = false;
      this.CurrentState = -1;
      this.LifetimeInMinutes = 60;
      this.SearchFilterViewModel = new SearchFilterViewModel();
    }

    public SearchViewModel(string searchText, LRC.Service.Search.Filter searchFilter)
      : this()
    {
      this.SearchFilterViewModel.SelectedSearchFilter = this.SearchFilterViewModel.SearchFilters[searchFilter];
      this.SearchText = searchText;
    }

    public event EventHandler<EventArgs> EventNewSearchCompleted;

    public static int MaxSearchStringLengthProperty => 500;

    public string NoResultsFoundText
    {
      get
      {
        return string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Search_NoResultsText, new object[1]
        {
          (object) this.SearchText
        });
      }
    }

    [IgnoreDataMember]
    [XmlIgnore]
    public ObservableCollection<SearchItemViewModel> SearchResults
    {
      get => this.searchResults;
      set
      {
        this.SetPropertyValue<ObservableCollection<SearchItemViewModel>>(ref this.searchResults, value, nameof (SearchResults));
      }
    }

    [DataMember]
    public SearchFilterViewModel SearchFilterViewModel
    {
      get => this.searchFilterViewModel;
      set
      {
        this.SetPropertyValue<SearchFilterViewModel>(ref this.searchFilterViewModel, value, nameof (SearchFilterViewModel));
      }
    }

    [DataMember]
    public string SearchText
    {
      get => this.searchText;
      set
      {
        this.SetPropertyValue<string>(ref this.searchText, value, nameof (SearchText));
        this.NotifyPropertyChanged("NoResultsFoundText");
      }
    }

    [XmlIgnore]
    [IgnoreDataMember]
    public LRC.Service.Search.Filter SearchFilter
    {
      get => this.SearchFilterViewModel.SelectedSearchFilter.Filter;
    }

    [XmlIgnore]
    [IgnoreDataMember]
    public VerticalAlignment BusyIndicatorVerticalAlignment
    {
      get => this.busyIndicatorVerticalAlignment;
      set
      {
        this.SetPropertyValue<VerticalAlignment>(ref this.busyIndicatorVerticalAlignment, value, nameof (BusyIndicatorVerticalAlignment));
      }
    }

    [XmlIgnore]
    public bool IsAddingData
    {
      get => this.isAddingData;
      set => this.SetPropertyValue<bool>(ref this.isAddingData, value, nameof (IsAddingData));
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      this.DoSearch();
    }

    public void FetchMoreData()
    {
      if (this.IsBusy || this.SearchResults.Count >= this.SearchFilterViewModel.SelectedSearchFilter.Count || this.SearchResults.Count >= 500)
        return;
      this.BusyIndicatorVerticalAlignment = (VerticalAlignment) 2;
      this.IsBusy = true;
      ISearchServiceManager searchServiceManager = ServiceManagerFactory.CreateSearchServiceManager();
      searchServiceManager.EventSearchCompleted += new EventHandler<ServiceProxyEventArgs<LRC.Service.Search.SearchResults>>(this.SearchServiceManager_EventFetchMoreDataCompleted);
      searchServiceManager.Search(this.SearchText, this.SearchFilter, this.continuationToken, 20U, MainViewModel.Instance.ConsoleLiveTVProviderTitleId, (object) this.SearchText);
    }

    public void ResetLastRefreshTime()
    {
      if (string.Equals(this.SearchText, this.prevSearchText, StringComparison.OrdinalIgnoreCase))
        return;
      this.LastRefreshTime = DateTime.MinValue;
    }

    private void SearchServiceManager_EventFetchMoreDataCompleted(
      object sender,
      ServiceProxyEventArgs<LRC.Service.Search.SearchResults> e)
    {
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventSearchCompleted -= new EventHandler<ServiceProxyEventArgs<LRC.Service.Search.SearchResults>>(this.SearchServiceManager_EventFetchMoreDataCompleted);
      if (e.Error != null)
        this.HandleSearchFailure(true);
      else
        this.HandleNewSearchResults(e.Result, true);
    }

    private void DoSearch()
    {
      if (this.IsBusy)
        return;
      this.BusyIndicatorVerticalAlignment = (VerticalAlignment) 0;
      this.IsBusy = true;
      this.CurrentState = 3;
      this.continuationToken = (string) null;
      ISearchServiceManager searchServiceManager = ServiceManagerFactory.CreateSearchServiceManager();
      searchServiceManager.EventSearchCompleted += new EventHandler<ServiceProxyEventArgs<LRC.Service.Search.SearchResults>>(this.SearchServiceManager_EventSearchCompleted);
      searchServiceManager.Search(this.SearchText, this.SearchFilter, this.continuationToken, 20U, MainViewModel.Instance.ConsoleLiveTVProviderTitleId, (object) this.SearchText);
      this.prevSearchText = this.SearchText;
      MainViewModel.Instance.SearchText = this.SearchText;
    }

    private void SearchServiceManager_EventSearchCompleted(
      object sender,
      ServiceProxyEventArgs<LRC.Service.Search.SearchResults> e)
    {
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventSearchCompleted -= new EventHandler<ServiceProxyEventArgs<LRC.Service.Search.SearchResults>>(this.SearchServiceManager_EventSearchCompleted);
      if (e.Error != null)
        this.HandleSearchFailure(false);
      else
        this.HandleNewSearchResults(e.Result, false);
      EventHandler<EventArgs> newSearchCompleted = this.EventNewSearchCompleted;
      if (newSearchCompleted == null)
        return;
      newSearchCompleted((object) this, EventArgs.Empty);
    }

    private void HandleSearchFailure(bool keepExistingResults)
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        if (!keepExistingResults)
        {
          this.SearchFilterViewModel.ResetFilterCounts();
          if (this.SearchResults != null)
            this.SearchResults.Clear();
        }
        this.CurrentState = 1;
        this.IsBusy = false;
        this.ShowNonfatalErrorMessage(Resource.Search_ErrorText);
      }, (object) null);
    }

    private void HandleNewSearchResults(LRC.Service.Search.SearchResults newResults, bool appendResults)
    {
      this.LastRefreshTime = DateTime.UtcNow;
      this.IsAddingData = appendResults;
      IEnumerable<SearchItemViewModel> source = newResults.Items.Select<SearchData, SearchItemViewModel>((Func<SearchData, SearchItemViewModel>) (item => new SearchItemViewModel(item)));
      this.continuationToken = newResults.ContinuationToken;
      List<SearchItemViewModel> searchItemList = source.ToList<SearchItemViewModel>();
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        if (this.SearchResults == null)
          this.SearchResults = new ObservableCollection<SearchItemViewModel>();
        if (!appendResults)
        {
          this.SearchResults.Clear();
          this.SearchFilterViewModel.Update(newResults.TotalResultsCounter);
        }
        foreach (SearchItemViewModel searchItemViewModel in searchItemList)
          this.SearchResults.Add(searchItemViewModel);
        this.IsBusy = false;
        this.IsAddingData = false;
        if (this.SearchResults != null && this.SearchResults.Count > 0)
          this.CurrentState = 0;
        else
          this.CurrentState = 2;
      }, (object) this);
    }
  }
}
