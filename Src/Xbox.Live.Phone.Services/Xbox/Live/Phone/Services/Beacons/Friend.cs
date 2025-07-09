// *********************************************************
// Type: Xbox.Live.Phone.Services.Beacons.Friend
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll


namespace Xbox.Live.Phone.Services.Beacons
{
  public class Friend
  {
    public string Gamertag { get; set; }

    public ulong UserId { get; set; }

    public bool IsOnline { get; set; }

    public uint? PresenceTitleId { get; set; }

    public SimpleDuration TimeSinceLastActivity { get; set; }

    public BeaconInfo Beacon { get; set; }
  }
}
