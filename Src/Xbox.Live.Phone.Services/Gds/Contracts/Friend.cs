// *********************************************************
// Type: Gds.Contracts.Friend
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace Gds.Contracts
{
  [XmlRoot(Namespace = "")]
  [DataContract(Name = "Friend", Namespace = "")]
  public class Friend
  {
    [DataMember(Order = 0)]
    public ProfileEx ProfileEx { get; set; }

    [DataMember(Order = 1)]
    public int FriendState { get; set; }
  }
}
