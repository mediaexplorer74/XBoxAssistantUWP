// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AvatarManifestEditor
// Assembly: Microsoft.XboxLive.Avatars.Editing, Version=1.2.0.0, Culture=neutral, PublicKeyToken=07d39f95d274f46c
// MVID: 59092871-53EE-4223-B483-13354A63CEA7
// *********************************************************Microsoft.XboxLive.Avatars.Editing.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Version1;
using Microsoft.XboxLive.MathUtilities;
using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class AvatarManifestEditor
  {
    private AvatarManifestV1 m_Manifest;
    private AssetLoader m_AvatarApi;
    private IDataManager m_dataManager;
    private bool m_dirty;

    public float AvatarWidthFactor
    {
      get => this.m_Manifest.WidthFactor;
      set
      {
        this.m_Manifest.WidthFactor = (double) value <= 1.0 && (double) value >= -1.0 ? value : throw new ArgumentOutOfRangeException("The value must be in range [-1,1]");
      }
    }

    public float AvatarHeightFactor
    {
      get => this.m_Manifest.HeightFactor;
      set
      {
        this.m_Manifest.HeightFactor = (double) value <= 1.0 && (double) value >= -1.0 ? value : throw new ArgumentOutOfRangeException("The value must be in range [-1,1]");
      }
    }

    public bool DressDefaultClothes
    {
      get => this.m_Manifest.DressDefaultClothes;
      set => this.m_Manifest.DressDefaultClothes = value;
    }

    public AvatarManifest Manifest => (AvatarManifest) this.m_Manifest;

    public bool IsDirty => this.m_dirty;

    public byte[] OwnerXuid
    {
      get => this.m_Manifest.m_OwnerXuid;
      set => this.m_Manifest.m_OwnerXuid = value;
    }

    public byte[] ConsoleId
    {
      get => this.m_Manifest.m_ConsoleId;
      set => this.m_Manifest.m_ConsoleId = value;
    }

    public AvatarManifestEditor(
      AvatarManifest manifest,
      IDataManager downloadManager,
      AssetLoader avatarApi)
    {
      if (manifest == (AvatarManifest) null)
        throw new ArgumentNullException(nameof (manifest));
      this.m_Manifest = manifest.Version == 1 ? manifest as AvatarManifestV1 : throw new ArgumentException(nameof (manifest), "Invalid manifest");
      this.m_dirty = false;
      this.m_AvatarApi = avatarApi;
      this.m_dataManager = downloadManager;
    }

    public void UpdateDependencies()
    {
      if (this.m_dataManager == null)
        throw new InvalidOperationException("Data manager has not beed specified");
      this.m_Manifest.UpdateDependencies(this.m_dataManager);
      this.m_dirty = false;
    }

    public void SetManekinColorScheme()
    {
      for (DynamicColorType type = DynamicColorType.Skin; type < DynamicColorType.Count; ++type)
      {
        Colorb color = Utilities.ColorbFromVector4(this.m_Manifest.GetDynamicColor(type));
        int num = (int) color.red + (int) color.green + (int) color.blue + 765;
        color.red = (byte) ((num + (int) color.red) / 7);
        color.green = (byte) ((num + (int) color.green) / 7);
        color.blue = (byte) ((num + (int) color.blue) / 7);
        this.m_Manifest.SetDynamicColor(type, color);
      }
      this.m_Manifest.SetDynamicColor(DynamicColorType.Skin, new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
      this.m_dirty = true;
    }

    public void SetAvatarColor(DynamicColorType colorType, Colorb avatarColor)
    {
      if (colorType == DynamicColorType.EyeShadow && this.m_Manifest.GetReplacementTexture(DynamicTextureType.EyeShadow).m_TextureAssetId == Guid.Empty)
        this.UpdateEyeShadow();
      this.m_Manifest.SetDynamicColor(colorType, avatarColor);
      this.m_dirty = true;
    }

    public Colorb GetAvatarColor(DynamicColorType colorType)
    {
      return Utilities.ColorbFromVector4(this.m_Manifest.GetDynamicColor(colorType));
    }

    public void RemoveAllComponents()
    {
      this.m_Manifest.ClearComponents(AvatarComponentMasks.Head | AvatarComponentMasks.Body | AvatarComponentMasks.Shirt | AvatarComponentMasks.Trousers | AvatarComponentMasks.Shoes | AvatarComponentMasks.Hat | AvatarComponentMasks.Gloves | AvatarComponentMasks.Glasses | AvatarComponentMasks.Wristwear | AvatarComponentMasks.Earrings | AvatarComponentMasks.Ring | AvatarComponentMasks.Carryable);
      this.m_dirty = true;
    }

    public EditorAssetInfo ReplaceAsset(EditorAssetInfo newAsset)
    {
      EditorAssetInfo editorAssetInfo = new EditorAssetInfo();
      Guid assetId = newAsset.m_assetId;
      if (!(newAsset.m_assetId == Guid.Empty) && newAsset.m_assetType == BinaryAssetType.Unknown)
      {
        EditorAssetInfo assetType = this.GetAssetType(newAsset.m_assetId);
        newAsset.m_assetType = assetType.m_assetType;
        newAsset.m_assetDetails = assetType.m_assetDetails;
      }
      editorAssetInfo.m_removeEyeShadow = false;
      if (newAsset.CustomColorType < DynamicColorType.Count)
      {
        if (newAsset.CustomColorType == DynamicColorType.EyeShadow && this.m_Manifest.GetReplacementTexture(DynamicTextureType.EyeShadow).m_TextureAssetId == Guid.Empty)
          editorAssetInfo.m_removeEyeShadow = true;
        Vector4 dynamicColor = this.m_Manifest.GetDynamicColor(newAsset.CustomColorType);
        editorAssetInfo.SetAssetCustomColor(Utilities.ColorbFromVector4(dynamicColor), newAsset.CustomColorType);
        this.SetAvatarColor(newAsset.CustomColorType, newAsset.Color);
      }
      if (newAsset.m_removeEyeShadow)
        this.m_Manifest.SetReplacementTexture(DynamicTextureType.EyeShadow, new AvatarManifestV1.ReplacementTexture()
        {
          m_TextureAssetId = Guid.Empty,
          m_LinkedAssetId = Guid.Empty,
          m_Placement = {
            m_Rotation = 0.0f,
            m_Scale = 1f,
            m_TranslationU = 0.0f,
            m_TranslationV = 0.0f
          }
        });
      editorAssetInfo.m_assetType = newAsset.m_assetType;
      editorAssetInfo.m_assetDetails = newAsset.m_assetDetails;
      switch (newAsset.m_assetType)
      {
        case BinaryAssetType.Unknown:
        case BinaryAssetType.Animation:
          throw new AvatarException(Resources.InvalidAssetTypeText);
        case BinaryAssetType.Component:
          ComponentInfo newComponent = new ComponentInfo();
          newComponent.m_AssetId = newAsset.m_assetId;
          newComponent.m_ComponentMask = (AvatarComponentMasks) newAsset.m_assetDetails;
          if (newAsset.m_useComponentColor)
          {
            newComponent.m_CustomColors0 = Utilities.ColorbFromVector4(newAsset.m_componentColor.CustomColor0);
            newComponent.m_CustomColors1 = Utilities.ColorbFromVector4(newAsset.m_componentColor.CustomColor1);
            newComponent.m_CustomColors2 = Utilities.ColorbFromVector4(newAsset.m_componentColor.CustomColor2);
          }
          editorAssetInfo.m_previousAssets = this.m_Manifest.GetComponents(newComponent.m_ComponentMask);
          if (editorAssetInfo.m_previousAssets.Count > 0)
          {
            ComponentInfo previousAsset = editorAssetInfo.m_previousAssets[0];
            editorAssetInfo.m_assetId = previousAsset.AssetId;
            editorAssetInfo.m_useComponentColor = true;
            editorAssetInfo.m_componentColor.CustomColor0 = Utilities.Vector4FromInt(previousAsset.CustomColor0.CompositeArgb);
            editorAssetInfo.m_componentColor.CustomColor1 = Utilities.Vector4FromInt(previousAsset.CustomColor1.CompositeArgb);
            editorAssetInfo.m_componentColor.CustomColor2 = Utilities.Vector4FromInt(previousAsset.CustomColor2.CompositeArgb);
          }
          else
            editorAssetInfo.m_assetId = Guid.Empty;
          if (newAsset.m_assetId == Guid.Empty)
          {
            this.m_Manifest.RemoveComponents(newComponent.m_ComponentMask);
            break;
          }
          if (newAsset.m_previousAssets != null && newAsset.m_previousAssets.Count > 0)
          {
            AvatarComponentMasks mask = AvatarComponentMasks.None;
            foreach (ComponentInfo previousAsset in newAsset.m_previousAssets)
              mask |= previousAsset.ComponentMask;
            this.m_Manifest.UpdatePreviousComponents(mask);
            this.m_Manifest.ClearComponents(mask);
            foreach (ComponentInfo previousAsset in newAsset.m_previousAssets)
              this.m_Manifest.m_ComponentInfo.Add(new ComponentDescription(previousAsset, Guid.Empty));
            this.m_Manifest.EquipRequiredComponents(true);
            break;
          }
          this.m_Manifest.UpdatePreviousComponents(newComponent.ComponentMask);
          this.m_Manifest.ReplaceComponent(newComponent);
          this.m_Manifest.EquipRequiredComponents(true);
          break;
        case BinaryAssetType.Texture:
          DynamicTextureType assetDetails1 = (DynamicTextureType) newAsset.m_assetDetails;
          AvatarManifestV1.ReplacementTexture texture = new AvatarManifestV1.ReplacementTexture();
          texture.m_LinkedAssetId = Guid.Empty;
          texture.m_TextureAssetId = newAsset.m_assetId;
          texture.m_Placement.m_Rotation = 0.0f;
          texture.m_Placement.m_Scale = 1f;
          texture.m_Placement.m_TranslationU = 0.0f;
          texture.m_Placement.m_TranslationV = 0.0f;
          AvatarManifestV1.ReplacementTexture replacementTexture = this.m_Manifest.GetReplacementTexture(assetDetails1);
          editorAssetInfo.m_assetId = replacementTexture.m_TextureAssetId;
          this.m_Manifest.SetReplacementTexture(assetDetails1, texture);
          if (assetDetails1 == DynamicTextureType.Eye && this.m_Manifest.GetReplacementTexture(DynamicTextureType.EyeShadow).m_TextureAssetId != Guid.Empty)
          {
            this.UpdateEyeShadow();
            break;
          }
          break;
        case BinaryAssetType.ShapeOverride:
        case BinaryAssetType.ShapeOverridePost:
          BlendShapeType assetDetails2 = (BlendShapeType) newAsset.m_assetDetails;
          editorAssetInfo.m_assetId = this.m_Manifest.GetBlendShape(assetDetails2);
          this.m_Manifest.SetBlendShape(assetDetails2, newAsset.m_assetId);
          break;
      }
      this.TryEquipAccessories(newAsset);
      this.m_Manifest.m_Dirty = true;
      this.m_dirty = true;
      return editorAssetInfo;
    }

    public EditorAssetInfo GetComponentInfo(EditorAssetInfo info)
    {
      EditorAssetInfo componentInfo = new EditorAssetInfo();
      componentInfo.m_assetType = info.m_assetType;
      componentInfo.m_assetDetails = info.m_assetDetails;
      if (componentInfo.m_assetType == BinaryAssetType.Unknown && info.m_assetId != Guid.Empty)
      {
        EditorAssetInfo assetType = this.GetAssetType(info.m_assetId);
        componentInfo.m_assetType = assetType.m_assetType;
        componentInfo.m_assetDetails = assetType.m_assetDetails;
      }
      componentInfo.m_removeEyeShadow = false;
      if (info.CustomColorType < DynamicColorType.Count)
      {
        if (info.CustomColorType == DynamicColorType.EyeShadow && this.m_Manifest.GetReplacementTexture(DynamicTextureType.EyeShadow).m_TextureAssetId == Guid.Empty)
          componentInfo.m_removeEyeShadow = true;
        Vector4 dynamicColor = this.m_Manifest.GetDynamicColor(info.CustomColorType);
        componentInfo.SetAssetCustomColor(Utilities.ColorbFromVector4(dynamicColor), info.CustomColorType);
      }
      switch (componentInfo.m_assetType)
      {
        case BinaryAssetType.Unknown:
        case BinaryAssetType.Animation:
          throw new AvatarException(Resources.InvalidAssetTypeText);
        case BinaryAssetType.Component:
          AvatarComponentMasks assetDetails1 = (AvatarComponentMasks) componentInfo.m_assetDetails;
          componentInfo.m_previousAssets = this.m_Manifest.GetComponents(assetDetails1);
          if (componentInfo.m_previousAssets.Count > 0)
          {
            ComponentInfo previousAsset = componentInfo.m_previousAssets[0];
            componentInfo.m_assetId = previousAsset.AssetId;
            Colorb[] colors = new Colorb[3]
            {
              previousAsset.CustomColor0,
              previousAsset.CustomColor1,
              previousAsset.CustomColor2
            };
            componentInfo.m_componentColor = new ComponentColors(colors);
            break;
          }
          break;
        case BinaryAssetType.Texture:
          AvatarManifestV1.ReplacementTexture replacementTexture = this.m_Manifest.GetReplacementTexture((DynamicTextureType) componentInfo.m_assetDetails);
          componentInfo.m_assetId = replacementTexture.m_TextureAssetId;
          break;
        case BinaryAssetType.ShapeOverride:
        case BinaryAssetType.ShapeOverridePost:
          BlendShapeType assetDetails2 = (BlendShapeType) componentInfo.m_assetDetails;
          componentInfo.m_assetId = this.m_Manifest.GetBlendShape(assetDetails2);
          break;
      }
      return componentInfo;
    }

    private EditorAssetInfo GetAssetType(Guid guid)
    {
      BlendShapeType blendShapeAssetType = AssetLoader.GetBlendShapeAssetType(guid);
      EditorAssetInfo assetType = new EditorAssetInfo();
      if (blendShapeAssetType != BlendShapeType.Count)
      {
        assetType.m_assetType = BinaryAssetType.ShapeOverride;
        assetType.m_assetDetails = (uint) blendShapeAssetType;
        return assetType;
      }
      DynamicTextureType textureAssetType = AssetLoader.GetTextureAssetType(guid);
      if (textureAssetType != DynamicTextureType.Count)
      {
        assetType.m_assetType = BinaryAssetType.Texture;
        assetType.m_assetDetails = (uint) textureAssetType;
        return assetType;
      }
      ComponentCategories componentTypeFromAssetId = AssetLoader.GetComponentTypeFromAssetId(guid);
      if (componentTypeFromAssetId == ComponentCategories.Animation)
      {
        assetType.m_assetType = BinaryAssetType.Animation;
        assetType.m_assetDetails = 0U;
        return assetType;
      }
      assetType.m_assetType = BinaryAssetType.Component;
      assetType.m_assetDetails = (uint) (componentTypeFromAssetId & ComponentCategories.Models);
      return assetType;
    }

    protected void UpdateEyeShadow()
    {
      AvatarManifestV1.ReplacementTexture texture = new AvatarManifestV1.ReplacementTexture();
      texture.m_LinkedAssetId = Guid.Empty;
      AvatarAssetDependency dependentAssets = AvatarAssetsDependenciesResolver.GetDependentAssets(this.m_dataManager, this.m_Manifest.GetReplacementTexture(DynamicTextureType.Eye).m_TextureAssetId);
      texture.m_TextureAssetId = dependentAssets.m_DependentAssetId;
      if (texture.m_TextureAssetId == Guid.Empty)
        throw new AvatarException("Failed to resolver eye shadow texture.");
      texture.m_Placement.m_Rotation = 0.0f;
      texture.m_Placement.m_Scale = 1f;
      texture.m_Placement.m_TranslationU = 0.0f;
      texture.m_Placement.m_TranslationV = 0.0f;
      this.m_Manifest.SetReplacementTexture(DynamicTextureType.EyeShadow, texture);
    }

    protected void TryEquipAccessories(EditorAssetInfo info)
    {
      if (info.m_accessories == null || info.m_accessories.Count == 0)
        return;
      foreach (ComponentInfo accessory in info.m_accessories)
      {
        if (this.m_Manifest.FindComponent(accessory.ComponentMask, AssetMatchingMode.Any) == -1)
          this.m_Manifest.SetComponentInfo(accessory);
      }
    }

    public void SetOutfit(AvatarManifest manifest)
    {
      AvatarManifestV1 avatarManifestV1 = manifest as AvatarManifestV1;
      if ((object) avatarManifestV1 == null)
        return;
      AvatarManifestEditor.StripNonOutfitComponents((AvatarManifest) this.m_Manifest);
      for (int index = 0; index < avatarManifestV1.m_ComponentInfo.Count; ++index)
      {
        ComponentInfo componentInfo = avatarManifestV1.m_ComponentInfo[index].m_ComponentInfo;
        if (componentInfo.ComponentMask != AvatarComponentMasks.Hair)
          this.m_Manifest.SetComponentInfo(componentInfo);
      }
      this.m_Manifest.SetDynamicColor(DynamicColorType.EyeShadow, Utilities.ColorbFromVector4(avatarManifestV1.GetDynamicColor(DynamicColorType.EyeShadow)));
      this.m_Manifest.SetDynamicColor(DynamicColorType.Mouth, Utilities.ColorbFromVector4(avatarManifestV1.GetDynamicColor(DynamicColorType.Mouth)));
      AvatarManifestV1.ReplacementTexture replacementTexture = avatarManifestV1.GetReplacementTexture(DynamicTextureType.EyeShadow);
      if (replacementTexture.m_TextureAssetId != Guid.Empty)
      {
        replacementTexture.m_TextureAssetId = this.m_Manifest.GetReplacementTexture(DynamicTextureType.Eye).m_LinkedAssetId;
        this.m_Manifest.SetReplacementTexture(DynamicTextureType.EyeShadow, replacementTexture);
      }
      else
        this.m_Manifest.SetReplacementTexture(DynamicTextureType.EyeShadow, new AvatarManifestV1.ReplacementTexture()
        {
          m_TextureAssetId = Guid.Empty
        });
    }

    public static void StripNonOutfitComponents(AvatarManifest manifest)
    {
      (manifest as AvatarManifestV1)?.ClearComponents(AvatarComponentMasks.Head | AvatarComponentMasks.Body | AvatarComponentMasks.Shirt | AvatarComponentMasks.Trousers | AvatarComponentMasks.Shoes | AvatarComponentMasks.Hat | AvatarComponentMasks.Gloves | AvatarComponentMasks.Glasses | AvatarComponentMasks.Wristwear | AvatarComponentMasks.Earrings | AvatarComponentMasks.Ring | AvatarComponentMasks.Carryable);
    }

    public void RemoveComponents(AvatarManifestEditor.RemovableComponents mask)
    {
      AvatarManifestV1.ReplacementTexture texture = new AvatarManifestV1.ReplacementTexture();
      switch (mask)
      {
        case AvatarManifestEditor.RemovableComponents.Earrings:
          this.m_Manifest.RemoveComponents(AvatarComponentMasks.Earrings);
          break;
        case AvatarManifestEditor.RemovableComponents.Glasses:
          this.m_Manifest.RemoveComponents(AvatarComponentMasks.Glasses);
          break;
        case AvatarManifestEditor.RemovableComponents.Gloves:
          this.m_Manifest.RemoveComponents(AvatarComponentMasks.Gloves);
          break;
        case AvatarManifestEditor.RemovableComponents.Hat:
          this.m_Manifest.RemoveComponents(AvatarComponentMasks.Hat);
          break;
        case AvatarManifestEditor.RemovableComponents.Ring:
          this.m_Manifest.RemoveComponents(AvatarComponentMasks.Ring);
          break;
        case AvatarManifestEditor.RemovableComponents.Wristwear:
          this.m_Manifest.RemoveComponents(AvatarComponentMasks.Wristwear);
          break;
        case AvatarManifestEditor.RemovableComponents.FacialHair:
          this.m_Manifest.SetReplacementTexture(DynamicTextureType.FacialHair, texture);
          break;
        case AvatarManifestEditor.RemovableComponents.EyeShadow:
          this.m_Manifest.SetReplacementTexture(DynamicTextureType.EyeShadow, texture);
          break;
        case AvatarManifestEditor.RemovableComponents.SkinFeatures:
          this.m_Manifest.SetReplacementTexture(DynamicTextureType.SkinFeatures, texture);
          break;
        case AvatarManifestEditor.RemovableComponents.Prop:
          this.m_Manifest.RemoveComponents(AvatarComponentMasks.Carryable);
          break;
        default:
          throw new InvalidOperationException("Shouldn't get here");
      }
    }

    public bool IsAssetPresent(Guid assetId)
    {
      EditorAssetInfo assetType = this.GetAssetType(assetId);
      switch (assetType.m_assetType)
      {
        case BinaryAssetType.Component:
          return this.m_Manifest.IsAssetPresent(assetId);
        case BinaryAssetType.Texture:
          return this.m_Manifest.GetReplacementTexture((DynamicTextureType) assetType.m_assetDetails).m_TextureAssetId == assetId;
        case BinaryAssetType.ShapeOverride:
          return this.m_Manifest.GetBlendShape((BlendShapeType) assetType.m_assetDetails) == assetId;
        default:
          return false;
      }
    }

    public Guid GetComponentGuid(string avatarComponentName)
    {
      try
      {
        return this.m_Manifest.GetComponentInfo((AvatarComponentMasks) Enum.Parse(typeof (AvatarComponentMasks), avatarComponentName, true), AssetMatchingMode.All).AssetId;
      }
      catch (ArgumentException ex)
      {
      }
      try
      {
        return this.m_Manifest.GetReplacementTexture((DynamicTextureType) Enum.Parse(typeof (DynamicTextureType), avatarComponentName, true)).m_TextureAssetId;
      }
      catch (ArgumentException ex)
      {
      }
      return this.m_Manifest.GetBlendShape((BlendShapeType) Enum.Parse(typeof (BlendShapeType), avatarComponentName, true));
    }

    public enum RemovableComponents
    {
      NotRemovable,
      Earrings,
      Glasses,
      Gloves,
      Hat,
      Ring,
      Wristwear,
      FacialHair,
      EyeShadow,
      SkinFeatures,
      Prop,
    }
  }
}
