// *********************************************************
// Type: Xbox.Live.Phone.Services.PromotedCategoryUtility
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Collections.Generic;


namespace Xbox.Live.Phone.Services
{
  public class PromotedCategoryUtility
  {
    private Dictionary<int, MarketPlacePage<Category>> promotedDictionary;

    public PromotedCategoryUtility()
    {
      this.promotedDictionary = new Dictionary<int, MarketPlacePage<Category>>();
    }

    public MarketPlacePage<Category> GetPromotedCategory(int categoryId)
    {
      if (!this.promotedDictionary.ContainsKey(categoryId))
        this.promotedDictionary[categoryId] = new MarketPlacePage<Category>();
      return this.promotedDictionary[categoryId];
    }
  }
}
