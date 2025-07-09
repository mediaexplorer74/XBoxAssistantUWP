// *********************************************************
// Type: LRC.VideoDetailsPanel
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace LRC
{
  public class VideoDetailsPanel : UserControl
  {
Image DetailsImage;
TextBlock DetailTitleTextBlock;
TextBlock ReleaseDetailsTextBlock;
TextBlock StudioTextBlock;
    

    public VideoDetailsPanel() => this.InitializeComponent();

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/VideoDetailsPanel.xaml", UriKind.Relative));
      this.DetailsImage = (Image) ((FrameworkElement) this).FindName("DetailsImage");
      this.DetailTitleTextBlock = (TextBlock) ((FrameworkElement) this).FindName("DetailTitleTextBlock");
      this.ReleaseDetailsTextBlock = (TextBlock) ((FrameworkElement) this).FindName("ReleaseDetailsTextBlock");
      this.StudioTextBlock = (TextBlock) ((FrameworkElement) this).FindName("StudioTextBlock");
    }
  }
}
