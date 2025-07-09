// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.DecompressStream
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class DecompressStream : EndianStream
  {
    private const int MAX_CACHED_WINDOW_LENGTH = 32768;
    internal int m_size;
    internal int m_winStart;
    internal int m_winLength;
    internal int m_streamSize;
    internal int m_offs;
    private MemoryStream m_block;
    private LZXDeflate m_deflate;

    public DecompressStream()
    {
    }

    public DecompressStream(Stream instream, int maxsize)
    {
      this.m_streamSize = maxsize;
      if (this.m_streamSize <= 0)
        throw new ArgumentException("input stream is empty");
      this.Initialize(instream, true);
      this.m_deflate = new LZXDeflate(32768);
    }

    internal void ReadBlock()
    {
      if (this.m_stream.Position >= (long) this.m_streamSize)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.UnexpectedEofText));
        throw new AvatarException(Resources.UnexpectedEofText);
      }
      this.m_size = EndianStream.ReadInt(this.m_stream, true);
      this.m_winStart = EndianStream.ReadInt(this.m_stream, true);
      this.m_winLength = EndianStream.ReadInt(this.m_stream, true);
      if (this.m_winStart < 0)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.InvalidStrbFileText));
        throw new AvatarException(Resources.InvalidStrbFileText);
      }
      if ((uint) this.m_size > 40000U)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.InvalidStrbFileText));
        throw new AvatarException(Resources.InvalidStrbFileText);
      }
      if ((uint) this.m_winLength > 32768U)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.InvalidStrbFileText));
        throw new AvatarException(Resources.InvalidStrbFileText);
      }
      byte[] src = new byte[this.m_size];
      this.m_stream.Read(src, 0, this.m_size);
      byte[] tg = new byte[this.m_winLength];
      try
      {
        if (this.m_deflate.Decompress(ref src, ref tg) >= 0)
        {
          if (this.m_deflate.Reset())
            goto label_13;
        }
        Logger.Log((Log) new DebugLog((object) this, Resources.DecompressionFailedText));
        throw new AvatarException(Resources.DecompressionFailedText);
      }
      catch (IndexOutOfRangeException ex)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.DecompressionFailedText));
        throw new AvatarException(Resources.DecompressionFailedText);
      }
label_13:
      this.m_block = new MemoryStream(tg);
    }

    internal override EndianSwapper Read16b()
    {
      EndianSwapper endianSwapper = new EndianSwapper();
      if (this.m_needRotate)
      {
        endianSwapper.llh = (byte) this.ReadByte();
        endianSwapper.lll = (byte) this.ReadByte();
      }
      else
      {
        endianSwapper.lll = (byte) this.ReadByte();
        endianSwapper.llh = (byte) this.ReadByte();
      }
      return endianSwapper;
    }

    internal override EndianSwapper Read32b()
    {
      EndianSwapper endianSwapper = new EndianSwapper();
      if (this.m_needRotate)
      {
        endianSwapper.lhh = (byte) this.ReadByte();
        endianSwapper.lhl = (byte) this.ReadByte();
        endianSwapper.llh = (byte) this.ReadByte();
        endianSwapper.lll = (byte) this.ReadByte();
      }
      else
      {
        endianSwapper.lll = (byte) this.ReadByte();
        endianSwapper.llh = (byte) this.ReadByte();
        endianSwapper.lhl = (byte) this.ReadByte();
        endianSwapper.lhh = (byte) this.ReadByte();
      }
      return endianSwapper;
    }

    internal override EndianSwapper Read64b()
    {
      EndianSwapper endianSwapper = new EndianSwapper();
      if (this.m_needRotate)
      {
        endianSwapper.hhh = (byte) this.ReadByte();
        endianSwapper.hhl = (byte) this.ReadByte();
        endianSwapper.hlh = (byte) this.ReadByte();
        endianSwapper.hll = (byte) this.ReadByte();
        endianSwapper.lhh = (byte) this.ReadByte();
        endianSwapper.lhl = (byte) this.ReadByte();
        endianSwapper.llh = (byte) this.ReadByte();
        endianSwapper.lll = (byte) this.ReadByte();
      }
      else
      {
        endianSwapper.lll = (byte) this.ReadByte();
        endianSwapper.llh = (byte) this.ReadByte();
        endianSwapper.lhl = (byte) this.ReadByte();
        endianSwapper.lhh = (byte) this.ReadByte();
        endianSwapper.hll = (byte) this.ReadByte();
        endianSwapper.hlh = (byte) this.ReadByte();
        endianSwapper.hhl = (byte) this.ReadByte();
        endianSwapper.hhh = (byte) this.ReadByte();
      }
      return endianSwapper;
    }

    public override int ReadByte()
    {
      if (this.m_offs < this.m_winStart)
      {
        ++this.m_offs;
        return 0;
      }
      while (this.m_winStart + this.m_winLength <= this.m_offs)
        this.ReadBlock();
      ++this.m_offs;
      return this.m_block.ReadByte();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      int offset1 = 0;
      for (; count > 0 && this.m_winStart > this.m_offs; ++this.m_offs)
      {
        buffer[offset1] = (byte) 0;
        ++offset1;
        --count;
      }
      if (count == 0)
        return offset1;
      while (this.m_winStart + this.m_winLength <= this.m_offs)
        this.ReadBlock();
      while (true)
      {
        int count1 = Math.Min(this.m_winLength - (this.m_offs - this.m_winStart), count);
        int num = this.m_block.Read(buffer, offset1, count1);
        offset1 += num;
        this.m_offs += num;
        count -= num;
        if (num == count1 && count != 0)
          this.ReadBlock();
        else
          break;
      }
      return offset1;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      this.m_offs = (int) offset;
      if (this.m_offs < this.m_winStart)
        this.m_stream.Seek(0L, SeekOrigin.Begin);
      while (this.m_offs > this.m_winStart + this.m_winLength)
        this.ReadBlock();
      return (long) this.m_offs;
    }
  }
}
