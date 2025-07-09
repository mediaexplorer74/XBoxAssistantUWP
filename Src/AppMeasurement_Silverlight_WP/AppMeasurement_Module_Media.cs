// *********************************************************
// Type: com.omniture.AppMeasurement_Module_Media
// Assembly: AppMeasurement_Silverlight_WP, Version=1.3.7.0, Culture=neutral, PublicKeyToken=null
// MVID: B2938048-604D-4B3E-B432-7854B0CBA8DA
// *********************************************************AppMeasurement_Silverlight_WP.dll

using System;
using System.Collections.Generic;


namespace com.omniture
{
  public class AppMeasurement_Module_Media : AppMeasurement_SetInterval
  {
    private AppMeasurement s;
    public Dictionary<string, object> list;
    public bool trackWhilePlaying = true;
    public bool trackUsingContextData;
    public string trackVars = "";
    public string trackEvents = "";
    public string channel = "";
    public int trackSeconds;
    public string trackMilestones = "";
    public string trackOffsetMilestones = "";
    public bool completeByCloseOffset = true;
    public double completeCloseOffsetThreshold = 1.0;
    public bool segmentByMilestones;
    public bool segmentByOffsetMilestones;
    private bool _autoTrack;
    private bool autoTrackDone;
    private int autoTrackInterval;
    public string autoTrackPlayerName = "";
    public bool autoTrackMediaLengthRequired = true;
    public AppMeasurement_Media_Monitor monitor;
    public Dictionary<string, object> contextDataMapping;

    public bool autoTrack
    {
      get => this._autoTrack;
      set
      {
        this._autoTrack = value;
        if (!this._autoTrack)
          return;
        this.doAutoTrack();
      }
    }

    private void doAutoTrack()
    {
      AppMeasurement_Module_Media measurementModuleMedia = this;
      if (!measurementModuleMedia.s.isSet(measurementModuleMedia.s.account) || !measurementModuleMedia.s.isSet(measurementModuleMedia.s.root))
        return;
      if (measurementModuleMedia.autoTrackInterval > 0)
      {
        AppMeasurement_SetInterval.clearInterval(measurementModuleMedia.autoTrackInterval);
        measurementModuleMedia.autoTrackInterval = 0;
      }
      if (!measurementModuleMedia._autoTrack || measurementModuleMedia.autoTrackDone)
        return;
      if (measurementModuleMedia.attach(measurementModuleMedia.s.root))
        measurementModuleMedia.autoTrackDone = true;
      else
        measurementModuleMedia.autoTrackInterval = AppMeasurement_SetInterval.setInterval(new EventHandler(measurementModuleMedia.doAutoTrack), 1000);
    }

    private void doAutoTrack(object s, object e) => this.doAutoTrack();

    public string playerName
    {
      get => this.autoTrackPlayerName;
      set => this.autoTrackPlayerName = value;
    }

    public AppMeasurement_Module_Media(AppMeasurement s) => this.s = s;

    public void variableOverridesApply(Dictionary<string, object> variableOverrides)
    {
      AppMeasurement_Module_Media measurementModuleMedia = this;
      foreach (KeyValuePair<string, object> variableOverride in variableOverrides)
      {
        string str = "";
        bool flag = false;
        int num = 0;
        try
        {
          str = (string) variableOverride.Value;
          if (str.ToLower() == "true")
            flag = true;
          if (measurementModuleMedia.s.isNumber(str))
            num = int.Parse(str);
          if (flag)
            num = 1;
        }
        catch
        {
          try
          {
            flag = (bool) variableOverride.Value;
            if (flag)
            {
              str = "true";
              num = 1;
            }
          }
          catch
          {
            try
            {
              num = (int) variableOverride.Value;
              str = string.Concat((object) num);
              if (num != 0)
                flag = true;
            }
            catch
            {
            }
          }
        }
        switch (variableOverride.Key)
        {
          case "autoTrack":
            measurementModuleMedia.autoTrack = flag;
            continue;
          case "autoTrackPlayerName":
            measurementModuleMedia.autoTrackPlayerName = str;
            continue;
          case "playerName":
            measurementModuleMedia.playerName = str;
            continue;
          case "trackUsingContextData":
            measurementModuleMedia.trackUsingContextData = flag;
            continue;
          case "contextDataMapping":
            measurementModuleMedia.contextDataMapping = variableOverride.Value as Dictionary<string, object>;
            continue;
          case "trackSeconds":
            measurementModuleMedia.trackSeconds = num;
            continue;
          case "trackMilestones":
            measurementModuleMedia.trackMilestones = str;
            continue;
          case "trackOffsetMilestones":
            measurementModuleMedia.trackOffsetMilestones = str;
            continue;
          case "trackVars":
            measurementModuleMedia.trackVars = str;
            continue;
          case "trackEvents":
            measurementModuleMedia.trackEvents = str;
            continue;
          case "segmentByMilestones":
            measurementModuleMedia.segmentByMilestones = flag;
            continue;
          case "segmentByOffsetMilestones":
            measurementModuleMedia.segmentByOffsetMilestones = flag;
            continue;
          default:
            continue;
        }
      }
    }

