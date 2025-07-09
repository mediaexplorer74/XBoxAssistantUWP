// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Version1.RandomAvatar
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using Microsoft.XboxLive.Avatars.Internal.Assets;
using Microsoft.XboxLive.MathUtilities;
using System;
using System.Collections;


namespace Microsoft.XboxLive.Avatars.Internal.Version1
{
  internal class RandomAvatar
  {
    private static int XAVATARTOC_CATEGORY_COUNT = 25;
    private static int XAVATAR_BODY_COUNT = 2;
    private static int XAVATARTOC_ASSET_INDEX_INVALID = (int) ushort.MaxValue;
    private static float[] c_aBodyHeights = new float[5]
    {
      -1f,
      -0.5f,
      0.0f,
      0.5f,
      1f
    };
    private static float[] c_aBodyWeights = new float[5]
    {
      -1f,
      -0.5f,
      0.0f,
      0.5f,
      1f
    };
    private static Colorb[] c_aBodyColors = new Colorb[18]
    {
      new Colorb((byte) 110, (byte) 65, (byte) 36),
      new Colorb((byte) 215, (byte) 170, (byte) 113),
      new Colorb((byte) 252, (byte) 238, (byte) 221),
      new Colorb((byte) 100, (byte) 65, (byte) 27),
      new Colorb((byte) 190, (byte) 135, (byte) 73),
      new Colorb((byte) 248, (byte) 219, (byte) 185),
      new Colorb((byte) 64, (byte) 40, (byte) 14),
      new Colorb((byte) 152, (byte) 101, (byte) 47),
      new Colorb((byte) 230, (byte) 188, (byte) 142),
      new Colorb((byte) 219, (byte) 202, (byte) 183),
      new Colorb((byte) 227, (byte) 169, (byte) 118),
      new Colorb((byte) 215, (byte) 185, (byte) 113),
      new Colorb((byte) 240, (byte) 179, (byte) 147),
      new Colorb((byte) 222, (byte) 145, (byte) 77),
      new Colorb((byte) 199, (byte) 152, (byte) 76),
      new Colorb((byte) 227, (byte) 148, (byte) 107),
      new Colorb((byte) 211, (byte) 121, (byte) 61),
      new Colorb((byte) 168, (byte) 125, (byte) 63)
    };
    private static Colorb[] c_aBodyColorsRandom = new Colorb[12]
    {
      new Colorb((byte) 110, (byte) 65, (byte) 36),
      new Colorb((byte) 215, (byte) 170, (byte) 113),
      new Colorb((byte) 100, (byte) 65, (byte) 27),
      new Colorb((byte) 190, (byte) 135, (byte) 73),
      new Colorb((byte) 152, (byte) 101, (byte) 47),
      new Colorb((byte) 230, (byte) 188, (byte) 142),
      new Colorb((byte) 227, (byte) 169, (byte) 118),
      new Colorb((byte) 215, (byte) 185, (byte) 113),
      new Colorb((byte) 240, (byte) 179, (byte) 147),
      new Colorb((byte) 222, (byte) 145, (byte) 77),
      new Colorb((byte) 199, (byte) 152, (byte) 76),
      new Colorb((byte) 168, (byte) 125, (byte) 63)
    };
    private static Colorb[] c_aHairColors = new Colorb[27]
    {
      new Colorb(byte.MaxValue, (byte) 252, (byte) 181),
      new Colorb((byte) 228, (byte) 211, (byte) 129),
      new Colorb((byte) 222, (byte) 207, (byte) 186),
      new Colorb((byte) 254, (byte) 239, (byte) 124),
      new Colorb((byte) 220, (byte) 183, (byte) 79),
      new Colorb((byte) 191, (byte) 167, (byte) 131),
      new Colorb((byte) 247, (byte) 215, (byte) 72),
      new Colorb((byte) 189, (byte) 147, (byte) 75),
      new Colorb((byte) 152, (byte) 128, (byte) 84),
      new Colorb((byte) 144, (byte) 102, (byte) 54),
      new Colorb((byte) 133, (byte) 84, (byte) 26),
      new Colorb((byte) 110, (byte) 83, (byte) 38),
      new Colorb((byte) 116, (byte) 81, (byte) 49),
      new Colorb((byte) 85, (byte) 57, (byte) 33),
      new Colorb((byte) 55, (byte) 33, (byte) 22),
      new Colorb((byte) 73, (byte) 52, (byte) 33),
      new Colorb((byte) 31, (byte) 18, (byte) 11),
      new Colorb((byte) 13, (byte) 13, (byte) 13),
      new Colorb((byte) 227, (byte) 128, (byte) 47),
      new Colorb((byte) 146, (byte) 52, (byte) 14),
      new Colorb((byte) 226, (byte) 222, (byte) 221),
      new Colorb((byte) 184, (byte) 94, (byte) 34),
      new Colorb((byte) 104, (byte) 38, (byte) 24),
      new Colorb((byte) 142, (byte) 130, (byte) 116),
      new Colorb((byte) 117, (byte) 62, (byte) 31),
      new Colorb((byte) 82, (byte) 52, (byte) 54),
      new Colorb((byte) 79, (byte) 70, (byte) 61)
    };
    private static Colorb[] c_aHairColorsRandomForBody2_11_12 = new Colorb[22]
    {
      new Colorb((byte) 222, (byte) 207, (byte) 186),
      new Colorb((byte) 220, (byte) 183, (byte) 79),
      new Colorb((byte) 191, (byte) 167, (byte) 131),
      new Colorb((byte) 189, (byte) 147, (byte) 75),
      new Colorb((byte) 152, (byte) 128, (byte) 84),
      new Colorb((byte) 144, (byte) 102, (byte) 54),
      new Colorb((byte) 133, (byte) 84, (byte) 26),
      new Colorb((byte) 110, (byte) 83, (byte) 38),
      new Colorb((byte) 116, (byte) 81, (byte) 49),
      new Colorb((byte) 85, (byte) 57, (byte) 33),
      new Colorb((byte) 55, (byte) 33, (byte) 22),
      new Colorb((byte) 73, (byte) 52, (byte) 33),
      new Colorb((byte) 31, (byte) 18, (byte) 11),
      new Colorb((byte) 13, (byte) 13, (byte) 13),
      new Colorb((byte) 227, (byte) 128, (byte) 47),
      new Colorb((byte) 146, (byte) 52, (byte) 14),
      new Colorb((byte) 184, (byte) 94, (byte) 34),
      new Colorb((byte) 104, (byte) 38, (byte) 24),
      new Colorb((byte) 142, (byte) 130, (byte) 116),
      new Colorb((byte) 117, (byte) 62, (byte) 31),
      new Colorb((byte) 82, (byte) 52, (byte) 54),
      new Colorb((byte) 79, (byte) 70, (byte) 61)
    };
    private static Colorb[] c_aHairColorsRandomForBody5_14_15 = new Colorb[17]
    {
      new Colorb((byte) 220, (byte) 183, (byte) 79),
      new Colorb((byte) 189, (byte) 147, (byte) 75),
      new Colorb((byte) 144, (byte) 102, (byte) 54),
      new Colorb((byte) 133, (byte) 84, (byte) 26),
      new Colorb((byte) 110, (byte) 83, (byte) 38),
      new Colorb((byte) 116, (byte) 81, (byte) 49),
      new Colorb((byte) 85, (byte) 57, (byte) 33),
      new Colorb((byte) 55, (byte) 33, (byte) 22),
      new Colorb((byte) 73, (byte) 52, (byte) 33),
      new Colorb((byte) 31, (byte) 18, (byte) 11),
      new Colorb((byte) 13, (byte) 13, (byte) 13),
      new Colorb((byte) 146, (byte) 52, (byte) 14),
      new Colorb((byte) 184, (byte) 94, (byte) 34),
      new Colorb((byte) 104, (byte) 38, (byte) 24),
      new Colorb((byte) 117, (byte) 62, (byte) 31),
      new Colorb((byte) 82, (byte) 52, (byte) 54),
      new Colorb((byte) 79, (byte) 70, (byte) 61)
    };
    private static Colorb[] c_aHairColorsRandomForBody6_13 = new Colorb[24]
    {
      new Colorb((byte) 228, (byte) 211, (byte) 129),
      new Colorb((byte) 254, (byte) 239, (byte) 124),
      new Colorb((byte) 220, (byte) 183, (byte) 79),
      new Colorb((byte) 191, (byte) 167, (byte) 131),
      new Colorb((byte) 247, (byte) 215, (byte) 72),
      new Colorb((byte) 189, (byte) 147, (byte) 75),
      new Colorb((byte) 152, (byte) 128, (byte) 84),
      new Colorb((byte) 144, (byte) 102, (byte) 54),
      new Colorb((byte) 133, (byte) 84, (byte) 26),
      new Colorb((byte) 110, (byte) 83, (byte) 38),
      new Colorb((byte) 116, (byte) 81, (byte) 49),
      new Colorb((byte) 85, (byte) 57, (byte) 33),
      new Colorb((byte) 55, (byte) 33, (byte) 22),
      new Colorb((byte) 73, (byte) 52, (byte) 33),
      new Colorb((byte) 31, (byte) 18, (byte) 11),
      new Colorb((byte) 13, (byte) 13, (byte) 13),
      new Colorb((byte) 227, (byte) 128, (byte) 47),
      new Colorb((byte) 146, (byte) 52, (byte) 14),
      new Colorb((byte) 184, (byte) 94, (byte) 34),
      new Colorb((byte) 104, (byte) 38, (byte) 24),
      new Colorb((byte) 142, (byte) 130, (byte) 116),
      new Colorb((byte) 117, (byte) 62, (byte) 31),
      new Colorb((byte) 82, (byte) 52, (byte) 54),
      new Colorb((byte) 79, (byte) 70, (byte) 61)
    };
    private static Colorb[] c_aHairColorsRandomForBody1_8_17_18 = new Colorb[11]
    {
      new Colorb((byte) 110, (byte) 83, (byte) 38),
      new Colorb((byte) 116, (byte) 81, (byte) 49),
      new Colorb((byte) 85, (byte) 57, (byte) 33),
      new Colorb((byte) 55, (byte) 33, (byte) 22),
      new Colorb((byte) 73, (byte) 52, (byte) 33),
      new Colorb((byte) 31, (byte) 18, (byte) 11),
      new Colorb((byte) 13, (byte) 13, (byte) 13),
      new Colorb((byte) 146, (byte) 52, (byte) 14),
      new Colorb((byte) 104, (byte) 38, (byte) 24),
      new Colorb((byte) 82, (byte) 52, (byte) 54),
      new Colorb((byte) 79, (byte) 70, (byte) 61)
    };
    private static Colorb[] c_aHairColorsRandomForBody4_7 = new Colorb[9]
    {
      new Colorb((byte) 85, (byte) 57, (byte) 33),
      new Colorb((byte) 55, (byte) 33, (byte) 22),
      new Colorb((byte) 73, (byte) 52, (byte) 33),
      new Colorb((byte) 31, (byte) 18, (byte) 11),
      new Colorb((byte) 13, (byte) 13, (byte) 13),
      new Colorb((byte) 146, (byte) 52, (byte) 14),
      new Colorb((byte) 104, (byte) 38, (byte) 24),
      new Colorb((byte) 82, (byte) 52, (byte) 54),
      new Colorb((byte) 79, (byte) 70, (byte) 61)
    };
    private static Colorb[] c_aHairColorsRandomForBody9_16 = new Colorb[21]
    {
      new Colorb((byte) 228, (byte) 211, (byte) 129),
      new Colorb((byte) 220, (byte) 183, (byte) 79),
      new Colorb((byte) 189, (byte) 147, (byte) 75),
      new Colorb((byte) 152, (byte) 128, (byte) 84),
      new Colorb((byte) 144, (byte) 102, (byte) 54),
      new Colorb((byte) 133, (byte) 84, (byte) 26),
      new Colorb((byte) 110, (byte) 83, (byte) 38),
      new Colorb((byte) 116, (byte) 81, (byte) 49),
      new Colorb((byte) 85, (byte) 57, (byte) 33),
      new Colorb((byte) 55, (byte) 33, (byte) 22),
      new Colorb((byte) 73, (byte) 52, (byte) 33),
      new Colorb((byte) 31, (byte) 18, (byte) 11),
      new Colorb((byte) 13, (byte) 13, (byte) 13),
      new Colorb((byte) 227, (byte) 128, (byte) 47),
      new Colorb((byte) 146, (byte) 52, (byte) 14),
      new Colorb((byte) 184, (byte) 94, (byte) 34),
      new Colorb((byte) 104, (byte) 38, (byte) 24),
      new Colorb((byte) 142, (byte) 130, (byte) 116),
      new Colorb((byte) 117, (byte) 62, (byte) 31),
      new Colorb((byte) 82, (byte) 52, (byte) 54),
      new Colorb((byte) 79, (byte) 70, (byte) 61)
    };
    private static Colorb[] c_aFaceColors = new Colorb[27]
    {
      new Colorb((byte) 232, (byte) 200, (byte) 79),
      new Colorb((byte) 230, (byte) 115, (byte) 184),
      new Colorb((byte) 102, (byte) 188, (byte) 220),
      new Colorb((byte) 225, (byte) 132, (byte) 23),
      new Colorb((byte) 127, (byte) 57, (byte) 121),
      new Colorb((byte) 42, (byte) 176, (byte) 171),
      new Colorb((byte) 209, (byte) 57, (byte) 39),
      new Colorb((byte) 95, (byte) 66, (byte) 148),
      new Colorb((byte) 66, (byte) 101, (byte) 188),
      new Colorb((byte) 138, (byte) 191, (byte) 85),
      new Colorb((byte) 188, (byte) 174, (byte) 137),
      new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue),
      new Colorb((byte) 57, (byte) 146, (byte) 81),
      new Colorb((byte) 140, (byte) 96, (byte) 58),
      new Colorb((byte) 139, (byte) 139, (byte) 139),
      new Colorb((byte) 77, (byte) 85, (byte) 35),
      new Colorb((byte) 101, (byte) 68, (byte) 40),
      new Colorb((byte) 97, (byte) 97, (byte) 97),
      new Colorb((byte) 170, (byte) 124, (byte) 101),
      new Colorb((byte) 215, (byte) 95, (byte) 71),
      new Colorb((byte) 236, (byte) 182, (byte) 190),
      new Colorb((byte) 173, (byte) 95, (byte) 71),
      new Colorb((byte) 170, (byte) 29, (byte) 38),
      new Colorb((byte) 207, (byte) 89, (byte) 105),
      new Colorb((byte) 89, (byte) 51, (byte) 47),
      new Colorb((byte) 129, (byte) 39, (byte) 31),
      new Colorb((byte) 178, (byte) 79, (byte) 125)
    };
    private static Colorb[] c_aEyeColors = new Colorb[18]
    {
      new Colorb((byte) 146, (byte) 178, (byte) 202),
      new Colorb((byte) 136, (byte) 176, (byte) 73),
      new Colorb((byte) 174, (byte) 152, (byte) 67),
      new Colorb((byte) 99, (byte) 129, (byte) 167),
      new Colorb((byte) 114, (byte) 127, (byte) 53),
      new Colorb((byte) 130, (byte) 80, (byte) 29),
      new Colorb((byte) 42, (byte) 114, (byte) 164),
      new Colorb((byte) 55, (byte) 81, (byte) 42),
      new Colorb((byte) 91, (byte) 53, (byte) 30),
      new Colorb((byte) 145, (byte) 122, (byte) 81),
      new Colorb((byte) 206, (byte) 206, (byte) 204),
      new Colorb((byte) 123, (byte) 123, (byte) 149),
      new Colorb((byte) 49, (byte) 33, (byte) 20),
      new Colorb((byte) 89, (byte) 93, (byte) 92),
      new Colorb((byte) 143, (byte) 116, (byte) 151),
      new Colorb((byte) 33, (byte) 33, (byte) 33),
      new Colorb((byte) 36, (byte) 63, (byte) 83),
      new Colorb((byte) 171, (byte) 2, (byte) 7)
    };
    private static Colorb[] c_aEyeColorsRandom = new Colorb[15]
    {
      new Colorb((byte) 146, (byte) 178, (byte) 202),
      new Colorb((byte) 136, (byte) 176, (byte) 73),
      new Colorb((byte) 174, (byte) 152, (byte) 67),
      new Colorb((byte) 99, (byte) 129, (byte) 167),
      new Colorb((byte) 114, (byte) 127, (byte) 53),
      new Colorb((byte) 130, (byte) 80, (byte) 29),
      new Colorb((byte) 42, (byte) 114, (byte) 164),
      new Colorb((byte) 55, (byte) 81, (byte) 42),
      new Colorb((byte) 91, (byte) 53, (byte) 30),
      new Colorb((byte) 145, (byte) 122, (byte) 81),
      new Colorb((byte) 206, (byte) 206, (byte) 204),
      new Colorb((byte) 49, (byte) 33, (byte) 20),
      new Colorb((byte) 89, (byte) 93, (byte) 92),
      new Colorb((byte) 33, (byte) 33, (byte) 33),
      new Colorb((byte) 36, (byte) 63, (byte) 83)
    };
    private static Colorb[] c_aEyeShadowColors = new Colorb[26]
    {
      new Colorb((byte) 186, (byte) 114, (byte) 182),
      new Colorb((byte) 170, (byte) 166, (byte) 201),
      new Colorb((byte) 247, (byte) 90, (byte) 135),
      new Colorb((byte) 154, (byte) 47, (byte) 125),
      new Colorb((byte) 119, (byte) 109, (byte) 211),
      new Colorb((byte) 197, (byte) 73, (byte) 109),
      new Colorb((byte) 99, (byte) 26, (byte) 70),
      new Colorb((byte) 110, (byte) 80, (byte) 164),
      new Colorb((byte) 169, (byte) 206, (byte) 240),
      new Colorb((byte) 178, (byte) 176, (byte) 93),
      new Colorb(byte.MaxValue, (byte) 226, (byte) 163),
      new Colorb((byte) 83, (byte) 149, (byte) 202),
      new Colorb((byte) 193, (byte) 195, (byte) 53),
      new Colorb((byte) 254, (byte) 200, (byte) 84),
      new Colorb((byte) 25, (byte) 83, (byte) 129),
      new Colorb((byte) 97, (byte) 112, (byte) 31),
      new Colorb((byte) 224, (byte) 110, (byte) 50),
      new Colorb((byte) 243, (byte) 188, (byte) 123),
      new Colorb((byte) 193, (byte) 165, (byte) 144),
      new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue),
      new Colorb((byte) 192, (byte) 137, (byte) 80),
      new Colorb((byte) 179, (byte) 125, (byte) 89),
      new Colorb((byte) 162, (byte) 168, (byte) 155),
      new Colorb((byte) 144, (byte) 79, (byte) 51),
      new Colorb((byte) 106, (byte) 57, (byte) 25),
      new Colorb((byte) 0, (byte) 0, (byte) 0)
    };
    private static Colorb[] c_aLipColors = new Colorb[27]
    {
      new Colorb((byte) 253, (byte) 106, (byte) 104),
      new Colorb((byte) 229, (byte) 137, (byte) 149),
      new Colorb((byte) 213, (byte) 146, (byte) 130),
      new Colorb((byte) 208, (byte) 14, (byte) 14),
      new Colorb((byte) 198, (byte) 82, (byte) 101),
      new Colorb((byte) 181, (byte) 97, (byte) 87),
      new Colorb((byte) 173, (byte) 45, (byte) 45),
      new Colorb((byte) 159, (byte) 93, (byte) 105),
      new Colorb((byte) 131, (byte) 70, (byte) 63),
      new Colorb((byte) 207, (byte) 133, (byte) 98),
      new Colorb((byte) 235, (byte) 125, (byte) 128),
      new Colorb((byte) 239, (byte) 194, (byte) 139),
      new Colorb((byte) 135, (byte) 87, (byte) 65),
      new Colorb((byte) 245, (byte) 104, (byte) 107),
      new Colorb((byte) 236, (byte) 168, (byte) 83),
      new Colorb((byte) 92, (byte) 59, (byte) 44),
      new Colorb((byte) 203, (byte) 103, (byte) 88),
      new Colorb((byte) 188, (byte) 117, (byte) 53),
      new Colorb((byte) 212, (byte) 101, (byte) 82),
      new Colorb((byte) 218, (byte) 97, (byte) 212),
      new Colorb((byte) 120, (byte) 109, (byte) 213),
      new Colorb((byte) 177, (byte) 74, (byte) 55),
      new Colorb((byte) 185, (byte) 57, (byte) 150),
      new Colorb((byte) 110, (byte) 81, (byte) 165),
      new Colorb((byte) 138, (byte) 26, (byte) 14),
      new Colorb((byte) 143, (byte) 45, (byte) 94),
      new Colorb((byte) 0, (byte) 0, (byte) 0)
    };
    private static Colorb[] c_aLipColorsRandom = new Colorb[21]
    {
      new Colorb((byte) 253, (byte) 106, (byte) 104),
      new Colorb((byte) 229, (byte) 137, (byte) 149),
      new Colorb((byte) 213, (byte) 146, (byte) 130),
      new Colorb((byte) 208, (byte) 14, (byte) 14),
      new Colorb((byte) 198, (byte) 82, (byte) 101),
      new Colorb((byte) 181, (byte) 97, (byte) 87),
      new Colorb((byte) 173, (byte) 45, (byte) 45),
      new Colorb((byte) 159, (byte) 93, (byte) 105),
      new Colorb((byte) 131, (byte) 70, (byte) 63),
      new Colorb((byte) 207, (byte) 133, (byte) 98),
      new Colorb((byte) 235, (byte) 125, (byte) 128),
      new Colorb((byte) 239, (byte) 194, (byte) 139),
      new Colorb((byte) 135, (byte) 87, (byte) 65),
      new Colorb((byte) 245, (byte) 104, (byte) 107),
      new Colorb((byte) 236, (byte) 168, (byte) 83),
      new Colorb((byte) 92, (byte) 59, (byte) 44),
      new Colorb((byte) 203, (byte) 103, (byte) 88),
      new Colorb((byte) 188, (byte) 117, (byte) 53),
      new Colorb((byte) 212, (byte) 101, (byte) 82),
      new Colorb((byte) 177, (byte) 74, (byte) 55),
      new Colorb((byte) 138, (byte) 26, (byte) 14)
    };
    private static Colorb[] c_aNoColor = new Colorb[0];
    private static Colorb[][] c_aColorTable = new Colorb[9][]
    {
      RandomAvatar.c_aBodyColors,
      RandomAvatar.c_aHairColors,
      RandomAvatar.c_aLipColors,
      RandomAvatar.c_aEyeColors,
      RandomAvatar.c_aHairColors,
      RandomAvatar.c_aEyeShadowColors,
      RandomAvatar.c_aHairColors,
      RandomAvatar.c_aFaceColors,
      RandomAvatar.c_aFaceColors
    };
    private static Colorb[][] c_aRandomColorTable = new Colorb[9][]
    {
      RandomAvatar.c_aBodyColorsRandom,
      RandomAvatar.c_aNoColor,
      RandomAvatar.c_aLipColorsRandom,
      RandomAvatar.c_aEyeColorsRandom,
      RandomAvatar.c_aNoColor,
      RandomAvatar.c_aNoColor,
      RandomAvatar.c_aNoColor,
      RandomAvatar.c_aNoColor,
      RandomAvatar.c_aNoColor
    };
    private static readonly RandomAvatar.RandomCategory[] c_aRandomCategories = new RandomAvatar.RandomCategory[25]
    {
      new RandomAvatar.RandomCategory(ComponentCategories.Head, new byte[2]
      {
        (byte) 100,
        (byte) 100
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Body, new byte[2]
      {
        (byte) 100,
        (byte) 100
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Hair, new byte[2]
      {
        (byte) 100,
        (byte) 100
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Shirt, new byte[2]
      {
        (byte) 100,
        (byte) 100
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Trousers, new byte[2]
      {
        (byte) 100,
        (byte) 100
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Shoes, new byte[2]
      {
        (byte) 100,
        (byte) 100
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Hat, new byte[2]
      {
        (byte) 20,
        (byte) 5
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Gloves, new byte[2]
      {
        (byte) 5,
        (byte) 5
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Glasses, new byte[2]
      {
        (byte) 10,
        (byte) 10
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Wristwear, new byte[2]
      {
        (byte) 5,
        (byte) 20
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Earrings, new byte[2]
      {
        (byte) 1,
        (byte) 75
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Ring, new byte[2]
      {
        (byte) 5,
        (byte) 75
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Carryable, new byte[2]),
      new RandomAvatar.RandomCategory(ComponentCategories.Eyes, new byte[2]
      {
        (byte) 100,
        (byte) 100
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Eyebrows, new byte[2]
      {
        (byte) 100,
        (byte) 100
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Mouth, new byte[2]
      {
        (byte) 100,
        (byte) 100
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.FacialHair, new byte[2]
      {
        (byte) 10,
        (byte) 0
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.FacialOther, new byte[2]
      {
        (byte) 20,
        (byte) 40
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.EyeShadow, new byte[2]),
      new RandomAvatar.RandomCategory(ComponentCategories.Nose, new byte[2]
      {
        (byte) 100,
        (byte) 100
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Chin, new byte[2]
      {
        (byte) 100,
        (byte) 100
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Ears, new byte[2]
      {
        (byte) 100,
        (byte) 100
      }),
      new RandomAvatar.RandomCategory(ComponentCategories.Shape, new byte[2]),
      new RandomAvatar.RandomCategory(ComponentCategories.Animation, new byte[2]),
      new RandomAvatar.RandomCategory(ComponentCategories.Costume, new byte[2])
    };
    private static readonly ComponentColors[] s_ColorTable_00000008_0048_0001_C1C8_F109A19CB2E0 = new ComponentColors[9]
    {
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 50, (byte) 134, (byte) 218),
        new Colorb((byte) 50, (byte) 134, (byte) 218),
        new Colorb((byte) 50, (byte) 134, (byte) 218)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue),
        new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue),
        new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 128, (byte) 128, (byte) 128),
        new Colorb((byte) 128, (byte) 128, (byte) 128),
        new Colorb((byte) 128, (byte) 128, (byte) 128)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 35, (byte) 35, (byte) 35),
        new Colorb((byte) 35, (byte) 35, (byte) 35),
        new Colorb((byte) 35, (byte) 35, (byte) 35)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 128, (byte) 64, (byte) 0),
        new Colorb((byte) 128, (byte) 64, (byte) 0),
        new Colorb((byte) 128, (byte) 64, (byte) 0)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb(byte.MaxValue, byte.MaxValue, (byte) 0),
        new Colorb(byte.MaxValue, byte.MaxValue, (byte) 0),
        new Colorb(byte.MaxValue, byte.MaxValue, (byte) 0)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb(byte.MaxValue, (byte) 141, (byte) 28),
        new Colorb(byte.MaxValue, (byte) 141, (byte) 28),
        new Colorb(byte.MaxValue, (byte) 141, (byte) 28)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 215, (byte) 41, (byte) 9),
        new Colorb((byte) 215, (byte) 41, (byte) 9),
        new Colorb((byte) 215, (byte) 41, (byte) 9)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 91, (byte) 134, (byte) 13),
        new Colorb((byte) 91, (byte) 134, (byte) 13),
        new Colorb((byte) 91, (byte) 134, (byte) 13)
      })
    };
    private static readonly ComponentColors[] s_ColorTable_00000008_008E_0001_C1C8_F109A19CB2E0 = new ComponentColors[1]
    {
      new ComponentColors(new Colorb[3]
      {
        new Colorb(byte.MaxValue, (byte) 0, (byte) 0),
        new Colorb((byte) 0, byte.MaxValue, (byte) 0),
        new Colorb((byte) 0, (byte) 0, byte.MaxValue)
      })
    };
    private static readonly ComponentColors[] s_ColorTable_00000008_0113_0002_C1C8_F109A19CB2E0 = new ComponentColors[9]
    {
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 35, (byte) 35, (byte) 35),
        new Colorb((byte) 35, (byte) 35, (byte) 35),
        new Colorb((byte) 35, (byte) 35, (byte) 35)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 97, (byte) 231, (byte) 103),
        new Colorb((byte) 97, (byte) 231, (byte) 103),
        new Colorb((byte) 97, (byte) 231, (byte) 103)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue),
        new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue),
        new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb(byte.MaxValue, byte.MaxValue, (byte) 0),
        new Colorb(byte.MaxValue, byte.MaxValue, (byte) 0),
        new Colorb(byte.MaxValue, byte.MaxValue, (byte) 0)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb(byte.MaxValue, (byte) 141, (byte) 28),
        new Colorb(byte.MaxValue, (byte) 141, (byte) 28),
        new Colorb(byte.MaxValue, (byte) 141, (byte) 28)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 217, (byte) 0, (byte) 0),
        new Colorb((byte) 217, (byte) 0, (byte) 0),
        new Colorb((byte) 217, (byte) 0, (byte) 0)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb(byte.MaxValue, (byte) 183, (byte) 219),
        new Colorb(byte.MaxValue, (byte) 183, (byte) 219),
        new Colorb(byte.MaxValue, (byte) 183, (byte) 219)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 166, (byte) 15, (byte) 93),
        new Colorb((byte) 166, (byte) 15, (byte) 93),
        new Colorb((byte) 166, (byte) 15, (byte) 93)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 15, (byte) 93, (byte) 166),
        new Colorb((byte) 15, (byte) 93, (byte) 166),
        new Colorb((byte) 15, (byte) 93, (byte) 166)
      })
    };
    private static readonly ComponentColors[] s_ColorTable_00000008_012C_0002_C1C8_F109A19CB2E0 = new ComponentColors[9]
    {
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 50, (byte) 134, (byte) 218),
        new Colorb((byte) 50, (byte) 134, (byte) 218),
        new Colorb((byte) 50, (byte) 134, (byte) 218)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue),
        new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue),
        new Colorb(byte.MaxValue, byte.MaxValue, byte.MaxValue)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 128, (byte) 128, (byte) 128),
        new Colorb((byte) 128, (byte) 128, (byte) 128),
        new Colorb((byte) 128, (byte) 128, (byte) 128)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 35, (byte) 35, (byte) 35),
        new Colorb((byte) 35, (byte) 35, (byte) 35),
        new Colorb((byte) 35, (byte) 35, (byte) 35)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 128, (byte) 64, (byte) 0),
        new Colorb((byte) 128, (byte) 64, (byte) 0),
        new Colorb((byte) 128, (byte) 64, (byte) 0)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb(byte.MaxValue, byte.MaxValue, (byte) 0),
        new Colorb(byte.MaxValue, byte.MaxValue, (byte) 0),
        new Colorb(byte.MaxValue, byte.MaxValue, (byte) 0)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb(byte.MaxValue, (byte) 141, (byte) 28),
        new Colorb(byte.MaxValue, (byte) 141, (byte) 28),
        new Colorb(byte.MaxValue, (byte) 141, (byte) 28)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 215, (byte) 41, (byte) 9),
        new Colorb((byte) 215, (byte) 41, (byte) 9),
        new Colorb((byte) 215, (byte) 41, (byte) 9)
      }),
      new ComponentColors(new Colorb[3]
      {
        new Colorb((byte) 91, (byte) 134, (byte) 13),
        new Colorb((byte) 91, (byte) 134, (byte) 13),
        new Colorb((byte) 91, (byte) 134, (byte) 13)
      })
    };
    private static readonly RandomAvatar.RandomAssetInfo[] s_RandomAssets = new RandomAvatar.RandomAssetInfo[470]
    {
      new RandomAvatar.RandomAssetInfo((byte) 1, new Guid("{00000002-0000-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 1, new Guid("{00000002-0001-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 0, new Guid("{00000001-0002-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-002c-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-002d-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-002e-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-002f-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0030-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0031-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0032-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0034-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0035-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0036-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0037-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0038-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0039-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-003a-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-003b-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-003c-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-03d8-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-003e-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0044-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0048-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, RandomAvatar.s_ColorTable_00000008_0048_0001_C1C8_F109A19CB2E0),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-004b-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-004f-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0050-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0052-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0054-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0056-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0058-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0060-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0061-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0062-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0063-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0064-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0065-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0067-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-006a-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-006e-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-006f-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-008e-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, RandomAvatar.s_ColorTable_00000008_008E_0001_C1C8_F109A19CB2E0),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-008f-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0090-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0091-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0092-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0094-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0096-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0098-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0099-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-009a-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-009c-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-009d-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-009e-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-009f-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-00a1-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-00a2-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-00a3-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-00a4-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-00a7-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-00a8-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-00a9-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-00b5-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-00b6-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-00b7-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-00b8-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-00b9-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-00ba-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-00bb-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-00bc-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04f2-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04f3-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04f4-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04f5-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04f6-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04f7-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04f8-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00bd-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00be-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00bf-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00c1-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00c2-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00c5-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00c6-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00c9-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00ca-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00cb-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00cc-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-03c2-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00cd-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00ce-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00cf-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00d0-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00d1-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00d5-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00d8-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00d9-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00da-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-00db-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-03c3-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 8, new Guid("{00000100-03c4-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-00dc-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-00dd-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-00de-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-00df-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-00e0-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-00e1-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-00e2-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-00e3-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-00e4-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-00e5-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-00e6-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-00e7-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-00e8-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-00e9-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-00ea-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-00eb-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-00ed-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-00ee-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-00ef-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-00f0-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-00f3-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-03c5-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-00f4-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-00f5-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-00f6-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-00f7-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-00f8-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-00f9-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-00fa-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-00fb-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-00fc-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-00fd-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-00fe-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-00ff-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0100-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0101-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0102-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0103-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0104-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0105-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0107-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0108-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-0109-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-010a-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-010b-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-010c-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 5, new Guid("{00000020-03c6-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-010f-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0111-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0113-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, RandomAvatar.s_ColorTable_00000008_0113_0002_C1C8_F109A19CB2E0),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0114-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0115-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0117-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-011b-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-011c-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-011e-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-011f-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0120-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0122-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0123-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0129-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-012a-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-012c-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, RandomAvatar.s_ColorTable_00000008_012C_0002_C1C8_F109A19CB2E0),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0130-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0131-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0132-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0134-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0136-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0138-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-0139-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-03cd-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-03d2-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 3, new Guid("{00000008-03d9-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0158-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0159-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-015a-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-015b-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-015c-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-015e-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-015f-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0160-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0161-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0162-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0163-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0164-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0165-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0166-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0167-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0168-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0169-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-016a-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-016b-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-016c-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-016d-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-016e-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-016f-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-0170-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-04ee-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 4, new Guid("{00000010-03d3-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-018a-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-018b-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-018c-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-018d-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04f9-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04fa-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04fb-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04fc-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04fd-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04fe-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04ff-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-0500-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-0501-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-03c7-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-03d6-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04ec-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04ed-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04f0-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 10, new Guid("{00000400-04f1-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-018e-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-018f-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-0190-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-0191-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-0192-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-0193-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-0194-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 9, new Guid("{00000200-0195-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-0196-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-0197-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-0198-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-0199-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-019a-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-019b-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-019c-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-019d-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-019f-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-01a0-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-01a1-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-01a2-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-01a3-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-01a4-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 6, new Guid("{00000040-03d1-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-01a6-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-01a7-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-01a8-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-01a9-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-01aa-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-01ab-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-01ac-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 11, new Guid("{00000800-01ad-0002-c1c8-f109a19cb2e0}"), AvatarGender.Female, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01ae-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01b0-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01b2-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01b6-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01bd-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01bf-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01c3-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01c4-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01c6-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01c8-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01ca-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01cc-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01ce-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01d0-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01d1-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01d6-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01d7-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01da-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01dc-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01de-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01e2-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01e6-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01e8-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01ea-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01f0-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01f2-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01f4-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01f6-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01f8-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01fa-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-01fe-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0200-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0201-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0204-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0206-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-020a-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-020e-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0210-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0214-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0216-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0217-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-021b-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-021d-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-021f-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0221-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0223-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0225-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0227-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0229-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-022b-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-022d-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-022f-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0231-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0233-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0235-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0237-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0239-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-023b-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-023d-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-023f-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0241-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0245-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0247-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0249-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-024b-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-024f-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-0253-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-025b-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 2, new Guid("{00000004-025d-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-0260-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-0261-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-0262-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-0263-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-0264-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-0266-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-0267-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-0269-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-026a-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-026b-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-026c-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-026d-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-026e-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-026f-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 14, new Guid("{00004000-0271-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-027a-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-027b-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-027d-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-027e-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-027f-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-0280-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-0281-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-0282-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-0283-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-0284-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-0285-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-0286-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-0287-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-0289-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-028a-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-028b-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-028c-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-028d-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-028e-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-028f-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-0290-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-0291-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-0292-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-0293-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-0295-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-0297-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-0298-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-0299-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-029a-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-029b-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-029c-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-029d-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-029e-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-029f-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02a0-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02a1-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02a2-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02a3-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02a4-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02a5-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02a6-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02a7-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02a9-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02ab-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02ac-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02ad-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02ae-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02af-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02b1-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02b3-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02b4-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02b5-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02b7-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02b9-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02ba-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02bb-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02bc-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02bd-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02be-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02bf-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02c0-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02c1-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02c2-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02c3-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02c5-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02c6-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02c7-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02c8-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02c9-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02ca-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02cb-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02cc-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02cd-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02ce-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02cf-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02d0-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02d1-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 13, new Guid("{00002000-02d2-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 18, new Guid("{00040000-02d3-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02d4-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02d5-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02d6-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02d7-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02d8-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02d9-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02da-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02db-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02dd-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02de-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02df-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02e0-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02e3-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 16, new Guid("{00010000-02e4-0001-c1c8-f109a19cb2e0}"), AvatarGender.Male, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02e6-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02e7-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02e9-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02ea-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02eb-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02ec-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02ee-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02ef-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02f0-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02f6-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02f9-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02fa-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02fc-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02fd-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 15, new Guid("{00008000-02ff-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 20, new Guid("{00100000-031a-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 20, new Guid("{00100000-031b-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 20, new Guid("{00100000-031c-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 20, new Guid("{00100000-031d-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 20, new Guid("{00100000-031e-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 20, new Guid("{00100000-031f-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 20, new Guid("{00100000-0320-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 20, new Guid("{00100000-0321-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-0323-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-0324-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-0325-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-0326-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-0327-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Female, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-0328-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-032a-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-032b-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-032d-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-032e-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-032f-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-0330-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-0331-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-0333-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 19, new Guid("{00080000-0334-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Male, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 21, new Guid("{00200000-0336-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 21, new Guid("{00200000-0337-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 21, new Guid("{00200000-0339-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 21, new Guid("{00200000-033a-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 21, new Guid("{00200000-033b-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null),
      new RandomAvatar.RandomAssetInfo((byte) 21, new Guid("{00200000-033c-0003-c1c8-f109a19cb2e0}"), AvatarGender.Both, AvatarGender.Both, (ComponentColors[]) null)
    };
    private static Random s_random = new Random(DateTime.Now.Second);
    private RandomAvatar.DynamicData m_data;

    internal RandomAvatar() => this.m_data = new RandomAvatar.DynamicData();

    internal AvatarManifest[] CreateAvatars(AvatarGender bodyMask, int avatarsCount)
    {
      AvatarManifest[] avatars = new AvatarManifest[avatarsCount];
      this.m_data.Initialize(avatarsCount);
      this.m_data.m_BodyMask = bodyMask;
      if (!this.BuildBodyTable())
        return (AvatarManifest[]) null;
      if (!this.BuildCategoryTable())
        return (AvatarManifest[]) null;
      if (!this.BuildAssetTable())
        return (AvatarManifest[]) null;
      for (int avatarIndex = 0; avatarIndex < this.m_data.m_cAvatars; ++avatarIndex)
      {
        int index = (int) this.m_data.m_aManifestOrder[avatarIndex];
        avatars[index] = this.CreateManifest(avatarIndex);
        if (avatars[index] == (AvatarManifest) null)
          return (AvatarManifest[]) null;
      }
      return avatars;
    }

    private bool BuildBodyTable()
    {
      int cAvatars = this.m_data.m_cAvatars;
      int num1 = this.m_data.m_BodyMask > AvatarGender.Female ? 2 : 1;
      if (this.m_data.m_cAvatars / 2 / num1 > 0)
      {
        for (int index = 0; index < RandomAvatar.XAVATAR_BODY_COUNT; ++index)
        {
          if ((this.m_data.m_BodyMask & (AvatarGender) (1 << index)) != AvatarGender.Unknown)
          {
            this.m_data.m_aAvatarsPerBodyType[index] = this.m_data.m_cAvatars / 2 / num1;
            cAvatars -= this.m_data.m_aAvatarsPerBodyType[index];
          }
        }
      }
      for (int index = 0; index < RandomAvatar.XAVATAR_BODY_COUNT; ++index)
      {
        if ((this.m_data.m_BodyMask & (AvatarGender) (1 << index)) != AvatarGender.Unknown)
        {
          if (--num1 > 0)
          {
            int num2 = RandomAvatar.s_random.Next(cAvatars + 1);
            this.m_data.m_aAvatarsPerBodyType[index] += num2;
            cAvatars -= num2;
          }
          else
          {
            this.m_data.m_aAvatarsPerBodyType[index] += cAvatars;
            break;
          }
        }
      }
      int num3 = 0;
      for (byte index1 = 0; (int) index1 < RandomAvatar.XAVATAR_BODY_COUNT; ++index1)
      {
        for (int index2 = 0; index2 < this.m_data.m_aAvatarsPerBodyType[(int) index1]; ++index2)
          this.m_data.m_aBodyTypes[num3++] = index1;
      }
      return true;
    }

    private bool BuildCategoryTable()
    {
      for (int index1 = 0; index1 < RandomAvatar.XAVATAR_BODY_COUNT; ++index1)
      {
        for (int index2 = 0; index2 < RandomAvatar.c_aRandomCategories.Length; ++index2)
        {
          if ((byte) 100 == RandomAvatar.c_aRandomCategories[index2].m_aChances[index1])
            this.m_data.m_aRequiredCategoryMasks[index1] |= RandomAvatar.c_aRandomCategories[index2].m_CategoryMask;
        }
      }
      for (int index3 = 0; index3 < this.m_data.m_cAvatars; ++index3)
      {
        int aBodyType = (int) this.m_data.m_aBodyTypes[index3];
        for (int index4 = 0; index4 < RandomAvatar.c_aRandomCategories.Length; ++index4)
        {
          double num = (double) RandomAvatar.c_aRandomCategories[index4].m_aChances[aBodyType] / 100.0;
          if (RandomAvatar.s_random.NextDouble() < num)
            this.m_data.m_aCategoryMasks[index3] |= RandomAvatar.c_aRandomCategories[index4].m_CategoryMask;
        }
      }
      return true;
    }

    private bool BuildAssetTable()
    {
      for (int iCategory = 0; iCategory < RandomAvatar.c_aRandomCategories.Length; ++iCategory)
      {
        for (int avatarIndex = 0; avatarIndex < this.m_data.m_cAvatars; ++avatarIndex)
        {
          if (!this.BuildAssetTable(iCategory, avatarIndex))
            return false;
        }
      }
      return true;
    }

    private bool BuildAssetTable(int iCategory, int avatarIndex)
    {
      int num1 = 1 << (int) this.m_data.m_aBodyTypes[avatarIndex];
      int maxValue = 0;
      this.m_data.m_aAssets[iCategory][avatarIndex] = RandomAvatar.XAVATARTOC_ASSET_INDEX_INVALID;
      for (int index = 0; index < RandomAvatar.s_RandomAssets.Length; ++index)
      {
        if ((int) RandomAvatar.s_RandomAssets[index].m_iCategory == iCategory && ((int) RandomAvatar.s_RandomAssets[index].m_RandomBodyMask & num1) != 0 && !this.m_data.m_AssetUsage.Get(index))
          ++maxValue;
      }
      if (maxValue == 0)
      {
        for (int index = 0; index < RandomAvatar.s_RandomAssets.Length; ++index)
        {
          if ((int) RandomAvatar.s_RandomAssets[index].m_iCategory == iCategory && ((int) RandomAvatar.s_RandomAssets[index].m_RandomBodyMask & num1) != 0)
          {
            this.m_data.m_AssetUsage.Set(index, false);
            ++maxValue;
          }
        }
      }
      if (maxValue == 0)
        return true;
      int num2 = RandomAvatar.s_random.Next(maxValue);
      for (int index = 0; index < RandomAvatar.s_RandomAssets.Length; ++index)
      {
        if ((int) RandomAvatar.s_RandomAssets[index].m_iCategory == iCategory && ((int) RandomAvatar.s_RandomAssets[index].m_RandomBodyMask & num1) != 0 && !this.m_data.m_AssetUsage.Get(index))
        {
          if (num2 == 0)
          {
            this.m_data.m_AssetUsage.Set(index, true);
            this.m_data.m_aAssets[iCategory][avatarIndex] = (int) (short) index;
            break;
          }
          --num2;
        }
      }
      return true;
    }

    private AvatarManifest CreateManifest(int avatarIndex)
    {
      int aBodyType = (int) this.m_data.m_aBodyTypes[avatarIndex];
      AvatarManifestV1 manifest = new AvatarManifestV1();
      manifest.m_Dirty = true;
      if (!this.SetBodySize(ref manifest, avatarIndex))
        return (AvatarManifest) null;
      if (!this.SetBlendShapes(ref manifest, avatarIndex))
        return (AvatarManifest) null;
      if (!this.SetTextures(ref manifest, avatarIndex))
        return (AvatarManifest) null;
      if (!this.SetDynamicColors(ref manifest, avatarIndex))
        return (AvatarManifest) null;
      if (!this.SetModels(ref manifest, avatarIndex))
        return (AvatarManifest) null;
      if ((this.m_data.m_aFoundCategoryMasks[avatarIndex] & this.m_data.m_aRequiredCategoryMasks[aBodyType]) != this.m_data.m_aRequiredCategoryMasks[aBodyType])
        throw new AvatarException("Unable to load a required asset");
      return (AvatarManifest) manifest;
    }

    private bool SetBodySize(ref AvatarManifestV1 manifest, int avatarIndex)
    {
      float cABodyHeight = RandomAvatar.c_aBodyHeights[(int) this.m_data.m_aHeights[avatarIndex]];
      float cABodyWeight = RandomAvatar.c_aBodyWeights[(int) this.m_data.m_aWeights[avatarIndex]];
      manifest.WidthFactor = cABodyWeight;
      manifest.HeightFactor = cABodyHeight;
      return true;
    }

    private uint lsb(uint v, int bitsize)
    {
      for (int index = 0; index < bitsize; ++index)
      {
        if (((long) v & (long) (1 << index)) > 0L)
          return (uint) index;
      }
      return (uint) -1;
    }

    private bool SetBlendShape(
      ref AvatarManifestV1 manifest,
      int avatarIndex,
      BlendShapeType eShape,
      ComponentCategories categoryMask)
    {
      if ((this.m_data.m_aCategoryMasks[avatarIndex] & categoryMask) == ComponentCategories.None)
        return true;
      Guid assetId = this.GetAssetId(this.m_data.m_aAssets[(IntPtr) this.lsb((uint) categoryMask, 32)][avatarIndex]);
      if (assetId == Guid.Empty)
        return true;
      manifest.SetBlendShape(eShape, assetId);
      this.m_data.m_aFoundCategoryMasks[avatarIndex] |= categoryMask;
      return true;
    }

    private bool SetBlendShapes(ref AvatarManifestV1 manifest, int avatarIndex)
    {
      return this.SetBlendShape(ref manifest, avatarIndex, BlendShapeType.Chin, ComponentCategories.Chin) && this.SetBlendShape(ref manifest, avatarIndex, BlendShapeType.Nose, ComponentCategories.Nose) && this.SetBlendShape(ref manifest, avatarIndex, BlendShapeType.Ear, ComponentCategories.Ears);
    }

    private bool SetTexture(
      ref AvatarManifestV1 manifest,
      int avatarIndex,
      DynamicTextureType eTexture,
      ComponentCategories categoryMask)
    {
      if ((this.m_data.m_aCategoryMasks[avatarIndex] & categoryMask) == ComponentCategories.None)
        return true;
      int iAsset = this.m_data.m_aAssets[(IntPtr) this.lsb((uint) categoryMask, 32)][avatarIndex];
      if (iAsset == -1)
        return true;
      Guid assetId = this.GetAssetId(iAsset);
      if (assetId == Guid.Empty)
        return true;
      manifest.SetReplacementTexture(eTexture, new AvatarManifestV1.ReplacementTexture()
      {
        m_LinkedAssetId = Guid.Empty,
        m_Placement = {
          m_Scale = 1f,
          m_Rotation = 0.0f,
          m_TranslationU = 0.0f,
          m_TranslationV = 0.0f
        },
        m_TextureAssetId = assetId
      });
      this.m_data.m_aFoundCategoryMasks[avatarIndex] |= categoryMask;
      return true;
    }

    private bool SetTextures(ref AvatarManifestV1 manifest, int avatarIndex)
    {
      return this.SetTexture(ref manifest, avatarIndex, DynamicTextureType.Eye, ComponentCategories.Eyes) && this.SetTexture(ref manifest, avatarIndex, DynamicTextureType.Eyebrow, ComponentCategories.Eyebrows) && this.SetTexture(ref manifest, avatarIndex, DynamicTextureType.Mouth, ComponentCategories.Mouth) && this.SetTexture(ref manifest, avatarIndex, DynamicTextureType.FacialHair, ComponentCategories.FacialHair) && this.SetTexture(ref manifest, avatarIndex, DynamicTextureType.SkinFeatures, ComponentCategories.FacialOther) && this.SetTexture(ref manifest, avatarIndex, DynamicTextureType.EyeShadow, ComponentCategories.EyeShadow);
    }

    private bool SetHairDynamicColors(ref AvatarManifestV1 manifest, int avatarIndex)
    {
      Colorb linkedBodyHairColor = this.XAvatarGetLinkedBodyHairColor((int) this.m_data.m_aDynamicColors[0][avatarIndex]);
      manifest.SetDynamicColor(DynamicColorType.Hair, linkedBodyHairColor);
      return true;
    }

    private Colorb XAvatarGetLinkedBodyHairColor(int ulSkinColorIndex)
    {
      Colorb colorb = new Colorb((byte) 0, (byte) 0, (byte) 0);
      Random random = new Random();
      int num = 0;
      for (int index = 0; index < RandomAvatar.c_aBodyColors.Length; ++index)
      {
        if ((int) RandomAvatar.c_aBodyColorsRandom[ulSkinColorIndex].red == (int) RandomAvatar.c_aBodyColors[index].red && (int) RandomAvatar.c_aBodyColorsRandom[ulSkinColorIndex].green == (int) RandomAvatar.c_aBodyColors[index].green && (int) RandomAvatar.c_aBodyColorsRandom[ulSkinColorIndex].blue == (int) RandomAvatar.c_aBodyColors[index].blue)
        {
          num = index + 1;
          break;
        }
      }
      Colorb linkedBodyHairColor;
      switch (num)
      {
        case 1:
        case 8:
        case 17:
        case 18:
          linkedBodyHairColor = RandomAvatar.c_aHairColorsRandomForBody1_8_17_18[random.Next(RandomAvatar.c_aHairColorsRandomForBody1_8_17_18.Length - 1)];
          break;
        case 2:
        case 3:
        case 10:
        case 11:
        case 12:
          linkedBodyHairColor = RandomAvatar.c_aHairColorsRandomForBody2_11_12[random.Next(RandomAvatar.c_aHairColorsRandomForBody2_11_12.Length - 1)];
          break;
        case 4:
        case 7:
          linkedBodyHairColor = RandomAvatar.c_aHairColorsRandomForBody4_7[random.Next(RandomAvatar.c_aHairColorsRandomForBody4_7.Length - 1)];
          break;
        case 5:
        case 14:
        case 15:
          linkedBodyHairColor = RandomAvatar.c_aHairColorsRandomForBody5_14_15[random.Next(RandomAvatar.c_aHairColorsRandomForBody5_14_15.Length - 1)];
          break;
        case 6:
        case 13:
          linkedBodyHairColor = RandomAvatar.c_aHairColorsRandomForBody6_13[random.Next(RandomAvatar.c_aHairColorsRandomForBody6_13.Length - 1)];
          break;
        case 9:
        case 16:
          linkedBodyHairColor = RandomAvatar.c_aHairColorsRandomForBody9_16[random.Next(RandomAvatar.c_aHairColorsRandomForBody9_16.Length - 1)];
          break;
        default:
          linkedBodyHairColor = RandomAvatar.c_aHairColorsRandomForBody2_11_12[random.Next(RandomAvatar.c_aHairColorsRandomForBody2_11_12.Length - 1)];
          break;
      }
      return linkedBodyHairColor;
    }

    private bool SetDynamicColor(
      ref AvatarManifestV1 manifest,
      int avatarIndex,
      DynamicColorType eColor)
    {
      Colorb color = RandomAvatar.c_aRandomColorTable[(int) eColor].Length <= 0 ? RandomAvatar.c_aColorTable[(int) eColor][(int) this.m_data.m_aDynamicColors[(int) eColor][avatarIndex]] : RandomAvatar.c_aRandomColorTable[(int) eColor][(int) this.m_data.m_aDynamicColors[(int) eColor][avatarIndex]];
      manifest.SetDynamicColor(eColor, color);
      return true;
    }

    private bool SetDynamicColors(ref AvatarManifestV1 manifest, int avatarIndex)
    {
      if (!this.SetDynamicColor(ref manifest, avatarIndex, DynamicColorType.Skin) || !this.SetHairDynamicColors(ref manifest, avatarIndex) || !this.SetDynamicColor(ref manifest, avatarIndex, DynamicColorType.Iris) || !this.SetDynamicColor(ref manifest, avatarIndex, DynamicColorType.EyeShadow) || !this.SetDynamicColor(ref manifest, avatarIndex, DynamicColorType.Mouth) || !this.SetDynamicColor(ref manifest, avatarIndex, DynamicColorType.SkinFeatures1))
        return false;
      manifest.SetDynamicColor(DynamicColorType.SkinFeatures2, Utilities.ColorbFromVector4(manifest.GetDynamicColor(DynamicColorType.SkinFeatures1)));
      manifest.SetDynamicColor(DynamicColorType.Eyebrow, Utilities.ColorbFromVector4(manifest.GetDynamicColor(DynamicColorType.Hair)));
      manifest.SetDynamicColor(DynamicColorType.FacialHair, Utilities.ColorbFromVector4(manifest.GetDynamicColor(DynamicColorType.Hair)));
      return true;
    }

    private bool SetModel(ref AvatarManifestV1 manifest, int avatarIndex, int iCategory)
    {
      ComponentCategories componentCategories = (ComponentCategories) (1 << iCategory);
      if ((this.m_data.m_aCategoryMasks[avatarIndex] & componentCategories) == ComponentCategories.None)
        return true;
      int iAsset = this.m_data.m_aAssets[iCategory][avatarIndex];
      if (iAsset == -1)
        return true;
      Guid assetId = this.GetAssetId(iAsset);
      if (assetId == Guid.Empty)
        return true;
      ComponentInfo info = new ComponentInfo();
      info.m_ComponentMask = (AvatarComponentMasks) componentCategories;
      info.m_AssetId = assetId;
      if (RandomAvatar.s_RandomAssets[iAsset].m_customColors != null)
      {
        ComponentColors[] customColors = RandomAvatar.s_RandomAssets[iAsset].m_customColors;
        int index = RandomAvatar.s_random.Next(customColors.Length);
        info.m_CustomColors0 = Utilities.ColorbFromVector4(customColors[index].CustomColor0);
        info.m_CustomColors1 = Utilities.ColorbFromVector4(customColors[index].CustomColor1);
        info.m_CustomColors2 = Utilities.ColorbFromVector4(customColors[index].CustomColor2);
      }
      if (ComponentCategories.Body == componentCategories)
        manifest.m_BodyComponentInfo = new ComponentDescription(info, Guid.Empty);
      else if (ComponentCategories.Head == componentCategories)
        manifest.m_HeadComponentInfo = new ComponentDescription(info, Guid.Empty);
      else
        manifest.SetComponentInfo(info);
      this.m_data.m_aFoundCategoryMasks[avatarIndex] |= componentCategories;
      return true;
    }

    private bool SetModels(ref AvatarManifestV1 manifest, int avatarIndex)
    {
      ComponentCategories componentCategories = this.m_data.m_aCategoryMasks[avatarIndex] & ComponentCategories.Models;
      for (int iCategory = 0; iCategory < 32; ++iCategory)
      {
        if ((componentCategories & (ComponentCategories) (1 << iCategory)) != ComponentCategories.None && !this.SetModel(ref manifest, avatarIndex, iCategory))
          return false;
      }
      return true;
    }

    private Guid GetAssetId(int iAsset)
    {
      return RandomAvatar.XAVATARTOC_ASSET_INDEX_INVALID == iAsset ? Guid.Empty : RandomAvatar.s_RandomAssets[iAsset].m_Asset;
    }

    internal struct RandomCategory
    {
      internal ComponentCategories m_CategoryMask;
      internal byte[] m_aChances;

      internal RandomCategory(ComponentCategories category, byte[] aChanges)
      {
        this.m_CategoryMask = category;
        this.m_aChances = aChanges;
      }
    }

    internal struct RandomAssetInfo
    {
      internal byte m_iCategory;
      internal Guid m_Asset;
      internal byte m_BodyMask;
      internal byte m_RandomBodyMask;
      internal ComponentColors[] m_customColors;

      internal RandomAssetInfo(
        byte category,
        Guid asset,
        AvatarGender bodyMask,
        AvatarGender randomBodyMask,
        ComponentColors[] colorSets)
      {
        this.m_iCategory = category;
        this.m_Asset = asset;
        this.m_BodyMask = (byte) bodyMask;
        this.m_RandomBodyMask = (byte) randomBodyMask;
        this.m_customColors = colorSets;
      }
    }

    internal struct DynamicData
    {
      internal int m_cAvatars;
      internal AvatarGender m_BodyMask;
      internal int[] m_aAvatarsPerBodyType;
      internal byte[] m_aBodyTypes;
      internal ComponentCategories[] m_aRequiredCategoryMasks;
      internal ComponentCategories[] m_aCategoryMasks;
      internal ComponentCategories[] m_aFoundCategoryMasks;
      internal byte[] m_aHeights;
      internal byte[] m_aWeights;
      internal int[][] m_aAssets;
      internal byte[][] m_aDynamicColors;
      internal short[] m_aManifestOrder;
      internal BitArray m_HeightUsage;
      internal BitArray m_WeightUsage;
      internal BitArray m_AssetUsage;
      internal BitArray[] m_aColorUsage;

      internal void Initialize(int count)
      {
        int length1 = 3;
        this.m_cAvatars = count;
        this.m_aAvatarsPerBodyType = new int[length1];
        this.m_aBodyTypes = new byte[count];
        this.m_aRequiredCategoryMasks = new ComponentCategories[length1];
        this.m_aCategoryMasks = new ComponentCategories[count];
        this.m_aFoundCategoryMasks = new ComponentCategories[count];
        this.m_aHeights = new byte[count];
        this.m_aWeights = new byte[count];
        this.m_aDynamicColors = new byte[9][];
        this.m_aAssets = new int[RandomAvatar.XAVATARTOC_CATEGORY_COUNT][];
        for (int index = 0; index < RandomAvatar.XAVATARTOC_CATEGORY_COUNT; ++index)
          this.m_aAssets[index] = new int[count];
        this.m_HeightUsage = new BitArray(RandomAvatar.c_aBodyHeights.Length);
        this.m_WeightUsage = new BitArray(RandomAvatar.c_aBodyWeights.Length);
        this.m_AssetUsage = new BitArray(RandomAvatar.s_RandomAssets.Length);
        this.m_aColorUsage = new BitArray[9];
        this.m_HeightUsage.SetAll(false);
        this.m_WeightUsage.SetAll(false);
        this.m_AssetUsage.SetAll(false);
        for (int index = 0; index < this.m_aColorUsage.Length; ++index)
        {
          int length2 = RandomAvatar.c_aRandomColorTable[index].Length;
          if (length2 == 0)
            length2 = RandomAvatar.c_aColorTable[index].Length;
          this.m_aColorUsage[index] = new BitArray(length2);
          this.m_aColorUsage[index].SetAll(false);
          this.m_aDynamicColors[index] = new byte[count];
        }
        this.GetValueSet(RandomAvatar.s_random, this.m_HeightUsage, this.m_aHeights, count);
        this.GetValueSet(RandomAvatar.s_random, this.m_WeightUsage, this.m_aWeights, count);
        for (int index = 0; index < this.m_aColorUsage.Length; ++index)
          this.GetValueSet(RandomAvatar.s_random, this.m_aColorUsage[index], this.m_aDynamicColors[index], count);
        this.m_aManifestOrder = new short[count];
        for (int index = 0; index < count; ++index)
          this.m_aManifestOrder[index] = (short) index;
        for (int index1 = 0; index1 < count; ++index1)
        {
          int index2 = RandomAvatar.s_random.Next(count - 1);
          short num = this.m_aManifestOrder[index2];
          this.m_aManifestOrder[index2] = this.m_aManifestOrder[index1];
          this.m_aManifestOrder[index1] = num;
        }
      }

      internal void GetValueSet(Random random, BitArray domain, byte[] result, int length)
      {
        int num1 = 0;
        for (int index = 0; index < domain.Count; ++index)
        {
          if (!domain.Get(index))
            ++num1;
        }
        int num2 = 0;
        while (num2 < length)
        {
          if (num1 == 0)
          {
            domain.SetAll(false);
            num1 = domain.Count;
          }
          int num3 = random.Next(num1--);
          int index;
          for (index = 0; index < domain.Count; ++index)
          {
            if (!domain.Get(index))
            {
              if (num3 != 0)
                --num3;
              else
                break;
            }
          }
          result[num2++] = (byte) index;
          domain.Set(index, true);
        }
      }
    }
  }
}
