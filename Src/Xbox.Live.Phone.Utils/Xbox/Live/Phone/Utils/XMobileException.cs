// *********************************************************
// Type: Xbox.Live.Phone.Utils.XMobileException
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;


namespace Xbox.Live.Phone.Utils
{
  public class XMobileException : Exception
  {
    private const string ComponentName = "XMobileException";
    private string errorMessage;

    public XMobileException()
    {
    }

    public XMobileException(int code)
      : this((Exception) null, code, (string) null)
    {
    }

    public XMobileException(int code, string message)
      : this((Exception) null, code, message)
    {
    }

    public XMobileException(string message)
      : this((Exception) null, 0, message)
    {
    }

    public XMobileException(string message, Exception innerException)
      : this(innerException, 0, message)
    {
    }

    public XMobileException(Exception innerException, int code, string message)
    {
      this.ErrorCode = code;
      this.InnerException = innerException;
      this.errorMessage = message;
    }

    public int ErrorCode { get; private set; }

    public override string Message => this.errorMessage;

    public new Exception InnerException { get; private set; }
  }
}
