// *********************************************************
// Type: LRC.ImageViewerPivot
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Diagnostics;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

//using System.Windows.Controls;
//using System.Windows.Navigation;
using XLToolKit;


namespace LRC
{
  public partial class ImageViewerPivot : LrcPage
  {
Grid LayoutRoot;
XLPivot PivotItems;
    //

    public ImageViewerPivot()
    {
      this.InitializeComponent();
      this.DataContext = (object) App.GlobalData.SelectedImageCollection;
      this.OmniturePageName = "wp:lrc:imageviewer";
      this.OmnitureChannelName = "wp:lrc:image";
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      this.OmnitureTrackPageVisit();
    }

   /* [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/ImageViewer.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.PivotItems = (XLPivot) ((FrameworkElement) this).FindName("PivotItems");
    }*/
  }
}
