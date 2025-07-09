// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.MatrixMath
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll

using System;


namespace Microsoft.XboxLive.MathUtilities
{
  public static class MatrixMath
  {
    public static Matrix43 Multiply(ref Matrix43 matrixA, ref Matrix43 matrixB)
    {
      Matrix43 matrix43;
      matrix43.M11 = (float) ((double) matrixA.M11 * (double) matrixB.M11 + (double) matrixA.M12 * (double) matrixB.M21 + (double) matrixA.M13 * (double) matrixB.M31);
      matrix43.M21 = (float) ((double) matrixA.M21 * (double) matrixB.M11 + (double) matrixA.M22 * (double) matrixB.M21 + (double) matrixA.M23 * (double) matrixB.M31);
      matrix43.M31 = (float) ((double) matrixA.M31 * (double) matrixB.M11 + (double) matrixA.M32 * (double) matrixB.M21 + (double) matrixA.M33 * (double) matrixB.M31);
      matrix43.M41 = (float) ((double) matrixA.M41 * (double) matrixB.M11 + (double) matrixA.M42 * (double) matrixB.M21 + (double) matrixA.M43 * (double) matrixB.M31) + matrixB.M41;
      matrix43.M12 = (float) ((double) matrixA.M11 * (double) matrixB.M12 + (double) matrixA.M12 * (double) matrixB.M22 + (double) matrixA.M13 * (double) matrixB.M32);
      matrix43.M22 = (float) ((double) matrixA.M21 * (double) matrixB.M12 + (double) matrixA.M22 * (double) matrixB.M22 + (double) matrixA.M23 * (double) matrixB.M32);
      matrix43.M32 = (float) ((double) matrixA.M31 * (double) matrixB.M12 + (double) matrixA.M32 * (double) matrixB.M22 + (double) matrixA.M33 * (double) matrixB.M32);
      matrix43.M42 = (float) ((double) matrixA.M41 * (double) matrixB.M12 + (double) matrixA.M42 * (double) matrixB.M22 + (double) matrixA.M43 * (double) matrixB.M32) + matrixB.M42;
      matrix43.M13 = (float) ((double) matrixA.M11 * (double) matrixB.M13 + (double) matrixA.M12 * (double) matrixB.M23 + (double) matrixA.M13 * (double) matrixB.M33);
      matrix43.M23 = (float) ((double) matrixA.M21 * (double) matrixB.M13 + (double) matrixA.M22 * (double) matrixB.M23 + (double) matrixA.M23 * (double) matrixB.M33);
      matrix43.M33 = (float) ((double) matrixA.M31 * (double) matrixB.M13 + (double) matrixA.M32 * (double) matrixB.M23 + (double) matrixA.M33 * (double) matrixB.M33);
      matrix43.M43 = (float) ((double) matrixA.M41 * (double) matrixB.M13 + (double) matrixA.M42 * (double) matrixB.M23 + (double) matrixA.M43 * (double) matrixB.M33) + matrixB.M43;
      return matrix43;
    }

    public static Matrix Multiply(ref Matrix43 matrixA, ref Matrix matrixB)
    {
      Matrix matrix;
      matrix.M11 = (float) ((double) matrixA.M11 * (double) matrixB.M11 + (double) matrixA.M12 * (double) matrixB.M21 + (double) matrixA.M13 * (double) matrixB.M31);
      matrix.M12 = (float) ((double) matrixA.M11 * (double) matrixB.M12 + (double) matrixA.M12 * (double) matrixB.M22 + (double) matrixA.M13 * (double) matrixB.M32);
      matrix.M13 = (float) ((double) matrixA.M11 * (double) matrixB.M13 + (double) matrixA.M12 * (double) matrixB.M23 + (double) matrixA.M13 * (double) matrixB.M33);
      matrix.M14 = (float) ((double) matrixA.M11 * (double) matrixB.M14 + (double) matrixA.M12 * (double) matrixB.M24 + (double) matrixA.M13 * (double) matrixB.M34);
      matrix.M21 = (float) ((double) matrixA.M21 * (double) matrixB.M11 + (double) matrixA.M22 * (double) matrixB.M21 + (double) matrixA.M23 * (double) matrixB.M31);
      matrix.M22 = (float) ((double) matrixA.M21 * (double) matrixB.M12 + (double) matrixA.M22 * (double) matrixB.M22 + (double) matrixA.M23 * (double) matrixB.M32);
      matrix.M23 = (float) ((double) matrixA.M21 * (double) matrixB.M13 + (double) matrixA.M22 * (double) matrixB.M23 + (double) matrixA.M23 * (double) matrixB.M33);
      matrix.M24 = (float) ((double) matrixA.M21 * (double) matrixB.M14 + (double) matrixA.M22 * (double) matrixB.M24 + (double) matrixA.M23 * (double) matrixB.M34);
      matrix.M31 = (float) ((double) matrixA.M31 * (double) matrixB.M11 + (double) matrixA.M32 * (double) matrixB.M21 + (double) matrixA.M33 * (double) matrixB.M31);
      matrix.M32 = (float) ((double) matrixA.M31 * (double) matrixB.M12 + (double) matrixA.M32 * (double) matrixB.M22 + (double) matrixA.M33 * (double) matrixB.M32);
      matrix.M33 = (float) ((double) matrixA.M31 * (double) matrixB.M13 + (double) matrixA.M32 * (double) matrixB.M23 + (double) matrixA.M33 * (double) matrixB.M33);
      matrix.M34 = (float) ((double) matrixA.M31 * (double) matrixB.M14 + (double) matrixA.M32 * (double) matrixB.M24 + (double) matrixA.M33 * (double) matrixB.M34);
      matrix.M41 = (float) ((double) matrixA.M41 * (double) matrixB.M11 + (double) matrixA.M42 * (double) matrixB.M21 + (double) matrixA.M43 * (double) matrixB.M31) + matrixB.M41;
      matrix.M42 = (float) ((double) matrixA.M41 * (double) matrixB.M12 + (double) matrixA.M42 * (double) matrixB.M22 + (double) matrixA.M43 * (double) matrixB.M32) + matrixB.M42;
      matrix.M43 = (float) ((double) matrixA.M41 * (double) matrixB.M13 + (double) matrixA.M42 * (double) matrixB.M23 + (double) matrixA.M43 * (double) matrixB.M33) + matrixB.M43;
      matrix.M44 = (float) ((double) matrixA.M41 * (double) matrixB.M14 + (double) matrixA.M42 * (double) matrixB.M24 + (double) matrixA.M43 * (double) matrixB.M34) + matrixB.M44;
      return matrix;
    }

