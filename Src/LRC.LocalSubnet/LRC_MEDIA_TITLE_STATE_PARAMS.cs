// *********************************************************
// Type: LRC.LocalSubnet.LRC_MEDIA_TITLE_STATE_PARAMS
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll


namespace LRC.LocalSubnet
{
  public struct LRC_MEDIA_TITLE_STATE_PARAMS
  {
    public uint TitleId;
    public ulong Duration;
    public ulong Position;
    public ulong MinSeek;
    public ulong MaxSeek;
    public float Rate;
    public uint TransportState;
    public uint TransportCapabilities;
    public uint MediaAssetIdLength;
    public string MediaAssetId;
  }
}
