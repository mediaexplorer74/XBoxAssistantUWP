// *********************************************************
// Type: Xbox.Live.Phone.Utils.AtomicWrapper`1
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll


namespace Xbox.Live.Phone.Utils
{
  public class AtomicWrapper<T>
  {
    private T data;

    public AtomicWrapper(T initial) => this.data = initial;

    public T Value
    {
      get
      {
        lock (this)
          return this.data;
      }
    }

    public bool TestAndSet(T testEquals, T setValue)
    {
      lock (this)
      {
        if (((object) this.data != null || (object) testEquals != null) && ((object) this.data == null || !this.data.Equals((object) testEquals)))
          return false;
        this.data = setValue;
        return true;
      }
    }

    public void ForceSet(T value)
    {
      lock (this)
        this.data = value;
    }
  }
}
