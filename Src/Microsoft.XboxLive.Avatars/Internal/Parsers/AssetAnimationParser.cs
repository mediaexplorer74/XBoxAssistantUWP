// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.AssetAnimationParser
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Animations;
using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.MathUtilities;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class AssetAnimationParser
  {
    private float m_FramesPerSecond;
    private int m_SecondaryOffset;
    private int m_TexturesOffset;
    private int m_CompressedSize;
    private byte[] m_CompressedAnim;
    private CoordinateSystem m_CoordinateSystem;
    private AvatarGender m_bodyTypeMask;

    public AssetAnimationParser(CoordinateSystem coordSys, AvatarGender bodyMask)
    {
      this.m_CoordinateSystem = coordSys;
      this.m_bodyTypeMask = bodyMask;
    }

    private static void InvertCoordinateSystem(
      InterleavedDataUnpacker<Pose, AssetAnimationParser.SkeletonPosePacker> unpacker)
    {
      AssetAnimationParser.SkeletonPosePacker[] unpackers = unpacker.Unpackers;
      int length = unpackers.Length;
      for (int index = 0; index < length; ++index)
      {
        unpackers[index].position.InvertCoordinateSystem();
        unpackers[index].rotation.InvertCoordinateSystem();
      }
    }

    public AvatarAnimation Parse(Stream stream)
    {
      BitStream bitStream = new BitStream(stream);
      bitStream.ReadInt(32);
      this.m_FramesPerSecond = bitStream.ReadFloat();
      bitStream.ReadInt(32);
      bitStream.ReadInt(32);
      bitStream.ReadInt(32);
      bitStream.ReadInt(32);
      this.m_SecondaryOffset = bitStream.ReadInt(32);
      this.m_TexturesOffset = bitStream.ReadInt(32);
      bitStream.ReadInt(32);
      this.m_CompressedSize = bitStream.ReadInt(32);
      this.m_CompressedAnim = new byte[this.m_CompressedSize];
      stream.Read(this.m_CompressedAnim, 0, this.m_CompressedSize);
      MemoryStream memoryStream = new MemoryStream(this.m_CompressedAnim);
      memoryStream.Seek(0L, SeekOrigin.Begin);
      InterleavedDataUnpacker<Pose, AssetAnimationParser.SkeletonPosePacker> unpacker1 = new InterleavedDataUnpacker<Pose, AssetAnimationParser.SkeletonPosePacker>(72);
      ByteStreamUnpacker<Pose[]> byteStreamUnpacker1 = new ByteStreamUnpacker<Pose[]>((Stream) memoryStream, (DataUnpackerGeneric<Pose[]>) unpacker1);
      byteStreamUnpacker1.UnpackHeader();
      if (this.m_CoordinateSystem == CoordinateSystem.LeftHanded)
        AssetAnimationParser.InvertCoordinateSystem(unpacker1);
      Pose[][] result1;
      byteStreamUnpacker1.UnpackData(out result1);
      memoryStream.Seek((long) this.m_SecondaryOffset, SeekOrigin.Begin);
      InterleavedDataUnpacker<Pose, AssetAnimationParser.SkeletonPosePacker> unpacker2 = new InterleavedDataUnpacker<Pose, AssetAnimationParser.SkeletonPosePacker>(72);
      ByteStreamUnpacker<Pose[]> byteStreamUnpacker2 = new ByteStreamUnpacker<Pose[]>((Stream) memoryStream, (DataUnpackerGeneric<Pose[]>) unpacker2);
      byteStreamUnpacker2.UnpackHeader();
      if (this.m_CoordinateSystem == CoordinateSystem.LeftHanded)
        AssetAnimationParser.InvertCoordinateSystem(unpacker2);
      Pose[][] result2;
      byteStreamUnpacker2.UnpackData(out result2);
      if (result2[0].Length == 0)
        result2 = (Pose[][]) null;
      memoryStream.Seek((long) this.m_TexturesOffset, SeekOrigin.Begin);
      AvatarExpression[] result3;
      new ByteStreamUnpacker<AvatarExpression>((Stream) memoryStream, (DataUnpackerGeneric<AvatarExpression>) new AssetAnimationParser.AvatarExpressionPacker()).Unpack(out result3);
      return new AvatarAnimation(result1, result2, result3, this.m_FramesPerSecond, this.m_bodyTypeMask);
    }

    private static void NormalizeKeyframesPolarity(Pose[][] keyframes)
    {
      int length1 = keyframes.Length;
      int length2 = keyframes[0].Length;
      for (int index1 = 0; index1 < length2; ++index1)
      {
        for (int index2 = 0; index2 < length1 - 1; ++index2)
        {
          Quaternion rotation1 = keyframes[index2][index1].rotation;
          Quaternion rotation2 = keyframes[index2 + 1][index1].rotation;
          if ((double) rotation1.X * (double) rotation2.X + (double) rotation1.Y * (double) rotation2.Y + (double) rotation1.Z * (double) rotation2.Z + (double) rotation1.W * (double) rotation2.W < 0.0)
          {
            rotation2.X = -rotation2.X;
            rotation2.Y = -rotation2.Y;
            rotation2.Z = -rotation2.Z;
            rotation2.W = -rotation2.W;
            keyframes[index2 + 1][index1].rotation = rotation2;
          }
        }
      }
    }

    public class SkeletonPosePacker : DataUnpackerGeneric<Pose>
    {
      public Vector3dDataUnpacker position = new Vector3dDataUnpacker();
      public QuaternionDataUnpacker rotation = new QuaternionDataUnpacker();
      public Vector3dDataUnpacker scale = new Vector3dDataUnpacker();

      public override int GetHeaderBitCount()
      {
        return this.position.GetHeaderBitCount() + this.rotation.GetHeaderBitCount() + this.scale.GetHeaderBitCount();
      }

      public override int GetPerDataBitCount()
      {
        return this.position.GetPerDataBitCount() + this.rotation.GetPerDataBitCount() + this.scale.GetPerDataBitCount();
      }

      public override void UnpackHeader(BitStream bitStream)
      {
        this.position.UnpackHeader(bitStream);
        this.rotation.UnpackHeader(bitStream);
        this.scale.UnpackHeader(bitStream);
      }

      public override void UnpackData(BitStream bitStream, out Pose pose)
      {
        this.position.UnpackData(bitStream, out pose.position);
        this.rotation.UnpackData(bitStream, out pose.rotation);
        this.scale.UnpackData(bitStream, out pose.scale);
      }
    }

    private class AvatarExpressionPacker : DataUnpackerGeneric<AvatarExpression>
    {
      private IntegerDataUnpacker MouthLayer = new IntegerDataUnpacker();
      private IntegerDataUnpacker LeftEyebrowLayer = new IntegerDataUnpacker();
      private IntegerDataUnpacker RightEyebrowLayer = new IntegerDataUnpacker();
      private IntegerDataUnpacker LeftEyeLayer = new IntegerDataUnpacker();
      private IntegerDataUnpacker RightEyeLayer = new IntegerDataUnpacker();

      public override void UnpackHeader(BitStream bitStream)
      {
        if (bitStream.ReadInt(32) != 5)
        {
          Logger.Log((Log) new DebugLog((object) this, Resources.InvalidAvatarExpressionFormatText));
          throw new AvatarException(Resources.InvalidAvatarExpressionFormatText);
        }
        this.RightEyeLayer.UnpackHeader(bitStream);
        this.LeftEyeLayer.UnpackHeader(bitStream);
        this.RightEyebrowLayer.UnpackHeader(bitStream);
        this.LeftEyebrowLayer.UnpackHeader(bitStream);
        this.MouthLayer.UnpackHeader(bitStream);
      }

      public override void UnpackData(BitStream bitStream, out AvatarExpression exp)
      {
        this.MouthLayer.UnpackData(bitStream, out exp.MouthLayer);
        if (exp.MouthLayer > 13)
          throw new AvatarException(Resources.InvalidAvatarMouthTextureIndex);
        this.LeftEyebrowLayer.UnpackData(bitStream, out exp.LeftEyebrowLayer);
        if (exp.LeftEyebrowLayer > 4)
          throw new AvatarException(Resources.InvalidAvatarEyebrowTextureIndex);
        this.RightEyebrowLayer.UnpackData(bitStream, out exp.RightEyebrowLayer);
        if (exp.RightEyebrowLayer > 4)
          throw new AvatarException(Resources.InvalidAvatarEyebrowTextureIndex);
        this.LeftEyeLayer.UnpackData(bitStream, out exp.LeftEyeLayer);
        if (exp.LeftEyeLayer > 13)
        {
          Logger.Log((Log) new DebugLog((object) this, string.Format("{0} error has occured in left eye layer.", (object) Resources.InvalidAvatarEyeTextureIndex)));
          exp.LeftEyeLayer = 12;
        }
        this.RightEyeLayer.UnpackData(bitStream, out exp.RightEyeLayer);
        if (exp.RightEyeLayer <= 13)
          return;
        Logger.Log((Log) new DebugLog((object) this, string.Format("{0} error has occured in right eye layer.", (object) Resources.InvalidAvatarEyeTextureIndex)));
        exp.RightEyeLayer = 12;
      }
    }
  }
}
