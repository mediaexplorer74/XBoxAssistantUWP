// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Assets.Skeleton
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll


namespace Microsoft.XboxLive.Avatars.Internal.Assets
{
  public class Skeleton
  {
    public const int AvatarMaxJoints = 72;
    public const int CarryableMaxJoints = 72;
    public Joint[] Joints;

    public enum SkeletonVersion
    {
      Invalid,
      Nxe,
      Natal,
    }
  }
}
