// *********************************************************
// Type: LRC.ViewModel.LaunchUtil
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System;
using System.Globalization;


namespace LRC.ViewModel
{
  public static class LaunchUtil
  {
    public const string LaunchAppTemplate = "app:{0:X}";
    public const string LaunchGameTemplate = "gamedetails:{0}";
    public const string LaunchGameContentTemplate = "contentdetails:{0}";
    public const string ZunePlayNowParameter = "&PlayNow=True";
    public const string ZunePlayNowParameterName = "&PlayNow=";

    public static string CreateLaunchParameter(uint titleId, LaunchType type, string deepLink)
    {
      string launchParameter = (string) null;
      switch (type)
      {
        case LaunchType.Game:
          if (!string.IsNullOrWhiteSpace(deepLink))
          {
            launchParameter = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "gamedetails:{0}", new object[1]
            {
              (object) deepLink
            });
            break;
          }
          break;
        case LaunchType.GameContent:
          if (!string.IsNullOrWhiteSpace(deepLink))
          {
            launchParameter = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "contentdetails:{0}", new object[1]
            {
              (object) deepLink
            });
            break;
          }
          break;
        case LaunchType.App:
          if (string.IsNullOrWhiteSpace(deepLink))
            launchParameter = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "app:{0:X}", new object[1]
            {
              (object) titleId
            });
          else
            launchParameter = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "app:{0:X}:{1}", new object[2]
            {
              (object) titleId,
              (object) deepLink
            });
          if (titleId == 1481115739U && !string.IsNullOrEmpty(deepLink) && launchParameter.IndexOf("&PlayNow=", StringComparison.OrdinalIgnoreCase) == -1)
          {
            launchParameter += "&PlayNow=True";
            break;
          }
          break;
      }
      return launchParameter;
    }
  }
}
