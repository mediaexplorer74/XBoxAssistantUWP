// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.XmediaServiceRestApi
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using Microsoft.XMedia.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Resources;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  internal class XmediaServiceRestApi
  {
    public const string Api2011Version = "2011-08-10";
    public const string XstsTokenInBodyHeaderValue = "XBL2.0BODY";
    public const string CompanionApiVersion = "2011-08-10";
    private Uri baseUri;
    private IClientTracer tracer;

    public Uri BaseUri => this.baseUri;

    public string ActivityId { get; private set; }

    public XmediaServiceRestApi(IClientTracer tracer)
    {
      this.tracer = tracer;
      this.baseUri = new Uri("https://xlink.vint.xboxlive.com");
      StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri("xMediaServiceUri.txt", UriKind.RelativeOrAbsolute));
      if (resourceStream == null)
        return;
      using (StreamReader streamReader = new StreamReader(resourceStream.Stream))
      {
        string uriString = streamReader.ReadLine();
        if (uriString == null)
          return;
        this.baseUri = new Uri(uriString);
      }
    }

    public void SetXboxLiveEnvironment(string envName)
    {
      switch (envName)
      {
        case "Storax":
          this.SetXboxLiveEnvironmentUri(new Uri("https://xlink.vint.xboxlive.com"));
          break;
        case "TestNet":
          this.SetXboxLiveEnvironmentUri(new Uri("https://xlink.vint.xboxlive.com"));
          break;
        case "VINT":
          this.SetXboxLiveEnvironmentUri(new Uri("https://xlink.vint.xboxlive.com"));
          break;
        case "INT2":
          this.SetXboxLiveEnvironmentUri(new Uri("https://xlink.vint.xboxlive.com"));
          break;
        case "INT2 (HTTP)":
          this.SetXboxLiveEnvironmentUri(new Uri("http://xlink.vint.xboxlive.com"));
          break;
        case "StressNet":
          this.SetXboxLiveEnvironmentUri(new Uri("https://xlink.vint.xboxlive.com"));
          break;
        case "PartnerNet":
          this.SetXboxLiveEnvironmentUri(new Uri("https://xlink.part.xboxlive.com"));
          break;
        case "Production":
          this.SetXboxLiveEnvironmentUri(new Uri("https://xlink.xboxlive.com"));
          break;
        case "Dev":
          this.SetXboxLiveEnvironmentUri(new Uri("http://local.xlink.vint.xboxlive.com"));
          break;
        case "CertNet":
          this.SetXboxLiveEnvironmentUri(new Uri("https://xlink.cert.xboxlive.com"));
          break;
        case "Test5":
          this.SetXboxLiveEnvironmentUri(new Uri("https://test5.dfhosted.net"));
          break;
        case "SysInt":
          this.SetXboxLiveEnvironmentUri(new Uri("https://sysint.dfhosted.net"));
          break;
        default:
          throw new ArgumentException("Unknown XBL environment", nameof (envName));
      }
    }

    public void SetXboxLiveEnvironmentUri(Uri address)
    {
      this.baseUri = !(address == (Uri) null) ? address : throw new ArgumentNullException();
    }

    public void CompanionLogin(
      TokenManager tokenManager,
      string hardwareId,
      CompanionLoginDelegate callback)
    {
      Uri requestUri = new Uri(this.baseUri, "/Auth/Signin");
      this.ActivityId = Guid.NewGuid().ToString();
      HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(requestUri);
      webRequest.Method = "POST";
      webRequest.ContentType = "application/x-www-form-urlencoded";
      webRequest.Headers["X-XBL-SigninType"] = "WindowsPhone";
      webRequest.Headers["X-XBL-HardwareID"] = hardwareId;
      webRequest.Headers["X-XBL-Contract-Version"] = "2011-08-10";
      webRequest.Headers["x-ms-activity-id"] = this.ActivityId;
      webRequest.Headers["Authorization"] = "XBL2.0BODY";
      if (tokenManager.PartnerToken == null)
        throw new InvalidOperationException("Live token not yet cached");
      this.tracer.WriteInfo("CompanionLogin starting");
      webRequest.BeginGetRequestStream((AsyncCallback) (getRequestStreamAsyncResult =>
      {
        try
        {
          using (Stream requestStream = webRequest.EndGetRequestStream(getRequestStreamAsyncResult))
          {
            byte[] bytes = Encoding.UTF8.GetBytes(tokenManager.PartnerToken);
            requestStream.Write(bytes, 0, bytes.Length);
          }
          webRequest.BeginGetResponse((AsyncCallback) (getResponseAsyncResult =>
          {
            string header4;
            int result;
            string header5;
            string header6;
            try
            {
              using (HttpWebResponse response = (HttpWebResponse) webRequest.EndGetResponse(getResponseAsyncResult))
              {
                header4 = response.Headers["Authorization"];
                if (!int.TryParse(response.Headers["X-XBL-AuthorizationRefresh"], out result))
                {
                  this.tracer.WriteError("CompanionLogin response couldn't be parsed");
                  throw new InvalidDataException("Bad server response. Unable to parse AutorizationRefresh");
                }
                header5 = response.Headers["X-XBL-UserIds"];
                header6 = response.Headers["X-XBL-DeviceID"];
                this.tracer.WriteInfo("CompanionLogin completed successfully");
              }
            }
            catch (Exception ex)
            {
              callback((string) null, 0, (string) null, (string) null, XmediaServiceRestApi.TransformException(ex));
              return;
            }
            callback(header4, result, header5, header6, (Exception) null);
          }), (object) null);
        }
        catch (Exception ex)
        {
          this.tracer.WriteError("CompanionLogin failed: {0}", (object) ex);
          callback((string) null, 0, (string) null, (string) null, XmediaServiceRestApi.TransformException(ex));
        }
      }), (object) null);
    }

    public void CompanionLogout(TokenManager tokenManager, CompanionLogoutDelegate callback)
    {
      HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(new Uri(this.baseUri, "/Auth/Signout"));
      webRequest.Method = "POST";
      webRequest.ContentType = "application/x-www-form-urlencoded";
      webRequest.Headers["X-XBL-Contract-Version"] = "2011-08-10";
      webRequest.Headers["x-ms-activity-id"] = this.ActivityId;
      tokenManager.AddCompanionTokenHeader(webRequest);
      this.tracer.WriteInfo("CompanionLogout starting");
      webRequest.BeginGetResponse((AsyncCallback) (getResponseAsyncResult =>
      {
        try
        {
          webRequest.EndGetResponse(getResponseAsyncResult).Close();
        }
        catch (Exception ex)
        {
          this.tracer.WriteError("CompanionLogout failed: {0}", (object) ex);
          callback(XmediaServiceRestApi.TransformException(ex));
          return;
        }
        this.tracer.WriteInfo("CompanionLogout completed successfully");
        callback((Exception) null);
      }), (object) null);
    }

    public void RefreshCompanionToken(
      TokenManager tokenManager,
      RefreshCompanionTokenDelegate callback)
    {
      HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(new Uri(this.baseUri, "/Auth/Refresh"));
      webRequest.Method = "POST";
      webRequest.ContentType = "application/x-www-form-urlencoded";
      webRequest.Headers["X-XBL-Contract-Version"] = "2011-08-10";
      webRequest.Headers["x-ms-activity-id"] = this.ActivityId;
      tokenManager.AddCompanionTokenHeader(webRequest);
      this.tracer.WriteInfo("RefreshCompanionToken starting");
      webRequest.BeginGetResponse((AsyncCallback) (getResponseAsyncResult =>
      {
        string header;
        int result;
        try
        {
          using (HttpWebResponse response = (HttpWebResponse) webRequest.EndGetResponse(getResponseAsyncResult))
          {
            header = response.Headers["Authorization"];
            if (!int.TryParse(response.Headers["X-XBL-AuthorizationRefresh"], out result))
            {
              this.tracer.WriteError("RefreshCompanionToken response couldn't be parsed");
              throw new InvalidDataException("Bad server response. Unable to parse AutorizationRefresh");
            }
          }
        }
        catch (Exception ex)
        {
          this.tracer.WriteError("RefreshCompanionToken failed: {0}", (object) ex);
          callback((string) null, 0, XmediaServiceRestApi.TransformException(ex));
          return;
        }
        this.tracer.WriteInfo("RefreshCompanionToken completed successfully");
        callback(header, result, (Exception) null);
      }), (object) null);
    }

    public void GetServiceStatus(GetStatusDelegate callback)
    {
      HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(new Uri(this.baseUri, "/Status"));
      webRequest.Method = "GET";
      webRequest.BeginGetResponse((AsyncCallback) (ar =>
      {
        bool online;
        try
        {
          using (HttpWebResponse response = (HttpWebResponse) webRequest.EndGetResponse(ar))
          {
            online = true;
            response.Close();
          }
        }
        catch (Exception ex)
        {
          callback(false, XmediaServiceRestApi.TransformException(ex));
          return;
        }
        callback(online, (Exception) null);
      }), (object) null);
    }

    public void GetPairedConsoles(TokenManager tokenManager, GetPairedConsolesDelegate callback)
    {
      HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(new Uri(this.baseUri, string.Format("/Users/{0}/Devices", (object) tokenManager.UserId)));
      webRequest.Method = "GET";
      webRequest.Headers["X-XBL-Contract-Version"] = "2011-08-10";
      webRequest.Headers["x-ms-activity-id"] = this.ActivityId;
      tokenManager.AddCompanionTokenHeader(webRequest);
      this.tracer.WriteInfo("GetPairedConsoles starting");
      webRequest.BeginGetResponse((AsyncCallback) (ar =>
      {
        IEnumerable<DeviceInfo> pairedConsoles;
        try
        {
          using (HttpWebResponse response = (HttpWebResponse) webRequest.EndGetResponse(ar))
          {
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
              pairedConsoles = DeviceInfo.CollectionFromJsonString(streamReader.ReadToEnd());
          }
        }
        catch (Exception ex)
        {
          this.tracer.WriteError("GetPairedConsoles failed: {0}", (object) ex);
          callback((IEnumerable<DeviceInfo>) null, XmediaServiceRestApi.TransformException(ex));
          return;
        }
        this.tracer.WriteInfo("GetPairedConsoles completed successfully");
        callback(pairedConsoles, (Exception) null);
      }), (object) null);
    }

    public void GetUserSessions(TokenManager tokenManager, GetUserSessionsDelegate callback)
    {
      HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(new Uri(this.baseUri, string.Format("/Users/{0}/Sessions", (object) tokenManager.UserId)));
      webRequest.Method = "GET";
      webRequest.Headers["X-XBL-Contract-Version"] = "2011-08-10";
      webRequest.Headers["x-ms-activity-id"] = this.ActivityId;
      tokenManager.AddCompanionTokenHeader(webRequest);
      this.tracer.WriteInfo("GetUserSessions starting");
      webRequest.BeginGetResponse((AsyncCallback) (ar =>
      {
        IEnumerable<SessionInfo> sessions;
        try
        {
          using (HttpWebResponse response = (HttpWebResponse) webRequest.EndGetResponse(ar))
          {
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
              sessions = SessionInfo.CollectionFromJsonString(streamReader.ReadToEnd());
          }
        }
        catch (Exception ex)
        {
          this.tracer.WriteError("GetUserSessions failed: {0}", (object) ex);
          callback((IEnumerable<SessionInfo>) null, XmediaServiceRestApi.TransformException(ex));
          return;
        }
        this.tracer.WriteInfo("GetUserSessions completed successfully");
        callback(sessions, (Exception) null);
      }), (object) null);
    }

    public void GetUserInfo(TokenManager tokenManager, string userId, GetUserInfoDelegate callback)
    {
      HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(new Uri(this.baseUri, string.Format("/Users/{0}", (object) userId)));
      webRequest.Headers["X-XBL-Contract-Version"] = "2011-08-10";
      webRequest.Headers["x-ms-activity-id"] = this.ActivityId;
      tokenManager.AddCompanionTokenHeader(webRequest);
      webRequest.Method = "GET";
      this.tracer.WriteInfo("GetUserInfo starting");
      webRequest.BeginGetResponse((AsyncCallback) (ar =>
      {
        UserInfo userInfo = (UserInfo) null;
        try
        {
          using (HttpWebResponse response = (HttpWebResponse) webRequest.EndGetResponse(ar))
          {
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
              userInfo = UserInfo.FromJsonString(streamReader.ReadToEnd());
          }
        }
        catch (Exception ex)
        {
          this.tracer.WriteError("GetUserInfo failed: {0}", (object) ex);
          callback((UserInfo) null, XmediaServiceRestApi.TransformException(ex));
          return;
        }
        this.tracer.WriteInfo("GetUserInfo completed successfully");
        callback(userInfo, (Exception) null);
      }), (object) null);
    }

    public void GetDeviceInfo(
      TokenManager tokenManager,
      string deviceId,
      GetDeviceInfoDelegate callback)
    {
      HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(new Uri(this.baseUri, string.Format("/Devices/{0}", (object) deviceId)));
      webRequest.Headers["X-XBL-Contract-Version"] = "2011-08-10";
      webRequest.Headers["x-ms-activity-id"] = this.ActivityId;
      tokenManager.AddCompanionTokenHeader(webRequest);
      webRequest.Method = "GET";
      this.tracer.WriteInfo("GetDeviceInfo starting");
      webRequest.BeginGetResponse((AsyncCallback) (ar =>
      {
        DeviceInfo deviceInfo = (DeviceInfo) null;
        try
        {
          using (HttpWebResponse response = (HttpWebResponse) webRequest.EndGetResponse(ar))
          {
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
              deviceInfo = DeviceInfo.FromJsonString(streamReader.ReadToEnd());
          }
        }
        catch (Exception ex)
        {
          this.tracer.WriteError("GetDeviceInfo failed: {0}", (object) ex);
          callback((DeviceInfo) null, XmediaServiceRestApi.TransformException(ex));
          return;
        }
        this.tracer.WriteInfo("GetDeviceInfo completed successfully");
        callback(deviceInfo, (Exception) null);
      }), (object) null);
    }

    private static Exception TransformException(Exception e)
    {
      if (e is WebException && ((WebException) e).Response is HttpWebResponse)
      {
        HttpWebResponse response = (HttpWebResponse) ((WebException) e).Response;
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
          response.Close();
          return (Exception) new UnauthorizedAccessException("Not authorized", e);
        }
      }
      return e;
    }

    internal static class Paths
    {
      public const string CompanionLogin = "/Auth/Signin";
      public const string CompanionLogout = "/Auth/Signout";
      public const string RefreshToken = "/Auth/Refresh";
      public const string GetPairedConsoles = "/Users/{0}/Devices";
      public const string GetUserSessions = "/Users/{0}/Sessions";
      public const string GetUserInfo = "/Users/{0}";
      public const string GetDeviceInfo = "/Devices/{0}";
      public const string Status = "/Status";
    }

    internal static class HeaderNames
    {
      public const string SigninType = "X-XBL-SigninType";
      public const string Authorization = "Authorization";
      public const string AuthorizationRefresh = "X-XBL-AuthorizationRefresh";
      public const string HardwareId = "X-XBL-HardwareID";
      public const string UserIds = "X-XBL-UserIds";
      public const string DeviceId = "X-XBL-DeviceID";
      public const string ContractVersion = "X-XBL-Contract-Version";
      public const string ActivityId = "x-ms-activity-id";
      public const string RequestId = "x-ms-request-id";
    }
  }
}
