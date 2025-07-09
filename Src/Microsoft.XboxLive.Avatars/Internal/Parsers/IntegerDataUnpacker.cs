// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.IntegerDataUnpacker
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class IntegerDataUnpacker : DataUnpackerGeneric<int>
  {
    private int m_MinValue;
    private int m_BitCount;
    private int m_ItemBitSize;

    public IntegerDataUnpacker() => this.m_ItemBitSize = 32;

    public IntegerDataUnpacker(int headBitSize) => this.m_ItemBitSize = headBitSize;

    public override int GetHeaderBitCount() => 2 * this.m_ItemBitSize;

    public override int GetPerDataBitCount() => this.m_BitCount;

    public override void UnpackHeader(BitStream bitStream)
    {
      this.m_MinValue = bitStream.ReadInt(this.m_ItemBitSize);
      this.m_BitCount = bitStream.ReadInt(this.m_ItemBitSize);
      if (this.m_BitCount > this.m_ItemBitSize)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.CorruptedStreamText));
        throw new AvatarException(Resources.CorruptedStreamText);
      }
    }

    public override void UnpackData(BitStream bitStream, out int data)
    {
      data = bitStream.ReadInt(this.m_BitCount) + this.m_MinValue;
    }
  }
}
