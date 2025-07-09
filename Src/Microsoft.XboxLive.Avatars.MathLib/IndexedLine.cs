// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.IndexedLine
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll

using System.Runtime.InteropServices;


namespace Microsoft.XboxLive.MathUtilities
{
  [StructLayout(LayoutKind.Explicit, Size = 16)]
  public struct IndexedLine(int vertexIndex1, int vertexIndex2)
  {
    [FieldOffset(0)]
    public int i1 = vertexIndex1;
    [FieldOffset(4)]
    public int i2 = vertexIndex2;

    public override int GetHashCode() => this.i1 ^ this.i2;

    public bool Equals(IndexedLine other) => this.i1 == other.i1 && this.i2 == other.i2;
  }
}
