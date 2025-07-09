// *********************************************************
// Type: Microsoft.XMedia.EventLink.ClientDisconnectedMessage
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System.Collections.Generic;
using System.IO;
using System.Json;


namespace Microsoft.XMedia.EventLink
{
  internal class ClientDisconnectedMessage : IBinarySerializable
  {
    public uint ClientId { get; set; }

    public static ClientDisconnectedMessage FromJson(string json)
    {
      JsonObject jsonObject = (JsonObject) JsonValue.Parse(json);
      return new ClientDisconnectedMessage()
      {
        ClientId = (uint) jsonObject["ClientId"]
      };
    }

    public string ToJson()
    {
      return new JsonObject(new KeyValuePair<string, JsonValue>[0])
      {
        {
          "ClientId",
          (JsonValue) this.ClientId
        }
      }.ToString();
    }

    public void Serialize(BinaryWriter writer) => writer.Write(this.ClientId);

    public void Deserialize(BinaryReader reader) => this.ClientId = reader.ReadUInt32();
  }
}
