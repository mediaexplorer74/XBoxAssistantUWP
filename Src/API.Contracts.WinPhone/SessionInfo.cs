// *********************************************************
// Type: Microsoft.XMedia.Service.SessionInfo
// Assembly: API.Contracts.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F4DA62B-04F1-4176-9D4F-6166D697B436
// *********************************************************API.Contracts.WinPhone.dll

using System;
using System.Runtime.Serialization;


namespace Microsoft.XMedia.Service
{
  [DataContract]
  public class SessionInfo : BaseResource
  {
    [DataMember]
    public string BaseUri { get; set; }

    [DataMember]
    public string SessionId { get; set; }

    [DataMember]
    public string[] ParticipantUserIds { get; set; }

    [DataMember]
    public string LanConnectionIPAddress { get; set; }

    [DataMember]
    public string LanKey { get; set; }

    internal DateTime LastModified { get; set; }

    public override string[] OwnerIds => this.ParticipantUserIds;

    internal int? DebugFlags { get; set; }
  }
}
