// *********************************************************
// Type: LRC.HeightScalingConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace LRC
{
  public class HeightScalingConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      double result = 0.0;
      if (parameter == null || !double.TryParse(parameter.ToString(), out result))
        return (object) double.NaN;
      if (value == null || !(value is Size size))
        return (object) result;
      return size.Width == 0.0 ? (object) double.NaN : (object) (result * size.Height / size.Width);
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
