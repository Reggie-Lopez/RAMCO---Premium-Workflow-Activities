// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.IdentityModelServiceAuthorizationManager
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class IdentityModelServiceAuthorizationManager : ServiceAuthorizationManager
  {
    protected static readonly ReadOnlyCollection<IAuthorizationPolicy> AnonymousAuthorizationPolicy = new ReadOnlyCollection<IAuthorizationPolicy>((IList<IAuthorizationPolicy>) new List<IAuthorizationPolicy>()
    {
      (IAuthorizationPolicy) new AuthorizationPolicy(ClaimsIdentity.AnonymousIdentity)
    });

    protected override ReadOnlyCollection<IAuthorizationPolicy> GetAuthorizationPolicies(
      OperationContext operationContext)
    {
      ReadOnlyCollection<IAuthorizationPolicy> authorizationPolicies = base.GetAuthorizationPolicies(operationContext);
      if (authorizationPolicies == null)
        return IdentityModelServiceAuthorizationManager.AnonymousAuthorizationPolicy;
      FederatedServiceCredentials serviceCredentials = IdentityModelServiceAuthorizationManager.GetFederatedServiceCredentials();
      AuthorizationPolicy authorizationPolicy = IdentityModelServiceAuthorizationManager.TransformAuthorizationPolicies(authorizationPolicies, serviceCredentials.ClaimsAuthenticationManager, serviceCredentials.SecurityTokenHandlers, true);
      if (authorizationPolicy == null || authorizationPolicy.IdentityCollection.Count == 0)
        return IdentityModelServiceAuthorizationManager.AnonymousAuthorizationPolicy;
      return new List<IAuthorizationPolicy>()
      {
        (IAuthorizationPolicy) authorizationPolicy
      }.AsReadOnly();
    }

    internal static AuthorizationPolicy TransformAuthorizationPolicies(
      ReadOnlyCollection<IAuthorizationPolicy> baseAuthorizationPolicies,
      ClaimsAuthenticationManager authnManager,
      SecurityTokenHandlerCollection securityTokenHandlerCollection,
      bool includeTransportTokens)
    {
      ClaimsIdentityCollection identityCollection = new ClaimsIdentityCollection();
      List<IAuthorizationPolicy> baseAuthorizationPolicies1 = new List<IAuthorizationPolicy>();
      foreach (IAuthorizationPolicy authorizationPolicy1 in baseAuthorizationPolicies)
      {
        switch (authorizationPolicy1)
        {
          case SctAuthorizationPolicy _:
          case EndpointAuthorizationPolicy _:
            continue;
          case AuthorizationPolicy authorizationPolicy:
            identityCollection.AddRange((IEnumerable<IClaimsIdentity>) authorizationPolicy.IdentityCollection);
            continue;
          default:
            baseAuthorizationPolicies1.Add(authorizationPolicy1);
            continue;
        }
      }
      if (includeTransportTokens && OperationContext.Current != null && (OperationContext.Current.IncomingMessageProperties != null && OperationContext.Current.IncomingMessageProperties.Security != null) && OperationContext.Current.IncomingMessageProperties.Security.TransportToken != null)
      {
        SecurityToken securityToken = OperationContext.Current.IncomingMessageProperties.Security.TransportToken.SecurityToken;
        ReadOnlyCollection<IAuthorizationPolicy> securityTokenPolicies = OperationContext.Current.IncomingMessageProperties.Security.TransportToken.SecurityTokenPolicies;
        bool flag = true;
        foreach (IAuthorizationPolicy authorizationPolicy in securityTokenPolicies)
        {
          if (authorizationPolicy is AuthorizationPolicy)
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          ClaimsIdentityCollection transportTokenIdentities = IdentityModelServiceAuthorizationManager.GetTransportTokenIdentities(securityToken, authnManager);
          identityCollection.AddRange((IEnumerable<IClaimsIdentity>) transportTokenIdentities);
          IdentityModelServiceAuthorizationManager.EliminateTransportTokenPolicy(securityToken, transportTokenIdentities, baseAuthorizationPolicies1);
        }
      }
      if (baseAuthorizationPolicies1.Count > 0)
        identityCollection.AddRange((IEnumerable<IClaimsIdentity>) IdentityModelServiceAuthorizationManager.ConvertToIDFxIdentities((IList<IAuthorizationPolicy>) baseAuthorizationPolicies1, authnManager, securityTokenHandlerCollection));
      return identityCollection.Count != 0 ? new AuthorizationPolicy(identityCollection) : new AuthorizationPolicy(ClaimsIdentity.AnonymousIdentity);
    }

    private static ClaimsIdentityCollection GetTransportTokenIdentities(
      SecurityToken transportToken,
      ClaimsAuthenticationManager authnManager)
    {
      if (transportToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (transportToken));
      if (authnManager == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (authnManager));
      FederatedServiceCredentials serviceCredentials = IdentityModelServiceAuthorizationManager.GetFederatedServiceCredentials();
      ClaimsIdentityCollection identityCollection = (ClaimsIdentityCollection) null;
      if (transportToken is X509SecurityToken || transportToken is UserNameSecurityToken)
        identityCollection = serviceCredentials.SecurityTokenHandlers.ValidateToken(transportToken);
      if (transportToken is WindowsSecurityToken windowsSecurityToken)
      {
        string windowsIssuerName = serviceCredentials.SecurityTokenHandlers.Configuration.IssuerNameRegistry.GetWindowsIssuerName();
        WindowsClaimsIdentity windowsClaimsIdentity = new WindowsClaimsIdentity(windowsSecurityToken.WindowsIdentity.Token, "Windows", windowsIssuerName);
        IdentityModelServiceAuthorizationManager.AddAuthenticationMethod((IClaimsIdentity) windowsClaimsIdentity, "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows");
        IdentityModelServiceAuthorizationManager.AddAuthenticationInstantClaim((IClaimsIdentity) windowsClaimsIdentity, XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), windowsIssuerName);
        if (serviceCredentials.SaveBootstrapTokens)
          windowsClaimsIdentity.BootstrapToken = transportToken;
        identityCollection = new ClaimsIdentityCollection((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
        {
          (IClaimsIdentity) windowsClaimsIdentity
        });
      }
      return authnManager.Authenticate(OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri.AbsoluteUri, (IClaimsPrincipal) new ClaimsPrincipal(identityCollection)).Identities;
    }

    private static void EliminateTransportTokenPolicy(
      SecurityToken transportToken,
      ClaimsIdentityCollection tranportTokenIdentities,
      List<IAuthorizationPolicy> baseAuthorizationPolicies)
    {
      if (transportToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (transportToken));
      if (tranportTokenIdentities == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tranportTokenIdentities));
      if (baseAuthorizationPolicies == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("baseAuthorizationPolicy");
      if (baseAuthorizationPolicies.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("baseAuthorizationPolicy", Microsoft.IdentityModel.SR.GetString("ID0020"));
      IAuthorizationPolicy authorizationPolicy1 = (IAuthorizationPolicy) null;
      foreach (IAuthorizationPolicy authorizationPolicy2 in baseAuthorizationPolicies)
      {
        if (IdentityModelServiceAuthorizationManager.DoesPolicyMatchTransportToken(transportToken, tranportTokenIdentities, authorizationPolicy2))
        {
          authorizationPolicy1 = authorizationPolicy2;
          break;
        }
      }
      if (authorizationPolicy1 == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4271", (object) transportToken));
      baseAuthorizationPolicies.Remove(authorizationPolicy1);
    }

    private static bool DoesPolicyMatchTransportToken(
      SecurityToken transportToken,
      ClaimsIdentityCollection tranportTokenIdentities,
      IAuthorizationPolicy authPolicy)
    {
      if (transportToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (transportToken));
      if (tranportTokenIdentities == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tranportTokenIdentities));
      if (authPolicy == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (authPolicy));
      X509SecurityToken x509SecurityToken = transportToken as X509SecurityToken;
      foreach (ClaimSet claimSet in System.IdentityModel.Policy.AuthorizationContext.CreateDefaultAuthorizationContext((IList<IAuthorizationPolicy>) new List<IAuthorizationPolicy>()
      {
        authPolicy
      }).ClaimSets)
      {
        if (x509SecurityToken != null)
        {
          if (claimSet.ContainsClaim(new System.IdentityModel.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumbprint", (object) x509SecurityToken.Certificate.GetCertHash(), Rights.PossessProperty)))
            return true;
        }
        else
        {
          foreach (IClaimsIdentity tranportTokenIdentity in tranportTokenIdentities)
          {
            if (claimSet.ContainsClaim(new System.IdentityModel.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", (object) tranportTokenIdentity.Name, Rights.PossessProperty), (IEqualityComparer<System.IdentityModel.Claims.Claim>) new ClaimStringValueComparer()))
              return true;
          }
        }
      }
      return false;
    }

    private static ClaimsIdentityCollection ConvertToIDFxIdentities(
      IList<IAuthorizationPolicy> authorizationPolicies,
      ClaimsAuthenticationManager authnManager,
      SecurityTokenHandlerCollection securityTokenHandlerCollection)
    {
      if (authorizationPolicies == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (authorizationPolicies));
      if (authnManager == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (authnManager));
      if (securityTokenHandlerCollection == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenHandlerCollection));
      ClaimsIdentityCollection identityCollection1 = new ClaimsIdentityCollection();
      SecurityTokenSpecification tokenSpecification1 = (SecurityTokenSpecification) null;
      System.IdentityModel.Policy.AuthorizationContext authorizationContext1 = (System.IdentityModel.Policy.AuthorizationContext) null;
      if (OperationContext.Current != null && OperationContext.Current.IncomingMessageProperties != null && OperationContext.Current.IncomingMessageProperties.Security != null)
      {
        foreach (SecurityTokenSpecification tokenSpecification2 in new SecurityTokenSpecificationEnumerable(OperationContext.Current.IncomingMessageProperties.Security))
        {
          if (tokenSpecification2.SecurityToken is KerberosReceiverSecurityToken)
          {
            tokenSpecification1 = tokenSpecification2;
            authorizationContext1 = System.IdentityModel.Policy.AuthorizationContext.CreateDefaultAuthorizationContext((IList<IAuthorizationPolicy>) tokenSpecification1.SecurityTokenPolicies);
            break;
          }
        }
      }
      bool flag1 = false;
      foreach (IAuthorizationPolicy authorizationPolicy in (IEnumerable<IAuthorizationPolicy>) authorizationPolicies)
      {
        IClaimsPrincipal claimsPrincipal = (IClaimsPrincipal) null;
        bool flag2 = false;
        if (tokenSpecification1 != null && !flag1)
        {
          if (tokenSpecification1.SecurityTokenPolicies.Contains(authorizationPolicy))
          {
            flag1 = true;
          }
          else
          {
            System.IdentityModel.Policy.AuthorizationContext authorizationContext2 = System.IdentityModel.Policy.AuthorizationContext.CreateDefaultAuthorizationContext((IList<IAuthorizationPolicy>) new List<IAuthorizationPolicy>()
            {
              authorizationPolicy
            });
            if (authorizationContext2.ClaimSets.Count == 1)
            {
              bool flag3 = true;
              foreach (System.IdentityModel.Claims.Claim claim in authorizationContext2.ClaimSets[0])
              {
                if (!authorizationContext1.ClaimSets[0].ContainsClaim(claim))
                {
                  flag3 = false;
                  break;
                }
              }
              flag1 = flag3;
            }
          }
          if (flag1)
          {
            SecurityTokenHandler securityTokenHandler = securityTokenHandlerCollection[tokenSpecification1.SecurityToken];
            if (securityTokenHandler != null && securityTokenHandler.CanValidateToken)
            {
              ClaimsIdentityCollection identityCollection2 = securityTokenHandler.ValidateToken(tokenSpecification1.SecurityToken);
              claimsPrincipal = authnManager.Authenticate(OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri.AbsoluteUri, (IClaimsPrincipal) new ClaimsPrincipal(identityCollection2));
              flag2 = true;
            }
          }
        }
        if (!flag2)
        {
          IClaimsIdentity idFxIdentity = IdentityModelServiceAuthorizationManager.ConvertToIDFxIdentity((IList<ClaimSet>) System.IdentityModel.Policy.AuthorizationContext.CreateDefaultAuthorizationContext((IList<IAuthorizationPolicy>) new List<IAuthorizationPolicy>()
          {
            authorizationPolicy
          }).ClaimSets, securityTokenHandlerCollection.Configuration);
          claimsPrincipal = authnManager.Authenticate(OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri.AbsoluteUri, (IClaimsPrincipal) new ClaimsPrincipal((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
          {
            idFxIdentity
          }));
        }
        identityCollection1.AddRange((IEnumerable<IClaimsIdentity>) claimsPrincipal.Identities);
      }
      return identityCollection1;
    }

    private static IClaimsIdentity ConvertToIDFxIdentity(
      IList<ClaimSet> claimSets,
      SecurityTokenHandlerConfiguration securityTokenHandlerConfiguration)
    {
      if (claimSets == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claimSets));
      IClaimsIdentity claimsIdentity = (IClaimsIdentity) null;
      foreach (ClaimSet claimSet in (IEnumerable<ClaimSet>) claimSets)
      {
        if (claimSet is WindowsClaimSet windowsClaimSet)
        {
          string windowsIssuerName = securityTokenHandlerConfiguration.IssuerNameRegistry.GetWindowsIssuerName();
          claimsIdentity = IdentityModelServiceAuthorizationManager.MergeClaims(claimsIdentity, (IClaimsIdentity) new WindowsClaimsIdentity(windowsClaimSet.WindowsIdentity.Token, "Negotiate", windowsIssuerName));
          IdentityModelServiceAuthorizationManager.AddAuthenticationMethod(claimsIdentity, "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows");
          IdentityModelServiceAuthorizationManager.AddAuthenticationInstantClaim(claimsIdentity, XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), windowsIssuerName);
        }
        else
        {
          claimsIdentity = IdentityModelServiceAuthorizationManager.MergeClaims(claimsIdentity, (IClaimsIdentity) new ClaimsIdentity(claimSet));
          IdentityModelServiceAuthorizationManager.AddAuthenticationInstantClaim(claimsIdentity, XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated));
        }
      }
      return claimsIdentity;
    }

    private static FederatedServiceCredentials GetFederatedServiceCredentials()
    {
      ServiceCredentials serviceCredentials1 = (ServiceCredentials) null;
      if (OperationContext.Current != null && OperationContext.Current.Host != null && (OperationContext.Current.Host.Description != null && OperationContext.Current.Host.Description.Behaviors != null))
        serviceCredentials1 = OperationContext.Current.Host.Description.Behaviors.Find<ServiceCredentials>();
      return serviceCredentials1 is FederatedServiceCredentials serviceCredentials2 ? serviceCredentials2 : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4267", (object) serviceCredentials1));
    }

    private static void AddAuthenticationMethod(
      IClaimsIdentity claimsIdentity,
      string authenticationMethod)
    {
      if (claimsIdentity.Claims.FirstOrDefault<Microsoft.IdentityModel.Claims.Claim>((Func<Microsoft.IdentityModel.Claims.Claim, bool>) (claim => claim.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod")) != null)
        return;
      claimsIdentity.Claims.Add(new Microsoft.IdentityModel.Claims.Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", authenticationMethod));
    }

    private static void AddAuthenticationInstantClaim(
      IClaimsIdentity claimsIdentity,
      string authenticationInstant)
    {
      IdentityModelServiceAuthorizationManager.AddAuthenticationInstantClaim(claimsIdentity, authenticationInstant, "LOCAL AUTHORITY");
    }

    private static void AddAuthenticationInstantClaim(
      IClaimsIdentity claimsIdentity,
      string authenticationInstant,
      string issuerName)
    {
      if (claimsIdentity.Claims.FirstOrDefault<Microsoft.IdentityModel.Claims.Claim>((Func<Microsoft.IdentityModel.Claims.Claim, bool>) (claim => claim.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant")) != null)
        return;
      claimsIdentity.Claims.Add(new Microsoft.IdentityModel.Claims.Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", authenticationInstant, "http://www.w3.org/2001/XMLSchema#dateTime", issuerName));
    }

    internal static IClaimsIdentity MergeClaims(
      IClaimsIdentity identity1,
      IClaimsIdentity identity2)
    {
      if (identity1 == null && identity2 == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4268"));
      if (identity1 == null)
        return identity2;
      if (identity2 == null)
        return identity1;
      if (identity1 is WindowsClaimsIdentity windowsClaimsIdentity)
      {
        windowsClaimsIdentity.Claims.CopyRange((IEnumerable<Microsoft.IdentityModel.Claims.Claim>) identity2.Claims);
        return (IClaimsIdentity) windowsClaimsIdentity;
      }
      if (identity2 is WindowsClaimsIdentity windowsClaimsIdentity)
      {
        windowsClaimsIdentity.Claims.CopyRange((IEnumerable<Microsoft.IdentityModel.Claims.Claim>) identity1.Claims);
        return (IClaimsIdentity) windowsClaimsIdentity;
      }
      identity1.Claims.CopyRange((IEnumerable<Microsoft.IdentityModel.Claims.Claim>) identity2.Claims);
      return identity1;
    }

    protected override bool CheckAccessCore(OperationContext operationContext)
    {
      if (operationContext == null)
        return false;
      string action = string.Empty;
      if (!string.IsNullOrEmpty(operationContext.IncomingMessageHeaders.Action))
        action = operationContext.IncomingMessageHeaders.Action;
      else if (operationContext.IncomingMessageProperties[HttpRequestMessageProperty.Name] is HttpRequestMessageProperty incomingMessageProperty)
        action = incomingMessageProperty.Method;
      Uri to = operationContext.IncomingMessageHeaders.To;
      FederatedServiceCredentials serviceCredentials = IdentityModelServiceAuthorizationManager.GetFederatedServiceCredentials();
      if (serviceCredentials == null || string.IsNullOrEmpty(action) || to == (Uri) null)
        return false;
      operationContext.IncomingMessageProperties["ServiceConfiguration"] = (object) serviceCredentials.ServiceConfiguration;
      if (!(operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] is IClaimsPrincipal property) || property.Identities == null)
        return false;
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, Microsoft.IdentityModel.SR.GetString("TraceAuthorize"), (TraceRecord) new AuthorizeTraceRecord(property, to.AbsoluteUri, action), (Exception) null);
      bool flag = serviceCredentials.ClaimsAuthorizationManager.CheckAccess(new Microsoft.IdentityModel.Claims.AuthorizationContext(property, to.AbsoluteUri, action));
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
      {
        if (flag)
          DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, Microsoft.IdentityModel.SR.GetString("TraceOnAuthorizeRequestSucceed"));
        else
          DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, Microsoft.IdentityModel.SR.GetString("TraceOnAuthorizeRequestFailed"));
      }
      return flag;
    }
  }
}
