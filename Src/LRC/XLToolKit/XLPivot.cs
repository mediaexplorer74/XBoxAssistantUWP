// *********************************************************
// Type: XLToolKit.XLPivot
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

//using Microsoft.Phone.Controls;
using System;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace XLToolKit
{
  public partial class XLPivot : Pivot
  {
    public XLPivot()
    {
      this.LoadingPivotItem += new EventHandler<PivotItemEventArgs>(this.XLPivot_LoadingPivotItem);
      this.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(this.XLPivot_LoadedPivotItem);
    }

    private void XLPivot_LoadingPivotItem(object sender, PivotItemEventArgs e)
    {
      ((UIElement) this).IsHitTestVisible = false;
    }

    private void XLPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
    {
      ((UIElement) this).IsHitTestVisible = true;
    }
  }
}
