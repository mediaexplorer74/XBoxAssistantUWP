// *********************************************************
// Type: LRC.Service.Search.TvSeasonData
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using Xbox.Live.Phone.Utils.Serialization;


namespace LRC.Service.Search
{
  public class TvSeasonData : TvData
  {
    public TvSeasonData()
      : this((TvData) null)
    {
    }

    public TvSeasonData(SearchData searchData)
      : base(searchData)
    {
    }

    public TvSeasonData(TvData data)
      : base(data)
    {
    }

    public int? SeasonNumber { get; set; }

    public XmlSerializableDictionary<string, TvEpisodeData> TvEpisodes { get; set; }
  }
}
