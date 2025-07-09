// *********************************************************
// Type: Xbox.Live.Phone.Services.ServiceManagerFactory
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Microsoft.Devices;
using Xbox.Live.Phone.Services.Beacons;
using Xbox.Live.Phone.Services.Facebook;
using Xbox.Live.Phone.Services.Leaderboards;
using Xbox.Live.Phone.Services.Programming;
using Xbox.Live.Phone.Services.TitleHistory;
using Xbox.Live.Phone.Services.UserClaims;


namespace Xbox.Live.Phone.Services
{
  public static class ServiceManagerFactory
  {
    public static bool IsRunningEmulator => 1 == Environment.DeviceType;

    public static IAvatarServiceManager CreateAvatarServiceManager()
    {
      return (IAvatarServiceManager) new AvatarServiceManager();
    }

    public static IProfileServiceManager CreateProfileServiceManager()
    {
      return (IProfileServiceManager) new ProfileServiceManager();
    }

    public static IFriendServiceManager CreateFriendServiceManager()
    {
      return (IFriendServiceManager) new FriendServiceManager();
    }

    public static IGameDataServiceManager CreateGameDataServiceManager()
    {
      return (IGameDataServiceManager) new GameDataServiceManager();
    }

    public static IMessageServiceManager CreateMessageServiceManager()
    {
      return (IMessageServiceManager) new MessageServiceManager();
    }

    public static IMarketplaceServiceManager CreateMarketplaceServiceManager()
    {
      return (IMarketplaceServiceManager) new MarketplaceServiceManager();
    }

    public static IProgrammingServiceManager CreateProgrammingServiceManager()
    {
      return (IProgrammingServiceManager) new ProgramingServiceManager();
    }

    public static IUserClaimsServiceManager CreateUserClaimsServiceManager()
    {
      return (IUserClaimsServiceManager) new UserClaimsServiceManager();
    }

    public static IBeaconsServiceManager CreateBeaconsServiceManager()
    {
      return (IBeaconsServiceManager) new BeaconsServiceManager();
    }

    public static ILeaderboardsServiceManager CreateLeaderboardsServiceManager()
    {
      return (ILeaderboardsServiceManager) new LeaderboardsServiceManager();
    }

    public static IFacebookServiceManager CreateFacebookServiceManager()
    {
      return (IFacebookServiceManager) new FacebookServiceManager();
    }

    public static ITitleHistoryServiceManager CreateTitleHistoryServiceManager()
    {
      return (ITitleHistoryServiceManager) new TitleHistoryServiceManager();
    }
  }
}
