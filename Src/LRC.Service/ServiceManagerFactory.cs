// *********************************************************
// Type: LRC.Service.ServiceManagerFactory
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using LRC.Service.Search;


namespace LRC.Service
{
  public static class ServiceManagerFactory
  {
    public static ISearchServiceManager CreateSearchServiceManager()
    {
      return (ISearchServiceManager) new SearchServiceManager();
    }

    public static IZuneCatalogServiceManager CreateZuneCatalogServiceManager()
    {
      return (IZuneCatalogServiceManager) new ZuneCatalogServiceManager();
    }
  }
}
