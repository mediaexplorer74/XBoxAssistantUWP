// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.QuaternionDataUnpacker
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.MathUtilities;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class QuaternionDataUnpacker : Vector3dDataUnpacker
  {
    public void UnpackData(BitStream bitStream, out Quaternion data)
    {
      Vector3 data1;
      this.UnpackData(bitStream, out data1);
      data = QuaternionMath.Exp(data1);
    }

    public new void InvertCoordinateSystem()
    {
      this.m_MinX = -this.m_MinX;
      this.m_Delta0 = -this.m_Delta0;
      this.m_MinY = -this.m_MinY;
      this.m_Delta1 = -this.m_Delta1;
    }
  }
}
