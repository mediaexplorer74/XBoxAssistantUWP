// *********************************************************
// Type: Microsoft.XMedia.BigEndianBinaryReader
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System.IO;
using System.Net;
using System.Text;


namespace Microsoft.XMedia
{
  internal class BigEndianBinaryReader : BinaryReader
  {
    public BigEndianBinaryReader(Stream input)
      : base(input)
    {
    }

    public BigEndianBinaryReader(Stream input, Encoding encoding)
      : base(input, encoding)
    {
    }

    public override short ReadInt16() => IPAddress.NetworkToHostOrder(base.ReadInt16());

    public override ushort ReadUInt16()
    {
      return (ushort) IPAddress.NetworkToHostOrder((short) base.ReadUInt16());
    }

    public override int ReadInt32() => IPAddress.NetworkToHostOrder(base.ReadInt32());

    public override uint ReadUInt32()
    {
      return (uint) IPAddress.NetworkToHostOrder((int) base.ReadUInt32());
    }

    public override long ReadInt64() => IPAddress.NetworkToHostOrder(base.ReadInt64());

    public override ulong ReadUInt64()
    {
      return (ulong) IPAddress.NetworkToHostOrder((long) base.ReadUInt64());
    }
  }
}
