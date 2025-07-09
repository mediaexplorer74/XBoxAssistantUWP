// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.ByteStreamUnpacker`1
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class ByteStreamUnpacker<Type>
  {
    private int m_DataCount;
    private BitStream m_Stream;
    private DataUnpackerGeneric<Type> m_Unpacker;

    public ByteStreamUnpacker(Stream stream, DataUnpackerGeneric<Type> unpacker)
    {
      this.m_Unpacker = unpacker;
      this.m_Stream = new BitStream(stream);
    }

    public int Unpack(out Type[] result)
    {
      this.m_DataCount = this.m_Stream.ReadInt(32);
      this.m_Unpacker.UnpackHeader(this.m_Stream);
      result = new Type[this.m_DataCount];
      for (int index = 0; index < this.m_DataCount; ++index)
        this.m_Unpacker.UnpackData(this.m_Stream, out result[index]);
      return this.m_DataCount;
    }

    public void UnpackHeader()
    {
      this.m_DataCount = this.m_Stream.ReadInt(32);
      this.m_Unpacker.UnpackHeader(this.m_Stream);
    }

    public int UnpackData(out Type[] result)
    {
      result = new Type[this.m_DataCount];
      for (int index = 0; index < this.m_DataCount; ++index)
        this.m_Unpacker.UnpackData(this.m_Stream, out result[index]);
      return this.m_DataCount;
    }
  }
}
