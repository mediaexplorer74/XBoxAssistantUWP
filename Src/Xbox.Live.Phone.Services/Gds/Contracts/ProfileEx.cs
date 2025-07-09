// *********************************************************
// Type: Gds.Contracts.ProfileEx
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Leet.UserGameData.DataContracts;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils.Serialization;


namespace Gds.Contracts
{
  [DataContract(Name = "ProfileEx", Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class ProfileEx
  {
    [DataMember]
    public long SectionFlags { get; set; }

    [DataMember]
    public XmlSerializableDictionary<ProfileProperty, object> ProfileProperties { get; set; }

    [DataMember]
    public XmlSerializableList<Achievement> RecentAchievements { get; set; }

    [DataMember]
    public XmlSerializableList<Leet.UserGameData.DataContracts.GameInfo> RecentGames { get; set; }

    [DataMember]
    public XmlSerializableList<ProfileEx> Friends { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "PresenceInfo", Order = 5)]
    public Presence PresenceInfo { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "PrivacySettings", Order = 6)]
    public XmlSerializableDictionary<PrivacySetting, uint> PrivacySettings { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "FriendList", Order = 7)]
    public XmlSerializableList<Friend> FriendList { get; set; }
  }
}
