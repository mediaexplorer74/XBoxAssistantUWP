// *********************************************************
// Type: Xbox.Live.Phone.Services.Programming.ProgrammingParser
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Xbox.Live.Phone.Utils;
using Xbox.Live.Phone.Utils.Linq;


namespace Xbox.Live.Phone.Services.Programming
{
  public class ProgrammingParser
  {
    private const string ComponentName = "ProgrammingParser";
    private const string NavToUri = "ECNAVTOURI";
    private const string NavToMarketplace = "ECNAVMARKETPLACESCRIPTACTION";
    private const string ChannelKeyword = "channel";
    private const string VideoKeyword = "VIDEO";
    private const string GameKeyword = "GAMES";
    private const string MusicKeyword = "MUSIC";
    private const string IdKeyword = "id";
    private const string SlotKeyword = "slot";
    private const string SlotgroupKeyword = "slotgroup";
    private const string ClickKeyword = "onclick";
    private const string CommandKeyword = "cmd";
    private const string ParamKeyword = "param";
    private const string Param6Keyword = "param6";
    private const string GameDetailsIndicator = "GAMEDETAILS";
    private const string ContentDetailsIndicator = "CONTENTDETAILS";
    private const string MediaIdIndicator = "MediaId=";
    private const string ContentMediaIdIndicator = "ContentMediaId=";
    private const string ContentMediaTypeIdIndicator = "ContentMediaTypeId=";
    private const string AlbumDetailsPageIndicator = "ALBUMDETAILSPAGE";
    private const string ArtistDetailsPageIndicator = "ARTISTDETAILSPAGE";
    private const string AlbumIdIndicator = "AlbumId=";
    private const string ArtistIdIndicator = "ArtistId=";
    private const string MovieDetailsPageIndicator = "MOVIEDETAILSPAGE";
    private const string TvSeriesDetailsPageIndicator = "TVSERIESDETAILSPAGE";
    private const string TvSeasonDetailsPageIndicator = "TVSEASONDETAILSPAGE";
    private const string TvEpisodeDetailsPageIndicator = "TVEPISODEDETAILSPAGE";
    private const string IdIndicator = "Id=";
    private const string SeriesIdIndicator = "SeriesId=";
    private const string SeasonNumberIndicator = "SeasonNumber=";
    private const string ParamKeyWordTitle = "title:";
    private const string ParamKeyWordApp = "app:";
    private const string ParamKeyWordHomePage = "HomePage";
    private const int GuidLength = 36;
    private const uint GameLauncherTitleId = 0;
    private const uint ZuneTitleId = 1481115739;
    private static readonly char[] DeepLinkSeparator = new char[1]
    {
      '?'
    };
    private static readonly char[] MediaArgSeparator = new char[2]
    {
      '&',
      ';'
    };
    private static readonly char[] ParameterSeparator = new char[1]
    {
      ':'
    };
    private static readonly char[] AppTitleIdSeparator = new char[2]
    {
      '{',
      '}'
    };

    public static List<PromoItem> GetProgrammingItems(string result)
    {
      List<PromoItem> programmingItems = new List<PromoItem>();
      programmingItems.AddRange((IEnumerable<PromoItem>) ProgrammingParser.GetFeaturedMediaItemList(result, "VIDEO"));
      programmingItems.AddRange((IEnumerable<PromoItem>) ProgrammingParser.GetFeaturedMediaItemList(result, "MUSIC"));
      programmingItems.AddRange((IEnumerable<PromoItem>) ProgrammingParser.GetFeaturedGameList(result));
      return programmingItems;
    }

    private static void ParseCommonFeed(XElement slot, PromoItem item)
    {
      item.ImageUrl = LinqHelper.GetValue(slot, (XName) "shallowimg", string.Empty);
      item.Title = LinqHelper.GetValue(slot, (XName) "description", string.Empty);
      item.Description = LinqHelper.GetValue(slot, (XName) "description2", string.Empty);
    }

    private static string GetCommand(XElement slot)
    {
      return LinqHelper.GetValue(slot, (XName) "onclick", (XName) "cmd", string.Empty);
    }

    private static string GetParam(XElement slot)
    {
      return LinqHelper.GetValue(slot, (XName) "onclick", (XName) "param", string.Empty);
    }

    private static string GetMarketplaceDeeplink(XElement slot)
    {
      return LinqHelper.GetValue(slot, (XName) "onclick", (XName) "param6", string.Empty);
    }

    private static bool ValidateLaunchParameter(string param)
    {
      return (param.StartsWith("title:", StringComparison.OrdinalIgnoreCase) || param.StartsWith("app:", StringComparison.OrdinalIgnoreCase)) && !param.EndsWith("HomePage", StringComparison.OrdinalIgnoreCase);
    }

