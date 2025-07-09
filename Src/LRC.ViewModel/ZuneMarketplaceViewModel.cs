// *********************************************************
// Type: LRC.ViewModel.ZuneMarketplaceViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [KnownType(typeof (MediaItemList))]
  public class ZuneMarketplaceViewModel : ViewModelBase
  {
    private ZuneCategory category;

    public ZuneMarketplaceViewModel()
    {
      this.ZuneVideoFeature = new ZuneVideoFeatureViewModel();
      this.RecommendedPicks = new RecommendedPicksViewModel();
      this.category = new ZuneCategory();
    }

    [XmlIgnore]
    public ObservableCollection<IItemList> MovieList => this.category.MovieList;

    [XmlIgnore]
    public ObservableCollection<IItemList> TVList => this.category.TVList;

    public ZuneVideoFeatureViewModel ZuneVideoFeature { get; set; }

    public RecommendedPicksViewModel RecommendedPicks { get; set; }

    public override void Load()
    {
      this.ZuneVideoFeature.Load();
      this.RecommendedPicks.Load();
    }
  }
}
