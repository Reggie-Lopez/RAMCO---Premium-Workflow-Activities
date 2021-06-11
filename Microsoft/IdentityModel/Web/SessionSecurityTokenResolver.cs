// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.SessionSecurityTokenResolver
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security;

namespace Microsoft.IdentityModel.Web
{
  internal class SessionSecurityTokenResolver : SecurityTokenResolver
  {
    private SecurityTokenCache _tokenCache;
    private string _endpointId;
    private bool _isSessionMode;

    internal SessionSecurityTokenResolver(
      SecurityTokenCache tokenCache,
      string endpointId,
      bool isSessionMode)
    {
      if (tokenCache == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenCache));
      if (endpointId == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (endpointId));
      this._tokenCache = tokenCache;
      this._endpointId = endpointId;
      this._isSessionMode = isSessionMode;
    }

    protected override bool TryResolveSecurityKeyCore(
      SecurityKeyIdentifierClause keyIdentifierClause,
      out SecurityKey key)
    {
      key = (SecurityKey) null;
      SecurityToken token;
      if (!this.TryResolveTokenCore(keyIdentifierClause, out token))
        return false;
      key = token.SecurityKeys[0];
      return true;
    }

    protected override bool TryResolveTokenCore(
      SecurityKeyIdentifier keyIdentifier,
      out SecurityToken token)
    {
      token = (SecurityToken) null;
      SecurityContextKeyIdentifierClause clause;
      return keyIdentifier.TryFind<SecurityContextKeyIdentifierClause>(out clause) && this.TryResolveTokenCore((SecurityKeyIdentifierClause) clause, out token);
    }

    protected override bool TryResolveTokenCore(
      SecurityKeyIdentifierClause keyIdentifierClause,
      out SecurityToken token)
    {
      SecurityContextKeyIdentifierClause identifierClause = keyIdentifierClause as SecurityContextKeyIdentifierClause;
      token = (SecurityToken) null;
      if (identifierClause != null)
        this._tokenCache.TryGetEntry((object) new SecurityTokenCacheKey(this._endpointId, identifierClause.ContextId, identifierClause.Generation, this._isSessionMode), out token);
      return token != null;
    }
  }
}