    public static Matrix43 InvertEuclidean(Matrix43 matrix)
    {
      Matrix43 matrix43;
      matrix43.M11 = matrix.M11;
      matrix43.M21 = matrix.M12;
      matrix43.M31 = matrix.M13;
      matrix43.M12 = matrix.M21;
      matrix43.M22 = matrix.M22;
      matrix43.M32 = matrix.M23;
      matrix43.M13 = matrix.M31;
      matrix43.M23 = matrix.M32;
      matrix43.M33 = matrix.M33;
      matrix43.M41 = (float) -((double) matrix.M41 * (double) matrix43.M11 + (double) matrix.M42 * (double) matrix43.M21 + (double) matrix.M43 * (double) matrix43.M31);
      matrix43.M42 = (float) -((double) matrix.M41 * (double) matrix43.M12 + (double) matrix.M42 * (double) matrix43.M22 + (double) matrix.M43 * (double) matrix43.M32);
      matrix43.M43 = (float) -((double) matrix.M41 * (double) matrix43.M13 + (double) matrix.M42 * (double) matrix43.M23 + (double) matrix.M43 * (double) matrix43.M33);
      return matrix43;
    }

    public static void SetTranslation(
      ref Matrix matrix,
      float translationX,
      float translationY,
      float translationZ)
    {
      matrix.M41 = translationX;
      matrix.M42 = translationY;
      matrix.M43 = translationZ;
    }

    public static Vector3 GetRotationEuler(Matrix matrix)
    {
      Vector3 rotationEuler = new Vector3();
      if ((double) matrix.M31 < 1.0 && (double) matrix.M31 > -1.0)
      {
        rotationEuler.X = (float) Math.Atan2((double) matrix.M32, (double) matrix.M33);
        rotationEuler.Z = (float) Math.Atan2((double) matrix.M21, (double) matrix.M11);
        rotationEuler.Y = (float) Math.Asin(-(double) matrix.M31);
      }
      else
      {
        rotationEuler.Z = 0.0f;
        if ((double) matrix.M31 < 0.0)
        {
          rotationEuler.Y = 1.57079637f;
          rotationEuler.X = (float) Math.Atan2((double) matrix.M12, (double) matrix.M13);
        }
        else
        {
          rotationEuler.Y = -1.57079637f;
          rotationEuler.X = (float) Math.Atan2(-(double) matrix.M12, -(double) matrix.M13);
        }
      }
      return rotationEuler;
    }

    public static void SetFocalLength(ref Matrix matrix, float focalLength)
    {
      matrix.M14 = matrix.M13 * focalLength;
      matrix.M24 = matrix.M23 * focalLength;
      matrix.M34 = matrix.M33 * focalLength;
      matrix.M44 = matrix.M43 * focalLength;
    }

    public static float GetProjectionFocalLength(Matrix matrix) => matrix.M34 / matrix.M33;

    public static Matrix CreateMatrixFromEulerOffset(
      float angleX,
      float angleY,
      float angleZ,
      float positionX,
      float positionY,
      float positionZ,
      float focalLength)
    {
      Matrix fromYawPitchRoll = Matrix.CreateFromYawPitchRoll(angleX, angleY, angleZ) with
      {
        M41 = positionX,
        M42 = positionY,
        M43 = positionZ
      };
      MatrixMath.SetFocalLength(ref fromYawPitchRoll, focalLength);
      return fromYawPitchRoll;
    }

    public static Matrix CreateMatrixFromEulerOffset(
      Vector3 eulers,
      Vector3 translation,
      float focal)
    {
      return MatrixMath.CreateMatrixFromEulerOffset(eulers.X, eulers.Y, eulers.Z, translation.X, translation.Y, translation.Z, focal);
    }

    public static Matrix InvertEuclidean(Matrix matrix)
    {
      Matrix matrix1 = new Matrix()
      {
        M11 = matrix.M11,
        M21 = matrix.M12,
        M31 = matrix.M13,
        M12 = matrix.M21,
        M22 = matrix.M22,
        M32 = matrix.M23,
        M13 = matrix.M31,
        M23 = matrix.M32,
        M33 = matrix.M33,
        M14 = 0.0f,
        M24 = 0.0f,
        M34 = 0.0f,
        M44 = 1f
      };
      matrix1.M41 = (float) -((double) matrix.M41 * (double) matrix1.M11 + (double) matrix.M42 * (double) matrix1.M21 + (double) matrix.M43 * (double) matrix1.M31);
      matrix1.M42 = (float) -((double) matrix.M41 * (double) matrix1.M12 + (double) matrix.M42 * (double) matrix1.M22 + (double) matrix.M43 * (double) matrix1.M32);
      matrix1.M43 = (float) -((double) matrix.M41 * (double) matrix1.M13 + (double) matrix.M42 * (double) matrix1.M23 + (double) matrix.M43 * (double) matrix1.M33);
      return matrix1;
    }
  }
}
