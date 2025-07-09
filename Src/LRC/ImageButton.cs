// *********************************************************
// Type: LRC.ImageButton
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using Delay;
using LRC.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;


namespace LRC
{
  public class ImageButton : Button
  {
    private static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register(nameof (ImageUrl), typeof (string), typeof (ImageButton), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ImageButton.ImageUrl_PropertyChangedCallback)));
    private static readonly DependencyProperty DefaultImageUrlProperty = DependencyProperty.Register(nameof (DefaultImageUrl), typeof (string), typeof (ImageButton), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ImageButton.DefaultImageUrl_PropertyChangedCallback)));
    private static readonly DependencyProperty ImageTextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (ImageButton), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ImageButton.ImageText_PropertyChangedCallback)));
    private static readonly DependencyProperty ContextProperty = DependencyProperty.Register(nameof (Context), typeof (ViewModelBase), typeof (ImageButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageButton.Context_PropertyChangedCallback)));
    private static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(nameof (DataSource), typeof (object), typeof (ImageButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageButton.DataSource_PropertyChangedCallback)));
    private Image buttonImage;
    private TextBlock buttonText;
    private RowDefinition imageRow;
    private string imageUrl;
    private string defaultImageUrl;
    private string imageText;

    public ImageButton() => ((Control) this).DefaultStyleKey = (object) typeof (ImageButton);

    public string ImageUrl
    {
      get => (string) ((DependencyObject) this).GetValue(ImageButton.ImageUrlProperty);
      set
      {
        ((DependencyObject) this).SetValue(ImageButton.ImageUrlProperty, (object) value);
        this.imageUrl = value;
      }
    }

    public string DefaultImageUrl
    {
      get => (string) ((DependencyObject) this).GetValue(ImageButton.DefaultImageUrlProperty);
      set
      {
        ((DependencyObject) this).SetValue(ImageButton.DefaultImageUrlProperty, (object) value);
        this.DefaultImageUrl = value;
      }
    }

    public GridLength ImageHeight { get; set; }

    public string Text
    {
      get => (string) ((DependencyObject) this).GetValue(ImageButton.ImageTextProperty);
      set
      {
        ((DependencyObject) this).SetValue(ImageButton.ImageTextProperty, (object) value);
        this.imageText = value;
      }
    }

    public object DataSource
    {
      get => ((DependencyObject) this).GetValue(ImageButton.DataSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.DataSourceProperty, value);
    }

    public ViewModelBase Context
    {
      get => (ViewModelBase) ((DependencyObject) this).GetValue(ImageButton.ContextProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.ContextProperty, (object) value);
    }

    public virtual void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.buttonText = ((Control) this).GetTemplateChild("ImageText") as TextBlock;
      this.buttonImage = ((Control) this).GetTemplateChild("ImageCtrl") as Image;
      this.imageRow = ((Control) this).GetTemplateChild("ImageRow") as RowDefinition;
      this.imageRow.Height = this.ImageHeight;
      if (!string.IsNullOrEmpty(this.imageUrl))
        ImageButton.LoadImage(this.buttonImage, this.imageUrl);
      if (!string.IsNullOrEmpty(this.defaultImageUrl))
        ImageButton.LoadDefaultImage(this.buttonImage, this.defaultImageUrl);
      if (this.imageText == null)
        return;
      ImageButton.SetText(this.imageText, this.buttonText);
    }

    private static void ImageUrl_PropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ImageButton imageButton) || e.NewValue == null)
        return;
      imageButton.imageUrl = e.NewValue.ToString();
      ImageButton.LoadImage(imageButton.buttonImage, imageButton.imageUrl);
    }

    private static void DefaultImageUrl_PropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ImageButton imageButton) || e.NewValue == null)
        return;
      imageButton.defaultImageUrl = e.NewValue.ToString();
      ImageButton.LoadDefaultImage(imageButton.buttonImage, imageButton.defaultImageUrl);
    }

    private static void ImageText_PropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ImageButton imageButton) || e.NewValue == null)
        return;
      imageButton.imageText = e.NewValue.ToString();
      ImageButton.SetText(imageButton.imageText, imageButton.buttonText);
    }

    private static void Context_PropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = d as ImageButton;
      if (!(e.NewValue is ViewModelBase newValue) || imageButton == null)
        return;
      imageButton.imageUrl = newValue.ImageUrl;
      imageButton.imageText = newValue.Title;
      ImageButton.LoadImage(imageButton.buttonImage, newValue.ImageUrl);
      ImageButton.SetText(newValue.Title, imageButton.buttonText);
    }

    private static void DataSource_PropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = d as ImageButton;
      if (!(e.NewValue is ViewModelBase newValue) || imageButton == null)
        return;
      imageButton.DataSource = (object) newValue;
    }

    private static void LoadImage(Image imgCtrl, string imageUrl)
    {
      if (string.IsNullOrEmpty(imageUrl) || imgCtrl == null)
        return;
      if (imageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        LowProfileImageLoader.SetUriSource(imgCtrl, new Uri(imageUrl, UriKind.Absolute));
      else
        LowProfileImageLoader.SetUriSource(imgCtrl, new Uri(imageUrl, UriKind.RelativeOrAbsolute));
    }

    private static void LoadDefaultImage(Image imgCtrl, string defaultImageUrl)
    {
      if (string.IsNullOrEmpty(defaultImageUrl) || imgCtrl == null)
        return;
      LowProfileImageLoader.SetDefaultUriSource(imgCtrl, new Uri(defaultImageUrl, UriKind.RelativeOrAbsolute));
    }

    private static void SetText(string text, TextBlock textBoxCtrl)
    {
      if (textBoxCtrl == null)
        return;
      textBoxCtrl.Text = text;
    }
  }
}
