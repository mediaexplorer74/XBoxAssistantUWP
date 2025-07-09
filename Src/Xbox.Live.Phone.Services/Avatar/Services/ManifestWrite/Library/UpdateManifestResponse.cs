// *********************************************************
// Type: Avatar.Services.ManifestWrite.Library.UpdateManifestResponse
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Runtime.Serialization;


namespace Avatar.Services.ManifestWrite.Library
{
  [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Avatar.Services.ManifestWrite.Library")]
  public class UpdateManifestResponse
  {
    [DataMember]
    public int SuccessCode { get; set; }
  }
}
