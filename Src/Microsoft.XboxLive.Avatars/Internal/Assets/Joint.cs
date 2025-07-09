// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Assets.Joint
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.MathUtilities;


namespace Microsoft.XboxLive.Avatars.Internal.Assets
{
  public struct Joint
  {
    public const int InvalidJointIndex = -1;
    public int Parent;
    public int Child;
    public int Sibling;
    public Vector3 BindPosition;
    public Quaternion BindRotation;
    public Pose Local;
  }
}
