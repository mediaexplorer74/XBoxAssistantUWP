// *********************************************************
// Type: Microsoft.XMedia.Service.User
// Assembly: API.Contracts.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F4DA62B-04F1-4176-9D4F-6166D697B436
// *********************************************************API.Contracts.WinPhone.dll

using System;
using System.Runtime.Serialization;


namespace Microsoft.XMedia.Service
{
  [DataContract]
  public class User : BaseResource
  {
    [DataMember]
    public string UserId { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Puid { get; set; }

    [DataMember]
    public string Xuid { get; set; }

    [DataMember]
    public string GamerTag { get; set; }

    [DataMember]
    public string[] AllowedContracts { get; set; }

    [DataMember]
    public DateTime? CreatedDate { get; set; }

    [DataMember]
    public DateTime? LastLogOnDate { get; set; }

    [DataMember]
    public int? DebugFlags { get; set; }

    public override string[] OwnerIds
    {
      get => new string[1]{ this.UserId };
    }
  }
}
