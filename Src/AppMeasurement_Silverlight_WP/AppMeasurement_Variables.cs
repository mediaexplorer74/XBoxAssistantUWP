// *********************************************************
// Type: com.omniture.AppMeasurement_Variables
// Assembly: AppMeasurement_Silverlight_WP, Version=1.3.7.0, Culture=neutral, PublicKeyToken=null
// MVID: B2938048-604D-4B3E-B432-7854B0CBA8DA
// *********************************************************AppMeasurement_Silverlight_WP.dll

using System.Collections.Generic;


namespace com.omniture
{
  public class AppMeasurement_Variables : AppMeasurement_SetInterval
  {
    protected Dictionary<string, object> variables;
    private bool _debugTracking;
    private bool _trackOffline;
    private int _offlineLimit;
    private int _offlineThrottleDelay;
    private bool _trackDownloadLinks;
    private bool _trackExternalLinks;
    private bool _trackClickMap;
    private bool _linkLeaveQueryString;
    private bool _ssl;
    private bool _mobile;
    private int _timestamp;
    private int _cookieDomainPeriods;
    private Dictionary<string, object> _contextData;
    private int _lightStoreForSeconds;
    private int _lightIncrementBy;
    private Dictionary<string, object> _retrieveLightData;
    protected string[] requiredVarList = new string[27]
    {
      nameof (timestamp),
      nameof (dynamicVariablePrefix),
      nameof (visitorID),
      nameof (vmk),
      nameof (visitorMigrationKey),
      nameof (visitorMigrationServer),
      nameof (visitorMigrationServerSecure),
      nameof (charSet),
      nameof (visitorNamespace),
      nameof (cookieDomainPeriods),
      nameof (cookieLifetime),
      nameof (pageName),
      nameof (pageURL),
      nameof (referrer),
      nameof (contextData),
      nameof (currencyCode),
      nameof (lightProfileID),
      nameof (lightStoreForSeconds),
      nameof (lightIncrementBy),
      nameof (retrieveLightProfiles),
      nameof (deleteLightProfiles),
      nameof (retrieveLightData),
      nameof (pe),
      nameof (pev1),
      nameof (pev2),
      nameof (pev3),
      nameof (resolution)
    };
    protected string[] accountVarList = new string[198]
    {
      nameof (timestamp),
      nameof (dynamicVariablePrefix),
      nameof (visitorID),
      nameof (vmk),
      nameof (visitorMigrationKey),
      nameof (visitorMigrationServer),
      nameof (visitorMigrationServerSecure),
      nameof (charSet),
      nameof (visitorNamespace),
      nameof (cookieDomainPeriods),
      nameof (cookieLifetime),
      nameof (pageName),
      nameof (pageURL),
      nameof (referrer),
      nameof (contextData),
      nameof (currencyCode),
      nameof (lightProfileID),
      nameof (lightStoreForSeconds),
      nameof (lightIncrementBy),
      nameof (retrieveLightProfiles),
      nameof (deleteLightProfiles),
      nameof (retrieveLightData),
      nameof (purchaseID),
      nameof (variableProvider),
      nameof (channel),
      nameof (server),
      nameof (pageType),
      nameof (transactionID),
      nameof (campaign),
      nameof (state),
      nameof (zip),
      nameof (events),
      nameof (events2),
      nameof (products),
      nameof (tnt),
      nameof (prop1),
      nameof (eVar1),
      nameof (hier1),
      nameof (list1),
      nameof (prop2),
      nameof (eVar2),
      nameof (hier2),
      nameof (list2),
      nameof (prop3),
      nameof (eVar3),
      nameof (hier3),
      nameof (list3),
      nameof (prop4),
      nameof (eVar4),
      nameof (hier4),
      nameof (prop5),
      nameof (eVar5),
      nameof (hier5),
      nameof (prop6),
      nameof (eVar6),
      nameof (prop7),
      nameof (eVar7),
      nameof (prop8),
      nameof (eVar8),
      nameof (prop9),
      nameof (eVar9),
      nameof (prop10),
      nameof (eVar10),
      nameof (prop11),
      nameof (eVar11),
      nameof (prop12),
      nameof (eVar12),
      nameof (prop13),
      nameof (eVar13),
      nameof (prop14),
      nameof (eVar14),
      nameof (prop15),
      nameof (eVar15),
      nameof (prop16),
      nameof (eVar16),
      nameof (prop17),
      nameof (eVar17),
      nameof (prop18),
      nameof (eVar18),
      nameof (prop19),
      nameof (eVar19),
      nameof (prop20),
      nameof (eVar20),
      nameof (prop21),
      nameof (eVar21),
      nameof (prop22),
      nameof (eVar22),
      nameof (prop23),
      nameof (eVar23),
      nameof (prop24),
      nameof (eVar24),
      nameof (prop25),
      nameof (eVar25),
      nameof (prop26),
      nameof (eVar26),
      nameof (prop27),
      nameof (eVar27),
      nameof (prop28),
      nameof (eVar28),
      nameof (prop29),
      nameof (eVar29),
      nameof (prop30),
      nameof (eVar30),
      nameof (prop31),
      nameof (eVar31),
      nameof (prop32),
      nameof (eVar32),
      nameof (prop33),
      nameof (eVar33),
      nameof (prop34),
      nameof (eVar34),
      nameof (prop35),
      nameof (eVar35),
      nameof (prop36),
      nameof (eVar36),
      nameof (prop37),
      nameof (eVar37),
      nameof (prop38),
      nameof (eVar38),
      nameof (prop39),
      nameof (eVar39),
      nameof (prop40),
      nameof (eVar40),
      nameof (prop41),
      nameof (eVar41),
      nameof (prop42),
      nameof (eVar42),
      nameof (prop43),
      nameof (eVar43),
      nameof (prop44),
      nameof (eVar44),
      nameof (prop45),
      nameof (eVar45),
      nameof (prop46),
      nameof (eVar46),
      nameof (prop47),
      nameof (eVar47),
      nameof (prop48),
      nameof (eVar48),
      nameof (prop49),
      nameof (eVar49),
      nameof (prop50),
      nameof (eVar50),
      nameof (prop51),
      nameof (eVar51),
      nameof (prop52),
      nameof (eVar52),
      nameof (prop53),
      nameof (eVar53),
      nameof (prop54),
      nameof (eVar54),
      nameof (prop55),
      nameof (eVar55),
      nameof (prop56),
      nameof (eVar56),
      nameof (prop57),
      nameof (eVar57),
      nameof (prop58),
      nameof (eVar58),
      nameof (prop59),
      nameof (eVar59),
      nameof (prop60),
      nameof (eVar60),
      nameof (prop61),
      nameof (eVar61),
      nameof (prop62),
      nameof (eVar62),
      nameof (prop63),
      nameof (eVar63),
      nameof (prop64),
      nameof (eVar64),
      nameof (prop65),
      nameof (eVar65),
      nameof (prop66),
      nameof (eVar66),
      nameof (prop67),
      nameof (eVar67),
      nameof (prop68),
      nameof (eVar68),
      nameof (prop69),
      nameof (eVar69),
      nameof (prop70),
      nameof (eVar70),
      nameof (prop71),
      nameof (eVar71),
      nameof (prop72),
      nameof (eVar72),
      nameof (prop73),
      nameof (eVar73),
      nameof (prop74),
      nameof (eVar74),
      nameof (prop75),
      nameof (eVar75),
      nameof (pe),
      nameof (pev1),
      nameof (pev2),
      nameof (pev3),
      nameof (resolution)
    };
    protected string[] lightRequiredVarList = new string[9]
    {
      nameof (timestamp),
      nameof (charSet),
      nameof (visitorNamespace),
      nameof (cookieDomainPeriods),
      nameof (cookieLifetime),
      nameof (contextData),
      nameof (lightProfileID),
      nameof (lightStoreForSeconds),
      nameof (lightIncrementBy)
    };
    protected string[] lightVarList = new string[159]
    {
      nameof (timestamp),
      nameof (charSet),
      nameof (visitorNamespace),
      nameof (cookieDomainPeriods),
      nameof (cookieLifetime),
      nameof (contextData),
      nameof (lightProfileID),
      nameof (lightStoreForSeconds),
      nameof (lightIncrementBy),
      nameof (prop1),
      nameof (eVar1),
      nameof (prop2),
      nameof (eVar2),
      nameof (prop3),
      nameof (eVar3),
      nameof (prop4),
      nameof (eVar4),
      nameof (prop5),
      nameof (eVar5),
      nameof (prop6),
      nameof (eVar6),
      nameof (prop7),
      nameof (eVar7),
      nameof (prop8),
      nameof (eVar8),
      nameof (prop9),
      nameof (eVar9),
      nameof (prop10),
      nameof (eVar10),
      nameof (prop11),
      nameof (eVar11),
      nameof (prop12),
      nameof (eVar12),
      nameof (prop13),
      nameof (eVar13),
      nameof (prop14),
      nameof (eVar14),
      nameof (prop15),
      nameof (eVar15),
      nameof (prop16),
      nameof (eVar16),
      nameof (prop17),
      nameof (eVar17),
      nameof (prop18),
      nameof (eVar18),
      nameof (prop19),
      nameof (eVar19),
      nameof (prop20),
      nameof (eVar20),
      nameof (prop21),
      nameof (eVar21),
      nameof (prop22),
      nameof (eVar22),
      nameof (prop23),
      nameof (eVar23),
      nameof (prop24),
      nameof (eVar24),
      nameof (prop25),
      nameof (eVar25),
      nameof (prop26),
      nameof (eVar26),
      nameof (prop27),
      nameof (eVar27),
      nameof (prop28),
      nameof (eVar28),
      nameof (prop29),
      nameof (eVar29),
      nameof (prop30),
      nameof (eVar30),
      nameof (prop31),
      nameof (eVar31),
      nameof (prop32),
      nameof (eVar32),
      nameof (prop33),
      nameof (eVar33),
      nameof (prop34),
      nameof (eVar34),
      nameof (prop35),
      nameof (eVar35),
      nameof (prop36),
      nameof (eVar36),
      nameof (prop37),
      nameof (eVar37),
      nameof (prop38),
      nameof (eVar38),
      nameof (prop39),
      nameof (eVar39),
      nameof (prop40),
      nameof (eVar40),
      nameof (prop41),
      nameof (eVar41),
      nameof (prop42),
      nameof (eVar42),
      nameof (prop43),
      nameof (eVar43),
      nameof (prop44),
      nameof (eVar44),
      nameof (prop45),
      nameof (eVar45),
      nameof (prop46),
      nameof (eVar46),
      nameof (prop47),
      nameof (eVar47),
      nameof (prop48),
      nameof (eVar48),
      nameof (prop49),
      nameof (eVar49),
      nameof (prop50),
      nameof (eVar50),
      nameof (prop51),
      nameof (eVar51),
      nameof (prop52),
      nameof (eVar52),
      nameof (prop53),
      nameof (eVar53),
      nameof (prop54),
      nameof (eVar54),
      nameof (prop55),
      nameof (eVar55),
      nameof (prop56),
      nameof (eVar56),
      nameof (prop57),
      nameof (eVar57),
      nameof (prop58),
      nameof (eVar58),
      nameof (prop59),
      nameof (eVar59),
      nameof (prop60),
      nameof (eVar60),
      nameof (prop61),
      nameof (eVar61),
      nameof (prop62),
      nameof (eVar62),
      nameof (prop63),
      nameof (eVar63),
      nameof (prop64),
      nameof (eVar64),
      nameof (prop65),
      nameof (eVar65),
      nameof (prop66),
      nameof (eVar66),
      nameof (prop67),
      nameof (eVar67),
      nameof (prop68),
      nameof (eVar68),
      nameof (prop69),
      nameof (eVar69),
      nameof (prop70),
      nameof (eVar70),
      nameof (prop71),
      nameof (eVar71),
      nameof (prop72),
      nameof (eVar72),
      nameof (prop73),
      nameof (eVar73),
      nameof (prop74),
      nameof (eVar74),
      nameof (prop75),
      nameof (eVar75)
    };
    protected string[] accountConfigList = new string[27]
    {
      nameof (account),
      nameof (debugTracking),
      nameof (requestCookiesFilename),
      nameof (trackOffline),
      nameof (offlineLimit),
      nameof (offlineThrottleDelay),
      nameof (offlineFilename),
      nameof (configURL),
      nameof (linkObject),
      nameof (linkURL),
      nameof (linkName),
      nameof (linkType),
      nameof (trackDownloadLinks),
      nameof (trackExternalLinks),
      nameof (trackClickMap),
      nameof (linkLeaveQueryString),
      nameof (linkTrackVars),
      nameof (linkTrackEvents),
      nameof (trackingServer),
      nameof (trackingServerSecure),
      nameof (ssl),
      nameof (mobile),
      nameof (dc),
      nameof (lightTrackVars),
      nameof (userAgent),
      nameof (acceptLanguage),
      nameof (xForwardedFor)
    };

