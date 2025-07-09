// *********************************************************
// Type: LRC.LrcPivotPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using LRC.UI.Omniture;
using Microsoft.Phone.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xbox.Live.Phone.Utils;


namespace LRC
{
  public class LrcPivotPage : LrcPage
  {
    private Uri backgroundImageUri;

    protected LrcPivotPage()
    {
    }

    protected virtual Pivot Pivot
    {
      get
      {
        if (DesignerProperties.IsInDesignTool)
          return (Pivot) null;
        throw new InvalidOperationException("The Pivot property must be overridden in a derived class.");
      }
    }

    protected override void OmnitureTrackPageVisit()
    {
      Pivot pivot = this.Pivot;
      if (pivot == null || !(pivot.SelectedItem is PivotItem selectedItem))
        return;
      OmnitureAppMeasurement.Instance.TrackVisit(OmnitureViewConstants.JoinNames(this.OmniturePageName, ((FrameworkElement) selectedItem).Name), this.OmnitureChannelName);
    }

    private void LoadPivotBackground()
    {
      if (this.Pivot == null || !(this.backgroundImageUri != (Uri) null))
        return;
      ((Control) this.Pivot).Background = (Brush) BackgroundImageConverter.GetImageBrush(this.backgroundImageUri);
    }

    private void UnloadPivotBackground()
    {
      if (this.Pivot == null || ((Control) this.Pivot).Background == null)
        return;
      this.backgroundImageUri = ImageUtil.UnloadImageBrush(((Control) this.Pivot).Background as ImageBrush);
    }
  }
}
