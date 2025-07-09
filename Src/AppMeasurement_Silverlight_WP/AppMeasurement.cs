// *********************************************************
// Type: com.omniture.AppMeasurement
// Assembly: AppMeasurement_Silverlight_WP, Version=1.3.7.0, Culture=neutral, PublicKeyToken=null
// MVID: B2938048-604D-4B3E-B432-7854B0CBA8DA
// *********************************************************AppMeasurement_Silverlight_WP.dll

using Microsoft.Phone.Info;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Threading;
using System.Windows;


namespace com.omniture
{
  public class AppMeasurement : AppMeasurement_Variables
  {
    public string version = "CS-1.3.7";
    public string csTarget = "WP";
    public AppMeasurement.DoPlugins doPlugins;
    public bool usePlugins;
    public AppMeasurement.DoRequest doRequest;
    private List<string> requestList;
    private Dictionary<int, Thread> requestThreads;
    private int requestThreadID;
    public int maxRequestThreads = 1;
    public int maxRequestsPerThread = 50;
    public bool offline;
    public object root;
    public AppMeasurement_Module_Media Media;
    public bool sendFromServer = true;
    private bool _1_referrer;
    private static AppMeasurement.PersistentStorage _storage;
    private string cookieDomain;
    private Dictionary<string, string> requestCookies;
    private Random rand;

    private bool _hasDoPlugins() => this.doPlugins != null;

    private void _doPlugins() => this.doPlugins(this);

    private bool _hasDoRequest() => this.doRequest != null;

    private bool _doRequest(string url, Dictionary<string, string> headers)
    {
      return this.doRequest(this, url, headers);
    }

    public void forceOffline()
    {
      AppMeasurement appMeasurement = this;
      appMeasurement.offline = true;
      if (appMeasurement.requestList == null)
        return;
      lock (appMeasurement.requestList)
        Monitor.Pulse((object) appMeasurement.requestList);
    }

    public void forceOnline()
    {
      AppMeasurement appMeasurement = this;
      appMeasurement.offline = false;
      if (appMeasurement.requestList == null)
        return;
      lock (appMeasurement.requestList)
        Monitor.Pulse((object) appMeasurement.requestList);
    }

    public void setInterface(object inter)
    {
      AppMeasurement appMeasurement = this;
      appMeasurement.root = inter;
      appMeasurement.modulesUpdate();
    }

    private void setup()
    {
      AppMeasurement s = this;
      s.contextData = new Dictionary<string, object>();
      s.retrieveLightData = new Dictionary<string, object>();
      s.charSet = "UTF-8";
      string pageUrl = s.getPageURL();
      if (s.isSet(pageUrl) && pageUrl.Length >= 6 && s.getPageURL().ToLower().Substring(0, 6) == "https:")
        s.ssl = true;
      else
        s.ssl = false;
      s.userAgent = s.getDefaultUserAgent();
      s.acceptLanguage = s.getDefaultAcceptLanguage();
      s.xForwardedFor = s.getDefaultXForwardedFor();
      s.visitorID = s.getDefaultVisitorID();
      s.requestCookiesFilename = "AppMeasurement.cookies";
      s.offlineFilename = "AppMeasurement.offline";
      s.Media = new AppMeasurement_Module_Media(s);
    }

    public AppMeasurement() => this.setup();

    private void modulesUpdate()
    {
      AppMeasurement appMeasurement = this;
      appMeasurement.Media.autoTrack = appMeasurement.Media.autoTrack;
    }

    public void logDebug(string msg)
    {
      this.callJavaScript("function s_logDebug(){var e;try{console.log(\"" + this.replace(this.replace(this.replace(msg, "\\", "\\\\"), "\n", "\\n"), "\"", "\\\"") + "\");}catch(e){}}s_logDebug();");
    }

    public bool isSet(bool val) => val;

    public bool isSet(int val) => val != 0;

    public bool isSet(double val) => val != 0.0;

    public bool isSet(string val)
    {
      switch (val)
      {
        case null:
          return false;
        case "":
          return false;
        default:
          return true;
      }
    }

    public bool isSet(object val)
    {
      switch (val)
      {
        case null:
          return false;
        case bool val1:
          return this.isSet(val1);
        case int val2:
          return this.isSet(val2);
        case double val3:
          return this.isSet(val3);
        case string _:
          return this.isSet((string) val);
        default:
          return true;
      }
    }

    public bool isNumber(int num) => true;

    public bool isNumber(string num)
    {
      if (!this.isSet(num))
        return false;
      try
      {
        if (!int.TryParse(num, out int _))
          return false;
      }
      catch
      {
        return false;
      }
      return true;
    }

    public string replace(string x, string o, string n)
    {
      string val = x;
      if (this.isSet(val))
      {
        if (o == "\r" && val.IndexOf('\r') >= 0)
          val = string.Join(n, val.Split('\r'));
        else if (o == "\n" && val.IndexOf('\n') >= 0)
          val = string.Join(n, val.Split('\n'));
        else if (o == "\t" && val.IndexOf('\t') >= 0)
          val = string.Join(n, val.Split('\t'));
        else if (val.IndexOf(o) >= 0)
          val = val.Replace(o, n);
      }
      return val;
    }

    public static string escape(string str) => Uri.EscapeDataString(str);

    public static string unescape(string str) => Uri.UnescapeDataString(str);

    public string md5(string str) => "";

    public double getTime()
    {
      return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
    }

    public string getUID() => this.replace(Guid.NewGuid().ToString(), "-", ".");

    private AppMeasurement.PersistentStorage persistentDataGetStorage()
    {
      if (AppMeasurement._storage == null)
      {
        lock (typeof (AppMeasurement))
        {
          if (AppMeasurement._storage == null)
          {
            AppMeasurement._storage = new AppMeasurement.PersistentStorage();
            try
            {
              AppMeasurement._storage.device = IsolatedStorageFile.GetUserStoreForApplication();
            }
            catch
            {
            }
            if (AppMeasurement._storage.device == null)
              AppMeasurement._storage = (AppMeasurement.PersistentStorage) null;
          }
        }
      }
      return AppMeasurement._storage;
    }

