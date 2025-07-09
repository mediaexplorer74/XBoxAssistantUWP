// *********************************************************
// Type: Avatar.Services.ManifestWrite.Library.UpdateGamerPicRequest
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Runtime.Serialization;


namespace Avatar.Services.ManifestWrite.Library
{
  [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Avatar.Services.ManifestWrite.Library")]
  public class UpdateGamerPicRequest
  {
    [DataMember]
    public Guid AnimationId { get; set; }

    [DataMember]
    public float Frame { get; set; }

    [DataMember]
    public bool UseProp { get; set; }

    [DataMember]
    public float OffsetX { get; set; }

    [DataMember]
    public float OffsetY { get; set; }

    [DataMember]
    public float OffsetZ { get; set; }

    [DataMember]
    public float RotationY { get; set; }

    [DataMember]
    public int Background { get; set; }

    [DataMember]
    public float FieldOfView { get; set; }

    [DataMember]
    public int FocalJoint { get; set; }
  }
}