    private string cleanName(string name)
    {
      AppMeasurement_Module_Media measurementModuleMedia = this;
      return measurementModuleMedia.s.replace(measurementModuleMedia.s.replace(measurementModuleMedia.s.replace(name, "\n", ""), "\r", ""), "--**--", "");
    }

    public void open(string name, double length, string playerName, string playerID)
    {
      AppMeasurement_Module_Media measurementModuleMedia = this;
      AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem moduleMediaMediaItem = new AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem();
      name = measurementModuleMedia.cleanName(name);
      if (!measurementModuleMedia.s.isSet(length))
        length = -1.0;
      if (!measurementModuleMedia.s.isSet(name) || !measurementModuleMedia.s.isSet(playerName))
        return;
      if (!measurementModuleMedia.s.isSet((object) measurementModuleMedia.list))
        measurementModuleMedia.list = new Dictionary<string, object>();
      if (measurementModuleMedia.list.ContainsKey(name))
        measurementModuleMedia.close(name);
      if (measurementModuleMedia.s.isSet(playerID))
      {
        List<string> stringList = new List<string>();
        foreach (KeyValuePair<string, object> keyValuePair in measurementModuleMedia.list)
          stringList.Add(keyValuePair.Key);
        foreach (string key in stringList)
        {
          if (((AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem) measurementModuleMedia.list[key]).playerID == playerID)
            measurementModuleMedia.close(((AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem) measurementModuleMedia.list[key]).name);
        }
      }
      moduleMediaMediaItem.name = name;
      moduleMediaMediaItem.length = length;
      moduleMediaMediaItem.offset = 0.0;
      moduleMediaMediaItem.percent = 0.0;
      moduleMediaMediaItem.playerName = measurementModuleMedia.cleanName(measurementModuleMedia.s.isSet(measurementModuleMedia.playerName) ? measurementModuleMedia.playerName : playerName);
      moduleMediaMediaItem.playerID = playerID;
      moduleMediaMediaItem.timePlayed = 0.0;
      moduleMediaMediaItem.timePlayedSinseTrack = 0.0;
      moduleMediaMediaItem.timestamp = Math.Floor(measurementModuleMedia.s.getTime() / 1000.0);
      moduleMediaMediaItem.lastEventType = 0;
      moduleMediaMediaItem.lastEventTimestamp = moduleMediaMediaItem.timestamp;
      moduleMediaMediaItem.lastEventOffset = 0.0;
      moduleMediaMediaItem.session = "";
      moduleMediaMediaItem.lastTrackOffset = -1.0;
      moduleMediaMediaItem.trackCount = 0;
      moduleMediaMediaItem.firstEventList = new Dictionary<string, object>();
      moduleMediaMediaItem.viewTracked = false;
      moduleMediaMediaItem.segmentNum = 0;
      moduleMediaMediaItem.segment = "";
      moduleMediaMediaItem.segmentLength = 0.0;
      moduleMediaMediaItem.segmentGenerated = false;
      moduleMediaMediaItem.segmentChanged = false;
      moduleMediaMediaItem.complete = false;
      moduleMediaMediaItem.completeTracked = false;
      moduleMediaMediaItem.lastMilestone = 0;
      moduleMediaMediaItem.lastOffsetMilestone = 0;
      measurementModuleMedia.list.Add(name, (object) moduleMediaMediaItem);
    }

    public void open(string name, int length, string playerName)
    {
      this.open(name, (double) length, playerName, (string) null);
    }

    private void _delete(string name)
    {
      AppMeasurement_Module_Media measurementModuleMedia = this;
      name = measurementModuleMedia.cleanName(name);
      AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem val = !measurementModuleMedia.s.isSet(name) || measurementModuleMedia.list == null || !measurementModuleMedia.list.ContainsKey(name) ? (AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem) null : (AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem) measurementModuleMedia.list[name];
      if (!measurementModuleMedia.s.isSet((object) val))
        return;
      if (measurementModuleMedia.s.isSet((object) val.monitor))
        val.monitor.stop();
      measurementModuleMedia.list.Remove(name);
    }

    public void close(string name)
    {
      this.playerEvent(name, 0, -1.0, 0, (string) null, -1.0, (object) null);
    }

    private void monitorPlayer(
      AppMeasurement_Module_Media.AppMeasurement_Module_Media_PlayerMonitor monitor)
    {
      AppMeasurement_Module_Media measurementModuleMedia = this;
      string node = (string) monitor.node;
      if (measurementModuleMedia.list == null || !measurementModuleMedia.list.ContainsKey(node))
        return;
      AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem moduleMediaMediaItem = (AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem) measurementModuleMedia.list[node];
      if (moduleMediaMediaItem.lastEventType != 1)
        return;
      measurementModuleMedia.playerEvent(moduleMediaMediaItem.name, 3, -1.0, 0, (string) null, -1.0, (object) null);
    }

