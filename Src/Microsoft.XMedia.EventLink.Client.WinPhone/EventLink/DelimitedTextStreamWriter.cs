// *********************************************************
// Type: Microsoft.XMedia.EventLink.DelimitedTextStreamWriter
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.IO;


namespace Microsoft.XMedia.EventLink
{
  internal class DelimitedTextStreamWriter : IDisposable
  {
    private TextWriter writer;
    private bool disposed;
    private Stream baseStream;

    public DelimitedTextStreamWriter(Stream stream, bool disposeStream = false)
    {
      this.baseStream = disposeStream ? stream : (Stream) new NonClosingStream(stream);
      this.writer = (TextWriter) new StreamWriter(this.baseStream);
    }

    public void Write(int value)
    {
      this.writer.Write(value);
      this.writer.Write('|');
    }

    public void Write(uint value)
    {
      this.writer.Write(value);
      this.writer.Write('|');
    }

    public void Write(string value)
    {
      if (string.IsNullOrEmpty(value))
      {
        this.Write(0);
      }
      else
      {
        this.Write(value.Length);
        this.writer.Write(value);
      }
    }

    public void Flush() => this.writer.Flush();

    public void Dispose()
    {
      if (this.disposed)
        return;
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
      this.disposed = true;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.writer.Flush();
      this.writer.Dispose();
    }
  }
}
