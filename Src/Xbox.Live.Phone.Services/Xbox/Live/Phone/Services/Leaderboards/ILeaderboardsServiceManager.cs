// *********************************************************
// Type: Xbox.Live.Phone.Services.Leaderboards.ILeaderboardsServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;


namespace Xbox.Live.Phone.Services.Leaderboards
{
  public interface ILeaderboardsServiceManager
  {
    event EventHandler<ServiceProxyEventArgs<LeaderboardsForTitleData>> EventGetLeaderboardsForTitleCompleted;

    event EventHandler<ServiceProxyEventArgs<LeaderboardInfo>> EventGetLeaderboardDetailsCompleted;

    void Initialize(ServiceCommon.Environment environmentName);

    void GetLeaderboardsForTitle(uint gameId);

    void GetLeaderboardDetails(uint gameId, uint leaderboardId);
  }
}
