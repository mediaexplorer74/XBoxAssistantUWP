// *********************************************************
// Type: Xbox.Live.Phone.Utils.Etc.HashSet`1
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System.Collections.Generic;


namespace Xbox.Live.Phone.Utils.Etc
{
  public class HashSet<T>
  {
    private Dictionary<T, object> innerDict = new Dictionary<T, object>();

    public int Count => this.innerDict.Count;

    public bool Add(T element)
    {
      bool flag = this.Contains(element);
      if (!flag)
        this.innerDict.Add(element, (object) null);
      return !flag;
    }

    public void Remove(T element) => this.innerDict.Remove(element);

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this.innerDict.Keys.GetEnumerator();

    public bool Contains(T element) => this.innerDict.ContainsKey(element);

    public void Clear() => this.innerDict.Clear();
  }
}
