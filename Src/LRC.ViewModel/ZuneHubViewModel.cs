// *********************************************************
// Type: LRC.ViewModel.ZuneHubViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
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
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class ZuneHubViewModel : ViewModelBase
  {
    private const string PerfGetFeedList = "ZuneHubViewModel:GetFeedList";
    private const string GetFeedListBaseUrl = "tv/series/{0}/seasons/{1}/episodes";
    private const string GetMoreFeedListBaseUrl = "tv/series/{0}/seasons/{1}/episodes?afterMarker={2}";
    private MediaItemList itemList;
    private string emptyContentOrErrorText;

    public ZuneHubViewModel()
    {
      this.ItemList = new MediaItemList();
      this.LifetimeInMinutes = 1440;
      this.NoContentOrErrorText = string.Empty;
    }

    [DataMember]
    public MediaItemList ItemList
    {
      get => this.itemList;
      set => this.SetPropertyValue<MediaItemList>(ref this.itemList, value, nameof (ItemList));
    }

    [DataMember]
    public MediaItem SelectedMedia { get; set; }

    [XmlIgnore]
    [IgnoreDataMember]
    public string Template
    {
      get
      {
        return this.SelectedMedia == null || this.SelectedMedia.MediaType != MediaType.tv_series ? "TwoPerRowTemplate" : "EpisodeTemplate";
      }
    }

    [IgnoreDataMember]
    [XmlIgnore]
    public string Style
    {
      get
      {
        return this.SelectedMedia == null || this.SelectedMedia.MediaType != MediaType.tv_series ? "ListBoxWithWrapping" : "ListBoxWithCompression";
      }
    }

    [IgnoreDataMember]
    [XmlIgnore]
    public string NoContentOrErrorText
    {
      get => this.emptyContentOrErrorText;
      set
      {
        this.SetPropertyValue<string>(ref this.emptyContentOrErrorText, value, nameof (NoContentOrErrorText));
      }
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      this.IsBusy = true;
      this.ItemList = new MediaItemList();
      MediaItem item = this.SelectedMedia;
      if (item == null)
        return;
      this.ItemList.Items = (ObservableCollection<ViewModelBase>) null;
      this.ItemList.Title = item.Title;
      MediaItemList itemList = this.ItemList;
      string str;
      if (item.MediaType != MediaType.tv_series)
        str = Resource.ZuneVideoMarketPlace_HubTitle;
      else
        str = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.ZuneVideoMarketPlace_TV_EpisodeTitle, new object[1]
        {
          (object) item.Id
        });
      itemList.SubTitle = str;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        string empty = string.Empty;
        string path;
        if (item.MediaType == MediaType.tv_series)
          path = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "tv/series/{0}/seasons/{1}/episodes", new object[2]
          {
            (object) ((TVMediaItem) item).ParentId,
            (object) item.Id
          });
        else
          path = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "hubs/{0}", new object[1]
          {
            (object) item.Id
          });
        IZuneCatalogServiceManager catalogServiceManager = ServiceManagerFactory.CreateZuneCatalogServiceManager();
        catalogServiceManager.EventGetFeedCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.GetFeedListCompleted);
        catalogServiceManager.GetFeedList(path, (object) item.MediaType);
      });
    }

    public void FetchMoreData()
    {
      if (this.IsBusy || string.IsNullOrEmpty(this.ItemList.AfterMarker))
        return;
      this.IsBusy = true;
      MediaItem item = this.SelectedMedia;
      this.ItemList.BusyIndicatorVerticalAlignment = (VerticalAlignment) 2;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        string empty = string.Empty;
        string path = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "tv/series/{0}/seasons/{1}/episodes?afterMarker={2}", new object[3]
        {
          (object) ((TVMediaItem) item).ParentId,
          (object) item.Id,
          (object) this.ItemList.AfterMarker
        });
        IZuneCatalogServiceManager catalogServiceManager = ServiceManagerFactory.CreateZuneCatalogServiceManager();
        catalogServiceManager.EventGetFeedCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.GetMoreFeedListCompleted);
        catalogServiceManager.GetFeedList(path, (object) item.MediaType);
        this.ItemList.AfterMarker = string.Empty;
      });
    }

    private void GetMoreFeedListCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        (sender as IZuneCatalogServiceManager).EventGetFeedCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(this.GetMoreFeedListCompleted);
        if (e.Error == null && e.UserState != null)
        {
          MediaType userState = (MediaType) e.UserState;
          IEnumerable<MediaItem> source = (IEnumerable<MediaItem>) null;
          if (userState == MediaType.tv_series)
          {
            source = ZuneVideoHelper.GetTvepisodeList(e.Result);
            this.ItemList.AfterMarker = ZuneVideoHelper.GetNextLinkForFeedList(e.Result);
          }
          if (source != null)
          {
            List<MediaItem> newItems = source.ToList<MediaItem>();
            ThreadManager.UIThreadPost((SendOrPostCallback) delegate
            {
              foreach (ViewModelBase viewModelBase in newItems)
                this.itemList.Items.Add(viewModelBase);
              this.LastRefreshTime = DateTime.UtcNow;
            }, (object) this);
          }
        }
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          this.IsBusy = false;
        }, (object) this);
      });
    }

    private void GetFeedListCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        (sender as IZuneCatalogServiceManager).EventGetFeedCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(this.GetFeedListCompleted);
        if (e.Error == null && e.UserState != null)
        {
          IEnumerable<MediaItem> mediaItems;
          switch ((MediaType) e.UserState)
          {
            case MediaType.tv_series:
              mediaItems = ZuneVideoHelper.GetTvepisodeList(e.Result);
              this.ItemList.AfterMarker = ZuneVideoHelper.GetNextLinkForFeedList(e.Result);
              break;
            case MediaType.hub:
              mediaItems = ZuneVideoHelper.GetMovieList(e.Result);
              break;
            default:
              mediaItems = ZuneVideoHelper.GetMovieList(e.Result);
              break;
          }
          ObservableCollection<ViewModelBase> list = (ObservableCollection<ViewModelBase>) null;
          if (mediaItems != null)
          {
            list = new ObservableCollection<ViewModelBase>();
            foreach (MediaItem mediaItem in mediaItems)
              list.Add((ViewModelBase) mediaItem);
          }
          if (list == null || list.Count == 0)
            this.NoContentOrErrorText = Resource.NoItemsFound;
          else
            ThreadManager.UIThreadPost((SendOrPostCallback) delegate
            {
              this.itemList.Items = list;
              this.LastRefreshTime = DateTime.UtcNow;
            }, (object) this);
        }
        else
          this.NoContentOrErrorText = Resource.LRC_Error_Code_FailedToRetrieveData;
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          this.IsBusy = false;
        }, (object) this);
      });
    }
  }
}
