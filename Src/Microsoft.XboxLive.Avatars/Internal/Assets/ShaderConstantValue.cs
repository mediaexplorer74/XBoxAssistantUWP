// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Assets.ShaderConstantValue
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.MathUtilities;


namespace Microsoft.XboxLive.Avatars.Internal.Assets
{
  public struct ShaderConstantValue
  {
    public float f0;
    public float f1;
    public float f2;
    public float f3;

    internal void SetValue(Vector4 value)
    {
      this.f0 = value.X;
      this.f1 = value.Y;
      this.f2 = value.Z;
      this.f3 = value.W;
    }
  }
}
