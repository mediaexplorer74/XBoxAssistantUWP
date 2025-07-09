// *********************************************************
// Type: Leet.Silverlight.XLiveWeb.XLiveHttpWebException
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Globalization;
using System.Net;


namespace Leet.Silverlight.XLiveWeb
{
  public class XLiveHttpWebException : Exception
  {
    public HttpStatusCode StatusCode { get; set; }

    public Uri ResponseUri { get; set; }

    public HttpWebResponse Response { get; set; }

    public string ResponseBody { get; set; }

    public string Method { get; set; }

    public int ErrorCode { get; set; }

    public string ErrorMessage { get; set; }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, " {0}: {1} {2}", new object[3]
      {
        (object) this.StatusCode,
        (object) this.Method,
        (object) this.ResponseUri
      });
    }
  }
}
