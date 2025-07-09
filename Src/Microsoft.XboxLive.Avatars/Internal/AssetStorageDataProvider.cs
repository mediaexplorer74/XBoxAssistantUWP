// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.AssetStorageDataProvider
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.Globalization;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public class AssetStorageDataProvider : IDataProvider
  {
    private static AssetLocalCache s_assetLocalCache;
    private string m_stockAssetAddressFormat;
    private string m_nonStockAssetAddressFormat;
    private string m_manifestServiceAddressFormat;

    internal AssetStorageDataProvider()
    {
      if (AssetStorageDataProvider.s_assetLocalCache != null)
        return;
      AssetStorageDataProvider.s_assetLocalCache = new AssetLocalCache();
    }

    public AssetStorageDataProvider(string manifestServiceAddressFormat)
    {
      this.m_manifestServiceAddressFormat = manifestServiceAddressFormat;
      if (AssetStorageDataProvider.s_assetLocalCache != null)
        return;
      AssetStorageDataProvider.s_assetLocalCache = new AssetLocalCache();
    }

    public AssetStorageDataProvider(
      string stockAssetAddressFormat,
      string nonStockAssetAddressFormat)
    {
      this.m_stockAssetAddressFormat = stockAssetAddressFormat;
      this.m_nonStockAssetAddressFormat = nonStockAssetAddressFormat;
      if (AssetStorageDataProvider.s_assetLocalCache != null)
        return;
      AssetStorageDataProvider.s_assetLocalCache = new AssetLocalCache();
    }

    internal AssetLocalCache LocalCache => AssetStorageDataProvider.s_assetLocalCache;

    public static string GetFullIdAddress(string addressFormat, string assetId)
    {
      string fullIdAddress = (string) null;
      try
      {
        uint hashCode = (uint) addressFormat.ToLower(CultureInfo.InvariantCulture).GetHashCode();
        fullIdAddress = Path.ChangeExtension(Path.GetFileName(assetId), hashCode.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
      catch (ArgumentException ex)
      {
      }
      return fullIdAddress;
    }

    internal static bool IncreaseCacheQuotaTo(long size)
    {
      return AssetStorageDataProvider.s_assetLocalCache != null && AssetStorageDataProvider.s_assetLocalCache.IncreaseQuotaTo(size);
    }

    public static void CleanCache() => AssetLocalCache.Clean();

    internal string GetAddressFormat(DataRequest request)
    {
      string addressFormat = (string) null;
      switch (request)
      {
        case DataRequestGuid _:
          addressFormat = this.GetAssetAddressFromGuid(((DataRequestGuid) request).DataId);
          break;
        case DataRequestString _:
          addressFormat = this.m_manifestServiceAddressFormat == null ? this.m_stockAssetAddressFormat : this.m_manifestServiceAddressFormat;
          break;
      }
      return addressFormat;
    }

    private string GetAssetAddressFromGuid(Guid avatarAssetGuid)
    {
      return !AssetLoader.IsStockAsset(avatarAssetGuid) ? this.m_nonStockAssetAddressFormat : this.m_stockAssetAddressFormat;
    }

    public void GetDataAsync(DataRequest request, DataProvider dataProvider)
    {
      string assetId = (string) null;
      switch (request)
      {
        case DataRequestGuid _:
          assetId = ((DataRequestGuid) request).DataId.ToString();
          break;
        case DataRequestString _:
          assetId = ((DataRequestString) request).DataId.ToString();
          break;
      }
      string fullIdAddress = AssetStorageDataProvider.GetFullIdAddress(this.GetAddressFormat(request), assetId);
      DataRequestCompletedEventArgs completedEventArgs;
      if (fullIdAddress == null)
        completedEventArgs = new DataRequestCompletedEventArgs((Exception) new ArgumentException("Invalid data request", nameof (request)), false, request.Context);
      if (AssetStorageDataProvider.s_assetLocalCache == null)
        completedEventArgs = new DataRequestCompletedEventArgs((Exception) new InvalidOperationException("Local cache is not initialized"), false, request.Context);
      long ticks = DateTime.Now.Ticks;
      Stream stream = AssetStorageDataProvider.s_assetLocalCache.TryOpen(fullIdAddress);
      DataRequestCompletedEventArgs eventArguments;
      if (stream != null)
      {
        eventArguments = new DataRequestCompletedEventArgs((Exception) null, false, request.Context);
        eventArguments.Result = stream;
      }
      else
        eventArguments = new DataRequestCompletedEventArgs((Exception) new InvalidOperationException("Failed to obtain resource."), false, request.Context);
      dataProvider.RequestProcessed(eventArguments, request);
    }

    public void CancelAsync()
    {
    }
  }
}
