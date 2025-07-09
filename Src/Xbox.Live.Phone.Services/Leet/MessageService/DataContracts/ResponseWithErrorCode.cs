// *********************************************************
// Type: Leet.MessageService.DataContracts.ResponseWithErrorCode
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Runtime.Serialization;


namespace Leet.MessageService.DataContracts
{
  [DataContract(Name = "ResponseWithErrorCode", Namespace = "http://schemas.datacontract.org/2004/07/GDS.Contracts")]
  public class ResponseWithErrorCode
  {
    [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "ErrorCode", Order = 0)]
    public uint ErrorCode { get; set; }

    [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "ErrorMessage", Order = 1)]
    public string ErrorMessage { get; set; }
  }
}
