// *********************************************************
// Type: Xbox.Live.Phone.Services.AvatarStoreCache
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils.Cache;


namespace Xbox.Live.Phone.Services
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class AvatarStoreCache(string key) : CacheItem(key)
  {
    [DataMember]
    public string LastModified { get; set; }

    [DataMember]
    public List<StoreData> AvatarStores { get; set; }
  }
}
