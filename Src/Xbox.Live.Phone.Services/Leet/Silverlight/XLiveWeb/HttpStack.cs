// *********************************************************
// Type: Leet.Silverlight.XLiveWeb.HttpStack
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Diagnostics.CodeAnalysis;


namespace Leet.Silverlight.XLiveWeb
{
  [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Only two supported values. None is not appropriate here.")]
  public enum HttpStack
  {
    PlatformDefault = 1,
    SilverlightClientStack = 2,
  }
}
