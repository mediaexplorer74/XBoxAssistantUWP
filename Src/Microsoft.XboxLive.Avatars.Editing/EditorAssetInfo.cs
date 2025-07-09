// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.EditorAssetInfo
// Assembly: Microsoft.XboxLive.Avatars.Editing, Version=1.2.0.0, Culture=neutral, PublicKeyToken=07d39f95d274f46c
// MVID: 59092871-53EE-4223-B483-13354A63CEA7
// *********************************************************Microsoft.XboxLive.Avatars.Editing.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.MathUtilities;
using System;
using System.Collections.Generic;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class EditorAssetInfo
  {
    internal Guid m_assetId;
    internal uint m_assetDetails;
    internal BinaryAssetType m_assetType;
    internal List<ComponentInfo> m_previousAssets;
    internal Colorb m_assetDynamicColor;
    internal DynamicColorType m_assetDynamicColorType;
    internal ComponentColors m_componentColor;
    internal bool m_useComponentColor;
    internal bool m_removeEyeShadow;
    internal List<ComponentInfo> m_accessories;

    public Guid AssetId => this.m_assetId;

    public Colorb Color => this.m_assetDynamicColor;

    public DynamicColorType CustomColorType => this.m_assetDynamicColorType;

    public ComponentColors ComponentColor => this.m_componentColor;

    public EditorAssetInfo()
    {
      this.m_assetId = Guid.Empty;
      this.m_assetType = BinaryAssetType.Unknown;
      this.m_assetDynamicColorType = DynamicColorType.Count;
      this.m_assetDynamicColor = new Colorb();
      this.m_useComponentColor = false;
    }

    public EditorAssetInfo(Guid newAsset)
    {
      this.m_assetId = newAsset;
      this.m_assetType = BinaryAssetType.Unknown;
      this.m_assetDynamicColorType = DynamicColorType.Count;
      this.m_assetDynamicColor = new Colorb();
      this.m_useComponentColor = false;
    }

    public EditorAssetInfo(BlendShapeType blendShapeType, Guid blendShapeId)
    {
      this.m_assetId = blendShapeId;
      this.m_assetDetails = (uint) blendShapeType;
      this.m_assetType = BinaryAssetType.ShapeOverride;
      this.m_assetDynamicColorType = DynamicColorType.Count;
      this.m_assetDynamicColor = new Colorb();
      this.m_useComponentColor = false;
    }

    public EditorAssetInfo(DynamicTextureType textureType, Guid textureId)
    {
      this.m_assetId = textureId;
      this.m_assetDetails = (uint) textureType;
      this.m_assetType = BinaryAssetType.Texture;
      this.m_assetDynamicColorType = DynamicColorType.Count;
      this.m_assetDynamicColor = new Colorb();
    }

    public EditorAssetInfo(Guid newAsset, ComponentColors customColors)
    {
      this.m_assetId = newAsset;
      this.m_assetType = BinaryAssetType.Unknown;
      this.m_assetDynamicColorType = DynamicColorType.Count;
      this.m_assetDynamicColor = new Colorb();
      this.m_useComponentColor = true;
      this.m_componentColor = customColors;
    }

    public EditorAssetInfo(AvatarComponentMasks removeMask)
    {
      this.m_assetId = Guid.Empty;
      this.m_assetDetails = (uint) removeMask;
      this.m_assetType = BinaryAssetType.Component;
      this.m_assetDynamicColorType = DynamicColorType.Count;
      this.m_assetDynamicColor = new Colorb();
    }

    public void SetAssetCustomColor(Colorb color, DynamicColorType type)
    {
      this.m_assetDynamicColor = color;
      this.m_assetDynamicColorType = type;
    }

    public void SetAssetCustomColor(ComponentColors color)
    {
      this.m_componentColor = color;
      this.m_useComponentColor = true;
    }

    public void SetAccessories(List<ComponentInfo> accessories) => this.m_accessories = accessories;
  }
}
