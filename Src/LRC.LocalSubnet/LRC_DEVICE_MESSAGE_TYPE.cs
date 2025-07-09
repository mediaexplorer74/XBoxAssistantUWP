// *********************************************************
// Type: LRC.LocalSubnet.LRC_DEVICE_MESSAGE_TYPE
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll

using System.Diagnostics.CodeAnalysis;


namespace LRC.LocalSubnet
{
  [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32", Justification = "Parity with console")]
  [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Following TMF spec")]
  public enum LRC_DEVICE_MESSAGE_TYPE : uint
  {
    Connected = 1,
    Disconnected = 2,
  }
}