    public string this[string key]
    {
      get
      {
        return this.variables != null && this.variables.ContainsKey(key) ? (string) this.variables[key] : (string) null;
      }
      set
      {
        if (this.variables == null)
          this.variables = new Dictionary<string, object>();
        if (!this.variables.ContainsKey(key))
          this.variables.Add(key, (object) "");
        this.variables[key] = (object) value;
      }
    }

    public string account
    {
      get => this[nameof (account)];
      set => this[nameof (account)] = value;
    }

    public bool debugTracking
    {
      get => this._debugTracking;
      set
      {
        this._debugTracking = value;
        this[nameof (debugTracking)] = value.ToString();
      }
    }

    public string requestCookiesFilename
    {
      get => this[nameof (requestCookiesFilename)];
      set => this[nameof (requestCookiesFilename)] = value;
    }

    public bool trackOffline
    {
      get => this._trackOffline;
      set
      {
        this._trackOffline = value;
        this[nameof (trackOffline)] = value.ToString();
      }
    }

    public int offlineLimit
    {
      get => this._offlineLimit;
      set
      {
        this._offlineLimit = value;
        this[nameof (offlineLimit)] = value != 0 ? value.ToString() : (string) null;
      }
    }

    public int offlineThrottleDelay
    {
      get => this._offlineThrottleDelay;
      set
      {
        this._offlineThrottleDelay = value;
        this[nameof (offlineThrottleDelay)] = value != 0 ? value.ToString() : (string) null;
      }
    }

