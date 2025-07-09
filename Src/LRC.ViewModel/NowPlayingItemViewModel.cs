// *********************************************************
// Type: LRC.ViewModel.NowPlayingItemViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
using LRC.Service.Search;
using Microsoft.Xmedia.Client.WindowsPhone;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class NowPlayingItemViewModel : ViewModelBase
  {
    private const string ComponentName = "NowPlayingItemViewModel";
    private const string ApplicationProductType = "61";
    private NowPlayingType nowPlayingItemType;
    private NowPlayingType nowPlayingAppType;
    private string nowPlayingAppTitle;
    private string nowPlayingAppImageUrl;
    private string nowPlayingAppBackgroundImageUrl;
    private string nowPlayingAppDescription;
    private string nowPlayingMediaImageUrl;
    private string nowPlayingMediaTitle;
    private string nowPlayingMediaSubTitle;
    private NowPlayingType nowPlayingMediaType;
    private string nowPlayingMediaBackgroundImageUrl;
    private string nowPlayingMediaDescription;
    private bool hasNoPendingCommand = true;
    private string partnerMediaId;
    private uint titleId;
    private ulong duration;
    private string displayedDuration;
    private ulong position;
    private string displayedPosition;
    private string progressDisplay;
    private string detailsUrl;
    private bool showMoreInfo;
    private ObservableCollection<string> moreInfoDetails;
    private bool isDashboardOrUnknown = true;
    private bool isNotDashboardOrUnknown;
    private bool isAppAndNoMediaPlaying;
    private uint previousTitleId;
    private string previousPartnerMediaId;
    private bool processUpdates = true;
    private bool isBeaconSet;

    public event EventHandler<LRCAsyncCompletedEventArgs> SendCommandCompletedEvent;

    [IgnoreDataMember]
    public static NowPlayingMediaState NowPlayingMediaState
    {
      get
      {
        if (MainViewModel.Instance.CurrentMediaState == null)
          return NowPlayingMediaState.Unknown;
        switch (MainViewModel.Instance.CurrentMediaState.MediaTransportState)
        {
          case MediaTransportState.Starting:
          case MediaTransportState.Playing:
          case MediaTransportState.Buffering:
            double rate = (double) MainViewModel.Instance.CurrentMediaState.Rate;
            if (rate < 0.0)
              return NowPlayingMediaState.Rewinding;
            if (rate < 1.0)
              return NowPlayingMediaState.SlowMotion;
            return rate > 1.0 ? NowPlayingMediaState.Fastforwarding : NowPlayingMediaState.Playing;
          case MediaTransportState.Paused:
            return NowPlayingMediaState.Paused;
          default:
            return NowPlayingMediaState.Unknown;
        }
      }
    }

    [DataMember]
    public NowPlayingType ItemType
    {
      get => this.nowPlayingItemType;
      set
      {
        this.SetPropertyValue<NowPlayingType>(ref this.nowPlayingItemType, value, "NowPlayingItemType");
        this.ProcessNewItemType();
      }
    }

    public bool HasNoPendingCommand
    {
      get => this.hasNoPendingCommand;
      set
      {
        this.SetPropertyValue<bool>(ref this.hasNoPendingCommand, value, nameof (HasNoPendingCommand));
      }
    }

    [DataMember]
    public string PartnerMediaId
    {
      get => this.partnerMediaId;
      set
      {
        this.SetPropertyValue<string>(ref this.partnerMediaId, value, nameof (PartnerMediaId));
        this.ProcessNewPartnerMediaId();
      }
    }

    [DataMember]
    public string AppId { get; set; }

    [DataMember]
    public string MediaId { get; set; }

    public string ParentMediaId { get; set; }

    [IgnoreDataMember]
    [XmlIgnore]
    public uint TitleId
    {
      get => this.titleId;
      set
      {
        this.SetPropertyValue<uint>(ref this.titleId, value, nameof (TitleId));
        this.ProcessNewTitleId();
      }
    }

    [DataMember]
    public ulong Duration
    {
      get => this.duration;
      set => this.SetPropertyValue<ulong>(ref this.duration, value, nameof (Duration));
    }

    [IgnoreDataMember]
    public string DisplayedDuration
    {
      get => this.displayedDuration;
      private set
      {
        this.SetPropertyValue<string>(ref this.displayedDuration, value, nameof (DisplayedDuration));
      }
    }

    [IgnoreDataMember]
    public string DetailsUrl
    {
      get => this.detailsUrl;
      set => this.SetPropertyValue<string>(ref this.detailsUrl, value, nameof (DetailsUrl));
    }

    [IgnoreDataMember]
    public string ProgressDisplay
    {
      get => this.progressDisplay;
      private set
      {
        this.SetPropertyValue<string>(ref this.progressDisplay, value, nameof (ProgressDisplay));
      }
    }

    [DataMember]
    public ulong Position
    {
      get => this.position;
      set => this.SetPropertyValue<ulong>(ref this.position, value, nameof (Position));
    }

    [XmlIgnore]
    [IgnoreDataMember]
    public string DisplayedPosition
    {
      get => this.displayedPosition;
      private set
      {
        this.SetPropertyValue<string>(ref this.displayedPosition, value, nameof (DisplayedPosition));
      }
    }

    [DataMember]
    public bool ShowMoreInfo
    {
      get => this.showMoreInfo;
      set => this.SetPropertyValue<bool>(ref this.showMoreInfo, value, nameof (ShowMoreInfo));
    }

    [DataMember]
    public ObservableCollection<string> MoreInfoDetails
    {
      get => this.moreInfoDetails;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.moreInfoDetails, value, nameof (MoreInfoDetails));
      }
    }

    [DataMember]
    public bool IsDashboardOrUnknown
    {
      get => this.isDashboardOrUnknown;
      set
      {
        this.SetPropertyValue<bool>(ref this.isDashboardOrUnknown, value, nameof (IsDashboardOrUnknown));
      }
    }

    [DataMember]
    public bool IsNotDashboardOrUnknown
    {
      get => this.isNotDashboardOrUnknown;
      set
      {
        this.SetPropertyValue<bool>(ref this.isNotDashboardOrUnknown, value, nameof (IsNotDashboardOrUnknown));
      }
    }

    [DataMember]
    public bool IsAppAndNoMediaPlaying
    {
      get => this.isAppAndNoMediaPlaying;
      set
      {
        this.SetPropertyValue<bool>(ref this.isAppAndNoMediaPlaying, value, nameof (IsAppAndNoMediaPlaying));
      }
    }

    [DataMember]
    public bool IsBeaconSet
    {
      get => this.isBeaconSet;
      set => this.SetPropertyValue<bool>(ref this.isBeaconSet, value, nameof (IsBeaconSet));
    }

    [XmlIgnore]
    public string Beacon_ActionText
    {
      get
      {
        return !this.isBeaconSet ? Resource.Beacon_SetBeaconButtonText : Resource.Beacon_EditBeaconButtonText;
      }
    }

    public bool ProcessUpdates
    {
      get => this.processUpdates;
      set => this.processUpdates = value;
    }

    public static bool IsNowPlaying(MediaItem mediaItem)
    {
      if (mediaItem == null)
        throw new ArgumentNullException(nameof (mediaItem));
      return MainViewModel.Instance != null && MainViewModel.Instance.NowPlayingItem != null && (string.Equals(MainViewModel.Instance.NowPlayingItem.PartnerMediaId, mediaItem.Id, StringComparison.OrdinalIgnoreCase) || string.Equals(MainViewModel.Instance.NowPlayingItem.ParentMediaId, mediaItem.Id, StringComparison.OrdinalIgnoreCase) || mediaItem is MusicTrackItem musicTrackItem && string.Equals(MainViewModel.Instance.NowPlayingItem.ParentMediaId, musicTrackItem.AlbumId, StringComparison.OrdinalIgnoreCase) || mediaItem is MusicVideoItem musicVideoItem && string.Equals(MainViewModel.Instance.NowPlayingItem.ParentMediaId, musicVideoItem.AlbumId, StringComparison.OrdinalIgnoreCase));
    }

    public static bool IsNowPlaying(SearchItemViewModel searchItem)
    {
      if (searchItem == null)
        throw new ArgumentNullException(nameof (searchItem));
      bool flag = false;
      if (MainViewModel.Instance != null && MainViewModel.Instance.NowPlayingItem != null)
      {
        if (searchItem.TitleId > 0U && (int) searchItem.TitleId == (int) MainViewModel.Instance.NowPlayingItem.TitleId)
          flag = true;
        else if (!string.IsNullOrEmpty(MainViewModel.Instance.NowPlayingItem.MediaId) && string.Equals(searchItem.Id, MainViewModel.Instance.NowPlayingItem.MediaId, StringComparison.OrdinalIgnoreCase))
          flag = true;
        else if (1481115739U == MainViewModel.Instance.NowPlayingItem.TitleId && (string.Equals(MainViewModel.Instance.NowPlayingItem.Title, searchItem.Title, StringComparison.Ordinal) || string.Equals(MainViewModel.Instance.NowPlayingItem.SubTitle, searchItem.Title, StringComparison.Ordinal)))
        {
          if (string.IsNullOrEmpty(MainViewModel.Instance.NowPlayingItem.MediaId) && string.IsNullOrEmpty(MainViewModel.Instance.NowPlayingItem.PartnerMediaId))
            flag = true;
          else if (searchItem.Providers != null)
          {
            foreach (ProviderViewModel provider in (Collection<ProviderViewModel>) searchItem.Providers)
            {
              if (provider.TitleId == 1481115739U)
              {
                flag = true;
                break;
              }
            }
          }
        }
        else if (string.Equals(GetMediaInfoUtil.GetMediaIdFromTitleId(MainViewModel.Instance.NowPlayingItem.TitleId), searchItem.Id, StringComparison.OrdinalIgnoreCase))
          flag = true;
      }
      return flag;
    }

    public void UpdateProgress(ulong mediaPosition, ulong mediaDuration)
    {
      this.Position = mediaPosition;
      this.Duration = mediaDuration;
      this.UpdateProgressDisplay();
    }

    public void SendCommand(ControlKey controlKey)
    {
      this.HasNoPendingCommand = false;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        MainViewModel.Instance.SendMediaControlCommand(controlKey);
        this.NotifyAsyncOperationCompleted(new EventHandler<LRCAsyncCompletedEventArgs>(this.SendCommandCompleted), ErrorCodeEnum.None);
      });
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      this.ProcessNewTitleId();
      this.ProcessNewPartnerMediaId();
    }

    private void ProcessNewTitleId()
    {
      if (!this.ProcessUpdates || (int) this.previousTitleId == (int) this.TitleId)
        return;
      this.previousTitleId = this.TitleId;
      this.nowPlayingAppBackgroundImageUrl = (string) null;
      this.nowPlayingAppDescription = (string) null;
      this.nowPlayingAppImageUrl = (string) null;
      this.nowPlayingAppTitle = (string) null;
      this.nowPlayingAppType = NowPlayingType.Unknown;
      this.UpdateView();
      ISearchServiceManager searchServiceManager = ServiceManagerFactory.CreateSearchServiceManager();
      searchServiceManager.EventGetNowPlayingAppDetailsCompleted += new EventHandler<ServiceProxyEventArgs<XboxGameData>>(this.SearchServiceManager_EventGetNowPlayingAppDetailsCompleted);
      searchServiceManager.GetNowPlayingAppDetails(this.TitleId, (object) this.TitleId);
    }

    private void ProcessNewPartnerMediaId()
    {
      if (this.TitleId == 0U || !this.ProcessUpdates || string.Equals(this.previousPartnerMediaId, this.PartnerMediaId, StringComparison.OrdinalIgnoreCase))
        return;
      this.previousPartnerMediaId = this.PartnerMediaId;
      this.MediaId = (string) null;
      this.ParentMediaId = (string) null;
      this.nowPlayingMediaBackgroundImageUrl = (string) null;
      this.nowPlayingMediaDescription = (string) null;
      this.nowPlayingMediaImageUrl = (string) null;
      this.nowPlayingMediaTitle = (string) null;
      this.nowPlayingMediaType = NowPlayingType.Unknown;
      this.DetailsUrl = (string) null;
      if (string.IsNullOrEmpty(this.PartnerMediaId))
      {
        this.UpdateView();
      }
      else
      {
        GetMediaInfoUtil getMediaInfoUtil = new GetMediaInfoUtil();
        getMediaInfoUtil.EventGetNowPlayingItemCompleted += new EventHandler<ServiceProxyEventArgs<NowPlayingItemViewModel>>(this.GetMediaInfoUtil_EventGetNowPlayingItemCompleted);
        getMediaInfoUtil.GetNowPlayingItemDetailsAsync(this.TitleId, this.PartnerMediaId);
      }
    }

    private void SendCommandCompleted(object sender, LRCAsyncCompletedEventArgs e)
    {
      this.HasNoPendingCommand = true;
      if (this.SendCommandCompletedEvent == null)
        return;
      this.SendCommandCompletedEvent((object) this, e);
    }

    private void UpdateProgressDisplay()
    {
      if (this.Duration <= 0UL)
      {
        this.DisplayedPosition = (string) null;
        this.DisplayedDuration = (string) null;
        this.ProgressDisplay = (string) null;
      }
      else
      {
        TimeSpan timeSpan1 = new TimeSpan((long) this.Position);
        TimeSpan timeSpan2 = new TimeSpan((long) this.Duration);
        this.DisplayedPosition = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.NowPlaying_TimeSpan_Format, new object[3]
        {
          (object) timeSpan1.Hours,
          (object) timeSpan1.Minutes,
          (object) timeSpan1.Seconds
        });
        this.DisplayedDuration = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.NowPlaying_TimeSpan_Format, new object[3]
        {
          (object) timeSpan2.Hours,
          (object) timeSpan2.Minutes,
          (object) timeSpan2.Seconds
        });
        this.ProgressDisplay = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.NowPlayingMediaProgress, new object[2]
        {
          (object) this.DisplayedPosition,
          (object) this.DisplayedDuration
        });
      }
    }

    private void ProcessNewItemType()
    {
      if (!this.ProcessUpdates)
        return;
      List<string> list = new List<string>();
      switch (this.ItemType)
      {
        case NowPlayingType.Game:
          list.Add(Resource.NowPlaying_MoreInfo_Overview);
          list.Add(Resource.NowPlaying_MoreInfo_Friends);
          list.Add(Resource.NowPlaying_MoreInfo_Achievements);
          list.Add(Resource.NowPlaying_MoreInfo_Images);
          list.Add(Resource.NowPlaying_MoreInfo_Related);
          break;
        case NowPlayingType.Music:
        case NowPlayingType.MusicVideo:
          list.Add(Resource.NowPlaying_MoreInfo_Overview);
          list.Add(Resource.NowPlaying_MoreInfo_Review);
          list.Add(Resource.NowPlaying_MoreInfo_Artist);
          list.Add(Resource.NowPlaying_MoreInfo_Images);
          list.Add(Resource.NowPlaying_MoreInfo_Related);
          break;
        case NowPlayingType.Movie:
        case NowPlayingType.TvEpisode:
          list.Add(Resource.NowPlaying_MoreInfo_Overview);
          list.Add(Resource.NowPlaying_MoreInfo_CastAndCrew);
          list.Add(Resource.NowPlaying_MoreInfo_Related);
          break;
      }
      if (list != null && list.Count > 0)
      {
        this.MoreInfoDetails = new ObservableCollection<string>(list);
        this.ShowMoreInfo = true;
      }
      else
      {
        this.MoreInfoDetails = new ObservableCollection<string>();
        this.ShowMoreInfo = false;
      }
    }

    private void GetMediaInfoUtil_EventGetNowPlayingItemCompleted(
      object sender,
      ServiceProxyEventArgs<NowPlayingItemViewModel> e)
    {
      if (sender is GetMediaInfoUtil getMediaInfoUtil)
        getMediaInfoUtil.EventGetNowPlayingItemCompleted -= new EventHandler<ServiceProxyEventArgs<NowPlayingItemViewModel>>(this.GetMediaInfoUtil_EventGetNowPlayingItemCompleted);
      if (e.Error == null && e.Result != null)
      {
        NowPlayingItemViewModel result = e.Result;
        if (result == null || !string.IsNullOrEmpty(this.PartnerMediaId) && !string.IsNullOrEmpty(result.PartnerMediaId) && string.CompareOrdinal(this.PartnerMediaId, result.PartnerMediaId) != 0)
          return;
        this.LastRefreshTime = DateTime.UtcNow;
        this.nowPlayingMediaType = result.ItemType;
        this.nowPlayingMediaTitle = result.Title;
        this.nowPlayingMediaSubTitle = result.SubTitle;
        this.nowPlayingMediaImageUrl = result.ImageUrl;
        this.nowPlayingMediaBackgroundImageUrl = result.BackgroundImageUrl;
        this.nowPlayingMediaDescription = result.Description;
        this.DetailsUrl = result.DetailsUrl;
        if (string.IsNullOrEmpty(this.nowPlayingMediaImageUrl))
        {
          switch (this.nowPlayingMediaType)
          {
            case NowPlayingType.Music:
              this.nowPlayingMediaImageUrl = "/UI/Images/DefaultBoxArt/music.png";
              break;
            case NowPlayingType.Movie:
              this.nowPlayingMediaImageUrl = "/UI/Images/DefaultBoxArt/movie.png";
              break;
            case NowPlayingType.MusicVideo:
              this.nowPlayingMediaImageUrl = "/UI/Images/DefaultBoxArt/musicVideo.png";
              break;
            case NowPlayingType.TvEpisode:
              this.nowPlayingMediaImageUrl = "/UI/Images/DefaultBoxArt/tv.png";
              break;
            default:
              this.nowPlayingMediaImageUrl = "/UI/Images/DefaultBoxArt/unknown.png";
              break;
          }
        }
        this.MediaId = !string.IsNullOrEmpty(result.MediaId) ? result.MediaId : this.MediaId;
        this.ParentMediaId = !string.IsNullOrEmpty(result.ParentMediaId) ? result.ParentMediaId : this.ParentMediaId;
        this.UpdateView();
      }
      else
      {
        Exception error = e.Error;
      }
    }

    private void SearchServiceManager_EventGetNowPlayingAppDetailsCompleted(
      object sender,
      ServiceProxyEventArgs<XboxGameData> e)
    {
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetNowPlayingAppDetailsCompleted -= new EventHandler<ServiceProxyEventArgs<XboxGameData>>(this.SearchServiceManager_EventGetNowPlayingAppDetailsCompleted);
      if (e.Error == null && e.Result != null)
      {
        if ((int) this.TitleId != (int) e.Result.TitleId)
          return;
        this.AppId = e.Result.Id;
        this.nowPlayingAppType = this.TitleId != 4294838225U ? (!(e.Result.ProductType == "61") ? NowPlayingType.Game : NowPlayingType.Application) : NowPlayingType.Dash;
        this.nowPlayingAppTitle = e.Result.Name;
        this.nowPlayingAppImageUrl = this.TitleId != 1481115739U ? e.Result.ImageUrl : "/UI/Images/Tile_Zune.jpg";
        if (string.IsNullOrEmpty(this.nowPlayingAppImageUrl))
          this.nowPlayingAppImageUrl = this.nowPlayingAppType == NowPlayingType.Game ? "/UI/Images/DefaultBoxArt/xboxgame.png" : "/UI/Images/DefaultBoxArt/xboxapp.png";
        this.nowPlayingAppBackgroundImageUrl = e.Result.BackgroundImageUrl;
        this.nowPlayingAppDescription = e.Result.Description;
        this.UpdateView();
      }
      else
      {
        Exception error = e.Error;
      }
    }

    private void UpdateView()
    {
      if (string.IsNullOrEmpty(this.nowPlayingMediaTitle) && string.IsNullOrEmpty(this.nowPlayingMediaImageUrl))
      {
        this.ItemType = this.nowPlayingAppType;
        this.ImageUrl = this.nowPlayingAppImageUrl;
        this.BackgroundImageUrl = this.nowPlayingAppBackgroundImageUrl;
        this.Title = this.nowPlayingAppTitle;
        this.SubTitle = (string) null;
        this.Description = this.nowPlayingAppDescription;
        this.IsAppAndNoMediaPlaying = this.ItemType != NowPlayingType.Game;
      }
      else
      {
        this.ItemType = this.nowPlayingMediaType;
        this.ImageUrl = this.nowPlayingMediaImageUrl;
        this.BackgroundImageUrl = this.nowPlayingMediaBackgroundImageUrl;
        this.Title = this.nowPlayingMediaTitle;
        this.SubTitle = this.nowPlayingMediaSubTitle;
        this.Description = this.nowPlayingMediaDescription;
        this.IsAppAndNoMediaPlaying = false;
      }
      this.IsDashboardOrUnknown = this.ItemType == NowPlayingType.Dash || this.ItemType == NowPlayingType.Unknown;
      this.IsNotDashboardOrUnknown = !this.IsDashboardOrUnknown;
    }
  }
}
