// *********************************************************
// Type: LRC.ViewModel.SearchResultFilterItem
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using System;
using System.Globalization;


namespace LRC.ViewModel
{
  public class SearchResultFilterItem
  {
    private LRC.Service.Search.Filter filter;
    private int count;

    public SearchResultFilterItem()
    {
    }

    public SearchResultFilterItem(LRC.Service.Search.Filter filterType, int count)
    {
      this.Filter = filterType;
      this.Count = count;
    }

    public LRC.Service.Search.Filter Filter
    {
      get => this.filter;
      set
      {
        this.filter = value;
        this.Update();
      }
    }

    public int Count
    {
      get => this.count;
      set
      {
        this.count = value;
        this.Update();
      }
    }

    public string Name { get; set; }

    public string FilterSummary { get; set; }

    private void Update()
    {
      switch (this.Filter)
      {
        case LRC.Service.Search.Filter.Movies:
          this.Name = Resource.SearchResultFilterItem_Movies;
          break;
        case LRC.Service.Search.Filter.Music:
          this.Name = Resource.SearchResultFilterItem_Music;
          break;
        case LRC.Service.Search.Filter.TV:
          this.Name = Resource.SearchResultFilterItem_TV;
          break;
        case LRC.Service.Search.Filter.Games:
          this.Name = Resource.SearchResultFilterItem_Games;
          break;
        case LRC.Service.Search.Filter.Apps:
          this.Name = Resource.SearchResultFilterItem_Apps;
          break;
        case LRC.Service.Search.Filter.All:
          this.Name = Resource.SearchResultFilterItem_All;
          break;
      }
      this.FilterSummary = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resource.SearchResultFilterSummary, new object[2]
      {
        (object) this.Name,
        (object) this.Count
      });
    }
  }
}
