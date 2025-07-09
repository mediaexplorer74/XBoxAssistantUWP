// *********************************************************
// Type: Leet.Silverlight.XLiveWeb.XLiveWebHttpClient
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;


namespace Leet.Silverlight.XLiveWeb
{
  public class XLiveWebHttpClient : IDisposable
  {
    private const string InvalidDataContract = "The given type does not have a DataContractAttribute";
    private const int DefaultBufferCopySize = 4096;
    private const int DefaultTimeoutInMilliseconds = 60000;
    private const int MaxAttempts = 10;
    private static Dictionary<Type, DataContractSerializer> deserializerTable = new Dictionary<Type, DataContractSerializer>();
    private static object lockObject = new object();
    private ManualResetEvent doneEvent = new ManualResetEvent(false);
    private WebHeaderCollection headers;
    private CookieContainer cookies;
    private RegisteredWaitHandle waitHandler;
    private bool enteredOperationCompleted;
    private XLiveWebHttpClient.ResponseCallbackData data;

    private XLiveWebHttpClient(Uri uri, HttpStack httpStack, object userToken)
    {
      this.TargetUri = uri;
      this.HttpStack = httpStack;
      this.TimeoutInMilliseconds = 60000;
      this.UserToken = userToken;
    }

    ~XLiveWebHttpClient() => this.Dispose(false);

    public event EventHandler<XLiveWebHttpClientEventArgs> OnRequestCompleted;

    public string ContentType { get; set; }

    public string Accepted { get; set; }

    public int TimeoutInMilliseconds { get; set; }

    public object UserToken { get; set; }

    public WebHeaderCollection Headers
    {
      get => this.headers;
      set => this.headers = value;
    }

    public CookieContainer Cookies
    {
      get => this.cookies;
      set => this.cookies = value;
    }

    private HttpStack HttpStack { get; set; }

    private Uri TargetUri { get; set; }

    private Type RequestObjectType { get; set; }

    private Type ResponseObjectType { get; set; }

    public static XLiveWebHttpClient GetHttpClient(
      Uri targetUri,
      HttpStack httpStack,
      object userToken)
    {
      if (httpStack != HttpStack.PlatformDefault)
        throw new InvalidOperationException("This type of HttpStack is not supported in non-Silverlight Platforms");
      return new XLiveWebHttpClient(targetUri, httpStack, userToken);
    }

    public static XLiveWebHttpClient GetHttpClient(Uri targetUri, HttpStack httpStack)
    {
      return XLiveWebHttpClient.GetHttpClient(targetUri, httpStack, (object) null);
    }

    public static XLiveWebHttpClient GetHttpClient(Uri targetUri)
    {
      return XLiveWebHttpClient.GetHttpClient(targetUri, HttpStack.PlatformDefault, (object) null);
    }

    public static XLiveWebHttpClient GetHttpClient(Uri targetUri, object userToken)
    {
      return XLiveWebHttpClient.GetHttpClient(targetUri, HttpStack.PlatformDefault, userToken);
    }

    public void Close() => this.Dispose();

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public void PostDataContractAsync<RequestType, ResponseType>(object requestObject)
    {
      this.RequestObjectType = typeof (RequestType);
      this.ResponseObjectType = typeof (ResponseType);
      this.CheckRequestTypes(requestObject);
      this.CheckResponseType();
      this.InternalBeginPost(requestObject);
    }

    public void GetDataContractAsync<ResponseType>()
    {
      this.ResponseObjectType = typeof (ResponseType);
      this.CheckResponseType();
      this.InternalBeginGet();
    }

    public void PutDataContractAsync<RequestType, ResponseType>(object requestObject)
    {
      this.RequestObjectType = typeof (RequestType);
      this.ResponseObjectType = typeof (ResponseType);
      this.CheckRequestTypes(requestObject);
      this.CheckResponseType();
      this.InternalBeginPut(requestObject);
    }

    public void DeleteDataContractAsync<ResponseType>(object requestObject)
    {
      this.ResponseObjectType = typeof (ResponseType);
      this.CheckResponseType();
      this.InternalBeginDelete(requestObject);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.doneEvent == null)
        return;
      try
      {
        if (!disposing)
          return;
        this.doneEvent.Dispose();
      }
      finally
      {
        this.doneEvent = (ManualResetEvent) null;
      }
    }

    private static bool HasCustomAttribute<T>(Type dataContractType) where T : Attribute
    {
      return dataContractType.GetCustomAttributes(typeof (T), false).Length > 0;
    }

