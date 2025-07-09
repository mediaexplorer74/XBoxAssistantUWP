// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.EndianSwapper
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System.Runtime.InteropServices;


namespace Microsoft.XboxLive.Avatars.Internal
{
  [StructLayout(LayoutKind.Explicit, Size = 8)]
  internal struct EndianSwapper
  {
    [FieldOffset(0)]
    public byte lll;
    [FieldOffset(1)]
    public byte llh;
    [FieldOffset(2)]
    public byte lhl;
    [FieldOffset(3)]
    public byte lhh;
    [FieldOffset(4)]
    public byte hll;
    [FieldOffset(5)]
    public byte hlh;
    [FieldOffset(6)]
    public byte hhl;
    [FieldOffset(7)]
    public byte hhh;
    [FieldOffset(0)]
    public ushort word;
    [FieldOffset(0)]
    public short aShort;
    [FieldOffset(0)]
    public uint aUint;
    [FieldOffset(0)]
    public int aInt;
    [FieldOffset(0)]
    public float aFloat;
    [FieldOffset(0)]
    public double aDouble;
    [FieldOffset(0)]
    public ulong aLonglong;
    [FieldOffset(0)]
    public long aLong;
  }
}
