// *********************************************************
// Type: Microsoft.XMedia.EventLink.Client.HttpAsyncClient
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Browser;
using System.Windows.Threading;


namespace Microsoft.XMedia.EventLink.Client
{
  internal class HttpAsyncClient
  {
    private const string ActivityIdHeader = "x-ms-activity-id";
    private const string RequestIdHeader = "x-ms-request-id";
    private const string VersionHeader = "X-XBL-Contract-Version";
    private const string Contract2011Version = "2011-08-10";
    private const string ContentType = "application/octet-stream";
    private HttpWebRequest currentLongPollRequest;
    private Dispatcher dispatcher;
    private string activityId;

    public HttpAsyncClient(TimeSpan timeout, Dispatcher dispatcher, string activityId)
    {
      this.HttpTimeout = timeout;
      this.dispatcher = dispatcher;
      this.activityId = activityId;
    }

    public string AuthHeader { get; set; }

    public TimeSpan HttpTimeout { get; set; }

    public void AbortOutstandingRequests()
    {
      if (this.currentLongPollRequest == null)
        return;
      this.currentLongPollRequest.Abort();
      this.currentLongPollRequest = (HttpWebRequest) null;
    }

    public HttpWebRequest GetAsync(
      Uri uri,
      string etag,
      Action<HttpWebResponse, Exception> responseCallback)
    {
      HttpWebRequest req = HttpAsyncClient.CreateHttpWebRequest(uri, "GET", this.AuthHeader, "application/octet-stream", this.activityId);
      if (!string.IsNullOrEmpty(etag))
      {
        req.Headers[HttpRequestHeader.IfNoneMatch] = etag;
        req.Headers[HttpRequestHeader.CacheControl] = "no-cache";
        req.Headers[HttpRequestHeader.Pragma] = "no-cache";
      }
      req.BeginGetResponse((AsyncCallback) (ar =>
      {
        HttpWebResponse response = (HttpWebResponse) null;
        Exception error = (Exception) null;
        try
        {
          response = (HttpWebResponse) req.EndGetResponse(ar);
        }
        catch (WebException ex)
        {
          response = ex.Response as HttpWebResponse;
          if (response != null)
          {
            if (response.StatusCode == HttpStatusCode.NotModified)
              goto label_5;
          }
          response = (HttpWebResponse) null;
          error = (Exception) ex;
        }
label_5:
        this.dispatcher.BeginInvoke((Action) (() => responseCallback(response, error)));
      }), (object) null);
      return req;
    }

    public void LongPollAsync(
      Uri uri,
      string etag,
      Action<Stream, long, string, Exception> callback)
    {
      this.currentLongPollRequest = this.GetAsync(uri, etag, (Action<HttpWebResponse, Exception>) ((response, error) =>
      {
        if (error != null)
          callback((Stream) null, 0L, (string) null, error);
        else
          callback(response.GetResponseStream(), response.ContentLength, response.Headers["ETag"], (Exception) null);
      }));
    }

    public void PostAsync(
      Uri uri,
      Action<Stream> writeCallback,
      Action<Exception> completionCallback)
    {
      this.WriteAsync(uri, "POST", writeCallback, completionCallback);
    }

    public void PostAsync(
      Uri uri,
      Action<Stream> writeCallback,
      Action<Stream, Exception> completionCallback)
    {
      this.WriteAsync(uri, "POST", writeCallback, completionCallback);
    }

    public void PostAsync(
      Uri uri,
      Action<Stream> writeCallback,
      Action<string, Exception> completionCallback)
    {
      this.WriteAsync(uri, "POST", writeCallback, completionCallback);
    }

    public void PutAsync(
      Uri uri,
      Action<Stream> writeCallback,
      Action<Exception> completionCallback)
    {
      this.WriteAsync(uri, "PUT", writeCallback, completionCallback);
    }

    public void PutAsync(
      Uri uri,
      Action<Stream> writeCallback,
      Action<Stream, Exception> completionCallback)
    {
      this.WriteAsync(uri, "PUT", writeCallback, completionCallback);
    }

    public void PutAsync(
      Uri uri,
      Action<Stream> writeCallback,
      Action<string, Exception> completionCallback)
    {
      this.WriteAsync(uri, "PUT", writeCallback, completionCallback);
    }

    public void DeleteAsync(Uri uri, Action<Exception> completionCallback)
    {
      this.WriteAsync(uri, "DELETE", (Action<Stream>) null, completionCallback);
    }

    private void PostAsync(
      Uri uri,
      Action<Stream> writeCallback,
      Action<HttpWebResponse, Exception> completionCallback)
    {
      this.WriteAsync(uri, "POST", writeCallback, completionCallback);
    }

