// *********************************************************
// Type: LRC.Service.Search.MusicData
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll


namespace LRC.Service.Search
{
  public class MusicData : SearchData
  {
    public MusicData()
      : this((MusicData) null)
    {
    }

    public MusicData(MusicData musicData)
      : base((SearchData) musicData)
    {
      if (musicData == null)
        return;
      this.Artist = musicData.Artist;
      this.ArtistId = musicData.ArtistId;
    }

    public string Artist { get; set; }

    public string ArtistId { get; set; }
  }
}
