// *********************************************************
// Type: LRC.ViewModel.ZuneCategory
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class ZuneCategory : ViewModelBase
  {
    private ObservableCollection<IItemList> movieList;
    private ObservableCollection<IItemList> televisionList;

    public ZuneCategory()
    {
      this.movieList = new ObservableCollection<IItemList>();
      this.televisionList = new ObservableCollection<IItemList>();
      this.LifetimeInMinutes = int.MaxValue;
      ObservableCollection<IItemList> movieList1 = this.movieList;
      VideoItemList videoItemList1 = new VideoItemList();
      videoItemList1.Uri = "hubs/movie";
      videoItemList1.Type = MediaType.movie;
      videoItemList1.Title = Resource.ZuneVideoMarketPlace_NewReleases;
      videoItemList1.Category = MediaCategory.NewReleases;
      videoItemList1.Template = "TwoPerRowTemplate";
      videoItemList1.Style = "ListBoxWithWrapping";
      videoItemList1.MinimumItems = 20;
      VideoItemList videoItemList2 = videoItemList1;
      movieList1.Add((IItemList) videoItemList2);
      ObservableCollection<IItemList> movieList2 = this.movieList;
      VideoItemList videoItemList3 = new VideoItemList();
      videoItemList3.Uri = "movie?chunkSize=50&orderby=downloadRank";
      videoItemList3.Type = MediaType.movie;
      videoItemList3.Title = Resource.ZuneVideoMarketPlace_TopPurchases;
      videoItemList3.Category = MediaCategory.TopPurchases;
      videoItemList3.Template = "TwoPerRowTemplate";
      videoItemList3.Style = "ListBoxWithWrapping";
      videoItemList3.MinimumItems = 20;
      VideoItemList videoItemList4 = videoItemList3;
      movieList2.Add((IItemList) videoItemList4);
      ObservableCollection<IItemList> movieList3 = this.movieList;
      VideoItemList videoItemList5 = new VideoItemList();
      videoItemList5.Uri = "movie?chunkSize=50&orderby=rentalRank";
      videoItemList5.Type = MediaType.movie;
      videoItemList5.Title = Resource.ZuneVideoMarketPlace_TopRentals;
      videoItemList5.Category = MediaCategory.TopRentals;
      videoItemList5.Template = "TwoPerRowTemplate";
      videoItemList5.Style = "ListBoxWithWrapping";
      videoItemList5.MinimumItems = 20;
      VideoItemList videoItemList6 = videoItemList5;
      movieList3.Add((IItemList) videoItemList6);
      ObservableCollection<IItemList> movieList4 = this.movieList;
      VideoItemList videoItemList7 = new VideoItemList();
      videoItemList7.Uri = "movieGenre";
      videoItemList7.Type = MediaType.movie;
      videoItemList7.Title = Resource.ZuneVideoMarketPlace_Genres;
      videoItemList7.Category = MediaCategory.Genres;
      videoItemList7.Template = "ListTemplate";
      videoItemList7.MinimumItems = 100;
      VideoItemList videoItemList8 = videoItemList7;
      movieList4.Add((IItemList) videoItemList8);
      ObservableCollection<IItemList> movieList5 = this.movieList;
      VideoItemList videoItemList9 = new VideoItemList();
      videoItemList9.Uri = "movie/studio";
      videoItemList9.Type = MediaType.movie;
      videoItemList9.Title = Resource.ZuneVideoMarketPlace_Studios;
      videoItemList9.Category = MediaCategory.Studio;
      videoItemList9.Template = "ListTemplate";
      videoItemList9.MinimumItems = 100;
      VideoItemList videoItemList10 = videoItemList9;
      movieList5.Add((IItemList) videoItemList10);
      ObservableCollection<IItemList> televisionList1 = this.televisionList;
      VideoItemList videoItemList11 = new VideoItemList();
      videoItemList11.Uri = "hubs/tvShow";
      videoItemList11.Type = MediaType.tv_series;
      videoItemList11.Title = Resource.ZuneVideoMarketPlace_NewReleases;
      videoItemList11.Category = MediaCategory.NewReleases;
      videoItemList11.Template = "TwoPerRowTvTemplate";
      videoItemList11.Style = "ListBoxWithWrapping";
      videoItemList11.MinimumItems = 20;
      VideoItemList videoItemList12 = videoItemList11;
      televisionList1.Add((IItemList) videoItemList12);
      ObservableCollection<IItemList> televisionList2 = this.televisionList;
      VideoItemList videoItemList13 = new VideoItemList();
      videoItemList13.Uri = "tv/series?chunkSize=50&orderby=salesRank";
      videoItemList13.Type = MediaType.tv_series;
      videoItemList13.Title = Resource.ZuneVideoMarketPlace_TopPurchases;
      videoItemList13.Category = MediaCategory.TopPurchases;
      videoItemList13.Template = "TwoPerRowTvTemplate";
      videoItemList13.Style = "ListBoxWithWrapping";
      videoItemList13.MinimumItems = 20;
      VideoItemList videoItemList14 = videoItemList13;
      televisionList2.Add((IItemList) videoItemList14);
      ObservableCollection<IItemList> televisionList3 = this.televisionList;
      VideoItemList videoItemList15 = new VideoItemList();
      videoItemList15.Uri = "tv/category";
      videoItemList15.Type = MediaType.tv_series;
      videoItemList15.Title = Resource.ZuneVideoMarketPlace_Genres;
      videoItemList15.Category = MediaCategory.Genres;
      videoItemList15.Template = "ListTemplate";
      videoItemList15.MinimumItems = 100;
      VideoItemList videoItemList16 = videoItemList15;
      televisionList3.Add((IItemList) videoItemList16);
      ObservableCollection<IItemList> televisionList4 = this.televisionList;
      VideoItemList videoItemList17 = new VideoItemList();
      videoItemList17.Uri = "tv/network";
      videoItemList17.Type = MediaType.tv_series;
      videoItemList17.Title = Resource.ZuneVideoMarketPlace_Network;
      videoItemList17.Category = MediaCategory.Network;
      videoItemList17.Template = "ListTemplate";
      videoItemList17.MinimumItems = 100;
      VideoItemList videoItemList18 = videoItemList17;
      televisionList4.Add((IItemList) videoItemList18);
    }

    [XmlIgnore]
    public ObservableCollection<IItemList> MovieList
    {
      get => this.movieList;
      set
      {
        this.SetPropertyValue<ObservableCollection<IItemList>>(ref this.movieList, value, nameof (MovieList));
      }
    }

    [XmlIgnore]
    public ObservableCollection<IItemList> TVList
    {
      get => this.televisionList;
      set
      {
        this.SetPropertyValue<ObservableCollection<IItemList>>(ref this.televisionList, value, nameof (TVList));
      }
    }
  }
}
