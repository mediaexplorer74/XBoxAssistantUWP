// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.InterleavedDataUnpacker`2
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class InterleavedDataUnpacker<Type, Unpacker> : DataUnpackerGeneric<Type[]> where Unpacker : DataUnpackerGeneric<Type>, new()
  {
    private Unpacker[] m_Unpackers;
    private int m_PerFrameBitsCount;
    private int m_HeaderMaxItems;

    public Unpacker[] Unpackers => this.m_Unpackers;

    public InterleavedDataUnpacker(int headerMaxItems) => this.m_HeaderMaxItems = headerMaxItems;

    public override int GetHeaderBitCount()
    {
      return 32 + this.m_HeaderMaxItems * this.m_Unpackers[0].GetHeaderBitCount();
    }

    public override int GetPerDataBitCount() => this.m_PerFrameBitsCount;

    public override void UnpackHeader(BitStream bitStream)
    {
      int length = bitStream.ReadInt(32);
      this.m_Unpackers = new Unpacker[length];
      int index1 = length;
      while (--index1 >= 0)
      {
        this.m_Unpackers[index1] = new Unpacker();
        this.m_Unpackers[index1].UnpackHeader(bitStream);
      }
      for (int index2 = length; index2 < this.m_HeaderMaxItems; ++index2)
        new Unpacker().UnpackHeader(bitStream);
      int num = 0;
      for (int index3 = 0; index3 < length; ++index3)
        num += this.m_Unpackers[index3].GetPerDataBitCount();
      this.m_PerFrameBitsCount = num;
    }

    public override void UnpackData(BitStream bitStream, out Type[] data)
    {
      int length = this.m_Unpackers.Length;
      data = new Type[length];
      for (int index = 0; index < length; ++index)
        this.m_Unpackers[index].UnpackData(bitStream, out data[index]);
    }
  }
}