    public void play(
      string name,
      double offset,
      int segmentNum,
      string segment,
      double segmentLength)
    {
      AppMeasurement_Module_Media measurementModuleMedia = this;
      measurementModuleMedia.playerEvent(name, 1, offset, segmentNum, segment, segmentLength, (object) null);
      name = measurementModuleMedia.cleanName(name);
      if (!measurementModuleMedia.s.isSet(name) || !measurementModuleMedia.list.ContainsKey(name))
        return;
      AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem moduleMediaMediaItem = (AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem) measurementModuleMedia.list[name];
      if (moduleMediaMediaItem.monitor != null)
        return;
      AppMeasurement_Module_Media.AppMeasurement_Module_Media_PlayerMonitor mediaPlayerMonitor = new AppMeasurement_Module_Media.AppMeasurement_Module_Media_PlayerMonitor();
      moduleMediaMediaItem.monitor = mediaPlayerMonitor;
      mediaPlayerMonitor.m = measurementModuleMedia;
      mediaPlayerMonitor.node = (object) name;
      mediaPlayerMonitor.monitor = new AppMeasurement_Module_Media.AppMeasurement_Module_Media_PlayerMonitorHandler(measurementModuleMedia.monitorPlayer);
      mediaPlayerMonitor.start();
    }

    public void play(string name, double offset, int segmentNum, string segment)
    {
      this.play(name, offset, segmentNum, segment, -1.0);
    }

    public void play(string name, double offset) => this.play(name, offset, 0, (string) null, -1.0);

    public void complete(string name, double offset)
    {
      this.playerEvent(name, 5, offset, 0, (string) null, -1.0, (object) null);
    }

    public void stop(string name, double offset)
    {
      this.playerEvent(name, 2, offset, 0, (string) null, -1.0, (object) null);
    }

    public void track(string name)
    {
      this.playerEvent(name, 4, -1.0, 0, (string) null, -1.0, (object) null);
    }

