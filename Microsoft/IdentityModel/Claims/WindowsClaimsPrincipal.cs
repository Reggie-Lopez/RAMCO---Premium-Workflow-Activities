// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.WindowsClaimsPrincipal
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Microsoft.IdentityModel.Claims
{
  [ComVisible(true)]
  [Serializable]
  public class WindowsClaimsPrincipal : WindowsPrincipal, IClaimsPrincipal, IPrincipal, IDisposable
  {
    private ClaimsIdentityCollection _identities = new ClaimsIdentityCollection();
    private bool _disposed;

    public WindowsClaimsPrincipal(WindowsClaimsIdentity identity)
      : base((WindowsIdentity) identity)
    {
      if (identity == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identity));
      this._identities.Add(identity.Copy());
    }

    public WindowsClaimsPrincipal(WindowsIdentity identity)
      : base((WindowsIdentity) new WindowsClaimsIdentity(identity))
      => this._identities.Add((IClaimsIdentity) base.Identity);

    public WindowsClaimsPrincipal(WindowsIdentity identity, string issuerName)
      : base((WindowsIdentity) new WindowsClaimsIdentity(identity, WindowsClaimsPrincipal.GetValidAuthenticationType(identity), issuerName))
      => this._identities.Add((IClaimsIdentity) base.Identity);

    private static string GetValidAuthenticationType(WindowsIdentity identity) => identity != null ? identity.AuthenticationType : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identity));

    public static IClaimsPrincipal CreateFromWindowsIdentity(
      WindowsIdentity identity)
    {
      return WindowsClaimsPrincipal.CreateFromWindowsIdentity(identity, "LOCAL AUTHORITY");
    }

    public static IClaimsPrincipal CreateFromWindowsIdentity(
      WindowsIdentity identity,
      string issuerName)
    {
      if (identity == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identity));
      return identity.Token == IntPtr.Zero ? ClaimsPrincipal.AnonymousPrincipal : (IClaimsPrincipal) new WindowsClaimsPrincipal(identity, issuerName);
    }

    public ClaimsIdentityCollection Identities => this._identities;

    public IClaimsPrincipal Copy() => (IClaimsPrincipal) new WindowsClaimsPrincipal((WindowsClaimsIdentity) base.Identity)
    {
      _identities = this.Identities.Copy()
    };

    public override IIdentity Identity
    {
      get
      {
        if (this._identities.Count > 0)
        {
          foreach (IClaimsIdentity identity in this._identities)
          {
            if (identity is WindowsClaimsIdentity)
              return (IIdentity) identity;
          }
        }
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4056")));
      }
    }

    public override bool IsInRole(string role)
    {
      switch (role)
      {
        case "":
        case null:
          return false;
        default:
          NTAccount ntAccount = new NTAccount(role);
          try
          {
            SecurityIdentifier sid = ntAccount.Translate(typeof (SecurityIdentifier)) as SecurityIdentifier;
            if (sid != (SecurityIdentifier) null)
              return this.IsInRole(sid);
          }
          catch (IdentityNotMappedException ex)
          {
            if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
              DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID8002", (object) role, (object) ex.Message));
          }
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

    public override bool IsInRole(SecurityIdentifier sid)
    {
      if (sid == (SecurityIdentifier) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sid));
      foreach (IClaimsIdentity identity in this._identities)
      {
        if (identity.Claims != null && identity.RoleClaimType != null)
        {
          foreach (Claim claim in identity.Claims)
          {
            if ((StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid") || StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid") || StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid")) && StringComparer.Ordinal.Equals(claim.Value, sid.Value))
              return true;
          }
        }
      }
      return false;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (disposing)
      {
        foreach (IClaimsIdentity identity in this._identities)
        {
          if (identity is WindowsClaimsIdentity windowsClaimsIdentity)
            windowsClaimsIdentity.Dispose();
        }
        this._identities.Clear();
      }
      this._disposed = true;
    }
  }
}
