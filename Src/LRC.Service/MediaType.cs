// *********************************************************
// Type: LRC.Service.MediaType
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.Service
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public enum MediaType
  {
    undefined,
    movie,
    music_album,
    music_track,
    tv_episode,
    music_musicvideo,
    music_artist,
    music_playlist,
    tv_series,
    hub,
    movietrailer,
  }
}
