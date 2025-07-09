// *********************************************************
// Type: LRC.Service.Search.GameMarketplaceResponseParser
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xbox.Live.Phone.Utils.Linq;


namespace LRC.Service.Search
{
  public static class GameMarketplaceResponseParser
  {
    public static XboxGameData ParseGameDetailsResponse(string serviceResponse)
    {
      XboxGameData gameDetailsResponse = (XboxGameData) null;
      if (!string.IsNullOrEmpty(serviceResponse))
      {
        XElement xelement = XElement.Parse(serviceResponse);
        if (xelement != null)
          gameDetailsResponse = GameMarketplaceResponseParser.GetGameDetail(xelement.Element(GameMarketplaceConstants.Entry));
      }
      return gameDetailsResponse;
    }

    public static XboxGameData GetGameDetail(XElement entry)
    {
      XboxGameData gameDetail = (XboxGameData) null;
      if (entry != null)
      {
        gameDetail = new XboxGameData();
        gameDetail.Name = LinqHelper.TryGetStringValue(entry.Element(GameMarketplaceConstants.ReducedTitle));
        gameDetail.Id = GameMarketplaceResponseParser.GetId(entry.Element(GameMarketplaceConstants.Id));
        gameDetail.TitleId = LinqHelper.TryGetHexTitleIdValue(entry.Element(GameMarketplaceConstants.HexTitleId));
        gameDetail.Description = LinqHelper.TryGetScrubbedStringValue(entry.Element(GameMarketplaceConstants.ReducedDescription));
        gameDetail.ReleaseDate = LinqHelper.TryGetDateTimeValue(entry.Element(GameMarketplaceConstants.CountryReleaseDate));
        gameDetail.ParentalRating = LinqHelper.TryGetStringValue(entry.Element(GameMarketplaceConstants.RatingId));
        gameDetail.AverageUserRating = LinqHelper.TryGetDoubleValue(entry.Element(GameMarketplaceConstants.UserRating));
        gameDetail.UserRatingCount = LinqHelper.TryGetIntValue(entry.Element(GameMarketplaceConstants.NumberOfRatings));
        gameDetail.Filter = Filter.Games;
        gameDetail.ItemType = "Xbox360Game";
        gameDetail.ParentItemType = "XboxGameItem";
        gameDetail.Genres = GameMarketplaceResponseParser.GetGenres(entry);
        gameDetail.ImageUrl = GameMarketplaceResponseParser.GetImageUrl(entry, "23", "33");
        gameDetail.BackgroundImageUrl = GameMarketplaceResponseParser.GetImageUrl(entry, "22", "25");
        gameDetail.ParentId = GameMarketplaceResponseParser.GetParentId(entry);
        gameDetail.Developer = LinqHelper.TryGetScrubbedStringValue(entry.Element(GameMarketplaceConstants.Developer));
        gameDetail.Publisher = LinqHelper.TryGetScrubbedStringValue(entry.Element(GameMarketplaceConstants.Publisher));
        gameDetail.ProductType = LinqHelper.TryGetStringValue(entry.Element(GameMarketplaceConstants.ProductType));
        gameDetail.GameRatingDescriptor = GameMarketplaceResponseParser.GetRatingDescriptor(entry);
        gameDetail.SlideShowImages = GameMarketplaceResponseParser.GetSlideShowImages(entry);
      }
      return gameDetail;
    }

    public static Dictionary<uint, XboxGameData> ParseGamesResponse(string serviceResponse)
    {
      Dictionary<uint, XboxGameData> gamesResponse = (Dictionary<uint, XboxGameData>) null;
      if (!string.IsNullOrEmpty(serviceResponse))
      {
        IEnumerable<XboxGameData> xboxGameDatas = XElement.Parse(serviceResponse).Descendants(GameMarketplaceConstants.Entry).Select<XElement, XboxGameData>((Func<XElement, XboxGameData>) (entry => GameMarketplaceResponseParser.GetGameDetail(entry)));
        gamesResponse = new Dictionary<uint, XboxGameData>();
        foreach (XboxGameData xboxGameData in xboxGameDatas)
        {
          if (!gamesResponse.ContainsKey(xboxGameData.TitleId))
            gamesResponse.Add(xboxGameData.TitleId, xboxGameData);
        }
      }
      return gamesResponse;
    }

