// *********************************************************
// Type: LRC.LocalSubnet.ReceivedLocalSubnetXmediaMessage
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll

using Microsoft.Xmedia.Client.WindowsPhone;


namespace LRC.LocalSubnet
{
  public class ReceivedLocalSubnetXmediaMessage : ReceivedXmediaMessage
  {
    public ReceivedLocalSubnetXmediaMessage(byte[] rawMessageBuffer)
      : base(0U, 0U, XmediaMessageType.ApplicationDefined, XmediaMessageKind.Response, (byte[]) null)
    {
      this.RawMessageBuffer = rawMessageBuffer;
    }
  }
}
