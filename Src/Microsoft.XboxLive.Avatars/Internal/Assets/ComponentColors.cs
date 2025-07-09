// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Assets.ComponentColors
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.MathUtilities;
using System;


namespace Microsoft.XboxLive.Avatars.Internal.Assets
{
  public struct ComponentColors
  {
    public Vector4 CustomColor0;
    public Vector4 CustomColor1;
    public Vector4 CustomColor2;

    public ComponentColors(Colorb[] colors)
    {
      switch (colors.Length)
      {
        case 0:
          this.CustomColor2 = this.CustomColor1 = this.CustomColor0 = Utilities.Vector4FromInt(0);
          break;
        case 1:
          this.CustomColor2 = this.CustomColor1 = this.CustomColor0 = Utilities.Vector4FromInt(colors[0].CompositeArgb);
          break;
        case 3:
          this.CustomColor0 = Utilities.Vector4FromInt(colors[0].CompositeArgb);
          this.CustomColor1 = Utilities.Vector4FromInt(colors[1].CompositeArgb);
          this.CustomColor2 = Utilities.Vector4FromInt(colors[2].CompositeArgb);
          break;
        default:
          this.CustomColor2 = this.CustomColor1 = this.CustomColor0 = Utilities.Vector4FromInt(0);
          throw new ArgumentException(string.Format("Invalid length of the array. Must be one of 0, 1 or 3."));
      }
    }

    public ComponentColors(byte R, byte G, byte B, byte A)
    {
      this.CustomColor2 = this.CustomColor1 = this.CustomColor0 = new Vector4((float) R / (float) byte.MaxValue, (float) G / (float) byte.MaxValue, (float) B / (float) byte.MaxValue, (float) A / (float) byte.MaxValue);
    }

    public Colorb[] ToColorArray()
    {
      return new Colorb[3]
      {
        Utilities.ColorbFromVector4(this.CustomColor0),
        Utilities.ColorbFromVector4(this.CustomColor1),
        Utilities.ColorbFromVector4(this.CustomColor2)
      };
    }
  }
}
