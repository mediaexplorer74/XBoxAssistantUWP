// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.AssetModelParser
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.MathUtilities;
using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class AssetModelParser
  {
    private AvatarComponent m_Model;
    private CoordinateSystem m_CoordinateSystem;

    public AssetModelParser(AvatarComponent model, CoordinateSystem coordinateSystem)
    {
      this.m_Model = model;
      this.m_CoordinateSystem = coordinateSystem;
    }

    public void Parse(EndianStream stream, IResourceFactory resourceFactory)
    {
      bool littleEndian = stream.LittleEndian;
      stream.LittleEndian = true;
      stream.ReadInt();
      stream.ReadInt();
      stream.ReadInt();
      stream.ReadInt();
      stream.ReadInt();
      uint length1 = stream.ReadUInt();
      uint length2 = stream.ReadUInt();
      uint num = stream.ReadUInt();
      stream.ReadInt();
      stream.ReadInt();
      stream.ReadInt();
      stream.ReadInt();
      if (length1 > 16U)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.ModelParserError1));
        throw new AvatarException(Resources.ModelParserError1);
      }
      if (length2 > 18U)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.ModelParserError2));
        throw new AvatarException(Resources.ModelParserError2);
      }
      this.m_Model.m_Batches = new TriangleBatch[(IntPtr) length1];
      this.m_Model.m_ShaderInstance = new ShaderInstance[(IntPtr) length1];
      this.m_Model.m_Textures = new IBaseTextureAnimated[(IntPtr) length2];
      uint[] offsetsTable = new uint[(IntPtr) length1];
      uint[] vertexSizesTable = new uint[(IntPtr) length1];
      for (int index = 0; (long) index < (long) length1; ++index)
      {
        AssetTriangleBatchParser triangleBatchParser = new AssetTriangleBatchParser(this.m_CoordinateSystem);
        triangleBatchParser.Parse(stream);
        this.m_Model.m_Batches[index] = triangleBatchParser.TriangleBatch;
        this.m_Model.m_ShaderInstance[index] = triangleBatchParser.ShaderInstance;
        offsetsTable[index] = triangleBatchParser.VertexGpuOffset - num;
        vertexSizesTable[index] = triangleBatchParser.VertexSize;
      }
      for (int index = 0; (long) index < (long) length2; ++index)
      {
        stream.LittleEndian = true;
        stream.ReadInt();
        stream.ReadInt();
        stream.LittleEndian = littleEndian;
        AssetTextureParser assetTextureParser = new AssetTextureParser();
        assetTextureParser.Parse(stream, resourceFactory);
        this.m_Model.m_Textures[index] = assetTextureParser.Texture;
      }
      this.ReadVertexPairs(stream, offsetsTable, vertexSizesTable);
    }

    private static void GetBatchAndIndex(
      uint gpuOffset,
      uint[] offsetsTable,
      uint[] vertexSizesTable,
      out uint batchIdx,
      out uint vertexIdx)
    {
      batchIdx = 0U;
      while ((long) batchIdx < (long) offsetsTable.Length && gpuOffset >= offsetsTable[(IntPtr) batchIdx])
        ++batchIdx;
      --batchIdx;
      gpuOffset -= offsetsTable[(IntPtr) batchIdx];
      vertexIdx = gpuOffset / vertexSizesTable[(IntPtr) batchIdx];
    }

    private void ReadVertexPairs(EndianStream stream, uint[] offsetsTable, uint[] vertexSizesTable)
    {
      IntegerDataUnpacker integerDataUnpacker = new IntegerDataUnpacker();
label_1:
      try
      {
        BitStream bitStream = new BitStream((Stream) stream);
        int num = bitStream.ReadInt(32);
        if (num > 400)
        {
          Logger.Log((Log) new DebugLog((object) this, Resources.ModelParserError3));
          throw new AvatarException(Resources.ModelParserError3);
        }
        if (num % 2 > 0)
          return;
        integerDataUnpacker.UnpackHeader(bitStream);
        for (int index = 0; index < num; index += 2)
        {
          int data1;
          integerDataUnpacker.UnpackData(bitStream, out data1);
          int data2;
          integerDataUnpacker.UnpackData(bitStream, out data2);
          uint batchIdx1;
          uint vertexIdx1;
          AssetModelParser.GetBatchAndIndex((uint) data1, offsetsTable, vertexSizesTable, out batchIdx1, out vertexIdx1);
          uint batchIdx2;
          uint vertexIdx2;
          AssetModelParser.GetBatchAndIndex((uint) data2, offsetsTable, vertexSizesTable, out batchIdx2, out vertexIdx2);
          this.m_Model.m_Batches[(IntPtr) batchIdx2].Vertices.RawPositions[(IntPtr) vertexIdx2] = this.m_Model.m_Batches[(IntPtr) batchIdx1].Vertices.RawPositions[(IntPtr) vertexIdx1];
          this.m_Model.m_Batches[(IntPtr) batchIdx2].Vertices.SkinWeights[(IntPtr) vertexIdx2] = this.m_Model.m_Batches[(IntPtr) batchIdx1].Vertices.SkinWeights[(IntPtr) vertexIdx1];
          this.m_Model.m_Batches[(IntPtr) batchIdx2].Vertices.SkinBindings[(IntPtr) vertexIdx2] = this.m_Model.m_Batches[(IntPtr) batchIdx1].Vertices.SkinBindings[(IntPtr) vertexIdx1];
        }
        goto label_1;
      }
      catch (IndexOutOfRangeException ex)
      {
        Logger.Log((Log) new DebugLog((object) this, string.Format("An error \"{0}\" occured while parsing asset {1}", (object) Resources.ModelParserError4, (object) this.m_Model.AssetId, (object) this.ToString())));
        throw new AvatarException(Resources.ModelParserError4);
      }
    }
  }
}
