// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.Vector3
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll

using System;
using System.Runtime.InteropServices;


namespace Microsoft.XboxLive.MathUtilities
{
  [StructLayout(LayoutKind.Explicit, Size = 12)]
  public struct Vector3(float positionX, float positionY, float positionZ)
  {
    [FieldOffset(0)]
    public float X = positionX;
    [FieldOffset(4)]
    public float Y = positionY;
    [FieldOffset(8)]
    public float Z = positionZ;

    public static Vector3 Subtract(Vector3 vector1, Vector3 vector2)
    {
      Vector3 vector3;
      vector3.X = vector1.X - vector2.X;
      vector3.Y = vector1.Y - vector2.Y;
      vector3.Z = vector1.Z - vector2.Z;
      return vector3;
    }

    public static Vector3 Add(Vector3 value1, Vector3 value2)
    {
      Vector3 vector3;
      vector3.X = value1.X + value2.X;
      vector3.Y = value1.Y + value2.Y;
      vector3.Z = value1.Z + value2.Z;
      return vector3;
    }

    public static Vector3 Multiply(Vector3 value, float scaleFactor)
    {
      Vector3 vector3;
      vector3.X = scaleFactor * value.X;
      vector3.Y = scaleFactor * value.Y;
      vector3.Z = scaleFactor * value.Z;
      return vector3;
    }

    public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
    {
      Vector3 vector3;
      vector3.X = (float) ((double) vector1.Y * (double) vector2.Z - (double) vector1.Z * (double) vector2.Y);
      vector3.Y = (float) ((double) vector1.Z * (double) vector2.X - (double) vector1.X * (double) vector2.Z);
      vector3.Z = (float) ((double) vector1.X * (double) vector2.Y - (double) vector1.Y * (double) vector2.X);
      return vector3;
    }

    public static float Dot(Vector3 vector1, Vector3 vector2)
    {
      return (float) ((double) vector1.X * (double) vector2.X + (double) vector1.Y * (double) vector2.Y + (double) vector1.Z * (double) vector2.Z);
    }

    public static Vector3 Normalize(Vector3 value)
    {
      float num = 1f / (float) Math.Sqrt((double) value.X * (double) value.X + (double) value.Y * (double) value.Y + (double) value.Z * (double) value.Z);
      Vector3 vector3;
      vector3.X = value.X * num;
      vector3.Y = value.Y * num;
      vector3.Z = value.Z * num;
      return vector3;
    }

    public float Length()
    {
      return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z);
    }
  }
}
