// *********************************************************
// Type: LRC.ViewModel.VideoFilteredViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class VideoFilteredViewModel : ViewModelBase
  {
    private const string PerfGetFeedList = "VideoFilteredViewModel:GetFeedList";
    private const int PageSize = 50;
    private const int InitialDisplayCount = 20;
    private const int SubsequentDisplayCount = 20;
    private const string GenreInMovieUri = "movieGenre/{0}/movies/?chunkSize={1}&orderby={2}&afterMarker={3}";
    private const string GenreInTelevisionUri = "tv/category/{0}/series/?chunkSize={1}&orderby={2}&afterMarker={3}";
    private const string NetworkUri = "tv/network/{0}/series/?chunkSize={1}&orderby={2}&afterMarker={3}";
    private const string StudioUri = "movie/studio/{0}/movies/?chunkSize={1}&orderby={2}&afterMarker={3}";
    private ObservableCollection<VideoFilter> televisionFilters;
    private ObservableCollection<VideoFilter> movieFilters;
    private ObservableCollection<PivotItem> pivotItems;
    private Guid requestId;
    private bool isAddingData;

    public VideoFilteredViewModel()
    {
      this.televisionFilters = new ObservableCollection<VideoFilter>();
      this.movieFilters = new ObservableCollection<VideoFilter>();
      this.AddTvFilter();
      this.AddMovieFilter();
    }

    [DataMember]
    public string TemplateName { get; set; }

    [DataMember]
    public MediaItemList SelectedMediaDetails { get; set; }

    [DataMember]
    public VideoFilterOrder OrderBy { get; set; }

    public int SelectedPivotTag
    {
      get
      {
        return !this.SelectedPivotTagInternal.HasValue ? 0 : this.SelectedPivotTagInternal.GetValueOrDefault();
      }
      set => this.SelectedPivotTagInternal = new int?(value);
    }

    [DataMember]
    public int? SelectedPivotTagInternal { get; set; }

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

    [XmlIgnore]
    public ObservableCollection<VideoFilter> ItemList
    {
      get
      {
        switch (this.SelectedMediaDetails.Type)
        {
          case MediaType.movie:
            return this.movieFilters;
          case MediaType.tv_series:
            return this.televisionFilters;
          default:
            return (ObservableCollection<VideoFilter>) null;
        }
      }
    }

    [XmlIgnore]
    public bool IsAddingData
    {
      get => this.isAddingData;
      set => this.SetPropertyValue<bool>(ref this.isAddingData, value, nameof (IsAddingData));
    }

    public override void Load() => this.Load(true);

    public void LoadPivotItems()
    {
      if (this.ItemList == null)
        return;
      this.requestId = Guid.NewGuid();
      ObservableCollection<PivotItem> observableCollection1 = new ObservableCollection<PivotItem>();
      ObservableCollection<PivotItem> observableCollection2 = new ObservableCollection<PivotItem>();
      int count = this.ItemList.Count;
      bool flag = false;
      this.Title = this.SelectedMediaDetails.Title;
      for (int index = 0; index < count; ++index)
      {
        this.ItemList[index].ItemList = new VideoItemList();
        this.ItemList[index].ItemList.Items = new ObservableCollection<ViewModelBase>();
        this.ItemList[index].ItemList.DisplayedItems = new ObservableCollection<ViewModelBase>();
        PivotItem pivotItem1 = new PivotItem();
        pivotItem1.Header = (object) this.ItemList[index].FilterName;
        ((FrameworkElement) pivotItem1).DataContext = (object) this.ItemList[index].ItemList;
        ((FrameworkElement) pivotItem1).Tag = (object) index;
        PivotItem pivotItem2 = pivotItem1;
        if (this.ItemList[index] != null)
          ((FrameworkElement) pivotItem2).Name = this.ItemList[index].FilterName.ToString();
        if (!this.SelectedPivotTagInternal.HasValue || (int) ((FrameworkElement) pivotItem2).Tag == this.SelectedPivotTag)
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

    public void FetchMoreDataOfCurrentPivot() => this.Load(false);

    public void Load(bool initialize)
    {
      if (this.ItemList == null || this.SelectedMediaDetails == null)
        return;
      VideoItemList itemList = this.ItemList[this.SelectedPivotTag].ItemList;
      if (initialize)
      {
        if (!itemList.ShouldLoadData || itemList.Items != null && itemList.Items.Count > 0)
          return;
        itemList.BusyIndicatorVerticalAlignment = (VerticalAlignment) 0;
      }
      else
      {
        if (itemList.IsBusy || string.IsNullOrWhiteSpace(itemList.AfterMarker))
          return;
        itemList.BusyIndicatorVerticalAlignment = (VerticalAlignment) 2;
      }
      itemList.IsBusy = true;
      itemList.Id = this.SelectedMediaDetails.Id;
      itemList.Type = this.SelectedMediaDetails.Type;
      VideoFilteredUserState userState = new VideoFilteredUserState()
      {
        MediaItemList = (MediaItemList) itemList,
        UniqueId = this.requestId,
        PivotIndex = this.SelectedPivotTag
      };
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        IZuneCatalogServiceManager catalogServiceManager = ServiceManagerFactory.CreateZuneCatalogServiceManager();
        catalogServiceManager.EventGetFeedCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.GetFeedListCompleted);
        catalogServiceManager.GetFeedList(this.GetUri(), (object) userState);
      });
    }

    private static IEnumerable<ViewModelBase> ParseTopEntries(string result, MediaType type)
    {
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return (IEnumerable<ViewModelBase>) null;
      switch (type)
      {
        case MediaType.movie:
          return document.Descendants(ZuneNamespace.Atom + "entry").Select<XElement, ViewModelBase>((Func<XElement, ViewModelBase>) (entry => ZuneVideoHelper.ParseTopMovies(entry)));
        case MediaType.tv_series:
          return document.Descendants(ZuneNamespace.Atom + "entry").Select<XElement, ViewModelBase>((Func<XElement, ViewModelBase>) (entry => ZuneVideoHelper.ParseTopTvSeries(entry)));
        default:
          return (IEnumerable<ViewModelBase>) null;
      }
    }

    private void AddTvFilter()
    {
      this.televisionFilters.Add(new VideoFilter()
      {
        FilterName = Resource.ZuneVideoMarketPlace_TopSellingFilter,
        FilterBy = VideoFilterOrder.SalesRank
      });
      this.televisionFilters.Add(new VideoFilter()
      {
        FilterName = Resource.ZuneVideoMarketPlace_ReleaseDateFilter,
        FilterBy = VideoFilterOrder.ReleaseDate
      });
      this.televisionFilters.Add(new VideoFilter()
      {
        FilterName = Resource.ZuneVideoMarketPlace_TitleFilter,
        FilterBy = VideoFilterOrder.Title
      });
    }

    private void AddMovieFilter()
    {
      this.movieFilters.Add(new VideoFilter()
      {
        FilterName = Resource.ZuneVideoMarketPlace_TopSellingFilter,
        FilterBy = VideoFilterOrder.DownloadRank
      });
      this.movieFilters.Add(new VideoFilter()
      {
        FilterName = Resource.ZuneVideoMarketPlace_TopRentalFilter,
        FilterBy = VideoFilterOrder.RentalRank
      });
      this.movieFilters.Add(new VideoFilter()
      {
        FilterName = Resource.ZuneVideoMarketPlace_TitleFilter,
        FilterBy = VideoFilterOrder.Title
      });
    }

    private string GetUri()
    {
      string empty = string.Empty;
      VideoFilterOrder videoFilterOrder = this.ItemList[this.SelectedPivotTag].FilterBy;
      string afterMarker = this.ItemList[this.SelectedPivotTag].ItemList.AfterMarker;
      string uri;
      if (this.SelectedMediaDetails.Type == MediaType.tv_series)
      {
        if (videoFilterOrder == VideoFilterOrder.None)
          videoFilterOrder = VideoFilterOrder.SalesRank;
        if (this.SelectedMediaDetails.Category == MediaCategory.Genres)
          uri = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "tv/category/{0}/series/?chunkSize={1}&orderby={2}&afterMarker={3}", (object) this.SelectedMediaDetails.Id, (object) 50, (object) videoFilterOrder, (object) afterMarker);
        else
          uri = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "tv/network/{0}/series/?chunkSize={1}&orderby={2}&afterMarker={3}", (object) this.SelectedMediaDetails.Id, (object) 50, (object) videoFilterOrder, (object) afterMarker);
      }
      else
      {
        if (videoFilterOrder == VideoFilterOrder.None)
          videoFilterOrder = VideoFilterOrder.Title;
        if (this.SelectedMediaDetails.Category == MediaCategory.Genres)
          uri = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "movieGenre/{0}/movies/?chunkSize={1}&orderby={2}&afterMarker={3}", (object) this.SelectedMediaDetails.Id, (object) 50, (object) videoFilterOrder, (object) afterMarker);
        else
          uri = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "movie/studio/{0}/movies/?chunkSize={1}&orderby={2}&afterMarker={3}", (object) this.SelectedMediaDetails.Id, (object) 50, (object) videoFilterOrder, (object) afterMarker);
      }
      return uri;
    }

    private void AddFormattedItem(MediaItemList itemList, ViewModelBase item)
    {
      if (item == null)
        return;
      if (itemList.Items == null)
        itemList.Items = new ObservableCollection<ViewModelBase>();
      lock (this)
      {
        if (itemList.Items.Count == 0)
        {
          itemList.Items.Add((ViewModelBase) new FormattedMediaItems()
          {
            DisplayedItems1 = item
          });
        }
        else
        {
          FormattedMediaItems formattedMediaItems = itemList.Items.Last<ViewModelBase>() as FormattedMediaItems;
          if (!formattedMediaItems.IsDisplayedItems2Available)
            formattedMediaItems.DisplayedItems2 = item;
          else
            itemList.Items.Add((ViewModelBase) new FormattedMediaItems()
            {
              DisplayedItems1 = item
            });
        }
      }
    }

    private void GetFeedListCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      (sender as IZuneCatalogServiceManager).EventGetFeedCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(this.GetFeedListCompleted);
      this.IsBusy = false;
      VideoFilteredUserState userState = (VideoFilteredUserState) e.UserState;
      if (userState == null || userState.MediaItemList == null || !userState.UniqueId.Equals(this.requestId))
        return;
      MediaType type = userState.MediaItemList.Type;
      VideoItemList mediaItemList = this.ItemList[userState.PivotIndex].ItemList;
      if (e.Error != null)
      {
        if (mediaItemList.Items == null || mediaItemList.Items.Count == 0)
        {
          mediaItemList.NoItemFound = Resource.LRC_Error_Code_FailedToRetrieveData;
          mediaItemList.HasContent = false;
        }
        mediaItemList.IsBusy = false;
      }
      else
      {
        IEnumerable<ViewModelBase> topEntries = VideoFilteredViewModel.ParseTopEntries(e.Result, type);
        mediaItemList.AfterMarker = ZuneVideoHelper.GetNextLinkForFeedList(e.Result);
        this.IsAddingData = true;
        List<ViewModelBase> items = (List<ViewModelBase>) null;
        if (topEntries != null)
          items = topEntries.ToList<ViewModelBase>();
        if (items != null && items.Count > 0)
        {
          mediaItemList.LastRefreshTime = DateTime.UtcNow;
          mediaItemList.NoItemFound = string.Empty;
          mediaItemList.HasContent = true;
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            foreach (ViewModelBase viewModelBase in items)
              this.AddFormattedItem((MediaItemList) mediaItemList, viewModelBase);
          }, (object) this);
        }
        else if (mediaItemList.Items == null || mediaItemList.Items.Count == 0)
        {
          mediaItemList.NoItemFound = Resource.NoItemsFound;
          mediaItemList.HasContent = false;
        }
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          mediaItemList.IsBusy = false;
          this.IsAddingData = false;
        }, (object) this);
      }
    }
  }
}