    private static PromoItem GetFeaturedMediaItem(XElement slot, string channelKeyword)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      PromoItem featuredMediaItem = (PromoItem) null;
      switch (ProgrammingParser.GetCommand(slot).ToUpperInvariant())
      {
        case "ECNAVTOURI":
          string str = ProgrammingParser.GetParam(slot);
          if (ProgrammingParser.ValidateLaunchParameter(str))
          {
            string[] strArray = str.Split(ProgrammingParser.ParameterSeparator);
            if (strArray.Length == 3)
            {
              featuredMediaItem = new PromoItem();
              ProgrammingParser.ParseCommonFeed(slot, featuredMediaItem);
              uint result = 0;
              uint.TryParse(strArray[1], NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out result);
              if (result == 0U)
                uint.TryParse(strArray[1].Split(ProgrammingParser.AppTitleIdSeparator)[0], NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out result);
              featuredMediaItem.Provider = result;
              featuredMediaItem.DeepLink = result == 0U ? str : strArray[2];
              featuredMediaItem.MediaId = strArray[2];
              break;
            }
            break;
          }
          break;
        case "ECNAVMARKETPLACESCRIPTACTION":
          string marketplaceDeeplink = ProgrammingParser.GetMarketplaceDeeplink(slot);
          if (!string.IsNullOrWhiteSpace(marketplaceDeeplink))
          {
            featuredMediaItem = new PromoItem();
            ProgrammingParser.ParseCommonFeed(slot, featuredMediaItem);
            featuredMediaItem.DeepLink = marketplaceDeeplink;
            featuredMediaItem.Provider = 1481115739U;
            break;
          }
          break;
      }
      if (featuredMediaItem != null && featuredMediaItem.Provider == 1481115739U && !string.IsNullOrWhiteSpace(featuredMediaItem.DeepLink))
      {
        string[] strArray = featuredMediaItem.DeepLink.Split(ProgrammingParser.DeepLinkSeparator);
        if (strArray.Length < 2)
          return featuredMediaItem;
        switch (strArray[0].ToUpperInvariant())
        {
          case "MOVIEDETAILSPAGE":
            featuredMediaItem.ItemType = PromoItemType.Movie;
            featuredMediaItem.MediaId = ProgrammingParser.GetMediaIdFromDeepLink("Id=", strArray[1]);
            break;
          case "TVSERIESDETAILSPAGE":
            featuredMediaItem.ItemType = PromoItemType.TvSeries;
            featuredMediaItem.MediaId = ProgrammingParser.GetMediaIdFromDeepLink("Id=", strArray[1]);
            break;
          case "TVSEASONDETAILSPAGE":
            featuredMediaItem.ItemType = PromoItemType.TvSeason;
            featuredMediaItem.TVSeriesId = ProgrammingParser.GetMediaIdFromDeepLink("SeriesId=", strArray[1]);
            featuredMediaItem.MediaId = featuredMediaItem.TVSeriesId;
            featuredMediaItem.TVSeasonNumber = ProgrammingParser.GetMediaIdFromDeepLink("SeasonNumber=", strArray[1]);
            break;
          case "TVEPISODEDETAILSPAGE":
            featuredMediaItem.ItemType = PromoItemType.TvEpisode;
            featuredMediaItem.MediaId = ProgrammingParser.GetMediaIdFromDeepLink("Id=", strArray[1]);
            break;
          case "ALBUMDETAILSPAGE":
            featuredMediaItem.ItemType = PromoItemType.Album;
            featuredMediaItem.MediaId = ProgrammingParser.GetMediaIdFromDeepLink("AlbumId=", strArray[1]);
            break;
          case "ARTISTDETAILSPAGE":
            featuredMediaItem.ItemType = PromoItemType.Artist;
            featuredMediaItem.MediaId = ProgrammingParser.GetMediaIdFromDeepLink("ArtistId=", strArray[1]);
            break;
          default:
            featuredMediaItem.MediaId = string.Empty;
            break;
        }
      }
      if (featuredMediaItem != null)
      {
        switch (channelKeyword)
        {
          case "MUSIC":
            featuredMediaItem.ChannelType = PromoChannelType.Music;
            break;
          case "VIDEO":
            featuredMediaItem.ChannelType = PromoChannelType.Video;
            break;
          default:
            featuredMediaItem.ChannelType = PromoChannelType.Undefined;
            break;
        }
      }
      return featuredMediaItem;
    }

    private static string GetMediaIdFromDeepLink(string idIndicator, string deepLink)
    {
      if (string.IsNullOrWhiteSpace(deepLink))
        return string.Empty;
      string mediaIdFromDeepLink = string.Empty;
      foreach (string str in deepLink.Split(ProgrammingParser.MediaArgSeparator))
      {
        if (str.StartsWith(idIndicator, StringComparison.OrdinalIgnoreCase))
          mediaIdFromDeepLink = str.Substring(idIndicator.Length);
      }
      return mediaIdFromDeepLink;
    }

