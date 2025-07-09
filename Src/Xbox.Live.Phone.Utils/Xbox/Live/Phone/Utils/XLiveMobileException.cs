// *********************************************************
// Type: Xbox.Live.Phone.Utils.XLiveMobileException
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xbox.Live.Phone.Utils.Resources;


namespace Xbox.Live.Phone.Utils
{
  [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "We expect certain additional information, so we only support the CreateException method.")]
  public class XLiveMobileException : XMobileException
  {
    private const string ComponentName = "XLiveMobileException";
    private static Dictionary<int, XLiveMobileException.ErrorInfo> errorInfoTable = new Dictionary<int, XLiveMobileException.ErrorInfo>()
    {
      {
        1000,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Terminate, XLiveMobileException.ErrorType.Fatal)
      },
      {
        1001,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        1002,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        1003,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        1004,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        2000,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        3000,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        3001,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        3002,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        3003,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        3004,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Ignore, XLiveMobileException.ErrorType.Warning)
      },
      {
        3005,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        4000,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        5000,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        6000,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        6001,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        6002,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        6003,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        6004,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        7000,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        7001,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        7002,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        8000,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        8001,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        8002,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        8003,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        8004,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        8100,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        8101,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        8102,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        8103,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Display, XLiveMobileException.ErrorType.Error)
      },
      {
        9000,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Ignore, XLiveMobileException.ErrorType.Error)
      },
      {
        9999,
        new XLiveMobileException.ErrorInfo(XLiveMobileException.ErrorAction.Terminate, XLiveMobileException.ErrorType.Fatal)
      }
    };
    private int statusCode;

    [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Initializes our error table.")]
    static XLiveMobileException()
    {
    }

    private XLiveMobileException(int code, string[] args)
      : base(code)
    {
      this.OperationCode = code;
      this.LookupErrorStringAndInfo(code, args);
    }

    public static XLiveMobileException LastException { get; private set; }

    public int OperationCode { get; private set; }

    public int StatusCode
    {
      get => this.statusCode;
      set
      {
        this.statusCode = value;
        this.StatusCodeMessage = Resource.ResourceManager.GetString("Status_Code_" + value.ToString((IFormatProvider) CultureInfo.InvariantCulture), Resource.Culture);
        if (!string.IsNullOrEmpty(this.StatusCodeMessage))
          return;
        this.StatusCodeMessage = this.StatusCode.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      }
    }

    public string StatusCodeMessage { get; set; }

    public uint ServiceErrorCode { get; set; }

    public string ServiceErrorMessage { get; set; }

    public override string Message
    {
      get
      {
        return string.Format((IFormatProvider) CultureInfo.CurrentUICulture, this.ErrorMessageFormat, new object[2]
        {
          (object) this.StatusCodeMessage,
          (object) this.ServiceErrorCode
        });
      }
    }

    public bool ShouldDisplay => this.Action != XLiveMobileException.ErrorAction.Ignore;

    public string ErrorMessageFormat { get; private set; }

    public XLiveMobileException.ErrorAction Action { get; set; }

    public XLiveMobileException.ErrorType Type { get; set; }

    public static XLiveMobileException CreateException(int code, string internalDescription)
    {
      return XLiveMobileException.CreateException((Exception) null, code, internalDescription, (string[]) null);
    }

    public static XLiveMobileException CreateException(
      int code,
      string internalDescription,
      string[] args)
    {
      return XLiveMobileException.CreateException((Exception) null, code, internalDescription, args);
    }

    public static XLiveMobileException CreateException(
      Exception innerException,
      int code,
      string internalDescription,
      string[] args)
    {
      XLiveMobileException exception = new XLiveMobileException(code, args);
      XLiveMobileException.LastException = exception;
      int action = (int) exception.Action;
      return exception;
    }

    [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "Only used by internal projects for simpler processing.")]
    [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "Only used by internal projects for simpler processing.")]
    public static void GetExceptionInfo(Exception exception, out string title, out string body)
    {
      if (exception == null)
        throw new ArgumentNullException(nameof (exception));
      if (exception is XLiveMobileException xliveMobileException)
      {
        title = Resource.Error_ErrorMessage_Title;
        body = xliveMobileException.Message;
      }
      else
      {
        title = Resource.Error_StandardException_Title;
        body = exception.Message;
      }
    }

    private void LookupErrorStringAndInfo(int code, string[] args)
    {
      string format = Resource.ResourceManager.GetString("Error_Code_" + code.ToString((IFormatProvider) CultureInfo.InvariantCulture), Resource.Culture);
      if (string.IsNullOrEmpty(format))
      {
        this.SetAsProgramingError(code, Resource.ProgrammingError_NoErrorStringInResource);
      }
      else
      {
        this.ErrorMessageFormat = args == null ? format : string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, (object[]) args);
        if (XLiveMobileException.errorInfoTable.ContainsKey(code))
        {
          this.Type = XLiveMobileException.errorInfoTable[code].Type;
          this.Action = XLiveMobileException.errorInfoTable[code].Action;
        }
        else
          this.SetAsProgramingError(code, Resource.ProgrammingError_InvalidTypeOrAction);
      }
    }

    private void SetAsProgramingError(int code, string format)
    {
      this.OperationCode = 9999;
      this.Type = XLiveMobileException.ErrorType.Fatal;
      this.Action = XLiveMobileException.ErrorAction.Terminate;
      this.ErrorMessageFormat = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, new object[1]
      {
        (object) code
      });
    }

    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "This enum is only used when using this parent class.")]
    public enum ErrorType
    {
      Warning,
      Error,
      Fatal,
    }

    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "This enum is only used when using this parent class.")]
    public enum ErrorAction
    {
      Ignore,
      Display,
      Terminate,
    }

    private class ErrorInfo
    {
      public ErrorInfo(XLiveMobileException.ErrorAction action, XLiveMobileException.ErrorType type)
      {
        this.Action = action;
        this.Type = type;
      }

      public XLiveMobileException.ErrorAction Action { get; set; }

      public XLiveMobileException.ErrorType Type { get; set; }
    }
  }
}
