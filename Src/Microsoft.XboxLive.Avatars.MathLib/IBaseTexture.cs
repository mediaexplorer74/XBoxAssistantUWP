// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.IBaseTexture
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll


namespace Microsoft.XboxLive.MathUtilities
{
  public interface IBaseTexture
  {
    int Width { get; }

    int Height { get; }

    bool IsEmptyTransparent { get; }

    bool IsEmptyOpaque { get; }

    void SetPixels(Colorb[] pixels);

    void SetPixels(IBaseTexture sourceTexture);

    Colorb[] GetPixels();

    int GetMemoryUsage();
  }
}
