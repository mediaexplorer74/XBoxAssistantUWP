// *********************************************************
// Type: LRC.LrcPanoramaPage
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
using System.Windows.Navigation;
using Xbox.Live.Phone.Utils;


namespace LRC
{
  public class LrcPanoramaPage : LrcPage
  {
    private Uri backgroundImageUri;

    protected LrcPanoramaPage()
    {
    }

    protected virtual Panorama Panorama
    {
      get
      {
        if (DesignerProperties.IsInDesignTool)
          return (Panorama) null;
        throw new InvalidOperationException("The Panorama property must be overridden in a derived class.");
      }
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      this.LoadPanoramaBackground();
      base.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      this.UnloadPanoramaBackground();
      base.OnNavigatedFrom(e);
    }

    protected override void OmnitureTrackPageVisit()
    {
      Panorama panorama = this.Panorama;
      if (panorama == null || !(panorama.SelectedItem is PanoramaItem selectedItem))
        return;
      OmnitureAppMeasurement.Instance.TrackVisit(OmnitureViewConstants.JoinNames(this.OmniturePageName, ((FrameworkElement) selectedItem).Name), this.OmnitureChannelName);
    }

    private void LoadPanoramaBackground()
    {
      if (this.Panorama == null || !(this.backgroundImageUri != (Uri) null))
        return;
      ((Control) this.Panorama).Background = (Brush) BackgroundImageConverter.GetImageBrush(this.backgroundImageUri);
    }

    private void UnloadPanoramaBackground()
    {
      if (this.Panorama == null || ((Control) this.Panorama).Background == null)
        return;
      this.backgroundImageUri = ImageUtil.UnloadImageBrush(((Control) this.Panorama).Background as ImageBrush);
    }
  }
}
