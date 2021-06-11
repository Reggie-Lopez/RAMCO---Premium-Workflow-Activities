// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.AuthorizationContext
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
  [ComVisible(true)]
  public class AuthorizationContext
  {
    private Collection<Claim> _action = new Collection<Claim>();
    private Collection<Claim> _resource = new Collection<Claim>();
    private IClaimsPrincipal _principal;

    public AuthorizationContext(IClaimsPrincipal principal, string resource, string action)
    {
      if (principal == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (principal));
      if (string.IsNullOrEmpty(resource))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (resource));
      this._principal = principal;
      this._resource.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", resource));
      if (action == null)
        return;
      this._action.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", action));
    }

    public AuthorizationContext(
      IClaimsPrincipal principal,
      Collection<Claim> resource,
      Collection<Claim> action)
    {
      if (principal == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (principal));
      if (resource == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (resource));
      if (action == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (action));
      this._principal = principal;
      this._resource = resource;
      this._action = action;
    }

    public Collection<Claim> Action => this._action;

    public Collection<Claim> Resource => this._resource;

    public IClaimsPrincipal Principal => this._principal;
  }
}
