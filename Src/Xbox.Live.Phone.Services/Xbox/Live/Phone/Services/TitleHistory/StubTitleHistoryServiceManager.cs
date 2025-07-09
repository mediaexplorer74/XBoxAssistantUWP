// *********************************************************
// Type: Xbox.Live.Phone.Services.TitleHistory.StubTitleHistoryServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services.TitleHistory
{
  public sealed class StubTitleHistoryServiceManager : ITitleHistoryServiceManager
  {
    public event EventHandler<ServiceProxyEventArgs<List<TitleHistoryInfo>>> EventGetTitleHistoryCompleted;

    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "environmentName", Justification = "This is stub")]
    public void Initialize(ServiceCommon.Environment environmentName)
    {
    }

    public void GetTitleHistory()
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        List<TitleHistoryInfo> listTitleHistory = TitleHistoryResponseParser.ParseGetTitleHistory(ServiceCommon.ReadResource("Xbox.Live.Phone.Services.StubData.TitleHistory.txt"));
        Thread.Sleep(2000);
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          EventHandler<ServiceProxyEventArgs<List<TitleHistoryInfo>>> historyCompleted = this.EventGetTitleHistoryCompleted;
          if (historyCompleted == null)
            return;
          historyCompleted((object) this, new ServiceProxyEventArgs<List<TitleHistoryInfo>>((object) listTitleHistory, (Exception) null, false, (object) null));
        }, (object) this);
      });
    }
  }
}
