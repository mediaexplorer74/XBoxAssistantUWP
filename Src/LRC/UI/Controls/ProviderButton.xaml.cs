// *********************************************************
// Type: LRC.ProviderButton
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace LRC
{
  public partial class ProviderButton : Button
  {
Image DefaultProviderImage;
Image ProviderImage;
TextBlock ProviderDetails;
    

    public ProviderButton() => this.InitializeComponent();

    // [Удалено для UWP портирования]
    // public void InitializeComponent()
    // {
      
        return;
      
      Application.LoadComponent((object) this, new Uri("/LRC;component/UI/Controls/ProviderButton.xaml", UriKind.Relative));
      this.DefaultProviderImage = (Image) ((FrameworkElement) this).FindName("DefaultProviderImage");
      this.ProviderImage = (Image) ((FrameworkElement) this).FindName("ProviderImage");
      this.ProviderDetails = (TextBlock) ((FrameworkElement) this).FindName("ProviderDetails");
    }
  }
}
