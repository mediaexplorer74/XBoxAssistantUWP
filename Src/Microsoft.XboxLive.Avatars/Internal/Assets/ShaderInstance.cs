// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Assets.ShaderInstance
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal.Assets
{
  public struct ShaderInstance
  {
    public ShaderId ShaderId;
    public ShaderParameter[] ShaderParameters;

    internal ShaderInstance Clone()
    {
      ShaderInstance shaderInstance = new ShaderInstance();
      shaderInstance.ShaderId = this.ShaderId;
      shaderInstance.ShaderParameters = new ShaderParameter[this.ShaderParameters.Length];
      this.ShaderParameters.CopyTo((Array) shaderInstance.ShaderParameters, 0);
      return shaderInstance;
    }
  }
}