    public string offlineFilename
    {
      get => this[nameof (offlineFilename)];
      set => this[nameof (offlineFilename)] = value;
    }

    public string configURL
    {
      get => this[nameof (configURL)];
      set => this[nameof (configURL)] = value;
    }

    public string linkObject
    {
      get => this[nameof (linkObject)];
      set => this[nameof (linkObject)] = value;
    }

    public string linkURL
    {
      get => this[nameof (linkURL)];
      set => this[nameof (linkURL)] = value;
    }

    public string linkName
    {
      get => this[nameof (linkName)];
      set => this[nameof (linkName)] = value;
    }

    public string linkType
    {
      get => this[nameof (linkType)];
      set => this[nameof (linkType)] = value;
    }

    public bool trackDownloadLinks
    {
      get => this._trackDownloadLinks;
      set
      {
        this._trackDownloadLinks = value;
        this[nameof (trackDownloadLinks)] = value.ToString();
      }
    }

    public bool trackExternalLinks
    {
      get => this._trackExternalLinks;
      set
      {
        this._trackExternalLinks = value;
        this[nameof (trackExternalLinks)] = value.ToString();
      }
    }

    public bool trackClickMap
    {
      get => this._trackClickMap;
      set
      {
        this._trackClickMap = value;
        this[nameof (trackClickMap)] = value.ToString();
      }
    }

