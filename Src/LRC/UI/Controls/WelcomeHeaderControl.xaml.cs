// *********************************************************
// Type: LRC.WelcomeHeaderControl
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace LRC
{
  public partial class WelcomeHeaderControl : UserControl
  {
TextBlock WelcomeText;
    

    public WelcomeHeaderControl()
    {
      this.InitializeComponent();
      ((FrameworkElement) this).DataContext = (object) App.GlobalData;
    }

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/WelcomeHeaderControl.xaml", UriKind.Relative));
      this.WelcomeText = (TextBlock) ((FrameworkElement) this).FindName("WelcomeText");
    }
  }
}
