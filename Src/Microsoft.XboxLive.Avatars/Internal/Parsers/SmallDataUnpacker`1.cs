// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.SmallDataUnpacker`1
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class SmallDataUnpacker<Type> : DataUnpackerGeneric<Type> where Type : new()
  {
    private int m_BitsCount;

    public SmallDataUnpacker(int byteSize) => this.m_BitsCount = 8 * byteSize;

    public override int GetHeaderBitCount() => 0;

    public override int GetPerDataBitCount() => this.m_BitsCount;

    public override void UnpackHeader(BitStream bitStream)
    {
    }

    public void UnpackData(BitStream bitStream, out byte data)
    {
      data = (byte) bitStream.ReadInt(this.m_BitsCount);
    }

    public void UnpackData(BitStream bitStream, out short data)
    {
      data = (short) bitStream.ReadInt(this.m_BitsCount);
    }

    public void UnpackData(BitStream bitStream, out int data)
    {
      data = bitStream.ReadInt(this.m_BitsCount);
    }

    public override void UnpackData(BitStream bitStream, out Type data)
    {
      throw new NotImplementedException();
    }
  }
}
