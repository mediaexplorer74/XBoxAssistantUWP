// *********************************************************
// Type: LRC.SearchDetailsPageBase
// Assembly: LRC, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ECC19289-BE61-4031-A6AE-6F34448F9C8F
// *********************************************************LRC.dll


namespace LRC
{
  public class SearchDetailsPageBase : LrcPivotPage
  {
    protected SearchDetailsPageBase()
    {
    }

    public virtual void ResetToDefaultView()
    {
      if (this.Pivot == null)
        return;
      this.Pivot.SelectedIndex = 0;
    }
  }
}
