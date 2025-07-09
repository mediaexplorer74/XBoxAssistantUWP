// *********************************************************
// Type: Xbox.Live.Phone.Utils.XLiveMobileExceptionEnum
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll


namespace Xbox.Live.Phone.Utils
{
  public enum XLiveMobileExceptionEnum
  {
    None = 0,
    CriticalFailure = 1000, // 0x000003E8
    FailedToConnect = 1001, // 0x000003E9
    OperationTimedOut = 1002, // 0x000003EA
    OperationNeedToWaitSignIn = 1003, // 0x000003EB
    OperationUnavailableOffline = 1004, // 0x000003EC
    FailedToSignIn = 2000, // 0x000007D0
    FailedToGetProfile = 3000, // 0x00000BB8
    FailedToUpdateProfile = 3001, // 0x00000BB9
    FailedToGetFriends = 3002, // 0x00000BBA
    FailedToChangeGamerTag = 3003, // 0x00000BBB
    FailedToUpdatePresence = 3004, // 0x00000BBC
    FailedToLocateGamerTag = 3005, // 0x00000BBD
    FailedToGetGamesData = 4000, // 0x00000FA0
    FailedToGetAchievement = 5000, // 0x00001388
    FailedToGetMessageSummary = 6000, // 0x00001770
    FailedToGetMessage = 6001, // 0x00001771
    FailedToDeleteMessage = 6002, // 0x00001772
    FailedToSendMessage = 6003, // 0x00001773
    FailedToBlockSender = 6004, // 0x00001774
    FailedToGetManifest = 7000, // 0x00001B58
    FailedToUpdateManifest = 7001, // 0x00001B59
    FailedToSetGamerPic = 7002, // 0x00001B5A
    FailedToAddFriend = 8000, // 0x00001F40
    FailedToDeleteFriend = 8001, // 0x00001F41
    FailedToAcceptFriendRequest = 8002, // 0x00001F42
    FailedToDeclineFriendRequest = 8003, // 0x00001F43
    FailedToCancelFriendRequest = 8004, // 0x00001F44
    FailedToGetAvatarItems = 8100, // 0x00001FA4
    FailedToGetGameListForGameStyle = 8101, // 0x00001FA5
    FailedToGetCategoryListForLifeStyle = 8102, // 0x00001FA6
    FailedToPurchaseAvatarItems = 8103, // 0x00001FA7
    GenericError = 9000, // 0x00002328
    ProgrammingError = 9999, // 0x0000270F
  }
}
