// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.Quaternion
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll

using System;


namespace Microsoft.XboxLive.MathUtilities
{
  public struct Quaternion
  {
    public float X;
    public float Y;
    public float Z;
    public float W;

    public static Quaternion Add(Quaternion quaternionA, Quaternion quaternionB)
    {
      Quaternion quaternion;
      quaternion.X = quaternionA.X + quaternionB.X;
      quaternion.Y = quaternionA.Y + quaternionB.Y;
      quaternion.Z = quaternionA.Z + quaternionB.Z;
      quaternion.W = quaternionA.W + quaternionB.W;
      return quaternion;
    }

    public static Quaternion Multiply(Quaternion quaternion, float scaleFactor)
    {
      Quaternion quaternion1;
      quaternion1.X = scaleFactor * quaternion.X;
      quaternion1.Y = scaleFactor * quaternion.Y;
      quaternion1.Z = scaleFactor * quaternion.Z;
      quaternion1.W = scaleFactor * quaternion.W;
      return quaternion1;
    }

    public static Quaternion CreateFromRotationMatrix(Matrix matrix)
    {
      float d = (float) ((double) matrix.M11 + (double) matrix.M22 + (double) matrix.M33 + 1.0);
      Quaternion fromRotationMatrix;
      if ((double) d > 1.0 / 1000.0)
      {
        float num = 0.5f / (float) Math.Sqrt((double) d);
        fromRotationMatrix.W = 0.25f / num;
        fromRotationMatrix.X = (matrix.M23 - matrix.M32) * num;
        fromRotationMatrix.Y = (matrix.M31 - matrix.M13) * num;
        fromRotationMatrix.Z = (matrix.M12 - matrix.M21) * num;
      }
      else if ((double) matrix.M11 > (double) matrix.M22 && (double) matrix.M11 > (double) matrix.M33)
      {
        float num1 = (float) Math.Sqrt(1.0 + (double) matrix.M11 - (double) matrix.M22 - (double) matrix.M33);
        fromRotationMatrix.X = 0.5f * num1;
        float num2 = (float) (1.0 / ((double) num1 + (double) num1));
        fromRotationMatrix.Y = (matrix.M12 + matrix.M21) * num2;
        fromRotationMatrix.Z = (matrix.M13 + matrix.M31) * num2;
        fromRotationMatrix.W = (matrix.M23 - matrix.M32) * num2;
      }
      else if ((double) matrix.M22 > (double) matrix.M33)
      {
        float num3 = (float) Math.Sqrt(1.0 + (double) matrix.M22 - (double) matrix.M11 - (double) matrix.M33);
        fromRotationMatrix.Y = 0.5f * num3;
        float num4 = (float) (1.0 / ((double) num3 + (double) num3));
        fromRotationMatrix.X = (matrix.M12 + matrix.M21) * num4;
        fromRotationMatrix.Z = (matrix.M23 + matrix.M32) * num4;
        fromRotationMatrix.W = (matrix.M31 - matrix.M13) * num4;
      }
      else
      {
        float num5 = (float) Math.Sqrt(1.0 + (double) matrix.M33 - (double) matrix.M11 - (double) matrix.M22);
        fromRotationMatrix.Z = 0.5f * num5;
        float num6 = (float) (1.0 / ((double) num5 + (double) num5));
        fromRotationMatrix.X = (matrix.M13 + matrix.M31) * num6;
        fromRotationMatrix.Y = (matrix.M23 + matrix.M32) * num6;
        fromRotationMatrix.W = (matrix.M12 - matrix.M21) * num6;
      }
      return fromRotationMatrix;
    }
  }
}
