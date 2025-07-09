// *********************************************************
// Type: Gds.Contracts.Achievements
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Gds.Contracts
{
  [DataContract(Name = "Achievements", Namespace = "http://schemas.datacontract.org/2004/07/GDS.Contracts")]
  public class Achievements
  {
    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "UserAchievementsCollection", Order = 0)]
    public List<UserAchievements> UserAchievementsCollection;
  }
}
