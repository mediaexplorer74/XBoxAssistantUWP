// *********************************************************
// Type: Microsoft.XMedia.Service.Device
// Assembly: API.Contracts.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F4DA62B-04F1-4176-9D4F-6166D697B436
// *********************************************************API.Contracts.WinPhone.dll

using System;
using System.Runtime.Serialization;


namespace Microsoft.XMedia.Service
{
  [DataContract]
  public class Device
  {
    [DataMember]
    public string DeviceId { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Platform { get; set; }

    [DataMember]
    public string HardwareId { get; set; }

    [DataMember]
    public DateTime? CreatedDate { get; set; }

    [DataMember]
    public DateTime? LastLogOnDate { get; set; }

    [DataMember]
    public int? DebugFlags { get; set; }

    internal string[] PairedUserIds { get; set; }
  }
}
