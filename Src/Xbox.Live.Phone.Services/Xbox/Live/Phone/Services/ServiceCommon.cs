// *********************************************************
// Type: Xbox.Live.Phone.Services.ServiceCommon
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.Silverlight.XLiveWeb;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services
{
  public static class ServiceCommon
  {
    public const string XmlContentType = "application/xml";
    public const string JsonContentType = "application/json";
    public const string ContentType = "Content-Type";
    public const string AuthenticationHeader = "X-PartnerAuthorization";
    public const string AuthenticationPrefix = "XBL1.0 x=";
    public const string CacheControl = "Cache-Control";
    public const string LocaleHeader = "X-Locale";
    public const string PlatformTypeHeader = "X-Platform-Type";
    public const string MobilePlatformType = "5";
    public const string HttpPostVerb = "POST";
    public const string AvatarServiceAudienceUri = "http://xboxlive.com/avatar";
    public const string ProfileServiceAudienceUri = "http://xboxlive.com/userdata";
    public const string MessageServiceAudienceUri = "http://xboxlive.com/userdata";
    public const string GameDataAudienceUri = "http://xboxlive.com/userdata";
    public const string FriendServiceAudienceUri = "http://xboxlive.com/userdata";
    public const string XSTSAudienceUri = "http://xboxlive.com";
    public const int BlockingServiceTimeOut = 30000;
    public const string SilverAccountString = "Silver";

    public static string GetEnvironmentName(ServiceCommon.Environment environment)
    {
      switch (environment)
      {
        case ServiceCommon.Environment.Storax:
          return "Storax";
        case ServiceCommon.Environment.TestNet:
          return "TestNet";
        case ServiceCommon.Environment.INT2:
          return "INT2";
        case ServiceCommon.Environment.StressNet:
          return "StressNet";
        case ServiceCommon.Environment.PartnerNet:
          return "PartnerNet";
        case ServiceCommon.Environment.Production:
          return "Production";
        case ServiceCommon.Environment.Dev:
          return "Dev";
        case ServiceCommon.Environment.CertNet:
          return "CertNet";
        case ServiceCommon.Environment.VINT:
          return "VINT";
        default:
          throw new ArgumentException("Invalid environment name");
      }
    }

    [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "This is not URL")]
    public static string GetEnvironmentUrlStringPrefix(ServiceCommon.Environment environment)
    {
      switch (environment)
      {
        case ServiceCommon.Environment.Storax:
          return ".storax";
        case ServiceCommon.Environment.TestNet:
          return ".test";
        case ServiceCommon.Environment.INT2:
          return ".int2";
        case ServiceCommon.Environment.StressNet:
          return ".stress";
        case ServiceCommon.Environment.PartnerNet:
          return ".part";
        case ServiceCommon.Environment.Production:
          return string.Empty;
        case ServiceCommon.Environment.Dev:
          return ".dev";
        case ServiceCommon.Environment.CertNet:
          return ".cert";
        case ServiceCommon.Environment.VINT:
          return ".vint";
        default:
          throw new ArgumentException("Invalid environment name");
      }
    }

    public static T ReadStubData<T>(string resourceName)
    {
      T obj = default (T);
      XmlReader reader = (XmlReader) null;
      DataContractSerializer contractSerializer = new DataContractSerializer(typeof (T));
      try
      {
        reader = XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName));
        return (T) contractSerializer.ReadObject(reader);
      }
      finally
      {
        reader?.Close();
      }
    }

    public static string ReadResource(string resourceName)
    {
      StreamReader streamReader = (StreamReader) null;
      string empty = string.Empty;
      try
      {
        streamReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName));
        return streamReader.ReadToEnd();
      }
      finally
      {
        streamReader?.Close();
      }
    }

    public static WebHeaderCollection CreateWebHeaders()
    {
      WebHeaderCollection webHeaders = new WebHeaderCollection();
      webHeaders["X-Locale"] = CultureInfo.CurrentUICulture.Name;
      webHeaders["X-Platform-Type"] = "5";
      webHeaders["Cache-Control"] = "no-store, no-cache, must-revalidate";
      webHeaders["PRAGMA"] = "no-cache";
      return webHeaders;
    }

    public static XLiveMobileException HandleServiceException(
      Exception ex,
      XLiveMobileExceptionEnum code,
      string internalDescription,
      string[] args)
    {
      if (ex is XLiveMobileException xliveMobileException)
        return xliveMobileException;
      XLiveMobileException exception = XLiveMobileException.CreateException(ex, (int) code, internalDescription, args);
      if (ex is WebException webException)
      {
        exception.StatusCode = (int) webException.Status;
        return exception;
      }
      if (ex is XLiveHttpWebException webEx)
      {
        exception.StatusCode = (int) webEx.StatusCode;
        uint errorCode;
        string message;
        if (ServiceCommon.ExtractUdsErrorCode(webEx, out errorCode, out message))
        {
          exception.ServiceErrorMessage = message;
          exception.ServiceErrorCode = errorCode;
        }
        return exception;
      }
      exception.StatusCode = 16;
      return exception;
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "None critical information. Prevent app failure.")]
    [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Convenience method not used by external teams.")]
    public static bool ExtractUdsErrorCode(
      XLiveHttpWebException webEx,
      out uint errorCode,
      out string message)
    {
      errorCode = 0U;
      message = string.Empty;
      bool udsErrorCode = false;
      if (webEx != null && !string.IsNullOrEmpty(webEx.ResponseBody))
      {
        using (Stream stream = (Stream) new MemoryStream(Encoding.Unicode.GetBytes(webEx.ResponseBody)))
        {
          DataContractSerializer contractSerializer = new DataContractSerializer(typeof (ServiceErrorMessage));
          if (contractSerializer != null)
          {
            if (stream != null)
            {
              try
              {
                ServiceErrorMessage serviceErrorMessage = (ServiceErrorMessage) contractSerializer.ReadObject(stream);
                if (serviceErrorMessage != null)
                {
                  errorCode = serviceErrorMessage.LIVEnErrorCode;
                  message = serviceErrorMessage.ErrorMessage;
                  udsErrorCode = true;
                }
              }
              catch (Exception ex)
              {
                udsErrorCode = false;
              }
            }
          }
        }
      }
      return udsErrorCode;
    }

    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "serviceMethod", Justification = "The parameter is used in WriteLine(), but that method only exists in DEBUG builds.")]
    public static bool CheckForErrorFromService(Exception ex, string serviceMethod)
    {
      return ServiceCommon.CheckForErrorFromService(ex, serviceMethod, (IErrorManager) ErrorManager.Instance);
    }

    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "serviceMethod", Justification = "The parameter is used in WriteLine(), but that method only exists in DEBUG builds.")]
    public static bool CheckForErrorFromService(
      Exception ex,
      string serviceMethod,
      IErrorManager errorManager)
    {
      bool flag = false;
      if (ex == null)
        return false;
      XLiveMobileException xliveMobileException = ex as XLiveMobileException;
      string title;
      string body;
      XLiveMobileException.GetExceptionInfo(ex, out title, out body);
      if (xliveMobileException != null)
        flag = xliveMobileException.ShouldDisplay;
      if (flag && errorManager != null)
        errorManager.Nonfatal(title, body);
      return true;
    }

    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "This is enum definition")]
    public enum Environment
    {
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Storax", Justification = "Our envirionment name")] Storax,
      TestNet,
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "INT", Justification = "Our envirionment name")] INT2,
      StressNet,
      PartnerNet,
      Production,
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DEV", Justification = "Our envirionment name")] Dev,
      CertNet,
      VINT,
    }

    [Flags]
    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Used in too many places to make this change worthwhile.")]
    public enum ProfileSections
    {
      XboxLiveProperties = 1,
      RecentGames = 8,
      RecentAchievements = 16, // 0x00000010
      PresenceInfo = 32, // 0x00000020
      PrivacySettings = 64, // 0x00000040
      Friends = 128, // 0x00000080
    }

    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Used in too many places to make this change worthwhile.")]
    [Flags]
    public enum GamerDataSections
    {
      Profile = 268435456, // 0x10000000
      Friends = 536870912, // 0x20000000
      Avatar = 1073741824, // 0x40000000
      Messages = 16777216, // 0x01000000
      Games = 33554432, // 0x02000000
      GameAchievements = 67108864, // 0x04000000
      AvatarClosetAssets = 134217728, // 0x08000000
      ProfileReduced = Profile, // 0x10000000
    }

    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Used in too many places to make this change worthwhile.")]
    public enum OnlineState
    {
      Online,
      Offline,
      Away,
      Busy,
      Unknown,
    }
  }
}
