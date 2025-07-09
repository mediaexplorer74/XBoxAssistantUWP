// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AssetWebRequestProvider
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.IO;
using System.Net;
using System.Threading;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class AssetWebRequestProvider : NetDataProvider, IDataProvider
  {
    private int requestTimeout = -1;

    public AssetWebRequestProvider(
      string stockAssetAddressFormat,
      string nonStockAssetAddressFormat)
      : base(stockAssetAddressFormat, nonStockAssetAddressFormat, (string) null)
    {
    }

    public AssetWebRequestProvider(
      string stockAssetAddressFormat,
      string nonStockAssetAddressFormat,
      int millisecondsRequestTimeout)
      : base(stockAssetAddressFormat, nonStockAssetAddressFormat, (string) null)
    {
      this.requestTimeout = millisecondsRequestTimeout;
    }

    public AssetWebRequestProvider(string manifestServiceAddressFormat)
      : base((string) null, (string) null, manifestServiceAddressFormat)
    {
    }

    public AssetWebRequestProvider(
      string manifestServiceAddressFormat,
      int millisecondsRequestTimeout)
      : base((string) null, (string) null, manifestServiceAddressFormat)
    {
      this.requestTimeout = millisecondsRequestTimeout;
    }

    public void GetDataAsync(DataRequest request, DataProvider dataProvider)
    {
      try
      {
        string address = this.GetAddress(request);
        WebRequest webRequest = WebRequest.Create(address);
        AssetWebRequestProvider.RequestState state = new AssetWebRequestProvider.RequestState(request, dataProvider);
        state.request = webRequest;
        state.Address = address;
        webRequest.BeginGetResponse(new AsyncCallback(AssetWebRequestProvider.RespCallback), (object) state);
        if (!state.workFinished.WaitOne(this.requestTimeout))
          webRequest.Abort();
        lock (state)
        {
          state.workFinished.Close();
          state.workFinished = (ManualResetEvent) null;
        }
      }
      catch (WebException ex)
      {
        ErrorLog errorLog = new ErrorLog((Exception) ex);
        errorLog.Sender = (object) this;
        errorLog.Message = string.Format("Failed to open a readable stream for data request.");
        Logger.Log((Log) errorLog);
        dataProvider.RequestProcessed(new DataRequestCompletedEventArgs((Exception) ex, false, request.Context), request);
      }
      catch (Exception ex)
      {
        ErrorLog errorLog = new ErrorLog(ex);
        errorLog.Sender = (object) this;
        Logger.Log((Log) errorLog);
        dataProvider.RequestProcessed(new DataRequestCompletedEventArgs(ex, false, request.Context), request);
      }
    }

    private static void RespCallback(IAsyncResult asynchronousResult)
    {
      AssetWebRequestProvider.RequestState asyncState = (AssetWebRequestProvider.RequestState) asynchronousResult.AsyncState;
      DataRequestCompletedEventArgs eventArguments;
      try
      {
        WebResponse webResponse = (WebResponse) null;
        HttpWebRequest request = (HttpWebRequest) asyncState.request;
        if (request != null && request.HaveResponse)
          webResponse = request.EndGetResponse(asynchronousResult);
        if (webResponse != null)
        {
          Stream responseStream = webResponse.GetResponseStream();
          if (responseStream != null)
          {
            eventArguments = new DataRequestCompletedEventArgs((Exception) null, false, asyncState.dataRequest.Context);
            eventArguments.Result = NetDataProvider.DuplicateStream(responseStream, false);
            eventArguments.SourceAddress = asyncState.Address;
            responseStream.Close();
          }
          else
            eventArguments = new DataRequestCompletedEventArgs((Exception) new InvalidOperationException("Failed to get Response Stream."), false, asyncState.dataRequest.Context);
          webResponse.Close();
        }
        else
          eventArguments = new DataRequestCompletedEventArgs((Exception) new InvalidOperationException("Failed to get Web Response."), false, asyncState.dataRequest.Context);
      }
      catch (WebException ex)
      {
        eventArguments = new DataRequestCompletedEventArgs((Exception) ex, false, asyncState.dataRequest.Context);
      }
      catch (Exception ex)
      {
        eventArguments = new DataRequestCompletedEventArgs(ex, false, asyncState.dataRequest.Context);
      }
      finally
      {
        lock (asyncState)
        {
          if (asyncState.workFinished != null)
            asyncState.workFinished.Set();
        }
      }
      asyncState.Provider.RequestProcessed(eventArguments, asyncState.dataRequest);
    }

    public void CancelAsync()
    {
    }

    private class RequestState
    {
      public WebRequest request;
      public ManualResetEvent workFinished = new ManualResetEvent(false);

      internal DataRequest dataRequest { get; private set; }

      public DataProvider Provider { get; private set; }

      internal string Address { get; set; }

      public RequestState(DataRequest dataRequestObj, DataProvider dataProvider)
      {
        this.request = (WebRequest) null;
        this.dataRequest = dataRequestObj;
        this.Provider = dataProvider;
      }
    }
  }
}
