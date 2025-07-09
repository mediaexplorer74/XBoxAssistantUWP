// *********************************************************
// Type: LRC.ListBoxWithCompression
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Collections;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xbox.Live.Phone.Utils;


namespace LRC
{
  public class ListBoxWithCompression : ListBox
  {
    private const string VerticalVisualStateName = "VerticalCompression";
    private const string NoVerticalCompression = "NoVerticalCompression";
    private const string CompressionBottom = "CompressionBottom";
    public static readonly DependencyProperty ScrollingDisabledProperty = DependencyProperty.Register(nameof (ScrollingDisabled), typeof (bool), typeof (ListBoxWithCompression), new PropertyMetadata((object) true, (PropertyChangedCallback) ((d, e) => ((ListBoxWithCompression) d).DisableScrolling((bool) e.NewValue))));
    private ScrollViewer scrollViewer;
    private VisualStateGroup vgroup;

    public ListBoxWithCompression()
    {
      ((FrameworkElement) this).Unloaded += new RoutedEventHandler(this.ListBox_Unloaded);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.ListBox_Loaded);
    }

    public event EventHandler<VisualStateChangedEventArgs> EventListboxBottomReached
    {
      add
      {
        this.ListboxBottomReached -= value;
        this.ListboxBottomReached += value;
      }
      remove => this.ListboxBottomReached -= value;
    }

    private event EventHandler<VisualStateChangedEventArgs> ListboxBottomReached;

    public ScrollViewer ScrollViewer => this.scrollViewer;

    public bool ScrollingDisabled
    {
      get
      {
        return (bool) ((DependencyObject) this).GetValue(ListBoxWithCompression.ScrollingDisabledProperty);
      }
      set
      {
        ((DependencyObject) this).SetValue(ListBoxWithCompression.ScrollingDisabledProperty, (object) value);
      }
    }

    public void ResetPosition()
    {
      if (this.scrollViewer == null)
        return;
      this.scrollViewer.ScrollToVerticalOffset(0.0);
      if (this.scrollViewer.HorizontalScrollBarVisibility == null)
        return;
      this.scrollViewer.ScrollToHorizontalOffset(0.0);
    }

    public virtual void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.scrollViewer = FindChildrenUtil.FindFirstChildOfType<ScrollViewer>((DependencyObject) this);
      if (this.scrollViewer != null)
      {
        ((Control) this.scrollViewer).ApplyTemplate();
        if (VisualTreeHelper.GetChildrenCount((DependencyObject) this.scrollViewer) > 0 && VisualTreeHelper.GetChild((DependencyObject) this.scrollViewer, 0) is FrameworkElement child)
          this.vgroup = ListBoxWithCompression.FindScrollViewerVisualState(child, "VerticalCompression");
      }
      this.HookScrollEvent();
    }

    private static VisualStateGroup FindScrollViewerVisualState(
      FrameworkElement element,
      string name)
    {
      if (element == null)
        return (VisualStateGroup) null;
      foreach (VisualStateGroup visualStateGroup in (IEnumerable) VisualStateManager.GetVisualStateGroups(element))
      {
        if (visualStateGroup.Name == name)
          return visualStateGroup;
      }
      return (VisualStateGroup) null;
    }

    private void HookScrollEvent()
    {
      if (this.vgroup == null)
        return;
      this.vgroup.CurrentStateChanging -= new EventHandler<VisualStateChangedEventArgs>(this.VisualgroupCurrentStateChanging);
      this.vgroup.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(this.VisualgroupCurrentStateChanging);
    }

    private void UnHookScrollEvent()
    {
      if (this.vgroup == null)
        return;
      this.vgroup.CurrentStateChanging -= new EventHandler<VisualStateChangedEventArgs>(this.VisualgroupCurrentStateChanging);
    }

    private void VisualgroupCurrentStateChanging(object sender, VisualStateChangedEventArgs e)
    {
      if (e == null || !(e.NewState.Name == "CompressionBottom"))
        return;
      EventHandler<VisualStateChangedEventArgs> listboxBottomReached = this.ListboxBottomReached;
      if (listboxBottomReached == null)
        return;
      listboxBottomReached((object) this, e);
    }

    private void ListBox_Unloaded(object sender, RoutedEventArgs e) => this.UnHookScrollEvent();

    private void ListBox_Loaded(object sender, RoutedEventArgs e)
    {
      this.HookScrollEvent();
      if (this.ScrollViewer == null)
        return;
      ((UIElement) this.ScrollViewer).InvalidateArrange();
      ((UIElement) this.ScrollViewer).UpdateLayout();
      this.ScrollViewer.ScrollToVerticalOffset(this.ScrollViewer.VerticalOffset);
    }

    private void DisableScrolling(bool disabled)
    {
      if (this.ScrollViewer == null)
        return;
      if (!disabled)
        ThreadManager.UIThreadPost((SendOrPostCallback) delegate
        {
          if (this.ScrollViewer == null)
            return;
          ((Control) this.ScrollViewer).IsEnabled = true;
          ((UIElement) this.ScrollViewer).InvalidateArrange();
          ((UIElement) this.ScrollViewer).UpdateLayout();
          this.ScrollViewer.ScrollToVerticalOffset(this.ScrollViewer.VerticalOffset);
        }, (object) this);
      else
        ((Control) this.ScrollViewer).IsEnabled = false;
    }
  }
}
