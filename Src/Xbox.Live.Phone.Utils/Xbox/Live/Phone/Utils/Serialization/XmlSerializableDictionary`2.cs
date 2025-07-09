// *********************************************************
// Type: Xbox.Live.Phone.Utils.Serialization.XmlSerializableDictionary`2
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
  public class XmlSerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
  {
    public XmlSerializableDictionary()
    {
    }

    public XmlSerializableDictionary(IDictionary<TKey, TValue> dictionary)
    {
      foreach (TKey key in (IEnumerable<TKey>) dictionary.Keys)
        this.Add(key, dictionary[key]);
    }

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      if (reader.IsEmptyElement || !reader.Read())
        return;
      XmlSerializer xmlSerializer1 = new XmlSerializer(typeof (TKey));
      XmlSerializer xmlSerializer2 = new XmlSerializer(typeof (TValue));
      try
      {
        reader.ReadStartElement("items");
      }
      catch (XmlException ex)
      {
        return;
      }
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        try
        {
          reader.ReadStartElement("item");
        }
        catch (XmlException ex)
        {
          break;
        }
        bool flag = true;
        TKey key = default (TKey);
        TValue obj = default (TValue);
        try
        {
          key = (TKey) xmlSerializer1.Deserialize(reader);
        }
        catch (InvalidOperationException ex)
        {
          flag = false;
          reader.Skip();
        }
        try
        {
          obj = (TValue) xmlSerializer2.Deserialize(reader);
        }
        catch (InvalidOperationException ex)
        {
          flag = false;
          reader.Skip();
        }
        if (flag)
          this.Add(key, obj);
        try
        {
          reader.ReadEndElement();
          int content = (int) reader.MoveToContent();
        }
        catch (XmlException ex)
        {
          break;
        }
      }
      try
      {
        reader.ReadEndElement();
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
      XmlSerializer xmlSerializer1 = new XmlSerializer(typeof (TKey));
      XmlSerializer xmlSerializer2 = new XmlSerializer(typeof (TValue));
      writer.WriteStartElement("items");
      foreach (TKey key in this.Keys)
      {
        writer.WriteStartElement("item");
        xmlSerializer1.Serialize(writer, (object) key);
        TValue o = this[key];
        xmlSerializer2.Serialize(writer, (object) o);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
  }
}
