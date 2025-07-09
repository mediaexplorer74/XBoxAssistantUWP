// *********************************************************
// Type: Microsoft.XMedia.DeviceTypeUtil
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;


namespace Microsoft.XMedia
{
  public static class DeviceTypeUtil
  {
    public static DeviceType FromName(string name)
    {
      switch (name)
      {
        case "iPadHost":
          return DeviceType.IPad;
        case "iPhoneHost":
          return DeviceType.IPhone;
        case "Xbox":
          return DeviceType.Xbox;
        case "WindowsPhone":
          return DeviceType.WinPhone;
        case "WebHost":
          return DeviceType.Web;
        case "WinHost":
          return DeviceType.PC;
        case "WindowsEntertainment":
          return DeviceType.WindowsEntertainment;
        default:
          throw new ArgumentException("Unknown device type", nameof (name));
      }
    }

    public static string ToString(DeviceType deviceType)
    {
      switch (deviceType)
      {
        case DeviceType.Xbox:
          return "Xbox";
        case DeviceType.Web:
          return "WebHost";
        case DeviceType.WinPhone:
          return "WindowsPhone";
        case DeviceType.IPad:
          return "iPadHost";
        case DeviceType.IPhone:
          return "iPhoneHost";
        case DeviceType.WindowsEntertainment:
          return "WindowsEntertainment";
        default:
          throw new ArgumentException("Unknown device type", nameof (deviceType));
      }
    }
  }
}
