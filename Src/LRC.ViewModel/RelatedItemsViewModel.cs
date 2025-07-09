// *********************************************************
// Type: LRC.ViewModel.RelatedItemsViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
using LRC.Service.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class RelatedItemsViewModel : ViewModelBase
  {
    public const int MaxRelatedItems = 10;
    private const string ComponentName = "RelatedItemsViewModel";
    private const string PerfGetRelatedItems = "RelatedItemsViewModel:GetRelatedItems";
    private string id;
    private string itemType;
    private bool isRelatedAvailable;
    private string relatedItemsNotFound;
    private ObservableCollection<SearchItemViewModel> relatedItems;

    public RelatedItemsViewModel()
    {
      this.relatedItemsNotFound = string.Empty;
      this.isRelatedAvailable = false;
    }

    public RelatedItemsViewModel(bool useMicrosoftItemsOnly)
      : this()
    {
      this.UseMicrosoftItemsOnly = useMicrosoftItemsOnly;
    }

    public string RelatedItemsNotFound
    {
      get => this.relatedItemsNotFound;
      set
      {
        this.SetPropertyValue<string>(ref this.relatedItemsNotFound, value, nameof (RelatedItemsNotFound));
      }
    }

    [IgnoreDataMember]
    [XmlIgnore]
    public bool IsRelatedAvailable
    {
      get => this.isRelatedAvailable;
      set
      {
        this.SetPropertyValue<bool>(ref this.isRelatedAvailable, value, nameof (IsRelatedAvailable));
      }
    }

    public string Id
    {
      get => this.id;
      set => this.SetPropertyValue<string>(ref this.id, value, nameof (Id));
    }

    [DefaultValue(false)]
    public bool UseMicrosoftItemsOnly { get; set; }

    public string ItemType
    {
      get => this.itemType;
      set => this.SetPropertyValue<string>(ref this.itemType, value, nameof (ItemType));
    }

    public ObservableCollection<SearchItemViewModel> RelatedItems
    {
      get => this.relatedItems;
      set
      {
        this.SetPropertyValue<ObservableCollection<SearchItemViewModel>>(ref this.relatedItems, value, nameof (RelatedItems));
      }
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      this.IsBusy = true;
      ISearchServiceManager searchServiceManager = ServiceManagerFactory.CreateSearchServiceManager();
      searchServiceManager.EventGetRelatedItemsCompleted += new EventHandler<ServiceProxyEventArgs<List<SearchData>>>(this.EventGetRelatedItemsCompleted);
      searchServiceManager.GetRelatedItems(this.Id, this.ItemType, this.UseMicrosoftItemsOnly, MainViewModel.Instance.ConsoleLiveTVProviderTitleId, (object) this.Id);
    }

    private void EventGetRelatedItemsCompleted(
      object sender,
      ServiceProxyEventArgs<List<SearchData>> e)
    {
      ISearchServiceManager searchServiceManager = sender as ISearchServiceManager;
      this.IsBusy = false;
      if (searchServiceManager != null)
        searchServiceManager.EventGetRelatedItemsCompleted -= new EventHandler<ServiceProxyEventArgs<List<SearchData>>>(this.EventGetRelatedItemsCompleted);
      if (e.Error != null)
      {
        this.ShowNonfatalErrorMessage(Resource.Search_ErrorText);
        this.RelatedItems = new ObservableCollection<SearchItemViewModel>();
        this.RelatedItemsNotFound = Resource.RelatedItems_NoResultsFound;
      }
      else
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          ObservableCollection<SearchItemViewModel> items = new ObservableCollection<SearchItemViewModel>();
          int num = Math.Min(10, e.Result.Count);
          for (int index = 0; index < num; ++index)
            items.Add(new SearchItemViewModel(e.Result[index]));
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            this.RelatedItems = items;
            this.LastRefreshTime = DateTime.UtcNow;
            this.IsRelatedAvailable = this.RelatedItems.Count > 0;
            if (this.IsRelatedAvailable)
              this.RelatedItemsNotFound = string.Empty;
            else
              this.RelatedItemsNotFound = Resource.RelatedItems_NoResultsFound;
          }, (object) null);
        });
    }
  }
}
