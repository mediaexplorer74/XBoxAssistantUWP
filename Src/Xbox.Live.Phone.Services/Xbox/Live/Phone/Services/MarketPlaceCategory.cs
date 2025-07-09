// *********************************************************
// Type: Xbox.Live.Phone.Services.MarketPlaceCategory
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Collections.Generic;
using System.Linq;


namespace Xbox.Live.Phone.Services
{
  public class MarketPlaceCategory
  {
    private MarketPlacePage<Category> parentCategory;

    public MarketPlaceCategory(MarketPlacePage<Category> parentCategory)
    {
      if (parentCategory == null)
        return;
      this.parentCategory = parentCategory;
    }

    public MarketPlacePage<Category> ParentCategory
    {
      get => this.parentCategory;
      set => this.parentCategory = value;
    }

    public void UpdateSubCategories(
      int parentCategoryId,
      List<Category> subCategoryList,
      int totalSubCategories,
      int pageNumber)
    {
      Category category = this.ParentCategory.CurrentDisplayedList.Where<Category>((Func<Category, bool>) (c => c.CategoryId == parentCategoryId)).First<Category>();
      if (category == null || category.CategoryId != parentCategoryId)
        return;
      category.TotalAvailableSubCategories = totalSubCategories;
      if (category.SubCategories == null)
        category.SubCategories = new Dictionary<int, List<Category>>();
      if (category.SubCategories.ContainsKey(pageNumber))
        category.SubCategories[pageNumber] = subCategoryList;
      else
        category.SubCategories.Add(pageNumber, subCategoryList);
    }

    public List<Category> GetCachedSubCategories(int parentCategoryId, int pageNumber)
    {
      if (this.ParentCategory != null && this.ParentCategory.CurrentDisplayedList != null)
      {
        Category category = this.ParentCategory.CurrentDisplayedList.Where<Category>((Func<Category, bool>) (c => c.CategoryId == parentCategoryId)).First<Category>();
        if (category != null && category.CategoryId == parentCategoryId && category.SubCategories != null && category.SubCategories.ContainsKey(pageNumber))
          return category.SubCategories[pageNumber];
      }
      return (List<Category>) null;
    }

    public bool IsNextPageForSubCategoriesAvailable(
      int parentCategoryId,
      int requestedPageSize,
      int requestedPageNumber)
    {
      Category category = this.ParentCategory.CurrentDisplayedList.Where<Category>((Func<Category, bool>) (c => c.CategoryId == parentCategoryId)).First<Category>();
      return category != null && category.CategoryId == parentCategoryId && requestedPageNumber * requestedPageSize < category.TotalAvailableSubCategories;
    }
  }
}
