// *********************************************************
// Type: Xbox.Live.Phone.Services.TitleHistory.TitleHistoryInfo
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll


namespace Xbox.Live.Phone.Services.TitleHistory
{
  public class TitleHistoryInfo
  {
    public uint TitleId { get; set; }

    public string Name { get; set; }

    public PlatformType PlatformType { get; set; }

    public TitleType TitleType { get; set; }
  }
}