    private bool persistentDataExists(string id)
    {
      AppMeasurement.PersistentStorage storage = this.persistentDataGetStorage();
      if (storage != null)
      {
        try
        {
          string[] fileNames;
          if ((fileNames = storage.device.GetFileNames(id)) != null)
          {
            if (fileNames.Length > 0)
            {
              for (int index = 0; index < fileNames.Length; ++index)
              {
                if (fileNames[index] == id)
                  return true;
              }
            }
          }
        }
        catch
        {
        }
      }
      return false;
    }

    private string persistentDataRead(string id)
    {
      string str = (string) null;
      AppMeasurement.PersistentStorage storage = this.persistentDataGetStorage();
      if (storage != null && this.persistentDataExists(id))
      {
        StreamReader streamReader = (StreamReader) null;
        IsolatedStorageFileStream storageFileStream = (IsolatedStorageFileStream) null;
        try
        {
          storageFileStream = new IsolatedStorageFileStream(id, FileMode.Open, FileAccess.Read, storage.device);
        }
        catch
        {
        }
        if (storageFileStream != null)
        {
          try
          {
            streamReader = new StreamReader((Stream) storageFileStream);
          }
          catch
          {
          }
          if (streamReader != null)
          {
            try
            {
              str = streamReader.ReadToEnd();
            }
            catch
            {
              str = (string) null;
            }
            try
            {
              streamReader.Close();
            }
            catch
            {
            }
          }
        }
        if (storageFileStream != null)
        {
          try
          {
            storageFileStream.Close();
          }
          catch
          {
          }
        }
      }
      return str;
    }

    private bool persistentDataWrite(string id, string data)
    {
      bool flag = false;
      AppMeasurement.PersistentStorage storage = this.persistentDataGetStorage();
      if (storage != null)
      {
        StreamWriter streamWriter = (StreamWriter) null;
        IsolatedStorageFileStream storageFileStream = (IsolatedStorageFileStream) null;
        try
        {
          storageFileStream = new IsolatedStorageFileStream(id, FileMode.Create, FileAccess.Write, storage.device);
        }
        catch
        {
        }
        if (storageFileStream != null)
        {
          try
          {
            streamWriter = new StreamWriter((Stream) storageFileStream);
          }
          catch
          {
          }
          if (streamWriter != null)
          {
            try
            {
              streamWriter.Write(data);
              flag = true;
            }
            catch
            {
            }
            try
            {
              streamWriter.Close();
            }
            catch
            {
            }
          }
        }
        if (storageFileStream != null)
        {
          try
          {
            storageFileStream.Close();
          }
          catch
          {
          }
        }
      }
      return flag;
    }

    private void persistentDataDelete(string id)
    {
      AppMeasurement.PersistentStorage storage = this.persistentDataGetStorage();
      if (storage == null)
        return;
      if (!this.persistentDataExists(id))
        return;
      try
      {
        storage.device.DeleteFile(id);
      }
      catch
      {
      }
    }

    public string callJavaScript(string script) => "";

    private string getCookieDomain()
    {
      AppMeasurement appMeasurement = this;
      string str = "";
      if (appMeasurement.cookieDomain != null)
        return appMeasurement.cookieDomain;
      int num = appMeasurement.cookieDomainPeriods;
      if (num < 2)
        num = 2;
      appMeasurement.cookieDomain = "";
      string[] strArray = str.Split('.');
      if (strArray.Length > num)
      {
        for (int index = strArray.Length - 1; index >= 0 && num > 0; --num)
        {
          appMeasurement.cookieDomain = "." + strArray[index] + appMeasurement.cookieDomain;
          --index;
        }
      }
      return appMeasurement.cookieDomain;
    }

    public string getCookie(string key) => (string) null;

    private bool _setCookie(string key, string value, DateTime expire, bool useExpire)
    {
      AppMeasurement appMeasurement = this;
      if (useExpire && appMeasurement.isSet(appMeasurement.cookieLifetime))
      {
        if (appMeasurement.cookieLifetime.ToUpper() == "NONE")
          return false;
        if (appMeasurement.cookieLifetime.ToUpper() == "SESSION")
        {
          useExpire = false;
        }
        else
        {
          int result;
          if (int.TryParse(appMeasurement.cookieLifetime, out result))
            expire = DateTime.Now.AddSeconds((double) result);
        }
      }
      return appMeasurement.getCookie(key) == value;
    }

    public bool setCookie(string key, string value, DateTime expire)
    {
      return this._setCookie(key, value, expire, true);
    }

    public bool setCookie(string key, string value)
    {
      return this._setCookie(key, value, DateTime.Now, false);
    }

    public string getQueryVar(string key) => (string) null;

    public string getPostVar(string key) => (string) null;

    private HttpWebRequest requestConnect(string url)
    {
      try
      {
        return (HttpWebRequest) WebRequest.Create(new Uri(url));
      }
      catch
      {
      }
      return (HttpWebRequest) null;
    }

    private void requestCookiesRead()
    {
      AppMeasurement appMeasurement = this;
      string str1 = appMeasurement.persistentDataRead(appMeasurement.requestCookiesFilename);
      if (str1 == null)
        return;
      string[] strArray1 = str1.Split('\n');
      for (int index = 0; index < strArray1.Length; ++index)
      {
        if (appMeasurement.isSet(strArray1[index]))
        {
          string[] strArray2 = strArray1[index].Split('\t');
          string key = strArray2[0];
          string str2 = strArray2[1];
          if (appMeasurement.requestCookies == null)
            appMeasurement.requestCookies = new Dictionary<string, string>();
          appMeasurement.requestCookies[key] = str2;
        }
      }
    }

    private void requestCookiesWrite()
    {
      AppMeasurement appMeasurement = this;
      if (appMeasurement.requestCookies == null)
        return;
      string data = "";
      foreach (KeyValuePair<string, string> requestCookie in this.requestCookies)
        data = data + requestCookie.Key + "\t" + requestCookie.Value + "\n";
      if (appMeasurement.persistentDataWrite(appMeasurement.requestCookiesFilename, data))
        return;
      appMeasurement.requestCookies = (Dictionary<string, string>) null;
    }

