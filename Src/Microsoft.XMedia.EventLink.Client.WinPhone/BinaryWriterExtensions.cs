// *********************************************************
// Type: Microsoft.XMedia.BinaryWriterExtensions
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Microsoft.XMedia
{
  public static class BinaryWriterExtensions
  {
    public static void Write(this BinaryWriter writer, Guid id)
    {
      byte[] byteArray = id.ToByteArray();
      writer.Write(byteArray);
    }

    public static void WriteInts(this BinaryWriter writer, IEnumerable<int> list)
    {
      int num1 = list.Count<int>();
      writer.Write(num1);
      foreach (int num2 in list)
        writer.Write(num2);
    }

    public static void WriteLongs(this BinaryWriter writer, IEnumerable<long> list)
    {
      int num1 = list.Count<long>();
      writer.Write(num1);
      foreach (long num2 in list)
        writer.Write(num2);
    }

    public static void Write(this BinaryWriter writer, IEnumerable<string> values)
    {
      writer.Write(values.Count<string>());
      foreach (string str in values)
        writer.Write(str);
    }

    public static void Write(this BinaryWriter writer, Guid? value)
    {
      if (value.HasValue)
      {
        writer.Write(true);
        writer.Write(value.Value);
      }
      else
        writer.Write(false);
    }
  }
}
