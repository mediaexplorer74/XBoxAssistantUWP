// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.Vector3dDataUnpacker
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.MathUtilities;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class Vector3dDataUnpacker : DataUnpackerGeneric<Vector3>
  {
    private const float QUANTRADIUS_TO_DELTA_MUL0 = 2f;
    private const float QUANTRADIUS_TO_DELTA_MUL1 = 1.6329931f;
    private const float QUANTRADIUS_TO_DELTA_MUL2 = 1.73205078f;
    private const float DELTA_TO_INSET_X_MUL = 0.5f;
    private const float DELTA_TO_INSET_Z_MUL = 0.577350259f;
    protected int m_BitCountX;
    protected int m_BitCountY;
    protected int m_BitCountZ;
    protected float m_MinX;
    protected float m_MinY;
    protected float m_MinZ;
    protected float m_QuantRadius;
    protected float m_Delta0;
    protected float m_Delta1;
    protected float m_Delta2;

    public override int GetHeaderBitCount() => 146;

    public override int GetPerDataBitCount()
    {
      return this.m_BitCountX + this.m_BitCountY + this.m_BitCountZ;
    }

    public override void UnpackHeader(BitStream bitStream)
    {
      this.m_QuantRadius = bitStream.ReadFloat();
      this.m_MinX = bitStream.ReadFloat();
      this.m_MinY = bitStream.ReadFloat();
      this.m_MinZ = bitStream.ReadFloat();
      this.m_BitCountX = bitStream.ReadInt(6);
      this.m_BitCountY = bitStream.ReadInt(6);
      this.m_BitCountZ = bitStream.ReadInt(6);
      this.m_Delta0 = this.m_QuantRadius * 2f;
      this.m_Delta1 = this.m_QuantRadius * 1.6329931f;
      this.m_Delta2 = this.m_QuantRadius * 1.73205078f;
    }

    public override void UnpackData(BitStream bitStream, out Vector3 data)
    {
      Vector3dDataUnpacker.QuantizedVector quantizedVector;
      quantizedVector.x = bitStream.ReadInt(this.m_BitCountX);
      quantizedVector.y = bitStream.ReadInt(this.m_BitCountY);
      quantizedVector.z = bitStream.ReadInt(this.m_BitCountZ);
      data = new Vector3();
      data.Y = this.m_MinY + this.m_Delta1 * (float) quantizedVector.y;
      data.Z = this.m_MinZ + this.m_Delta2 * (float) quantizedVector.z;
      if ((quantizedVector.y & 1) != 0)
        data.Z += 0.577350259f * this.m_Delta2;
      data.X = this.m_MinX + this.m_Delta0 * (float) quantizedVector.x;
      if ((quantizedVector.y & 1) == (quantizedVector.z & 1))
        return;
      data.X += 0.5f * this.m_Delta0;
    }

    public void InvertCoordinateSystem()
    {
      this.m_MinZ = -this.m_MinZ;
      this.m_Delta2 = -this.m_Delta2;
    }

    private struct QuantizedVector
    {
      public int x;
      public int y;
      public int z;
    }
  }
}
