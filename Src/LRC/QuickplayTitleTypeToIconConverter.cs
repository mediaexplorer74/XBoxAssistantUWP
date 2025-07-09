// *********************************************************
// Type: LRC.QuickplayTitleTypeToIconConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.ViewModel;
using System;
using System.Globalization;
using System.Windows.Data;
using Xbox.Live.Phone.Services.TitleHistory;


namespace LRC
{
  public class QuickplayTitleTypeToIconConverter : IValueConverter
  {
    public static string GetBoxArtImagePath(TitleType titleType)
    {
      string boxArtImagePath = "/UI/Images/DefaultBoxArt/unknown.png";
      switch (titleType)
      {
        case TitleType.Standard:
        case TitleType.Demo:
        case TitleType.Arcade:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/xboxgame.png";
          break;
        case TitleType.Application:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/xboxapp.png";
          break;
      }
      return boxArtImagePath;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string str = (string) null;
      if (value is RecentItemViewModel recentItemViewModel)
        str = QuickplayTitleTypeToIconConverter.GetBoxArtImagePath(recentItemViewModel.TitleType);
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
