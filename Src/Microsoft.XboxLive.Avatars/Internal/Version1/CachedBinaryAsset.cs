// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.CachedBinaryAsset
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal abstract class CachedBinaryAsset : CachedAsset
  {
    internal AssetMetadataParser m_Metadata = new AssetMetadataParser();
    internal BinaryAssetParserType m_AssetType;
    internal CachedBinaryAsset.AssetState m_AssetState;

    public CachedBinaryAsset() => this.m_AssetType = BinaryAssetParserType.Base;

    public bool Parse(Stream stream, BinaryAssetParseContext context)
    {
      this.m_CoordinateSystem = context.m_CoordinateSystem;
      StructuredBinary structuredBinary = new StructuredBinary();
      stream.Seek(0L, SeekOrigin.Begin);
      if (!structuredBinary.Open(stream) || !(structuredBinary.Namespace == BinaryAsset.AvatarAssetGuid))
        return false;
      BlockIterator iterator = structuredBinary.Iterator;
      if (iterator == null)
        return false;
      if (!this.m_Metadata.LoadFromStrb(iterator))
      {
        Logger.Log((Log) new DebugLog((object) this, string.Format("Asset {0} does not contain metadata.", (object) this.AssetId)));
        return false;
      }
      if (!this.ParseAsset(structuredBinary, context))
        return false;
      this.m_MemoryUsage = this.GetMemoryUsageInternal();
      return true;
    }

    public Skeleton.SkeletonVersion GetAssetSkeletonVersion()
    {
      return this.m_Metadata.AssetSkeletonVersion;
    }

    public abstract bool ParseAsset(
      StructuredBinary structuredBinary,
      BinaryAssetParseContext context);

    public override AssetCacheKey AssetKey
    {
      get
      {
        return new AssetCacheKey(this.AssetId, (int) this.m_AssetType, this.m_CoordinateSystem, 1, this.m_Metadata.AssetSkeletonVersion);
      }
    }

    public enum AssetState
    {
      Default,
      Invalid,
      Downloaded,
      Parsed,
    }
  }
}
