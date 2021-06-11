// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml11.Saml11Assertion
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.XmlSignature;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens.Saml11
{
  [ComVisible(true)]
  public class Saml11Assertion : SamlAssertion
  {
    private XmlTokenStream _sourceData;
    private SecurityToken _issuerToken;

    public Saml11Assertion()
    {
    }

    public Saml11Assertion(
      string assertionId,
      string issuer,
      DateTime issueInstant,
      SamlConditions samlConditions,
      SamlAdvice samlAdvice,
      IEnumerable<SamlStatement> samlStatements)
      : base(assertionId, issuer, issueInstant, samlConditions, samlAdvice, samlStatements)
    {
    }

    public new virtual bool CanWriteSourceData => null != this._sourceData;

    public virtual void CaptureSourceData(EnvelopedSignatureReader reader) => this._sourceData = reader != null ? reader.XmlTokens : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));

    public new virtual void WriteSourceData(XmlWriter writer)
    {
      if (!this.CanWriteSourceData)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4140")));
      XmlDictionaryWriter dictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter(writer);
      this._sourceData.SetElementExclusion((string) null, (string) null);
      this._sourceData.GetWriter().WriteTo(dictionaryWriter);
    }

    public SecurityToken IssuerToken
    {
      get => this._issuerToken;
      set => this._issuerToken = value;
    }
  }
}
