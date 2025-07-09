// *********************************************************
// Type: Xbox.Live.Phone.Utils.TextUtil
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Collections.ObjectModel;


namespace Xbox.Live.Phone.Utils
{
  public class TextUtil
  {
    private const int DefaultTextBlockLimit = 2500;

    public static ObservableCollection<string> SplitString(string input)
    {
      return TextUtil.SplitString(2500, input);
    }

    public static ObservableCollection<string> SplitString(int limit, string input)
    {
      if (string.IsNullOrWhiteSpace(input))
        return (ObservableCollection<string>) null;
      ObservableCollection<string> observableCollection = new ObservableCollection<string>();
      int startIndex = 0;
      int num;
      for (; startIndex + limit < input.Length; startIndex = num + 1)
      {
        num = input.IndexOf(" ", startIndex + limit, StringComparison.OrdinalIgnoreCase);
        if (num != -1)
          observableCollection.Add(input.Substring(startIndex, num - startIndex));
        else
          break;
      }
      observableCollection.Add(input.Substring(startIndex, input.Length - startIndex));
      return observableCollection;
    }
  }
}
