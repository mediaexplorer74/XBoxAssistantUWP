// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.JsonObjectHelper
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll

using System.IO;
using System.Json;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public static class JsonObjectHelper
  {
    public static string GetRequiredStringField(this JsonObject jsonObject, string fieldName)
    {
      JsonValue requiredStringField;
      if (!jsonObject.TryGetValue(fieldName, out requiredStringField))
        throw new InvalidDataException("Json Object is missing" + fieldName);
      return (string) requiredStringField;
    }

    public static string GetOptionalStringField(this JsonObject jsonObject, string fieldName)
    {
      JsonValue optionalStringField = (JsonValue) null;
      jsonObject.TryGetValue(fieldName, out optionalStringField);
      return (string) optionalStringField;
    }

    public static JsonArray GetOptionalJsonArrayField(this JsonObject jsonObject, string arrayName)
    {
      JsonArray optionalJsonArrayField = (JsonArray) null;
      JsonValue jsonValue;
      if (jsonObject.TryGetValue(arrayName, out jsonValue) && jsonValue.JsonType == JsonType.Array)
        optionalJsonArrayField = jsonValue as JsonArray;
      return optionalJsonArrayField;
    }

    public static JsonObject GetOptionalJsonObjectField(
      this JsonObject jsonObject,
      string objectName)
    {
      JsonObject optionalJsonObjectField = (JsonObject) null;
      JsonValue jsonValue;
      if (jsonObject.TryGetValue(objectName, out jsonValue) && jsonValue.JsonType == JsonType.Object)
        optionalJsonObjectField = jsonValue as JsonObject;
      return optionalJsonObjectField;
    }
  }
}
