// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.NetDataProvider
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;
using System.Globalization;
using System.IO;


namespace Microsoft.XboxLive.Avatars.Internal
{
  public abstract class NetDataProvider
  {
    private string m_stockAssetAddressFormat;
    private string m_nonStockAssetAddressFormat;
    private string m_manifestServiceAddressFormat;

    protected NetDataProvider(
      string stockAssetAddressFormat,
      string nonStockAssetAddressFormat,
      string manifestServiceAddressFormat)
    {
      this.m_stockAssetAddressFormat = stockAssetAddressFormat;
      this.m_nonStockAssetAddressFormat = nonStockAssetAddressFormat;
      this.m_manifestServiceAddressFormat = manifestServiceAddressFormat;
    }

    protected static Stream DuplicateStream(Stream source, bool seekToBegin)
    {
      lock (source)
      {
        Stream stream = (Stream) new MemoryStream();
        byte[] buffer = new byte[32768];
        if (seekToBegin)
          source.Seek(0L, SeekOrigin.Begin);
        while (true)
        {
          int count = source.Read(buffer, 0, buffer.Length);
          if (count > 0)
            stream.Write(buffer, 0, count);
          else
            break;
        }
        stream.Seek(0L, SeekOrigin.Begin);
        return stream;
      }
    }

    private string MakeStockAssetAddress(string asset)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, this.m_stockAssetAddressFormat, new object[1]
      {
        (object) asset
      });
    }

    private string MakeNonStockAssetAddress(string asset, string titleId)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, this.m_nonStockAssetAddressFormat, new object[2]
      {
        (object) titleId,
        (object) asset
      });
    }

    private string MakeManifestAddress(string gamertag)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, this.m_manifestServiceAddressFormat, new object[1]
      {
        (object) gamertag
      });
    }

    private string MakeAssetAddressFromGuid(Guid avatarAssetGuid)
    {
      return (!AssetLoader.IsStockAsset(avatarAssetGuid) ? this.MakeNonStockAssetAddress(avatarAssetGuid.ToString(), AssetLoader.GetTitleIdFromAssetId(avatarAssetGuid)) : this.MakeStockAssetAddress(avatarAssetGuid.ToString())).ToLower(CultureInfo.InvariantCulture);
    }

    protected string GetAddress(DataRequest request)
    {
      string address = (string) null;
      switch (request)
      {
        case DataRequestGuid _:
          address = this.MakeAssetAddressFromGuid((request as DataRequestGuid).DataId);
          break;
        case DataRequestString _:
          DataRequestString dataRequestString = request as DataRequestString;
          address = this.m_manifestServiceAddressFormat == null ? this.MakeStockAssetAddress(dataRequestString.DataId) : this.MakeManifestAddress(dataRequestString.DataId);
          break;
      }
      return address;
    }

    internal string GetAddressFormat(DataRequest request)
    {
      string addressFormat = (string) null;
      switch (request)
      {
        case DataRequestGuid _:
          addressFormat = this.GetAssetAddressFromGuid((request as DataRequestGuid).DataId);
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
  }
}
