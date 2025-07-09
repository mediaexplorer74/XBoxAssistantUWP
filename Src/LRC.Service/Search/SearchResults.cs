// *********************************************************
// Type: LRC.Service.Search.SearchResults
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System.Collections.Generic;
using Xbox.Live.Phone.Utils.Serialization;


namespace LRC.Service.Search
{
  public class SearchResults
  {
    public XmlSerializableDictionary<Filter, int> TotalResultsCounter { get; set; }

    public List<SearchData> Items { get; set; }

    public string ContinuationToken { get; set; }
  }
}
