// *********************************************************
// Type: Avatar.Services.ManifestRead.Library.AvatarManifest
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Runtime.Serialization;


namespace Avatar.Services.ManifestRead.Library
{
  [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Avatar.Services.ManifestRead.Library")]
  public class AvatarManifest
  {
    [DataMember]
    public string Gamertag { get; set; }

    [DataMember]
    public bool Filtered { get; set; }

    [DataMember]
    public string Manifest { get; set; }
  }
}
