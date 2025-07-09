// *********************************************************
// Type: Xbox.Live.Phone.Utils.HttpWebRequestRetry
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Net;


namespace Xbox.Live.Phone.Utils
{
  public class HttpWebRequestRetry
  {
    private const int MaxAttempts = 10;
    private int attempts;
    private Uri uri;
    private object state;
    private HttpWebRequest request;

    public HttpWebRequestRetry(Uri requestUri) => this.uri = requestUri;

    public event EventHandler<HttpWebRequestRetryEventArgs> ResponseAvailable;

    public void BeginGetResponse(object state = null)
    {
      this.state = state;
      this.request = (HttpWebRequest) WebRequest.Create(this.uri);
      this.request.Method = "GET";
      this.request.BeginGetResponse(new AsyncCallback(this.HandleResponse), (object) null);
    }

    private void HandleResponse(IAsyncResult result)
    {
      try
      {
        HttpWebResponse response = (HttpWebResponse) this.request.EndGetResponse(result);
        EventHandler<HttpWebRequestRetryEventArgs> responseAvailable = this.ResponseAvailable;
        if (responseAvailable == null)
          return;
        responseAvailable((object) this, new HttpWebRequestRetryEventArgs(response, this.state));
      }
      catch (Exception ex)
      {
        ++this.attempts;
        if (ex is WebException webException && webException.Status == WebExceptionStatus.RequestCanceled && this.attempts <= 10)
        {
          this.BeginGetResponse(this.state);
        }
        else
        {
          EventHandler<HttpWebRequestRetryEventArgs> responseAvailable = this.ResponseAvailable;
          if (responseAvailable == null)
            return;
          responseAvailable((object) this, new HttpWebRequestRetryEventArgs(ex, this.state));
        }
      }
    }
  }
}
