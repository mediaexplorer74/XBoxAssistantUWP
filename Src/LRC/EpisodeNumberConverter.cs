// *********************************************************
// Type: LRC.EpisodeNumberConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Resources;
using System;
using System.Globalization;
using System.Windows.Data;


namespace LRC
{
  public class EpisodeNumberConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null)
      {
        int? nullable1 = value as int?;
        if (nullable1.HasValue)
        {
          int? nullable2 = nullable1;
          if ((nullable2.GetValueOrDefault() <= 0 ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
            return (object) string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.TVEpisodeTitle, new object[1]
            {
              value
            });
        }
      }
      return (object) null;
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
