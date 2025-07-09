// *********************************************************
// Type: Xbox.Live.Phone.Utils.ClosetAssetCategory
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using Microsoft.Xna.Framework.GamerServices.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml;


namespace Xbox.Live.Phone.Utils
{
  public class ClosetAssetCategory
  {
    public const string XmlName = "AssetCategory";
    private static Dictionary<Guid, ClosetAsset> guidToClosetAsset = new Dictionary<Guid, ClosetAsset>();
    private static Dictionary<string, AvatarManagerComponentCategories> categoryNameToType = (Dictionary<string, AvatarManagerComponentCategories>) null;
    private static Dictionary<AvatarManagerComponentCategories, AvatarManifestEditor.RemovableComponents> categoryTypeToRemovableComponent = (Dictionary<AvatarManagerComponentCategories, AvatarManifestEditor.RemovableComponents>) null;
    private List<ClosetAsset> assets;

    [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Declares dictionary contents.")]
    static ClosetAssetCategory()
    {
      ClosetAssetCategory.categoryNameToType = new Dictionary<string, AvatarManagerComponentCategories>();
      ClosetAssetCategory.categoryNameToType["Hair"] = AvatarManagerComponentCategories.Hair;
      ClosetAssetCategory.categoryNameToType["Eyes"] = AvatarManagerComponentCategories.Eyes;
      ClosetAssetCategory.categoryNameToType["Eyebrows"] = AvatarManagerComponentCategories.Eyebrows;
      ClosetAssetCategory.categoryNameToType["Ears"] = AvatarManagerComponentCategories.Ears;
      ClosetAssetCategory.categoryNameToType["Nose"] = AvatarManagerComponentCategories.Nose;
      ClosetAssetCategory.categoryNameToType["Chin"] = AvatarManagerComponentCategories.Chin;
      ClosetAssetCategory.categoryNameToType["Mouth"] = AvatarManagerComponentCategories.Mouth;
      ClosetAssetCategory.categoryNameToType["Facial Hair"] = AvatarManagerComponentCategories.FacialHair;
      ClosetAssetCategory.categoryNameToType["Facial other"] = AvatarManagerComponentCategories.FacialOther;
      ClosetAssetCategory.categoryNameToType["Shirt"] = AvatarManagerComponentCategories.Shirt;
      ClosetAssetCategory.categoryNameToType["Hat"] = AvatarManagerComponentCategories.Hat;
      ClosetAssetCategory.categoryNameToType["Trousers"] = AvatarManagerComponentCategories.Trousers;
      ClosetAssetCategory.categoryNameToType["Costume"] = AvatarManagerComponentCategories.Costume;
      ClosetAssetCategory.categoryNameToType["Shoes"] = AvatarManagerComponentCategories.Shoes;
      ClosetAssetCategory.categoryNameToType["Props"] = AvatarManagerComponentCategories.Carryable;
      ClosetAssetCategory.categoryNameToType["Glasses"] = AvatarManagerComponentCategories.Glasses;
      ClosetAssetCategory.categoryNameToType["Earrings"] = AvatarManagerComponentCategories.Earrings;
      ClosetAssetCategory.categoryNameToType["Wristwear"] = AvatarManagerComponentCategories.Wristwear;
      ClosetAssetCategory.categoryNameToType["Ring"] = AvatarManagerComponentCategories.Ring;
      ClosetAssetCategory.categoryNameToType["Gloves"] = AvatarManagerComponentCategories.Gloves;
      ClosetAssetCategory.categoryTypeToRemovableComponent = new Dictionary<AvatarManagerComponentCategories, AvatarManifestEditor.RemovableComponents>();
      ClosetAssetCategory.categoryTypeToRemovableComponent[AvatarManagerComponentCategories.Earrings] = AvatarManifestEditor.RemovableComponents.Earrings;
      ClosetAssetCategory.categoryTypeToRemovableComponent[AvatarManagerComponentCategories.Glasses] = AvatarManifestEditor.RemovableComponents.Glasses;
      ClosetAssetCategory.categoryTypeToRemovableComponent[AvatarManagerComponentCategories.Gloves] = AvatarManifestEditor.RemovableComponents.Gloves;
      ClosetAssetCategory.categoryTypeToRemovableComponent[AvatarManagerComponentCategories.Hat] = AvatarManifestEditor.RemovableComponents.Hat;
      ClosetAssetCategory.categoryTypeToRemovableComponent[AvatarManagerComponentCategories.Ring] = AvatarManifestEditor.RemovableComponents.Ring;
      ClosetAssetCategory.categoryTypeToRemovableComponent[AvatarManagerComponentCategories.Wristwear] = AvatarManifestEditor.RemovableComponents.Wristwear;
      ClosetAssetCategory.categoryTypeToRemovableComponent[AvatarManagerComponentCategories.FacialHair] = AvatarManifestEditor.RemovableComponents.FacialHair;
      ClosetAssetCategory.categoryTypeToRemovableComponent[AvatarManagerComponentCategories.FacialOther] = AvatarManifestEditor.RemovableComponents.SkinFeatures;
      ClosetAssetCategory.categoryTypeToRemovableComponent[AvatarManagerComponentCategories.Carryable] = AvatarManifestEditor.RemovableComponents.Prop;
    }

    private ClosetAssetCategory(AvatarManagerComponentCategories type)
    {
      this.Type = type;
      this.assets = new List<ClosetAsset>();
    }

    public AvatarManagerComponentCategories Type { get; private set; }

    public bool CanRemoveItem
    {
      get => this.RemovableComponent != AvatarManifestEditor.RemovableComponents.NotRemovable;
    }

    public AvatarManifestEditor.RemovableComponents RemovableComponent
    {
      get
      {
        AvatarManifestEditor.RemovableComponents removableComponent = AvatarManifestEditor.RemovableComponents.NotRemovable;
        if (ClosetAssetCategory.categoryTypeToRemovableComponent.ContainsKey(this.Type))
          removableComponent = ClosetAssetCategory.categoryTypeToRemovableComponent[this.Type];
        return removableComponent;
      }
    }

    public ReadOnlyCollection<ClosetAsset> Assets => this.assets.AsReadOnly();

    public static ClosetAssetCategory StockAssetCategory(XmlReader reader)
    {
      if (reader == null)
        throw new ArgumentException("reader is invalid");
      if (reader.NodeType != XmlNodeType.Element || !(reader.Name == "AssetCategory"))
        throw new XmlException();
      string attribute = reader.GetAttribute("name");
      ClosetAssetCategory parentCategory = new ClosetAssetCategory(ClosetAssetCategory.categoryNameToType[attribute]);
      reader.ReadToDescendant("Asset");
      do
      {
        parentCategory.AddAsset(ClosetAsset.StockAsset(reader, parentCategory));
      }
      while (reader.ReadToNextSibling("Asset"));
      return parentCategory;
    }

    public static ClosetAssetCategory NonStockAssetCategory(AvatarManagerComponentCategories type)
    {
      return new ClosetAssetCategory(type);
    }

    public static ClosetAsset GuidToAsset(Guid id)
    {
      return !ClosetAssetCategory.guidToClosetAsset.ContainsKey(id) ? (ClosetAsset) null : ClosetAssetCategory.guidToClosetAsset[id];
    }

    public void AddAsset(ClosetAsset asset)
    {
      if (asset == null)
        return;
      this.assets.Add(asset);
      ClosetAssetCategory.guidToClosetAsset[asset.Id] = asset;
    }

    public void SortByIndex()
    {
      this.assets.Sort(new Comparison<ClosetAsset>(ClosetAssetCategory.ClosetAssetComparison));
    }

    private static int ClosetAssetComparison(ClosetAsset lhs, ClosetAsset rhs)
    {
      return lhs.Index.CompareTo(rhs.Index);
    }
  }
}
