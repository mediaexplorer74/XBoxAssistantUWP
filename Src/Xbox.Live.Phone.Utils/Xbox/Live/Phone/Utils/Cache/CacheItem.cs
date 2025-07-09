// *********************************************************
// Type: Xbox.Live.Phone.Utils.Cache.CacheItem
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace Xbox.Live.Phone.Utils.Cache
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public abstract class CacheItem
  {
    private object storageLock;

    public CacheItem()
    {
    }

    public CacheItem(string key)
    {
      this.StorageKey = !string.IsNullOrEmpty(key) ? key : throw new ArgumentNullException(nameof (key));
    }

    public object StorageLock
    {
      get
      {
        lock (this)
        {
          if (this.storageLock == null)
            this.storageLock = new object();
          return this.storageLock;
        }
      }
    }

    [DataMember]
    public string StorageKey { get; set; }

    [DataMember]
    public DateTime LastUpdateTime { get; set; }
  }
}
