// *********************************************************
// Type: LRC.ViewModel.RecentItemViewModel
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Services.TitleHistory;


namespace LRC.ViewModel
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class RecentItemViewModel : ViewModelBase
  {
    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public uint TitleId { get; set; }

    [DataMember]
    public TitleType TitleType { get; set; }

    public bool IsGame
    {
      get
      {
        return this.TitleType == TitleType.Arcade || this.TitleType == TitleType.Demo || this.TitleType == TitleType.Standard;
      }
    }
  }
}
