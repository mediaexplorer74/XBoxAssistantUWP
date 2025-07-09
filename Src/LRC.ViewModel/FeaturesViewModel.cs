// *********************************************************
// Type: LRC.ViewModel.FeaturesViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone;
using Xbox.Live.Phone.Services;
using Xbox.Live.Phone.Services.Programming;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class FeaturesViewModel : ViewModelBase
  {
    private const string ComponentName = "FeaturesViewModel";
    private const string PerfGetProgrammingContentAsync = "FeaturesViewModel:GetProgrammingContentAsync";
    private ObservableCollection<PromoItem> featuresOneByOne;
    private ObservableCollection<PromoItem> featuresFourByThree;

    public FeaturesViewModel() => this.LifetimeInMinutes = 1440;

    public ObservableCollection<PromoItem> FeaturesOneByOne
    {
      get => this.featuresOneByOne;
      set
      {
        this.SetPropertyValue<ObservableCollection<PromoItem>>(ref this.featuresOneByOne, value, nameof (FeaturesOneByOne));
      }
    }

    public ObservableCollection<PromoItem> FeaturesFourByThree
    {
      get => this.featuresFourByThree;
      set
      {
        this.SetPropertyValue<ObservableCollection<PromoItem>>(ref this.featuresFourByThree, value, nameof (FeaturesFourByThree));
      }
    }

    public override void Load()
    {
      if (!this.ShouldLoadData)
        return;
      this.CurrentState = 3;
      if (this.featuresOneByOne == null)
        this.featuresOneByOne = new ObservableCollection<PromoItem>();
      if (this.featuresFourByThree == null)
        this.featuresFourByThree = new ObservableCollection<PromoItem>();
      IProgrammingServiceManager programmingServiceManager = ServiceManagerFactory.CreateProgrammingServiceManager();
      programmingServiceManager.Initialize(EnvironmentState.Instance.Environment);
      programmingServiceManager.EventGetProgrammingXmlCompleted += new EventHandler<ServiceProxyEventArgs<List<PromoItem>>>(this.GetFeedListCompleted);
      programmingServiceManager.GetProgrammingContentAsync();
      this.IsBusy = true;
    }

    private void GetFeedListCompleted(object sender, ServiceProxyEventArgs<List<PromoItem>> e)
    {
      this.CurrentState = 0;
      if (sender is IProgrammingServiceManager programmingServiceManager)
        programmingServiceManager.EventGetProgrammingXmlCompleted -= new EventHandler<ServiceProxyEventArgs<List<PromoItem>>>(this.GetFeedListCompleted);
      ObservableCollection<PromoItem> observableCollection1 = new ObservableCollection<PromoItem>();
      ObservableCollection<PromoItem> observableCollection2 = new ObservableCollection<PromoItem>();
      PromoItem promoItem1 = new PromoItem()
      {
        ItemType = PromoItemType.Uri,
        Title = Resource.NowPlaying_ZuneMarketplaceTitle,
        ImageUrl = "/UI/Images/Tile_Zune.jpg",
        DeepLink = "/UI/Views/ZuneMarketplace/ZuneHomePage.xaml"
      };
      observableCollection1.Add(promoItem1);
      this.IsBusy = false;
      if (e.Error == null && e.Result != null)
      {
        foreach (PromoItem promoItem2 in e.Result)
        {
          if (promoItem2.ChannelType == PromoChannelType.Video)
            observableCollection1.Add(promoItem2);
          else if (promoItem2.ChannelType == PromoChannelType.Game || promoItem2.ChannelType == PromoChannelType.Music)
            observableCollection2.Add(promoItem2);
        }
        this.LastRefreshTime = DateTime.UtcNow;
      }
      if (observableCollection1.Count > 1 && (observableCollection1.Count & 1) != 0)
        observableCollection1.RemoveAt(observableCollection1.Count - 1);
      this.FeaturesOneByOne = observableCollection1;
      this.FeaturesFourByThree = observableCollection2;
    }
  }
}
