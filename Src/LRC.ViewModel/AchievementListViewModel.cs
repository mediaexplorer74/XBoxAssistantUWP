// *********************************************************
// Type: LRC.ViewModel.AchievementListViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using Leet.UserGameData.DataContracts;
using LRC.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Threading;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  public class AchievementListViewModel : ViewModelBase
  {
    private const string ComponentName = "AchievementListViewModel";
    private const string PerfLoadGamerData = "AchievementListViewModel:LoadGamerData";
    private const int LoadDelayInMilliseconds = 500;
    private const int AchievementsPerLoad = 10;
    private List<AchievementViewModel> pendingAchievementList;
    private ObservableCollection<AchievementViewModel> achievementList;
    private bool isProcessingAchievements;
    private string achievementProgressText;
    private string gamerscoreText;
    private bool errorLoadingAchievements;

    public AchievementListViewModel() => this.LifetimeInMinutes = 5;

    [DataMember]
    public uint TitleId { get; set; }

    [DataMember]
    public string GameName { get; set; }

    [DataMember]
    public ObservableCollection<AchievementViewModel> AchievementList
    {
      get => this.achievementList;
      set
      {
        this.SetPropertyValue<ObservableCollection<AchievementViewModel>>(ref this.achievementList, value, nameof (AchievementList));
        if (this.HasNoAchievements)
          this.CurrentState = 2;
        else
          this.CurrentState = 0;
        this.NotifyPropertyChanged("CurrentState");
        this.NotifyPropertyChanged("HasNoAchievements");
        this.NotifyPropertyChanged("HasAchievements");
      }
    }

    [DataMember]
    public string GamerscoreText
    {
      get => this.gamerscoreText;
      set => this.SetPropertyValue<string>(ref this.gamerscoreText, value, nameof (GamerscoreText));
    }

    public bool IsProcessingAchievements
    {
      get => this.isProcessingAchievements;
      set
      {
        this.SetPropertyValue<bool>(ref this.isProcessingAchievements, value, nameof (IsProcessingAchievements));
      }
    }

    [DataMember]
    public string AchievementProgressText
    {
      get => this.achievementProgressText;
      set
      {
        this.SetPropertyValue<string>(ref this.achievementProgressText, value, nameof (AchievementProgressText));
      }
    }

    [DataMember]
    public bool HasNoAchievements
    {
      get
      {
        if (this.ErrorLoadingAchievements)
          return false;
        return this.AchievementList == null || this.AchievementList.Count == 0;
      }
    }

    [DataMember]
    public bool HasAchievements
    {
      get
      {
        return !this.ErrorLoadingAchievements && this.AchievementList != null && this.AchievementList.Count != 0;
      }
    }

    [DataMember]
    public bool ErrorLoadingAchievements
    {
      get => this.errorLoadingAchievements;
      set
      {
        if (value && this.AchievementList != null)
          return;
        this.SetPropertyValue<bool>(ref this.errorLoadingAchievements, value, nameof (ErrorLoadingAchievements));
        this.NotifyPropertyChanged("HasNoAchievements");
        this.NotifyPropertyChanged("HasAchievements");
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
      this.IsBusy = true;
      this.CurrentState = 3;
      XboxLiveGamer.CurrentGamer.EventGamerDataUpdated += new EventHandler<ServiceProxyEventArgs<long>>(this.OnAchievementsUpdated);
      XboxLiveGamer.CurrentGamer.LoadGamerData(67108864L, XboxLiveGamer.DataSource.WebService, this.TitleId, 0U);
    }

    public void OnAchievementsUpdated(object sender, ServiceProxyEventArgs<long> e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      if (((long) e.UserState & 67108864L) == 0L)
        return;
      XboxLiveGamer.CurrentGamer.EventGamerDataUpdated -= new EventHandler<ServiceProxyEventArgs<long>>(this.OnAchievementsUpdated);
      this.IsBusy = false;
      this.LastRefreshTime = DateTime.UtcNow;
      if (e.Error != null)
      {
        this.NotifyError(ErrorCodeEnum.FailedToGetAchievements);
        this.ErrorLoadingAchievements = true;
        this.CurrentState = 1;
      }
      else
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          List<AchievementViewModel> achievementViewModelList = new List<AchievementViewModel>();
          int earnedAchievements = 0;
          int totalAchievements = 0;
          int earnedGamerscore = 0;
          int totalGamerscore = 0;
          if (XboxLiveGamer.CurrentGamer.AchievementsPerGame.Achievements.ContainsKey(this.TitleId))
          {
            List<Achievement> achievementList = XboxLiveGamer.CurrentGamer.AchievementsPerGame.Achievements[this.TitleId].AchievementList;
            if (achievementList != null)
            {
              foreach (Achievement achievement in achievementList)
              {
                if (achievement.IsEarned)
                {
                  ++earnedAchievements;
                  earnedGamerscore += achievement.Gamerscore;
                }
                ++totalAchievements;
                totalGamerscore += achievement.Gamerscore;
                achievement.GameName = this.GameName;
                achievementViewModelList.Add(new AchievementViewModel(achievement));
              }
            }
          }
          this.pendingAchievementList = achievementViewModelList;
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            this.ErrorLoadingAchievements = false;
            if (this.pendingAchievementList.Count == 0)
              this.AchievementList = new ObservableCollection<AchievementViewModel>();
            else
              this.ProcessPendingAchievements(false);
            this.AchievementProgressText = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Achievements_ProgressText, new object[2]
            {
              (object) earnedAchievements,
              (object) totalAchievements
            });
            this.GamerscoreText = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Achievements_GamerscoreText, new object[2]
            {
              (object) earnedGamerscore,
              (object) totalGamerscore
            });
          }, (object) null);
        });
    }

    public void ProcessPendingAchievements(bool isLoadingMore)
    {
      int milliseconds = isLoadingMore ? 500 : 0;
      if (this.pendingAchievementList.Count == 0)
        return;
      if (isLoadingMore)
        this.IsProcessingAchievements = true;
      DispatcherTimer timer = new DispatcherTimer();
      timer.Interval = new TimeSpan(0, 0, 0, 0, milliseconds);
      timer.Tick += (EventHandler) ((s, e1) =>
      {
        timer.Stop();
        timer = (DispatcherTimer) null;
        this.IsProcessingAchievements = false;
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          ObservableCollection<AchievementViewModel> observableCollection = this.AchievementList;
          if (!isLoadingMore || observableCollection == null)
            observableCollection = new ObservableCollection<AchievementViewModel>();
          for (int index = 0; index < 10 && this.pendingAchievementList.Count > 0; ++index)
          {
            observableCollection.Add(this.pendingAchievementList[0]);
            this.pendingAchievementList.RemoveAt(0);
          }
          this.AchievementList = observableCollection;
          this.IsProcessingAchievements = false;
        }, (object) null);
      });
      timer.Start();
    }
  }
}
