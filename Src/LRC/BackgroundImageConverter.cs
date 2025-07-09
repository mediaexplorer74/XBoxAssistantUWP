// *********************************************************
// Type: LRC.BackgroundImageConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace LRC
{
  public class BackgroundImageConverter : IValueConverter
  {
    public static ImageBrush GetImageBrush(Uri uriSource)
    {
      ImageBrush imageBrush = (ImageBrush) null;
      if (uriSource != (Uri) null)
      {
        imageBrush = new ImageBrush();
        imageBrush.ImageSource = (ImageSource) new BitmapImage(uriSource);
        ((Brush) imageBrush).Opacity = 0.25;
        ((TileBrush) imageBrush).Stretch = (Stretch) 3;
      }
      return imageBrush;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null)
      {
        string uriString = value.ToString();
        if (!string.IsNullOrEmpty(uriString))
          return (object) BackgroundImageConverter.GetImageBrush(new Uri(uriString, UriKind.Absolute));
      }
      return (object) null;
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