    public bool linkLeaveQueryString
    {
      get => this._linkLeaveQueryString;
      set
      {
        this._linkLeaveQueryString = value;
        this[nameof (linkLeaveQueryString)] = value.ToString();
      }
    }

    public string linkTrackVars
    {
      get => this[nameof (linkTrackVars)];
      set => this[nameof (linkTrackVars)] = value;
    }

    public string linkTrackEvents
    {
      get => this[nameof (linkTrackEvents)];
      set => this[nameof (linkTrackEvents)] = value;
    }

    public string trackingServer
    {
      get => this[nameof (trackingServer)];
      set => this[nameof (trackingServer)] = value;
    }

    public string trackingServerSecure
    {
      get => this[nameof (trackingServerSecure)];
      set => this[nameof (trackingServerSecure)] = value;
    }

    public bool ssl
    {
      get => this._ssl;
      set
      {
        this._ssl = value;
        this[nameof (ssl)] = value.ToString();
      }
    }

    public bool mobile
    {
      get => this._mobile;
      set
      {
        this._mobile = value;
        this[nameof (mobile)] = value.ToString();
      }
    }

    public string dc
    {
      get => this[nameof (dc)];
      set => this[nameof (dc)] = value;
    }

    public string lightTrackVars
    {
      get => this[nameof (lightTrackVars)];
      set => this[nameof (lightTrackVars)] = value;
    }

    public string userAgent
    {
      get => this[nameof (userAgent)];
      set => this[nameof (userAgent)] = value;
    }

    public string acceptLanguage
    {
      get => this[nameof (acceptLanguage)];
      set => this[nameof (acceptLanguage)] = value;
    }

    public string xForwardedFor
    {
      get => this[nameof (xForwardedFor)];
      set => this[nameof (xForwardedFor)] = value;
    }

    public int timestamp
    {
      get => this._timestamp;
      set
      {
        this._timestamp = value;
        this[nameof (timestamp)] = value != 0 ? value.ToString() : (string) null;
      }
    }

    public string dynamicVariablePrefix
    {
      get => this[nameof (dynamicVariablePrefix)];
      set => this[nameof (dynamicVariablePrefix)] = value;
    }

    public string visitorID
    {
      get => this[nameof (visitorID)];
      set => this[nameof (visitorID)] = value;
    }

    public string vmk
    {
      get => this[nameof (vmk)];
      set => this[nameof (vmk)] = value;
    }

    public string visitorMigrationKey
    {
      get => this[nameof (visitorMigrationKey)];
      set => this[nameof (visitorMigrationKey)] = value;
    }

    public string visitorMigrationServer
    {
      get => this[nameof (visitorMigrationServer)];
      set => this[nameof (visitorMigrationServer)] = value;
    }

    public string visitorMigrationServerSecure
    {
      get => this[nameof (visitorMigrationServerSecure)];
      set => this[nameof (visitorMigrationServerSecure)] = value;
    }

    public string charSet
    {
      get => this[nameof (charSet)];
      set => this[nameof (charSet)] = value;
    }

    public string visitorNamespace
    {
      get => this[nameof (visitorNamespace)];
      set => this[nameof (visitorNamespace)] = value;
    }

    public int cookieDomainPeriods
    {
      get => this._cookieDomainPeriods;
      set
      {
        this._cookieDomainPeriods = value;
        this[nameof (cookieDomainPeriods)] = value != 0 ? value.ToString() : (string) null;
      }
    }

    public string cookieLifetime
    {
      get => this[nameof (cookieLifetime)];
      set => this[nameof (cookieLifetime)] = value;
    }

    public string pageName
    {
      get => this[nameof (pageName)];
      set => this[nameof (pageName)] = value;
    }

    public string pageURL
    {
      get => this[nameof (pageURL)];
      set => this[nameof (pageURL)] = value;
    }

    public string referrer
    {
      get => this[nameof (referrer)];
      set => this[nameof (referrer)] = value;
    }

    public Dictionary<string, object> contextData
    {
      get => this._contextData;
      set
      {
        this._contextData = value;
        this[nameof (contextData)] = value != null ? "NOT-NULL" : (string) null;
      }
    }

    public string currencyCode
    {
      get => this[nameof (currencyCode)];
      set => this[nameof (currencyCode)] = value;
    }

    public string lightProfileID
    {
      get => this[nameof (lightProfileID)];
      set => this[nameof (lightProfileID)] = value;
    }

    public int lightStoreForSeconds
    {
      get => this._lightStoreForSeconds;
      set
      {
        this._lightStoreForSeconds = value;
        this[nameof (lightStoreForSeconds)] = value != 0 ? value.ToString() : (string) null;
      }
    }

    public int lightIncrementBy
    {
      get => this._lightIncrementBy;
      set
      {
        this._lightIncrementBy = value;
        this[nameof (lightIncrementBy)] = value != 0 ? value.ToString() : (string) null;
      }
    }

    public string retrieveLightProfiles
    {
      get => this[nameof (retrieveLightProfiles)];
      set => this[nameof (retrieveLightProfiles)] = value;
    }

