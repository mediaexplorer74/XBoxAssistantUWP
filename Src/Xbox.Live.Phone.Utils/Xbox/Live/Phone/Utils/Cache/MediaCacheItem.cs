// *********************************************************
// Type: Xbox.Live.Phone.Utils.Cache.MediaCacheItem
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils.Etc;


namespace Xbox.Live.Phone.Utils.Cache
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class MediaCacheItem : CacheItem
  {
    public MediaCacheItem(string key, byte[] data)
      : base(MediaCacheItem.UriToFileName(key))
    {
      this.Data = data != null ? data : throw new ArgumentNullException();
      this.Size = data.Length;
      this.Checksum = Crc32.Calculate(data);
    }

    [DataMember]
    public int Size { get; set; }

    [DataMember]
    public uint Checksum { get; set; }

    [DataMember]
    public byte[] Data { get; set; }

    public static bool ExistInCache(string key)
    {
      return CacheManager.Instance.ExistInCache(MediaCacheItem.UriToFileName(key));
    }

    public static MediaCacheItem Load(string key)
    {
      MediaCacheItem mediaCacheItem = CacheManager.Load<MediaCacheItem>(MediaCacheItem.UriToFileName(key));
      bool flag = mediaCacheItem != null && mediaCacheItem.Data != null;
      if (flag)
      {
        uint num = Crc32.Calculate(mediaCacheItem.Data);
        flag = (int) mediaCacheItem.Checksum == (int) num;
      }
      return !flag || mediaCacheItem.Data.Length != mediaCacheItem.Size ? (MediaCacheItem) null : mediaCacheItem;
    }

    private static string UriToFileName(string uriString)
    {
      return uriString.Replace("_", "__").Replace(":", "_1").Replace("/", "_2").Replace("?", "_3").Replace("-", "_4");
    }
  }
}
