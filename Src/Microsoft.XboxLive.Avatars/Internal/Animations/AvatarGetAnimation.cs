// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Animations.AvatarGetAnimation
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Parsers;
using Microsoft.XboxLive.Avatars.Internal.Version1;
using System;
using System.Collections.Generic;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Animations
{
  internal class AvatarGetAnimation
  {
    private CoordinateSystem m_CoordSystem;

    internal AvatarGetAnimation(CoordinateSystem flags) => this.m_CoordSystem = flags;

    private static void DownloadAssetCompleted(object sender, DataRequestCompletedEventArgs e)
    {
      SyncDownloadContext userState = (SyncDownloadContext) e.UserState;
      if (e.Error != null)
      {
        userState.error = e.Error;
        userState.stream = (Stream) null;
      }
      else
        userState.stream = !e.Cancelled ? e.Result : (Stream) null;
      userState.syncEvent.Set();
    }

    internal AvatarAnimation Load(
      Guid animationAssetId,
      AssetLoader assetLoader,
      IDataManager dataManager)
    {
      AvatarAssetCacheManager assetCacheManager = assetLoader.GetAssetCacheManager();
      if (assetCacheManager != null)
      {
        AvatarAssetCacheV1 assetCache = assetCacheManager.GetAssetCache(1) as AvatarAssetCacheV1;
        BinaryAssetAnimation binaryAssetAnimation = new BinaryAssetAnimation(animationAssetId);
        assetCache.LoadAssets(new List<BinaryAsset>()
        {
          (BinaryAsset) binaryAssetAnimation
        }, new BinaryAssetParseContext()
        {
          m_CoordinateSystem = this.m_CoordSystem,
          m_AssetCache = assetCacheManager
        }, dataManager);
        if (!(binaryAssetAnimation.m_Cache is CachedBinaryAssetAnimation cache))
          throw new AvatarException(Resources.InvalidAnimationAssetFileText);
        switch (cache.m_AssetState)
        {
          case CachedBinaryAsset.AssetState.Invalid:
            throw new AvatarException(Resources.FailedToDownloadAnimationAssetText);
          case CachedBinaryAsset.AssetState.Downloaded:
            throw new AvatarException(Resources.FailedToParseAnimationAssetFileText);
          default:
            return cache.Animation;
        }
      }
      else
      {
        using (SyncDownloadContext context = new SyncDownloadContext())
        {
          dataManager.GetAssetAsync(animationAssetId, new DownloadRequestEventHandler(AvatarGetAnimation.DownloadAssetCompleted), (object) context);
          context.syncEvent.WaitOne();
          Stream stream = context.stream;
          if (stream == null)
            throw new AvatarException(Resources.FailedToDownloadAnimationAssetText);
          StructuredBinary structuredBinary = new StructuredBinary();
          BlockIterator blockIterator = structuredBinary.Open(stream) ? structuredBinary.Iterator : throw new AvatarException(Resources.InvalidAnimationAssetFileText);
          if (!blockIterator.FindFirst(StructuredBinaryBlockId.Animation))
            throw new AvatarException(Resources.InvalidAnimationAssetFileText);
          return new AssetAnimationParser(this.m_CoordSystem, AssetLoader.GetAssetBodyType(animationAssetId)).Parse((Stream) blockIterator);
        }
      }
    }

    internal AvatarAnimation Load(Stream animationStream)
    {
      animationStream.Seek(0L, SeekOrigin.Begin);
      StructuredBinary structuredBinary = new StructuredBinary();
      BlockIterator strb = structuredBinary.Open(animationStream) ? structuredBinary.Iterator : throw new AvatarException(Resources.InvalidAnimationAssetFileText);
      AssetMetadataParser assetMetadataParser = new AssetMetadataParser();
      if (!assetMetadataParser.LoadFromStrb(strb))
        throw new AvatarException(Resources.InvalidAnimationAssetFileText);
      if (!strb.FindFirst(StructuredBinaryBlockId.Animation))
        throw new AvatarException(Resources.InvalidAnimationAssetFileText);
      return new AssetAnimationParser(this.m_CoordSystem, assetMetadataParser.m_bodyTypeMask).Parse((Stream) strb);
    }
  }
}
