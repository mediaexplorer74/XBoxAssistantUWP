// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.BinaryAsset
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal abstract class BinaryAsset
  {
    public const long AvatarAssetVersion = 265023668400864;
    public static readonly Guid AvatarAssetGuid = new Guid(1505616011, (short) 18697, (short) 20196, (byte) 185, (byte) 145, (byte) 173, (byte) 168, (byte) 123, (byte) 124, (byte) 11, (byte) 107);
    public Stream m_Stream;
    public Guid m_AssetId;
    public BinaryAssetParserType m_AssetType;
    public AvatarComponentMasks m_ComponentMask;
    public Skeleton.SkeletonVersion m_SkeletonVersion;
    public CachedBinaryAsset m_Cache;

    public Stream Stream
    {
      get => this.m_Stream;
      set => this.m_Stream = value;
    }

    public bool IsPreloaded => this.m_Stream != null;

    public BinaryAsset(AvatarComponentMasks mask)
    {
      this.m_ComponentMask = mask;
      this.m_AssetType = BinaryAssetParserType.Base;
      this.m_SkeletonVersion = Skeleton.SkeletonVersion.Invalid;
    }

    public virtual bool ProcessComponentsFromStream(BinaryAssetParseContext context) => true;

    public virtual bool ProcessComponentsFromCache(BinaryAssetParseContext context) => true;

    public virtual bool ProcessAssetsFromStream(BinaryAssetParseContext context) => true;

    public virtual bool ProcessAssetsFromCache(BinaryAssetParseContext context) => true;

    public virtual bool ProcessOverridesFromStream(BinaryAssetParseContext context) => true;

    public virtual bool ProcessOverridesFromCache(BinaryAssetParseContext context) => true;

    public abstract bool Validate(BinaryAssetParseContext context);

    public abstract bool ValidateFromCache(BinaryAssetParseContext context);

    public Guid AssetId => this.m_AssetId;

    public virtual bool IsCoordinateSystemIndependent => false;

    public AssetMetadataParser GetMetadata()
    {
      if (this.m_Stream == null)
        return (AssetMetadataParser) null;
      StructuredBinary structuredBinary = new StructuredBinary();
      this.m_Stream.Seek(0L, SeekOrigin.Begin);
      if (!structuredBinary.Open(this.m_Stream))
        return (AssetMetadataParser) null;
      if (!(structuredBinary.Namespace == BinaryAsset.AvatarAssetGuid))
        return (AssetMetadataParser) null;
      BlockIterator iterator = structuredBinary.Iterator;
      AssetMetadataParser metadata = new AssetMetadataParser();
      if (!metadata.LoadFromStrb(iterator))
      {
        Logger.Log((Log) new DebugLog((object) this, string.Format("Asset {0} does not contain metadata.", (object) this.AssetId)));
        return (AssetMetadataParser) null;
      }
      this.m_SkeletonVersion = metadata.AssetSkeletonVersion;
      return metadata;
    }

    public abstract CachedBinaryAsset CreateCacheItem();

    public AssetCacheKey GetAssetKey(
      Guid assetId,
      CoordinateSystem coordinateSystem,
      Skeleton.SkeletonVersion skeletonVersion)
    {
      return this.IsCoordinateSystemIndependent ? new AssetCacheKey(assetId, (int) this.m_AssetType, CoordinateSystem.LeftHanded, 1, skeletonVersion) : new AssetCacheKey(assetId, (int) this.m_AssetType, coordinateSystem, 1, skeletonVersion);
    }
  }
}
