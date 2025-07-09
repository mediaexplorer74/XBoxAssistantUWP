// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.Matrix
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll

using System;


namespace Microsoft.XboxLive.MathUtilities
{
  public struct Matrix
  {
    public float M11;
    public float M12;
    public float M13;
    public float M14;
    public float M21;
    public float M22;
    public float M23;
    public float M24;
    public float M31;
    public float M32;
    public float M33;
    public float M34;
    public float M41;
    public float M42;
    public float M43;
    public float M44;

    public Matrix(
      float component11,
      float component12,
      float component13,
      float component14,
      float component21,
      float component22,
      float component23,
      float component24,
      float component31,
      float component32,
      float component33,
      float component34,
      float component41,
      float component42,
      float component43,
      float component44)
    {
      this.M11 = component11;
      this.M12 = component12;
      this.M13 = component13;
      this.M14 = component14;
      this.M21 = component21;
      this.M22 = component22;
      this.M23 = component23;
      this.M24 = component24;
      this.M31 = component31;
      this.M32 = component32;
      this.M33 = component33;
      this.M34 = component34;
      this.M41 = component41;
      this.M42 = component42;
      this.M43 = component43;
      this.M44 = component44;
    }

    public Matrix(float scalar)
    {
      this.M11 = scalar;
      this.M12 = 0.0f;
      this.M13 = 0.0f;
      this.M14 = 0.0f;
      this.M21 = 0.0f;
      this.M22 = scalar;
      this.M23 = 0.0f;
      this.M24 = 0.0f;
      this.M31 = 0.0f;
      this.M32 = 0.0f;
      this.M33 = scalar;
      this.M34 = 0.0f;
      this.M41 = 0.0f;
      this.M42 = 0.0f;
      this.M43 = 0.0f;
      this.M44 = scalar;
    }

    public static Matrix Identity
    {
      get
      {
        return new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
      }
    }

    public Vector3 Translation
    {
      get => new Vector3(this.M41, this.M42, this.M43);
      set
      {
        this.M41 = value.X;
        this.M42 = value.Y;
        this.M43 = value.Z;
      }
    }

