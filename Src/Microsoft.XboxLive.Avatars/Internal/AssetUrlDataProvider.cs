// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AssetUrlDataProvider
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.Net;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class AssetUrlDataProvider : NetDataProvider, IDataProvider
  {
    private const int numWebClients = 10;
    private static WebClientsManager m_webClients;

    public AssetUrlDataProvider(string stockAssetAddressFormat, string nonStockAssetAddressFormat)
      : base(stockAssetAddressFormat, nonStockAssetAddressFormat, (string) null)
    {
      AssetUrlDataProvider.InitializeWebClientsManager(10);
    }

    public AssetUrlDataProvider(string manifestServiceAddressFormat)
      : base((string) null, (string) null, manifestServiceAddressFormat)
    {
      AssetUrlDataProvider.InitializeWebClientsManager(10);
    }

    private static void InitializeWebClientsManager(int maxNumWebClients)
    {
      if (AssetUrlDataProvider.m_webClients != null)
        return;
      AssetUrlDataProvider.m_webClients = new WebClientsManager(maxNumWebClients, new OpenReadCompletedEventHandler(AssetUrlDataProvider.DownloadOpenReadCompleted), (DownloadProgressChangedEventHandler) null);
    }

    public static int MaxSimultaneousDownloads
    {
      get
      {
        if (AssetUrlDataProvider.m_webClients == null)
          AssetUrlDataProvider.InitializeWebClientsManager(10);
        return AssetUrlDataProvider.m_webClients.MaxNumWebClients;
      }
      set
      {
        if (AssetUrlDataProvider.m_webClients == null)
          AssetUrlDataProvider.InitializeWebClientsManager(value);
        else
          AssetUrlDataProvider.m_webClients.MaxNumWebClients = value;
      }
    }

    private static void DownloadOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
    {
      AssetUrlDataProvider.AssetUrlDataProviderContext userState = e.UserState as AssetUrlDataProvider.AssetUrlDataProviderContext;
      DataRequest request = userState.Request;
      DebugLog debugLog1 = new DebugLog();
      debugLog1.Sender = (object) userState.Provider.ProcessingProvider;
      debugLog1.Message = string.Format("Download completed for {0}.", (object) userState.Address);
      Logger.Log((Log) debugLog1);
      DataRequestCompletedEventArgs eventArguments;
      if (e.Error != null)
      {
        eventArguments = new DataRequestCompletedEventArgs(e.Error, false, request.Context);
        ErrorLog errorLog = new ErrorLog(e.Error);
        errorLog.Sender = (object) userState.Provider.ProcessingProvider;
        errorLog.Message = string.Format("Download failed for {0}.", (object) userState.Address);
        Logger.Log((Log) errorLog);
      }
      else if (e.Cancelled)
      {
        eventArguments = new DataRequestCompletedEventArgs((Exception) null, true, request.Context);
        DebugLog debugLog2 = new DebugLog();
        debugLog2.Sender = (object) userState.Provider.ProcessingProvider;
        debugLog2.Message = string.Format("Download cancelled for {0}.", (object) userState.Address);
        Logger.Log((Log) debugLog2);
      }
      else
      {
        eventArguments = new DataRequestCompletedEventArgs((Exception) null, false, request.Context);
        eventArguments.Result = e.Result.CanSeek ? e.Result : NetDataProvider.DuplicateStream(e.Result, false);
        eventArguments.SourceAddress = userState.Address;
      }
      AssetUrlDataProvider.m_webClients.ReleaseWebClient((WebClient) sender);
      userState.Provider.RequestProcessed(eventArguments, request);
    }

    private void ReleaseWebClient(
      AssetUrlDataProvider.AssetUrlDataProviderContext urlProviderContext)
    {
      WebClient client = urlProviderContext.Client;
      AssetUrlDataProvider.m_webClients.ReleaseWebClient(client);
    }

    public void GetDataAsync(DataRequest request, DataProvider dataProvider)
    {
      WebClient webClient = AssetUrlDataProvider.m_webClients.GetWebClient(true);
      if (webClient == null)
        dataProvider.RequestProcessed(new DataRequestCompletedEventArgs((Exception) new InvalidOperationException(), false, request.Context), request);
      AssetUrlDataProvider.AssetUrlDataProviderContext dataProviderContext = new AssetUrlDataProvider.AssetUrlDataProviderContext(request, webClient, dataProvider);
      try
      {
        dataProviderContext.Address = this.GetAddress(request);
        webClient.OpenReadAsync(new Uri(dataProviderContext.Address), (object) dataProviderContext);
      }
      catch (Exception ex)
      {
        this.ReleaseWebClient(dataProviderContext);
        ErrorLog errorLog = new ErrorLog(ex);
        errorLog.Sender = (object) this;
        errorLog.Message = string.Format("Failed to open a readable stream for data request.");
        Logger.Log((Log) errorLog);
        dataProvider.RequestProcessed(new DataRequestCompletedEventArgs(ex, false, request.Context), request);
      }
    }

    public void CancelAsync() => AssetUrlDataProvider.m_webClients.StopDownloading();

    private class AssetUrlDataProviderContext
    {
      internal AssetUrlDataProviderContext(
        DataRequest request,
        WebClient client,
        DataProvider dataProvider)
      {
        this.Request = request;
        this.Client = client;
        this.Provider = dataProvider;
      }

      internal DataRequest Request { get; private set; }

      public DataProvider Provider { get; private set; }

      public WebClient Client { get; private set; }

      internal string Address { get; set; }
    }
  }
}
