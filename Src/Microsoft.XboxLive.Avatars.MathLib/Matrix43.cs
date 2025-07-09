// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.Matrix43
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll

using System;


namespace Microsoft.XboxLive.MathUtilities
{
  public struct Matrix43
  {
    public float M11;
    public float M12;
    public float M13;
    public float M21;
    public float M22;
    public float M23;
    public float M31;
    public float M32;
    public float M33;
    public float M41;
    public float M42;
    public float M43;

    public void SetIdentity()
    {
      this.M11 = 1f;
      this.M12 = 0.0f;
      this.M13 = 0.0f;
      this.M21 = 0.0f;
      this.M22 = 1f;
      this.M23 = 0.0f;
      this.M31 = 0.0f;
      this.M32 = 0.0f;
      this.M33 = 1f;
      this.M41 = 0.0f;
      this.M42 = 0.0f;
      this.M43 = 0.0f;
    }

    public void ClearRotation()
    {
      this.M11 = 1f;
      this.M12 = 0.0f;
      this.M13 = 0.0f;
      this.M21 = 0.0f;
      this.M22 = 1f;
      this.M23 = 0.0f;
      this.M31 = 0.0f;
      this.M32 = 0.0f;
      this.M33 = 1f;
    }

    public Matrix43 InvertEuclidean()
    {
      Matrix43 matrix43;
      matrix43.M11 = this.M11;
      matrix43.M21 = this.M12;
      matrix43.M31 = this.M13;
      matrix43.M12 = this.M21;
      matrix43.M22 = this.M22;
      matrix43.M32 = this.M23;
      matrix43.M13 = this.M31;
      matrix43.M23 = this.M32;
      matrix43.M33 = this.M33;
      matrix43.M41 = (float) -((double) this.M41 * (double) matrix43.M11 + (double) this.M42 * (double) matrix43.M21 + (double) this.M43 * (double) matrix43.M31);
      matrix43.M42 = (float) -((double) this.M41 * (double) matrix43.M12 + (double) this.M42 * (double) matrix43.M22 + (double) this.M43 * (double) matrix43.M32);
      matrix43.M43 = (float) -((double) this.M41 * (double) matrix43.M13 + (double) this.M42 * (double) matrix43.M23 + (double) this.M43 * (double) matrix43.M33);
      return matrix43;
    }

    public Matrix43 Invert()
    {
      float num1 = (float) ((double) this.M11 * ((double) this.M33 * (double) this.M22 - (double) this.M32 * (double) this.M23) - (double) this.M21 * ((double) this.M33 * (double) this.M12 - (double) this.M32 * (double) this.M13) + (double) this.M31 * ((double) this.M23 * (double) this.M12 - (double) this.M22 * (double) this.M13));
      if ((double) num1 > -9.9999999747524271E-07 && (double) num1 < 9.9999999747524271E-07)
        throw new DivideByZeroException("Invert method failed, matrix is singular");
      float num2 = 1f / num1;
      Matrix43 matrix43;
      matrix43.M11 = num2 * (float) ((double) this.M33 * (double) this.M22 - (double) this.M32 * (double) this.M23);
      matrix43.M12 = num2 * (float) ((double) this.M13 * (double) this.M32 - (double) this.M12 * (double) this.M33);
      matrix43.M13 = num2 * (float) ((double) this.M23 * (double) this.M12 - (double) this.M22 * (double) this.M13);
      matrix43.M21 = num2 * (float) ((double) this.M31 * (double) this.M23 - (double) this.M33 * (double) this.M21);
      matrix43.M22 = num2 * (float) ((double) this.M11 * (double) this.M33 - (double) this.M13 * (double) this.M31);
      matrix43.M23 = num2 * (float) ((double) this.M21 * (double) this.M13 - (double) this.M23 * (double) this.M11);
      matrix43.M31 = num2 * (float) ((double) this.M32 * (double) this.M21 - (double) this.M31 * (double) this.M22);
      matrix43.M32 = num2 * (float) ((double) this.M12 * (double) this.M31 - (double) this.M11 * (double) this.M32);
      matrix43.M33 = num2 * (float) ((double) this.M22 * (double) this.M11 - (double) this.M21 * (double) this.M12);
      matrix43.M41 = (float) -((double) matrix43.M11 * (double) this.M41 + (double) matrix43.M21 * (double) this.M42 + (double) matrix43.M31 * (double) this.M43);
      matrix43.M42 = (float) -((double) matrix43.M12 * (double) this.M41 + (double) matrix43.M22 * (double) this.M42 + (double) matrix43.M32 * (double) this.M43);
      matrix43.M43 = (float) -((double) matrix43.M13 * (double) this.M41 + (double) matrix43.M23 * (double) this.M42 + (double) matrix43.M33 * (double) this.M43);
      return matrix43;
    }

