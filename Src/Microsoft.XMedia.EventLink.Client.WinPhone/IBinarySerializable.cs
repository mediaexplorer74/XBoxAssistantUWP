// *********************************************************
// Type: Microsoft.XMedia.IBinarySerializable
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System.IO;


namespace Microsoft.XMedia
{
  public interface IBinarySerializable
  {
    void Serialize(BinaryWriter writer);

    void Deserialize(BinaryReader reader);
  }
}
