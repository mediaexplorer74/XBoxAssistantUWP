// *********************************************************
// Type: LRC.ViewModel.AlbumItem
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
using System.Windows;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  public class AlbumItem : MediaItem
  {
    private const string PerfGetMediaDetail = "AlbumItemViewModel:GetMediaDetail";
    private const string PerfLoadReview = "AlbumItemViewModel:LoadReview";
    private const string PerfLoadBio = "AlbumItemViewModel:LoadBio";
    private const string PerfLoadPhotos = "AlbumItemViewModel:LoadPhotos";
    private const string PerfLoadRelated = "AlbumItemViewModel:LoadRelated";
    private const string PerfLoadAlbumsOfSameArtist = "AlbumItemViewModel:LoadAlbumsOfSameArtist";
    private ObservableCollection<ViewModelBase> items;
    private ObservableCollection<string> photos;
    private ObservableCollection<MediaItem> relatedAlbums;
    private bool isPhotoAvailable;
    private bool isRelatedAvailable;
    private bool isReviewAvailable;
    private bool isBioAvailable;
    private bool isLoadingAlbums;
    private bool isLoadingBio;
    private ObservableCollection<MediaItem> albumsbySameArtist;
    private ObservableCollection<string> extendedBio;
    private ObservableCollection<string> extendedReview;
    private string discographyNotFound;
    private ObservableCollection<ViewModelBase> displayedItems;
    private string artist;
    private string label;
    private bool isExplicit;
    private int bioState;
    private int albumState;
    private VerticalAlignment busyIndicatorVerticalAlignment;

    public AlbumItem()
    {
      this.LifetimeInMinutes = 1440;
      this.MediaType = MediaType.music_album;
      this.IsUpdateable = true;
      this.DiscographyNotFound = string.Empty;
    }

    public AlbumItem(string id)
      : this()
    {
      this.Id = id;
    }

    public bool IsActionable { get; set; }

    public string Label
    {
      get => this.label;
      set => this.SetPropertyValue<string>(ref this.label, value, nameof (Label));
    }

    public int AlbumState
    {
      get => this.albumState;
      set
      {
        this.SetPropertyValue<int>(ref this.albumState, value, nameof (AlbumState));
        this.NotifyPropertyChanged("AlbumBioState");
        this.NotifyPropertyChanged("IsAlbumAvailable");
      }
    }

    public bool IsAlbumAvailable => this.AlbumState == 0;

    public int BioState
    {
      get => this.bioState;
      set
      {
        this.SetPropertyValue<int>(ref this.bioState, value, nameof (BioState));
        this.NotifyPropertyChanged("AlbumBioState");
      }
    }

    public int AlbumBioState
    {
      get
      {
        switch (this.AlbumState)
        {
          case 0:
            return 0;
          case 1:
            return this.BioState != 2 ? this.BioState : 1;
          case 2:
            return this.BioState;
          case 3:
            return 3;
          default:
            return this.AlbumState;
        }
      }
    }

    public ObservableCollection<string> ExtendedBio
    {
      get => this.extendedBio;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.extendedBio, value, nameof (ExtendedBio));
      }
    }

    public ObservableCollection<string> ExtendedReview
    {
      get => this.extendedReview;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.extendedReview, value, nameof (ExtendedReview));
      }
    }

    [IgnoreDataMember]
    [XmlIgnore]
    public bool IsBioAvailable
    {
      get => this.isBioAvailable;
      set => this.SetPropertyValue<bool>(ref this.isBioAvailable, value, nameof (IsBioAvailable));
    }

    [IgnoreDataMember]
    [XmlIgnore]
    public bool IsReviewAvailable
    {
      get => this.isReviewAvailable;
      set
      {
        this.SetPropertyValue<bool>(ref this.isReviewAvailable, value, nameof (IsReviewAvailable));
      }
    }

    [XmlIgnore]
    [IgnoreDataMember]
    public bool IsRelatedAvailable
    {
      get => this.isRelatedAvailable;
      set
      {
        this.SetPropertyValue<bool>(ref this.isRelatedAvailable, value, nameof (IsRelatedAvailable));
      }
    }

    [IgnoreDataMember]
    [XmlIgnore]
    public bool IsPhotoAvailable
    {
      get => this.isPhotoAvailable;
      set
      {
        this.SetPropertyValue<bool>(ref this.isPhotoAvailable, value, nameof (IsPhotoAvailable));
      }
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

    [XmlIgnore]
    [IgnoreDataMember]
    public bool IsLoadingBio
    {
      get => this.isLoadingBio;
      set
      {
        this.isLoadingBio = value;
        this.NotifyPropertyChanged("IsLoadingArtistItems");
      }
    }

    [XmlIgnore]
    [IgnoreDataMember]
    public bool IsLoadingAlbums
    {
      get => this.isLoadingAlbums;
      set
      {
        this.isLoadingAlbums = value;
        this.NotifyPropertyChanged("IsLoadingArtistItems");
      }
    }

    public bool IsLoadingArtistItems => this.isLoadingBio || this.isLoadingAlbums;

    [IgnoreDataMember]
    [XmlIgnore]
    public bool IsExplicit
    {
      get => this.isExplicit;
      set => this.SetPropertyValue<bool>(ref this.isExplicit, value, nameof (IsExplicit));
    }

    public ObservableCollection<ViewModelBase> Items
    {
      get
      {
        if (this.items == null)
          this.items = new ObservableCollection<ViewModelBase>();
        return this.items;
      }
      set
      {
        this.SetPropertyValue<ObservableCollection<ViewModelBase>>(ref this.items, value, nameof (Items));
      }
    }

    public ObservableCollection<MediaItem> AlbumsBySameArtist
    {
      get => this.albumsbySameArtist;
      set
      {
        this.SetPropertyValue<ObservableCollection<MediaItem>>(ref this.albumsbySameArtist, value, nameof (AlbumsBySameArtist));
      }
    }

    [XmlIgnore]
    [IgnoreDataMember]
    public string DiscographyNotFound
    {
      get => this.discographyNotFound;
      set
      {
        this.SetPropertyValue<string>(ref this.discographyNotFound, value, nameof (DiscographyNotFound));
      }
    }

    public string Artist
    {
      get => this.artist;
      set => this.SetPropertyValue<string>(ref this.artist, value, nameof (Artist));
    }

    public string ArtistId { get; set; }

    public string ReviewUrl { get; set; }

    public string BioUrl { get; set; }

    public string AlbumsBySameArtistUrl { get; set; }

    public string PhotoUrl { get; set; }

    public string RelatedUrl { get; set; }

    public int MinimumItems { get; set; }

    public ObservableCollection<ViewModelBase> DisplayedItems
    {
      get => this.displayedItems;
      set
      {
        this.SetPropertyValue<ObservableCollection<ViewModelBase>>(ref this.displayedItems, value, nameof (DisplayedItems));
      }
    }

    public VerticalAlignment BusyIndicatorVerticalAlignment
    {
      get => this.busyIndicatorVerticalAlignment;
      set
      {
        this.SetPropertyValue<VerticalAlignment>(ref this.busyIndicatorVerticalAlignment, value, nameof (BusyIndicatorVerticalAlignment));
      }
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      this.IsBusy = true;
      this.CurrentState = 3;
      this.BioState = 3;
      this.AlbumState = 3;
      this.IsLoadingAlbums = true;
      this.isLoadingBio = true;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        IZuneCatalogServiceManager catalogServiceManager = ServiceManagerFactory.CreateZuneCatalogServiceManager();
        catalogServiceManager.EventGetVideoItemCompleted += new EventHandler<ServiceProxyEventArgs<string>>(this.GetResponseCompleted);
        catalogServiceManager.GetMediaDetail(this.Id, MediaType.music_album.ToString(), (object) this);
      });
    }

    private static Uri GetSafeUri(string uriString)
    {
      Uri safeUri = (Uri) null;
      if (!string.IsNullOrWhiteSpace(uriString))
      {
        try
        {
          safeUri = new Uri(uriString);
        }
        catch (UriFormatException ex)
        {
        }
      }
      return safeUri;
    }

    private void LoadReview()
    {
      WebClient webClient = new WebClient();
      webClient.DownloadStringCompleted += (DownloadStringCompletedEventHandler) ((o, e) =>
      {
        if (e != null && e.Error == null)
        {
          string albumReview = ZuneVideoHelper.ParseAlbumReview(e.Result);
          this.ExtendedReview = TextUtil.SplitString(2500, albumReview);
          this.IsReviewAvailable = !string.IsNullOrWhiteSpace(albumReview);
        }
        else
          this.IsReviewAvailable = false;
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          this.LoadPhotos();
        });
      });
      Uri safeUri = AlbumItem.GetSafeUri(this.ReviewUrl);
      if (safeUri != (Uri) null)
        webClient.DownloadStringAsync(safeUri);
      else
        this.isReviewAvailable = false;
    }

    private void LoadBio()
    {
      WebClient webClient = new WebClient();
      webClient.DownloadStringCompleted += (DownloadStringCompletedEventHandler) ((o, e) =>
      {
        this.IsLoadingBio = false;
        if (e != null && e.Error == null)
        {
          string artistBio = ZuneVideoHelper.ParseArtistBio(e.Result);
          this.IsBioAvailable = !string.IsNullOrEmpty(artistBio);
          this.ExtendedBio = TextUtil.SplitString(2500, artistBio);
          if (this.IsBioAvailable)
            this.BioState = 0;
          else
            this.BioState = 2;
        }
        else
          this.BioState = 1;
      });
      Uri safeUri = AlbumItem.GetSafeUri(this.BioUrl);
      if (safeUri != (Uri) null)
      {
        webClient.DownloadStringAsync(safeUri);
      }
      else
      {
        this.IsBioAvailable = false;
        this.BioState = 2;
        this.IsLoadingBio = false;
      }
    }

    private void LoadPhotos()
    {
      WebClient webClient = new WebClient();
      webClient.DownloadStringCompleted += (DownloadStringCompletedEventHandler) ((o, e) =>
      {
        if (e != null && e.Error == null)
        {
          this.Photos = ZuneVideoHelper.GetAlbumPhotos(e.Result);
          this.IsPhotoAvailable = this.Photos != null && this.Photos.Count > 0;
        }
        else
          this.IsPhotoAvailable = false;
        ThreadPool.QueueUserWorkItem((WaitCallback) delegate
        {
          this.LoadRelated();
        });
      });
      Uri safeUri = AlbumItem.GetSafeUri(this.PhotoUrl);
      if (safeUri != (Uri) null)
        webClient.DownloadStringAsync(safeUri);
      else
        this.IsPhotoAvailable = false;
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
      Uri safeUri = AlbumItem.GetSafeUri(this.RelatedUrl);
      if (safeUri != (Uri) null)
        webClient.DownloadStringAsync(safeUri);
      else
        this.isRelatedAvailable = false;
    }

    private void LoadAlbumsOfSameArtist()
    {
      WebClient webClient = new WebClient();
      webClient.DownloadStringCompleted += (DownloadStringCompletedEventHandler) ((o, e) =>
      {
        this.IsLoadingAlbums = false;
        if (e != null && e.Error == null)
        {
          this.AlbumsBySameArtist = ZuneVideoHelper.GetAlbumsOfSameArtist(e.Result);
          if (this.albumsbySameArtist == null || this.AlbumsBySameArtist.Count == 0)
          {
            this.DiscographyNotFound = Resource.Discography_NotFound;
            this.AlbumState = 2;
          }
          else
          {
            this.DiscographyNotFound = string.Empty;
            this.AlbumState = 0;
          }
        }
        else
        {
          this.DiscographyNotFound = Resource.Discography_NotFound;
          this.AlbumState = 1;
        }
      });
      if (AlbumItem.GetSafeUri(this.AlbumsBySameArtistUrl) != (Uri) null)
      {
        webClient.DownloadStringAsync(new Uri(this.AlbumsBySameArtistUrl));
      }
      else
      {
        this.albumState = 2;
        this.DiscographyNotFound = Resource.Discography_NotFound;
        this.IsLoadingAlbums = false;
      }
    }

    private void GetResponseCompleted(object sender, ServiceProxyEventArgs<string> e)
    {
      ThreadManager.UIThread.AssertIsNotCurrentThread();
      this.IsBusy = false;
      (sender as IZuneCatalogServiceManager).EventGetVideoItemCompleted -= new EventHandler<ServiceProxyEventArgs<string>>(this.GetResponseCompleted);
      if (e.Error == null && e.UserState != null)
      {
        ZuneVideoHelper.ParseAlbumResponse(e.Result, this);
        this.CurrentState = 0;
        this.LastRefreshTime = DateTime.UtcNow;
        this.LoadReview();
        this.LoadBio();
        this.LoadAlbumsOfSameArtist();
      }
      else
      {
        this.CurrentState = 1;
        this.BioState = 1;
        this.AlbumState = 1;
      }
    }
  }
}
