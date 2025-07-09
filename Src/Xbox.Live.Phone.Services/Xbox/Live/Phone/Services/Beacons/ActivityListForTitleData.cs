// *********************************************************
// Type: Xbox.Live.Phone.Services.Beacons.ActivityListForTitleData
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Collections.ObjectModel;


namespace Xbox.Live.Phone.Services.Beacons
{
  public class ActivityListForTitleData
  {
    public ActivityListForTitleData()
    {
      this.FriendActivityList = new ObservableCollection<Friend>();
    }

    public ObservableCollection<Friend> FriendActivityList { get; set; }
  }
}
