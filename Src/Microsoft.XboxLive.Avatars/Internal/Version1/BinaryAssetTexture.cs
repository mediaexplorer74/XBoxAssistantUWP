// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.BinaryAssetTexture
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using System;
using System.Collections;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class BinaryAssetTexture : BinaryAsset
  {
    private const int MaxTexturesPerModel = 18;
    internal static ShaderParameterUsage[] MANIFEST_TEXTURE_TO_PARAM_USAGE_MAP = new ShaderParameterUsage[18]
    {
      ShaderParameterUsage.TextureMouth,
      ShaderParameterUsage.None,
      ShaderParameterUsage.None,
      ShaderParameterUsage.TextureEyeLeft,
      ShaderParameterUsage.TextureEyeRight,
      ShaderParameterUsage.None,
      ShaderParameterUsage.TextureEyebrowLeft,
      ShaderParameterUsage.TextureEyebrowRight,
      ShaderParameterUsage.None,
      ShaderParameterUsage.TextureFacialHair,
      ShaderParameterUsage.None,
      ShaderParameterUsage.None,
      ShaderParameterUsage.TextureEyeShadow,
      ShaderParameterUsage.None,
      ShaderParameterUsage.None,
      ShaderParameterUsage.TextureSkinFeatures,
      ShaderParameterUsage.None,
      ShaderParameterUsage.None
    };
    public DynamicTextureType m_Texture;

    public BinaryAssetTexture(DynamicTextureType texture, Guid id)
      : base(AvatarComponentMasks.None)
    {
      this.m_AssetId = id;
      this.m_Texture = texture;
      this.m_AssetType = BinaryAssetParserType.Texture;
    }

    public override bool ProcessAssetsFromStream(BinaryAssetParseContext context)
    {
      StructuredBinary structuredBinary = new StructuredBinary();
      this.m_Stream.Seek(0L, SeekOrigin.Begin);
      if (!structuredBinary.Open(this.m_Stream) || !(structuredBinary.Namespace == BinaryAsset.AvatarAssetGuid))
        return false;
      BlockIterator iterator = structuredBinary.Iterator;
      if (!iterator.Find(StructuredBinaryBlockId.Texture))
        return false;
      if ((int) iterator.Length <= 0)
      {
        Logger.Log((Log) new DebugLog((object) this, "block size <= 0; strb file is corrupted"));
        return false;
      }
      DecompressStream stream = new DecompressStream((Stream) iterator, (int) iterator.Length);
      bool flag1 = false;
      AssetTextureParser assetTextureParser = new AssetTextureParser();
      Avatar target = context.m_Target;
      int count = target.Models.Count;
      while (--count >= 0)
      {
        AvatarComponent model = target.Models[count];
        bool flag2 = false;
        BitArray bitArray = new BitArray(18);
        int texture = (int) this.m_Texture;
        ShaderParameterUsage[] textureToParamUsageMap = BinaryAssetTexture.MANIFEST_TEXTURE_TO_PARAM_USAGE_MAP;
        for (int index = 0; index < 3; ++index)
        {
          ShaderParameterUsage shaderParameterUsage = textureToParamUsageMap[texture * 3 + index];
          int length1 = model.m_Batches.Length;
          while (--length1 >= 0)
          {
            ShaderInstance shaderInstance = model.m_ShaderInstance[length1];
            int length2 = shaderInstance.ShaderParameters.Length;
            while (--length2 >= 0)
            {
              ShaderParameter shaderParameter = shaderInstance.ShaderParameters[length2];
              if (shaderParameter.usage == shaderParameterUsage && !bitArray.Get((int) shaderParameter.data.Texture.TextureIndex))
              {
                bitArray.Set((int) shaderParameter.data.Texture.TextureIndex, true);
                if (!flag1)
                {
                  flag1 = true;
                  assetTextureParser.Parse((EndianStream) stream, context.m_ResourceFactory);
                }
                model.m_Textures[(int) shaderParameter.data.Texture.TextureIndex] = assetTextureParser.Texture;
                if (!flag2)
                {
                  model.m_AvatarComponentManifest.AddAsset(this.m_AssetId);
                  flag2 = true;
                }
              }
            }
          }
        }
      }
      return true;
    }

    public override bool Validate(BinaryAssetParseContext context)
    {
      AssetMetadataParser metadata = this.GetMetadata();
      return metadata != null && (metadata.BodyTypeMask & context.m_BodyType) != AvatarGender.Unknown && metadata.AssetType == BinaryAssetType.Texture && (DynamicTextureType) metadata.AssetTypeDetails == this.m_Texture;
    }

    public override bool ValidateFromCache(BinaryAssetParseContext context)
    {
      return this.m_Cache != null && this.m_Cache.m_Metadata != null && (this.m_Cache.m_Metadata.BodyTypeMask & context.m_BodyType) != AvatarGender.Unknown && this.m_Cache.m_Metadata.AssetType == BinaryAssetType.Texture && (DynamicTextureType) this.m_Cache.m_Metadata.AssetTypeDetails == this.m_Texture;
    }

    public override bool ProcessAssetsFromCache(BinaryAssetParseContext context)
    {
      if (!(this.m_Cache is CachedBinaryAssetTexture cache))
        return false;
      this.m_SkeletonVersion = this.m_Cache.m_Metadata.m_skeletonVersion;
      Avatar target = context.m_Target;
      int count = target.m_Models.Count;
      while (--count >= 0)
      {
        AvatarComponent model = target.m_Models[count];
        bool flag = false;
        int num = 0;
        int texture = (int) this.m_Texture;
        for (int index = 0; index < 3; ++index)
        {
          ShaderParameterUsage textureToParamUsage = BinaryAssetTexture.MANIFEST_TEXTURE_TO_PARAM_USAGE_MAP[texture * 3 + index];
          int length1 = model.m_Batches.Length;
          while (--length1 >= 0)
          {
            ShaderInstance[] shaderInstance = model.m_ShaderInstance;
            int length2 = shaderInstance[length1].ShaderParameters.Length;
            while (--length2 >= 0)
            {
              ShaderParameter shaderParameter = shaderInstance[length1].ShaderParameters[length2];
              if (shaderParameter.usage == textureToParamUsage && (num & 1 << (int) shaderParameter.data.Texture.TextureIndex) == 0)
              {
                num |= 1 << (int) shaderParameter.data.Texture.TextureIndex;
                model.m_Textures[(int) shaderParameter.data.Texture.TextureIndex] = cache.GetTexture();
                if (!flag)
                {
                  model.m_AvatarComponentManifest.AddAsset(this.m_AssetId);
                  flag = true;
                }
              }
            }
          }
        }
      }
      return true;
    }

    public override CachedBinaryAsset CreateCacheItem()
    {
      return (CachedBinaryAsset) new CachedBinaryAssetTexture(this.m_AssetId);
    }

    public override bool IsCoordinateSystemIndependent => true;
  }
}
