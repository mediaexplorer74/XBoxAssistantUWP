// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.DataUnpackerGeneric`1
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal abstract class DataUnpackerGeneric<Type>
  {
    public virtual int GetHeaderBitCount() => throw new NotImplementedException();

    public virtual int GetPerDataBitCount() => throw new NotImplementedException();

    public abstract void UnpackHeader(BitStream bitStream);

    public abstract void UnpackData(BitStream bitStream, out Type data);
  }
}
