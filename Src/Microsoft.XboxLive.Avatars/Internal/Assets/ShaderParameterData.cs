// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Assets.ShaderParameterData
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System.Runtime.InteropServices;


namespace Microsoft.XboxLive.Avatars.Internal.Assets
{
  [StructLayout(LayoutKind.Explicit)]
  public struct ShaderParameterData
  {
    [FieldOffset(0)]
    public TextureInstance Texture;
    [FieldOffset(0)]
    public ShaderConstantValue Constant;
    [FieldOffset(0)]
    public ShaderConstantInt ConstantInt;
  }
}
