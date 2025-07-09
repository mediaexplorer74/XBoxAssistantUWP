// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.BoundingBox2
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll


namespace Microsoft.XboxLive.MathUtilities
{
  public struct BoundingBox2
  {
    public Vector2 minCoord;
    public Vector2 maxCoord;

    public void Clear()
    {
      this.minCoord.X = float.MaxValue;
      this.minCoord.Y = float.MaxValue;
      this.maxCoord.X = float.MinValue;
      this.maxCoord.Y = float.MinValue;
    }

    public void Extend(BoundingBox2 boundingBox2)
    {
      if ((double) boundingBox2.minCoord.X < (double) this.minCoord.X)
        this.minCoord.X = boundingBox2.minCoord.X;
      if ((double) boundingBox2.minCoord.Y < (double) this.minCoord.Y)
        this.minCoord.Y = boundingBox2.minCoord.Y;
      if ((double) boundingBox2.maxCoord.X > (double) this.maxCoord.X)
        this.maxCoord.X = boundingBox2.maxCoord.X;
      if ((double) boundingBox2.maxCoord.Y <= (double) this.maxCoord.Y)
        return;
      this.maxCoord.Y = boundingBox2.maxCoord.Y;
    }

    public void Extend(Vector2 point)
    {
      if ((double) point.X < (double) this.minCoord.X)
        this.minCoord.X = point.X;
      if ((double) point.Y < (double) this.minCoord.Y)
        this.minCoord.Y = point.Y;
      if ((double) point.X > (double) this.maxCoord.X)
        this.maxCoord.X = point.X;
      if ((double) point.Y <= (double) this.maxCoord.Y)
        return;
      this.maxCoord.Y = point.Y;
    }

    public void Extend(float value)
    {
      this.minCoord.X -= value;
      this.minCoord.Y -= value;
      this.maxCoord.X += value;
      this.maxCoord.Y += value;
    }

    public bool IntersectInner(BoundingBox2 boundingBox2)
    {
      return (double) this.minCoord.X < (double) boundingBox2.maxCoord.X && (double) this.minCoord.Y < (double) boundingBox2.maxCoord.Y && (double) this.maxCoord.X > (double) boundingBox2.minCoord.X && (double) this.maxCoord.Y > (double) boundingBox2.minCoord.Y;
    }

    public bool Contains(Vector2 point)
    {
      return (double) point.X >= (double) this.minCoord.X && (double) point.Y >= (double) this.minCoord.Y && (double) point.X <= (double) this.maxCoord.X && (double) point.Y <= (double) this.maxCoord.Y;
    }
  }
}
