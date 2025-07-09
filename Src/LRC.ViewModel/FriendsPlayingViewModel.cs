// *********************************************************
// Type: LRC.ViewModel.FriendsPlayingViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Services.Beacons;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class FriendsPlayingViewModel : ViewModelBase
  {
    private const string PerfGetActivityListForTitle = "FriendsPlayingViewModel:GetActivityListForTitle";
    private ObservableCollection<FriendViewModel> friendsList;
    private string friendsWithBeaconsText;
    private string friendsPlayingNowText;

    public FriendsPlayingViewModel() => this.LifetimeInMinutes = 5;

    [DataMember]
    public uint TitleId { get; set; }

    [IgnoreDataMember]
    public bool IsFriendListEmpty => this.friendsList == null || this.friendsList.Count == 0;

    [IgnoreDataMember]
    public bool HasFriends => this.friendsList != null && this.friendsList.Count > 0;

    [DataMember]
    public ObservableCollection<FriendViewModel> FriendsList
    {
      get => this.friendsList;
      set
      {
        this.SetPropertyValue<ObservableCollection<FriendViewModel>>(ref this.friendsList, value, nameof (FriendsList));
        if (this.IsFriendListEmpty)
          this.CurrentState = 2;
        else
          this.CurrentState = 0;
        this.NotifyPropertyChanged("CurrentState");
        this.NotifyPropertyChanged("IsFriendListEmpty");
        this.NotifyPropertyChanged("HasFriends");
      }
    }

    [DataMember]
    public string FriendsWithBeaconsText
    {
      get => this.friendsWithBeaconsText;
      set
      {
        this.SetPropertyValue<string>(ref this.friendsWithBeaconsText, value, nameof (FriendsWithBeaconsText));
      }
    }

    [DataMember]
    public string FriendsPlayingNowText
    {
      get => this.friendsPlayingNowText;
      set
      {
        this.SetPropertyValue<string>(ref this.friendsPlayingNowText, value, nameof (FriendsPlayingNowText));
      }
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      this.RefreshData();
    }

    public void RefreshData()
    {
      if (this.CurrentState == 3)
        return;
      this.CurrentState = 3;
      if (XboxLiveGamer.CurrentGamer.XboxUserId == 0UL)
      {
        this.IsBusy = true;
        XboxLiveGamer.CurrentGamer.EventLoadedXboxUserId += new EventHandler<EventArgs>(this.LoadXboxUserIdCompleted);
        XboxLiveGamer.LoadXboxUserId();
      }
      else
        this.InternalRefreshData();
    }

    public void OnActivityListForTitleUpdated(
      object sender,
      ServiceProxyEventArgs<ActivityListForTitleData> e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      XboxLiveGamer.CurrentGamer.EventGetActivityListForTitleCompleted -= new EventHandler<ServiceProxyEventArgs<ActivityListForTitleData>>(this.OnActivityListForTitleUpdated);
      this.IsBusy = false;
      if (e.Error != null)
      {
        this.CurrentState = 1;
        this.NotifyError(ErrorCodeEnum.FailedToGetFriendList);
      }
      else
      {
        this.LastRefreshTime = DateTime.UtcNow;
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          ObservableCollection<FriendViewModel> friendActivityThisGame = new ObservableCollection<FriendViewModel>();
          int friendsPlayingNow = 0;
          int friendsWithBeacons = 0;
          if (e.Result != null && e.Result.FriendActivityList != null)
          {
            foreach (Friend friendActivity in (Collection<Friend>) e.Result.FriendActivityList)
            {
              if (friendActivity != null)
              {
                friendActivityThisGame.Add(new FriendViewModel(friendActivity, this.TitleId));
                if (friendActivity.IsOnline && friendActivity.PresenceTitleId.HasValue)
                {
                  uint? presenceTitleId = friendActivity.PresenceTitleId;
                  uint titleId = this.TitleId;
                  if (((int) presenceTitleId.GetValueOrDefault() != (int) titleId ? 0 : (presenceTitleId.HasValue ? 1 : 0)) != 0)
                    ++friendsPlayingNow;
                }
                if (friendActivity.Beacon != null)
                  ++friendsWithBeacons;
              }
            }
          }
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            this.FriendsList = friendActivityThisGame;
            if (1 == friendsPlayingNow)
              this.FriendsPlayingNowText = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Beacon_OneFriendPlayingNow);
            else
              this.FriendsPlayingNowText = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Beacon_FriendsPlayingNow, new object[1]
              {
                (object) friendsPlayingNow
              });
            if (1 == friendsWithBeacons)
              this.FriendsWithBeaconsText = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Beacon_OneFriendWithBeacon);
            else
              this.FriendsWithBeaconsText = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Beacon_FriendsWithBeacons, new object[1]
              {
                (object) friendsWithBeacons
              });
          }, (object) null);
        });
      }
    }

    private void LoadXboxUserIdCompleted(object sender, EventArgs e)
    {
      this.IsBusy = false;
      XboxLiveGamer.CurrentGamer.EventLoadedXboxUserId -= new EventHandler<EventArgs>(this.LoadXboxUserIdCompleted);
      this.InternalRefreshData();
    }

    private void InternalRefreshData()
    {
      if (XboxLiveGamer.CurrentGamer.XboxUserId == 0UL)
      {
        this.NotifyError(ErrorCodeEnum.FailedToGetFriendList);
      }
      else
      {
        this.IsBusy = true;
        XboxLiveGamer.CurrentGamer.EventGetActivityListForTitleCompleted += new EventHandler<ServiceProxyEventArgs<ActivityListForTitleData>>(this.OnActivityListForTitleUpdated);
        XboxLiveGamer.CurrentGamer.GetActivityListForTitle(this.TitleId);
      }
    }
  }
}
