// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AvatarComponentMasks
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal
{
  [Flags]
  public enum AvatarComponentMasks : short
  {
    None = 0,
    Head = 1,
    Body = 2,
    Hair = 4,
    Shirt = 8,
    Trousers = 16, // 0x0010
    Shoes = 32, // 0x0020
    Hat = 64, // 0x0040
    Gloves = 128, // 0x0080
    Glasses = 256, // 0x0100
    Wristwear = 512, // 0x0200
    Earrings = 1024, // 0x0400
    Ring = 2048, // 0x0800
    Carryable = 4096, // 0x1000
    All = Carryable | Ring | Earrings | Wristwear | Glasses | Gloves | Hat | Shoes | Trousers | Shirt | Hair | Body | Head, // 0x1FFF
  }
}
