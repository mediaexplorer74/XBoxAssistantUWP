// *********************************************************
// Type: Microsoft.Phone.Marketplace.HttpRequest
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using Microsoft.Phone.Marketplace.Resources;
using Microsoft.Phone.Marketplace.Util;
using System;
using System.IO;
using System.Net;
using System.Net.Browser;
using System.Threading;


namespace Microsoft.Phone.Marketplace
{
  internal class HttpRequest
  {
    private const int MaxAttempts = 10;
    private Uri _uri;
    private HttpWebRequest _webRequest;
    private object _userState;
    protected object _syncObject = new object();
    private int attempts;

    internal static Uri SafeCombine(Uri baseUri, string relative)
    {
      string str = baseUri.ToString();
      if (!str.EndsWith("/") && !relative.StartsWith("?") && !relative.StartsWith("/"))
        baseUri = new Uri(str + "/");
      return new Uri(baseUri, relative);
    }

    internal HttpRequest(Uri uri)
    {
      this._uri = uri;
      this.HeaderCollection = new WebHeaderCollection();
      this._webRequest = (HttpWebRequest) WebRequestCreator.ClientHttp.Create(this._uri);
    }

    internal void GetResponseAsync() => this.GetResponseAsync((object) null);

    internal void GetResponseAsync(object userState)
    {
      this.StartBusy();
      this._userState = userState;
      this.OnPreRequest();
      this.RequestCore();
    }

    internal WebHeaderCollection HeaderCollection { get; set; }

    protected virtual void OnPreRequest()
    {
    }

    protected void RequestCore()
    {
      this._webRequest = (HttpWebRequest) WebRequestCreator.ClientHttp.Create(this._uri);
      if (this.HeaderCollection != null)
        this._webRequest.Headers = this.HeaderCollection;
      if (!string.IsNullOrEmpty(this.Method))
        this._webRequest.Method = this.Method;
      this.AddHeaders(this._webRequest);
      if (this.RequestStream != null)
        this._webRequest.BeginGetRequestStream((AsyncCallback) (result =>
        {
          try
          {
            Stream requestStream = this._webRequest.EndGetRequestStream(result);
            StreamHelper.CopyStream(this.RequestStream, requestStream);
            this._webRequest.ContentType = "text/xml";
            requestStream.Close();
            this.GetResponse();
          }
          catch (WebException ex)
          {
            if (ex.Status == WebExceptionStatus.RequestCanceled)
              this.HandleCancel();
            else
              this.HandleError((Exception) ex);
          }
          catch (ThreadAbortException ex)
          {
            this.HandleCancel();
          }
          catch (Exception ex)
          {
            this.HandleError(ex);
          }
        }), this._userState);
      else
        this.GetResponse();
    }

    private void GetResponse()
    {
      this._webRequest.BeginGetResponse((AsyncCallback) (result =>
      {
        bool flag = false;
        try
        {
          WebResponse response = this._webRequest.EndGetResponse(result);
          Stream responseStream1 = response.GetResponseStream();
          if (!string.IsNullOrEmpty(response.Headers["Content-Encoding"]) && response.Headers["Content-Encoding"].IndexOf("gzip", StringComparison.OrdinalIgnoreCase) != -1)
          {
            GZipStream gzipStream = new GZipStream(responseStream1, CompressionMode.Decompress);
            Stream responseStream2 = (Stream) new MemoryStream();
            int count = 2048;
            byte[] buffer = new byte[2048];
            while (true)
            {
              count = gzipStream.Read(buffer, 0, count);
              if (count > 0)
                responseStream2.Write(buffer, 0, count);
              else
                break;
            }
            gzipStream.Close();
            responseStream2.Position = 0L;
            flag = true;
            this.HandleSuccess(responseStream2);
          }
          else
          {
            flag = true;
            this.HandleSuccess(responseStream1);
          }
        }
        catch (WebException ex)
        {
          if (flag)
            return;
          ++this.attempts;
          if (ex.Status == WebExceptionStatus.RequestCanceled)
          {
            if (this.attempts <= 10)
              this.RequestCore();
            else
              this.HandleCancel();
          }
          else
            this.HandleError((Exception) ex);
        }
        catch (Exception ex)
        {
          if (flag)
            return;
          this.HandleError(ex);
        }
      }), this._userState);
    }

    private void StartBusy()
    {
      lock (this._syncObject)
        this.IsBusy = !this.IsBusy ? true : throw new InvalidOperationException(NonLocalizedResources.ObjectCannotBeModified);
    }

    protected virtual void ResetRequestState()
    {
      lock (this._syncObject)
      {
        this.IsBusy = false;
        this._webRequest = (HttpWebRequest) null;
        this._userState = (object) null;
      }
    }

    protected virtual void AddHeaders(HttpWebRequest webRequest)
    {
    }

    internal event EventHandler<GetResponseCompletedEventArgs> GetResponseCompleted;

    private void RaiseGetResponseCompleted(GetResponseCompletedEventArgs e)
    {
      this.ResetRequestState();
      EventHandler<GetResponseCompletedEventArgs> responseCompleted = this.GetResponseCompleted;
      if (responseCompleted == null)
        return;
      responseCompleted((object) this, e);
    }

    internal void CancelAsync()
    {
      lock (this._syncObject)
      {
        if (!this.IsBusy || this._webRequest == null)
          return;
        this._webRequest.Abort();
      }
    }

    public string Method { get; set; }

    public Stream RequestStream { get; set; }

    internal bool IsBusy { get; set; }

    protected virtual void HandleSuccess(Stream responseStream)
    {
      this.RaiseGetResponseCompleted(new GetResponseCompletedEventArgs((Exception) null, false, this._userState)
      {
        Response = responseStream
      });
    }

    protected virtual void HandleError(Exception e)
    {
      this.RaiseGetResponseCompleted(new GetResponseCompletedEventArgs(e, false, this._userState));
    }

    protected virtual void HandleCancel()
    {
      this.RaiseGetResponseCompleted(new GetResponseCompletedEventArgs((Exception) null, true, this._userState));
    }
  }
}
