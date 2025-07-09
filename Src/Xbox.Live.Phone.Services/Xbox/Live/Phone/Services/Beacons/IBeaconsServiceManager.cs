// *********************************************************
// Type: Xbox.Live.Phone.Services.Beacons.IBeaconsServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;


namespace Xbox.Live.Phone.Services.Beacons
{
  public interface IBeaconsServiceManager
  {
    event EventHandler<ServiceProxyEventArgs<ActivityListForTitleData>> EventGetActivityListForTitleCompleted;

    event EventHandler<ServiceProxyEventArgs<BeaconDefaultTextData>> EventGetBeaconDefaultTextCompleted;

    event EventHandler<ServiceProxyEventArgs<BeaconInfo>> EventGetBeaconForTitleCompleted;

    event EventHandler<ServiceProxyEventArgs<string>> EventSetBeaconForTitleCompleted;

    event EventHandler<ServiceProxyEventArgs<string>> EventDeleteBeaconForTitleCompleted;

    void Initialize(ServiceCommon.Environment environmentName);

    void GetActivityListForTitle(uint gameId);

    void GetBeaconDefaultText(uint titleId);

    void GetBeaconForTitle(uint gameId);

    void SetBeaconForTitle(uint gameId, string beaconText);

    void DeleteBeaconForTitle(uint gameId);
  }
}
