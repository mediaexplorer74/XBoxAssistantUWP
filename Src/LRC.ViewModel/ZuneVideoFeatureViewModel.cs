// *********************************************************
// Type: LRC.ViewModel.ZuneVideoFeatureViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  public class ZuneVideoFeatureViewModel : ViewModelBase
  {
    public const int MaxFeature = 8;
    private const string PerfGetFeedList = "ZuneVideoFeatureViewModel:GetFeedList";
    private ObservableCollection<MediaItem> featureList;

    public ZuneVideoFeatureViewModel()
    {
      this.LifetimeInMinutes = 1440;
      this.featureList = new ObservableCollection<MediaItem>();
    }

    [DataMember]
    public ObservableCollection<MediaItem> FeatureList
    {
      get => this.featureList;
      set
      {
        this.SetPropertyValue<ObservableCollection<MediaItem>>(ref this.featureList, value, nameof (FeatureList));
      }
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      this.IsBusy = true;
      IZuneCatalogServiceManager catalogServiceManager = ServiceManagerFactory.CreateZuneCatalogServiceManager();
      catalogServiceManager.EventGetFeedCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.GetFeedListCompleted);
      catalogServiceManager.GetFeedList("music/hub/video", (object) null);
    }

    private void GetFeedListCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is IZuneCatalogServiceManager catalogServiceManager)
        catalogServiceManager.EventGetFeedCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(this.GetFeedListCompleted);
      this.IsBusy = false;
      if (e.Error != null)
        return;
      this.ParseResponse(e.Result);
      this.LastRefreshTime = DateTime.UtcNow;
    }

    private void ParseResponse(string result)
    {
      XDocument document = ZuneVideoHelper.SafeParseDocument(result);
      if (document == null)
        return;
      List<XElement> xelementList = new List<XElement>();
      IEnumerable<XElement> collection = document.Descendants(ZuneNamespace.Atom + "entry").Select<XElement, XElement>((Func<XElement, XElement>) (entry => entry));
      xelementList.AddRange(collection);
      List<MediaItem> mediaItemList = (List<MediaItem>) null;
      try
      {
        mediaItemList = xelementList[0].Descendants(ZuneNamespace.ZuneDefaultNamespace + "editorialItem").Select<XElement, MediaItem>((Func<XElement, MediaItem>) (entry => ZuneVideoHelper.GetMediaItemEntry(entry))).Where<MediaItem>((Func<MediaItem, bool>) (item => item != null)).ToList<MediaItem>();
      }
      catch (InvalidOperationException ex)
      {
      }
      catch (NullReferenceException ex)
      {
      }
      if (mediaItemList == null)
        return;
      ObservableCollection<MediaItem> observableCollection = new ObservableCollection<MediaItem>();
      for (int index = 0; index < mediaItemList.Count; ++index)
      {
        if (index < 5 || index > 8)
        {
          MediaItem mediaItem = mediaItemList[index];
          if (mediaItem.MediaType == MediaType.movie || mediaItem.MediaType == MediaType.tv_series)
          {
            observableCollection.Add(mediaItem);
            if (observableCollection.Count >= 8)
              break;
          }
        }
      }
      this.FeatureList = observableCollection;
    }
  }
}
