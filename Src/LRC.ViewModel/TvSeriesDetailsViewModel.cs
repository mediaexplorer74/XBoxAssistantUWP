// *********************************************************
// Type: LRC.ViewModel.TvSeriesDetailsViewModel
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
  public class TvSeriesDetailsViewModel : ViewModelBase
  {
    private const string PerfGetMediaDetail = "TvSeriesDetailsVewModel:GetMediaDetail";

    public TvSeriesDetailsViewModel() => this.LifetimeInMinutes = int.MaxValue;

    [DataMember]
    public MediaItem SelectedMediaDetails { get; set; }

    public override void Load()
    {
      if (this.SelectedMediaDetails == null)
        return;
      if (!this.SelectedMediaDetails.ShouldLoadData)
      {
        this.CurrentState = 0;
        this.SelectedMediaDetails.CurrentState = 0;
      }
      else
      {
        if (!this.ShouldLoadData)
          return;
        this.IsBusy = true;
        this.CurrentState = 3;
        this.SelectedMediaDetails.CurrentState = 3;
        IZuneCatalogServiceManager catalogServiceManager = ServiceManagerFactory.CreateZuneCatalogServiceManager();
        catalogServiceManager.EventGetVideoItemCompleted += new EventHandler<ServiceProxyEventArgs<string>>(TvSeriesDetailsViewModel.GetVideoItemCompleted);
        catalogServiceManager.GetMediaDetail(this.SelectedMediaDetails.Id, this.SelectedMediaDetails.MediaType.ToString(), (object) this);
      }
    }

    private static void GetVideoItemCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      (sender as IZuneCatalogServiceManager).EventGetVideoItemCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(TvSeriesDetailsViewModel.GetVideoItemCompleted);
      if (!(e.UserState is TvSeriesDetailsViewModel userState))
        return;
      userState.IsBusy = false;
      if (!(userState.SelectedMediaDetails is TVMediaItem selectedMediaDetails))
        return;
      selectedMediaDetails.IsBusy = false;
      if (e.Error == null)
      {
        ZuneVideoHelper.ParseTvSeriesResponse(e.Result, selectedMediaDetails);
        selectedMediaDetails.LastRefreshTime = DateTime.UtcNow;
        userState.LastRefreshTime = DateTime.UtcNow;
        if (selectedMediaDetails.Seasons == null || selectedMediaDetails.Seasons.Count == 0)
          userState.CurrentState = 2;
        else
          userState.CurrentState = 0;
        selectedMediaDetails.CurrentState = string.IsNullOrWhiteSpace(selectedMediaDetails.Description) ? 2 : 0;
      }
      else
      {
        userState.SelectedMediaDetails.CurrentState = 1;
        userState.CurrentState = 1;
      }
    }
  }
}
