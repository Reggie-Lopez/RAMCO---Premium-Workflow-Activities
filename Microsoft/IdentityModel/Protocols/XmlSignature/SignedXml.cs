// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.SignedXml
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal sealed class SignedXml : IDisposable
  {
    private SecurityTokenSerializer _tokenSerializer;
    private Signature _signature;
    private TransformFactory _transformFactory;
    private bool _disposed;

    public SignedXml(SecurityTokenSerializer securityTokenSerializer)
      : this(new SignedInfo(), securityTokenSerializer)
    {
    }

    internal SignedXml(SignedInfo signedInfo, SecurityTokenSerializer securityTokenSerializer)
    {
      if (signedInfo == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (signedInfo));
      if (securityTokenSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenSerializer));
      this._transformFactory = TransformFactory.Instance;
      this._tokenSerializer = securityTokenSerializer;
      this._signature = new Signature(this, signedInfo);
    }

    ~SignedXml() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (disposing && !this._disposed)
        this._signature.Dispose();
      this._disposed = true;
    }

    public string Id
    {
      get => this._signature.Id;
      set => this._signature.Id = value;
    }

    public SecurityTokenSerializer SecurityTokenSerializer => this._tokenSerializer;

    public Signature Signature => this._signature;

    public TransformFactory TransformFactory
    {
      get => this._transformFactory;
      set => this._transformFactory = value;
    }

    private void ComputeSignature(
      HashAlgorithm hash,
      AsymmetricSignatureFormatter formatter,
      string signatureMethod)
    {
      this.Signature.SignedInfo.ComputeReferenceDigests();
      this.Signature.SignedInfo.ComputeHash(hash);
      this.Signature.SetSignatureValue(!StringComparer.Ordinal.Equals("http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", signatureMethod) ? formatter.CreateSignature(hash) : CryptoUtil.CreateSignatureForSha256(formatter, hash));
    }

    private void ComputeSignature(KeyedHashAlgorithm hash)
    {
      this.Signature.SignedInfo.ComputeReferenceDigests();
      this.Signature.SignedInfo.ComputeHash((HashAlgorithm) hash);
      this.Signature.SetSignatureValue(hash.Hash);
    }

    public void ComputeSignature(SecurityKey signingKey)
    {
      string signatureMethod = this.Signature.SignedInfo.SignatureMethod;
      switch (signingKey)
      {
        case SymmetricSecurityKey symmetricSecurityKey:
          using (KeyedHashAlgorithm keyedHashAlgorithm = symmetricSecurityKey.GetKeyedHashAlgorithm(signatureMethod))
          {
            if (keyedHashAlgorithm == null)
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID6010", (object) symmetricSecurityKey, (object) signatureMethod)));
            this.ComputeSignature(keyedHashAlgorithm);
            break;
          }
        case AsymmetricSecurityKey asymmetricSecurityKey:
          HashAlgorithm hash = (HashAlgorithm) null;
          AsymmetricSignatureFormatter asymmetricSignatureFormatter = (AsymmetricSignatureFormatter) null;
          if (DelegatingXmlDictionaryWriter.GetAsymmetricSignatureOperators != null)
            DelegatingXmlDictionaryWriter.GetAsymmetricSignatureOperators(asymmetricSecurityKey, signatureMethod, out asymmetricSignatureFormatter, out hash);
          if (signatureMethod == "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256")
          {
            if (hash == null)
              hash = CryptoUtil.Algorithms.CreateHashAlgorithm(signatureMethod);
            if (asymmetricSignatureFormatter == null)
              asymmetricSignatureFormatter = CryptoUtil.GetSignatureFormatterForSha256(asymmetricSecurityKey);
          }
          else
          {
            if (hash == null)
              hash = asymmetricSecurityKey.GetHashAlgorithmForSignature(signatureMethod);
            if (hash == null)
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID6011", (object) signatureMethod)));
            if (asymmetricSignatureFormatter == null)
              asymmetricSignatureFormatter = asymmetricSecurityKey.GetSignatureFormatter(signatureMethod);
          }
          if (asymmetricSignatureFormatter == null)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID6012", (object) signatureMethod, (object) asymmetricSecurityKey)));
          try
          {
            this.ComputeSignature(hash, asymmetricSignatureFormatter, signatureMethod);
            break;
          }
          catch (CryptographicException ex)
          {
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID6035", (object) signatureMethod, (object) signingKey.GetType().FullName), (Exception) ex));
          }
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID6015", (object) signingKey)));
      }
    }

    public void CompleteSignatureVerification() => this.Signature.SignedInfo.EnsureAllReferencesVerified();

    public void EnsureDigestValidity(string id, object resolvedXmlSource) => this.Signature.SignedInfo.EnsureDigestValidity(id, resolvedXmlSource);

    public byte[] GetSignatureValue() => this.Signature.GetSignatureBytes();

    public void ReadFrom(XmlDictionaryReader reader) => this._signature.ReadFrom(reader);

    private void VerifySignature(KeyedHashAlgorithm hash)
    {
      this.Signature.SignedInfo.ComputeHash((HashAlgorithm) hash);
      if (!ByteArrayComparer.Instance.Equals(hash.Hash, this.GetSignatureValue()))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6013")));
    }

    private void VerifySignature(
      HashAlgorithm hash,
      AsymmetricSignatureDeformatter deformatter,
      string signatureMethod)
    {
      this.Signature.SignedInfo.ComputeHash(hash);
      if (StringComparer.Ordinal.Equals(signatureMethod, "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"))
      {
        if (!CryptoUtil.VerifySignatureForSha256(deformatter, hash, this.GetSignatureValue()))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6013")));
      }
      else if (!deformatter.VerifySignature(hash, this.GetSignatureValue()))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6013")));
    }

    public void StartSignatureVerification(SecurityKey verificationKey)
    {
      string signatureMethod = this.Signature.SignedInfo.SignatureMethod;
      switch (verificationKey)
      {
        case SymmetricSecurityKey symmetricSecurityKey:
          using (KeyedHashAlgorithm keyedHashAlgorithm = symmetricSecurityKey.GetKeyedHashAlgorithm(signatureMethod))
          {
            if (keyedHashAlgorithm == null)
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6014", (object) signatureMethod, (object) symmetricSecurityKey)));
            this.VerifySignature(keyedHashAlgorithm);
            break;
          }
        case AsymmetricSecurityKey key:
          HashAlgorithm hash;
          AsymmetricSignatureDeformatter deformatter;
          if (signatureMethod == "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256")
          {
            hash = CryptoUtil.Algorithms.CreateHashAlgorithm(signatureMethod);
            deformatter = CryptoUtil.GetSignatureDeFormatterForSha256(key);
          }
          else
          {
            hash = key.GetHashAlgorithmForSignature(signatureMethod);
            if (hash == null)
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6011", (object) signatureMethod)));
            deformatter = key.GetSignatureDeformatter(signatureMethod);
          }
          if (deformatter == null)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6016", (object) signatureMethod, (object) key)));
          this.VerifySignature(hash, deformatter, signatureMethod);
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID6015", (object) verificationKey)));
      }
    }

    public void WriteTo(XmlDictionaryWriter writer) => this._signature.WriteTo(writer);
  }
}
