// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.AssetSkeletonParser
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.MathUtilities;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class AssetSkeletonParser
  {
    private CoordinateSystem m_CoordSys;

    public AssetSkeletonParser(CoordinateSystem coordSys) => this.m_CoordSys = coordSys;

    private static void RecalculateChildren(Joint[] joints)
    {
      int length = joints.Length;
      for (int index = 0; index < length; ++index)
      {
        joints[index].Child = -1;
        joints[index].Sibling = -1;
      }
      int index1 = length;
      while (--index1 >= 0)
      {
        int parent = joints[index1].Parent;
        if (parent != (int) byte.MaxValue)
        {
          if (joints[parent].Child == -1)
          {
            joints[parent].Child = index1;
          }
          else
          {
            joints[index1].Sibling = joints[parent].Child;
            joints[parent].Child = index1;
          }
        }
      }
    }

    private static void RecalculateLocalTransforms(Joint[] joints)
    {
      int length = joints.Length;
      while (--length >= 1)
      {
        Matrix fromQuaternion = Matrix.CreateFromQuaternion(joints[length].BindRotation) with
        {
          Translation = joints[length].BindPosition
        };
        int parent = joints[length].Parent;
        Matrix matrixB = MatrixMath.InvertEuclidean(Matrix.CreateFromQuaternion(joints[parent].BindRotation) with
        {
          Translation = joints[parent].BindPosition
        });
        Matrix matrix = Matrix.Multiply(fromQuaternion, matrixB);
        joints[length].Local.position = matrix.Translation;
        joints[length].Local.rotation = Quaternion.CreateFromRotationMatrix(matrix);
        joints[length].Local.scale = new Vector3(1f, 1f, 1f);
      }
      joints[0].Local.position = joints[0].BindPosition;
      joints[0].Local.rotation = joints[0].BindRotation;
      joints[0].Local.scale = new Vector3(1f, 1f, 1f);
    }

    public Skeleton Parse(Stream stream)
    {
      Skeleton skeleton = new Skeleton();
      new ByteStreamUnpacker<Joint>(stream, (DataUnpackerGeneric<Joint>) new AssetSkeletonParser.SkeletonDataParser()).Unpack(out skeleton.Joints);
      if (skeleton.Joints.Length > 72)
      {
        Logger.Log((Log) new DebugLog((object) this, Resources.SkeletonParserError1));
        throw new AvatarException(Resources.SkeletonParserError1);
      }
      if (this.m_CoordSys == CoordinateSystem.LeftHanded)
      {
        int length = skeleton.Joints.Length;
        for (int index = 0; index < length; ++index)
        {
          skeleton.Joints[index].BindPosition.Z = -skeleton.Joints[index].BindPosition.Z;
          skeleton.Joints[index].BindRotation.X = -skeleton.Joints[index].BindRotation.X;
          skeleton.Joints[index].BindRotation.Y = -skeleton.Joints[index].BindRotation.Y;
        }
      }
      AssetSkeletonParser.RecalculateChildren(skeleton.Joints);
      AssetSkeletonParser.RecalculateLocalTransforms(skeleton.Joints);
      return skeleton;
    }

    private class SkeletonDataParser : DataUnpackerGeneric<Joint>
    {
      private SmallDataUnpacker<byte> m_Parent = new SmallDataUnpacker<byte>(1);
      private Vector3dDataUnpacker m_BindPosePosition = new Vector3dDataUnpacker();
      private QuaternionDataUnpacker m_BindPoseRotation = new QuaternionDataUnpacker();

      public override void UnpackHeader(BitStream bitStream)
      {
        this.m_Parent.UnpackHeader(bitStream);
        this.m_BindPosePosition.UnpackHeader(bitStream);
        this.m_BindPoseRotation.UnpackHeader(bitStream);
      }

      public override void UnpackData(BitStream bitStream, out Joint data)
      {
        data = new Joint();
        byte data1;
        this.m_Parent.UnpackData(bitStream, out data1);
        this.m_BindPosePosition.UnpackData(bitStream, out data.BindPosition);
        this.m_BindPoseRotation.UnpackData(bitStream, out data.BindRotation);
        data.Parent = (int) data1;
      }
    }
  }
}
