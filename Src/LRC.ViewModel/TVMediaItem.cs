// *********************************************************
// Type: LRC.ViewModel.TVMediaItem
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Service;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class TVMediaItem : MediaItem
  {
    private string parentId;
    private string episodeNumber;
    private ObservableCollection<MediaItem> seasons;

    public TVMediaItem()
    {
      this.MediaType = MediaType.tv_series;
      this.LifetimeInMinutes = 1440;
    }

    public TVMediaItem(string id)
      : this()
    {
      this.Id = id;
    }

    [DataMember]
    public string ParentId
    {
      get => this.parentId;
      set => this.SetPropertyValue<string>(ref this.parentId, value, nameof (ParentId));
    }

    [DataMember]
    public ObservableCollection<MediaItem> Seasons
    {
      get => this.seasons;
      set
      {
        this.SetPropertyValue<ObservableCollection<MediaItem>>(ref this.seasons, value, nameof (Seasons));
      }
    }

    public string EpisodeNumber
    {
      get => this.episodeNumber;
      set => this.SetPropertyValue<string>(ref this.episodeNumber, value, nameof (EpisodeNumber));
    }
  }
}
