// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Assets.AvatarCarryable
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Animations;


namespace Microsoft.XboxLive.Avatars.Internal.Assets
{
  public class AvatarCarryable
  {
    internal Skeleton m_Skeleton;
    internal AvatarComponent m_ComponentModel = new AvatarComponent();
    internal AvatarAnimation m_Animation;

    public AvatarAnimation Animation => this.m_Animation;

    public Skeleton Skeleton => this.m_Skeleton;

    public AvatarComponent Model => this.m_ComponentModel;

    public float[] GetCarryableMaxSkeletonScaling()
    {
      return this.m_Animation.GetCarryableMaxSkeletonScaling(this.m_Skeleton);
    }
  }
}
