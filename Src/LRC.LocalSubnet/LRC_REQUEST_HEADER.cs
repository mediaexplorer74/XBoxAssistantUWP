// *********************************************************
// Type: LRC.LocalSubnet.LRC_REQUEST_HEADER
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll

using System;


namespace LRC.LocalSubnet
{
  public class LRC_REQUEST_HEADER
  {
    public uint Signature;
    public uint MessageLength;
    public uint SequenceNumber;
    public uint ProtocolVersion;
    public uint ToClientId;
    public uint FromClientId;
    public uint MessageKind;
    public uint MessageType;

    public LRC_REQUEST_HEADER()
    {
    }

    public LRC_REQUEST_HEADER(LRC_REQUEST_HEADER copy)
    {
      this.Signature = copy != null ? copy.Signature : throw new ArgumentNullException(nameof (copy));
      this.MessageLength = copy.MessageLength;
      this.SequenceNumber = copy.SequenceNumber;
      this.ProtocolVersion = copy.ProtocolVersion;
      this.ToClientId = copy.ToClientId;
      this.FromClientId = copy.FromClientId;
      this.MessageKind = copy.MessageKind;
      this.MessageType = copy.MessageType;
    }
  }
}
