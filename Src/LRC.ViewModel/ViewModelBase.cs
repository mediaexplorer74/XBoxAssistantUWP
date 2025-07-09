// *********************************************************
// Type: LRC.ViewModel.ViewModelBase
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
using LRC.Session;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  [XmlInclude(typeof (NowPlayingItemViewModel))]
  [KnownType(typeof (MusicArtistViewModel))]
  [KnownType(typeof (MusicTrackItem))]
  [KnownType(typeof (NowPlayingDetailsViewModel))]
  [KnownType(typeof (NowPlayingItemViewModel))]
  [KnownType(typeof (ProviderViewModel))]
  [KnownType(typeof (RecentItemViewModel))]
  [KnownType(typeof (RecentsViewModel))]
  [KnownType(typeof (RecommendedPicksViewModel))]
  [KnownType(typeof (SearchDetailsViewModel))]
  [KnownType(typeof (SearchFilterViewModel))]
  [KnownType(typeof (SearchItemViewModel))]
  [KnownType(typeof (SearchViewModel))]
  [KnownType(typeof (TVMediaItem))]
  [KnownType(typeof (TvSeriesDetailsViewModel))]
  [KnownType(typeof (VideoDetailViewModel))]
  [KnownType(typeof (VideoItemList))]
  [KnownType(typeof (VideoFilteredViewModel))]
  [KnownType(typeof (ZuneCategoriesViewModel))]
  [KnownType(typeof (ZuneCategory))]
  [KnownType(typeof (ZuneHubViewModel))]
  [KnownType(typeof (ZuneMarketplaceViewModel))]
  [KnownType(typeof (MovieMediaItem))]
  [XmlInclude(typeof (AchievementViewModel))]
  [XmlInclude(typeof (BeaconSetEditViewModel))]
  [XmlInclude(typeof (FeaturedItemViewModel))]
  [XmlInclude(typeof (FeaturesViewModel))]
  [XmlInclude(typeof (FriendsPlayingViewModel))]
  [XmlInclude(typeof (GameItem))]
  [XmlInclude(typeof (LeaderboardEntryViewModel))]
  [XmlInclude(typeof (LeaderboardListViewModel))]
  [XmlInclude(typeof (LeaderboardViewModel))]
  [XmlInclude(typeof (LookForXboxViewModel))]
  [XmlInclude(typeof (MainViewModel))]
  [XmlInclude(typeof (MediaItem))]
  [XmlInclude(typeof (MediaItemList))]
  [XmlInclude(typeof (MovieMediaItem))]
  [XmlInclude(typeof (MusicArtistViewModel))]
  [XmlInclude(typeof (MusicTrackItem))]
  [XmlInclude(typeof (NowPlayingDetailsViewModel))]
  [XmlRoot(Namespace = "")]
  [XmlInclude(typeof (ProviderViewModel))]
  [XmlInclude(typeof (RecentItemViewModel))]
  [XmlInclude(typeof (RecentsViewModel))]
  [XmlInclude(typeof (RecommendedPicksViewModel))]
  [XmlInclude(typeof (SearchDetailsViewModel))]
  [XmlInclude(typeof (SearchFilterViewModel))]
  [XmlInclude(typeof (SearchItemViewModel))]
  [XmlInclude(typeof (SearchViewModel))]
  [XmlInclude(typeof (TVMediaItem))]
  [XmlInclude(typeof (TvSeriesDetailsViewModel))]
  [XmlInclude(typeof (VideoDetailViewModel))]
  [XmlInclude(typeof (VideoItemList))]
  [XmlInclude(typeof (VideoFilteredViewModel))]
  [XmlInclude(typeof (ZuneCategoriesViewModel))]
  [XmlInclude(typeof (ZuneCategory))]
  [XmlInclude(typeof (ZuneHubViewModel))]
  [XmlInclude(typeof (ZuneMarketplaceViewModel))]
  [XmlInclude(typeof (ZuneVideoFeatureViewModel))]
  [KnownType(typeof (ZuneVideoFeatureViewModel))]
  [KnownType(typeof (AchievementViewModel))]
  [KnownType(typeof (BeaconSetEditViewModel))]
  [KnownType(typeof (FeaturedItemViewModel))]
  [DataContract(Namespace = "")]
  [KnownType(typeof (FeaturesViewModel))]
  [KnownType(typeof (FriendsPlayingViewModel))]
  [KnownType(typeof (GameItem))]
  [KnownType(typeof (LeaderboardEntryViewModel))]
  [KnownType(typeof (LeaderboardListViewModel))]
  [KnownType(typeof (LeaderboardViewModel))]
  [KnownType(typeof (LookForXboxViewModel))]
  [KnownType(typeof (MainViewModel))]
  [KnownType(typeof (MediaItem))]
  [KnownType(typeof (MediaItemList))]
  public abstract class ViewModelBase : NotifyPropertyBase
  {
    public const int InvalidState = -1;
    public const int ValidContentState = 0;
    public const int ErrorState = 1;
    public const int NoContentState = 2;
    public const int LoadingState = 3;
    public const int ValidForever = 2147483647;
    public const int ValidForFiveMinutes = 5;
    public const int ValidForAnHour = 60;
    public const int ValidForADay = 1440;
    private int currentState = -1;
    private string title;
    private string subTitle;
    private string description;
    private string imageUrl;
    private int? lifetimeInMinutes;
    private bool isBusy;
    private bool isBlocking;
    private DateTime lastRefreshTime;
    private string backgroundImageUrl;
    private bool forceLaunch;
    private Action lastAction;

    public virtual event EventHandler<LRCAsyncCompletedEventArgs> EventOnAsyncOperationError
    {
      add
      {
        this.InternalEventOnAsyncOperationError -= value;
        this.InternalEventOnAsyncOperationError += value;
      }
      remove => this.InternalEventOnAsyncOperationError -= value;
    }

    public event EventHandler<LRCAsyncCompletedEventArgs> EventLaunchTitleCompleted
    {
      add
      {
        this.InternalEventLaunchTitleCompleted -= value;
        this.InternalEventLaunchTitleCompleted += value;
      }
      remove => this.InternalEventLaunchTitleCompleted -= value;
    }

    protected event EventHandler<LRCAsyncCompletedEventArgs> InternalEventOnAsyncOperationError;

    private event EventHandler<LRCAsyncCompletedEventArgs> InternalEventLaunchTitleCompleted;

    public int CurrentState
    {
      get => this.currentState;
      set => this.SetPropertyValue<int>(ref this.currentState, value, nameof (CurrentState));
    }

    [DataMember]
    public string Title
    {
      get => this.title;
      set => this.SetPropertyValue<string>(ref this.title, value, nameof (Title));
    }

    [DataMember]
    public string SubTitle
    {
      get => this.subTitle;
      set => this.SetPropertyValue<string>(ref this.subTitle, value, nameof (SubTitle));
    }

    [DataMember]
    public string Description
    {
      get => this.description;
      set => this.SetPropertyValue<string>(ref this.description, value, nameof (Description));
    }

    [XmlIgnore]
    [IgnoreDataMember]
    public bool IsBlocking
    {
      get => this.isBlocking;
      set => this.SetPropertyValue<bool>(ref this.isBlocking, value, nameof (IsBlocking));
    }

    [DataMember]
    public string ImageUrl
    {
      get => this.imageUrl;
      set => this.SetPropertyValue<string>(ref this.imageUrl, value, nameof (ImageUrl));
    }

    [IgnoreDataMember]
    [XmlIgnore]
    public string BackgroundImageUrl
    {
      get => this.backgroundImageUrl;
      set
      {
        this.SetPropertyValue<string>(ref this.backgroundImageUrl, value, nameof (BackgroundImageUrl));
      }
    }

    [IgnoreDataMember]
    [XmlIgnore]
    public bool IsBusy
    {
      get => this.isBusy;
      set => this.SetPropertyValue<bool>(ref this.isBusy, value, nameof (IsBusy));
    }

    [IgnoreDataMember]
    [XmlIgnore]
    public DateTime LastRefreshTime
    {
      get => this.lastRefreshTime;
      set => this.lastRefreshTime = value;
    }

    [DataMember]
    public int LifetimeInMinutes
    {
      get
      {
        if (!this.lifetimeInMinutes.HasValue)
          this.lifetimeInMinutes = new int?(60);
        return this.lifetimeInMinutes.Value;
      }
      set => this.lifetimeInMinutes = new int?(value);
    }

    public bool ShouldLoadData
    {
      get
      {
        return XboxLiveGamer.CurrentGamer != null && XboxLiveGamer.GamerState.GamerState == XboxLiveGamerState.GamerStates.SignedIn && !this.isBusy && (this.LastRefreshTime.Equals(DateTime.MinValue) || TimeSpan.Compare(DateTime.UtcNow - this.LastRefreshTime, TimeSpan.FromMinutes((double) this.LifetimeInMinutes)) > 0);
      }
    }

    public virtual void Load()
    {
    }

    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "state", Justification = "this is required by the callback prototype")]
    public void RetryLastLaunchCommand(object state)
    {
      if (this.lastAction == null)
        return;
      this.forceLaunch = true;
      this.lastAction();
      this.forceLaunch = false;
      this.lastAction = (Action) null;
    }

    public void LaunchApp(uint titleId, string deepLink, object userState)
    {
      this.Launch(titleId, LaunchType.App, deepLink, userState);
    }

    public void LaunchGame(uint titleId, string gameGuid, object userState)
    {
      this.Launch(titleId, LaunchType.Game, gameGuid, userState);
    }

    public void LaunchContent(string deepLink, object userState)
    {
      this.Launch(0U, LaunchType.GameContent, deepLink, userState);
    }

    public void LaunchContent(
      string contentId,
      string mediaId,
      string productId,
      object userState)
    {
      this.LaunchContent(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "ContentMediaId={0};MediaId={1};ContentMediaTypeId={2}", new object[3]
      {
        (object) contentId,
        (object) mediaId,
        (object) productId
      }), userState);
    }

    public void LaunchTitle(uint titleId, string deepLink, object userState)
    {
      if (this.IsBlocking)
        return;
      if (this.CheckSafeToLaunch())
      {
        this.IsBlocking = true;
        MainViewModel.Instance.CurrentSession.BeginLaunchTitle(titleId, deepLink, new AsyncCallback(this.LaunchOnConsoleCompleted), userState);
      }
      else
        this.lastAction = (Action) (() => this.LaunchTitle(titleId, deepLink, userState));
    }

    public void LaunchZuneMedia(
      MediaType mediaType,
      string contentId,
      string contentId2,
      object userState)
    {
      string empty = string.Empty;
      string deepLink;
      switch (mediaType)
      {
        case MediaType.movie:
          deepLink = "Details?ContentType=Movie&ContentId=" + contentId;
          break;
        case MediaType.music_album:
          deepLink = "Details?ContentType=Album&ContentId=" + contentId;
          break;
        case MediaType.music_track:
          deepLink = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Details?ContentType=Song&ContentId={0}&ContentId2={1}", new object[2]
          {
            (object) contentId,
            (object) contentId2
          });
          break;
        case MediaType.tv_episode:
          deepLink = "Details?ContentType=TVEpisode&ContentId=" + contentId;
          break;
        case MediaType.music_musicvideo:
          deepLink = "Details?ContentType=MusicVideo&ContentId=" + contentId;
          break;
        default:
          throw new NotSupportedException();
      }
      if (string.IsNullOrEmpty(deepLink))
        return;
      this.Launch(1481115739U, LaunchType.App, deepLink, userState);
    }

    protected static LRCAsyncCompletedEventArgs CreateFailedToSendCommandEventArgs()
    {
      return ViewModelBase.LRCAsyncCompletedEventArgsInternal();
    }

    protected void NotifyError(ErrorCodeEnum errorCodeEnum)
    {
      this.NotifyAsyncOperationCompleted(this.InternalEventOnAsyncOperationError, errorCodeEnum);
    }

    protected void NotifyError(LRCAsyncCompletedEventArgs eventArgs)
    {
      this.NotifyAsyncOperationCompleted(this.InternalEventOnAsyncOperationError, eventArgs);
    }

    protected void ShowNonfatalErrorMessage(string errorMessage)
    {
      if (string.IsNullOrEmpty(errorMessage))
        return;
      EventHandler<LRCAsyncCompletedEventArgs> tempEvent = this.InternalEventOnAsyncOperationError;
      if (tempEvent == null)
        return;
      LRCAsyncCompletedEventArgs eventArgs = new LRCAsyncCompletedEventArgs()
      {
        StatusCode = ErrorCodeEnum.Error,
        MessageTitle = Resource.Error_Title,
        MessageBody = errorMessage
      };
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        tempEvent((object) this, eventArgs);
      }, (object) null);
    }

    protected virtual LRCAsyncCompletedEventArgs ParseErrorCode(ErrorCodeEnum errorCodeEnum)
    {
      LRCAsyncCompletedEventArgs errorCode = new LRCAsyncCompletedEventArgs()
      {
        StatusCode = errorCodeEnum
      };
      if (errorCode.IsError)
      {
        switch (errorCode.Severity)
        {
          case ErrorSeverity.Fatal:
            errorCode.MessageTitle = Resource.Fatal_Title;
            break;
          case ErrorSeverity.Error:
          case ErrorSeverity.Info:
            errorCode.MessageTitle = Resource.Error_Title;
            break;
        }
        string name = "LRC_Error_Code_" + errorCodeEnum.ToString();
        errorCode.MessageBody = Resource.ResourceManager.GetString(name, Resource.Culture);
      }
      return errorCode;
    }

    protected void NotifyAsyncOperationCompleted(
      EventHandler<LRCAsyncCompletedEventArgs> registeredEvent,
      ErrorCodeEnum errorCodeEnum)
    {
      this.NotifyAsyncOperationCompleted(registeredEvent, this.ParseErrorCode(errorCodeEnum));
    }

    protected void NotifyAsyncOperationCompleted(
      EventHandler<LRCAsyncCompletedEventArgs> registeredEvent,
      LRCAsyncCompletedEventArgs eventArgs)
    {
      if (registeredEvent == null)
        return;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        registeredEvent((object) this, eventArgs);
      }, (object) null);
    }

    private static LRCAsyncCompletedEventArgs LRCAsyncCompletedEventArgsInternal()
    {
      return new LRCAsyncCompletedEventArgs()
      {
        StatusCode = ErrorCodeEnum.FailedToSendCommand,
        MessageTitle = Resource.Error_Title,
        MessageBody = Resource.MessageBody_LaunchOnConsoleFailed
      };
    }

    private void Launch(uint titleId, LaunchType type, string deepLink, object userState)
    {
      if (this.IsBlocking)
        return;
      if (this.CheckSafeToLaunch())
      {
        this.IsBlocking = true;
        MainViewModel.Instance.CurrentSession.BeginLaunchTitle(0U, LaunchUtil.CreateLaunchParameter(titleId, type, deepLink), new AsyncCallback(this.LaunchOnConsoleCompleted), userState);
      }
      else
        this.lastAction = (Action) (() => this.Launch(titleId, type, deepLink, userState));
    }

    private void LaunchOnConsoleCompleted(IAsyncResult result)
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        SessionAsyncResult sessionAsyncResult = result as SessionAsyncResult;
        LRCAsyncCompletedEventArgs e = new LRCAsyncCompletedEventArgs(sessionAsyncResult.AsyncState)
        {
          StatusCode = ErrorCodeEnum.None
        };
        if (sessionAsyncResult != null)
        {
          if (sessionAsyncResult.Error != null && sessionAsyncResult.Error is XMobileException && (sessionAsyncResult.Error as XMobileException).ErrorCode == 2021)
            e = ViewModelBase.CreateFailedToSendCommandEventArgs();
        }
        else
          e = ViewModelBase.CreateFailedToSendCommandEventArgs();
        EventHandler<LRCAsyncCompletedEventArgs> launchTitleCompleted = this.InternalEventLaunchTitleCompleted;
        if (launchTitleCompleted != null)
          launchTitleCompleted((object) this, e);
        this.IsBlocking = false;
        this.forceLaunch = false;
      }, (object) this);
    }

    private bool CheckSafeToLaunch()
    {
      ThreadManager.UIThread.AssertIsCurrentThread();
      if (this.forceLaunch || MainViewModel.Instance.CurrentRunningTitleId == 4294838225U)
        return true;
      LRCAsyncCompletedEventArgs e = new LRCAsyncCompletedEventArgs()
      {
        StatusCode = ErrorCodeEnum.WillTerminateCurrentTitle
      };
      EventHandler<LRCAsyncCompletedEventArgs> launchTitleCompleted = this.InternalEventLaunchTitleCompleted;
      if (launchTitleCompleted != null)
        launchTitleCompleted((object) this, e);
      return false;
    }
  }
}
