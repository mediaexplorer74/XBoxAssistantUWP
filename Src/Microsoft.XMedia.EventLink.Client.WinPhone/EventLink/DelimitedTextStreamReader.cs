// *********************************************************
// Type: Microsoft.XMedia.EventLink.DelimitedTextStreamReader
// Assembly: Microsoft.XMedia.EventLink.Client.WinPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946994A4-3A3C-41D3-A520-1292D57CD5ED
// *********************************************************Microsoft.XMedia.EventLink.Client.WinPhone.dll

using System;
using System.IO;
using System.Text;


namespace Microsoft.XMedia.EventLink
{
  public class DelimitedTextStreamReader : IDisposable
  {
    private StreamReader reader;

    public DelimitedTextStreamReader(Stream stream, bool disposeStream = false)
    {
      this.reader = new StreamReader(disposeStream ? stream : (Stream) new NonClosingStream(stream), Encoding.UTF8);
    }

    public Stream BaseStream => this.reader.BaseStream;

    public bool EndOfStream => this.reader.Peek() == -1;

    public int ReadInt32()
    {
      int result;
      if (!int.TryParse(this.ReadToken(), out result))
        throw new InvalidDataException();
      return result;
    }

    public uint ReadUInt32()
    {
      uint result;
      if (!uint.TryParse(this.ReadToken(), out result))
        throw new InvalidDataException();
      return result;
    }

    public string ReadString()
    {
      int count = this.ReadInt32();
      if (count < 0)
        throw new InvalidDataException();
      if (count <= 0)
        return string.Empty;
      char[] buffer = new char[count];
      if (this.reader.ReadBlock(buffer, 0, count) != count)
        throw new InvalidDataException();
      return new string(buffer);
    }

    private string ReadToken()
    {
      StringBuilder stringBuilder = new StringBuilder();
      while (true)
      {
        int num = this.reader.Read();
        switch (num)
        {
          case -1:
            goto label_2;
          case 124:
            goto label_4;
          default:
            stringBuilder.Append((char) num);
            continue;
        }
      }
label_2:
      throw new InvalidDataException();
label_4:
      return stringBuilder.ToString();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this.reader == null)
        return;
      this.reader.Dispose();
      this.reader = (StreamReader) null;
    }
  }
}
