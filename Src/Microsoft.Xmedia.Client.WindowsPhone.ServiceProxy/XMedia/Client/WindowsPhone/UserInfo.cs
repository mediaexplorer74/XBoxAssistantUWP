// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.UserInfo
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using Microsoft.XMedia.Service;
using System.Collections.Generic;
using System.IO;
using System.Json;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public class UserInfo
  {
    internal UserInfo()
    {
      this.Id = "";
      this.GamerTag = "";
      this.DisplayName = "";
    }

    internal UserInfo(string id, string gamerTag, string displayName)
    {
      this.Id = id;
      this.GamerTag = gamerTag;
      this.DisplayName = displayName;
    }

    public string Id { get; private set; }

    public string GamerTag { get; private set; }

    public string DisplayName { get; private set; }

    internal static UserInfo FromJsonString(string jsonString)
    {
      return UserInfo.UserInfoFromContract(UserInfo.UserContractFromJson(jsonString));
    }

    internal static string ToJsonString(UserInfo userInfo)
    {
      return UserInfo.UserContractToJsonString(UserInfo.UserContractFromInfo(userInfo));
    }

    private static User UserContractFromInfo(UserInfo info)
    {
      return new User()
      {
        UserId = info.Id,
        GamerTag = info.GamerTag,
        Name = info.DisplayName
      };
    }

    private static UserInfo UserInfoFromContract(User contract)
    {
      return new UserInfo()
      {
        Id = contract.UserId,
        GamerTag = contract.GamerTag,
        DisplayName = contract.Name
      };
    }

    private static string UserContractToJsonString(User contract)
    {
      JsonObject jsonObject = new JsonObject(new KeyValuePair<string, JsonValue>[0]);
      jsonObject["UserId"] = (JsonValue) contract.UserId;
      jsonObject["GamerTag"] = (JsonValue) contract.GamerTag;
      jsonObject["Name"] = (JsonValue) contract.Name;
      return jsonObject.ToString();
    }

    private static User UserContractFromJson(string json)
    {
      return UserInfo.UserContractFromJson(JsonValue.Parse(json) as JsonObject);
    }

    private static User UserContractFromJson(JsonObject jsonObject)
    {
      return jsonObject != null ? new User()
      {
        UserId = jsonObject.GetRequiredStringField("UserId"),
        GamerTag = jsonObject.GetOptionalStringField("GamerTag"),
        Name = jsonObject.GetOptionalStringField("Name")
      } : throw new InvalidDataException("Invalid User JSON object");
    }
  }
}
