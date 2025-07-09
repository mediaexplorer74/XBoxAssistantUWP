// *********************************************************
// Type: Xbox.Live.Phone.Services.Leaderboards.LeaderboardInfo
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Collections.ObjectModel;


namespace Xbox.Live.Phone.Services.Leaderboards
{
  public class LeaderboardInfo
  {
    public LeaderboardInfo()
    {
      this.LeaderboardEntries = new ObservableCollection<LeaderboardEntry>();
      this.GamerEntryIndex = -1;
    }

    public uint LeaderboardId { get; set; }

    public uint TitleId { get; set; }

    public string Name { get; set; }

    public string RatingHeader { get; set; }

    public ObservableCollection<LeaderboardEntry> LeaderboardEntries { get; set; }

    public int GamerEntryIndex { get; set; }
  }
}
