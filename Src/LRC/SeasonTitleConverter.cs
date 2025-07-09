// *********************************************************
// Type: LRC.SeasonTitleConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Resources;
using System;
using System.Globalization;
using System.Windows.Data;
using Windows.UI.Xaml.Data;


namespace LRC
{
  public class SeasonTitleConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return (object) null;
      return (object) string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.TVSeasonTitle, new object[1]
      {
        value
      });
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
