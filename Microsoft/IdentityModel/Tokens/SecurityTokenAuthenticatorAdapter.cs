// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityTokenAuthenticatorAdapter
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security.Tokens;

namespace Microsoft.IdentityModel.Tokens
{
  internal class SecurityTokenAuthenticatorAdapter : SecurityTokenAuthenticator
  {
    private SecurityTokenHandler _securityTokenHandler;
    private SecurityTokenRequirement _securityTokenRequirement;
    private ClaimsAuthenticationManager _claimsAuthenticationManager;
    private ExceptionMapper _exceptionMapper;

    public SecurityTokenAuthenticatorAdapter(
      SecurityTokenHandler securityTokenHandler,
      SecurityTokenRequirement securityTokenRequirement,
      ClaimsAuthenticationManager claimsAuthenticationManager,
      ExceptionMapper exceptionMapper)
    {
      if (securityTokenHandler == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenHandler));
      if (securityTokenRequirement == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenRequirement));
      if (claimsAuthenticationManager == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claimsAuthenticationManager));
      if (exceptionMapper == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (exceptionMapper));
      this._securityTokenHandler = securityTokenHandler;
      this._securityTokenRequirement = securityTokenRequirement;
      this._claimsAuthenticationManager = claimsAuthenticationManager;
      this._exceptionMapper = exceptionMapper;
    }

    protected override bool CanValidateTokenCore(SecurityToken token)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      return (object) token.GetType() == (object) this._securityTokenHandler.TokenType && this._securityTokenHandler.CanValidateToken;
    }

    protected override sealed ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(
      SecurityToken token)
    {
      ClaimsIdentityCollection identityCollection = (ClaimsIdentityCollection) null;
      try
      {
        identityCollection = this._securityTokenHandler.ValidateToken(token);
      }
      catch (Exception ex)
      {
        if (!this._exceptionMapper.HandleSecurityTokenProcessingException(ex))
          throw;
      }
      RecipientServiceModelSecurityTokenRequirement tokenRequirement = this._securityTokenRequirement as RecipientServiceModelSecurityTokenRequirement;
      string resourceName = (string) null;
      if (tokenRequirement != null && tokenRequirement.ListenUri != (Uri) null)
        resourceName = tokenRequirement.ListenUri.AbsoluteUri;
      IClaimsPrincipal claimsPrincipal = this._claimsAuthenticationManager.Authenticate(resourceName, (IClaimsPrincipal) new ClaimsPrincipal(identityCollection));
      return new List<IAuthorizationPolicy>((IEnumerable<IAuthorizationPolicy>) new AuthorizationPolicy[1]
      {
        new AuthorizationPolicy(claimsPrincipal != null ? claimsPrincipal.Identities : new ClaimsIdentityCollection())
      }).AsReadOnly();
    }
  }
}
