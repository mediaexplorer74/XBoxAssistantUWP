// *********************************************************
// Type: LRC.Service.Omniture.OmnitureAppMeasurement
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using com.omniture;
using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Xbox.Live.Phone;
using Xbox.Live.Phone.Utils.Cache;


namespace LRC.Service.Omniture
{
  public sealed class OmnitureAppMeasurement
  {
    private const string ComponentName = "OmnitureAppMeasurement";
    private const string NotAvailable = "na";
    private const int UpLimit = 15;
    private const int OfflineLimit = 200;
    private const int OfflineThrottleDelay = 0;
    private const int OnlineSleepTime = 60000;
    private readonly AppMeasurement appMeasurementInstance = new AppMeasurement();
    private static readonly object Padlock = new object();
    private static OmnitureAppMeasurement instance = (OmnitureAppMeasurement) null;
    private int hitDataCount;
    private string deviceUniqueId;

    private OmnitureAppMeasurement()
    {
      this.appMeasurementInstance.account = EnvironmentState.Instance.IsProduction ? "msxboxwinphone" : "msxboxwinphone-dev";
      this.appMeasurementInstance.charSet = "UTF-8";
      this.appMeasurementInstance.currencyCode = "USD";
      this.appMeasurementInstance.visitorNamespace = "microsoftxbox";
      this.appMeasurementInstance.mobile = true;
      this.appMeasurementInstance.offlineLimit = 200;
      this.appMeasurementInstance.server = "Windows Phone";
      this.appMeasurementInstance.offlineThrottleDelay = 0;
      this.appMeasurementInstance.offlineFilename = "lrcwp_omniture.txt";
      this.appMeasurementInstance.trackingServer = "o.xbox.com";
      this.appMeasurementInstance.debugTracking = false;
      this.appMeasurementInstance.trackOffline = true;
      this.appMeasurementInstance.forceOffline();
    }

    public static OmnitureAppMeasurement Instance
    {
      get
      {
        lock (OmnitureAppMeasurement.Padlock)
          return OmnitureAppMeasurement.instance ?? (OmnitureAppMeasurement.instance = new OmnitureAppMeasurement());
      }
    }

    public string SessionId { get; set; }

    public ulong XboxUserId { get; set; }

    public int HitDataCount
    {
      get => this.hitDataCount;
      set
      {
        this.hitDataCount = value;
        if (this.hitDataCount < 15)
          return;
        this.ForceOnline();
        this.hitDataCount = 0;
      }
    }

    private string DeviceUniqueId
    {
      get
      {
        if (string.IsNullOrWhiteSpace(this.deviceUniqueId))
          this.deviceUniqueId = OmnitureAppMeasurement.GetDeviceId();
        return this.deviceUniqueId;
      }
    }

