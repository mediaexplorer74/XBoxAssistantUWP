// *********************************************************
// Type: LRC.MediaControlButton
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace LRC
{
  public class MediaControlButton : Button
  {
    private static readonly DependencyProperty DefaultImageSourceProperty = DependencyProperty.Register(nameof (DefaultImageSource), typeof (string), typeof (MediaControlButton), (PropertyMetadata) null);
    private static readonly DependencyProperty AlternateImageSourceProperty = DependencyProperty.Register(nameof (AlternateImageSource), typeof (string), typeof (MediaControlButton), (PropertyMetadata) null);
    private Image defaultImage;
    private Image alternateImage;
    private string defaultImageSource;
    private string alternateImageSource;

    public MediaControlButton()
    {
      ((Control) this).DefaultStyleKey = (object) typeof (MediaControlButton);
    }

    public string DefaultImageSource
    {
      get
      {
        return (string) ((DependencyObject) this).GetValue(MediaControlButton.DefaultImageSourceProperty);
      }
      set
      {
        ((DependencyObject) this).SetValue(MediaControlButton.DefaultImageSourceProperty, (object) value);
        this.defaultImageSource = value;
      }
    }

    public string AlternateImageSource
    {
      get
      {
        return (string) ((DependencyObject) this).GetValue(MediaControlButton.AlternateImageSourceProperty);
      }
      set
      {
        ((DependencyObject) this).SetValue(MediaControlButton.AlternateImageSourceProperty, (object) value);
        this.alternateImageSource = value;
      }
    }

    public virtual void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.defaultImage = ((Control) this).GetTemplateChild("DefaultImageCtrl") as Image;
      this.alternateImage = ((Control) this).GetTemplateChild("AlternateImageCtrl") as Image;
      if (this.defaultImageSource != null)
        this.defaultImage.Source = (ImageSource) new BitmapImage(new Uri(this.defaultImageSource, UriKind.RelativeOrAbsolute));
      if (this.alternateImageSource != null)
        this.alternateImage.Source = (ImageSource) new BitmapImage(new Uri(this.alternateImageSource, UriKind.RelativeOrAbsolute));
      VisualStateManager.GoToState((Control) this, "Ready", false);
    }

    private static void DefaultImageSource_PropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MediaControlButton mediaControlButton))
        return;
      mediaControlButton.defaultImageSource = e.NewValue.ToString();
      mediaControlButton.defaultImage.Source = (ImageSource) new BitmapImage(new Uri(mediaControlButton.defaultImageSource, UriKind.RelativeOrAbsolute));
    }

    private static void AlternateImageSource_PropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MediaControlButton mediaControlButton))
        return;
      mediaControlButton.alternateImageSource = e.NewValue.ToString();
      mediaControlButton.alternateImage.Source = (ImageSource) new BitmapImage(new Uri(mediaControlButton.alternateImageSource, UriKind.RelativeOrAbsolute));
    }
  }
}
