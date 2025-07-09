// *********************************************************
// Type: LRC.SearchItem
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace LRC
{
  public partial class SearchItem : UserControl
  {
Grid LayoutRoot;
Image Image;
StackPanel DetailsPanel;
TextBlock Title;
TextBlock Artist;
StackPanel infoStack;
Image Icon;
TextBlock ReleaseDetails;
StarRatingControl StarRating;
    

    public SearchItem() => this.InitializeComponent();

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/Search/SearchItem.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.Image = (Image) ((FrameworkElement) this).FindName("Image");
      this.DetailsPanel = (StackPanel) ((FrameworkElement) this).FindName("DetailsPanel");
      this.Title = (TextBlock) ((FrameworkElement) this).FindName("Title");
      this.Artist = (TextBlock) ((FrameworkElement) this).FindName("Artist");
      this.infoStack = (StackPanel) ((FrameworkElement) this).FindName("infoStack");
      this.Icon = (Image) ((FrameworkElement) this).FindName("Icon");
      this.ReleaseDetails = (TextBlock) ((FrameworkElement) this).FindName("ReleaseDetails");
      this.StarRating = (StarRatingControl) ((FrameworkElement) this).FindName("StarRating");
    }
  }
}
