// *********************************************************
// Type: LRC.ViewModel.MediaItem
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Service;
using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [XmlInclude(typeof (MovieMediaItem))]
  [KnownType(typeof (MovieMediaItem))]
  [KnownType(typeof (MusicTrackItem))]
  [XmlInclude(typeof (TVMediaItem))]
  [KnownType(typeof (TVMediaItem))]
  [KnownType(typeof (TVEpisodeItem))]
  [XmlInclude(typeof (AlbumItem))]
  [KnownType(typeof (MusicVideoItem))]
  [XmlInclude(typeof (TVEpisodeItem))]
  [XmlInclude(typeof (MusicVideoItem))]
  [XmlInclude(typeof (MusicTrackItem))]
  [DataContract(Namespace = "")]
  [KnownType(typeof (AlbumItem))]
  [XmlRoot(Namespace = "")]
  public class MediaItem : ViewModelBase
  {
    private string detailTitle;
    private string id;
    private string rating;
    private DateTime releaseDate;
    private TimeSpan duration;
    private string releaseDetails;
    private string genre;
    private string studio;
    private string detailsImageUrl;
    private bool showProviders = true;
    private ObservableCollection<ProviderViewModel> providers;
    private RelatedItemsViewModel relatedItemsViewModel;

    [DataMember]
    public MediaType MediaType { get; set; }

    [DataMember]
    public string DetailTitle
    {
      get => this.detailTitle;
      set => this.SetPropertyValue<string>(ref this.detailTitle, value, nameof (DetailTitle));
    }

    [DataMember]
    public bool ShowProviders
    {
      get => this.showProviders;
      set => this.SetPropertyValue<bool>(ref this.showProviders, value, nameof (ShowProviders));
    }

    [DataMember]
    public ObservableCollection<ProviderViewModel> Providers
    {
      get => this.providers;
      set
      {
        this.SetPropertyValue<ObservableCollection<ProviderViewModel>>(ref this.providers, value, nameof (Providers));
        this.ShowProviders = this.Providers != null && this.Providers.Count > 0 && !this.IsNowPlaying;
      }
    }

    public bool IsNowPlaying => NowPlayingItemViewModel.IsNowPlaying(this);

    [DataMember]
    public string Id
    {
      get => this.id;
      set => this.SetPropertyValue<string>(ref this.id, value, nameof (Id));
    }

    [DataMember]
    public string Rating
    {
      get => this.rating;
      set => this.SetPropertyValue<string>(ref this.rating, value, nameof (Rating));
    }

    [DataMember]
    public DateTime ReleaseDate
    {
      get => this.releaseDate;
      set => this.SetPropertyValue<DateTime>(ref this.releaseDate, value, nameof (ReleaseDate));
    }

    [DataMember]
    public TimeSpan Duration
    {
      get => this.duration;
      set => this.SetPropertyValue<TimeSpan>(ref this.duration, value, nameof (Duration));
    }

    [DataMember]
    public string ReleaseDetails
    {
      get => this.releaseDetails;
      set => this.SetPropertyValue<string>(ref this.releaseDetails, value, nameof (ReleaseDetails));
    }

    [DataMember]
    public string Genre
    {
      get => this.genre;
      set => this.SetPropertyValue<string>(ref this.genre, value, nameof (Genre));
    }

    [DataMember]
    public string Studio
    {
      get => this.studio;
      set => this.SetPropertyValue<string>(ref this.studio, value, nameof (Studio));
    }

    [DataMember]
    public string DetailsImageUrl
    {
      get => this.detailsImageUrl;
      set
      {
        this.SetPropertyValue<string>(ref this.detailsImageUrl, value, nameof (DetailsImageUrl));
      }
    }

    public RelatedItemsViewModel RelatedItemsViewModel
    {
      get => this.relatedItemsViewModel;
      set
      {
        this.SetPropertyValue<RelatedItemsViewModel>(ref this.relatedItemsViewModel, value, nameof (RelatedItemsViewModel));
      }
    }

    [DataMember]
    public bool IsUpdateable { get; set; }

    public static MediaItem CreateMediaItem(MediaType mediaType, string id)
    {
      switch (mediaType)
      {
        case MediaType.movie:
          return (MediaItem) new MovieMediaItem(id);
        case MediaType.music_album:
          return (MediaItem) new AlbumItem(id);
        case MediaType.music_track:
          return (MediaItem) new MusicTrackItem(id);
        case MediaType.tv_episode:
          return (MediaItem) new TVEpisodeItem(id);
        case MediaType.tv_series:
          return (MediaItem) new TVMediaItem(id);
        default:
          return (MediaItem) null;
      }
    }
  }
}
