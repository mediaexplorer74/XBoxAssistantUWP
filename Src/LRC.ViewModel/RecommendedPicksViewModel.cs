// *********************************************************
// Type: LRC.ViewModel.RecommendedPicksViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Service;
using LRC.Service.Search;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class RecommendedPicksViewModel : ViewModelBase
  {
    public const int MaxFeature = 25;
    private const string PerfGetRecommendedList = "RecommendedPicksViewModel:GetRecommendedList";
    private List<MediaItem> recommendedPicks;

    public RecommendedPicksViewModel() => this.LifetimeInMinutes = 1440;

    [DataMember]
    public List<MediaItem> RecommendedList
    {
      get => this.recommendedPicks;
      set
      {
        this.SetPropertyValue<List<MediaItem>>(ref this.recommendedPicks, value, nameof (RecommendedList));
      }
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      ISearchServiceManager searchServiceManager = ServiceManagerFactory.CreateSearchServiceManager();
      searchServiceManager.EventGetRecommendedItemsCompleted += new EventHandler<ServiceProxyEventArgs<List<SearchData>>>(RecommendedPicksViewModel.GetRecommendedListCompleted);
      searchServiceManager.GetRecommendedItems(LRC.Service.Search.Filter.Movies, MainViewModel.Instance.ConsoleLiveTVProviderTitleId, (object) this);
      this.IsBusy = true;
    }

    private static void GetRecommendedListCompleted(
      object sender,
      ServiceProxyEventArgs<List<SearchData>> e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetRecommendedItemsCompleted -= new EventHandler<ServiceProxyEventArgs<List<SearchData>>>(RecommendedPicksViewModel.GetRecommendedListCompleted);
      if (!(e.UserState is RecommendedPicksViewModel userState))
        return;
      userState.IsBusy = false;
      if (e.Error != null || e.Result == null)
        return;
      List<MediaItem> mediaItemList = new List<MediaItem>();
      foreach (SearchData searchData in e.Result)
      {
        MediaItem mediaItem;
        switch (searchData.ItemType.ToUpperInvariant())
        {
          case "MOVIE":
            mediaItem = (MediaItem) new MovieMediaItem();
            break;
          case "TVSERIES":
            mediaItem = (MediaItem) new TVMediaItem();
            break;
          case "TVEPISODE":
            mediaItem = (MediaItem) new TVEpisodeItem();
            break;
          default:
            mediaItem = new MediaItem();
            break;
        }
        mediaItem.Id = searchData.Id;
        mediaItem.ImageUrl = searchData.ImageUrl;
        mediaItem.Title = searchData.Name;
        mediaItem.Description = searchData.Description;
        mediaItemList.Add(mediaItem);
      }
      userState.RecommendedList = mediaItemList;
      userState.LastRefreshTime = DateTime.UtcNow;
    }
  }
}
