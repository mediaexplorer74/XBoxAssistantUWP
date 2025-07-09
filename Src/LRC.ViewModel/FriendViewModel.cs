// *********************************************************
// Type: LRC.ViewModel.FriendViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Services.Beacons;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class FriendViewModel : ViewModelBase
  {
    private string gamerTag;
    private bool isPlayingNow;
    private bool isOnline;
    private SimpleDuration timeSinceLastActivity;
    private BeaconInfo beacon;

    public FriendViewModel()
    {
    }

    public FriendViewModel(Friend friendData, uint titleId)
    {
      this.GamerTag = friendData != null ? friendData.Gamertag : throw new ArgumentNullException(nameof (friendData));
      this.ImageUrl = XboxLiveGamer.GetAvatarPicImageSource(friendData.Gamertag);
      this.Beacon = friendData.Beacon;
      this.IsOnline = friendData.IsOnline;
      this.TimeSinceLastActivity = friendData.TimeSinceLastActivity;
      int num1;
      if (friendData.IsOnline && friendData.PresenceTitleId.HasValue)
      {
        uint? presenceTitleId = friendData.PresenceTitleId;
        uint num2 = titleId;
        num1 = (int) presenceTitleId.GetValueOrDefault() != (int) num2 ? 0 : (presenceTitleId.HasValue ? 1 : 0);
      }
      else
        num1 = 0;
      this.IsPlayingNow = num1 != 0;
    }

    [DataMember]
    public string GamerTag
    {
      get => this.gamerTag;
      set => this.SetPropertyValue<string>(ref this.gamerTag, value, nameof (GamerTag));
    }

    [DataMember]
    public bool IsPlayingNow
    {
      get => this.isPlayingNow;
      set => this.SetPropertyValue<bool>(ref this.isPlayingNow, value, nameof (IsPlayingNow));
    }

    [DataMember]
    public bool IsOnline
    {
      get => this.isOnline;
      set
      {
        this.SetPropertyValue<bool>(ref this.isOnline, value, nameof (IsOnline));
        this.NotifyPropertyChanged("IsOffline");
      }
    }

    [IgnoreDataMember]
    public bool IsOffline => !this.isOnline;

    [IgnoreDataMember]
    public bool IsBeaconSet => this.Beacon != null;

    [DataMember]
    public SimpleDuration TimeSinceLastActivity
    {
      get => this.timeSinceLastActivity;
      set
      {
        this.SetPropertyValue<SimpleDuration>(ref this.timeSinceLastActivity, value, nameof (TimeSinceLastActivity));
      }
    }

    [DataMember]
    public BeaconInfo Beacon
    {
      get => this.beacon;
      set
      {
        this.SetPropertyValue<BeaconInfo>(ref this.beacon, value, nameof (Beacon));
        this.NotifyPropertyChanged("IsBeaconSet");
      }
    }
  }
}
