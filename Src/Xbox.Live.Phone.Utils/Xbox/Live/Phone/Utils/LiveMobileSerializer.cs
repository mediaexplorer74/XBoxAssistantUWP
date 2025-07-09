// *********************************************************
// Type: Xbox.Live.Phone.Utils.LiveMobileSerializer
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;


namespace Xbox.Live.Phone.Utils
{
  public static class LiveMobileSerializer
  {
    public static string SerializeToString(object obj)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        new XmlSerializer(obj.GetType()).Serialize((TextWriter) stringWriter, obj);
        return stringWriter.ToString();
      }
    }

    public static T DeserializeFromString<T>(string xml)
    {
      using (StringReader stringReader = new StringReader(xml))
        return (T) new XmlSerializer(typeof (T)).Deserialize((TextReader) stringReader);
    }
  }
}
