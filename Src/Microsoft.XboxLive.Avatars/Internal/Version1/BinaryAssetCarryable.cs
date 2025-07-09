// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.BinaryAssetCarryable
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class BinaryAssetCarryable : BinaryAssetModel
  {
    public BinaryAssetCarryable(
      ComponentInfo description,
      int shaderOverridesCount,
      AvatarComponentMasks mask)
      : base(description, shaderOverridesCount, mask)
    {
      this.m_AssetType = BinaryAssetParserType.Carryable;
    }

    public override bool ProcessComponentsFromStream(BinaryAssetParseContext context)
    {
      StructuredBinary structuredBinary = new StructuredBinary();
      this.m_Stream.Seek(0L, SeekOrigin.Begin);
      if (!structuredBinary.Open(this.m_Stream) || !(structuredBinary.Namespace == BinaryAsset.AvatarAssetGuid))
        return false;
      BlockIterator iterator = structuredBinary.Iterator;
      if (!iterator.FindFirst(StructuredBinaryBlockId.Model))
        return false;
      Avatar target = context.m_Target;
      target.m_Carryable = new AvatarCarryable();
      target.m_Carryable.m_ComponentModel.m_AvatarComponentManifest = new ComponentManifest(this.m_ComponentDescription, this.m_ShaderConstantOverrides, 0);
      this.ProcessModel(iterator, ref target.m_Carryable.m_ComponentModel, context);
      if (!iterator.FindFirst(StructuredBinaryBlockId.Skeleton))
        return false;
      if ((int) iterator.Length <= 0)
      {
        Logger.Log((Log) new DebugLog((object) this, "block size <= 0; strb file is corrupted"));
        return false;
      }
      DecompressStream decompressStream = new DecompressStream((Stream) iterator, (int) iterator.Length);
      AssetSkeletonParser assetSkeletonParser = new AssetSkeletonParser(context.m_CoordinateSystem);
      target.m_Carryable.m_Skeleton = assetSkeletonParser.Parse((Stream) decompressStream);
      if (iterator.FindFirst(StructuredBinaryBlockId.Animation))
      {
        AssetAnimationParser assetAnimationParser = new AssetAnimationParser(context.m_CoordinateSystem, AvatarGender.Both);
        target.m_Carryable.m_Animation = assetAnimationParser.Parse((Stream) iterator);
      }
      this.m_ContainShapeOverrides = iterator.FindFirst(StructuredBinaryBlockId.ShapeOverrides);
      return true;
    }

    public override bool ProcessComponentsFromCache(BinaryAssetParseContext context)
    {
      if (!(this.m_Cache is CachedBinaryAssetCarryable cache))
        return false;
      this.m_SkeletonVersion = this.m_Cache.m_Metadata.m_skeletonVersion;
      Avatar target = context.m_Target;
      AvatarCarryable avatarCarryable = new AvatarCarryable()
      {
        m_Animation = cache.m_CarryableAnimation,
        m_Skeleton = new Skeleton()
      };
      avatarCarryable.m_Skeleton.Joints = new Joint[cache.m_CarryableSkeleton.Joints.Length];
      cache.m_CarryableSkeleton.Joints.CopyTo((Array) avatarCarryable.m_Skeleton.Joints, 0);
      avatarCarryable.m_ComponentModel.m_AvatarComponentManifest = new ComponentManifest(this.m_ComponentDescription, this.m_ShaderConstantOverrides, 0);
      target.m_Carryable = avatarCarryable;
      if (!this.ProcessModel(cache.Models[0], cache.ColorTable, ref avatarCarryable.m_ComponentModel))
        return false;
      this.m_ContainShapeOverrides = cache.m_ShapeOverrides.Count != 0;
      return true;
    }

    public override CachedBinaryAsset CreateCacheItem()
    {
      return (CachedBinaryAsset) new CachedBinaryAssetCarryable(this.m_AssetId);
    }
  }
}
