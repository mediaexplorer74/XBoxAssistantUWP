// *********************************************************
// Type: LRC.ViewModel.VideoDetailViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Service;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class VideoDetailViewModel : ViewModelBase
  {
    private const string PerfGetMediaDetail = "VideoDetailViewModel:GetMediaDetail";
    private MediaItem selectedMediaDetails;

    public VideoDetailViewModel() => this.LifetimeInMinutes = int.MaxValue;

    [DataMember]
    public MediaItem SelectedMediaDetails
    {
      get => this.selectedMediaDetails;
      set
      {
        this.SetPropertyValue<MediaItem>(ref this.selectedMediaDetails, value, nameof (SelectedMediaDetails));
        if (this.selectedMediaDetails == null || this.selectedMediaDetails.RelatedItemsViewModel != null)
          return;
        this.SelectedMediaDetails.RelatedItemsViewModel = new RelatedItemsViewModel(true);
        this.SelectedMediaDetails.RelatedItemsViewModel.Id = this.SelectedMediaDetails.Id;
        switch (this.SelectedMediaDetails.MediaType)
        {
          case MediaType.movie:
            this.SelectedMediaDetails.RelatedItemsViewModel.ItemType = "MOVIE";
            break;
          case MediaType.tv_episode:
            this.SelectedMediaDetails.RelatedItemsViewModel.ItemType = "TVEPISODE";
            break;
          case MediaType.tv_series:
            this.SelectedMediaDetails.RelatedItemsViewModel.ItemType = "TVSERIES";
            break;
        }
      }
    }

    public override void Load()
    {
      if (this.ShouldLoadData)
      {
        if (this.SelectedMediaDetails.MediaType == MediaType.movie)
        {
          this.IsBusy = true;
          this.CurrentState = 3;
          IZuneCatalogServiceManager catalogServiceManager = ServiceManagerFactory.CreateZuneCatalogServiceManager();
          catalogServiceManager.EventGetVideoItemCompleted += new EventHandler<ServiceProxyEventArgs<string>>(VideoDetailViewModel.GetMovieItemCompleted);
          catalogServiceManager.GetMediaDetail(this.SelectedMediaDetails.Id, this.SelectedMediaDetails.MediaType.ToString(), (object) this);
        }
        else if (this.SelectedMediaDetails.MediaType == MediaType.tv_episode)
        {
          this.IsBusy = true;
          this.CurrentState = 3;
          IZuneCatalogServiceManager catalogServiceManager = ServiceManagerFactory.CreateZuneCatalogServiceManager();
          catalogServiceManager.EventGetVideoItemCompleted += new EventHandler<ServiceProxyEventArgs<string>>(VideoDetailViewModel.GetTvEpisodeItemCompleted);
          catalogServiceManager.GetMediaDetail(this.SelectedMediaDetails.Id, this.SelectedMediaDetails.MediaType.ToString(), (object) this);
        }
      }
      this.SelectedMediaDetails.RelatedItemsViewModel.Load();
    }

    private static void GetMovieItemCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      (sender as IZuneCatalogServiceManager).EventGetVideoItemCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(VideoDetailViewModel.GetMovieItemCompleted);
      if (e.UserState is VideoDetailViewModel)
      {
        VideoDetailViewModel userState = e.UserState as VideoDetailViewModel;
        userState.IsBusy = false;
        MovieMediaItem selectedMediaDetails = (MovieMediaItem) userState.SelectedMediaDetails;
        if (selectedMediaDetails != null)
        {
          selectedMediaDetails.IsBusy = false;
          if (e.Error == null)
          {
            ZuneVideoHelper.ParseMovieResponse(e.Result, selectedMediaDetails);
            selectedMediaDetails.LastRefreshTime = DateTime.UtcNow;
            userState.LastRefreshTime = DateTime.UtcNow;
            userState.CurrentState = 0;
            selectedMediaDetails.CurrentState = selectedMediaDetails.IsCastAndCrewNotFound ? 2 : 0;
          }
          else
          {
            userState.CurrentState = 1;
            selectedMediaDetails.CurrentState = 1;
          }
        }
        else
          userState.CurrentState = 1;
      }
      else
      {
        object userState1 = e.UserState;
      }
    }

    private static void GetTvEpisodeItemCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      (sender as IZuneCatalogServiceManager).EventGetVideoItemCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(VideoDetailViewModel.GetTvEpisodeItemCompleted);
      if (e.UserState is VideoDetailViewModel)
      {
        VideoDetailViewModel userState = e.UserState as VideoDetailViewModel;
        userState.IsBusy = false;
        TVEpisodeItem selectedMediaDetails = (TVEpisodeItem) userState.SelectedMediaDetails;
        if (selectedMediaDetails != null)
        {
          selectedMediaDetails.IsBusy = false;
          if (e.Error == null)
          {
            ZuneVideoHelper.ParseTvEpisodeResponse(e.Result, selectedMediaDetails);
            selectedMediaDetails.LastRefreshTime = DateTime.UtcNow;
            userState.LastRefreshTime = DateTime.UtcNow;
            selectedMediaDetails.CurrentState = 0;
            userState.CurrentState = 0;
          }
          else
          {
            selectedMediaDetails.CurrentState = 1;
            userState.CurrentState = 1;
          }
        }
        else
          userState.CurrentState = 1;
      }
      else
      {
        object userState1 = e.UserState;
      }
    }
  }
}
