// *********************************************************
// Type: Xbox.Live.Phone.ClosetAsset
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Xbox.Live.Phone.Services.Resources;
using Xbox.Live.Phone.Utils;


namespace Xbox.Live.Phone
{
  public class ClosetAsset
  {
    public const string XmlName = "Asset";

    public ClosetAsset(
      ClosetAssetCategory parentCategory,
      Guid id,
      int index,
      bool useAssetColors,
      string description,
      List<Color> colors)
    {
      this.ParentCategory = parentCategory;
      this.Id = id;
      this.Index = index;
      this.UseAssetColors = useAssetColors;
      this.Description = description;
      this.Colors = colors;
    }

    public Guid Id { get; private set; }

    public int Index { get; private set; }

    public List<Color> Colors { get; private set; }

    public bool UseAssetColors { get; private set; }

    public ClosetAssetCategory ParentCategory { get; private set; }

    public bool Remove => this.Id.Equals(Guid.Empty);

    public string Description { get; private set; }

    public static ClosetAsset StockAsset(XmlReader reader, ClosetAssetCategory parentCategory)
    {
      if (reader == null)
        throw new ArgumentException("reader is invalid");
      if (reader.NodeType != XmlNodeType.Element || !(reader.Name == "Asset"))
        throw new XmlException();
      Guid id = new Guid(reader.GetAttribute("id"));
      int index = int.Parse(reader.GetAttribute("index"), (IFormatProvider) CultureInfo.InvariantCulture);
      bool useAssetColors = bool.Parse(reader.GetAttribute("colorSupported"));
      reader.ReadToDescendant("Description");
      string description = reader.ReadElementContentAsString("Description", reader.NamespaceURI);
      List<Color> colors = (List<Color>) null;
      while (reader.IsStartElement("Color"))
      {
        string attribute = reader.GetAttribute("rgb");
        if (attribute.Length != 7)
          throw new InvalidOperationException(Resource.ClosetReadColor);
        if (colors == null)
          colors = new List<Color>();
        byte r = byte.Parse(attribute.Substring(1, 2), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
        byte g = byte.Parse(attribute.Substring(3, 2), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
        byte b = byte.Parse(attribute.Substring(5, 2), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
        colors.Add(new Color((int) r, (int) g, (int) b));
        reader.ReadToNextSibling("Color");
      }
      return new ClosetAsset(parentCategory, id, index, useAssetColors, description, colors);
    }

    public static ClosetAsset NonStockAsset(
      ClosetAssetCategory parentCategory,
      Guid id,
      string description)
    {
      return new ClosetAsset(parentCategory, id, int.MaxValue, false, description, (List<Color>) null);
    }
  }
}
