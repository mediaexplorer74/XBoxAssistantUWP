// *********************************************************
// Type: Xbox.Live.Phone.Services.AchievementsData
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
  public class AchievementsData : CacheItem
  {
    public AchievementsData(string key)
      : base(key)
    {
      this.Achievements = new Dictionary<uint, UserAchievements>();
    }

    [DataMember]
    public Dictionary<uint, UserAchievements> Achievements { get; set; }
  }
}
