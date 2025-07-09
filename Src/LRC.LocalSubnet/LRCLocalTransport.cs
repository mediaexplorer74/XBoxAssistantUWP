// *********************************************************
// Type: LRC.LocalSubnet.LRCLocalTransport
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll

using Microsoft.Xmedia.Client.WindowsPhone;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Xbox.Live.Phone.Utils;


namespace LRC.LocalSubnet
{
  [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Objects are disposed after each operation, such as socket and SocketEventArgs")]
  public class LRCLocalTransport : IDisposable
  {
    public const string ComponentName = "LRCLocalTransport";
    public const string MulticastAddress = "239.255.255.250";
    public const int NotificationPort = 1902;
    private const int MaxBufferSize = 1025;
    private const int SocketTimeOutMilliSeconds = 5000;
    private byte[] notificationBuffer = new byte[4096];
    private IntIteractor intIterator = new IntIteractor();
    private Socket clientSocket;
    private UdpAnySourceMulticastClient udpClient;
    private LRCLocalTransport.MulticastState multicastState;
    private IAsyncResult currentNotificationAsyncResult;
    private bool stopNotificationReceiver;
    private byte[] sendPacketBuffer;
    private byte[] buffer = new byte[1025];
    private int bufferOffset;
    private string hostIPAddress;
    private int hostPortNumber;
    private Timer socketTimer;
    private bool isOperationPending;
    private ulong requestId;
    private bool disposed;
    private bool isStopping;

    public LRCLocalTransport(string hostIP, int portNumber)
    {
      this.isStopping = false;
      this.requestId = 0UL;
      this.isOperationPending = false;
      this.hostIPAddress = hostIP;
      this.hostPortNumber = portNumber;
    }

    ~LRCLocalTransport() => this.Dispose(false);

    public event EventHandler<XmediaMessageReceivedEventArgs> MessageReceived;

    public Exception LastError { get; private set; }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public void StartIO()
    {
      lock (this)
        this.isStopping = false;
    }

    public void StopIO()
    {
      lock (this)
      {
        this.isStopping = true;
        this.StopMulticastListener();
      }
    }

    [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "SocketEventArgs is disposed when operation is completed")]
    public void SendTCPCommand(byte[] requestBuffer)
    {
      requestBuffer = LRCCrypto.EncryptLrcMessage(requestBuffer);
      if (requestBuffer == null)
        throw new XMobileException(3003, "Failed to encrypt LRC message request");
      lock (this)
      {
        try
        {
          this.LastError = (Exception) null;
          this.sendPacketBuffer = requestBuffer;
          SocketAsyncEventArgs e = new SocketAsyncEventArgs();
          DnsEndPoint dnsEndPoint = new DnsEndPoint(this.hostIPAddress, this.hostPortNumber);
          this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
          this.clientSocket.NoDelay = true;
          e.Completed += new EventHandler<SocketAsyncEventArgs>(this.SocketEventArg_Completed);
          e.RemoteEndPoint = (EndPoint) dnsEndPoint;
          e.UserToken = (object) this.clientSocket;
          this.isOperationPending = true;
          this.clientSocket.ConnectAsync(e);
          this.StartSocketTimer();
        }
        catch (SocketException ex)
        {
          throw new XMobileException((Exception) ex, 2010, "SendTCPCommand CanNotConnect: ");
        }
      }
    }

    public void StartMulticastListener()
    {
      try
      {
        this.StopMulticastListener();
        this.udpClient = new UdpAnySourceMulticastClient(IPAddress.Parse("239.255.255.250"), 1902);
        this.multicastState = LRCLocalTransport.MulticastState.Joining;
        this.currentNotificationAsyncResult = this.udpClient.BeginJoinGroup((AsyncCallback) (r =>
        {
          try
          {
            this.udpClient.EndJoinGroup(r);
            this.currentNotificationAsyncResult = (IAsyncResult) null;
            this.Listen();
          }
          catch (SocketException ex)
          {
          }
        }), (object) null);
      }
      catch (SocketException ex)
      {
      }
    }

    public void StopMulticastListener()
    {
      if (this.udpClient == null)
        return;
      this.multicastState = LRCLocalTransport.MulticastState.Closing;
      this.udpClient.Dispose();
      this.udpClient = (UdpAnySourceMulticastClient) null;
      this.multicastState = LRCLocalTransport.MulticastState.Uninitialized;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        if (this.socketTimer != null)
          this.StopSocketTimer();
        if (this.clientSocket != null)
        {
          this.clientSocket.Dispose();
          this.clientSocket = (Socket) null;
        }
        if (this.udpClient != null)
          this.StopMulticastListener();
      }
      this.disposed = true;
    }

