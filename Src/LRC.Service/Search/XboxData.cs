// *********************************************************
// Type: LRC.Service.Search.XboxData
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll


namespace LRC.Service.Search
{
  public class XboxData : SearchData
  {
    public XboxData()
      : this((XboxData) null)
    {
    }

    public XboxData(SearchData searchData)
      : base(searchData)
    {
    }

    public XboxData(XboxData xboxData)
      : base((SearchData) xboxData)
    {
      if (xboxData == null)
        return;
      this.TitleId = xboxData.TitleId;
      this.Description = xboxData.Description;
    }

    public uint TitleId { get; set; }
  }
}
