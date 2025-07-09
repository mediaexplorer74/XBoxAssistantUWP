// *********************************************************
// Type: LRC.ViewModelLocator
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.ViewModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xbox.Live.Phone.Utils.Serialization;


namespace LRC
{
  [DataContract(Namespace = "")]
  [XmlRoot(Namespace = "")]
  public partial class ViewModelLocator
  {
    public ViewModelLocator()
    {
      this.Cabinet = new XmlSerializableDictionary<string, ViewModelBase>();
    }

    [DataMember]
    public XmlSerializableDictionary<string, ViewModelBase> Cabinet { get; set; }

    public void AddOrSet(string key, ViewModelBase viewModel) => this.Cabinet[key] = viewModel;

    public ViewModelBase Locate(string key)
    {
      return this.Cabinet.ContainsKey(key) ? this.Cabinet[key] : (ViewModelBase) null;
    }

    public void Remove(string key)
    {
      if (!this.Cabinet.ContainsKey(key))
        return;
      this.Cabinet.Remove(key);
    }

    public bool ContainsKey(string key) => this.Cabinet.ContainsKey(key);
  }
}
