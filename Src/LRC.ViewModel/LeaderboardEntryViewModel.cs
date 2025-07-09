// *********************************************************
// Type: LRC.ViewModel.LeaderboardEntryViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Services.Leaderboards;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class LeaderboardEntryViewModel : ViewModelBase
  {
    private string rank;
    private string gamertag;
    private string score;

    public LeaderboardEntryViewModel()
    {
    }

    public LeaderboardEntryViewModel(LeaderboardEntry leaderboardEntry)
    {
      if (leaderboardEntry == null)
        return;
      this.Gamertag = leaderboardEntry.Gamertag;
      this.ImageUrl = XboxLiveGamer.GetAvatarPicImageSource(this.Gamertag);
      this.Score = leaderboardEntry.Rating;
      this.Rank = leaderboardEntry.Rank.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    [DataMember]
    public string Rank
    {
      get => this.rank;
      set => this.SetPropertyValue<string>(ref this.rank, value, nameof (Rank));
    }

    [DataMember]
    public string Gamertag
    {
      get => this.gamertag;
      set => this.SetPropertyValue<string>(ref this.gamertag, value, nameof (Gamertag));
    }

    [DataMember]
    public string Score
    {
      get => this.score;
      set => this.SetPropertyValue<string>(ref this.score, value, nameof (Score));
    }
  }
}
