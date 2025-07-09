// *********************************************************
// Type: LRC.SearchItemHeightConverter
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
  public class SearchItemHeightConverter : IValueConverter
  {
    private const string ComponentName = "SearchItemHeightConverter";

    public static int GetItemHeight(string edsItemType)
    {
      int itemHeight = 99;
      if (!string.IsNullOrEmpty(edsItemType))
      {
        switch (edsItemType.ToUpperInvariant())
        {
          case "MOVIE":
          case "XBOX360GAME":
          case "XBOXAPP":
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
            itemHeight = 132;
            break;
          default:
            itemHeight = 99;
            break;
        }
      }
      return itemHeight;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      int num = 99;
      switch (value)
      {
        case SearchItemViewModel searchItemViewModel:
          num = SearchItemHeightConverter.GetItemHeight(searchItemViewModel.ItemType);
          break;
        case SearchData searchData:
          num = SearchItemHeightConverter.GetItemHeight(searchData.ItemType);
          break;
      }
      return (object) num;
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
