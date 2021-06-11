// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.ClaimsPrincipal
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Microsoft.IdentityModel.Claims
{
  [ComVisible(true)]
  [Serializable]
  public class ClaimsPrincipal : IClaimsPrincipal, IPrincipal
  {
    private ClaimsIdentityCollection _identities = new ClaimsIdentityCollection();

    public static IClaimsPrincipal CreateFromPrincipal(IPrincipal principal) => ClaimsPrincipal.CreateFromPrincipal(principal, "LOCAL AUTHORITY");

    public static IClaimsPrincipal CreateFromPrincipal(
      IPrincipal principal,
      string windowsIssuerName)
    {
      if (principal == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (principal));
      if (principal is IClaimsPrincipal claimsPrincipal)
        return claimsPrincipal;
      if (principal.Identity is WindowsClaimsIdentity identity)
        return (IClaimsPrincipal) new WindowsClaimsPrincipal(identity);
      if (principal.Identity is IClaimsIdentity identity)
        return (IClaimsPrincipal) new ClaimsPrincipal()
        {
          Identities = {
            identity
          }
        };
      switch (principal)
      {
        case WindowsPrincipal windowsPrincipal:
          return WindowsClaimsPrincipal.CreateFromWindowsIdentity((WindowsIdentity) windowsPrincipal.Identity, windowsIssuerName);
        case RolePrincipal rolePrincipal:
          ClaimsPrincipal claimsPrincipal1 = new ClaimsPrincipal((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
          {
            ClaimsIdentity.CreateFromIdentity(rolePrincipal.Identity)
          });
          foreach (string role in rolePrincipal.GetRoles())
            claimsPrincipal1.Identities[0].Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role));
          return (IClaimsPrincipal) claimsPrincipal1;
        default:
          return (IClaimsPrincipal) new ClaimsPrincipal(principal);
      }
    }

    public static IClaimsPrincipal AnonymousPrincipal => (IClaimsPrincipal) new ClaimsPrincipal(new ClaimsIdentityCollection((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
    {
      ClaimsIdentity.AnonymousIdentity
    }));

    public static IClaimsPrincipal CreateFromIdentities(
      ClaimsIdentityCollection identities)
    {
      return ClaimsPrincipal.CreateFromIdentities(identities, "LOCAL AUTHORITY");
    }

    public static IClaimsPrincipal CreateFromIdentities(
      ClaimsIdentityCollection identities,
      string windowsIssuerName)
    {
      IClaimsIdentity claimsIdentity = ClaimsPrincipal.SelectPrimaryIdentity(identities);
      if (claimsIdentity == null)
        return ClaimsPrincipal.AnonymousPrincipal;
      IClaimsPrincipal fromIdentity = ClaimsPrincipal.CreateFromIdentity((IIdentity) claimsIdentity, windowsIssuerName);
      foreach (IClaimsIdentity identity in identities)
      {
        if (identity != claimsIdentity)
          fromIdentity.Identities.Add(identity);
      }
      return fromIdentity;
    }

    public static IClaimsPrincipal CreateFromIdentity(IIdentity identity) => ClaimsPrincipal.CreateFromIdentity(identity, "LOCAL AUTHORITY");

    public static IClaimsPrincipal CreateFromIdentity(
      IIdentity identity,
      string windowsIssuerName)
    {
      switch (identity)
      {
        case null:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identity));
        case WindowsClaimsIdentity identity1:
          return (IClaimsPrincipal) new WindowsClaimsPrincipal(identity1);
        case WindowsIdentity identity1:
          return WindowsClaimsPrincipal.CreateFromWindowsIdentity(identity1, windowsIssuerName);
        case IClaimsIdentity claimsIdentity:
          return (IClaimsPrincipal) new ClaimsPrincipal(new ClaimsIdentityCollection((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
          {
            claimsIdentity
          }));
        default:
          return (IClaimsPrincipal) new ClaimsPrincipal(new ClaimsIdentityCollection((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
          {
            (IClaimsIdentity) new ClaimsIdentity(identity)
          }));
      }
    }

    public static IClaimsPrincipal CreateFromHttpContext(HttpContext httpContext) => ClaimsPrincipal.CreateFromHttpContext(httpContext, false);

    public static IClaimsPrincipal CreateFromHttpContext(
      HttpContext httpContext,
      bool clientCertificateAuthenticationEnabled)
    {
      if (httpContext == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (httpContext));
      ServiceConfiguration current = ServiceConfiguration.GetCurrent();
      IClaimsPrincipal claimsPrincipal = httpContext.User == null ? ClaimsPrincipal.AnonymousPrincipal : ClaimsPrincipal.CreateFromPrincipal(httpContext.User, current.IssuerNameRegistry.GetWindowsIssuerName());
      if (clientCertificateAuthenticationEnabled)
      {
        HttpClientCertificate clientCertificate = httpContext.Request.ClientCertificate;
        if (clientCertificate != null && clientCertificate.IsPresent && clientCertificate.IsValid)
        {
          X509Certificate2 certificate = new X509Certificate2(clientCertificate.Certificate);
          string certificateIssuerName = X509Util.GetCertificateIssuerName(certificate, current.IssuerNameRegistry);
          ClaimsIdentity claimsIdentity = !string.IsNullOrEmpty(certificateIssuerName) ? new ClaimsIdentity(certificate, certificateIssuerName) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4174")));
          if (claimsPrincipal == null || !claimsPrincipal.Identity.IsAuthenticated)
            claimsPrincipal = ClaimsPrincipal.CreateFromIdentity((IIdentity) claimsIdentity);
          else
            claimsPrincipal.Identities.Add((IClaimsIdentity) claimsIdentity);
        }
      }
      return claimsPrincipal;
    }

    public static IClaimsIdentity SelectPrimaryIdentity(
      ClaimsIdentityCollection identities)
    {
      if (identities == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identities));
      if (identities.Count == 0)
        return (IClaimsIdentity) null;
      IClaimsIdentity claimsIdentity = (IClaimsIdentity) null;
      foreach (IClaimsIdentity identity in identities)
      {
        if (identity is WindowsIdentity)
        {
          claimsIdentity = identity;
          break;
        }
        if (!(identity is RsaClaimsIdentity) && claimsIdentity == null)
          claimsIdentity = identity;
      }
      if (claimsIdentity == null)
        claimsIdentity = identities[0];
      return claimsIdentity;
    }

    public ClaimsPrincipal()
    {
    }

    public ClaimsPrincipal(IPrincipal principal)
      : this()
    {
      if (principal == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (principal));
      if (principal.Identity == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (principal));
      if (!(principal is IClaimsPrincipal claimsPrincipal))
        this._identities.Add(ClaimsIdentity.CreateFromIdentity(principal.Identity));
      else if (claimsPrincipal.Identities != null)
        this._identities.AddRange((IEnumerable<IClaimsIdentity>) claimsPrincipal.Identities);
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (principal), Microsoft.IdentityModel.SR.GetString("ID0003", (object) "principal.identities"));
    }

    public ClaimsPrincipal(IEnumerable<IClaimsIdentity> identities)
    {
      if (identities == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identities));
      this._identities.AddRange(identities);
    }

    public ClaimsPrincipal(ClaimsIdentityCollection identityCollection)
    {
      if (identityCollection == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identityCollection));
      this._identities.AddRange((IEnumerable<IClaimsIdentity>) identityCollection);
    }

    public ClaimsIdentityCollection Identities => this._identities;

    public IClaimsPrincipal Copy() => (IClaimsPrincipal) new ClaimsPrincipal(this.Identities.Copy());

    public virtual IIdentity Identity => (IIdentity) ClaimsPrincipal.SelectPrimaryIdentity(this._identities);

    public bool IsInRole(string role)
    {
      foreach (IClaimsIdentity identity in this._identities)
      {
        if (identity.Claims != null && identity.RoleClaimType != null)
        {
          foreach (Claim claim in identity.Claims)
          {
            if (StringComparer.Ordinal.Equals(claim.ClaimType, identity.RoleClaimType) && StringComparer.Ordinal.Equals(claim.Value, role))
              return true;
          }
        }
      }
      return false;
    }
  }
}
