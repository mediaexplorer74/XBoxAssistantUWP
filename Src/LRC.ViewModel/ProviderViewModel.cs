// *********************************************************
// Type: LRC.ViewModel.ProviderViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Resources;
using LRC.Service.Search;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public class ProviderViewModel : ViewModelBase
  {
    private const string ComponentName = "ProviderOffers";
    private const string OfferTypeFree = "FREE";
    private const string OfferTypeFreeWithAds = "FREEWITHADS";
    private const string OfferTypeFreeWithSubscription = "FREEWITHSUBSCRIPTION";
    private const string OfferTypeRent = "RENT";
    private const string OfferTypePayPerView = "PAYPERVIEW";
    private const string OfferTypePurchaseToOwn = "PURCHASETOOWN";

    public ProviderViewModel()
      : this((Provider) null)
    {
    }

    public ProviderViewModel(Provider provider) => this.Initialize(provider);

    public string ProviderName { get; set; }

    public string ProductId { get; set; }

    public string OfferDescription { get; set; }

    public uint TitleId { get; set; }

    public string DeepLinkInfo { get; set; }

    private void Initialize(Provider provider)
    {
      if (provider == null)
        return;
      this.ProviderName = provider.Name;
      this.ImageUrl = provider.ImageUrl;
      this.ProductId = provider.ProductId;
      if (provider.PartnerApplicationLaunchInfos != null)
      {
        foreach (PartnerApplicationLaunchInfo applicationLaunchInfo in provider.PartnerApplicationLaunchInfos)
        {
          if (string.Equals(applicationLaunchInfo.ClientType, "Xbox360", StringComparison.OrdinalIgnoreCase))
          {
            this.TitleId = applicationLaunchInfo.TitleId;
            this.DeepLinkInfo = applicationLaunchInfo.DeepLinkInfo;
            break;
          }
        }
      }
      List<string> stringList = new List<string>();
      if (provider.ProviderContents != null)
      {
        foreach (ProviderContent providerContent in provider.ProviderContents)
        {
          if (providerContent != null && string.Equals(providerContent.Device, "Xbox360", StringComparison.OrdinalIgnoreCase) && providerContent.OfferInstances != null)
          {
            foreach (OfferInstance offerInstance in providerContent.OfferInstances)
            {
              string str = !string.IsNullOrEmpty(offerInstance.OfferType) ? offerInstance.OfferType.ToUpperInvariant() : string.Empty;
              if (string.Compare(offerInstance.OfferType, "PURCHASETOOWN", StringComparison.OrdinalIgnoreCase) == 0 && offerInstance.Price.HasValue && offerInstance.Price.Value == 0)
                str = "FREE";
              if (!stringList.Contains(str))
                stringList.Add(str);
            }
          }
        }
      }
      if (stringList.Count <= 0)
        return;
      if (stringList.Contains("FREE"))
        this.OfferDescription = Resource.ProviderOffer_Free;
      else if (stringList.Contains("FREEWITHADS"))
        this.OfferDescription = Resource.ProviderOffer_FreeWithAds;
      else if (stringList.Contains("FREEWITHSUBSCRIPTION"))
        this.OfferDescription = Resource.ProviderOffer_FreeWithSubscription;
      else if (stringList.Contains("RENT"))
        this.OfferDescription = Resource.ProviderOffer_Rent;
      else if (stringList.Contains("PAYPERVIEW"))
        this.OfferDescription = Resource.ProviderOffer_PayPerView;
      else if (stringList.Contains("PURCHASETOOWN"))
        this.OfferDescription = Resource.ProviderOffer_Purchase;
      else
        this.OfferDescription = (string) null;
    }
  }
}
