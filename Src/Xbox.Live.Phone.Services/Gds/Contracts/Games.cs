// *********************************************************
// Type: Gds.Contracts.Games
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Gds.Contracts
{
  [DataContract(Name = "Games", Namespace = "http://schemas.datacontract.org/2004/07/GDS.Contracts")]
  public class Games
  {
    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "TotalUniqueGames", Order = 0)]
    public uint TotalUniqueGames;
    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "UserGamesCollection", Order = 1)]
    public List<UserGames> UserGamesCollection;

    public uint PageNumber { get; set; }
  }
}
