// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.BinaryAssetAnimation
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Parsers;
using System;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class BinaryAssetAnimation : BinaryAsset
  {
    public BinaryAssetAnimation(Guid animationAssetId)
      : base(AvatarComponentMasks.None)
    {
      this.m_AssetId = animationAssetId;
      this.m_AssetType = BinaryAssetParserType.Animation;
    }

    public override bool Validate(BinaryAssetParseContext context)
    {
      AssetMetadataParser metadata = this.GetMetadata();
      if (metadata == null)
        return false;
      if (context.m_skeletonVersion == metadata.AssetSkeletonVersion)
        return true;
      Logger.Log((Log) new DebugLog((object) this, string.Format("Incompatible skeleton version in animation asset. Required {0}, received {1} for component {2}", (object) context.m_skeletonVersion, (object) metadata.AssetSkeletonVersion, (object) this.m_AssetId)));
      return false;
    }

    public override bool ValidateFromCache(BinaryAssetParseContext context)
    {
      if (this.m_Cache == null || this.m_Cache.m_Metadata == null)
        return false;
      if (context.m_skeletonVersion == this.m_Cache.m_Metadata.m_skeletonVersion)
        return true;
      Logger.Log((Log) new DebugLog((object) this, string.Format("Incompatible skeleton version in animation asset. Required {0}, received {1} for component {2}", (object) context.m_skeletonVersion, (object) this.m_Cache.m_Metadata.m_skeletonVersion, (object) this.m_AssetId)));
      return false;
    }

    public override CachedBinaryAsset CreateCacheItem()
    {
      return (CachedBinaryAsset) new CachedBinaryAssetAnimation(this.m_AssetId);
    }
  }
}
