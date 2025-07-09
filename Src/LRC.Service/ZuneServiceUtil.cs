// *********************************************************
// Type: LRC.Service.ZuneServiceUtil
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xbox.Live.Phone.Services;


namespace LRC.Service
{
  public static class ZuneServiceUtil
  {
    private const string BaseUriTemplate = "http://catalog.zune.net/v3.2/{0}/";
    private const string ImageBaseUrlTemplate = "http://image.catalog.zune.net/v3.2/{0}/image/";
    private const string MediaTypeImageBaseUrlTemplate = "http://image.catalog.zune.net/v3.2/{0}/";
    private static string baseUri = string.Empty;
    private static string artistInfoBaseUri = string.Empty;
    private static string imageBaseUrl = string.Empty;
    private static string mediaTypeImageBaseUrl = string.Empty;

    public static string BaseUri
    {
      get
      {
        if (string.IsNullOrWhiteSpace(ZuneServiceUtil.baseUri))
          ZuneServiceUtil.baseUri = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "http://catalog.zune.net/v3.2/{0}/", new object[1]
          {
            (object) XboxLiveGamer.CurrentGamer.LegalLocale
          });
        return ZuneServiceUtil.baseUri;
      }
    }

    public static string ImageBaseUrl
    {
      get
      {
        if (string.IsNullOrWhiteSpace(ZuneServiceUtil.imageBaseUrl))
          ZuneServiceUtil.imageBaseUrl = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "http://image.catalog.zune.net/v3.2/{0}/image/", new object[1]
          {
            (object) XboxLiveGamer.CurrentGamer.LegalLocale
          });
        return ZuneServiceUtil.imageBaseUrl;
      }
    }

    public static string MediaTypeImageBaseUrl
    {
      get
      {
        if (string.IsNullOrWhiteSpace(ZuneServiceUtil.mediaTypeImageBaseUrl))
          ZuneServiceUtil.mediaTypeImageBaseUrl = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "http://image.catalog.zune.net/v3.2/{0}/", new object[1]
          {
            (object) XboxLiveGamer.CurrentGamer.LegalLocale
          });
        return ZuneServiceUtil.mediaTypeImageBaseUrl;
      }
    }

    public static MediaType ParseMediaType(string typeName)
    {
      MediaType mediaType = MediaType.undefined;
      if (!string.IsNullOrEmpty(typeName))
      {
        switch (typeName.ToUpper(CultureInfo.InvariantCulture))
        {
          case "SERIES":
          case "TV_SERIES":
            mediaType = MediaType.tv_series;
            break;
          case "EPISODE":
          case "TV_EPISODE":
            mediaType = MediaType.tv_episode;
            break;
          case "TRACK":
          case "MUSIC_TRACK":
            mediaType = MediaType.music_track;
            break;
          case "ALBUM":
          case "MUSIC_ALBUM":
            mediaType = MediaType.music_album;
            break;
          case "MOVIE":
            mediaType = MediaType.movie;
            break;
          case "MOVIETRAILER":
            mediaType = MediaType.movietrailer;
            break;
          case "HUB":
            mediaType = MediaType.hub;
            break;
          case "MUSIC_ARTIST":
          case "MUSICARTIST":
            mediaType = MediaType.music_artist;
            break;
          case "MUSICVIDEO":
          case "MUSIC_MUSICVIDEO":
            mediaType = MediaType.music_musicvideo;
            break;
        }
      }
      return mediaType;
    }

    public static string GetMediaPath(string mediaTypeString, string id)
    {
      return ZuneServiceUtil.GetMediaPath(ZuneServiceUtil.ParseMediaType(mediaTypeString), id);
    }

    [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "this will be part of the URL")]
    public static string GetMediaPath(MediaType mediaType, string id)
    {
      return mediaType.ToString().Replace("_", "/") + "/" + id;
    }

    public static string GetInfoPath(InfoType info)
    {
      switch (info)
      {
        case InfoType.albums:
          return "/albums?chunkSize=100&orderBy=PlayRank";
        case InfoType.biography:
          return "/biography";
        case InfoType.backgroundImage:
          return "/backgroundImage?height=720&resize=true";
        case InfoType.images:
          return "/images";
        case InfoType.overview:
          return string.Empty;
        case InfoType.primaryImage:
          return "/primaryImage?height=200&resize=true";
        case InfoType.relatedAlbums:
          return "/relatedAlbums";
        case InfoType.review:
          return "/review";
        default:
          return string.Empty;
      }
    }

    [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "this is by design")]
    public static string GetRequestUrl(MediaType mediaType, string id, InfoType info)
    {
      return ZuneServiceUtil.BaseUri + ZuneServiceUtil.GetMediaPath(mediaType, id) + ZuneServiceUtil.GetInfoPath(info);
    }
  }
}
