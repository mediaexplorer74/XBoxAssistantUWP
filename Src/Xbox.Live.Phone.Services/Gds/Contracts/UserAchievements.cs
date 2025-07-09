// *********************************************************
// Type: Gds.Contracts.UserAchievements
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.UserGameData.DataContracts;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Gds.Contracts
{
  [DataContract(Name = "UserAchievements", Namespace = "http://schemas.datacontract.org/2004/07/GDS.Contracts")]
  public class UserAchievements
  {
    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "GamerTag", Order = 0)]
    public string Gamertag { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "TotalAchievementsEarned", Order = 1)]
    public ulong TotalAchievementsEarned { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "TotalPossibleAchievements", Order = 2)]
    public ulong TotalPossibleAchievements { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "Gamerscore", Order = 3)]
    public ulong Gamerscore { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "TotalPossibleGamerscore", Order = 4)]
    public ulong TotalPossibleGamerscore { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "AchievementList", Order = 5)]
    public List<Achievement> AchievementList { get; set; }
  }
}
