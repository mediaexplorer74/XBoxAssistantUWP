// *********************************************************
// Type: Microsoft.XMedia.EventLink.BinaryMessageStreamReader
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.IO;
using System.Net;
using System.Text;


namespace Microsoft.XMedia.EventLink
{
  internal class BinaryMessageStreamReader : IDisposable
  {
    private BinaryReader reader;

    public BinaryMessageStreamReader(Stream input, bool disposeStream = false)
    {
      this.reader = new BinaryReader(disposeStream ? input : (Stream) new NonClosingStream(input), Encoding.UTF8);
    }

    public int ReadInt32() => IPAddress.NetworkToHostOrder(this.reader.ReadInt32());

    public uint ReadUInt32() => (uint) IPAddress.NetworkToHostOrder((int) this.reader.ReadUInt32());

    public string ReadString(int length)
    {
      byte[] bytes = this.reader.ReadBytes(length);
      return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
    }

    public byte[] ReadBytes(int length)
    {
      byte[] numArray = this.reader.ReadBytes(length);
      if (numArray.Length < length)
        throw new InvalidDataException();
      return numArray;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this.reader == null)
        return;
      this.reader.Dispose();
      this.reader = (BinaryReader) null;
    }
  }
}
