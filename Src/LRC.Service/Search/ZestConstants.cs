// *********************************************************
// Type: LRC.Service.Search.ZestConstants
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System.Xml.Linq;


namespace LRC.Service.Search
{
  internal static class ZestConstants
  {
    internal const string DefaultImageUrlBaseUri = "http://image.catalog.zune.net/v3.2/{0}/image/{1}/?width=200&resize=true";
    internal const string UUIDheader = "urn:uuid:";
    internal const string DesiredSmallImageWidth = "136";
    internal const string DesiredSmallImageHeight = "102";
    internal const string DesiredLargeImageWidth = "1012";
    internal const string DesiredLargeImageHeight = "693";
    internal const string ParentItemType = "MusicItem";
    internal const string ItemTypeMusicArtist = "MusicArtist";
    internal const string ItemTypeMusicAlbum = "MusicAlbum";
    internal const string ItemTypeMusicTrack = "MusicTrack";
    internal static readonly XNamespace ZestNamespace = (XNamespace) "http://schemas.zune.net/catalog/music/2007/10";
    internal static readonly XNamespace AtomNamespace = (XNamespace) "http://www.w3.org/2005/Atom";
    internal static readonly XName Entry = ZestConstants.AtomNamespace + "entry";
    internal static readonly XName Content = ZestConstants.AtomNamespace + "content";
    internal static readonly XName Instances = ZestConstants.ZestNamespace + "instances";
    internal static readonly XName ImageInstance = ZestConstants.ZestNamespace + "imageInstance";
    internal static readonly XName Width = ZestConstants.ZestNamespace + "width";
    internal static readonly XName Height = ZestConstants.ZestNamespace + "height";
    internal static readonly XName Url = ZestConstants.ZestNamespace + "url";
    internal static readonly XName AtomTitle = ZestConstants.AtomNamespace + "title";
    internal static readonly XName ZestTitle = ZestConstants.ZestNamespace + "title";
    internal static readonly XName AtomId = ZestConstants.AtomNamespace + "id";
    internal static readonly XName ZestId = ZestConstants.ZestNamespace + "id";
    internal static readonly XName Image = ZestConstants.ZestNamespace + "image";
    internal static readonly XName PrimaryArtist = ZestConstants.ZestNamespace + "primaryArtist";
    internal static readonly XName Name = ZestConstants.ZestNamespace + "name";
    internal static readonly XName IsActionable = ZestConstants.ZestNamespace + "isActionable";
    internal static readonly XName ReleaseDate = ZestConstants.ZestNamespace + "releaseDate";
    internal static readonly XName DiscNumber = ZestConstants.ZestNamespace + "discNumber";
    internal static readonly XName TrackNumber = ZestConstants.ZestNamespace + "trackNumber";
    internal static readonly XName Length = ZestConstants.ZestNamespace + "length";
    internal static readonly XName PrimaryGenre = ZestConstants.ZestNamespace + "primaryGenre";
  }
}
