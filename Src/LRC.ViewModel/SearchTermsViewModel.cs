// *********************************************************
// Type: LRC.ViewModel.SearchTermsViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
using LRC.Service.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class SearchTermsViewModel : ViewModelBase
  {
    private const string ComponentName = "SearchTermsViewModel";
    private ObservableCollection<string> items;

    public SearchTermsViewModel() => this.LifetimeInMinutes = 60;

    public ObservableCollection<string> Items
    {
      get => this.items;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.items, value, nameof (Items));
      }
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      this.IsBusy = true;
      ISearchServiceManager searchServiceManager = ServiceManagerFactory.CreateSearchServiceManager();
      searchServiceManager.EventGetSearchTermsCompleted += new EventHandler<ServiceProxyEventArgs<List<string>>>(this.SearchServiceManager_EventGetSearchTermsCompleted);
      searchServiceManager.GetSearchTerms((object) null);
    }

    private void SearchServiceManager_EventGetSearchTermsCompleted(
      object sender,
      ServiceProxyEventArgs<List<string>> e)
    {
      this.IsBusy = false;
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetSearchTermsCompleted -= new EventHandler<ServiceProxyEventArgs<List<string>>>(this.SearchServiceManager_EventGetSearchTermsCompleted);
      if (e.Error != null)
      {
        this.Title = (string) null;
        this.Items = new ObservableCollection<string>();
      }
      else
      {
        this.Items = new ObservableCollection<string>(e.Result);
        this.Title = Resource.SearchTerms_Title;
        this.LastRefreshTime = DateTime.UtcNow;
      }
    }
  }
}
