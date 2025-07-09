// *********************************************************
// Type: LRC.NumberToVisibilityConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace LRC
{
  public class NumberToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      int result = 0;
      if (parameter != null && !int.TryParse(parameter.ToString(), out result))
        result = 0;
      return value is int num && num > result ? (object) (Visibility) 0 : (object) (Visibility) 1;
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
