// *********************************************************
// Type: LRC.ViewModel.TVEpisodeItem
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Service;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class TVEpisodeItem : MediaItem
  {
    private string seriesTitle;
    private string episodeNumber;
    private string seasonNumber;

    public TVEpisodeItem()
    {
      this.MediaType = MediaType.tv_episode;
      this.IsUpdateable = false;
    }

    public TVEpisodeItem(string id)
      : this()
    {
      this.Id = id;
    }

    [DataMember]
    public string SeriesId { get; set; }

    [DataMember]
    public string SeriesTitle
    {
      get => this.seriesTitle;
      set => this.SetPropertyValue<string>(ref this.seriesTitle, value, nameof (SeriesTitle));
    }

    public string SeasonNumber
    {
      get => this.seasonNumber;
      set => this.SetPropertyValue<string>(ref this.seasonNumber, value, nameof (SeasonNumber));
    }

    public string EpisodeNumber
    {
      get => this.episodeNumber;
      set => this.SetPropertyValue<string>(ref this.episodeNumber, value, nameof (EpisodeNumber));
    }
  }
}
