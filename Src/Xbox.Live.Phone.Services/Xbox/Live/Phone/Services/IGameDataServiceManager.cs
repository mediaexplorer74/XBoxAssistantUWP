// *********************************************************
// Type: Xbox.Live.Phone.Services.IGameDataServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Gds.Contracts;
using System;


namespace Xbox.Live.Phone.Services
{
  public interface IGameDataServiceManager
  {
    event EventHandler<ServiceProxyEventArgs<Games>> EventGetGamesCompleted;

    event EventHandler<ServiceProxyEventArgs<Achievements>> EventGetAchievementsCompleted;

    void Initialize(ServiceCommon.Environment environmentName);

    void GetGamesAsync(string gamerTag, uint pageNumber);

    void GetAchievementsAsync(string gamerTag, uint gameId);

    bool IsLoading();
  }
}
