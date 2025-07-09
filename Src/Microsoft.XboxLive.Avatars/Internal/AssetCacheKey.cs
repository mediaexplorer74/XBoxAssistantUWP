// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AssetCacheKey
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal struct AssetCacheKey(
    Guid assetId,
    int assetType,
    CoordinateSystem assetCoordinates,
    int version,
    Skeleton.SkeletonVersion skeletonVersion)
  {
    public int version = version;
    public int assetType = assetType;
    public Guid assetId = assetId;
    public CoordinateSystem assetCoordinates = assetCoordinates;
    public Skeleton.SkeletonVersion skeletonVersion = Skeleton.SkeletonVersion.Invalid;
  }
}
