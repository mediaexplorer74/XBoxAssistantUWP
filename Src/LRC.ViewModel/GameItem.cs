// *********************************************************
// Type: LRC.ViewModel.GameItem
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
using LRC.Service.Search;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class GameItem : ViewModelBase
  {
    private const string ComponentName = "GameItem";
    private const string PerfGetGameDetailsByMediaId = "GameItem:GetGameDetailsByMediaId";
    private const string PerfGetGameDetailsByTitleId = "GameItem:GetGameDetailsByTitleId";
    private const string PerfGetGameDetails = "GameItem:PerfGetGameDetails";
    private AchievementListViewModel achievementListViewModel;
    private RelatedItemsViewModel relatedItemsViewModel;
    private FriendsPlayingViewModel friendsPlaying;
    private LeaderboardListViewModel leaderboardsViewModel;
    private SearchItemViewModel searchItemViewModel;
    private uint titleId;
    private string id;

    public GameItem(string mediaId)
      : this()
    {
      this.SearchItemViewModel = new SearchItemViewModel(mediaId, "XBOXGAMEITEM");
      this.Id = mediaId;
    }

    public GameItem(uint titleId)
      : this()
    {
      this.SearchItemViewModel = new SearchItemViewModel(titleId, "XBOXGAMEITEM");
      this.TitleId = titleId;
    }

    public GameItem()
    {
      this.LifetimeInMinutes = 60;
      this.AchievementListViewModel = new AchievementListViewModel();
      this.FriendsPlaying = new FriendsPlayingViewModel();
      this.LeaderboardsViewModel = new LeaderboardListViewModel();
      this.RelatedItemsViewModel = new RelatedItemsViewModel(true);
      this.RelatedItemsViewModel.ItemType = "XBOX360GAME";
    }

    public override event EventHandler<LRCAsyncCompletedEventArgs> EventOnAsyncOperationError
    {
      add
      {
        this.InternalEventOnAsyncOperationError -= value;
        this.InternalEventOnAsyncOperationError += value;
        this.AchievementListViewModel.EventOnAsyncOperationError += value;
        this.FriendsPlaying.EventOnAsyncOperationError += value;
        this.SearchItemViewModel.EventOnAsyncOperationError += value;
        this.LeaderboardsViewModel.EventOnAsyncOperationError += value;
      }
      remove
      {
        this.InternalEventOnAsyncOperationError -= value;
        this.AchievementListViewModel.EventOnAsyncOperationError -= value;
        this.FriendsPlaying.EventOnAsyncOperationError -= value;
        this.SearchItemViewModel.EventOnAsyncOperationError -= value;
        this.LeaderboardsViewModel.EventOnAsyncOperationError -= value;
      }
    }

    [DataMember]
    public uint TitleId
    {
      get => this.titleId;
      set
      {
        this.titleId = value;
        this.AchievementListViewModel.TitleId = this.titleId;
        this.FriendsPlaying.TitleId = this.titleId;
        this.LeaderboardsViewModel.TitleId = this.titleId;
      }
    }

    [DataMember]
    public string Id
    {
      get => this.id;
      set
      {
        this.SetPropertyValue<string>(ref this.id, value, nameof (Id));
        this.RelatedItemsViewModel.Id = value;
      }
    }

    [DataMember]
    public AchievementListViewModel AchievementListViewModel
    {
      get => this.achievementListViewModel;
      set
      {
        this.SetPropertyValue<AchievementListViewModel>(ref this.achievementListViewModel, value, nameof (AchievementListViewModel));
      }
    }

    [DataMember]
    public RelatedItemsViewModel RelatedItemsViewModel
    {
      get => this.relatedItemsViewModel;
      set
      {
        this.SetPropertyValue<RelatedItemsViewModel>(ref this.relatedItemsViewModel, value, nameof (RelatedItemsViewModel));
      }
    }

    [DataMember]
    public FriendsPlayingViewModel FriendsPlaying
    {
      get => this.friendsPlaying;
      set
      {
        this.SetPropertyValue<FriendsPlayingViewModel>(ref this.friendsPlaying, value, nameof (FriendsPlaying));
      }
    }

    [DataMember]
    public LeaderboardListViewModel LeaderboardsViewModel
    {
      get => this.leaderboardsViewModel;
      set
      {
        this.SetPropertyValue<LeaderboardListViewModel>(ref this.leaderboardsViewModel, value, nameof (LeaderboardsViewModel));
      }
    }

    [DataMember]
    public SearchItemViewModel SearchItemViewModel
    {
      get => this.searchItemViewModel;
      set
      {
        this.SetPropertyValue<SearchItemViewModel>(ref this.searchItemViewModel, value, nameof (SearchItemViewModel));
      }
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      ISearchServiceManager searchServiceManager = ServiceManagerFactory.CreateSearchServiceManager();
      this.IsBusy = true;
      this.CurrentState = 3;
      searchServiceManager.EventGetGameDetailsCompleted += new EventHandler<ServiceProxyEventArgs<XboxGameData>>(this.OnLoadingGameDetailsCompleted);
      if (!string.IsNullOrEmpty(this.Id))
        searchServiceManager.GetGameDetailsByMediaId(this.Id, (object) null);
      else
        searchServiceManager.GetGameDetailsByTitleId(this.TitleId, (object) null);
    }

    public void LaunchGamercard(string gamertag)
    {
      try
      {
        Guide.ShowGamerCard(PlayerIndex.One, Gamer.GetFromGamertag(gamertag));
      }
      catch (Exception ex)
      {
        switch (ex)
        {
          case GamerPrivilegeException _:
            this.NotifyError(ErrorCodeEnum.FailedToLaunchGamercard);
            break;
          case InvalidOperationException _:
            this.NotifyError(ErrorCodeEnum.FailedToLaunchGamercard);
            break;
          default:
            throw;
        }
      }
    }

    private void OnLoadingGameDetailsCompleted(object sender, ServiceProxyEventArgs<XboxGameData> e)
    {
      this.IsBusy = false;
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetGameDetailsCompleted -= new EventHandler<ServiceProxyEventArgs<XboxGameData>>(this.OnLoadingGameDetailsCompleted);
      if (e.Error != null || e.Result == null)
      {
        this.NotifyError(ErrorCodeEnum.FailedToRetrieveData);
        this.CurrentState = 1;
        this.AchievementListViewModel.CurrentState = 1;
        this.FriendsPlaying.CurrentState = 1;
      }
      else
      {
        this.LastRefreshTime = DateTime.UtcNow;
        this.CurrentState = 0;
        this.SearchItemViewModel.Initialize((SearchData) e.Result);
        this.Id = this.SearchItemViewModel.Id;
        this.TitleId = this.SearchItemViewModel.TitleId;
        this.Title = this.SearchItemViewModel.Title;
        this.AchievementListViewModel.GameName = this.SearchItemViewModel.Title;
        this.AchievementListViewModel.Load();
        this.FriendsPlaying.Load();
        this.RelatedItemsViewModel.Load();
      }
    }
  }
}
