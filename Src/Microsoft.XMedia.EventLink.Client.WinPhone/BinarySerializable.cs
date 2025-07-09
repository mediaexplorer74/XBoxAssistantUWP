// *********************************************************
// Type: Microsoft.XMedia.BinarySerializable
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System.IO;


namespace Microsoft.XMedia
{
  public static class BinarySerializable
  {
    public static T FromReader<T>(BinaryReader reader) where T : IBinarySerializable, new()
    {
      T obj = new T();
      obj.Deserialize(reader);
      return obj;
    }

    public static T FromStream<T>(Stream input) where T : IBinarySerializable, new()
    {
      return BinarySerializable.FromReader<T>((BinaryReader) new BigEndianBinaryReader(input));
    }

    public static T FromByteArray<T>(byte[] byteArray) where T : IBinarySerializable, new()
    {
      using (MemoryStream input = new MemoryStream(byteArray))
        return BinarySerializable.FromStream<T>((Stream) input);
    }
  }
}