    private void buildContextData(
      Dictionary<string, object> variableOverrides,
      AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem media)
    {
      AppMeasurement_Module_Media measurementModuleMedia = this;
      string str1 = "a.media.";
      string val1 = variableOverrides["linkTrackVars"] as string;
      string val2 = variableOverrides["linkTrackEvents"] as string;
      string str2 = "m_i";
      Dictionary<string, object> variableOverride = variableOverrides["contextData"] as Dictionary<string, object>;
      variableOverride["a.contentType"] = (object) "video";
      variableOverride[str1 + "channel"] = (object) measurementModuleMedia.channel;
      variableOverride[str1 + "name"] = (object) media.name;
      variableOverride[str1 + "playerName"] = (object) media.playerName;
      if (media.length > 0.0)
        variableOverride[str1 + "length"] = (object) media.length;
      variableOverride[str1 + "timePlayed"] = (object) Math.Floor(media.timePlayedSinseTrack);
      if (!media.viewTracked)
      {
        variableOverride[str1 + "view"] = (object) true;
        str2 = "m_s";
        media.viewTracked = true;
      }
      if (measurementModuleMedia.s.isSet(media.segment))
      {
        variableOverride[str1 + "segmentNum"] = (object) media.segmentNum;
        variableOverride[str1 + "segment"] = (object) media.segment;
        if (media.segmentLength > 0.0)
          variableOverride[str1 + "segmentLength"] = (object) media.segmentLength;
        if (media.segmentChanged && media.timePlayedSinseTrack > 0.0)
          variableOverride[str1 + "segmentView"] = (object) true;
      }
      if (!media.completeTracked && media.complete)
      {
        variableOverride[str1 + "complete"] = (object) true;
        media.completeTracked = true;
      }
      if (media.lastMilestone > 0)
        variableOverride[str1 + "milestone"] = (object) media.lastMilestone;
      if (media.lastOffsetMilestone > 0)
        variableOverride[str1 + "offsetMilestone"] = (object) media.lastOffsetMilestone;
      if (measurementModuleMedia.s.isSet(val1))
      {
        foreach (KeyValuePair<string, object> keyValuePair in variableOverride)
        {
          string key = keyValuePair.Key;
          val1 = val1 + ",contextData." + key;
        }
      }
      string str3 = "video";
      variableOverrides["pe"] = (object) str2;
      variableOverrides["pev3"] = (object) str3;
      if (measurementModuleMedia.s.isSet((object) measurementModuleMedia.contextDataMapping))
      {
        variableOverrides["events2"] = (object) "";
        if (measurementModuleMedia.s.isSet(val1))
          val1 += ",events";
        foreach (KeyValuePair<string, object> keyValuePair in measurementModuleMedia.contextDataMapping)
        {
          string key1 = keyValuePair.Key;
          string str4 = key1.Length <= str1.Length || !(key1.Substring(0, str1.Length) == str1) ? "" : key1.Substring(str1.Length);
          object obj = (object) (measurementModuleMedia.contextDataMapping[key1] as string);
          if (obj is string && variableOverride.ContainsKey(key1))
          {
            string str5 = (string) obj;
            char[] chArray = new char[1]{ ',' };
            foreach (object key2 in str5.Split(chArray))
            {
              if (key1 == "a.contentType")
              {
                if (measurementModuleMedia.s.isSet(val1))
                  val1 = val1 + "," + key2;
                variableOverrides[(string) key2] = variableOverride[key1];
              }
              else if (str4 != "")
              {
                switch (str4)
                {
                  case "view":
                  case "segmentView":
                  case "complete":
                  case "timePlayed":
                    if (measurementModuleMedia.s.isSet(val2))
                      val2 = val2 + "," + key2;
                    if (str4 == "timePlayed")
                    {
                      if (measurementModuleMedia.s.isSet((double) variableOverride[key1]))
                      {
                        Dictionary<string, object> dictionary;
                        string str6 = (dictionary = variableOverrides)["events2"].ToString() + (measurementModuleMedia.s.isSet(variableOverrides["events2"]) ? (object) "," : (object) "") + key2 + "=" + variableOverride[key1];
                        dictionary["events2"] = (object) str6;
                        continue;
                      }
                      continue;
                    }
                    if ((bool) variableOverride[key1])
                    {
                      Dictionary<string, object> dictionary;
                      (dictionary = variableOverrides)["events2"] = (object) (dictionary["events2"].ToString() + (measurementModuleMedia.s.isSet(variableOverrides["events2"]) ? (object) "," : (object) "") + key2);
                      continue;
                    }
                    continue;
                  case "segment":
                    if (variableOverride.ContainsKey(key1 + "Num") && measurementModuleMedia.s.isSet(variableOverride[key1 + "Num"]))
                    {
                      if (measurementModuleMedia.s.isSet(val1))
                        val1 = val1 + "," + key2;
                      variableOverrides[(string) key2] = (object) (variableOverride[key1 + "Num"].ToString() + ":" + variableOverride[key1]);
                      continue;
                    }
                    break;
                }
                if (measurementModuleMedia.s.isSet(val1))
                  val1 = val1 + "," + key2;
                variableOverrides[(string) key2] = variableOverride[key1];
              }
            }
          }
          else if (str4 == "milestones" || str4 == "offsetMilestones")
          {
            string key3 = key1.Substring(0, key1.Length - 1);
            if (variableOverride.ContainsKey(key3) && measurementModuleMedia.s.isSet(variableOverride[key3]) && (measurementModuleMedia.contextDataMapping[key3 + "s"] as Dictionary<int, string>).ContainsKey((int) variableOverride[key3]))
            {
              if (measurementModuleMedia.s.isSet(val2))
                val2 = val2 + "," + (measurementModuleMedia.contextDataMapping[key3 + "s"] as Dictionary<int, string>)[(int) variableOverride[key3]];
              Dictionary<string, object> dictionary;
              (dictionary = variableOverrides)["events2"] = (object) (dictionary["events2"].ToString() + (measurementModuleMedia.s.isSet(variableOverrides["events2"]) ? (object) "," : (object) "") + (measurementModuleMedia.contextDataMapping[key3 + "s"] as Dictionary<int, string>)[(int) variableOverride[key3]]);
            }
          }
        }
        variableOverrides["contextData"] = (object) null;
      }
      variableOverrides["linkTrackVars"] = (object) val1;
      variableOverrides["linkTrackEvents"] = (object) val2;
    }

    private void buildPageEvent(
      Dictionary<string, object> variableOverrides,
      AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem media,
      int eventType,
      double offset)
    {
      string str1 = "--**--";
      string str2 = "m_o";
      if (!media.viewTracked)
      {
        str2 = "m_s";
        media.viewTracked = true;
      }
      else if (eventType == 4)
        str2 = "m_i";
      string str3 = AppMeasurement.escape(media.name) + str1 + (object) Math.Floor(media.length > 0.0 ? media.length : 1.0) + str1 + AppMeasurement.escape(media.playerName) + str1 + (object) Math.Floor(media.timePlayed) + str1 + (object) media.timestamp + str1 + (media.lastTrackOffset >= 0.0 ? (object) ("L" + (object) Math.Floor(media.lastTrackOffset)) : (object) "") + media.session + (eventType == 0 || eventType == 2 ? (object) "" : (object) ("L" + (object) Math.Floor(offset)));
      variableOverrides["pe"] = (object) str2;
      variableOverrides["pev3"] = (object) str3;
    }

