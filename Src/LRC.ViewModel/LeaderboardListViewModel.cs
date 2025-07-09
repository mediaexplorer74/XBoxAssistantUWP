// *********************************************************
// Type: LRC.ViewModel.LeaderboardListViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Services.Leaderboards;
using Xbox.Live.Phone.Utils;
using Xbox.Live.Phone.Utils.Serialization;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class LeaderboardListViewModel : ViewModelBase
  {
    private const string PerfLoadXboxUserId = "LeaderboardListViewModel:LoadXboxUserId";
    private const string PerfGetLeaderboardsForTitle = "LeaderboardListViewModel:GetLeaderboardsForTitle";
    private const string PerfRefreshData = "LeaderboardListViewModel:RefreshData";
    private XmlSerializableDictionary<uint, LeaderboardViewModel> leaderboardCollection;
    private uint primaryLeaderboardId;

    public LeaderboardListViewModel() => this.LifetimeInMinutes = int.MaxValue;

    [DataMember]
    public uint TitleId { get; set; }

    [DataMember]
    public uint PrimaryLeaderboardId
    {
      get => this.primaryLeaderboardId;
      set
      {
        this.SetPropertyValue<uint>(ref this.primaryLeaderboardId, value, nameof (PrimaryLeaderboardId));
        this.NotifyPropertyChanged("PrimaryLeaderboard");
        this.NotifyPropertyChanged("HasNoPrimaryLeaderboard");
      }
    }

    [IgnoreDataMember]
    public bool HasNoPrimaryLeaderboard => this.PrimaryLeaderboard == null;

    [IgnoreDataMember]
    public LeaderboardViewModel PrimaryLeaderboard
    {
      get
      {
        return this.LeaderboardCollection == null || !this.LeaderboardCollection.ContainsKey(this.PrimaryLeaderboardId) ? (LeaderboardViewModel) null : this.LeaderboardCollection[this.PrimaryLeaderboardId];
      }
    }

    [IgnoreDataMember]
    [XmlIgnore]
    public XmlSerializableDictionary<uint, LeaderboardViewModel> LeaderboardCollection
    {
      get => this.leaderboardCollection;
      set
      {
        this.SetPropertyValue<XmlSerializableDictionary<uint, LeaderboardViewModel>>(ref this.leaderboardCollection, value, nameof (LeaderboardCollection));
        this.NotifyPropertyChanged("PrimaryLeaderboard");
        this.NotifyPropertyChanged("HasNoPrimaryLeaderboard");
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
      if (XboxLiveGamer.CurrentGamer.XboxUserId == 0UL)
      {
        this.IsBusy = true;
        XboxLiveGamer.CurrentGamer.EventLoadedXboxUserId += new EventHandler<EventArgs>(this.LoadXboxUserIdCompleted);
        XboxLiveGamer.LoadXboxUserId();
      }
      else
        this.InternalRefreshData();
    }

    public void OnGetLeaderboardsForTitleCompleted(
      object sender,
      ServiceProxyEventArgs<LeaderboardsForTitleData> e)
    {
      XboxLiveGamer.CurrentGamer.EventGetLeaderboardsForTitleCompleted -= new EventHandler<ServiceProxyEventArgs<LeaderboardsForTitleData>>(this.OnGetLeaderboardsForTitleCompleted);
      this.IsBusy = false;
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      if (e.Error != null)
      {
        this.NotifyError(ErrorCodeEnum.FailedToGetLeaderboards);
      }
      else
      {
        this.LastRefreshTime = DateTime.UtcNow;
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          if (e.Result == null || e.Result.Leaderboards == null)
            return;
          XmlSerializableDictionary<uint, LeaderboardViewModel> leaderboards = new XmlSerializableDictionary<uint, LeaderboardViewModel>();
          foreach (uint key in e.Result.Leaderboards.Keys)
            leaderboards.Add(key, new LeaderboardViewModel(this.TitleId, e.Result.Leaderboards[key]));
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            this.PrimaryLeaderboardId = e.Result.PrimaryLeaderboardId;
            this.LeaderboardCollection = leaderboards;
            this.RefreshPrimaryLeaderboardData();
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
        this.NotifyError(ErrorCodeEnum.FailedToGetLeaderboards);
      else if (this.LeaderboardCollection == null || this.PrimaryLeaderboard == null)
      {
        this.IsBusy = true;
        XboxLiveGamer.CurrentGamer.EventGetLeaderboardsForTitleCompleted += new EventHandler<ServiceProxyEventArgs<LeaderboardsForTitleData>>(this.OnGetLeaderboardsForTitleCompleted);
        XboxLiveGamer.CurrentGamer.GetLeaderboardsForTitle(this.TitleId);
      }
      else
      {
        if (this.PrimaryLeaderboard == null)
          return;
        this.RefreshPrimaryLeaderboardData();
      }
    }

    private void RefreshPrimaryLeaderboardData()
    {
      if (this.PrimaryLeaderboard == null)
        return;
      this.IsBusy = true;
      XboxLiveGamer.CurrentGamer.EventGetLeaderboardDetailsCompleted += new EventHandler<ServiceProxyEventArgs<LeaderboardInfo>>(this.OnRefreshPrimaryLeaderboardDataCompleted);
      this.PrimaryLeaderboard.RefreshData();
    }

    private void OnRefreshPrimaryLeaderboardDataCompleted(
      object sender,
      ServiceProxyEventArgs<LeaderboardInfo> e)
    {
      XboxLiveGamer.CurrentGamer.EventGetLeaderboardDetailsCompleted -= new EventHandler<ServiceProxyEventArgs<LeaderboardInfo>>(this.OnRefreshPrimaryLeaderboardDataCompleted);
      this.IsBusy = false;
    }
  }
}
