// *********************************************************
// Type: LRC.Service.Search.GameMarketplaceConstants
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using Xbox.Live.Phone;


namespace LRC.Service.Search
{
  internal static class GameMarketplaceConstants
  {
    internal const string UUIDheader = "urn:uuid:";
    internal const string DesiredImageSize = "23";
    internal const string DesiredImageRelationship = "33";
    internal const string DesiredBackgroundImageSize = "22";
    internal const string DesiredBackgroundImageRelationship = "25";
    internal const string ItemType = "Xbox360Game";
    internal const string ParentItemType = "XboxGameItem";
    internal const int GameCategoryId = 3000;
    internal const int XboxLiveGameCategoryId = 3027;
    internal const int ArcadeGameCategoryId = 3026;
    internal const string HexTitleIdSeperator = ".";
    private const string VintBaseUri = "http://marketplace-wp.vint.xboxlive.com/marketplacecatalog/v1/product/";
    private const string ProdBaseUri = "http://marketplace-wp.xboxlive.com/marketplacecatalog/v1/product/";
    internal static readonly string DetailsByMediaIdBaseUri;
    internal static readonly string GameDetailsByTitleIdBaseUri;
    internal static readonly XNamespace GamesNamespace = (XNamespace) "http://marketplace.xboxlive.com/resource/product/v1";
    internal static readonly XNamespace AtomNamespace = (XNamespace) "http://www.w3.org/2005/Atom";
    internal static readonly XName Entry = GameMarketplaceConstants.AtomNamespace + "entry";
    internal static readonly XName ReducedTitle = GameMarketplaceConstants.GamesNamespace + "reducedTitle";
    internal static readonly XName Id = GameMarketplaceConstants.AtomNamespace + "id";
    internal static readonly XName HexTitleId = GameMarketplaceConstants.GamesNamespace + "hexTitleId";
    internal static readonly XName ReducedDescription = GameMarketplaceConstants.GamesNamespace + "reducedDescription";
    internal static readonly XName CountryReleaseDate = GameMarketplaceConstants.GamesNamespace + "countryReleaseDate";
    internal static readonly XName ProductType = GameMarketplaceConstants.GamesNamespace + "productType";
    internal static readonly XName RatingId = GameMarketplaceConstants.GamesNamespace + "ratingId";
    internal static readonly XName UserRating = GameMarketplaceConstants.GamesNamespace + "userRating";
    internal static readonly XName NumberOfRatings = GameMarketplaceConstants.GamesNamespace + "numberOfRatings";
    internal static readonly XName Categories = GameMarketplaceConstants.GamesNamespace + "categories";
    internal static readonly XName Category = GameMarketplaceConstants.GamesNamespace + "category";
    internal static readonly XName CategoryId = GameMarketplaceConstants.GamesNamespace + "categoryId";
    internal static readonly XName CategoryName = GameMarketplaceConstants.GamesNamespace + "categoryName";
    internal static readonly XName Images = GameMarketplaceConstants.GamesNamespace + "images";
    internal static readonly XName Image = GameMarketplaceConstants.GamesNamespace + "image";
    internal static readonly XName Size = GameMarketplaceConstants.GamesNamespace + "size";
    internal static readonly XName RelationshipType = GameMarketplaceConstants.GamesNamespace + "relationshipType";
    internal static readonly XName FileUrl = GameMarketplaceConstants.GamesNamespace + "fileUrl";
    internal static readonly XName ParentProducts = GameMarketplaceConstants.GamesNamespace + "parentProducts";
    internal static readonly XName ParentProduct = GameMarketplaceConstants.GamesNamespace + "parentProduct";
    internal static readonly XName ParentProductId = GameMarketplaceConstants.GamesNamespace + "parentProductId";
    internal static readonly XName Developer = GameMarketplaceConstants.GamesNamespace + "developerName";
    internal static readonly XName Publisher = GameMarketplaceConstants.GamesNamespace + "publisherName";
    internal static readonly XName RatingDescriptors = GameMarketplaceConstants.GamesNamespace + "ratingDescriptors";
    internal static readonly XName RatingDescriptor = GameMarketplaceConstants.GamesNamespace + "ratingDescriptor";
    internal static readonly XName SlideShows = GameMarketplaceConstants.GamesNamespace + "slideShows";
    internal static readonly XName SlideShow = GameMarketplaceConstants.GamesNamespace + "slideShow";

    [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "We don't know the environment for explicit, and we want to make sure this only happens once.")]
    static GameMarketplaceConstants()
    {
      string str = EnvironmentState.Instance.IsProduction ? "http://marketplace-wp.xboxlive.com/marketplacecatalog/v1/product/" : "http://marketplace-wp.vint.xboxlive.com/marketplacecatalog/v1/product/";
      GameMarketplaceConstants.DetailsByMediaIdBaseUri = str + "{0}/{1}?producttypes=1.5.18.19.20.21.22.23.30.34.37.47.61&tiers=2.3.4&OfferFilter=1&platformtypes=1";
      GameMarketplaceConstants.GameDetailsByTitleIdBaseUri = str + "{0}?hextitles={1}&producttypes=1.21.23.37.61&tiers=2.3.4&OfferFilter=1&platformtypes=1";
    }
  }
}
