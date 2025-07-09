// *********************************************************
// Type: LRC.Service.Search.ZestResponseParser
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Utils.Linq;


namespace LRC.Service.Search
{
  public static class ZestResponseParser
  {
    private const string ComponentName = "ZestResponseParser";

    public static MusicAlbumData ParseMusicAlbumDetailsResponse(string serviceResponse)
    {
      try
      {
        return ZestResponseParser.CreateMusicAlbumData(XElement.Parse(serviceResponse));
      }
      catch (NullReferenceException ex)
      {
      }
      return (MusicAlbumData) null;
    }

    public static MusicArtistData ParseMusicArtistDetailsResponse(string serviceResponse)
    {
      try
      {
        return ZestResponseParser.CreateMusicArtistData(XElement.Parse(serviceResponse));
      }
      catch (NullReferenceException ex)
      {
      }
      return (MusicArtistData) null;
    }

    public static MusicArtistData ParseMusicArtistAlbumsResponse(
      MusicArtistData artistData,
      string serviceResponse)
    {
      try
      {
        XElement xelement = XElement.Parse(serviceResponse);
        if (artistData == null)
          artistData = new MusicArtistData((MusicData) ZestResponseParser.CreateMusicArtistData(xelement));
        IEnumerable<MusicAlbumData> collection = xelement.Elements(ZestConstants.Entry).Select<XElement, MusicAlbumData>((Func<XElement, MusicAlbumData>) (albumEntry => ZestResponseParser.CreateMusicAlbumData(albumEntry)));
        artistData.Albums = new List<MusicAlbumData>(collection);
        return artistData;
      }
      catch (NullReferenceException ex)
      {
      }
      return (MusicArtistData) null;
    }

    public static MusicArtistData ParseMusicArtistBioResponse(
      MusicArtistData artistData,
      string serviceResponse)
    {
      try
      {
        XElement xelement = XElement.Parse(serviceResponse);
        if (artistData == null)
          artistData = new MusicArtistData((MusicData) ZestResponseParser.CreateMusicArtistData(xelement));
        artistData.Bio = LinqHelper.TryGetScrubbedStringValue(xelement.Element(ZestConstants.Content));
        return artistData;
      }
      catch (NullReferenceException ex)
      {
      }
      return (MusicArtistData) null;
    }

    public static MusicArtistData ParseMusicArtistImagesResponse(
      MusicArtistData artistData,
      string serviceResponse)
    {
      try
      {
        XElement xelement = XElement.Parse(serviceResponse);
        if (artistData == null)
          artistData = new MusicArtistData((MusicData) ZestResponseParser.CreateMusicArtistData(xelement));
        IEnumerable<string> collection1 = xelement.Elements(ZestConstants.Entry).Elements<XElement>(ZestConstants.Instances).Elements<XElement>(ZestConstants.ImageInstance).Where<XElement>((Func<XElement, bool>) (imageInstance => "136" == LinqHelper.TryGetElementValue(imageInstance, ZestConstants.Width) && "102" == LinqHelper.TryGetElementValue(imageInstance, ZestConstants.Height))).Select<XElement, string>((Func<XElement, string>) (imageInstance => LinqHelper.TryGetElementValue(imageInstance, ZestConstants.Url)));
        artistData.SmallImages = new List<string>(collection1);
        IEnumerable<string> collection2 = xelement.Elements(ZestConstants.Entry).Elements<XElement>(ZestConstants.Instances).Elements<XElement>(ZestConstants.ImageInstance).Where<XElement>((Func<XElement, bool>) (imageInstance => "1012" == LinqHelper.TryGetElementValue(imageInstance, ZestConstants.Width) && "693" == LinqHelper.TryGetElementValue(imageInstance, ZestConstants.Height))).Select<XElement, string>((Func<XElement, string>) (imageInstance => LinqHelper.TryGetElementValue(imageInstance, ZestConstants.Url)));
        artistData.LargeImages = new List<string>(collection2);
        return artistData;
      }
      catch (NullReferenceException ex)
      {
      }
      return (MusicArtistData) null;
    }

