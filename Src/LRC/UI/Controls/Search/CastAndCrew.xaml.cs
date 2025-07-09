// *********************************************************
// Type: LRC.CastAndCrew
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace LRC
{
  public partial class CastAndCrew : UserControl
  {
    private const string ComponentName = "CastAndCrew";
ScrollViewer ScrollViewerWrapper;
StackPanel MovieRootPanel;
TextBlock NoCastAndCrewFound;
PersonGroup StarringPersonGroup;
PersonGroup DirectedByPersonGroup;
PersonGroup WrittenByPersonGroup;
    

    public CastAndCrew() => this.InitializeComponent();

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/Search/CastAndCrew.xaml", UriKind.Relative));
      this.ScrollViewerWrapper = (ScrollViewer) ((FrameworkElement) this).FindName("ScrollViewerWrapper");
      this.MovieRootPanel = (StackPanel) ((FrameworkElement) this).FindName("MovieRootPanel");
      this.NoCastAndCrewFound = (TextBlock) ((FrameworkElement) this).FindName("NoCastAndCrewFound");
      this.StarringPersonGroup = (PersonGroup) ((FrameworkElement) this).FindName("StarringPersonGroup");
      this.DirectedByPersonGroup = (PersonGroup) ((FrameworkElement) this).FindName("DirectedByPersonGroup");
      this.WrittenByPersonGroup = (PersonGroup) ((FrameworkElement) this).FindName("WrittenByPersonGroup");
    }
  }
}
