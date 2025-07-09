// *********************************************************
// Type: LRC.Service.Search.PartnerApplicationLaunchInfo
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll


namespace LRC.Service.Search
{
  public class PartnerApplicationLaunchInfo
  {
    public const string Xbox360ClientType = "Xbox360";

    public string ClientType { get; set; }

    public string DeepLinkInfo { get; set; }

    public uint TitleId { get; set; }
  }
}
