// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Utilities
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.MathUtilities;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public sealed class Utilities
  {
    private Utilities()
    {
    }

    public static Vector4 Vector4FromInt(int color)
    {
      return new Vector4()
      {
        W = (float) (color >> 24 & (int) byte.MaxValue) / (float) byte.MaxValue,
        X = (float) (color >> 16 & (int) byte.MaxValue) / (float) byte.MaxValue,
        Y = (float) (color >> 8 & (int) byte.MaxValue) / (float) byte.MaxValue,
        Z = (float) (color & (int) byte.MaxValue) / (float) byte.MaxValue
      };
    }

    public static Vector4 ColorbToVector4(Colorb color)
    {
      return new Vector4()
      {
        W = (float) color.alpha * 0.003921569f,
        X = (float) color.red * 0.003921569f,
        Y = (float) color.green * 0.003921569f,
        Z = (float) color.blue * 0.003921569f
      };
    }

    public static Colorb ColorbFromVector4(Vector4 color)
    {
      return new Colorb()
      {
        alpha = (byte) ((double) color.W * (double) byte.MaxValue),
        red = (byte) ((double) color.X * (double) byte.MaxValue),
        green = (byte) ((double) color.Y * (double) byte.MaxValue),
        blue = (byte) ((double) color.Z * (double) byte.MaxValue)
      };
    }

    public static Colorb ColorFromUint(uint color)
    {
      return new Colorb()
      {
        alpha = (byte) (color >> 24 & (uint) byte.MaxValue),
        red = (byte) (color >> 16 & (uint) byte.MaxValue),
        green = (byte) (color >> 8 & (uint) byte.MaxValue),
        blue = (byte) (color & (uint) byte.MaxValue)
      };
    }
  }
}
