// *********************************************************
// Type: LRC.Service.InfoType
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.Service
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public enum InfoType
  {
    albums,
    biography,
    backgroundImage,
    images,
    overview,
    primaryImage,
    relatedAlbums,
    review,
  }
}
