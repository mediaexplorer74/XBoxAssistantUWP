// *********************************************************
// Type: Xbox.Live.Phone.Services.XboxLiveGamer
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Avatar.Services.ManifestRead.Library;
using Avatar.Services.ManifestWrite.Library;
using Gds.Contracts;
using Leet.MessageService.DataContracts;
using Leet.UserGameData.DataContracts;
using Microsoft.Phone.Marketplace;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xbox.Live.Phone.Services.Beacons;
using Xbox.Live.Phone.Services.Facebook;
using Xbox.Live.Phone.Services.Leaderboards;
using Xbox.Live.Phone.Services.Resources;
using Xbox.Live.Phone.Services.TitleHistory;
using Xbox.Live.Phone.Services.UserClaims;
using Xbox.Live.Phone.Utils;
using Xbox.Live.Phone.Utils.Cache;
using Xbox.Live.Phone.Utils.Etc;
using Xbox.Live.Phone.Utils.Serialization;


namespace Xbox.Live.Phone.Services
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class XboxLiveGamer : CacheItem, INotifyPropertyChanged
  {
    private const int DayHours = 24;
    private const int HourMinutes = 60;
    private const int CacheExpirationTimeInSeconds = 120;
    private const int SignInWaitIntervalMilliSeconds = 1000;
    private const string ComponentName = "XboxLiveGamer";
    private const string LocalGamerGamerTag = "Player";
    private const string GamerPrefix = "XLG_";
    private const string MessagesPrefix = "MSG_";
    private const string GamesPrefix = "GAM_";
    private const string AchievementsPrefix = "ACH_";
    private const string FriendsPrefix = "FRI_";
    private const string AvatarPicUrlFormat = "http://avatar{0}.xboxlive.com/avatar/{1}/avatarpic-l.png";
    private const string PdlcGamerContextSavePath = "PdlcGamerContextEx.xml";
    private const int MaxGamers = 4;
    private static DateTime invalidLastSeenDateTime = new DateTime(2003, 1, 31);
    private static XboxLiveGamer currentGamer = (XboxLiveGamer) null;
    private static Dictionary<string, XboxLiveGamer> gamerInMemoryCache = new Dictionary<string, XboxLiveGamer>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private static Queue<string> gamerQueue = new Queue<string>(5);
    private static XboxLiveGamerState gamerState = new XboxLiveGamerState();
    private int profileState;
    private int friendsState;
    private int gamesState;
    private int messagesState;
    private IAvatarServiceManager avatarServiceManager;
    private IProfileServiceManager profileServiceManager;
    private IProfileServiceManager profileServiceManagerForFriend;
    private IFriendServiceManager friendServiceManager;
    private IGameDataServiceManager gameDataServiceManager;
    private IMessageServiceManager messageServiceManager;
    private IBeaconsServiceManager beaconsServiceManager;
    private ILeaderboardsServiceManager leaderboardsServiceManager;
    private IFacebookServiceManager facebookServiceManager;
    private ITitleHistoryServiceManager titleHistoryServiceManager;
    private bool avatarFiltered;
    private bool avatarFilteredDialogShown;
    private byte[] avatarManifest;
    private ProfileEx pendingProfileUpdates;
    private AtomicWrapper<AchievementsData> achievementsPerGame;
    private AtomicWrapper<GamesData> gamesData;
    private AtomicWrapper<MessagesData> messagesData;
    private AtomicWrapper<FriendsListData> friendsListData;
    private AtomicWrapper<bool> isLoadingProfile;
    private AtomicWrapper<bool> isLoadingFriends;
    private AtomicWrapper<bool> isLoadingAvatar;
    private AtomicWrapper<bool> isLoadingAvatarClosetAssets;
    private AtomicWrapper<bool> isLoadingGames;
    private AtomicWrapper<bool> isLoadingMessages;
    private AtomicWrapper<bool> isLoadingGameAchievements;
    private AtomicWrapper<bool> isLoadingXboxUserId;
    private Presence presenceInfo;
    private bool isModifyingFriendsListData;
    private GamerContextEx gamerContextEx;

    private XboxLiveGamer() => this.Initialize();

    private XboxLiveGamer(string gamerTag)
      : this()
    {
      this.GamerTag = gamerTag;
      this.AvatarManifest = (byte[]) null;
      this.AchievementsPerGame = new AchievementsData("ACH_" + this.GamerTag);
      if (string.IsNullOrEmpty(gamerTag))
      {
        this.IsCurrentGamer = true;
      }
      else
      {
        this.IsCurrentGamer = false;
        this.AllowCommunication = GamerPrivilegeSetting.Blocked;
        this.AllowProfileViewing = GamerPrivilegeSetting.Blocked;
      }
      if (this.IsCurrentGamer)
        this.StorageKey = "XLG_";
      else
        this.StorageKey = "XLG_" + this.GamerTag;
    }

    public static event EventHandler<ServiceProxyEventArgs<XboxLiveGamerState.GamerStates>> EventGamerStateUpdate;

    public static event EventHandler<EventArgs> EventGamerLoadingStateUpdated;

    public event EventHandler<ServiceProxyEventArgs<long>> EventGamerDataUpdated
    {
      add
      {
        this.InternalEventGamerDataUpdated -= value;
        this.InternalEventGamerDataUpdated += value;
      }
      remove => this.InternalEventGamerDataUpdated -= value;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public event EventHandler<ServiceProxyEventArgs<XboxLiveGamer>> EventUpdateAvatarCompleted;

    public event EventHandler<ServiceProxyEventArgs<XboxLiveGamer>> EventSetGamerpicCompleted;

    public event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventSendMessageCompleted;

    public event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventDeleteMessageCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventUpdateProfileCompleted;

    public event EventHandler<ServiceProxyEventArgs<MessageDetails>> EventGetOneMessageCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventAddFriendRequestCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventRemoveFriendCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventAcceptFriendRequestCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventDeclineFriendRequestCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventCancelFriendRequestCompleted;

    public event EventHandler<ServiceProxyEventArgs<string[]>> EventGamerTagChangeRequestCompleted;

    public event EventHandler<ServiceProxyEventArgs<LoadingObjectWrapper>> EventLoadedAvatarManifest;

    public event EventHandler<EventArgs> EventLoadedXboxUserId;

    public event EventHandler<ServiceProxyEventArgs<ActivityListForTitleData>> EventGetActivityListForTitleCompleted;

    public event EventHandler<ServiceProxyEventArgs<BeaconInfo>> EventGetBeaconForTitleCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventSetBeaconForTitleCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventDeleteBeaconForTitleCompleted;

    public event EventHandler<ServiceProxyEventArgs<BeaconDefaultTextData>> EventGetBeaconDefaultTextCompleted;

    public event EventHandler<ServiceProxyEventArgs<LeaderboardsForTitleData>> EventGetLeaderboardsForTitleCompleted;

    public event EventHandler<ServiceProxyEventArgs<LeaderboardInfo>> EventGetLeaderboardDetailsCompleted;

    public event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventPostLinkCompleted;

    public event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventPostCredentialsCompleted;

    public event EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> EventPostPermissionsCompleted;

    public event EventHandler<ServiceProxyEventArgs<List<TitleHistoryInfo>>> EventGetTitleHistoryCompleted;

    private event EventHandler<ServiceProxyEventArgs<long>> InternalEventGamerDataUpdated;

    public static ManualResetEvent ApplicationLaunchEvent { get; set; }

    public static string CurrentGamerTag
    {
      get => XboxLiveGamer.currentGamer.GamerTag;
      set
      {
        XboxLiveGamer.currentGamer.GamerTag = value;
        CacheManager.Instance.SaveAsync((CacheItem) XboxLiveGamer.CurrentGamer);
        XboxLiveGamer.CurrentGamer.OnPropertyChanged("GamerTag");
      }
    }

    public static XboxLiveGamer CurrentGamer => XboxLiveGamer.currentGamer;

    public static XboxLiveGamerState GamerState => XboxLiveGamer.gamerState;

    public static bool IsOnline => XboxLiveGamer.gamerState.IsOnline;

    public static bool IsLocalGamer
    {
      get
      {
        int num = ServiceManagerFactory.IsRunningEmulator ? 1 : 0;
        return Gamer.SignedInGamers[PlayerIndex.One] != null && string.CompareOrdinal(Gamer.SignedInGamers[PlayerIndex.One].Gamertag, "Player") == 0;
      }
    }

    public bool GamerTagChangeAvailable
    {
      get
      {
        return this.IsCurrentGamer && this.ProfileProperties != null && this.ProfileProperties.ContainsKey(ProfileProperty.FreeGamertagChangeEligible) && (bool) this.ProfileProperties[ProfileProperty.FreeGamertagChangeEligible];
      }
    }

    public bool IsGamerOnline
    {
      get
      {
        return this.PresenceInfo != null && (this.PresenceInfo.OnlineState == 0U || this.PresenceInfo.OnlineState == 3U || this.PresenceInfo.OnlineState == 2U);
      }
    }

    public bool IsModifyingFriendsListData
    {
      get => this.isModifyingFriendsListData;
      private set
      {
        ThreadManager.UIThread.AssertIsCurrentThread();
        this.isModifyingFriendsListData = value;
      }
    }

    public bool HasGamerPic
    {
      get
      {
        string str = string.Empty;
        if (this.ProfileProperties != null && this.ProfileProperties.ContainsKey(ProfileProperty.GamerPicUrl))
          str = (string) this.ProfileProperties[ProfileProperty.GamerPicUrl];
        return !string.IsNullOrEmpty(str);
      }
    }

    public bool IsChild
    {
      get
      {
        return this.ProfileProperties == null || !this.ProfileProperties.ContainsKey(ProfileProperty.IsParentallyControlled) || (bool) this.ProfileProperties[ProfileProperty.IsParentallyControlled];
      }
    }

    public bool AllowAcceptFriendRequest => !this.IsChild;

    [DataMember]
    public bool IsCurrentGamer { get; set; }

    [DataMember]
    public string GamerTag { get; set; }

    [DataMember]
    public bool AvatarFiltered
    {
      get => this.avatarFiltered;
      set => this.avatarFiltered = value;
    }

    public bool AvatarFilteredDialogShown
    {
      get => this.avatarFilteredDialogShown;
      set => this.avatarFilteredDialogShown = value;
    }

    [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Need to keep it as a property, since it is part of the data contract.")]
    [DataMember]
    public byte[] AvatarManifest
    {
      get
      {
        lock (this.StorageLock)
          return this.avatarManifest;
      }
      set
      {
        lock (this.StorageLock)
          this.avatarManifest = value;
        this.OnPropertyChanged(nameof (AvatarManifest));
      }
    }

    public DateTime AvatarLastUpdated { get; set; }

    [DataMember]
    public ulong XboxUserId { get; set; }

    [DataMember]
    public XmlSerializableDictionary<ProfileProperty, object> ProfileProperties { get; set; }

    [DataMember]
    public XmlSerializableDictionary<PrivacySetting, uint> PrivacySettings { get; set; }

    [DataMember]
    public Presence PresenceInfo
    {
      get => this.presenceInfo;
      set
      {
        this.presenceInfo = value;
        this.UpdatePresenceString();
      }
    }

    [DataMember]
    public string PresenceString { get; set; }

    public string DetailedPresenceString
    {
      get
      {
        string detailedPresenceString = this.PresenceString;
        if (this.IsGamerOnline && this.PresenceInfo != null && !string.IsNullOrEmpty(this.PresenceInfo.DetailedPresence))
          detailedPresenceString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}{1}{2}", new object[3]
          {
            (object) this.PresenceString,
            (object) System.Environment.NewLine,
            (object) this.PresenceInfo.DetailedPresence
          });
        return detailedPresenceString;
      }
    }

    [DataMember]
    public XmlSerializableList<Leet.UserGameData.DataContracts.Achievement> RecentAchievements { get; set; }

    [DataMember]
    public GamerPrivilegeSetting AllowCommunication { get; set; }

    [DataMember]
    public GamerPrivilegeSetting AllowProfileViewing { get; set; }

    public ProfileData ProfileData { get; private set; }

    public AchievementsData AchievementsPerGame
    {
      get => this.achievementsPerGame.Value;
      private set => this.achievementsPerGame.ForceSet(value);
    }

    public ClosetAssetsHolder AvatarClosetAssets { get; private set; }

    public GamesData GamesData
    {
      get => this.gamesData.Value;
      private set => this.gamesData.ForceSet(value);
    }

    public MessagesData MessagesData
    {
      get => this.messagesData.Value;
      private set => this.messagesData.ForceSet(value);
    }

    public FriendsListData FriendsListData
    {
      get => this.friendsListData.Value;
      private set => this.friendsListData.ForceSet(value);
    }

    [DefaultValue(false)]
    public bool ReceiveAvatarUpdate { get; set; }

    public bool IsLoadingProfile
    {
      get => this.isLoadingProfile.Value;
      private set => this.isLoadingProfile.ForceSet(value);
    }

    public bool IsLoadingFriends
    {
      get => this.isLoadingFriends.Value;
      private set => this.isLoadingFriends.ForceSet(value);
    }

    public bool IsLoadingAvatar
    {
      get => this.isLoadingAvatar.Value;
      private set => this.isLoadingAvatar.ForceSet(value);
    }

    public bool IsLoadingAvatarClosetAssets
    {
      get => this.isLoadingAvatarClosetAssets.Value;
      private set => this.isLoadingAvatarClosetAssets.ForceSet(value);
    }

    public bool IsLoadingGames
    {
      get => this.isLoadingGames.Value;
      private set => this.isLoadingGames.ForceSet(value);
    }

    public bool IsLoadingMessages
    {
      get => this.isLoadingMessages.Value;
      private set => this.isLoadingMessages.ForceSet(value);
    }

    public bool IsLoadingGameAchievements
    {
      get => this.isLoadingGameAchievements.Value;
      private set => this.isLoadingGameAchievements.ForceSet(value);
    }

    public bool IsLoadingXboxUserId
    {
      get => this.isLoadingXboxUserId.Value;
      private set => this.isLoadingXboxUserId.ForceSet(value);
    }

    [DefaultValue(false)]
    public bool IsLoading
    {
      get
      {
        return this.profileServiceManager.IsLoading() || this.messageServiceManager.IsLoading() || this.gameDataServiceManager.IsLoading() || this.avatarServiceManager.IsLoading();
      }
    }

    [DefaultValue(LoadState.NotInitialized)]
    public int ProfileState
    {
      get => this.profileState;
      set
      {
        if (this.profileState == value)
          return;
        this.profileState = value;
        this.OnPropertyChanged(nameof (ProfileState));
      }
    }

    [DefaultValue(LoadState.NotInitialized)]
    public int FriendsState
    {
      get => this.friendsState;
      set
      {
        if (this.friendsState == value)
          return;
        this.friendsState = value;
        this.OnPropertyChanged(nameof (FriendsState));
      }
    }

    [DefaultValue(LoadState.NotInitialized)]
    public int GamesState
    {
      get => this.gamesState;
      set
      {
        if (this.gamesState == value)
          return;
        this.gamesState = value;
        this.OnPropertyChanged(nameof (GamesState));
      }
    }

    [DefaultValue(LoadState.NotInitialized)]
    public int MessagesState
    {
      get => this.messagesState;
      set
      {
        if (this.messagesState == value)
          return;
        this.messagesState = value;
        this.OnPropertyChanged(nameof (MessagesState));
      }
    }

    public string LegalLocale
    {
      get => this.gamerContextEx != null ? this.gamerContextEx.LegalLocale : string.Empty;
    }

    public static XboxLiveGamer GetXboxLiveGamer(string gamertag)
    {
      return XboxLiveGamer.GetXboxLiveGamer(gamertag, true);
    }

    public static XboxLiveGamer GetXboxLiveGamer(string gamerTag, bool addToCache)
    {
      XboxLiveGamer currentGamer = XboxLiveGamer.CurrentGamer;
      if (string.IsNullOrEmpty(gamerTag) || string.Compare(gamerTag, XboxLiveGamer.CurrentGamer.GamerTag, StringComparison.OrdinalIgnoreCase) == 0)
        return currentGamer;
      if (XboxLiveGamer.gamerInMemoryCache.ContainsKey(gamerTag))
        return XboxLiveGamer.gamerInMemoryCache[gamerTag];
      XboxLiveGamer gamer = XboxLiveGamer.CreateGamer(gamerTag);
      if (addToCache)
        XboxLiveGamer.AddGamerToCache(gamer);
      return gamer;
    }

    public static string GetAvatarPicImageSource(string gamerTag)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "http://avatar{0}.xboxlive.com/avatar/{1}/avatarpic-l.png", new object[2]
      {
        (object) ServiceCommon.GetEnvironmentUrlStringPrefix(EnvironmentState.Instance.Environment),
        (object) gamerTag
      });
    }

    [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Underlying API expects a string, not a URI.")]
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "audienceUri", Justification = "This parameter is used based on compiler flags.")]
    public static string GetPartnerToken(string audienceUri)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      if (Gamer.SignedInGamers.Count <= 0)
        return string.Empty;
      string partnerToken = Gamer.GetPartnerToken(audienceUri);
      if (string.IsNullOrEmpty(partnerToken) || !string.Equals(audienceUri, "http://xboxlive.com"))
        return partnerToken;
      int startIndex = partnerToken.IndexOf("</saml:Assertion>", StringComparison.InvariantCultureIgnoreCase) + "</saml:Assertion>".Length;
      return partnerToken.Substring(startIndex);
    }

    public static void LoadXboxUserId()
    {
      if (XboxLiveGamer.CurrentGamer.IsLoadingXboxUserId)
        return;
      if (XboxLiveGamer.CurrentGamer.XboxUserId != 0UL)
      {
        EventHandler<EventArgs> loadedXboxUserId = XboxLiveGamer.CurrentGamer.EventLoadedXboxUserId;
        if (loadedXboxUserId != null)
          loadedXboxUserId((object) XboxLiveGamer.CurrentGamer, (EventArgs) null);
      }
      XboxLiveGamer.CurrentGamer.IsLoadingXboxUserId = true;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        string partnerToken = XboxLiveGamer.GetPartnerToken("http://xboxlive.com/userdata");
        if (!string.IsNullOrWhiteSpace(partnerToken))
        {
          XDocument xdocument = XDocument.Parse(partnerToken);
          if (xdocument != null)
          {
            XNamespace tokenNamespace = (XNamespace) "urn:oasis:names:tc:SAML:1.0:assertion";
            foreach (XContainer xcontainer in xdocument.Descendants(tokenNamespace + "AttributeStatement").Select<XElement, XElement>((Func<XElement, XElement>) (item => item)))
            {
              using (IEnumerator<string> enumerator = xcontainer.Descendants(tokenNamespace + "Attribute").Where<XElement>((Func<XElement, bool>) (item => ((string) item.Attribute((XName) "AttributeName")).Equals("PartnerID0"))).Select<XElement, string>((Func<XElement, string>) (item => (string) item.Element(tokenNamespace + "AttributeValue"))).GetEnumerator())
              {
                if (enumerator.MoveNext())
                {
                  string xuid = enumerator.Current;
                  ThreadManager.UIThreadPost((SendOrPostCallback) delegate
                  {
                    XboxLiveGamer.CurrentGamer.XboxUserId = ulong.Parse(xuid, (IFormatProvider) CultureInfo.InvariantCulture);
                    XboxLiveGamer.CurrentGamer.IsLoadingXboxUserId = false;
                    EventHandler<EventArgs> loadedXboxUserId = XboxLiveGamer.CurrentGamer.EventLoadedXboxUserId;
                    if (loadedXboxUserId == null)
                      return;
                    loadedXboxUserId((object) XboxLiveGamer.CurrentGamer, (EventArgs) null);
                  }, (object) null);
                  return;
                }
              }
            }
          }
        }
        IUserClaimsServiceManager claimsServiceManager = ServiceManagerFactory.CreateUserClaimsServiceManager();
        claimsServiceManager.Initialize(EnvironmentState.Instance.Environment);
        claimsServiceManager.EventGetUserInfoCompleted += new EventHandler<ServiceProxyEventArgs<UserInfo>>(XboxLiveGamer.CurrentGamer.GetUserInfoCompleted);
        claimsServiceManager.GetUserInfo();
      });
    }

    public static void InitializeCurrentGamer(ManualResetEvent applicationLaunchEvent)
    {
      XboxLiveGamer.ApplicationLaunchEvent = applicationLaunchEvent;
      XboxLiveGamer.currentGamer = XboxLiveGamer.CreateGamer(string.Empty);
      XboxLiveGamer.GamerState.EventGamerStateUpdate += new EventHandler<ServiceProxyEventArgs<XboxLiveGamerState.GamerStates>>(XboxLiveGamer.currentGamer.OnGamerStateUpdate);
      XboxLiveGamer.currentGamer.ForceSignIn();
      if (ServiceManagerFactory.IsRunningEmulator)
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        if (XboxLiveGamer.ApplicationLaunchEvent != null)
          XboxLiveGamer.ApplicationLaunchEvent.WaitOne();
        ThreadManager.UIThreadSend((SendOrPostCallback) delegate
        {
          SignedInGamer.SignedIn += new EventHandler<SignedInEventArgs>(XboxLiveGamer.SignedInGamer_SignedIn);
          SignedInGamer.SignedOut += new EventHandler<SignedOutEventArgs>(XboxLiveGamer.SignedInGamer_SignedOut);
        }, (object) null);
      });
    }

    public static void SignedInGamer_SignedOut(object sender, SignedOutEventArgs e)
    {
      if (!ServiceManagerFactory.IsRunningEmulator)
      {
        if (e == null)
          throw new ArgumentNullException(nameof (e));
        if (e.Gamer == null || string.CompareOrdinal(e.Gamer.Gamertag, "Player") == 0 || !e.Gamer.IsSignedInToLive)
          return;
      }
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        XboxLiveGamer.GamerState.UpdateState(XboxLiveGamerState.GamerStateEvent.SignedOut);
        XboxLiveGamer.CurrentGamer.ForceSignIn();
      }, (object) null);
    }

    public bool IsGamerTagLocalFriend(string gamerTag)
    {
      FriendsListData friendsListData = this.FriendsListData;
      return friendsListData != null && friendsListData.IsGamerTagFriend(gamerTag);
    }

    public bool IsGamerTagLocalPendingFriend(string gamerTag)
    {
      FriendsListData friendsListData = this.FriendsListData;
      return friendsListData != null && friendsListData.IsGamerTagPendingFriend(gamerTag);
    }

    public bool IsGamerTagLocalRequestingFriend(string gamerTag)
    {
      FriendsListData friendsListData = this.FriendsListData;
      return friendsListData != null && friendsListData.IsGamerTagRequestingFriend(gamerTag);
    }

    public void AddToFriendsList(FriendEntryData friend)
    {
      ThreadManager.UIThread.AssertIsCurrentThread();
      FriendsListData friendsListData = this.FriendsListData;
      if (friendsListData == null)
        return;
      this.IsModifyingFriendsListData = true;
      friendsListData.AddToFriendsList(friend);
      friendsListData.CreateOnlineOfflineFriends();
      friendsListData.AddToFriendsWhiteList(friend);
      this.IsModifyingFriendsListData = false;
      CacheManager.Instance.SaveAsync((CacheItem) friendsListData);
    }

    public void AddToRequestingFriendsList(FriendEntryData friend)
    {
      if (friend == null)
        throw new ArgumentNullException(nameof (friend));
      ThreadManager.UIThread.AssertIsCurrentThread();
      FriendsListData friendsListData = this.FriendsListData;
      if (friendsListData == null)
        return;
      this.IsModifyingFriendsListData = true;
      friendsListData.AddToRequestingFriendsList(friend);
      friendsListData.AddToRequestingFriendsWhiteList(friend.GamerTag);
      this.IsModifyingFriendsListData = false;
      friendsListData.RequestedFriendsList.Sort();
      CacheManager.Instance.SaveAsync((CacheItem) friendsListData);
    }

    public void RemoveFromFriendsList(string gamerTag)
    {
      ThreadManager.UIThread.AssertIsCurrentThread();
      FriendsListData friendsListData = this.FriendsListData;
      if (friendsListData == null)
        return;
      this.IsModifyingFriendsListData = true;
      friendsListData.RemoveFromFriendsList(gamerTag);
      friendsListData.CreateOnlineOfflineFriends();
      friendsListData.AddToFriendsBlackList(gamerTag);
      this.IsModifyingFriendsListData = false;
      CacheManager.Instance.SaveAsync((CacheItem) friendsListData);
    }

    public void RemoveFromPendingFriendsList(string gamerTag)
    {
      ThreadManager.UIThread.AssertIsCurrentThread();
      FriendsListData friendsListData = this.FriendsListData;
      if (friendsListData == null)
        return;
      this.IsModifyingFriendsListData = true;
      friendsListData.RemoveFromPendingFriendsList(gamerTag);
      friendsListData.AddToPendingFriendsBlackList(gamerTag);
      this.IsModifyingFriendsListData = false;
      CacheManager.Instance.SaveAsync((CacheItem) friendsListData);
    }

    public void RemoveFromRequestingFriendsList(string gamerTag)
    {
      ThreadManager.UIThread.AssertIsCurrentThread();
      FriendsListData friendsListData = this.FriendsListData;
      if (friendsListData == null)
        return;
      this.IsModifyingFriendsListData = true;
      friendsListData.RemoveFromRequestingFriendsList(gamerTag);
      friendsListData.AddToRequestingFriendsBlackList(gamerTag);
      this.IsModifyingFriendsListData = false;
      CacheManager.Instance.SaveAsync((CacheItem) friendsListData);
    }

    public string GetLocalFriendGamerPic(string gamerTag)
    {
      FriendsListData friendsListData = this.FriendsListData;
      return friendsListData != null ? friendsListData.GetFriendGamerPic(gamerTag) : string.Empty;
    }

    public void LoadGamerData(
      long sectionFlags,
      XboxLiveGamer.DataSource dataSource,
      uint gameId,
      uint pageNumber)
    {
      this.LoadGamerData(sectionFlags, dataSource, true, gameId, pageNumber);
    }

    public void LoadGamerData(
      long sectionFlags,
      XboxLiveGamer.DataSource dataSource,
      bool alwaysForceServiceCall,
      uint gameId,
      uint pageNumber)
    {
      if (dataSource == XboxLiveGamer.DataSource.Cache || dataSource == XboxLiveGamer.DataSource.Both)
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          long num1 = sectionFlags & 134217728L;
          if ((sectionFlags & 536870912L) != 0L)
          {
            if (this.FriendsListData != null)
            {
              DateTime lastUpdateTime1 = this.FriendsListData.LastUpdateTime;
            }
            else
            {
              if (this.friendsListData.TestAndSet((FriendsListData) null, CacheManager.Load<FriendsListData>("FRI_" + this.GamerTag)))
                this.OnPropertyChanged("FriendsListData");
              if (this.FriendsListData != null)
                this.FriendsState = 1;
            }
          }
          if ((sectionFlags & 33554432L) != 0L)
          {
            if (this.GamesData != null)
            {
              DateTime lastUpdateTime2 = this.GamesData.LastUpdateTime;
            }
            else
            {
              if (this.gamesData.TestAndSet((GamesData) null, CacheManager.Load<GamesData>("GAM_" + this.GamerTag)))
                this.OnPropertyChanged("GamesData");
              if (this.GamesData != null)
                this.GamesState = 1;
            }
          }
          if ((sectionFlags & 16777216L) != 0L)
          {
            if (this.MessagesData != null)
            {
              DateTime lastUpdateTime3 = this.MessagesData.LastUpdateTime;
            }
            else
            {
              if (this.messagesData.TestAndSet((MessagesData) null, CacheManager.Load<MessagesData>("MSG_" + this.GamerTag)))
                this.OnPropertyChanged("MessagesData");
              if (this.MessagesData != null)
                this.MessagesState = 1;
            }
          }
          long num2 = sectionFlags & 67108864L;
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
            if (gamerDataUpdated == null)
              return;
            gamerDataUpdated((object) this, new ServiceProxyEventArgs<long>((object) sectionFlags, (Exception) null, false, (object) sectionFlags));
          }, (object) this);
        });
      if (dataSource != XboxLiveGamer.DataSource.WebService && dataSource != XboxLiveGamer.DataSource.Both)
        return;
      if (XboxLiveGamer.GamerState.IsOnline)
      {
        this.LoadGamerDataInternal(sectionFlags, alwaysForceServiceCall, gameId, pageNumber);
        this.UpdateLoadingStatus();
      }
      else
      {
        int exceptionCode = XboxLiveGamer.GamerState.GamerState != XboxLiveGamerState.GamerStates.SigningIn ? 1004 : 1003;
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
          if (gamerDataUpdated == null)
            return;
          XLiveMobileException exception = XLiveMobileException.CreateException(exceptionCode, (string) null, (string[]) null);
          exception.Action = XLiveMobileException.ErrorAction.Ignore;
          gamerDataUpdated((object) this, new ServiceProxyEventArgs<long>((object) sectionFlags, (Exception) exception, false, (object) sectionFlags));
        }, (object) this);
      }
    }

    public void SendPropertyChangedNotification(string propertyName)
    {
      this.OnPropertyChanged(propertyName);
    }

    public void UpdateProfileAsync(
      long sectionFlags,
      XmlSerializableDictionary<ProfileProperty, object> profileUpdates,
      XmlSerializableDictionary<PrivacySetting, uint> privacyUpdates)
    {
      XboxLiveGamer.IssueOnlineUpdateCalls((XboxLiveGamer.OnlineUpdateDelegate) (() =>
      {
        if (this.pendingProfileUpdates != null)
          throw new InvalidOperationException("UpdateProfileAsync should be not be called before a previously pending operation has completed.");
        if (this.profileServiceManager == null)
          return;
        ProfileEx profileUpdates1 = this.PopulateProfileForUpdate(sectionFlags, profileUpdates, privacyUpdates);
        this.pendingProfileUpdates = profileUpdates1;
        this.profileServiceManager.EventUpdateProfileCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.UpdateProfileCompleted);
        this.profileServiceManager.UpdateProfileAsync(profileUpdates1);
      }), (XboxLiveGamer.OnlineUpdateFailDelegate) (ex => this.UpdateProfileCompleted((object) null, new ServiceProxyEventArgs<string>((object) null, (Exception) ex, false, (object) null))));
    }

    public void UpdatePresenceAsync()
    {
      if (this.profileServiceManager == null)
        return;
      this.profileServiceManager.UpdatePresence();
    }

    public void ChangeGamerTag(string desiredGamerTag)
    {
      XboxLiveGamer.IssueOnlineUpdateCalls((XboxLiveGamer.OnlineUpdateDelegate) (() => this.profileServiceManager.ChangeGamerTag(desiredGamerTag)), (XboxLiveGamer.OnlineUpdateFailDelegate) (ex => this.OnGamerTagChangeCompleted((object) null, new ServiceProxyEventArgs<string[]>((object) null, (Exception) ex, false, (object) null))));
    }

    public void UpdateAvatarAsync(string manifestString)
    {
      manifestString = manifestString != null ? manifestString.Replace("-", string.Empty) : throw XLiveMobileException.CreateException(7001, "No avatar manifest to update.");
      this.avatarServiceManager.UpdateManifestAsync(manifestString);
    }

    public void SetGamerpicAsync(GamerpicParameters gamerPicParameters)
    {
      this.avatarServiceManager.SetGamerpicAsync(gamerPicParameters);
    }

    public void AddFriendAsync(string gamerTag)
    {
      XboxLiveGamer.IssueOnlineUpdateCalls((XboxLiveGamer.OnlineUpdateDelegate) (() => this.friendServiceManager.AddFriendAsync(gamerTag)), (XboxLiveGamer.OnlineUpdateFailDelegate) (ex => this.OnAddFriendRequestCompleted((object) null, new ServiceProxyEventArgs<string>((object) null, (Exception) ex, false, (object) null))));
    }

    public void RemoveFriendAsync(string gamerTag)
    {
      XboxLiveGamer.IssueOnlineUpdateCalls((XboxLiveGamer.OnlineUpdateDelegate) (() => this.friendServiceManager.RemoveFriendAsync(gamerTag)), (XboxLiveGamer.OnlineUpdateFailDelegate) (ex => this.OnRemoveFriendCompleted((object) null, new ServiceProxyEventArgs<string>((object) null, (Exception) ex, false, (object) null))));
    }

    public void OnRemoveFriendCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      Exception error = e.Error;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        EventHandler<ServiceProxyEventArgs<string>> removeFriendCompleted = this.EventRemoveFriendCompleted;
        if (removeFriendCompleted == null)
          return;
        removeFriendCompleted((object) this, e);
      }, (object) this);
    }

    public void AcceptFriendRequestAsync(string gamerTag)
    {
      XboxLiveGamer.IssueOnlineUpdateCalls((XboxLiveGamer.OnlineUpdateDelegate) (() => this.friendServiceManager.AcceptFriendRequestAsync(gamerTag)), (XboxLiveGamer.OnlineUpdateFailDelegate) (ex => this.OnAcceptFriendRequestCompleted((object) null, new ServiceProxyEventArgs<string>((object) null, (Exception) ex, false, (object) null))));
    }

    public void OnAcceptFriendRequestCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      Exception error = e.Error;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        EventHandler<ServiceProxyEventArgs<string>> requestCompleted = this.EventAcceptFriendRequestCompleted;
        if (requestCompleted == null)
          return;
        requestCompleted((object) this, e);
      }, (object) this);
    }

    public void DeclineFriendRequestAsync(string gamerTag)
    {
      XboxLiveGamer.IssueOnlineUpdateCalls((XboxLiveGamer.OnlineUpdateDelegate) (() => this.friendServiceManager.DeclineFriendRequestAsync(gamerTag)), (XboxLiveGamer.OnlineUpdateFailDelegate) (ex => this.OnDeclineFriendRequestCompleted((object) null, new ServiceProxyEventArgs<string>((object) null, (Exception) ex, false, (object) null))));
    }

    public void OnDeclineFriendRequestCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      Exception error = e.Error;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        EventHandler<ServiceProxyEventArgs<string>> requestCompleted = this.EventDeclineFriendRequestCompleted;
        if (requestCompleted == null)
          return;
        requestCompleted((object) this, e);
      }, (object) this);
    }

    public void CancelFriendRequestAsync(string gamerTag)
    {
      XboxLiveGamer.IssueOnlineUpdateCalls((XboxLiveGamer.OnlineUpdateDelegate) (() => this.friendServiceManager.CancelFriendRequestAsync(gamerTag)), (XboxLiveGamer.OnlineUpdateFailDelegate) (ex => this.OnCancelFriendRequestCompleted((object) null, new ServiceProxyEventArgs<string>((object) null, (Exception) ex, false, (object) null))));
    }

    public void OnCancelFriendRequestCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      Exception error = e.Error;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        EventHandler<ServiceProxyEventArgs<string>> requestCompleted = this.EventCancelFriendRequestCompleted;
        if (requestCompleted == null)
          return;
        requestCompleted((object) this, e);
      }, (object) this);
    }

    public void GetManifestCompleted(object sender, ServiceProxyEventArgs<AvatarManifests> e)
    {
      bool avatarChanged = false;
      Exception error = e.Error;
      ServiceProxyEventArgs<long> responseArgs;
      AvatarManifests result;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.IsLoadingAvatar = false;
        this.UpdateLoadingStatus();
        LoadingObjectWrapper loadingObjectWrapper = new LoadingObjectWrapper();
        if (e.ResultAvailable)
        {
          result = e.Result;
          byte[] b = (byte[]) null;
          bool flag = false;
          if (result.Manifests.Length > 0)
          {
            flag = result.Manifests[0].Filtered;
            b = ShaConverter.Sha1000to1021(LiveAppLib.ManifestStringToByteArray(result.Manifests[0].Manifest));
          }
          this.AvatarLastUpdated = DateTime.UtcNow;
          if (!LiveAppLib.CompareByteArray(this.AvatarManifest, b))
            avatarChanged = true;
          else if (b == null)
            avatarChanged = true;
          if (this.AvatarFiltered != flag)
          {
            this.AvatarFilteredDialogShown = false;
            this.AvatarFiltered = flag;
          }
          loadingObjectWrapper.SetLoaded(b);
          responseArgs = new ServiceProxyEventArgs<long>((object) 1073741824L, (Exception) null, false, (object) 1073741824L);
        }
        else
        {
          loadingObjectWrapper.SetFailed();
          responseArgs = new ServiceProxyEventArgs<long>((object) 1073741824L, e.Error, false, (object) 1073741824L);
        }
        this.ChangeAvatarManifest(loadingObjectWrapper);
        if (avatarChanged || this.ReceiveAvatarUpdate)
        {
          EventHandler<ServiceProxyEventArgs<LoadingObjectWrapper>> loadedAvatarManifest = this.EventLoadedAvatarManifest;
          if (loadedAvatarManifest != null)
            loadedAvatarManifest((object) this, new ServiceProxyEventArgs<LoadingObjectWrapper>((object) loadingObjectWrapper, (Exception) null, false, (object) null));
          EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
          if (gamerDataUpdated != null)
            gamerDataUpdated((object) this, responseArgs);
        }
        this.ReceiveAvatarUpdate = false;
      }, (object) this);
    }

    public void GetClosetAssetsCompleted(object sender, ServiceProxyEventArgs<ClosetAssetsHolder> e)
    {
      Exception error = e.Error;
      ServiceProxyEventArgs<long> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.IsLoadingAvatarClosetAssets = false;
        this.UpdateLoadingStatus();
        if (e.ResultAvailable)
        {
          this.AvatarClosetAssets = e.Result;
          responseArgs = new ServiceProxyEventArgs<long>((object) 134217728L, (Exception) null, false, (object) 134217728L);
        }
        else
          responseArgs = new ServiceProxyEventArgs<long>((object) 134217728L, e.Error, false, (object) 134217728L);
        EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
        if (gamerDataUpdated == null)
          return;
        gamerDataUpdated((object) this, responseArgs);
      }, (object) this);
    }

    public void OnAddFriendRequestCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        EventHandler<ServiceProxyEventArgs<string>> requestCompleted = this.EventAddFriendRequestCompleted;
        if (requestCompleted == null)
          return;
        requestCompleted((object) this, e);
      }, (object) this);
    }

    public void OnGamerTagChangeCompleted(object sender, ServiceProxyEventArgs<string[]> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        EventHandler<ServiceProxyEventArgs<string[]>> requestCompleted = this.EventGamerTagChangeRequestCompleted;
        if (requestCompleted == null)
          return;
        requestCompleted((object) this, e);
      }, (object) this);
    }

    public void SetGamerpicCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<XboxLiveGamer> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        EventHandler<ServiceProxyEventArgs<XboxLiveGamer>> gamerpicCompleted = this.EventSetGamerpicCompleted;
        if (gamerpicCompleted == null)
          return;
        responseArgs = !e.ResultAvailable ? new ServiceProxyEventArgs<XboxLiveGamer>((object) null, e.Error, false, (object) null) : new ServiceProxyEventArgs<XboxLiveGamer>((object) this, (Exception) null, false, (object) null);
        gamerpicCompleted((object) this, responseArgs);
      }, (object) this);
    }

    public void UpdateManifestCompleted(
      object sender,
      ServiceProxyEventArgs<UpdateManifestResponse> e)
    {
      Exception error = e.Error;
      ServiceProxyEventArgs<XboxLiveGamer> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        EventHandler<ServiceProxyEventArgs<XboxLiveGamer>> updateAvatarCompleted = this.EventUpdateAvatarCompleted;
        if (updateAvatarCompleted == null)
          return;
        if (e.ResultAvailable)
        {
          responseArgs = new ServiceProxyEventArgs<XboxLiveGamer>((object) this, (Exception) null, false, (object) null);
          CacheManager.Instance.SaveAsync((CacheItem) this);
        }
        else
          responseArgs = new ServiceProxyEventArgs<XboxLiveGamer>((object) null, e.Error, false, (object) null);
        updateAvatarCompleted((object) this, responseArgs);
      }, (object) this);
    }

    public void GetProfileCompleted(object sender, ServiceProxyEventArgs<ProfileEx> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<long> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.IsLoadingProfile = false;
        this.UpdateLoadingStatus();
        if (e.ResultAvailable)
        {
          this.PopulateProfiles(e.Result);
          if (e.Result.PrivacySettings != null && e.Result.PrivacySettings.ContainsKey(PrivacySetting.VoiceAndText))
            this.SetCommunicationLevel(e.Result.PrivacySettings[PrivacySetting.VoiceAndText]);
          this.LastUpdateTime = DateTime.UtcNow;
          CacheManager.Instance.SaveAsync((CacheItem) this);
          this.OnPropertyChanged("GamerTag");
          this.OnPropertyChanged("ProfileData");
          this.OnPropertyChanged("GamerTagChangeAvailable");
          this.OnPropertyChanged("RecentAchievements");
          this.ProfileState = 1;
          responseArgs = new ServiceProxyEventArgs<long>((object) 268435456L, (Exception) null, false, (object) 268435456L);
        }
        else
        {
          responseArgs = new ServiceProxyEventArgs<long>((object) 268435456L, e.Error, false, (object) 268435456L);
          this.ProfileState = 2;
        }
        EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
        if (gamerDataUpdated == null)
          return;
        gamerDataUpdated((object) this, responseArgs);
      }, (object) this);
    }

    public void GetFriendsCompleted(object sender, ServiceProxyEventArgs<ProfileEx> e)
    {
      if (e.ResultAvailable)
        this.PopulateFriends(e.Result);
      ServiceProxyEventArgs<long> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.IsLoadingFriends = false;
        this.UpdateLoadingStatus();
        if (e.ResultAvailable)
        {
          this.OnPropertyChanged("FriendsListData");
          this.FriendsState = 1;
          responseArgs = new ServiceProxyEventArgs<long>((object) 536870912L, (Exception) null, false, (object) 536870912L);
        }
        else
        {
          responseArgs = new ServiceProxyEventArgs<long>((object) 536870912L, e.Error, false, (object) 536870912L);
          this.FriendsState = 2;
        }
        EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
        if (gamerDataUpdated == null)
          return;
        gamerDataUpdated((object) this, responseArgs);
      }, (object) this);
    }

    public void GetGamesCompleted(object sender, ServiceProxyEventArgs<Games> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<long> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.IsLoadingGames = false;
        this.UpdateLoadingStatus();
        if (e.ResultAvailable)
        {
          GamesData gamesData = new GamesData("GAM_" + this.GamerTag);
          gamesData.PlayedGames = e.Result.UserGamesCollection;
          gamesData.TotalUniqueGames = e.Result.TotalUniqueGames;
          gamesData.PageNumber = e.Result.PageNumber;
          gamesData.LastUpdateTime = DateTime.UtcNow;
          this.GamesData = gamesData;
          this.OnPropertyChanged("GamesData");
          this.GamesState = 1;
          if (this.IsCurrentGamer && e.Result.PageNumber == 0U)
            CacheManager.Instance.SaveAsync((CacheItem) gamesData);
          responseArgs = new ServiceProxyEventArgs<long>((object) 33554432L, (Exception) null, false, (object) 33554432L);
        }
        else
        {
          responseArgs = new ServiceProxyEventArgs<long>((object) 33554432L, e.Error, false, (object) 33554432L);
          this.GamesState = 2;
        }
        EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
        if (gamerDataUpdated == null)
          return;
        gamerDataUpdated((object) this, responseArgs);
      }, (object) this);
    }

    public void GetAchievementsCompleted(object sender, ServiceProxyEventArgs<Achievements> e)
    {
      Exception error = e.Error;
      ServiceProxyEventArgs<long> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.IsLoadingGameAchievements = false;
        this.UpdateLoadingStatus();
        if (e.ResultAvailable && e.Result != null && e.Result.UserAchievementsCollection != null && e.Result.UserAchievementsCollection.Count > 0)
        {
          Achievements result = e.Result;
          for (int index = 0; index < result.UserAchievementsCollection.Count; ++index)
          {
            if (result.UserAchievementsCollection[index].AchievementList.Count > 0)
            {
              uint gameId = e.Result.UserAchievementsCollection[index].AchievementList[0].GameId;
              foreach (Leet.UserGameData.DataContracts.Achievement achievement in e.Result.UserAchievementsCollection[index].AchievementList)
                achievement.Name = achievement.Name.Trim();
              XboxLiveGamer.UpdateGamerAchievements(gameId, this.AchievementsPerGame, result.UserAchievementsCollection[index]);
            }
          }
          this.OnPropertyChanged("AchievementsPerGame");
          responseArgs = new ServiceProxyEventArgs<long>((object) 67108864L, (Exception) null, false, (object) 67108864L);
        }
        else
          responseArgs = new ServiceProxyEventArgs<long>((object) 67108864L, e.Error, false, (object) 67108864L);
        EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
        if (gamerDataUpdated == null)
          return;
        gamerDataUpdated((object) this, responseArgs);
      }, (object) this);
    }

    public void LoadOneMessageAsync(uint messageId)
    {
      this.messageServiceManager.GetOneMessageAsync(messageId);
    }

    public void SendMessage(SendMessageRequest messageToSend)
    {
      XboxLiveGamer.IssueOnlineUpdateCalls((XboxLiveGamer.OnlineUpdateDelegate) (() => this.messageServiceManager.SendMessageAsync(messageToSend)), (XboxLiveGamer.OnlineUpdateFailDelegate) (ex => this.SendMessageCompleted((object) null, new ServiceProxyEventArgs<ResponseWithErrorCode>((object) null, (Exception) ex, false, (object) null))));
    }

    public void DeleteMessage(uint messageId, bool blockSender)
    {
      XboxLiveGamer.IssueOnlineUpdateCalls((XboxLiveGamer.OnlineUpdateDelegate) (() => this.messageServiceManager.DeleteMessageAsync(messageId, blockSender)), (XboxLiveGamer.OnlineUpdateFailDelegate) (ex => this.DeleteMessageCompleted((object) null, new ServiceProxyEventArgs<ResponseWithErrorCode>((object) null, (Exception) ex, false, (object) null))));
    }

    public void DeleteMessageCompleted(
      object sender,
      ServiceProxyEventArgs<ResponseWithErrorCode> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<ResponseWithErrorCode> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        responseArgs = !e.ResultAvailable ? new ServiceProxyEventArgs<ResponseWithErrorCode>((object) null, e.Error, false, (object) null) : new ServiceProxyEventArgs<ResponseWithErrorCode>((object) this, (Exception) null, false, (object) null);
        EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> messageCompleted = this.EventDeleteMessageCompleted;
        if (messageCompleted == null)
          return;
        messageCompleted((object) this, responseArgs);
      }, (object) this);
    }

    public void SendMessageCompleted(object sender, ServiceProxyEventArgs<ResponseWithErrorCode> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<ResponseWithErrorCode> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        responseArgs = !e.ResultAvailable ? new ServiceProxyEventArgs<ResponseWithErrorCode>((object) null, e.Error, false, (object) null) : new ServiceProxyEventArgs<ResponseWithErrorCode>((object) this, (Exception) null, false, (object) null);
        EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> messageCompleted = this.EventSendMessageCompleted;
        if (messageCompleted == null)
          return;
        messageCompleted((object) this, responseArgs);
      }, (object) this);
    }

    public void GetMessageSummaryListCompleted(
      object sender,
      ServiceProxyEventArgs<MessageSummariesResponse> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<long> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.IsLoadingMessages = false;
        this.UpdateLoadingStatus();
        if (e.ResultAvailable)
        {
          MessagesData messagesData = new MessagesData("MSG_" + this.GamerTag);
          messagesData.MessageSummaryList = e.Result.Summaries;
          messagesData.HashCode = e.Result.HashCode;
          messagesData.LastUpdateTime = DateTime.UtcNow;
          this.MessagesData = messagesData;
          this.OnPropertyChanged("MessagesData");
          this.MessagesState = 1;
          CacheManager.Instance.SaveAsync((CacheItem) messagesData);
          responseArgs = new ServiceProxyEventArgs<long>((object) 16777216L, (Exception) null, false, (object) 16777216L);
        }
        else
        {
          responseArgs = new ServiceProxyEventArgs<long>((object) 16777216L, e.Error, false, (object) 16777216L);
          this.MessagesState = 2;
        }
        EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
        if (gamerDataUpdated == null)
          return;
        gamerDataUpdated((object) this, responseArgs);
      }, (object) this);
    }

    public void GetOneMessageCompleted(object sender, ServiceProxyEventArgs<MessageDetails> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<MessageDetails> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.UpdateLoadingStatus();
        responseArgs = !e.ResultAvailable ? new ServiceProxyEventArgs<MessageDetails>((object) null, e.Error, false, (object) null) : new ServiceProxyEventArgs<MessageDetails>((object) e.Result, (Exception) null, false, (object) null);
        EventHandler<ServiceProxyEventArgs<MessageDetails>> messageCompleted = this.EventGetOneMessageCompleted;
        if (messageCompleted == null)
          return;
        messageCompleted((object) this, responseArgs);
      }, (object) this);
    }

    public XboxLiveGamer.MessageCheckResult AllowSendMessageToGamertag(string gamerTag)
    {
      if (!this.IsCurrentGamer)
        return XboxLiveGamer.MessageCheckResult.NotCurrentGamer;
      if (this.ProfileProperties == null)
        return XboxLiveGamer.MessageCheckResult.DisabledForNoProfile;
      if (this.ProfileProperties.ContainsKey(ProfileProperty.MembershipLevel) && string.CompareOrdinal((string) this.ProfileProperties[ProfileProperty.MembershipLevel], "Silver") == 0)
        return XboxLiveGamer.MessageCheckResult.DisabledForServiceLevel;
      switch (this.AllowCommunication)
      {
        case GamerPrivilegeSetting.Blocked:
          return XboxLiveGamer.MessageCheckResult.DisabledForPrivacy;
        case GamerPrivilegeSetting.Everyone:
          return XboxLiveGamer.MessageCheckResult.Allowed;
        default:
          if (string.IsNullOrEmpty(gamerTag))
            return XboxLiveGamer.MessageCheckResult.FriendsOnly;
          return this.IsGamerTagLocalFriend(gamerTag) ? XboxLiveGamer.MessageCheckResult.Allowed : XboxLiveGamer.MessageCheckResult.DisabledForPrivacy;
      }
    }

    public void GetActivityListForTitle(uint titleId)
    {
      this.beaconsServiceManager.GetActivityListForTitle(titleId);
    }

    public void GetActivityListForTitleCompleted(
      object sender,
      ServiceProxyEventArgs<ActivityListForTitleData> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<ActivityListForTitleData> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.UpdateLoadingStatus();
        responseArgs = !e.ResultAvailable ? new ServiceProxyEventArgs<ActivityListForTitleData>((object) null, e.Error, false, (object) null) : new ServiceProxyEventArgs<ActivityListForTitleData>((object) e.Result, (Exception) null, false, (object) null);
        EventHandler<ServiceProxyEventArgs<ActivityListForTitleData>> forTitleCompleted = this.EventGetActivityListForTitleCompleted;
        if (forTitleCompleted == null)
          return;
        forTitleCompleted((object) this, responseArgs);
      }, (object) this);
    }

    public void GetBeaconForTitle(uint titleId)
    {
      this.beaconsServiceManager.GetBeaconForTitle(titleId);
    }

    public void GetBeaconForTitleCompleted(object sender, ServiceProxyEventArgs<BeaconInfo> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<BeaconInfo> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.UpdateLoadingStatus();
        responseArgs = !e.ResultAvailable ? new ServiceProxyEventArgs<BeaconInfo>((object) null, e.Error, false, (object) null) : new ServiceProxyEventArgs<BeaconInfo>((object) e.Result, (Exception) null, false, (object) null);
        EventHandler<ServiceProxyEventArgs<BeaconInfo>> forTitleCompleted = this.EventGetBeaconForTitleCompleted;
        if (forTitleCompleted == null)
          return;
        forTitleCompleted((object) this, responseArgs);
      }, (object) this);
    }

    public void SetBeaconForTitle(uint titleId, string beaconText)
    {
      this.beaconsServiceManager.SetBeaconForTitle(titleId, beaconText);
    }

    public void SetBeaconForTitleCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        EventHandler<ServiceProxyEventArgs<string>> forTitleCompleted = this.EventSetBeaconForTitleCompleted;
        if (forTitleCompleted == null)
          return;
        forTitleCompleted((object) this, e);
      }, (object) this);
    }

    public void DeleteBeaconForTitle(uint titleId)
    {
      this.beaconsServiceManager.DeleteBeaconForTitle(titleId);
    }

    public void DeleteBeaconForTitleCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        EventHandler<ServiceProxyEventArgs<string>> forTitleCompleted = this.EventDeleteBeaconForTitleCompleted;
        if (forTitleCompleted == null)
          return;
        forTitleCompleted((object) this, e);
      }, (object) this);
    }

    public void GetBeaconDefaultText(uint titleId)
    {
      this.beaconsServiceManager.GetBeaconDefaultText(titleId);
    }

    public void GetBeaconDefaultTextCompleted(
      object sender,
      ServiceProxyEventArgs<BeaconDefaultTextData> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<BeaconDefaultTextData> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.UpdateLoadingStatus();
        responseArgs = !e.ResultAvailable ? new ServiceProxyEventArgs<BeaconDefaultTextData>((object) null, e.Error, false, (object) null) : new ServiceProxyEventArgs<BeaconDefaultTextData>((object) e.Result, (Exception) null, false, (object) null);
        EventHandler<ServiceProxyEventArgs<BeaconDefaultTextData>> defaultTextCompleted = this.EventGetBeaconDefaultTextCompleted;
        if (defaultTextCompleted == null)
          return;
        defaultTextCompleted((object) this, responseArgs);
      }, (object) this);
    }

    public void GetLeaderboardsForTitle(uint titleId)
    {
      this.leaderboardsServiceManager.GetLeaderboardsForTitle(titleId);
    }

    public void GetLeaderboardsForTitleCompleted(
      object sender,
      ServiceProxyEventArgs<LeaderboardsForTitleData> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<LeaderboardsForTitleData> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.UpdateLoadingStatus();
        responseArgs = !e.ResultAvailable ? new ServiceProxyEventArgs<LeaderboardsForTitleData>((object) null, e.Error, false, (object) null) : new ServiceProxyEventArgs<LeaderboardsForTitleData>((object) e.Result, (Exception) null, false, (object) null);
        EventHandler<ServiceProxyEventArgs<LeaderboardsForTitleData>> forTitleCompleted = this.EventGetLeaderboardsForTitleCompleted;
        if (forTitleCompleted == null)
          return;
        forTitleCompleted((object) this, responseArgs);
      }, (object) this);
    }

    public void GetLeaderboardDetails(uint titleId, uint leaderboardId)
    {
      this.leaderboardsServiceManager.GetLeaderboardDetails(titleId, leaderboardId);
    }

    public void GetLeaderboardDetailsCompleted(
      object sender,
      ServiceProxyEventArgs<LeaderboardInfo> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<LeaderboardInfo> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.UpdateLoadingStatus();
        responseArgs = !e.ResultAvailable ? new ServiceProxyEventArgs<LeaderboardInfo>((object) null, e.Error, false, (object) null) : new ServiceProxyEventArgs<LeaderboardInfo>((object) e.Result, (Exception) null, false, (object) null);
        EventHandler<ServiceProxyEventArgs<LeaderboardInfo>> detailsCompleted = this.EventGetLeaderboardDetailsCompleted;
        if (detailsCompleted == null)
          return;
        detailsCompleted((object) this, responseArgs);
      }, (object) this);
    }

    public void PostLink(
      uint phoneAppTitleId,
      string message,
      string caption,
      string linkDescription,
      string linkName,
      string linkImage,
      string linkUrl,
      string actionName,
      string actionUrl)
    {
      this.facebookServiceManager.PostLink(phoneAppTitleId, message, caption, linkDescription, linkName, linkImage, linkUrl, actionName, actionUrl);
    }

    public void PostLinkCompleted(object sender, ServiceProxyEventArgs<ResponseWithErrorCode> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<ResponseWithErrorCode> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.UpdateLoadingStatus();
        responseArgs = !e.ResultAvailable ? new ServiceProxyEventArgs<ResponseWithErrorCode>((object) null, e.Error, false, (object) null) : new ServiceProxyEventArgs<ResponseWithErrorCode>((object) this, (Exception) null, false, (object) null);
        EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> postLinkCompleted = this.EventPostLinkCompleted;
        if (postLinkCompleted == null)
          return;
        postLinkCompleted((object) this, responseArgs);
      }, (object) this);
    }

    public void PostCredentials(string username, string password, bool allowFacebookInfoOnXboxLive)
    {
      this.facebookServiceManager.PostCredentials(username, password, allowFacebookInfoOnXboxLive);
    }

    public void PostCredentialsCompleted(
      object sender,
      ServiceProxyEventArgs<ResponseWithErrorCode> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<ResponseWithErrorCode> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.UpdateLoadingStatus();
        responseArgs = !e.ResultAvailable ? new ServiceProxyEventArgs<ResponseWithErrorCode>((object) null, e.Error, false, (object) null) : new ServiceProxyEventArgs<ResponseWithErrorCode>((object) this, (Exception) null, false, (object) null);
        EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> credentialsCompleted = this.EventPostCredentialsCompleted;
        if (credentialsCompleted == null)
          return;
        credentialsCompleted((object) this, responseArgs);
      }, (object) this);
    }

    public void PostPermissions(uint phoneAppTitleId, bool allow)
    {
      this.facebookServiceManager.PostPermissions(phoneAppTitleId, allow);
    }

    public void PostPermissionsCompleted(
      object sender,
      ServiceProxyEventArgs<ResponseWithErrorCode> e)
    {
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<ResponseWithErrorCode> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.UpdateLoadingStatus();
        responseArgs = !e.ResultAvailable ? new ServiceProxyEventArgs<ResponseWithErrorCode>((object) null, e.Error, false, (object) null) : new ServiceProxyEventArgs<ResponseWithErrorCode>((object) this, (Exception) null, false, (object) null);
        EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>> permissionsCompleted = this.EventPostPermissionsCompleted;
        if (permissionsCompleted == null)
          return;
        permissionsCompleted((object) this, responseArgs);
      }, (object) this);
    }

    public void GetGamerContextCompleted(object sender, AsyncCompletedEventArgs e)
    {
      if (XboxLiveGamer.currentGamer != null && XboxLiveGamer.currentGamer.gamerContextEx != null)
      {
        XboxLiveGamer.currentGamer.gamerContextEx.GetGamerContextCompleted -= new EventHandler<AsyncCompletedEventArgs>(XboxLiveGamer.currentGamer.GetGamerContextCompleted);
        if (e.Error == null && !e.Cancelled)
        {
          CacheManager.Save("PdlcGamerContextEx.xml", (object) this.gamerContextEx);
          XboxLiveGamer.GamerState.UpdateState(XboxLiveGamerState.GamerStateEvent.SignInSucceed);
        }
        else
        {
          Exception error = e.Error;
          XboxLiveGamer.GamerState.UpdateState(XboxLiveGamerState.GamerStateEvent.SignInTimeout);
        }
      }
      else
      {
        if (XboxLiveGamer.currentGamer == null)
          return;
        GamerContextEx gamerContextEx = XboxLiveGamer.currentGamer.gamerContextEx;
      }
    }

    public void GetTitleHistory() => this.titleHistoryServiceManager.GetTitleHistory();

    public void GetTitleHistoryCompleted(
      object sender,
      ServiceProxyEventArgs<List<TitleHistoryInfo>> e)
    {
      Exception error = e.Error;
      EventHandler<ServiceProxyEventArgs<List<TitleHistoryInfo>>> historyCompleted = this.EventGetTitleHistoryCompleted;
      if (historyCompleted == null)
        return;
      historyCompleted((object) this, e);
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This block of code is too diverse to realistically manage exception types.")]
    private static XboxLiveGamer CreateGamer(string gamerTag)
    {
      XboxLiveGamer gamer;
      try
      {
        gamer = CacheManager.Load<XboxLiveGamer>("XLG_" + gamerTag);
        if (gamer != null)
        {
          gamer.Initialize();
          gamer.ProfileState = 1;
        }
      }
      catch (Exception ex)
      {
        gamer = (XboxLiveGamer) null;
      }
      if (gamer == null)
        gamer = new XboxLiveGamer(gamerTag);
      return gamer;
    }

    private static void AddGamerToCache(XboxLiveGamer gamer)
    {
      if (!gamer.IsCurrentGamer)
      {
        XboxLiveGamer.gamerInMemoryCache.Add(gamer.GamerTag, gamer);
        XboxLiveGamer.gamerQueue.Enqueue(gamer.GamerTag);
      }
      if (XboxLiveGamer.gamerInMemoryCache.Count <= 4)
        return;
      string key = XboxLiveGamer.gamerQueue.Dequeue();
      XboxLiveGamer.gamerInMemoryCache.Remove(key);
    }

    private static void IssueOnlineUpdateCalls(
      XboxLiveGamer.OnlineUpdateDelegate update,
      XboxLiveGamer.OnlineUpdateFailDelegate updateFail)
    {
      if (XboxLiveGamer.GamerState.GamerState == XboxLiveGamerState.GamerStates.SigningIn)
        updateFail(XLiveMobileException.CreateException(1003, (string) null, (string[]) null));
      else if (XboxLiveGamer.GamerState.GamerState == XboxLiveGamerState.GamerStates.Offline)
        updateFail(XLiveMobileException.CreateException(1004, (string) null, (string[]) null));
      else
        update();
    }

    private static void SignedInGamer_SignedIn(object sender, SignedInEventArgs e)
    {
      if (!ThreadManager.UIThread.IsCurrentThread())
      {
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          XboxLiveGamer.SignedInGamer_SignedIn(sender, e);
        }, (object) null);
      }
      else
      {
        XboxLiveGamer currentGamer = XboxLiveGamer.CurrentGamer;
        if (!ServiceManagerFactory.IsRunningEmulator)
        {
          if (e == null || e.Gamer == null || string.CompareOrdinal(e.Gamer.Gamertag, "Player") == 0 || !e.Gamer.IsSignedInToLive)
            return;
          XboxLiveGamer.CurrentGamerTag = e.Gamer.Gamertag;
          currentGamer.AllowCommunication = e.Gamer.Privileges.AllowCommunication;
          currentGamer.AllowProfileViewing = e.Gamer.Privileges.AllowProfileViewing;
        }
        XboxLiveGamer.currentGamer.gamerContextEx = CacheManager.Load<GamerContextEx>("PdlcGamerContextEx.xml");
        if (XboxLiveGamer.currentGamer.gamerContextEx != null && !string.IsNullOrWhiteSpace(XboxLiveGamer.currentGamer.gamerContextEx.LegalLocale))
        {
          XboxLiveGamer.GamerState.UpdateState(XboxLiveGamerState.GamerStateEvent.SignInSucceed);
        }
        else
        {
          XboxLiveGamer.currentGamer.gamerContextEx = new GamerContextEx();
          XboxLiveGamer.currentGamer.gamerContextEx.GetGamerContextCompleted += new EventHandler<AsyncCompletedEventArgs>(XboxLiveGamer.currentGamer.GetGamerContextCompleted);
          XboxLiveGamer.currentGamer.gamerContextEx.GetGamerContextAsync();
        }
      }
    }

    private static void UpdateGamerAchievements(
      uint gameId,
      AchievementsData achievementsPerGameData,
      UserAchievements userAchievements)
    {
      lock (achievementsPerGameData.StorageLock)
      {
        if (achievementsPerGameData.Achievements.ContainsKey(gameId))
          achievementsPerGameData.Achievements[gameId] = userAchievements;
        else
          achievementsPerGameData.Achievements.Add(gameId, userAchievements);
        achievementsPerGameData.LastUpdateTime = DateTime.UtcNow;
      }
    }

    private void Initialize()
    {
      this.avatarServiceManager = ServiceManagerFactory.CreateAvatarServiceManager();
      this.avatarServiceManager.Initialize(EnvironmentState.Instance.Environment);
      this.avatarServiceManager.EventGetManifestCompleted += new EventHandler<ServiceProxyEventArgs<AvatarManifests>>(this.GetManifestCompleted);
      this.avatarServiceManager.EventUpdateManifestCompleted += new EventHandler<ServiceProxyEventArgs<UpdateManifestResponse>>(this.UpdateManifestCompleted);
      this.avatarServiceManager.EventGetClosetAssetsCompleted += new EventHandler<ServiceProxyEventArgs<ClosetAssetsHolder>>(this.GetClosetAssetsCompleted);
      this.avatarServiceManager.EventSetGamerpicCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.SetGamerpicCompleted);
      this.profileServiceManager = ServiceManagerFactory.CreateProfileServiceManager();
      this.profileServiceManager.Initialize(EnvironmentState.Instance.Environment);
      this.profileServiceManager.EventGetProfileCompleted += new EventHandler<ServiceProxyEventArgs<ProfileEx>>(this.GetProfileCompleted);
      this.profileServiceManager.EventChangeGamerTagCompleted += new EventHandler<ServiceProxyEventArgs<string[]>>(this.OnGamerTagChangeCompleted);
      this.profileServiceManagerForFriend = ServiceManagerFactory.CreateProfileServiceManager();
      this.profileServiceManagerForFriend.Initialize(EnvironmentState.Instance.Environment);
      this.profileServiceManagerForFriend.EventGetProfileCompleted += new EventHandler<ServiceProxyEventArgs<ProfileEx>>(this.GetFriendsCompleted);
      this.gameDataServiceManager = ServiceManagerFactory.CreateGameDataServiceManager();
      this.gameDataServiceManager.Initialize(EnvironmentState.Instance.Environment);
      this.gameDataServiceManager.EventGetGamesCompleted += new EventHandler<ServiceProxyEventArgs<Games>>(this.GetGamesCompleted);
      this.gameDataServiceManager.EventGetAchievementsCompleted += new EventHandler<ServiceProxyEventArgs<Achievements>>(this.GetAchievementsCompleted);
      this.friendServiceManager = ServiceManagerFactory.CreateFriendServiceManager();
      this.friendServiceManager.Initialize(EnvironmentState.Instance.Environment);
      this.friendServiceManager.EventAddFriendCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.OnAddFriendRequestCompleted);
      this.friendServiceManager.EventRemoveFriendCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.OnRemoveFriendCompleted);
      this.friendServiceManager.EventAcceptFriendRequestCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.OnAcceptFriendRequestCompleted);
      this.friendServiceManager.EventDeclineFriendRequestCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.OnDeclineFriendRequestCompleted);
      this.friendServiceManager.EventCancelFriendRequestCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.OnCancelFriendRequestCompleted);
      this.messageServiceManager = ServiceManagerFactory.CreateMessageServiceManager();
      this.messageServiceManager.Initialize(EnvironmentState.Instance.Environment);
      this.messageServiceManager.EventGetMessageSummaryListCompleted += new EventHandler<ServiceProxyEventArgs<MessageSummariesResponse>>(this.GetMessageSummaryListCompleted);
      this.messageServiceManager.EventGetOneMessageCompleted += new EventHandler<ServiceProxyEventArgs<MessageDetails>>(this.GetOneMessageCompleted);
      this.messageServiceManager.EventDeleteMessageCompleted += new EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>>(this.DeleteMessageCompleted);
      this.messageServiceManager.EventSendMessageCompleted += new EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>>(this.SendMessageCompleted);
      this.beaconsServiceManager = ServiceManagerFactory.CreateBeaconsServiceManager();
      this.beaconsServiceManager.Initialize(EnvironmentState.Instance.Environment);
      this.beaconsServiceManager.EventGetActivityListForTitleCompleted += new EventHandler<ServiceProxyEventArgs<ActivityListForTitleData>>(this.GetActivityListForTitleCompleted);
      this.beaconsServiceManager.EventGetBeaconForTitleCompleted += new EventHandler<ServiceProxyEventArgs<BeaconInfo>>(this.GetBeaconForTitleCompleted);
      this.beaconsServiceManager.EventSetBeaconForTitleCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.SetBeaconForTitleCompleted);
      this.beaconsServiceManager.EventDeleteBeaconForTitleCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.DeleteBeaconForTitleCompleted);
      this.beaconsServiceManager.EventGetBeaconDefaultTextCompleted += new EventHandler<ServiceProxyEventArgs<BeaconDefaultTextData>>(this.GetBeaconDefaultTextCompleted);
      this.leaderboardsServiceManager = ServiceManagerFactory.CreateLeaderboardsServiceManager();
      this.leaderboardsServiceManager.Initialize(EnvironmentState.Instance.Environment);
      this.leaderboardsServiceManager.EventGetLeaderboardsForTitleCompleted += new EventHandler<ServiceProxyEventArgs<LeaderboardsForTitleData>>(this.GetLeaderboardsForTitleCompleted);
      this.leaderboardsServiceManager.EventGetLeaderboardDetailsCompleted += new EventHandler<ServiceProxyEventArgs<LeaderboardInfo>>(this.GetLeaderboardDetailsCompleted);
      this.facebookServiceManager = ServiceManagerFactory.CreateFacebookServiceManager();
      this.facebookServiceManager.Initialize(EnvironmentState.Instance.Environment);
      this.facebookServiceManager.EventPostLinkCompleted += new EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>>(this.PostLinkCompleted);
      this.facebookServiceManager.EventPostCredentialsCompleted += new EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>>(this.PostCredentialsCompleted);
      this.facebookServiceManager.EventPostPermissionsCompleted += new EventHandler<ServiceProxyEventArgs<ResponseWithErrorCode>>(this.PostPermissionsCompleted);
      this.titleHistoryServiceManager = ServiceManagerFactory.CreateTitleHistoryServiceManager();
      this.titleHistoryServiceManager.Initialize(EnvironmentState.Instance.Environment);
      this.titleHistoryServiceManager.EventGetTitleHistoryCompleted += new EventHandler<ServiceProxyEventArgs<List<TitleHistoryInfo>>>(this.GetTitleHistoryCompleted);
      this.ProfileData = new ProfileData(this);
      this.achievementsPerGame = new AtomicWrapper<AchievementsData>((AchievementsData) null);
      this.gamesData = new AtomicWrapper<GamesData>((GamesData) null);
      this.messagesData = new AtomicWrapper<MessagesData>((MessagesData) null);
      this.friendsListData = new AtomicWrapper<FriendsListData>((FriendsListData) null);
      this.isLoadingProfile = new AtomicWrapper<bool>(false);
      this.isLoadingFriends = new AtomicWrapper<bool>(false);
      this.isLoadingAvatar = new AtomicWrapper<bool>(false);
      this.isLoadingAvatarClosetAssets = new AtomicWrapper<bool>(false);
      this.isLoadingGames = new AtomicWrapper<bool>(false);
      this.isLoadingMessages = new AtomicWrapper<bool>(false);
      this.isLoadingGameAchievements = new AtomicWrapper<bool>(false);
      this.isLoadingXboxUserId = new AtomicWrapper<bool>(false);
    }

    private void GetUserInfoCompleted(object sender, ServiceProxyEventArgs<UserInfo> e)
    {
      ((IUserClaimsServiceManager) sender).EventGetUserInfoCompleted -= new EventHandler<ServiceProxyEventArgs<UserInfo>>(this.GetUserInfoCompleted);
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        if (e.Error == null && e.Result != null)
          this.XboxUserId = e.Result.XboxUserId;
        XboxLiveGamer.CurrentGamer.IsLoadingXboxUserId = false;
        EventHandler<EventArgs> loadedXboxUserId = this.EventLoadedXboxUserId;
        if (loadedXboxUserId == null)
          return;
        loadedXboxUserId((object) this, (EventArgs) null);
      }, (object) this);
    }

    private bool ShouldExpireCache(ServiceCommon.GamerDataSections section)
    {
      switch (section)
      {
        case ServiceCommon.GamerDataSections.Profile:
          DateTime lastUpdateTime = this.LastUpdateTime;
          return (DateTime.UtcNow - this.LastUpdateTime).TotalSeconds > 120.0;
        case ServiceCommon.GamerDataSections.Avatar:
          DateTime avatarLastUpdated = this.AvatarLastUpdated;
          return (DateTime.UtcNow - this.AvatarLastUpdated).TotalSeconds > 120.0;
        default:
          return true;
      }
    }

    private void SetCommunicationLevel(uint value)
    {
      switch (value)
      {
        case 0:
          this.AllowCommunication = GamerPrivilegeSetting.Everyone;
          break;
        case 1:
          this.AllowCommunication = GamerPrivilegeSetting.FriendsOnly;
          break;
        case 2:
          this.AllowCommunication = GamerPrivilegeSetting.Blocked;
          break;
      }
    }

    private void UpdateProfileCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      this.profileServiceManager.EventUpdateProfileCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(this.UpdateProfileCompleted);
      int num = e.ResultAvailable ? 1 : 0;
      ServiceProxyEventArgs<string> responseArgs;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        if (e.ResultAvailable)
        {
          this.PopulateProfiles(this.pendingProfileUpdates);
          if (this.pendingProfileUpdates.PrivacySettings != null && this.pendingProfileUpdates.PrivacySettings.ContainsKey(PrivacySetting.VoiceAndText))
            this.SetCommunicationLevel(this.pendingProfileUpdates.PrivacySettings[PrivacySetting.VoiceAndText]);
          this.LastUpdateTime = DateTime.UtcNow;
          CacheManager.Instance.SaveAsync((CacheItem) this);
          this.pendingProfileUpdates = (ProfileEx) null;
          this.OnPropertyChanged("ProfileData");
          this.OnPropertyChanged("RecentAchievements");
          this.ProfileState = 1;
          responseArgs = new ServiceProxyEventArgs<string>((object) 268435456L, (Exception) null, false, (object) 268435456L);
        }
        else
        {
          this.pendingProfileUpdates = (ProfileEx) null;
          responseArgs = new ServiceProxyEventArgs<string>((object) 268435456L, e.Error, false, (object) 268435456L);
          this.ProfileState = 2;
        }
        EventHandler<ServiceProxyEventArgs<string>> profileCompleted = this.EventUpdateProfileCompleted;
        if (profileCompleted == null)
          return;
        profileCompleted((object) this, responseArgs);
      }, (object) this);
    }

    private long PopulateProfiles(ProfileEx gamerProfile)
    {
      long num = 0;
      if (gamerProfile.ProfileProperties != null)
      {
        this.ProfileProperties = gamerProfile.ProfileProperties;
        num |= 268435456L;
        if (this.ProfileProperties.ContainsKey(ProfileProperty.GamerTag))
          this.GamerTag = (string) this.ProfileProperties[ProfileProperty.GamerTag];
      }
      if (gamerProfile.RecentAchievements != null && gamerProfile.RecentAchievements.Count > 0)
      {
        this.RecentAchievements = gamerProfile.RecentAchievements;
        num |= 268435456L;
      }
      if (gamerProfile.PrivacySettings != null)
      {
        this.PrivacySettings = gamerProfile.PrivacySettings;
        num |= 268435456L;
      }
      if (gamerProfile.PresenceInfo != null)
      {
        this.PresenceInfo = gamerProfile.PresenceInfo;
        num |= 268435456L;
        if (!this.IsCurrentGamer && XboxLiveGamer.CurrentGamer != null && XboxLiveGamer.CurrentGamer.FriendsListData != null && (XboxLiveGamer.CurrentGamer.IsGamerTagLocalFriend(this.GamerTag) || XboxLiveGamer.CurrentGamer.IsGamerTagLocalPendingFriend(this.GamerTag) || XboxLiveGamer.CurrentGamer.IsGamerTagLocalRequestingFriend(this.GamerTag)))
        {
          FriendEntryData friendData = XboxLiveGamer.CurrentGamer.FriendsListData.GetFriendData(this.GamerTag);
          if (friendData != null)
          {
            friendData.IsOnline = this.IsGamerOnline;
            friendData.DetailedPresence = this.PresenceInfo.DetailedPresence;
            friendData.PresenceString = this.PresenceString;
            friendData.LastSeenTitleId = this.PresenceInfo.LastSeenTitleId;
            num |= 536870912L;
          }
        }
      }
      return num;
    }

    private void PopulateFriends(ProfileEx gamerProfile)
    {
      if (!this.IsCurrentGamer || gamerProfile.FriendList == null)
        return;
      FriendsListData newListData = new FriendsListData("FRI_" + this.GamerTag);
      List<XboxLiveGamer> xboxLiveGamerList = new List<XboxLiveGamer>(gamerProfile.FriendList.Capacity);
      foreach (Gds.Contracts.Friend friend1 in (List<Gds.Contracts.Friend>) gamerProfile.FriendList)
      {
        XboxLiveGamer xboxLiveGamer = XboxLiveGamer.GetXboxLiveGamer((string) friend1.ProfileEx.ProfileProperties[ProfileProperty.GamerTag], false);
        xboxLiveGamer.PopulateProfiles(friend1.ProfileEx);
        xboxLiveGamerList.Add(xboxLiveGamer);
        FriendEntryData friend2 = new FriendEntryData();
        friend2.GamerTag = xboxLiveGamer.GamerTag;
        friend2.IsOnline = xboxLiveGamer.IsGamerOnline;
        friend2.DetailedPresence = xboxLiveGamer.PresenceInfo.DetailedPresence;
        friend2.GamerPicUrl = (string) xboxLiveGamer.ProfileProperties[ProfileProperty.GamerPicUrl];
        friend2.FriendState = (FriendState) friend1.FriendState;
        friend2.PresenceString = xboxLiveGamer.PresenceString;
        friend2.LastSeenTitleId = xboxLiveGamer.PresenceInfo.LastSeenTitleId;
        if (friend2.FriendState == FriendState.Friend)
          newListData.AddToFriendsList(friend2);
        else if (FriendState.Requesting == friend2.FriendState)
          newListData.AddToPendingFriendsList(friend2);
        else if (FriendState.Pending == friend2.FriendState)
          newListData.AddToRequestingFriendsList(friend2);
      }
      newListData.RequestedFriendsList.Sort();
      newListData.PendingFriendsList.Sort();
      newListData.CreateOnlineOfflineFriends();
      newListData.LastUpdateTime = DateTime.UtcNow;
      newListData.IsLoadedFromWebService = true;
      if (this.IsModifyingFriendsListData)
        return;
      newListData.SyncWithLocalData(this.FriendsListData);
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.FriendsListData = newListData;
      }, (object) null);
      CacheManager.Instance.SaveAsync((CacheItem) newListData);
      foreach (CacheItem cacheItem in xboxLiveGamerList)
        CacheManager.Instance.SaveAsync(cacheItem);
    }

    private ProfileEx PopulateProfileForUpdate(
      long sectionFlags,
      XmlSerializableDictionary<ProfileProperty, object> profileUpdates,
      XmlSerializableDictionary<PrivacySetting, uint> privacyUpdates)
    {
      if (sectionFlags == 0L)
        throw new ArgumentException("Invalid sectionFlags");
      ProfileEx profileEx = new ProfileEx();
      profileEx.SectionFlags = sectionFlags;
      if ((sectionFlags & 1L) != 0L)
      {
        if (profileUpdates == null || profileUpdates.Count == 0)
          throw new ArgumentException("Invalid profileUpdates");
        profileEx.ProfileProperties = new XmlSerializableDictionary<ProfileProperty, object>();
        foreach (ProfileProperty key in this.ProfileProperties.Keys)
        {
          if (profileUpdates.ContainsKey(key))
            profileEx.ProfileProperties.Add(key, profileUpdates[key]);
          else
            profileEx.ProfileProperties.Add(key, this.ProfileProperties[key]);
        }
      }
      if ((sectionFlags & 64L) != 0L)
      {
        if (privacyUpdates == null || privacyUpdates.Count == 0)
          throw new ArgumentException("Invalid privacyUpdates");
        profileEx.PrivacySettings = new XmlSerializableDictionary<PrivacySetting, uint>();
        foreach (PrivacySetting key in this.PrivacySettings.Keys)
        {
          if (privacyUpdates.ContainsKey(key))
            profileEx.PrivacySettings.Add(key, privacyUpdates[key]);
          else
            profileEx.PrivacySettings.Add(key, this.PrivacySettings[key]);
        }
      }
      return profileEx;
    }

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler tmpEvent = this.PropertyChanged;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        if (tmpEvent == null)
          return;
        tmpEvent((object) this, new PropertyChangedEventArgs(propertyName));
      }, (object) this);
    }

    private void LoadGamerDataInternal(
      long sectionFlags,
      bool alwaysRefresh,
      uint gameId,
      uint pageNumber)
    {
      long sectionFlags1 = 33;
      if (this.IsCurrentGamer)
        sectionFlags1 |= 80L;
      if ((sectionFlags & 268435456L) != 0L)
        sectionFlags1 = 1L;
      if ((sectionFlags & 268435456L) != 0L || (sectionFlags & 268435456L) != 0L)
      {
        if (!this.IsLoadingProfile && (alwaysRefresh || this.ShouldExpireCache(ServiceCommon.GamerDataSections.Profile)))
        {
          this.IsLoadingProfile = true;
          this.profileServiceManager.GetProfileAsync(this.GamerTag, sectionFlags1);
        }
        else if (!this.IsLoadingProfile)
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
            if (gamerDataUpdated == null)
              return;
            gamerDataUpdated((object) this, new ServiceProxyEventArgs<long>((object) 268435456L, (Exception) null, false, (object) 268435456L));
          }, (object) this);
      }
      if ((sectionFlags & 536870912L) != 0L)
      {
        if (!this.IsLoadingFriends && (alwaysRefresh || this.ShouldExpireCache(ServiceCommon.GamerDataSections.Friends)))
        {
          this.IsLoadingFriends = true;
          this.profileServiceManagerForFriend.GetProfileAsync(this.GamerTag, 128L);
        }
        else if (!this.IsLoadingFriends)
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
            if (gamerDataUpdated == null)
              return;
            gamerDataUpdated((object) this, new ServiceProxyEventArgs<long>((object) 536870912L, (Exception) null, false, (object) 536870912L));
          }, (object) this);
      }
      if ((sectionFlags & 33554432L) != 0L)
      {
        if (!this.IsLoadingGames && (alwaysRefresh || this.ShouldExpireCache(ServiceCommon.GamerDataSections.Games)))
        {
          this.IsLoadingGames = true;
          this.gameDataServiceManager.GetGamesAsync(this.IsCurrentGamer ? (string) null : this.GamerTag, pageNumber);
        }
        else
        {
          int num1 = this.IsLoadingGames ? 1 : 0;
        }
      }
      if ((sectionFlags & 16777216L) != 0L)
      {
        if (!this.IsLoadingMessages && (alwaysRefresh || this.ShouldExpireCache(ServiceCommon.GamerDataSections.Messages)))
        {
          this.IsLoadingMessages = true;
          this.messageServiceManager.GetMessageListAsync(this.MessagesData == null ? (string) null : this.MessagesData.HashCode);
        }
        else if (!this.IsLoadingMessages)
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
            if (gamerDataUpdated == null)
              return;
            gamerDataUpdated((object) this, new ServiceProxyEventArgs<long>((object) 16777216L, (Exception) null, false, (object) 16777216L));
          }, (object) this);
      }
      if ((sectionFlags & 1073741824L) != 0L)
      {
        if (!this.IsLoadingAvatar && (alwaysRefresh || this.ShouldExpireCache(ServiceCommon.GamerDataSections.Avatar)))
        {
          this.IsLoadingAvatar = true;
          this.avatarServiceManager.GetManifestAsync(this.GamerTag);
        }
        else if (!this.IsLoadingAvatar)
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
            if (gamerDataUpdated == null)
              return;
            gamerDataUpdated((object) this, new ServiceProxyEventArgs<long>((object) 1073741824L, (Exception) null, false, (object) 1073741824L));
          }, (object) this);
      }
      if ((sectionFlags & 134217728L) != 0L)
      {
        if (!this.IsLoadingAvatarClosetAssets && (alwaysRefresh || this.ShouldExpireCache(ServiceCommon.GamerDataSections.AvatarClosetAssets)))
        {
          this.IsLoadingAvatarClosetAssets = true;
          this.avatarServiceManager.GetClosetAssetsAsync();
        }
        else
        {
          int num2 = this.IsLoadingAvatarClosetAssets ? 1 : 0;
        }
      }
      if ((sectionFlags & 67108864L) != 0L)
      {
        if (!this.IsLoadingGameAchievements && (alwaysRefresh || this.ShouldExpireCache(ServiceCommon.GamerDataSections.GameAchievements)))
        {
          this.IsLoadingGameAchievements = true;
          this.gameDataServiceManager.GetAchievementsAsync(this.GamerTag, gameId);
        }
        else if (!this.IsLoadingGameAchievements)
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            EventHandler<ServiceProxyEventArgs<long>> gamerDataUpdated = this.InternalEventGamerDataUpdated;
            if (gamerDataUpdated == null)
              return;
            gamerDataUpdated((object) this, new ServiceProxyEventArgs<long>((object) 67108864L, (Exception) null, false, (object) 67108864L));
          }, (object) this);
      }
      this.UpdateLoadingStatus();
    }

    private void UpdateLoadingStatus()
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        EventHandler<EventArgs> loadingStateUpdated = XboxLiveGamer.EventGamerLoadingStateUpdated;
        if (loadingStateUpdated == null)
          return;
        loadingStateUpdated((object) this, EventArgs.Empty);
      }, (object) this);
    }

    private void OnGamerStateUpdate(
      object sender,
      ServiceProxyEventArgs<XboxLiveGamerState.GamerStates> e)
    {
      if (!e.ResultAvailable || e.Result == XboxLiveGamerState.GamerStates.Offline)
      {
        XboxLiveGamer.CurrentGamer.FriendsState = 2;
        XboxLiveGamer.CurrentGamer.ProfileState = 2;
        XboxLiveGamer.CurrentGamer.MessagesState = 2;
        XboxLiveGamer.CurrentGamer.GamesState = 2;
      }
      EventHandler<ServiceProxyEventArgs<XboxLiveGamerState.GamerStates>> gamerStateUpdate = XboxLiveGamer.EventGamerStateUpdate;
      if (gamerStateUpdate == null)
        return;
      gamerStateUpdate((object) this, e);
    }

    private void ForceSignIn()
    {
      ThreadManager.UIThreadSend((SendOrPostCallback) delegate
      {
        if (XboxLiveGamer.IsOnline)
          return;
        XboxLiveGamer.GamerState.UpdateState(XboxLiveGamerState.GamerStateEvent.SignInStart);
        Guide.ShowSignIn(0, true);
      }, (object) this);
    }

    private void UpdatePresenceString()
    {
      if (this.PresenceInfo != null)
      {
        TimeSpan timeSpan = DateTime.UtcNow - this.PresenceInfo.LastSeenDateTime;
        if (!this.IsGamerOnline && this.PresenceInfo.LastSeenDateTime.ToLocalTime() < XboxLiveGamer.invalidLastSeenDateTime)
        {
          this.PresenceString = string.Empty;
          return;
        }
        if (this.IsGamerOnline)
        {
          if (!string.IsNullOrEmpty(this.PresenceInfo.LastSeenTitleName))
          {
            this.PresenceString = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resource.Playing, new object[1]
            {
              (object) this.PresenceInfo.LastSeenTitleName
            });
            return;
          }
          this.PresenceString = string.Empty;
          return;
        }
        if (string.IsNullOrEmpty(this.PresenceInfo.LastSeenTitleName))
        {
          if (timeSpan.TotalHours >= 24.0)
            this.PresenceString = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resource.LastSeen_Date, new object[1]
            {
              (object) this.PresenceInfo.LastSeenDateTime.ToLocalTime().ToShortDateString()
            });
          else if (timeSpan.TotalMinutes >= 60.0)
          {
            int totalHours = (int) timeSpan.TotalHours;
            if (totalHours == 1)
              this.PresenceString = Resource.LastSeen_Hour_Singular;
            else
              this.PresenceString = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resource.LastSeen_Hour, new object[1]
              {
                (object) totalHours
              });
          }
          else
          {
            int totalMinutes = (int) timeSpan.TotalMinutes;
            if (totalMinutes == 1)
              this.PresenceString = Resource.LastSeen_Minute_Singular;
            else if (totalMinutes < 1)
              this.PresenceString = Resource.LastSeen_LessThanOneMinute;
            else
              this.PresenceString = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resource.LastSeen_Minute, new object[1]
              {
                (object) totalMinutes
              });
          }
        }
        else if (timeSpan.TotalHours >= 24.0)
          this.PresenceString = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resource.LastSeenPlaying_Date, new object[2]
          {
            (object) this.PresenceInfo.LastSeenDateTime.ToLocalTime().ToShortDateString(),
            (object) this.PresenceInfo.LastSeenTitleName
          });
        else if (timeSpan.TotalMinutes >= 60.0)
        {
          int totalHours = (int) timeSpan.TotalHours;
          if (totalHours == 1)
            this.PresenceString = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resource.LastSeenPlaying_Hour_Singular, new object[1]
            {
              (object) this.PresenceInfo.LastSeenTitleName
            });
          else
            this.PresenceString = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resource.LastSeenPlaying_Hour, new object[2]
            {
              (object) totalHours,
              (object) this.PresenceInfo.LastSeenTitleName
            });
        }
        else
        {
          int totalMinutes = (int) timeSpan.TotalMinutes;
          if (totalMinutes == 1)
            this.PresenceString = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resource.LastSeenPlaying_Minute_Singular, new object[1]
            {
              (object) this.PresenceInfo.LastSeenTitleName
            });
          else if (totalMinutes < 1)
            this.PresenceString = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resource.LastSeenPlaying_LessThanOneMinute, new object[1]
            {
              (object) this.PresenceInfo.LastSeenTitleName
            });
          else
            this.PresenceString = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resource.LastSeenPlaying_Minute, new object[2]
            {
              (object) totalMinutes,
              (object) this.PresenceInfo.LastSeenTitleName
            });
        }
      }
      this.OnPropertyChanged("PresenceString");
      this.OnPropertyChanged("DetailedPresenceString");
    }

    private void ChangeAvatarManifest(LoadingObjectWrapper newAvatar)
    {
      ThreadManager.UIThread.AssertIsCurrentThread();
      if (newAvatar.LoadingState != LoadingObjectWrapperState.Loaded)
        return;
      this.AvatarManifest = newAvatar.GetValue();
      this.OnPropertyChanged("AvatarManifest");
      CacheManager.Instance.SaveAsync((CacheItem) this);
    }

    private delegate void OnlineUpdateDelegate();

    private delegate void OnlineUpdateFailDelegate(XLiveMobileException ex);

    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Makes for cleaner usage, since it only relates to XboxLiveGamer.")]
    public enum DataSource
    {
      None,
      Cache,
      WebService,
      Both,
    }

    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Makes for cleaner usage, since it only relates to XboxLiveGamer.")]
    public enum MessageCheckResult
    {
      NotCurrentGamer,
      Allowed,
      FriendsOnly,
      DisabledForPrivacy,
      DisabledForServiceLevel,
      DisabledForNoProfile,
    }
  }
}
