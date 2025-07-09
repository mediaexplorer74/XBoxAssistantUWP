// *********************************************************
// Type: LRC.Service.Search.XboxGameData
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System.Collections.Generic;


namespace LRC.Service.Search
{
  public class XboxGameData : XboxData
  {
    public XboxGameData()
      : this((XboxGameData) null)
    {
    }

    public XboxGameData(SearchData searchData)
      : base(searchData)
    {
    }

    public XboxGameData(XboxGameData xboxData)
      : base((XboxData) xboxData)
    {
      if (xboxData == null)
        return;
      this.ParentId = xboxData.ParentId;
      this.SlideShowImages = xboxData.SlideShowImages != null ? new List<string>((IEnumerable<string>) xboxData.SlideShowImages) : (List<string>) null;
    }

    public List<string> SlideShowImages { get; set; }

    public string ParentId { get; set; }

    public string Developer { get; set; }

    public string Publisher { get; set; }

    public string ProductType { get; set; }

    public List<string> GameRatingDescriptor { get; set; }
  }
}
