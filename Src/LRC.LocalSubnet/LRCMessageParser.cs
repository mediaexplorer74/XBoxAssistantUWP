// *********************************************************
// Type: LRC.LocalSubnet.LRCMessageParser
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll

using Microsoft.Xmedia.Client.WindowsPhone;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;


namespace LRC.LocalSubnet
{
  public class LRCMessageParser
  {
    private const string ComponentName = "LRCMessageParser";
    private const int MaxMediaAssetIdLength = 1024;
    private IntIteractor intIterator = new IntIteractor();

    public static uint GetDataSize(uint messageLength)
    {
      return messageLength > 4294967291U ? 0U : messageLength + 4U;
    }

    [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "Need to cast")]
    public static byte[] CreateBinaryRequest(LRC_REQUEST request)
    {
      switch (request)
      {
        case LRC_JOIN_SESSION_REQUEST _:
          return LRCMessageParser.CreateJoinSessionRequest((LRC_JOIN_SESSION_REQUEST) request);
        case LRC_LEAVE_SESSION_REQUEST _:
          return LRCMessageParser.CreateLeaveSessionRequest((LRC_LEAVE_SESSION_REQUEST) request);
        case LRC_GET_ACTIVE_TITLEID_REQUEST _:
          return LRCMessageParser.CreateGetActiveTitleIdRequest((LRC_GET_ACTIVE_TITLEID_REQUEST) request);
        case LRC_LAUNCH_TITLE_REQUEST _:
          return LRCMessageParser.CreateLaunchTitleRequest((LRC_LAUNCH_TITLE_REQUEST) request);
        case LRC_SEND_INPUT_REQUEST _:
          return LRCMessageParser.CreateSendInputRequest((LRC_SEND_INPUT_REQUEST) request);
        case LRC_GET_MEDIA_TITLE_STATE_REQUEST _:
          return LRCMessageParser.CreateGetMediaTitleStateRequest((LRC_GET_MEDIA_TITLE_STATE_REQUEST) request);
        case LRC_GET_CONSOLE_SETTINGS_REQUEST _:
          return LRCMessageParser.CreateGetConsoleSettingsRequest((LRC_GET_CONSOLE_SETTINGS_REQUEST) request);
        default:
          throw new ArgumentException("Unknown request type");
      }
    }

    public static MediaState ConvertMediaStateObject(LRC_MEDIA_TITLE_STATE_PARAMS param)
    {
      return new MediaState()
      {
        TitleId = param.TitleId,
        ContentId = param.MediaAssetId,
        Position = param.Position,
        Rate = param.Rate,
        MinimumSeekPosition = param.MinSeek,
        MaximumSeekPosition = param.MaxSeek,
        MediaTransportCapabilities = (MediaTransportCapabilities) param.TransportCapabilities,
        MediaTransportState = (MediaTransportState) param.TransportState,
        Duration = param.Duration
      };
    }

