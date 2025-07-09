// *********************************************************
// Type: LRC.LocalSubnet.LRC_SEND_INPUT_REQUEST
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll


namespace LRC.LocalSubnet
{
  public class LRC_SEND_INPUT_REQUEST : LRC_REQUEST
  {
    public LRC_SEND_INPUT_PARAMS Params;

    public LRC_SEND_INPUT_REQUEST()
    {
      this.Header.MessageType = 3U;
      this.Header.MessageKind = 1U;
    }
  }
}
