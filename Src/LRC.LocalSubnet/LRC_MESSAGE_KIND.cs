// *********************************************************
// Type: LRC.LocalSubnet.LRC_MESSAGE_KIND
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll

using System.Diagnostics.CodeAnalysis;


namespace LRC.LocalSubnet
{
  [SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", Justification = "Value can't be combined")]
  public enum LRC_MESSAGE_KIND
  {
    None = 0,
    LRC_MESSAGE_KIND_REQUEST = 1,
    LRC_MESSAGE_KIND_RESPONSE = 2,
    LRC_MESSAGE_KIND_NOTIFICATION = 3,
    LRC_MESSAGE_KIND_PRESENCE = 2048, // 0x00000800
  }
}
