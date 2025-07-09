// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.QuaternionMath
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll

using System;
using System.Runtime.InteropServices;


namespace Microsoft.XboxLive.MathUtilities
{
  public sealed class QuaternionMath
  {
    private QuaternionMath()
    {
    }

    public static Quaternion FastNormalizedLerp(
      ref Quaternion quaternionA,
      ref Quaternion quaternionB,
      float weight)
    {
      Quaternion quaternion = new Quaternion();
      quaternion.X = quaternionA.X + weight * (quaternionB.X - quaternionA.X);
      quaternion.Y = quaternionA.Y + weight * (quaternionB.Y - quaternionA.Y);
      quaternion.Z = quaternionA.Z + weight * (quaternionB.Z - quaternionA.Z);
      quaternion.W = quaternionA.W + weight * (quaternionB.W - quaternionA.W);
      float num = (float) ((double) quaternion.X * (double) quaternion.X + (double) quaternion.Y * (double) quaternion.Y + (double) quaternion.Z * (double) quaternion.Z + (double) quaternion.W * (double) quaternion.W);
      QuaternionMath.FloatInt floatInt = new QuaternionMath.FloatInt()
      {
        f = num
      };
      floatInt.i = 3194926348U - floatInt.i >> 1;
      floatInt.f = (float) (0.5 * (double) floatInt.f * (3.0 - (double) num * (double) floatInt.f * (double) floatInt.f));
      quaternion.X *= floatInt.f;
      quaternion.Y *= floatInt.f;
      quaternion.Z *= floatInt.f;
      quaternion.W *= floatInt.f;
      return quaternion;
    }

    public static Quaternion NormalizedLerp(
      ref Quaternion quaternionA,
      ref Quaternion quaternionB,
      float weight)
    {
      Quaternion quaternion = new Quaternion();
      quaternion.X = quaternionA.X + weight * (quaternionB.X - quaternionA.X);
      quaternion.Y = quaternionA.Y + weight * (quaternionB.Y - quaternionA.Y);
      quaternion.Z = quaternionA.Z + weight * (quaternionB.Z - quaternionA.Z);
      quaternion.W = quaternionA.W + weight * (quaternionB.W - quaternionA.W);
      float d = (float) ((double) quaternion.X * (double) quaternion.X + (double) quaternion.Y * (double) quaternion.Y + (double) quaternion.Z * (double) quaternion.Z + (double) quaternion.W * (double) quaternion.W);
      if ((double) d > 0.0)
      {
        float num = (float) (1.0 / Math.Sqrt((double) d));
        quaternion.X *= num;
        quaternion.Y *= num;
        quaternion.Z *= num;
        quaternion.W *= num;
      }
      return quaternion;
    }

    public static Quaternion Exp(Vector3 vector)
    {
      Quaternion quaternion = new Quaternion();
      float num1 = (float) Math.Sqrt((double) vector.X * (double) vector.X + (double) vector.Y * (double) vector.Y + (double) vector.Z * (double) vector.Z);
      if ((double) num1 < 1.0000000116860974E-07)
      {
        quaternion.X = 0.0f;
        quaternion.Y = 0.0f;
        quaternion.Z = 0.0f;
        quaternion.W = 1f;
      }
      else
      {
        float num2 = (float) Math.Sin((double) num1);
        float num3 = (float) Math.Cos((double) num1);
        float num4 = num2 / num1;
        quaternion.X = vector.X * num4;
        quaternion.Y = vector.Y * num4;
        quaternion.Z = vector.Z * num4;
        quaternion.W = num3;
      }
      return quaternion;
    }

    public static Vector3 Ln(Quaternion quat)
    {
      float a = (float) Math.Acos((double) quat.W);
      float num1 = (float) Math.Sin((double) a);
      Vector3 vector3;
      if ((double) Math.Abs(num1) < 1.0000000116860974E-07)
      {
        vector3.X = 0.0f;
        vector3.Y = 0.0f;
        vector3.Z = 0.0f;
      }
      else
      {
        float num2 = a / num1;
        vector3.X = quat.X * num2;
        vector3.Y = quat.Y * num2;
        vector3.Z = quat.Z * num2;
      }
      return vector3;
    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    private struct FloatInt
    {
      [FieldOffset(0)]
      public float f;
      [FieldOffset(0)]
      public uint i;
    }
  }
}
