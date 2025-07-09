// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.AssetTriangleOverrideParser
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using System;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class AssetTriangleOverrideParser
  {
    public int m_GlobalIndexBufferSize;
    public Guid m_OriginalAssetId;
    private int[] m_aDegeneratedTriangleIndexes;

    public Guid GetOriginalAssetId() => this.m_OriginalAssetId;

    public void Parse(Stream stream)
    {
      BitStream bitStream = new BitStream(stream);
      uint length = bitStream.ReadUint(32);
      if (length > 8192U)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.TriangleOverrideError1));
        throw new AvatarException(Resources.TriangleOverrideError1);
      }
      this.m_GlobalIndexBufferSize = bitStream.ReadInt(32);
      this.m_OriginalAssetId = new Guid(bitStream.ReadByteArray(16));
      this.m_aDegeneratedTriangleIndexes = new int[(IntPtr) length];
      IntegerDataUnpacker integerDataUnpacker = new IntegerDataUnpacker();
      integerDataUnpacker.UnpackHeader(bitStream);
      for (int index = 0; (long) index < (long) length; ++index)
        integerDataUnpacker.UnpackData(bitStream, out this.m_aDegeneratedTriangleIndexes[index]);
    }

    public void Apply(AvatarComponent avatarComponent)
    {
      int length1 = avatarComponent.m_Batches.Length;
      int[] numArray = new int[length1 + 1];
      int num = 0;
      for (int index = 0; index < length1; ++index)
      {
        numArray[index] = num;
        num += avatarComponent.m_Batches[index].Triangles.Length;
      }
      numArray[length1] = num;
      int length2 = this.m_aDegeneratedTriangleIndexes.Length;
      for (int index1 = 0; index1 < length2; ++index1)
      {
        int degeneratedTriangleIndex = this.m_aDegeneratedTriangleIndexes[index1];
        for (int index2 = 1; index2 <= length1; ++index2)
        {
          if (degeneratedTriangleIndex < numArray[index2])
          {
            int index3 = index2 - 1;
            int index4 = degeneratedTriangleIndex - numArray[index3];
            int i1 = avatarComponent.m_Batches[index3].Triangles[index4].i1;
            avatarComponent.m_Batches[index3].Triangles[index4].i2 = i1;
            avatarComponent.m_Batches[index3].Triangles[index4].i3 = i1;
            break;
          }
        }
      }
    }

    public int GetMemoryUsage() => this.m_aDegeneratedTriangleIndexes.Length * 4 + 64;
  }
}
