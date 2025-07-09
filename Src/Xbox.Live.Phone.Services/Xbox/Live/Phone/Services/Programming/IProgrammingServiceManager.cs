// *********************************************************
// Type: Xbox.Live.Phone.Services.Programming.IProgrammingServiceManager
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Collections.Generic;


namespace Xbox.Live.Phone.Services.Programming
{
  public interface IProgrammingServiceManager
  {
    event EventHandler<ServiceProxyEventArgs<List<PromoItem>>> EventGetProgrammingXmlCompleted;

    void Initialize(ServiceCommon.Environment environmentName);

    void GetProgrammingContentAsync();
  }
}
