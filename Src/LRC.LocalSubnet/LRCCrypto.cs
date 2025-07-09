// *********************************************************
// Type: LRC.LocalSubnet.LRCCrypto
// Assembly: LRC.LocalSubnet, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67B18A68-32AE-4F0B-8110-A02EDA1EEA1C
// *********************************************************LRC.LocalSubnet.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Cryptography;


namespace LRC.LocalSubnet
{
  public static class LRCCrypto
  {
    private const string ComponentName = "LRCCrypto";
    private static int lrcEncryptionBlockSize = 16;
    private static uint lrcMessageHashSize = 20;
    private static int lrcKeySize = 16;
    private static byte[] sessionEncryptionkey;
    private static byte[] sessionHashKey;

    public static void SetEncryptionKey(byte[] encryptionkey)
    {
      LRCCrypto.sessionEncryptionkey = new byte[LRCCrypto.lrcKeySize];
      LRCCrypto.sessionHashKey = new byte[LRCCrypto.lrcKeySize];
      if (encryptionkey == null || encryptionkey.Length < LRCCrypto.lrcKeySize)
        throw new ArgumentException("Invalid encryption key buffer");
      if (encryptionkey.Length <= LRCCrypto.lrcKeySize)
        return;
      for (int index = 0; index < LRCCrypto.lrcKeySize; ++index)
        LRCCrypto.sessionEncryptionkey[index] = encryptionkey[index];
      if (encryptionkey.Length - LRCCrypto.lrcKeySize < LRCCrypto.lrcKeySize)
        return;
      int lrcKeySize = LRCCrypto.lrcKeySize;
      int index1 = 0;
      for (; lrcKeySize < encryptionkey.Length; ++lrcKeySize)
      {
        LRCCrypto.sessionHashKey[index1] = encryptionkey[lrcKeySize];
        ++index1;
      }
    }

    public static uint CalculateBufferSize(uint bufferSize)
    {
      return bufferSize >= 12U ? LRCCrypto.GetEncryptedDataSize(bufferSize - 4U) : throw new ArgumentException("buffer too small to include message signature and message length");
    }

    public static uint GetMessageSignature(byte[] message)
    {
      byte[] destinationArray = new byte[4];
      Array.Copy((Array) message, 0, (Array) destinationArray, 0, destinationArray.Length);
      Array.Reverse((Array) destinationArray);
      return BitConverter.ToUInt32(destinationArray, 0);
    }

    public static uint GetMessageLength(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentException("buffer is null");
      if (buffer.Length < 8)
        throw new ArgumentException("not enough bytes to extract message length");
      byte[] destinationArray = new byte[4];
      Array.Copy((Array) buffer, 4, (Array) destinationArray, 0, destinationArray.Length);
      Array.Reverse((Array) destinationArray);
      return BitConverter.ToUInt32(destinationArray, 0);
    }

    public static uint GetMessageSequenceNumber(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentException("buffer is null");
      if (buffer.Length < 12)
        throw new ArgumentException("not enough bytes to extract message length");
      byte[] destinationArray = new byte[4];
      Array.Copy((Array) buffer, 8, (Array) destinationArray, 0, destinationArray.Length);
      Array.Reverse((Array) destinationArray);
      return BitConverter.ToUInt32(destinationArray, 0);
    }

    public static uint GetEncryptedDataSize(uint msgLen)
    {
      return (uint) ((int) LRCCrypto.GetEncryptedMessageLength(msgLen) + 4 + 4 + 4);
    }

    public static uint GetEncryptedMessageLength(uint msgLen)
    {
      return (uint) (((ulong) ((long) msgLen - 4L - 4L) + (ulong) LRCCrypto.lrcEncryptionBlockSize) / (ulong) LRCCrypto.lrcEncryptionBlockSize * (ulong) LRCCrypto.lrcEncryptionBlockSize);
    }

    public static uint GetDecryptedDataSize(uint msgLen)
    {
      return (uint) ((int) LRCCrypto.GetDecryptedMessageLength(msgLen) + 4 + 4 + 4);
    }

    public static uint GetDecryptedMessageLength(uint msgLen)
    {
      int num = (int) msgLen - 4 - 4;
      if (msgLen < 8U)
        return 0;
      return num % LRCCrypto.lrcEncryptionBlockSize == 0 ? (uint) (num + LRCCrypto.lrcEncryptionBlockSize) : (uint) ((num + LRCCrypto.lrcEncryptionBlockSize) / LRCCrypto.lrcEncryptionBlockSize * LRCCrypto.lrcEncryptionBlockSize + LRCCrypto.lrcEncryptionBlockSize);
    }

