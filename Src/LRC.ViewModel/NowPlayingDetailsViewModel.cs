// *********************************************************
// Type: LRC.ViewModel.NowPlayingDetailsViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class NowPlayingDetailsViewModel : ViewModelBase
  {
    private const string ComponentName = "NowPlayingDetailsViewModel";
    private SearchItemViewModel item;

    public NowPlayingDetailsViewModel()
      : this((SearchItemViewModel) null)
    {
    }

    public NowPlayingDetailsViewModel(SearchItemViewModel searchItem)
    {
      this.SearchItem = searchItem;
      if (this.SearchItem == null)
        return;
      this.Title = this.SearchItem.Title;
    }

    [DataMember]
    public SearchItemViewModel SearchItem
    {
      get => this.item;
      set => this.SetPropertyValue<SearchItemViewModel>(ref this.item, value, nameof (SearchItem));
    }

    public override void Load()
    {
      if (this.SearchItem == null)
        return;
      this.SearchItem.Load();
    }
  }
}
