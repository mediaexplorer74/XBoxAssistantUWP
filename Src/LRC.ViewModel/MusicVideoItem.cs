// *********************************************************
// Type: LRC.ViewModel.MusicVideoItem
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
  public class MusicVideoItem : MediaItem
  {
    private string artist;
    private string album;
    private string lablelOwner;
    private bool isExplicity;

    public MusicVideoItem()
    {
      this.MediaType = MediaType.music_musicvideo;
      this.IsUpdateable = true;
    }

    [DataMember]
    public string Artist
    {
      get => this.artist;
      set => this.SetPropertyValue<string>(ref this.artist, value, nameof (Artist));
    }

    [DataMember]
    public string Album
    {
      get => this.album;
      set => this.SetPropertyValue<string>(ref this.album, value, nameof (Album));
    }

    [DataMember]
    public string LabelOwner
    {
      get => this.lablelOwner;
      set => this.SetPropertyValue<string>(ref this.lablelOwner, value, nameof (LabelOwner));
    }

    [DataMember]
    public bool IsExplicit
    {
      get => this.isExplicity;
      set => this.SetPropertyValue<bool>(ref this.isExplicity, value, nameof (IsExplicit));
    }

    public string ArtistId { get; set; }

    public string AlbumId { get; set; }

    public string TrackId { get; set; }
  }
}
