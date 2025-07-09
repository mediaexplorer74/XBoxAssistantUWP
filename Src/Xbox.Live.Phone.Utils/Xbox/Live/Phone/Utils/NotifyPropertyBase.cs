// *********************************************************
// Type: Xbox.Live.Phone.Utils.NotifyPropertyBase
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;


namespace Xbox.Live.Phone.Utils
{
  [XmlRoot(Namespace = "")]
  [DataContract(Namespace = "")]
  public abstract class NotifyPropertyBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public void NotifyPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      if (handler == null)
        return;
      ThreadManager.UIThreadPost((SendOrPostCallback) delegate
      {
        handler((object) this, new PropertyChangedEventArgs(propertyName));
      }, (object) null);
    }

    [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "We're using pass by reference correctly here.")]
    protected void SetPropertyValue<T>(ref T variable, T newValue, string name)
    {
      if (object.Equals((object) variable, (object) newValue))
        return;
      variable = newValue;
      this.NotifyPropertyChanged(name);
    }
  }
}
