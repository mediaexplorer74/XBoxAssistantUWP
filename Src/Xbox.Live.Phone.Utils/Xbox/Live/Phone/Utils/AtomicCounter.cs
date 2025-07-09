// *********************************************************
// Type: Xbox.Live.Phone.Utils.AtomicCounter
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll


namespace Xbox.Live.Phone.Utils
{
  public class AtomicCounter
  {
    private int counter;

    public AtomicCounter(int initial) => this.counter = initial;

    public int Count
    {
      get
      {
        lock (this)
          return this.counter;
      }
    }

    public int Plus(int incremental)
    {
      lock (this)
      {
        this.counter += incremental;
        return this.counter;
      }
    }

    public int Subtract(int decremental)
    {
      lock (this)
      {
        this.counter -= decremental;
        return this.counter;
      }
    }
  }
}
