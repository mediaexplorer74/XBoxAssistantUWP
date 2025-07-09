// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Assets.ComponentInfo
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.MathUtilities;
using System;


namespace Microsoft.XboxLive.Avatars.Internal.Assets
{
  public struct ComponentInfo(
    Guid assetId,
    AvatarComponentMasks componentMask,
    Colorb customColor1,
    Colorb customColor2,
    Colorb customColor3)
  {
    internal Guid m_AssetId = assetId;
    internal AvatarComponentMasks m_ComponentMask = componentMask;
    internal Colorb m_CustomColors0 = customColor1;
    internal Colorb m_CustomColors1 = customColor2;
    internal Colorb m_CustomColors2 = customColor3;
    internal static readonly ComponentInfo Zero = new ComponentInfo();

    internal bool IsEmpty => this.m_AssetId == Guid.Empty;

    public Colorb CustomColor0 => this.m_CustomColors0;

    public Colorb CustomColor1 => this.m_CustomColors1;

    public Colorb CustomColor2 => this.m_CustomColors2;

    public AvatarComponentMasks ComponentMask => this.m_ComponentMask;

    public Guid AssetId => this.m_AssetId;

    public override bool Equals(object obj)
    {
      return obj is ComponentInfo componentInfo && this == componentInfo;
    }

    public override int GetHashCode()
    {
      return (int) ((short) (this.m_CustomColors0.GetHashCode() + this.m_CustomColors1.GetHashCode() + this.m_CustomColors2.GetHashCode()) + this.m_ComponentMask + (short) this.m_AssetId.GetHashCode());
    }

    public static bool operator ==(ComponentInfo a, ComponentInfo b)
    {
      return a.m_AssetId == b.m_AssetId && a.m_ComponentMask == b.m_ComponentMask && a.m_CustomColors0 == b.m_CustomColors0 && a.m_CustomColors1 == b.m_CustomColors1 && a.m_CustomColors2 == b.m_CustomColors2;
    }

    public static bool operator !=(ComponentInfo a, ComponentInfo b) => !(a == b);
  }
}
