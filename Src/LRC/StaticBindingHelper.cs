// *********************************************************
// Type: LRC.StaticBindingHelper
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll

using System.Globalization;
using Xbox.Live.Phone.Services;


namespace LRC
{
  public class StaticBindingHelper
  {
    private CultureInfo accountCulture;

    public CultureInfo AccountCulture
    {
      get
      {
        if (this.accountCulture == null)
          this.accountCulture = new CultureInfo(XboxLiveGamer.CurrentGamer.LegalLocale);
        return this.accountCulture;
      }
    }
  }
}
