// *********************************************************
// Type: LRC.TextLengthToVisibilityConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Globalization;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
//using System.Windows.Data;


namespace LRC
{
  public partial class TextLengthToVisibilityConverter : IValueConverter
  {
  
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (object)(Visibility)(value == null || string.IsNullOrEmpty(value.ToString()) ? 1 : 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
