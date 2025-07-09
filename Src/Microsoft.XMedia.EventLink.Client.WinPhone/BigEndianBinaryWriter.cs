// *********************************************************
// Type: Microsoft.XMedia.BigEndianBinaryWriter
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System.IO;
using System.Net;
using System.Text;


namespace Microsoft.XMedia
{
  public class BigEndianBinaryWriter : BinaryWriter
  {
    public BigEndianBinaryWriter(Stream output)
      : base(output)
    {
    }

    public BigEndianBinaryWriter(Stream output, Encoding encoding)
      : base(output, encoding)
    {
    }

    public override void Write(short value) => base.Write(IPAddress.HostToNetworkOrder(value));

    public override void Write(ushort value)
    {
      base.Write((ushort) IPAddress.HostToNetworkOrder((short) value));
    }

    public override void Write(int value) => base.Write(IPAddress.HostToNetworkOrder(value));

    public override void Write(uint value)
    {
      base.Write((uint) IPAddress.HostToNetworkOrder((int) value));
    }

    public override void Write(long value) => base.Write(IPAddress.HostToNetworkOrder(value));

    public override void Write(ulong value)
    {
      base.Write((ulong) IPAddress.HostToNetworkOrder((long) value));
    }
  }
}
