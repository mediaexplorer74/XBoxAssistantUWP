// *********************************************************
// Type: Xbox.Live.Phone.Services.Product
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Runtime.Serialization;


namespace Xbox.Live.Phone.Services
{
  [DataContract]
  public class Product
  {
    [DataMember]
    public string ImageUrl { get; set; }

    [DataMember]
    public string Title { get; set; }

    [DataMember]
    public bool IsBlocked { get; set; }
  }
}
