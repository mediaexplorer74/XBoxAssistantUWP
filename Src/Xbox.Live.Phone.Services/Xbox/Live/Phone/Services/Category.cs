// *********************************************************
// Type: Xbox.Live.Phone.Services.Category
// Assembly: Xbox.Live.Phone.Services, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 49E76159-45B0-4CC6-9C8B-7A1E120063F4
// *********************************************************Xbox.Live.Phone.Services.dll

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace Xbox.Live.Phone.Services
{
  [DataContract]
  public class Category : Product
  {
    [DataMember]
    public int ParentCateogryId { get; set; }

    [DataMember]
    public int CategoryId { get; set; }

    [DataMember]
    public bool HasSubCategories { get; set; }

    [XmlIgnore]
    [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "There is no need to create another class to hold just the list")]
    public Dictionary<int, List<Category>> SubCategories { get; set; }

    [DataMember]
    public int TotalAvailableSubCategories { get; set; }

    [XmlIgnore]
    public int TotalRequestedSubCategories { get; set; }
  }
}
