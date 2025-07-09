// *********************************************************
// Type: Xbox.Live.Phone.Services.ServiceErrorMessage
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Runtime.Serialization;


namespace Xbox.Live.Phone.Services
{
  [DataContract(Name = "ServiceErrorMessage", Namespace = "http://schemas.datacontract.org/2004/07/Leet.Core.Utils")]
  public class ServiceErrorMessage
  {
    [DataMember]
    public string ApiName { get; set; }

    [DataMember]
    public uint LIVEnErrorCode { get; set; }

    [DataMember]
    public string ErrorMessage { get; set; }
  }
}
