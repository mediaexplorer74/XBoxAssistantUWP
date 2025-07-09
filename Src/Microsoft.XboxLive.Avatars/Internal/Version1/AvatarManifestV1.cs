// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.AvatarManifestV1
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using Microsoft.XboxLive.MathUtilities;
using System;
using System.Collections.Generic;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  public class AvatarManifestV1 : AvatarManifest
  {
    private static readonly AvatarComponentMasks[] s_RequiredComponents = new AvatarComponentMasks[4]
    {
      AvatarComponentMasks.Shoes,
      AvatarComponentMasks.Trousers,
      AvatarComponentMasks.Shirt,
      AvatarComponentMasks.Hair
    };
    private static readonly DynamicTextureType[] s_RequiredTextures = new DynamicTextureType[3]
    {
      DynamicTextureType.Mouth,
      DynamicTextureType.Eye,
      DynamicTextureType.Eyebrow
    };
    private static readonly BlendShapeType[] s_RequiredBlendShapes = new BlendShapeType[3]
    {
      BlendShapeType.Chin,
      BlendShapeType.Nose,
      BlendShapeType.Ear
    };
    private static readonly ComponentInfo[,] s_DefaultRequiredComponents = new ComponentInfo[2, 4]
    {
      {
        new ComponentInfo(AvatarAssetsPack.GrungeTrainers, AvatarComponentMasks.Shoes, new Colorb(0), new Colorb(0), new Colorb(0)),
        new ComponentInfo(AvatarAssetsPack.Jeans, AvatarComponentMasks.Trousers, new Colorb(0), new Colorb(0), new Colorb(0)),
        new ComponentInfo(AvatarAssetsPack.MalePowerTee, AvatarComponentMasks.Shirt, new Colorb((byte) 148, (byte) 214, (byte) 20), new Colorb(0), new Colorb(0)),
        new ComponentInfo(AvatarAssetsPack.ShortAndSpikey, AvatarComponentMasks.Hair, new Colorb(0), new Colorb(0), new Colorb(0))
      },
      {
        new ComponentInfo(AvatarAssetsPack.Sandals, AvatarComponentMasks.Shoes, new Colorb(0), new Colorb(0), new Colorb(0)),
        new ComponentInfo(AvatarAssetsPack.PolkadotSkirt, AvatarComponentMasks.Trousers, new Colorb(0), new Colorb(0), new Colorb(0)),
        new ComponentInfo(AvatarAssetsPack.TeeWithBelt, AvatarComponentMasks.Shirt, new Colorb(0), new Colorb(0), new Colorb(0)),
        new ComponentInfo(AvatarAssetsPack.EvenLength, AvatarComponentMasks.Hair, new Colorb(0), new Colorb(0), new Colorb(0))
      }
    };
    private static readonly List<Guid> s_CoreAssets = new List<Guid>((IEnumerable<Guid>) new Guid[10]
    {
      AvatarAssetsPack.GrungeTrainers,
      AvatarAssetsPack.Jeans,
      AvatarAssetsPack.MalePowerTee,
      AvatarAssetsPack.ShortAndSpikey,
      AvatarAssetsPack.Sandals,
      AvatarAssetsPack.PolkadotSkirt,
      AvatarAssetsPack.TeeWithBelt,
      AvatarAssetsPack.EvenLength,
      AvatarAssetsPack.FemaleBody,
      AvatarAssetsPack.MaleBody
    });
    private static readonly byte[] s_TocAssetVersionId = new byte[6]
    {
      (byte) 241,
      (byte) 9,
      (byte) 161,
      (byte) 156,
      (byte) 178,
      (byte) 224
    };
    private float m_WeightFactor;
    private float m_HeightFactor;
    internal byte[] m_OwnerXuid;
    internal byte[] m_ConsoleId;
    private AvatarManifestV1.BlendShape[] m_BlendShapes;
    private AvatarManifestV1.ReplacementTexture[] m_ReplacementTextures;
    private Colorb[] m_DynamicColors;
    internal ComponentDescription m_BodyComponentInfo;
    internal ComponentDescription m_HeadComponentInfo;
    internal bool m_Dirty;
    internal List<ComponentDescription> m_ComponentInfo = new List<ComponentDescription>();
    public ComponentInfo[] m_PreviousRequiredComponentInfo;

    private static bool LoadBlendShape(
      EndianStream stream,
      out AvatarManifestV1.BlendShape blendShape)
    {
      blendShape.m_BlendShapeAssetId = stream.ReadGuid();
      return AvatarManifestV1.ValidateAssetId(blendShape.m_BlendShapeAssetId);
    }

    private static bool LoadReplacementTexture(
      EndianStream stream,
      out AvatarManifestV1.ReplacementTexture replacementTexture)
    {
      replacementTexture = new AvatarManifestV1.ReplacementTexture();
      replacementTexture.m_TextureAssetId = stream.ReadGuid();
      if (!AvatarManifestV1.ValidateAssetId(replacementTexture.m_TextureAssetId))
        return false;
      replacementTexture.m_Placement.m_Scale = stream.ReadFloat();
      replacementTexture.m_Placement.m_Rotation = stream.ReadFloat();
      replacementTexture.m_Placement.m_TranslationU = stream.ReadFloat();
      replacementTexture.m_Placement.m_TranslationV = stream.ReadFloat();
      return true;
    }

    private static bool LoadComponentInfo(EndianStream stream, out ComponentInfo componentInfo)
    {
      componentInfo = new ComponentInfo();
      componentInfo.m_AssetId = stream.ReadGuid();
      if (!AvatarManifestV1.ValidateAssetId(componentInfo.m_AssetId))
        return false;
      componentInfo.m_ComponentMask = (AvatarComponentMasks) stream.ReadShort();
      int num = (int) stream.ReadShort();
      byte alpha1 = (byte) stream.ReadByte();
      byte red1 = (byte) stream.ReadByte();
      byte green1 = (byte) stream.ReadByte();
      byte blue1 = (byte) stream.ReadByte();
      componentInfo.m_CustomColors0 = new Colorb(red1, green1, blue1, alpha1);
      byte alpha2 = (byte) stream.ReadByte();
      byte red2 = (byte) stream.ReadByte();
      byte green2 = (byte) stream.ReadByte();
      byte blue2 = (byte) stream.ReadByte();
      componentInfo.m_CustomColors1 = new Colorb(red2, green2, blue2, alpha2);
      byte alpha3 = (byte) stream.ReadByte();
      byte red3 = (byte) stream.ReadByte();
      byte green3 = (byte) stream.ReadByte();
      byte blue3 = (byte) stream.ReadByte();
      componentInfo.m_CustomColors2 = new Colorb(red3, green3, blue3, alpha3);
      return true;
    }

    internal bool InitFromBinary(EndianStream stream)
    {
      this.m_WeightFactor = stream.ReadFloat();
      this.m_HeightFactor = stream.ReadFloat();
      for (int index = 0; index < this.m_BlendShapes.Length; ++index)
      {
        if (!AvatarManifestV1.LoadBlendShape(stream, out this.m_BlendShapes[index]))
          return false;
      }
      for (int index = 0; index < this.m_ReplacementTextures.Length; ++index)
      {
        if (!AvatarManifestV1.LoadReplacementTexture(stream, out this.m_ReplacementTextures[index]))
          return false;
      }
      for (int index = 0; index < this.m_DynamicColors.Length; ++index)
      {
        byte alpha = (byte) stream.ReadByte();
        byte red = (byte) stream.ReadByte();
        byte green = (byte) stream.ReadByte();
        byte blue = (byte) stream.ReadByte();
        this.m_DynamicColors[index] = new Colorb(red, green, blue, alpha);
      }
      ComponentInfo componentInfo;
      if (!AvatarManifestV1.LoadComponentInfo(stream, out componentInfo) || componentInfo.ComponentMask != AvatarComponentMasks.Body || componentInfo.AssetId == Guid.Empty)
        return false;
      this.m_BodyComponentInfo = new ComponentDescription(componentInfo, Guid.Empty);
      if (!AvatarManifestV1.LoadComponentInfo(stream, out componentInfo) || componentInfo.ComponentMask != AvatarComponentMasks.Head || componentInfo.AssetId == Guid.Empty)
        return false;
      this.m_HeadComponentInfo = new ComponentDescription(componentInfo, Guid.Empty);
      for (int index = 0; index < 13; ++index)
      {
        if (!AvatarManifestV1.LoadComponentInfo(stream, out componentInfo))
          return false;
        if (componentInfo.m_AssetId != Guid.Empty)
          this.m_ComponentInfo.Add(new ComponentDescription(componentInfo, Guid.Empty));
      }
      for (int index = 0; index < this.m_PreviousRequiredComponentInfo.Length; ++index)
      {
        if (!AvatarManifestV1.LoadComponentInfo(stream, out componentInfo))
          return false;
        this.m_PreviousRequiredComponentInfo[index] = componentInfo;
      }
      stream.Read(this.m_ConsoleId, 0, 5);
      stream.Read(this.m_OwnerXuid, 0, 8);
      this.m_Dirty = true;
      return this.Validate();
    }

    private static void SaveBlendShape(EndianStream stream, AvatarManifestV1.BlendShape blendShape)
    {
      stream.WriteGuid(blendShape.m_BlendShapeAssetId);
    }

    private static void SaveReplacementTexture(
      EndianStream stream,
      AvatarManifestV1.ReplacementTexture replacementTexture)
    {
      stream.WriteGuid(replacementTexture.m_TextureAssetId);
      stream.WriteFloat(replacementTexture.m_Placement.m_Scale);
      stream.WriteFloat(replacementTexture.m_Placement.m_Rotation);
      stream.WriteFloat(replacementTexture.m_Placement.m_TranslationU);
      stream.WriteFloat(replacementTexture.m_Placement.m_TranslationV);
    }

    private static void SaveColor(EndianStream stream, Colorb color)
    {
      stream.WriteByte(color.alpha);
      stream.WriteByte(color.red);
      stream.WriteByte(color.green);
      stream.WriteByte(color.blue);
    }

    private static void SaveComponentInfo(EndianStream stream, ComponentInfo component)
    {
      stream.WriteGuid(component.AssetId);
      stream.WriteShort((short) component.m_ComponentMask);
      stream.WriteShort((short) 0);
      AvatarManifestV1.SaveColor(stream, component.m_CustomColors0);
      AvatarManifestV1.SaveColor(stream, component.m_CustomColors1);
      AvatarManifestV1.SaveColor(stream, component.m_CustomColors2);
    }

    public override byte[] SaveToBinary()
    {
      byte[] buffer = new byte[1000];
      EndianStream stream = new EndianStream((Stream) new MemoryStream(buffer));
      stream.WriteUint(0U);
      stream.WriteFloat(this.m_WeightFactor);
      stream.WriteFloat(this.m_HeightFactor);
      int length1 = this.m_BlendShapes.Length;
      int index1;
      for (index1 = 0; index1 < length1; ++index1)
        AvatarManifestV1.SaveBlendShape(stream, this.m_BlendShapes[index1]);
      if (index1 < 3)
      {
        AvatarManifestV1.BlendShape blendShape = new AvatarManifestV1.BlendShape();
        for (; index1 < 3; ++index1)
          AvatarManifestV1.SaveBlendShape(stream, blendShape);
      }
      int length2 = this.m_ReplacementTextures.Length;
      int index2;
      for (index2 = 0; index2 < length2; ++index2)
        AvatarManifestV1.SaveReplacementTexture(stream, this.m_ReplacementTextures[index2]);
      if (index2 < 6)
      {
        AvatarManifestV1.ReplacementTexture replacementTexture = new AvatarManifestV1.ReplacementTexture();
        for (; index2 < 6; ++index2)
          AvatarManifestV1.SaveReplacementTexture(stream, replacementTexture);
      }
      for (int index3 = 0; index3 < 9; ++index3)
        AvatarManifestV1.SaveColor(stream, this.m_DynamicColors[index3]);
      AvatarManifestV1.SaveComponentInfo(stream, this.m_BodyComponentInfo.m_ComponentInfo);
      AvatarManifestV1.SaveComponentInfo(stream, this.m_HeadComponentInfo.m_ComponentInfo);
      int count = this.m_ComponentInfo.Count;
      int index4;
      for (index4 = 0; index4 < count; ++index4)
        AvatarManifestV1.SaveComponentInfo(stream, this.m_ComponentInfo[index4].m_ComponentInfo);
      if (index4 < 13)
      {
        ComponentInfo component = new ComponentInfo();
        for (; index4 < 13; ++index4)
          AvatarManifestV1.SaveComponentInfo(stream, component);
      }
      int length3 = this.m_PreviousRequiredComponentInfo.Length;
      for (int index5 = 0; index5 < length3; ++index5)
        AvatarManifestV1.SaveComponentInfo(stream, this.m_PreviousRequiredComponentInfo[index5]);
      stream.Write(this.m_ConsoleId, 0, 5);
      stream.Write(this.m_OwnerXuid, 0, 8);
      return buffer;
    }

    internal bool DressDefaultClothes { get; set; }

    internal AvatarManifestV1()
    {
      this.m_ConsoleId = new byte[5];
      this.m_OwnerXuid = new byte[8];
      this.m_BlendShapes = new AvatarManifestV1.BlendShape[3];
      this.m_ReplacementTextures = new AvatarManifestV1.ReplacementTexture[6];
      this.m_DynamicColors = new Colorb[9];
      this.m_PreviousRequiredComponentInfo = new ComponentInfo[4];
      this.m_VersionNumber = 1;
      this.DressDefaultClothes = true;
    }

    public ComponentDescription BodyComponentInfo => this.m_BodyComponentInfo;

    public ComponentDescription HeadComponentInfo => this.m_HeadComponentInfo;

    public float HeightFactor
    {
      get => this.m_HeightFactor;
      set => this.m_HeightFactor = value;
    }

    public float WidthFactor
    {
      get => this.m_WeightFactor;
      set => this.m_WeightFactor = value;
    }

    public override AvatarManifest Clone()
    {
      AvatarManifestV1 avatarManifestV1 = new AvatarManifestV1();
      avatarManifestV1.m_WeightFactor = this.m_WeightFactor;
      avatarManifestV1.m_HeightFactor = this.m_HeightFactor;
      avatarManifestV1.DressDefaultClothes = this.DressDefaultClothes;
      avatarManifestV1.m_VersionNumber = this.m_VersionNumber;
      avatarManifestV1.m_DynamicColors = this.m_DynamicColors.Clone() as Colorb[];
      avatarManifestV1.m_ReplacementTextures = this.m_ReplacementTextures.Clone() as AvatarManifestV1.ReplacementTexture[];
      avatarManifestV1.m_PreviousRequiredComponentInfo = this.m_PreviousRequiredComponentInfo.Clone() as ComponentInfo[];
      avatarManifestV1.m_HeadComponentInfo = new ComponentDescription();
      avatarManifestV1.m_HeadComponentInfo.m_ComponentInfo = this.m_HeadComponentInfo.m_ComponentInfo;
      avatarManifestV1.m_HeadComponentInfo.m_OverrideAsset = this.m_HeadComponentInfo.m_OverrideAsset;
      avatarManifestV1.m_ComponentInfo = new List<ComponentDescription>();
      for (int index = 0; index < this.m_ComponentInfo.Count; ++index)
        avatarManifestV1.m_ComponentInfo.Add(new ComponentDescription()
        {
          m_ComponentInfo = this.m_ComponentInfo[index].m_ComponentInfo,
          m_OverrideAsset = this.m_ComponentInfo[index].m_OverrideAsset
        });
      avatarManifestV1.m_BodyComponentInfo = new ComponentDescription();
      avatarManifestV1.m_BodyComponentInfo.m_ComponentInfo = this.m_BodyComponentInfo.m_ComponentInfo;
      avatarManifestV1.m_BodyComponentInfo.m_OverrideAsset = this.m_BodyComponentInfo.m_OverrideAsset;
      avatarManifestV1.m_BlendShapes = this.m_BlendShapes.Clone() as AvatarManifestV1.BlendShape[];
      avatarManifestV1.m_Dirty = this.m_Dirty;
      avatarManifestV1.m_OwnerXuid = this.m_OwnerXuid;
      avatarManifestV1.m_ConsoleId = this.m_ConsoleId;
      return (AvatarManifest) avatarManifestV1;
    }

    private static bool ValidateAssetId(Guid guid)
    {
      if (AssetLoader.GetAssetGuidType(guid) == AssetGuidType.TOC)
      {
        byte[] byteArray = guid.ToByteArray();
        for (int index = 0; index < 6; ++index)
        {
          if ((int) AvatarManifestV1.s_TocAssetVersionId[index] != (int) byteArray[index + 10])
            return false;
        }
      }
      return true;
    }

    internal void UpdatePreviousComponents(AvatarComponentMasks mask)
    {
      for (int index1 = 0; index1 < this.m_ComponentInfo.Count; ++index1)
      {
        if ((this.m_ComponentInfo[index1].m_ComponentInfo.ComponentMask & mask) > AvatarComponentMasks.None)
        {
          for (uint index2 = 0; (long) index2 < (long) AvatarManifestV1.s_RequiredComponents.Length; ++index2)
          {
            if (this.m_ComponentInfo[index1].m_ComponentInfo.ComponentMask == AvatarManifestV1.s_RequiredComponents[(IntPtr) index2])
            {
              this.m_PreviousRequiredComponentInfo[(IntPtr) index2] = this.m_ComponentInfo[index1].m_ComponentInfo;
              break;
            }
          }
        }
      }
    }

    public override void ReplaceComponent(ComponentInfo newComponent)
    {
      this.ClearComponents(newComponent.m_ComponentMask);
      this.m_ComponentInfo.Add(new ComponentDescription(newComponent, Guid.Empty));
      this.m_Dirty = true;
    }

    public override AvatarGender BodyType
    {
      get
      {
        Guid assetId = this.m_BodyComponentInfo.m_ComponentInfo.m_AssetId;
        if (assetId == AvatarAssetsPack.MaleBody)
          return AvatarGender.Male;
        return assetId == AvatarAssetsPack.FemaleBody ? AvatarGender.Female : AvatarGender.Unknown;
      }
    }

    internal Vector4 GetDynamicColor(DynamicColorType type)
    {
      return Utilities.ColorbToVector4(this.m_DynamicColors[(int) type]);
    }

    internal void SetDynamicColor(DynamicColorType type, Colorb color)
    {
      this.m_DynamicColors[(int) type] = color;
    }

    internal bool GetRequiredComponentsPresent()
    {
      AvatarComponentMasks combinedComponentMask = this.GetCombinedComponentMask();
      for (int index = 0; index < AvatarManifestV1.s_RequiredComponents.Length; ++index)
      {
        if ((combinedComponentMask & AvatarManifestV1.s_RequiredComponents[index]) == AvatarComponentMasks.None)
          return false;
      }
      return true;
    }

    public override bool RemoveComponents(AvatarComponentMasks mask)
    {
      bool flag1 = true;
      AvatarGender bodyType = this.BodyType;
      int index1 = bodyType == AvatarGender.Male ? 0 : 1;
      if (bodyType == AvatarGender.Unknown)
        return false;
      this.m_Dirty = true;
      for (uint index2 = 0; (long) index2 < (long) AvatarManifestV1.s_RequiredComponents.Length; ++index2)
      {
        if ((mask & AvatarManifestV1.s_RequiredComponents[(IntPtr) index2]) != AvatarComponentMasks.None)
        {
          ComponentInfo componentInfo = this.GetComponentInfo(AvatarManifestV1.s_RequiredComponents[(IntPtr) index2], AssetMatchingMode.Exact);
          if (!componentInfo.IsEmpty)
          {
            bool flag2 = componentInfo.m_AssetId != AvatarManifestV1.s_DefaultRequiredComponents[index1, index2].m_AssetId && this.SetComponentInfo(AvatarManifestV1.s_DefaultRequiredComponents[index1, index2]);
            if (flag1 && !flag2)
              flag1 = flag2;
          }
        }
      }
      this.ClearComponents(mask);
      bool flag3 = this.EquipRequiredComponents(false);
      if (flag1 && !flag3)
        flag1 = flag3;
      return flag1;
    }

    internal bool GetRequiredBlendShapesPresent()
    {
      for (int index = 0; index < AvatarManifestV1.s_RequiredBlendShapes.Length; ++index)
      {
        if (Guid.Empty == this.m_BlendShapes[index].m_BlendShapeAssetId)
          return false;
      }
      return true;
    }

    internal bool GetRequiredReplacementTexturesPresent()
    {
      for (int index = 0; index < AvatarManifestV1.s_RequiredTextures.Length; ++index)
      {
        if (Guid.Empty == this.m_ReplacementTextures[(int) AvatarManifestV1.s_RequiredTextures[index]].m_TextureAssetId)
          return false;
      }
      return true;
    }

    public AvatarManifestV1.ReplacementTexture GetReplacementTexture(DynamicTextureType type)
    {
      return this.m_ReplacementTextures[(int) type];
    }

    internal void SetReplacementTexture(
      DynamicTextureType type,
      AvatarManifestV1.ReplacementTexture texture)
    {
      this.m_ReplacementTextures[(int) type] = texture;
    }

    public Guid GetBlendShape(BlendShapeType type)
    {
      return this.m_BlendShapes[(int) type].m_BlendShapeAssetId;
    }

    internal void SetBlendShape(BlendShapeType type, Guid asset)
    {
      this.m_BlendShapes[(int) type].m_BlendShapeAssetId = asset;
    }

    internal ComponentInfo GetComponentInfo(
      AvatarComponentMasks mask,
      AssetMatchingMode matchingMode)
    {
      int componentInfoCount = this.GetComponentInfoCount();
      switch (matchingMode)
      {
        case AssetMatchingMode.Exact:
          for (int index = 0; index < componentInfoCount; ++index)
          {
            if (this.m_ComponentInfo[index].m_ComponentInfo.m_ComponentMask == mask)
              return this.m_ComponentInfo[index].m_ComponentInfo;
          }
          return ComponentInfo.Zero;
        case AssetMatchingMode.Any:
          for (int index = 0; index < componentInfoCount; ++index)
          {
            if ((this.m_ComponentInfo[index].m_ComponentInfo.m_ComponentMask & mask) != AvatarComponentMasks.None)
              return this.m_ComponentInfo[index].m_ComponentInfo;
          }
          return ComponentInfo.Zero;
        case AssetMatchingMode.All:
          for (int index = 0; index < componentInfoCount; ++index)
          {
            if ((this.m_ComponentInfo[index].m_ComponentInfo.m_ComponentMask & mask) == mask)
              return this.m_ComponentInfo[index].m_ComponentInfo;
          }
          return ComponentInfo.Zero;
        default:
          return ComponentInfo.Zero;
      }
    }

    internal int FindComponent(AvatarComponentMasks mask, AssetMatchingMode matchingMode)
    {
      int componentInfoCount = this.GetComponentInfoCount();
      switch (matchingMode)
      {
        case AssetMatchingMode.Exact:
          for (int index = 0; index < componentInfoCount; ++index)
          {
            if (this.m_ComponentInfo[index].m_ComponentInfo.m_ComponentMask == mask)
              return index;
          }
          return -1;
        case AssetMatchingMode.Any:
          for (int index = 0; index < componentInfoCount; ++index)
          {
            if ((this.m_ComponentInfo[index].m_ComponentInfo.m_ComponentMask & mask) != AvatarComponentMasks.None)
              return index;
          }
          return -1;
        case AssetMatchingMode.All:
          for (int index = 0; index < componentInfoCount; ++index)
          {
            if ((this.m_ComponentInfo[index].m_ComponentInfo.m_ComponentMask & mask) == mask)
              return index;
          }
          return -1;
        default:
          return -1;
      }
    }

    internal int GetComponentInfoCount() => this.m_ComponentInfo.Count;

    internal bool SetComponentInfo(ComponentInfo info)
    {
      this.m_Dirty = true;
      for (uint index = 0; (long) index < (long) AvatarManifestV1.s_RequiredComponents.Length; ++index)
      {
        if (info.m_ComponentMask == AvatarManifestV1.s_RequiredComponents[(IntPtr) index])
        {
          this.m_PreviousRequiredComponentInfo[(IntPtr) index] = info;
          break;
        }
      }
      this.ClearComponents(info.m_ComponentMask);
      this.m_ComponentInfo.Add(new ComponentDescription(info, Guid.Empty));
      return this.EquipRequiredComponents(false);
    }

    internal void ClearComponents(AvatarComponentMasks mask)
    {
      int index = 0;
      while (index < this.m_ComponentInfo.Count)
      {
        if ((this.m_ComponentInfo[index].m_ComponentInfo.m_ComponentMask & mask) != AvatarComponentMasks.None)
        {
          this.m_ComponentInfo.RemoveAt(index);
          this.m_Dirty = true;
        }
        else
          ++index;
      }
    }

    public override List<ComponentInfo> GetComponents(AvatarComponentMasks mask)
    {
      List<ComponentInfo> components = new List<ComponentInfo>();
      for (int index = 0; index < this.m_ComponentInfo.Count; ++index)
      {
        if ((mask & this.m_ComponentInfo[index].m_ComponentInfo.m_ComponentMask) != AvatarComponentMasks.None)
          components.Add(this.m_ComponentInfo[index].m_ComponentInfo);
      }
      return components;
    }

    internal bool EquipRequiredComponents(bool fUseDefaultsIfNecessary)
    {
      AvatarGender bodyType = this.BodyType;
      int index1 = bodyType == AvatarGender.Male ? 0 : 1;
      if (bodyType == AvatarGender.Unknown)
        return false;
      AvatarComponentMasks combinedComponentMask = this.GetCombinedComponentMask();
      for (int index2 = 0; index2 < AvatarManifestV1.s_RequiredComponents.Length; ++index2)
      {
        if ((AvatarManifestV1.s_RequiredComponents[index2] & combinedComponentMask) == AvatarComponentMasks.None)
        {
          ComponentDescription componentDescription = new ComponentDescription(this.m_PreviousRequiredComponentInfo[index2], Guid.Empty);
          if (fUseDefaultsIfNecessary && componentDescription.m_ComponentInfo.m_AssetId == Guid.Empty)
            componentDescription.m_ComponentInfo = AvatarManifestV1.s_DefaultRequiredComponents[index1, index2];
          if (componentDescription.m_ComponentInfo.AssetId != Guid.Empty)
            this.m_ComponentInfo.Add(componentDescription);
          this.m_Dirty = true;
        }
      }
      return true;
    }

    internal bool ReplaceMissingComponents(bool fUseDefaultsIfNecessary)
    {
      AvatarGender bodyType = this.BodyType;
      int index1 = bodyType == AvatarGender.Male ? 0 : 1;
      if (bodyType == AvatarGender.Unknown)
        return false;
      AvatarComponentMasks combinedComponentMask = this.GetCombinedComponentMask();
      for (int index2 = 0; index2 < AvatarManifestV1.s_RequiredComponents.Length; ++index2)
      {
        if ((AvatarManifestV1.s_RequiredComponents[index2] & combinedComponentMask) == AvatarComponentMasks.None)
        {
          ComponentDescription componentDescription = new ComponentDescription(this.m_PreviousRequiredComponentInfo[index2], Guid.Empty);
          this.m_PreviousRequiredComponentInfo[index2] = new ComponentInfo();
          if (fUseDefaultsIfNecessary && componentDescription.m_ComponentInfo.m_AssetId == Guid.Empty)
            componentDescription.m_ComponentInfo = AvatarManifestV1.s_DefaultRequiredComponents[index1, index2];
          if (componentDescription.m_ComponentInfo.AssetId != Guid.Empty)
            this.m_ComponentInfo.Add(componentDescription);
          this.m_Dirty = true;
        }
      }
      return true;
    }

    internal AvatarComponentMasks GetCombinedComponentMask()
    {
      AvatarComponentMasks combinedComponentMask = AvatarComponentMasks.None;
      for (int index = 0; index < this.m_ComponentInfo.Count; ++index)
        combinedComponentMask |= this.m_ComponentInfo[index].m_ComponentInfo.m_ComponentMask;
      return combinedComponentMask;
    }

    internal AvatarComponentMasks GetUserCombinedComponentMask(AvatarComponentMasks componentMask)
    {
      AvatarComponentMasks combinedComponentMask = AvatarComponentMasks.None;
      for (int index = 0; index < this.m_ComponentInfo.Count; ++index)
      {
        if ((this.m_ComponentInfo[index].m_ComponentInfo.m_ComponentMask & componentMask) != AvatarComponentMasks.None)
          combinedComponentMask |= this.m_ComponentInfo[index].m_ComponentInfo.m_ComponentMask;
      }
      return combinedComponentMask;
    }

    public override bool IsComponentPresent(AvatarComponentType componentId)
    {
      AvatarComponentMasks avatarComponentMasks = (AvatarComponentMasks) (1 << (int) (componentId & (AvatarComponentType) 31));
      for (int index = 0; index < this.m_ComponentInfo.Count; ++index)
      {
        if ((this.m_ComponentInfo[index].m_ComponentInfo.m_ComponentMask & avatarComponentMasks) != AvatarComponentMasks.None)
          return true;
      }
      return false;
    }

    public bool IsAssetPresent(Guid asset)
    {
      if (this.m_BodyComponentInfo.m_ComponentInfo.m_AssetId == asset || this.m_HeadComponentInfo.m_ComponentInfo.m_AssetId == asset)
        return true;
      int count = this.m_ComponentInfo.Count;
      for (int index = 0; index < count; ++index)
      {
        if (this.m_ComponentInfo[index].m_ComponentInfo.m_AssetId == asset)
          return true;
      }
      return false;
    }

    internal bool IsCoreAsset(Guid asset) => AvatarManifestV1.s_CoreAssets.Contains(asset);

    internal void RemoveAsset(Guid asset)
    {
      if (this.m_BodyComponentInfo.m_ComponentInfo.m_AssetId == asset)
      {
        this.m_BodyComponentInfo.m_ComponentInfo = new ComponentInfo();
        this.m_Dirty = true;
      }
      if (this.m_HeadComponentInfo.m_ComponentInfo.m_AssetId == asset)
      {
        this.m_HeadComponentInfo.m_ComponentInfo = new ComponentInfo();
        this.m_Dirty = true;
      }
      int count = this.m_ComponentInfo.Count;
      for (int index = 0; index < count; ++index)
      {
        if (this.m_ComponentInfo[index].m_ComponentInfo.m_AssetId == asset)
        {
          this.m_Dirty = true;
          this.m_ComponentInfo.RemoveAt(index);
          break;
        }
      }
      int length1 = this.m_PreviousRequiredComponentInfo.Length;
      for (int index = 0; index < length1; ++index)
      {
        if (this.m_PreviousRequiredComponentInfo[index].m_AssetId == asset)
        {
          this.m_PreviousRequiredComponentInfo[index] = new ComponentInfo();
          break;
        }
      }
      int length2 = this.m_BlendShapes.Length;
      for (int index = 0; index < length2; ++index)
      {
        if (this.m_BlendShapes[index].m_BlendShapeAssetId == asset)
        {
          this.m_Dirty = true;
          this.m_BlendShapes[index] = new AvatarManifestV1.BlendShape();
          break;
        }
      }
      int length3 = this.m_ReplacementTextures.Length;
      for (int index = 0; index < length3; ++index)
      {
        if (this.m_ReplacementTextures[index].m_TextureAssetId == asset)
        {
          this.m_Dirty = true;
          this.m_ReplacementTextures[index] = new AvatarManifestV1.ReplacementTexture();
          break;
        }
      }
    }

    private Guid ResolveDependencies(IDataManager downloadManager, Guid assetId)
    {
      AvatarAssetDependency dependentAssets = AvatarAssetsDependenciesResolver.GetDependentAssets(downloadManager, assetId);
      if (dependentAssets != null)
      {
        if (dependentAssets.m_ModifiedAssetList == null)
          return dependentAssets.m_DependentAssetId;
        int length = dependentAssets.m_ModifiedAssetList.Length;
        for (int index = 0; index < length; ++index)
        {
          if (this.IsAssetPresent(dependentAssets.m_ModifiedAssetList[index]))
            return dependentAssets.m_DependentAssetId;
        }
      }
      return Guid.Empty;
    }

    public override void UpdateDependencies(IDataManager downloadManager)
    {
      if (downloadManager == null)
        throw new ArgumentNullException("dataManager");
      this.m_BodyComponentInfo.m_OverrideAsset = this.ResolveDependencies(downloadManager, this.m_BodyComponentInfo.m_ComponentInfo.AssetId);
      this.m_HeadComponentInfo.m_OverrideAsset = this.ResolveDependencies(downloadManager, this.m_HeadComponentInfo.m_ComponentInfo.AssetId);
      int count = this.m_ComponentInfo.Count;
      for (int index = 0; index < count; ++index)
        this.m_ComponentInfo[index].m_OverrideAsset = this.ResolveDependencies(downloadManager, this.m_ComponentInfo[index].m_ComponentInfo.AssetId);
      int length = this.m_ReplacementTextures.Length;
      for (int index = 0; index < length; ++index)
        this.m_ReplacementTextures[index].m_LinkedAssetId = this.ResolveDependencies(downloadManager, this.m_ReplacementTextures[index].m_TextureAssetId);
    }

    private bool Validate()
    {
      switch (this.BodyType)
      {
        case AvatarGender.Male:
        case AvatarGender.Female:
          return true;
        default:
          return false;
      }
    }

    public static bool operator ==(AvatarManifestV1 a1, AvatarManifestV1 b1)
    {
      if ((object) a1 == (object) b1)
        return true;
      if (a1.Version != b1.Version || (double) a1.m_WeightFactor != (double) b1.m_WeightFactor || (double) a1.m_HeightFactor != (double) b1.m_HeightFactor || a1.m_DynamicColors.Length != b1.m_DynamicColors.Length)
        return false;
      for (int index = 0; index < a1.m_DynamicColors.Length; ++index)
      {
        if (!(a1.m_DynamicColors[index] == b1.m_DynamicColors[index]))
          return false;
      }
      if (a1.m_ReplacementTextures.Length != b1.m_ReplacementTextures.Length)
        return false;
      for (int index = 0; index < a1.m_ReplacementTextures.Length; ++index)
      {
        if (a1.m_ReplacementTextures[index].m_LinkedAssetId != b1.m_ReplacementTextures[index].m_LinkedAssetId)
          return false;
        float num = 1E-06f;
        if ((double) Math.Abs(a1.m_ReplacementTextures[index].m_Placement.m_Rotation - b1.m_ReplacementTextures[index].m_Placement.m_Rotation) > (double) num || (double) Math.Abs(a1.m_ReplacementTextures[index].m_Placement.m_Scale - b1.m_ReplacementTextures[index].m_Placement.m_Scale) > (double) num || (double) Math.Abs(a1.m_ReplacementTextures[index].m_Placement.m_TranslationU - b1.m_ReplacementTextures[index].m_Placement.m_TranslationU) > (double) num || (double) Math.Abs(a1.m_ReplacementTextures[index].m_Placement.m_TranslationV - b1.m_ReplacementTextures[index].m_Placement.m_TranslationV) > (double) num || a1.m_ReplacementTextures[index].m_TextureAssetId != b1.m_ReplacementTextures[index].m_TextureAssetId)
          return false;
      }
      if (a1.m_PreviousRequiredComponentInfo.Length != b1.m_PreviousRequiredComponentInfo.Length)
        return false;
      for (int index = 0; index < a1.m_PreviousRequiredComponentInfo.Length; ++index)
      {
        if (a1.m_PreviousRequiredComponentInfo[index] != b1.m_PreviousRequiredComponentInfo[index])
          return false;
      }
      if (a1.m_HeadComponentInfo != b1.m_HeadComponentInfo || a1.m_BodyComponentInfo != b1.m_BodyComponentInfo || a1.m_ComponentInfo.Count != b1.m_ComponentInfo.Count)
        return false;
      for (int index1 = 0; index1 < b1.m_ComponentInfo.Count; ++index1)
      {
        int index2 = 0;
        while (index2 < b1.m_ComponentInfo.Count && !(a1.m_ComponentInfo[index2] == b1.m_ComponentInfo[index1]))
          ++index2;
        if (index2 == b1.m_ComponentInfo.Count)
          return false;
      }
      if (a1.m_BlendShapes.Length != b1.m_BlendShapes.Length)
        return false;
      for (int index = 0; index < a1.m_BlendShapes.Length; ++index)
      {
        if (a1.m_BlendShapes[index].m_BlendShapeAssetId != b1.m_BlendShapes[index].m_BlendShapeAssetId)
          return false;
      }
      return true;
    }

    public static bool operator !=(AvatarManifestV1 a, AvatarManifestV1 b) => !(a == b);

    public override bool Equals(object obj)
    {
      return (object) (obj as AvatarManifestV1) != null && this == (AvatarManifestV1) obj;
    }

    public override int GetHashCode()
    {
      int num = this.m_OwnerXuid.GetHashCode() + this.m_HeadComponentInfo.GetHashCode() + this.m_BodyComponentInfo.GetHashCode();
      for (int index = 0; index < this.m_ComponentInfo.Count; ++index)
        num += this.m_ComponentInfo[index].GetHashCode();
      for (int index = 0; index < this.m_PreviousRequiredComponentInfo.Length; ++index)
        num += this.m_PreviousRequiredComponentInfo[index].GetHashCode();
      for (int index = 0; index < this.m_ReplacementTextures.Length; ++index)
        num += this.m_ReplacementTextures[index].GetHashCode();
      for (int index = 0; index < this.m_DynamicColors.Length; ++index)
        num += this.m_DynamicColors[index].GetHashCode();
      for (int index = 0; index < this.m_BlendShapes.Length; ++index)
        num += this.m_BlendShapes[index].m_BlendShapeAssetId.GetHashCode();
      return num + ((int) ((double) this.m_WeightFactor * 32.0) + (int) ((double) this.m_HeightFactor * 32.0));
    }

    internal struct TexturePlacement
    {
      internal float m_Scale;
      internal float m_Rotation;
      internal float m_TranslationU;
      internal float m_TranslationV;
    }

    public struct ReplacementTexture
    {
      public Guid m_TextureAssetId;
      public Guid m_LinkedAssetId;
      internal AvatarManifestV1.TexturePlacement m_Placement;
    }

    internal struct BlendShape
    {
      internal Guid m_BlendShapeAssetId;
    }
  }
}
