// *********************************************************
// Type: Microsoft.XMedia.EventLink.ClientConnectedMessage
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Text;


namespace Microsoft.XMedia.EventLink
{
  internal class ClientConnectedMessage : IBinarySerializable
  {
    private const int MaxDisplayNameCount = 4;
    private const int MaxDisplayNameBytes = 15;
    private const int DisplayNameByteLength = 64;
    private const string DisplayNameSeparator = ",";
    private const char DisplayNameSeparatorChar = ',';

    public static ClientConnectedMessage FromDeviceInfo(
      uint clientId,
      DeviceType deviceType,
      IEnumerable<string> userDisplayNames)
    {
      return new ClientConnectedMessage()
      {
        ClientId = clientId,
        ClientDeviceType = deviceType,
        UserDisplayNames = (IList<string>) new List<string>(userDisplayNames)
      };
    }

    public uint ClientId { get; private set; }

    public DeviceType ClientDeviceType { get; private set; }

    public IList<string> UserDisplayNames { get; private set; }

    public string ToJson()
    {
      return new JsonObject(new KeyValuePair<string, JsonValue>[0])
      {
        {
          "ClientId",
          (JsonValue) this.ClientId
        },
        {
          "DeviceType",
          (JsonValue) this.ClientDeviceType.ToString()
        },
        {
          "UserDisplayNames",
          (JsonValue) new JsonArray(this.UserDisplayNames.Select<string, JsonValue>((Func<string, JsonValue>) (n => (JsonValue) n)))
        }
      }.ToString();
    }

    public static ClientConnectedMessage FromJson(string json)
    {
      JsonObject jsonObject = (JsonObject) JsonValue.Parse(json);
      JsonValue jsonValue1 = jsonObject["ClientId"];
      JsonValue jsonValue2 = jsonObject["DeviceType"];
      JsonArray source = (JsonArray) jsonObject["UserDisplayNames"];
      return new ClientConnectedMessage()
      {
        ClientId = (uint) jsonValue1,
        ClientDeviceType = (DeviceType) Enum.Parse(typeof (DeviceType), (string) jsonValue2, true),
        UserDisplayNames = (IList<string>) source.Select<JsonValue, string>((Func<JsonValue, string>) (v => (string) v)).ToList<string>()
      };
    }

    public void Serialize(BinaryWriter writer)
    {
      writer.Write(this.ClientId);
      writer.Write((uint) this.ClientDeviceType);
      byte[] numArray = new byte[64];
      byte[] bytes = Encoding.UTF8.GetBytes(string.Join(",", (IEnumerable<string>) this.UserDisplayNames));
      Buffer.BlockCopy((Array) bytes, 0, (Array) numArray, 0, Math.Min(bytes.Length, 64));
      writer.Write(numArray);
    }

    public void Deserialize(BinaryReader reader)
    {
      this.ClientId = reader.ReadUInt32();
      this.ClientDeviceType = (DeviceType) reader.ReadUInt32();
      byte[] bytes = reader.ReadBytes(64);
      this.UserDisplayNames = (IList<string>) new List<string>(((IEnumerable<string>) Encoding.UTF8.GetString(bytes, 0, bytes.Length).Split(',')).Select<string, string>((Func<string, string>) (s => s.Trim(new char[1]))));
    }
  }
}
