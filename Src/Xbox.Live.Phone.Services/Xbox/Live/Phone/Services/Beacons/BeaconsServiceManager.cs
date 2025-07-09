// *********************************************************
// Type: Xbox.Live.Phone.Services.Beacons.BeaconsServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.Silverlight.XLiveWeb;
using System;
using System.Globalization;
using System.Net;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services.Beacons
{
  public sealed class BeaconsServiceManager : IBeaconsServiceManager
  {
    private const string ComponentName = "BeaconsServiceManager";
    private const string GetActivityListForTitleUriTemplate = "https://services{0}.xboxlive.com/social/jumpin/users/xuid({1})/friends?title={2}";
    private const string GetBeaconDefaultTextUriTemplate = "https://services{0}.xboxlive.com/users/xuid({1})/beacons/titles/{2}/defaultText";
    private const string BeaconForTitleUriTemplate = "https://services{0}.xboxlive.com/users/xuid({1})/beacons/titles/{2}";
    private string currentEnvironmentPrefix = string.Empty;

    public event EventHandler<ServiceProxyEventArgs<ActivityListForTitleData>> EventGetActivityListForTitleCompleted;

    public event EventHandler<ServiceProxyEventArgs<BeaconDefaultTextData>> EventGetBeaconDefaultTextCompleted;

    public event EventHandler<ServiceProxyEventArgs<BeaconInfo>> EventGetBeaconForTitleCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventSetBeaconForTitleCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventDeleteBeaconForTitleCompleted;

    private static string PartnerToken => XboxLiveGamer.GetPartnerToken("http://xboxlive.com");

    public void Initialize(ServiceCommon.Environment environmentName)
    {
      this.currentEnvironmentPrefix = ServiceCommon.GetEnvironmentUrlStringPrefix(environmentName);
    }

    public void GetActivityListForTitle(uint gameId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://services{0}.xboxlive.com/social/jumpin/users/xuid({1})/friends?title={2}", new object[3]
        {
          (object) this.currentEnvironmentPrefix,
          (object) XboxLiveGamer.CurrentGamer.XboxUserId,
          (object) gameId
        }), UriKind.Absolute));
        httpClient.Headers = BeaconsServiceManager.GetWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetActivityListForTitleCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    public void GetBeaconDefaultText(uint gameId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://services{0}.xboxlive.com/users/xuid({1})/beacons/titles/{2}/defaultText", new object[3]
        {
          (object) this.currentEnvironmentPrefix,
          (object) XboxLiveGamer.CurrentGamer.XboxUserId,
          (object) gameId
        }), UriKind.Absolute));
        httpClient.Headers = BeaconsServiceManager.GetWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetBeaconDefaultTextCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    public void GetBeaconForTitle(uint gameId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://services{0}.xboxlive.com/users/xuid({1})/beacons/titles/{2}", new object[3]
        {
          (object) this.currentEnvironmentPrefix,
          (object) XboxLiveGamer.CurrentGamer.XboxUserId,
          (object) gameId
        }), UriKind.Absolute));
        httpClient.Headers = BeaconsServiceManager.GetWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.GetBeaconForTitleCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.GetDataContractAsync<string>();
      });
    }

    public void SetBeaconForTitle(uint gameId, string beaconText)
    {
      string setBeaconRequestBody;
      try
      {
        setBeaconRequestBody = BeaconsServiceManager.CreateSetBeaconRequestBody(beaconText);
      }
      catch (Exception ex)
      {
        EventHandler<ServiceProxyEventArgs<string>> forTitleCompleted = this.EventSetBeaconForTitleCompleted;
        if (forTitleCompleted == null)
          return;
        forTitleCompleted((object) this, new ServiceProxyEventArgs<string>((object) null, ex, false, (object) null));
        return;
      }
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://services{0}.xboxlive.com/users/xuid({1})/beacons/titles/{2}", new object[3]
        {
          (object) this.currentEnvironmentPrefix,
          (object) XboxLiveGamer.CurrentGamer.XboxUserId,
          (object) gameId
        }), UriKind.Absolute));
        httpClient.Headers = BeaconsServiceManager.GetWebHeaders();
        httpClient.ContentType = "application/json";
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.SetBeaconForTitleCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.PutDataContractAsync<string, string>((object) setBeaconRequestBody);
      });
    }

    public void DeleteBeaconForTitle(uint gameId)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://services{0}.xboxlive.com/users/xuid({1})/beacons/titles/{2}", new object[3]
        {
          (object) this.currentEnvironmentPrefix,
          (object) XboxLiveGamer.CurrentGamer.XboxUserId,
          (object) gameId
        }), UriKind.Absolute));
        httpClient.Headers = BeaconsServiceManager.GetWebHeaders();
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.DeleteBeaconForTitleCompleted);
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.DeleteDataContractAsync<string>((object) null);
      });
    }

    private static WebHeaderCollection GetWebHeaders()
    {
      WebHeaderCollection webHeaders = new WebHeaderCollection();
      webHeaders["Authorization"] = "XBL2.0 x=" + BeaconsServiceManager.PartnerToken;
      webHeaders["Accept-Language"] = CultureInfo.CurrentUICulture.Name;
      webHeaders["x-xbl-contract-version"] = "1";
      webHeaders["Cache-Control"] = "no-store, no-cache, must-revalidate";
      webHeaders["PRAGMA"] = "no-cache";
      return webHeaders;
    }

    private static string CreateSetBeaconRequestBody(string beaconText)
    {
      return JsonHelper.Serialize<SetBeaconRequest>(new SetBeaconRequest()
      {
        BeaconText = beaconText
      });
    }

    private void GetActivityListForTitleCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetActivityListForTitleCompleted);
      if (e.Error == null)
      {
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          ActivityListForTitleData activityListForTitleData = BeaconsResponseParser.ParseGetActivityListForTitleResponse(e.Result as string);
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            EventHandler<ServiceProxyEventArgs<ActivityListForTitleData>> forTitleCompleted = this.EventGetActivityListForTitleCompleted;
            if (forTitleCompleted == null)
              return;
            forTitleCompleted((object) this, new ServiceProxyEventArgs<ActivityListForTitleData>((object) activityListForTitleData, (Exception) null, false, (object) null));
          }, (object) this);
        });
      }
      else
      {
        EventHandler<ServiceProxyEventArgs<ActivityListForTitleData>> forTitleCompleted = this.EventGetActivityListForTitleCompleted;
        if (forTitleCompleted == null)
          return;
        forTitleCompleted((object) this, new ServiceProxyEventArgs<ActivityListForTitleData>((object) null, e.Error, false, (object) null));
      }
    }

    private void GetBeaconDefaultTextCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetBeaconDefaultTextCompleted);
      if (e.Error == null)
      {
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          BeaconDefaultTextData beaconDefaultTextData = BeaconsResponseParser.ParseGetBeaconDefaultTextResponse(e.Result as string);
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            EventHandler<ServiceProxyEventArgs<BeaconDefaultTextData>> defaultTextCompleted = this.EventGetBeaconDefaultTextCompleted;
            if (defaultTextCompleted == null)
              return;
            defaultTextCompleted((object) this, new ServiceProxyEventArgs<BeaconDefaultTextData>((object) beaconDefaultTextData, (Exception) null, false, (object) null));
          }, (object) this);
        });
      }
      else
      {
        Exception exception = e.Error;
        if (e.Error is XLiveHttpWebException error && error.StatusCode == HttpStatusCode.Forbidden)
          exception = (Exception) new XMobileException(2);
        EventHandler<ServiceProxyEventArgs<BeaconDefaultTextData>> defaultTextCompleted = this.EventGetBeaconDefaultTextCompleted;
        if (defaultTextCompleted == null)
          return;
        defaultTextCompleted((object) this, new ServiceProxyEventArgs<BeaconDefaultTextData>((object) null, exception, false, (object) null));
      }
    }

    private void GetBeaconForTitleCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.GetBeaconForTitleCompleted);
      if (e.Error == null)
      {
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          BeaconInfo beaconInfo = BeaconsResponseParser.ParseGetBeaconForTitleResponse(e.Result as string);
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            EventHandler<ServiceProxyEventArgs<BeaconInfo>> forTitleCompleted = this.EventGetBeaconForTitleCompleted;
            if (forTitleCompleted == null)
              return;
            forTitleCompleted((object) this, new ServiceProxyEventArgs<BeaconInfo>((object) beaconInfo, (Exception) null, false, (object) null));
          }, (object) this);
        });
      }
      else
      {
        Exception exception = e.Error;
        if (e.Error is XLiveHttpWebException error && error.StatusCode == HttpStatusCode.NotFound)
          exception = (Exception) new XMobileException(1);
        EventHandler<ServiceProxyEventArgs<BeaconInfo>> forTitleCompleted = this.EventGetBeaconForTitleCompleted;
        if (forTitleCompleted == null)
          return;
        forTitleCompleted((object) this, new ServiceProxyEventArgs<BeaconInfo>((object) null, exception, false, (object) null));
      }
    }

    private void SetBeaconForTitleCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.SetBeaconForTitleCompleted);
      if (e.Error == null)
      {
        EventHandler<ServiceProxyEventArgs<string>> forTitleCompleted = this.EventSetBeaconForTitleCompleted;
        if (forTitleCompleted == null)
          return;
        forTitleCompleted((object) this, new ServiceProxyEventArgs<string>(e.Result, (Exception) null, false, (object) null));
      }
      else
      {
        Exception exception = e.Error;
        if (e.Error is XLiveHttpWebException error)
        {
          if (error.StatusCode == HttpStatusCode.BadRequest)
            exception = (Exception) new XMobileException(4);
          else if (error.StatusCode == HttpStatusCode.RequestEntityTooLarge)
            exception = (Exception) new XMobileException(3);
        }
        EventHandler<ServiceProxyEventArgs<string>> forTitleCompleted = this.EventSetBeaconForTitleCompleted;
        if (forTitleCompleted == null)
          return;
        forTitleCompleted((object) this, new ServiceProxyEventArgs<string>((object) null, exception, false, (object) null));
      }
    }

    private void DeleteBeaconForTitleCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      if (sender is XLiveWebHttpClient xliveWebHttpClient)
        xliveWebHttpClient.OnRequestCompleted -= new EventHandler<XLiveWebHttpClientEventArgs>(this.DeleteBeaconForTitleCompleted);
      if (e.Error == null)
      {
        EventHandler<ServiceProxyEventArgs<string>> forTitleCompleted = this.EventDeleteBeaconForTitleCompleted;
        if (forTitleCompleted == null)
          return;
        forTitleCompleted((object) this, new ServiceProxyEventArgs<string>(e.Result, (Exception) null, false, (object) null));
      }
      else
      {
        Exception exception = e.Error;
        if (e.Error is XLiveHttpWebException error && error.StatusCode == HttpStatusCode.NotFound)
          exception = (Exception) new XMobileException(1);
        EventHandler<ServiceProxyEventArgs<string>> forTitleCompleted = this.EventDeleteBeaconForTitleCompleted;
        if (forTitleCompleted == null)
          return;
        forTitleCompleted((object) this, new ServiceProxyEventArgs<string>((object) null, exception, false, (object) null));
      }
    }
  }
}
