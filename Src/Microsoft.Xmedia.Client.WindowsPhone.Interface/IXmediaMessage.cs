// *********************************************************
// Type: Microsoft.Xmedia.Client.WindowsPhone.IXmediaMessage
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B6D5468-129C-4117-AE93-587315C3ADB1
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.Interface.dll

using System.IO;


namespace Microsoft.Xmedia.Client.WindowsPhone
{
  public interface IXmediaMessage : IXmediaMessageHeaders
  {
    uint GetSerializedSize();

    void Serialize(BinaryWriter writer);
  }
}
