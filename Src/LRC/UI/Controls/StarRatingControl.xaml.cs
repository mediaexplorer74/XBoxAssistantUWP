// *********************************************************
// Type: LRC.StarRatingControl
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Resources;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace LRC
{
  public partial class StarRatingControl : UserControl
  {
    private const string ComponentName = "StarRatingControl";
    private static readonly DependencyProperty UserCountProperty = DependencyProperty.Register(nameof (UserCount), typeof (double?), typeof (StarRatingControl), new PropertyMetadata((object) null, new PropertyChangedCallback(StarRatingControl.DependencyPropertyChangedCallback)));
    private static readonly DependencyProperty UserRatingProperty = DependencyProperty.Register(nameof (UserRating), typeof (double?), typeof (StarRatingControl), new PropertyMetadata((object) null, new PropertyChangedCallback(StarRatingControl.DependencyPropertyChangedCallback)));
    private static readonly DependencyProperty ShowStarRatingProperty = DependencyProperty.Register(nameof (ShowStarRating), typeof (bool), typeof (StarRatingControl), new PropertyMetadata((object) false, (PropertyChangedCallback) null));
Grid LayoutRoot;
Image Star1;
Image Star2;
Image Star3;
Image Star4;
Image Star5;
TextBlock VoterCountDisplay;
    

    public StarRatingControl() => this.InitializeComponent();

    public bool ShowStarRating
    {
      get => (bool) ((DependencyObject) this).GetValue(StarRatingControl.ShowStarRatingProperty);
      set
      {
        ((DependencyObject) this).SetValue(StarRatingControl.ShowStarRatingProperty, (object) value);
      }
    }

    public double? UserCount
    {
      get => (double?) ((DependencyObject) this).GetValue(StarRatingControl.UserCountProperty);
      set
      {
        ((DependencyObject) this).SetValue(StarRatingControl.UserCountProperty, (object) value);
      }
    }

    public double? UserRating
    {
      get => (double?) ((DependencyObject) this).GetValue(StarRatingControl.UserRatingProperty);
      set
      {
        ((DependencyObject) this).SetValue(StarRatingControl.UserRatingProperty, (object) value);
      }
    }

    private static void DependencyPropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is StarRatingControl starRatingControl))
        return;
      if (e.Property == StarRatingControl.UserRatingProperty)
      {
        if (starRatingControl.UserRating.HasValue)
        {
          double score = Math.Max(0.0, Math.Min(5.0, starRatingControl.UserRating.Value));
          StarRatingControl.SetStarSource(starRatingControl.Star1, score);
          StarRatingControl.SetStarSource(starRatingControl.Star2, score - 1.0);
          StarRatingControl.SetStarSource(starRatingControl.Star3, score - 2.0);
          StarRatingControl.SetStarSource(starRatingControl.Star4, score - 3.0);
          StarRatingControl.SetStarSource(starRatingControl.Star5, score - 4.0);
          starRatingControl.ShowStarRating = true;
        }
        else
          starRatingControl.ShowStarRating = false;
      }
      else
      {
        if (e.Property != StarRatingControl.UserCountProperty || !starRatingControl.UserCount.HasValue)
          return;
        starRatingControl.VoterCountDisplay.Text = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resource.Search_UserVoterCount, new object[1]
        {
          (object) starRatingControl.UserCount.Value
        });
      }
    }

    private static void SetStarSource(Image starImage, double score)
    {
      if (score < 0.24)
      {
        BitmapImage bitmapImage = new BitmapImage(new Uri("/UI/Images/StarRatingControl/star_empty.png", UriKind.RelativeOrAbsolute));
        starImage.Source = (ImageSource) bitmapImage;
      }
      else if (score < 0.49)
      {
        BitmapImage bitmapImage = new BitmapImage(new Uri("/UI/Images/StarRatingControl/star_qtr.png", UriKind.RelativeOrAbsolute));
        starImage.Source = (ImageSource) bitmapImage;
      }
      else if (score < 0.74)
      {
        BitmapImage bitmapImage = new BitmapImage(new Uri("/UI/Images/StarRatingControl/star_half.png", UriKind.RelativeOrAbsolute));
        starImage.Source = (ImageSource) bitmapImage;
      }
      else if (score < 0.99)
      {
        BitmapImage bitmapImage = new BitmapImage(new Uri("/UI/Images/StarRatingControl/star_3qtr.png", UriKind.RelativeOrAbsolute));
        starImage.Source = (ImageSource) bitmapImage;
      }
      else
      {
        BitmapImage bitmapImage = new BitmapImage(new Uri("/UI/Images/StarRatingControl/star_full.png", UriKind.RelativeOrAbsolute));
        starImage.Source = (ImageSource) bitmapImage;
      }
    }

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/StarRatingControl.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.Star1 = (Image) ((FrameworkElement) this).FindName("Star1");
      this.Star2 = (Image) ((FrameworkElement) this).FindName("Star2");
      this.Star3 = (Image) ((FrameworkElement) this).FindName("Star3");
      this.Star4 = (Image) ((FrameworkElement) this).FindName("Star4");
      this.Star5 = (Image) ((FrameworkElement) this).FindName("Star5");
      this.VoterCountDisplay = (TextBlock) ((FrameworkElement) this).FindName("VoterCountDisplay");
    }
  }
}
