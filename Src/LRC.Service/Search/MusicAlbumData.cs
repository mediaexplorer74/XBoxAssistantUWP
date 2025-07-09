// *********************************************************
// Type: LRC.Service.Search.MusicAlbumData
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System.Collections.Generic;


namespace LRC.Service.Search
{
  public class MusicAlbumData(MusicData musicData) : MusicData(musicData)
  {
    public MusicAlbumData()
      : this((MusicData) null)
    {
    }

    public List<MusicTrackData> Tracks { get; set; }
  }
}