    public string deleteLightProfiles
    {
      get => this[nameof (deleteLightProfiles)];
      set => this[nameof (deleteLightProfiles)] = value;
    }

    public Dictionary<string, object> retrieveLightData
    {
      get => this._retrieveLightData;
      set
      {
        this._retrieveLightData = value;
        this[nameof (retrieveLightData)] = value != null ? "NOT-NULL" : (string) null;
      }
    }

    public string purchaseID
    {
      get => this[nameof (purchaseID)];
      set => this[nameof (purchaseID)] = value;
    }

    public string variableProvider
    {
      get => this[nameof (variableProvider)];
      set => this[nameof (variableProvider)] = value;
    }

    public string channel
    {
      get => this[nameof (channel)];
      set => this[nameof (channel)] = value;
    }

    public string server
    {
      get => this[nameof (server)];
      set => this[nameof (server)] = value;
    }

    public string pageType
    {
      get => this[nameof (pageType)];
      set => this[nameof (pageType)] = value;
    }

    public string transactionID
    {
      get => this[nameof (transactionID)];
      set => this[nameof (transactionID)] = value;
    }

    public string campaign
    {
      get => this[nameof (campaign)];
      set => this[nameof (campaign)] = value;
    }

    public string state
    {
      get => this[nameof (state)];
      set => this[nameof (state)] = value;
    }

    public string zip
    {
      get => this[nameof (zip)];
      set => this[nameof (zip)] = value;
    }

    public string events
    {
      get => this[nameof (events)];
      set => this[nameof (events)] = value;
    }

    public string events2
    {
      get => this[nameof (events2)];
      set => this[nameof (events2)] = value;
    }

    public string products
    {
      get => this[nameof (products)];
      set => this[nameof (products)] = value;
    }

    public string tnt
    {
      get => this[nameof (tnt)];
      set => this[nameof (tnt)] = value;
    }

    public string prop1
    {
      get => this[nameof (prop1)];
      set => this[nameof (prop1)] = value;
    }

    public string eVar1
    {
      get => this[nameof (eVar1)];
      set => this[nameof (eVar1)] = value;
    }

    public string hier1
    {
      get => this[nameof (hier1)];
      set => this[nameof (hier1)] = value;
    }

    public string list1
    {
      get => this[nameof (list1)];
      set => this[nameof (list1)] = value;
    }

    public string prop2
    {
      get => this[nameof (prop2)];
      set => this[nameof (prop2)] = value;
    }

    public string eVar2
    {
      get => this[nameof (eVar2)];
      set => this[nameof (eVar2)] = value;
    }

    public string hier2
    {
      get => this[nameof (hier2)];
      set => this[nameof (hier2)] = value;
    }

    public string list2
    {
      get => this[nameof (list2)];
      set => this[nameof (list2)] = value;
    }

    public string prop3
    {
      get => this[nameof (prop3)];
      set => this[nameof (prop3)] = value;
    }

    public string eVar3
    {
      get => this[nameof (eVar3)];
      set => this[nameof (eVar3)] = value;
    }

    public string hier3
    {
      get => this[nameof (hier3)];
      set => this[nameof (hier3)] = value;
    }

    public string list3
    {
      get => this[nameof (list3)];
      set => this[nameof (list3)] = value;
    }

    public string prop4
    {
      get => this[nameof (prop4)];
      set => this[nameof (prop4)] = value;
    }

    public string eVar4
    {
      get => this[nameof (eVar4)];
      set => this[nameof (eVar4)] = value;
    }

    public string hier4
    {
      get => this[nameof (hier4)];
      set => this[nameof (hier4)] = value;
    }

    public string prop5
    {
      get => this[nameof (prop5)];
      set => this[nameof (prop5)] = value;
    }

    public string eVar5
    {
      get => this[nameof (eVar5)];
      set => this[nameof (eVar5)] = value;
    }

    public string hier5
    {
      get => this[nameof (hier5)];
      set => this[nameof (hier5)] = value;
    }

    public string prop6
    {
      get => this[nameof (prop6)];
      set => this[nameof (prop6)] = value;
    }

    public string eVar6
    {
      get => this[nameof (eVar6)];
      set => this[nameof (eVar6)] = value;
    }

    public string prop7
    {
      get => this[nameof (prop7)];
      set => this[nameof (prop7)] = value;
    }

    public string eVar7
    {
      get => this[nameof (eVar7)];
      set => this[nameof (eVar7)] = value;
    }

    public string prop8
    {
      get => this[nameof (prop8)];
      set => this[nameof (prop8)] = value;
    }

    public string eVar8
    {
      get => this[nameof (eVar8)];
      set => this[nameof (eVar8)] = value;
    }

    public string prop9
    {
      get => this[nameof (prop9)];
      set => this[nameof (prop9)] = value;
    }

    public string eVar9
    {
      get => this[nameof (eVar9)];
      set => this[nameof (eVar9)] = value;
    }

    public string prop10
    {
      get => this[nameof (prop10)];
      set => this[nameof (prop10)] = value;
    }

    public string eVar10
    {
      get => this[nameof (eVar10)];
      set => this[nameof (eVar10)] = value;
    }

    public string prop11
    {
      get => this[nameof (prop11)];
      set => this[nameof (prop11)] = value;
    }

    public string eVar11
    {
      get => this[nameof (eVar11)];
      set => this[nameof (eVar11)] = value;
    }

