// *********************************************************
// Type: Xbox.Live.Phone.Services.Beacons.BeaconsResponseParser
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Text;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services.Beacons
{
  public static class BeaconsResponseParser
  {
    private const string FriendActivityListKey = "friendActivityList";
    private const string GamertagKey = "gamertag";
    private const string UserIdKey = "userId";
    private const string IsOnlineKey = "isOnline";
    private const string PresenceTitleIdKey = "presenceTitleId";
    private const string TimeSinceLastActivityKey = "timeSinceLastActivity";
    private const string BeaconKey = "beacon";
    private const string ValueKey = "value";
    private const string UnitsKey = "units";
    private const string TitleIdKey = "titleId";
    private const string BeaconTextKey = "beaconText";
    private const string LastUpdatedKey = "lastUpdated";
    private const string DefaultBeaconTextKey = "defaultBeaconText";

    public static ActivityListForTitleData ParseGetActivityListForTitleResponse(string response)
    {
      ActivityListForTitleData forTitleResponse = new ActivityListForTitleData();
      if (!string.IsNullOrEmpty(response))
      {
        using (MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(response)))
        {
          JsonObject jsonObject1 = (JsonObject) JsonValue.Load((Stream) memoryStream);
          if (jsonObject1 != null)
          {
            JsonArray arrayValueForKey = JsonHelper.TryGetJsonArrayValueForKey(jsonObject1, "friendActivityList");
            if (arrayValueForKey != null)
            {
              foreach (JsonObject jsonObject2 in (IEnumerable<JsonValue>) arrayValueForKey)
              {
                Friend friend = new Friend();
                friend.Gamertag = JsonHelper.TryGetStringValueForKey(jsonObject2, "gamertag");
                if (!string.IsNullOrEmpty(friend.Gamertag))
                {
                  ulong? unsignedLongValueForKey = JsonHelper.TryGetUnsignedLongValueForKey(jsonObject2, "userId");
                  if (unsignedLongValueForKey.HasValue)
                  {
                    friend.UserId = unsignedLongValueForKey.Value;
                    bool? booleanValueForKey = JsonHelper.TryGetBooleanValueForKey(jsonObject2, "isOnline");
                    if (booleanValueForKey.HasValue)
                    {
                      friend.IsOnline = booleanValueForKey.Value;
                      friend.PresenceTitleId = JsonHelper.TryGetUnsignedIntValueForKey(jsonObject2, "presenceTitleId");
                      friend.TimeSinceLastActivity = BeaconsResponseParser.TryGetSimpleDurationValueForKey(jsonObject2, "timeSinceLastActivity");
                      friend.Beacon = BeaconsResponseParser.TryGetBeaconInfoValueForKey(jsonObject2, "beacon");
                      forTitleResponse.FriendActivityList.Add(friend);
                    }
                  }
                }
              }
            }
          }
        }
      }
      return forTitleResponse;
    }

    public static BeaconInfo ParseGetBeaconForTitleResponse(string response)
    {
      if (string.IsNullOrEmpty(response))
        return (BeaconInfo) null;
      using (MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(response)))
        return BeaconsResponseParser.TryGetBeaconInfoFromObject((JsonObject) JsonValue.Load((Stream) memoryStream));
    }

    public static BeaconDefaultTextData ParseGetBeaconDefaultTextResponse(string response)
    {
      BeaconDefaultTextData defaultTextResponse = new BeaconDefaultTextData();
      if (!string.IsNullOrEmpty(response))
      {
        using (MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(response)))
        {
          JsonObject jsonObject = (JsonObject) JsonValue.Load((Stream) memoryStream);
          if (jsonObject != null)
            defaultTextResponse.BeaconDefaultText = JsonHelper.TryGetStringValueForKey(jsonObject, "defaultBeaconText");
        }
      }
      return defaultTextResponse;
    }

    private static SimpleDuration TryGetSimpleDurationValueForKey(JsonObject jsonObject, string key)
    {
      SimpleDuration durationValueForKey = (SimpleDuration) null;
      JsonObject objectValueForKey = JsonHelper.TryGetJsonObjectValueForKey(jsonObject, key);
      if (objectValueForKey != null)
      {
        int? intValueForKey = JsonHelper.TryGetIntValueForKey(objectValueForKey, "value");
        string stringValueForKey = JsonHelper.TryGetStringValueForKey(objectValueForKey, "units");
        if (intValueForKey.HasValue && !string.IsNullOrWhiteSpace(stringValueForKey))
        {
          durationValueForKey = new SimpleDuration();
          durationValueForKey.Value = intValueForKey.Value;
          try
          {
            durationValueForKey.Units = (SimpleDurationUnit) Enum.Parse(typeof (SimpleDurationUnit), stringValueForKey, true);
          }
          catch (ArgumentException ex)
          {
            return (SimpleDuration) null;
          }
          catch (OverflowException ex)
          {
            return (SimpleDuration) null;
          }
        }
      }
      return durationValueForKey;
    }

    private static BeaconInfo TryGetBeaconInfoValueForKey(JsonObject jsonObject, string key)
    {
      return BeaconsResponseParser.TryGetBeaconInfoFromObject(JsonHelper.TryGetJsonObjectValueForKey(jsonObject, key));
    }

    private static BeaconInfo TryGetBeaconInfoFromObject(JsonObject jsonObject)
    {
      BeaconInfo beaconInfoFromObject = (BeaconInfo) null;
      if (jsonObject != null)
      {
        uint? unsignedIntValueForKey = JsonHelper.TryGetUnsignedIntValueForKey(jsonObject, "titleId");
        if (unsignedIntValueForKey.HasValue)
        {
          beaconInfoFromObject = new BeaconInfo();
          beaconInfoFromObject.TitleId = unsignedIntValueForKey.Value;
          beaconInfoFromObject.BeaconText = JsonHelper.TryGetStringValueForKey(jsonObject, "beaconText");
          beaconInfoFromObject.LastUpdated = BeaconsResponseParser.TryGetSimpleDurationValueForKey(jsonObject, "lastUpdated");
        }
      }
      return beaconInfoFromObject;
    }
  }
}
