// *********************************************************
// Type: Xbox.Live.Phone.Utils.XLiveMobileStatusCodeEnum
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll


namespace Xbox.Live.Phone.Utils
{
  public enum XLiveMobileStatusCodeEnum
  {
    NoError = 0,
    NameResolutionFailure = 1,
    ConnectionFailure = 2,
    ReceiveFailure = 3,
    SendFailure = 4,
    PipelineFailure = 5,
    RequestCanceled = 6,
    ProtocolError = 7,
    ConnectionClosed = 8,
    TrustFailure = 9,
    SecureChannelFailure = 10, // 0x0000000A
    ServerProtocolViolation = 11, // 0x0000000B
    KeepAliveFailure = 12, // 0x0000000C
    Pending = 13, // 0x0000000D
    Timeout = 14, // 0x0000000E
    ProxyNameResolutionFailure = 15, // 0x0000000F
    UnknownError = 16, // 0x00000010
    VersionMismatch = 20, // 0x00000014
    NotFound = 21, // 0x00000015
    InternalError = 99, // 0x00000063
  }
}
