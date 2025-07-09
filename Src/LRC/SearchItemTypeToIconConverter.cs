// *********************************************************
// Type: LRC.SearchItemTypeToIconConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Globalization;
using System.Windows.Data;


namespace LRC
{
  public class SearchItemTypeToIconConverter : IValueConverter
  {
    public static string GetIconPath(string edsItemType)
    {
      string iconPath = (string) null;
      if (!string.IsNullOrEmpty(edsItemType))
      {
        switch (edsItemType.ToUpperInvariant())
        {
          case "MOVIE":
            iconPath = "/UI/Images/Search/movie.png";
            break;
          case "MUSICALBUM":
          case "MUSICARTIST":
            iconPath = "/UI/Images/Search/musictrack.png";
            break;
          case "MUSICVIDEO":
            iconPath = "/UI/Images/Search/musicvideo.png";
            break;
          case "TVSERIES":
          case "TVSEASON":
          case "TVEPISODE":
          case "TVSHOW":
            iconPath = "/UI/Images/Search/tv.png";
            break;
          case "XBOXAPP":
            iconPath = "/UI/Images/Search/xboxapp.png";
            break;
          case "XBOX360GAMECONTENT":
            iconPath = "/UI/Images/Search/xboxgamecontent.png";
            break;
          case "XBOXTHEME":
            iconPath = "/UI/Images/Search/xboxtheme.png";
            break;
          case "XBOXARCADEGAME":
            iconPath = "/UI/Images/Search/xboxarcadegame.png";
            break;
          case "XBOX360GAME":
            iconPath = "/UI/Images/Search/xboxgame.png";
            break;
          case "XBOXORIGINALGAME":
            iconPath = "/UI/Images/Search/xboxoriginalgame.png";
            break;
          case "XBOXXNACOMMUNITYGAME":
            iconPath = "/UI/Images/Search/xboxcommunitygame.png";
            break;
          case "XBOX360GAMEDEMO":
            iconPath = "/UI/Images/Search/xboxgamedemo.png";
            break;
          case "XBOXGAMETRIAL":
            iconPath = "/UI/Images/Search/xboxgametrial.png";
            break;
          case "XBOXGAMETRAILER":
            iconPath = "/UI/Images/Search/xboxgametrailer.png";
            break;
          case "XBOXGAMECONSUMABLE":
            iconPath = "/UI/Images/Search/xboxgamedemo.png";
            break;
          case "XBOXGAMERTILE":
            iconPath = "/UI/Images/Search/xboxgamertile.png";
            break;
          case "XBOXGAMEVIDEO":
            iconPath = "/UI/Images/Search/xboxgametrailer.png";
            break;
        }
      }
      return iconPath;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string str = (string) null;
      if (value != null)
        str = SearchItemTypeToIconConverter.GetIconPath(value.ToString());
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