    private AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem playerEvent(
      string name,
      int eventType,
      double offset,
      int segmentNum,
      string segment,
      double segmentLength,
      object playerData)
    {
      AppMeasurement_Module_Media measurementModuleMedia = this;
      double num1 = Math.Floor(measurementModuleMedia.s.getTime() / 1000.0);
      string str1 = measurementModuleMedia.trackVars;
      string str2 = measurementModuleMedia.trackEvents;
      double trackSeconds = (double) measurementModuleMedia.trackSeconds;
      string trackMilestones = measurementModuleMedia.trackMilestones;
      string offsetMilestones1 = measurementModuleMedia.trackOffsetMilestones;
      bool segmentByMilestones = measurementModuleMedia.segmentByMilestones;
      bool offsetMilestones2 = measurementModuleMedia.segmentByOffsetMilestones;
      bool flag = true;
      AppMeasurement_Media_State media = new AppMeasurement_Media_State();
      name = measurementModuleMedia.cleanName(name);
      AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem moduleMediaMediaItem1 = !measurementModuleMedia.s.isSet(name) || measurementModuleMedia.list == null || !measurementModuleMedia.list.ContainsKey(name) ? (AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem) null : (AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem) measurementModuleMedia.list[name];
      if (measurementModuleMedia.s.isSet((object) moduleMediaMediaItem1))
      {
        if (offset < 0.0)
          offset = moduleMediaMediaItem1.lastEventType != 1 || moduleMediaMediaItem1.lastEventTimestamp <= 0.0 ? moduleMediaMediaItem1.lastEventOffset : num1 - moduleMediaMediaItem1.lastEventTimestamp + moduleMediaMediaItem1.lastEventOffset;
        if (moduleMediaMediaItem1.length > 0.0)
          offset = offset < moduleMediaMediaItem1.length ? offset : moduleMediaMediaItem1.length;
        if (offset < 0.0)
          offset = 0.0;
        moduleMediaMediaItem1.offset = offset;
        if (moduleMediaMediaItem1.length > 0.0)
        {
          moduleMediaMediaItem1.percent = moduleMediaMediaItem1.offset / moduleMediaMediaItem1.length * 100.0;
          moduleMediaMediaItem1.percent = moduleMediaMediaItem1.percent > 100.0 ? 100.0 : moduleMediaMediaItem1.percent;
        }
        if (moduleMediaMediaItem1.lastEventOffset < 0.0)
          moduleMediaMediaItem1.lastEventOffset = offset;
        int trackCount = moduleMediaMediaItem1.trackCount;
        media.name = name;
        media.length = moduleMediaMediaItem1.length;
        AppMeasurement_Media_State measurementMediaState1 = media;
        AppMeasurement_Media_State measurementMediaState2 = media;
        DateTime dateTime1 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        ref DateTime local = ref dateTime1;
        double num2 = moduleMediaMediaItem1.timestamp + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalSeconds;
        DateTime dateTime2;
        DateTime dateTime3 = dateTime2 = local.AddSeconds(num2);
        measurementMediaState2.openTime = dateTime2;
        DateTime dateTime4 = dateTime3;
        measurementMediaState1.openTime = dateTime4;
        media.offset = moduleMediaMediaItem1.offset;
        media.percent = moduleMediaMediaItem1.percent;
        media.playerName = moduleMediaMediaItem1.playerName;
        if (moduleMediaMediaItem1.lastTrackOffset < 0.0)
        {
          media.mediaEvent = "OPEN";
        }
        else
        {
          AppMeasurement_Media_State measurementMediaState3 = media;
          string str3;
          switch (eventType)
          {
            case 1:
              str3 = "PLAY";
              break;
            case 2:
              str3 = "STOP";
              break;
            case 3:
              str3 = "MONITOR";
              break;
            case 4:
              str3 = "TRACK";
              break;
            case 5:
              str3 = "COMPLETE";
              break;
            default:
              str3 = "CLOSE";
              break;
          }
          measurementMediaState3.mediaEvent = str3;
        }
        if (!measurementModuleMedia.s.isSet(playerData))
        {
          if (measurementModuleMedia.s.isSet(moduleMediaMediaItem1.playerData))
            playerData = moduleMediaMediaItem1.playerData;
        }
        else
          moduleMediaMediaItem1.playerData = playerData;
        media.player = playerData;
        if (eventType > 2 || eventType != moduleMediaMediaItem1.lastEventType && (eventType != 2 || moduleMediaMediaItem1.lastEventType == 1))
        {
          if (!measurementModuleMedia.s.isSet(segment))
          {
            segmentNum = moduleMediaMediaItem1.segmentNum;
            segment = moduleMediaMediaItem1.segment;
            segmentLength = moduleMediaMediaItem1.segmentLength;
          }
          if (measurementModuleMedia.s.isSet(eventType))
          {
            if (eventType == 1)
              moduleMediaMediaItem1.lastEventOffset = offset;
            if ((eventType <= 3 || eventType == 5) && moduleMediaMediaItem1.lastTrackOffset >= 0.0)
            {
              flag = false;
              str1 = str2 = "None";
              if (moduleMediaMediaItem1.lastTrackOffset != offset)
              {
                double num3 = moduleMediaMediaItem1.lastTrackOffset;
                if (num3 > offset)
                {
                  num3 = moduleMediaMediaItem1.lastEventOffset;
                  if (num3 > offset)
                    num3 = offset;
                }
                string[] strArray1;
                if (!measurementModuleMedia.s.isSet(trackMilestones))
                  strArray1 = (string[]) null;
                else
                  strArray1 = trackMilestones.Split(',');
                string[] val1 = strArray1;
                if (moduleMediaMediaItem1.length > 0.0 && measurementModuleMedia.s.isSet((object) val1) && offset >= num3)
                {
                  for (int index = 0; index < val1.Length; ++index)
                  {
                    double val2;
                    try
                    {
                      val2 = measurementModuleMedia.s.isSet(val1[index]) ? (double) int.Parse(val1[index] ?? "") : 0.0;
                    }
                    catch
                    {
                      val2 = 0.0;
                    }
                    if (measurementModuleMedia.s.isSet(val2) && num3 / moduleMediaMediaItem1.length * 100.0 < val2 && moduleMediaMediaItem1.percent >= val2)
                    {
                      flag = true;
                      index = val1.Length;
                      media.mediaEvent = "MILESTONE";
                      moduleMediaMediaItem1.lastMilestone = media.milestone = (int) val2;
                    }
                  }
                }
                string[] strArray2;
                if (!measurementModuleMedia.s.isSet(offsetMilestones1))
                  strArray2 = (string[]) null;
                else
                  strArray2 = offsetMilestones1.Split(',');
                string[] val3 = strArray2;
                if (measurementModuleMedia.s.isSet((object) val3) && offset >= num3)
                {
                  for (int index = 0; index < val3.Length; ++index)
                  {
                    double val4;
                    try
                    {
                      val4 = measurementModuleMedia.s.isSet(val3[index]) ? (double) int.Parse(val3[index] ?? "") : 0.0;
                    }
                    catch
                    {
                      val4 = 0.0;
                    }
                    if (measurementModuleMedia.s.isSet(val4) && num3 < val4 && offset >= val4)
                    {
                      flag = true;
                      index = val3.Length;
                      media.mediaEvent = "OFFSET_MILESTONE";
                      moduleMediaMediaItem1.lastOffsetMilestone = media.offsetMilestone = (int) val4;
                    }
                  }
                }
              }
            }
            if (moduleMediaMediaItem1.segmentGenerated || !measurementModuleMedia.s.isSet(segment))
            {
              if (segmentByMilestones && measurementModuleMedia.s.isSet(trackMilestones) && moduleMediaMediaItem1.length > 0.0)
              {
                string[] val5 = (trackMilestones + ",100").Split(',');
                if (measurementModuleMedia.s.isSet((object) val5))
                {
                  double num4 = 0.0;
                  for (int index = 0; index < val5.Length; ++index)
                  {
                    double val6;
                    try
                    {
                      val6 = measurementModuleMedia.s.isSet(val5[index]) ? (double) int.Parse(val5[index] ?? "") : 0.0;
                    }
                    catch
                    {
                      val6 = 0.0;
                    }
                    if (measurementModuleMedia.s.isSet(val6))
                    {
                      if (moduleMediaMediaItem1.percent < val6)
                      {
                        segmentNum = index + 1;
                        segment = "M:" + (object) num4 + "-" + (object) val6;
                        index = val5.Length;
                      }
                      num4 = val6;
                    }
                  }
                }
              }
              else if (offsetMilestones2 && measurementModuleMedia.s.isSet(offsetMilestones1))
              {
                string[] val7 = (offsetMilestones1 + "," + (moduleMediaMediaItem1.length > 0.0 ? string.Concat((object) moduleMediaMediaItem1.length) : "E")).Split(',');
                if (measurementModuleMedia.s.isSet((object) val7))
                {
                  double num5 = 0.0;
                  for (int index = 0; index < val7.Length; ++index)
                  {
                    double val8;
                    try
                    {
                      val8 = measurementModuleMedia.s.isSet(val7[index]) ? (double) int.Parse(val7[index] ?? "") : 0.0;
                    }
                    catch
                    {
                      val8 = 0.0;
                    }
                    if (measurementModuleMedia.s.isSet(val8) || val7[index] == "E")
                    {
                      if (offset < val8 || val7[index] == "E")
                      {
                        segmentNum = index + 1;
                        segment = "O:" + (object) num5 + "-" + (object) val8;
                        index = val7.Length;
                      }
                      num5 = val8;
                    }
                  }
                }
              }
              if (measurementModuleMedia.s.isSet(segment))
                moduleMediaMediaItem1.segmentGenerated = true;
            }
            if ((measurementModuleMedia.s.isSet(segment) || measurementModuleMedia.s.isSet(moduleMediaMediaItem1.segment)) && segment != moduleMediaMediaItem1.segment)
            {
              moduleMediaMediaItem1.updateSegment = true;
              if (!measurementModuleMedia.s.isSet(moduleMediaMediaItem1.segment))
              {
                moduleMediaMediaItem1.segmentNum = segmentNum;
                moduleMediaMediaItem1.segment = segment;
              }
              if (moduleMediaMediaItem1.lastTrackOffset >= 0.0)
                flag = true;
            }
            if (eventType >= 2 && moduleMediaMediaItem1.lastEventOffset < offset)
            {
              moduleMediaMediaItem1.timePlayed += offset - moduleMediaMediaItem1.lastEventOffset;
              moduleMediaMediaItem1.timePlayedSinseTrack += offset - moduleMediaMediaItem1.lastEventOffset;
            }
            if (eventType <= 2 || eventType == 3 && !measurementModuleMedia.s.isSet(moduleMediaMediaItem1.lastEventType))
            {
              AppMeasurement_Module_Media.AppMeasurement_Module_Media_MediaItem moduleMediaMediaItem2 = moduleMediaMediaItem1;
              moduleMediaMediaItem2.session = moduleMediaMediaItem2.session + (eventType == 1 || eventType == 3 ? (object) "S" : (object) "E") + (object) Math.Floor(offset);
              moduleMediaMediaItem1.lastEventType = eventType == 3 ? 1 : eventType;
            }
            if (!flag && moduleMediaMediaItem1.lastTrackOffset >= 0.0 && eventType <= 3)
            {
              double val = measurementModuleMedia.s.isSet(trackSeconds) ? trackSeconds : 0.0;
              if (measurementModuleMedia.s.isSet(val) && moduleMediaMediaItem1.timePlayedSinseTrack >= val)
              {
                flag = true;
                media.mediaEvent = "SECONDS";
              }
            }
            moduleMediaMediaItem1.lastEventTimestamp = num1;
            moduleMediaMediaItem1.lastEventOffset = offset;
          }
          if (!measurementModuleMedia.s.isSet(eventType) || moduleMediaMediaItem1.percent >= 100.0)
          {
            eventType = 0;
            measurementModuleMedia.playerEvent(name, 2, -1.0, 0, (string) null, -1.0, playerData);
            str1 = str2 = "None";
            media.mediaEvent = "CLOSE";
          }
          if (eventType == 5 || measurementModuleMedia.s.isSet(measurementModuleMedia.completeByCloseOffset) && (!measurementModuleMedia.s.isSet(eventType) || moduleMediaMediaItem1.percent >= 100.0) && moduleMediaMediaItem1.length > 0.0 && offset >= moduleMediaMediaItem1.length - measurementModuleMedia.completeCloseOffsetThreshold)
          {
            media.complete = moduleMediaMediaItem1.complete = true;
            flag = true;
          }
          string key = media.mediaEvent;
          switch (key)
          {
            case "MILESTONE":
              key = key + "_" + (object) media.milestone;
              break;
            case "OFFSET_MILESTONE":
              key = key + "_" + (object) media.offsetMilestone;
              break;
          }
          if (!moduleMediaMediaItem1.firstEventList.ContainsKey(key))
          {
            media.eventFirstTime = true;
            moduleMediaMediaItem1.firstEventList[key] = (object) 1;
          }
          else
            media.eventFirstTime = false;
          media.timePlayed = moduleMediaMediaItem1.timePlayed;
          media.segmentNum = moduleMediaMediaItem1.segmentNum;
          media.segment = moduleMediaMediaItem1.segment;
          media.segmentLength = moduleMediaMediaItem1.segmentLength;
          if (measurementModuleMedia.s.isSet((object) measurementModuleMedia.monitor) && eventType != 4)
            measurementModuleMedia.monitor(measurementModuleMedia.s, media);
          if (eventType == 0)
            measurementModuleMedia._delete(name);
          if (flag && moduleMediaMediaItem1.trackCount == trackCount)
          {
            Dictionary<string, object> variableOverrides = new Dictionary<string, object>();
            variableOverrides["contextData"] = (object) new Dictionary<string, object>();
            variableOverrides["linkTrackVars"] = (object) str1;
            variableOverrides["linkTrackEvents"] = (object) str2;
            if (!measurementModuleMedia.s.isSet(variableOverrides["linkTrackVars"]))
              variableOverrides["linkTrackVars"] = (object) "";
            if (!measurementModuleMedia.s.isSet(variableOverrides["linkTrackEvents"]))
              variableOverrides["linkTrackEvents"] = (object) "";
            if (measurementModuleMedia.s.isSet(measurementModuleMedia.trackUsingContextData))
              measurementModuleMedia.buildContextData(variableOverrides, moduleMediaMediaItem1);
            else
              measurementModuleMedia.buildPageEvent(variableOverrides, moduleMediaMediaItem1, eventType, offset);
            measurementModuleMedia.s.track(variableOverrides);
            if (moduleMediaMediaItem1.updateSegment)
            {
              moduleMediaMediaItem1.segmentNum = segmentNum;
              moduleMediaMediaItem1.segment = segment;
              moduleMediaMediaItem1.segmentChanged = true;
              moduleMediaMediaItem1.updateSegment = false;
            }
            else if (moduleMediaMediaItem1.timePlayedSinseTrack > 0.0)
              moduleMediaMediaItem1.segmentChanged = false;
            moduleMediaMediaItem1.session = "";
            moduleMediaMediaItem1.lastMilestone = moduleMediaMediaItem1.lastOffsetMilestone = 0;
            moduleMediaMediaItem1.timePlayedSinseTrack -= Math.Floor(moduleMediaMediaItem1.timePlayedSinseTrack);
            moduleMediaMediaItem1.lastTrackOffset = offset;
            ++moduleMediaMediaItem1.trackCount;
          }
        }
      }
      return moduleMediaMediaItem1;
    }

