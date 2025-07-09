// *********************************************************
// Type: LRC.ViewModel.ZuneCategoriesViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class ZuneCategoriesViewModel : ViewModelBase
  {
    private const string PerfGetFeedList = "ZuneCategoriesViewModel:GetFeedList";
    private const int LoadDelayInMilliseconds = 1000;
    private const string ComponentName = "ZuneCategoriesViewModel";
    private ObservableCollection<PivotItem> pivotItems;
    private MediaItemList selectedMediaDetals;
    private ZuneCategory category;

    public ZuneCategoriesViewModel()
    {
      this.LifetimeInMinutes = 1440;
      this.category = new ZuneCategory();
    }

    [XmlIgnore]
    public ObservableCollection<PivotItem> PivotItems
    {
      get
      {
        if (this.pivotItems == null)
          this.LoadPivotItems();
        return this.pivotItems;
      }
      set
      {
        this.SetPropertyValue<ObservableCollection<PivotItem>>(ref this.pivotItems, value, nameof (PivotItems));
      }
    }

    public int SelectedPivotTag
    {
      get
      {
        return !this.SelectedPivotTagInternal.HasValue ? 0 : this.SelectedPivotTagInternal.GetValueOrDefault();
      }
      set => this.SelectedPivotTagInternal = new int?(value);
    }

    [DataMember]
    public MediaItemList SelectedMediaDetails
    {
      get => this.selectedMediaDetals;
      set
      {
        this.selectedMediaDetals = value;
        if (value == null)
          return;
        if (value.Type == MediaType.movie)
        {
          this.Title = Resource.ZuneMarketplace_MoviesHeader;
        }
        else
        {
          if (value.Type != MediaType.tv_series)
            return;
          this.Title = Resource.ZuneMarketplace_TVHeader;
        }
      }
    }

    [XmlIgnore]
    public ObservableCollection<IItemList> ItemList
    {
      get
      {
        return this.SelectedMediaDetails.Type == MediaType.movie ? this.category.MovieList : this.category.TVList;
      }
    }

    [DataMember]
    public int? SelectedPivotTagInternal { get; set; }

    public void LoadPivotItems()
    {
      if (this.SelectedMediaDetails == null)
        return;
      ObservableCollection<PivotItem> observableCollection1 = new ObservableCollection<PivotItem>();
      ObservableCollection<PivotItem> observableCollection2 = new ObservableCollection<PivotItem>();
      int count = this.ItemList.Count;
      bool flag = false;
      for (int index = 0; index < count; ++index)
      {
        PivotItem pivotItem1 = new PivotItem();
        pivotItem1.Header = (object) this.ItemList[index].Title;
        ((FrameworkElement) pivotItem1).DataContext = (object) this.ItemList[index];
        ((FrameworkElement) pivotItem1).Tag = (object) index;
        PivotItem pivotItem2 = pivotItem1;
        if (this.ItemList[index] is MediaItemList)
          ((FrameworkElement) pivotItem2).Name = (this.ItemList[index] as MediaItemList).Category.ToString();
        if (!this.SelectedPivotTagInternal.HasValue)
        {
          if (string.Compare(this.ItemList[index].Title, this.SelectedMediaDetails.Title, StringComparison.Ordinal) == 0)
          {
            this.SelectedPivotTagInternal = new int?(index);
            flag = true;
          }
        }
        else if ((int) ((FrameworkElement) pivotItem2).Tag == this.SelectedPivotTag)
          flag = true;
        if (flag)
          observableCollection2.Add(pivotItem2);
        else
          observableCollection1.Add(pivotItem2);
      }
      if (observableCollection1.Count > 0)
      {
        foreach (PivotItem pivotItem in (Collection<PivotItem>) observableCollection1)
          observableCollection2.Add(pivotItem);
      }
      this.PivotItems = observableCollection2;
    }

    public override void Load()
    {
      if (this.ItemList[this.SelectedPivotTag].Items != null && !((ViewModelBase) this.ItemList[this.SelectedPivotTag]).ShouldLoadData)
        return;
      if (this.ItemList[this.SelectedPivotTag] is MediaItemList mediaItemList)
      {
        mediaItemList.IsBusy = true;
        mediaItemList.BusyIndicatorVerticalAlignment = (VerticalAlignment) 0;
      }
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        ZuneCategoriesUserState userState = new ZuneCategoriesUserState()
        {
          MediaItemList = (MediaItemList) this.ItemList[this.SelectedPivotTag],
          Index = this.SelectedPivotTag
        };
        IZuneCatalogServiceManager catalogServiceManager = ServiceManagerFactory.CreateZuneCatalogServiceManager();
        catalogServiceManager.EventGetFeedCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.GetFeedListByCategoryCompleted);
        catalogServiceManager.GetFeedList(((MediaItemList) this.ItemList[this.SelectedPivotTag]).Uri, (object) userState);
      });
    }

    public void LoadMoreDataOfCurrentPivot()
    {
      VideoItemList itemList = this.ItemList[this.SelectedPivotTag] as VideoItemList;
      if (itemList == null || itemList.IsBusy || !itemList.HasMoreItemToLoad)
        return;
      itemList.BusyIndicatorVerticalAlignment = (VerticalAlignment) 2;
      itemList.IsBusy = true;
      DispatcherTimer timer = new DispatcherTimer();
      timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
      timer.Tick += (EventHandler) ((s, e1) =>
      {
        timer.Stop();
        timer = (DispatcherTimer) null;
        itemList.LoadMoreData();
        itemList.IsBusy = false;
      });
      timer.Start();
    }

    private void GetFeedListByCategoryCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        (sender as IZuneCatalogServiceManager).EventGetFeedCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(this.GetFeedListByCategoryCompleted);
        if (e.UserState == null)
          return;
        ZuneCategoriesUserState userState = (ZuneCategoriesUserState) e.UserState;
        MediaItemList mediaItemList1 = userState.MediaItemList;
        if (mediaItemList1 == null)
          return;
        VideoItemList mediaItemList = (VideoItemList) this.ItemList[userState.Index];
        if (mediaItemList == null)
          return;
        if (e.Error == null)
        {
          IEnumerable<ViewModelBase> byCategory = ZuneVideoHelper.ParseByCategory(e.Result, mediaItemList1.Category, mediaItemList1.Type);
          List<ViewModelBase> viewModelBaseList = (List<ViewModelBase>) null;
          if (byCategory != null)
            viewModelBaseList = byCategory.ToList<ViewModelBase>();
          if (viewModelBaseList == null || viewModelBaseList.Count == 0)
          {
            mediaItemList.NoItemFound = Resource.NoItemsFound;
            mediaItemList.HasContent = false;
          }
          else
          {
            mediaItemList.NoItemFound = string.Empty;
            mediaItemList.HasContent = true;
            mediaItemList.Items = new ObservableCollection<ViewModelBase>();
            for (int index = 0; index < viewModelBaseList.Count; ++index)
              mediaItemList.Items.Add(viewModelBaseList[index]);
            mediaItemList.LastRefreshTime = DateTime.UtcNow;
            ThreadManager.UIThreadPost((SendOrPostCallback) delegate
            {
              mediaItemList.LoadMoreData();
            }, (object) this);
          }
        }
        else if (mediaItemList.Items == null || mediaItemList.Items.Count == 0)
        {
          mediaItemList.NoItemFound = Resource.LRC_Error_Code_FailedToRetrieveData;
          mediaItemList.HasContent = false;
        }
        mediaItemList.IsBusy = false;
      });
    }
  }
}
