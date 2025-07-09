// *********************************************************
// Type: LRC.Service.Omniture.OmniturePersistentData
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System.Runtime.Serialization;


namespace LRC.Service.Omniture
{
  [DataContract(Namespace = "")]
  public class OmniturePersistentData
  {
    [DataMember]
    public int HitDataCount { get; set; }

    [DataMember]
    public string SessionId { get; set; }
  }
}
