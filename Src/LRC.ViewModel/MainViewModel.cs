// *********************************************************
// Type: LRC.ViewModel.MainViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Session;
using Microsoft.Xmedia.Client.WindowsPhone;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils;
using Xbox.Live.Phone.Utils.Cache;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class MainViewModel : ViewModelBase, IDisposable
  {
    private const string ComponentName = "MainViewModel";
    private const string PersistentDataPath = "PersistentData.xml";
    private const string RecentsDataPath = "RecentsData.xml";
    private const int PositionTimerInterval = 1000;
    private const int PollingInterval = 30000;
    private const double ZuneMaxRewindPlayRate = -32.0;
    private const double ZuneMaxFastForwardPlayRate = 32.0;
    private const double GreenLightMaxRewindPlayRate = -128.0;
    private const double GreenLightMaxFastForwardPlayRate = 128.0;
    private static MainViewModel instance;
    private bool disposed;
    private IXmediaSession lrcSession;
    private NowPlayingItemViewModel nowPlayingItem;
    private DirectLaunchViewModel directLaunchViewModel;
    private SearchTermsViewModel searchTermsViewModel;
    private string welcomeHeader;
    private PersistentData persistentData;
    private uint currentRunningTitleId;
    private MediaState currentMediaState;
    private ulong mediaStateVersionNumber;
    private bool isMediaStateAvailable;
    private bool isMediaBarExpanded;
    private Timer positionUpdateTimer;
    private Timer pollTimer;
    private bool isUsingCloud;
    private string searchText;
    private bool isSessionConnected;
    private int errorCode;
    private string errorHeaderString;
    private string errorString;

    public MainViewModel()
    {
      this.RecentsData = MainViewModel.LoadRecentsData();
      this.NowPlayingItem = new NowPlayingItemViewModel();
      this.DirectLaunchViewModel = new DirectLaunchViewModel();
      this.SearchTerms = new SearchTermsViewModel();
      this.Features = new FeaturesViewModel();
      this.ZuneMarketplace = new ZuneMarketplaceViewModel();
      this.searchText = (string) null;
      this.mediaStateVersionNumber = 0UL;
      this.isUsingCloud = false;
      this.isSessionConnected = true;
      this.ShouldPollState = true;
      this.pollTimer = new Timer(new TimerCallback(this.RefreshState), (object) null, -1, -1);
    }

    ~MainViewModel() => this.Dispose(false);

    public event EventHandler<LRCAsyncCompletedEventArgs> EventMediaStateChanged;

    public event EventHandler<ConnectionEventArgs> EventConnectionFailed;

    public static MainViewModel Instance
    {
      get
      {
        if (MainViewModel.instance == null)
          MainViewModel.instance = new MainViewModel();
        return MainViewModel.instance;
      }
      set => MainViewModel.instance = value;
    }

    public LRCSession LRCSession => this.lrcSession as LRCSession;

    public bool ShouldPollState { get; set; }

    public bool ShouldLookForXbox { get; set; }

    public int ErrorCode
    {
      get => this.errorCode;
      set => this.SetPropertyValue<int>(ref this.errorCode, value, nameof (ErrorCode));
    }

    public string ErrorHeaderString
    {
      get => this.errorHeaderString;
      set
      {
        this.SetPropertyValue<string>(ref this.errorHeaderString, value, nameof (ErrorHeaderString));
      }
    }

    public string ErrorString
    {
      get => this.errorString;
      set => this.SetPropertyValue<string>(ref this.errorString, value, nameof (ErrorString));
    }

    [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed in dispose method")]
    public IXmediaSession CurrentSession
    {
      get
      {
        if (this.lrcSession == null)
          this.lrcSession = (IXmediaSession) new LRCSession();
        return this.lrcSession;
      }
    }

    public bool IsSessionConnected
    {
      get => this.isSessionConnected;
      set
      {
        this.SetPropertyValue<bool>(ref this.isSessionConnected, value, nameof (IsSessionConnected));
      }
    }

    [XmlIgnore]
    public uint CurrentRunningTitleId
    {
      get => this.currentRunningTitleId;
      set
      {
        if ((int) value == (int) this.currentRunningTitleId)
          return;
        this.currentRunningTitleId = value;
        this.ProcessTitleChange(this.currentRunningTitleId);
      }
    }

    [XmlIgnore]
    public uint ConsoleLiveTVProviderTitleId
    {
      get
      {
        return this.CurrentSession is LRCSession currentSession ? currentSession.ConsoleLiveTVTitleId : 0U;
      }
    }

    public bool IsRunningMediaCenter
    {
      get
      {
        return this.currentRunningTitleId == 1480918993U || this.currentRunningTitleId == 1480918994U || this.currentRunningTitleId == 1480918995U;
      }
    }

    [XmlIgnore]
    public MediaState CurrentMediaState
    {
      get => this.currentMediaState;
      set
      {
        MediaState currentMediaState = this.currentMediaState;
        ++this.mediaStateVersionNumber;
        this.currentMediaState = value;
        this.ProcessMediaStateChange(value);
        if (value != null)
        {
          if (currentMediaState != null && !(currentMediaState.ContentId != value.ContentId))
            return;
          this.ProcessMediaContentChange(value);
        }
        else
        {
          if (currentMediaState == null)
            return;
          this.ProcessTitleChange(this.CurrentRunningTitleId);
        }
      }
    }

    [XmlIgnore]
    public bool IsMediaStateAvailable
    {
      get => this.isMediaStateAvailable;
      set
      {
        this.SetPropertyValue<bool>(ref this.isMediaStateAvailable, value, nameof (IsMediaStateAvailable));
      }
    }

    [XmlIgnore]
    public bool HideIntroductoryScreens
    {
      get => this.PersistentData.HideIntroductoryScreen;
      set
      {
        this.PersistentData.HideIntroductoryScreen = value;
        CacheManager.Save("PersistentData.xml", (object) this.PersistentData);
      }
    }

    public string WelcomeHeader
    {
      get => this.welcomeHeader;
      set => this.SetPropertyValue<string>(ref this.welcomeHeader, value, nameof (WelcomeHeader));
    }

    [DataMember]
    public RecentsViewModel RecentsData { get; set; }

    [DataMember]
    public ViewModelBase SelectedMediaDetails { get; set; }

    [DataMember]
    public AchievementViewModel SelectedAchievement { get; set; }

    [DataMember]
    public SearchDetailsViewModel SelectedSearchItem { get; set; }

    [IgnoreDataMember]
    [XmlIgnore]
    public NowPlayingDetailsViewModel SelectedNowPlayingItem { get; set; }

    [DataMember]
    public ViewModelBase CurrentSearch { get; set; }

    [XmlIgnore]
    [IgnoreDataMember]
    public NowPlayingItemViewModel NowPlayingItem
    {
      get => this.nowPlayingItem;
      set
      {
        this.SetPropertyValue<NowPlayingItemViewModel>(ref this.nowPlayingItem, value, nameof (NowPlayingItem));
      }
    }

    public DirectLaunchViewModel DirectLaunchViewModel
    {
      get => this.directLaunchViewModel;
      set
      {
        this.SetPropertyValue<DirectLaunchViewModel>(ref this.directLaunchViewModel, value, nameof (DirectLaunchViewModel));
      }
    }

    [DataMember]
    public SearchTermsViewModel SearchTerms
    {
      get => this.searchTermsViewModel;
      set
      {
        this.SetPropertyValue<SearchTermsViewModel>(ref this.searchTermsViewModel, value, nameof (SearchTerms));
      }
    }

    [DataMember]
    public bool IsMediaBarExpanded
    {
      get => this.isMediaBarExpanded;
      set
      {
        this.SetPropertyValue<bool>(ref this.isMediaBarExpanded, value, nameof (IsMediaBarExpanded));
      }
    }

    [DataMember]
    public FeaturesViewModel Features { get; set; }

    [DataMember]
    public ZuneMarketplaceViewModel ZuneMarketplace { get; set; }

    [DataMember]
    public ObservableCollection<string> SelectedImageCollection { get; set; }

    [DataMember]
    public int SelectedImageIndex { get; set; }

    [XmlIgnore]
    public bool ShouldResetHomePageIndex { get; set; }

    public bool DpadExpandedAutomatically { get; set; }

    public bool IsUsingCloud
    {
      get => this.isUsingCloud;
      set => this.SetPropertyValue<bool>(ref this.isUsingCloud, value, nameof (IsUsingCloud));
    }

    public string SearchText
    {
      get => this.searchText;
      set => this.SetPropertyValue<string>(ref this.searchText, value, nameof (SearchText));
    }

    [DataMember]
    public string OmnitureEntryPoint { get; set; }

    private PersistentData PersistentData
    {
      get
      {
        if (this.persistentData == null)
        {
          this.persistentData = CacheManager.Load<PersistentData>("PersistentData.xml");
          if (this.persistentData == null)
            this.persistentData = new PersistentData();
        }
        return this.persistentData;
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public override void Load()
    {
      this.Features.Load();
      this.RecentsData.Load();
      this.NowPlayingItem.Load();
      this.SearchTerms.Load();
      this.ZuneMarketplace.Load();
      this.LastRefreshTime = DateTime.UtcNow;
    }

    public void FlipMediaBarStatus()
    {
      this.IsMediaBarExpanded = !this.IsMediaBarExpanded;
      this.DpadExpandedAutomatically = false;
    }

    public void ExpandMediaBar()
    {
      if (this.IsMediaBarExpanded)
        return;
      this.IsMediaBarExpanded = true;
      this.DpadExpandedAutomatically = true;
    }

    public void CollapseMediaBar()
    {
      this.DpadExpandedAutomatically = false;
      this.IsMediaBarExpanded = false;
    }

    public void CollapseMediaBarIfNecessary()
    {
      if (!this.DpadExpandedAutomatically || !this.IsMediaBarExpanded)
        return;
      this.DpadExpandedAutomatically = false;
      this.IsMediaBarExpanded = false;
    }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "This is a debug string.")]
    public ErrorCodeEnum InitializeSession()
    {
      ErrorCodeEnum errorCodeEnum = ErrorCodeEnum.None;
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      this.StopRefreshTimer();
      try
      {
        this.CurrentSession.TitleChanged += new EventHandler<TitleChangedEventArgs>(this.CurrentSession_OnTitleChanged);
        this.CurrentSession.MediaStateUpdated += new EventHandler<MediaStateUpdatedEventArgs>(this.CurrentSession_OnMediaStateUpdated);
        if (this.CurrentSession is LRCSession)
          ((LRCSession) this.CurrentSession).ConnectionFailed += new EventHandler<ConnectionEventArgs>(this.CurrentSession_ConnectionFailed);
        this.CurrentSession.EndJoinSession(this.CurrentSession.BeginJoinSession((AsyncCallback) null, (object) null));
        this.IsUsingCloud = (this.CurrentSession.CommunicationCapabilities & CommunicationCapabilities.CanConnectViaTcp) == CommunicationCapabilities.CanNotConnect;
        this.IsSessionConnected = true;
        if (this.CurrentSession is LRCSession currentSession)
        {
          IAsyncResult consoleSettings = currentSession.BeginGetConsoleSettings((AsyncCallback) null, (object) null);
          currentSession.EndGetConsoleSettings(consoleSettings);
        }
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          this.RefreshState((object) null);
        });
      }
      catch (XMobileException ex)
      {
        errorCodeEnum = (ErrorCodeEnum) ex.ErrorCode;
        this.IsSessionConnected = false;
      }
      return errorCodeEnum;
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "We need to catch all exceptions running on background threads to prevent the app from crashing.")]
    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "This is a debug string.")]
    public void SendMediaControlCommand(ControlKey controlKey)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      try
      {
        this.CurrentSession.EndSendControlCommand(this.CurrentSession.BeginSendControlCommand(controlKey, (AsyncCallback) null, (object) null));
        ThreadManager.UIThreadPost(new SendOrPostCallback(this.UpdateLocalMediaState), (object) controlKey);
      }
      catch (Exception ex)
      {
      }
    }

    public void OnAppActivated()
    {
      if (this.LRCSession == null || !this.IsSessionConnected)
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        this.RefreshState((object) null);
      }, (object) null);
    }

    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Will be used in the future")]
    public void OnAppDeactivated()
    {
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Worker thread")]
    public void RefreshState(object stateInfo)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      try
      {
        if (this.LRCSession == null || !this.IsSessionConnected)
          return;
        this.CurrentSession.EndGetMediaState(this.CurrentSession.BeginGetMediaState((AsyncCallback) null, (object) null));
        this.ResetRefreshTimer();
      }
      catch (Exception ex)
      {
      }
    }

    public void SaveRecentsData()
    {
      CacheManager.Save("RecentsData.xml", (object) this.RecentsData);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        if (this.lrcSession != null)
        {
          if (this.lrcSession is LRCSession lrcSession)
            lrcSession.Dispose();
          this.lrcSession = (IXmediaSession) null;
        }
        if (this.positionUpdateTimer != null)
        {
          this.positionUpdateTimer.Dispose();
          this.positionUpdateTimer = (Timer) null;
        }
        if (this.pollTimer != null)
        {
          this.pollTimer.Dispose();
          this.pollTimer = (Timer) null;
        }
      }
      this.disposed = true;
    }

    private static RecentsViewModel LoadRecentsData()
    {
      return CacheManager.Load<RecentsViewModel>("RecentsData.xml") ?? new RecentsViewModel();
    }

    private void UpdateLocalMediaState(object state)
    {
      ThreadManager.UIThread.AssertIsCurrentThread();
      switch ((ControlKey) state)
      {
        case ControlKey.Pause:
          this.CurrentMediaState.Rate = 0.0f;
          this.CurrentMediaState.MediaTransportState = MediaTransportState.Paused;
          break;
        case ControlKey.Play:
          this.CurrentMediaState.Rate = 1f;
          this.CurrentMediaState.MediaTransportState = MediaTransportState.Playing;
          break;
        case ControlKey.FastForward:
          if ((double) this.CurrentMediaState.Rate != (this.NowPlayingItem.TitleId != 1481115739U ? 128.0 : 32.0))
          {
            if ((double) this.CurrentMediaState.Rate > 0.0)
            {
              this.CurrentMediaState.Rate *= 2f;
              break;
            }
            if ((double) this.CurrentMediaState.Rate < 0.0)
            {
              this.CurrentMediaState.Rate /= 2f;
              break;
            }
            this.CurrentMediaState.Rate = 2f;
            this.CurrentMediaState.MediaTransportState = MediaTransportState.Playing;
            break;
          }
          break;
        case ControlKey.Rewind:
          if ((double) this.CurrentMediaState.Rate != (this.NowPlayingItem.TitleId != 1481115739U ? (double) sbyte.MinValue : -32.0))
          {
            if ((double) this.CurrentMediaState.Rate < 0.0)
            {
              this.CurrentMediaState.Rate *= 2f;
              break;
            }
            if ((double) this.CurrentMediaState.Rate > 1.0)
            {
              this.CurrentMediaState.Rate /= 2f;
              break;
            }
            this.CurrentMediaState.Rate = -2f;
            this.CurrentMediaState.MediaTransportState = MediaTransportState.Playing;
            break;
          }
          break;
        case ControlKey.Skip:
        case ControlKey.Replay:
          if ((double) this.CurrentMediaState.Rate > 1.0 || (double) this.CurrentMediaState.Rate < 0.0)
          {
            this.CurrentMediaState.Rate = 1f;
            break;
          }
          break;
      }
      this.ProcessMediaStateChange(this.CurrentMediaState);
    }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Trace message")]
    private void CurrentSession_OnTitleChanged(object sender, TitleChangedEventArgs e)
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.CurrentRunningTitleId = e.CurrentRunningTitleId;
        this.CurrentMediaState = (MediaState) null;
        this.ResetRefreshTimer();
      }, (object) this);
    }

    private void CurrentSession_OnMediaStateUpdated(object sender, MediaStateUpdatedEventArgs e)
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.CurrentMediaState = e.CurrentMediaState;
        this.ResetRefreshTimer();
      }, (object) this);
    }

    private void CurrentSession_ConnectionFailed(object sender, ConnectionEventArgs e)
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        EventHandler<ConnectionEventArgs> connectionFailed = this.EventConnectionFailed;
        if (connectionFailed == null)
          return;
        connectionFailed((object) this, e);
      }, (object) this);
    }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "debug strings")]
    private void ProcessTitleChange(uint newTitleId)
    {
      this.NowPlayingItem = new NowPlayingItemViewModel()
      {
        TitleId = newTitleId
      };
    }

    private void ProcessMediaContentChange(MediaState newMediaState)
    {
      if (newMediaState == null)
        return;
      this.NowPlayingItem.PartnerMediaId = newMediaState.ContentId;
    }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "debug strings")]
    private void ProcessMediaStateChange(MediaState newMediaState)
    {
      if (newMediaState != null)
      {
        this.NowPlayingItem.UpdateProgress(newMediaState.Position, newMediaState.Duration);
        switch (newMediaState.MediaTransportState)
        {
          case MediaTransportState.Invalid:
          case MediaTransportState.Stopped:
            this.IsMediaStateAvailable = false;
            this.DisablePositionUpdateTimer();
            break;
          default:
            this.IsMediaStateAvailable = true;
            if (newMediaState.MediaTransportState == MediaTransportState.Playing || newMediaState.MediaTransportState == MediaTransportState.Starting)
            {
              this.EnablePositionUpdateTimer();
              this.CollapseMediaBarIfNecessary();
              break;
            }
            this.DisablePositionUpdateTimer();
            break;
        }
      }
      else
      {
        this.NowPlayingItem.UpdateProgress(0UL, 0UL);
        this.IsMediaStateAvailable = false;
        this.DisablePositionUpdateTimer();
      }
      this.NotifyAsyncOperationCompleted(this.EventMediaStateChanged, ErrorCodeEnum.None);
    }

    private void EnablePositionUpdateTimer()
    {
      ThreadManager.UIThread.AssertIsCurrentThread();
      if (this.positionUpdateTimer != null)
      {
        this.positionUpdateTimer.Dispose();
        this.positionUpdateTimer = (Timer) null;
      }
      this.positionUpdateTimer = new Timer(new TimerCallback(this.OnPositionTimerFired), (object) this.mediaStateVersionNumber, 1000, 1000);
    }

    private void DisablePositionUpdateTimer()
    {
      ThreadManager.UIThread.AssertIsCurrentThread();
      if (this.positionUpdateTimer == null)
        return;
      this.positionUpdateTimer.Dispose();
      this.positionUpdateTimer = (Timer) null;
    }

    private void OnPositionTimerFired(object state)
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        if ((long) (ulong) state != (long) this.mediaStateVersionNumber || this.currentMediaState == null || !this.isMediaStateAvailable)
          return;
        TimeSpan timeSpan = TimeSpan.FromTicks((long) this.CurrentMediaState.Position) + TimeSpan.FromMilliseconds((double) this.currentMediaState.Rate * 1000.0);
        if (timeSpan.CompareTo(TimeSpan.Zero) < 0)
          timeSpan = TimeSpan.Zero;
        if (timeSpan.Ticks > (long) this.currentMediaState.Duration)
          timeSpan = new TimeSpan((long) this.currentMediaState.Duration);
        this.currentMediaState.Position = (ulong) timeSpan.Ticks;
        this.NotifyPropertyChanged("CurrentMediaState");
        this.NowPlayingItem.UpdateProgress(this.currentMediaState.Position, this.currentMediaState.Duration);
        this.NotifyAsyncOperationCompleted(this.EventMediaStateChanged, ErrorCodeEnum.None);
      }, (object) this);
    }

    private void ResetRefreshTimer()
    {
      if (!this.ShouldPollState)
        return;
      this.pollTimer.Change(30000, -1);
    }

    private void StopRefreshTimer() => this.pollTimer.Change(-1, -1);
  }
}
