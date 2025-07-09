// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Assets.TriangleBatch
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.MathUtilities;
using System;


namespace Microsoft.XboxLive.Avatars.Internal.Assets
{
  public struct TriangleBatch
  {
    public IndexedTriangle[] Triangles;
    public VertexDataBuffer Vertices;

    internal TriangleBatch Clone()
    {
      TriangleBatch triangleBatch = new TriangleBatch();
      int length = this.Triangles.Length;
      triangleBatch.Triangles = new IndexedTriangle[length];
      this.Triangles.CopyTo((Array) triangleBatch.Triangles, 0);
      triangleBatch.Vertices = this.Vertices.Clone();
      return triangleBatch;
    }

    internal int GetMemoryUsage()
    {
      return this.Triangles.Length * 12 + (this.Vertices.ColorChannelCount * 4 + this.Vertices.TextureChannelCount * 8 + 48) * this.Vertices.Positions.Length;
    }
  }
}
