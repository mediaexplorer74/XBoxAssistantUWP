// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.BinaryAssetParseContext
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class BinaryAssetParseContext
  {
    public CoordinateSystem m_CoordinateSystem;
    public Avatar m_Target;
    public AvatarComponentMasks m_CombinedComponentMask;
    public AvatarGender m_BodyType;
    public IResourceFactory m_ResourceFactory;
    public AvatarAssetCacheManager m_AssetCache;
    public Skeleton.SkeletonVersion m_skeletonVersion;
  }
}
