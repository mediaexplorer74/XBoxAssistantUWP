// *********************************************************
// Type: Microsoft.Phone.Marketplace.XboxLIVESettingsParser
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Info;
using Microsoft.Phone.Marketplace.Resources;
using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;


namespace Microsoft.Phone.Marketplace
{
  internal class XboxLIVESettingsParser
  {
    internal Uri ParseMarketplaceURL()
    {
      try
      {
        string uriString = XElement.Load("XboxLIVESettings.xml", LoadOptions.None).Descendants((XName) "Item").Where<XElement>((Func<XElement, bool>) (item => (string) item.Element((XName) "Key") == "MarketplacePdlcURL")).Select<XElement, string>((Func<XElement, string>) (item => (string) item.Element((XName) "Value"))).First<string>();
        if (uriString.Contains("WP_DEVICE_$TESTDUID$"))
          uriString = uriString.Replace("WP_DEVICE_$TESTDUID$", XboxLIVESettingsParser.GetDuidString());
        return new Uri(uriString);
      }
      catch
      {
        throw new ServiceFailureException(string.Format(NonLocalizedResources.FailedToParseXBLSettings, (object) "XboxLIVESettings.xml", (object) "MarketplacePdlcURL"));
      }
    }

    private static string GetDuidString()
    {
      string str1 = "WP_DEVICE_";
      MethodInfo method = typeof (DeviceExtendedProperties).GetMethod("GetValue", new Type[1]
      {
        typeof (string)
      });
      string duidString = str1 + "DEFAULTID";
      try
      {
        byte[] numArray = (byte[]) method.Invoke((object) null, new object[1]
        {
          (object) "DeviceUniqueId"
        });
        string str2 = "";
        foreach (byte num in numArray)
          str2 += num.ToString("X");
        duidString = str1 + str2;
      }
      catch (Exception ex)
      {
      }
      return duidString;
    }
  }
}
