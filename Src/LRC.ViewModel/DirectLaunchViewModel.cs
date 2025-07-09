// *********************************************************
// Type: LRC.ViewModel.DirectLaunchViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
using LRC.Service.Search;
using System;


namespace LRC.ViewModel
{
  public class DirectLaunchViewModel : ViewModelBase
  {
    private const string ComponentName = "DirectLaunchViewModel";
    private const string ApplicationProductType = "61";
    private NowPlayingItemViewModel launchItem;

    public event EventHandler<LRCAsyncCompletedEventArgs> EventGetLaunchDataCompleted;

    public uint TitleId { get; set; }

    public string PartnerMediaId { get; set; }

    public bool NeedToDirectLaunch { get; set; }

    public NowPlayingItemViewModel LaunchItem
    {
      get => this.launchItem;
      set => this.launchItem = value;
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      this.LaunchItem = (NowPlayingItemViewModel) null;
      this.NeedToDirectLaunch = false;
      if (this.TitleId <= 0U)
      {
        this.CurrentState = 2;
        this.NotifyAsyncOperationCompleted(this.EventGetLaunchDataCompleted, ErrorCodeEnum.Error);
      }
      else
      {
        this.CurrentState = 3;
        if (string.IsNullOrEmpty(this.PartnerMediaId))
        {
          ISearchServiceManager searchServiceManager = ServiceManagerFactory.CreateSearchServiceManager();
          searchServiceManager.EventGetNowPlayingAppDetailsCompleted += new EventHandler<ServiceProxyEventArgs<XboxGameData>>(this.SearchServiceManager_EventGetAppDetailsCompleted);
          searchServiceManager.GetNowPlayingAppDetails(this.TitleId, (object) this.TitleId);
        }
        else
        {
          GetMediaInfoUtil getMediaInfoUtil = new GetMediaInfoUtil();
          getMediaInfoUtil.EventGetNowPlayingItemCompleted += new EventHandler<ServiceProxyEventArgs<NowPlayingItemViewModel>>(this.GetMediaInfoUtil_EventGetNowPlayingItemCompleted);
          getMediaInfoUtil.GetNowPlayingItemDetailsAsync(this.TitleId, this.PartnerMediaId);
        }
      }
    }

    private void SearchServiceManager_EventGetAppDetailsCompleted(
      object sender,
      ServiceProxyEventArgs<XboxGameData> e)
    {
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetNowPlayingAppDetailsCompleted -= new EventHandler<ServiceProxyEventArgs<XboxGameData>>(this.SearchServiceManager_EventGetAppDetailsCompleted);
      ErrorCodeEnum errorCodeEnum;
      if (e.Error == null && e.Result != null)
      {
        this.CurrentState = 0;
        errorCodeEnum = ErrorCodeEnum.None;
        XboxGameData result = e.Result;
        NowPlayingItemViewModel playingItemViewModel = new NowPlayingItemViewModel();
        playingItemViewModel.AppId = result.Id;
        playingItemViewModel.Title = result.Name;
        playingItemViewModel.BackgroundImageUrl = result.BackgroundImageUrl;
        playingItemViewModel.Description = result.Description;
        playingItemViewModel.ItemType = result.TitleId != 4294838225U ? (!(e.Result.ProductType == "61") ? NowPlayingType.Game : NowPlayingType.Application) : NowPlayingType.Dash;
        if (this.TitleId == 1481115739U)
          playingItemViewModel.ImageUrl = "/UI/Images/Tile_Zune.jpg";
        else
          playingItemViewModel.ImageUrl = result.ImageUrl;
        if (string.IsNullOrEmpty(playingItemViewModel.ImageUrl))
          playingItemViewModel.ImageUrl = playingItemViewModel.ItemType == NowPlayingType.Game ? "/UI/Images/DefaultBoxArt/xboxgame.png" : "/UI/Images/DefaultBoxArt/xboxapp.png";
        this.LaunchItem = playingItemViewModel;
        this.LaunchItem.ProcessUpdates = false;
        this.LaunchItem.TitleId = this.TitleId;
        this.NeedToDirectLaunch = this.LaunchItem != null && this.LaunchItem.ItemType != NowPlayingType.Application && this.LaunchItem.ItemType != NowPlayingType.Dash && this.LaunchItem.ItemType != NowPlayingType.Unknown;
      }
      else if (e.Error != null)
      {
        this.CurrentState = 1;
        errorCodeEnum = ErrorCodeEnum.Error;
      }
      else
      {
        this.CurrentState = 2;
        errorCodeEnum = ErrorCodeEnum.FailedToRetrieveData;
      }
      this.NotifyAsyncOperationCompleted(this.EventGetLaunchDataCompleted, errorCodeEnum);
    }

    private void GetMediaInfoUtil_EventGetNowPlayingItemCompleted(
      object sender,
      ServiceProxyEventArgs<NowPlayingItemViewModel> e)
    {
      if (sender is GetMediaInfoUtil getMediaInfoUtil)
        getMediaInfoUtil.EventGetNowPlayingItemCompleted -= new EventHandler<ServiceProxyEventArgs<NowPlayingItemViewModel>>(this.GetMediaInfoUtil_EventGetNowPlayingItemCompleted);
      ErrorCodeEnum errorCodeEnum;
      if (e.Error == null && e.Result != null)
      {
        this.CurrentState = 0;
        errorCodeEnum = ErrorCodeEnum.None;
        this.LaunchItem = e.Result;
        this.LaunchItem.ProcessUpdates = false;
        this.LaunchItem.TitleId = this.TitleId;
        this.NeedToDirectLaunch = true;
      }
      else if (e.Error != null)
      {
        this.CurrentState = 1;
        errorCodeEnum = ErrorCodeEnum.Error;
      }
      else
      {
        this.CurrentState = 2;
        errorCodeEnum = ErrorCodeEnum.FailedToRetrieveData;
      }
      this.NotifyAsyncOperationCompleted(this.EventGetLaunchDataCompleted, errorCodeEnum);
    }
  }
}
