// *********************************************************
// Type: Xbox.Live.Phone.Services.ProfileServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Gds.Contracts;
using Leet.Silverlight.XLiveWeb;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services
{
  public sealed class ProfileServiceManager : IProfileServiceManager
  {
    private const string ComponentName = "ProfileServiceManager";
    private const string GetProfileUriBase = "https://uds-part{0}.xboxlive.com/Profile.svc/profile?sectionFlags={1}";
    private const string GetProfileUriExtension = "&gamerTag={2}";
    private const string UpdateProfileUriBase = "https://uds-part{0}.xboxlive.com/Profile.svc/";
    private const string UpdatePresenceUri = "https://uds-part{0}.xboxlive.com/Presence.svc/update?";
    private const string GamerTagChangeUri = "https://uds-part{0}.xboxlive.com/Profile.svc/gamertag/change?gamertag={1}";
    private static AtomicCounter ongoingRequestCounter = new AtomicCounter(0);
    private string currentEnvironmentPrefix = string.Empty;

    public event EventHandler<ServiceProxyEventArgs<ProfileEx>> EventGetProfileCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventUpdateProfileCompleted;

    public event EventHandler<ServiceProxyEventArgs<string[]>> EventChangeGamerTagCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventUpdatePresenceCompleted;

    public WebHeaderCollection WebHeaders { get; set; }

    private static string PartnerToken
    {
      get => XboxLiveGamer.GetPartnerToken("http://xboxlive.com/userdata");
    }

    public void Initialize(ServiceCommon.Environment environmentName)
    {
      this.currentEnvironmentPrefix = ServiceCommon.GetEnvironmentUrlStringPrefix(environmentName);
      this.WebHeaders = ServiceCommon.CreateWebHeaders();
    }

    public void GetProfileAsync(string gamerTag, long sectionFlags)
    {
      if (sectionFlags == 0L)
        throw new ArgumentException("Invalid sectionFlags");
      if (XboxLiveGamer.CurrentGamer == null || XboxLiveGamer.CurrentGamer.GamerTag == null)
        throw new InvalidOperationException("shouldn't call this before the gamer has signed in");
      string gamerTagToUse = XboxLiveGamer.CurrentGamer.GamerTag.Equals(gamerTag, StringComparison.OrdinalIgnoreCase) ? string.Empty : gamerTag;
      ProfileServiceManager.ongoingRequestCounter.Plus(1);
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        string empty = string.Empty;
        string uriString;
        if (sectionFlags == 128L || string.IsNullOrEmpty(gamerTagToUse))
          uriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Profile.svc/profile?sectionFlags={1}", new object[2]
          {
            (object) this.currentEnvironmentPrefix,
            (object) sectionFlags
          });
        else
          uriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Profile.svc/profile?sectionFlags={1}&gamerTag={2}", new object[3]
          {
            (object) this.currentEnvironmentPrefix,
            (object) sectionFlags,
            (object) gamerTagToUse
          });
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(uriString), HttpStack.PlatformDefault);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + ProfileServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnGetProfileCompleted);
        httpClient.GetDataContractAsync<ProfileEx>();
      });
    }

    public void UpdateProfileAsync(ProfileEx profileUpdates)
    {
      if (profileUpdates == null)
        throw new ArgumentException("Invalid profileUpdates");
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Profile.svc/", new object[1]
        {
          (object) this.currentEnvironmentPrefix
        })), HttpStack.PlatformDefault);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + ProfileServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.ContentType = "application/xml";
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnUpdateProfileCompleted);
        httpClient.PostDataContractAsync<ProfileEx, string>((object) profileUpdates);
      });
    }

    public void ChangeGamerTag(string desiredGamerTag)
    {
      if (string.IsNullOrEmpty(desiredGamerTag))
        throw new ArgumentException("Gamertag is invalid");
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Profile.svc/gamertag/change?gamertag={1}", new object[2]
        {
          (object) this.currentEnvironmentPrefix,
          (object) desiredGamerTag
        })), HttpStack.PlatformDefault);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + ProfileServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.ContentType = "application/xml";
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnGamerTagChangeCompleted);
        httpClient.PostDataContractAsync<string, string>((object) string.Empty);
      });
    }

    public bool IsLoading() => ProfileServiceManager.ongoingRequestCounter.Count > 0;

    public void UpdatePresence()
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://uds-part{0}.xboxlive.com/Presence.svc/update?", new object[1]
        {
          (object) this.currentEnvironmentPrefix
        })), HttpStack.PlatformDefault);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + ProfileServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.ContentType = "application/xml";
        string empty = string.Empty;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnUpdatePresenceCompleted);
        httpClient.PostDataContractAsync<string, string>((object) empty);
      });
    }

    private static string[] ParseChangeGamerTagResponse(string responseXml)
    {
      if (string.IsNullOrEmpty(responseXml))
        return (string[]) null;
      string[] gamerTagResponse = (string[]) null;
      try
      {
        using (StringReader stringReader = new StringReader(responseXml))
          gamerTagResponse = (string[]) new XmlSerializer(typeof (string[])).Deserialize((TextReader) stringReader);
      }
      catch (ArgumentException ex)
      {
      }
      catch (InvalidOperationException ex)
      {
      }
      catch (XmlException ex)
      {
      }
      return gamerTagResponse;
    }

    private void HttpClient_OnGetProfileCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<ProfileEx>> profileCompleted = this.EventGetProfileCompleted;
      ProfileServiceManager.ongoingRequestCounter.Subtract(1);
      if (profileCompleted != null)
      {
        ServiceProxyEventArgs<ProfileEx> e1;
        if (e.ResultAvailable)
          e1 = new ServiceProxyEventArgs<ProfileEx>(e.Result, (Exception) null, false, (object) null);
        else if (e.Error is XLiveHttpWebException)
        {
          XLiveHttpWebException error = e.Error as XLiveHttpWebException;
          e1 = error.StatusCode == HttpStatusCode.BadRequest || error.StatusCode == HttpStatusCode.InternalServerError ? new ServiceProxyEventArgs<ProfileEx>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToLocateGamerTag, "Failed to locate gamertag", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<ProfileEx>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToGetProfile, "Failed to get profile", (string[]) null), false, (object) null);
        }
        else
          e1 = new ServiceProxyEventArgs<ProfileEx>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToGetProfile, "Failed to get profile", (string[]) null), false, (object) null);
        profileCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }

    private void HttpClient_OnUpdateProfileCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<string>> profileCompleted = this.EventUpdateProfileCompleted;
      Exception error = e.Error;
      if (profileCompleted != null)
      {
        ServiceProxyEventArgs<string> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<string>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToUpdateProfile, "Failed to update profile", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<string>(e.Result, (Exception) null, false, (object) null);
        profileCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }

    private void HttpClient_OnGamerTagChangeCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<string[]>> gamerTagCompleted = this.EventChangeGamerTagCompleted;
      Exception error = e.Error;
      if (gamerTagCompleted != null)
      {
        ServiceProxyEventArgs<string[]> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<string[]>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToChangeGamerTag, "Failed to change profile", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<string[]>((object) ProfileServiceManager.ParseChangeGamerTagResponse((string) e.Result), (Exception) null, false, (object) null);
        gamerTagCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }

    private void HttpClient_OnUpdatePresenceCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<string>> presenceCompleted = this.EventUpdatePresenceCompleted;
      if (presenceCompleted != null)
      {
        ServiceProxyEventArgs<string> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<string>((object) null, (Exception) ServiceCommon.HandleServiceException(e.Error, XLiveMobileExceptionEnum.FailedToUpdatePresence, "Failed to update presence", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<string>(e.Result, (Exception) null, false, (object) null);
        presenceCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }
  }
}
