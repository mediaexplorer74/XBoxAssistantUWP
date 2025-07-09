// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AvatarSkeletonScalingV2
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.Avatars.Internal.Version1;
using Microsoft.XboxLive.MathUtilities;


namespace Microsoft.XboxLive.Avatars.Internal
{
  internal class AvatarSkeletonScalingV2
  {
    private AvatarSkeletonScalingV2()
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
        AvatarSkeletonScalingV2.InitTallScalingWeights(out Scale);
        AvatarSkeletonScalingV2.ApplyTo(skeleton, heightFactor, Scale);
      }
      else
      {
        AvatarSkeletonScalingV2.InitShortScalingWeights(out Scale);
        AvatarSkeletonScalingV2.ApplyTo(skeleton, -heightFactor, Scale);
      }
      switch (bodyType)
      {
        case AvatarGender.Male:
          if ((double) weightFactor >= 0.0)
          {
            AvatarSkeletonScalingV2.InitFatMaleScalingWeights(out Scale);
            AvatarSkeletonScalingV2.ApplyTo(skeleton, weightFactor, Scale);
            break;
          }
          AvatarSkeletonScalingV2.InitThinMaleScalingWeights(out Scale);
          AvatarSkeletonScalingV2.ApplyTo(skeleton, -weightFactor, Scale);
          break;
        case AvatarGender.Female:
          if ((double) weightFactor > 0.0)
          {
            AvatarSkeletonScalingV2.InitFatFemaleScalingWeights(out Scale);
            AvatarSkeletonScalingV2.ApplyTo(skeleton, weightFactor, Scale);
            break;
          }
          AvatarSkeletonScalingV2.InitThinFemaleScalingWeights(out Scale);
          AvatarSkeletonScalingV2.ApplyTo(skeleton, -weightFactor, Scale);
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
        vector3 = AvatarSkeletonScalingV2.VectorLerp(V0, scaleTemplate[length], blendValue);
        skeleton.Joints[length].Local.scale.X *= vector3.X;
        skeleton.Joints[length].Local.scale.Y *= vector3.Y;
        skeleton.Joints[length].Local.scale.Z *= vector3.Z;
      }
      vector3 = AvatarSkeletonScalingV2.VectorLerp(V0, scaleTemplate[0], blendValue);
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
      AvatarSkeletonScalingV2.InitIdentityScales(out Scale);
      Scale[32] = Scale[35] = Scale[26] = Scale[29] = Scale[27] = Scale[30] = new Vector3(1f, 1.5f, 1.5f);
      Scale[7] = Scale[13] = Scale[9] = Scale[17] = new Vector3(1.5f, 1f, 1.5f);
      Scale[24] = new Vector3(1.9f, 1f, 1.5f);
      Scale[18] = new Vector3(1.5f, 1f, 1.4f);
      Scale[10] = new Vector3(1.8f, 1f, 1.9f);
      Scale[4] = new Vector3(1.6f, 1.6f, 2.2f);
      Vector3 V0 = new Vector3(1f, 1f, 1f);
      int index = 72;
      while (--index >= 0)
        Scale[index] = AvatarSkeletonScalingV2.VectorLerp(V0, Scale[index], 0.6f);
    }

    private static void InitFatFemaleScalingWeights(out Vector3[] Scale)
    {
      AvatarSkeletonScalingV2.InitIdentityScales(out Scale);
      Scale[32] = Scale[35] = Scale[26] = Scale[29] = Scale[27] = Scale[30] = new Vector3(1f, 1.6f, 1.6f);
      Scale[7] = Scale[13] = Scale[9] = Scale[17] = new Vector3(1.6f, 1f, 1.6f);
      Scale[24] = new Vector3(2f, 1f, 1.6f);
      Scale[18] = new Vector3(1.6f, 1f, 1.6f);
      Scale[10] = new Vector3(1.6f, 1f, 2f);
      Scale[4] = new Vector3(1.5f, 1.5f, 2f);
      Vector3 V0 = new Vector3(1f, 1f, 1f);
      int index = 72;
      while (--index >= 0)
        Scale[index] = AvatarSkeletonScalingV2.VectorLerp(V0, Scale[index], 0.6f);
    }

    private static void InitThinMaleScalingWeights(out Vector3[] Scale)
    {
      AvatarSkeletonScalingV2.InitIdentityScales(out Scale);
      Scale[32] = Scale[35] = Scale[26] = Scale[29] = Scale[27] = Scale[30] = new Vector3(1f, 0.76f, 0.76f);
      Scale[7] = Scale[13] = Scale[9] = Scale[17] = new Vector3(0.76f, 1f, 0.76f);
      Scale[24] = new Vector3(0.76f, 1f, 0.76f);
      Scale[18] = new Vector3(0.92f, 1f, 0.84f);
      Scale[10] = new Vector3(0.68f, 1f, 0.68f);
      Scale[4] = new Vector3(0.84f, 1f, 0.92f);
    }

    private static void InitThinFemaleScalingWeights(out Vector3[] Scale)
    {
      AvatarSkeletonScalingV2.InitIdentityScales(out Scale);
      Scale[32] = Scale[35] = Scale[26] = Scale[29] = Scale[27] = Scale[30] = new Vector3(1f, 0.82f, 0.82f);
      Scale[7] = Scale[13] = Scale[9] = Scale[17] = new Vector3(0.82f, 1f, 0.82f);
      Scale[24] = new Vector3(0.82f, 1f, 0.82f);
      Scale[18] = new Vector3(0.88f, 1f, 0.82f);
      Scale[10] = new Vector3(0.82f, 1f, 0.7f);
      Scale[4] = new Vector3(0.79f, 1f, 0.82f);
    }

    private static void InitTallScalingWeights(out Vector3[] Scale)
    {
      AvatarSkeletonScalingV2.InitIdentityScales(out Scale);
      Scale[0] = new Vector3(1.1f, 1.1f, 1.1f);
      Scale[19] = new Vector3(0.9f, 0.9f, 0.9f);
    }

    private static void InitShortScalingWeights(out Vector3[] Scale)
    {
      AvatarSkeletonScalingV2.InitIdentityScales(out Scale);
      Scale[0] = new Vector3(0.9f, 0.9f, 0.9f);
      Scale[19] = new Vector3(1.05f, 1.05f, 1.05f);
    }

    private static void RescaleV1toV2(ref Skeleton skeleton)
    {
      float num1 = 0.85f;
      float num2 = 0.85f;
      float num3 = 0.85f;
      float num4 = 0.0240310188f;
      float num5 = 1.2924f;
      float num6 = 0.8351f;
      float num7 = 1.1524f;
      float num8 = 0.7606f;
      float num9 = 0.95f;
      float num10 = 0.95f;
      float num11 = 1f;
      float num12 = 0.95f;
      float num13 = 1.12f;
      float positionX = num5 * num13;
      float num14 = num6 * num13;
      float num15 = num7 * num13;
      float num16 = num8 * num13;
      float num17 = 1.1f;
      if ((double) num14 < (double) num17)
      {
        float num18 = (float) (16.961540222167969 * (double) positionX + 15.419582366943359 * (double) num14);
        num14 = num17;
        positionX = (float) (((double) num18 - 15.419582366943359 * (double) num14) / 16.961540222167969);
      }
      float positionY = (float) (26.87384033203125 * (double) num15 + 25.968425750732422 * (double) num16) / 52.8422661f;
      float num19 = positionY;
      float num20 = (float) (((double) num19 - 1.0) * 0.75 + 1.0);
      float num21 = num19 / num20;
      skeleton.Joints[1].Local.position.Y += num4;
      skeleton.Joints[5].Local.position.Y += num4;
      skeleton.Joints[20].Local.scale = skeleton.Joints[22].Local.scale = new Vector3(positionX, num9, num9);
      skeleton.Joints[25].Local.scale = skeleton.Joints[28].Local.scale = new Vector3(num14 / positionX, num10 / num9, num10 / num9);
      skeleton.Joints[25].Local.position.Y *= positionX / num9;
      skeleton.Joints[28].Local.position.Y *= positionX / num9;
      skeleton.Joints[25].Local.position.Z *= positionX / num9;
      skeleton.Joints[28].Local.position.Z *= positionX / num9;
      AvatarSkeletonScalingV2.Joint_e[] jointEArray1 = new AvatarSkeletonScalingV2.Joint_e[4]
      {
        AvatarSkeletonScalingV2.Joint_e.LF_W,
        AvatarSkeletonScalingV2.Joint_e.RT_W,
        AvatarSkeletonScalingV2.Joint_e.LF_E_TWIST,
        AvatarSkeletonScalingV2.Joint_e.RT_E_TWIST
      };
      foreach (AvatarSkeletonScalingV2.Joint_e index in jointEArray1)
      {
        skeleton.Joints[(int) index].Local.position.Y *= num14 / num10;
        skeleton.Joints[(int) index].Local.position.Z *= num14 / num10;
      }
      skeleton.Joints[2].Local.scale = skeleton.Joints[3].Local.scale = new Vector3(num11, positionY, num11);
      AvatarSkeletonScalingV2.Joint_e[] jointEArray2 = new AvatarSkeletonScalingV2.Joint_e[2]
      {
        AvatarSkeletonScalingV2.Joint_e.LF_K,
        AvatarSkeletonScalingV2.Joint_e.RT_K
      };
      foreach (AvatarSkeletonScalingV2.Joint_e index in jointEArray2)
      {
        skeleton.Joints[(int) index].Local.scale = new Vector3(num12 / num11, num20 / positionY, num12 / num11);
        skeleton.Joints[(int) index].Local.position.X *= positionY / num11;
        skeleton.Joints[(int) index].Local.position.Z *= positionY / num11;
      }
      AvatarSkeletonScalingV2.Joint_e[] jointEArray3 = new AvatarSkeletonScalingV2.Joint_e[2]
      {
        AvatarSkeletonScalingV2.Joint_e.LF_A,
        AvatarSkeletonScalingV2.Joint_e.RT_A
      };
      foreach (AvatarSkeletonScalingV2.Joint_e index in jointEArray3)
      {
        skeleton.Joints[(int) index].Local.scale = new Vector3(num3 / num12, num3 / num20, num3 / num12);
        skeleton.Joints[(int) index].Local.position.X *= num19 / num12;
        skeleton.Joints[(int) index].Local.position.Y *= num21;
        skeleton.Joints[(int) index].Local.position.Z *= num19 / num12;
      }
      skeleton.Joints[14].Local.scale = new Vector3(num1, num1, num1);
      skeleton.Joints[33].Local.scale = new Vector3(num2 / num14, num2 / num10, num2 / num10);
      skeleton.Joints[36].Local.scale = new Vector3(num2 / num14, num2 / num10, num2 / num10);
      skeleton.Joints[31].Local.scale = new Vector3(1f / num14, num2 / num10, num2 / num10);
      skeleton.Joints[34].Local.scale = new Vector3(1f / num14, num2 / num10, num2 / num10);
      skeleton.Joints[27].Local.scale = new Vector3(1f / positionX, 1f, 1f);
      skeleton.Joints[30].Local.scale = new Vector3(1f / positionX, 1f, 1f);
      skeleton.Joints[27].Local.position.X = skeleton.Joints[25].Local.position.X * 0.02f;
      skeleton.Joints[30].Local.position.X = skeleton.Joints[28].Local.position.X * 0.02f;
      skeleton.Joints[0].Local.position.Y = 0.776924849f;
    }

    private static void RescaleV2toV1(ref Skeleton skeleton)
    {
      float num1 = 1.17647052f;
      float num2 = 1.17647052f;
      float num3 = 1.17647052f;
      float num4 = 0.0240310188f;
      float positionX = 0.770551562f;
      float num5 = 0.9090909f;
      float positionY = 0.9301985f;
      float num6 = 0.9301985f;
      float num7 = 1.05263162f;
      float num8 = 1.05263162f;
      float num9 = 1f;
      float num10 = 1.05263162f;
      float num11 = (float) (((double) num6 - 1.0) * 0.75 + 1.0);
      float num12 = num6 / num11;
      skeleton.Joints[1].Local.position.Y -= num4;
      skeleton.Joints[5].Local.position.Y -= num4;
      skeleton.Joints[20].Local.scale = skeleton.Joints[22].Local.scale = new Vector3(positionX, num7, num7);
      skeleton.Joints[25].Local.scale = skeleton.Joints[28].Local.scale = new Vector3(num5 / positionX, num8 / num7, num8 / num7);
      skeleton.Joints[25].Local.position.Y *= positionX / num7;
      skeleton.Joints[28].Local.position.Y *= positionX / num7;
      skeleton.Joints[25].Local.position.Z *= positionX / num7;
      skeleton.Joints[28].Local.position.Z *= positionX / num7;
      AvatarSkeletonScalingV2.Joint_e[] jointEArray1 = new AvatarSkeletonScalingV2.Joint_e[4]
      {
        AvatarSkeletonScalingV2.Joint_e.LF_W,
        AvatarSkeletonScalingV2.Joint_e.RT_W,
        AvatarSkeletonScalingV2.Joint_e.LF_E_TWIST,
        AvatarSkeletonScalingV2.Joint_e.RT_E_TWIST
      };
      foreach (AvatarSkeletonScalingV2.Joint_e index in jointEArray1)
      {
        skeleton.Joints[(int) index].Local.position.Y *= num5 / num8;
        skeleton.Joints[(int) index].Local.position.Z *= num5 / num8;
      }
      skeleton.Joints[2].Local.scale = skeleton.Joints[3].Local.scale = new Vector3(num9, positionY, num9);
      AvatarSkeletonScalingV2.Joint_e[] jointEArray2 = new AvatarSkeletonScalingV2.Joint_e[2]
      {
        AvatarSkeletonScalingV2.Joint_e.LF_K,
        AvatarSkeletonScalingV2.Joint_e.RT_K
      };
      foreach (AvatarSkeletonScalingV2.Joint_e index in jointEArray2)
      {
        skeleton.Joints[(int) index].Local.scale = new Vector3(num10 / num9, num11 / positionY, num10 / num9);
        skeleton.Joints[(int) index].Local.position.X *= positionY / num9;
        skeleton.Joints[(int) index].Local.position.Z *= positionY / num9;
      }
      AvatarSkeletonScalingV2.Joint_e[] jointEArray3 = new AvatarSkeletonScalingV2.Joint_e[2]
      {
        AvatarSkeletonScalingV2.Joint_e.LF_A,
        AvatarSkeletonScalingV2.Joint_e.RT_A
      };
      foreach (AvatarSkeletonScalingV2.Joint_e index in jointEArray3)
      {
        skeleton.Joints[(int) index].Local.scale = new Vector3(num2 / num10, num2 / num11, num2 / num10);
        skeleton.Joints[(int) index].Local.position.X *= num6 / num10;
        skeleton.Joints[(int) index].Local.position.Y *= num12;
        skeleton.Joints[(int) index].Local.position.Z *= num6 / num10;
      }
      skeleton.Joints[14].Local.scale = new Vector3(num3, num3, num3);
      skeleton.Joints[33].Local.scale = new Vector3(num1 / num5, num1 / num8, num1 / num8);
      skeleton.Joints[36].Local.scale = new Vector3(num1 / num5, num1 / num8, num1 / num8);
      skeleton.Joints[31].Local.scale = skeleton.Joints[34].Local.scale = new Vector3(1f / num5, num1 / num8, num1 / num8);
      skeleton.Joints[27].Local.scale = skeleton.Joints[30].Local.scale = new Vector3(1f / positionX, 1f, 1f);
      skeleton.Joints[27].Local.position.X = skeleton.Joints[25].Local.position.X * 0.02f;
      skeleton.Joints[30].Local.position.X = skeleton.Joints[28].Local.position.X * 0.02f;
      skeleton.Joints[27].Local.position.X -= 0.00440244f;
      skeleton.Joints[30].Local.position.X += 0.00440244f;
      skeleton.Joints[0].Local.position.Y = 0.7551987f;
    }

    public static void GetRescaling(
      CoordinateSystem coordinateSystem,
      Skeleton.SkeletonVersion baseVersion,
      Skeleton.SkeletonVersion targetVersion,
      out Skeleton skeleton)
    {
      if (baseVersion == Skeleton.SkeletonVersion.Invalid)
        baseVersion = Skeleton.SkeletonVersion.Natal;
      if (targetVersion == Skeleton.SkeletonVersion.Invalid)
        targetVersion = Skeleton.SkeletonVersion.Natal;
      skeleton = EmbeddedSkeleton.GetEmbeddedSkeleton(coordinateSystem, baseVersion);
      if (baseVersion == Skeleton.SkeletonVersion.Nxe && targetVersion == Skeleton.SkeletonVersion.Natal)
      {
        AvatarSkeletonScalingV2.RescaleV1toV2(ref skeleton);
      }
      else
      {
        if (baseVersion != Skeleton.SkeletonVersion.Natal || targetVersion != Skeleton.SkeletonVersion.Nxe)
          return;
        AvatarSkeletonScalingV2.RescaleV2toV1(ref skeleton);
      }
    }

    private enum Joint_e
    {
      BASE,
      BACKA,
      LF_H,
      RT_H,
      SC_BASE,
      BACKB,
      LF_K,
      LF_SC_H,
      RT_K,
      RT_SC_H,
      SC_BACKA,
      LF_A,
      LF_C,
      LF_SC_K,
      NECK,
      RT_A,
      RT_C,
      RT_SC_K,
      SC_BACKB,
      HEAD,
      LF_S,
      LF_T,
      RT_S,
      RT_T,
      SC_NECK,
      LF_E,
      LF_SC_S,
      LF_SC_TWIST_S,
      RT_E,
      RT_SC_S,
      RT_SC_TWIST_S,
      LF_E_TWIST,
      LF_SC_E,
      LF_W,
      RT_E_TWIST,
      RT_SC_E,
      RT_W,
      LF_FINGA,
      LF_FINGB,
      LF_FINGC,
      LF_FINGD,
      LF_PROP,
      LF_SPECIAL,
      LF_THUMB,
      RT_FINGA,
      RT_FINGB,
      RT_FINGC,
      RT_FINGD,
      RT_PROP,
      RT_SPECIAL,
      RT_THUMB,
      LF_FINGA1,
      LF_FINGB1,
      LF_FINGC1,
      LF_FINGD1,
      LF_THUMB1,
      RT_FINGA1,
      RT_FINGB1,
      RT_FINGC1,
      RT_FINGD1,
      RT_THUMB1,
      LF_FINGA2,
      LF_FINGB2,
      LF_FINGC2,
      LF_FINGD2,
      LF_THUMB2,
      RT_FINGA2,
      RT_FINGB2,
      RT_FINGC2,
      RT_FINGD2,
      RT_THUMB2,
    }
  }
}
