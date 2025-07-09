// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Avatar
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using System.Collections.Generic;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class Avatar
  {
    internal Skeleton.SkeletonVersion m_SkeletonVersion;
    internal Skeleton m_Skeleton;
    internal List<AvatarComponent> m_Models = new List<AvatarComponent>();
    internal AvatarCarryable m_Carryable;
    internal AvatarManifest m_Manifest;

    internal Avatar(AvatarManifest manifest)
    {
      this.m_SkeletonVersion = Skeleton.SkeletonVersion.Invalid;
      this.m_Manifest = manifest;
    }

    internal void AddComponent(AvatarComponent component) => this.m_Models.Add(component);

    public AvatarManifest Manifest => this.m_Manifest;

    public Skeleton Skeleton => this.m_Skeleton;

    public Skeleton.SkeletonVersion SkeletonVersion => this.m_SkeletonVersion;

    public AvatarCarryable Carryable
    {
      get => this.m_Carryable;
      set => this.m_Carryable = value;
    }

    public List<AvatarComponent> Models => this.m_Models;
  }
}
