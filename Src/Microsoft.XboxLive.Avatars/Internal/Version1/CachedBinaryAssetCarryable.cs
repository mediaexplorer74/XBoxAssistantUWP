// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.CachedBinaryAssetCarryable
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Animations;
using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Parsers;
using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class CachedBinaryAssetCarryable : CachedBinaryAssetModel
  {
    public Skeleton m_CarryableSkeleton;
    public AvatarAnimation m_CarryableAnimation;

    public CachedBinaryAssetCarryable(Guid assetId)
      : base(assetId)
    {
      this.m_AssetType = BinaryAssetParserType.Carryable;
    }

    public override bool ParseAsset(
      StructuredBinary structuredBinary,
      BinaryAssetParseContext context)
    {
      BlockIterator iterator = structuredBinary.Iterator;
      if (iterator == null || !iterator.FindFirst(StructuredBinaryBlockId.Model))
        return false;
      AvatarComponent model = new AvatarComponent();
      this.Models.Add(model);
      if (!CachedBinaryAssetModel.ParseModel(iterator, model, context.m_ResourceFactory, context.m_CoordinateSystem) || !this.ParseColorTable(iterator) || !iterator.FindFirst(StructuredBinaryBlockId.Skeleton))
        return false;
      if ((int) iterator.Length <= 0)
      {
        Logger.Log((Log) new DebugLog((object) this, "block size <= 0; strb file is corrupted"));
        return false;
      }
      DecompressStream decompressStream = new DecompressStream((Stream) iterator, (int) iterator.Length);
      this.m_CarryableSkeleton = new AssetSkeletonParser(context.m_CoordinateSystem).Parse((Stream) decompressStream);
      if (iterator.FindFirst(StructuredBinaryBlockId.Animation))
        this.m_CarryableAnimation = new AssetAnimationParser(context.m_CoordinateSystem, AvatarGender.Both).Parse((Stream) iterator);
      return base.ParseAsset(structuredBinary, context);
    }
  }
}
