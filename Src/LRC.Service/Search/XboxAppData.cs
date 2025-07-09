// *********************************************************
// Type: LRC.Service.Search.XboxAppData
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll


namespace LRC.Service.Search
{
  public class XboxAppData : XboxData
  {
    public XboxAppData()
      : this((XboxGameData) null)
    {
    }

    public XboxAppData(SearchData searchData)
      : base(searchData)
    {
    }

    public XboxAppData(XboxGameData xboxData)
      : base((XboxData) xboxData)
    {
      if (xboxData == null)
        return;
      this.TitleId = xboxData.TitleId;
      this.Description = xboxData.Description;
    }
  }
}