    private static MusicData CreateMusicData(XElement item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      MusicData musicData = new MusicData();
      musicData.Name = LinqHelper.TryGetScrubbedStringValue(item.Element(ZestConstants.AtomTitle));
      musicData.Id = ZestResponseParser.GetId(item.Element(ZestConstants.AtomId));
      musicData.ImageUrl = ZestResponseParser.GetImageUrl(item.Element(ZestConstants.Image));
      musicData.ArtistId = LinqHelper.TryGetElementValue(item.Element(ZestConstants.PrimaryArtist), ZestConstants.ZestId);
      musicData.Artist = LinqHelper.TryGetElementValue(item.Element(ZestConstants.PrimaryArtist), ZestConstants.Name);
      musicData.Genres = ZestResponseParser.GetGenres(item);
      musicData.ParentItemType = "MusicItem";
      musicData.Filter = Filter.Music;
      return musicData;
    }

    private static MusicAlbumData CreateMusicAlbumData(XElement item)
    {
      MusicAlbumData musicAlbumData = new MusicAlbumData(ZestResponseParser.CreateMusicData(item));
      musicAlbumData.ItemType = "MusicAlbum";
      musicAlbumData.IsActionable = LinqHelper.TryGetBoolValue(item.Element(ZestConstants.IsActionable));
      musicAlbumData.Tracks = new List<MusicTrackData>(item.Elements(ZestConstants.Entry).Select<XElement, MusicTrackData>((Func<XElement, MusicTrackData>) (trackEntry => ZestResponseParser.CreateMusicTrackData(trackEntry))));
      musicAlbumData.ReleaseDate = LinqHelper.TryGetDateTimeValue(item.Element(ZestConstants.ReleaseDate));
      return musicAlbumData;
    }

    private static MusicArtistData CreateMusicArtistData(XElement item)
    {
      MusicArtistData musicArtistData = new MusicArtistData(ZestResponseParser.CreateMusicData(item));
      musicArtistData.ItemType = "MusicArtist";
      musicArtistData.IsActionable = new bool?(false);
      musicArtistData.Artist = LinqHelper.TryGetScrubbedStringValue(item.Element(ZestConstants.AtomTitle));
      musicArtistData.ArtistId = LinqHelper.TryGetStringValue(item.Element(ZestConstants.AtomId));
      return musicArtistData;
    }

    private static MusicTrackData CreateMusicTrackData(XElement item)
    {
      MusicTrackData musicTrackData = new MusicTrackData(ZestResponseParser.CreateMusicData(item));
      musicTrackData.DiscNumber = LinqHelper.TryGetIntValue(item.Element(ZestConstants.DiscNumber));
      musicTrackData.TrackNumber = LinqHelper.TryGetIntValue(item.Element(ZestConstants.TrackNumber));
      musicTrackData.Duration = LinqHelper.TryGetTimeSpanValue(item.Element(ZestConstants.Length));
      musicTrackData.ItemType = "MusicTrack";
      musicTrackData.IsActionable = new bool?(true);
      return musicTrackData;
    }

    private static string GetImageUrl(XElement imageElement)
    {
      string imageUrl = (string) null;
      if (imageElement != null)
      {
        string stringValue = LinqHelper.TryGetStringValue(imageElement.Element(ZestConstants.ZestId));
        if (!string.IsNullOrEmpty(stringValue))
          imageUrl = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "http://image.catalog.zune.net/v3.2/{0}/image/{1}/?width=200&resize=true", new object[2]
          {
            (object) XboxLiveGamer.CurrentGamer.LegalLocale,
            (object) stringValue
          });
      }
      return imageUrl;
    }

    private static List<string> GetGenres(XElement rootElement)
    {
      List<string> genres = new List<string>();
      XElement xelement = rootElement.Element(ZestConstants.PrimaryGenre);
      if (xelement != null)
      {
        string scrubbedStringValue = LinqHelper.TryGetScrubbedStringValue(xelement.Element(ZestConstants.ZestTitle));
        if (!string.IsNullOrEmpty(scrubbedStringValue))
          genres.Add(scrubbedStringValue);
      }
      return genres;
    }

    private static string GetId(XElement item)
    {
      string id = (string) null;
      string stringValue = LinqHelper.TryGetStringValue(item);
      if (!string.IsNullOrEmpty(stringValue))
        id = stringValue.Substring("urn:uuid:".Length);
      return id;
    }
  }
}
