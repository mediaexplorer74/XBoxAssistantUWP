// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.PairingInviteInfo
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using Microsoft.XMedia.Service;
using System.Collections.Generic;
using System.IO;
using System.Json;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public class PairingInviteInfo
  {
    internal PairingInviteInfo()
    {
    }

    public uint Code { get; private set; }

    internal static PairingInviteInfo FromJsonString(string jsonString)
    {
      return PairingInviteInfo.PairingInviteInfoFromContract(PairingInviteInfo.PairingInviteContractFromJson(jsonString));
    }

    internal static string ToJsonString(PairingInviteInfo info)
    {
      return PairingInviteInfo.PairingInviteContractToJsonString(PairingInviteInfo.PairingInviteContractFromInfo(info));
    }

    private static PairingInvite PairingInviteContractFromInfo(PairingInviteInfo info)
    {
      return new PairingInvite() { InviteCode = info.Code };
    }

    private static PairingInviteInfo PairingInviteInfoFromContract(PairingInvite contract)
    {
      return new PairingInviteInfo()
      {
        Code = contract.InviteCode
      };
    }

    private static string PairingInviteContractToJsonString(PairingInvite contract)
    {
      JsonObject jsonObject = new JsonObject(new KeyValuePair<string, JsonValue>[0]);
      jsonObject["InviteCode"] = (JsonValue) contract.InviteCode;
      return jsonObject.ToString();
    }

    private static PairingInvite PairingInviteContractFromJson(string json)
    {
      return PairingInviteInfo.PairingInviteContractFromJson((string) (JsonValue) (JsonValue.Parse(json) as JsonObject));
    }

    private static PairingInvite UserContractFromJson(JsonObject jsonObject)
    {
      if (jsonObject == null)
        throw new InvalidDataException("Invalid User JSON object");
      PairingInvite pairingInvite = new PairingInvite();
      JsonValue jsonValue;
      if (!jsonObject.TryGetValue("inviteCode", out jsonValue))
        throw new InvalidDataException("PairingInvite is missing InviteCode");
      pairingInvite.InviteCode = (uint) jsonValue;
      return pairingInvite;
    }
  }
}
