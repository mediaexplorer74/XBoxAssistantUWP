// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.Colorf
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll


namespace Microsoft.XboxLive.MathUtilities
{
  public struct Colorf(float red, float green, float blue, float alpha)
  {
    public float blue = blue;
    public float green = green;
    public float red = red;
    public float alpha = alpha;

    public override bool Equals(object obj) => obj is Colorf other && this.Equals(other);

    public bool Equals(Colorf other)
    {
      return (double) this.red == (double) other.red && (double) this.green == (double) other.green && (double) this.blue == (double) other.blue && (double) this.alpha == (double) other.alpha;
    }

    public static bool operator ==(Colorf color1, Colorf color2) => color1.Equals(color2);

    public static bool operator !=(Colorf color1, Colorf color2) => !color1.Equals(color2);

    public override int GetHashCode()
    {
      return (int) ((double) this.red + (double) byte.MaxValue * (double) this.green + 65280.0 * (double) this.blue + 16711680.0 * (double) this.alpha);
    }

    public int CompositeArgb
    {
      get
      {
        return ((int) ((double) this.alpha * (double) byte.MaxValue) << 24) + ((int) ((double) this.red * (double) byte.MaxValue) << 16) + ((int) ((double) this.green * (double) byte.MaxValue) << 8) + (int) ((double) this.blue * (double) byte.MaxValue);
      }
      set
      {
        this.alpha = (float) (byte) (value >> 24) / (float) byte.MaxValue;
        this.red = (float) (byte) (value >> 16) / (float) byte.MaxValue;
        this.green = (float) (byte) (value >> 8) / (float) byte.MaxValue;
        this.blue = (float) (byte) value / (float) byte.MaxValue;
      }
    }
  }
}
