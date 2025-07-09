// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.BlockIterator
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal sealed class BlockIterator : EndianStream
  {
    private int m_offset;
    private StructuredBinaryBlockId m_blockId;
    private int m_dataLength;
    private int m_fieldSize;
    private StructuredBinary m_strb;

    public BlockIterator(StructuredBinary structuredBinary)
    {
      this.Initialize(structuredBinary.Stream, false);
      this.m_strb = structuredBinary;
      this.FirstBlock();
    }

    public StructuredBinaryBlockId BlockID => this.m_blockId;

    public bool IsEnd => this.m_blockId == StructuredBinaryBlockId.Eof;

    public bool FindFirst(StructuredBinaryBlockId blockId)
    {
      this.FirstBlock();
      while (!this.IsEnd)
      {
        if (this.m_blockId == blockId)
          return true;
        if (!this.NextBlock())
          return false;
      }
      return false;
    }

    public bool Find(StructuredBinaryBlockId blockId)
    {
      while (!this.IsEnd)
      {
        if (this.m_blockId == blockId)
          return true;
        if (!this.NextBlock())
          return false;
      }
      return false;
    }

    public bool FirstBlock()
    {
      this.m_dataLength = 0;
      this.m_offset = this.m_strb.BlockStartOffset;
      this.m_blockId = StructuredBinaryBlockId.Invalid;
      return this.NextBlock();
    }

    public bool NextBlock()
    {
      if (this.m_blockId == StructuredBinaryBlockId.Eof)
        return false;
      int offset = this.m_offset + this.m_strb.RoundUpToBlockAlignment(this.m_dataLength);
      if ((long) (offset + this.m_strb.BlockHeaderSize) <= this.m_stream.Length)
      {
        this.Seek((long) offset, SeekOrigin.Begin);
        this.m_blockId = (StructuredBinaryBlockId) this.ReadMultiByte(this.m_strb.BlockIdSize);
        this.m_dataLength = (int) this.ReadMultiByte(this.m_strb.BlockSpanSize);
        this.m_fieldSize = (int) this.ReadMultiByte(this.m_strb.BlockSpanSize);
        if (this.m_dataLength < 0)
        {
          Logger.Log((Log) new DebugLog((object) this, Resources.InvalidStrbFileText));
          throw new AvatarException(Resources.InvalidStrbFileText);
        }
        if (this.m_fieldSize == 0)
        {
          Logger.Log((Log) new DebugLog((object) this, Resources.InvalidStrbFileText));
          throw new AvatarException(Resources.InvalidStrbFileText);
        }
        if (this.m_dataLength % this.m_fieldSize != 0)
        {
          Logger.Log((Log) new DebugLog((object) this, Resources.InvalidStrbFileText));
          throw new AvatarException(Resources.InvalidStrbFileText);
        }
        this.m_offset = offset + this.m_strb.BlockHeaderSize;
        this.Seek((long) this.m_offset, SeekOrigin.Begin);
        return true;
      }
      this.m_blockId = StructuredBinaryBlockId.Eof;
      this.m_dataLength = 0;
      this.m_fieldSize = 0;
      return false;
    }

    public override long Length => (long) this.m_dataLength;

    public override long Position
    {
      get => this.m_stream.Position - (long) this.m_offset;
      set => this.m_stream.Position = value + (long) this.m_offset;
    }
  }
}
