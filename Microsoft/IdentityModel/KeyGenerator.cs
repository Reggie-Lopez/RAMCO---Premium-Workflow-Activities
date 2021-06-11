// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.KeyGenerator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceModel.Security;

namespace Microsoft.IdentityModel
{
  [ComVisible(true)]
  public static class KeyGenerator
  {
    private const int _maxKeyIterations = 20;
    private static RandomNumberGenerator _random = CryptoUtil.Algorithms.NewRandomNumberGenerator();

    public static byte[] ComputeCombinedKey(
      byte[] requestorEntropy,
      byte[] issuerEntropy,
      int keySizeInBits)
    {
      if (requestorEntropy == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestorEntropy));
      if (issuerEntropy == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (issuerEntropy));
      int length = KeyGenerator.ValidateKeySizeInBytes(keySizeInBits);
      byte[] numArray1 = new byte[length];
      using (KeyedHashAlgorithm keyedHashAlgorithm = CryptoUtil.Algorithms.NewHmacSha1())
      {
        keyedHashAlgorithm.Key = requestorEntropy;
        byte[] buffer1 = issuerEntropy;
        byte[] buffer2 = new byte[keyedHashAlgorithm.HashSize / 8 + buffer1.Length];
        byte[] numArray2 = (byte[]) null;
        try
        {
          int num = 0;
label_10:
          while (num < length)
          {
            keyedHashAlgorithm.Initialize();
            buffer1 = keyedHashAlgorithm.ComputeHash(buffer1);
            buffer1.CopyTo((Array) buffer2, 0);
            issuerEntropy.CopyTo((Array) buffer2, buffer1.Length);
            keyedHashAlgorithm.Initialize();
            numArray2 = keyedHashAlgorithm.ComputeHash(buffer2);
            int index = 0;
            while (true)
            {
              if (index < numArray2.Length && num < length)
              {
                numArray1[num++] = numArray2[index];
                ++index;
              }
              else
                goto label_10;
            }
          }
        }
        catch
        {
          Array.Clear((Array) numArray1, 0, numArray1.Length);
          throw;
        }
        finally
        {
          if (numArray2 != null)
            Array.Clear((Array) numArray2, 0, numArray2.Length);
          Array.Clear((Array) buffer2, 0, buffer2.Length);
          keyedHashAlgorithm.Clear();
        }
      }
      return numArray1;
    }

    public static SecurityKeyIdentifier GetSecurityKeyIdentifier(
      byte[] secret,
      Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials wrappingCredentials)
    {
      if (secret == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (secret));
      if (secret.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (secret), SR.GetString("ID6031"));
      return wrappingCredentials == null || wrappingCredentials.SecurityKey == null ? new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
      {
        (SecurityKeyIdentifierClause) new BinarySecretKeyIdentifierClause(secret)
      }) : new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
      {
        (SecurityKeyIdentifierClause) new EncryptedKeyIdentifierClause(wrappingCredentials.SecurityKey.EncryptKey(wrappingCredentials.Algorithm, secret), wrappingCredentials.Algorithm, wrappingCredentials.SecurityKeyIdentifier)
      });
    }

    public static byte[] GenerateSymmetricKey(int keySizeInBits)
    {
      byte[] data = new byte[KeyGenerator.ValidateKeySizeInBytes(keySizeInBits)];
      CryptoUtil.GenerateRandomBytes(data);
      return data;
    }

    public static byte[] GenerateSymmetricKey(
      int keySizeInBits,
      byte[] senderEntropy,
      out byte[] receiverEntropy)
    {
      if (senderEntropy == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (senderEntropy));
      int length = KeyGenerator.ValidateKeySizeInBytes(keySizeInBits);
      receiverEntropy = new byte[length];
      KeyGenerator._random.GetNonZeroBytes(receiverEntropy);
      return KeyGenerator.ComputeCombinedKey(senderEntropy, receiverEntropy, keySizeInBits);
    }

    public static byte[] GenerateDESKey(int keySizeInBits)
    {
      byte[] numArray = new byte[KeyGenerator.ValidateKeySizeInBytes(keySizeInBits)];
      int num = 0;
      while (num <= 20)
      {
        CryptoUtil.GenerateRandomBytes(numArray);
        ++num;
        if (!TripleDES.IsWeakKey(numArray))
          return numArray;
      }
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(SR.GetString("ID6048", (object) 20)));
    }

    public static byte[] GenerateDESKey(
      int keySizeInBits,
      byte[] senderEntropy,
      out byte[] receiverEntropy)
    {
      int length = KeyGenerator.ValidateKeySizeInBytes(keySizeInBits);
      byte[] numArray = new byte[length];
      int num = 0;
      while (num <= 20)
      {
        receiverEntropy = new byte[length];
        KeyGenerator._random.GetNonZeroBytes(receiverEntropy);
        byte[] combinedKey = KeyGenerator.ComputeCombinedKey(senderEntropy, receiverEntropy, keySizeInBits);
        ++num;
        if (!TripleDES.IsWeakKey(combinedKey))
          return combinedKey;
      }
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(SR.GetString("ID6048", (object) 20)));
    }

    private static int ValidateKeySizeInBytes(int keySizeInBits)
    {
      int num = keySizeInBits / 8;
      if (keySizeInBits <= 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentOutOfRangeException(nameof (keySizeInBits), SR.GetString("ID6033", (object) keySizeInBits)));
      return num * 8 == keySizeInBits ? num : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(SR.GetString("ID6002", (object) keySizeInBits), nameof (keySizeInBits)));
    }
  }
}
