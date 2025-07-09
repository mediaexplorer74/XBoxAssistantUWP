// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.ConcatStream
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.IO;


namespace Microsoft.Phone.Marketplace.Util
{
  internal class ConcatStream : Stream
  {
    private Stream _firstStream;
    private Stream _secondStream;

    public ConcatStream(Stream firstStream, Stream secondStream)
    {
      this._firstStream = firstStream;
      this._secondStream = secondStream;
    }

    public override bool CanRead => true;

    public override bool CanSeek => throw new NotImplementedException();

    public override bool CanWrite => throw new NotImplementedException();

    public override void Flush() => throw new NotImplementedException();

    public override long Length => throw new NotImplementedException();

    public override long Position
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      if (this._firstStream != null)
      {
        int num = this._firstStream.Read(buffer, offset, count);
        if (num != 0)
          return num;
        this._firstStream = (Stream) null;
      }
      if (this._secondStream != null)
      {
        int num = this._secondStream.Read(buffer, offset, count);
        if (num != 0)
          return num;
        this._secondStream = (Stream) null;
      }
      return 0;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      throw new NotImplementedException();
    }

    public override void SetLength(long value) => throw new NotImplementedException();

    public override void Write(byte[] buffer, int offset, int count)
    {
      throw new NotImplementedException();
    }
  }
}
