// *********************************************************
// Type: LRC.ViewModel.ZuneNamespace
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System.Xml.Linq;


namespace LRC.ViewModel
{
  public static class ZuneNamespace
  {
    private static XNamespace atom = (XNamespace) "http://www.w3.org/2005/Atom";
    private static XNamespace zuneDefaultNamespace = (XNamespace) "http://schemas.zune.net/catalog/music/2007/10";

    public static XNamespace Atom => ZuneNamespace.atom;

    public static XNamespace ZuneDefaultNamespace => ZuneNamespace.zuneDefaultNamespace;
  }
}
