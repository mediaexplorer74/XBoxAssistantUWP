// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.DataProvider
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.Collections.Generic;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class DataProvider
  {
    private DataManagerBase dataManager;
    private IDataProvider dataProvider;
    private Queue<IDataProvider> providers = new Queue<IDataProvider>();

    public DataProvider()
    {
    }

    public DataProvider(Queue<IDataProvider> dataProviders)
    {
      foreach (IDataProvider dataProvider in dataProviders)
        this.providers.Enqueue(dataProvider);
    }

    public void AddDataProvider(IDataProvider dataProvider)
    {
      if (dataProvider == null)
        throw new ArgumentNullException(nameof (dataProvider));
      this.providers.Enqueue(dataProvider);
    }

    public IDataProvider ProcessingProvider => this.dataProvider;

    internal void ProccessRequest(DataRequest request, DataManagerBase dataManager)
    {
      while (this.providers.Count > 0)
      {
        this.dataProvider = this.providers.Dequeue();
        if (this.dataProvider != null)
        {
          this.dataManager = dataManager;
          this.dataProvider.GetDataAsync(request, this);
          break;
        }
      }
    }

    public void RequestProcessed(
      DataRequestCompletedEventArgs eventArguments,
      DataRequest dataRequest)
    {
      if (this.dataManager == null)
        throw new InvalidOperationException();
      if (eventArguments.Error != null && this.providers.Count > 0)
      {
        this.dataManager.EnqueueRequest(dataRequest, DownloadPriority.High);
      }
      else
      {
        this.dataManager.RequestProcessed(eventArguments, dataRequest);
        if (dataRequest.Handler != null)
          dataRequest.Handler((object) null, eventArguments);
        this.dataProvider = (IDataProvider) null;
      }
    }

    public void CancelRequest()
    {
      if (this.dataProvider == null)
        return;
      this.dataProvider.CancelAsync();
    }
  }
}