    public string prop12
    {
      get => this[nameof (prop12)];
      set => this[nameof (prop12)] = value;
    }

    public string eVar12
    {
      get => this[nameof (eVar12)];
      set => this[nameof (eVar12)] = value;
    }

    public string prop13
    {
      get => this[nameof (prop13)];
      set => this[nameof (prop13)] = value;
    }

    public string eVar13
    {
      get => this[nameof (eVar13)];
      set => this[nameof (eVar13)] = value;
    }

    public string prop14
    {
      get => this[nameof (prop14)];
      set => this[nameof (prop14)] = value;
    }

    public string eVar14
    {
      get => this[nameof (eVar14)];
      set => this[nameof (eVar14)] = value;
    }

    public string prop15
    {
      get => this[nameof (prop15)];
      set => this[nameof (prop15)] = value;
    }

    public string eVar15
    {
      get => this[nameof (eVar15)];
      set => this[nameof (eVar15)] = value;
    }

    public string prop16
    {
      get => this[nameof (prop16)];
      set => this[nameof (prop16)] = value;
    }

    public string eVar16
    {
      get => this[nameof (eVar16)];
      set => this[nameof (eVar16)] = value;
    }

    public string prop17
    {
      get => this[nameof (prop17)];
      set => this[nameof (prop17)] = value;
    }

    public string eVar17
    {
      get => this[nameof (eVar17)];
      set => this[nameof (eVar17)] = value;
    }

    public string prop18
    {
      get => this[nameof (prop18)];
      set => this[nameof (prop18)] = value;
    }

    public string eVar18
    {
      get => this[nameof (eVar18)];
      set => this[nameof (eVar18)] = value;
    }

    public string prop19
    {
      get => this[nameof (prop19)];
      set => this[nameof (prop19)] = value;
    }

    public string eVar19
    {
      get => this[nameof (eVar19)];
      set => this[nameof (eVar19)] = value;
    }

    public string prop20
    {
      get => this[nameof (prop20)];
      set => this[nameof (prop20)] = value;
    }

    public string eVar20
    {
      get => this[nameof (eVar20)];
      set => this[nameof (eVar20)] = value;
    }

    public string prop21
    {
      get => this[nameof (prop21)];
      set => this[nameof (prop21)] = value;
    }

    public string eVar21
    {
      get => this[nameof (eVar21)];
      set => this[nameof (eVar21)] = value;
    }

    public string prop22
    {
      get => this[nameof (prop22)];
      set => this[nameof (prop22)] = value;
    }

    public string eVar22
    {
      get => this[nameof (eVar22)];
      set => this[nameof (eVar22)] = value;
    }

    public string prop23
    {
      get => this[nameof (prop23)];
      set => this[nameof (prop23)] = value;
    }

    public string eVar23
    {
      get => this[nameof (eVar23)];
      set => this[nameof (eVar23)] = value;
    }

    public string prop24
    {
      get => this[nameof (prop24)];
      set => this[nameof (prop24)] = value;
    }

    public string eVar24
    {
      get => this[nameof (eVar24)];
      set => this[nameof (eVar24)] = value;
    }

    public string prop25
    {
      get => this[nameof (prop25)];
      set => this[nameof (prop25)] = value;
    }

    public string eVar25
    {
      get => this[nameof (eVar25)];
      set => this[nameof (eVar25)] = value;
    }

    public string prop26
    {
      get => this[nameof (prop26)];
      set => this[nameof (prop26)] = value;
    }

    public string eVar26
    {
      get => this[nameof (eVar26)];
      set => this[nameof (eVar26)] = value;
    }

    public string prop27
    {
      get => this[nameof (prop27)];
      set => this[nameof (prop27)] = value;
    }

    public string eVar27
    {
      get => this[nameof (eVar27)];
      set => this[nameof (eVar27)] = value;
    }

    public string prop28
    {
      get => this[nameof (prop28)];
      set => this[nameof (prop28)] = value;
    }

    public string eVar28
    {
      get => this[nameof (eVar28)];
      set => this[nameof (eVar28)] = value;
    }

    public string prop29
    {
      get => this[nameof (prop29)];
      set => this[nameof (prop29)] = value;
    }

    public string eVar29
    {
      get => this[nameof (eVar29)];
      set => this[nameof (eVar29)] = value;
    }

    public string prop30
    {
      get => this[nameof (prop30)];
      set => this[nameof (prop30)] = value;
    }

    public string eVar30
    {
      get => this[nameof (eVar30)];
      set => this[nameof (eVar30)] = value;
    }

    public string prop31
    {
      get => this[nameof (prop31)];
      set => this[nameof (prop31)] = value;
    }

    public string eVar31
    {
      get => this[nameof (eVar31)];
      set => this[nameof (eVar31)] = value;
    }

    public string prop32
    {
      get => this[nameof (prop32)];
      set => this[nameof (prop32)] = value;
    }

    public string eVar32
    {
      get => this[nameof (eVar32)];
      set => this[nameof (eVar32)] = value;
    }

    public string prop33
    {
      get => this[nameof (prop33)];
      set => this[nameof (prop33)] = value;
    }

    public string eVar33
    {
      get => this[nameof (eVar33)];
      set => this[nameof (eVar33)] = value;
    }

