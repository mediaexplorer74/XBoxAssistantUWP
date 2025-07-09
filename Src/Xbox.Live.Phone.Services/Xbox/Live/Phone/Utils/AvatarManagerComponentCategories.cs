// *********************************************************
// Type: Xbox.Live.Phone.Utils.AvatarManagerComponentCategories
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System;


namespace Xbox.Live.Phone.Utils
{
  [Flags]
  public enum AvatarManagerComponentCategories
  {
    None = 0,
    Head = 1,
    Body = 2,
    Hair = 4,
    Shirt = 8,
    Trousers = 16, // 0x00000010
    Shoes = 32, // 0x00000020
    Hat = 64, // 0x00000040
    Gloves = 128, // 0x00000080
    Glasses = 256, // 0x00000100
    Wristwear = 512, // 0x00000200
    Earrings = 1024, // 0x00000400
    Ring = 2048, // 0x00000800
    Carryable = 4096, // 0x00001000
    Eyes = 8192, // 0x00002000
    Eyebrows = 16384, // 0x00004000
    Mouth = 32768, // 0x00008000
    FacialHair = 65536, // 0x00010000
    FacialOther = 131072, // 0x00020000
    EyeShadow = 262144, // 0x00040000
    Nose = 524288, // 0x00080000
    Chin = 1048576, // 0x00100000
    Ears = 2097152, // 0x00200000
    Shape = 16777216, // 0x01000000
    Animation = 4194304, // 0x00400000
    Costume = 8388608, // 0x00800000
    Models = Carryable | Ring | Earrings | Wristwear | Glasses | Gloves | Hat | Shoes | Trousers | Shirt | Hair | Body | Head, // 0x00001FFF
    Textures = EyeShadow | FacialOther | FacialHair | Mouth | Eyebrows | Eyes, // 0x0007E000
    BlendShapes = Shape | Ears | Chin | Nose, // 0x01380000
    Animations = Animation, // 0x00400000
    Costumes = Costume, // 0x00800000
    Clothing = Costumes | Carryable | Ring | Earrings | Wristwear | Glasses | Gloves | Hat | Shoes | Trousers | Shirt, // 0x00801FF8
    Valid = Clothing | Animations | BlendShapes | Textures | Hair | Body | Head, // 0x01FFFFFF
    Awardable = 33554432, // 0x02000000
  }
}
