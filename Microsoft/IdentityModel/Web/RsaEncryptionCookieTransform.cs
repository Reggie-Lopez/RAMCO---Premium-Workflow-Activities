// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.RsaEncryptionCookieTransform
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public class RsaEncryptionCookieTransform : CookieTransform
  {
    private RSA _encryptionKey;
    private List<RSA> _decryptionKeys = new List<RSA>();
    private string _hashName = "SHA256";

    public RsaEncryptionCookieTransform()
    {
    }

    public RsaEncryptionCookieTransform(RSA key)
    {
      this._encryptionKey = key != null ? key : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (key));
      this._decryptionKeys.Add(this._encryptionKey);
    }

    public RsaEncryptionCookieTransform(X509Certificate2 certificate)
    {
      this._encryptionKey = certificate != null ? X509Util.EnsureAndGetPrivateRSAKey(certificate) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (certificate));
      this._decryptionKeys.Add(this._encryptionKey);
    }

    public virtual RSA EncryptionKey
    {
      get => this._encryptionKey;
      set
      {
        this._encryptionKey = value;
        this._decryptionKeys = new List<RSA>((IEnumerable<RSA>) new RSA[1]
        {
          this._encryptionKey
        });
      }
    }

    protected virtual ReadOnlyCollection<RSA> DecryptionKeys => this._decryptionKeys.AsReadOnly();

    public string HashName
    {
      get => this._hashName;
      set
      {
        using (HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(value))
        {
          if (hashAlgorithm == null)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID6034", (object) value));
          this._hashName = value;
        }
      }
    }

    public override byte[] Decode(byte[] encoded)
    {
      if (encoded == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (encoded));
      if (encoded.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (encoded), Microsoft.IdentityModel.SR.GetString("ID6045"));
      ReadOnlyCollection<RSA> decryptionKeys = this.DecryptionKeys;
      if (decryptionKeys.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID6039"));
      RSA rsa1 = (RSA) null;
      byte[] rgb;
      byte[] inputBuffer;
      using (HashAlgorithm hashAlgorithm = HashAlgorithm.Create(this._hashName))
      {
        int count1 = hashAlgorithm.HashSize / 8;
        byte[] b;
        using (BinaryReader binaryReader = new BinaryReader((Stream) new MemoryStream(encoded)))
        {
          b = binaryReader.ReadBytes(count1);
          int count2 = binaryReader.ReadInt32();
          rgb = binaryReader.ReadBytes(count2);
          int count3 = binaryReader.ReadInt32();
          inputBuffer = binaryReader.ReadBytes(count3);
        }
        foreach (RSA rsa2 in decryptionKeys)
        {
          if (CryptoUtil.AreEqual(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(rsa2.ToXmlString(false))), b))
          {
            rsa1 = rsa2;
            break;
          }
        }
      }
      if (rsa1 == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID6040"));
      byte[] numArray = rsa1 is RSACryptoServiceProvider cryptoServiceProvider ? cryptoServiceProvider.Decrypt(rgb, true) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID6041"));
      SymmetricAlgorithm symmetricAlgorithm = CryptoUtil.Algorithms.NewDefaultEncryption();
      byte[] rgbKey = new byte[symmetricAlgorithm.KeySize / 8];
      if (numArray.Length - rgbKey.Length < 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID6047"));
      byte[] rgbIV = new byte[numArray.Length - rgbKey.Length];
      Array.Copy((Array) numArray, (Array) rgbKey, rgbKey.Length);
      Array.Copy((Array) numArray, rgbKey.Length, (Array) rgbIV, 0, rgbIV.Length);
      return symmetricAlgorithm.CreateDecryptor(rgbKey, rgbIV).TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
    }

    public override byte[] Encode(byte[] value)
    {
      if (value == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
      if (value.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID6044"));
      RSA encryptionKey = this.EncryptionKey;
      if (encryptionKey == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID6043"));
      byte[] hash;
      using (HashAlgorithm hashAlgorithm = HashAlgorithm.Create(this._hashName))
        hash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey.ToXmlString(false)));
      byte[] buffer1;
      byte[] buffer2;
      using (SymmetricAlgorithm symmetricAlgorithm = CryptoUtil.Algorithms.NewDefaultEncryption())
      {
        symmetricAlgorithm.GenerateIV();
        symmetricAlgorithm.GenerateKey();
        buffer1 = symmetricAlgorithm.CreateEncryptor().TransformFinalBlock(value, 0, value.Length);
        if (!(encryptionKey is RSACryptoServiceProvider cryptoServiceProvider))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID6041"));
        byte[] rgb = new byte[symmetricAlgorithm.Key.Length + symmetricAlgorithm.IV.Length];
        Array.Copy((Array) symmetricAlgorithm.Key, (Array) rgb, symmetricAlgorithm.Key.Length);
        Array.Copy((Array) symmetricAlgorithm.IV, 0, (Array) rgb, symmetricAlgorithm.Key.Length, symmetricAlgorithm.IV.Length);
        buffer2 = cryptoServiceProvider.Encrypt(rgb, true);
      }
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write(hash);
          binaryWriter.Write(buffer2.Length);
          binaryWriter.Write(buffer2);
          binaryWriter.Write(buffer1.Length);
          binaryWriter.Write(buffer1);
          binaryWriter.Flush();
        }
        return memoryStream.ToArray();
      }
    }
  }
}
