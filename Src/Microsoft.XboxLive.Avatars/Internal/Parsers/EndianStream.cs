// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.EndianStream
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class EndianStream : Stream
  {
    protected internal Stream m_stream;
    protected internal bool m_needRotate;
    protected internal bool m_streamEndian;

    public EndianStream()
    {
    }

    public EndianStream(Stream inputStream)
    {
      this.m_stream = inputStream;
      this.m_needRotate = EndianStream.IsMachineLittleEndian();
    }

    public void Initialize(Stream inputStream, bool inLittleEndian)
    {
      this.m_stream = inputStream;
      this.m_needRotate = EndianStream.IsMachineLittleEndian() != inLittleEndian;
      this.m_streamEndian = inLittleEndian;
    }

    internal virtual EndianSwapper Read16b()
    {
      EndianSwapper endianSwapper = new EndianSwapper();
      if (this.m_needRotate)
      {
        endianSwapper.llh = (byte) this.m_stream.ReadByte();
        endianSwapper.lll = (byte) this.m_stream.ReadByte();
      }
      else
      {
        endianSwapper.lll = (byte) this.m_stream.ReadByte();
        endianSwapper.llh = (byte) this.m_stream.ReadByte();
      }
      return endianSwapper;
    }

    internal virtual EndianSwapper Read32b()
    {
      EndianSwapper endianSwapper = new EndianSwapper();
      if (this.m_needRotate)
      {
        endianSwapper.lhh = (byte) this.m_stream.ReadByte();
        endianSwapper.lhl = (byte) this.m_stream.ReadByte();
        endianSwapper.llh = (byte) this.m_stream.ReadByte();
        endianSwapper.lll = (byte) this.m_stream.ReadByte();
      }
      else
      {
        endianSwapper.lll = (byte) this.m_stream.ReadByte();
        endianSwapper.llh = (byte) this.m_stream.ReadByte();
        endianSwapper.lhl = (byte) this.m_stream.ReadByte();
        endianSwapper.lhh = (byte) this.m_stream.ReadByte();
      }
      return endianSwapper;
    }

    internal virtual EndianSwapper Read64b()
    {
      EndianSwapper endianSwapper = new EndianSwapper();
      if (this.m_needRotate)
      {
        endianSwapper.hhh = (byte) this.m_stream.ReadByte();
        endianSwapper.hhl = (byte) this.m_stream.ReadByte();
        endianSwapper.hlh = (byte) this.m_stream.ReadByte();
        endianSwapper.hll = (byte) this.m_stream.ReadByte();
        endianSwapper.lhh = (byte) this.m_stream.ReadByte();
        endianSwapper.lhl = (byte) this.m_stream.ReadByte();
        endianSwapper.llh = (byte) this.m_stream.ReadByte();
        endianSwapper.lll = (byte) this.m_stream.ReadByte();
      }
      else
      {
        endianSwapper.lll = (byte) this.m_stream.ReadByte();
        endianSwapper.llh = (byte) this.m_stream.ReadByte();
        endianSwapper.lhl = (byte) this.m_stream.ReadByte();
        endianSwapper.lhh = (byte) this.m_stream.ReadByte();
        endianSwapper.hll = (byte) this.m_stream.ReadByte();
        endianSwapper.hlh = (byte) this.m_stream.ReadByte();
        endianSwapper.hhl = (byte) this.m_stream.ReadByte();
        endianSwapper.hhh = (byte) this.m_stream.ReadByte();
      }
      return endianSwapper;
    }

    public override int ReadByte() => this.m_stream.ReadByte();

    public ushort ReadUShort() => this.Read16b().word;

    public short ReadShort() => this.Read16b().aShort;

    public int ReadInt() => this.Read32b().aInt;

    public float ReadFloat() => this.Read32b().aFloat;

    public uint ReadUInt() => this.Read32b().aUint;

    public long ReadLong() => this.Read64b().aLong;

    public ulong ReadULong() => this.Read64b().aLonglong;

    public double ReadDouble() => this.Read64b().aDouble;

    public Guid ReadGuid()
    {
      byte[] numArray = new byte[8];
      int a = this.ReadInt();
      short b = this.ReadShort();
      short c = this.ReadShort();
      this.m_stream.Read(numArray, 0, 8);
      return new Guid(a, b, c, numArray);
    }

    public long ReadMultiByte(int size)
    {
      switch (size)
      {
        case 1:
          return (long) this.ReadByte();
        case 2:
          return (long) this.ReadShort();
        case 4:
          return (long) this.ReadInt();
        case 8:
          return this.ReadLong();
        default:
          Logger.Log((Log) new DebugLog((object) this, Resources.InvalidWordSizeText));
          throw new AvatarException(Resources.InvalidWordSizeText);
      }
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      this.m_stream.Write(buffer, offset, count);
    }

    public override void WriteByte(byte value) => this.m_stream.WriteByte(value);

    internal virtual void Write16b(EndianSwapper value)
    {
      if (this.m_needRotate)
      {
        this.m_stream.WriteByte(value.llh);
        this.m_stream.WriteByte(value.lll);
      }
      else
      {
        this.m_stream.WriteByte(value.lll);
        this.m_stream.WriteByte(value.llh);
      }
    }

    internal virtual void Write32b(EndianSwapper value)
    {
      if (this.m_needRotate)
      {
        this.m_stream.WriteByte(value.lhh);
        this.m_stream.WriteByte(value.lhl);
        this.m_stream.WriteByte(value.llh);
        this.m_stream.WriteByte(value.lll);
      }
      else
      {
        this.m_stream.WriteByte(value.lll);
        this.m_stream.WriteByte(value.llh);
        this.m_stream.WriteByte(value.lhl);
        this.m_stream.WriteByte(value.lhh);
      }
    }

    internal virtual void Write64b(EndianSwapper value)
    {
      if (this.m_needRotate)
      {
        this.m_stream.WriteByte(value.hhh);
        this.m_stream.WriteByte(value.hhl);
        this.m_stream.WriteByte(value.hlh);
        this.m_stream.WriteByte(value.hll);
        this.m_stream.WriteByte(value.lhh);
        this.m_stream.WriteByte(value.lhl);
        this.m_stream.WriteByte(value.llh);
        this.m_stream.WriteByte(value.lll);
      }
      else
      {
        this.m_stream.WriteByte(value.lll);
        this.m_stream.WriteByte(value.llh);
        this.m_stream.WriteByte(value.lhl);
        this.m_stream.WriteByte(value.lhh);
        this.m_stream.WriteByte(value.hll);
        this.m_stream.WriteByte(value.hlh);
        this.m_stream.WriteByte(value.hhl);
        this.m_stream.WriteByte(value.hhh);
      }
    }

    public void WriteArray(byte[] buffer, int offset, int count)
    {
      if (this.m_needRotate)
      {
        for (int index = 1; index <= count; ++index)
          this.WriteByte(buffer[offset + count - index]);
      }
      else
      {
        for (int index = 0; index < count; ++index)
          this.WriteByte(buffer[offset + index]);
      }
    }

    public void WriteFloat(float value)
    {
      this.Write32b(new EndianSwapper() { aFloat = value });
    }

    public void WriteUint(uint value)
    {
      this.Write32b(new EndianSwapper() { aUint = value });
    }

    public void WriteShort(short value)
    {
      this.Write16b(new EndianSwapper() { aShort = value });
    }

    public void WriteLong(long value)
    {
      this.Write64b(new EndianSwapper() { aLong = value });
    }

    public void WriteGuid(Guid value)
    {
      byte[] byteArray = value.ToByteArray();
      this.WriteArray(byteArray, 0, 4);
      this.WriteArray(byteArray, 4, 2);
      this.WriteArray(byteArray, 6, 2);
      for (int index = 8; index < 16; ++index)
        this.WriteByte(byteArray[index]);
    }

    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      return this.m_stream.BeginRead(buffer, offset, count, callback, state);
    }

    public override int EndRead(IAsyncResult asyncResult) => this.m_stream.EndRead(asyncResult);

    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      return this.m_stream.BeginWrite(buffer, offset, count, callback, state);
    }

    public override void EndWrite(IAsyncResult asyncResult) => this.m_stream.EndWrite(asyncResult);

    public override void Close()
    {
      this.Flush();
      this.m_stream.Close();
    }

    public override void Flush() => this.m_stream.Flush();

    public override int Read(byte[] buffer, int offset, int count)
    {
      return this.m_stream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin) => this.m_stream.Seek(offset, origin);

    public override void SetLength(long value) => this.m_stream.SetLength(value);

    public override bool CanRead => this.m_stream.CanRead;

    public override bool CanSeek => this.m_stream.CanSeek;

    public override bool CanTimeout => this.m_stream.CanTimeout;

    public override bool CanWrite => this.m_stream.CanWrite;

    public override long Length => this.m_stream.Length;

    public override long Position
    {
      get => this.m_stream.Position;
      set => this.m_stream.Position = value;
    }

    public override int ReadTimeout
    {
      get => this.m_stream.ReadTimeout;
      set => this.m_stream.ReadTimeout = value;
    }

    public override int WriteTimeout
    {
      get => this.m_stream.WriteTimeout;
      set => this.m_stream.WriteTimeout = value;
    }

    public bool LittleEndian
    {
      get => this.m_streamEndian;
      set
      {
        bool flag = EndianStream.IsMachineLittleEndian();
        this.m_streamEndian = value;
        this.m_needRotate = flag != this.m_streamEndian;
      }
    }

    public static bool IsMachineLittleEndian() => BitConverter.IsLittleEndian;

    internal static EndianSwapper Read16b(Stream stream, bool streamEndianLittle)
    {
      EndianSwapper endianSwapper = new EndianSwapper();
      if (streamEndianLittle ^ BitConverter.IsLittleEndian)
      {
        endianSwapper.llh = (byte) stream.ReadByte();
        endianSwapper.lll = (byte) stream.ReadByte();
      }
      else
      {
        endianSwapper.lll = (byte) stream.ReadByte();
        endianSwapper.llh = (byte) stream.ReadByte();
      }
      return endianSwapper;
    }

    internal static EndianSwapper Read32b(Stream stream, bool streamEndianLittle)
    {
      EndianSwapper endianSwapper = new EndianSwapper();
      if (streamEndianLittle ^ BitConverter.IsLittleEndian)
      {
        endianSwapper.lhh = (byte) stream.ReadByte();
        endianSwapper.lhl = (byte) stream.ReadByte();
        endianSwapper.llh = (byte) stream.ReadByte();
        endianSwapper.lll = (byte) stream.ReadByte();
      }
      else
      {
        endianSwapper.lll = (byte) stream.ReadByte();
        endianSwapper.llh = (byte) stream.ReadByte();
        endianSwapper.lhl = (byte) stream.ReadByte();
        endianSwapper.lhh = (byte) stream.ReadByte();
      }
      return endianSwapper;
    }

    internal static EndianSwapper Read64b(Stream stream, bool streamEndianLittle)
    {
      EndianSwapper endianSwapper = new EndianSwapper();
      if (streamEndianLittle ^ BitConverter.IsLittleEndian)
      {
        endianSwapper.hhh = (byte) stream.ReadByte();
        endianSwapper.hhl = (byte) stream.ReadByte();
        endianSwapper.hlh = (byte) stream.ReadByte();
        endianSwapper.hll = (byte) stream.ReadByte();
        endianSwapper.lhh = (byte) stream.ReadByte();
        endianSwapper.lhl = (byte) stream.ReadByte();
        endianSwapper.llh = (byte) stream.ReadByte();
        endianSwapper.lll = (byte) stream.ReadByte();
      }
      else
      {
        endianSwapper.lll = (byte) stream.ReadByte();
        endianSwapper.llh = (byte) stream.ReadByte();
        endianSwapper.lhl = (byte) stream.ReadByte();
        endianSwapper.lhh = (byte) stream.ReadByte();
        endianSwapper.hll = (byte) stream.ReadByte();
        endianSwapper.hlh = (byte) stream.ReadByte();
        endianSwapper.hhl = (byte) stream.ReadByte();
        endianSwapper.hhh = (byte) stream.ReadByte();
      }
      return endianSwapper;
    }

    public static int ReadByte(Stream stream) => stream.ReadByte();

    public static ushort ReadUShort(Stream stream, bool streamEndianLittle)
    {
      return EndianStream.Read16b(stream, streamEndianLittle).word;
    }

    public static short ReadShort(Stream stream, bool streamEndianLittle)
    {
      return EndianStream.Read16b(stream, streamEndianLittle).aShort;
    }

    public static int ReadInt(Stream stream, bool streamEndianLittle)
    {
      return EndianStream.Read32b(stream, streamEndianLittle).aInt;
    }

    public static float ReadFloat(Stream stream, bool streamEndianLittle)
    {
      return EndianStream.Read32b(stream, streamEndianLittle).aFloat;
    }

    public static uint ReadUInt(Stream stream, bool streamEndianLittle)
    {
      return EndianStream.Read32b(stream, streamEndianLittle).aUint;
    }

    public static long ReadLong(Stream stream, bool streamEndianLittle)
    {
      return EndianStream.Read64b(stream, streamEndianLittle).aLong;
    }

    public static ulong ReadULong(Stream stream, bool streamEndianLittle)
    {
      return EndianStream.Read64b(stream, streamEndianLittle).aLonglong;
    }

    public static double ReadDouble(Stream stream, bool streamEndianLittle)
    {
      return EndianStream.Read64b(stream, streamEndianLittle).aDouble;
    }

    public static long ReadMultiByte(Stream stream, bool streamEndianLittle, int size)
    {
      switch (size)
      {
        case 1:
          return (long) EndianStream.ReadByte(stream);
        case 2:
          return (long) EndianStream.ReadShort(stream, streamEndianLittle);
        case 4:
          return (long) EndianStream.ReadInt(stream, streamEndianLittle);
        case 8:
          return EndianStream.ReadLong(stream, streamEndianLittle);
        default:
          return 0;
      }
    }
  }
}
