// *********************************************************
// Type: Xbox.Live.Phone.Services.TitleHistory.TitleHistoryResponseParser
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Text;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services.TitleHistory
{
  public static class TitleHistoryResponseParser
  {
    private const string TitleListKey = "titles";
    private const string NameKey = "name";
    private const string PlatformsKey = "platforms";
    private const string TitleIdKey = "titleId";
    private const string TitleTypeKey = "titleType";

    public static List<TitleHistoryInfo> ParseGetTitleHistory(string response)
    {
      List<TitleHistoryInfo> getTitleHistory = new List<TitleHistoryInfo>();
      if (!string.IsNullOrEmpty(response))
      {
        using (MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(response)))
        {
          JsonObject jsonObject1 = (JsonObject) JsonValue.Load((Stream) memoryStream);
          if (jsonObject1 != null)
          {
            JsonArray arrayValueForKey1 = JsonHelper.TryGetJsonArrayValueForKey(jsonObject1, "titles");
            if (arrayValueForKey1 != null)
            {
              foreach (JsonObject jsonObject2 in (IEnumerable<JsonValue>) arrayValueForKey1)
              {
                TitleHistoryInfo titleHistoryInfo = new TitleHistoryInfo();
                JsonArray arrayValueForKey2 = JsonHelper.TryGetJsonArrayValueForKey(jsonObject2, "platforms");
                if (arrayValueForKey2 != null && arrayValueForKey2.Any<JsonValue>((Func<JsonValue, bool>) (t => (uint) t == 1U)))
                {
                  titleHistoryInfo.PlatformType = PlatformType.Xbox360;
                  titleHistoryInfo.Name = JsonHelper.TryGetStringValueForKey(jsonObject2, "name");
                  uint? unsignedIntValueForKey1 = JsonHelper.TryGetUnsignedIntValueForKey(jsonObject2, "titleId");
                  if (unsignedIntValueForKey1.HasValue)
                  {
                    titleHistoryInfo.TitleId = unsignedIntValueForKey1.Value;
                    uint? unsignedIntValueForKey2 = JsonHelper.TryGetUnsignedIntValueForKey(jsonObject2, "titleType");
                    if (unsignedIntValueForKey2.HasValue)
                      titleHistoryInfo.TitleType = (TitleType) unsignedIntValueForKey2.Value;
                    getTitleHistory.Add(titleHistoryInfo);
                  }
                }
              }
            }
          }
        }
      }
      return getTitleHistory;
    }
  }
}
