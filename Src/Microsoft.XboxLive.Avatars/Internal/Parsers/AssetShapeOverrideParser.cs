// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.AssetShapeOverrideParser
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class AssetShapeOverrideParser
  {
    private AssetTriangleOverrideParser m_TriangleOverride;
    private AssetVertexOverrideParser m_VertexOverride;

    public AssetShapeOverrideParser(CoordinateSystem coordinateSystem)
    {
      this.m_VertexOverride = new AssetVertexOverrideParser(coordinateSystem);
      this.m_TriangleOverride = new AssetTriangleOverrideParser();
    }

    public void Parse(Stream stream)
    {
      this.m_TriangleOverride.Parse(stream);
      this.m_VertexOverride.Parse(stream);
    }

    public Guid GetTargetAssetId() => this.m_TriangleOverride.GetOriginalAssetId();

    public bool Apply(AvatarComponent model)
    {
      Guid assetId = model.AssetId;
      if (assetId != this.m_TriangleOverride.GetOriginalAssetId() || assetId != this.m_VertexOverride.GetOriginalAssetId())
        return false;
      int length = model.m_Batches.Length;
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < length; ++index)
      {
        num1 += model.m_Batches[index].Triangles.Length * 6;
        num2 += model.m_Batches[index].Vertices.Positions.Length * (28 + model.m_Batches[index].Vertices.TextureChannelCount * 4);
      }
      int num3 = num2 + (int) sbyte.MaxValue & (int) sbyte.MinValue;
      if (num1 != this.m_TriangleOverride.m_GlobalIndexBufferSize || num3 != this.m_VertexOverride.m_GlobalVertexBufferSize)
        return false;
      this.m_TriangleOverride.Apply(model);
      this.m_VertexOverride.Apply(model);
      return true;
    }

    public int GetMemoryUsage()
    {
      return this.m_TriangleOverride.GetMemoryUsage() + this.m_VertexOverride.GetMemoryUsage() + 16;
    }
  }
}
