// *********************************************************
// Type: Xbox.Live.Phone.Services.GamesData
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Gds.Contracts;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils.Cache;


namespace Xbox.Live.Phone.Services
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class GamesData : CacheItem
  {
    public GamesData(string key)
      : base(key)
    {
      this.PlayedGames = new List<UserGames>();
    }

    [DataMember]
    public uint TotalUniqueGames { get; set; }

    [DataMember]
    public List<UserGames> PlayedGames { get; set; }

    public UserGames PlayedGamesUser0
    {
      get
      {
        return this.PlayedGames == null || this.PlayedGames.Count == 0 ? (UserGames) null : this.PlayedGames[0];
      }
    }

    public uint PageNumber { get; set; }
  }
}
