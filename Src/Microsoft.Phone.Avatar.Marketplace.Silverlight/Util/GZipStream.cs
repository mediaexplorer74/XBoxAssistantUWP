// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.GZipStream
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.IO;


namespace Microsoft.Phone.Marketplace.Util
{
  public class GZipStream : Stream
  {
    private DeflateStream deflateStream;

    public GZipStream(Stream stream, CompressionMode mode)
      : this(stream, mode, false)
    {
    }

    public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen)
    {
      this.deflateStream = new DeflateStream(stream, mode, leaveOpen);
      this.SetDeflateStreamFileFormatter(mode);
    }

    public GZipStream(Stream stream, CompressionLevel compressionLevel)
      : this(stream, compressionLevel, false)
    {
    }

    public GZipStream(Stream stream, CompressionLevel compressionLevel, bool leaveOpen)
    {
      this.deflateStream = new DeflateStream(stream, compressionLevel, leaveOpen);
      this.SetDeflateStreamFileFormatter(CompressionMode.Compress);
    }

    private void SetDeflateStreamFileFormatter(CompressionMode mode)
    {
      if (mode == CompressionMode.Compress)
        this.deflateStream.SetFileFormatWriter((IFileFormatWriter) new GZipFormatter());
      else
        this.deflateStream.SetFileFormatReader((IFileFormatReader) new GZipDecoder());
    }

    public override bool CanRead => this.deflateStream != null && this.deflateStream.CanRead;

    public override bool CanWrite => this.deflateStream != null && this.deflateStream.CanWrite;

    public override bool CanSeek => this.deflateStream != null && this.deflateStream.CanSeek;

    public override long Length => throw new NotSupportedException("Not supported");

    public override long Position
    {
      get => throw new NotSupportedException("Not supported");
      set => throw new NotSupportedException("Not supported");
    }

    public override void Flush()
    {
      if (this.deflateStream == null)
        throw new ObjectDisposedException((string) null, "Object disposed: stream closed");
      this.deflateStream.Flush();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      throw new NotSupportedException("Not supported");
    }

    public override void SetLength(long value) => throw new NotSupportedException("Not supported");

    public override IAsyncResult BeginRead(
      byte[] array,
      int offset,
      int count,
      AsyncCallback asyncCallback,
      object asyncState)
    {
      if (this.deflateStream == null)
        throw new InvalidOperationException("Object disposed: stream closed");
      return this.deflateStream.BeginRead(array, offset, count, asyncCallback, asyncState);
    }

    public override int EndRead(IAsyncResult asyncResult)
    {
      return this.deflateStream != null ? this.deflateStream.EndRead(asyncResult) : throw new InvalidOperationException("Object disposed: stream closed");
    }

    public override IAsyncResult BeginWrite(
      byte[] array,
      int offset,
      int count,
      AsyncCallback asyncCallback,
      object asyncState)
    {
      if (this.deflateStream == null)
        throw new InvalidOperationException("Object disposed: stream closed");
      return this.deflateStream.BeginWrite(array, offset, count, asyncCallback, asyncState);
    }

    public override void EndWrite(IAsyncResult asyncResult)
    {
      if (this.deflateStream == null)
        throw new InvalidOperationException("Object disposed: stream closed");
      this.deflateStream.EndWrite(asyncResult);
    }

    public override int Read(byte[] array, int offset, int count)
    {
      if (this.deflateStream == null)
        throw new ObjectDisposedException((string) null, "Object disposed: stream closed");
      return this.deflateStream.Read(array, offset, count);
    }

    public override void Write(byte[] array, int offset, int count)
    {
      if (this.deflateStream == null)
        throw new ObjectDisposedException((string) null, "Object disposed: stream closed");
      this.deflateStream.Write(array, offset, count);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (disposing && this.deflateStream != null)
          this.deflateStream.Close();
        this.deflateStream = (DeflateStream) null;
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    public Stream BaseStream
    {
      get => this.deflateStream != null ? this.deflateStream.BaseStream : (Stream) null;
    }
  }
}
