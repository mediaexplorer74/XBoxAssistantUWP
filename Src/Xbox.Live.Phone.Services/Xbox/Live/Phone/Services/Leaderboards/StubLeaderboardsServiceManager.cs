// *********************************************************
// Type: Xbox.Live.Phone.Services.Leaderboards.StubLeaderboardsServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services.Leaderboards
{
  public sealed class StubLeaderboardsServiceManager : ILeaderboardsServiceManager
  {
    public event EventHandler<ServiceProxyEventArgs<LeaderboardsForTitleData>> EventGetLeaderboardsForTitleCompleted;

    public event EventHandler<ServiceProxyEventArgs<LeaderboardInfo>> EventGetLeaderboardDetailsCompleted;

    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "environmentName", Justification = "This is stub")]
    public void Initialize(ServiceCommon.Environment environmentName)
    {
    }

    public void GetLeaderboardsForTitle(uint gameId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        LeaderboardsForTitleData leaderboardsForTitleData = LeaderboardsResponseParser.ParseGetLeaderboardsForTitleResponse(ServiceCommon.ReadResource("Xbox.Live.Phone.Services.StubData.LeaderboardsForTitle.txt"));
        leaderboardsForTitleData.TitleId = gameId;
        Thread.Sleep(2000);
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          EventHandler<ServiceProxyEventArgs<LeaderboardsForTitleData>> forTitleCompleted = this.EventGetLeaderboardsForTitleCompleted;
          if (forTitleCompleted == null)
            return;
          forTitleCompleted((object) this, new ServiceProxyEventArgs<LeaderboardsForTitleData>((object) leaderboardsForTitleData, (Exception) null, false, (object) null));
        }, (object) this);
      });
    }

    public void GetLeaderboardDetails(uint gameId, uint leaderboardId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        LeaderboardInfo leaderboardInfo = LeaderboardsResponseParser.ParseGetLeaderboardDetailsResponse(ServiceCommon.ReadResource("Xbox.Live.Phone.Services.StubData.LeaderboardDetails.txt"));
        leaderboardInfo.LeaderboardId = leaderboardId;
        leaderboardInfo.TitleId = gameId;
        Thread.Sleep(2000);
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          EventHandler<ServiceProxyEventArgs<LeaderboardInfo>> detailsCompleted = this.EventGetLeaderboardDetailsCompleted;
          if (detailsCompleted == null)
            return;
          detailsCompleted((object) this, new ServiceProxyEventArgs<LeaderboardInfo>((object) leaderboardInfo, (Exception) null, false, (object) null));
        }, (object) this);
      });
    }
  }
}
