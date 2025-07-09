// *********************************************************
// Type: LRC.FriendBeaconTextConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Resources;
using System;
using System.Globalization;
using System.Windows.Data;


namespace LRC
{
  public class FriendBeaconTextConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string str = value as string;
      if (string.IsNullOrWhiteSpace(str))
        return (object) string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Friend_DefaultBeaconText);
      return (object) string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Friend_BeaconText, new object[1]
      {
        (object) str
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
