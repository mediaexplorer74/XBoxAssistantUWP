// *********************************************************
// Type: Xbox.Live.Phone.EnvironmentState
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Xbox.Live.Phone.Services;


namespace Xbox.Live.Phone
{
  public class EnvironmentState
  {
    private const string XboxLiveSettingsFileName = "XboxLIVESettings.xml";
    private const string LiveEnvironmentSettingKey = "LIVEEnvironment";
    private static EnvironmentState staticInstance;
    private Dictionary<string, string> xboxLiveSettings = new Dictionary<string, string>();

    private EnvironmentState()
    {
      using (Stream stream = TitleContainer.OpenStream("XboxLIVESettings.xml"))
      {
        foreach (XElement descendant in XElement.Load(stream).Descendants((XName) "Item"))
          this.xboxLiveSettings.Add(descendant.Element((XName) "Key").Value, descendant.Element((XName) "Value").Value);
      }
    }

    public static EnvironmentState Instance
    {
      get
      {
        if (EnvironmentState.staticInstance == null)
          EnvironmentState.staticInstance = new EnvironmentState();
        return EnvironmentState.staticInstance;
      }
    }

    public bool IsProduction => this.Environment == ServiceCommon.Environment.Production;

    public ServiceCommon.Environment Environment
    {
      get
      {
        return (ServiceCommon.Environment) Enum.Parse(typeof (ServiceCommon.Environment), this.xboxLiveSettings["LIVEEnvironment"], true);
      }
    }
  }
}
