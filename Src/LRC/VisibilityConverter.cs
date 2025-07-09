// *********************************************************
// Type: LRC.VisibilityConverter
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Globalization;
using System.Windows;
using Windows.UI.Xaml;

//using System.Windows.Data;
using Windows.UI.Xaml.Data;


namespace LRC
{
  public partial class VisibilityConverter : IValueConverter
  {
  
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? (object)(Visibility)0 : (object)(Visibility)1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