    private void requestCookiesDelete()
    {
      AppMeasurement appMeasurement = this;
      appMeasurement.persistentDataDelete(appMeasurement.requestCookiesFilename);
      appMeasurement.requestCookies = (Dictionary<string, string>) null;
    }

    private void requestCookiesAddToConnection(HttpWebRequest connection)
    {
      AppMeasurement appMeasurement = this;
      try
      {
        if (appMeasurement.requestCookies == null)
          appMeasurement.requestCookiesRead();
        connection.CookieContainer = new CookieContainer();
        if (appMeasurement.requestCookies == null)
          return;
        foreach (KeyValuePair<string, string> requestCookie in appMeasurement.requestCookies)
          connection.CookieContainer.Add(new Uri("http://" + connection.RequestUri.Host + "/"), new Cookie()
          {
            Name = requestCookie.Key,
            Value = requestCookie.Value
          });
      }
      catch
      {
      }
    }

    private void requestCookiesSet(string key, string value)
    {
      AppMeasurement appMeasurement = this;
      bool flag = false;
      try
      {
        if (appMeasurement.requestCookies == null)
          appMeasurement.requestCookiesRead();
        if (appMeasurement.requestCookies == null)
          appMeasurement.requestCookies = new Dictionary<string, string>();
        if (appMeasurement.isSet(value))
        {
          if (appMeasurement.requestCookies.ContainsKey(key))
          {
            if (!(appMeasurement.requestCookies[key] != value))
              goto label_13;
          }
          appMeasurement.requestCookies[key] = value;
          flag = true;
        }
        else if (appMeasurement.requestCookies.ContainsKey(key))
        {
          appMeasurement.requestCookies.Remove(key);
          flag = true;
        }
      }
      catch
      {
      }
label_13:
      if (!flag)
        return;
      if (appMeasurement.requestCookies.Count > 0)
        this.requestCookiesWrite();
      else
        this.requestCookiesDelete();
    }

    private bool requestRequest(string request)
    {
      AppMeasurement appMeasurement = this;
      HttpWebResponse response = (HttpWebResponse) null;
      bool flag = false;
      string[] strArray = request.Split('\t');
      if (strArray.Length > 0 && strArray[0].Length > 0)
      {
        string str1 = strArray[0];
        while (appMeasurement.isSet(str1))
        {
          HttpWebRequest httpWebRequest;
          try
          {
            httpWebRequest = this.requestConnect(str1);
            str1 = "";
            httpWebRequest.AllowAutoRedirect = false;
          }
          catch
          {
            httpWebRequest = (HttpWebRequest) null;
          }
          if (httpWebRequest != null)
          {
            for (int index = 1; index < strArray.Length; index += 2)
            {
              string name = strArray[index];
              if (name != null && name != "")
              {
                if (index < strArray.Length - 1)
                {
                  try
                  {
                    string str2 = strArray[index + 1];
                    if (str2 != null)
                    {
                      if (str2 != "")
                      {
                        switch (name)
                        {
                          case "User-Agent":
                            httpWebRequest.UserAgent = str2;
                            continue;
                          case "Accept-Language":
                            continue;
                          default:
                            httpWebRequest.Headers[name] = str2;
                            continue;
                        }
                      }
                    }
                  }
                  catch
                  {
                  }
                }
              }
            }
            appMeasurement.requestCookiesAddToConnection(httpWebRequest);
            try
            {
              response = (HttpWebResponse) null;
              ManualResetEvent asyncLock = new ManualResetEvent(false);
              bool timedout = false;
              httpWebRequest.BeginGetResponse((AsyncCallback) (asynchronousResult =>
              {
                try
                {
                  if (!timedout)
                    response = (HttpWebResponse) ((WebRequest) asynchronousResult.AsyncState).EndGetResponse(asynchronousResult);
                }
                catch
                {
                  response = (HttpWebResponse) null;
                }
                asyncLock.Set();
              }), (object) httpWebRequest);
              timedout = !asyncLock.WaitOne(5000);
              if (timedout)
              {
                httpWebRequest.Abort();
                response = (HttpWebResponse) null;
              }
              if (response != null)
                flag = true;
            }
            catch
            {
            }
          }
          if (response != null)
          {
            try
            {
              if (response.Headers != null)
              {
                if (appMeasurement.isSet(response.Headers["Location"]))
                  str1 = response.Headers["Location"];
                foreach (Cookie cookie in httpWebRequest.CookieContainer.GetCookies(new Uri("http://" + httpWebRequest.RequestUri.Host + "/")))
                  appMeasurement.requestCookiesSet(cookie.Name, cookie.Value);
              }
              response.Close();
            }
            catch
            {
            }
          }
        }
      }
      return flag;
    }

    private void offlineRequestListRead()
    {
      AppMeasurement appMeasurement = this;
      if (appMeasurement.offlineFilename == null)
        return;
      string str = appMeasurement.persistentDataRead(appMeasurement.offlineFilename);
      appMeasurement.persistentDataDelete(appMeasurement.offlineFilename);
      if (str == null)
        return;
      string[] strArray = str.Split('\n');
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (appMeasurement.isSet(strArray[index]))
        {
          lock (appMeasurement.requestList)
            appMeasurement.requestList.Add(strArray[index]);
        }
      }
    }

    private void offlineRequestListWrite()
    {
      AppMeasurement appMeasurement = this;
      if (appMeasurement.offlineFilename == null)
        return;
      string data = "";
      for (int index = 0; index < appMeasurement.requestList.Count; ++index)
        data = data + appMeasurement.requestList[index] + "\n";
      appMeasurement.persistentDataWrite(appMeasurement.offlineFilename, data);
    }

    private void offlineRequestListDelete()
    {
      AppMeasurement appMeasurement = this;
      appMeasurement.persistentDataDelete(appMeasurement.offlineFilename);
    }

