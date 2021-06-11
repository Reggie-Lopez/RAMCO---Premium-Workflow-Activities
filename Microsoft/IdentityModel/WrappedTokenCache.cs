// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.WrappedTokenCache
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;

namespace Microsoft.IdentityModel
{
  internal class WrappedTokenCache : SecurityTokenResolver, ISecurityContextSecurityTokenCache
  {
    private SecurityTokenCache _tokenCache;
    private Microsoft.IdentityModel.Tokens.SctClaimsHandler _claimsHandler;
    private bool _isSessionMode;

    public WrappedTokenCache(
      SecurityTokenCache tokenCache,
      Microsoft.IdentityModel.Tokens.SctClaimsHandler sctClaimsHandler,
      bool isSessionMode)
    {
      if (tokenCache == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenCache));
      if (sctClaimsHandler == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sctClaimsHandler));
      this._tokenCache = tokenCache;
      this._claimsHandler = sctClaimsHandler;
      this._isSessionMode = isSessionMode;
    }

    public bool IsSessionMode => this._isSessionMode;

    public void AddContext(SecurityContextSecurityToken token)
    {
      this._claimsHandler.SetPrincipalBootstrapTokensAndBindIdfxAuthPolicy(token);
      if (!this._tokenCache.TryAddEntry((object) new SecurityTokenCacheKey(this._claimsHandler.EndpointId, token.ContextId, token.KeyGeneration, this._isSessionMode), (SecurityToken) new Microsoft.IdentityModel.Tokens.SessionSecurityToken(token, SecureConversationVersion.Default)))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4234"));
    }

    public void ClearContexts() => this._tokenCache.TryRemoveAllEntries((object) new SecurityTokenCacheKey(this._claimsHandler.EndpointId, (System.Xml.UniqueId) null, (System.Xml.UniqueId) null, this._isSessionMode)
    {
      CanIgnoreContextId = true,
      CanIgnoreKeyGeneration = true
    });

    public Collection<SecurityContextSecurityToken> GetAllContexts(
      System.Xml.UniqueId contextId)
    {
      SecurityTokenCacheKey securityTokenCacheKey = new SecurityTokenCacheKey(this._claimsHandler.EndpointId, contextId, (System.Xml.UniqueId) null, this._isSessionMode);
      securityTokenCacheKey.CanIgnoreKeyGeneration = true;
      Collection<SecurityContextSecurityToken> collection = new Collection<SecurityContextSecurityToken>();
      IList<SecurityToken> tokens = (IList<SecurityToken>) null;
      this._tokenCache.TryGetAllEntries((object) securityTokenCacheKey, out tokens);
      if (tokens != null)
      {
        for (int index = 0; index < tokens.Count; ++index)
        {
          if (!(tokens[index] is SecurityContextSecurityToken contextSecurityToken) && tokens[index] is Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken && sessionSecurityToken.IsSecurityContextSecurityTokenWrapper)
            contextSecurityToken = sessionSecurityToken.SecurityContextSecurityToken;
          if (contextSecurityToken != null)
            collection.Add(contextSecurityToken);
        }
      }
      return collection;
    }

    public SecurityContextSecurityToken GetContext(
      System.Xml.UniqueId contextId,
      System.Xml.UniqueId generation)
    {
      SecurityToken securityToken;
      this._tokenCache.TryGetEntry((object) new SecurityTokenCacheKey(this._claimsHandler.EndpointId, contextId, generation, this._isSessionMode) ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4286")), out securityToken);
      switch (securityToken)
      {
        case Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken:
          if (sessionSecurityToken.IsSecurityContextSecurityTokenWrapper)
          {
            pattern_0 = sessionSecurityToken.SecurityContextSecurityToken;
            break;
          }
          break;
      }
      return pattern_0;
    }

    public void RemoveAllContexts(System.Xml.UniqueId contextId) => this._tokenCache.TryRemoveAllEntries((object) new SecurityTokenCacheKey(this._claimsHandler.EndpointId, contextId, (System.Xml.UniqueId) null, this._isSessionMode)
    {
      CanIgnoreKeyGeneration = true
    });

    public void RemoveContext(System.Xml.UniqueId contextId, System.Xml.UniqueId generation) => this._tokenCache.TryRemoveEntry((object) new SecurityTokenCacheKey(this._claimsHandler.EndpointId, contextId, generation, this._isSessionMode) ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4286")));

    public bool TryAddContext(SecurityContextSecurityToken token)
    {
      this._claimsHandler.SetPrincipalBootstrapTokensAndBindIdfxAuthPolicy(token);
      object key = (object) new SecurityTokenCacheKey(this._claimsHandler.EndpointId, token.ContextId, token.KeyGeneration, this._isSessionMode);
      if (key == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4286"));
      Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken = new Microsoft.IdentityModel.Tokens.SessionSecurityToken(token, SecureConversationVersion.Default);
      return this._tokenCache.TryAddEntry(key, (SecurityToken) sessionSecurityToken);
    }

    public void UpdateContextCachingTime(
      SecurityContextSecurityToken token,
      DateTime expirationTime)
    {
      if (token.ValidTo <= expirationTime.ToUniversalTime())
        return;
      object key = (object) new SecurityTokenCacheKey(this._claimsHandler.EndpointId, token.ContextId, token.KeyGeneration, this._isSessionMode);
      if (key == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4286"));
      Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken = new Microsoft.IdentityModel.Tokens.SessionSecurityToken(token, SecureConversationVersion.Default);
      if (!this._tokenCache.TryReplaceEntry(key, (SecurityToken) sessionSecurityToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4285", (object) token.ContextId.ToString()));
    }

    protected override bool TryResolveSecurityKeyCore(
      SecurityKeyIdentifierClause keyIdentifierClause,
      out SecurityKey key)
    {
      SecurityToken token;
      if (this.TryResolveTokenCore(keyIdentifierClause, out token))
      {
        key = token.SecurityKeys[0];
        return true;
      }
      key = (SecurityKey) null;
      return false;
    }

    protected override bool TryResolveTokenCore(
      SecurityKeyIdentifierClause keyIdentifierClause,
      out SecurityToken token)
    {
      token = !(keyIdentifierClause is SecurityContextKeyIdentifierClause identifierClause) ? (SecurityToken) null : (SecurityToken) this.GetContext(identifierClause.ContextId, identifierClause.Generation);
      return token != null;
    }

    protected override bool TryResolveTokenCore(
      SecurityKeyIdentifier keyIdentifier,
      out SecurityToken token)
    {
      SecurityContextKeyIdentifierClause clause;
      if (keyIdentifier.TryFind<SecurityContextKeyIdentifierClause>(out clause))
        return this.TryResolveTokenCore((SecurityKeyIdentifierClause) clause, out token);
      token = (SecurityToken) null;
      return false;
    }
  }
}
