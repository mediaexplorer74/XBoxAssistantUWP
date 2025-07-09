// *********************************************************
// Type: LRC.WatermarkedTextBox
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Diagnostics.CodeAnalysis;
//using System.Windows;
//using System.Windows.Controls;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace LRC
{
  [TemplatePart(Name = "Watermark", Type = typeof (ContentControl))]
  [TemplateVisualState(Name = "Watermarked", GroupName = "WatermarkStates")]
  [TemplateVisualState(Name = "Unwatermarked", GroupName = "WatermarkStates")]
  [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
  [TemplateVisualState(Name = "Focused", GroupName = "FocusStates")]
  [TemplatePart(Name = "WatermarkText", Type = typeof (TextBlock))]
  public partial class WatermarkedTextBox : TextBox
  {
    private const string ElementContentName = "Watermark";
    private const string WaterMarkControlHostName = "WatermarkText";
    public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(nameof (Watermark), typeof (string), typeof (WatermarkedTextBox), new PropertyMetadata(new PropertyChangedCallback(WatermarkedTextBox.OnWatermarkPropertyChanged)));
    private ContentControl elementContent;
    private TextBlock watermarkHost;
    private bool isHovered;
    private bool hasFocus;

    public WatermarkedTextBox()
    {
      ((Control) this).DefaultStyleKey = (object) typeof (WatermarkedTextBox);
      this.SetDefaults();
      ((UIElement) this).MouseEnter += new MouseEventHandler(this.OnMouseEnter);
      ((UIElement) this).MouseLeave += new MouseEventHandler(this.OnMouseLeave);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.OnLoaded);
      ((UIElement) this).LostFocus += new RoutedEventHandler(this.OnLostFocus);
      ((UIElement) this).GotFocus += new RoutedEventHandler(this.OnGotFocus);
      this.TextChanged += new TextChangedEventHandler(this.OnTextChanged);
    }

    public string Watermark
    {
      get => (string) ((DependencyObject) this).GetValue(WatermarkedTextBox.WatermarkProperty);
      set
      {
        ((DependencyObject) this).SetValue(WatermarkedTextBox.WatermarkProperty, (object) value);
      }
    }

    public TextBlock WatermarkHost => this.watermarkHost;

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this.elementContent = this.ExtractTemplatePart<ContentControl>("Watermark");
      this.watermarkHost = this.ExtractTemplatePart<TextBlock>("WatermarkText");
      this.ChangeVisualState(false);
    }

    private static void OnWatermarkPropertyChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(sender is WatermarkedTextBox watermarkedTextBox))
        return;
      watermarkedTextBox.ChangeVisualState();
    }

    private static void OnIsEnabledPropertyChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(sender is WatermarkedTextBox watermarkedTextBox))
        return;
      bool newValue = (bool) e.NewValue;
      ((Control) watermarkedTextBox).IsEnabled = newValue;
      watermarkedTextBox.ChangeVisualState();
    }

    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "partName", Justification = "This is external source file")]
    private static T ExtractTemplatePart<T>(string partName, DependencyObject obj) where T : DependencyObject
    {
      return obj as T;
    }

    private T ExtractTemplatePart<T>(string partName) where T : DependencyObject
    {
      DependencyObject templateChild = ((Control) this).GetTemplateChild(partName);
      return WatermarkedTextBox.ExtractTemplatePart<T>(partName, templateChild);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      ((Control) this).ApplyTemplate();
      this.ChangeVisualState(false);
    }

    private void ChangeVisualState() => this.ChangeVisualState(true);

    private void ChangeVisualState(bool useTransitions)
    {
      if (!((Control) this).IsEnabled)
        WatermarkedTextBox.VisualStateHelper.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "Disabled", "Normal");
      else if (this.isHovered)
        WatermarkedTextBox.VisualStateHelper.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "MouseOver", "Normal");
      else
        WatermarkedTextBox.VisualStateHelper.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "Normal");
      if (this.hasFocus && ((Control) this).IsEnabled)
        WatermarkedTextBox.VisualStateHelper.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "Focused", "Unfocused");
      else
        WatermarkedTextBox.VisualStateHelper.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "Unfocused");
      if (!this.hasFocus && this.Watermark != null && string.IsNullOrEmpty(this.Text))
        WatermarkedTextBox.VisualStateHelper.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "Watermarked", "Unwatermarked");
      else
        WatermarkedTextBox.VisualStateHelper.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "Unwatermarked");
    }

    private void OnGotFocus(object sender, RoutedEventArgs e)
    {
      if (!((Control) this).IsEnabled)
        return;
      this.hasFocus = true;
      if (!string.IsNullOrEmpty(this.Text))
        this.Select(0, this.Text.Length);
      this.ChangeVisualState();
    }

    private void OnLostFocus(object sender, RoutedEventArgs e)
    {
      this.hasFocus = false;
      this.ChangeVisualState();
    }

    private void OnMouseEnter(object sender, MouseEventArgs e)
    {
      this.isHovered = true;
      if (this.hasFocus)
        return;
      this.ChangeVisualState();
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
      this.isHovered = false;
      this.ChangeVisualState();
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e) => this.ChangeVisualState();

    private void SetDefaults() => ((Control) this).IsEnabled = true;

    private static class VisualStateHelper
    {
      public const string GroupCommon = "CommonStates";
      public const string GroupFocus = "FocusStates";
      public const string GroupSelection = "SelectionStates";
      public const string GroupWatermark = "WatermarkStates";
      public const string StateDisabled = "Disabled";
      public const string StateFocused = "Focused";
      public const string StateMouseOver = "MouseOver";
      public const string StateNormal = "Normal";
      public const string StateUnfocused = "Unfocused";
      public const string StateUnwatermarked = "Unwatermarked";
      public const string StateWatermarked = "Watermarked";

      public static void GoToState(
        Control control,
        bool useTransitions,
        params string[] stateNames)
      {
        if (control == null)
          throw new ArgumentNullException(nameof (control));
        if (stateNames == null)
          return;
        foreach (string stateName in stateNames)
        {
          if (VisualStateManager.GoToState(control, stateName, useTransitions))
            break;
        }
      }
    }
  }
}
