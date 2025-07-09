// *********************************************************
// Type: LRC.Service.Search.SearchData
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System;
using System.Collections.Generic;


namespace LRC.Service.Search
{
  public class SearchData
  {
    public SearchData()
      : this((SearchData) null)
    {
    }

    public SearchData(SearchData searchData)
    {
      if (searchData == null)
        return;
      this.Id = searchData.Id;
      this.Name = searchData.Name;
      this.Description = searchData.Description;
      this.DetailsUrl = searchData.DetailsUrl;
      this.BackgroundImageUrl = searchData.BackgroundImageUrl;
      this.IsActionable = searchData.IsActionable;
      this.ReleaseDate = searchData.ReleaseDate;
      this.ItemType = searchData.ItemType;
      this.ParentItemType = searchData.ParentItemType;
      this.Filter = searchData.Filter;
      this.ImageUrl = searchData.ImageUrl;
      this.ImageHeight = searchData.ImageHeight;
      this.ImageWidth = searchData.ImageWidth;
      this.Genres = new List<string>((IEnumerable<string>) searchData.Genres);
      this.ParentalRating = searchData.ParentalRating;
      this.ParentalRatingSystem = searchData.ParentalRatingSystem;
      this.AverageUserRating = searchData.AverageUserRating;
      this.UserRatingCount = searchData.UserRatingCount;
      this.CriticRating = searchData.CriticRating;
    }

    public string Id { get; set; }

    public string ContinuationToken { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string DetailsUrl { get; set; }

    public bool? IsActionable { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public string ArtistName { get; set; }

    public string ItemType { get; set; }

    public string ParentItemType { get; set; }

    public Filter Filter { get; set; }

    public string BackgroundImageUrl { get; set; }

    public string ImageUrl { get; set; }

    public double? ImageHeight { get; set; }

    public double? ImageWidth { get; set; }

    public List<string> Genres { get; set; }

    public string ParentalRating { get; set; }

    public string ParentalRatingSystem { get; set; }

    public double? CriticRating { get; set; }

    public double? AverageUserRating { get; set; }

    public int? UserRatingCount { get; set; }

    public bool? TvSeriesHasSeasons { get; set; }
  }
}
