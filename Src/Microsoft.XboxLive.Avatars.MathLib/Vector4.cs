// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.Vector4
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll


namespace Microsoft.XboxLive.MathUtilities
{
  public struct Vector4(float x, float y, float z, float w)
  {
    public float X = x;
    public float Y = y;
    public float Z = z;
    public float W = w;

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object obj)
    {
      try
      {
        Vector4 vector4 = (Vector4) obj;
        if ((double) vector4.W == (double) this.W)
        {
          if ((double) vector4.X == (double) this.X)
          {
            if ((double) vector4.Y == (double) this.Y)
            {
              if ((double) vector4.Z == (double) this.Z)
                return true;
            }
          }
        }
      }
      catch
      {
        return false;
      }
      return false;
    }

    public static Vector4 Multiply(Vector4 vector, float scaleFactor)
    {
      Vector4 vector4;
      vector4.X = scaleFactor * vector.X;
      vector4.Y = scaleFactor * vector.Y;
      vector4.Z = scaleFactor * vector.Z;
      vector4.W = scaleFactor * vector.W;
      return vector4;
    }

    public static float DistanceSquared(Vector4 vector1, Vector4 vector2)
    {
      float num1 = vector1.X - vector2.X;
      float num2 = vector1.Y - vector2.Y;
      float num3 = vector1.Z - vector2.Z;
      float num4 = vector1.W - vector2.W;
      return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3 + (double) num4 * (double) num4);
    }
  }
}