    public static Matrix Invert(Matrix matrix)
    {
      float num1 = (float) ((double) matrix.M33 * (double) matrix.M44 - (double) matrix.M43 * (double) matrix.M34);
      float num2 = (float) ((double) matrix.M23 * (double) matrix.M44 - (double) matrix.M43 * (double) matrix.M24);
      float num3 = (float) ((double) matrix.M23 * (double) matrix.M34 - (double) matrix.M33 * (double) matrix.M24);
      float num4 = (float) ((double) matrix.M13 * (double) matrix.M44 - (double) matrix.M43 * (double) matrix.M14);
      float num5 = (float) ((double) matrix.M13 * (double) matrix.M34 - (double) matrix.M33 * (double) matrix.M14);
      float num6 = (float) ((double) matrix.M13 * (double) matrix.M24 - (double) matrix.M23 * (double) matrix.M14);
      Matrix matrix1;
      matrix1.M11 = (float) ((double) matrix.M22 * (double) num1 - (double) matrix.M32 * (double) num2 + (double) matrix.M42 * (double) num3);
      matrix1.M12 = (float) (-(double) matrix.M12 * (double) num1 + (double) matrix.M32 * (double) num4 - (double) matrix.M42 * (double) num5);
      matrix1.M13 = (float) ((double) matrix.M12 * (double) num2 - (double) matrix.M22 * (double) num4 + (double) matrix.M42 * (double) num6);
      matrix1.M14 = (float) (-(double) matrix.M12 * (double) num3 + (double) matrix.M22 * (double) num5 - (double) matrix.M32 * (double) num6);
      matrix1.M21 = (float) (-(double) matrix.M21 * (double) num1 + (double) matrix.M31 * (double) num2 - (double) matrix.M41 * (double) num3);
      matrix1.M22 = (float) ((double) matrix.M11 * (double) num1 - (double) matrix.M31 * (double) num4 + (double) matrix.M41 * (double) num5);
      matrix1.M23 = (float) (-(double) matrix.M11 * (double) num2 + (double) matrix.M21 * (double) num4 - (double) matrix.M41 * (double) num6);
      matrix1.M24 = (float) ((double) matrix.M11 * (double) num3 - (double) matrix.M21 * (double) num5 + (double) matrix.M31 * (double) num6);
      float num7 = (float) ((double) matrix.M31 * (double) matrix.M42 - (double) matrix.M41 * (double) matrix.M32);
      float num8 = (float) ((double) matrix.M21 * (double) matrix.M42 - (double) matrix.M41 * (double) matrix.M22);
      float num9 = (float) ((double) matrix.M21 * (double) matrix.M32 - (double) matrix.M31 * (double) matrix.M22);
      float num10 = (float) ((double) matrix.M11 * (double) matrix.M42 - (double) matrix.M41 * (double) matrix.M12);
      float num11 = (float) ((double) matrix.M11 * (double) matrix.M32 - (double) matrix.M31 * (double) matrix.M12);
      float num12 = (float) ((double) matrix.M11 * (double) matrix.M22 - (double) matrix.M21 * (double) matrix.M12);
      matrix1.M31 = (float) ((double) matrix.M24 * (double) num7 - (double) matrix.M34 * (double) num8 + (double) matrix.M44 * (double) num9);
      matrix1.M32 = (float) (-(double) matrix.M14 * (double) num7 + (double) matrix.M34 * (double) num10 - (double) matrix.M44 * (double) num11);
      matrix1.M33 = (float) ((double) matrix.M14 * (double) num8 - (double) matrix.M24 * (double) num10 + (double) matrix.M44 * (double) num12);
      matrix1.M34 = (float) (-(double) matrix.M14 * (double) num9 + (double) matrix.M24 * (double) num11 - (double) matrix.M34 * (double) num12);
      matrix1.M41 = (float) (-(double) matrix.M23 * (double) num7 + (double) matrix.M33 * (double) num8 - (double) matrix.M43 * (double) num9);
      matrix1.M42 = (float) ((double) matrix.M13 * (double) num7 - (double) matrix.M33 * (double) num10 + (double) matrix.M43 * (double) num11);
      matrix1.M43 = (float) (-(double) matrix.M13 * (double) num8 + (double) matrix.M23 * (double) num10 - (double) matrix.M43 * (double) num12);
      matrix1.M44 = (float) ((double) matrix.M13 * (double) num9 - (double) matrix.M23 * (double) num11 + (double) matrix.M33 * (double) num12);
      float num13 = (float) ((double) matrix.M11 * (double) matrix1.M11 + (double) matrix.M21 * (double) matrix1.M12 + (double) matrix.M31 * (double) matrix1.M13 + (double) matrix.M41 * (double) matrix1.M14);
      if ((double) num13 > -9.9999999747524271E-07 && (double) num13 < 9.9999999747524271E-07)
        throw new DivideByZeroException("Invert method failed, matrix is singular");
      float num14 = 1f / num13;
      matrix1.M11 *= num14;
      matrix1.M12 *= num14;
      matrix1.M13 *= num14;
      matrix1.M14 *= num14;
      matrix1.M21 *= num14;
      matrix1.M22 *= num14;
      matrix1.M23 *= num14;
      matrix1.M24 *= num14;
      matrix1.M31 *= num14;
      matrix1.M32 *= num14;
      matrix1.M33 *= num14;
      matrix1.M34 *= num14;
      matrix1.M41 *= num14;
      matrix1.M42 *= num14;
      matrix1.M43 *= num14;
      matrix1.M44 *= num14;
      return matrix1;
    }

