// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Assets.AvatarComponent
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.MathUtilities;
using System;


namespace Microsoft.XboxLive.Avatars.Internal.Assets
{
  public class AvatarComponent
  {
    internal IBaseTextureAnimated[] m_Textures;
    internal TriangleBatch[] m_Batches;
    internal ShaderInstance[] m_ShaderInstance;
    internal ComponentManifest m_AvatarComponentManifest;

    public Guid AssetId => this.m_AvatarComponentManifest.m_ComponentInfo.m_AssetId;

    public ComponentInfo AvatarComponentInfo => this.m_AvatarComponentManifest.m_ComponentInfo;

    public ComponentManifest Manifest => this.m_AvatarComponentManifest;

    public IBaseTextureAnimated GetTexture(int index) => this.m_Textures[index];

    public int GetTexturesCount() => this.m_Textures.Length;

    public IBaseTextureAnimated[] GetTextures() => this.m_Textures;

    public ShaderInstance[] ShaderInstanceParameters => this.m_ShaderInstance;

    public TriangleBatch[] TriangleBatches => this.m_Batches;

    internal virtual int GetMemoryUsage()
    {
      int num = 0;
      foreach (TriangleBatch batch in this.m_Batches)
        num += batch.GetMemoryUsage();
      foreach (IBaseTextureAnimated texture in this.m_Textures)
        num += texture.GetMemoryUsage();
      return num + this.m_ShaderInstance.Length * 32;
    }
  }
}