    public List<object> ParseMessages(byte[] buffer)
    {
      int srcOffset = 0;
      List<object> messages = new List<object>();
      if (buffer == null || buffer.Length < 0)
        throw new ArgumentException("Invalid buffer");
      uint dataSize;
      for (; srcOffset < buffer.Length; srcOffset += (int) dataSize)
      {
        this.intIterator.Reset();
        if (buffer.Length - srcOffset >= 32)
        {
          byte[] numArray1 = new byte[32];
          Buffer.BlockCopy((Array) buffer, srcOffset, (Array) numArray1, 0, numArray1.Length);
          LRC_NOTIFICATION_HEADER header = new LRC_NOTIFICATION_HEADER();
          this.ParseNotificationHeader(ref header, numArray1);
          if (header.Signature == 3202006746U)
          {
            dataSize = LRCMessageParser.GetDataSize(header.MessageLength);
            if ((long) (buffer.Length - srcOffset) < (long) dataSize)
              throw new ArgumentException("Invalid response size: Remaining buffer is inconsistent with the message length");
            byte[] numArray2 = new byte[(int) dataSize];
            Buffer.BlockCopy((Array) buffer, srcOffset, (Array) numArray2, 0, numArray2.Length);
            switch (header.MessageKind)
            {
              case 2:
                switch (header.MessageType)
                {
                  case 1:
                    messages.Add((object) this.ParseResponse<LRC_GET_ACTIVE_TITLEID_RESPONSE>(numArray2));
                    continue;
                  case 2:
                    messages.Add((object) this.ParseResponse<LRC_LAUNCH_TITLE_RESPONSE>(numArray2));
                    continue;
                  case 3:
                    messages.Add((object) this.ParseResponse<LRC_SEND_INPUT_RESPONSE>(numArray2));
                    continue;
                  case 4:
                    messages.Add((object) this.ParseResponse<LRC_GET_MEDIA_TITLE_STATE_RESPONSE>(numArray2));
                    continue;
                  case 12:
                    messages.Add((object) this.ParseResponse<LRC_GET_CONSOLE_SETTINGS_RESPONSE>(numArray2));
                    continue;
                  case 2147483649:
                    messages.Add((object) this.ParseResponse<LRC_JOIN_SESSION_RESPONSE>(numArray2));
                    continue;
                  case 2147483650:
                    messages.Add((object) this.ParseResponse<LRC_LEAVE_SESSION_RESPONSE>(numArray2));
                    continue;
                  default:
                    continue;
                }
              case 3:
                switch (header.MessageType)
                {
                  case 5:
                    messages.Add((object) this.ParseNotification<LRC_NON_MEDIA_TITLE_STATE_NOTIFICATION>(header, numArray2));
                    continue;
                  case 6:
                    messages.Add((object) this.ParseNotification<LRC_MEDIA_TITLE_STATE_NOTIFICATION>(header, numArray2));
                    continue;
                  default:
                    continue;
                }
              case 2048:
                messages.Add((object) this.ParsePresenceNotification(numArray2));
                break;
            }
          }
          else
            break;
        }
        else
          break;
      }
      return messages;
    }

    public T ParseResponse<T>(byte[] buffer)
    {
      if (buffer == null || buffer.Length < 0)
        throw new ArgumentException("Invalid buffer");
      this.intIterator.Reset();
      if ((object) typeof (T) == (object) typeof (LRC_JOIN_SESSION_RESPONSE))
        return buffer.Length >= 72 ? (T) (object) this.ParseJoinSessionResponse(buffer) : throw new ArgumentException("Invalid response size");
      if ((object) typeof (T) == (object) typeof (LRC_LEAVE_SESSION_RESPONSE))
        return buffer.Length >= 60 ? (T) (object) this.ParseLeaveSessionResponse(buffer) : throw new ArgumentException("Invalid response size");
      if ((object) typeof (T) == (object) typeof (LRC_GET_ACTIVE_TITLEID_RESPONSE))
        return buffer.Length >= 64 ? (T) (object) this.ParseGetActiveTitleIdResponse(buffer) : throw new ArgumentException("Invalid response size");
      if ((object) typeof (T) == (object) typeof (LRC_LAUNCH_TITLE_RESPONSE))
        return buffer.Length >= 60 ? (T) (object) this.ParseLaunchTitleResponse(buffer) : throw new ArgumentException("Invalid response size");
      if ((object) typeof (T) == (object) typeof (LRC_SEND_INPUT_RESPONSE))
        return buffer.Length >= 60 ? (T) (object) this.ParseSendInputResponse(buffer) : throw new ArgumentException("Invalid response size");
      if ((object) typeof (T) == (object) typeof (LRC_GET_MEDIA_TITLE_STATE_RESPONSE))
        return buffer.Length >= 368 ? (T) (object) this.ParseMediaTitleStateResponse(buffer) : throw new ArgumentException("Invalid response size");
      if ((object) typeof (T) != (object) typeof (LRC_GET_CONSOLE_SETTINGS_RESPONSE))
        throw new ArgumentException("Unknow response type");
      return buffer.Length >= 74 ? (T) (object) this.ParseGetConsoleSettingsResponse(buffer) : throw new ArgumentException("Invalid response size");
    }

    public object ParseNotification(byte[] buffer)
    {
      if (buffer == null || buffer.Length < 0)
        throw new ArgumentException("Invalid buffer");
      this.intIterator.Reset();
      if (buffer.Length < 52)
        throw new ArgumentException("Invalid notification size");
      LRC_NOTIFICATION_HEADER header = new LRC_NOTIFICATION_HEADER();
      this.ParseNotificationHeader(ref header, buffer);
      if (header.MessageType == 5U)
        return buffer.Length >= 56 ? (object) this.ParseNotification<LRC_NON_MEDIA_TITLE_STATE_NOTIFICATION>(header, buffer) : throw new ArgumentException("Invalid notification size");
      if (header.MessageType != 6U)
        return (object) null;
      return buffer.Length >= 360 ? (object) this.ParseNotification<LRC_MEDIA_TITLE_STATE_NOTIFICATION>(header, buffer) : throw new ArgumentException("Invalid notification size");
    }

