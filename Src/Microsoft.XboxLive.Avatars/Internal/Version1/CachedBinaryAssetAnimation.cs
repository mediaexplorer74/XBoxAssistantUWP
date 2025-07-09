// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.CachedBinaryAssetAnimation
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Animations;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class CachedBinaryAssetAnimation : CachedBinaryAsset
  {
    private AvatarAnimation m_Animation;

    public CachedBinaryAssetAnimation(Guid assetId)
    {
      this.AssetId = assetId;
      this.m_AssetType = BinaryAssetParserType.Animation;
    }

    public override bool ParseAsset(
      StructuredBinary structuredBinary,
      BinaryAssetParseContext context)
    {
      BlockIterator iterator = structuredBinary.Iterator;
      if (!iterator.FindFirst(StructuredBinaryBlockId.Animation))
        return false;
      this.m_Animation = new AssetAnimationParser(context.m_CoordinateSystem, this.m_Metadata.BodyTypeMask).Parse((Stream) iterator);
      return this.m_Animation != null;
    }

    public AvatarAnimation Animation => this.m_Animation;

    protected override int GetMemoryUsageInternal()
    {
      return this.m_Animation == null ? base.GetMemoryUsageInternal() : this.m_Animation.GetMemoryUsage() + base.GetMemoryUsageInternal();
    }
  }
}
