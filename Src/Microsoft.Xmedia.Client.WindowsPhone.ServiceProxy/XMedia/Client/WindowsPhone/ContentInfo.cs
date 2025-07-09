// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.ContentInfo
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using Microsoft.XMedia.Service;
using System.Collections.Generic;
using System.IO;
using System.Json;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public class ContentInfo
  {
    public ContentInfo()
    {
      this.Id = "";
      this.Name = "";
      this.Description = "";
      this.PartnerMediaIds = new string[0];
      this.RootDomains = new string[0];
    }

    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string[] RootDomains { get; set; }

    public string[] PartnerMediaIds { get; set; }

    internal static ContentInfo FromJsonString(string jsonString)
    {
      return ContentInfo.ContentInfoFromContract(ContentInfo.ContentContractFromJson(jsonString));
    }

    internal static string ToJsonString(ContentInfo contentInfo)
    {
      return ContentInfo.ContentContractToJsonString(ContentInfo.ContentContractFromInfo(contentInfo));
    }

    internal static IEnumerable<ContentInfo> CollectionFromJsonStream(Stream jsonStream)
    {
      using (StreamReader streamReader = new StreamReader(jsonStream))
      {
        JsonArray jsonArray = JsonValue.Parse(streamReader.ReadToEnd()) is JsonObject jsonObject ? jsonObject.GetOptionalJsonArrayField("content") : throw new InvalidDataException("Invalid json data for ContentInfo collection.");
        if (jsonArray == null)
          throw new InvalidDataException("Invalid json data for ContentInfo collection.");
        List<ContentInfo> contentInfoList = new List<ContentInfo>();
        foreach (JsonValue jsonValue in (IEnumerable<JsonValue>) jsonArray)
        {
          if (!(jsonValue is JsonObject))
            throw new InvalidDataException("Invalid value in Activities array");
          contentInfoList.Add(ContentInfo.ContentInfoFromContract(ContentInfo.ContentContractFromJson(jsonValue as JsonObject)));
        }
        return (IEnumerable<ContentInfo>) contentInfoList;
      }
    }

    internal static IEnumerable<ContentInfo> CollectionFromJsonString(string jsonString)
    {
      JsonArray jsonArray = JsonValue.Parse(jsonString) is JsonObject jsonObject ? jsonObject.GetOptionalJsonArrayField("content") : throw new InvalidDataException("Invalid json data for ContentInfo collection.");
      if (jsonArray == null)
        throw new InvalidDataException("Invalid json data for ContentInfo collection.");
      List<ContentInfo> contentInfoList = new List<ContentInfo>();
      foreach (JsonValue jsonValue in (IEnumerable<JsonValue>) jsonArray)
      {
        if (!(jsonValue is JsonObject))
          throw new InvalidDataException("Invalid value in Activities array");
        contentInfoList.Add(ContentInfo.ContentInfoFromContract(ContentInfo.ContentContractFromJson(jsonValue as JsonObject)));
      }
      return (IEnumerable<ContentInfo>) contentInfoList;
    }

    private static Content ContentContractFromInfo(ContentInfo info)
    {
      return new Content()
      {
        ContentId = info.Id,
        Description = info.Description,
        Name = info.Name,
        RootDomains = info.RootDomains,
        PartnerMediaIds = info.PartnerMediaIds
      };
    }

    private static ContentInfo ContentInfoFromContract(Content contract)
    {
      return new ContentInfo()
      {
        Id = contract.ContentId,
        Description = contract.Description,
        Name = contract.Name,
        RootDomains = contract.RootDomains,
        PartnerMediaIds = contract.PartnerMediaIds
      };
    }

    private static string ContentContractToJsonString(Content contract)
    {
      JsonObject jsonObject = new JsonObject(new KeyValuePair<string, JsonValue>[0]);
      jsonObject["ContentId"] = (JsonValue) contract.ContentId;
      jsonObject["Description"] = (JsonValue) contract.Description;
      jsonObject["Name"] = (JsonValue) contract.Name;
      if (contract.RootDomains != null)
      {
        JsonArray jsonArray = new JsonArray(new JsonValue[0]);
        foreach (string rootDomain in contract.RootDomains)
          jsonArray.Add((JsonValue) new JsonPrimitive(rootDomain));
        jsonObject["RootDomains"] = (JsonValue) jsonArray;
      }
      if (contract.PartnerMediaIds != null)
      {
        JsonArray jsonArray = new JsonArray(new JsonValue[0]);
        foreach (string partnerMediaId in contract.PartnerMediaIds)
          jsonArray.Add((JsonValue) new JsonPrimitive(partnerMediaId));
        jsonObject["PartnerMediaIds"] = (JsonValue) jsonArray;
      }
      return jsonObject.ToString();
    }

    private static Content ContentContractFromJson(string json)
    {
      return ContentInfo.ContentContractFromJson(JsonValue.Parse(json) as JsonObject);
    }

    private static Content ContentContractFromJson(JsonObject jsonObject)
    {
      if (jsonObject == null)
        throw new InvalidDataException("Invalid Content JSON object");
      Content content = new Content();
      content.ContentId = jsonObject.GetRequiredStringField("contentId");
      content.Name = jsonObject.GetOptionalStringField("name");
      content.Description = jsonObject.GetOptionalStringField("description");
      JsonArray optionalJsonArrayField1 = jsonObject.GetOptionalJsonArrayField("rootDomains");
      if (optionalJsonArrayField1 != null)
      {
        content.RootDomains = new string[optionalJsonArrayField1.Count];
        for (int index = 0; index < optionalJsonArrayField1.Count; ++index)
          content.RootDomains[index] = (string) optionalJsonArrayField1[index];
      }
      JsonArray optionalJsonArrayField2 = jsonObject.GetOptionalJsonArrayField("partnerMediaIds");
      if (optionalJsonArrayField2 != null)
      {
        content.PartnerMediaIds = new string[optionalJsonArrayField2.Count];
        for (int index = 0; index < optionalJsonArrayField2.Count; ++index)
          content.PartnerMediaIds[index] = (string) optionalJsonArrayField2[index];
      }
      return content;
    }
  }
}
