// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.Vector4b
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll


namespace Microsoft.XboxLive.MathUtilities
{
  public struct Vector4b(int packedVector)
  {
    public byte X = (byte) (packedVector >> 24);
    public byte Y = (byte) (packedVector >> 16);
    public byte Z = (byte) (packedVector >> 8);
    public byte W = (byte) packedVector;
  }
}
