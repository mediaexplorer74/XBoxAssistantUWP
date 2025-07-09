// *********************************************************
// Type: LRC.LocalizedResourceIndexer
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Resources;
using System.Globalization;
using System.Resources;
using System.Windows;


namespace LRC
{
  public class LocalizedResourceIndexer : DependencyObject
  {
    public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(nameof (Culture), typeof (CultureInfo), typeof (LocalizedResourceIndexer), new PropertyMetadata((object) CultureInfo.CurrentUICulture));
    private ResourceManager resourceManager = new ResourceManager("LRC.Resources.Resources.Resource", typeof (Resource).Assembly);

    public CultureInfo Culture
    {
      get => (CultureInfo) this.GetValue(LocalizedResourceIndexer.CultureProperty);
      set => this.SetValue(LocalizedResourceIndexer.CultureProperty, (object) value);
    }

    public string this[string key] => this.resourceManager.GetString(key, this.Culture);
  }
}
