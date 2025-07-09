// *********************************************************
// Type: Microsoft.XboxLive.MathUtilities.VertexDataBuffer
// Assembly: Microsoft.XboxLive.Avatars.MathLib, Version=1.2.0.0, Culture=neutral, PublicKeyToken=7f5f5c78ffd609de
// MVID: 7604E429-D6C9-4874-8BC3-52338DCCDA22
// *********************************************************Microsoft.XboxLive.Avatars.MathLib.dll

using System;


namespace Microsoft.XboxLive.MathUtilities
{
  public class VertexDataBuffer
  {
    private Vector3[] m_Positions;
    private Vector3[] m_Normals;
    private Colorf[][] m_Colors;
    private Vector2[][] m_TextureCoordinates;
    private Vector4b[] m_SkinBindings;
    private Vector4b[] m_SkinWeights;
    private Vector3[] m_RawPositions;
    private Vector3[] m_RawNormals;

    public VertexDataBuffer()
    {
      this.m_TextureCoordinates = new Vector2[8][];
      this.m_Colors = new Colorf[8][];
    }

    public VertexDataBuffer Clone()
    {
      int textureChannelCount = this.TextureChannelCount;
      int colorChannelCount = this.ColorChannelCount;
      VertexDataBuffer vertexDataBuffer = new VertexDataBuffer(textureChannelCount, colorChannelCount);
      if (this.m_Positions != null)
      {
        vertexDataBuffer.m_Positions = new Vector3[this.m_Positions.Length];
        this.m_Positions.CopyTo((Array) vertexDataBuffer.m_Positions, 0);
      }
      for (int index = 0; index < textureChannelCount; ++index)
      {
        if (this.m_TextureCoordinates[index] != null)
        {
          vertexDataBuffer.m_TextureCoordinates[index] = new Vector2[this.m_TextureCoordinates[index].Length];
          this.m_TextureCoordinates[index].CopyTo((Array) vertexDataBuffer.m_TextureCoordinates[index], 0);
        }
      }
      for (int index = 0; index < colorChannelCount; ++index)
      {
        if (this.m_Colors[index] != null)
        {
          vertexDataBuffer.m_Colors[index] = new Colorf[this.m_Colors[index].Length];
          this.m_Colors[index].CopyTo((Array) vertexDataBuffer.m_Colors[index], 0);
        }
      }
      if (this.m_Normals != null)
      {
        vertexDataBuffer.m_Normals = new Vector3[this.m_Normals.Length];
        this.m_Normals.CopyTo((Array) vertexDataBuffer.m_Normals, 0);
      }
      if (this.m_RawPositions != null)
      {
        vertexDataBuffer.m_RawPositions = new Vector3[this.m_RawPositions.Length];
        this.m_RawPositions.CopyTo((Array) vertexDataBuffer.m_RawPositions, 0);
      }
      if (this.m_RawNormals != null)
      {
        vertexDataBuffer.m_RawNormals = new Vector3[this.m_RawNormals.Length];
        this.m_RawNormals.CopyTo((Array) vertexDataBuffer.m_RawNormals, 0);
      }
      if (this.m_SkinBindings != null)
      {
        vertexDataBuffer.m_SkinBindings = new Vector4b[this.m_SkinBindings.Length];
        this.m_SkinBindings.CopyTo((Array) vertexDataBuffer.m_SkinBindings, 0);
      }
      if (this.m_SkinWeights != null)
      {
        vertexDataBuffer.m_SkinWeights = new Vector4b[this.m_SkinWeights.Length];
        this.m_SkinWeights.CopyTo((Array) vertexDataBuffer.m_SkinWeights, 0);
      }
      return vertexDataBuffer;
    }

    public VertexDataBuffer(int textureStreamsCount, int colorStreamsCount)
    {
      this.m_TextureCoordinates = new Vector2[textureStreamsCount][];
      this.m_Colors = new Colorf[colorStreamsCount][];
    }

    public Vector2[] GetTextureChannelByIndex(int index) => this.m_TextureCoordinates[index];

