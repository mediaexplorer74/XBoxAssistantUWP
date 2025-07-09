// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AvatarGender
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  [Flags]
  public enum AvatarGender
  {
    Unknown = 0,
    Male = 1,
    Female = 2,
    Both = Female | Male, // 0x00000003
  }
}
