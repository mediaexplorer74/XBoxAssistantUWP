// *********************************************************
// Type: LRC.VideoListBox
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using LRC.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;


namespace LRC
{
  public class VideoListBox : ResourceDictionary
  {
    

    public VideoListBox() => this.InitializeComponent();

    public Action<Uri> ExecuteAction { get; set; }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      MediaItem tag = (MediaItem) ((FrameworkElement) sender).Tag;
      if (tag == null)
        return;
      App.GlobalData.SelectedMediaDetails = (ViewModelBase) tag;
      if (this.ExecuteAction == null)
        return;
      this.ExecuteAction(NavHelper.GetZuneVideoDetailsPage(tag.MediaType));
      App.GlobalData.OmnitureEntryPoint = "browse";
      OmnitureAppMeasurement.Instance.TrackZuneCategoryContentItemSelectedEvent("media selected", "browse", tag.Title, tag.Id, "na");
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Templates/VideoListBox.xaml", UriKind.Relative));
    }
  }
}
