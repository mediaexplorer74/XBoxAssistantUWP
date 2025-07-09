// *********************************************************
// Type: Xbox.Live.Phone.Services.Programming.PromoItem
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone.Services.Programming
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public class PromoItem : NotifyPropertyBase
  {
    private string title;
    private string imageUrl;
    private string description;

    public PromoItem() => this.ItemType = PromoItemType.Undefined;

    [DataMember]
    public string Title
    {
      get => this.title;
      set => this.SetPropertyValue<string>(ref this.title, value, nameof (Title));
    }

    [DataMember]
    public string Description
    {
      get => this.description;
      set => this.SetPropertyValue<string>(ref this.description, value, nameof (Description));
    }

    [DataMember]
    public string ImageUrl
    {
      get => this.imageUrl;
      set => this.SetPropertyValue<string>(ref this.imageUrl, value, nameof (ImageUrl));
    }

    [DataMember]
    public string DeepLink { get; set; }

    [DataMember]
    public uint Provider { get; set; }

    [DataMember]
    public PromoItemType ItemType { get; set; }

    [DataMember]
    public PromoChannelType ChannelType { get; set; }

    [DataMember]
    public string MediaId { get; set; }

    [DataMember]
    public string ContentMediaId { get; set; }

    [DataMember]
    public string ContentMediaTypeId { get; set; }

    [DataMember]
    public string TVSeriesId { get; set; }

    [DataMember]
    public string TVSeasonNumber { get; set; }
  }
}
