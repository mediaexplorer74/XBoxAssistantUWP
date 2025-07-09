// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.Vector2
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll


namespace Microsoft.XboxLive.MathUtilities
{
  public struct Vector2(float positionX, float positionY)
  {
    public float X = positionX;
    public float Y = positionY;

    public static Vector2 Add(Vector2 vector1, Vector2 vector2)
    {
      Vector2 vector2_1;
      vector2_1.X = vector1.X + vector2.X;
      vector2_1.Y = vector1.Y + vector2.Y;
      return vector2_1;
    }

    public static Vector2 Subtract(Vector2 vector1, Vector2 vector2)
    {
      Vector2 vector2_1;
      vector2_1.X = vector1.X - vector2.X;
      vector2_1.Y = vector1.Y - vector2.Y;
      return vector2_1;
    }

    public static Vector2 Multiply(Vector2 value, float scaleFactor)
    {
      Vector2 vector2;
      vector2.X = scaleFactor * value.X;
      vector2.Y = scaleFactor * value.Y;
      return vector2;
    }

    public static float Dot(Vector2 vector1, Vector2 vector2)
    {
      return (float) ((double) vector1.X * (double) vector2.X + (double) vector1.Y * (double) vector2.Y);
    }
  }
}
