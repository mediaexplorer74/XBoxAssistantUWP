// *********************************************************
// Type: LRC.ViewModel.MusicArtistViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  public class MusicArtistViewModel : ViewModelBase
  {
    private const string ZestRequestBase = "http://catalog.zune.net/v3.2/en-us/music/artist/";
    private const string PerfLoadBio = "MusicArtistViewModel:LoadBio";
    private const string PerfLoadPhotos = "MusicArtistViewModel:LoadPhotos";
    private const string PerfLoadRelated = "MusicArtistViewModel:LoadRelated";
    private const string PerfLoadAlbumsOfSameArtist = "MusicArtistViewModel:LoadAlbumsOfSameArtist";
    private ObservableCollection<string> photos;
    private ObservableCollection<MediaItem> relatedAlbums;
    private ObservableCollection<MediaItem> albumsBySameArtist;
    private ObservableCollection<string> extendedBio;
    private bool isBioAvailable;
    private bool isRelatedAvailable;
    private string artist;
    private string discographyNotFound;

    public MusicArtistViewModel()
    {
    }

    public MusicArtistViewModel(string id, string artist)
      : this()
    {
      this.ArtistId = id;
      this.Artist = artist;
      this.DiscographyNotFound = string.Empty;
      this.LifetimeInMinutes = 1440;
      this.BioUrl = ZuneServiceUtil.GetRequestUrl(MediaType.music_artist, this.ArtistId, InfoType.biography);
      this.RelatedUrl = ZuneServiceUtil.GetRequestUrl(MediaType.music_artist, this.ArtistId, InfoType.relatedAlbums);
      this.AlbumsOfSameArtistUrl = ZuneServiceUtil.GetRequestUrl(MediaType.music_artist, this.ArtistId, InfoType.albums);
      this.PhotoUrl = ZuneServiceUtil.GetRequestUrl(MediaType.music_artist, this.ArtistId, InfoType.images);
      this.BackgroundImageUrl = ZuneServiceUtil.GetRequestUrl(MediaType.music_artist, this.ArtistId, InfoType.backgroundImage);
      this.ImageUrl = ZuneServiceUtil.GetRequestUrl(MediaType.music_artist, this.ArtistId, InfoType.primaryImage);
    }

    public ObservableCollection<string> ExtendedBio
    {
      get => this.extendedBio;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.extendedBio, value, nameof (ExtendedBio));
      }
    }

    [XmlIgnore]
    [IgnoreDataMember]
    public bool IsBioAvailable
    {
      get => this.isBioAvailable;
      set => this.SetPropertyValue<bool>(ref this.isBioAvailable, value, nameof (IsBioAvailable));
    }

    public ObservableCollection<string> Photos
    {
      get
      {
        if (this.photos == null)
          this.photos = new ObservableCollection<string>();
        return this.photos;
      }
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.photos, value, nameof (Photos));
      }
    }

    public ObservableCollection<MediaItem> Related
    {
      get
      {
        if (this.relatedAlbums == null)
          this.relatedAlbums = new ObservableCollection<MediaItem>();
        return this.relatedAlbums;
      }
      set
      {
        this.SetPropertyValue<ObservableCollection<MediaItem>>(ref this.relatedAlbums, value, nameof (Related));
      }
    }

    [IgnoreDataMember]
    [XmlIgnore]
    public bool IsRelatedAvailable
    {
      get => this.isRelatedAvailable;
      set
      {
        this.SetPropertyValue<bool>(ref this.isRelatedAvailable, value, nameof (IsRelatedAvailable));
      }
    }

    [DataMember]
    public ObservableCollection<MediaItem> AlbumsBySameArtist
    {
      get => this.albumsBySameArtist;
      set
      {
        this.SetPropertyValue<ObservableCollection<MediaItem>>(ref this.albumsBySameArtist, value, nameof (AlbumsBySameArtist));
      }
    }

    [DataMember]
    public string Artist
    {
      get => this.artist;
      set => this.SetPropertyValue<string>(ref this.artist, value, nameof (Artist));
    }

    public string DiscographyNotFound
    {
      get => this.discographyNotFound;
      set
      {
        this.SetPropertyValue<string>(ref this.discographyNotFound, value, nameof (DiscographyNotFound));
      }
    }

    [DataMember]
    public string ArtistId { get; set; }

    public string BioUrl { get; set; }

    public string AlbumsOfSameArtistUrl { get; set; }

    public string PhotoUrl { get; set; }

    public string RelatedUrl { get; set; }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        this.LoadBio();
        this.LoadAlbumsOfSameArtist();
      });
    }

    private void LoadBio()
    {
      WebClient webClient = new WebClient();
      webClient.DownloadStringCompleted += (DownloadStringCompletedEventHandler) ((o, e) =>
      {
        if (e != null && e.Error == null)
        {
          string artistBio = ZuneVideoHelper.ParseArtistBio(e.Result);
          this.ExtendedBio = TextUtil.SplitString(2500, artistBio);
          this.IsBioAvailable = !string.IsNullOrWhiteSpace(artistBio);
        }
        else
          this.IsBioAvailable = false;
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          this.LoadRelated();
        });
      });
      webClient.DownloadStringAsync(new Uri(this.BioUrl));
    }

    private void LoadPhotos()
    {
      WebClient webClient = new WebClient();
      webClient.DownloadStringCompleted += (DownloadStringCompletedEventHandler) ((o, e) =>
      {
        if (e == null || e.Error != null)
          return;
        this.Photos = ZuneVideoHelper.GetAlbumPhotos(e.Result);
      });
      webClient.DownloadStringAsync(new Uri(this.PhotoUrl));
    }

    private void LoadRelated()
    {
      WebClient webClient = new WebClient();
      webClient.DownloadStringCompleted += (DownloadStringCompletedEventHandler) ((o, e) =>
      {
        if (e != null && e.Error == null)
        {
          this.Related = ZuneVideoHelper.GetAlbumRelated(e.Result);
          this.IsRelatedAvailable = this.Related != null && this.Related.Count > 0;
        }
        else
          this.IsRelatedAvailable = false;
      });
      webClient.DownloadStringAsync(new Uri(this.RelatedUrl));
    }

    private void LoadAlbumsOfSameArtist()
    {
      WebClient webClient = new WebClient();
      this.IsBusy = true;
      this.CurrentState = 3;
      webClient.DownloadStringCompleted += (DownloadStringCompletedEventHandler) ((o, e) =>
      {
        this.IsBusy = false;
        if (e != null && e.Error == null)
        {
          this.LastRefreshTime = DateTime.UtcNow;
          this.AlbumsBySameArtist = ZuneVideoHelper.GetAlbumsOfSameArtist(e.Result);
          if (this.AlbumsBySameArtist != null && this.AlbumsBySameArtist.Count > 0)
          {
            this.DiscographyNotFound = string.Empty;
            this.CurrentState = 0;
          }
          else
          {
            this.DiscographyNotFound = Resource.Discography_NotFound;
            this.CurrentState = 2;
          }
        }
        else
        {
          this.DiscographyNotFound = Resource.Discography_NotFound;
          this.CurrentState = 1;
        }
      });
      webClient.DownloadStringAsync(new Uri(this.AlbumsOfSameArtistUrl));
    }
  }
}
