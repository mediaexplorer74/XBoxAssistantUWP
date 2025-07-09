// *********************************************************
// Type: Microsoft.XMedia.BinaryReaderExtensions
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.IO;


namespace Microsoft.XMedia
{
  public static class BinaryReaderExtensions
  {
    public static Guid ReadGuid(this BinaryReader reader) => new Guid(reader.ReadBytes(16));

    public static int[] ReadInts(this BinaryReader reader)
    {
      int length = reader.ReadInt32();
      int[] numArray = new int[length];
      for (int index = 0; index < length; ++index)
        numArray[index] = reader.ReadInt32();
      return numArray;
    }

    public static long[] ReadLongs(this BinaryReader reader)
    {
      int length = reader.ReadInt32();
      long[] numArray = new long[length];
      for (int index = 0; index < length; ++index)
        numArray[index] = reader.ReadInt64();
      return numArray;
    }

    public static string[] ReadStrings(this BinaryReader reader)
    {
      int length = reader.ReadInt32();
      string[] strArray = new string[length];
      for (int index = 0; index < length; ++index)
        strArray[index] = reader.ReadString();
      return strArray;
    }

    public static Guid? ReadNullableGuid(this BinaryReader reader)
    {
      return !reader.ReadBoolean() ? new Guid?() : new Guid?(reader.ReadGuid());
    }
  }
}
