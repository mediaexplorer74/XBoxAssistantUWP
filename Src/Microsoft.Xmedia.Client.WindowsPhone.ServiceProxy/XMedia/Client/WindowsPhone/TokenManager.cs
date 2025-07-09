// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.TokenManager
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using System;
using System.Net;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  internal class TokenManager
  {
    public TokenManager()
    {
      this.PartnerToken = (string) null;
      this.CompanionToken = (string) null;
      this.UserId = (string) null;
      this.DeviceId = (string) null;
    }

    public string DeviceId { get; set; }

    public string UserId { get; set; }

    public string PartnerToken { get; set; }

    public string CompanionToken { get; set; }

    public bool HasPartnerToken => !string.IsNullOrWhiteSpace(this.PartnerToken);

    public void AddPartnerAuthorizationHeader(HttpWebRequest request)
    {
      request.Headers["Authorization"] = this.PartnerToken != null ? this.PartnerToken : throw new InvalidOperationException("Live token not yet cached");
    }

    public void AddCompanionTokenHeader(HttpWebRequest request)
    {
      request.Headers["Authorization"] = this.CompanionToken != null ? this.CompanionToken : throw new InvalidOperationException("Companion token ID not yet cached");
      request.Headers["X-XBL-UserIds"] = this.UserId;
      request.Headers["X-XBL-DeviceID"] = this.DeviceId;
    }
  }
}
