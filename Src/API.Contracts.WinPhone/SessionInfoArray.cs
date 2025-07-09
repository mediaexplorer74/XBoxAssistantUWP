// *********************************************************
// Type: Microsoft.XMedia.Service.SessionInfoArray
// Assembly: API.Contracts.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F4DA62B-04F1-4176-9D4F-6166D697B436
// *********************************************************API.Contracts.WinPhone.dll

using System.Runtime.Serialization;


namespace Microsoft.XMedia.Service
{
  [DataContract]
  public class SessionInfoArray
  {
    [DataMember]
    public PagingInfo PagingInfo { get; set; }

    [DataMember]
    public SessionInfo[] Sessions { get; set; }
  }
}
