// *********************************************************
// Type: Xbox.Live.Phone.Services.IProfileServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Gds.Contracts;
using System;


namespace Xbox.Live.Phone.Services
{
  public interface IProfileServiceManager
  {
    event EventHandler<ServiceProxyEventArgs<ProfileEx>> EventGetProfileCompleted;

    event EventHandler<ServiceProxyEventArgs<string>> EventUpdateProfileCompleted;

    event EventHandler<ServiceProxyEventArgs<string[]>> EventChangeGamerTagCompleted;

    event EventHandler<ServiceProxyEventArgs<string>> EventUpdatePresenceCompleted;

    void Initialize(ServiceCommon.Environment environmentName);

    void GetProfileAsync(string gamerTag, long sectionFlags);

    void UpdateProfileAsync(ProfileEx profileUpdates);

    void ChangeGamerTag(string desiredGamerTag);

    void UpdatePresence();

    bool IsLoading();
  }
}
