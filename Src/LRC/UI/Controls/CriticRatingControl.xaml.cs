// *********************************************************
// Type: LRC.CriticRatingControl
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;


namespace LRC
{
  public partial class CriticRatingControl : UserControl
  {
    private const string ComponentName = "CriticRatingControl";
    private static readonly DependencyProperty CriticRatingProperty = DependencyProperty.Register(nameof (CriticRating), typeof (double?), typeof (CriticRatingControl), new PropertyMetadata((object) null, new PropertyChangedCallback(CriticRatingControl.DependencyPropertyChangedCallback)));
    private static readonly DependencyProperty ShowCriticRatingProperty = DependencyProperty.Register(nameof (ShowCriticRating), typeof (bool), typeof (CriticRatingControl), new PropertyMetadata((object) false, (PropertyChangedCallback) null));
Grid LayoutRoot;
TextBlock RatingDisplay;
Image RatingImage;
    

    public CriticRatingControl() => this.InitializeComponent();

    public double? CriticRating
    {
      get => (double?) ((DependencyObject) this).GetValue(CriticRatingControl.CriticRatingProperty);
      set
      {
        ((DependencyObject) this).SetValue(CriticRatingControl.CriticRatingProperty, (object) value);
      }
    }

    public bool ShowCriticRating
    {
      get
      {
        return (bool) ((DependencyObject) this).GetValue(CriticRatingControl.ShowCriticRatingProperty);
      }
      set
      {
        ((DependencyObject) this).SetValue(CriticRatingControl.ShowCriticRatingProperty, (object) value);
      }
    }

    private static void DependencyPropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is CriticRatingControl criticRatingControl) || e.Property != CriticRatingControl.CriticRatingProperty)
        return;
      criticRatingControl.RatingDisplay.Text = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.#}", new object[1]
      {
        (object) criticRatingControl.CriticRating.Value
      });
      criticRatingControl.ShowCriticRating = criticRatingControl.CriticRating.HasValue;
    }

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/CriticRatingControl.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.RatingDisplay = (TextBlock) ((FrameworkElement) this).FindName("RatingDisplay");
      this.RatingImage = (Image) ((FrameworkElement) this).FindName("RatingImage");
    }
  }
}
