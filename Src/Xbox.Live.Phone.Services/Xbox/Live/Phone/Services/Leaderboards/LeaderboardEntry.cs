// *********************************************************
// Type: Xbox.Live.Phone.Services.Leaderboards.LeaderboardEntry
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll


namespace Xbox.Live.Phone.Services.Leaderboards
{
  public class LeaderboardEntry
  {
    public uint Rank { get; set; }

    public string Gamertag { get; set; }

    public ulong XboxUserId { get; set; }

    public string Rating { get; set; }
  }
}
