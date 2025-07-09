// *********************************************************
// Type: Xbox.Live.Phone.Services.AvatarServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Avatar.Services.Closet.Library;
using Avatar.Services.ManifestRead.Library;
using Avatar.Services.ManifestWrite.Library;
using Leet.Silverlight.XLiveWeb;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services
{
  public sealed class AvatarServiceManager : IAvatarServiceManager
  {
    public const string AvatarAssetsThumbnailUrlFormat = "Content/Images/AvatarEditorThumbnail/{0}";
    public const string AvatarNonStockAssetsThumbnailUrlFormat = "http://avatar.xboxlive.com/global/t.{0}/avataritem/{1}/128";
    public const string GamerpicServiceUriBase = "https://avatarwrite-part{0}.xboxlive.com/GamerPic.svc/Update";
    private const string ComponentName = "AvatarServiceManager";
    private const string GetManifestUriBase = "https://avatarread-part{0}.xboxlive.com/Manifest.svc/GetManifest";
    private const string GetOthersManifestUriBase = "http://avatarread{0}.xboxlive.com/Manifest.svc/?gt=";
    private const string UpdateManifestUriBase = "https://avatarwrite-part{0}.xboxlive.com/Manifest.svc/Update";
    private const string ClosetUriBase = "https://avatarcloset-part{0}.xboxlive.com/Closet.svc/GetClosetAssets";
    private static AtomicCounter ongoingRequestCount = new AtomicCounter(0);
    private string getManifestUriString;
    private string getOthersManifestUriString;
    private string updateManifestUriString;
    private string setGamerpicUrlString;
    [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Not implemented")]
    private string closetUriString;
    private string currentEnvironmentPrefix = string.Empty;

    public event EventHandler<ServiceProxyEventArgs<AvatarManifests>> EventGetManifestCompleted;

    public event EventHandler<ServiceProxyEventArgs<UpdateManifestResponse>> EventUpdateManifestCompleted;

    public event EventHandler<ServiceProxyEventArgs<ClosetAssetsHolder>> EventGetClosetAssetsCompleted;

    public event EventHandler<ServiceProxyEventArgs<string>> EventSetGamerpicCompleted;

    public WebHeaderCollection WebHeaders { get; set; }

    private static string PartnerToken
    {
      get => XboxLiveGamer.GetPartnerToken("http://xboxlive.com/avatar");
    }

    public void Initialize(ServiceCommon.Environment environmentName)
    {
      this.currentEnvironmentPrefix = ServiceCommon.GetEnvironmentUrlStringPrefix(environmentName);
      this.getManifestUriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://avatarread-part{0}.xboxlive.com/Manifest.svc/GetManifest", new object[1]
      {
        (object) this.currentEnvironmentPrefix
      });
      this.getOthersManifestUriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "http://avatarread{0}.xboxlive.com/Manifest.svc/?gt=", new object[1]
      {
        (object) this.currentEnvironmentPrefix
      });
      this.updateManifestUriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://avatarwrite-part{0}.xboxlive.com/Manifest.svc/Update", new object[1]
      {
        (object) this.currentEnvironmentPrefix
      });
      this.closetUriString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://avatarcloset-part{0}.xboxlive.com/Closet.svc/GetClosetAssets", new object[1]
      {
        (object) this.currentEnvironmentPrefix
      });
      this.setGamerpicUrlString = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://avatarwrite-part{0}.xboxlive.com/GamerPic.svc/Update", new object[1]
      {
        (object) this.currentEnvironmentPrefix
      });
      this.WebHeaders = ServiceCommon.CreateWebHeaders();
    }

    public void GetManifestAsync(string gamerTag)
    {
      bool requireAuthentication = true;
      AvatarServiceManager.ongoingRequestCount.Plus(1);
      string fullUriString;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        if (string.IsNullOrEmpty(gamerTag) || string.Compare(gamerTag, XboxLiveGamer.CurrentGamer.GamerTag, StringComparison.OrdinalIgnoreCase) == 0)
        {
          fullUriString = this.getManifestUriString;
        }
        else
        {
          fullUriString = this.getOthersManifestUriString + gamerTag;
          requireAuthentication = false;
        }
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(fullUriString), HttpStack.PlatformDefault);
        if (requireAuthentication)
          this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + AvatarServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnGetManifestCompleted);
        httpClient.GetDataContractAsync<AvatarManifests>();
      });
    }

    public void UpdateManifestAsync(string manifest)
    {
      if (string.IsNullOrEmpty(manifest))
        throw new ArgumentException("Invalid manifest");
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(this.updateManifestUriString), HttpStack.PlatformDefault);
        httpClient.ContentType = "application/xml";
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnUpdateManifestCompleted);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + AvatarServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.PostDataContractAsync<UpdateManifestRequest, UpdateManifestResponse>((object) new UpdateManifestRequest()
        {
          Manifest = manifest
        });
      });
    }

    public bool IsLoading() => AvatarServiceManager.ongoingRequestCount.Count > 0;

    public void SetGamerpicAsync(GamerpicParameters gamerpicParams)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(this.setGamerpicUrlString), HttpStack.PlatformDefault);
        httpClient.ContentType = "application/xml";
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnSetGamerpicCompleted);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + AvatarServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.PostDataContractAsync<UpdateGamerPicRequest, string>((object) new UpdateGamerPicRequest()
        {
          AnimationId = gamerpicParams.AnimationId,
          Background = gamerpicParams.Background,
          FieldOfView = gamerpicParams.FieldOfView,
          FocalJoint = gamerpicParams.Joint,
          UseProp = gamerpicParams.UseProp,
          Frame = gamerpicParams.AnimationFrame,
          OffsetX = gamerpicParams.Offset.X,
          OffsetY = gamerpicParams.Offset.Y,
          OffsetZ = gamerpicParams.Offset.Z,
          RotationY = gamerpicParams.Rotation.Y
        });
      });
    }

    public void GetClosetAssetsAsync()
    {
      string fullUriString = this.closetUriString;
      AvatarServiceManager.ongoingRequestCount.Plus(1);
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        XLiveWebHttpClient httpClient = XLiveWebHttpClient.GetHttpClient(new Uri(fullUriString), HttpStack.PlatformDefault);
        this.WebHeaders["X-PartnerAuthorization"] = "XBL1.0 x=" + AvatarServiceManager.PartnerToken;
        httpClient.Headers = this.WebHeaders;
        httpClient.TimeoutInMilliseconds = 30000;
        httpClient.OnRequestCompleted += new EventHandler<XLiveWebHttpClientEventArgs>(this.HttpClient_OnGetClosetAssetsCompleted);
        httpClient.GetDataContractAsync<ClosetAssets>();
      });
    }

    private void HttpClient_OnSetGamerpicCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<string>> gamerpicCompleted = this.EventSetGamerpicCompleted;
      Exception error = e.Error;
      if (gamerpicCompleted != null)
      {
        ServiceProxyEventArgs<string> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<string>((object) null, (Exception) XLiveMobileException.CreateException(e.Error, 7002, "Failed to set gamerpic", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<string>(e.Result, (Exception) null, false, (object) null);
        gamerpicCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }

    private void HttpClient_OnGetManifestCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<AvatarManifests>> manifestCompleted = this.EventGetManifestCompleted;
      AvatarServiceManager.ongoingRequestCount.Subtract(1);
      Exception error = e.Error;
      if (manifestCompleted != null)
      {
        ServiceProxyEventArgs<AvatarManifests> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<AvatarManifests>((object) null, (Exception) XLiveMobileException.CreateException(e.Error, 7000, "Failed to get manifest", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<AvatarManifests>(e.Result, (Exception) null, false, (object) null);
        manifestCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }

    private void HttpClient_OnUpdateManifestCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<UpdateManifestResponse>> manifestCompleted = this.EventUpdateManifestCompleted;
      Exception error = e.Error;
      if (manifestCompleted != null)
      {
        ServiceProxyEventArgs<UpdateManifestResponse> e1 = !e.ResultAvailable ? new ServiceProxyEventArgs<UpdateManifestResponse>((object) null, (Exception) XLiveMobileException.CreateException(e.Error, 7001, "Failed to update manifest", (string[]) null), false, (object) null) : new ServiceProxyEventArgs<UpdateManifestResponse>(e.Result, (Exception) null, false, (object) null);
        manifestCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }

    private void HttpClient_OnGetClosetAssetsCompleted(object sender, XLiveWebHttpClientEventArgs e)
    {
      EventHandler<ServiceProxyEventArgs<ClosetAssetsHolder>> closetAssetsCompleted = this.EventGetClosetAssetsCompleted;
      AvatarServiceManager.ongoingRequestCount.Subtract(1);
      Exception error = e.Error;
      if (closetAssetsCompleted != null)
      {
        ClosetAssetsHolder objectResult = new ClosetAssetsHolder();
        if (e.ResultAvailable)
          objectResult.AwardableMpAssets = (ClosetAssets) e.Result;
        ServiceProxyEventArgs<ClosetAssetsHolder> e1 = new ServiceProxyEventArgs<ClosetAssetsHolder>((object) objectResult, (Exception) null, false, (object) null);
        closetAssetsCompleted((object) this, e1);
      }
      if (!(sender is XLiveWebHttpClient xliveWebHttpClient))
        return;
      xliveWebHttpClient.Dispose();
    }
  }
}
