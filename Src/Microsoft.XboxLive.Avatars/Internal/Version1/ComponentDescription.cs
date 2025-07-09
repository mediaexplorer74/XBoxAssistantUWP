// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.ComponentDescription
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using System;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  public class ComponentDescription
  {
    public ComponentInfo m_ComponentInfo;
    public Guid m_OverrideAsset;

    public ComponentDescription()
    {
    }

    public ComponentDescription(ComponentInfo info, Guid overrideAssetId)
    {
      this.m_ComponentInfo = info;
      this.m_OverrideAsset = overrideAssetId;
    }

    public static bool operator ==(ComponentDescription a, ComponentDescription b)
    {
      return (object) a == (object) b || a.m_ComponentInfo == b.m_ComponentInfo && a.m_OverrideAsset == b.m_OverrideAsset;
    }

    public static bool operator !=(ComponentDescription a, ComponentDescription b) => !(a == b);

    public override bool Equals(object obj)
    {
      return (object) (obj as ComponentDescription) != null && this == (ComponentDescription) obj;
    }

    public override int GetHashCode()
    {
      return this.m_ComponentInfo.GetHashCode() + this.m_OverrideAsset.GetHashCode();
    }
  }
}
