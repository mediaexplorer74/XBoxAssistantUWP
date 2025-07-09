// *********************************************************
// Type: Xbox.Live.Phone.Services.MarketPlacePage`1
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils.Cache;


namespace Xbox.Live.Phone.Services
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class MarketPlacePage<T> : CacheItem
  {
    public MarketPlacePage() => this.Initialize();

    public MarketPlacePage(string key)
      : base(key)
    {
      this.Initialize();
    }

    [DataMember]
    [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "There is no need to create another class to hold just the list")]
    public Dictionary<int, List<T>> ProductDictionary { get; set; }

    [DataMember]
    public int TotalItemsAvailable { get; set; }

    [DataMember]
    public int ClientPageSize { get; set; }

    [DataMember]
    public int ClientPageNumber { get; set; }

    public List<T> CurrentDisplayedList { get; set; }

    public int PageNumber
    {
      get
      {
        return (this.ClientPageSize * this.ClientPageNumber - this.ClientPageSize) / this.PageSize + 1;
      }
    }

    public int PageSize { get; set; }

    public bool IsNextPageAvailable
    {
      get => this.ClientPageSize * this.ClientPageNumber < this.TotalItemsAvailable;
    }

    public List<T> CachedItems
    {
      get
      {
        List<T> cachedItems = new List<T>();
        int num = this.ClientPageSize * this.ClientPageNumber;
        int index = 0;
        int key = 1;
        if (num - this.ClientPageSize > 0)
        {
          index = (num - this.ClientPageSize) % this.PageSize;
          key = (num - this.ClientPageSize) / this.PageSize + 1;
        }
        if (this.ProductDictionary.ContainsKey(key))
        {
          List<T> product = this.ProductDictionary[key];
          if (product.Count > index + this.ClientPageSize)
            cachedItems = product.GetRange(index, this.ClientPageSize);
          else if (product.Count > index)
            cachedItems = product.GetRange(index, product.Count - index);
        }
        return cachedItems;
      }
    }

    public virtual bool IsDataAvailable()
    {
      int num = this.ClientPageSize * this.ClientPageNumber;
      int key = 1;
      if (num - this.ClientPageSize > 0)
        key = (num - this.ClientPageSize) / this.PageSize + 1;
      if (this.TotalItemsAvailable == 0)
        return true;
      return this.ProductDictionary != null && this.ProductDictionary.ContainsKey(key);
    }

    public void UpdatePage(List<T> list, int totalItems, int pageNumber)
    {
      this.TotalItemsAvailable = totalItems;
      if (list == null)
        return;
      this.ProductDictionary[pageNumber] = list;
    }

    private void Initialize()
    {
      this.ProductDictionary = new Dictionary<int, List<T>>();
      this.TotalItemsAvailable = -1;
    }
  }
}