    public void ForceOnline()
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        this.appMeasurementInstance.forceOnline();
        Thread.Sleep(60000);
        this.appMeasurementInstance.forceOffline();
      });
    }

    public void TrackVisit(string pageName, string channel)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.channel = channel;
          this.appMeasurementInstance.pageName = pageName;
          this.appMeasurementInstance.events = "event4";
          this.appMeasurementInstance.hier1 = "D=pageName";
          this.appMeasurementInstance.eVar13 = "D=pageName";
          this.appMeasurementInstance.track();
          this.ClearEvents();
          this.appMeasurementInstance.hier1 = string.Empty;
          this.appMeasurementInstance.eVar13 = string.Empty;
        }
      });
    }

    public void TrackEvent(string eventVariable, string eventName)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.linkTrackEvents = eventVariable;
          this.appMeasurementInstance.linkTrackVars = "events";
          this.appMeasurementInstance.events = eventVariable;
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
        }
      });
    }

    public void TrackPlayControlClickEvent(string controlName, string eventName)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.linkTrackEvents = "event8";
          this.appMeasurementInstance.linkTrackVars = "events,prop30,eVar30";
          this.appMeasurementInstance.events = "event8";
          this.appMeasurementInstance.eVar30 = OmnitureAppMeasurement.GetValidString(controlName);
          this.appMeasurementInstance.prop30 = "D=v30";
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
          this.appMeasurementInstance.eVar30 = string.Empty;
          this.appMeasurementInstance.prop30 = string.Empty;
        }
      });
    }

    public void TrackAppStartEvent(string eventName, string appName, string launchType)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.RestorePersistentData();
          this.appMeasurementInstance.linkTrackEvents = "event1";
          this.appMeasurementInstance.linkTrackVars = "events,prop1,eVar1,prop2,eVar2,prop3,eVar3,prop4,eVar4,prop5,eVar5,prop6,eVar6,prop35,eVar35";
          this.appMeasurementInstance.events = "event1";
          this.SetAppLaunchDataBlock(appName, launchType);
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
          this.ClearAppLaunchDataBlock();
        }
      });
    }

    public void TrackUnhandledExceptionEvent(string message, string eventName)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.linkTrackEvents = "event9";
          this.appMeasurementInstance.linkTrackVars = "events,prop31,eVar31";
          this.appMeasurementInstance.events = "event9";
          this.appMeasurementInstance.eVar31 = OmnitureAppMeasurement.GetValidString(message);
          this.appMeasurementInstance.prop31 = "D=v31";
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
          this.appMeasurementInstance.eVar31 = string.Empty;
          this.appMeasurementInstance.prop31 = string.Empty;
        }
      });
    }

    public void TrackLookForXboxErrorEvent(string eventName, int errorCode, string errorString)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.linkTrackEvents = "event17";
          this.appMeasurementInstance.linkTrackVars = "events,prop33,eVar33,prop34,eVar34";
          this.appMeasurementInstance.events = "event17";
          this.appMeasurementInstance.eVar33 = errorCode.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          this.appMeasurementInstance.prop33 = "D=v33";
          this.appMeasurementInstance.eVar34 = OmnitureAppMeasurement.GetValidString(errorString);
          this.appMeasurementInstance.prop34 = "D=v34";
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
          this.appMeasurementInstance.eVar33 = string.Empty;
          this.appMeasurementInstance.prop33 = string.Empty;
          this.appMeasurementInstance.eVar34 = string.Empty;
          this.appMeasurementInstance.prop34 = string.Empty;
        }
      });
    }

    public void TrackSessionStartEvent(string eventName, string sessionType)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.linkTrackEvents = "event3";
          this.appMeasurementInstance.linkTrackVars = "events,prop7,eVar7,prop9,eVar9,prop10,eVar10,prop11,eVar11,prop12,eVar12,prop32,eVar32";
          this.appMeasurementInstance.events = "event3";
          this.appMeasurementInstance.eVar7 = OmnitureAppMeasurement.GetDeviceConnectionType();
          this.appMeasurementInstance.prop7 = "D=v7";
          this.appMeasurementInstance.eVar9 = TimeZoneInfo.Local.ToString();
          this.appMeasurementInstance.prop9 = "D=v9";
          this.appMeasurementInstance.eVar10 = DateTime.UtcNow.ToShortTimeString();
          this.appMeasurementInstance.prop10 = "D=v10";
          this.appMeasurementInstance.eVar11 = OmnitureAppMeasurement.GetValidString(this.SessionId);
          this.appMeasurementInstance.prop11 = "D=v11";
          this.appMeasurementInstance.eVar12 = OmnitureAppMeasurement.GetValidString(sessionType);
          this.appMeasurementInstance.prop12 = "D=v12";
          this.appMeasurementInstance.eVar32 = OmnitureAppMeasurement.GetValidString(this.GetUserId());
          this.appMeasurementInstance.prop32 = "D=v32";
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
          this.appMeasurementInstance.eVar7 = string.Empty;
          this.appMeasurementInstance.prop7 = string.Empty;
          this.appMeasurementInstance.eVar9 = string.Empty;
          this.appMeasurementInstance.prop9 = string.Empty;
          this.appMeasurementInstance.eVar10 = string.Empty;
          this.appMeasurementInstance.prop10 = string.Empty;
          this.appMeasurementInstance.eVar11 = string.Empty;
          this.appMeasurementInstance.prop11 = string.Empty;
          this.appMeasurementInstance.eVar12 = string.Empty;
          this.appMeasurementInstance.prop12 = string.Empty;
          this.appMeasurementInstance.eVar32 = string.Empty;
          this.appMeasurementInstance.prop32 = string.Empty;
        }
      });
    }

    public void TrackSearchUsedEvent(string eventName, string searchType, string term)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.linkTrackEvents = "event5";
          this.appMeasurementInstance.linkTrackVars = "events,prop8,eVar8,prop14,eVar14";
          this.appMeasurementInstance.events = "event5";
          this.appMeasurementInstance.eVar8 = OmnitureAppMeasurement.GetValidString(searchType);
          this.appMeasurementInstance.prop8 = "D=v8";
          this.appMeasurementInstance.eVar14 = OmnitureAppMeasurement.GetValidString(term);
          this.appMeasurementInstance.prop14 = "D=v14";
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
          this.appMeasurementInstance.eVar8 = string.Empty;
          this.appMeasurementInstance.prop8 = string.Empty;
          this.appMeasurementInstance.eVar14 = string.Empty;
          this.appMeasurementInstance.prop14 = string.Empty;
        }
      });
    }

    public void TrackSearchResultsPageVisit(
      string pageName,
      string channel,
      string term,
      string restult,
      string itemType)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.channel = channel;
          this.appMeasurementInstance.pageName = pageName;
          this.appMeasurementInstance.events = "event4";
          this.appMeasurementInstance.eVar13 = "D=pageName";
          this.appMeasurementInstance.hier1 = "D=pageName";
          this.appMeasurementInstance.eVar14 = OmnitureAppMeasurement.GetValidString(term);
          this.appMeasurementInstance.prop14 = "D=v14";
          this.appMeasurementInstance.eVar15 = OmnitureAppMeasurement.GetValidString(restult);
          this.appMeasurementInstance.prop15 = "D=v15";
          this.appMeasurementInstance.eVar16 = OmnitureAppMeasurement.GetValidString(itemType);
          this.appMeasurementInstance.prop16 = "D=v16";
          this.appMeasurementInstance.track();
          this.ClearEvents();
          this.appMeasurementInstance.hier1 = string.Empty;
          this.appMeasurementInstance.eVar13 = string.Empty;
          this.appMeasurementInstance.eVar14 = string.Empty;
          this.appMeasurementInstance.prop14 = string.Empty;
          this.appMeasurementInstance.eVar15 = string.Empty;
          this.appMeasurementInstance.prop15 = string.Empty;
          this.appMeasurementInstance.eVar16 = string.Empty;
          this.appMeasurementInstance.prop16 = string.Empty;
        }
      });
    }

    public void TrackZuneBrowseEvent(
      string eventName,
      string entryPoint,
      string mediaType,
      string category)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.linkTrackEvents = "event11";
          this.appMeasurementInstance.linkTrackVars = "events,prop17,eVar17,prop19,eVar19,prop23,eVar23";
          this.appMeasurementInstance.events = "event11";
          this.appMeasurementInstance.eVar17 = OmnitureAppMeasurement.GetValidString(entryPoint);
          this.appMeasurementInstance.prop17 = "D=v17";
          this.appMeasurementInstance.eVar19 = OmnitureAppMeasurement.GetValidString(mediaType);
          this.appMeasurementInstance.prop19 = "D=v19";
          this.appMeasurementInstance.eVar23 = OmnitureAppMeasurement.GetValidString(category);
          this.appMeasurementInstance.prop23 = "D=v23";
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
          this.appMeasurementInstance.eVar17 = string.Empty;
          this.appMeasurementInstance.prop17 = string.Empty;
          this.appMeasurementInstance.eVar19 = string.Empty;
          this.appMeasurementInstance.prop19 = string.Empty;
          this.appMeasurementInstance.eVar23 = string.Empty;
          this.appMeasurementInstance.prop23 = string.Empty;
        }
      });
    }

    public void TrackZuneGenreStudioNetworkClickedEvent(
      string eventName,
      string genre,
      string studio,
      string network)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.linkTrackEvents = "event12";
          this.appMeasurementInstance.linkTrackVars = "events,prop22,eVar22,prop24,eVar24,prop25,eVar25";
          this.appMeasurementInstance.events = "event12";
          this.appMeasurementInstance.eVar22 = OmnitureAppMeasurement.GetValidString(genre);
          this.appMeasurementInstance.prop22 = "D=v22";
          this.appMeasurementInstance.eVar24 = OmnitureAppMeasurement.GetValidString(studio);
          this.appMeasurementInstance.prop24 = "D=v24";
          this.appMeasurementInstance.eVar25 = OmnitureAppMeasurement.GetValidString(network);
          this.appMeasurementInstance.prop25 = "D=v25";
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
          this.appMeasurementInstance.eVar22 = string.Empty;
          this.appMeasurementInstance.prop22 = string.Empty;
          this.appMeasurementInstance.eVar24 = string.Empty;
          this.appMeasurementInstance.prop24 = string.Empty;
          this.appMeasurementInstance.eVar25 = string.Empty;
          this.appMeasurementInstance.prop25 = string.Empty;
        }
      });
    }

    public void TrackZuneCategoryContentItemSelectedEvent(
      string eventName,
      string entryPoint,
      string mediaTitle,
      string mediaId,
      string rank)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.linkTrackEvents = "event6";
          this.appMeasurementInstance.linkTrackVars = "events,prop17,eVar17,prop18,eVar18,prop20,eVar20,prop21,eVar21";
          this.appMeasurementInstance.events = "event6";
          this.appMeasurementInstance.eVar17 = OmnitureAppMeasurement.GetValidString(entryPoint);
          this.appMeasurementInstance.prop17 = "D=v17";
          this.appMeasurementInstance.eVar18 = OmnitureAppMeasurement.GetValidString(mediaTitle);
          this.appMeasurementInstance.prop18 = "D=v18";
          this.appMeasurementInstance.eVar20 = OmnitureAppMeasurement.GetValidString(mediaId);
          this.appMeasurementInstance.prop20 = "D=v20";
          this.appMeasurementInstance.eVar21 = OmnitureAppMeasurement.GetValidString(rank);
          this.appMeasurementInstance.prop21 = "D=v21";
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
          this.appMeasurementInstance.eVar17 = string.Empty;
          this.appMeasurementInstance.prop17 = string.Empty;
          this.appMeasurementInstance.eVar18 = string.Empty;
          this.appMeasurementInstance.prop18 = string.Empty;
          this.appMeasurementInstance.eVar20 = string.Empty;
          this.appMeasurementInstance.prop20 = string.Empty;
          this.appMeasurementInstance.eVar21 = string.Empty;
          this.appMeasurementInstance.prop21 = string.Empty;
        }
      });
    }

    public void MediaItemClickedEvent(
      string eventName,
      string entryPoint,
      OmnitureMediaContentTrackInfo mediaContent)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.linkTrackEvents = "event6";
          this.appMeasurementInstance.linkTrackVars = "events,prop17,eVar17,prop18,eVar18,prop19,eVar19,prop20,eVar20,prop21,eVar21,prop22,eVar22,prop23,eVar23,prop24,eVar24,prop25,eVar25";
          this.appMeasurementInstance.events = "event6";
          this.appMeasurementInstance.eVar17 = OmnitureAppMeasurement.GetValidString(entryPoint);
          this.appMeasurementInstance.prop17 = "D=v17";
          this.SetMediaBlock(mediaContent);
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
          this.appMeasurementInstance.eVar17 = string.Empty;
          this.appMeasurementInstance.prop17 = string.Empty;
          this.ClearMediaBlock();
        }
      });
    }

    public void TrackPlayOnConsoleEvent(
      string eventName,
      string entryPoint,
      OmnitureMediaContentTrackInfo mediaContent,
      OmnitureProviderTrackInfo provider)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.linkTrackEvents = "event7";
          this.appMeasurementInstance.linkTrackVars = "events,prop17,eVar17,prop18,eVar18,prop19,eVar19,prop20,eVar20,prop22,eVar22,prop23,eVar23,prop24,eVar24,prop25,eVar25,prop26,eVar26,prop27,eVar27,prop28,eVar28,prop29,eVar29";
          this.appMeasurementInstance.events = "event7";
          this.appMeasurementInstance.eVar17 = OmnitureAppMeasurement.GetValidString(entryPoint);
          this.appMeasurementInstance.prop17 = "D=v17";
          this.SetMediaBlock(mediaContent);
          this.SetProviderBlock(provider);
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
          this.appMeasurementInstance.eVar17 = string.Empty;
          this.appMeasurementInstance.prop17 = string.Empty;
          this.ClearMediaBlock();
          this.ClearProviderBlock();
        }
      });
    }

    public void TrackQuickplayLauchOnConsoleEvent(
      string eventName,
      string entryPoint,
      OmnitureMediaContentTrackInfo mediaContent,
      OmnitureProviderTrackInfo provider)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.linkTrackEvents = "event7";
          this.appMeasurementInstance.linkTrackVars = "events,prop17,eVar17,prop18,eVar18,prop19,eVar19,prop20,eVar20,prop21,eVar21,prop22,eVar22,prop23,eVar23,prop24,eVar24,prop25,eVar25,prop26,eVar26,prop27,eVar27,prop28,eVar28,prop29,eVar29";
          this.appMeasurementInstance.events = "event7";
          this.appMeasurementInstance.eVar17 = OmnitureAppMeasurement.GetValidString(entryPoint);
          this.appMeasurementInstance.prop17 = "D=v17";
          this.SetMediaBlock(mediaContent);
          this.SetProviderBlock(provider);
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
          this.appMeasurementInstance.eVar17 = string.Empty;
          this.appMeasurementInstance.prop17 = string.Empty;
          this.ClearMediaBlock();
          this.ClearProviderBlock();
        }
      });
    }

    public void TrackZuneBrowsingPlayOnConsoleEvent(
      string eventName,
      string entryPoint,
      OmnitureMediaContentTrackInfo mediaContent,
      OmnitureProviderTrackInfo provider)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        lock (this.appMeasurementInstance)
        {
          this.appMeasurementInstance.linkTrackEvents = "event7";
          this.appMeasurementInstance.linkTrackVars = "events,prop17,eVar17,prop18,eVar18,prop19,eVar19,prop20,eVar20,prop22,eVar22,prop26,eVar26,prop27,eVar27,prop28,eVar28,prop29,eVar29";
          this.appMeasurementInstance.events = "event7";
          this.appMeasurementInstance.eVar17 = OmnitureAppMeasurement.GetValidString(entryPoint);
          this.appMeasurementInstance.prop17 = "D=v17";
          this.appMeasurementInstance.eVar18 = OmnitureAppMeasurement.GetValidString(mediaContent.Title);
          this.appMeasurementInstance.prop18 = "D=v18";
          this.appMeasurementInstance.eVar19 = OmnitureAppMeasurement.GetValidString(mediaContent.MediaType);
          this.appMeasurementInstance.prop19 = "D=v19";
          this.appMeasurementInstance.eVar20 = OmnitureAppMeasurement.GetValidString(mediaContent.MediaId);
          this.appMeasurementInstance.prop20 = "D=v20";
          this.appMeasurementInstance.eVar22 = OmnitureAppMeasurement.GetValidString(mediaContent.Genre);
          this.appMeasurementInstance.prop22 = "D=v22";
          this.SetProviderBlock(provider);
          this.appMeasurementInstance.trackLink((string) null, "o", eventName);
          this.ClearEvents();
          this.appMeasurementInstance.eVar17 = string.Empty;
          this.appMeasurementInstance.prop17 = string.Empty;
          this.appMeasurementInstance.eVar18 = string.Empty;
          this.appMeasurementInstance.prop18 = string.Empty;
          this.appMeasurementInstance.eVar19 = string.Empty;
          this.appMeasurementInstance.prop19 = string.Empty;
          this.appMeasurementInstance.eVar20 = string.Empty;
          this.appMeasurementInstance.prop20 = string.Empty;
          this.appMeasurementInstance.eVar22 = string.Empty;
          this.appMeasurementInstance.prop22 = string.Empty;
          this.ClearProviderBlock();
        }
      });
    }

    public void SavePersistentData()
    {
      CacheManager.Save("OmniturePersistentData.xml", (object) new OmniturePersistentData()
      {
        HitDataCount = this.HitDataCount,
        SessionId = this.SessionId
      });
    }

    private static string GetValidString(string input)
    {
      if (string.IsNullOrWhiteSpace(input))
        return "na";
      if (input.Length > 250)
        input = input.Substring(0, 250);
      return input;
    }

    private static string GetDeviceConnectionType()
    {
      string deviceConnectionType = "Unknown";
      if (DeviceNetworkInformation.IsNetworkAvailable)
      {
        using (NetworkInterfaceList source = new NetworkInterfaceList())
        {
          foreach (NetworkInterfaceInfo networkInterfaceInfo in ((IEnumerable<NetworkInterfaceInfo>) source).Where<NetworkInterfaceInfo>((Func<NetworkInterfaceInfo, bool>) (info => 1 == info.InterfaceState)))
          {
            NetworkInterfaceType interfaceType = networkInterfaceInfo.InterfaceType;
            if (interfaceType != 6 && interfaceType != 71)
            {
              switch (interfaceType - 145)
              {
                case 0:
                case 1:
                  break;
                default:
                  continue;
              }
            }
            switch (networkInterfaceInfo.InterfaceSubtype - 5)
            {
              case 0:
                deviceConnectionType = "3G";
                continue;
              case 3:
                deviceConnectionType = "Tethered";
                continue;
              case 4:
                deviceConnectionType = "WiFi";
                continue;
              default:
                continue;
            }
          }
        }
      }
      return deviceConnectionType;
    }

    private static string GetDeviceId()
    {
      object obj;
      return DeviceExtendedProperties.TryGetValue("DeviceUniqueId", ref obj) && obj is byte[] numArray ? BitConverter.ToString(numArray) : string.Empty;
    }

    private void RestorePersistentData()
    {
      OmniturePersistentData omniturePersistentData = CacheManager.Load<OmniturePersistentData>("OmniturePersistentData.xml");
      if (omniturePersistentData == null)
        return;
      this.HitDataCount = omniturePersistentData.HitDataCount;
      this.SessionId = omniturePersistentData.SessionId;
    }

    private void ClearEvents()
    {
      this.appMeasurementInstance.events = string.Empty;
      if (this.appMeasurementInstance.offline)
        ++this.HitDataCount;
      else
        this.hitDataCount = 0;
    }

    private void SetMediaBlock(OmnitureMediaContentTrackInfo mediaContent)
    {
      this.appMeasurementInstance.eVar18 = OmnitureAppMeasurement.GetValidString(mediaContent.Title);
      this.appMeasurementInstance.prop18 = "D=v18";
      this.appMeasurementInstance.eVar19 = OmnitureAppMeasurement.GetValidString(mediaContent.MediaType);
      this.appMeasurementInstance.prop19 = "D=v19";
      this.appMeasurementInstance.eVar20 = OmnitureAppMeasurement.GetValidString(mediaContent.MediaId);
      this.appMeasurementInstance.prop20 = "D=v20";
      this.appMeasurementInstance.eVar21 = mediaContent.Rank < 0 ? "na" : mediaContent.Rank.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      this.appMeasurementInstance.prop21 = "D=v21";
      this.appMeasurementInstance.eVar22 = OmnitureAppMeasurement.GetValidString(mediaContent.Genre);
      this.appMeasurementInstance.prop22 = "D=v22";
      this.appMeasurementInstance.eVar23 = OmnitureAppMeasurement.GetValidString(mediaContent.Category);
      this.appMeasurementInstance.prop23 = "D=v23";
      this.appMeasurementInstance.eVar24 = OmnitureAppMeasurement.GetValidString(mediaContent.Studio);
      this.appMeasurementInstance.prop24 = "D=v24";
      this.appMeasurementInstance.eVar25 = OmnitureAppMeasurement.GetValidString(mediaContent.Network);
      this.appMeasurementInstance.prop25 = "D=v25";
    }

    private void ClearMediaBlock()
    {
      this.appMeasurementInstance.eVar18 = string.Empty;
      this.appMeasurementInstance.prop18 = string.Empty;
      this.appMeasurementInstance.eVar19 = string.Empty;
      this.appMeasurementInstance.prop19 = string.Empty;
      this.appMeasurementInstance.eVar20 = string.Empty;
      this.appMeasurementInstance.prop20 = string.Empty;
      this.appMeasurementInstance.eVar21 = string.Empty;
      this.appMeasurementInstance.prop21 = string.Empty;
      this.appMeasurementInstance.eVar22 = string.Empty;
      this.appMeasurementInstance.prop22 = string.Empty;
      this.appMeasurementInstance.eVar23 = string.Empty;
      this.appMeasurementInstance.prop23 = string.Empty;
      this.appMeasurementInstance.eVar24 = string.Empty;
      this.appMeasurementInstance.prop24 = string.Empty;
      this.appMeasurementInstance.eVar25 = string.Empty;
      this.appMeasurementInstance.prop25 = string.Empty;
    }

    private void SetProviderBlock(OmnitureProviderTrackInfo provider)
    {
      this.appMeasurementInstance.eVar26 = OmnitureAppMeasurement.GetValidString(provider.Title);
      this.appMeasurementInstance.prop26 = "D=v26";
      this.appMeasurementInstance.eVar27 = provider.TitleId > 0U ? provider.TitleId.ToString((IFormatProvider) CultureInfo.InvariantCulture) : "na";
      this.appMeasurementInstance.prop27 = "D=v27";
      this.appMeasurementInstance.eVar28 = OmnitureAppMeasurement.GetValidString(provider.ProductId);
      this.appMeasurementInstance.prop28 = "D=v28";
      this.appMeasurementInstance.eVar29 = OmnitureAppMeasurement.GetValidString(provider.LaunchType);
      this.appMeasurementInstance.prop29 = "D=v29";
    }

    private void ClearProviderBlock()
    {
      this.appMeasurementInstance.eVar26 = string.Empty;
      this.appMeasurementInstance.prop26 = string.Empty;
      this.appMeasurementInstance.eVar27 = string.Empty;
      this.appMeasurementInstance.prop27 = string.Empty;
      this.appMeasurementInstance.eVar28 = string.Empty;
      this.appMeasurementInstance.prop28 = string.Empty;
      this.appMeasurementInstance.eVar29 = string.Empty;
      this.appMeasurementInstance.prop29 = string.Empty;
    }

    private void SetAppLaunchDataBlock(string appName, string launchType)
    {
      this.appMeasurementInstance.eVar1 = "Windows Phone";
      this.appMeasurementInstance.prop1 = "D=v1";
      this.appMeasurementInstance.eVar2 = Environment.OSVersion.ToString();
      this.appMeasurementInstance.prop2 = "D=v2";
      this.appMeasurementInstance.eVar3 = DeviceStatus.DeviceName;
      this.appMeasurementInstance.prop3 = "D=v3";
      this.appMeasurementInstance.eVar4 = CultureInfo.CurrentUICulture.EnglishName;
      this.appMeasurementInstance.prop4 = "D=v4";
      this.appMeasurementInstance.eVar5 = appName;
      this.appMeasurementInstance.prop5 = "D=v5";
      this.appMeasurementInstance.eVar6 = this.DeviceUniqueId;
      this.appMeasurementInstance.prop6 = "D=v6";
      this.appMeasurementInstance.eVar35 = launchType;
      this.appMeasurementInstance.prop35 = "D=v35";
    }

    private void ClearAppLaunchDataBlock()
    {
      this.appMeasurementInstance.eVar1 = string.Empty;
      this.appMeasurementInstance.prop1 = string.Empty;
      this.appMeasurementInstance.eVar2 = string.Empty;
      this.appMeasurementInstance.prop2 = string.Empty;
      this.appMeasurementInstance.eVar3 = string.Empty;
      this.appMeasurementInstance.prop3 = string.Empty;
      this.appMeasurementInstance.eVar4 = string.Empty;
      this.appMeasurementInstance.prop4 = string.Empty;
      this.appMeasurementInstance.eVar5 = string.Empty;
      this.appMeasurementInstance.prop5 = string.Empty;
      this.appMeasurementInstance.eVar6 = string.Empty;
      this.appMeasurementInstance.prop6 = string.Empty;
      this.appMeasurementInstance.eVar35 = string.Empty;
      this.appMeasurementInstance.prop35 = string.Empty;
    }

    private string GetUserId()
    {
      object obj;
      if (!DeviceExtendedProperties.TryGetValue("DeviceUniqueId", ref obj) || !(obj is byte[] destinationArray))
        return string.Empty;
      Array.Copy((Array) BitConverter.GetBytes(this.XboxUserId ^ BitConverter.ToUInt64(destinationArray, 12)), 0, (Array) destinationArray, 12, 8);
      return BitConverter.ToString(destinationArray);
    }
  }
}