    private static PromoItem GetFeaturedGameItem(XElement slot)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      string command = ProgrammingParser.GetCommand(slot);
      if (string.IsNullOrWhiteSpace(command))
        return (PromoItem) null;
      PromoItem featuredGameItem = (PromoItem) null;
      switch (command.ToUpperInvariant())
      {
        case "ECNAVTOURI":
          string str = ProgrammingParser.GetParam(slot);
          if (!string.IsNullOrWhiteSpace(str))
          {
            featuredGameItem = new PromoItem();
            ProgrammingParser.ParseCommonFeed(slot, featuredGameItem);
            featuredGameItem.DeepLink = str;
            featuredGameItem.Provider = 0U;
            string[] strArray = featuredGameItem.DeepLink.Split(ProgrammingParser.ParameterSeparator);
            if (strArray.Length >= 2)
            {
              switch (strArray[0].ToUpperInvariant())
              {
                case "GAMEDETAILS":
                  if (strArray[1].Length >= 36)
                  {
                    featuredGameItem.ItemType = PromoItemType.Game;
                    featuredGameItem.MediaId = strArray[1].Substring(0, 36);
                    break;
                  }
                  break;
                case "CONTENTDETAILS":
                  featuredGameItem.ItemType = PromoItemType.GameContent;
                  featuredGameItem.MediaId = ProgrammingParser.GetMediaIdFromDeepLink("MediaId=", strArray[1]);
                  featuredGameItem.ContentMediaId = ProgrammingParser.GetMediaIdFromDeepLink("ContentMediaId=", strArray[1]);
                  featuredGameItem.ContentMediaTypeId = ProgrammingParser.GetMediaIdFromDeepLink("ContentMediaTypeId=", strArray[1]);
                  break;
              }
            }
            else
              break;
          }
          else
            break;
          break;
      }
      if (featuredGameItem != null)
        featuredGameItem.ChannelType = PromoChannelType.Game;
      return featuredGameItem;
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "prevent random error crashes app")]
    private static List<PromoItem> GetFeaturedMediaItemList(string response, string channelKeyword)
    {
      List<PromoItem> featuredMediaItemList = new List<PromoItem>();
      try
      {
        XElement xelement1 = XDocument.Parse(response).Descendants((XName) "channel").Where<XElement>((Func<XElement, bool>) (channel => channel.Element((XName) "id") != null && channel.Descendants((XName) "id").First<XElement>().Value.StartsWith(channelKeyword, StringComparison.Ordinal))).First<XElement>();
        XElement xelement2 = (XElement) null;
        switch (channelKeyword)
        {
          case "VIDEO":
            xelement2 = xelement1.Descendants((XName) "slotgroup").Select<XElement, XElement>((Func<XElement, XElement>) (entry => entry)).Skip<XElement>(1).First<XElement>();
            break;
          case "MUSIC":
            xelement2 = xelement1.Descendants((XName) "slotgroup").Select<XElement, XElement>((Func<XElement, XElement>) (entry => entry)).Skip<XElement>(2).First<XElement>();
            break;
        }
        if (xelement2 != null)
        {
          IEnumerable<PromoItem> source = xelement2.Descendants((XName) "slot").Select<XElement, PromoItem>((Func<XElement, PromoItem>) (slot => ProgrammingParser.GetFeaturedMediaItem(slot, channelKeyword)));
          featuredMediaItemList.AddRange(source.Where<PromoItem>((Func<PromoItem, bool>) (item => item != null)));
        }
      }
      catch (Exception ex)
      {
      }
      return featuredMediaItemList;
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "prevent random error crashes app")]
    private static List<PromoItem> GetFeaturedGameList(string response)
    {
      List<PromoItem> featuredGameList = new List<PromoItem>();
      try
      {
        foreach (PromoItem promoItem in XDocument.Parse(response).Descendants((XName) "channel").Where<XElement>((Func<XElement, bool>) (channel => channel.Element((XName) "id") != null && channel.Descendants((XName) "id").First<XElement>().Value.StartsWith("GAMES", StringComparison.Ordinal))).First<XElement>().Descendants((XName) "slotgroup").Select<XElement, XElement>((Func<XElement, XElement>) (entry => entry)).Skip<XElement>(1).First<XElement>().Descendants((XName) "slot").Select<XElement, PromoItem>((Func<XElement, PromoItem>) (slot => ProgrammingParser.GetFeaturedGameItem(slot))))
        {
          if (promoItem != null)
            featuredGameList.Add(promoItem);
        }
      }
      catch (Exception ex)
      {
      }
      return featuredGameList;
    }
  }
}
