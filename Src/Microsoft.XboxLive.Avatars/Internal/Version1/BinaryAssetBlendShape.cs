// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.BinaryAssetBlendShape
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Parsers;
using System;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class BinaryAssetBlendShape : BinaryAssetShapeOverride
  {
    private BlendShapeType m_Shape;

    public BinaryAssetBlendShape(BlendShapeType shapeId, Guid id)
      : base(id, AvatarComponentMasks.None)
    {
      this.m_Shape = shapeId;
      this.m_AssetType = BinaryAssetParserType.BlendShape;
    }

    public override bool Validate(BinaryAssetParseContext context)
    {
      AssetMetadataParser metadata = this.GetMetadata();
      if (metadata == null || (metadata.BodyTypeMask & context.m_BodyType) == AvatarGender.Unknown || metadata.AssetType != BinaryAssetType.ShapeOverride || (BlendShapeType) metadata.AssetTypeDetails != this.m_Shape)
        return false;
      if (context.m_skeletonVersion == metadata.AssetSkeletonVersion)
        return true;
      Logger.Log((Log) new DebugLog((object) this, string.Format("Incompatible skeleton version. Required {0}, received {1} for component {2}", (object) context.m_skeletonVersion, (object) metadata.AssetSkeletonVersion, (object) this.m_AssetId)));
      return false;
    }

    public override bool ProcessOverridesFromStream(BinaryAssetParseContext context) => true;

    public override bool ProcessOverridesFromCache(BinaryAssetParseContext context) => true;

    public override bool ProcessAssetsFromStream(BinaryAssetParseContext context)
    {
      return base.ProcessOverridesFromStream(context);
    }

    public override bool ProcessAssetsFromCache(BinaryAssetParseContext context)
    {
      return base.ProcessOverridesFromCache(context);
    }

    public override CachedBinaryAsset CreateCacheItem()
    {
      return (CachedBinaryAsset) new CachedBinaryAssetBlendShape(this.m_AssetId);
    }
  }
}
