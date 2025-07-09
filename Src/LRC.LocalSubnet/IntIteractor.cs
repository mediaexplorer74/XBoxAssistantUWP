// *********************************************************
// Type: LRC.LocalSubnet.IntIteractor
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll


namespace LRC.LocalSubnet
{
  public class IntIteractor
  {
    private const int TypeSize = 4;
    private const int LongTypeSize = 8;
    private uint offset;

    public void Reset() => this.offset = 0U;

    public uint Next()
    {
      uint offset = this.offset;
      this.offset += 4U;
      return offset;
    }

    public ulong NextLong()
    {
      ulong offset = (ulong) this.offset;
      this.offset += 8U;
      return offset;
    }
  }
}