    public string prop34
    {
      get => this[nameof (prop34)];
      set => this[nameof (prop34)] = value;
    }

    public string eVar34
    {
      get => this[nameof (eVar34)];
      set => this[nameof (eVar34)] = value;
    }

    public string prop35
    {
      get => this[nameof (prop35)];
      set => this[nameof (prop35)] = value;
    }

    public string eVar35
    {
      get => this[nameof (eVar35)];
      set => this[nameof (eVar35)] = value;
    }

    public string prop36
    {
      get => this[nameof (prop36)];
      set => this[nameof (prop36)] = value;
    }

    public string eVar36
    {
      get => this[nameof (eVar36)];
      set => this[nameof (eVar36)] = value;
    }

    public string prop37
    {
      get => this[nameof (prop37)];
      set => this[nameof (prop37)] = value;
    }

    public string eVar37
    {
      get => this[nameof (eVar37)];
      set => this[nameof (eVar37)] = value;
    }

    public string prop38
    {
      get => this[nameof (prop38)];
      set => this[nameof (prop38)] = value;
    }

    public string eVar38
    {
      get => this[nameof (eVar38)];
      set => this[nameof (eVar38)] = value;
    }

    public string prop39
    {
      get => this[nameof (prop39)];
      set => this[nameof (prop39)] = value;
    }

    public string eVar39
    {
      get => this[nameof (eVar39)];
      set => this[nameof (eVar39)] = value;
    }

    public string prop40
    {
      get => this[nameof (prop40)];
      set => this[nameof (prop40)] = value;
    }

    public string eVar40
    {
      get => this[nameof (eVar40)];
      set => this[nameof (eVar40)] = value;
    }

    public string prop41
    {
      get => this[nameof (prop41)];
      set => this[nameof (prop41)] = value;
    }

    public string eVar41
    {
      get => this[nameof (eVar41)];
      set => this[nameof (eVar41)] = value;
    }

    public string prop42
    {
      get => this[nameof (prop42)];
      set => this[nameof (prop42)] = value;
    }

    public string eVar42
    {
      get => this[nameof (eVar42)];
      set => this[nameof (eVar42)] = value;
    }

    public string prop43
    {
      get => this[nameof (prop43)];
      set => this[nameof (prop43)] = value;
    }

    public string eVar43
    {
      get => this[nameof (eVar43)];
      set => this[nameof (eVar43)] = value;
    }

    public string prop44
    {
      get => this[nameof (prop44)];
      set => this[nameof (prop44)] = value;
    }

    public string eVar44
    {
      get => this[nameof (eVar44)];
      set => this[nameof (eVar44)] = value;
    }

    public string prop45
    {
      get => this[nameof (prop45)];
      set => this[nameof (prop45)] = value;
    }

    public string eVar45
    {
      get => this[nameof (eVar45)];
      set => this[nameof (eVar45)] = value;
    }

    public string prop46
    {
      get => this[nameof (prop46)];
      set => this[nameof (prop46)] = value;
    }

    public string eVar46
    {
      get => this[nameof (eVar46)];
      set => this[nameof (eVar46)] = value;
    }

    public string prop47
    {
      get => this[nameof (prop47)];
      set => this[nameof (prop47)] = value;
    }

    public string eVar47
    {
      get => this[nameof (eVar47)];
      set => this[nameof (eVar47)] = value;
    }

    public string prop48
    {
      get => this[nameof (prop48)];
      set => this[nameof (prop48)] = value;
    }

    public string eVar48
    {
      get => this[nameof (eVar48)];
      set => this[nameof (eVar48)] = value;
    }

    public string prop49
    {
      get => this[nameof (prop49)];
      set => this[nameof (prop49)] = value;
    }

    public string eVar49
    {
      get => this[nameof (eVar49)];
      set => this[nameof (eVar49)] = value;
    }

    public string prop50
    {
      get => this[nameof (prop50)];
      set => this[nameof (prop50)] = value;
    }

    public string eVar50
    {
      get => this[nameof (eVar50)];
      set => this[nameof (eVar50)] = value;
    }

    public string prop51
    {
      get => this[nameof (prop51)];
      set => this[nameof (prop51)] = value;
    }

    public string eVar51
    {
      get => this[nameof (eVar51)];
      set => this[nameof (eVar51)] = value;
    }

    public string prop52
    {
      get => this[nameof (prop52)];
      set => this[nameof (prop52)] = value;
    }

    public string eVar52
    {
      get => this[nameof (eVar52)];
      set => this[nameof (eVar52)] = value;
    }

    public string prop53
    {
      get => this[nameof (prop53)];
      set => this[nameof (prop53)] = value;
    }

    public string eVar53
    {
      get => this[nameof (eVar53)];
      set => this[nameof (eVar53)] = value;
    }

    public string prop54
    {
      get => this[nameof (prop54)];
      set => this[nameof (prop54)] = value;
    }

    public string eVar54
    {
      get => this[nameof (eVar54)];
      set => this[nameof (eVar54)] = value;
    }

    public string prop55
    {
      get => this[nameof (prop55)];
      set => this[nameof (prop55)] = value;
    }

    public string eVar55
    {
      get => this[nameof (eVar55)];
      set => this[nameof (eVar55)] = value;
    }