    private void handleRequestList()
    {
      AppMeasurement appMeasurement = this;
      string str1 = "";
      double num1 = 0.0;
      while (true)
      {
        string request;
        do
        {
          bool flag = false;
          lock (appMeasurement.requestList)
          {
            while (appMeasurement.requestList.Count == 0)
            {
              try
              {
                if (appMeasurement.requestThreads.Count > 1)
                  return;
                Monitor.Wait((object) appMeasurement.requestList, 1000);
              }
              catch
              {
              }
              if (appMeasurement.requestList.Count == 0)
                return;
            }
            request = appMeasurement.requestList[0];
            appMeasurement.requestList.RemoveAt(0);
          }
          if (!appMeasurement.trackOffline || !appMeasurement.offline)
          {
            if (appMeasurement.trackOffline && num1 > 0.0 && appMeasurement.offlineThrottleDelay > 0)
            {
              double num2 = appMeasurement.getTime() - num1;
              if (num2 < (double) appMeasurement.offlineThrottleDelay)
              {
                try
                {
                  Thread.Sleep((int) ((double) appMeasurement.offlineThrottleDelay - num2));
                }
                catch
                {
                }
              }
            }
            Dictionary<string, string> headers = (Dictionary<string, string>) null;
            if (appMeasurement._hasDoRequest())
            {
              string[] strArray = request.Split('\t');
              if (strArray.Length > 0 && strArray[0].Length > 0)
              {
                string url = strArray[0];
                for (int index = 1; index < strArray.Length; index += 2)
                {
                  string key = strArray[index];
                  if (key != null && key != "" && index < strArray.Length - 1)
                  {
                    string str2 = strArray[index + 1];
                    if (str2 != null && str2 != "")
                    {
                      if (headers == null)
                        headers = new Dictionary<string, string>();
                      headers[key] = str2;
                    }
                  }
                }
                if (appMeasurement._doRequest(url, headers))
                  flag = true;
              }
            }
            else if (appMeasurement.requestRequest(request))
              flag = true;
            if (flag)
              num1 = appMeasurement.requestList.Count <= 0 ? 0.0 : appMeasurement.getTime();
          }
          if (flag)
            goto label_42;
        }
        while (!appMeasurement.trackOffline);
        lock (appMeasurement.requestList)
        {
          appMeasurement.requestList.Insert(0, request);
          if (appMeasurement.requestList.Count != 0)
          {
            if (appMeasurement.requestList[appMeasurement.requestList.Count - 1] != str1)
            {
              str1 = appMeasurement.requestList[appMeasurement.requestList.Count - 1];
              appMeasurement.offlineRequestListWrite();
            }
          }
          try
          {
            Monitor.Wait((object) appMeasurement.requestList, 500);
            continue;
          }
          catch
          {
            continue;
          }
        }
label_42:
        appMeasurement.offlineRequestListDelete();
      }
    }

    private void requestThreadStart()
    {
      AppMeasurement s = this;
      if (!s.isSet(s.maxRequestThreads))
        s.maxRequestThreads = 1;
      if (s.requestThreads == null)
        s.requestThreads = new Dictionary<int, Thread>();
      int num = (int) Math.Ceiling((double) s.requestList.Count / (double) s.maxRequestsPerThread) + 1;
      lock (s.requestThreads)
      {
        while (s.requestThreads.Count < num && s.requestThreads.Count < s.maxRequestThreads)
        {
          if (s.requestThreads.Count <= 0)
            s.requestThreadID = 0;
          int threadID = s.requestThreadID;
          Thread thread = new Thread((ThreadStart) (() =>
          {
            s.handleRequestList();
            lock (s.requestThreads)
              s.requestThreads.Remove(threadID);
          }));
          s.requestThreads[s.requestThreadID] = thread;
          thread.Start();
          ++s.requestThreadID;
        }
      }
    }

    private void sendRequest(string request)
    {
      AppMeasurement appMeasurement = this;
      if (appMeasurement.requestList == null)
      {
        appMeasurement.requestList = new List<string>();
        appMeasurement.offlineRequestListRead();
      }
      appMeasurement.requestThreadStart();
      lock (appMeasurement.requestList)
      {
        if (appMeasurement.isSet(appMeasurement.trackOffline))
        {
          if (!appMeasurement.isSet(appMeasurement.offlineLimit))
            appMeasurement.offlineLimit = 10;
          if (appMeasurement.requestList.Count >= appMeasurement.offlineLimit)
            appMeasurement.requestList.RemoveAt(0);
        }
        appMeasurement.requestList.Add(request);
        Monitor.Pulse((object) appMeasurement.requestList);
      }
      if (!appMeasurement.isSet(appMeasurement.debugTracking))
        return;
      string str1 = "AppMeasurement Debug: ";
      string[] strArray = request.Split('\t');
      if (strArray.Length <= 0 || strArray[0].Length <= 0)
        return;
      string str2 = strArray[0];
      string msg = str1 + str2;
      string str3 = str2;
      char[] chArray = new char[1]{ '&' };
      foreach (string str4 in str3.Split(chArray))
        msg = msg + "\n\t" + AppMeasurement.unescape(str4);
      for (int index = 1; index < strArray.Length; index += 2)
      {
        string str5 = strArray[index];
        if (str5 != null && str5 != "" && index < strArray.Length - 1)
        {
          string str6 = strArray[index + 1];
          if (str6 != null && str6 != "")
            msg = msg + "\n\t" + str5 + ": " + str6;
        }
      }
      appMeasurement.logDebug(msg);
    }