    private void autoEvent(
      string name,
      double length,
      string playerName,
      int eventType,
      double offset,
      int segmentNum,
      string segment,
      double segmentLength,
      object playerData,
      string playerID)
    {
      AppMeasurement_Module_Media measurementModuleMedia = this;
      bool flag = false;
      name = measurementModuleMedia.cleanName(name);
      if (!measurementModuleMedia.s.isSet(name) || measurementModuleMedia.autoTrackMediaLengthRequired && (!measurementModuleMedia.s.isSet(length) || length <= 0.0) || !measurementModuleMedia.s.isSet(playerName))
        return;
      if (!measurementModuleMedia.s.isSet((object) measurementModuleMedia.list) || !measurementModuleMedia.list.ContainsKey(name))
      {
        if (eventType == 1 || eventType == 3)
        {
          measurementModuleMedia.open(name, length, playerName, playerID);
          flag = true;
        }
      }
      else
        flag = true;
      if (!flag)
        return;
      measurementModuleMedia.playerEvent(name, eventType, offset, segmentNum, segment, segmentLength, playerData);
    }

    public bool attach(object node)
    {
      AppMeasurement_Module_Media measurementModuleMedia = this;
      bool flag = false;
      if (!measurementModuleMedia.autoTrack)
        return flag;
      measurementModuleMedia.s.isSet(node);
      return flag;
    }

