// *********************************************************
// Type: LRC.ViewModel.SearchItemViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows;
using System.Xml.Serialization;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Utils;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class SearchItemViewModel : ViewModelBase
  {
    internal const string GameRatingIconUrlBase = "http://epix.xbox.com/consoleassets/vm_ems/DetailsPages/RatingIcons/{0}.png";
    private const string PerfGetMovieDetails = "SearchItemViewModel:GetMovieDetails";
    private const string PerfGetTvSeasonsForSeries = "SearchItemViewModel:GetTvSeasonsForSeries";
    private const string PerfGetTvEpisodesForSeason = "SearchItemViewModel:GetTvEpisodesForSeason";
    private const string PerfGetTVEpisodeDetails = "SearchItemViewModel:GetTVEpisodeDetails";
    private const string PerfGetAppDetails = "SearchItemViewModel:GetAppDetails";
    private const string PerfGetGameDetails = "SearchItemViewModel:GetGameDetails";
    private const string PerfFetchMoreGetTvSeasonsForSeries = "SearchItemViewModel:FetchMoreGetTvSeasonsForSeries";
    private const string PerfFetchMoreGetTvEpisodesForSeason = "SearchItemViewModel:FetchMoreGetTvEpisodesForSeason";
    private const string PerfProcessSearchItemResult = "SearchItemViewModel:ProcessSearchItemResult";
    private const string PerfFetchMoreTvSeasonsForSeries = "SearchItemViewModel:FetchMoreTvSeasonsForSeries";
    private const string PerfFetchMoreTvEpisodesForSeason = "SearchItemViewModel:FetchMoreTvEpisodesForSeason";
    private const string ComponentName = "SearchItemViewModel";
    private const uint MaxSearchResults = 500;
    private const uint ResultsPerSearch = 20;
    private const int MaxImageCount = 16;
    private bool moreResultsAvailable;
    private VerticalAlignment busyIndicatorVerticalAlignment;
    private RelatedItemsViewModel relatedItemsViewModel;
    private string id;
    private string continuationToken;
    private string detailsUrl;
    private bool? isActionable;
    private DateTime? releaseDate;
    private LRC.Service.Search.Filter filter;
    private ObservableCollection<string> genres;
    private string itemType;
    private string parentItemType;
    private string parentalRating;
    private string parentalRatingSystem;
    private double? criticRating;
    private double? averageUserRating;
    private int? userRatingCount;
    private TimeSpan? duration;
    private string releaseDetails;
    private string extendedReleaseDetails;
    private string gameReleaseDetails;
    private string ratingIconUrl;
    private bool isAddingData;
    private Size? imageSize;
    private CultureInfo accountCulture = new CultureInfo(XboxLiveGamer.CurrentGamer.LegalLocale);
    private uint titleId;
    private string parentId;
    private string gameReleaseDate;
    private string developer;
    private string publisher;
    private ObservableCollection<string> gameRatingDescriptor;
    private string imagesNotFound;
    private string artist;
    private string artistId;
    private string bio;
    private ObservableCollection<string> smallImageUrls;
    private ObservableCollection<string> largeImageUrls;
    private ObservableCollection<MusicAlbumData> albums;
    private ObservableCollection<MusicTrackData> tracks;
    private int? discNumber;
    private int? trackNumber;
    private bool isCastAndCrewNotFound;
    private bool isCastAndCrewAvailable;
    private string actorsTitle = Resource.Actors_Header;
    private ObservableCollection<string> actors;
    private string directorsTitle = Resource.Directors_Header;
    private ObservableCollection<string> directors;
    private string writersTitle = Resource.Writers_Header;
    private ObservableCollection<string> writers;
    private bool showProviders = true;
    private ObservableCollection<ProviderViewModel> providers;
    private ObservableCollection<TvSeasonData> televisionSeasons;
    private int? televisionSeasonNumber;
    private int? televisionEpisodeNumber;
    private ObservableCollection<TvEpisodeData> televsionEpisodes;
    private bool televisionSeriesHasSeasons;

    public SearchItemViewModel()
      : this((SearchData) null)
    {
    }

    public SearchItemViewModel(SearchData searchData)
    {
      this.RelatedItemsViewModel = new RelatedItemsViewModel(searchData != null && searchData.Filter == LRC.Service.Search.Filter.Games);
      this.LifetimeInMinutes = 60;
      this.Initialize(searchData, false);
    }

    public SearchItemViewModel(string itemId, string itemType)
      : this(itemId, itemType, (string) null, (string) null)
    {
    }

    [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Naming convention matches existing data structures.")]
    public SearchItemViewModel(string itemId, string itemType, string itemName, string detailsUrl)
    {
      if (string.IsNullOrEmpty(itemType))
        throw new ArgumentNullException(nameof (itemType));
      this.RelatedItemsViewModel = new RelatedItemsViewModel();
      this.ItemType = itemType;
      this.Title = itemName;
      this.DetailsUrl = detailsUrl;
      this.LifetimeInMinutes = 60;
      if (!string.IsNullOrEmpty(itemId))
        this.Id = itemId;
      if (!string.IsNullOrEmpty(this.ItemType))
        this.Filter = EdsResponseParser.ParseFilter(this.ItemType);
      this.RelatedItemsViewModel.UseMicrosoftItemsOnly = this.Filter == LRC.Service.Search.Filter.Games;
    }

    public SearchItemViewModel(uint titleId, string itemType)
      : this((string) null, itemType, (string) null, (string) null)
    {
      this.TitleId = titleId;
    }

    public RelatedItemsViewModel RelatedItemsViewModel
    {
      get => this.relatedItemsViewModel;
      set
      {
        this.SetPropertyValue<RelatedItemsViewModel>(ref this.relatedItemsViewModel, value, nameof (RelatedItemsViewModel));
      }
    }

    [XmlIgnore]
    [IgnoreDataMember]
    public VerticalAlignment BusyIndicatorVerticalAlignment
    {
      get => this.busyIndicatorVerticalAlignment;
      set
      {
        this.SetPropertyValue<VerticalAlignment>(ref this.busyIndicatorVerticalAlignment, value, nameof (BusyIndicatorVerticalAlignment));
      }
    }

    public string Id
    {
      get => this.id;
      set
      {
        this.SetPropertyValue<string>(ref this.id, value, nameof (Id));
        this.RelatedItemsViewModel.Id = this.Id;
      }
    }

    public bool IsNowPlaying => NowPlayingItemViewModel.IsNowPlaying(this);

    public string ProductType { get; set; }

    public string DetailsUrl
    {
      get => this.detailsUrl;
      set => this.SetPropertyValue<string>(ref this.detailsUrl, value, nameof (DetailsUrl));
    }

    public bool? IsActionable
    {
      get => this.isActionable;
      set => this.SetPropertyValue<bool?>(ref this.isActionable, value, nameof (IsActionable));
    }

    public DateTime? ReleaseDate
    {
      get => this.releaseDate;
      set => this.SetPropertyValue<DateTime?>(ref this.releaseDate, value, nameof (ReleaseDate));
    }

    public LRC.Service.Search.Filter Filter
    {
      get => this.filter;
      set => this.SetPropertyValue<LRC.Service.Search.Filter>(ref this.filter, value, nameof (Filter));
    }

    public ObservableCollection<string> Genres
    {
      get => this.genres;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.genres, value, nameof (Genres));
      }
    }

    public string ItemType
    {
      get => this.itemType;
      set
      {
        this.SetPropertyValue<string>(ref this.itemType, value, nameof (ItemType));
        this.RelatedItemsViewModel.ItemType = this.itemType;
      }
    }

    public string ParentItemType
    {
      get => this.parentItemType;
      set => this.SetPropertyValue<string>(ref this.parentItemType, value, nameof (ParentItemType));
    }

    public string ParentalRating
    {
      get => this.parentalRating;
      set => this.SetPropertyValue<string>(ref this.parentalRating, value, nameof (ParentalRating));
    }

    public string ParentalRatingSystem
    {
      get => this.parentalRatingSystem;
      set
      {
        this.SetPropertyValue<string>(ref this.parentalRatingSystem, value, nameof (ParentalRatingSystem));
      }
    }

    public double? CriticRating
    {
      get => this.criticRating;
      set => this.SetPropertyValue<double?>(ref this.criticRating, value, nameof (CriticRating));
    }

    public double? AverageUserRating
    {
      get => this.averageUserRating;
      set
      {
        this.SetPropertyValue<double?>(ref this.averageUserRating, value, nameof (AverageUserRating));
      }
    }

    public int? UserRatingCount
    {
      get => this.userRatingCount;
      set => this.SetPropertyValue<int?>(ref this.userRatingCount, value, nameof (UserRatingCount));
    }

    public TimeSpan? Duration
    {
      get => this.duration;
      set => this.SetPropertyValue<TimeSpan?>(ref this.duration, value, nameof (Duration));
    }

    public string ReleaseDetails
    {
      get => this.releaseDetails;
      set => this.SetPropertyValue<string>(ref this.releaseDetails, value, nameof (ReleaseDetails));
    }

    public string ExtendedReleaseDetails
    {
      get => this.extendedReleaseDetails;
      set
      {
        this.SetPropertyValue<string>(ref this.extendedReleaseDetails, value, nameof (ExtendedReleaseDetails));
      }
    }

    public string GameReleaseDetails
    {
      get => this.gameReleaseDetails;
      set
      {
        this.SetPropertyValue<string>(ref this.gameReleaseDetails, value, nameof (GameReleaseDetails));
      }
    }

    public string RatingIconUrl
    {
      get => this.ratingIconUrl;
      set => this.SetPropertyValue<string>(ref this.ratingIconUrl, value, nameof (RatingIconUrl));
    }

    [XmlIgnore]
    public bool IsAddingData
    {
      get => this.isAddingData;
      set => this.SetPropertyValue<bool>(ref this.isAddingData, value, nameof (IsAddingData));
    }

    [DataMember]
    public Size? ImageSize
    {
      get => this.imageSize;
      set => this.SetPropertyValue<Size?>(ref this.imageSize, value, nameof (ImageSize));
    }

    public uint TitleId
    {
      get => this.titleId;
      set => this.SetPropertyValue<uint>(ref this.titleId, value, nameof (TitleId));
    }

    public string ParentId
    {
      get => this.parentId;
      set => this.SetPropertyValue<string>(ref this.parentId, value, nameof (ParentId));
    }

    public string GameReleaseDate
    {
      get => this.gameReleaseDate;
      set
      {
        this.SetPropertyValue<string>(ref this.gameReleaseDate, value, nameof (GameReleaseDate));
      }
    }

    public string Developer
    {
      get => this.developer;
      set => this.SetPropertyValue<string>(ref this.developer, value, nameof (Developer));
    }

    public string Publisher
    {
      get => this.publisher;
      set => this.SetPropertyValue<string>(ref this.publisher, value, nameof (Publisher));
    }

    public ObservableCollection<string> GameRatingDescriptor
    {
      get => this.gameRatingDescriptor;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.gameRatingDescriptor, value, nameof (GameRatingDescriptor));
      }
    }

    public string ImagesNotFound
    {
      get => this.imagesNotFound;
      set => this.SetPropertyValue<string>(ref this.imagesNotFound, value, nameof (ImagesNotFound));
    }

    public bool IsGame
    {
      get
      {
        bool isGame = false;
        if (!string.IsNullOrEmpty(this.ItemType))
        {
          switch (this.ItemType.ToUpperInvariant())
          {
            case "XBOX360GAME":
            case "XBOXARCADEGAME":
            case "XBOXORIGINALGAME":
            case "XBOXGAMETRIAL":
            case "XBOX360GAMEDEMO":
            case "XBOXXNACOMMUNITYGAME":
              isGame = true;
              break;
          }
        }
        return isGame;
      }
    }

    public bool IsMusicAlbum
    {
      get => string.Compare(this.ItemType, "MUSICALBUM", StringComparison.OrdinalIgnoreCase) == 0;
    }

    public string Artist
    {
      get => this.artist;
      set => this.SetPropertyValue<string>(ref this.artist, value, nameof (Artist));
    }

    public string ArtistId
    {
      get => this.artistId;
      set => this.SetPropertyValue<string>(ref this.artistId, value, nameof (ArtistId));
    }

    public string Bio
    {
      get => this.bio;
      set => this.SetPropertyValue<string>(ref this.bio, value, nameof (Bio));
    }

    public ObservableCollection<string> SmallImageUrls
    {
      get => this.smallImageUrls;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.smallImageUrls, value, nameof (SmallImageUrls));
      }
    }

    public ObservableCollection<string> LargeImageUrls
    {
      get => this.largeImageUrls;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.largeImageUrls, value, nameof (LargeImageUrls));
      }
    }

    public ObservableCollection<MusicAlbumData> Albums
    {
      get => this.albums;
      set
      {
        this.SetPropertyValue<ObservableCollection<MusicAlbumData>>(ref this.albums, value, nameof (Albums));
      }
    }

    public ObservableCollection<MusicTrackData> Tracks
    {
      get => this.tracks;
      set
      {
        this.SetPropertyValue<ObservableCollection<MusicTrackData>>(ref this.tracks, value, nameof (Tracks));
      }
    }

    public int? DiscNumber
    {
      get => this.discNumber;
      set => this.SetPropertyValue<int?>(ref this.discNumber, value, nameof (DiscNumber));
    }

    public int? TrackNumber
    {
      get => this.trackNumber;
      set => this.SetPropertyValue<int?>(ref this.trackNumber, value, nameof (TrackNumber));
    }

    [XmlIgnore]
    [IgnoreDataMember]
    public bool IsCastAndCrewAvailable
    {
      get => this.isCastAndCrewAvailable;
      set
      {
        this.SetPropertyValue<bool>(ref this.isCastAndCrewAvailable, value, nameof (IsCastAndCrewAvailable));
      }
    }

    [IgnoreDataMember]
    [XmlIgnore]
    public bool IsCastAndCrewNotFound
    {
      get => this.isCastAndCrewNotFound;
      set
      {
        this.SetPropertyValue<bool>(ref this.isCastAndCrewNotFound, value, nameof (IsCastAndCrewNotFound));
      }
    }

    public string ActorsTitle
    {
      get => this.actorsTitle;
      set => this.SetPropertyValue<string>(ref this.actorsTitle, value, nameof (ActorsTitle));
    }

    public ObservableCollection<string> Actors
    {
      get => this.actors;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.actors, value, nameof (Actors));
        this.ActorsTitle = this.Actors == null || this.Actors.Count <= 1 ? Resource.Actor_Header : Resource.Actors_Header;
      }
    }

    public string DirectorsTitle
    {
      get => this.directorsTitle;
      set => this.SetPropertyValue<string>(ref this.directorsTitle, value, nameof (DirectorsTitle));
    }

    public ObservableCollection<string> Directors
    {
      get => this.directors;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.directors, value, nameof (Directors));
        this.DirectorsTitle = this.Directors == null || this.Directors.Count <= 1 ? Resource.Director_Header : Resource.Directors_Header;
      }
    }

    public string WritersTitle
    {
      get => this.writersTitle;
      set => this.SetPropertyValue<string>(ref this.writersTitle, value, nameof (WritersTitle));
    }

    public ObservableCollection<string> Writers
    {
      get => this.writers;
      set
      {
        this.SetPropertyValue<ObservableCollection<string>>(ref this.writers, value, nameof (Writers));
        this.writersTitle = this.Writers == null || this.Writers.Count != 1 ? Resource.Writers_Header : Resource.Writer_Header;
      }
    }

    public bool ShowProviders
    {
      get => this.showProviders;
      set => this.SetPropertyValue<bool>(ref this.showProviders, value, nameof (ShowProviders));
    }

    public ObservableCollection<ProviderViewModel> Providers
    {
      get => this.providers;
      set
      {
        this.SetPropertyValue<ObservableCollection<ProviderViewModel>>(ref this.providers, value, nameof (Providers));
        this.ShowProviders = this.Providers != null && this.Providers.Count > 0 && !this.IsNowPlaying;
      }
    }

    public ObservableCollection<TvSeasonData> TelevisionSeasons
    {
      get => this.televisionSeasons;
      set
      {
        this.SetPropertyValue<ObservableCollection<TvSeasonData>>(ref this.televisionSeasons, value, nameof (TelevisionSeasons));
      }
    }

    public int? TelevisionSeasonNumber
    {
      get => this.televisionSeasonNumber;
      set
      {
        this.SetPropertyValue<int?>(ref this.televisionSeasonNumber, value, nameof (TelevisionSeasonNumber));
      }
    }

    public int? TelevisionEpisodeNumber
    {
      get => this.televisionEpisodeNumber;
      set
      {
        this.SetPropertyValue<int?>(ref this.televisionEpisodeNumber, value, nameof (TelevisionEpisodeNumber));
      }
    }

    public ObservableCollection<TvEpisodeData> TelevisionEpisodes
    {
      get => this.televsionEpisodes;
      set
      {
        this.SetPropertyValue<ObservableCollection<TvEpisodeData>>(ref this.televsionEpisodes, value, nameof (TelevisionEpisodes));
      }
    }

    public bool TelevisionSeriesHasSeasons
    {
      get => this.televisionSeriesHasSeasons;
      set
      {
        this.SetPropertyValue<bool>(ref this.televisionSeriesHasSeasons, value, nameof (TelevisionSeriesHasSeasons));
      }
    }

    private static uint ConsoleLiveTVProviderTitleId
    {
      get => MainViewModel.Instance.ConsoleLiveTVProviderTitleId;
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      this.continuationToken = (string) null;
      if (string.IsNullOrEmpty(this.Id) || string.IsNullOrEmpty(this.ItemType))
        return;
      ISearchServiceManager searchServiceManager = LRC.Service.ServiceManagerFactory.CreateSearchServiceManager();
      this.BusyIndicatorVerticalAlignment = (VerticalAlignment) 0;
      switch (this.ItemType.ToUpperInvariant())
      {
        case "MOVIE":
          this.IsBusy = true;
          this.CurrentState = 3;
          searchServiceManager.EventGetMovieDetailsCompleted += new EventHandler<LRC.Service.ServiceProxyEventArgs<MovieData>>(this.SearchServiceManager_EventGetMovieDetailsCompleted);
          searchServiceManager.GetMovieDetails(this.Id, this.Title, this.DetailsUrl, SearchItemViewModel.ConsoleLiveTVProviderTitleId, (object) null);
          break;
        case "TVSERIES":
          this.IsBusy = true;
          this.CurrentState = 3;
          if (this.TelevisionSeriesHasSeasons)
          {
            searchServiceManager.EventGetTvSeasonsForSeriesCompleted += new EventHandler<LRC.Service.ServiceProxyEventArgs<TvSeriesData>>(this.SearchServiceManager_EventGetTvSeasonsForSeriesCompleted);
            searchServiceManager.GetTvSeasonsForSeries(this.Id, this.continuationToken, 20U, this.Title, this.DetailsUrl, SearchItemViewModel.ConsoleLiveTVProviderTitleId, (object) null);
            break;
          }
          searchServiceManager.EventGetTvEpisodesForSeasonCompleted += new EventHandler<LRC.Service.ServiceProxyEventArgs<TvSeasonData>>(this.SearchServiceManager_EventGetTvEpisodesForSeasonCompleted);
          searchServiceManager.GetTvEpisodesForSeason(this.Id, this.continuationToken, 20U, this.Title, this.DetailsUrl, SearchItemViewModel.ConsoleLiveTVProviderTitleId, (object) null);
          break;
        case "TVSEASON":
          this.IsBusy = true;
          this.CurrentState = 3;
          searchServiceManager.EventGetTvEpisodesForSeasonCompleted += new EventHandler<LRC.Service.ServiceProxyEventArgs<TvSeasonData>>(this.SearchServiceManager_EventGetTvEpisodesForSeasonCompleted);
          searchServiceManager.GetTvEpisodesForSeason(this.Id, this.continuationToken, 20U, this.Title, this.DetailsUrl, SearchItemViewModel.ConsoleLiveTVProviderTitleId, (object) null);
          break;
        case "TVEPISODE":
        case "TVSHOW":
          this.IsBusy = true;
          this.CurrentState = 3;
          searchServiceManager.EventGetTvEpisodeDetailsCompleted += new EventHandler<LRC.Service.ServiceProxyEventArgs<TvEpisodeData>>(this.SearchServiceManager_EventGetTvEpisodeDetailsCompleted);
          searchServiceManager.GetTvEpisodeDetails(this.Id, this.Title, this.DetailsUrl, SearchItemViewModel.ConsoleLiveTVProviderTitleId, (object) null);
          break;
        case "XBOXAPP":
          this.IsBusy = true;
          this.CurrentState = 3;
          searchServiceManager.EventGetAppDetailsCompleted += new EventHandler<LRC.Service.ServiceProxyEventArgs<XboxAppData>>(this.SearchServiceManager_EventGetAppDetailsCompleted);
          searchServiceManager.GetAppDetails(this.Id, this.Title, this.DetailsUrl, (object) null);
          break;
        case "XBOXARCADEGAME":
        case "XBOX360GAME":
        case "XBOXGAMECONSUMABLE":
        case "XBOX360GAMECONTENT":
        case "XBOX360GAMEDEMO":
        case "XBOXGAMERTILE":
        case "XBOXGAMETRAILER":
        case "XBOXGAMETRIAL":
        case "XBOXGAMEVIDEO":
        case "XBOXORIGINALGAME":
        case "XBOXTHEME":
        case "XBOXXNACOMMUNITYGAME":
          this.IsBusy = true;
          this.CurrentState = 3;
          searchServiceManager.EventGetGameDetailsCompleted += new EventHandler<LRC.Service.ServiceProxyEventArgs<XboxGameData>>(this.SearchServiceManagerEventGetGameDetailsCompleted);
          if (!string.IsNullOrEmpty(this.Id))
          {
            searchServiceManager.GetGameDetailsByMediaId(this.Id, (object) null);
            break;
          }
          searchServiceManager.GetGameDetailsByTitleId(this.TitleId, (object) null);
          break;
      }
    }

    public void FetchMoreData()
    {
      if (this.IsBusy || !this.moreResultsAvailable || string.IsNullOrEmpty(this.Id) || string.IsNullOrEmpty(this.ItemType))
        return;
      this.BusyIndicatorVerticalAlignment = (VerticalAlignment) 2;
      switch (this.ItemType.ToUpperInvariant())
      {
        case "TVSERIES":
          ISearchServiceManager searchServiceManager1 = LRC.Service.ServiceManagerFactory.CreateSearchServiceManager();
          if (this.TelevisionSeriesHasSeasons && this.TelevisionSeasons != null && this.TelevisionSeasons.Count < 500)
          {
            this.IsBusy = true;
            this.CurrentState = 3;
            searchServiceManager1.EventGetTvSeasonsForSeriesCompleted += new EventHandler<LRC.Service.ServiceProxyEventArgs<TvSeriesData>>(this.SearchServiceManager_EventFetchMoreTvSeasonsForSeriesCompleted);
            searchServiceManager1.GetTvSeasonsForSeries(this.Id, this.continuationToken, 20U, this.Title, this.DetailsUrl, SearchItemViewModel.ConsoleLiveTVProviderTitleId, (object) null);
            break;
          }
          if (this.TelevisionEpisodes == null || this.TelevisionEpisodes.Count >= 500)
            break;
          this.IsBusy = true;
          this.CurrentState = 3;
          searchServiceManager1.EventGetTvEpisodesForSeasonCompleted += new EventHandler<LRC.Service.ServiceProxyEventArgs<TvSeasonData>>(this.SearchServiceManager_EventFetchMoreTvEpisodesForSeasonCompleted);
          searchServiceManager1.GetTvEpisodesForSeason(this.Id, this.continuationToken, 20U, this.Title, this.DetailsUrl, SearchItemViewModel.ConsoleLiveTVProviderTitleId, (object) null);
          break;
        case "TVSEASON":
          if (this.TelevisionEpisodes == null || this.TelevisionEpisodes.Count >= 500)
            break;
          this.IsBusy = true;
          this.CurrentState = 3;
          ISearchServiceManager searchServiceManager2 = LRC.Service.ServiceManagerFactory.CreateSearchServiceManager();
          searchServiceManager2.EventGetTvEpisodesForSeasonCompleted += new EventHandler<LRC.Service.ServiceProxyEventArgs<TvSeasonData>>(this.SearchServiceManager_EventFetchMoreTvEpisodesForSeasonCompleted);
          searchServiceManager2.GetTvEpisodesForSeason(this.Id, this.continuationToken, 20U, this.Title, this.DetailsUrl, SearchItemViewModel.ConsoleLiveTVProviderTitleId, (object) null);
          break;
      }
    }

    public void Initialize(SearchData searchData) => this.Initialize(searchData, false);

    private static ObservableCollection<T> GetObservableCollection<T>(IEnumerable<T> list)
    {
      return list == null ? (ObservableCollection<T>) null : new ObservableCollection<T>(list);
    }

    private static ObservableCollection<ProviderViewModel> GetProviders(
      IEnumerable<Provider> providerList)
    {
      ObservableCollection<ProviderViewModel> providers = new ObservableCollection<ProviderViewModel>();
      if (providerList != null)
      {
        foreach (Provider provider in providerList)
          providers.Add(new ProviderViewModel(provider));
      }
      return providers;
    }

    private static string GetReleaseDetails(SearchItemViewModel item, bool isDetailed)
    {
      List<string> values = new List<string>();
      if (item.ReleaseDate.HasValue)
        values.Add(item.ReleaseDate.Value.Year.ToString((IFormatProvider) CultureInfo.CurrentCulture));
      if (!string.IsNullOrEmpty(item.ParentalRating) && isDetailed)
        values.Add(item.ParentalRating);
      if (item.Duration.HasValue)
        values.Add(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.DurationInMinutes, new object[1]
        {
          (object) (int) item.Duration.Value.TotalMinutes
        }));
      return string.Join(Resource.Search_DetailsSeparator, (IEnumerable<string>) values);
    }

    private static string GetGameReleaseDetails(SearchItemViewModel item)
    {
      List<string> values = new List<string>();
      if (item.ReleaseDate.HasValue)
        values.Add(item.ReleaseDate.Value.Year.ToString((IFormatProvider) CultureInfo.CurrentCulture));
      if (!string.IsNullOrEmpty(item.Developer))
        values.Add(item.Developer);
      return string.Join(Resource.Search_DetailsSeparator, (IEnumerable<string>) values);
    }

    private static string GetValueOrDefault(string originalValue, string newValue)
    {
      return string.IsNullOrEmpty(newValue) ? originalValue : newValue;
    }

    private static int? GetValueOrDefault(int? originalValue, int? newValue)
    {
      return !newValue.HasValue ? originalValue : newValue;
    }

    private void Initialize(SearchData searchData, bool loadRelatedItems)
    {
      if (searchData == null)
        return;
      if (loadRelatedItems)
        this.RelatedItemsViewModel.Load();
      this.LastRefreshTime = DateTime.UtcNow;
      this.Id = SearchItemViewModel.GetValueOrDefault(this.Id, searchData.Id);
      this.continuationToken = searchData.ContinuationToken;
      this.Title = SearchItemViewModel.GetValueOrDefault(this.Title, searchData.Name);
      this.Description = SearchItemViewModel.GetValueOrDefault(this.Description, searchData.Description);
      this.DetailsUrl = SearchItemViewModel.GetValueOrDefault(this.DetailsUrl, searchData.DetailsUrl);
      this.BackgroundImageUrl = SearchItemViewModel.GetValueOrDefault(this.BackgroundImageUrl, searchData.BackgroundImageUrl);
      this.IsActionable = searchData.IsActionable;
      this.ReleaseDate = searchData.ReleaseDate;
      this.Artist = SearchItemViewModel.GetValueOrDefault(this.Artist, searchData.ArtistName);
      this.ItemType = SearchItemViewModel.GetValueOrDefault(this.ItemType, searchData.ItemType);
      this.ParentItemType = SearchItemViewModel.GetValueOrDefault(this.ParentItemType, searchData.ParentItemType);
      this.Filter = searchData.Filter;
      this.ImageUrl = SearchItemViewModel.GetValueOrDefault(this.ImageUrl, searchData.ImageUrl);
      this.Genres = SearchItemViewModel.GetObservableCollection<string>((IEnumerable<string>) searchData.Genres);
      this.CriticRating = searchData.CriticRating;
      this.AverageUserRating = searchData.AverageUserRating;
      this.UserRatingCount = searchData.UserRatingCount;
      if (searchData.ImageHeight.HasValue && searchData.ImageWidth.HasValue)
        this.ImageSize = new Size?(new Size(searchData.ImageWidth.Value, searchData.ImageHeight.Value));
      this.TelevisionSeriesHasSeasons = !searchData.TvSeriesHasSeasons.HasValue || searchData.TvSeriesHasSeasons.Value;
      string newValue = (string) null;
      if (!string.IsNullOrEmpty(searchData.ParentalRating))
      {
        switch (this.filter)
        {
          case LRC.Service.Search.Filter.Movies:
            newValue = RatingStringsHelper.GetMovieRatingString(searchData.ParentalRatingSystem, searchData.ParentalRating, this.accountCulture);
            break;
          case LRC.Service.Search.Filter.TV:
            newValue = RatingStringsHelper.GetTVRatingString(searchData.ParentalRatingSystem, searchData.ParentalRating, this.accountCulture);
            break;
          case LRC.Service.Search.Filter.Games:
            newValue = RatingStringsHelper.GetGameRatingString(searchData.ParentalRating, this.accountCulture);
            break;
          case LRC.Service.Search.Filter.Apps:
            newValue = RatingStringsHelper.GetGameRatingString(searchData.ParentalRating, this.accountCulture);
            break;
        }
      }
      this.ParentalRating = SearchItemViewModel.GetValueOrDefault(searchData.ParentalRating, newValue);
      this.ParentalRatingSystem = SearchItemViewModel.GetValueOrDefault(searchData.ParentalRatingSystem, searchData.ParentalRatingSystem);
      this.InitializeMovieData(searchData as MovieData);
      this.InitializeMusicData(searchData as MusicData);
      this.InitializeXboxData(searchData as XboxData);
      this.InitializeTelevisionData(searchData as TvData);
      this.ExtendedReleaseDetails = SearchItemViewModel.GetReleaseDetails(this, true);
      this.ReleaseDetails = SearchItemViewModel.GetReleaseDetails(this, false);
      this.GameReleaseDetails = SearchItemViewModel.GetGameReleaseDetails(this);
    }

    private void InitializeMovieData(MovieData movieData)
    {
      if (movieData == null)
        return;
      this.Duration = movieData.Duration;
      this.Actors = SearchItemViewModel.GetObservableCollection<string>((IEnumerable<string>) movieData.Actors);
      this.Directors = SearchItemViewModel.GetObservableCollection<string>((IEnumerable<string>) movieData.Directors);
      this.Writers = SearchItemViewModel.GetObservableCollection<string>((IEnumerable<string>) movieData.Writers);
      this.IsCastAndCrewNotFound = (this.Actors == null || this.Actors.Count == 0) && (this.Directors == null || this.Directors.Count == 0) && (this.Writers == null || this.Writers.Count == 0);
      this.IsCastAndCrewAvailable = !this.isCastAndCrewNotFound;
      this.Providers = SearchItemViewModel.GetProviders((IEnumerable<Provider>) movieData.Providers);
    }

    private void InitializeMusicData(MusicData musicData)
    {
      if (musicData == null)
        return;
      this.Artist = musicData.Artist;
      this.ArtistId = musicData.ArtistId;
      if (musicData is MusicArtistData musicArtistData)
      {
        this.Bio = musicArtistData.Bio;
        int count1 = Math.Min(musicArtistData.SmallImages.Count, 16);
        this.SmallImageUrls = SearchItemViewModel.GetObservableCollection<string>((IEnumerable<string>) musicArtistData.SmallImages.GetRange(0, count1));
        int count2 = Math.Min(musicArtistData.LargeImages.Count, 16);
        this.LargeImageUrls = SearchItemViewModel.GetObservableCollection<string>((IEnumerable<string>) musicArtistData.LargeImages.GetRange(0, count2));
        this.Albums = SearchItemViewModel.GetObservableCollection<MusicAlbumData>((IEnumerable<MusicAlbumData>) musicArtistData.Albums);
      }
      else if (musicData is MusicAlbumData musicAlbumData)
      {
        this.Tracks = SearchItemViewModel.GetObservableCollection<MusicTrackData>((IEnumerable<MusicTrackData>) musicAlbumData.Tracks);
      }
      else
      {
        if (!(musicData is MusicTrackData musicTrackData))
          return;
        this.DiscNumber = musicTrackData.DiscNumber;
        this.TrackNumber = musicTrackData.TrackNumber;
        this.Duration = musicTrackData.Duration;
      }
    }

    private void InitializeXboxData(XboxData xboxData)
    {
      if (xboxData == null)
        return;
      this.TitleId = xboxData.TitleId;
      if (!(xboxData is XboxGameData xboxGameData))
        return;
      this.ParentId = xboxGameData.ParentId;
      this.TitleId = xboxGameData.TitleId;
      int count = Math.Min(xboxGameData.SlideShowImages.Count, 16);
      this.SmallImageUrls = SearchItemViewModel.GetObservableCollection<string>((IEnumerable<string>) xboxGameData.SlideShowImages.GetRange(0, count));
      this.LargeImageUrls = this.SmallImageUrls;
      this.ImagesNotFound = xboxGameData.SlideShowImages.Count <= 0 ? Resource.SearchGameDetail_NoImages : string.Empty;
      if (xboxGameData.ReleaseDate.HasValue)
        this.GameReleaseDate = xboxGameData.ReleaseDate.Value.Year.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      this.Developer = xboxGameData.Developer;
      this.Publisher = xboxGameData.Publisher;
      this.RatingIconUrl = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "http://epix.xbox.com/consoleassets/vm_ems/DetailsPages/RatingIcons/{0}.png", new object[1]
      {
        (object) xboxGameData.ParentalRating
      });
      this.ParentalRating = RatingStringsHelper.GetGameRatingString(xboxGameData.ParentalRating, this.accountCulture);
      this.ProductType = xboxGameData.ProductType;
      if (this.GameRatingDescriptor == null)
        this.GameRatingDescriptor = new ObservableCollection<string>();
      else
        this.GameRatingDescriptor.Clear();
      foreach (string ratingDescriptor in xboxGameData.GameRatingDescriptor)
        this.GameRatingDescriptor.Add(RatingStringsHelper.GetGameRatingDescriptor(ratingDescriptor, this.accountCulture));
    }

    private void InitializeTelevisionData(TvData televisionData)
    {
      switch (televisionData)
      {
        case TvEpisodeData tvEpisodeData:
          this.TelevisionSeasonNumber = SearchItemViewModel.GetValueOrDefault(this.TelevisionSeasonNumber, tvEpisodeData.SeasonNumber);
          this.TelevisionEpisodeNumber = SearchItemViewModel.GetValueOrDefault(this.TelevisionEpisodeNumber, tvEpisodeData.EpisodeNumber);
          this.Duration = tvEpisodeData.Duration;
          this.Actors = SearchItemViewModel.GetObservableCollection<string>((IEnumerable<string>) tvEpisodeData.Actors);
          this.Directors = SearchItemViewModel.GetObservableCollection<string>((IEnumerable<string>) tvEpisodeData.Directors);
          this.Writers = SearchItemViewModel.GetObservableCollection<string>((IEnumerable<string>) tvEpisodeData.Writers);
          this.IsCastAndCrewNotFound = (this.Actors == null || this.Actors.Count == 0) && (this.Directors == null || this.Directors.Count == 0) && (this.Writers == null || this.Writers.Count == 0);
          this.IsCastAndCrewAvailable = !this.isCastAndCrewNotFound;
          this.Providers = SearchItemViewModel.GetProviders((IEnumerable<Provider>) tvEpisodeData.Providers);
          break;
        case TvSeasonData tvSeasonData:
          this.TelevisionSeasonNumber = SearchItemViewModel.GetValueOrDefault(this.TelevisionSeasonNumber, tvSeasonData.SeasonNumber);
          if (tvSeasonData.TvEpisodes == null)
            break;
          this.TelevisionEpisodes = SearchItemViewModel.GetObservableCollection<TvEpisodeData>((IEnumerable<TvEpisodeData>) tvSeasonData.TvEpisodes.Values);
          break;
        case TvSeriesData tvSeriesData:
          this.TelevisionSeriesHasSeasons = true;
          if (tvSeriesData.TvSeasons != null)
            this.TelevisionSeasons = SearchItemViewModel.GetObservableCollection<TvSeasonData>((IEnumerable<TvSeasonData>) tvSeriesData.TvSeasons.Values);
          if (tvSeriesData.TvEpisodes == null)
            break;
          this.TelevisionSeriesHasSeasons = false;
          this.TelevisionEpisodes = SearchItemViewModel.GetObservableCollection<TvEpisodeData>((IEnumerable<TvEpisodeData>) tvSeriesData.TvEpisodes.Values);
          break;
      }
    }

    private void SearchServiceManager_EventGetMovieDetailsCompleted(
      object sender,
      LRC.Service.ServiceProxyEventArgs<MovieData> e)
    {
      this.IsBusy = false;
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetMovieDetailsCompleted -= new EventHandler<LRC.Service.ServiceProxyEventArgs<MovieData>>(this.SearchServiceManager_EventGetMovieDetailsCompleted);
      this.ProcessSearchItemResult((SearchData) e.Result, e.Error, true);
    }

    private void SearchServiceManager_EventGetAppDetailsCompleted(
      object sender,
      LRC.Service.ServiceProxyEventArgs<XboxAppData> e)
    {
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetAppDetailsCompleted -= new EventHandler<LRC.Service.ServiceProxyEventArgs<XboxAppData>>(this.SearchServiceManager_EventGetAppDetailsCompleted);
      this.ProcessSearchItemResult((SearchData) e.Result, e.Error, true);
    }

    private void SearchServiceManager_EventGetTvSeasonsForSeriesCompleted(
      object sender,
      LRC.Service.ServiceProxyEventArgs<TvSeriesData> e)
    {
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetTvSeasonsForSeriesCompleted -= new EventHandler<LRC.Service.ServiceProxyEventArgs<TvSeriesData>>(this.SearchServiceManager_EventGetTvSeasonsForSeriesCompleted);
      this.ProcessSearchItemResult((SearchData) e.Result, e.Error, true);
      if (e.Error != null)
        return;
      this.moreResultsAvailable = e.Result != null && e.Result.TvSeasons != null && e.Result.TvSeasons.Count >= 20;
    }

    private void SearchServiceManager_EventFetchMoreTvSeasonsForSeriesCompleted(
      object sender,
      LRC.Service.ServiceProxyEventArgs<TvSeriesData> e)
    {
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetTvSeasonsForSeriesCompleted -= new EventHandler<LRC.Service.ServiceProxyEventArgs<TvSeriesData>>(this.SearchServiceManager_EventFetchMoreTvSeasonsForSeriesCompleted);
      if (e.Error != null)
      {
        this.ProcessFetchMoreDataFailure(Resource.Search_ErrorText);
      }
      else
      {
        bool hasMoreResults = e.Result != null && e.Result.TvSeasons != null & e.Result.TvSeasons.Count >= 20;
        this.ProcessFetchMoreDataInitialization((SearchData) e.Result, hasMoreResults);
        if (e.Result != null && e.Result.TvSeasons != null && e.Result.TvSeasons.Count > 0)
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            foreach (TvSeasonData tvSeasonData in e.Result.TvSeasons.Values)
              this.TelevisionSeasons.Add(tvSeasonData);
          }, (object) this);
        this.ProcessFetchMoreDataComplete();
      }
    }

    private void SearchServiceManager_EventGetTvEpisodesForSeasonCompleted(
      object sender,
      LRC.Service.ServiceProxyEventArgs<TvSeasonData> e)
    {
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetTvEpisodesForSeasonCompleted -= new EventHandler<LRC.Service.ServiceProxyEventArgs<TvSeasonData>>(this.SearchServiceManager_EventGetTvEpisodesForSeasonCompleted);
      this.ProcessSearchItemResult((SearchData) e.Result, e.Error, true);
      if (e.Error != null)
        return;
      this.moreResultsAvailable = e.Result != null && e.Result.TvEpisodes != null && e.Result.TvEpisodes.Count >= 20;
    }

    private void SearchServiceManager_EventFetchMoreTvEpisodesForSeasonCompleted(
      object sender,
      LRC.Service.ServiceProxyEventArgs<TvSeasonData> e)
    {
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetTvEpisodesForSeasonCompleted -= new EventHandler<LRC.Service.ServiceProxyEventArgs<TvSeasonData>>(this.SearchServiceManager_EventFetchMoreTvEpisodesForSeasonCompleted);
      if (e.Error != null)
      {
        this.ProcessFetchMoreDataFailure(Resource.Search_ErrorText);
      }
      else
      {
        bool hasMoreResults = e.Result != null && e.Result.TvEpisodes != null & e.Result.TvEpisodes.Count >= 20;
        this.ProcessFetchMoreDataInitialization((SearchData) e.Result, hasMoreResults);
        if (e.Result != null && e.Result.TvEpisodes != null && e.Result.TvEpisodes.Count > 0)
          ThreadManager.UIThreadPost((SendOrPostCallback) delegate
          {
            foreach (TvEpisodeData tvEpisodeData in e.Result.TvEpisodes.Values)
              this.TelevisionEpisodes.Add(tvEpisodeData);
          }, (object) this);
        this.ProcessFetchMoreDataComplete();
      }
    }

    private void SearchServiceManager_EventGetTvEpisodeDetailsCompleted(
      object sender,
      LRC.Service.ServiceProxyEventArgs<TvEpisodeData> e)
    {
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetTvEpisodeDetailsCompleted -= new EventHandler<LRC.Service.ServiceProxyEventArgs<TvEpisodeData>>(this.SearchServiceManager_EventGetTvEpisodeDetailsCompleted);
      this.ProcessSearchItemResult((SearchData) e.Result, e.Error, true);
    }

    private void SearchServiceManagerEventGetGameDetailsCompleted(
      object sender,
      LRC.Service.ServiceProxyEventArgs<XboxGameData> e)
    {
      if (sender is ISearchServiceManager searchServiceManager)
        searchServiceManager.EventGetGameDetailsCompleted -= new EventHandler<LRC.Service.ServiceProxyEventArgs<XboxGameData>>(this.SearchServiceManagerEventGetGameDetailsCompleted);
      this.ProcessSearchItemResult((SearchData) e.Result, e.Error, true);
    }

    private void ProcessSearchItemResult(
      SearchData searchData,
      Exception error,
      bool loadRelatedItems)
    {
      if (error != null)
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          this.IsBusy = false;
          this.CurrentState = 1;
        }, (object) this);
      else if (searchData == null)
      {
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          this.IsBusy = false;
          this.CurrentState = 2;
        }, (object) this);
      }
      else
      {
        this.Initialize(searchData, loadRelatedItems);
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          this.IsBusy = false;
          this.CurrentState = 0;
        }, (object) this);
      }
    }

    private void ProcessFetchMoreDataFailure(string errorMessage)
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.IsBusy = false;
        this.CurrentState = 1;
        this.ShowNonfatalErrorMessage(errorMessage);
      }, (object) this);
    }

    private void ProcessFetchMoreDataInitialization(SearchData searchData, bool hasMoreResults)
    {
      this.LastRefreshTime = DateTime.UtcNow;
      this.IsAddingData = true;
      this.moreResultsAvailable = hasMoreResults;
      this.continuationToken = searchData?.ContinuationToken;
    }

    private void ProcessFetchMoreDataComplete()
    {
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        this.IsBusy = false;
        this.IsAddingData = false;
        this.CurrentState = 0;
      }, (object) this);
    }
  }
}