    private string makeRequest(string cacheBusting, string queryString)
    {
      AppMeasurement appMeasurement = this;
      string val1 = appMeasurement.trackingServer;
      string val2 = "";
      string dc = appMeasurement.dc;
      string str1 = "sc.";
      string val3 = appMeasurement.visitorNamespace;
      if (appMeasurement.isSet(val1))
      {
        if (appMeasurement.isSet(appMeasurement.trackingServerSecure) && appMeasurement.isSet(appMeasurement.ssl))
          val1 = appMeasurement.trackingServerSecure;
      }
      else
      {
        if (!appMeasurement.isSet(val3))
        {
          string str2 = appMeasurement.account;
          int length = str2.IndexOf(",");
          if (length >= 0)
            str2 = str2.Substring(0, length);
          val3 = string.Join("-", str2.Split('_'));
        }
        if (!appMeasurement.isSet(val2))
          val2 = "2o7.net";
        string str3 = !appMeasurement.isSet(dc) ? "d1" : dc.ToLower();
        if (val2 == "2o7.net")
        {
          switch (str3)
          {
            case "d1":
              str3 = "112";
              break;
            case "d2":
              str3 = "122";
              break;
          }
          str1 = "";
        }
        val1 = val3 + "." + str3 + "." + str1 + val2;
      }
      string request = (!appMeasurement.isSet(appMeasurement.ssl) ? "http://" : "https://") + val1 + "/b/ss/" + appMeasurement.account + "/" + (appMeasurement.mobile ? "5." : "") + (appMeasurement.sendFromServer ? "0" : "1") + "/" + appMeasurement.version + "-" + appMeasurement.csTarget + "/" + cacheBusting + "?AQB=1&ndh=1&" + queryString + "&AQE=1";
      if (appMeasurement.isSet(appMeasurement.userAgent))
        request = request + "\tUser-Agent\t" + appMeasurement.replace(appMeasurement.replace(appMeasurement.replace(appMeasurement.userAgent, "\t", " "), "\n", " "), "\r", " ");
      if (appMeasurement.isSet(appMeasurement.acceptLanguage))
        request = request + "\tAccept-Language\t" + appMeasurement.replace(appMeasurement.replace(appMeasurement.replace(appMeasurement.acceptLanguage, "\t", " "), "\n", " "), "\r", " ");
      if (appMeasurement.isSet(appMeasurement.xForwardedFor))
        request = request + "\tX-Forwarded-For\t" + appMeasurement.replace(appMeasurement.replace(appMeasurement.replace(appMeasurement.xForwardedFor, "\t", " "), "\n", " "), "\r", " ");
      appMeasurement.sendRequest(request);
      return "";
    }

    public string getPageURL() => "";

    public string getReferrer()
    {
      return this.callJavaScript("function s_ActionSource_r(){\tvar \t\tr = '',\t\tw = window,\t\te,\t\tp,\t\tl,\t\te;\tif ((w) && (w.document)) {\t\tr = w.document.referrer;\t\ttry {\t\t\tp = w.parent;\t\t\tl = w.location;\t\t\twhile ((p) && (p.location) && (l) && (''+p.location != ''+l) && (w.location) && (''+p.location != ''+w.location) && (p.location.host == l.host)) {\t\t\t\tw = p;\t\t\t\tp = w.parent;\t\t\t}\t\t} catch (e) {}\t\tif ((w) && (w.document)) {\t\t\tr = w.document.referrer;\t\t}\t}\treturn r;}s_ActionSource_r();");
    }

    private string serializeToQueryString(
      string varKey,
      Dictionary<string, object> varValue,
      string varFilter,
      string varFilterPrefix,
      string filter)
    {
      AppMeasurement appMeasurement = this;
      string queryString = "";
      List<string> stringList = (List<string>) null;
      if (varKey == "contextData")
        varKey = "c";
      if (varValue != null)
      {
        foreach (KeyValuePair<string, object> keyValuePair in varValue)
        {
          string str1 = keyValuePair.Key;
          object val = varValue[str1];
          if ((filter == null || (str1.Length >= filter.Length ? (str1.Substring(0, filter.Length) == filter ? 1 : 0) : 0) != 0) && appMeasurement.isSet(val) && (!appMeasurement.isSet(varFilter) || varFilter.IndexOf("," + (appMeasurement.isSet(varFilterPrefix) ? varFilterPrefix + "." : "") + str1 + ",") >= 0))
          {
            bool flag1 = false;
            if (stringList != null)
            {
              for (int index = 0; index < stringList.Count; ++index)
              {
                if ((str1.Length >= stringList[index].Length ? (str1.Substring(0, stringList[index].Length) == stringList[index] ? 1 : 0) : 0) != 0)
                  flag1 = true;
              }
            }
            if (!flag1)
            {
              if (queryString == "")
                queryString = queryString + "&" + varKey + ".";
              if (filter != null)
                str1 = str1.Substring(filter.Length);
              if (str1.Length > 0)
              {
                int length = str1.IndexOf(".");
                if (length > 0)
                {
                  string varKey1 = str1.Substring(0, length);
                  string filter1 = (filter != null ? filter : "") + varKey1 + ".";
                  if (stringList == null)
                    stringList = new List<string>();
                  stringList.Add(filter1);
                  queryString += appMeasurement.serializeToQueryString(varKey1, varValue, varFilter, varFilterPrefix, filter1);
                }
                else
                {
                  if (val is bool flag2)
                    val = !flag2 ? (object) "false" : (object) "true";
                  if (appMeasurement.isSet(val))
                  {
                    if (varFilterPrefix == "retrieveLightData" && filter.IndexOf(".contextData.") < 0)
                    {
                      string str2 = str1.Length > 4 ? str1.Substring(0, 4) : "";
                      string num = str1.Length > 4 ? str1.Substring(4) : "";
                      switch (str1)
                      {
                        case "transactionID":
                          str1 = "xact";
                          break;
                        case "channel":
                          str1 = "ch";
                          break;
                        case "campaign":
                          str1 = "v0";
                          break;
                        default:
                          if (appMeasurement.isNumber(num))
                          {
                            switch (str2)
                            {
                              case "prop":
                                str1 = "c" + num;
                                break;
                              case "eVar":
                                str1 = "v" + num;
                                break;
                              case "list":
                                str1 = "l" + num;
                                break;
                              case "hier":
                                str1 = "h" + num;
                                val = (object) (val as string).Substring(0, (int) byte.MaxValue);
                                break;
                            }
                          }
                          else
                            break;
                          break;
                      }
                    }
                    queryString = queryString + "&" + AppMeasurement.escape(str1) + "=" + AppMeasurement.escape(string.Concat(val));
                  }
                }
              }
            }
          }
        }
        if (queryString != "")
          queryString = queryString + "&." + varKey;
      }
      return queryString;
    }

    private string serializeToQueryString(
      string varKey,
      Dictionary<string, object> varValue,
      string varFilter,
      string varFilterPrefix)
    {
      return this.serializeToQueryString(varKey, varValue, varFilter, varFilterPrefix, (string) null);
    }