    private class AppMeasurement_Module_Media_MediaItem
    {
      public string name = "";
      public double length;
      public double offset;
      public double percent;
      public string playerName = "";
      public string playerID = "";
      public double timePlayed;
      public double timePlayedSinseTrack;
      public double timestamp;
      public int lastEventType;
      public double lastEventTimestamp;
      public double lastEventOffset;
      public string session = "";
      public double lastTrackOffset;
      public int trackCount;
      public Dictionary<string, object> firstEventList;
      public object playerData;
      public bool viewTracked;
      public int segmentNum;
      public string segment = "";
      public double segmentLength;
      public bool segmentGenerated;
      public bool segmentChanged;
      public bool updateSegment;
      public bool complete;
      public bool completeTracked;
      public int lastMilestone;
      public int lastOffsetMilestone;
      public AppMeasurement_Module_Media.AppMeasurement_Module_Media_PlayerMonitor monitor;
    }

    private delegate void AppMeasurement_Module_Media_PlayerMonitorHandler(
      AppMeasurement_Module_Media.AppMeasurement_Module_Media_PlayerMonitor monitor);

    private class AppMeasurement_Module_Media_PlayerMonitor : AppMeasurement_SetInterval
    {
      private static List<object> monitorList;
      public AppMeasurement_Module_Media m;
      public object node;
      public AppMeasurement_Module_Media.AppMeasurement_Module_Media_PlayerMonitorHandler monitor;
      private int interval;

