// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.RsaSignatureCookieTransform
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public class RsaSignatureCookieTransform : CookieTransform
  {
    private RSA _signingKey;
    private List<RSA> _verificationKeys = new List<RSA>();
    private string _hashName = "SHA256";

    public RsaSignatureCookieTransform()
    {
    }

    public RsaSignatureCookieTransform(RSA key)
    {
      this._signingKey = key != null ? key : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (key));
      this._verificationKeys.Add(this._signingKey);
    }

    public RsaSignatureCookieTransform(X509Certificate2 certificate)
    {
      this._signingKey = certificate != null ? X509Util.EnsureAndGetPrivateRSAKey(certificate) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (certificate));
      this._verificationKeys.Add(this._signingKey);
    }

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

    public virtual RSA SigningKey
    {
      get => this._signingKey;
      set
      {
        this._signingKey = value;
        this._verificationKeys = new List<RSA>((IEnumerable<RSA>) new RSA[1]
        {
          this._signingKey
        });
      }
    }

    protected virtual ReadOnlyCollection<RSA> VerificationKeys => this._verificationKeys.AsReadOnly();

    public override byte[] Decode(byte[] encoded)
    {
      if (encoded == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (encoded));
      if (encoded.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (encoded), Microsoft.IdentityModel.SR.GetString("ID6045"));
      ReadOnlyCollection<RSA> verificationKeys = this.VerificationKeys;
      if (verificationKeys.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID6036"));
      int startIndex = 0;
      int length = encoded.Length >= 4 ? BitConverter.ToInt32(encoded, startIndex) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new FormatException(Microsoft.IdentityModel.SR.GetString("ID1012")));
      if (length < 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new FormatException(Microsoft.IdentityModel.SR.GetString("ID1005", (object) length)));
      if (length >= encoded.Length - 4)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new FormatException(Microsoft.IdentityModel.SR.GetString("ID1013")));
      int sourceIndex1 = startIndex + 4;
      byte[] numArray = new byte[length];
      Array.Copy((Array) encoded, sourceIndex1, (Array) numArray, 0, numArray.Length);
      int sourceIndex2 = sourceIndex1 + numArray.Length;
      byte[] buffer = new byte[encoded.Length - sourceIndex2];
      Array.Copy((Array) encoded, sourceIndex2, (Array) buffer, 0, buffer.Length);
      bool flag = false;
      try
      {
        using (HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(this.HashName))
        {
          hashAlgorithm.ComputeHash(buffer);
          foreach (RSA rsa in verificationKeys)
          {
            AsymmetricSignatureDeformatter signatureDeformatter = this.GetSignatureDeformatter(rsa);
            if (this.isSha256() && CryptoUtil.VerifySignatureForSha256(signatureDeformatter, hashAlgorithm, numArray) || signatureDeformatter.VerifySignature(hashAlgorithm, numArray))
            {
              flag = true;
              break;
            }
          }
        }
      }
      catch (CryptographicException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID6035", (object) this.HashName, (object) verificationKeys[0].GetType().FullName), (Exception) ex));
      }
      if (!flag)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID1014")));
      return buffer;
    }

    public override byte[] Encode(byte[] value)
    {
      if (value == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
      if (value.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID6044"));
      RSA signingKey = this.SigningKey;
      RSACryptoServiceProvider cryptoServiceProvider = signingKey as RSACryptoServiceProvider;
      if (signingKey == null || cryptoServiceProvider == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID6042"));
      if (cryptoServiceProvider.PublicOnly)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID6046"));
      byte[] numArray1;
      using (HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(this.HashName))
      {
        try
        {
          hashAlgorithm.ComputeHash(value);
          AsymmetricSignatureFormatter signatureFormatter = this.GetSignatureFormatter(signingKey);
          numArray1 = !this.isSha256() ? signatureFormatter.CreateSignature(hashAlgorithm) : CryptoUtil.CreateSignatureForSha256(signatureFormatter, hashAlgorithm);
        }
        catch (CryptographicException ex)
        {
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID6035", (object) this.HashName, (object) signingKey.GetType().FullName), (Exception) ex));
        }
      }
      byte[] bytes = BitConverter.GetBytes(numArray1.Length);
      int destinationIndex1 = 0;
      byte[] numArray2 = new byte[bytes.Length + numArray1.Length + value.Length];
      Array.Copy((Array) bytes, 0, (Array) numArray2, destinationIndex1, bytes.Length);
      int destinationIndex2 = destinationIndex1 + bytes.Length;
      Array.Copy((Array) numArray1, 0, (Array) numArray2, destinationIndex2, numArray1.Length);
      int destinationIndex3 = destinationIndex2 + numArray1.Length;
      Array.Copy((Array) value, 0, (Array) numArray2, destinationIndex3, value.Length);
      return numArray2;
    }

    private AsymmetricSignatureFormatter GetSignatureFormatter(RSA rsa)
    {
      RSACryptoServiceProvider rsaProvider = rsa as RSACryptoServiceProvider;
      return this.isSha256() && rsaProvider != null ? CryptoUtil.GetSignatureFormatterForSha256(rsaProvider) : (AsymmetricSignatureFormatter) new RSAPKCS1SignatureFormatter((AsymmetricAlgorithm) rsa);
    }

    private AsymmetricSignatureDeformatter GetSignatureDeformatter(
      RSA rsa)
    {
      RSACryptoServiceProvider rsaProvider = rsa as RSACryptoServiceProvider;
      return this.isSha256() && rsaProvider != null ? CryptoUtil.GetSignatureDeFormatterForSha256(rsaProvider) : (AsymmetricSignatureDeformatter) new RSAPKCS1SignatureDeformatter((AsymmetricAlgorithm) rsa);
    }

    private bool isSha256() => StringComparer.OrdinalIgnoreCase.Equals(this.HashName, "SHA256") || StringComparer.OrdinalIgnoreCase.Equals(this.HashName, "SHA-256") || StringComparer.OrdinalIgnoreCase.Equals(this.HashName, "System.Security.Cryptography.SHA256");
  }
}
