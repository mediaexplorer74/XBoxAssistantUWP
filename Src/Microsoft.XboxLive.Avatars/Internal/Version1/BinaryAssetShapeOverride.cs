// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.BinaryAssetShapeOverride
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Parsers;
using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class BinaryAssetShapeOverride : BinaryAsset
  {
    public BinaryAssetShapeOverride(Guid id, AvatarComponentMasks mask)
      : base(mask)
    {
      this.m_AssetId = id;
      this.m_AssetType = BinaryAssetParserType.ShapeOverride;
    }

    public override bool ProcessOverridesFromStream(BinaryAssetParseContext context)
    {
      StructuredBinary structuredBinary = new StructuredBinary();
      this.m_Stream.Seek(0L, SeekOrigin.Begin);
      if (!structuredBinary.Open(this.m_Stream) || !(structuredBinary.Namespace == BinaryAsset.AvatarAssetGuid))
        return false;
      BlockIterator iterator = structuredBinary.Iterator;
      while (iterator.Find(StructuredBinaryBlockId.ShapeOverrides))
      {
        if (!this.ProcessShapeOverride(context, (Stream) iterator))
          return false;
        iterator.NextBlock();
      }
      return true;
    }

    public override bool ProcessOverridesFromCache(BinaryAssetParseContext context)
    {
      if (!(this.m_Cache is CachedBinaryAssetShapeOverride cache))
        return false;
      this.m_SkeletonVersion = this.m_Cache.m_Metadata.m_skeletonVersion;
      int count1 = cache.m_ShapeOverrides.Count;
      for (int index = 0; index < count1; ++index)
      {
        AssetShapeOverrideParser shapeOverride = cache.m_ShapeOverrides[index];
        Guid targetAssetId = shapeOverride.GetTargetAssetId();
        Avatar target = context.m_Target;
        int count2 = target.m_Models.Count;
        while (--count2 >= 0)
        {
          if (target.m_Models[count2].AssetId == targetAssetId)
          {
            if (!shapeOverride.Apply(target.m_Models[count2]))
              return false;
            target.m_Models[count2].m_AvatarComponentManifest.AddAsset(this.m_AssetId);
          }
        }
      }
      return true;
    }

    public override bool Validate(BinaryAssetParseContext context)
    {
      AssetMetadataParser metadata = this.GetMetadata();
      if (metadata == null)
        return false;
      if (context.m_skeletonVersion == metadata.AssetSkeletonVersion)
        return true;
      Logger.Log((Log) new DebugLog((object) this, string.Format("Incompatible skeleton version. Required {0}, received {1} for component {2}", (object) context.m_skeletonVersion, (object) metadata.AssetSkeletonVersion, (object) this.m_AssetId)));
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

    internal bool ProcessShapeOverride(BinaryAssetParseContext context, Stream stream)
    {
      AssetShapeOverrideParser shapeOverrideParser = new AssetShapeOverrideParser(context.m_CoordinateSystem);
      shapeOverrideParser.Parse(stream);
      Guid targetAssetId = shapeOverrideParser.GetTargetAssetId();
      Avatar target = context.m_Target;
      int count = target.Models.Count;
      while (--count >= 0)
      {
        if (target.Models[count].AssetId == targetAssetId)
        {
          if (!shapeOverrideParser.Apply(target.Models[count]))
            return false;
          target.m_Models[count].m_AvatarComponentManifest.AddAsset(this.m_AssetId);
        }
      }
      return true;
    }

    public override CachedBinaryAsset CreateCacheItem()
    {
      return (CachedBinaryAsset) new CachedBinaryAssetShapeOverride(this.m_AssetId);
    }
  }
}
