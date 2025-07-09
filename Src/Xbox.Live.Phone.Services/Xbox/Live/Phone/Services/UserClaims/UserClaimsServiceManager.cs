// *********************************************************
// Type: Xbox.Live.Phone.Services.UserClaims.UserClaimsServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.Silverlight.XLiveWeb;
using System;
using System.Globalization;
using System.Net;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services.UserClaims
{
  public sealed class UserClaimsServiceManager : IUserClaimsServiceManager
  {
    private const string ComponentName = "UserClaimsServiceManager";
    private const string GetUserInfoUriTemplate = "https://services{0}.xboxlive.com/users/me/id";
    private string currentEnvironmentPrefix = string.Empty;

    public event EventHandler<ServiceProxyEventArgs<UserInfo>> EventGetUserInfoCompleted;

    private static string PartnerToken => XboxLiveGamer.GetPartnerToken("http://xboxlive.com");

    public void Initialize(ServiceCommon.Environment environmentName)
    {
      this.currentEnvironmentPrefix = ServiceCommon.GetEnvironmentUrlStringPrefix(environmentName);
    }

    public void GetUserInfo()
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://services{0}.xboxlive.com/users/me/id", new object[1]
        {
          (object) this.currentEnvironmentPrefix
        }), UriKind.Absolute));
        httpClient.Headers = UserClaimsServiceManager.GetWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetUserInfoCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    private static WebHeaderCollection GetWebHeaders()
    {
      WebHeaderCollection webHeaders = new WebHeaderCollection();
      webHeaders["x-xbl-contract-version"] = "1";
      webHeaders["Authorization"] = "XBL2.0 x=" + UserClaimsServiceManager.PartnerToken;
      foreach (string allKey in webHeaders.AllKeys)
        ;
      return webHeaders;
    }

    private void GetUserInfoCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetUserInfoCompleted);
      if (e.Error == null)
      {
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          UserInfo userInfo = UserClaimsResponseParser.ParseGetUserInfoResponse(e.Result as string);
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            EventHandler<ServiceProxyEventArgs<UserInfo>> userInfoCompleted = this.EventGetUserInfoCompleted;
            if (userInfoCompleted == null)
              return;
            userInfoCompleted((object) this, new ServiceProxyEventArgs<UserInfo>((object) userInfo, (Exception) null, false, (object) null));
          }, (object) this);
        });
      }
      else
      {
        XLiveHttpWebException error = (XLiveHttpWebException) e.Error;
        EventHandler<ServiceProxyEventArgs<UserInfo>> userInfoCompleted = this.EventGetUserInfoCompleted;
        if (userInfoCompleted == null)
          return;
        userInfoCompleted((object) this, new ServiceProxyEventArgs<UserInfo>((object) null, e.Error, false, (object) null));
      }
    }
  }
}
