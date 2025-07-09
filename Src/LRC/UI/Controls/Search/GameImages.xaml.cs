// *********************************************************
// Type: LRC.GameImages
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;


namespace LRC
{
  public partial class GameImages : UserControl
  {
    private const string ComponentName = "GameImages";
Grid LayoutRoot;
TextBlock ImagesNoItemsFound;
ListBox ImagesListBox;
    

    public GameImages() => this.InitializeComponent();

    private void ImagesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (((Selector) this.ImagesListBox).SelectedIndex < 0)
        return;
      if (e.AddedItems != null && e.AddedItems.Count > 0 && e.AddedItems[0] is string)
      {
        LrcPage lrcPageParent = LrcPage.GetLrcPageParent((FrameworkElement) this);
        if (lrcPageParent != null && ((FrameworkElement) this).DataContext is SearchItemViewModel dataContext)
        {
          ObservableCollection<string> observableCollection = new ObservableCollection<string>();
          for (int selectedIndex = ((Selector) this.ImagesListBox).SelectedIndex; selectedIndex < dataContext.LargeImageUrls.Count; ++selectedIndex)
            observableCollection.Add(dataContext.LargeImageUrls[selectedIndex]);
          for (int index = 0; index < ((Selector) this.ImagesListBox).SelectedIndex; ++index)
            observableCollection.Add(dataContext.LargeImageUrls[index]);
          App.GlobalData.SelectedImageCollection = observableCollection;
          NavHelper.SafeNavigate((Page) lrcPageParent, NavHelper.ImageViewerUri);
        }
      }
      ((Selector) this.ImagesListBox).SelectedIndex = -1;
    }

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/Search/GameImages.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.ImagesNoItemsFound = (TextBlock) ((FrameworkElement) this).FindName("ImagesNoItemsFound");
      this.ImagesListBox = (ListBox) ((FrameworkElement) this).FindName("ImagesListBox");
    }
  }
}
