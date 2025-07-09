// *********************************************************
// Type: Leet.MessageService.DataContracts.SendMessageRequest
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Leet.MessageService.DataContracts
{
  [DataContract(Name = "SendMessageRequest", Namespace = "http://schemas.datacontract.org/2004/07/GDS.Contracts")]
  public class SendMessageRequest
  {
    [DataMember(IsRequired = true, EmitDefaultValue = true, Order = 0, Name = "Recipients")]
    public List<string> Recipients { get; set; }

    [DataMember(IsRequired = true, EmitDefaultValue = true, Order = 1, Name = "MessageText")]
    public string MessageText { get; set; }
  }
}
