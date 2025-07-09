// *********************************************************
// Type: Gds.Contracts.GameInfo
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Runtime.Serialization;


namespace Gds.Contracts
{
  [DataContract(Name = "GameInfo", Namespace = "http://schemas.datacontract.org/2004/07/GDS.Contracts")]
  public class GameInfo
  {
    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "Id", Order = 0)]
    public uint Id { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "Name", Order = 1)]
    public string Name { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "Type", Order = 2)]
    public uint Type { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "GameUrl", Order = 3)]
    public string GameUrl { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "ImageUrl", Order = 4)]
    public string ImageUrl { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "LastPlayed", Order = 5)]
    public DateTime LastPlayed { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "AchievementsEarned", Order = 6)]
    public int AchievementsEarned { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "TotalAchievements", Order = 7)]
    public int TotalAchievements { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "Gamerscore", Order = 8)]
    public uint Gamerscore { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "TotalPossibleGamerscore", Order = 9)]
    public uint TotalPossibleGamerscore { get; set; }

    public GameInfo GameDetails => this;
  }
}
