// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Assets.Limits
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll


namespace Microsoft.XboxLive.Avatars.Internal.Assets
{
  internal class Limits
  {
    public const uint MaxTexturesPerComponent = 18;
    public const uint MaxUvStreams = 6;
    public const uint MaxTextureWidth = 1024;
    public const uint MaxTextureHeight = 1024;
    public const uint MaxTrianglesPerBatch = 8192;
    public const uint MaxVerticesPerBatch = 8192;
    public const uint MaxBatchesPerModel = 16;
    public const uint MaxTextureLayers = 14;
    public const uint MaxPackedItems = 1048576;
    public const uint MaxCompressedBlockSize = 40000;

    private Limits()
    {
    }
  }
}