    private void PutAsync(
      Uri uri,
      Action<Stream> writeCallback,
      Action<HttpWebResponse, Exception> completionCallback)
    {
      this.WriteAsync(uri, "PUT", writeCallback, completionCallback);
    }

    private void WriteAsync(
      Uri uri,
      string method,
      Action<Stream> writeCallback,
      Action<HttpWebResponse, Exception> completionCallback)
    {
      HttpWebRequest req = HttpAsyncClient.CreateHttpWebRequest(uri, method, this.AuthHeader, "application/octet-stream", this.activityId);
      if (writeCallback == null)
        writeCallback = (Action<Stream>) (s => { });
      req.BeginGetRequestStream((AsyncCallback) (ar =>
      {
        Stream stream = (Stream) null;
        try
        {
          stream = req.EndGetRequestStream(ar);
        }
        catch (WebException ex)
        {
          this.dispatcher.BeginInvoke((Action) (() => completionCallback((HttpWebResponse) null, (Exception) ex)));
        }
        if (stream == null)
          return;
        using (stream)
          writeCallback(stream);
        this.GetResponseAsync(req, completionCallback);
      }), (object) null);
    }

    private void WriteAsync(
      Uri uri,
      string method,
      Action<Stream> writeCallback,
      Action<Exception> completionCallback)
    {
      this.WriteAsync(uri, method, writeCallback, (Action<HttpWebResponse, Exception>) ((response, error) =>
      {
        if (response != null)
          response.Close();
        else if (error != null && error is WebException webException2 && webException2.Response != null)
          webException2.Response.Close();
        completionCallback(error);
      }));
    }

    private void WriteAsync(
      Uri uri,
      string method,
      Action<Stream> writeCallback,
      Action<Stream, Exception> completionCallback)
    {
      this.WriteAsync(uri, method, writeCallback, (Action<HttpWebResponse, Exception>) ((response, error) =>
      {
        if (error != null)
          completionCallback((Stream) null, error);
        else
          completionCallback(response.GetResponseStream(), (Exception) null);
      }));
    }

    private void WriteAsync(
      Uri uri,
      string method,
      Action<Stream> writeCallback,
      Action<string, Exception> completionCallback)
    {
      this.WriteAsync(uri, method, writeCallback, (Action<Stream, Exception>) ((stream, error) =>
      {
        if (error != null)
        {
          completionCallback((string) null, error);
        }
        else
        {
          string str = string.Empty;
          try
          {
            using (StreamReader streamReader = new StreamReader(stream))
              str = streamReader.ReadToEnd();
          }
          catch (IOException ex)
          {
            error = (Exception) ex;
          }
          completionCallback(str, error);
        }
      }));
    }

    private static HttpWebRequest CreateHttpWebRequest(
      Uri uri,
      string method,
      string authHeader,
      string contentType,
      string activityId)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequestCreator.ClientHttp.Create(uri);
      if (method == "GET")
      {
        httpWebRequest.Accept = contentType;
      }
      else
      {
        httpWebRequest.Method = method;
        httpWebRequest.ContentType = contentType;
      }
      httpWebRequest.Headers["X-XBL-Contract-Version"] = "2011-08-10";
      if (!string.IsNullOrEmpty(activityId))
        httpWebRequest.Headers["x-ms-activity-id"] = activityId;
      if (!string.IsNullOrEmpty(authHeader))
        httpWebRequest.Headers[HttpRequestHeader.Authorization] = authHeader;
      return httpWebRequest;
    }

    private void GetResponseAsync(
      HttpWebRequest request,
      Action<HttpWebResponse, Exception> completionCallback)
    {
      request.BeginGetResponse((AsyncCallback) (ar =>
      {
        HttpWebResponse response = (HttpWebResponse) null;
        Exception error = (Exception) null;
        try
        {
          response = (HttpWebResponse) request.EndGetResponse(ar);
        }
        catch (WebException ex)
        {
          if (ex.Response is HttpWebResponse response2)
            ((IEnumerable<string>) response2.Headers.AllKeys).Contains<string>("x-ms-request-id");
          error = (Exception) ex;
        }
        this.dispatcher.BeginInvoke((Action) (() =>
        {
          try
          {
            completionCallback(response, error);
          }
          finally
          {
            response?.Close();
          }
        }));
      }), (object) null);
    }

    private static void OnHttpRequestTimeout(object state, bool timedOut)
    {
      if (!timedOut || !(state is HttpWebRequest httpWebRequest))
        return;
      httpWebRequest.Abort();
    }
  }
}
