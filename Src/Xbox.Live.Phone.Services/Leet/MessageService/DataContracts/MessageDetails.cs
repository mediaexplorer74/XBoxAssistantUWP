// *********************************************************
// Type: Leet.MessageService.DataContracts.MessageDetails
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace Leet.MessageService.DataContracts
{
  [XmlRoot(Namespace = "")]
  [DataContract(Name = "MessageDetails", Namespace = "http://schemas.datacontract.org/2004/07/GDS.Contracts")]
  public class MessageDetails
  {
    [DataMember]
    public uint MessageId { get; set; }

    [DataMember]
    public DateTime SentTime { get; set; }

    [DataMember]
    public string SenderGamerTag { get; set; }

    [DataMember]
    public string MessageBody { get; set; }
  }
}
