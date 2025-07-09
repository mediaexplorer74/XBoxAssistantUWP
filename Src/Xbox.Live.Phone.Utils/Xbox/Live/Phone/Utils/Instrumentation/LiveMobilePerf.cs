// *********************************************************
// Type: Xbox.Live.Phone.Utils.Instrumentation.LiveMobilePerf
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using Microsoft.Phone.Logging;
using System.Diagnostics;


namespace Xbox.Live.Phone.Utils.Instrumentation
{
  public static class LiveMobilePerf
  {
    public const uint KPITestId = 2000;

    [Conditional("DEBUG")]
    public static void BeginPerfMarker(LiveMobilePerf.PerfMarkerEvents eventCode, string details)
    {
    }

    [Conditional("DEBUG")]
    public static void EndPerfMarker(LiveMobilePerf.PerfMarkerEvents eventCode, string details)
    {
    }

    [Conditional("DEBUG")]
    public static void InfoPerfMarker(LiveMobilePerf.PerfMarkerEvents eventCode, string details)
    {
    }

    [Conditional("DEBUG")]
    private static void LogPerfMarker(
      LogFlags yceLogFlag,
      LiveMobilePerf.PerfMarkerEvents eventCode,
      string message)
    {
      Logger.YLogEvent(2000U, (uint) eventCode, yceLogFlag, message);
    }

    public enum PerfMarkerEvents
    {
      AppCtor,
      AppLoadComplete,
      AvatarAssetLoadComplete,
      AvatarOnScreen,
      OnLoad,
      CreateScreenBegin,
      CreateScreenEnd,
      SetScreenBegin,
      SetScreenEnd,
      SetScreenByScreenBegin,
      SetScreenByScreenEnd,
      PushScreenBegin,
      PushScreenEnd,
      ScreenLoadBegin,
      ScreenLoadEnd,
      ServiceCallBegin,
      ServiceCallEnd,
    }
  }
}
