// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2Assertion
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.XmlSignature;
using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2Assertion
  {
    private Saml2Advice _advice;
    private Saml2Conditions _conditions;
    private Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials _encryptingCredentials;
    private Collection<EncryptedKeyIdentifierClause> _externalEncryptedKeys = new Collection<EncryptedKeyIdentifierClause>();
    private Saml2Id _id = new Saml2Id();
    private DateTime _issueInstant = DateTime.UtcNow;
    private Saml2NameIdentifier _issuer;
    private SigningCredentials _signingCredentials;
    private XmlTokenStream _sourceData;
    private Collection<Saml2Statement> _statements = new Collection<Saml2Statement>();
    private Saml2Subject _subject;
    private string _version = "2.0";

    public Saml2Assertion(Saml2NameIdentifier issuer) => this._issuer = issuer != null ? issuer : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (issuer));

    public Saml2Advice Advice
    {
      get => this._advice;
      set => this._advice = value;
    }

    public virtual bool CanWriteSourceData => null != this._sourceData;

    public virtual void CaptureSourceData(EnvelopedSignatureReader reader) => this._sourceData = reader != null ? reader.XmlTokens : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));

    public Saml2Conditions Conditions
    {
      get => this._conditions;
      set => this._conditions = value;
    }

    public Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials EncryptingCredentials
    {
      get => this._encryptingCredentials;
      set => this._encryptingCredentials = value;
    }

    public Collection<EncryptedKeyIdentifierClause> ExternalEncryptedKeys => this._externalEncryptedKeys;

    public Saml2Id Id
    {
      get => this._id;
      set
      {
        this._id = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
        this._sourceData = (XmlTokenStream) null;
      }
    }

    public DateTime IssueInstant
    {
      get => this._issueInstant;
      set => this._issueInstant = DateTimeUtil.ToUniversalTime(value);
    }

    public Saml2NameIdentifier Issuer
    {
      get => this._issuer;
      set => this._issuer = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public SigningCredentials SigningCredentials
    {
      get => this._signingCredentials;
      set => this._signingCredentials = value;
    }

    public Saml2Subject Subject
    {
      get => this._subject;
      set => this._subject = value;
    }

    public Collection<Saml2Statement> Statements => this._statements;

    public string Version => this._version;

    public virtual void WriteSourceData(XmlWriter writer)
    {
      if (!this.CanWriteSourceData)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4140")));
      XmlDictionaryWriter dictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter(writer);
      this._sourceData.SetElementExclusion((string) null, (string) null);
      this._sourceData.GetWriter().WriteTo(dictionaryWriter);
    }
  }
}
