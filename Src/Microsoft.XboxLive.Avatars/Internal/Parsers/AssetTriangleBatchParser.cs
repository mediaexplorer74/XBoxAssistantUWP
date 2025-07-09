// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.AssetTriangleBatchParser
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.MathUtilities;
using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class AssetTriangleBatchParser
  {
    private TriangleBatch m_Batch;
    private ShaderInstance m_Shader;
    private CoordinateSystem m_CoordinateSystem;
    private uint m_vertexGpuOfs;
    private uint m_vertexSize;

    public TriangleBatch TriangleBatch => this.m_Batch;

    public ShaderInstance ShaderInstance => this.m_Shader;

    public uint VertexGpuOffset => this.m_vertexGpuOfs;

    public uint VertexSize => this.m_vertexSize;

    public AssetTriangleBatchParser(CoordinateSystem coordinateSystem)
    {
      this.m_Batch = new TriangleBatch();
      this.m_Shader = new ShaderInstance();
      this.m_CoordinateSystem = coordinateSystem;
    }

    public void Parse(EndianStream stream)
    {
      BitStream bitStream = new BitStream((Stream) stream);
      this.m_Shader.ShaderId = (ShaderId) bitStream.ReadInt(32);
      uint length = bitStream.ReadUint(5);
      uint triangleCount = bitStream.ReadUint(32);
      uint VertexCount = bitStream.ReadUint(32);
      uint VertexUvCount = bitStream.ReadUint(32);
      uint num1 = bitStream.ReadUint(32);
      int num2 = (int) bitStream.ReadUint(32);
      uint num3 = bitStream.ReadUint(32);
      int num4 = (int) bitStream.ReadUint(32);
      if (VertexUvCount > 6U)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.TriangleParserError1));
        throw new AvatarException(Resources.TriangleParserError1);
      }
      if (triangleCount > 8192U)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.TriangleParserError2));
        throw new AvatarException(Resources.TriangleParserError2);
      }
      if (VertexCount > 8192U)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.TriangleParserError3));
        throw new AvatarException(Resources.TriangleParserError3);
      }
      this.m_vertexGpuOfs = num3;
      this.m_vertexSize = num1;
      if ((int) num1 != 32 + 4 * ((int) VertexUvCount - 1))
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.TriangleParserError4));
        throw new AvatarException(Resources.TriangleParserError4);
      }
      this.m_Batch.Triangles = new IndexedTriangle[(IntPtr) triangleCount];
      this.m_Batch.Vertices = new VertexDataBuffer();
      this.m_Shader.ShaderParameters = new ShaderParameter[(IntPtr) length];
      for (int index = 0; (long) index < (long) length; ++index)
        this.ReadShaderParameter(stream, index);
      this.UnpackVertices(stream, VertexCount, VertexUvCount);
      this.UnpackTriangleData(stream, triangleCount);
    }

    private void UnpackTriangleData(EndianStream stream, uint triangleCount)
    {
      BitStream bitStream = new BitStream((Stream) stream);
      if ((long) bitStream.ReadInt(32) != (long) (3U * triangleCount))
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.TriangleParserError5));
        throw new AvatarException(Resources.TriangleParserError5);
      }
      AssetTriangleBatchParser.TriangleIndicesParser triangleIndicesParser = new AssetTriangleBatchParser.TriangleIndicesParser();
      triangleIndicesParser.UnpackHeader(bitStream);
      this.m_Batch.Triangles = new IndexedTriangle[(IntPtr) triangleCount];
      if (this.m_CoordinateSystem == CoordinateSystem.RightHanded)
      {
        for (uint index = 0; index < triangleCount; ++index)
          triangleIndicesParser.UnpackDataRightHanded(bitStream, out this.m_Batch.Triangles[(IntPtr) index]);
      }
      else
      {
        for (uint index = 0; index < triangleCount; ++index)
          triangleIndicesParser.UnpackDataLeftHanded(bitStream, out this.m_Batch.Triangles[(IntPtr) index]);
      }
    }

    private void UnpackVertices(EndianStream stream, uint VertexCount, uint VertexUvCount)
    {
      BitStream bitStream = new BitStream((Stream) stream);
      uint num = bitStream.ReadUint(32);
      if ((int) num != (int) VertexCount)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.TriangleParserError6));
        throw new AvatarException(Resources.TriangleParserError6);
      }
      AssetTriangleBatchParser.BatchStreamParser batchStreamParser = new AssetTriangleBatchParser.BatchStreamParser(VertexUvCount);
      batchStreamParser.UnpackHeader(bitStream);
      this.m_Batch.Vertices = new VertexDataBuffer((int) VertexUvCount, 1);
      this.m_Batch.Vertices.AllocateVertexCount((int) VertexCount);
      if (this.m_CoordinateSystem == CoordinateSystem.RightHanded)
      {
        for (uint idx = 0; idx < num; ++idx)
          batchStreamParser.UnpackDataRightHanded(bitStream, ref this.m_Batch.Vertices, (int) idx);
      }
      else
      {
        batchStreamParser.FlipCoordinateSystem();
        for (uint idx = 0; idx < num; ++idx)
          batchStreamParser.UnpackDataLeftHanded(bitStream, ref this.m_Batch.Vertices, (int) idx);
      }
    }

    private void ReadShaderParameter(EndianStream stream, int index)
    {
      bool littleEndian = stream.LittleEndian;
      stream.LittleEndian = true;
      this.m_Shader.ShaderParameters[index].type = (ShaderParameterType) stream.ReadInt();
      this.m_Shader.ShaderParameters[index].usage = (ShaderParameterUsage) stream.ReadInt();
      if (this.m_Shader.ShaderParameters[index].type == ShaderParameterType.Texture)
      {
        this.m_Shader.ShaderParameters[index].data.Texture.TextureIndex = stream.ReadShort();
        this.m_Shader.ShaderParameters[index].data.Texture.UvLayer = stream.ReadShort();
        this.m_Shader.ShaderParameters[index].data.Texture.textureWrapMode = (TextureWrapModes) stream.ReadInt();
        stream.ReadInt();
        stream.ReadInt();
      }
      else
      {
        if (this.m_Shader.ShaderParameters[index].type != ShaderParameterType.PixelConstant && this.m_Shader.ShaderParameters[index].type == ShaderParameterType.VertexConstant)
        {
          Logger.Log((Log) new DebugLog((object) this, Resources.TriangleParserError7));
          throw new AvatarException(Resources.TriangleParserError7);
        }
        this.m_Shader.ShaderParameters[index].data.ConstantInt.i0 = stream.ReadInt();
        this.m_Shader.ShaderParameters[index].data.ConstantInt.i1 = stream.ReadInt();
        this.m_Shader.ShaderParameters[index].data.ConstantInt.i2 = stream.ReadInt();
        this.m_Shader.ShaderParameters[index].data.ConstantInt.i3 = stream.ReadInt();
      }
      stream.LittleEndian = littleEndian;
    }

    private class TriangleIndicesParser
    {
      private IntegerDataUnpacker m_idx = new IntegerDataUnpacker(16);

      public void UnpackHeader(BitStream bitStream) => this.m_idx.UnpackHeader(bitStream);

      public void UnpackDataRightHanded(BitStream bitStream, out IndexedTriangle data)
      {
        data = new IndexedTriangle();
        this.m_idx.UnpackData(bitStream, out data.i1);
        this.m_idx.UnpackData(bitStream, out data.i2);
        this.m_idx.UnpackData(bitStream, out data.i3);
      }

      public void UnpackDataLeftHanded(BitStream bitStream, out IndexedTriangle data)
      {
        data = new IndexedTriangle();
        this.m_idx.UnpackData(bitStream, out data.i1);
        this.m_idx.UnpackData(bitStream, out data.i3);
        this.m_idx.UnpackData(bitStream, out data.i2);
      }
    }

    private class BatchStreamParser
    {
      private uint m_uvcnt;
      private Vector3dDataUnpacker m_position = new Vector3dDataUnpacker();
      private IntegerDataUnpacker m_normal = new IntegerDataUnpacker();
      private IntegerDataUnpacker m_weights = new IntegerDataUnpacker();
      private IntegerDataUnpacker m_bindings = new IntegerDataUnpacker();
      private IntegerDataUnpacker m_color = new IntegerDataUnpacker();
      private SmallDataUnpacker<short>[] m_U;
      private SmallDataUnpacker<short>[] m_V;

      public BatchStreamParser(uint UVscnt)
      {
        this.m_uvcnt = UVscnt;
        this.m_U = new SmallDataUnpacker<short>[(IntPtr) this.m_uvcnt];
        this.m_V = new SmallDataUnpacker<short>[(IntPtr) this.m_uvcnt];
      }

      public void UnpackHeader(BitStream bitStream)
      {
        this.m_position.UnpackHeader(bitStream);
        this.m_normal.UnpackHeader(bitStream);
        this.m_weights.UnpackHeader(bitStream);
        this.m_bindings.UnpackHeader(bitStream);
        this.m_color.UnpackHeader(bitStream);
        for (int index = 0; (long) index < (long) this.m_uvcnt; ++index)
        {
          this.m_U[index] = new SmallDataUnpacker<short>(2);
          this.m_V[index] = new SmallDataUnpacker<short>(2);
          this.m_U[index].UnpackHeader(bitStream);
          this.m_V[index].UnpackHeader(bitStream);
        }
      }

      private static Colorf Int2Colorf(int i)
      {
        Colorf colorf;
        colorf.blue = (float) (i & (int) byte.MaxValue) / (float) byte.MaxValue;
        colorf.green = (float) (i >> 8 & (int) byte.MaxValue) / (float) byte.MaxValue;
        colorf.red = (float) (i >> 16 & (int) byte.MaxValue) / (float) byte.MaxValue;
        colorf.alpha = (float) (i >> 24 & (int) byte.MaxValue) / (float) byte.MaxValue;
        return colorf;
      }

      private static float HalfToFloat(ushort x)
      {
        uint num1 = (uint) x & 31744U;
        uint num2 = (uint) x & 1023U;
        switch (num1)
        {
          case 0:
            if (num2 != 0U)
            {
              num1 = 947912704U;
              do
              {
                num1 -= 8388608U;
                num2 <<= 1;
              }
              while (((int) num2 & 1024) == 0);
              num2 &= 1023U;
              break;
            }
            break;
          case 31744:
            num1 = 2139095040U;
            break;
          default:
            num1 = (uint) (((int) num1 << 13) + 939524096);
            break;
        }
        return new EndianSwapper()
        {
          aUint = ((uint) (((int) x & 32768) << 16 | (int) num1 | (int) num2 << 13))
        }.aFloat;
      }

      public void UnpackDataRightHanded(BitStream bitStream, ref VertexDataBuffer data, int idx)
      {
        this.m_position.UnpackData(bitStream, out data.RawPositions[idx]);
        int data1;
        this.m_normal.UnpackData(bitStream, out data1);
        int data2;
        this.m_weights.UnpackData(bitStream, out data2);
        int data3;
        this.m_bindings.UnpackData(bitStream, out data3);
        int data4;
        this.m_color.UnpackData(bitStream, out data4);
        for (int index = 0; (long) index < (long) this.m_uvcnt; ++index)
        {
          short data5;
          this.m_U[index].UnpackData(bitStream, out data5);
          short data6;
          this.m_V[index].UnpackData(bitStream, out data6);
          data.TextureCoordinates[index][idx].X = AssetTriangleBatchParser.BatchStreamParser.HalfToFloat((ushort) data5);
          data.TextureCoordinates[index][idx].Y = AssetTriangleBatchParser.BatchStreamParser.HalfToFloat((ushort) data6);
        }
        data.RawNormals[idx] = VectorMath.CreateFromPackedNormal(data1);
        data.SkinBindings[idx] = new Vector4b(data3);
        data.SkinWeights[idx] = new Vector4b(data2);
        data.Color0[idx] = AssetTriangleBatchParser.BatchStreamParser.Int2Colorf(data4);
      }

      public void UnpackDataLeftHanded(BitStream bitStream, ref VertexDataBuffer data, int idx)
      {
        this.m_position.UnpackData(bitStream, out data.RawPositions[idx]);
        int data1;
        this.m_normal.UnpackData(bitStream, out data1);
        int data2;
        this.m_weights.UnpackData(bitStream, out data2);
        int data3;
        this.m_bindings.UnpackData(bitStream, out data3);
        int data4;
        this.m_color.UnpackData(bitStream, out data4);
        for (int index = 0; (long) index < (long) this.m_uvcnt; ++index)
        {
          short data5;
          this.m_U[index].UnpackData(bitStream, out data5);
          short data6;
          this.m_V[index].UnpackData(bitStream, out data6);
          data.TextureCoordinates[index][idx].X = AssetTriangleBatchParser.BatchStreamParser.HalfToFloat((ushort) data5);
          data.TextureCoordinates[index][idx].Y = AssetTriangleBatchParser.BatchStreamParser.HalfToFloat((ushort) data6);
        }
        data.RawNormals[idx] = VectorMath.CreateFromPackedNormalFlipCoordinates(data1);
        data.SkinBindings[idx] = new Vector4b(data3);
        data.SkinWeights[idx] = new Vector4b(data2);
        data.Color0[idx] = AssetTriangleBatchParser.BatchStreamParser.Int2Colorf(data4);
      }

      public void FlipCoordinateSystem() => this.m_position.InvertCoordinateSystem();
    }
  }
}
