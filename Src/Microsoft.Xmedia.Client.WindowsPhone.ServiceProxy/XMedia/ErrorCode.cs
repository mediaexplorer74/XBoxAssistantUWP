// *********************************************************
// Type: Microsoft.XMedia.ErrorCode
// Assembly: Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E3A1F77-365B-4EB2-85E1-D467924E2195
// *********************************************************Microsoft.Xmedia.Client.WindowsPhone.ServiceProxy.dll


namespace Microsoft.XMedia
{
  public enum ErrorCode
  {
    NoError,
    UnexpectedError,
    ExceptionEncountered,
    IncorrectVersionInSecurityToken,
    ExpiredDataInSecurityToken,
    ErrorDecodingFromBase64,
    ErrorWhileDeserializing,
    TooBigClaimIdInSerialize,
    NullOrEmptyAuthHeader,
    NullOrMissingContentInAuthHeader,
    MissingContentInSamlToken,
    IncorrectClaimId,
    IncorrectClaimValue,
    InputDataTooSmallInSecurityToken,
    IncorrectBlockSizeInSecurityToken,
    HashMismatchInSecurityToken,
    TooSmallDataLeftInSecurityToken,
    XblTokenClientCertificateDeviceIdMismatch,
    XblTokenIncorrectIssuerName,
    XblTokenFoundDeviceInfoForMobileType,
    XblTokenIncorrectStatementCount,
    XblTokenIncorrectStatementType,
    XblTokenIncorrectVersion,
    WindowsLiveErrorRetrievingUserInfo,
    DynamicThrottled,
    BadOrMissingContractVersionHeader,
    IncorrectServiceToServiceAuthText,
    TooManyClients,
    TooManyMessagesInQueue,
    CompanionNotSignedIn,
    CompanionSigninInProgress,
    InvalidIPAddressFormat,
    MaxGenerationInAuthToken,
    InvalidDeviceType,
    FailureFromUpdateOrCreateSession,
    IncorrectSessionToDelete,
    MaxErrorCodes,
  }
}
