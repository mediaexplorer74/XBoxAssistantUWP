// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.SessionInfo
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using System.Collections.Generic;
using System.IO;
using System.Json;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public class SessionInfo
  {
    internal SessionInfo()
    {
      this.BaseUri = "";
      this.SessionId = "";
      this.ParticipantUserIds = new string[0];
      this.LanConnectionIPAddress = "";
      this.LanConnectionKey = "";
    }

    public string BaseUri { get; private set; }

    public string SessionId { get; private set; }

    public string[] ParticipantUserIds { get; private set; }

    public string LanConnectionIPAddress { get; private set; }

    public string LanConnectionKey { get; private set; }

    internal static SessionInfo FromJsonString(string jsonString)
    {
      return SessionInfo.SessionInfoFromContract(SessionInfo.SessionInfoContractFromJson(jsonString));
    }

    internal static string ToJsonString(SessionInfo sessionInfo)
    {
      return SessionInfo.SessionInfoContractToJsonString(SessionInfo.SessionInfoContractFromInfo(sessionInfo));
    }

    internal static IEnumerable<SessionInfo> CollectionFromJsonString(string jsonString)
    {
      JsonArray jsonArray = JsonValue.Parse(jsonString) is JsonObject jsonObject ? jsonObject.GetOptionalJsonArrayField("Sessions") : throw new InvalidDataException("Invalid json data for SessionInfo collection.");
      if (jsonArray == null)
        throw new InvalidDataException("Invalid json data for SessionInfo collection.");
      List<SessionInfo> sessionInfoList = new List<SessionInfo>();
      foreach (JsonValue jsonValue in (IEnumerable<JsonValue>) jsonArray)
      {
        if (!(jsonValue is JsonObject))
          throw new InvalidDataException("Invalid value in Activities array");
        sessionInfoList.Add(SessionInfo.SessionInfoFromContract(SessionInfo.SessionInfoContractFromJson(jsonValue as JsonObject)));
      }
      return (IEnumerable<SessionInfo>) sessionInfoList;
    }

    private static Microsoft.XMedia.Service.SessionInfo SessionInfoContractFromInfo(SessionInfo info)
    {
      return new Microsoft.XMedia.Service.SessionInfo()
      {
        BaseUri = info.BaseUri,
        LanConnectionIPAddress = info.LanConnectionIPAddress,
        LanKey = info.LanConnectionKey,
        ParticipantUserIds = info.ParticipantUserIds,
        SessionId = info.SessionId
      };
    }

    private static SessionInfo SessionInfoFromContract(Microsoft.XMedia.Service.SessionInfo contract)
    {
      return new SessionInfo()
      {
        BaseUri = contract.BaseUri,
        LanConnectionIPAddress = contract.LanConnectionIPAddress,
        LanConnectionKey = contract.LanKey,
        ParticipantUserIds = contract.ParticipantUserIds,
        SessionId = contract.SessionId
      };
    }

    private static string SessionInfoContractToJsonString(Microsoft.XMedia.Service.SessionInfo contract)
    {
      JsonObject jsonObject = new JsonObject(new KeyValuePair<string, JsonValue>[0]);
      jsonObject["SessionId"] = (JsonValue) contract.SessionId;
      jsonObject["BaseUri"] = (JsonValue) contract.BaseUri;
      jsonObject["LanKey"] = (JsonValue) contract.LanKey;
      jsonObject["LanConnectionIPAddress"] = (JsonValue) contract.LanConnectionIPAddress;
      if (contract.ParticipantUserIds != null)
      {
        JsonArray jsonArray = new JsonArray(new JsonValue[0]);
        foreach (string participantUserId in contract.ParticipantUserIds)
          jsonArray.Add((JsonValue) new JsonPrimitive(participantUserId));
        jsonObject["ParticipantUserIds"] = (JsonValue) jsonArray;
      }
      return jsonObject.ToString();
    }

    private static Microsoft.XMedia.Service.SessionInfo SessionInfoContractFromJson(string json)
    {
      return SessionInfo.SessionInfoContractFromJson(JsonValue.Parse(json) as JsonObject);
    }

    private static Microsoft.XMedia.Service.SessionInfo SessionInfoContractFromJson(
      JsonObject jsonObject)
    {
      if (jsonObject == null)
        throw new InvalidDataException("Invalid SessionInfo JSON object");
      Microsoft.XMedia.Service.SessionInfo sessionInfo = new Microsoft.XMedia.Service.SessionInfo();
      sessionInfo.SessionId = jsonObject.GetRequiredStringField("SessionId");
      sessionInfo.BaseUri = jsonObject.GetRequiredStringField("BaseUri");
      sessionInfo.LanKey = jsonObject.GetRequiredStringField("LanKey");
      sessionInfo.LanConnectionIPAddress = jsonObject.GetRequiredStringField("LanConnectionIPAddress");
      JsonArray optionalJsonArrayField = jsonObject.GetOptionalJsonArrayField("ParticipantUserIds");
      sessionInfo.ParticipantUserIds = optionalJsonArrayField != null ? new string[optionalJsonArrayField.Count] : throw new InvalidDataException("Json Object is missing ParticipantUserIds");
      for (int index = 0; index < optionalJsonArrayField.Count; ++index)
        sessionInfo.ParticipantUserIds[index] = (string) optionalJsonArrayField[index];
      return sessionInfo;
    }
  }
}
