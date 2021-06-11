// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.EnvelopedSignatureReader
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  [ComVisible(true)]
  public sealed class EnvelopedSignatureReader : DelegatingXmlDictionaryReader
  {
    private bool _automaticallyReadSignature;
    private int _elementCount;
    private bool _resolveIntrinsicSigningKeys;
    private bool _requireSignature;
    private SigningCredentials _signingCredentials;
    private SecurityTokenResolver _signingTokenResolver;
    private SignedXml _signedXml;
    private SecurityTokenSerializer _tokenSerializer;
    private WrappedReader _wrappedReader;
    private bool _disposed;

    public EnvelopedSignatureReader(
      XmlReader reader,
      SecurityTokenSerializer securityTokenSerializer)
      : this(reader, securityTokenSerializer, (SecurityTokenResolver) null)
    {
    }

    public EnvelopedSignatureReader(
      XmlReader reader,
      SecurityTokenSerializer securityTokenSerializer,
      SecurityTokenResolver signingTokenResolver)
      : this(reader, securityTokenSerializer, signingTokenResolver, true, true, true)
    {
    }

    public EnvelopedSignatureReader(
      XmlReader reader,
      SecurityTokenSerializer securityTokenSerializer,
      SecurityTokenResolver signingTokenResolver,
      bool requireSignature,
      bool automaticallyReadSignature,
      bool resolveIntrinsicSigningKeys)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (securityTokenSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenSerializer));
      this._automaticallyReadSignature = automaticallyReadSignature;
      this._tokenSerializer = securityTokenSerializer;
      this._requireSignature = requireSignature;
      this._signingTokenResolver = signingTokenResolver ?? Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance;
      this._resolveIntrinsicSigningKeys = resolveIntrinsicSigningKeys;
      this._wrappedReader = new WrappedReader(XmlDictionaryReader.CreateDictionaryReader(reader));
      this.InitializeInnerReader((XmlDictionaryReader) this._wrappedReader);
    }

    private void OnEndOfRootElement()
    {
      if (this._signedXml == null)
      {
        if (this._requireSignature)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID3089")));
      }
      else
      {
        this.ResolveSigningCredentials();
        this._signedXml.StartSignatureVerification(this._signingCredentials.SigningKey);
        this._wrappedReader.XmlTokens.SetElementExclusion("Signature", "http://www.w3.org/2000/09/xmldsig#");
        this._signedXml.EnsureDigestValidity(this._signedXml.Signature.SignedInfo[0].ExtractReferredId(), (object) this._wrappedReader);
        this._signedXml.CompleteSignatureVerification();
        this._signedXml.Dispose();
      }
    }

    public SigningCredentials SigningCredentials => this._signingCredentials;

    internal XmlTokenStream XmlTokens => this._wrappedReader.XmlTokens.Trim();

    public override bool Read()
    {
      if (this.NodeType == XmlNodeType.Element && !this.IsEmptyElement)
        ++this._elementCount;
      if (this.NodeType == XmlNodeType.EndElement)
      {
        --this._elementCount;
        if (this._elementCount == 0)
          this.OnEndOfRootElement();
      }
      bool flag = base.Read();
      if (this._automaticallyReadSignature && this._signedXml == null && (flag && this.InnerReader.IsLocalName("Signature")) && this.InnerReader.IsNamespaceUri("http://www.w3.org/2000/09/xmldsig#"))
        this.ReadSignature();
      return flag;
    }

    private void ReadSignature()
    {
      this._signedXml = new SignedXml(this._tokenSerializer);
      this._signedXml.TransformFactory = TransformFactory.Instance;
      this._signedXml.ReadFrom((XmlDictionaryReader) this._wrappedReader);
      if (this._signedXml.Signature.SignedInfo.ReferenceCount != 1)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID3057")));
    }

    private void ResolveSigningCredentials()
    {
      if (this._signedXml.Signature == null || this._signedXml.Signature.KeyIdentifier == null || this._signedXml.Signature.KeyIdentifier.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3276"));
      SecurityKey key = (SecurityKey) null;
      if (!this._signingTokenResolver.TryResolveSecurityKey(this._signedXml.Signature.KeyIdentifier[0], out key))
      {
        if (this._resolveIntrinsicSigningKeys && this._signedXml.Signature.KeyIdentifier.CanCreateKey)
        {
          if (this._signedXml.Signature.KeyIdentifier.Count < 2)
          {
            key = this._signedXml.Signature.KeyIdentifier.CreateKey();
          }
          else
          {
            foreach (SecurityKeyIdentifierClause identifierClause in this._signedXml.Signature.KeyIdentifier)
            {
              if (identifierClause.CanCreateKey)
              {
                this._signingCredentials = new SigningCredentials(identifierClause.CreateKey(), this._signedXml.Signature.SignedInfo.SignatureMethod, this._signedXml.Signature.SignedInfo[0].DigestMethod, new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
                {
                  identifierClause
                }));
                return;
              }
            }
          }
        }
        else
        {
          if (this._signedXml.Signature.KeyIdentifier.TryFind<EncryptedKeyIdentifierClause>(out EncryptedKeyIdentifierClause _))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SignatureVerificationFailedException(Microsoft.IdentityModel.SR.GetString("ID4036", (object) XmlUtil.SerializeSecurityKeyIdentifier(this._signedXml.Signature.KeyIdentifier, this._tokenSerializer))));
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SignatureVerificationFailedException(Microsoft.IdentityModel.SR.GetString("ID4037", (object) this._signedXml.Signature.KeyIdentifier.ToString())));
        }
      }
      if (this._signedXml.Signature.KeyIdentifier.Count < 2)
        this._signingCredentials = new SigningCredentials(key, this._signedXml.Signature.SignedInfo.SignatureMethod, this._signedXml.Signature.SignedInfo[0].DigestMethod, this._signedXml.Signature.KeyIdentifier);
      else
        this._signingCredentials = new SigningCredentials(key, this._signedXml.Signature.SignedInfo.SignatureMethod, this._signedXml.Signature.SignedInfo[0].DigestMethod, new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
        {
          this._signedXml.Signature.KeyIdentifier[0]
        }));
    }

    public bool TryReadSignature()
    {
      if (!this.IsStartElement("Signature", "http://www.w3.org/2000/09/xmldsig#"))
        return false;
      this.ReadSignature();
      return true;
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this._disposed)
        return;
      if (disposing)
      {
        if (this._signedXml != null)
        {
          this._signedXml.Dispose();
          this._signedXml = (SignedXml) null;
        }
        if (this._wrappedReader != null)
        {
          this._wrappedReader.Close();
          this._wrappedReader = (WrappedReader) null;
        }
      }
      this._disposed = true;
    }
  }
}