    public string prop56
    {
      get => this[nameof (prop56)];
      set => this[nameof (prop56)] = value;
    }

    public string eVar56
    {
      get => this[nameof (eVar56)];
      set => this[nameof (eVar56)] = value;
    }

    public string prop57
    {
      get => this[nameof (prop57)];
      set => this[nameof (prop57)] = value;
    }

    public string eVar57
    {
      get => this[nameof (eVar57)];
      set => this[nameof (eVar57)] = value;
    }

    public string prop58
    {
      get => this[nameof (prop58)];
      set => this[nameof (prop58)] = value;
    }

    public string eVar58
    {
      get => this[nameof (eVar58)];
      set => this[nameof (eVar58)] = value;
    }

    public string prop59
    {
      get => this[nameof (prop59)];
      set => this[nameof (prop59)] = value;
    }

    public string eVar59
    {
      get => this[nameof (eVar59)];
      set => this[nameof (eVar59)] = value;
    }

    public string prop60
    {
      get => this[nameof (prop60)];
      set => this[nameof (prop60)] = value;
    }

    public string eVar60
    {
      get => this[nameof (eVar60)];
      set => this[nameof (eVar60)] = value;
    }

    public string prop61
    {
      get => this[nameof (prop61)];
      set => this[nameof (prop61)] = value;
    }

    public string eVar61
    {
      get => this[nameof (eVar61)];
      set => this[nameof (eVar61)] = value;
    }

    public string prop62
    {
      get => this[nameof (prop62)];
      set => this[nameof (prop62)] = value;
    }

    public string eVar62
    {
      get => this[nameof (eVar62)];
      set => this[nameof (eVar62)] = value;
    }

    public string prop63
    {
      get => this[nameof (prop63)];
      set => this[nameof (prop63)] = value;
    }

    public string eVar63
    {
      get => this[nameof (eVar63)];
      set => this[nameof (eVar63)] = value;
    }

    public string prop64
    {
      get => this[nameof (prop64)];
      set => this[nameof (prop64)] = value;
    }

    public string eVar64
    {
      get => this[nameof (eVar64)];
      set => this[nameof (eVar64)] = value;
    }

    public string prop65
    {
      get => this[nameof (prop65)];
      set => this[nameof (prop65)] = value;
    }

    public string eVar65
    {
      get => this[nameof (eVar65)];
      set => this[nameof (eVar65)] = value;
    }

    public string prop66
    {
      get => this[nameof (prop66)];
      set => this[nameof (prop66)] = value;
    }

    public string eVar66
    {
      get => this[nameof (eVar66)];
      set => this[nameof (eVar66)] = value;
    }

    public string prop67
    {
      get => this[nameof (prop67)];
      set => this[nameof (prop67)] = value;
    }

    public string eVar67
    {
      get => this[nameof (eVar67)];
      set => this[nameof (eVar67)] = value;
    }

    public string prop68
    {
      get => this[nameof (prop68)];
      set => this[nameof (prop68)] = value;
    }

    public string eVar68
    {
      get => this[nameof (eVar68)];
      set => this[nameof (eVar68)] = value;
    }

    public string prop69
    {
      get => this[nameof (prop69)];
      set => this[nameof (prop69)] = value;
    }

    public string eVar69
    {
      get => this[nameof (eVar69)];
      set => this[nameof (eVar69)] = value;
    }

    public string prop70
    {
      get => this[nameof (prop70)];
      set => this[nameof (prop70)] = value;
    }

    public string eVar70
    {
      get => this[nameof (eVar70)];
      set => this[nameof (eVar70)] = value;
    }

    public string prop71
    {
      get => this[nameof (prop71)];
      set => this[nameof (prop71)] = value;
    }

    public string eVar71
    {
      get => this[nameof (eVar71)];
      set => this[nameof (eVar71)] = value;
    }

    public string prop72
    {
      get => this[nameof (prop72)];
      set => this[nameof (prop72)] = value;
    }

    public string eVar72
    {
      get => this[nameof (eVar72)];
      set => this[nameof (eVar72)] = value;
    }

    public string prop73
    {
      get => this[nameof (prop73)];
      set => this[nameof (prop73)] = value;
    }

    public string eVar73
    {
      get => this[nameof (eVar73)];
      set => this[nameof (eVar73)] = value;
    }

    public string prop74
    {
      get => this[nameof (prop74)];
      set => this[nameof (prop74)] = value;
    }

    public string eVar74
    {
      get => this[nameof (eVar74)];
      set => this[nameof (eVar74)] = value;
    }

    public string prop75
    {
      get => this[nameof (prop75)];
      set => this[nameof (prop75)] = value;
    }

    public string eVar75
    {
      get => this[nameof (eVar75)];
      set => this[nameof (eVar75)] = value;
    }

    public string pe
    {
      get => this[nameof (pe)];
      set => this[nameof (pe)] = value;
    }

    public string pev1
    {
      get => this[nameof (pev1)];
      set => this[nameof (pev1)] = value;
    }

    public string pev2
    {
      get => this[nameof (pev2)];
      set => this[nameof (pev2)] = value;
    }

    public string pev3
    {
      get => this[nameof (pev3)];
      set => this[nameof (pev3)] = value;
    }

    public string resolution
    {
      get => this[nameof (resolution)];
      set => this[nameof (resolution)] = value;
    }
  }
}
