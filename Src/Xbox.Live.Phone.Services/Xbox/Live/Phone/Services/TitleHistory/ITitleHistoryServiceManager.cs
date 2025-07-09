// *********************************************************
// Type: Xbox.Live.Phone.Services.TitleHistory.ITitleHistoryServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Collections.Generic;


namespace Xbox.Live.Phone.Services.TitleHistory
{
  public interface ITitleHistoryServiceManager
  {
    event EventHandler<ServiceProxyEventArgs<List<TitleHistoryInfo>>> EventGetTitleHistoryCompleted;

    void GetTitleHistory();

    void Initialize(ServiceCommon.Environment environmentName);
  }
}
