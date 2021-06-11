// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.EnvelopingSignatureReader
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  [ComVisible(true)]
  public sealed class EnvelopingSignatureReader : DelegatingXmlDictionaryReader
  {
    private SignedXml _signedXml;
    private SigningCredentials _signingCredentials;
    private bool _disposed;

    public EnvelopingSignatureReader(
      XmlReader innerReader,
      SecurityTokenSerializer securityTokenSerializer)
      : this(innerReader, securityTokenSerializer, (SecurityTokenResolver) null)
    {
    }

    public EnvelopingSignatureReader(
      XmlReader innerReader,
      SecurityTokenSerializer securityTokenSerializer,
      SecurityTokenResolver signingTokenResolver)
    {
      if (innerReader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (innerReader));
      if (securityTokenSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenSerializer));
      SecurityTokenResolver securityTokenResolver = signingTokenResolver != null ? signingTokenResolver : Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance;
      XmlDictionaryReader dictionaryReader = XmlDictionaryReader.CreateDictionaryReader(innerReader);
      this._signedXml = new SignedXml(securityTokenSerializer);
      this._signedXml.ReadFrom(dictionaryReader);
      if (this._signedXml.Signature == null || this._signedXml.Signature.KeyIdentifier == null || this._signedXml.Signature.KeyIdentifier.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3276"));
      if (this._signedXml.Signature.SignedObjects.Count != 1)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3039")));
      XmlDictionaryReader signedObjectReader = this._signedXml.Signature.GetSignedObjectReader(0);
      int content = (int) signedObjectReader.MoveToContent();
      string attribute = signedObjectReader.GetAttribute("Id", (string) null);
      SecurityKey key = (SecurityKey) null;
      bool flag = false;
      if (!securityTokenResolver.TryResolveSecurityKey(this._signedXml.Signature.KeyIdentifier[0], out key))
      {
        if (this._signedXml.Signature.KeyIdentifier.CanCreateKey)
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
                flag = true;
                break;
              }
            }
          }
        }
        else
        {
          if (this._signedXml.Signature.KeyIdentifier.TryFind<EncryptedKeyIdentifierClause>(out EncryptedKeyIdentifierClause _))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SignatureVerificationFailedException(Microsoft.IdentityModel.SR.GetString("ID4036", (object) XmlUtil.SerializeSecurityKeyIdentifier(this._signedXml.Signature.KeyIdentifier, securityTokenSerializer))));
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SignatureVerificationFailedException(Microsoft.IdentityModel.SR.GetString("ID4037", (object) this._signedXml.Signature.KeyIdentifier.ToString())));
        }
      }
      if (!flag)
      {
        if (this._signedXml.Signature.KeyIdentifier.Count < 2)
          this._signingCredentials = new SigningCredentials(key, this._signedXml.Signature.SignedInfo.SignatureMethod, this._signedXml.Signature.SignedInfo[0].DigestMethod, this._signedXml.Signature.KeyIdentifier);
        else
          this._signingCredentials = new SigningCredentials(key, this._signedXml.Signature.SignedInfo.SignatureMethod, this._signedXml.Signature.SignedInfo[0].DigestMethod, new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
          {
            this._signedXml.Signature.KeyIdentifier[0]
          }));
      }
      this._signedXml.StartSignatureVerification(this._signingCredentials.SigningKey);
      this._signedXml.Signature.SignedInfo.EnsureDigestValidity(attribute, (object) this._signedXml.Signature.GetSignedObjectReader(0));
      this._signedXml.CompleteSignatureVerification();
      this.InitializeInnerReader(this._signedXml.Signature.GetSignedObjectReader(0));
      this.InnerReader.ReadStartElement("Object", "http://www.w3.org/2000/09/xmldsig#");
    }

    public SigningCredentials SigningCredentials => this._signingCredentials;

    private void OnEndOfRootElement()
    {
      this._signedXml.Dispose();
      this._signedXml = (SignedXml) null;
    }

    public override bool Read()
    {
      if (this.NodeType == XmlNodeType.EndElement && this.Depth == 1)
      {
        bool flag = base.Read();
        this.OnEndOfRootElement();
        return flag;
      }
      return (this.NodeType != XmlNodeType.EndElement || this.Depth != 0) && base.Read();
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this._disposed)
        return;
      if (disposing && this._signedXml != null)
      {
        this._signedXml.Dispose();
        this._signedXml = (SignedXml) null;
      }
      this._disposed = true;
    }
  }
}