    private string queryStringAccountVariables()
    {
      AppMeasurement appMeasurement = this;
      string str1 = "";
      string str2 = "";
      string val1 = "";
      string val2 = "";
      string[] strArray1;
      if (appMeasurement.isSet(appMeasurement.lightProfileID))
      {
        strArray1 = appMeasurement.lightVarList;
        str2 = appMeasurement.lightTrackVars;
        if (appMeasurement.isSet(str2))
          str2 = "," + str2 + "," + string.Join(",", appMeasurement.lightRequiredVarList) + ",";
      }
      else
      {
        strArray1 = appMeasurement.accountVarList;
        if (appMeasurement.isSet(appMeasurement.pe) || appMeasurement.isSet(appMeasurement.linkType))
        {
          str2 = appMeasurement.linkTrackVars;
          val1 = appMeasurement.linkTrackEvents;
        }
        if (appMeasurement.isSet(str2))
          str2 = "," + str2 + "," + string.Join(",", appMeasurement.requiredVarList) + ",";
        if (appMeasurement.isSet(val1))
        {
          val1 = "," + val1 + ",";
          if (appMeasurement.isSet(str2))
            str2 += ",events,";
        }
        if (appMeasurement.isSet(appMeasurement.events2))
          val2 = val2 + (val2 != "" ? "," : "") + appMeasurement.events2;
      }
      for (int index1 = 0; index1 < strArray1.Length; ++index1)
      {
        string str3 = strArray1[index1];
        string str4 = appMeasurement[str3];
        string str5;
        string num;
        if (str3.Length > 4)
        {
          str5 = str3.Substring(0, 4);
          num = str3.Substring(4);
        }
        else
        {
          str5 = "";
          num = "";
        }
        if (!appMeasurement.isSet(str4) && str3 == "events" && appMeasurement.isSet(val2))
        {
          str4 = val2;
          val2 = "";
        }
        if (appMeasurement.isSet(str4) && (!appMeasurement.isSet(str2) || str2.IndexOf("," + str3 + ",") >= 0))
        {
          switch (str3)
          {
            case "timestamp":
              str3 = "ts";
              break;
            case "dynamicVariablePrefix":
              str3 = "D";
              break;
            case "visitorID":
              str3 = "vid";
              break;
            case "pageURL":
              str3 = "g";
              break;
            case "referrer":
              str3 = "r";
              break;
            case "vmk":
            case "visitorMigrationKey":
              str3 = "vmt";
              break;
            case "visitorMigrationServer":
              str3 = "vmf";
              if (appMeasurement.isSet(appMeasurement.ssl) && appMeasurement.isSet(appMeasurement.visitorMigrationServerSecure))
              {
                str4 = "";
                break;
              }
              break;
            case "visitorMigrationServerSecure":
              str3 = "vmf";
              if (!appMeasurement.isSet(appMeasurement.ssl) && appMeasurement.isSet(appMeasurement.visitorMigrationServer))
              {
                str4 = "";
                break;
              }
              break;
            case "charSet":
              str3 = "ce";
              break;
            case "visitorNamespace":
              str3 = "ns";
              break;
            case "cookieDomainPeriods":
              str3 = "cdp";
              break;
            case "cookieLifetime":
              str3 = "cl";
              break;
            case "variableProvider":
              str3 = "vvp";
              break;
            case "currencyCode":
              str3 = "cc";
              break;
            case "channel":
              str3 = "ch";
              break;
            case "transactionID":
              str3 = "xact";
              break;
            case "campaign":
              str3 = "v0";
              break;
            case "events":
              if (appMeasurement.isSet(val2))
                str4 = str4 + (str4 != "" ? "," : "") + val2;
              if (appMeasurement.isSet(val1))
              {
                string[] strArray2 = str4.Split(',');
                str4 = "";
                for (int index2 = 0; index2 < strArray2.Length; ++index2)
                {
                  string str6 = strArray2[index2];
                  int length1 = str6.IndexOf("=");
                  if (length1 >= 0)
                    str6 = str6.Substring(0, length1);
                  int length2 = str6.IndexOf(":");
                  if (length2 >= 0)
                    str6 = str6.Substring(0, length2);
                  if (val1.IndexOf("," + str6 + ",") >= 0)
                    str4 = str4 + (appMeasurement.isSet(str4) ? "," : "") + strArray2[index2];
                }
                break;
              }
              break;
            case "events2":
              str4 = "";
              break;
            case "contextData":
              str1 += appMeasurement.serializeToQueryString("c", appMeasurement.contextData, str2, str3, (string) null);
              str4 = "";
              break;
            case "lightProfileID":
              str3 = "mtp";
              break;
            case "lightStoreForSeconds":
              str3 = "mtss";
              if (!appMeasurement.isSet(appMeasurement["lightProfileID"]))
              {
                str4 = "";
                break;
              }
              break;
            case "lightIncrementBy":
              str3 = "mti";
              if (!appMeasurement.isSet(appMeasurement["lightProfileID"]))
              {
                str4 = "";
                break;
              }
              break;
            case "retrieveLightProfiles":
              str3 = "mtsr";
              break;
            case "deleteLightProfiles":
              str3 = "mtsd";
              break;
            case "retrieveLightData":
              if (appMeasurement.isSet(appMeasurement["retrieveLightProfiles"]))
                str1 += appMeasurement.serializeToQueryString("mts", appMeasurement.retrieveLightData, str2, str3, (string) null);
              str4 = "";
              break;
            default:
              if (appMeasurement.isNumber(num))
              {
                switch (str5)
                {
                  case "prop":
                    str3 = "c" + num;
                    break;
                  case "eVar":
                    str3 = "v" + num;
                    break;
                  case "list":
                    str3 = "l" + num;
                    break;
                  case "hier":
                    str3 = "h" + num;
                    if (str4.Length > (int) byte.MaxValue)
                    {
                      str4 = str4.Substring(0, (int) byte.MaxValue);
                      break;
                    }
                    break;
                }
              }
              else
                break;
              break;
          }
          if (appMeasurement.isSet(str4))
            str1 = str1 + "&" + AppMeasurement.escape(str3) + "=" + (str3.Length < 3 || str3.Substring(0, 3) != "pev" ? AppMeasurement.escape(str4) : str4);
        }
      }
      return str1;
    }

