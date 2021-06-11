// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.WrappedX509SecurityTokenAuthenticator
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
  internal class WrappedX509SecurityTokenAuthenticator : X509SecurityTokenAuthenticator
  {
    private X509SecurityTokenHandler _wrappedX509SecurityTokenHandler;
    private SecurityTokenRequirement _securityTokenRequirement;
    private ClaimsAuthenticationManager _claimsAuthenticationManager;
    private ExceptionMapper _exceptionMapper;

    public WrappedX509SecurityTokenAuthenticator(
      X509SecurityTokenHandler wrappedX509SecurityTokenHandler,
      SecurityTokenRequirement securityTokenRequirement,
      ClaimsAuthenticationManager claimsAuthenticationManager,
      ExceptionMapper exceptionMapper)
      : base(X509CertificateValidator.None, WrappedX509SecurityTokenAuthenticator.GetMapToWindowsSetting(wrappedX509SecurityTokenHandler), true)
    {
      if (wrappedX509SecurityTokenHandler == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (wrappedX509SecurityTokenHandler));
      if (securityTokenRequirement == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenRequirement));
      if (claimsAuthenticationManager == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claimsAuthenticationManager));
      if (exceptionMapper == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (exceptionMapper));
      this._wrappedX509SecurityTokenHandler = wrappedX509SecurityTokenHandler;
      this._securityTokenRequirement = securityTokenRequirement;
      this._claimsAuthenticationManager = claimsAuthenticationManager;
      this._exceptionMapper = exceptionMapper;
    }

    protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(
      SecurityToken token)
    {
      ClaimsIdentityCollection identityCollection = (ClaimsIdentityCollection) null;
      try
      {
        identityCollection = this._wrappedX509SecurityTokenHandler.ValidateToken(token);
      }
      catch (Exception ex)
      {
        if (!this._exceptionMapper.HandleSecurityTokenProcessingException(ex))
          throw;
      }
      if (this._securityTokenRequirement is RecipientServiceModelSecurityTokenRequirement tokenRequirement)
      {
        string resourceName = (string) null;
        if (tokenRequirement.ListenUri != (Uri) null)
          resourceName = tokenRequirement.ListenUri.AbsoluteUri;
        IClaimsPrincipal claimsPrincipal = this._claimsAuthenticationManager.Authenticate(resourceName, (IClaimsPrincipal) new ClaimsPrincipal(identityCollection));
        identityCollection = claimsPrincipal != null ? claimsPrincipal.Identities : new ClaimsIdentityCollection();
      }
      bool saveBootstrapTokens = SecurityTokenHandlerConfiguration.DefaultSaveBootstrapTokens;
      if (this._wrappedX509SecurityTokenHandler.Configuration != null)
        saveBootstrapTokens = this._wrappedX509SecurityTokenHandler.Configuration.SaveBootstrapTokens;
      if (saveBootstrapTokens)
      {
        SecurityToken securityToken = !(token is X509SecurityToken x509SecurityToken) ? token : (SecurityToken) new X509SecurityToken(x509SecurityToken.Certificate);
        foreach (IClaimsIdentity claimsIdentity in identityCollection)
          claimsIdentity.BootstrapToken = securityToken;
      }
      return new List<IAuthorizationPolicy>((IEnumerable<IAuthorizationPolicy>) new AuthorizationPolicy[1]
      {
        new AuthorizationPolicy(identityCollection)
      }).AsReadOnly();
    }

    private static bool GetMapToWindowsSetting(X509SecurityTokenHandler securityTokenHandler) => securityTokenHandler != null ? securityTokenHandler.MapToWindows : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenHandler));
  }
}
