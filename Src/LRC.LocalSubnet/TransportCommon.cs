// *********************************************************
// Type: LRC.LocalSubnet.TransportCommon
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll


namespace LRC.LocalSubnet
{
  public static class TransportCommon
  {
    public const uint LRC_MESSAGE_HEADER_SIGNATURE = 3202006746;
    public const uint LRC_PROTOCOL_VERSION = 1;
    public const int LRC_MESSAGE_HASH_SIZE = 20;
    public const int LRC_MEDIA_ASSETID_SIZE = 256;
    public const int LRC_LAUNCH_PARAMETER_SIZE = 900;
    public const int LRC_USER_DISPLAY_NAMES_SIZE = 65;
    public const int EmptyRequestMessageSize = 52;
    public const int EmptyResponseMessageSize = 60;
    public const int EmptyNotificationMessageSize = 52;
    public const int MinimumMessageSize = 32;
    public const int IntSize = 4;
    public const int LocaleString = 14;
    public const int JoinSessionRequestSize = 52;
    public const int LeaveSessionRequestSize = 52;
    public const int GetActiveTitleIdRequestSize = 52;
    public const int LaunchTitleRequestSize = 960;
    public const int GetMediaStateRequestSize = 52;
    public const int SendInputRequestSize = 68;
    public const int JoinSessionResponseSize = 72;
    public const int LeaveSessionResponseSize = 60;
    public const int GetActiveTitleIdResponseSize = 64;
    public const int LaunchTitleResponseSize = 60;
    public const int SendInputResponseSize = 60;
    public const int GetMediaStateResponseSize = 368;
    public const int NonMediaTitleNotificationSize = 56;
    public const int MediaTitleNotificationSize = 360;
    public const int MediaTitleParamSize = 308;
    public const int GetConsoleSettingsRequestSize = 52;
    public const int GetConsoleSettingsResponseSize = 74;
  }
}
