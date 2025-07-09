// *********************************************************
// Type: LRC.LocalSubnet.LRC_LAUNCH_TITLE_REQUEST
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll


namespace LRC.LocalSubnet
{
  public class LRC_LAUNCH_TITLE_REQUEST : LRC_REQUEST
  {
    public LRC_LAUNCH_TITLE_PARAMS Params;

    public LRC_LAUNCH_TITLE_REQUEST()
    {
      this.Header.MessageType = 2U;
      this.Header.MessageKind = 1U;
    }
  }
}
