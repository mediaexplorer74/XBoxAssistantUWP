// *********************************************************
// Type: Microsoft.XMedia.Service.Content
// Assembly: API.Contracts.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F4DA62B-04F1-4176-9D4F-6166D697B436
// *********************************************************API.Contracts.WinPhone.dll

using System.Runtime.Serialization;


namespace Microsoft.XMedia.Service
{
  [DataContract]
  public class Content
  {
    [DataMember]
    public string ContentId { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public string[] RootDomains { get; set; }

    [DataMember]
    public string[] PartnerMediaIds { get; set; }

    [DataMember]
    public bool SandboxMode { get; set; }

    [DataMember]
    public string Secret { get; set; }

    [DataMember]
    public string[] DeveloperIds { get; set; }
  }
}