    private static DataContractSerializer TryGetDeserializer(Type serializationType)
    {
      DataContractSerializer deserializer = (DataContractSerializer) null;
      if ((object) serializationType == null)
        return (DataContractSerializer) null;
      if ((object) serializationType == (object) typeof (string))
        return (DataContractSerializer) null;
      if (!XLiveWebHttpClient.deserializerTable.TryGetValue(serializationType, out deserializer))
      {
        lock (XLiveWebHttpClient.lockObject)
        {
          if (!XLiveWebHttpClient.deserializerTable.TryGetValue(serializationType, out deserializer))
          {
            deserializer = new DataContractSerializer(serializationType);
            XLiveWebHttpClient.deserializerTable.Add(serializationType, deserializer);
          }
        }
      }
      return deserializer;
    }

    private static HttpWebRequest GetHttpStackInitialized(XLiveWebHttpClient httpClient)
    {
      HttpWebRequest stackInitialized = (HttpWebRequest) null;
      if (httpClient.HttpStack == HttpStack.PlatformDefault)
        stackInitialized = (HttpWebRequest) WebRequest.Create(httpClient.TargetUri);
      if (httpClient.Headers != null)
        stackInitialized.Headers = httpClient.Headers;
      if (httpClient.Cookies != null)
        stackInitialized.CookieContainer = httpClient.Cookies;
      if (!string.IsNullOrEmpty(httpClient.Accepted))
        stackInitialized.Accept = httpClient.Accepted;
      if (!string.IsNullOrEmpty(httpClient.ContentType))
        stackInitialized.ContentType = httpClient.ContentType;
      return stackInitialized;
    }

    private void TimeoutCallback(object state, bool timeOut)
    {
      if (this.waitHandler != null)
        this.waitHandler.Unregister((WaitHandle) null);
      if (!timeOut)
        return;
      if (state is HttpWebRequest httpWebRequest)
        httpWebRequest.Abort();
      this.FireRequestCompleted((Exception) new WebException("Timeout", (Exception) null, WebExceptionStatus.Timeout, (WebResponse) null), (HttpWebResponse) null);
    }

    private void InternalBeginPost(object dataContractObject)
    {
      this.InternalBeginHttpAction(dataContractObject, "POST");
    }

    private void InternalBeginPut(object dataContractObject)
    {
      this.InternalBeginHttpAction(dataContractObject, "PUT");
    }