    public T ParseNotification<T>(LRC_NOTIFICATION_HEADER header, byte[] buffer)
    {
      if (buffer == null || buffer.Length < 0)
        throw new ArgumentException("Invalid buffer");
      if ((object) typeof (T) == (object) typeof (LRC_NON_MEDIA_TITLE_STATE_NOTIFICATION))
        return buffer.Length >= 56 ? (T) (object) this.ParseNonMediaTitleStateNotification(header, buffer) : throw new ArgumentException("Invalid notification size");
      if ((object) typeof (T) != (object) typeof (LRC_MEDIA_TITLE_STATE_NOTIFICATION))
        throw new ArgumentException("Unknow notification type");
      return buffer.Length >= 360 ? (T) (object) this.ParseMediaTitleStateNotification(header, buffer) : throw new ArgumentException("Invalid notification size");
    }

    public LRC_PRESENCE_NOTIFICATION ParsePresenceNotification(byte[] buffer)
    {
      if (buffer == null || buffer.Length <= 0)
        throw new ArgumentException("Invalid buffer");
      this.intIterator.Reset();
      if (buffer.Length < 117)
        throw new ArgumentException("Invalid notification size");
      LRC_PRESENCE_NOTIFICATION presenceNotification = new LRC_PRESENCE_NOTIFICATION();
      this.ParseNotificationHeader(ref presenceNotification.Header, buffer);
      if (presenceNotification.Header.MessageType != 1U && presenceNotification.Header.MessageType != 2U)
        return (LRC_PRESENCE_NOTIFICATION) null;
      presenceNotification.Params.ClientId = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      presenceNotification.Params.DeviceType = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      int count = 0;
      uint num = this.intIterator.Next();
      for (int index = 0; index < 65; ++index)
      {
        count = index;
        if (buffer[(long) num + (long) index] == (byte) 0)
          break;
      }
      byte[] numArray = new byte[count];
      Buffer.BlockCopy((Array) buffer, (int) this.intIterator.Next(), (Array) numArray, 0, count);
      presenceNotification.Params.UserDisplayNames = new string(Encoding.UTF8.GetChars(numArray));
      return presenceNotification;
    }

    internal static ushort Swapushort(ushort inValue)
    {
      byte[] bytes = BitConverter.GetBytes(inValue);
      Array.Reverse((Array) bytes);
      return BitConverter.ToUInt16(bytes, 0);
    }

    internal static uint Swapuint(uint inValue)
    {
      byte[] bytes = BitConverter.GetBytes(inValue);
      Array.Reverse((Array) bytes);
      return BitConverter.ToUInt32(bytes, 0);
    }

    internal static ulong Swapulong(ulong inValue)
    {
      byte[] bytes = BitConverter.GetBytes(inValue);
      Array.Reverse((Array) bytes);
      return BitConverter.ToUInt64(bytes, 0);
    }

    private static void SwapNotificationHeader(ref LRC_NOTIFICATION_HEADER header)
    {
      header.Signature = LRCMessageParser.Swapuint(header.Signature);
      header.MessageLength = LRCMessageParser.Swapuint(header.MessageLength);
      header.SequenceNumber = LRCMessageParser.Swapuint(header.SequenceNumber);
      header.ProtocolVersion = LRCMessageParser.Swapuint(header.ProtocolVersion);
      header.ToClientId = LRCMessageParser.Swapuint(header.ToClientId);
      header.FromClientId = LRCMessageParser.Swapuint(header.FromClientId);
      header.MessageKind = LRCMessageParser.Swapuint(header.MessageKind);
      header.MessageType = LRCMessageParser.Swapuint(header.MessageType);
    }

