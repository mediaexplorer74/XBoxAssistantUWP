// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.Matrix33
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll

using System;


namespace Microsoft.XboxLive.MathUtilities
{
  public struct Matrix33
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
  }
}
