// *********************************************************
// Type: LRC.BusyIndicator
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System.Windows;
using System.Windows.Controls;


namespace LRC
{
  [TemplateVisualState(Name = "Hidden", GroupName = "VisibilityStates")]
  [TemplateVisualState(Name = "Visible", GroupName = "VisibilityStates")]
  public class BusyIndicator : ContentControl
  {
    public const double DefaultOpacity = 0.65;
    public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(nameof (IsBusy), typeof (bool), typeof (BusyIndicator), new PropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((BusyIndicator) d).ChangeVisualState(true))));
    public static readonly DependencyProperty IsOffscreenProperty = DependencyProperty.Register(nameof (IsOffscreen), typeof (bool), typeof (BusyIndicator), new PropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((BusyIndicator) d).ChangeVisualState(true))));
    public static readonly DependencyProperty BusyTextProperty = DependencyProperty.Register(nameof (BusyText), typeof (string), typeof (BusyIndicator), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ProgressBarVisibilityProperty = DependencyProperty.Register(nameof (ProgressBarVisibility), typeof (Visibility), typeof (BusyIndicator), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty OverlayVisiblityProperty = DependencyProperty.Register(nameof (OverlayVisiblity), typeof (Visibility), typeof (BusyIndicator), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty OverlayOpacityProperty = DependencyProperty.Register(nameof (OverlayOpacity), typeof (double), typeof (BusyIndicator), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IsBlockingProperty = DependencyProperty.Register(nameof (IsBlockingProperty), typeof (bool), typeof (BusyIndicator), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(nameof (IsIndeterminate), typeof (bool), typeof (BusyIndicator), new PropertyMetadata((PropertyChangedCallback) null));

    public BusyIndicator() => this.OverlayOpacity = 0.65;

    public bool IsBusy
    {
      get => (bool) ((DependencyObject) this).GetValue(BusyIndicator.IsBusyProperty);
      set => ((DependencyObject) this).SetValue(BusyIndicator.IsBusyProperty, (object) value);
    }

    public bool IsOffscreen
    {
      get => (bool) ((DependencyObject) this).GetValue(BusyIndicator.IsOffscreenProperty);
      set => ((DependencyObject) this).SetValue(BusyIndicator.IsOffscreenProperty, (object) value);
    }

    public string BusyText
    {
      get => (string) ((DependencyObject) this).GetValue(BusyIndicator.BusyTextProperty);
      set => ((DependencyObject) this).SetValue(BusyIndicator.BusyTextProperty, (object) value);
    }

    public bool IsIndeterminate
    {
      get => (bool) ((DependencyObject) this).GetValue(BusyIndicator.IsIndeterminateProperty);
      set
      {
        ((DependencyObject) this).SetValue(BusyIndicator.IsIndeterminateProperty, (object) value);
      }
    }

    public Visibility ProgressBarVisibility
    {
      get
      {
        return (Visibility) ((DependencyObject) this).GetValue(BusyIndicator.ProgressBarVisibilityProperty);
      }
      set
      {
        ((DependencyObject) this).SetValue(BusyIndicator.ProgressBarVisibilityProperty, (object) value);
      }
    }

    public Visibility OverlayVisiblity
    {
      get
      {
        return (Visibility) ((DependencyObject) this).GetValue(BusyIndicator.OverlayVisiblityProperty);
      }
      set
      {
        ((DependencyObject) this).SetValue(BusyIndicator.OverlayVisiblityProperty, (object) value);
      }
    }

    public double OverlayOpacity
    {
      get => (double) ((DependencyObject) this).GetValue(BusyIndicator.OverlayOpacityProperty);
      set
      {
        ((DependencyObject) this).SetValue(BusyIndicator.OverlayOpacityProperty, (object) value);
      }
    }

    public bool IsBlocking
    {
      get => (bool) ((DependencyObject) this).GetValue(BusyIndicator.IsBlockingProperty);
      set => ((DependencyObject) this).SetValue(BusyIndicator.IsBlockingProperty, (object) value);
    }

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this.ChangeVisualState(true);
    }

    private void ChangeVisualState(bool useTransitions)
    {
      if (this.IsBlocking)
      {
        VisualStateManager.GoToState((Control) this, this.IsBusy ? "VisibleFullScreenBlocking" : "Hidden", useTransitions);
        this.ProgressBarVisibility = (Visibility) 1;
        this.IsIndeterminate = false;
      }
      else
      {
        if (this.OverlayVisiblity == null)
          VisualStateManager.GoToState((Control) this, this.IsBusy ? "Visible" : "Hidden", useTransitions);
        else
          VisualStateManager.GoToState((Control) this, this.IsBusy ? "VisibleWithoutOverlay" : "HiddenWithoutOverlay", useTransitions);
        this.ProgressBarVisibility = !this.IsBusy || this.IsOffscreen ? (Visibility) 1 : (Visibility) 0;
        this.IsIndeterminate = this.IsBusy && !this.IsOffscreen;
      }
    }

    private static class VisualStates
    {
      public const string StateVisible = "Visible";
      public const string StateHidden = "Hidden";
      public const string StateHiddenWithoutOverlay = "HiddenWithoutOverlay";
      public const string StateVisibleWithoutOverlay = "VisibleWithoutOverlay";
      public const string StateVisibleFullScreenBlocking = "VisibleFullScreenBlocking";
      public const string GroupVisibility = "VisibilityStates";
    }
  }
}
