// *********************************************************
// Type: LRC.LrcPerformanceProgressBar
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;


namespace LRC
{
  [TemplateVisualState(GroupName = "VisibilityStates", Name = "Hidden")]
  [TemplateVisualState(GroupName = "VisibilityStates", Name = "Normal")]
  [TemplatePart(Name = "EmbeddedProgressBar", Type = typeof (ProgressBar))]
  public class LrcPerformanceProgressBar : Control
  {
    private const string EmbeddedProgressBarName = "EmbeddedProgressBar";
    private const string VisibilityVisualStateGroupName = "VisibilityStates";
    private const string VisibilityNormalState = "Normal";
    private const string VisibilityHiddenState = "Hidden";
    public static readonly DependencyProperty ActualIsIndeterminateProperty = DependencyProperty.Register(nameof (ActualIsIndeterminate), typeof (bool), typeof (LrcPerformanceProgressBar), new PropertyMetadata((object) false));
    public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(nameof (IsIndeterminate), typeof (bool), typeof (LrcPerformanceProgressBar), new PropertyMetadata((object) false, new PropertyChangedCallback(LrcPerformanceProgressBar.OnIsIndeterminatePropertyChanged)));
    private static readonly PropertyPath ActualIsIndeterminatePath = new PropertyPath(nameof (ActualIsIndeterminate), new object[0]);
    private ProgressBar progressBar;
    private VisualStateGroup visibilityVisualStateGroup;

    public LrcPerformanceProgressBar()
    {
      this.DefaultStyleKey = (object) typeof (LrcPerformanceProgressBar);
      ((FrameworkElement) this).Unloaded += new RoutedEventHandler(this.OnUnloaded);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.OnLoaded);
    }

    public bool ActualIsIndeterminate
    {
      get
      {
        return (bool) ((DependencyObject) this).GetValue(LrcPerformanceProgressBar.ActualIsIndeterminateProperty);
      }
      set
      {
        ((DependencyObject) this).SetValue(LrcPerformanceProgressBar.ActualIsIndeterminateProperty, (object) value);
      }
    }

    public bool IsIndeterminate
    {
      get
      {
        return (bool) ((DependencyObject) this).GetValue(LrcPerformanceProgressBar.IsIndeterminateProperty);
      }
      set
      {
        ((DependencyObject) this).SetValue(LrcPerformanceProgressBar.IsIndeterminateProperty, (object) value);
      }
    }

    public static FrameworkElement GetImplementationRoot(DependencyObject dependencyObject)
    {
      return 1 != VisualTreeHelper.GetChildrenCount(dependencyObject) ? (FrameworkElement) null : VisualTreeHelper.GetChild(dependencyObject, 0) as FrameworkElement;
    }

    public static VisualStateGroup TryGetVisualStateGroup(
      DependencyObject dependencyObject,
      string groupName)
    {
      FrameworkElement implementationRoot = LrcPerformanceProgressBar.GetImplementationRoot(dependencyObject);
      return implementationRoot == null ? (VisualStateGroup) null : VisualStateManager.GetVisualStateGroups(implementationRoot).OfType<VisualStateGroup>().Where<VisualStateGroup>((Func<VisualStateGroup, bool>) (group => string.CompareOrdinal(groupName, group.Name) == 0)).FirstOrDefault<VisualStateGroup>();
    }

    public virtual void OnApplyTemplate()
    {
      if (this.visibilityVisualStateGroup != null)
        this.visibilityVisualStateGroup.CurrentStateChanged -= new EventHandler<VisualStateChangedEventArgs>(this.OnVisualStateChanged);
      ((FrameworkElement) this).OnApplyTemplate();
      this.visibilityVisualStateGroup = LrcPerformanceProgressBar.TryGetVisualStateGroup((DependencyObject) this, "VisibilityStates");
      if (this.visibilityVisualStateGroup != null)
        this.visibilityVisualStateGroup.CurrentStateChanged += new EventHandler<VisualStateChangedEventArgs>(this.OnVisualStateChanged);
      this.progressBar = this.GetTemplateChild("EmbeddedProgressBar") as ProgressBar;
      this.UpdateVisualStates(false);
    }

    private static void OnIsIndeterminatePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is LrcPerformanceProgressBar performanceProgressBar))
        return;
      performanceProgressBar.OnIsIndeterminateChanged((bool) e.NewValue);
    }

    private void OnVisualStateChanged(object sender, VisualStateChangedEventArgs e)
    {
      if (e.NewState == null || !this.ActualIsIndeterminate || e == null || !(e.NewState.Name == "Hidden") || this.IsIndeterminate)
        return;
      this.ActualIsIndeterminate = false;
    }

    private void OnIsIndeterminateChanged(bool newValue)
    {
      if (newValue)
        this.ActualIsIndeterminate = true;
      else if (this.ActualIsIndeterminate && this.visibilityVisualStateGroup == null)
        this.ActualIsIndeterminate = false;
      this.UpdateVisualStates(true);
    }

    private void UpdateVisualStates(bool useTransitions)
    {
      VisualStateManager.GoToState((Control) this, this.IsIndeterminate ? "Normal" : "Hidden", useTransitions);
    }

    private void OnUnloaded(object sender, RoutedEventArgs ea)
    {
      if (this.progressBar == null)
        return;
      this.progressBar.IsIndeterminate = false;
    }

    private void OnLoaded(object sender, RoutedEventArgs ea)
    {
      if (this.progressBar == null)
        return;
      ((FrameworkElement) this.progressBar).SetBinding(ProgressBar.IsIndeterminateProperty, new Binding()
      {
        Source = (object) this,
        Path = LrcPerformanceProgressBar.ActualIsIndeterminatePath
      });
    }
  }
}
