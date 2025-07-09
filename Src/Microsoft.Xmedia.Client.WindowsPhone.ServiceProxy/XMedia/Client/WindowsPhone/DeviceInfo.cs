// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.DeviceInfo
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using Microsoft.XMedia;
using Microsoft.XMedia.Service;
using System.Collections.Generic;
using System.IO;
using System.Json;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public class DeviceInfo
  {
    internal DeviceInfo()
    {
      this.Id = "";
      this.DisplayName = "";
      this.DeviceType = DeviceType.Xbox;
    }

    internal DeviceInfo(string id, string displayName, DeviceType deviceType)
    {
      this.Id = id;
      this.DisplayName = displayName;
      this.DeviceType = deviceType;
    }

    public string Id { get; private set; }

    public string DisplayName { get; private set; }

    public DeviceType DeviceType { get; private set; }

    internal static DeviceInfo FromJsonString(string jsonString)
    {
      return DeviceInfo.DeviceInfoFromContract(DeviceInfo.DeviceContractFromJson(jsonString));
    }

    internal static string ToJsonString(DeviceInfo deviceInfo)
    {
      return DeviceInfo.DeviceContractToJsonString(DeviceInfo.DeviceContractFromInfo(deviceInfo));
    }

    internal static IEnumerable<DeviceInfo> CollectionFromJsonString(string jsonString)
    {
      JsonArray jsonArray = JsonValue.Parse(jsonString) is JsonObject jsonObject ? jsonObject.GetOptionalJsonArrayField("Devices") : throw new InvalidDataException("Invalid json data for DeviceInfo collection.");
      if (jsonArray == null)
        throw new InvalidDataException("Invalid json data for DeviceInfo collection.");
      List<DeviceInfo> deviceInfoList = new List<DeviceInfo>();
      foreach (JsonValue jsonValue in (IEnumerable<JsonValue>) jsonArray)
      {
        if (!(jsonValue is JsonObject))
          throw new InvalidDataException("Invalid value in Activities array");
        deviceInfoList.Add(DeviceInfo.DeviceInfoFromContract(DeviceInfo.DeviceContractFromJson(jsonValue as JsonObject)));
      }
      return (IEnumerable<DeviceInfo>) deviceInfoList;
    }

    private static Device DeviceContractFromInfo(DeviceInfo info)
    {
      return new Device()
      {
        DeviceId = info.Id,
        Name = info.DisplayName,
        Platform = DeviceTypeUtil.ToString(info.DeviceType)
      };
    }

    private static DeviceInfo DeviceInfoFromContract(Device device)
    {
      return new DeviceInfo()
      {
        Id = device.DeviceId,
        DisplayName = device.Name,
        DeviceType = DeviceTypeUtil.FromName(device.Platform)
      };
    }

    private static string DeviceContractToJsonString(Device contract)
    {
      JsonObject jsonObject = new JsonObject(new KeyValuePair<string, JsonValue>[0]);
      jsonObject["DeviceId"] = (JsonValue) contract.DeviceId;
      jsonObject["Name"] = (JsonValue) contract.Name;
      jsonObject["Platform"] = (JsonValue) contract.Platform;
      return jsonObject.ToString();
    }

    private static Device DeviceContractFromJson(string json)
    {
      return DeviceInfo.DeviceContractFromJson(JsonValue.Parse(json) as JsonObject);
    }

    private static Device DeviceContractFromJson(JsonObject jsonObject)
    {
      return jsonObject != null ? new Device()
      {
        DeviceId = jsonObject.GetRequiredStringField("DeviceId"),
        Name = jsonObject.GetRequiredStringField("Name"),
        Platform = jsonObject.GetRequiredStringField("Platform")
      } : throw new InvalidDataException("Invalid Content JSON object");
    }
  }
}
