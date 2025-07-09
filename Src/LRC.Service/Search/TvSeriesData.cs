// *********************************************************
// Type: LRC.Service.Search.TvSeriesData
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using Xbox.Live.Phone.Utils.Serialization;


namespace LRC.Service.Search
{
  public class TvSeriesData : TvData
  {
    public XmlSerializableDictionary<string, TvSeasonData> TvSeasons { get; set; }

    public XmlSerializableDictionary<string, TvEpisodeData> TvEpisodes { get; set; }
  }
}
