// *********************************************************
// Type: Microsoft.Phone.Marketplace.Util.DeflateStream
// Assembly: Microsoft.Phone.Avatar.Marketplace.Silverlight, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3DD9BB19-1343-4B20-A060-EAD685E48D94
// *********************************************************Microsoft.Phone.Avatar.Marketplace.Silverlight.dll

using System;
using System.IO;
using System.Threading;


namespace Microsoft.Phone.Marketplace.Util
{
  public class DeflateStream : Stream
  {
    internal const string ManagedDeflaterCompatSwitchName = "NetFx45_LegacyManagedDeflateStream";
    internal const int DefaultBufferSize = 8192;
    private Stream _stream;
    private CompressionMode _mode;
    private bool _leaveOpen;
    private Inflater inflater;
    private IDeflater deflater;
    private byte[] buffer;
    private int asyncOperations;
    private readonly AsyncCallback m_CallBack;
    private readonly DeflateStream.AsyncWriteDelegate m_AsyncWriterDelegate;
    private IFileFormatWriter formatWriter;
    private bool wroteHeader;
    private bool wroteBytes;
    private static volatile DeflateStream.WorkerType deflaterType = DeflateStream.WorkerType.Unknown;

    public DeflateStream(Stream stream, CompressionMode mode)
      : this(stream, mode, false)
    {
    }

    public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      if (CompressionMode.Compress != mode && mode != CompressionMode.Decompress)
        throw new ArgumentException("Enum argument out of range", nameof (mode));
      this._stream = stream;
      this._mode = mode;
      this._leaveOpen = leaveOpen;
      switch (this._mode)
      {
        case CompressionMode.Decompress:
          if (!this._stream.CanRead)
            throw new ArgumentException("Not a readable stream", nameof (stream));
          this.inflater = new Inflater();
          this.m_CallBack = new AsyncCallback(this.ReadCallback);
          break;
        case CompressionMode.Compress:
          if (!this._stream.CanWrite)
            throw new ArgumentException("Not a writeable stream", nameof (stream));
          this.deflater = DeflateStream.CreateDeflater(new CompressionLevel?());
          this.m_AsyncWriterDelegate = new DeflateStream.AsyncWriteDelegate(this.InternalWrite);
          this.m_CallBack = new AsyncCallback(this.WriteCallback);
          break;
      }
      this.buffer = new byte[8192];
    }

    public DeflateStream(Stream stream, CompressionLevel compressionLevel)
      : this(stream, compressionLevel, false)
    {
    }

