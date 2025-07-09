// *********************************************************
// Type: XLToolKit.SwitchPanel
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.ComponentModel;
using System.Windows;
using Windows.UI.Xaml;

//using System.Windows.Controls;
//using System.Windows.Markup;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;


namespace XLToolKit
{
  public partial class SwitchPanel : ItemsControl, INotifyPropertyChanged
  {
    private const string ItemsPanelStyle 
            = "<ItemsPanelTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Grid/></ItemsPanelTemplate>";
    public static readonly DependencyProperty CurrentStateProperty 
            = DependencyProperty.Register(nameof (CurrentState), typeof (int), typeof (SwitchPanel), 
                new PropertyMetadata((object) 0, (PropertyChangedCallback) ((d, e) => ((SwitchPanel) d).OnStateChanged(e))));
    private bool isLoaded;
    private int currentState;

    public SwitchPanel()
    {
      ((UIElement) this).Visibility = (Visibility) 1;
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.SwitchPanel_Loaded);
      ((FrameworkElement) this).Unloaded += new RoutedEventHandler(this.SwitchPanel_Unloaded);
      this.ItemsPanel = (ItemsPanelTemplate) XamlReader.Load(
          "<ItemsPanelTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Grid/></ItemsPanelTemplate>");
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public int CurrentState
    {
      get => this.currentState;
      set
      {
        if (this.currentState == value)
          return;
        this.currentState = value;
        if (this.isLoaded)
          this.SetVisibilities();
        this.NotifyPropertyChanged(nameof (CurrentState));
      }
    }

    protected void NotifyPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    private void SwitchPanel_Loaded(object sender, RoutedEventArgs e)
    {
      ((UIElement) this).Visibility = (Visibility) 0;
      this.isLoaded = true;
      this.SetVisibilities();
    }

    private void SwitchPanel_Unloaded(object sender, RoutedEventArgs e) => this.isLoaded = false;

    private void OnStateChanged(DependencyPropertyChangedEventArgs e)
    {
      this.CurrentState = (int) e.NewValue;
    }

    private void SetVisibilities()
    {
      if (this.Items == null || ((PresentationFrameworkCollection<object>) this.Items).Count <= this.currentState)
        throw new ArgumentException("Invalid CurrentState");
      for (int index = 0; index < ((PresentationFrameworkCollection<object>) this.Items).Count; ++index)
      {
        if (!(((PresentationFrameworkCollection<object>) this.Items)[index] is UIElement uiElement))
          throw new ArgumentException("SwitchPanel item must be UIElement");
        uiElement.Visibility = index != this.currentState ? (Visibility) 1 : (Visibility) 0;
      }
    }
  }
}
