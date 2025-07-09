// *********************************************************
// Type: Xbox.Live.Phone.Services.UserClaims.UserClaimsResponseParser
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.IO;
using System.Json;
using System.Text;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services.UserClaims
{
  public static class UserClaimsResponseParser
  {
    private const string GamertagKey = "gamerTag";
    private const string XuidKey = "xuid";

    public static UserInfo ParseGetUserInfoResponse(string response)
    {
      UserInfo userInfoResponse = new UserInfo();
      if (!string.IsNullOrEmpty(response))
      {
        using (MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(response)))
        {
          JsonObject jsonObject = (JsonObject) JsonValue.Load((Stream) memoryStream);
          if (jsonObject != null)
          {
            userInfoResponse.Gamertag = JsonHelper.TryGetStringValueForKey(jsonObject, "gamerTag");
            ulong? unsignedLongValueForKey = JsonHelper.TryGetUnsignedLongValueForKey(jsonObject, "xuid");
            userInfoResponse.XboxUserId = !unsignedLongValueForKey.HasValue ? 0UL : unsignedLongValueForKey.Value;
          }
        }
      }
      return userInfoResponse;
    }
  }
}
