// *********************************************************
// Type: Microsoft.XMedia.EventLink.BinaryMessageStreamWriter
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.IO;
using System.Net;
using System.Text;


namespace Microsoft.XMedia.EventLink
{
  internal class BinaryMessageStreamWriter : IDisposable
  {
    private BinaryWriter writer;

    public BinaryMessageStreamWriter(Stream output, bool disposeStream = false)
    {
      this.writer = new BinaryWriter(disposeStream ? output : (Stream) new NonClosingStream(output), Encoding.UTF8);
    }

    public void Write(int value)
    {
      value = IPAddress.HostToNetworkOrder(value);
      this.writer.Write(value);
    }

    public void Write(uint value)
    {
      value = (uint) IPAddress.HostToNetworkOrder((int) value);
      this.writer.Write(value);
    }

    public void Write(byte[] buffer, int index, int length)
    {
      this.writer.Write(buffer, index, length);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this.writer == null)
        return;
      this.writer.Flush();
      this.writer.Dispose();
      this.writer = (BinaryWriter) null;
    }
  }
}
