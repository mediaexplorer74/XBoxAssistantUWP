// *********************************************************
// Type: Xbox.Live.Phone.Services.Programming.ProgramingServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Xml.Linq;
using Xbox.Live.Phone.Utils;
using Xbox.Live.Phone.Utils.Linq;


namespace Xbox.Live.Phone.Services.Programming
{
  public sealed class ProgramingServiceManager : IProgrammingServiceManager
  {
    private const string ComponentName = "ProgrammingServiceManager";
    private const string EpixUrlBaseProduction = "https://epix-ssl.xbox.com/epix/{0}/dashhome.xml";
    private const string EpixUrlElementName = "url";
    private string configUrl;

    public event EventHandler<ServiceProxyEventArgs<List<PromoItem>>> EventGetProgrammingXmlCompleted;

    public void Initialize(ServiceCommon.Environment environmentName)
    {
      switch (environmentName)
      {
        case ServiceCommon.Environment.PartnerNet:
          this.configUrl = "http://www.part.xbox.com/en-US/Platform/WindowsPhone/XboxLive/Companion/config";
          break;
        case ServiceCommon.Environment.Production:
          this.configUrl = "http://www.xbox.com/en-US/Platform/WindowsPhone/XboxLive/Companion/config";
          break;
        case ServiceCommon.Environment.CertNet:
          this.configUrl = "http://www.cert.xbox.com/en-US/Platform/WindowsPhone/XboxLive/Companion/config";
          break;
        case ServiceCommon.Environment.VINT:
          this.configUrl = "http://www.rtm.vint.xbox.com/en-US/Platform/WindowsPhone/XboxLive/Companion/config";
          break;
        default:
          this.configUrl = "http://www.xbox.com/en-US/Platform/WindowsPhone/XboxLive/Companion/config";
          break;
      }
    }

    public void GetProgrammingContentAsync()
    {
      HttpWebRequestRetry httpWebRequestRetry = new HttpWebRequestRetry(new Uri(this.configUrl, UriKind.Absolute));
      httpWebRequestRetry.ResponseAvailable += new EventHandler<HttpWebRequestRetryEventArgs>(this.OnGetEpixUrlBaseCompleted);
      httpWebRequestRetry.BeginGetResponse();
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Almost any type of exception caught here will be handled the same way.")]
    private void OnGetEpixUrlBaseCompleted(object sender, HttpWebRequestRetryEventArgs e)
    {
      if (sender is HttpWebRequestRetry httpWebRequestRetry1)
        httpWebRequestRetry1.ResponseAvailable -= new EventHandler<HttpWebRequestRetryEventArgs>(this.OnGetEpixUrlBaseCompleted);
      string format = string.Empty;
      if (e.Exception == null)
      {
        if (e.Response != null)
        {
          try
          {
            format = LinqHelper.GetValue(XDocument.Load(e.Response.GetResponseStream()).Root, (XName) "url", string.Empty);
          }
          catch (Exception ex)
          {
            format = "https://epix-ssl.xbox.com/epix/{0}/dashhome.xml";
          }
        }
      }
      if (!string.IsNullOrWhiteSpace(format))
      {
        HttpWebRequestRetry httpWebRequestRetry2 = new HttpWebRequestRetry(new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, format, new object[1]
        {
          (object) XboxLiveGamer.CurrentGamer.LegalLocale
        }), UriKind.Absolute));
        httpWebRequestRetry2.ResponseAvailable += new EventHandler<HttpWebRequestRetryEventArgs>(this.OnResponseComplete);
        httpWebRequestRetry2.BeginGetResponse();
      }
      else
      {
        Exception exception = e.Exception;
        EventHandler<ServiceProxyEventArgs<List<PromoItem>>> programmingXmlCompleted = this.EventGetProgrammingXmlCompleted;
        if (programmingXmlCompleted == null)
          return;
        ServiceProxyEventArgs<List<PromoItem>> e1 = new ServiceProxyEventArgs<List<PromoItem>>((object) null, e.Exception, false, (object) null);
        programmingXmlCompleted((object) this, e1);
      }
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Almost any type of exception caught here will be handled the same way.")]
    private void OnResponseComplete(object sender, HttpWebRequestRetryEventArgs e)
    {
      if (sender is HttpWebRequestRetry httpWebRequestRetry)
        httpWebRequestRetry.ResponseAvailable -= new EventHandler<HttpWebRequestRetryEventArgs>(this.OnResponseComplete);
      EventHandler<ServiceProxyEventArgs<List<PromoItem>>> programmingXmlCompleted = this.EventGetProgrammingXmlCompleted;
      if (programmingXmlCompleted == null)
        return;
      ServiceProxyEventArgs<List<PromoItem>> e1;
      if (e.Exception == null && e.Response != null)
      {
        e1 = new ServiceProxyEventArgs<List<PromoItem>>((object) ProgrammingParser.GetProgrammingItems(CleanXmlUtil.ReadCleanedResponseXmlToString((WebResponse) e.Response)), (Exception) null, false, (object) null);
      }
      else
      {
        Exception exception = e.Exception;
        e1 = new ServiceProxyEventArgs<List<PromoItem>>((object) null, e.Exception, false, (object) null);
      }
      programmingXmlCompleted((object) this, e1);
    }
  }
}
