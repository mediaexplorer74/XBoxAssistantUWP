// *********************************************************
// Type: LRC.SearchItemBoxArtConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Search;
using LRC.ViewModel;
using System;
using System.Globalization;
using System.Windows.Data;


namespace LRC
{
  public class SearchItemBoxArtConverter : IValueConverter
  {
    private const string ComponentName = "SearchItemBoxArtConverter";

    public static string GetBoxArtImagePath(string edsItemType)
    {
      string boxArtImagePath = (string) null;
      if (!string.IsNullOrEmpty(edsItemType))
      {
        switch (edsItemType.ToUpperInvariant())
        {
          case "MOVIE":
            boxArtImagePath = "/UI/Images/DefaultBoxArt/movie.png";
            break;
          case "MUSICALBUM":
            boxArtImagePath = "/UI/Images/DefaultBoxArt/musicTrack.png";
            break;
          case "MUSICARTIST":
            boxArtImagePath = "/UI/Images/DefaultBoxArt/music.png";
            break;
          case "MUSICVIDEO":
            boxArtImagePath = "/UI/Images/DefaultBoxArt/musicVideo.png";
            break;
          case "TVSERIES":
          case "TVSEASON":
          case "TVEPISODE":
          case "TVSHOW":
            boxArtImagePath = "/UI/Images/DefaultBoxArt/tv.png";
            break;
          case "XBOXAPP":
            boxArtImagePath = "/UI/Images/DefaultBoxArt/xboxapp.png";
            break;
          case "XBOX360GAME":
          case "XBOXARCADEGAME":
          case "XBOXORIGINALGAME":
          case "XBOXXNACOMMUNITYGAME":
          case "XBOXGAMECONSUMABLE":
          case "XBOX360GAMECONTENT":
          case "XBOX360GAMEDEMO":
          case "XBOXGAMEITEM":
          case "XBOXGAMETRIAL":
          case "XBOXGAMETRAILER":
          case "XBOXGAMERTILE":
          case "XBOXGAMEVIDEO":
          case "XBOXTHEME":
            boxArtImagePath = "/UI/Images/DefaultBoxArt/xboxgame.png";
            break;
          default:
            boxArtImagePath = "/UI/Images/DefaultBoxArt/unknown.png";
            break;
        }
      }
      return boxArtImagePath;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string str = (string) null;
      switch (value)
      {
        case SearchItemViewModel searchItemViewModel:
          str = SearchItemBoxArtConverter.GetBoxArtImagePath(searchItemViewModel.ItemType);
          break;
        case SearchData searchData:
          str = SearchItemBoxArtConverter.GetBoxArtImagePath(searchData.ItemType);
          break;
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
