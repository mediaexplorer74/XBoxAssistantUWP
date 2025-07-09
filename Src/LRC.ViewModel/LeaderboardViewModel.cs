// *********************************************************
// Type: LRC.ViewModel.LeaderboardViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Services.Leaderboards;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class LeaderboardViewModel : ViewModelBase
  {
    private const string PerfGetLeaderboardDetails = "LeaderboardViewModel:GetLeaderboardDetails";
    private string scoreHeader;
    private ObservableCollection<LeaderboardEntryViewModel> entries;

    public LeaderboardViewModel() => this.LifetimeInMinutes = 5;

    public LeaderboardViewModel(uint titleId, LeaderboardInfo leaderboardInfo)
    {
      this.LifetimeInMinutes = 5;
      this.TitleId = titleId;
      if (leaderboardInfo == null)
        return;
      this.ScoreHeader = leaderboardInfo.RatingHeader;
      this.Title = leaderboardInfo.Name;
      this.LeaderboardId = leaderboardInfo.LeaderboardId;
      this.Entries = LeaderboardViewModel.CreateLeaderboardEntries(leaderboardInfo.LeaderboardEntries);
    }

    [DataMember]
    public uint TitleId { get; set; }

    [DataMember]
    public uint LeaderboardId { get; set; }

    [DataMember]
    public string ScoreHeader
    {
      get => this.scoreHeader;
      set => this.SetPropertyValue<string>(ref this.scoreHeader, value, nameof (ScoreHeader));
    }

    [DataMember]
    public ObservableCollection<LeaderboardEntryViewModel> Entries
    {
      get => this.entries;
      set
      {
        this.SetPropertyValue<ObservableCollection<LeaderboardEntryViewModel>>(ref this.entries, value, nameof (Entries));
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
      this.IsBusy = true;
      XboxLiveGamer.CurrentGamer.EventGetLeaderboardDetailsCompleted += new EventHandler<ServiceProxyEventArgs<LeaderboardInfo>>(this.OnGetLeaderboardDetailsCompleted);
      XboxLiveGamer.CurrentGamer.GetLeaderboardDetails(this.TitleId, this.LeaderboardId);
    }

    public void OnGetLeaderboardDetailsCompleted(
      object sender,
      ServiceProxyEventArgs<LeaderboardInfo> e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      XboxLiveGamer.CurrentGamer.EventGetLeaderboardDetailsCompleted -= new EventHandler<ServiceProxyEventArgs<LeaderboardInfo>>(this.OnGetLeaderboardDetailsCompleted);
      this.IsBusy = false;
      if (e.Error != null)
      {
        this.NotifyError(ErrorCodeEnum.FailedToGetLeaderboards);
      }
      else
      {
        this.LastRefreshTime = DateTime.UtcNow;
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          if (e.Result == null)
            return;
          ObservableCollection<LeaderboardEntryViewModel> leaderboardEntriesForView = LeaderboardViewModel.CreateLeaderboardEntries(e.Result.LeaderboardEntries);
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            this.Entries = leaderboardEntriesForView;
          }, (object) null);
        });
      }
    }

    private static ObservableCollection<LeaderboardEntryViewModel> CreateLeaderboardEntries(
      ObservableCollection<LeaderboardEntry> leaderboardEntries)
    {
      ObservableCollection<LeaderboardEntryViewModel> leaderboardEntries1 = new ObservableCollection<LeaderboardEntryViewModel>();
      if (leaderboardEntries != null)
      {
        foreach (LeaderboardEntry leaderboardEntry in (Collection<LeaderboardEntry>) leaderboardEntries)
          leaderboardEntries1.Add(new LeaderboardEntryViewModel(leaderboardEntry));
      }
      return leaderboardEntries1;
    }
  }
}
