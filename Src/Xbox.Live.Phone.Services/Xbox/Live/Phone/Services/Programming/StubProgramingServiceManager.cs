// *********************************************************
// Type: Xbox.Live.Phone.Services.Programming.StubProgramingServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Collections.Generic;
using System.Threading;


namespace Xbox.Live.Phone.Services.Programming
{
  public sealed class StubProgramingServiceManager : IProgrammingServiceManager
  {
    public event EventHandler<ServiceProxyEventArgs<List<PromoItem>>> EventGetProgrammingXmlCompleted;

    public void Initialize(ServiceCommon.Environment environmentName)
    {
    }

    public void GetProgrammingContentAsync()
    {
      if (this.EventGetProgrammingXmlCompleted == null)
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        this.EventGetProgrammingXmlCompleted((object) this, new ServiceProxyEventArgs<List<PromoItem>>((object) ProgrammingParser.GetProgrammingItems(ServiceCommon.ReadResource("Xbox.Live.Phone.Services.StubData.Programming.xml")), (Exception) null, false, (object) null));
      });
    }
  }
}
