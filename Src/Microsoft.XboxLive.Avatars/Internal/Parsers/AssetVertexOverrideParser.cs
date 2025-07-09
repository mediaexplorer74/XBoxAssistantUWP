// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.AssetVertexOverrideParser
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.MathUtilities;
using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class AssetVertexOverrideParser
  {
    public int m_GlobalVertexBufferSize;
    public Guid m_OriginalAssetId;
    public CoordinateSystem m_CoordinateSystem;
    private AssetVertexOverrideParser.VertexOverride[] m_VertexData;

    public AssetVertexOverrideParser(CoordinateSystem coordinateSystem)
    {
      this.m_CoordinateSystem = coordinateSystem;
    }

    public Guid GetOriginalAssetId() => this.m_OriginalAssetId;

    public void Parse(Stream stream)
    {
      BitStream bitStream = new BitStream(stream);
      int length = bitStream.ReadInt(32);
      if (length > 8192)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.VertexOverrideError1));
        throw new AvatarException(Resources.VertexOverrideError1);
      }
      this.m_GlobalVertexBufferSize = bitStream.ReadInt(32);
      this.m_OriginalAssetId = new Guid(bitStream.ReadByteArray(16));
      AssetVertexOverrideParser.VertexOverrideParser vertexOverrideParser = new AssetVertexOverrideParser.VertexOverrideParser();
      vertexOverrideParser.UnpackHeader(bitStream);
      this.m_VertexData = new AssetVertexOverrideParser.VertexOverride[length];
      if (this.m_CoordinateSystem == CoordinateSystem.RightHanded)
      {
        for (int index = 0; index < length; ++index)
          vertexOverrideParser.UnpackDataRightHanded(bitStream, out this.m_VertexData[index]);
      }
      else
      {
        vertexOverrideParser.FlipCoordinateSystem();
        for (int index = 0; index < length; ++index)
          vertexOverrideParser.UnpackDataLeftHanded(bitStream, out this.m_VertexData[index]);
      }
    }

    public void Apply(AvatarComponent avatarComponent)
    {
      int length1 = avatarComponent.m_Batches.Length;
      int[] numArray1 = new int[length1 + 1];
      int[] numArray2 = new int[length1];
      int num1 = 0;
      for (int index = 0; index < length1; ++index)
      {
        int num2 = avatarComponent.m_Batches[index].Vertices.TextureChannelCount * 4 + 28;
        numArray1[index] = num1;
        numArray2[index] = num2;
        num1 += avatarComponent.m_Batches[index].Vertices.Positions.Length * num2;
      }
      numArray1[length1] = num1;
      int length2 = this.m_VertexData.Length;
      int index1 = 0;
label_10:
      if (index1 < length2)
      {
        int originalIndex = this.m_VertexData[index1].originalIndex;
        for (int index2 = 1; index2 <= length1; ++index2)
        {
          if (originalIndex < numArray1[index2])
          {
            int index3 = index2 - 1;
            int index4 = (originalIndex - numArray1[index3]) / numArray2[index3];
            avatarComponent.m_Batches[index3].Vertices.Positions[index4] = this.m_VertexData[index1].position;
            avatarComponent.m_Batches[index3].Vertices.Normals[index4] = this.m_VertexData[index1].normal;
            avatarComponent.m_Batches[index3].Vertices.RawPositions[index4] = this.m_VertexData[index1].position;
            avatarComponent.m_Batches[index3].Vertices.RawNormals[index4] = this.m_VertexData[index1].normal;
            avatarComponent.m_Batches[index3].Vertices.SkinBindings[index4] = this.m_VertexData[index1].skinBindings;
            avatarComponent.m_Batches[index3].Vertices.SkinWeights[index4] = this.m_VertexData[index1].skinWeights;
            avatarComponent.m_Batches[index3].Vertices.Color0[index4].red = (float) this.m_VertexData[index1].vertexColor.red / (float) byte.MaxValue;
            avatarComponent.m_Batches[index3].Vertices.Color0[index4].green = (float) this.m_VertexData[index1].vertexColor.green / (float) byte.MaxValue;
            avatarComponent.m_Batches[index3].Vertices.Color0[index4].blue = (float) this.m_VertexData[index1].vertexColor.blue / (float) byte.MaxValue;
            avatarComponent.m_Batches[index3].Vertices.Color0[index4].alpha = (float) this.m_VertexData[index1].vertexColor.alpha / (float) byte.MaxValue;
            ++index1;
            goto label_10;
          }
        }
        Logger.Log((Log) new DebugLog((object) this, Resources.VertexOverrideError2));
        throw new AvatarException(Resources.VertexOverrideError2);
      }
    }

    public int GetMemoryUsage() => this.m_VertexData.Length * 32 + 64;

    private struct VertexOverride
    {
      public int originalIndex;
      public Vector3 position;
      public Vector3 normal;
      public Vector4b skinWeights;
      public Vector4b skinBindings;
      public Colorb vertexColor;
    }

    private class VertexOverrideParser
    {
      private IntegerDataUnpacker m_OriginalIndexPacker = new IntegerDataUnpacker();
      private Vector3dDataUnpacker m_PositionPacker = new Vector3dDataUnpacker();
      private IntegerDataUnpacker m_NormalPacker = new IntegerDataUnpacker();
      private IntegerDataUnpacker m_SkinWeightsPacker = new IntegerDataUnpacker();
      private IntegerDataUnpacker m_SkinBindingsPacker = new IntegerDataUnpacker();
      private IntegerDataUnpacker m_ColorPacker = new IntegerDataUnpacker();

      public void UnpackHeader(BitStream bitStream)
      {
        this.m_OriginalIndexPacker.UnpackHeader(bitStream);
        this.m_PositionPacker.UnpackHeader(bitStream);
        this.m_NormalPacker.UnpackHeader(bitStream);
        this.m_SkinWeightsPacker.UnpackHeader(bitStream);
        this.m_SkinBindingsPacker.UnpackHeader(bitStream);
        this.m_ColorPacker.UnpackHeader(bitStream);
      }

      public void UnpackDataRightHanded(
        BitStream bitStream,
        out AssetVertexOverrideParser.VertexOverride data)
      {
        this.m_OriginalIndexPacker.UnpackData(bitStream, out data.originalIndex);
        this.m_PositionPacker.UnpackData(bitStream, out data.position);
        int data1;
        this.m_NormalPacker.UnpackData(bitStream, out data1);
        data.normal = VectorMath.CreateFromPackedNormal(data1);
        this.m_SkinWeightsPacker.UnpackData(bitStream, out data1);
        data.skinWeights = new Vector4b(data1);
        this.m_SkinBindingsPacker.UnpackData(bitStream, out data1);
        data.skinBindings = new Vector4b(data1);
        this.m_ColorPacker.UnpackData(bitStream, out data1);
        data.vertexColor = new Colorb(data1);
      }

      public void UnpackDataLeftHanded(
        BitStream bitStream,
        out AssetVertexOverrideParser.VertexOverride data)
      {
        this.m_OriginalIndexPacker.UnpackData(bitStream, out data.originalIndex);
        this.m_PositionPacker.UnpackData(bitStream, out data.position);
        int data1;
        this.m_NormalPacker.UnpackData(bitStream, out data1);
        data.normal = VectorMath.CreateFromPackedNormalFlipCoordinates(data1);
        this.m_SkinWeightsPacker.UnpackData(bitStream, out data1);
        data.skinWeights = new Vector4b(data1);
        this.m_SkinBindingsPacker.UnpackData(bitStream, out data1);
        data.skinBindings = new Vector4b(data1);
        this.m_ColorPacker.UnpackData(bitStream, out data1);
        data.vertexColor = new Colorb(data1);
      }

      public void FlipCoordinateSystem() => this.m_PositionPacker.InvertCoordinateSystem();
    }
  }
}