    private void SocketEventArg_Completed(object sender, SocketAsyncEventArgs e)
    {
      lock (this)
      {
        if (this.isStopping)
        {
          this.CompleteCurrentOperation(e, (Exception) new SocketException(995), (byte[]) null);
        }
        else
        {
          switch (e.LastOperation)
          {
            case SocketAsyncOperation.Connect:
              this.ProcessConnect(e);
              break;
            case SocketAsyncOperation.Receive:
              this.ProcessReceive(e);
              break;
            case SocketAsyncOperation.Send:
              this.ProcessSend(e);
              break;
            default:
              this.CompleteCurrentOperation(e, (Exception) new XMobileException(2013, "Invalid operation completed"), (byte[]) null);
              break;
          }
        }
      }
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception will be sent to caller")]
    private void ProcessConnect(SocketAsyncEventArgs e)
    {
      try
      {
        this.StopSocketTimer();
        if (e.SocketError == SocketError.Success)
        {
          e.SetBuffer(this.sendPacketBuffer, 0, this.sendPacketBuffer.Length);
          if ((e.UserToken as Socket).SendAsync(e))
            return;
          this.ProcessSend(e);
        }
        else
          this.CompleteCurrentOperation(e, (Exception) new SocketException((int) e.SocketError), (byte[]) null);
      }
      catch (Exception ex)
      {
        this.CompleteCurrentOperation(e, ex, (byte[]) null);
      }
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception will be sent to caller")]
    private void ProcessReceive(SocketAsyncEventArgs e)
    {
      Exception ex1 = (Exception) null;
      try
      {
        if (e.SocketError == SocketError.Success)
        {
          Socket userToken = e.UserToken as Socket;
          if (e.BytesTransferred > 0)
          {
            if (1025 > this.bufferOffset + e.BytesTransferred)
            {
              Buffer.BlockCopy((Array) e.Buffer, 0, (Array) this.buffer, this.bufferOffset, e.BytesTransferred);
              this.bufferOffset += e.BytesTransferred;
            }
            userToken.ReceiveAsync(e);
          }
          else
          {
            byte[] numArray = this.bufferOffset > 0 ? new byte[this.bufferOffset] : throw new XMobileException(2014, "No data is received, but connection is closed.");
            Buffer.BlockCopy((Array) this.buffer, 0, (Array) numArray, 0, this.bufferOffset);
            this.bufferOffset = 0;
            if (numArray != null && numArray.Length > 0)
            {
              numArray = LRCCrypto.DecryptLrcMessage(numArray);
              if (numArray == null)
                throw new XMobileException(3004, "Failed to decrypt Lrc response message\n");
            }
            this.CompleteCurrentOperation(e, ex1, numArray);
          }
        }
        else
          this.CompleteCurrentOperation(e, (Exception) new SocketException((int) e.SocketError), (byte[]) null);
      }
      catch (Exception ex2)
      {
        this.CompleteCurrentOperation(e, ex2, (byte[]) null);
      }
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception will be sent to caller")]
    private void ProcessSend(SocketAsyncEventArgs e)
    {
      try
      {
        if (e.SocketError == SocketError.Success)
        {
          if ((e.UserToken as Socket).ReceiveAsync(e))
            return;
          this.ProcessReceive(e);
        }
        else
          this.CompleteCurrentOperation(e, (Exception) new SocketException((int) e.SocketError), (byte[]) null);
      }
      catch (Exception ex)
      {
        this.CompleteCurrentOperation(e, ex, (byte[]) null);
      }
    }

    private void CompleteCurrentOperation(
      SocketAsyncEventArgs e,
      Exception ex,
      byte[] responseBuffer)
    {
      if (!this.isOperationPending)
        return;
      this.isOperationPending = false;
      ++this.requestId;
      e?.Dispose();
      if (this.clientSocket != null)
      {
        try
        {
          this.clientSocket.Close();
        }
        catch (ObjectDisposedException ex1)
        {
        }
        finally
        {
          this.clientSocket = (Socket) null;
        }
      }
      XmediaMessageReceivedEventArgs e1 = new XmediaMessageReceivedEventArgs((ReceivedXmediaMessage) new ReceivedLocalSubnetXmediaMessage(responseBuffer));
      this.LastError = ex;
      EventHandler<XmediaMessageReceivedEventArgs> messageReceived = this.MessageReceived;
      if (messageReceived == null)
        return;
      messageReceived((object) this, e1);
    }

    private void Listen()
    {
      if (this.stopNotificationReceiver)
        return;
      this.multicastState = LRCLocalTransport.MulticastState.Listening;
      this.currentNotificationAsyncResult = this.udpClient.BeginReceiveFromGroup(this.notificationBuffer, 0, this.notificationBuffer.Length, new AsyncCallback(this.OnNotificationReceived), (object) null);
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "No app crashing")]
    private void OnNotificationReceived(IAsyncResult result)
    {
      bool flag = false;
      try
      {
        if (this.udpClient == null)
        {
          flag = true;
        }
        else
        {
          IPEndPoint source;
          int fromGroup = this.udpClient.EndReceiveFromGroup(result, out source);
          if (fromGroup <= 0 || string.Compare(source.Address.ToString(), this.hostIPAddress, StringComparison.OrdinalIgnoreCase) != 0)
            return;
          EventHandler<XmediaMessageReceivedEventArgs> messageReceived = this.MessageReceived;
          if (messageReceived == null)
            return;
          byte[] numArray = new byte[fromGroup];
          Buffer.BlockCopy((Array) this.notificationBuffer, 0, (Array) numArray, 0, fromGroup);
          if (numArray != null && numArray.Length > 0)
          {
            numArray = LRCCrypto.DecryptLrcMessage(numArray);
            if (numArray == null)
              throw new XMobileException(3004, "Failed to decrypt notification message\n");
          }
          ReceivedLocalSubnetXmediaMessage message = new ReceivedLocalSubnetXmediaMessage(numArray);
          messageReceived((object) this, new XmediaMessageReceivedEventArgs((ReceivedXmediaMessage) message));
        }
      }
      catch (SocketException ex)
      {
        flag = true;
      }
      catch (ObjectDisposedException ex)
      {
        flag = true;
      }
      catch (Exception ex)
      {
      }
      finally
      {
        if (flag)
          this.StartMulticastListener();
        else
          this.Listen();
      }
    }

    private void StartSocketTimer()
    {
      this.StopSocketTimer();
      this.socketTimer = new Timer(new TimerCallback(this.OnSocketTimeOut), (object) this.requestId, 5000, -1);
    }

    private void StopSocketTimer()
    {
      if (this.socketTimer == null)
        return;
      this.socketTimer.Dispose();
      this.socketTimer = (Timer) null;
    }

    private void OnSocketTimeOut(object state)
    {
      lock (this)
      {
        if (!this.isOperationPending || (long) this.requestId != (long) (ulong) state)
          return;
        this.CompleteCurrentOperation((SocketAsyncEventArgs) null, (Exception) new XMobileException((Exception) null, 2020, "Time out"), (byte[]) null);
      }
    }

    private enum MulticastState
    {
      Uninitialized,
      Joining,
      Listening,
      Closing,
    }
  }
}
