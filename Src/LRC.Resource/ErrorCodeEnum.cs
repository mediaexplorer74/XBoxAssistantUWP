// *********************************************************
// Type: LRC.Resources.ErrorCodeEnum
// Assembly: LRC.Resource, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9093DA41-95AA-481C-B970-06E86E67D53B
// *********************************************************LRC.Resource.dll


namespace LRC.Resources
{
  public enum ErrorCodeEnum
  {
    None = 0,
    Fatal = 1000, // 0x000003E8
    FailedToSignIn = 1001, // 0x000003E9
    FailedToConnectToService = 1002, // 0x000003EA
    FailedToConnectToConsole = 1003, // 0x000003EB
    FailedToAuthenticate = 1004, // 0x000003EC
    FailedCompanionSignIn = 1005, // 0x000003ED
    FailedToGetUserSession = 1006, // 0x000003EE
    FailedToConnectToSession = 1007, // 0x000003EF
    NoIPAddress = 1008, // 0x000003F0
    WrongIPAddressFormat = 1009, // 0x000003F1
    NoConsoleJoinEvent = 1010, // 0x000003F2
    FailedToJoinSession = 1011, // 0x000003F3
    FailedToFindSession = 1012, // 0x000003F4
    Error = 2000, // 0x000007D0
    ActionNotAllowed = 2001, // 0x000007D1
    FailedToGetUserProfile = 2002, // 0x000007D2
    FailedToGetFriendList = 2003, // 0x000007D3
    FailedToGetAchievements = 2004, // 0x000007D4
    FailedToSendCommand = 2005, // 0x000007D5
    FailedToGetLeaderboards = 2006, // 0x000007D6
    OperationTimeOut = 2007, // 0x000007D7
    RequestFailed = 2008, // 0x000007D8
    WillTerminateCurrentTitle = 2009, // 0x000007D9
    CanNotConnect = 2010, // 0x000007DA
    SessionFailure = 2011, // 0x000007DB
    FailedToLaunchGamercard = 2012, // 0x000007DC
    InvalidSocketOperation = 2013, // 0x000007DD
    InvalidTransportBufferSize = 2014, // 0x000007DE
    FailedToDeleteBeacon = 2015, // 0x000007DF
    FailedToSetBeacon = 2016, // 0x000007E0
    FailedToSetBeacon_BeaconTextTooLong = 2017, // 0x000007E1
    FailedToSetBeacon_BeaconLimitReached = 2018, // 0x000007E2
    TooManyClients = 2019, // 0x000007E3
    SocketTimeOut = 2020, // 0x000007E4
    RequestFailedByConsole = 2021, // 0x000007E5
    Info = 3000, // 0x00000BB8
    RequestCancelled = 3001, // 0x00000BB9
    FailedToRetrieveData = 3002, // 0x00000BBA
    FailedToEncryptMessage = 3003, // 0x00000BBB
    FailedToDecryptMessage = 3004, // 0x00000BBC
  }
}
