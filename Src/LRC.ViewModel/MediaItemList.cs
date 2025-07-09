// *********************************************************
// Type: LRC.ViewModel.MediaItemList
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using LRC.Service;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows;
using System.Xml.Serialization;


namespace LRC.ViewModel
{
  [XmlInclude(typeof (VideoItemList))]
  [KnownType(typeof (VideoItemList))]
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class MediaItemList : ViewModelBase, IItemList
  {
    private string itemNotFound;
    private bool hasContent;
    private ObservableCollection<ViewModelBase> items;
    private VerticalAlignment busyIndicatorVerticalAlignment;

    public MediaItemList()
    {
      this.NoItemFound = string.Empty;
      this.LifetimeInMinutes = 1440;
      this.BusyIndicatorVerticalAlignment = (VerticalAlignment) 0;
    }

    [DataMember]
    public string Uri { get; set; }

    [DataMember]
    public MediaType Type { get; set; }

    [DataMember]
    public MediaCategory Category { get; set; }

    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string AfterMarker { get; set; }

    [DataMember]
    public ObservableCollection<ViewModelBase> Items
    {
      get => this.items;
      set
      {
        this.SetPropertyValue<ObservableCollection<ViewModelBase>>(ref this.items, value, nameof (Items));
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

    [DataMember]
    public string NoItemFound
    {
      get => this.itemNotFound;
      set => this.SetPropertyValue<string>(ref this.itemNotFound, value, nameof (NoItemFound));
    }

    [DataMember]
    public bool HasContent
    {
      get => this.hasContent;
      set => this.SetPropertyValue<bool>(ref this.hasContent, value, nameof (HasContent));
    }
  }
}
