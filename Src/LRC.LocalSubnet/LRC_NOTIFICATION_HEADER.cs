// *********************************************************
// Type: LRC.LocalSubnet.LRC_NOTIFICATION_HEADER
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll


namespace LRC.LocalSubnet
{
  public class LRC_NOTIFICATION_HEADER
  {
    public uint Signature;
    public uint MessageLength;
    public uint SequenceNumber;
    public uint ProtocolVersion;
    public uint ToClientId;
    public uint FromClientId;
    public uint MessageKind;
    public uint MessageType;
  }
}
