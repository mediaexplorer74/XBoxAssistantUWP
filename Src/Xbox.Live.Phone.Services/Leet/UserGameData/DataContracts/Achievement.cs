// *********************************************************
// Type: Leet.UserGameData.DataContracts.Achievement
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Runtime.Serialization;


namespace Leet.UserGameData.DataContracts
{
  [DataContract(Name = "Achievement", Namespace = "")]
  public class Achievement
  {
    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "GameId", Order = 0)]
    public uint GameId { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "Description", Order = 1)]
    public string Description { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "DisplayBeforeEarned", Order = 2)]
    public bool DisplayBeforeEarned { get; set; }

    [DataMember(EmitDefaultValue = false, IsRequired = false, Name = "EarnedDateTime", Order = 3)]
    public DateTime EarnedDateTime { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "EarnedOnline", Order = 4)]
    public bool EarnedOnline { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "GameName", Order = 5)]
    public string GameName { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "Gamerscore", Order = 6)]
    public int Gamerscore { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "HowToEarn", Order = 7)]
    public string HowToEarn { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "IsEarned", Order = 8)]
    public bool IsEarned { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "Key", Order = 9)]
    public string Key { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "Name", Order = 10)]
    public string Name { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "PictureUrl", Order = 11)]
    public string PictureUrl { get; set; }

    public Achievement AchievementDetails => this;
  }
}
