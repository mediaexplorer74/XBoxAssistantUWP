// *********************************************************
// Type: Leet.MessageService.DataContracts.MessageTypeEnum
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll


namespace Leet.MessageService.DataContracts
{
  public enum MessageTypeEnum
  {
    TitleCustom = 1,
    FriendRequest = 2,
    GameInvite = 3,
    TeamRecruit = 4,
    CompetitionReminder = 5,
    CompetitionRequest = 6,
    LiveMessage = 7,
    PersonalMessage = 8,
    VideoMessage = 9,
    QuickChatInvite = 10, // 0x0000000A
    VideoChatInvite = 11, // 0x0000000B
    PartyInvite = 12, // 0x0000000C
  }
}
