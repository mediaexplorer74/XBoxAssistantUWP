// *********************************************************
// Type: LRC.ZuneHubPage
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using LRC.Service.Omniture;
using LRC.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;


namespace LRC
{
  public class ZuneHubPage : LrcPage
  {
    internal Grid LayoutRoot;
    internal Grid PageContents;
    internal TextBlock PageTitle;
    internal TextBlock PageSubTitle;
    internal BusyIndicator BusyIndicator;
    internal Grid ContentPanel;
    internal ListBoxWithCompression ListBox;
    internal MediaBar MediaControls;
    private bool _contentLoaded;

    public ZuneHubPage()
    {
      this.InitializeComponent();
      this.Persistent = false;
      this.AllowMultipleInstances = false;
      this.NeedToSaveViewModel = true;
      this.OmniturePageName = "wp:lrc:zunemarketplace:hub";
      this.OmnitureChannelName = "wp:lrc:zunemarketplace";
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (this.DataContext is ZuneHubViewModel zuneHubViewModel)
      {
        zuneHubViewModel.Load();
      }
      else
      {
        zuneHubViewModel = new ZuneHubViewModel();
        zuneHubViewModel.SelectedMedia = App.GlobalData.SelectedMediaDetails as MediaItem;
        App.GlobalData.SelectedMediaDetails = (ViewModelBase) null;
        this.DataContext = (object) zuneHubViewModel;
        zuneHubViewModel.Load();
      }
      if (zuneHubViewModel == null)
        return;
      if (((FrameworkElement) this.ListBox).Style == null)
      {
        ((FrameworkElement) this.ListBox).Style = Application.Current.Resources[(object) zuneHubViewModel.Style] as Style;
        ((ItemsControl) this.ListBox).ItemTemplate = Application.Current.Resources[(object) zuneHubViewModel.Template] as DataTemplate;
        ((Control) this.ListBox).ApplyTemplate();
      }
      OmnitureAppMeasurement.Instance.TrackVisit(this.OmniturePageName, this.OmnitureChannelName);
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(sender is System.Windows.Controls.ListBox listBox) || ((Selector) listBox).SelectedIndex == -1)
        return;
      ViewModelBase selectedItem = (ViewModelBase) ((Selector) listBox).SelectedItem;
      App.GlobalData.SelectedMediaDetails = selectedItem;
      NavHelper.SafeNavigate((Page) this, NavHelper.GetZuneVideoDetailsPage(((MediaItem) selectedItem).MediaType));
      ((Selector) listBox).SelectedIndex = -1;
    }

    private void ListBox_BottomReached(object sender, VisualStateChangedEventArgs e)
    {
      ((ZuneHubViewModel) this.DataContext)?.FetchMoreData();
    }

    private void VideoList_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is ListBoxWithCompression boxWithCompression))
        return;
      boxWithCompression.EventListboxBottomReached += new EventHandler<VisualStateChangedEventArgs>(this.ListBox_BottomReached);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Views/ZuneMarketplace/ZuneHubPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.PageContents = (Grid) ((FrameworkElement) this).FindName("PageContents");
      this.PageTitle = (TextBlock) ((FrameworkElement) this).FindName("PageTitle");
      this.PageSubTitle = (TextBlock) ((FrameworkElement) this).FindName("PageSubTitle");
      this.BusyIndicator = (BusyIndicator) ((FrameworkElement) this).FindName("BusyIndicator");
      this.ContentPanel = (Grid) ((FrameworkElement) this).FindName("ContentPanel");
      this.ListBox = (ListBoxWithCompression) ((FrameworkElement) this).FindName("ListBox");
      this.MediaControls = (MediaBar) ((FrameworkElement) this).FindName("MediaControls");
    }
  }
}
