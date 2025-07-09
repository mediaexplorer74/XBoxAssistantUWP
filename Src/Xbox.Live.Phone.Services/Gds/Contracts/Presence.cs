// *********************************************************
// Type: Gds.Contracts.Presence
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Runtime.Serialization;


namespace Gds.Contracts
{
  [DataContract(Name = "Presence", Namespace = "")]
  public class Presence
  {
    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "OnlineState", Order = 0)]
    public uint OnlineState { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "LastSeenDateTime", Order = 1)]
    public DateTime LastSeenDateTime { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "LastSeenTitleId", Order = 2)]
    public uint LastSeenTitleId { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "LastSeenTitleName", Order = 3)]
    public string LastSeenTitleName { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "DetailedPresence", Order = 4)]
    public string DetailedPresence { get; set; }
  }
}