    private string queryStringLinkTracking()
    {
      AppMeasurement appMeasurement = this;
      string str1 = "";
      string linkType = appMeasurement.linkType;
      string str2 = appMeasurement.linkURL;
      string linkName = appMeasurement.linkName;
      if (appMeasurement.isSet(linkType) && (appMeasurement.isSet(str2) || appMeasurement.isSet(linkName)))
      {
        string str3 = linkType.ToLower();
        if (str3 != "d" && str3 != "e")
          str3 = "o";
        if (appMeasurement.isSet(str2) && !appMeasurement.isSet(appMeasurement.linkLeaveQueryString))
        {
          int length = str2.IndexOf('?');
          if (length >= 0)
            str2 = str2.Substring(0, length);
        }
        str1 = str1 + "&pe=lnk_" + AppMeasurement.escape(str3) + (appMeasurement.isSet(str2) ? "&pev1=" + AppMeasurement.escape(str2) : "") + (appMeasurement.isSet(linkName) ? "&pev2=" + AppMeasurement.escape(linkName) : "");
      }
      return str1;
    }

    private string queryStringTechnology()
    {
      string str = "";
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      if (num1 > 0 && num2 > 0 && num3 > 0)
        str = str + "&s=" + (object) num1 + "x" + (object) num2 + "&c=" + (object) num3;
      return str;
    }

    private void variableOverridesApply(Dictionary<string, object> variableOverrides)
    {
      this.variableOverridesApply(variableOverrides, false);
    }

    private void variableOverridesApply(
      Dictionary<string, object> variableOverrides,
      bool restoring)
    {
      AppMeasurement appMeasurement = this;
      for (int index = 0; index < appMeasurement.accountVarList.Length; ++index)
      {
        string accountVar = appMeasurement.accountVarList[index];
        if (variableOverrides.ContainsKey(accountVar) && (variableOverrides[accountVar] is string && appMeasurement.isSet((string) variableOverrides[accountVar]) || appMeasurement.isSet(variableOverrides[accountVar])) || variableOverrides.ContainsKey("!" + accountVar) && appMeasurement.isSet((string) variableOverrides["!" + accountVar]))
        {
          object variableOverride = variableOverrides.ContainsKey(accountVar) ? variableOverrides[accountVar] : (object) null;
          if (!restoring && (accountVar == "contextData" || accountVar == "retrieveLightData"))
          {
            Dictionary<string, object> dictionary = variableOverride as Dictionary<string, object>;
            if (appMeasurement.isSet((object) appMeasurement.contextData))
            {
              foreach (KeyValuePair<string, object> keyValuePair in appMeasurement.contextData)
              {
                string key = keyValuePair.Key;
                if (!dictionary.ContainsKey(key) || !appMeasurement.isSet(dictionary[key]))
                  dictionary[key] = appMeasurement.contextData[key];
              }
            }
            else if (appMeasurement.isSet((object) appMeasurement.retrieveLightData))
            {
              foreach (KeyValuePair<string, object> keyValuePair in appMeasurement.retrieveLightData)
              {
                string key = keyValuePair.Key;
                if (!dictionary.ContainsKey(key) || !appMeasurement.isSet(dictionary[key]))
                  dictionary[key] = appMeasurement.retrieveLightData[key];
              }
            }
          }
          switch (accountVar)
          {
            case "contextData":
              appMeasurement.contextData = variableOverride as Dictionary<string, object>;
              continue;
            case "retrieveLightData":
              appMeasurement.retrieveLightData = variableOverride as Dictionary<string, object>;
              continue;
            default:
              appMeasurement[accountVar] = (string) variableOverride;
              continue;
          }
        }
      }
      for (int index = 0; index < appMeasurement.accountConfigList.Length; ++index)
      {
        string accountConfig = appMeasurement.accountConfigList[index];
        if (variableOverrides.ContainsKey(accountConfig) && appMeasurement.isSet((string) variableOverrides[accountConfig]) || variableOverrides.ContainsKey("!" + accountConfig) && appMeasurement.isSet((string) variableOverrides["!" + accountConfig]))
        {
          object variableOverride = variableOverrides.ContainsKey(accountConfig) ? variableOverrides[accountConfig] : (object) null;
          appMeasurement[accountConfig] = (string) variableOverride;
        }
      }
    }

    private void variableOverridesBuild(Dictionary<string, object> variableOverrides)
    {
      AppMeasurement appMeasurement = this;
      string str1;
      for (int index = 0; index < appMeasurement.accountVarList.Length; ++index)
      {
        string accountVar = appMeasurement.accountVarList[index];
        if (!variableOverrides.ContainsKey(accountVar) || !appMeasurement.isSet((string) variableOverrides[accountVar]))
        {
          str1 = appMeasurement[accountVar];
          switch (accountVar)
          {
            case "contextData":
              variableOverrides[accountVar] = (object) appMeasurement.contextData;
              break;
            case "retrieveLightData":
              variableOverrides[accountVar] = (object) appMeasurement.retrieveLightData;
              break;
            default:
              string str2;
              if (appMeasurement.isSet(str2 = appMeasurement[accountVar]))
              {
                variableOverrides[accountVar] = (object) str2;
                break;
              }
              break;
          }
          if (!variableOverrides.ContainsKey(accountVar) || !appMeasurement.isSet(variableOverrides[accountVar]))
            variableOverrides["!" + accountVar] = (object) "1";
        }
      }
      for (int index = 0; index < appMeasurement.accountConfigList.Length; ++index)
      {
        string accountConfig = appMeasurement.accountConfigList[index];
        if (!variableOverrides.ContainsKey(accountConfig) || !appMeasurement.isSet((string) variableOverrides[accountConfig]))
        {
          if (appMeasurement.isSet(str1 = appMeasurement[accountConfig]))
            variableOverrides[accountConfig] = (object) appMeasurement[accountConfig];
          if (!variableOverrides.ContainsKey(accountConfig) || !appMeasurement.isSet(variableOverrides[accountConfig]))
            variableOverrides["!" + accountConfig] = (object) "1";
        }
      }
    }

