// *********************************************************
// Type: Xbox.Live.Phone.Services.ProductResponse`1
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Collections.Generic;


namespace Xbox.Live.Phone.Services
{
  public class ProductResponse<T>
  {
    public List<T> ProductList { get; set; }

    public int TotalItems { get; set; }
  }
}
