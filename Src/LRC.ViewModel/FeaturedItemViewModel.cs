// *********************************************************
// Type: LRC.ViewModel.FeaturedItemViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System.Runtime.Serialization;


namespace LRC.ViewModel
{
  public class FeaturedItemViewModel : ViewModelBase
  {
    public FeaturedItemViewModel()
    {
    }

    public FeaturedItemViewModel(
      string title,
      string deepLink,
      string mediaId,
      uint providerTitleId)
      : this()
    {
      this.Title = title;
      this.DeepLink = deepLink;
      this.MediaId = mediaId;
      this.ProviderTitleId = providerTitleId;
    }

    [DataMember]
    public uint ProviderTitleId { get; set; }

    [DataMember]
    public string DeepLink { get; set; }

    [DataMember]
    public string MediaId { get; set; }
  }
}
