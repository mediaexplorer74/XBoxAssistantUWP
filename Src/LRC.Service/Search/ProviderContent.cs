// *********************************************************
// Type: LRC.Service.Search.ProviderContent
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System.Collections.Generic;


namespace LRC.Service.Search
{
  public class ProviderContent
  {
    public const string Xbox360Device = "Xbox360";

    public string Device { get; set; }

    public List<OfferInstance> OfferInstances { get; set; }

    public bool? HasClosedCaptioning { get; set; }

    public string ResolutionFormat { get; set; }

    public string VideoType { get; set; }
  }
}
