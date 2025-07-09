// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Assets.ComponentManifest
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.Collections.Generic;


namespace Microsoft.XboxLive.Avatars.Internal.Assets
{
  public class ComponentManifest
  {
    internal ComponentInfo m_ComponentInfo;
    internal List<Guid> m_AssetIds = new List<Guid>();
    internal ShaderConstantOverride[] m_ShaderOverrides;
    internal int m_ModelIndex;

    internal ComponentManifest(
      ComponentInfo componentInfo,
      ShaderConstantOverride[] shaderOverrides,
      int modelIndex)
    {
      this.m_ComponentInfo = componentInfo;
      this.m_ShaderOverrides = shaderOverrides;
      this.m_ModelIndex = modelIndex;
    }

    public override bool Equals(object obj)
    {
      return obj is ComponentManifest && this.Equals(obj as ComponentManifest);
    }

    public override int GetHashCode()
    {
      int hashCode = this.m_ComponentInfo.GetHashCode() ^ this.m_AssetIds.Count ^ this.m_ComponentInfo.CustomColor0.CompositeArgb ^ this.m_ComponentInfo.CustomColor1.CompositeArgb ^ this.m_ComponentInfo.CustomColor2.CompositeArgb ^ this.m_ModelIndex;
      int length = this.m_ShaderOverrides.Length;
      for (int index = 0; index < length; ++index)
        hashCode ^= this.m_ShaderOverrides[index].GetHashCode();
      int count = this.m_AssetIds.Count;
      for (int index = 0; index < count; ++index)
        hashCode ^= this.m_AssetIds[index].GetHashCode();
      return hashCode;
    }

    public bool Equals(ComponentManifest other)
    {
      if (this.m_ComponentInfo.AssetId != other.m_ComponentInfo.AssetId || this.m_ModelIndex != other.m_ModelIndex || this.m_AssetIds.Count != other.m_AssetIds.Count || this.m_ComponentInfo.CustomColor0 != other.m_ComponentInfo.CustomColor0 || this.m_ComponentInfo.CustomColor1 != other.m_ComponentInfo.CustomColor1 || this.m_ComponentInfo.CustomColor2 != other.m_ComponentInfo.CustomColor2 || this.m_ShaderOverrides.Length != other.m_ShaderOverrides.Length)
        return false;
      int length = this.m_ShaderOverrides.Length;
      for (int index = 0; index < length; ++index)
      {
        if (!this.m_ShaderOverrides[index].Equals((object) other.m_ShaderOverrides[index]))
          return false;
      }
      int count = this.m_AssetIds.Count;
      for (int index = 0; index < count; ++index)
      {
        if (!this.m_AssetIds[index].Equals(other.m_AssetIds[index]))
          return false;
      }
      return true;
    }

    internal void AddAsset(Guid assetId) => this.m_AssetIds.Add(assetId);
  }
}
