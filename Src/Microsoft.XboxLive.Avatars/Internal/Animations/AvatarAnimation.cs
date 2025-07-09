// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Animations.AvatarAnimation
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.MathUtilities;
using System;


namespace Microsoft.XboxLive.Avatars.Internal.Animations
{
  public class AvatarAnimation : Animation
  {
    private int m_JointCount;
    private Pose[][] m_AvatarKeyframes;
    private Pose[][] m_CarraybleKeyframes;
    private AvatarExpression[] m_AvatarFacialAnimation;
    private AvatarGender m_bodyTypeMask;

    public AvatarGender Gender => this.m_bodyTypeMask;

    internal AvatarAnimation(
      Pose[][] avatarPoses,
      Pose[][] carryablePoses,
      AvatarExpression[] facialExp,
      float fps,
      AvatarGender bodyTypeMask)
      : base(avatarPoses.Length, fps)
    {
      this.m_bodyTypeMask = bodyTypeMask;
      this.m_AvatarKeyframes = avatarPoses;
      this.m_CarraybleKeyframes = carryablePoses;
      this.m_JointCount = avatarPoses[0].Length;
      this.m_AvatarFacialAnimation = facialExp;
    }

    public static AnimationCursor InitializeCursor(float speed, AnimationPlayMode animationPlayMode)
    {
      return new AnimationCursor(speed, animationPlayMode);
    }

    public float GetLoopPhase(AnimationCursor cursor) => cursor.Time % this.Length;

