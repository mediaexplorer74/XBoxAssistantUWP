// *********************************************************
// Type: Microsoft.XMedia.Service.Timeline
// Assembly: API.Contracts.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F4DA62B-04F1-4176-9D4F-6166D697B436
// *********************************************************API.Contracts.WinPhone.dll

using System;
using System.Runtime.Serialization;


namespace Microsoft.XMedia.Service
{
  [DataContract]
  public class Timeline
  {
    [DataMember]
    public string ContentId { get; set; }

    [DataMember]
    public DateTime? LastUpdated { get; set; }

    [DataMember]
    public TimelineEvent[] Events { get; set; }
  }
}