    [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Dispose not available, use Clear instead")]
    public static byte[] EncryptLrcMessage(byte[] message)
    {
      bool flag1 = false;
      if (message == null || message.Length == 0)
        return (byte[]) null;
      LRCCrypto.ComputeMessageHash(message);
      if (LRCCrypto.GetMessageSignature(message) != 3202006746U)
        return (byte[]) null;
      uint messageLength = LRCCrypto.GetMessageLength(message);
      uint encryptedDataSize = LRCCrypto.GetEncryptedDataSize(messageLength);
      uint num1 = (uint) ((int) messageLength - 4 - 4);
      int num2 = 12;
      uint num3 = 0;
      if ((long) encryptedDataSize > (long) message.Length)
        return (byte[]) null;
      uint messageSequenceNumber = LRCCrypto.GetMessageSequenceNumber(message);
      AesManaged aesManaged = new AesManaged();
      aesManaged.IV = LRCCrypto.CreateIvFromSequenceNumber(messageSequenceNumber);
      aesManaged.Key = LRCCrypto.sessionEncryptionkey;
      aesManaged.BlockSize = 128;
      ICryptoTransform encryptor = aesManaged.CreateEncryptor();
      while (num3 < num1)
      {
        bool flag2 = (long) (num1 - num3) <= (long) LRCCrypto.lrcEncryptionBlockSize;
        int inputCount = !flag2 ? LRCCrypto.lrcEncryptionBlockSize : (int) num1 - (int) num3;
        if (flag2)
        {
          Buffer.BlockCopy((Array) encryptor.TransformFinalBlock(message, num2, inputCount), 0, (Array) message, num2, LRCCrypto.lrcEncryptionBlockSize);
          flag1 = true;
          break;
        }
        num3 += (uint) encryptor.TransformBlock(message, num2, inputCount, message, num2);
        num2 += LRCCrypto.lrcEncryptionBlockSize;
      }
      encryptor.Dispose();
      aesManaged.Clear();
      return !flag1 ? (byte[]) null : message;
    }

    [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Dispose not available, use Clear instead")]
    public static byte[] DecryptLrcMessage(byte[] message)
    {
      bool flag = false;
      if (message == null || message.Length < 8)
        return (byte[]) null;
      if (LRCCrypto.GetMessageSignature(message) != 3202006746U)
        return (byte[]) null;
      uint messageSequenceNumber = LRCCrypto.GetMessageSequenceNumber(message);
      AesManaged aesManaged = new AesManaged();
      aesManaged.IV = LRCCrypto.CreateIvFromSequenceNumber(messageSequenceNumber);
      aesManaged.Key = LRCCrypto.sessionEncryptionkey;
      aesManaged.BlockSize = 128;
      uint num1 = (uint) ((int) LRCCrypto.GetMessageLength(message) - 4 - 4);
      int decryptedDataSize = (int) LRCCrypto.GetDecryptedDataSize(LRCCrypto.GetMessageLength(message));
      int num2 = 12;
      ICryptoTransform decryptor = aesManaged.CreateDecryptor();
      if ((long) decryptor.TransformBlock(message, num2, decryptedDataSize - num2, message, num2) >= (long) num1)
        flag = true;
      decryptor.Dispose();
      aesManaged.Clear();
      byte[] hash = new byte[(IntPtr) LRCCrypto.lrcMessageHashSize];
      LRCCrypto.ComputeMessageHash(message, hash);
      int index = 0;
      while ((long) index < (long) LRCCrypto.lrcMessageHashSize && (int) message[(long) (LRCCrypto.GetMessageLength(message) + 4U - LRCCrypto.lrcMessageHashSize) + (long) index] == (int) hash[index])
        ++index;
      if (!flag)
        return (byte[]) null;
      Buffer.BlockCopy((Array) BitConverter.GetBytes(LRCMessageParser.Swapuint(3202006746U)), 0, (Array) message, 0, 4);
      Array.Resize<byte>(ref message, (int) LRCCrypto.GetMessageLength(message) + 4);
      return message;
    }

    public static byte[] HexStringToByteArray(string hexString)
    {
      if (string.IsNullOrEmpty(hexString))
        return (byte[]) null;
      byte[] byteArray = new byte[hexString.Length / 2];
      for (int index = 0; index < byteArray.Length; ++index)
        byteArray[index] = byte.Parse(hexString.Substring(index * 2, 2), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
      return byteArray;
    }

    public static string ByteToHexString(byte[] bytes)
    {
      string empty = string.Empty;
      if (bytes == null)
        return empty;
      for (int index = 0; index < bytes.Length; ++index)
        empty += bytes[index].ToString("X2", (IFormatProvider) CultureInfo.InvariantCulture);
      return empty;
    }

    private static byte[] CreateIvFromSequenceNumber(uint sequenceNumber)
    {
      using (HMACSHA1 hmacshA1 = new HMACSHA1(LRCCrypto.sessionHashKey))
      {
        byte[] bytes = BitConverter.GetBytes(sequenceNumber);
        Array.Reverse((Array) bytes);
        byte[] hash = hmacshA1.ComputeHash(bytes);
        Array.Resize<byte>(ref hash, LRCCrypto.lrcEncryptionBlockSize);
        return hash;
      }
    }

    private static void ComputeMessageHash(byte[] message)
    {
      using (HMACSHA1 hmacshA1 = new HMACSHA1(LRCCrypto.sessionHashKey))
      {
        uint num = LRCCrypto.GetMessageLength(message) + 4U;
        Array.Copy((Array) hmacshA1.ComputeHash(message, 0, (int) num - (int) LRCCrypto.lrcMessageHashSize), 0, (Array) message, (int) num - (int) LRCCrypto.lrcMessageHashSize, (int) LRCCrypto.lrcMessageHashSize);
      }
    }

    private static void ComputeMessageHash(byte[] message, byte[] hash)
    {
      uint count = LRCCrypto.GetMessageLength(message) + 4U - LRCCrypto.lrcMessageHashSize;
      using (HMACSHA1 hmacshA1 = new HMACSHA1(LRCCrypto.sessionHashKey))
        Array.Copy((Array) hmacshA1.ComputeHash(message, 0, (int) count), (Array) hash, (int) LRCCrypto.lrcMessageHashSize);
    }
  }
}
