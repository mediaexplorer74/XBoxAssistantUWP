// *********************************************************
// Type: Microsoft.XMedia.NonClosingStream
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.IO;


namespace Microsoft.XMedia
{
  public class NonClosingStream : Stream
  {
    private bool isClosed;

    public NonClosingStream(Stream stream) => this.BaseStream = stream;

    public Stream BaseStream { get; private set; }

    public override void Close() => this.isClosed = true;

    public override bool CanRead
    {
      get
      {
        this.VerifyDisposition();
        return this.BaseStream.CanRead;
      }
    }

    public override bool CanSeek
    {
      get
      {
        this.VerifyDisposition();
        return this.BaseStream.CanSeek;
      }
    }

    public override bool CanWrite
    {
      get
      {
        this.VerifyDisposition();
        return this.BaseStream.CanWrite;
      }
    }

    public override void Flush()
    {
      this.VerifyDisposition();
      this.BaseStream.Flush();
    }

    public override long Length
    {
      get
      {
        this.VerifyDisposition();
        return this.BaseStream.Length;
      }
    }

    public override long Position
    {
      get
      {
        this.VerifyDisposition();
        return this.BaseStream.Position;
      }
      set
      {
        this.VerifyDisposition();
        this.BaseStream.Position = value;
      }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      this.VerifyDisposition();
      return this.BaseStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      this.VerifyDisposition();
      return this.BaseStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
      this.VerifyDisposition();
      this.BaseStream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      this.VerifyDisposition();
      this.BaseStream.Write(buffer, offset, count);
    }

    public void VerifyDisposition()
    {
      if (this.isClosed)
        throw new InvalidOperationException("Stream is closed");
    }
  }
}
