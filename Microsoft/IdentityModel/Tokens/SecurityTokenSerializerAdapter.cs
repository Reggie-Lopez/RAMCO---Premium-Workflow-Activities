// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityTokenSerializerAdapter
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.XmlSignature;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class SecurityTokenSerializerAdapter : WSSecurityTokenSerializer
  {
    private SecureConversationVersion _scVersion;
    private SecurityTokenHandlerCollection _securityTokenHandlers;
    private bool _mapExceptionsToSoapFaults;
    private Microsoft.IdentityModel.ExceptionMapper _exceptionMapper = new Microsoft.IdentityModel.ExceptionMapper();

    public SecurityTokenSerializerAdapter(
      SecurityTokenHandlerCollection securityTokenHandlerCollection)
      : this(securityTokenHandlerCollection, MessageSecurityVersion.Default.SecurityVersion)
    {
    }

    public SecurityTokenSerializerAdapter(
      SecurityTokenHandlerCollection securityTokenHandlerCollection,
      SecurityVersion securityVersion)
      : this(securityTokenHandlerCollection, securityVersion, true, new SamlSerializer(), (SecurityStateEncoder) null, (IEnumerable<Type>) null)
    {
    }

    public SecurityTokenSerializerAdapter(
      SecurityTokenHandlerCollection securityTokenHandlerCollection,
      SecurityVersion securityVersion,
      bool emitBspAttributes,
      SamlSerializer samlSerializer,
      SecurityStateEncoder stateEncoder,
      IEnumerable<Type> knownTypes)
      : this(securityTokenHandlerCollection, securityVersion, TrustVersion.WSTrust13, SecureConversationVersion.WSSecureConversation13, emitBspAttributes, samlSerializer, stateEncoder, knownTypes)
    {
    }

    public SecurityTokenSerializerAdapter(
      SecurityTokenHandlerCollection securityTokenHandlerCollection,
      SecurityVersion securityVersion,
      TrustVersion trustVersion,
      SecureConversationVersion secureConversationVersion,
      bool emitBspAttributes,
      SamlSerializer samlSerializer,
      SecurityStateEncoder stateEncoder,
      IEnumerable<Type> knownTypes)
      : base(securityVersion, trustVersion, secureConversationVersion, emitBspAttributes, samlSerializer, stateEncoder, knownTypes)
    {
      if (securityTokenHandlerCollection == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenHandlerCollection));
      this._scVersion = secureConversationVersion;
      this._securityTokenHandlers = securityTokenHandlerCollection;
    }

    public bool MapExceptionsToSoapFaults
    {
      get => this._mapExceptionsToSoapFaults;
      set => this._mapExceptionsToSoapFaults = value;
    }

    public SecurityTokenHandlerCollection SecurityTokenHandlers => this._securityTokenHandlers;

    public Microsoft.IdentityModel.ExceptionMapper ExceptionMapper
    {
      get => this._exceptionMapper;
      set => this._exceptionMapper = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    protected override bool CanReadTokenCore(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      return this._securityTokenHandlers.CanReadToken(reader) || base.CanReadTokenCore(reader);
    }

    protected override bool CanWriteTokenCore(SecurityToken token)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      return this._securityTokenHandlers.CanWriteToken(token) || base.CanWriteTokenCore(token);
    }

    protected override SecurityToken ReadTokenCore(
      XmlReader reader,
      SecurityTokenResolver tokenResolver)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      try
      {
        foreach (SecurityTokenHandler securityTokenHandler in (Collection<SecurityTokenHandler>) this._securityTokenHandlers)
        {
          if (securityTokenHandler.CanReadToken(reader))
          {
            SecurityToken securityToken = securityTokenHandler.ReadToken(reader, tokenResolver);
            if (!(securityToken is SessionSecurityToken sessionSecurityToken))
              return securityToken;
            if (sessionSecurityToken.SecureConversationVersion != this._scVersion)
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4053", (object) sessionSecurityToken.SecureConversationVersion, (object) this._scVersion));
            return (SecurityToken) sessionSecurityToken.SecurityContextSecurityToken;
          }
        }
        return base.ReadTokenCore(reader, tokenResolver);
      }
      catch (Exception ex)
      {
        if (this.MapExceptionsToSoapFaults)
        {
          if (this._exceptionMapper.HandleSecurityTokenProcessingException(ex))
            goto label_18;
        }
        throw;
      }
