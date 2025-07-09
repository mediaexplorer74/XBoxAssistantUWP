// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AvatarSkeletonScaling
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.MathUtilities;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class AvatarSkeletonScaling
  {
    private AvatarSkeletonScaling()
    {
    }

    public static void ApplyTo(
      AvatarGender bodyType,
      float weightFactor,
      float heightFactor,
      Skeleton skeleton)
    {
      Vector3[] Scale;
      if ((double) heightFactor >= 0.0)
      {
        AvatarSkeletonScaling.InitTallScalingWeights(out Scale);
        AvatarSkeletonScaling.ApplyTo(skeleton, heightFactor, Scale);
      }
      else
      {
        AvatarSkeletonScaling.InitShortScalingWeights(out Scale);
        AvatarSkeletonScaling.ApplyTo(skeleton, -heightFactor, Scale);
      }
      switch (bodyType)
      {
        case AvatarGender.Male:
          if ((double) weightFactor >= 0.0)
          {
            AvatarSkeletonScaling.InitFatMaleScalingWeights(out Scale);
            AvatarSkeletonScaling.ApplyTo(skeleton, weightFactor, Scale);
            break;
          }
          AvatarSkeletonScaling.InitThinMaleScalingWeights(out Scale);
          AvatarSkeletonScaling.ApplyTo(skeleton, -weightFactor, Scale);
          break;
        case AvatarGender.Female:
          if ((double) weightFactor > 0.0)
          {
            AvatarSkeletonScaling.InitFatFemaleScalingWeights(out Scale);
            AvatarSkeletonScaling.ApplyTo(skeleton, weightFactor, Scale);
            break;
          }
          AvatarSkeletonScaling.InitThinFemaleScalingWeights(out Scale);
          AvatarSkeletonScaling.ApplyTo(skeleton, -weightFactor, Scale);
          break;
      }
    }

    private static Vector3 VectorLerp(Vector3 V0, Vector3 V1, float t)
    {
      return new Vector3()
      {
        X = V0.X + t * (V1.X - V0.X),
        Y = V0.Y + t * (V1.Y - V0.Y),
        Z = V0.Z + t * (V1.Z - V0.Z)
      };
    }

    private static void ApplyTo(Skeleton skeleton, float blendValue, Vector3[] scaleTemplate)
    {
      blendValue = (double) blendValue < 0.0 ? 0.0f : ((double) blendValue > 1.0 ? 1f : blendValue);
      Vector3 V0 = new Vector3(1f, 1f, 1f);
      int length = skeleton.Joints.Length;
      Vector3 vector3;
      while (--length >= 0)
      {
        vector3 = AvatarSkeletonScaling.VectorLerp(V0, scaleTemplate[length], blendValue);
        skeleton.Joints[length].Local.scale.X *= vector3.X;
        skeleton.Joints[length].Local.scale.Y *= vector3.Y;
        skeleton.Joints[length].Local.scale.Z *= vector3.Z;
      }
      vector3 = AvatarSkeletonScaling.VectorLerp(V0, scaleTemplate[0], blendValue);
      skeleton.Joints[0].Local.position.X *= vector3.X;
      skeleton.Joints[0].Local.position.Y *= vector3.Y;
      skeleton.Joints[0].Local.position.Z *= vector3.Z;
    }

    private static void InitIdentityScales(out Vector3[] scales)
    {
      Vector3 vector3 = new Vector3(1f, 1f, 1f);
      scales = new Vector3[72];
      int index = 72;
      while (--index >= 0)
        scales[index] = vector3;
    }

    private static void InitFatMaleScalingWeights(out Vector3[] Scale)
    {
      AvatarSkeletonScaling.InitIdentityScales(out Scale);
      Scale[32] = Scale[35] = Scale[26] = Scale[29] = Scale[27] = Scale[30] = new Vector3(1f, 1.5f, 1.5f);
      Scale[7] = Scale[13] = Scale[9] = Scale[17] = new Vector3(1.5f, 1f, 1.5f);
      Scale[24] = new Vector3(1.9f, 1f, 1.5f);
      Scale[18] = new Vector3(1.5f, 1f, 1.4f);
      Scale[10] = new Vector3(1.8f, 1f, 1.9f);
      Scale[4] = new Vector3(1.6f, 1.6f, 2.2f);
      Vector3 V0 = new Vector3(1f, 1f, 1f);
      int index = 72;
      while (--index >= 0)
        Scale[index] = AvatarSkeletonScaling.VectorLerp(V0, Scale[index], 0.6f);
    }

    private static void InitFatFemaleScalingWeights(out Vector3[] Scale)
    {
      AvatarSkeletonScaling.InitIdentityScales(out Scale);
      Scale[32] = Scale[35] = Scale[26] = Scale[29] = Scale[27] = Scale[30] = new Vector3(1f, 1.6f, 1.6f);
      Scale[7] = Scale[13] = Scale[9] = Scale[17] = new Vector3(1.6f, 1f, 1.6f);
      Scale[24] = new Vector3(2f, 1f, 1.6f);
      Scale[18] = new Vector3(1.6f, 1f, 1.6f);
      Scale[10] = new Vector3(1.6f, 1f, 2f);
      Scale[4] = new Vector3(1.5f, 1.5f, 2f);
      Vector3 V0 = new Vector3(1f, 1f, 1f);
      int index = 72;
      while (--index >= 0)
        Scale[index] = AvatarSkeletonScaling.VectorLerp(V0, Scale[index], 0.6f);
    }

    private static void InitThinMaleScalingWeights(out Vector3[] Scale)
    {
      AvatarSkeletonScaling.InitIdentityScales(out Scale);
      Scale[32] = Scale[35] = Scale[26] = Scale[29] = Scale[27] = Scale[30] = new Vector3(1f, 0.7f, 0.7f);
      Scale[7] = Scale[13] = Scale[9] = Scale[17] = new Vector3(0.7f, 1f, 0.7f);
      Scale[24] = new Vector3(0.7f, 1f, 0.7f);
      Scale[18] = new Vector3(0.9f, 1f, 0.8f);
      Scale[10] = new Vector3(0.6f, 1f, 0.6f);
      Scale[4] = new Vector3(0.8f, 1f, 0.9f);
    }

    private static void InitThinFemaleScalingWeights(out Vector3[] Scale)
    {
      AvatarSkeletonScaling.InitIdentityScales(out Scale);
      Scale[32] = Scale[35] = Scale[26] = Scale[29] = Scale[27] = Scale[30] = new Vector3(1f, 0.7f, 0.7f);
      Scale[7] = Scale[13] = Scale[9] = Scale[17] = new Vector3(0.7f, 1f, 0.7f);
      Scale[24] = new Vector3(0.7f, 1f, 0.7f);
      Scale[18] = new Vector3(0.8f, 1f, 0.7f);
      Scale[10] = new Vector3(0.7f, 1f, 0.5f);
      Scale[4] = new Vector3(0.65f, 1f, 0.7f);
    }

    private static void InitTallScalingWeights(out Vector3[] Scale)
    {
      AvatarSkeletonScaling.InitIdentityScales(out Scale);
      Scale[0] = new Vector3(1.1f, 1.1f, 1.1f);
      Scale[19] = new Vector3(0.9f, 0.9f, 0.9f);
    }

    private static void InitShortScalingWeights(out Vector3[] Scale)
    {
      AvatarSkeletonScaling.InitIdentityScales(out Scale);
      Scale[0] = new Vector3(0.9f, 0.9f, 0.9f);
      Scale[19] = new Vector3(1.05f, 1.05f, 1.05f);
    }

    private enum Joint_e
    {
      BASE = 0,
      SC_BASE = 4,
      LF_SC_H = 7,
      RT_SC_H = 9,
      SC_BACKA = 10, // 0x0000000A
      LF_SC_K = 13, // 0x0000000D
      RT_SC_K = 17, // 0x00000011
      SC_BACKB = 18, // 0x00000012
      HEAD = 19, // 0x00000013
      SC_NECK = 24, // 0x00000018
      LF_SC_S = 26, // 0x0000001A
      LF_SC_S_SKIN = 27, // 0x0000001B
      RT_SC_S = 29, // 0x0000001D
      RT_SC_S_SKIN = 30, // 0x0000001E
      LF_SC_E = 32, // 0x00000020
      RT_SC_E = 35, // 0x00000023
    }
  }
}
