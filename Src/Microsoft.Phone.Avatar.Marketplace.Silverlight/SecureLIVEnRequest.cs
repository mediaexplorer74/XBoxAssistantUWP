// *********************************************************
// Type: Microsoft.Phone.Marketplace.SecureLIVEnRequest
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using System;
using System.Net;


namespace Microsoft.Phone.Marketplace
{
  internal class SecureLIVEnRequest : HttpRequest
  {
    internal string PartnerToken;
    internal string Locale;
    private int _retryCount;

    internal SecureLIVEnRequest(Uri uri)
      : base(uri)
    {
      SecureLIVEnRequest.ValidateSecure(uri);
    }

    protected override void OnPreRequest()
    {
      this._retryCount = 0;
      base.OnPreRequest();
    }

    protected override void AddHeaders(HttpWebRequest webRequest)
    {
      if (!string.IsNullOrEmpty(this.PartnerToken))
        webRequest.Headers["X-PartnerAuthorization"] = "XBL1.0 x=" + this.PartnerToken;
      if (!string.IsNullOrEmpty(this.Locale))
        webRequest.Headers["X-Locale"] = this.Locale;
      webRequest.Headers["X-Platform-Type"] = 5.ToString();
      base.AddHeaders(webRequest);
    }

    protected override void HandleError(Exception e)
    {
      if (e is WebException webException)
      {
        HttpWebResponse response = webException.Response as HttpWebResponse;
        switch (response.StatusCode)
        {
          case HttpStatusCode.BadRequest:
          case HttpStatusCode.InternalServerError:
            try
            {
              ServiceFailureException serviceFailure = new ServiceErrorMessageParser().Parse(webException.Response.GetResponseStream(), e);
              if (ExceptionHelper.HandleServiceFailure(serviceFailure) && this._retryCount == 0)
              {
                ++this._retryCount;
                this.RequestCore();
                return;
              }
              e = (Exception) serviceFailure;
              break;
            }
            catch
            {
              break;
            }
          default:
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
              e = (Exception) new UnauthorizedRequestException(NonLocalizedResources.UserNotAuthorized, e);
              break;
            }
            break;
        }
      }
      base.HandleError(e);
    }

    internal static void ValidateSecure(Uri uri)
    {
      if (uri.Scheme != Uri.UriSchemeHttps)
        throw new InvalidOperationException(NonLocalizedResources.UrlMustBeSecure);
    }
  }
}
