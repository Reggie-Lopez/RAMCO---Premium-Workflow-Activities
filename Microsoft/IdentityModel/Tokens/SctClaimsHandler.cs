// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SctClaimsHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Security.Tokens;

namespace Microsoft.IdentityModel.Tokens
{
  internal class SctClaimsHandler
  {
    private static Type s_type;
    private static Assembly s_assembly;
    private BindingFlags setFieldFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetField;
    private ClaimsAuthenticationManager _claimsAuthenticationManager;
    private SecurityTokenHandlerCollection _securityTokenHandlerCollection;
    private string _endpointId;

    public SctClaimsHandler(
      ClaimsAuthenticationManager claimsAuthenticationManager,
      SecurityTokenHandlerCollection securityTokenHandlerCollection,
      string endpointId)
    {
      if (claimsAuthenticationManager == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claimsAuthenticationManager));
      if (securityTokenHandlerCollection == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenHandlerCollection));
      if (endpointId == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (endpointId));
      this._claimsAuthenticationManager = claimsAuthenticationManager;
      this._securityTokenHandlerCollection = securityTokenHandlerCollection;
      this._endpointId = endpointId;
    }

    public string EndpointId => this._endpointId;

    public SecurityTokenHandlerCollection SecurityTokenHandlerCollection => this._securityTokenHandlerCollection;

    internal void SetPrincipalBootstrapTokensAndBindIdfxAuthPolicy(SecurityContextSecurityToken sct)
    {
      if (sct == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sct));
      List<IAuthorizationPolicy> authorizationPolicyList = new List<IAuthorizationPolicy>();
      if (sct.AuthorizationPolicies != null && sct.AuthorizationPolicies.Count > 0 && this.ContainsEndpointAuthPolicy(sct.AuthorizationPolicies))
        return;
      if (sct.AuthorizationPolicies != null && sct.AuthorizationPolicies.Count > 0)
      {
        AuthorizationPolicy authorizationPolicy1 = IdentityModelServiceAuthorizationManager.TransformAuthorizationPolicies(sct.AuthorizationPolicies, this._claimsAuthenticationManager, this._securityTokenHandlerCollection, false);
        authorizationPolicyList.Add((IAuthorizationPolicy) authorizationPolicy1);
        SctAuthorizationPolicy authorizationPolicy2 = new SctAuthorizationPolicy(this.GetPrimaryIdentityClaim(System.IdentityModel.Policy.AuthorizationContext.CreateDefaultAuthorizationContext((IList<IAuthorizationPolicy>) sct.AuthorizationPolicies)));
        authorizationPolicyList.Add((IAuthorizationPolicy) authorizationPolicy2);
      }
      authorizationPolicyList.Add((IAuthorizationPolicy) new EndpointAuthorizationPolicy(this._endpointId));
      this.ReplaceAuthPolicies(sct, authorizationPolicyList.AsReadOnly());
    }

    private bool ContainsEndpointAuthPolicy(ReadOnlyCollection<IAuthorizationPolicy> policies)
    {
      if (policies == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (policies));
      for (int index = 0; index < policies.Count; ++index)
      {
        if (policies[index] is EndpointAuthorizationPolicy)
          return true;
      }
      return false;
    }

    private System.IdentityModel.Claims.Claim GetPrimaryIdentityClaim(
      System.IdentityModel.Policy.AuthorizationContext authContext)
    {
      if (authContext != null)
      {
        for (int index = 0; index < authContext.ClaimSets.Count; ++index)
        {
          using (IEnumerator<System.IdentityModel.Claims.Claim> enumerator = authContext.ClaimSets[index].FindClaims((string) null, Rights.Identity).GetEnumerator())
          {
            if (enumerator.MoveNext())
              return enumerator.Current;
          }
        }
      }
      return (System.IdentityModel.Claims.Claim) null;
    }

    private void ReplaceAuthPolicies(
      SecurityContextSecurityToken sct,
      ReadOnlyCollection<IAuthorizationPolicy> policies)
    {
      if ((object) SctClaimsHandler.s_assembly == null)
        SctClaimsHandler.s_assembly = typeof (SecurityContextSecurityToken).Assembly;
      if ((object) SctClaimsHandler.s_type == null)
        SctClaimsHandler.s_type = SctClaimsHandler.s_assembly.GetType("System.ServiceModel.Security.Tokens.SecurityContextSecurityToken");
      if ((object) SctClaimsHandler.s_type == null)
        return;
      SctClaimsHandler.s_type.InvokeMember("authorizationPolicies", this.setFieldFlags, (Binder) null, (object) sct, new object[1]
      {
        (object) policies
      }, CultureInfo.InvariantCulture);
    }

    public void OnTokenIssued(SecurityToken issuedToken, EndpointAddress tokenRequestor) => this.SetPrincipalBootstrapTokensAndBindIdfxAuthPolicy(issuedToken as SecurityContextSecurityToken);

    public void OnTokenRenewed(SecurityToken issuedToken, SecurityToken oldToken) => this.SetPrincipalBootstrapTokensAndBindIdfxAuthPolicy(issuedToken as SecurityContextSecurityToken);
  }
}
