// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityTokenHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public abstract class SecurityTokenHandler
  {
    private SecurityTokenHandlerCollection _collection;
    private SecurityTokenHandlerConfiguration _configuration;

    public SecurityTokenHandlerConfiguration Configuration
    {
      get => this._configuration;
      set => this._configuration = value;
    }

    public SecurityTokenHandlerCollection ContainingCollection
    {
      get => this._collection;
      set => this._collection = value;
    }

    public virtual bool CanReadToken(XmlReader reader) => false;

    public virtual SecurityToken ReadToken(XmlReader reader) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID4008", (object) nameof (SecurityTokenHandler), (object) nameof (ReadToken))));

    public virtual SecurityToken ReadToken(
      XmlReader reader,
      SecurityTokenResolver tokenResolver)
    {
      return this.ReadToken(reader);
    }

    public virtual bool CanWriteToken => false;

    protected virtual void DetectReplayedTokens(SecurityToken token)
    {
    }

    public virtual void WriteToken(XmlWriter writer, SecurityToken token) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID4008", (object) nameof (SecurityTokenHandler), (object) nameof (WriteToken))));

    public virtual bool CanReadKeyIdentifierClause(XmlReader reader) => false;

    public virtual SecurityKeyIdentifierClause ReadKeyIdentifierClause(
      XmlReader reader)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID4008", (object) nameof (SecurityTokenHandler), (object) nameof (ReadKeyIdentifierClause))));
    }

    public virtual bool CanWriteKeyIdentifierClause(
      SecurityKeyIdentifierClause securityKeyIdentifierClause)
    {
      return false;
    }

    public virtual void WriteKeyIdentifierClause(
      XmlWriter writer,
      SecurityKeyIdentifierClause securityKeyIdentifierClause)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID4008", (object) nameof (SecurityTokenHandler), (object) nameof (WriteKeyIdentifierClause))));
    }

    public virtual SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID4008", (object) nameof (SecurityTokenHandler), (object) nameof (CreateToken))));

    public virtual SecurityKeyIdentifierClause CreateSecurityTokenReference(
      SecurityToken token,
      bool attached)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID4008", (object) nameof (SecurityTokenHandler), (object) nameof (CreateSecurityTokenReference))));
    }

    public abstract Type TokenType { get; }

    public abstract string[] GetTokenTypeIdentifiers();

    public virtual bool CanValidateToken => false;

    public virtual ClaimsIdentityCollection ValidateToken(
      SecurityToken token)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID4008", (object) nameof (SecurityTokenHandler), (object) nameof (ValidateToken))));
    }
  }
}
