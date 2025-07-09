// *********************************************************
// Type: LRC.ViewModel.MediaCategory
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public enum MediaCategory
  {
    None,
    NewReleases,
    TopPurchases,
    TopRentals,
    Genres,
    Studio,
    Network,
  }
}