    public void Scale(ref Vector3 scaleFactor)
    {
      this.M11 *= scaleFactor.X;
      this.M12 *= scaleFactor.Y;
      this.M13 *= scaleFactor.Z;
      this.M21 *= scaleFactor.X;
      this.M22 *= scaleFactor.Y;
      this.M23 *= scaleFactor.Z;
      this.M31 *= scaleFactor.X;
      this.M32 *= scaleFactor.Y;
      this.M33 *= scaleFactor.Z;
    }

    public void SetTranslation(float translationX, float translationY, float translationZ)
    {
      this.M41 = translationX;
      this.M42 = translationY;
      this.M43 = translationZ;
    }

    public void SetTranslation(Vector3 translation)
    {
      this.M41 = translation.X;
      this.M42 = translation.Y;
      this.M43 = translation.Z;
    }

    public Vector3 GetTranslation()
    {
      return new Vector3()
      {
        X = this.M41,
        Y = this.M42,
        Z = this.M43
      };
    }

    public Quaternion GetRotationQuaternion()
    {
      Quaternion rotationQuaternion = new Quaternion();
      float num1 = (float) ((double) this.M11 + (double) this.M22 + (double) this.M33 + 1.0);
      if ((double) num1 > 0.0099999997764825821)
      {
        rotationQuaternion.W = num1;
        rotationQuaternion.X = this.M23 - this.M32;
        rotationQuaternion.Y = this.M31 - this.M13;
        rotationQuaternion.Z = this.M12 - this.M21;
      }
      else if ((double) this.M11 > (double) this.M22 && (double) this.M11 > (double) this.M33)
      {
        rotationQuaternion.W = this.M32 - this.M23;
        rotationQuaternion.X = (float) ((double) this.M33 + (double) this.M22 - (double) this.M11 - 1.0);
        rotationQuaternion.Y = (float) -((double) this.M21 + (double) this.M12);
        rotationQuaternion.Z = (float) -((double) this.M31 + (double) this.M13);
      }
      else if ((double) this.M22 > (double) this.M33)
      {
        rotationQuaternion.W = this.M31 - this.M13;
        rotationQuaternion.X = this.M21 + this.M12;
        rotationQuaternion.Y = 1f - this.M11 + this.M22 - this.M33;
        rotationQuaternion.Z = this.M32 + this.M23;
      }
      else
      {
        rotationQuaternion.W = this.M21 - this.M12;
        rotationQuaternion.X = (float) -((double) this.M31 + (double) this.M13);
        rotationQuaternion.Y = (float) -((double) this.M32 + (double) this.M23);
        rotationQuaternion.Z = (float) ((double) this.M11 + (double) this.M22 - (double) this.M33 - 1.0);
      }
      float num2 = 1f / (float) Math.Sqrt((double) rotationQuaternion.X * (double) rotationQuaternion.X + (double) rotationQuaternion.Y * (double) rotationQuaternion.Y + (double) rotationQuaternion.Z * (double) rotationQuaternion.Z + (double) rotationQuaternion.W * (double) rotationQuaternion.W);
      rotationQuaternion.X *= num2;
      rotationQuaternion.Y *= num2;
      rotationQuaternion.Z *= num2;
      rotationQuaternion.W *= num2;
      return rotationQuaternion;
    }

    public void SetRotationEuler(float angleX, float angleY, float angleZ)
    {
      float num1 = (float) Math.Cos((double) angleX);
      float num2 = (float) Math.Sin((double) angleX);
      float num3 = (float) Math.Cos((double) angleY);
      float num4 = (float) Math.Sin((double) angleY);
      float num5 = (float) Math.Cos((double) angleZ);
      float num6 = (float) Math.Sin((double) angleZ);
      this.M11 = num3 * num5;
      this.M12 = (float) ((double) num2 * (double) num4 * (double) num5 - (double) num1 * (double) num6);
      this.M13 = (float) ((double) num1 * (double) num4 * (double) num5 + (double) num2 * (double) num6);
      this.M21 = num3 * num6;
      this.M22 = (float) ((double) num2 * (double) num4 * (double) num6 + (double) num1 * (double) num5);
      this.M23 = (float) ((double) num1 * (double) num4 * (double) num6 - (double) num2 * (double) num5);
      this.M31 = -num4;
      this.M32 = num2 * num3;
      this.M33 = num1 * num3;
    }

    public static Matrix43 CreateFromQuaternion(Quaternion quaternion)
    {
      Matrix43 fromQuaternion = new Matrix43();
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
      fromQuaternion.M21 = (float) (2.0 * ((double) num2 - (double) num9));
      fromQuaternion.M22 = (float) (1.0 - 2.0 * ((double) num1 + (double) num8));
      fromQuaternion.M23 = (float) (2.0 * ((double) num6 + (double) num4));
      fromQuaternion.M31 = (float) (2.0 * ((double) num3 + (double) num7));
      fromQuaternion.M32 = (float) (2.0 * ((double) num6 - (double) num4));
      fromQuaternion.M33 = (float) (1.0 - 2.0 * ((double) num1 + (double) num5));
      fromQuaternion.M41 = 0.0f;
      fromQuaternion.M42 = 0.0f;
      fromQuaternion.M43 = 0.0f;
      return fromQuaternion;
    }
  }
}
