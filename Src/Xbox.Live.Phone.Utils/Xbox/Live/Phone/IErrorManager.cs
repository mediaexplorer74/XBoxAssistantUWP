// *********************************************************
// Type: Xbox.Live.Phone.IErrorManager
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Diagnostics.CodeAnalysis;


namespace Xbox.Live.Phone
{
  public interface IErrorManager
  {
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This method declaration should be consistent with similar methods.")]
    void Fatal(string message);

    void Nonfatal(string message);

    void Nonfatal(string title, string message);

    void Nonfatal(string message, Action callback);

    void Nonfatal(string title, string body, Action callback);
  }
}
