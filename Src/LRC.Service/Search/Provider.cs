// *********************************************************
// Type: LRC.Service.Search.Provider
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System.Collections.Generic;


namespace LRC.Service.Search
{
  public class Provider
  {
    public string Name { get; set; }

    public string Id { get; set; }

    public string ProductId { get; set; }

    public string ImageUrl { get; set; }

    public List<PartnerApplicationLaunchInfo> PartnerApplicationLaunchInfos { get; set; }

    public List<ProviderContent> ProviderContents { get; set; }
  }
}