    private static string GetId(XElement item)
    {
      string id = (string) null;
      if (item != null)
      {
        string stringValue = LinqHelper.TryGetStringValue(item);
        if (!string.IsNullOrEmpty(stringValue))
          id = stringValue.Substring("urn:uuid:".Length);
      }
      return id;
    }

    private static List<string> GetGenres(XElement item)
    {
      return item != null && item.Element(GameMarketplaceConstants.Categories) != null ? new List<string>(item.Element(GameMarketplaceConstants.Categories).Elements(GameMarketplaceConstants.Category).Select(category => new
      {
        category = category,
        id = (int) category.Element(GameMarketplaceConstants.CategoryId)
      }).Where(_param0 => _param0.id != 3000 && _param0.id != 3027 && _param0.id != 3026).OrderByDescending(_param0 => _param0.id).Select(_param0 => LinqHelper.TryGetScrubbedStringValue(_param0.category.Element(GameMarketplaceConstants.CategoryName)))) : new List<string>();
    }

    private static string GetImageUrl(XElement item, string imageSize, string relationshipType)
    {
      string imageUrl = (string) null;
      if (item != null && item.Element(GameMarketplaceConstants.Images) != null)
      {
        IEnumerable<string> source = item.Element(GameMarketplaceConstants.Images).Elements(GameMarketplaceConstants.Image).Where<XElement>((Func<XElement, bool>) (image => string.Equals(image.Element(GameMarketplaceConstants.Size).Value, imageSize, StringComparison.OrdinalIgnoreCase) && string.Equals(image.Element(GameMarketplaceConstants.RelationshipType).Value, relationshipType, StringComparison.OrdinalIgnoreCase))).Select<XElement, string>((Func<XElement, string>) (image => LinqHelper.TryGetElementValue(image, GameMarketplaceConstants.FileUrl)));
        if (source != null)
          imageUrl = source.FirstOrDefault<string>();
      }
      return imageUrl;
    }

    private static string GetParentId(XElement item)
    {
      string parentId = (string) null;
      if (item != null && item.Element(GameMarketplaceConstants.ParentProducts) != null)
      {
        IEnumerable<string> source = item.Element(GameMarketplaceConstants.ParentProducts).Elements(GameMarketplaceConstants.ParentProduct).Select<XElement, string>((Func<XElement, string>) (parentProduct => LinqHelper.TryGetElementValue(parentProduct, GameMarketplaceConstants.ParentProductId)));
        if (source != null)
          parentId = source.FirstOrDefault<string>();
      }
      return parentId;
    }

    private static List<string> GetSlideShowImages(XElement item)
    {
      return item != null && item.Element(GameMarketplaceConstants.SlideShows) != null ? new List<string>(item.Element(GameMarketplaceConstants.SlideShows).Elements(GameMarketplaceConstants.SlideShow).Elements<XElement>(GameMarketplaceConstants.Image).Select<XElement, string>((Func<XElement, string>) (image => LinqHelper.TryGetElementValue(image, GameMarketplaceConstants.FileUrl)))) : new List<string>();
    }

    private static List<string> GetRatingDescriptor(XElement item)
    {
      return item != null && item.Element(GameMarketplaceConstants.RatingDescriptors) != null ? new List<string>(item.Element(GameMarketplaceConstants.RatingDescriptors).Elements(GameMarketplaceConstants.RatingDescriptor).Select<XElement, string>((Func<XElement, string>) (ratingDescriptor => LinqHelper.TryGetScrubbedStringValue(ratingDescriptor)))) : new List<string>();
    }
  }
}
