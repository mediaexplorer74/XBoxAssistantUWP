// *********************************************************
// Type: Xbox.Live.Phone.Services.Beacons.SetBeaconRequest
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Runtime.Serialization;


namespace Xbox.Live.Phone.Services.Beacons
{
  [DataContract]
  public class SetBeaconRequest
  {
    [DataMember(Name = "beaconText")]
    public string BeaconText { get; set; }
  }
}
