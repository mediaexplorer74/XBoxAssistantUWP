// *********************************************************
// Type: Xbox.Live.Phone.Utils.JsonHelper
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.IO;
using System.Json;
using System.Runtime.Serialization.Json;


namespace Xbox.Live.Phone.Utils
{
  public static class JsonHelper
  {
    public static JsonObject TryGetJsonObjectValueForKey(JsonObject jsonObject, string key)
    {
      JsonObject objectValueForKey = (JsonObject) null;
      if (jsonObject != null && jsonObject.ContainsKey(key))
        objectValueForKey = jsonObject[key] as JsonObject;
      return objectValueForKey;
    }

    public static JsonArray TryGetJsonArrayValueForKey(JsonObject jsonObject, string key)
    {
      JsonArray arrayValueForKey = (JsonArray) null;
      if (jsonObject != null && jsonObject.ContainsKey(key))
        arrayValueForKey = jsonObject[key] as JsonArray;
      return arrayValueForKey;
    }

    public static string TryGetStringValueForKey(JsonObject jsonObject, string key)
    {
      string stringValueForKey = (string) null;
      if (jsonObject != null && jsonObject.ContainsKey(key))
      {
        JsonValue jsonValue = jsonObject[key];
        if (jsonValue != null && jsonValue.JsonType == JsonType.String)
          stringValueForKey = (string) jsonValue;
      }
      return stringValueForKey;
    }

    public static ulong? TryGetUnsignedLongValueForKey(JsonObject jsonObject, string key)
    {
      ulong? unsignedLongValueForKey = new ulong?();
      if (jsonObject != null && jsonObject.ContainsKey(key))
      {
        JsonValue jsonValue = jsonObject[key];
        if (jsonValue != null)
        {
          if (jsonValue.JsonType == JsonType.Number)
          {
            try
            {
              unsignedLongValueForKey = new ulong?((ulong) jsonValue);
            }
            catch (OverflowException ex)
            {
            }
          }
        }
      }
      return unsignedLongValueForKey;
    }

    public static uint? TryGetUnsignedIntValueForKey(JsonObject jsonObject, string key)
    {
      uint? unsignedIntValueForKey = new uint?();
      if (jsonObject != null && jsonObject.ContainsKey(key))
      {
        JsonValue jsonValue = jsonObject[key];
        if (jsonValue != null)
        {
          if (jsonValue.JsonType == JsonType.Number)
          {
            try
            {
              unsignedIntValueForKey = new uint?((uint) jsonValue);
            }
            catch (OverflowException ex)
            {
            }
          }
        }
      }
      return unsignedIntValueForKey;
    }

    public static int? TryGetIntValueForKey(JsonObject jsonObject, string key)
    {
      int? intValueForKey = new int?();
      if (jsonObject != null && jsonObject.ContainsKey(key))
      {
        JsonValue jsonValue = jsonObject[key];
        if (jsonValue != null)
        {
          if (jsonValue.JsonType == JsonType.Number)
          {
            try
            {
              intValueForKey = new int?((int) jsonValue);
            }
            catch (OverflowException ex)
            {
            }
          }
        }
      }
      return intValueForKey;
    }

    public static bool? TryGetBooleanValueForKey(JsonObject jsonObject, string key)
    {
      bool? booleanValueForKey = new bool?();
      if (jsonObject != null && jsonObject.ContainsKey(key))
      {
        JsonValue jsonValue = jsonObject[key];
        if (jsonValue != null && jsonValue.JsonType == JsonType.Boolean)
          booleanValueForKey = new bool?((bool) jsonValue);
      }
      return booleanValueForKey;
    }

    public static string Serialize<T>(T dataObject)
    {
      MemoryStream memoryStream = (MemoryStream) null;
      try
      {
        memoryStream = new MemoryStream();
        new DataContractJsonSerializer(typeof (T)).WriteObject((Stream) memoryStream, (object) dataObject);
        memoryStream.Position = 0L;
        using (StreamReader streamReader = new StreamReader((Stream) memoryStream))
        {
          memoryStream = (MemoryStream) null;
          return streamReader.ReadToEnd();
        }
      }
      finally
      {
        memoryStream?.Dispose();
      }
    }
  }
}
