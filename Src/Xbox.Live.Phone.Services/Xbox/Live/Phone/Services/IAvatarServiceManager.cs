// *********************************************************
// Type: Xbox.Live.Phone.Services.IAvatarServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Avatar.Services.ManifestRead.Library;
using Avatar.Services.ManifestWrite.Library;
using System;


namespace Xbox.Live.Phone.Services
{
  public interface IAvatarServiceManager
  {
    event EventHandler<ServiceProxyEventArgs<AvatarManifests>> EventGetManifestCompleted;

    event EventHandler<ServiceProxyEventArgs<UpdateManifestResponse>> EventUpdateManifestCompleted;

    event EventHandler<ServiceProxyEventArgs<ClosetAssetsHolder>> EventGetClosetAssetsCompleted;

    event EventHandler<ServiceProxyEventArgs<string>> EventSetGamerpicCompleted;

    void Initialize(ServiceCommon.Environment environmentName);

    void SetGamerpicAsync(GamerpicParameters gamerpicParams);

    void GetManifestAsync(string gamerTag);

    void UpdateManifestAsync(string manifest);

    void GetClosetAssetsAsync();

    bool IsLoading();
  }
}
