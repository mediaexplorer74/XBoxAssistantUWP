// *********************************************************
// Type: Xbox.Live.Phone.Utils.CircularBuffer`1
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll


namespace Xbox.Live.Phone.Utils
{
  public class CircularBuffer<T>
  {
    private T[] data;
    private int index;

    public CircularBuffer(int size) => this.data = new T[size];

    public int Count => this.data.Length;

    public T this[int i]
    {
      get => this.data[this.IndexToInnerIndex(i)];
      set => this.data[this.IndexToInnerIndex(i)] = value;
    }

    public void Add(T value)
    {
      this.index = (this.index + 1) % this.Count;
      this.data[this.index] = value;
    }

    private int IndexToInnerIndex(int i) => (i - this.index + this.Count) % this.Count;
  }
}
