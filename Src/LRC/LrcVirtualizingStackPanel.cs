// *********************************************************
// Type: LRC.LrcVirtualizingStackPanel
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Xbox.Live.Phone.Utils;


namespace LRC
{
  public class LrcVirtualizingStackPanel : VirtualizingStackPanel
  {
    protected virtual void OnCleanUpVirtualizedItem(CleanUpVirtualizedItemEventArgs e)
    {
      base.OnCleanUpVirtualizedItem(e);
      if (e == null)
        return;
      List<Image> childrenOfType = FindChildrenUtil.FindChildrenOfType<Image>((DependencyObject) e.UIElement);
      if (childrenOfType == null)
        return;
      foreach (Image imageCtrl in childrenOfType)
      {
        if (imageCtrl != null && ((FrameworkElement) imageCtrl).Tag != null)
          ImageUtil.UnloadImage(imageCtrl);
      }
    }
  }
}