      public AppMeasurement_Module_Media_PlayerMonitor()
      {
        this.m = (AppMeasurement_Module_Media) null;
        this.node = (object) null;
        this.monitor = (AppMeasurement_Module_Media.AppMeasurement_Module_Media_PlayerMonitorHandler) null;
        this.interval = 0;
      }

      public void start()
      {
        if (AppMeasurement_Module_Media.AppMeasurement_Module_Media_PlayerMonitor.monitorList == null)
          AppMeasurement_Module_Media.AppMeasurement_Module_Media_PlayerMonitor.monitorList = new List<object>();
        AppMeasurement_Module_Media.AppMeasurement_Module_Media_PlayerMonitor.monitorList.Add((object) this);
        this.update((object) null, (object) null);
      }

      public void stop()
      {
        if (this.interval > 0)
          AppMeasurement_SetInterval.clearInterval(this.interval);
        if (AppMeasurement_Module_Media.AppMeasurement_Module_Media_PlayerMonitor.monitorList == null)
          return;
        AppMeasurement_Module_Media.AppMeasurement_Module_Media_PlayerMonitor.monitorList.Remove((object) this);
      }

      private void update(object s, object e)
      {
        string key = (string) null;
        try
        {
          if (key != null)
            key = this.node as string;
        }
        catch
        {
          key = (string) null;
        }
        if (this.m == null || this.m.s == null || this.node == null || key != null && !this.m.list.ContainsKey(key))
          this.stop();
        else
          this.monitor(this);
        if (this.interval > 0)
          return;
        this.interval = AppMeasurement_SetInterval.setInterval(new EventHandler(this.update), this.m.trackWhilePlaying ? 1000 : 5000);
      }
    }
  }
}