    public string track(Dictionary<string, object> variableOverrides)
    {
      AppMeasurement appMeasurement = this;
      string str1 = "";
      Dictionary<string, object> variableOverrides1 = (Dictionary<string, object>) null;
      DateTime now = DateTime.Now;
      if (this.rand == null)
        this.rand = new Random();
      double num = Math.Floor(this.rand.NextDouble() * 10000000000000.0);
      string cacheBusting = "s" + (object) (Math.Floor(appMeasurement.getTime() / 10800000.0) % 10.0) + (object) num;
      string str2 = "t=" + AppMeasurement.escape(now.Day.ToString() + "/" + (object) (now.Month - 1) + "/" + (object) now.Year + " " + (object) now.Hour + ":" + (object) now.Minute + ":" + (object) now.Second + " " + (object) (int) now.DayOfWeek + " " + (object) -TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes);
      if (appMeasurement.isSet((object) variableOverrides))
      {
        variableOverrides1 = new Dictionary<string, object>();
        appMeasurement.variableOverridesBuild(variableOverrides1);
        appMeasurement.variableOverridesApply(variableOverrides);
      }
      if (appMeasurement.usePlugins && appMeasurement._hasDoPlugins())
        appMeasurement._doPlugins();
      if (appMeasurement.isSet(appMeasurement.account))
      {
        if (appMeasurement.isSet(appMeasurement.trackOffline) && !appMeasurement.isSet(appMeasurement.timestamp))
          appMeasurement.timestamp = (int) Math.Floor(appMeasurement.getTime() / 1000.0);
        if (!appMeasurement.isSet(appMeasurement.pageURL))
          appMeasurement.pageURL = appMeasurement.getPageURL();
        if (!appMeasurement.isSet(appMeasurement.referrer) && !appMeasurement.isSet(appMeasurement._1_referrer))
        {
          appMeasurement.referrer = appMeasurement.getReferrer();
          appMeasurement._1_referrer = true;
        }
        string queryString = str2 + appMeasurement.queryStringAccountVariables() + appMeasurement.queryStringLinkTracking() + appMeasurement.queryStringTechnology();
        str1 = appMeasurement.makeRequest(cacheBusting, queryString);
      }
      if (appMeasurement.isSet((object) variableOverrides))
        appMeasurement.variableOverridesApply(variableOverrides1, true);
      appMeasurement.timestamp = 0;
      appMeasurement.referrer = (string) null;
      appMeasurement.pe = (string) null;
      appMeasurement.pev1 = (string) null;
      appMeasurement.pev2 = (string) null;
      appMeasurement.pev3 = (string) null;
      appMeasurement.linkURL = (string) null;
      appMeasurement.linkName = (string) null;
      appMeasurement.linkType = (string) null;
      appMeasurement.lightProfileID = (string) null;
      appMeasurement.retrieveLightProfiles = (string) null;
      appMeasurement.deleteLightProfiles = (string) null;
      return str1;
    }

    public string track() => this.track((Dictionary<string, object>) null);

    public string trackLink(
      string linkURL,
      string linkType,
      string linkName,
      Dictionary<string, object> variableOverrides)
    {
      AppMeasurement appMeasurement = this;
      appMeasurement.linkURL = linkURL;
      appMeasurement.linkType = linkType;
      appMeasurement.linkName = linkName;
      return appMeasurement.track(variableOverrides);
    }

    public string trackLink(string linkURL, string linkType, string linkName)
    {
      return this.trackLink(linkURL, linkType, linkName, (Dictionary<string, object>) null);
    }

    public string trackLight(
      string profileID,
      int storeForSeconds,
      int incrementBy,
      Dictionary<string, object> variableOverrides)
    {
      AppMeasurement appMeasurement = this;
      appMeasurement.lightProfileID = profileID;
      appMeasurement.lightStoreForSeconds = storeForSeconds;
      appMeasurement.lightIncrementBy = incrementBy;
      return appMeasurement.track(variableOverrides);
    }

    public string trackLight(string profileID, int storeForSeconds, int incrementBy)
    {
      return this.trackLight(profileID, storeForSeconds, incrementBy, (Dictionary<string, object>) null);
    }

    public string trackLight(string profileID, int storeForSeconds)
    {
      return this.trackLight(profileID, storeForSeconds, 0, (Dictionary<string, object>) null);
    }

    public string trackLight(string profileID)
    {
      return this.trackLight(profileID, 0, 0, (Dictionary<string, object>) null);
    }

    private string getDefaultUserAgent()
    {
      AppMeasurement appMeasurement = this;
      string applicationId = appMeasurement.getApplicationID();
      try
      {
        object obj;
        string val1 = DeviceExtendedProperties.TryGetValue("DeviceManufacturer", ref obj) ? obj as string : (string) null;
        string val2 = DeviceExtendedProperties.TryGetValue("DeviceName", ref obj) ? obj as string : (string) null;
        DeviceExtendedProperties.TryGetValue("DeviceOS", ref obj);
        DeviceExtendedProperties.TryGetValue("DeviceFirmwareVersion", ref obj);
        DeviceExtendedProperties.TryGetValue("DeviceHardwareVersion", ref obj);
        return "Mozilla/4.0 (compatible; MSIE 7.0; Windows Phone OS 7.0; Trident/3.1; IEMobile/7.0" + (appMeasurement.isSet(val1) ? "; " + (val1 == "Microsoft" ? "Microsoft Corporation" : val1) : "") + (appMeasurement.isSet(val2) ? "; " + val2 : "") + ")" + (applicationId != null ? " " + applicationId : "");
      }
      catch
      {
      }
      return (string) null;
    }

    private string getDefaultAcceptLanguage()
    {
      try
      {
        return CultureInfo.CurrentCulture.Name;
      }
      catch
      {
      }
      return (string) null;
    }

    private string getDefaultXForwardedFor() => (string) null;

    private string getDefaultVisitorID() => (string) null;

    private string getApplicationID()
    {
      AppMeasurement appMeasurement = this;
      string val1 = (string) null;
      string val2 = (string) null;
      try
      {
        val1 = Application.Current.ToString();
      }
      catch
      {
      }
      return appMeasurement.isSet(val1) ? val1 + (appMeasurement.isSet(val2) ? "/" + val2 : "") : (string) null;
    }

    public delegate void DoPlugins(AppMeasurement s);

    public delegate bool DoRequest(
      AppMeasurement s,
      string url,
      Dictionary<string, string> headers);

    private class PersistentStorage
    {
      public IsolatedStorageFile device;
    }
  }
}
