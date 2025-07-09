// *********************************************************
// Type: LRC.FriendPlayingTextConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Resources;
using LRC.ViewModel;
using System;
using System.Globalization;
using System.Windows.Data;
using Xbox.Live.Phone.Services.Beacons;


namespace LRC
{
  public class FriendPlayingTextConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string str = (string) null;
      if (value is FriendViewModel friendViewModel)
      {
        if (friendViewModel.IsPlayingNow)
          str = Resource.Friend_PlayingNowText;
        else if (friendViewModel.TimeSinceLastActivity != null)
        {
          if (friendViewModel.TimeSinceLastActivity.Value == 1)
          {
            switch (friendViewModel.TimeSinceLastActivity.Units)
            {
              case SimpleDurationUnit.Minutes:
                str = Resource.Friend_PlayedOneMinuteAgoText;
                break;
              case SimpleDurationUnit.Hours:
                str = Resource.Friend_PlayedOneHourAgoText;
                break;
              case SimpleDurationUnit.Days:
                str = Resource.Friend_PlayedOneDayAgoText;
                break;
              case SimpleDurationUnit.Months:
                str = Resource.Friend_PlayedOneMonthAgoText;
                break;
            }
          }
          else if (friendViewModel.TimeSinceLastActivity.Value > 1)
          {
            switch (friendViewModel.TimeSinceLastActivity.Units)
            {
              case SimpleDurationUnit.Minutes:
                str = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Friend_PlayedMinutesAgoText, new object[1]
                {
                  (object) friendViewModel.TimeSinceLastActivity.Value
                });
                break;
              case SimpleDurationUnit.Hours:
                str = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Friend_PlayedHoursAgoText, new object[1]
                {
                  (object) friendViewModel.TimeSinceLastActivity.Value
                });
                break;
              case SimpleDurationUnit.Days:
                str = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Friend_PlayedDaysAgoText, new object[1]
                {
                  (object) friendViewModel.TimeSinceLastActivity.Value
                });
                break;
              case SimpleDurationUnit.Months:
                str = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Friend_PlayedMonthsAgoText, new object[1]
                {
                  (object) friendViewModel.TimeSinceLastActivity.Value
                });
                break;
            }
          }
        }
      }
      return (object) str;
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
