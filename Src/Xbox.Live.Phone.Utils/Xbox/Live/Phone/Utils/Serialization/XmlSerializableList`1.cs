// *********************************************************
// Type: Xbox.Live.Phone.Utils.Serialization.XmlSerializableList`1
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;


namespace Xbox.Live.Phone.Utils.Serialization
{
  public class XmlSerializableList<T> : List<T>, IXmlSerializable
  {
    public XmlSerializableList()
    {
    }

    public XmlSerializableList(List<T> list)
    {
      if (list == null)
        throw new ArgumentNullException(nameof (list));
      foreach (T obj in list)
        this.Add(obj);
    }

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      if (reader.IsEmptyElement || !reader.Read())
        return;
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
      reader.ReadStartElement("items");
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        this.Add((T) xmlSerializer.Deserialize(reader));
        int content = (int) reader.MoveToContent();
      }
      reader.ReadEndElement();
      try
      {
        reader.ReadEndElement();
      }
      catch (XmlException ex)
      {
      }
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (this.Count == 0)
        return;
      writer.WriteStartElement("items");
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
      foreach (T o in (List<T>) this)
        xmlSerializer.Serialize(writer, (object) o);
      writer.WriteEndElement();
    }
  }
}
