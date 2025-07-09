// *********************************************************
// Type: LRC.DateConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Globalization;
using System.Windows.Data;


namespace LRC
{
  public class DateConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null)
      {
        DateTime dateTime = (DateTime) value;
        if (dateTime.Ticks > 0L)
          return parameter == null ? (object) dateTime.ToShortDateString() : (object) dateTime.ToString(parameter.ToString(), (IFormatProvider) CultureInfo.CurrentCulture);
      }
      return (object) string.Empty;
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
