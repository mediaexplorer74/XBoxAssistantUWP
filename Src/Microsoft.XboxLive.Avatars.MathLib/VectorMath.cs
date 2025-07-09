// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.VectorMath
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll


namespace Microsoft.XboxLive.MathUtilities
{
  public sealed class VectorMath
  {
    private VectorMath()
    {
    }

    public static Vector3 Transform(Vector3 vector, Matrix43 matrix)
    {
      Vector3 vector3;
      vector3.X = (float) ((double) vector.X * (double) matrix.M11 + (double) vector.Y * (double) matrix.M21 + (double) vector.Z * (double) matrix.M31) + matrix.M41;
      vector3.Y = (float) ((double) vector.X * (double) matrix.M12 + (double) vector.Y * (double) matrix.M22 + (double) vector.Z * (double) matrix.M32) + matrix.M42;
      vector3.Z = (float) ((double) vector.X * (double) matrix.M13 + (double) vector.Y * (double) matrix.M23 + (double) vector.Z * (double) matrix.M33) + matrix.M43;
      return vector3;
    }

    public static float Cross(Vector2 vector1, Vector2 vector2)
    {
      return (float) ((double) vector1.X * (double) vector2.Y - (double) vector2.X * (double) vector1.Y);
    }

    public static Vector3 CreateFromPackedNormal(int packedNormal)
    {
      if (packedNormal == 0)
        return new Vector3(0.01f, 0.01f, 0.01f);
      return Vector3.Normalize(new Vector3()
      {
        X = (float) ((packedNormal & 2047) << 21 >> 21) / 1023f,
        Y = (float) ((packedNormal & 4192256) << 10 >> 21) / 1023f,
        Z = (float) (packedNormal >> 22) / 511f
      });
    }

    public static Vector3 CreateFromPackedNormalFlipCoordinates(int packedNormal)
    {
      if (packedNormal == 0)
        return new Vector3(0.01f, 0.01f, -0.01f);
      return Vector3.Normalize(new Vector3()
      {
        X = (float) ((packedNormal & 2047) << 21 >> 21) / 1023f,
        Y = (float) ((packedNormal & 4192256) << 10 >> 21) / 1023f,
        Z = (float) (packedNormal >> 22) / -511f
      });
    }

    public static float FullAngle(Vector3 vectorA, Vector3 vectorB, Vector3 normal)
    {
      float num = Vector3.Dot(vectorA, vectorB);
      if ((double) Vector3.Dot(Vector3.Cross(vectorA, vectorB), normal) >= 9.9999997473787516E-06)
        num = 2f - num;
      return num;
    }

    public static float DotSat(Vector3 vectorA, Vector3 vectorB)
    {
      float num = (float) ((double) vectorA.X * (double) vectorB.X + (double) vectorA.Y * (double) vectorB.Y + (double) vectorA.Z * (double) vectorB.Z);
      return (double) num < 0.0 ? 0.0f : num;
    }

    public static int VectorOrientation(Vector3 value)
    {
      int num1 = 0;
      float num2 = value.X;
      float num3 = value.X * value.X;
      float num4;
      if ((double) (num4 = value.Y * value.Y) > (double) num3)
      {
        num3 = num4;
        num2 = value.Y;
        num1 = 2;
      }
      float num5;
      if ((double) (num5 = value.Z * value.Z) > (double) num3)
      {
        num2 = value.Z;
        num1 = 4;
      }
      if ((double) num2 < 0.0)
        num1 |= 1;
      return num1;
    }
  }
}
