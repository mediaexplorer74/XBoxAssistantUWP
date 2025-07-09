// *********************************************************
// Type: LRC.Service.Search.MusicArtistData
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System.Collections.Generic;


namespace LRC.Service.Search
{
  public class MusicArtistData(MusicData musicData) : MusicData(musicData)
  {
    public MusicArtistData()
      : this((MusicData) null)
    {
    }

    public string Bio { get; set; }

    public List<string> SmallImages { get; set; }

    public List<string> LargeImages { get; set; }

    public List<MusicAlbumData> Albums { get; set; }
  }
}
