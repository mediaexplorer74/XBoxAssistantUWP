// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.BinaryAssetModel
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using Microsoft.XboxLive.MathUtilities;
using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class BinaryAssetModel : BinaryAssetShapeOverride
  {
    public ShaderConstantOverride[] m_ShaderConstantOverrides;
    public ComponentInfo m_ComponentDescription;
    public bool m_ContainShapeOverrides;

    public ShaderConstantOverride[] ShaderConstantOverrides => this.m_ShaderConstantOverrides;

    public BinaryAssetModel(
      ComponentInfo description,
      int shaderOverridesCount,
      AvatarComponentMasks mask)
      : base(description.m_AssetId, mask)
    {
      this.m_ComponentDescription = description;
      this.m_AssetType = BinaryAssetParserType.Model;
      this.m_ShaderConstantOverrides = new ShaderConstantOverride[shaderOverridesCount];
    }

    public void SetAssetId(Guid id)
    {
      this.m_AssetId = id;
      this.m_ComponentDescription.m_AssetId = id;
    }

    public override bool Validate(BinaryAssetParseContext context)
    {
      AssetMetadataParser metadata = this.GetMetadata();
      if (metadata == null)
      {
        Logger.Log((Log) new DebugLog((object) this, string.Format("Missing metadata in {0}", (object) this.m_AssetId)));
        return false;
      }
      if ((metadata.BodyTypeMask & context.m_BodyType) == AvatarGender.Unknown)
      {
        Logger.Log((Log) new DebugLog((object) this, string.Format("Gender mismatch in {0}", (object) this.m_AssetId)));
        return false;
      }
      if (metadata.AssetType != BinaryAssetType.Component)
      {
        Logger.Log((Log) new DebugLog((object) this, string.Format("Asset type mismatch in {0}", (object) this.m_AssetId)));
        return false;
      }
      if (this.m_ComponentMask != AvatarComponentMasks.None)
      {
        AvatarComponentMasks avatarComponentMasks = (AvatarComponentMasks) ((int) metadata.AssetTypeDetails & -8388609);
        if (avatarComponentMasks != this.m_ComponentMask)
        {
          Logger.Log((Log) new DebugLog((object) this, string.Format("Invalid component category mask in asset {0}. Mask {1} expected and {2} get", (object) this.m_AssetId, (object) this.m_ComponentMask, (object) avatarComponentMasks)));
          return false;
        }
      }
      if (context.m_skeletonVersion == metadata.AssetSkeletonVersion)
        return true;
      Logger.Log((Log) new DebugLog((object) this, string.Format("Skeleton version does not match. Required {0}, received {1} for component {2}", (object) context.m_skeletonVersion, (object) metadata.AssetSkeletonVersion, (object) this.m_AssetId)));
      return false;
    }

    public override bool ValidateFromCache(BinaryAssetParseContext context)
    {
      if (this.m_Cache == null)
        return false;
      if (this.m_Cache.m_Metadata == null)
      {
        Logger.Log((Log) new DebugLog((object) this, string.Format("Missing metadata in {0}", (object) this.m_AssetId)));
        return false;
      }
      if ((this.m_Cache.m_Metadata.BodyTypeMask & context.m_BodyType) == AvatarGender.Unknown)
      {
        Logger.Log((Log) new DebugLog((object) this, string.Format("Gender mismatch in {0}", (object) this.m_AssetId)));
        return false;
      }
      if (this.m_Cache.m_Metadata.AssetType != BinaryAssetType.Component)
      {
        Logger.Log((Log) new DebugLog((object) this, string.Format("Asset type mismatch in {0}", (object) this.m_AssetId)));
        return false;
      }
      if (this.m_ComponentMask != AvatarComponentMasks.None)
      {
        AvatarComponentMasks avatarComponentMasks = (AvatarComponentMasks) ((int) this.m_Cache.m_Metadata.AssetTypeDetails & -8388609);
        if (avatarComponentMasks != this.m_ComponentMask)
        {
          Logger.Log((Log) new DebugLog((object) this, string.Format("Invalid component category mask in asset {0}. Mask {1} expected and {2} get", (object) this.m_AssetId, (object) this.m_ComponentMask, (object) avatarComponentMasks)));
          return false;
        }
      }
      if (context.m_skeletonVersion == this.m_Cache.m_Metadata.m_skeletonVersion)
        return true;
      Logger.Log((Log) new DebugLog((object) this, string.Format("Skeleton version does not match. Required {0}, received {1} for component {2}", (object) context.m_skeletonVersion, (object) this.m_Cache.m_Metadata.m_skeletonVersion, (object) this.m_AssetId)));
      return false;
    }

    public override bool ProcessOverridesFromStream(BinaryAssetParseContext context)
    {
      return !this.m_ContainShapeOverrides || base.ProcessOverridesFromStream(context);
    }

    public override bool ProcessOverridesFromCache(BinaryAssetParseContext context)
    {
      return !this.m_ContainShapeOverrides || base.ProcessOverridesFromCache(context);
    }

    public override bool ProcessComponentsFromStream(BinaryAssetParseContext context)
    {
      if (this.GetMetadata() == null)
        return false;
      StructuredBinary structuredBinary = new StructuredBinary();
      this.m_Stream.Seek(0L, SeekOrigin.Begin);
      if (!structuredBinary.Open(this.m_Stream) || !(structuredBinary.Namespace == BinaryAsset.AvatarAssetGuid))
        return false;
      BlockIterator iterator = structuredBinary.Iterator;
      if (!iterator.FindFirst(StructuredBinaryBlockId.Model))
        return false;
      int modelIndex = 0;
      if ((context.m_CombinedComponentMask & AvatarComponentMasks.Hat) != AvatarComponentMasks.None && this.m_ComponentDescription.m_ComponentMask == AvatarComponentMasks.Hair)
      {
        iterator.NextBlock();
        if (!iterator.Find(StructuredBinaryBlockId.Model))
          iterator.FindFirst(StructuredBinaryBlockId.Model);
        else
          modelIndex = 1;
      }
      AvatarComponent model = new AvatarComponent();
      model.m_AvatarComponentManifest = new ComponentManifest(this.m_ComponentDescription, this.m_ShaderConstantOverrides, modelIndex);
      this.ProcessModel(iterator, ref model, context);
      iterator.FirstBlock();
      this.m_ContainShapeOverrides = iterator.FindFirst(StructuredBinaryBlockId.ShapeOverrides);
      context.m_Target.AddComponent(model);
      return true;
    }

    protected void ProcessModel(
      BlockIterator iterator,
      ref AvatarComponent model,
      BinaryAssetParseContext context)
    {
      Vector4[] customColors = new Vector4[3];
      AssetModelParser assetModelParser = new AssetModelParser(model, context.m_CoordinateSystem);
      if ((int) iterator.Length <= 0)
      {
        Logger.Log((Log) new DebugLog((object) this, "block size <= 0; strb file is corrupted"));
        throw new AvatarException(Resources.InvalidStrbFileText);
      }
      DecompressStream stream = new DecompressStream((Stream) iterator, (int) iterator.Length);
      assetModelParser.Parse((EndianStream) stream, context.m_ResourceFactory);
      int length = this.m_ShaderConstantOverrides.Length;
      while (--length >= 0)
      {
        ShaderConstantOverride constantOverride = this.m_ShaderConstantOverrides[length];
        ShaderParameterData paramData = new ShaderParameterData();
        paramData.Constant.SetValue(constantOverride.m_Value);
        BinaryAssetModel.OverrideShaderParameter(model, constantOverride.m_Constant, paramData);
        if (constantOverride.m_Constant >= ShaderParameterUsage.PixelConstantColorCustom0 && constantOverride.m_Constant <= ShaderParameterUsage.PixelConstantColorCustom2)
          customColors[(int) (constantOverride.m_Constant - 22)] = constantOverride.m_Value;
      }
      if (!iterator.FindFirst(StructuredBinaryBlockId.CustomColorTable))
        return;
      ComponentColorTable colorTable = AssetCustomColorTableParser.Parse(iterator);
      BinaryAssetModel.ApplyColorTable(model, customColors, colorTable);
    }

    private static void OverrideShaderParameter(
      AvatarComponent model,
      ShaderParameterUsage paramUsage,
      ShaderParameterData paramData)
    {
      if (paramUsage == ShaderParameterUsage.None)
        return;
      int length1 = model.m_Batches.Length;
      while (--length1 >= 0)
      {
        ShaderInstance shaderInstance = model.m_ShaderInstance[length1];
        int length2 = shaderInstance.ShaderParameters.Length;
        while (--length2 >= 0)
        {
          if (shaderInstance.ShaderParameters[length2].usage == paramUsage)
            shaderInstance.ShaderParameters[length2].data = paramData;
        }
      }
    }

    public override bool ProcessComponentsFromCache(BinaryAssetParseContext context)
    {
      if (!(this.m_Cache is CachedBinaryAssetModel cache))
        return false;
      this.m_SkeletonVersion = this.m_Cache.m_Metadata.m_skeletonVersion;
      int count = cache.Models.Count;
      if (count == 0)
        return false;
      int num = 0;
      if ((context.m_CombinedComponentMask & AvatarComponentMasks.Hat) != AvatarComponentMasks.None && this.m_ComponentDescription.ComponentMask == AvatarComponentMasks.Hair && count > 1)
        num = 1;
      AvatarComponent model = new AvatarComponent();
      model.m_AvatarComponentManifest = new ComponentManifest(this.m_ComponentDescription, this.m_ShaderConstantOverrides, num);
      if (!this.ProcessModel(cache.Models[num], cache.ColorTable, ref model))
        return false;
      context.m_Target.AddComponent(model);
      this.m_ContainShapeOverrides = cache.m_ShapeOverrides.Count > 0;
      return true;
    }

    protected bool ProcessModel(
      AvatarComponent cachedModel,
      ComponentColorTable cachedColorTable,
      ref AvatarComponent model)
    {
      int length1 = cachedModel.m_Textures.Length;
      model.m_Textures = new IBaseTextureAnimated[length1];
      for (int index = 0; index < length1; ++index)
        model.m_Textures[index] = cachedModel.m_Textures[index];
      int length2 = cachedModel.m_Batches.Length;
      model.m_Batches = new TriangleBatch[length2];
      for (int index = 0; index < length2; ++index)
        model.m_Batches[index] = cachedModel.m_Batches[index].Clone();
      int length3 = cachedModel.m_ShaderInstance.Length;
      model.m_ShaderInstance = new ShaderInstance[length3];
      for (int index = 0; index < length3; ++index)
        model.m_ShaderInstance[index] = cachedModel.m_ShaderInstance[index].Clone();
      Vector4[] customColors = new Vector4[3];
      int length4 = this.m_ShaderConstantOverrides.Length;
      while (--length4 >= 0)
      {
        ShaderConstantOverride constantOverride = this.m_ShaderConstantOverrides[length4];
        ShaderParameterData paramData = new ShaderParameterData();
        paramData.Constant.SetValue(constantOverride.m_Value);
        BinaryAssetModel.OverrideShaderParameter(model, constantOverride.m_Constant, paramData);
        if (constantOverride.m_Constant >= ShaderParameterUsage.PixelConstantColorCustom0 && constantOverride.m_Constant <= ShaderParameterUsage.PixelConstantColorCustom2)
          customColors[(int) (constantOverride.m_Constant - 22)] = constantOverride.m_Value;
      }
      BinaryAssetModel.ApplyColorTable(model, customColors, cachedColorTable);
      return true;
    }

    private static void ApplyColorTable(
      AvatarComponent model,
      Vector4[] customColors,
      ComponentColorTable colorTable)
    {
      if (colorTable == null)
        return;
      bool flag = false;
      int length = colorTable.Colors.Length;
      while (--length >= 0)
      {
        Vector4 customColor0 = colorTable.Colors[length].CustomColor0;
        Vector4 customColor1 = colorTable.Colors[length].CustomColor1;
        Vector4 customColor2 = colorTable.Colors[length].CustomColor2;
        if ((double) Vector4.DistanceSquared(customColors[0], customColor0) + (double) Vector4.DistanceSquared(customColors[1], customColor1) + (double) Vector4.DistanceSquared(customColors[2], customColor2) < 9.9999997473787516E-06)
        {
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      ShaderParameterData paramData = new ShaderParameterData();
      paramData.Constant.SetValue(colorTable.Colors[0].CustomColor0);
      BinaryAssetModel.OverrideShaderParameter(model, ShaderParameterUsage.PixelConstantColorCustom0, paramData);
      paramData.Constant.SetValue(colorTable.Colors[0].CustomColor1);
      BinaryAssetModel.OverrideShaderParameter(model, ShaderParameterUsage.PixelConstantColorCustom1, paramData);
      paramData.Constant.SetValue(colorTable.Colors[0].CustomColor2);
      BinaryAssetModel.OverrideShaderParameter(model, ShaderParameterUsage.PixelConstantColorCustom2, paramData);
    }

    public override CachedBinaryAsset CreateCacheItem()
    {
      return (CachedBinaryAsset) new CachedBinaryAssetModel(this.m_AssetId);
    }
  }
}
