// *********************************************************
// Type: Microsoft.XMedia.EventLink.DuplicatingBinaryMessageStreamReader
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.IO;
using System.Net;


namespace Microsoft.XMedia.EventLink
{
  internal class DuplicatingBinaryMessageStreamReader : IDisposable
  {
    private BinaryWriter writer;
    private MemoryStream memoryStream;
    private BinaryMessageStreamReader reader;

    public DuplicatingBinaryMessageStreamReader(
      BinaryMessageStreamReader reader,
      uint headerSignature,
      uint messageLength)
    {
      this.memoryStream = new MemoryStream((int) messageLength + 4);
      this.writer = new BinaryWriter((Stream) this.memoryStream);
      this.reader = reader;
      this.writer.Write((uint) IPAddress.HostToNetworkOrder((int) headerSignature));
      this.writer.Write((uint) IPAddress.HostToNetworkOrder((int) messageLength));
    }

    public uint ReadUInt32()
    {
      uint host = this.reader.ReadUInt32();
      this.writer.Write((uint) IPAddress.HostToNetworkOrder((int) host));
      return host;
    }

    public byte[] ReadBytes(int length)
    {
      byte[] buffer = this.reader.ReadBytes(length);
      if (buffer.Length < length)
        throw new InvalidDataException();
      this.writer.Write(buffer);
      return buffer;
    }

    public byte[] GetDuplicatedBuffer() => this.memoryStream.GetBuffer();

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      if (this.writer != null)
      {
        this.writer.Dispose();
        this.writer = (BinaryWriter) null;
      }
      if (this.memoryStream == null)
        return;
      this.memoryStream.Dispose();
      this.memoryStream = (MemoryStream) null;
    }
  }
}
