// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.SimpleSecurityTokenProvider
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Security.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class SimpleSecurityTokenProvider : SecurityTokenProvider
  {
    private SecurityToken _securityToken;

    public SimpleSecurityTokenProvider(
      SecurityToken token,
      SecurityTokenRequirement tokenRequirement)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (token is GenericXmlSecurityToken issuedToken)
        this._securityToken = (SecurityToken) SimpleSecurityTokenProvider.WrapWithAuthPolicy(issuedToken, tokenRequirement);
      else
        this._securityToken = token;
    }

    protected override SecurityToken GetTokenCore(TimeSpan timeout) => this._securityToken;

    private static GenericXmlSecurityToken WrapWithAuthPolicy(
      GenericXmlSecurityToken issuedToken,
      SecurityTokenRequirement tokenRequirement)
    {
      EndpointIdentity endpointIdentity = (EndpointIdentity) null;
      if (tokenRequirement is InitiatorServiceModelSecurityTokenRequirement tokenRequirement1)
      {
        EndpointAddress targetAddress = tokenRequirement1.TargetAddress;
        if (targetAddress.Uri.IsAbsoluteUri)
          endpointIdentity = EndpointIdentity.CreateDnsIdentity(targetAddress.Uri.DnsSafeHost);
      }
      ReadOnlyCollection<IAuthorizationPolicy> authorizationPolicies = SimpleSecurityTokenProvider.GetServiceAuthorizationPolicies(endpointIdentity);
      return new GenericXmlSecurityToken(issuedToken.TokenXml, issuedToken.ProofToken, issuedToken.ValidFrom, issuedToken.ValidTo, issuedToken.InternalTokenReference, issuedToken.ExternalTokenReference, authorizationPolicies);
    }

    private static ReadOnlyCollection<IAuthorizationPolicy> GetServiceAuthorizationPolicies(
      EndpointIdentity endpointIdentity)
    {
      if (endpointIdentity == null)
        return EmptyReadOnlyCollection<IAuthorizationPolicy>.Instance;
      List<System.IdentityModel.Claims.Claim> claimList = new List<System.IdentityModel.Claims.Claim>(1);
      claimList.Add(endpointIdentity.IdentityClaim);
      List<IAuthorizationPolicy> authorizationPolicyList = new List<IAuthorizationPolicy>(1);
      List<ClaimSet> claimSetList = new List<ClaimSet>()
      {
        (ClaimSet) new DefaultClaimSet((IList<System.IdentityModel.Claims.Claim>) claimList)
      };
      authorizationPolicyList.Add((IAuthorizationPolicy) new ClaimFactoryPolicy(new ReadOnlyCollection<ClaimSet>((IList<ClaimSet>) claimSetList)));
      return authorizationPolicyList.AsReadOnly();
    }
  }
}
