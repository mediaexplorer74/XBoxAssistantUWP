// *********************************************************
// Type: LRC.Service.Search.MusicTrackData
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System;


namespace LRC.Service.Search
{
  public class MusicTrackData(MusicData musicData) : MusicData(musicData)
  {
    public MusicTrackData()
      : this((MusicData) null)
    {
    }

    public int? DiscNumber { get; set; }

    public int? TrackNumber { get; set; }

    public TimeSpan? Duration { get; set; }
  }
}