label_18:
      return (SecurityToken) null;
    }

    protected override void WriteTokenCore(XmlWriter writer, SecurityToken token)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      try
      {
        if (token is SecurityContextSecurityToken securityContextToken)
          token = (SecurityToken) new SessionSecurityToken(securityContextToken, this._scVersion);
        SecurityTokenHandler securityTokenHandler = this._securityTokenHandlers[token];
        if (securityTokenHandler != null && securityTokenHandler.CanWriteToken)
          securityTokenHandler.WriteToken(writer, token);
        else
          base.WriteTokenCore(writer, token);
      }
      catch (Exception ex)
      {
        if (this.MapExceptionsToSoapFaults && this._exceptionMapper.HandleSecurityTokenProcessingException(ex))
          return;
        throw;
      }
    }

    protected override bool CanReadKeyIdentifierCore(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      return reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#") || base.CanReadKeyIdentifierCore(reader);
    }

    protected override SecurityKeyIdentifier ReadKeyIdentifierCore(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4192"));
      KeyInfo keyInfo = new KeyInfo((SecurityTokenSerializer) this);
      keyInfo.ReadXml(XmlDictionaryReader.CreateDictionaryReader(reader));
      return keyInfo.KeyIdentifier;
    }

    protected override bool CanReadKeyIdentifierClauseCore(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      foreach (SecurityTokenHandler securityTokenHandler in (Collection<SecurityTokenHandler>) this._securityTokenHandlers)
      {
        if (securityTokenHandler.CanReadKeyIdentifierClause(reader))
          return true;
      }
      return base.CanReadKeyIdentifierClauseCore(reader);
    }

    protected override bool CanWriteKeyIdentifierClauseCore(
      SecurityKeyIdentifierClause keyIdentifierClause)
    {
      if (keyIdentifierClause == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyIdentifierClause));
      foreach (SecurityTokenHandler securityTokenHandler in (Collection<SecurityTokenHandler>) this._securityTokenHandlers)
      {
        if (securityTokenHandler.CanWriteKeyIdentifierClause(keyIdentifierClause))
          return true;
      }
      return base.CanWriteKeyIdentifierClauseCore(keyIdentifierClause);
    }

    protected override SecurityKeyIdentifierClause ReadKeyIdentifierClauseCore(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      try
      {
        foreach (SecurityTokenHandler securityTokenHandler in (Collection<SecurityTokenHandler>) this._securityTokenHandlers)
        {
          if (securityTokenHandler.CanReadKeyIdentifierClause(reader))
            return securityTokenHandler.ReadKeyIdentifierClause(reader);
        }
        return base.ReadKeyIdentifierClauseCore(reader);
      }
      catch (Exception ex)
      {
        if (this.MapExceptionsToSoapFaults)
        {
          if (this._exceptionMapper.HandleSecurityTokenProcessingException(ex))
            goto label_14;
        }
        throw;
      }
label_14:
      return (SecurityKeyIdentifierClause) null;
    }

    protected override void WriteKeyIdentifierClauseCore(
      XmlWriter writer,
      SecurityKeyIdentifierClause keyIdentifierClause)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (keyIdentifierClause == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyIdentifierClause));
      try
      {
        foreach (SecurityTokenHandler securityTokenHandler in (Collection<SecurityTokenHandler>) this._securityTokenHandlers)
        {
          if (securityTokenHandler.CanWriteKeyIdentifierClause(keyIdentifierClause))
          {
            securityTokenHandler.WriteKeyIdentifierClause(writer, keyIdentifierClause);
            return;
          }
        }
        base.WriteKeyIdentifierClauseCore(writer, keyIdentifierClause);
      }
      catch (Exception ex)
      {
        if (this.MapExceptionsToSoapFaults && this._exceptionMapper.HandleSecurityTokenProcessingException(ex))
          return;
        throw;
      }
    }
  }
}
