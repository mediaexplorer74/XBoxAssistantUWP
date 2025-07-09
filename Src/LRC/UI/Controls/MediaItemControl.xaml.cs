// *********************************************************
// Type: LRC.MediaItemControl
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace LRC
{
  public partial class MediaItemControl : UserControl
  {
    public static readonly DependencyProperty LineOneProperty = DependencyProperty.Register(nameof (LineOne), typeof (string), typeof (MediaItemControl), new PropertyMetadata((object) null, new PropertyChangedCallback(MediaItemControl.LineOneChangedCallback)));
    public static readonly DependencyProperty LineTwoProperty = DependencyProperty.Register(nameof (LineTwo), typeof (string), typeof (MediaItemControl), new PropertyMetadata((object) null, new PropertyChangedCallback(MediaItemControl.LineTwoChangedCallback)));
    public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register(nameof (ImageUrl), typeof (string), typeof (MediaItemControl), new PropertyMetadata((PropertyChangedCallback) null));
Grid LayoutRoot;
Image ImageIcon;
TextBlock LineOneTextBlock;
TextBlock LineTwoTextBlock;
    

    public MediaItemControl() => this.InitializeComponent();

    public string LineOne
    {
      get => (string) ((DependencyObject) this).GetValue(MediaItemControl.LineOneProperty);
      set
      {
        ((DependencyObject) this).SetValue(MediaItemControl.LineOneProperty, (object) value);
        this.LineOneTextBlock.Text = value;
      }
    }

    public string LineTwo
    {
      get => (string) ((DependencyObject) this).GetValue(MediaItemControl.LineTwoProperty);
      set
      {
        ((DependencyObject) this).SetValue(MediaItemControl.LineTwoProperty, (object) value);
        this.LineTwoTextBlock.Text = value;
      }
    }

    public string ImageUrl
    {
      get => (string) ((DependencyObject) this).GetValue(MediaItemControl.ImageUrlProperty);
      set => ((DependencyObject) this).SetValue(MediaItemControl.ImageUrlProperty, (object) value);
    }

    private static void LineTwoChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MediaItemControl mediaItemControl))
        return;
      mediaItemControl.LineTwoTextBlock.Text = e.NewValue as string;
    }

    private static void LineOneChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MediaItemControl mediaItemControl))
        return;
      mediaItemControl.LineOneTextBlock.Text = e.NewValue as string;
    }

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/MediaItemControl.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.ImageIcon = (Image) ((FrameworkElement) this).FindName("ImageIcon");
      this.LineOneTextBlock = (TextBlock) ((FrameworkElement) this).FindName("LineOneTextBlock");
      this.LineTwoTextBlock = (TextBlock) ((FrameworkElement) this).FindName("LineTwoTextBlock");
    }
  }
}
