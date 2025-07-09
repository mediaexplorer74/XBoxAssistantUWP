// *********************************************************
// Type: Xbox.Live.Phone.Services.Leaderboards.LeaderboardsForTitleData
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Xbox.Live.Phone.Utils.Serialization;


namespace Xbox.Live.Phone.Services.Leaderboards
{
  public class LeaderboardsForTitleData
  {
    public LeaderboardsForTitleData()
    {
      this.Leaderboards = new XmlSerializableDictionary<uint, LeaderboardInfo>();
    }

    public uint TitleId { get; set; }

    public uint PrimaryLeaderboardId { get; set; }

    public XmlSerializableDictionary<uint, LeaderboardInfo> Leaderboards { get; set; }
  }
}