    private void GetSkeletonPose(
      Pose[][] keyframes,
      AnimationCursor cursor,
      float blendWeight,
      Pose[] jointBuffer)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = 1f - blendWeight;
      int index1 = 0;
      int index2 = 0;
      if (cursor.PlayMode == AnimationPlayMode.Once)
      {
        float d = cursor.Time * this.Fps;
        int val1 = (int) Math.Floor((double) d);
        num2 = d - (float) val1;
        num1 = 1f - num2;
        index1 = Math.Min(val1, this.FrameCount - 1);
        index2 = Math.Min(index1 + 1, this.FrameCount - 1);
      }
      else if (cursor.PlayMode == AnimationPlayMode.Loop)
      {
        float d = cursor.Time * this.Fps;
        int num4 = (int) Math.Floor((double) d);
        num2 = d - (float) num4;
        num1 = 1f - num2;
        index1 = num4 % this.FrameCount;
        index2 = (index1 + 1) % this.FrameCount;
      }
      else if (cursor.PlayMode == AnimationPlayMode.Bounce)
      {
        float d = cursor.Time * this.Fps;
        int num5 = (int) Math.Floor((double) d);
        num2 = d - (float) num5;
        num1 = 1f - num2;
        if ((num5 / this.FrameCount & 1) == 0)
        {
          index1 = num5 % this.FrameCount;
          index2 = Math.Min(index1 + 1, this.FrameCount - 1);
        }
        else
        {
          index1 = this.FrameCount - 1 - num5 % this.FrameCount;
          index2 = Math.Max(index1 - 1, 0);
        }
      }
      for (int index3 = 0; index3 < jointBuffer.Length && index3 < keyframes[index1].Length; ++index3)
      {
        Vector3 position1 = keyframes[index1][index3].position;
        Vector3 position2 = keyframes[index2][index3].position;
        Vector3 scale1 = keyframes[index1][index3].scale;
        Vector3 scale2 = keyframes[index2][index3].scale;
        Quaternion rotation1 = keyframes[index1][index3].rotation;
        Quaternion rotation2 = keyframes[index2][index3].rotation;
        position1.X = (float) ((double) num1 * (double) position1.X + (double) num2 * (double) position2.X);
        position1.Y = (float) ((double) num1 * (double) position1.Y + (double) num2 * (double) position2.Y);
        position1.Z = (float) ((double) num1 * (double) position1.Z + (double) num2 * (double) position2.Z);
        scale1.X = (float) ((double) num1 * (double) scale1.X + (double) num2 * (double) scale2.X);
        scale1.Y = (float) ((double) num1 * (double) scale1.Y + (double) num2 * (double) scale2.Y);
        scale1.Z = (float) ((double) num1 * (double) scale1.Z + (double) num2 * (double) scale2.Z);
        if ((double) rotation1.X * (double) rotation2.X + (double) rotation1.Y * (double) rotation2.Y + (double) rotation1.Z * (double) rotation2.Z + (double) rotation1.W * (double) rotation2.W >= 0.0)
        {
          rotation1.X = (float) ((double) num1 * (double) rotation1.X + (double) num2 * (double) rotation2.X);
          rotation1.Y = (float) ((double) num1 * (double) rotation1.Y + (double) num2 * (double) rotation2.Y);
          rotation1.Z = (float) ((double) num1 * (double) rotation1.Z + (double) num2 * (double) rotation2.Z);
          rotation1.W = (float) ((double) num1 * (double) rotation1.W + (double) num2 * (double) rotation2.W);
        }
        else
        {
          rotation1.X = (float) ((double) num1 * (double) rotation1.X - (double) num2 * (double) rotation2.X);
          rotation1.Y = (float) ((double) num1 * (double) rotation1.Y - (double) num2 * (double) rotation2.Y);
          rotation1.Z = (float) ((double) num1 * (double) rotation1.Z - (double) num2 * (double) rotation2.Z);
          rotation1.W = (float) ((double) num1 * (double) rotation1.W - (double) num2 * (double) rotation2.W);
        }
        float d1 = (float) ((double) rotation1.X * (double) rotation1.X + (double) rotation1.Y * (double) rotation1.Y + (double) rotation1.Z * (double) rotation1.Z + (double) rotation1.W * (double) rotation1.W);
        if ((double) d1 > 0.0)
        {
          float num6 = (float) (1.0 / Math.Sqrt((double) d1));
          rotation1.X *= num6;
          rotation1.Y *= num6;
          rotation1.Z *= num6;
          rotation1.W *= num6;
        }
        if ((double) blendWeight == 1.0)
        {
          jointBuffer[index3].position = position1;
          jointBuffer[index3].scale = scale1;
          jointBuffer[index3].rotation = rotation1;
        }
        else
        {
          Vector3 position3 = jointBuffer[index3].position;
          scale2 = jointBuffer[index3].scale;
          rotation2 = jointBuffer[index3].rotation;
          position3.X = (float) ((double) blendWeight * (double) position1.X + (double) num3 * (double) position3.X);
          position3.Y = (float) ((double) blendWeight * (double) position1.Y + (double) num3 * (double) position3.Y);
          position3.Z = (float) ((double) blendWeight * (double) position1.Z + (double) num3 * (double) position3.Z);
          scale2.X = (float) ((double) blendWeight * (double) scale1.X + (double) num3 * (double) scale2.X);
          scale2.Y = (float) ((double) blendWeight * (double) scale1.Y + (double) num3 * (double) scale2.Y);
          scale2.Z = (float) ((double) blendWeight * (double) scale1.Z + (double) num3 * (double) scale2.Z);
          if ((double) rotation1.X * (double) rotation2.X + (double) rotation1.Y * (double) rotation2.Y + (double) rotation1.Z * (double) rotation2.Z + (double) rotation1.W * (double) rotation2.W >= 0.0)
          {
            rotation2.X = (float) ((double) blendWeight * (double) rotation1.X + (double) num3 * (double) rotation2.X);
            rotation2.Y = (float) ((double) blendWeight * (double) rotation1.Y + (double) num3 * (double) rotation2.Y);
            rotation2.Z = (float) ((double) blendWeight * (double) rotation1.Z + (double) num3 * (double) rotation2.Z);
            rotation2.W = (float) ((double) blendWeight * (double) rotation1.W + (double) num3 * (double) rotation2.W);
          }
          else
          {
            rotation2.X = (float) ((double) blendWeight * (double) rotation1.X - (double) num3 * (double) rotation2.X);
            rotation2.Y = (float) ((double) blendWeight * (double) rotation1.Y - (double) num3 * (double) rotation2.Y);
            rotation2.Z = (float) ((double) blendWeight * (double) rotation1.Z - (double) num3 * (double) rotation2.Z);
            rotation2.W = (float) ((double) blendWeight * (double) rotation1.W - (double) num3 * (double) rotation2.W);
          }
          float d2 = (float) ((double) rotation2.X * (double) rotation2.X + (double) rotation2.Y * (double) rotation2.Y + (double) rotation2.Z * (double) rotation2.Z + (double) rotation2.W * (double) rotation2.W);
          if ((double) d2 > 0.0)
          {
            float num7 = (float) (1.0 / Math.Sqrt((double) d2));
            rotation2.X *= num7;
            rotation2.Y *= num7;
            rotation2.Z *= num7;
            rotation2.W *= num7;
          }
          jointBuffer[index3].position = position3;
          jointBuffer[index3].scale = scale2;
          jointBuffer[index3].rotation = rotation2;
        }
      }
    }

    public bool HasCarryableKeyframes => this.m_CarraybleKeyframes != null;

