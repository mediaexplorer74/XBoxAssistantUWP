// *********************************************************
// Type: LRC.ViewModel.SearchFilterViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils.Serialization;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class SearchFilterViewModel : ViewModelBase
  {
    private const string ComponentName = "SearchFilterViewModel";
    private string searchText;
    private SearchResultFilterItem selectedSearchFilter;
    private XmlSerializableDictionary<LRC.Service.Search.Filter, SearchResultFilterItem> searchFilters;

    public SearchFilterViewModel()
    {
      this.IsBusy = false;
      this.LifetimeInMinutes = 60;
      this.searchFilters = new XmlSerializableDictionary<LRC.Service.Search.Filter, SearchResultFilterItem>();
      this.searchFilters.Add(LRC.Service.Search.Filter.All, new SearchResultFilterItem(LRC.Service.Search.Filter.All, 0));
      this.searchFilters.Add(LRC.Service.Search.Filter.Movies, new SearchResultFilterItem(LRC.Service.Search.Filter.Movies, 0));
      this.searchFilters.Add(LRC.Service.Search.Filter.TV, new SearchResultFilterItem(LRC.Service.Search.Filter.TV, 0));
      this.searchFilters.Add(LRC.Service.Search.Filter.Games, new SearchResultFilterItem(LRC.Service.Search.Filter.Games, 0));
      this.searchFilters.Add(LRC.Service.Search.Filter.Apps, new SearchResultFilterItem(LRC.Service.Search.Filter.Apps, 0));
      this.searchFilters.Add(LRC.Service.Search.Filter.Music, new SearchResultFilterItem(LRC.Service.Search.Filter.Music, 0));
      this.SelectedSearchFilter = this.searchFilters[LRC.Service.Search.Filter.All];
      this.CurrentState = 0;
    }

    public SearchFilterViewModel(SearchViewModel searchViewModel)
      : this()
    {
      if (searchViewModel == null)
        throw new ArgumentNullException(nameof (searchViewModel));
      foreach (LRC.Service.Search.Filter key in searchViewModel.SearchFilterViewModel.SearchFilters.Keys)
        this.SearchFilters[key].Count = searchViewModel.SearchFilterViewModel.SearchFilters[key].Count;
      this.SelectedSearchFilter = this.SearchFilters[searchViewModel.SearchFilter];
      this.SearchText = searchViewModel.SearchText;
      this.CurrentState = 0;
    }

    public XmlSerializableDictionary<LRC.Service.Search.Filter, SearchResultFilterItem> SearchFilters
    {
      get => this.searchFilters;
      set
      {
        this.SetPropertyValue<XmlSerializableDictionary<LRC.Service.Search.Filter, SearchResultFilterItem>>(ref this.searchFilters, value, nameof (SearchFilters));
      }
    }

    public SearchResultFilterItem SelectedSearchFilter
    {
      get => this.selectedSearchFilter;
      set
      {
        this.SetPropertyValue<SearchResultFilterItem>(ref this.selectedSearchFilter, value, nameof (SelectedSearchFilter));
      }
    }

    public LRC.Service.Search.Filter SearchFilter => this.SelectedSearchFilter.Filter;

    public string SearchText
    {
      get => this.searchText;
      set => this.SetPropertyValue<string>(ref this.searchText, value, nameof (SearchText));
    }

    public void ResetFilterCounts()
    {
      foreach (LRC.Service.Search.Filter key in this.SearchFilters.Keys)
        this.SearchFilters[key].Count = 0;
      this.CurrentState = 0;
      this.NotifyPropertyChanged("SearchFilters");
      this.NotifyPropertyChanged("SelectedSearchFilter");
    }

    public void Update(
      XmlSerializableDictionary<LRC.Service.Search.Filter, int> newFilterCounts)
    {
      if (newFilterCounts == null)
        throw new ArgumentNullException(nameof (newFilterCounts));
      this.CurrentState = 3;
      foreach (LRC.Service.Search.Filter key in newFilterCounts.Keys)
      {
        if (this.SearchFilters.ContainsKey(key))
          this.SearchFilters[key].Count = newFilterCounts[key];
      }
      this.CurrentState = 0;
      this.NotifyPropertyChanged("SearchFilters");
      this.NotifyPropertyChanged("SelectedSearchFilter");
    }
  }
}
