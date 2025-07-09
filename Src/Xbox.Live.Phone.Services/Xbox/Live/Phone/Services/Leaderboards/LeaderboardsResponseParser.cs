// *********************************************************
// Type: Xbox.Live.Phone.Services.Leaderboards.LeaderboardsResponseParser
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Json;
using System.Text;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services.Leaderboards
{
  public static class LeaderboardsResponseParser
  {
    private const string LeaderboardsKey = "leaderboards";
    private const string TitleIdKey = "titleid";
    private const string PrimaryIdKey = "primaryid";
    private const string LeaderboardIdKey = "lbid";
    private const string NameKey = "name";
    private const string RatingKey = "rating";
    private const string LeaderboardNameKey = "lbname";
    private const string UserDataKey = "userData";
    private const string FriendsKey = "friends";
    private const string GamertagKey = "gamertag";
    private const string XboxUserIdKey = "xuid";
    private const string RankKey = "rank";

    public static LeaderboardsForTitleData ParseGetLeaderboardsForTitleResponse(string response)
    {
      LeaderboardsForTitleData forTitleResponse = new LeaderboardsForTitleData();
      if (!string.IsNullOrEmpty(response))
      {
        using (MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(response)))
        {
          JsonObject jsonObject = (JsonObject) JsonValue.Load((Stream) memoryStream);
          if (jsonObject != null)
          {
            uint? unsignedIntValueForKey1 = JsonHelper.TryGetUnsignedIntValueForKey(jsonObject, "titleid");
            if (unsignedIntValueForKey1.HasValue)
              forTitleResponse.TitleId = unsignedIntValueForKey1.Value;
            uint? unsignedIntValueForKey2 = JsonHelper.TryGetUnsignedIntValueForKey(jsonObject, "primaryid");
            if (!unsignedIntValueForKey2.HasValue)
              return forTitleResponse;
            forTitleResponse.PrimaryLeaderboardId = unsignedIntValueForKey2.Value;
            ObservableCollection<LeaderboardInfo> collectionForKey = LeaderboardsResponseParser.TryGetLeaderboardInfoCollectionForKey(jsonObject, "leaderboards");
            if (collectionForKey == null)
              return forTitleResponse;
            foreach (LeaderboardInfo leaderboardInfo in (Collection<LeaderboardInfo>) collectionForKey)
              forTitleResponse.Leaderboards.Add(leaderboardInfo.LeaderboardId, leaderboardInfo);
          }
        }
      }
      return forTitleResponse;
    }

    public static LeaderboardInfo ParseGetLeaderboardDetailsResponse(string response)
    {
      LeaderboardInfo leaderboardDetailsResponse = new LeaderboardInfo();
      if (!string.IsNullOrEmpty(response))
      {
        using (MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(response)))
        {
          JsonObject jsonObject1 = (JsonObject) JsonValue.Load((Stream) memoryStream);
          if (jsonObject1 != null)
          {
            uint? unsignedIntValueForKey = JsonHelper.TryGetUnsignedIntValueForKey(jsonObject1, "lbid");
            if (!unsignedIntValueForKey.HasValue)
              return (LeaderboardInfo) null;
            leaderboardDetailsResponse.LeaderboardId = unsignedIntValueForKey.Value;
            leaderboardDetailsResponse.Name = JsonHelper.TryGetStringValueForKey(jsonObject1, "lbname");
            if (string.IsNullOrWhiteSpace(leaderboardDetailsResponse.Name))
              return (LeaderboardInfo) null;
            leaderboardDetailsResponse.RatingHeader = JsonHelper.TryGetStringValueForKey(jsonObject1, "rating");
            if (string.IsNullOrWhiteSpace(leaderboardDetailsResponse.RatingHeader))
              return (LeaderboardInfo) null;
            JsonArray arrayValueForKey = JsonHelper.TryGetJsonArrayValueForKey(jsonObject1, "friends");
            if (arrayValueForKey != null)
            {
              foreach (JsonObject jsonObject2 in (IEnumerable<JsonValue>) arrayValueForKey)
              {
                LeaderboardEntry leaderboardEntry = LeaderboardsResponseParser.ParseLeaderboardEntry(jsonObject2);
                if (leaderboardEntry != null)
                  leaderboardDetailsResponse.LeaderboardEntries.Add(leaderboardEntry);
              }
            }
            LeaderboardEntry leaderboardEntry1 = LeaderboardsResponseParser.ParseLeaderboardEntry(JsonHelper.TryGetJsonObjectValueForKey(jsonObject1, "userData"));
            if (leaderboardEntry1 != null)
            {
              int index = 0;
              while (index < leaderboardDetailsResponse.LeaderboardEntries.Count && leaderboardDetailsResponse.LeaderboardEntries[index].Rank <= leaderboardEntry1.Rank)
                ++index;
              leaderboardDetailsResponse.LeaderboardEntries.Insert(index, leaderboardEntry1);
              leaderboardDetailsResponse.GamerEntryIndex = index;
            }
          }
        }
      }
      return leaderboardDetailsResponse;
    }

    private static LeaderboardEntry ParseLeaderboardEntry(JsonObject jsonObject)
    {
      if (jsonObject == null)
        return (LeaderboardEntry) null;
      LeaderboardEntry leaderboardEntry = new LeaderboardEntry();
      leaderboardEntry.Gamertag = JsonHelper.TryGetStringValueForKey(jsonObject, "gamertag");
      if (string.IsNullOrWhiteSpace(leaderboardEntry.Gamertag))
        return (LeaderboardEntry) null;
      ulong? unsignedLongValueForKey = JsonHelper.TryGetUnsignedLongValueForKey(jsonObject, "xuid");
      if (!unsignedLongValueForKey.HasValue)
        return (LeaderboardEntry) null;
      leaderboardEntry.XboxUserId = unsignedLongValueForKey.Value;
      uint? unsignedIntValueForKey = JsonHelper.TryGetUnsignedIntValueForKey(jsonObject, "rank");
      if (!unsignedIntValueForKey.HasValue)
        return (LeaderboardEntry) null;
      leaderboardEntry.Rank = unsignedIntValueForKey.Value;
      leaderboardEntry.Rating = JsonHelper.TryGetStringValueForKey(jsonObject, "rating");
      return string.IsNullOrWhiteSpace(leaderboardEntry.Rating) ? (LeaderboardEntry) null : leaderboardEntry;
    }

    private static ObservableCollection<LeaderboardInfo> TryGetLeaderboardInfoCollectionForKey(
      JsonObject jsonObject,
      string key)
    {
      JsonArray arrayValueForKey = JsonHelper.TryGetJsonArrayValueForKey(jsonObject, key);
      if (arrayValueForKey == null)
        return (ObservableCollection<LeaderboardInfo>) null;
      ObservableCollection<LeaderboardInfo> collectionForKey = new ObservableCollection<LeaderboardInfo>();
      foreach (JsonObject jsonObject1 in (IEnumerable<JsonValue>) arrayValueForKey)
      {
        LeaderboardInfo leaderboardInfo = new LeaderboardInfo();
        uint? unsignedIntValueForKey = JsonHelper.TryGetUnsignedIntValueForKey(jsonObject1, "lbid");
        if (unsignedIntValueForKey.HasValue)
        {
          leaderboardInfo.LeaderboardId = unsignedIntValueForKey.Value;
          leaderboardInfo.Name = JsonHelper.TryGetStringValueForKey(jsonObject1, "name");
          if (!string.IsNullOrWhiteSpace(leaderboardInfo.Name))
          {
            leaderboardInfo.RatingHeader = JsonHelper.TryGetStringValueForKey(jsonObject1, "rating");
            if (!string.IsNullOrWhiteSpace(leaderboardInfo.RatingHeader))
              collectionForKey.Add(leaderboardInfo);
          }
        }
      }
      return collectionForKey;
    }
  }
}
