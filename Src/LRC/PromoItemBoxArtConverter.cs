// *********************************************************
// Type: LRC.PromoItemBoxArtConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Globalization;
using System.Windows.Data;
using Xbox.Live.Phone.Services.Programming;


namespace LRC
{
  public class PromoItemBoxArtConverter : IValueConverter
  {
    private const string ComponentName = "PromoItemBoxArtConverter";

    public static string GetBoxArtImagePath(PromoItemType promoItemType)
    {
      string boxArtImagePath;
      switch (promoItemType)
      {
        case PromoItemType.Undefined:
        case PromoItemType.Uri:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/unknown.png";
          break;
        case PromoItemType.Game:
        case PromoItemType.GameContent:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/xboxgame.png";
          break;
        case PromoItemType.Album:
        case PromoItemType.Artist:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/music.png";
          break;
        case PromoItemType.Movie:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/movie.png";
          break;
        case PromoItemType.TvSeries:
        case PromoItemType.TvSeason:
        case PromoItemType.TvEpisode:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/tv.png";
          break;
        case PromoItemType.App:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/xboxapp.png";
          break;
        default:
          boxArtImagePath = "/UI/Images/DefaultBoxArt/unknown.png";
          break;
      }
      return boxArtImagePath;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string str = (string) null;
      if (value is PromoItem promoItem)
        str = PromoItemBoxArtConverter.GetBoxArtImagePath(promoItem.ItemType);
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
