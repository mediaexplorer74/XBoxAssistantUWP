// *********************************************************
// Type: Leet.UserGameData.DataContracts.GameInfo
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Runtime.Serialization;


namespace Leet.UserGameData.DataContracts
{
  [DataContract]
  public class GameInfo
  {
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public int Type { get; set; }

    [DataMember]
    public string GameUrl { get; set; }

    [DataMember]
    public string ImageUrl { get; set; }

    [DataMember]
    public DateTime LastPlayed { get; set; }

    [DataMember]
    public int AchievementsEarned { get; set; }

    [DataMember]
    public int TotalAchievements { get; set; }
  }
}
