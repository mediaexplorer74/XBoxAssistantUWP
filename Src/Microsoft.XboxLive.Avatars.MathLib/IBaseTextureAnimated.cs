// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.IBaseTextureAnimated
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll


namespace Microsoft.XboxLive.MathUtilities
{
  public interface IBaseTextureAnimated : IBaseTexture
  {
    int LayersCount { get; }

    void SetTextureLayer(int layerIndex, Colorb[] pixels);

    Colorb[] GetTextureLayerPixels(int layerIndex);

    void SetTextureLayer(int layerIndex, byte[] data, int dataWidth, int dataHeight);

    void SelectTextureLayer(int layerIndex);
  }
}