    public static Matrix CreateFromQuaternion(Quaternion quaternion)
    {
      Matrix fromQuaternion = new Matrix();
      float num1 = quaternion.X * quaternion.X;
      float num2 = quaternion.X * quaternion.Y;
      float num3 = quaternion.X * quaternion.Z;
      float num4 = quaternion.X * quaternion.W;
      float num5 = quaternion.Y * quaternion.Y;
      float num6 = quaternion.Y * quaternion.Z;
      float num7 = quaternion.Y * quaternion.W;
      float num8 = quaternion.Z * quaternion.Z;
      float num9 = quaternion.Z * quaternion.W;
      fromQuaternion.M11 = (float) (1.0 - 2.0 * ((double) num5 + (double) num8));
      fromQuaternion.M12 = (float) (2.0 * ((double) num2 + (double) num9));
      fromQuaternion.M13 = (float) (2.0 * ((double) num3 - (double) num7));
      fromQuaternion.M14 = 0.0f;
      fromQuaternion.M21 = (float) (2.0 * ((double) num2 - (double) num9));
      fromQuaternion.M22 = (float) (1.0 - 2.0 * ((double) num1 + (double) num8));
      fromQuaternion.M23 = (float) (2.0 * ((double) num6 + (double) num4));
      fromQuaternion.M24 = 0.0f;
      fromQuaternion.M31 = (float) (2.0 * ((double) num3 + (double) num7));
      fromQuaternion.M32 = (float) (2.0 * ((double) num6 - (double) num4));
      fromQuaternion.M33 = (float) (1.0 - 2.0 * ((double) num1 + (double) num5));
      fromQuaternion.M34 = 0.0f;
      fromQuaternion.M41 = 0.0f;
      fromQuaternion.M42 = 0.0f;
      fromQuaternion.M43 = 0.0f;
      fromQuaternion.M44 = 1f;
      return fromQuaternion;
    }

    public static Matrix CreateFromYawPitchRoll(float yaw, float pitch, float roll)
    {
      float num1 = (float) Math.Cos((double) yaw);
      float num2 = (float) Math.Sin((double) yaw);
      float num3 = (float) Math.Cos((double) pitch);
      float num4 = (float) Math.Sin((double) pitch);
      float num5 = (float) Math.Cos((double) roll);
      float num6 = (float) Math.Sin((double) roll);
      Matrix fromYawPitchRoll;
      fromYawPitchRoll.M11 = num3 * num5;
      fromYawPitchRoll.M12 = (float) ((double) num2 * (double) num4 * (double) num5 - (double) num1 * (double) num6);
      fromYawPitchRoll.M13 = (float) ((double) num1 * (double) num4 * (double) num5 + (double) num2 * (double) num6);
      fromYawPitchRoll.M14 = 0.0f;
      fromYawPitchRoll.M21 = num3 * num6;
      fromYawPitchRoll.M22 = (float) ((double) num2 * (double) num4 * (double) num6 + (double) num1 * (double) num5);
      fromYawPitchRoll.M23 = (float) ((double) num1 * (double) num4 * (double) num6 - (double) num2 * (double) num5);
      fromYawPitchRoll.M24 = 0.0f;
      fromYawPitchRoll.M31 = -num4;
      fromYawPitchRoll.M32 = num2 * num3;
      fromYawPitchRoll.M33 = num1 * num3;
      fromYawPitchRoll.M34 = 0.0f;
      fromYawPitchRoll.M41 = 0.0f;
      fromYawPitchRoll.M42 = 0.0f;
      fromYawPitchRoll.M43 = 0.0f;
      fromYawPitchRoll.M44 = 1f;
      return fromYawPitchRoll;
    }

