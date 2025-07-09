// *********************************************************
// Type: Microsoft.XMedia.BinarySerializableExtensions
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System.IO;


namespace Microsoft.XMedia
{
  public static class BinarySerializableExtensions
  {
    public static void Write(this BinaryWriter writer, IBinarySerializable serializable)
    {
      serializable.Serialize(writer);
    }

    public static byte[] ToByteArray(this IBinarySerializable serializable)
    {
      using (MemoryStream output = new MemoryStream())
      {
        using (BinaryWriter writer = (BinaryWriter) new BigEndianBinaryWriter((Stream) output))
        {
          serializable.Serialize(writer);
          return output.ToArray();
        }
      }
    }
  }
}
