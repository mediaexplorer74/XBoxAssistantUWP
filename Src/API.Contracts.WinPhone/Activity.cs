// *********************************************************
// Type: Microsoft.XMedia.Service.Activity
// Assembly: API.Contracts.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F4DA62B-04F1-4176-9D4F-6166D697B436
// *********************************************************API.Contracts.WinPhone.dll

using System.Runtime.Serialization;


namespace Microsoft.XMedia.Service
{
  [DataContract]
  public class Activity
  {
    [DataMember]
    public string ActivityId { get; set; }

    [DataMember]
    public int DisplayOrder { get; set; }

    [DataMember(IsRequired = true)]
    public string DisplayName { get; set; }

    [DataMember]
    public string Icon { get; set; }

    [DataMember(IsRequired = true)]
    public string Uri { get; set; }

    internal string ContentId { get; set; }
  }
}
