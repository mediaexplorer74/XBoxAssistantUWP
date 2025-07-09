// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.BitStream
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System.IO;
using System.Runtime.InteropServices;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class BitStream
  {
    private Stream m_stream;
    private int m_current;
    private int m_bitOfs;

    public BitStream(Stream stream) => this.m_stream = stream;

    public byte ReadByte(int bitSize)
    {
      if (bitSize > 8)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.InvalidWordSizeText));
        throw new AvatarException(Resources.InvalidWordSizeText);
      }
      return (byte) this.ReadUint(bitSize);
    }

    public uint ReadUint(int bitSize)
    {
      if (bitSize > 32)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.InvalidWordSizeText));
        throw new AvatarException(Resources.InvalidWordSizeText);
      }
      uint num1 = (uint) ((ulong) (1L << bitSize) - 1UL);
      uint num2 = (uint) (this.m_current >>> 8 - this.m_bitOfs);
      int bitOfs = this.m_bitOfs;
      bitSize -= this.m_bitOfs;
      while (bitSize > 0)
      {
        this.m_current = this.m_stream.ReadByte();
        if (this.m_current == -1)
        {
          Logger.Log((Log) new DebugLog((object) this, Resources.UnexpectedEofText));
          throw new AvatarException(Resources.UnexpectedEofText);
        }
        num2 += (uint) (this.m_current << bitOfs);
        bitSize -= 8;
        bitOfs += 8;
      }
      this.m_bitOfs = -bitSize;
      return num2 & num1;
    }

    public int ReadInt(int bitsize) => (int) this.ReadUint(bitsize);

    public bool ReadBool(int bitsize) => this.ReadUint(bitsize) > 0U;

    public byte[] ReadByteArray(int size)
    {
      byte[] numArray = new byte[size];
      for (int index = 0; index < size; ++index)
        numArray[index] = this.ReadByte(8);
      return numArray;
    }

    public float ReadFloat()
    {
      return new BitStream.FloatToInt()
      {
        i = this.ReadInt(32)
      }.f;
    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    private struct FloatToInt
    {
      [FieldOffset(0)]
      public float f;
      [FieldOffset(0)]
      public int i;
    }
  }
}
