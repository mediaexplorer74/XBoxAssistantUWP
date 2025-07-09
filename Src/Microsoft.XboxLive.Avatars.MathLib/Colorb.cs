// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.Colorb
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll


namespace Microsoft.XboxLive.MathUtilities
{
  public struct Colorb
  {
    public byte blue;
    public byte green;
    public byte red;
    public byte alpha;

    public Colorb(byte red, byte green, byte blue)
    {
      this.red = red;
      this.green = green;
      this.blue = blue;
      this.alpha = byte.MaxValue;
    }

    public Colorb(byte red, byte green, byte blue, byte alpha)
    {
      this.red = red;
      this.green = green;
      this.blue = blue;
      this.alpha = alpha;
    }

    public Colorb(int colorArgb8)
    {
      this.alpha = (byte) (colorArgb8 >> 24);
      this.red = (byte) (colorArgb8 >> 16);
      this.green = (byte) (colorArgb8 >> 8);
      this.blue = (byte) colorArgb8;
    }

    public int CompositeArgb
    {
      get
      {
        return ((int) this.alpha << 24) + ((int) this.red << 16) + ((int) this.green << 8) + (int) this.blue;
      }
      set
      {
        this.alpha = (byte) (value >> 24);
        this.red = (byte) (value >> 16);
        this.green = (byte) (value >> 8);
        this.blue = (byte) value;
      }
    }

    public int CompositeRgb => ((int) this.red << 16) + ((int) this.green << 8) + (int) this.blue;

    public override bool Equals(object obj) => obj is Colorb other && this.Equals(other);

    public bool Equals(Colorb other) => this.CompositeArgb == other.CompositeArgb;

    public static bool operator ==(Colorb color1, Colorb color2) => color1.Equals(color2);

    public static bool operator !=(Colorb color1, Colorb color2) => !color1.Equals(color2);

    public override int GetHashCode() => this.CompositeArgb;
  }
}