    private static void SerializeHeader(
      LRC_REQUEST_HEADER header,
      ref byte[] packetData,
      ref int destPosition)
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(header.Signature), 0, (Array) packetData, destPosition, 4);
      destPosition += 4;
      Buffer.BlockCopy((Array) BitConverter.GetBytes(header.MessageLength), 0, (Array) packetData, destPosition, 4);
      destPosition += 4;
      Buffer.BlockCopy((Array) BitConverter.GetBytes(header.SequenceNumber), 0, (Array) packetData, destPosition, 4);
      destPosition += 4;
      Buffer.BlockCopy((Array) BitConverter.GetBytes(header.ProtocolVersion), 0, (Array) packetData, destPosition, 4);
      destPosition += 4;
      Buffer.BlockCopy((Array) BitConverter.GetBytes(header.ToClientId), 0, (Array) packetData, destPosition, 4);
      destPosition += 4;
      Buffer.BlockCopy((Array) BitConverter.GetBytes(header.FromClientId), 0, (Array) packetData, destPosition, 4);
      destPosition += 4;
      Buffer.BlockCopy((Array) BitConverter.GetBytes(header.MessageKind), 0, (Array) packetData, destPosition, 4);
      destPosition += 4;
      Buffer.BlockCopy((Array) BitConverter.GetBytes(header.MessageType), 0, (Array) packetData, destPosition, 4);
      destPosition += 4;
    }

    private static void SwapResponseHeader(ref LRC_RESPONSE_HEADER header)
    {
      header.Signature = LRCMessageParser.Swapuint(header.Signature);
      header.MessageLength = LRCMessageParser.Swapuint(header.MessageLength);
      header.SequenceNumber = LRCMessageParser.Swapuint(header.SequenceNumber);
      header.ProtocolVersion = LRCMessageParser.Swapuint(header.ProtocolVersion);
      header.ToClientId = LRCMessageParser.Swapuint(header.ToClientId);
      header.FromClientId = LRCMessageParser.Swapuint(header.FromClientId);
      header.MessageKind = LRCMessageParser.Swapuint(header.MessageKind);
      header.MessageType = LRCMessageParser.Swapuint(header.MessageType);
      header.ResponseTo = LRCMessageParser.Swapuint(header.ResponseTo);
      header.ResultCode = LRCMessageParser.Swapuint(header.ResultCode);
    }

    private static uint UIntFromBytes(byte[] bytes, uint offset)
    {
      byte[] numArray = new byte[4];
      for (int index = 0; index <= 3; ++index)
        numArray[3 - index] = bytes[(long) index + (long) offset];
      return BitConverter.ToUInt32(numArray, 0);
    }

    private static float FloatFromBytes(byte[] bytes, uint offset)
    {
      byte[] numArray = new byte[4];
      for (int index = 0; index <= 3; ++index)
        numArray[3 - index] = bytes[(long) index + (long) offset];
      return BitConverter.ToSingle(numArray, 0);
    }

    private static ulong UlongFromBytes(byte[] bytes, ulong offset)
    {
      byte[] numArray = new byte[8];
      for (ulong index = 0; index <= 7UL; ++index)
        numArray[checked ((ulong) unchecked (7L - (long) index))] = bytes[checked ((ulong) unchecked ((long) index + (long) offset))];
      return BitConverter.ToUInt64(numArray, 0);
    }

    private static void FillUpHeaderInfo(
      ref LRC_REQUEST_HEADER header,
      LRC_MESSAGE_TYPE messageType,
      uint dataSize)
    {
      header.Signature = LRCMessageParser.Swapuint(header.Signature);
      header.MessageLength = LRCMessageParser.Swapuint(LRCMessageParser.GetMessageLength(dataSize));
      header.ProtocolVersion = LRCMessageParser.Swapuint(1U);
      header.MessageKind = LRCMessageParser.Swapuint(1U);
      header.MessageType = LRCMessageParser.Swapuint((uint) messageType);
      header.SequenceNumber = LRCMessageParser.Swapuint(header.SequenceNumber);
      header.ToClientId = LRCMessageParser.Swapuint(header.ToClientId);
      header.FromClientId = LRCMessageParser.Swapuint(header.FromClientId);
    }

    private static uint GetMessageLength(uint dataSize) => dataSize - 4U;

    private static byte[] CreateJoinSessionRequest(LRC_JOIN_SESSION_REQUEST request)
    {
      int destPosition = 0;
      byte[] packetData = new byte[(IntPtr) LRCCrypto.CalculateBufferSize(52U)];
      LRC_REQUEST_HEADER header = new LRC_REQUEST_HEADER(request.Header);
      LRCMessageParser.FillUpHeaderInfo(ref header, LRC_MESSAGE_TYPE.LRC_MESSAGE_JOIN_SESSION, 52U);
      LRCMessageParser.SerializeHeader(header, ref packetData, ref destPosition);
      return packetData;
    }

    private static byte[] CreateLeaveSessionRequest(LRC_LEAVE_SESSION_REQUEST request)
    {
      int destPosition = 0;
      byte[] packetData = new byte[(IntPtr) LRCCrypto.CalculateBufferSize(52U)];
      LRC_REQUEST_HEADER header = new LRC_REQUEST_HEADER(request.Header);
      LRCMessageParser.FillUpHeaderInfo(ref header, LRC_MESSAGE_TYPE.LRC_MESSAGE_LEAVE_SESSION, 52U);
      LRCMessageParser.SerializeHeader(header, ref packetData, ref destPosition);
      return packetData;
    }

    private static byte[] CreateGetActiveTitleIdRequest(LRC_GET_ACTIVE_TITLEID_REQUEST request)
    {
      int destPosition = 0;
      byte[] packetData = new byte[(IntPtr) LRCCrypto.CalculateBufferSize(52U)];
      LRC_REQUEST_HEADER header = new LRC_REQUEST_HEADER(request.Header);
      LRCMessageParser.FillUpHeaderInfo(ref header, LRC_MESSAGE_TYPE.LRC_MESSAGE_GET_ACTIVE_TITLEID, 52U);
      LRCMessageParser.SerializeHeader(header, ref packetData, ref destPosition);
      return packetData;
    }

    private static byte[] CreateLaunchTitleRequest(LRC_LAUNCH_TITLE_REQUEST request)
    {
      if (request.Params.LaunchParameterLength >= 900U)
        throw new ArgumentException("LaunchParameter too long");
      int destPosition = 0;
      byte[] packetData = new byte[(IntPtr) LRCCrypto.CalculateBufferSize(960U)];
      LRC_REQUEST_HEADER header = new LRC_REQUEST_HEADER(request.Header);
      LRCMessageParser.FillUpHeaderInfo(ref header, LRC_MESSAGE_TYPE.LRC_MESSAGE_LAUNCH_TITLE, 960U);
      LRCMessageParser.SerializeHeader(header, ref packetData, ref destPosition);
      request.Params.TitleId = LRCMessageParser.Swapuint(request.Params.TitleId);
      request.Params.LaunchParameterLength = LRCMessageParser.Swapuint(request.Params.LaunchParameterLength);
      Buffer.BlockCopy((Array) BitConverter.GetBytes(request.Params.TitleId), 0, (Array) packetData, destPosition, 4);
      int dstOffset1 = destPosition + 4;
      Buffer.BlockCopy((Array) BitConverter.GetBytes(request.Params.LaunchParameterLength), 0, (Array) packetData, dstOffset1, 4);
      int dstOffset2 = dstOffset1 + 4;
      if (request.Params.LaunchParameter != null)
      {
        Buffer.BlockCopy((Array) Encoding.UTF8.GetBytes(request.Params.LaunchParameter), 0, (Array) packetData, dstOffset2, request.Params.LaunchParameter.Length);
        int num = dstOffset2 + request.Params.LaunchParameter.Length;
      }
      return packetData;
    }

    private static byte[] CreateSendInputRequest(LRC_SEND_INPUT_REQUEST request)
    {
      int destPosition = 0;
      byte[] packetData = new byte[(IntPtr) LRCCrypto.CalculateBufferSize(68U)];
      LRC_REQUEST_HEADER header = new LRC_REQUEST_HEADER(request.Header);
      LRCMessageParser.FillUpHeaderInfo(ref header, LRC_MESSAGE_TYPE.LRC_MESSAGE_SEND_INPUT, 68U);
      LRCMessageParser.SerializeHeader(header, ref packetData, ref destPosition);
      request.Params.ValidFields = LRCMessageParser.Swapuint(request.Params.ValidFields);
      request.Params.VirtualKey = LRCMessageParser.Swapuint(request.Params.VirtualKey);
      request.Params.SeekPos = LRCMessageParser.Swapulong(request.Params.SeekPos);
      Buffer.BlockCopy((Array) BitConverter.GetBytes(request.Params.ValidFields), 0, (Array) packetData, destPosition, 4);
      int dstOffset1 = destPosition + 4;
      Buffer.BlockCopy((Array) BitConverter.GetBytes(request.Params.VirtualKey), 0, (Array) packetData, dstOffset1, 4);
      int dstOffset2 = dstOffset1 + 4;
      Buffer.BlockCopy((Array) BitConverter.GetBytes(request.Params.SeekPos), 0, (Array) packetData, dstOffset2, 8);
      int num = dstOffset2 + 4;
      return packetData;
    }

    private static byte[] CreateGetMediaTitleStateRequest(LRC_GET_MEDIA_TITLE_STATE_REQUEST request)
    {
      int destPosition = 0;
      byte[] packetData = new byte[(IntPtr) LRCCrypto.CalculateBufferSize(52U)];
      LRC_REQUEST_HEADER header = new LRC_REQUEST_HEADER(request.Header);
      LRCMessageParser.FillUpHeaderInfo(ref header, LRC_MESSAGE_TYPE.LRC_MESSAGE_GET_MEDIA_TITLE_STATE, 52U);
      LRCMessageParser.SerializeHeader(header, ref packetData, ref destPosition);
      return packetData;
    }

    private static byte[] CreateGetConsoleSettingsRequest(LRC_GET_CONSOLE_SETTINGS_REQUEST request)
    {
      int destPosition = 0;
      byte[] packetData = new byte[(IntPtr) LRCCrypto.CalculateBufferSize(52U)];
      LRC_REQUEST_HEADER header = new LRC_REQUEST_HEADER(request.Header);
      LRCMessageParser.FillUpHeaderInfo(ref header, LRC_MESSAGE_TYPE.LRC_MESSAGE_GET_CONSOLE_SETTINGS, 52U);
      LRCMessageParser.SerializeHeader(header, ref packetData, ref destPosition);
      return packetData;
    }

    private LRC_JOIN_SESSION_RESPONSE ParseJoinSessionResponse(byte[] buffer)
    {
      LRC_JOIN_SESSION_RESPONSE joinSessionResponse = new LRC_JOIN_SESSION_RESPONSE();
      this.ParseResponseHeader(ref joinSessionResponse.Header, buffer);
      joinSessionResponse.Params.SupportedProtocolVersion = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      joinSessionResponse.Params.SessionSequenceNumber = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      joinSessionResponse.Params.NotificationSequenceNumber = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      return joinSessionResponse;
    }

    private LRC_LEAVE_SESSION_RESPONSE ParseLeaveSessionResponse(byte[] buffer)
    {
      LRC_LEAVE_SESSION_RESPONSE leaveSessionResponse = new LRC_LEAVE_SESSION_RESPONSE();
      this.ParseResponseHeader(ref leaveSessionResponse.Header, buffer);
      return leaveSessionResponse;
    }

    private LRC_GET_ACTIVE_TITLEID_RESPONSE ParseGetActiveTitleIdResponse(byte[] buffer)
    {
      LRC_GET_ACTIVE_TITLEID_RESPONSE activeTitleIdResponse = new LRC_GET_ACTIVE_TITLEID_RESPONSE();
      this.ParseResponseHeader(ref activeTitleIdResponse.Header, buffer);
      activeTitleIdResponse.Params.TitleId = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      return activeTitleIdResponse;
    }

    private LRC_LAUNCH_TITLE_RESPONSE ParseLaunchTitleResponse(byte[] buffer)
    {
      LRC_LAUNCH_TITLE_RESPONSE launchTitleResponse = new LRC_LAUNCH_TITLE_RESPONSE();
      this.ParseResponseHeader(ref launchTitleResponse.Header, buffer);
      return launchTitleResponse;
    }

    private LRC_SEND_INPUT_RESPONSE ParseSendInputResponse(byte[] buffer)
    {
      LRC_SEND_INPUT_RESPONSE sendInputResponse = new LRC_SEND_INPUT_RESPONSE();
      this.ParseResponseHeader(ref sendInputResponse.Header, buffer);
      return sendInputResponse;
    }

    private LRC_GET_MEDIA_TITLE_STATE_RESPONSE ParseMediaTitleStateResponse(byte[] buffer)
    {
      LRC_GET_MEDIA_TITLE_STATE_RESPONSE titleStateResponse = new LRC_GET_MEDIA_TITLE_STATE_RESPONSE();
      this.ParseResponseHeader(ref titleStateResponse.Header, buffer);
      this.ParseMediaTitleStateParams(ref titleStateResponse.Params, buffer);
      return titleStateResponse;
    }

    private LRC_GET_CONSOLE_SETTINGS_RESPONSE ParseGetConsoleSettingsResponse(byte[] buffer)
    {
      LRC_GET_CONSOLE_SETTINGS_RESPONSE settingsResponse = new LRC_GET_CONSOLE_SETTINGS_RESPONSE();
      this.ParseResponseHeader(ref settingsResponse.Header, buffer);
      settingsResponse.Params.LiveTvProvider = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      byte[] numArray = new byte[14];
      Buffer.BlockCopy((Array) buffer, (int) this.intIterator.Next(), (Array) numArray, 0, 14);
      string str = new string(Encoding.BigEndianUnicode.GetChars(numArray));
      if (!string.IsNullOrEmpty(str))
        settingsResponse.Params.Locale = str.TrimEnd(new char[1]);
      settingsResponse.Params.FlashVersion = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      return settingsResponse;
    }

    private void ParseResponseHeader(ref LRC_RESPONSE_HEADER header, byte[] buffer)
    {
      header.Signature = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.MessageLength = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.SequenceNumber = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.ProtocolVersion = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.ToClientId = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.FromClientId = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.MessageKind = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.MessageType = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.ResponseTo = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.ResultCode = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
    }

    private LRC_NON_MEDIA_TITLE_STATE_NOTIFICATION ParseNonMediaTitleStateNotification(
      LRC_NOTIFICATION_HEADER header,
      byte[] buffer)
    {
      LRC_NON_MEDIA_TITLE_STATE_NOTIFICATION stateNotification = new LRC_NON_MEDIA_TITLE_STATE_NOTIFICATION();
      stateNotification.Header = header;
      stateNotification.Params.TitleId = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      return stateNotification;
    }

    private LRC_MEDIA_TITLE_STATE_NOTIFICATION ParseMediaTitleStateNotification(
      LRC_NOTIFICATION_HEADER header,
      byte[] buffer)
    {
      LRC_MEDIA_TITLE_STATE_NOTIFICATION stateNotification = new LRC_MEDIA_TITLE_STATE_NOTIFICATION();
      stateNotification.Header = header;
      this.ParseMediaTitleStateParams(ref stateNotification.Params, buffer);
      return stateNotification;
    }

    private void ParseMediaTitleStateParams(
      ref LRC_MEDIA_TITLE_STATE_PARAMS mediaState,
      byte[] buffer)
    {
      mediaState = new LRC_MEDIA_TITLE_STATE_PARAMS();
      mediaState.TitleId = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      mediaState.Duration = LRCMessageParser.UlongFromBytes(buffer, this.intIterator.NextLong());
      mediaState.Position = LRCMessageParser.UlongFromBytes(buffer, this.intIterator.NextLong());
      mediaState.MinSeek = LRCMessageParser.UlongFromBytes(buffer, this.intIterator.NextLong());
      mediaState.MaxSeek = LRCMessageParser.UlongFromBytes(buffer, this.intIterator.NextLong());
      mediaState.Rate = LRCMessageParser.FloatFromBytes(buffer, this.intIterator.Next());
      mediaState.TransportState = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      mediaState.TransportCapabilities = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      mediaState.MediaAssetIdLength = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      if (mediaState.MediaAssetIdLength > 1024U)
        return;
      byte[] numArray = new byte[(IntPtr) mediaState.MediaAssetIdLength];
      Buffer.BlockCopy((Array) buffer, (int) this.intIterator.Next(), (Array) numArray, 0, (int) mediaState.MediaAssetIdLength);
      mediaState.MediaAssetId = new string(Encoding.UTF8.GetChars(numArray));
    }

    private void ParseNotificationHeader(ref LRC_NOTIFICATION_HEADER header, byte[] buffer)
    {
      header.Signature = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.MessageLength = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.SequenceNumber = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.ProtocolVersion = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.ToClientId = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.FromClientId = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.MessageKind = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
      header.MessageType = LRCMessageParser.UIntFromBytes(buffer, this.intIterator.Next());
    }
  }
}