    public void SetTextureChannelByIndex(int index, Vector2[] textureCoordinates)
    {
      this.m_TextureCoordinates[index] = textureCoordinates;
    }

    public Colorf[] GetColors(int index) => this.m_Colors[index];

    public void SetColors(int index, Colorf[] colors) => this.m_Colors[index] = colors;

    public void AllocateVertexCount(int vertexCount)
    {
      int textureChannelCount = this.TextureChannelCount;
      int colorChannelCount = this.ColorChannelCount;
      this.m_Positions = new Vector3[vertexCount];
      this.m_Normals = new Vector3[vertexCount];
      for (int index = 0; index < colorChannelCount; ++index)
        this.m_Colors[index] = new Colorf[vertexCount];
      for (int index = 0; index < textureChannelCount; ++index)
        this.m_TextureCoordinates[index] = new Vector2[vertexCount];
      this.m_SkinBindings = new Vector4b[vertexCount];
      this.m_SkinWeights = new Vector4b[vertexCount];
      this.m_RawPositions = new Vector3[vertexCount];
      this.m_RawNormals = new Vector3[vertexCount];
    }

    public Vector3[] Positions
    {
      get => this.m_Positions;
      set => this.m_Positions = value;
    }

    public Vector3[] Normals
    {
      get => this.m_Normals;
      set => this.m_Normals = value;
    }

    public Vector4b[] SkinWeights
    {
      get => this.m_SkinWeights;
      set => this.m_SkinWeights = value;
    }

    public Vector4b[] SkinBindings
    {
      get => this.m_SkinBindings;
      set => this.m_SkinBindings = value;
    }

    public Vector3[] RawPositions
    {
      get => this.m_RawPositions;
      set => this.m_RawPositions = value;
    }

    public Vector3[] RawNormals
    {
      get => this.m_RawNormals;
      set => this.m_RawNormals = value;
    }

    public int TextureChannelCount => this.m_TextureCoordinates.Length;

    public int ColorChannelCount => this.m_Colors.Length;

    public Vector2[][] TextureCoordinates
    {
      get => this.m_TextureCoordinates;
      set => this.m_TextureCoordinates = value;
    }

    public Vector2[] TextureCoordinateChannel0
    {
      get => this.m_TextureCoordinates[0];
      set => this.m_TextureCoordinates[0] = value;
    }

    public Vector2[] TextureCoordinateChannel1
    {
      get => this.m_TextureCoordinates[1];
      set => this.m_TextureCoordinates[1] = value;
    }

    public Vector2[] TextureCoordinateChannel2
    {
      get => this.m_TextureCoordinates[2];
      set => this.m_TextureCoordinates[2] = value;
    }

    public Vector2[] TextureCoordinateChannel3
    {
      get => this.m_TextureCoordinates[3];
      set => this.m_TextureCoordinates[3] = value;
    }

    public Vector2[] TextureCoordinateChannel4
    {
      get => this.m_TextureCoordinates[4];
      set => this.m_TextureCoordinates[4] = value;
    }

    public Vector2[] TextureCoordinateChannel5
    {
      get => this.m_TextureCoordinates[5];
      set => this.m_TextureCoordinates[5] = value;
    }

    public Vector2[] TextureCoordinateChannel6
    {
      get => this.m_TextureCoordinates[6];
      set => this.m_TextureCoordinates[6] = value;
    }

    public Vector2[] TextureCoordinateChannel7
    {
      get => this.m_TextureCoordinates[7];
      set => this.m_TextureCoordinates[7] = value;
    }

    public Colorf[] Color0
    {
      get => this.m_Colors[0];
      set => this.m_Colors[0] = value;
    }

    public Colorf[] Color1
    {
      get => this.m_Colors[1];
      set => this.m_Colors[1] = value;
    }

    public Colorf[] Color2
    {
      get => this.m_Colors[2];
      set => this.m_Colors[2] = value;
    }

    public Colorf[] Color3
    {
      get => this.m_Colors[3];
      set => this.m_Colors[3] = value;
    }
  }
}