    public DeflateStream(Stream stream, CompressionLevel compressionLevel, bool leaveOpen)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      this._stream = stream.CanWrite ? stream : throw new ArgumentException("Not a writeable stream", nameof (stream));
      this._mode = CompressionMode.Compress;
      this._leaveOpen = leaveOpen;
      this.deflater = DeflateStream.CreateDeflater(new CompressionLevel?(compressionLevel));
      this.m_AsyncWriterDelegate = new DeflateStream.AsyncWriteDelegate(this.InternalWrite);
      this.m_CallBack = new AsyncCallback(this.WriteCallback);
      this.buffer = new byte[8192];
    }

    private static IDeflater CreateDeflater(CompressionLevel? compressionLevel)
    {
      if (DeflateStream.GetDeflaterType() == DeflateStream.WorkerType.Managed)
        return (IDeflater) new DeflaterManaged();
      throw new SystemException("Program entered an unexpected state.");
    }

    private static DeflateStream.WorkerType GetDeflaterType()
    {
      return DeflateStream.WorkerType.Unknown != DeflateStream.deflaterType ? DeflateStream.deflaterType : (DeflateStream.deflaterType = DeflateStream.WorkerType.Managed);
    }

    internal void SetFileFormatReader(IFileFormatReader reader)
    {
      if (reader == null)
        return;
      this.inflater.SetFileFormatReader(reader);
    }

    internal void SetFileFormatWriter(IFileFormatWriter writer)
    {
      if (writer == null)
        return;
      this.formatWriter = writer;
    }

    public Stream BaseStream => this._stream;

    public override bool CanRead
    {
      get
      {
        return this._stream != null && this._mode == CompressionMode.Decompress && this._stream.CanRead;
      }
    }

    public override bool CanWrite
    {
      get
      {
        return this._stream != null && this._mode == CompressionMode.Compress && this._stream.CanWrite;
      }
    }

    public override bool CanSeek => false;

    public override long Length => throw new NotSupportedException("Not supported");

    public override long Position
    {
      get => throw new NotSupportedException("Not supported");
      set => throw new NotSupportedException("Not supported");
    }

    public override void Flush() => this.EnsureNotDisposed();

    public override long Seek(long offset, SeekOrigin origin)
    {
      throw new NotSupportedException("Not supported");
    }

    public override void SetLength(long value) => throw new NotSupportedException("Not supported");

    public override int Read(byte[] array, int offset, int count)
    {
      this.EnsureDecompressionMode();
      this.ValidateParameters(array, offset, count);
      this.EnsureNotDisposed();
      int offset1 = offset;
      int length1 = count;
      while (true)
      {
        int num = this.inflater.Inflate(array, offset1, length1);
        offset1 += num;
        length1 -= num;
        if (length1 != 0 && !this.inflater.Finished())
        {
          int length2 = this._stream.Read(this.buffer, 0, this.buffer.Length);
          if (length2 != 0)
            this.inflater.SetInput(this.buffer, 0, length2);
          else
            break;
        }
        else
          break;
      }
      return count - length1;
    }

    private void ValidateParameters(byte[] array, int offset, int count)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (array.Length - offset < count)
        throw new ArgumentException("Invalid argument offset count");
    }

    private void EnsureNotDisposed()
    {
      if (this._stream == null)
        throw new ObjectDisposedException((string) null, "Oject diesposed: stream closed");
    }

    private void EnsureDecompressionMode()
    {
      if (this._mode != CompressionMode.Decompress)
        throw new InvalidOperationException("Cannot read from deflate stream");
    }

    private void EnsureCompressionMode()
    {
      if (this._mode != CompressionMode.Compress)
        throw new InvalidOperationException("Cannot write to deflate stream");
    }

    public override IAsyncResult BeginRead(
      byte[] array,
      int offset,
      int count,
      AsyncCallback asyncCallback,
      object asyncState)
    {
      this.EnsureDecompressionMode();
      if (this.asyncOperations != 0)
        throw new InvalidOperationException("Invalid begin call");
      this.ValidateParameters(array, offset, count);
      this.EnsureNotDisposed();
      Interlocked.Increment(ref this.asyncOperations);
      try
      {
        DeflateStreamAsyncResult state = new DeflateStreamAsyncResult((object) this, asyncState, asyncCallback, array, offset, count);
        state.isWrite = false;
        int result = this.inflater.Inflate(array, offset, count);
        if (result != 0)
        {
          state.InvokeCallback(true, (object) result);
          return (IAsyncResult) state;
        }
        if (this.inflater.Finished())
        {
          state.InvokeCallback(true, (object) 0);
          return (IAsyncResult) state;
        }
        this._stream.BeginRead(this.buffer, 0, this.buffer.Length, this.m_CallBack, (object) state);
        state.m_CompletedSynchronously &= state.IsCompleted;
        return (IAsyncResult) state;
      }
      catch
      {
        Interlocked.Decrement(ref this.asyncOperations);
        throw;
      }
    }

    private void ReadCallback(IAsyncResult baseStreamResult)
    {
      DeflateStreamAsyncResult asyncState = (DeflateStreamAsyncResult) baseStreamResult.AsyncState;
      asyncState.m_CompletedSynchronously &= baseStreamResult.CompletedSynchronously;
      try
      {
        this.EnsureNotDisposed();
        int length = this._stream.EndRead(baseStreamResult);
        if (length <= 0)
        {
          asyncState.InvokeCallback((object) 0);
        }
        else
        {
          this.inflater.SetInput(this.buffer, 0, length);
          int result = this.inflater.Inflate(asyncState.buffer, asyncState.offset, asyncState.count);
          if (result == 0 && !this.inflater.Finished())
            this._stream.BeginRead(this.buffer, 0, this.buffer.Length, this.m_CallBack, (object) asyncState);
          else
            asyncState.InvokeCallback((object) result);
        }
      }
      catch (Exception ex)
      {
        asyncState.InvokeCallback((object) ex);
      }
    }

    public override int EndRead(IAsyncResult asyncResult)
    {
      this.EnsureDecompressionMode();
      this.CheckEndXxxxLegalStateAndParams(asyncResult);
      DeflateStreamAsyncResult asyncResult1 = (DeflateStreamAsyncResult) asyncResult;
      this.AwaitAsyncResultCompletion(asyncResult1);
      if (asyncResult1.Result is Exception result)
        throw result;
      return (int) asyncResult1.Result;
    }

    public override void Write(byte[] array, int offset, int count)
    {
      this.EnsureCompressionMode();
      this.ValidateParameters(array, offset, count);
      this.EnsureNotDisposed();
      this.InternalWrite(array, offset, count, false);
    }

    internal void InternalWrite(byte[] array, int offset, int count, bool isAsync)
    {
      this.DoMaintenance(array, offset, count);
      this.WriteDeflaterOutput(isAsync);
      this.deflater.SetInput(array, offset, count);
      this.WriteDeflaterOutput(isAsync);
    }

    private void WriteDeflaterOutput(bool isAsync)
    {
      while (!this.deflater.NeedsInput())
      {
        int deflateOutput = this.deflater.GetDeflateOutput(this.buffer);
        if (deflateOutput > 0)
          this.DoWrite(this.buffer, 0, deflateOutput, isAsync);
      }
    }

    private void DoWrite(byte[] array, int offset, int count, bool isAsync)
    {
      if (isAsync)
        this._stream.EndWrite(this._stream.BeginWrite(array, offset, count, (AsyncCallback) null, (object) null));
      else
        this._stream.Write(array, offset, count);
    }

    private void DoMaintenance(byte[] array, int offset, int count)
    {
      if (count <= 0)
        return;
      this.wroteBytes = true;
      if (this.formatWriter == null)
        return;
      if (!this.wroteHeader)
      {
        byte[] header = this.formatWriter.GetHeader();
        this._stream.Write(header, 0, header.Length);
        this.wroteHeader = true;
      }
      this.formatWriter.UpdateWithBytesRead(array, offset, count);
    }

    private void PurgeBuffers(bool disposing)
    {
      if (!disposing || this._stream == null)
        return;
      this.Flush();
      if (this._mode != CompressionMode.Compress)
        return;
      if (this.wroteBytes)
      {
        this.WriteDeflaterOutput(false);
        bool flag;
        do
        {
          int bytesRead;
          flag = this.deflater.Finish(this.buffer, out bytesRead);
          if (bytesRead > 0)
            this.DoWrite(this.buffer, 0, bytesRead, false);
        }
        while (!flag);
      }
      if (this.formatWriter == null || !this.wroteHeader)
        return;
      byte[] footer = this.formatWriter.GetFooter();
      this._stream.Write(footer, 0, footer.Length);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        this.PurgeBuffers(disposing);
      }
      finally
      {
        try
        {
          if (disposing)
          {
            if (!this._leaveOpen)
            {
              if (this._stream != null)
                this._stream.Close();
            }
          }
        }
        finally
        {
          this._stream = (Stream) null;
          try
          {
            if (this.deflater != null)
              this.deflater.Dispose();
          }
          finally
          {
            this.deflater = (IDeflater) null;
            base.Dispose(disposing);
          }
        }
      }
    }

    public override IAsyncResult BeginWrite(
      byte[] array,
      int offset,
      int count,
      AsyncCallback asyncCallback,
      object asyncState)
    {
      this.EnsureCompressionMode();
      if (this.asyncOperations != 0)
        throw new InvalidOperationException("Invalid begin call");
      this.ValidateParameters(array, offset, count);
      this.EnsureNotDisposed();
      Interlocked.Increment(ref this.asyncOperations);
      try
      {
        DeflateStreamAsyncResult streamAsyncResult = new DeflateStreamAsyncResult((object) this, asyncState, asyncCallback, array, offset, count);
        streamAsyncResult.isWrite = true;
        this.m_AsyncWriterDelegate.BeginInvoke(array, offset, count, true, this.m_CallBack, (object) streamAsyncResult);
        streamAsyncResult.m_CompletedSynchronously &= streamAsyncResult.IsCompleted;
        return (IAsyncResult) streamAsyncResult;
      }
      catch
      {
        Interlocked.Decrement(ref this.asyncOperations);
        throw;
      }
    }

    private void WriteCallback(IAsyncResult asyncResult)
    {
      DeflateStreamAsyncResult asyncState = (DeflateStreamAsyncResult) asyncResult.AsyncState;
      asyncState.m_CompletedSynchronously &= asyncResult.CompletedSynchronously;
      try
      {
        this.m_AsyncWriterDelegate.EndInvoke(asyncResult);
      }
      catch (Exception ex)
      {
        asyncState.InvokeCallback((object) ex);
        return;
      }
      asyncState.InvokeCallback((object) null);
    }

    public override void EndWrite(IAsyncResult asyncResult)
    {
      this.EnsureCompressionMode();
      this.CheckEndXxxxLegalStateAndParams(asyncResult);
      DeflateStreamAsyncResult asyncResult1 = (DeflateStreamAsyncResult) asyncResult;
      this.AwaitAsyncResultCompletion(asyncResult1);
      if (asyncResult1.Result is Exception result)
        throw result;
    }

    private void CheckEndXxxxLegalStateAndParams(IAsyncResult asyncResult)
    {
      if (this.asyncOperations != 1)
        throw new InvalidOperationException("Invalid end call");
      if (asyncResult == null)
        throw new ArgumentNullException(nameof (asyncResult));
      this.EnsureNotDisposed();
      if (!(asyncResult is DeflateStreamAsyncResult))
        throw new ArgumentNullException(nameof (asyncResult));
    }

    private void AwaitAsyncResultCompletion(DeflateStreamAsyncResult asyncResult)
    {
      try
      {
        if (asyncResult.IsCompleted)
          return;
        asyncResult.AsyncWaitHandle.WaitOne();
      }
      finally
      {
        Interlocked.Decrement(ref this.asyncOperations);
        asyncResult.Close();
      }
    }

    internal delegate void AsyncWriteDelegate(byte[] array, int offset, int count, bool isAsync);

    private enum WorkerType : byte
    {
      Managed,
      ZLib,
      Unknown,
    }
  }
}
