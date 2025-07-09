// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.StructuredBinary
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class StructuredBinary : IDisposable
  {
    private EndianStream m_stream;
    private bool m_littleEndian;
    private byte m_blockIdSize;
    private byte m_blockSpanSize;
    private byte m_blockAlignment;
    private int m_blockStartOffset;
    private Guid m_namespace;
    private BlockIterator m_iterator;

    public Guid Namespace => this.m_namespace;

    public BlockIterator Iterator => this.m_iterator;

    public int BlockIdSize => (int) this.m_blockIdSize;

    public int BlockStartOffset => this.m_blockStartOffset;

    public int BlockSpanSize => (int) this.m_blockSpanSize;

    public int BlockHeaderSize
    {
      get
      {
        return this.RoundUpToBlockAlignment(this.BlockIdSize + this.BlockSpanSize + this.BlockSpanSize);
      }
    }

    public Stream Stream => (Stream) this.m_stream;

    public bool Open(Stream stream)
    {
      this.m_stream = new EndianStream(stream);
      if (!this.ReadHeader())
        return false;
      this.m_iterator = new BlockIterator(this);
      return true;
    }

    internal int RoundUpToBlockAlignment(int valueToRound)
    {
      if (((int) this.m_blockAlignment & (int) this.m_blockAlignment - 1) != 0 || this.m_blockAlignment == (byte) 0)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.InvalidStrbFileText));
        throw new AvatarException(Resources.InvalidStrbFileText);
      }
      int blockAlignment = (int) this.m_blockAlignment;
      return valueToRound + blockAlignment - 1 & ~(blockAlignment - 1);
    }

    private bool ReadHeader()
    {
      byte[] buffer = new byte[4];
      this.m_stream.Read(buffer, 0, 4);
      uint num1 = 0;
      if (buffer[0] == (byte) 89 && buffer[1] == (byte) 84 && buffer[2] == (byte) 71 && buffer[3] == (byte) 82)
      {
        this.m_stream.LittleEndian = true;
        if (!this.VerifyXSigSignature(this.m_stream, out uint _) || !this.SearchSTRBHeader(this.m_stream, out uint _))
          return false;
        buffer = new byte[4]
        {
          (byte) 83,
          (byte) 84,
          (byte) 82,
          (byte) 66
        };
        num1 = (uint) this.m_stream.Position - 4U;
      }
      byte num2 = (byte) this.m_stream.ReadByte();
      if (num2 >= (byte) 2)
        return false;
      this.m_littleEndian = this.m_stream.ReadByte() > 0;
      this.m_stream.LittleEndian = this.m_littleEndian;
      if (buffer[0] == (byte) 83 && buffer[1] == (byte) 84 && buffer[2] == (byte) 82 && buffer[3] == (byte) 66)
      {
        byte[] numArray = new byte[8];
        int a = this.m_stream.ReadInt();
        short b = this.m_stream.ReadShort();
        short c = this.m_stream.ReadShort();
        this.m_stream.Read(numArray, 0, 8);
        this.m_namespace = new Guid(a, b, c, numArray);
        this.m_blockIdSize = (byte) this.m_stream.ReadByte();
        this.m_blockSpanSize = (byte) this.m_stream.ReadByte();
        int num3 = (int) this.m_stream.ReadUShort();
        this.m_blockAlignment = num2 != (byte) 1 ? (byte) 1 : (byte) this.m_stream.ReadByte();
        int valueToRound = num2 != (byte) 0 ? 30 : 26;
        this.m_blockStartOffset = (int) num1 + this.RoundUpToBlockAlignment(valueToRound);
        return num2 < (byte) 2;
      }
      this.m_blockStartOffset = 0;
      return false;
    }

    protected bool SearchSTRBHeader(EndianStream stream, out uint strbOffset)
    {
      int index = 0;
      strbOffset = 0U;
      char[] chArray = new char[4]{ 'S', 'T', 'R', 'B' };
      while (index < 4)
      {
        int num = stream.ReadByte();
        if (num < 0)
          return false;
        if ((int) (byte) num == (int) chArray[index])
        {
          ++index;
        }
        else
        {
          index = 0;
          if ((int) (byte) num == (int) chArray[index])
            ++index;
        }
        ++strbOffset;
      }
      return true;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (this.m_stream != null)
          this.m_stream.Dispose();
        if (this.m_iterator != null)
          this.m_iterator.Dispose();
      }
      this.m_stream = (EndianStream) null;
      this.m_iterator = (BlockIterator) null;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual bool VerifyXSigSignature(EndianStream stream, out uint signatureBlockSize)
    {
      int num1 = (int) stream.ReadUInt();
      uint num2 = stream.ReadUInt();
      stream.Seek((long) (num2 - 12U), SeekOrigin.Current);
      signatureBlockSize = num2;
      return true;
    }
  }
}
