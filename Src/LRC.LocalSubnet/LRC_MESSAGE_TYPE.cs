// *********************************************************
// Type: LRC.LocalSubnet.LRC_MESSAGE_TYPE
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll

using System.Diagnostics.CodeAnalysis;


namespace LRC.LocalSubnet
{
  [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32", Justification = "Keep parity with console code")]
  public enum LRC_MESSAGE_TYPE : uint
  {
    None = 0,
    LRC_MESSAGE_GET_ACTIVE_TITLEID = 1,
    LRC_MESSAGE_LAUNCH_TITLE = 2,
    LRC_MESSAGE_SEND_INPUT = 3,
    LRC_MESSAGE_GET_MEDIA_TITLE_STATE = 4,
    LRC_MESSAGE_NON_MEDIA_TITLE_STATE_NOTIFICATION = 5,
    LRC_MESSAGE_MEDIA_TITLE_STATE_NOTIFICATION = 6,
    LRC_MESSAGE_GET_CONSOLE_SETTINGS = 12, // 0x0000000C
    LRC_MESSAGE_JOIN_SESSION = 2147483649, // 0x80000001
    LRC_MESSAGE_LEAVE_SESSION = 2147483650, // 0x80000002
  }
}
