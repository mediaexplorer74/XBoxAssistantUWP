// *********************************************************
// Type: Gds.Contracts.UserGames
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Gds.Contracts
{
  [DataContract(Name = "UserGames", Namespace = "http://schemas.datacontract.org/2004/07/GDS.Contracts")]
  public class UserGames
  {
    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "GamerTag", Order = 0)]
    public string Gamertag { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "TotalGamesPlayed", Order = 1)]
    public uint TotalGamesPlayed { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "Gamerscore", Order = 2)]
    public uint Gamerscore { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "TotalPossibleGamerscore", Order = 3)]
    public uint TotalPossibleGamerscore { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "GameList", Order = 4)]
    public List<GameInfo> GameList { get; set; }
  }
}