    private void InternalBeginDelete(object dataContractObject)
    {
      this.InternalBeginHttpAction(dataContractObject, "DELETE");
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This block of code has too many potential exception types to manage individually.")]
    private void InternalBeginHttpAction(object dataContractObject, string httpMethod)
    {
      HttpWebRequest hwr = XLiveWebHttpClient.GetHttpStackInitialized(this);
      hwr.Method = httpMethod;
      hwr.BeginGetRequestStream((AsyncCallback) (asyncGetRequestResult =>
      {
        try
        {
          using (Stream requestStream = hwr.EndGetRequestStream(asyncGetRequestResult))
          {
            if ((object) this.RequestObjectType == (object) typeof (string))
            {
              if (!string.IsNullOrEmpty(dataContractObject as string))
              {
                byte[] bytes = Encoding.UTF8.GetBytes((string) dataContractObject);
                requestStream.Write(bytes, 0, bytes.Length);
              }
            }
            else
              XLiveWebHttpClient.TryGetDeserializer(this.RequestObjectType)?.WriteObject(requestStream, dataContractObject);
          }
          Leet.Silverlight.RESTProxy.Logging.Dump<HttpWebRequest>(hwr);
          this.doneEvent.Reset();
          this.enteredOperationCompleted = false;
          hwr.BeginGetResponse((AsyncCallback) (asyncGetResponseResult =>
          {
            try
            {
              this.doneEvent.Set();
              using (HttpWebResponse response = (HttpWebResponse) hwr.EndGetResponse(asyncGetResponseResult))
                this.FireRequestCompleted((Exception) null, response);
            }
            catch (Exception ex)
            {
              this.FireRequestCompleted(ex, (HttpWebResponse) null);
            }
          }), (object) null);
          this.waitHandler = ThreadPool.RegisterWaitForSingleObject((WaitHandle) this.doneEvent, new WaitOrTimerCallback(this.TimeoutCallback), (object) hwr, this.TimeoutInMilliseconds, true);
        }
        catch (Exception ex)
        {
          this.FireRequestCompleted(ex, (HttpWebResponse) null);
        }
      }), (object) null);
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This block of code has too many potential exception types to manage individually.")]
    private void InternalBeginGet()
    {
      this.data = new XLiveWebHttpClient.ResponseCallbackData();
      this.enteredOperationCompleted = false;
      this.data.Attempts = 0;
      this.AttemptGetResponse();
    }

    private void AttemptGetResponse()
    {
      if (this.waitHandler != null)
        this.waitHandler.Unregister((WaitHandle) null);
      this.doneEvent.Reset();
      this.data.Hwr = XLiveWebHttpClient.GetHttpStackInitialized(this);
      Leet.Silverlight.RESTProxy.Logging.Dump<HttpWebRequest>(this.data.Hwr);
      try
      {
        this.data.Hwr.BeginGetResponse(new AsyncCallback(this.GetResponseCallback), (object) null);
        this.waitHandler = ThreadPool.RegisterWaitForSingleObject((WaitHandle) this.doneEvent, new WaitOrTimerCallback(this.TimeoutCallback), (object) this.data.Hwr, this.TimeoutInMilliseconds, true);
      }
      catch (Exception ex)
      {
        this.FireRequestCompleted(ex, (HttpWebResponse) null);
      }
    }

    private void GetResponseCallback(IAsyncResult asyncResultResponse)
    {
      this.doneEvent.Set();
      try
      {
        using (HttpWebResponse response = (HttpWebResponse) this.data.Hwr.EndGetResponse(asyncResultResponse))
          this.FireRequestCompleted((Exception) null, response);
      }
      catch (Exception ex)
      {
        WebException webException = ex as WebException;
        ++this.data.Attempts;
        if (webException != null && webException.Status == WebExceptionStatus.RequestCanceled && this.data.Attempts <= 10)
          this.AttemptGetResponse();
        else
          this.FireRequestCompleted(ex, (HttpWebResponse) null);
      }
    }

    private void FireRequestCompleted(Exception exception, HttpWebResponse httpWebResponse)
    {
      if (exception == null)
      {
        Leet.Silverlight.RESTProxy.Logging.Dump<HttpWebResponse>(httpWebResponse);
      }
      else
      {
        Leet.Silverlight.RESTProxy.Logging.Dump<Exception>(exception);
        if (exception is WebException webException && webException.Response is HttpWebResponse response)
        {
          XLiveHttpWebException httpWebException = new XLiveHttpWebException();
          httpWebException.ResponseUri = response.ResponseUri;
          httpWebException.StatusCode = response.StatusCode;
          httpWebException.Method = response.Method;
          exception = (Exception) httpWebException;
          httpWebException.Response = response;
          if (response != null && response.ContentLength > 0L)
          {
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
              httpWebException.ResponseBody = streamReader.ReadToEnd();
          }
        }
      }
      lock (this)
      {
        if (this.enteredOperationCompleted)
          return;
        this.enteredOperationCompleted = true;
      }
      object responseObject = (object) null;
      if (httpWebResponse != null && httpWebResponse.ContentLength > 0L && exception == null)
      {
        Stream stream = (Stream) null;
        try
        {
          stream = httpWebResponse.GetResponseStream();
          DataContractSerializer deserializer = XLiveWebHttpClient.TryGetDeserializer(this.ResponseObjectType);
          if (deserializer != null)
          {
            try
            {
              responseObject = deserializer.ReadObject(stream);
            }
            catch (SerializationException ex)
            {
              exception = (Exception) ex;
            }
          }
          else
          {
            using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
            {
              stream = (Stream) null;
              responseObject = (object) streamReader.ReadToEnd();
            }
          }
        }
        finally
        {
          stream?.Dispose();
        }
      }
      EventHandler<XLiveWebHttpClientEventArgs> requestCompleted = this.OnRequestCompleted;
      if (requestCompleted == null)
        return;
      XLiveWebHttpClientEventArgs e = new XLiveWebHttpClientEventArgs(responseObject, exception, false, this.UserToken);
      requestCompleted((object) this, e);
    }

    private void CheckRequestTypes(object requestObject)
    {
      if ((object) requestObject.GetType() != (object) this.RequestObjectType)
        throw new ArgumentException("requestObject type mismatch with RequestType", nameof (requestObject));
      if (!XLiveWebHttpClient.HasCustomAttribute<DataContractAttribute>(this.RequestObjectType) && (object) requestObject.GetType() != (object) typeof (string))
        throw new InvalidOperationException("requestObject cannot be a non-string non-serializable object");
    }

    private void CheckResponseType()
    {
      Type dataContractType;
      if (this.ResponseObjectType.IsArray)
        dataContractType = Type.GetType(this.ResponseObjectType.FullName.TrimEnd('[', ']'));
      else
        dataContractType = this.ResponseObjectType;
      if (!XLiveWebHttpClient.HasCustomAttribute<DataContractAttribute>(dataContractType) && (object) this.ResponseObjectType != (object) typeof (string))
        throw new InvalidOperationException("ResponseType cannot be a non-string non-serializable type");
    }

    internal class ResponseCallbackData
    {
      public HttpWebRequest Hwr;
      public int Attempts;
    }
  }
}
