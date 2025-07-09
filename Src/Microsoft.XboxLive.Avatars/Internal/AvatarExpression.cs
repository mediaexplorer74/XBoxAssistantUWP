// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AvatarExpression
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public struct AvatarExpression
  {
    public int MouthLayer;
    public int LeftEyebrowLayer;
    public int RightEyebrowLayer;
    public int LeftEyeLayer;
    public int RightEyeLayer;

    public AvatarMouthTextureIndex Mouth
    {
      get => (AvatarMouthTextureIndex) this.MouthLayer;
      set
      {
        this.MouthLayer = value <= AvatarMouthTextureIndex.PhoneticDTH && value >= AvatarMouthTextureIndex.Invalid ? (int) value : throw new ArgumentOutOfRangeException(nameof (value), "Invalid index");
      }
    }

    public AvatarLeftEyeTextureIndex LeftEye
    {
      get => (AvatarLeftEyeTextureIndex) this.LeftEyeLayer;
      set
      {
        this.LeftEyeLayer = value <= AvatarLeftEyeTextureIndex.Blink && value >= AvatarLeftEyeTextureIndex.Invalid ? (int) value : throw new ArgumentOutOfRangeException(nameof (value), "Invalid index");
      }
    }

    public AvatarRightEyeTextureIndex RightEye
    {
      get => (AvatarRightEyeTextureIndex) this.RightEyeLayer;
      set
      {
        this.RightEyeLayer = value <= AvatarRightEyeTextureIndex.Blink && value >= AvatarRightEyeTextureIndex.Invalid ? (int) value : throw new ArgumentOutOfRangeException(nameof (value), "Invalid index");
      }
    }

    public AvatarEyebrowTextureIndex LeftEyebrow
    {
      get => (AvatarEyebrowTextureIndex) this.LeftEyebrowLayer;
      set
      {
        this.LeftEyebrowLayer = value <= AvatarEyebrowTextureIndex.Raised && value >= AvatarEyebrowTextureIndex.Invalid ? (int) value : throw new ArgumentOutOfRangeException(nameof (value), "Invalid index");
      }
    }

    public AvatarEyebrowTextureIndex RightEyebrow
    {
      get => (AvatarEyebrowTextureIndex) this.RightEyebrowLayer;
      set
      {
        this.RightEyebrowLayer = value <= AvatarEyebrowTextureIndex.Raised && value >= AvatarEyebrowTextureIndex.Invalid ? (int) value : throw new ArgumentOutOfRangeException(nameof (value), "Invalid index");
      }
    }
  }
}