    public int AvatarJointsCount
    {
      get => this.m_AvatarKeyframes == null ? 0 : this.m_AvatarKeyframes[0].Length;
    }

    public int CarryableJointsCount
    {
      get => this.m_CarraybleKeyframes == null ? 0 : this.m_CarraybleKeyframes[0].Length;
    }

    public bool GetCarryablePose(AnimationCursor cursor, float blendWeight, Pose[] jointBuffer)
    {
      if (this.m_CarraybleKeyframes == null)
        return false;
      this.GetSkeletonPose(this.m_CarraybleKeyframes, cursor, blendWeight, jointBuffer);
      return true;
    }

    public bool GetAvatarPose(AnimationCursor cursor, float blendWeight, Pose[] jointBuffer)
    {
      this.GetSkeletonPose(this.m_AvatarKeyframes, cursor, blendWeight, jointBuffer);
      return true;
    }

    internal float[] GetCarryableMaxSkeletonScaling(Skeleton carryableSkeleton)
    {
      if (this.m_CarraybleKeyframes == null)
        return (float[]) null;
      int length = this.m_CarraybleKeyframes[0].Length;
      float[] maxSkeletonScaling = new float[length];
      float[] numArray = new float[length];
      for (int index1 = 0; index1 < this.m_CarraybleKeyframes.Length; ++index1)
      {
        for (int index2 = 0; index2 < length; ++index2)
        {
          Vector3 scale = this.m_CarraybleKeyframes[index1][index2].scale;
          numArray[index2] = scale.X;
          if ((double) numArray[index2] < (double) scale.Y)
            numArray[index2] = scale.Y;
          if ((double) numArray[index2] < (double) scale.Z)
            numArray[index2] = scale.Z;
          float num1 = carryableSkeleton.Joints[index2].Local.scale.X;
          if ((double) num1 < (double) carryableSkeleton.Joints[index2].Local.scale.Y)
            num1 = carryableSkeleton.Joints[index2].Local.scale.Y;
          if ((double) num1 < (double) carryableSkeleton.Joints[index2].Local.scale.Z)
            num1 = carryableSkeleton.Joints[index2].Local.scale.Z;
          numArray[index2] *= num1;
          float num2 = index2 == 0 ? 1f : numArray[carryableSkeleton.Joints[index2].Parent];
          numArray[index2] *= num2;
          if ((double) numArray[index2] > (double) maxSkeletonScaling[index2])
            maxSkeletonScaling[index2] = numArray[index2];
        }
      }
      return maxSkeletonScaling;
    }

    public AvatarExpression GetAnimableTextureLayers(AnimationCursor cursor)
    {
      float num;
      if (cursor.PlayMode == AnimationPlayMode.Once)
      {
        num = cursor.Time;
      }
      else
      {
        num = cursor.Time % this.Length;
        if (cursor.PlayMode == AnimationPlayMode.Bounce && ((int) ((double) cursor.Time / (double) this.Length) & 1) == 1)
          num = this.Length - num;
      }
      int index = (int) ((double) num * (double) this.Fps);
      if (index >= this.FrameCount)
        index = this.FrameCount - 1;
      return this.m_AvatarFacialAnimation[index];
    }

    internal AvatarAnimation(int jointCount, int frameCount, int fps)
      : base(frameCount, (float) fps)
    {
      this.m_JointCount = jointCount;
      this.m_AvatarKeyframes = new Pose[frameCount][];
      for (int index = 0; index < frameCount; ++index)
        this.m_AvatarKeyframes[index] = new Pose[this.m_JointCount];
      this.m_AvatarFacialAnimation = new AvatarExpression[frameCount];
    }

    internal void SetSkeletonPose(int joint, int frame, ref Pose pose)
    {
      this.m_AvatarKeyframes[frame][joint] = pose;
    }

    internal void SetAvatarExpression(int frame, ref AvatarExpression expression)
    {
      this.m_AvatarFacialAnimation[frame] = expression;
    }

    internal Pose[] GetKeyframe(int frame) => this.m_AvatarKeyframes[frame];

    public int GetMemoryUsage()
    {
      int memoryUsage = 64;
      if (this.m_AvatarKeyframes != null)
        memoryUsage += this.m_AvatarKeyframes[0].Length * this.m_AvatarKeyframes.Length * 48;
      if (this.m_CarraybleKeyframes != null)
        memoryUsage += this.m_CarraybleKeyframes[0].Length * this.m_CarraybleKeyframes.Length * 48;
      if (this.m_AvatarFacialAnimation != null)
        memoryUsage += this.m_AvatarFacialAnimation.Length * 32;
      return memoryUsage;
    }
  }
}
