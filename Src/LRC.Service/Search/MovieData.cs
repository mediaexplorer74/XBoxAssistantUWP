// *********************************************************
// Type: LRC.Service.Search.MovieData
// Assembly: LRC.Service, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9AC9DF80-1812-4A95-A1ED-40E18E090056
// *********************************************************LRC.Service.dll

using System;
using System.Collections.Generic;


namespace LRC.Service.Search
{
  public class MovieData : SearchData
  {
    public MovieData()
      : this((MovieData) null)
    {
    }

    public MovieData(SearchData searchData)
      : base(searchData)
    {
    }

    public MovieData(MovieData movieData)
      : base((SearchData) movieData)
    {
    }

    public TimeSpan? Duration { get; set; }

    public List<string> Actors { get; set; }

    public List<string> Directors { get; set; }

    public List<string> Writers { get; set; }

    public List<Provider> Providers { get; set; }
  }
}