    public static Matrix Multiply(Matrix matrixA, Matrix matrixB)
    {
      Matrix matrix;
      matrix.M11 = (float) ((double) matrixA.M11 * (double) matrixB.M11 + (double) matrixA.M12 * (double) matrixB.M21 + (double) matrixA.M13 * (double) matrixB.M31 + (double) matrixA.M14 * (double) matrixB.M41);
      matrix.M12 = (float) ((double) matrixA.M11 * (double) matrixB.M12 + (double) matrixA.M12 * (double) matrixB.M22 + (double) matrixA.M13 * (double) matrixB.M32 + (double) matrixA.M14 * (double) matrixB.M42);
      matrix.M13 = (float) ((double) matrixA.M11 * (double) matrixB.M13 + (double) matrixA.M12 * (double) matrixB.M23 + (double) matrixA.M13 * (double) matrixB.M33 + (double) matrixA.M14 * (double) matrixB.M43);
      matrix.M14 = (float) ((double) matrixA.M11 * (double) matrixB.M14 + (double) matrixA.M12 * (double) matrixB.M24 + (double) matrixA.M13 * (double) matrixB.M34 + (double) matrixA.M14 * (double) matrixB.M44);
      matrix.M21 = (float) ((double) matrixA.M21 * (double) matrixB.M11 + (double) matrixA.M22 * (double) matrixB.M21 + (double) matrixA.M23 * (double) matrixB.M31 + (double) matrixA.M24 * (double) matrixB.M41);
      matrix.M22 = (float) ((double) matrixA.M21 * (double) matrixB.M12 + (double) matrixA.M22 * (double) matrixB.M22 + (double) matrixA.M23 * (double) matrixB.M32 + (double) matrixA.M24 * (double) matrixB.M42);
      matrix.M23 = (float) ((double) matrixA.M21 * (double) matrixB.M13 + (double) matrixA.M22 * (double) matrixB.M23 + (double) matrixA.M23 * (double) matrixB.M33 + (double) matrixA.M24 * (double) matrixB.M43);
      matrix.M24 = (float) ((double) matrixA.M21 * (double) matrixB.M14 + (double) matrixA.M22 * (double) matrixB.M24 + (double) matrixA.M23 * (double) matrixB.M34 + (double) matrixA.M24 * (double) matrixB.M44);
      matrix.M31 = (float) ((double) matrixA.M31 * (double) matrixB.M11 + (double) matrixA.M32 * (double) matrixB.M21 + (double) matrixA.M33 * (double) matrixB.M31 + (double) matrixA.M34 * (double) matrixB.M41);
      matrix.M32 = (float) ((double) matrixA.M31 * (double) matrixB.M12 + (double) matrixA.M32 * (double) matrixB.M22 + (double) matrixA.M33 * (double) matrixB.M32 + (double) matrixA.M34 * (double) matrixB.M42);
      matrix.M33 = (float) ((double) matrixA.M31 * (double) matrixB.M13 + (double) matrixA.M32 * (double) matrixB.M23 + (double) matrixA.M33 * (double) matrixB.M33 + (double) matrixA.M34 * (double) matrixB.M43);
      matrix.M34 = (float) ((double) matrixA.M31 * (double) matrixB.M14 + (double) matrixA.M32 * (double) matrixB.M24 + (double) matrixA.M33 * (double) matrixB.M34 + (double) matrixA.M34 * (double) matrixB.M44);
      matrix.M41 = (float) ((double) matrixA.M41 * (double) matrixB.M11 + (double) matrixA.M42 * (double) matrixB.M21 + (double) matrixA.M43 * (double) matrixB.M31 + (double) matrixA.M44 * (double) matrixB.M41);
      matrix.M42 = (float) ((double) matrixA.M41 * (double) matrixB.M12 + (double) matrixA.M42 * (double) matrixB.M22 + (double) matrixA.M43 * (double) matrixB.M32 + (double) matrixA.M44 * (double) matrixB.M42);
      matrix.M43 = (float) ((double) matrixA.M41 * (double) matrixB.M13 + (double) matrixA.M42 * (double) matrixB.M23 + (double) matrixA.M43 * (double) matrixB.M33 + (double) matrixA.M44 * (double) matrixB.M43);
      matrix.M44 = (float) ((double) matrixA.M41 * (double) matrixB.M14 + (double) matrixA.M42 * (double) matrixB.M24 + (double) matrixA.M43 * (double) matrixB.M34 + (double) matrixA.M44 * (double) matrixB.M44);
      return matrix;
    }
  }
}
