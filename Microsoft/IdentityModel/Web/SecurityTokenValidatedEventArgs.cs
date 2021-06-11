// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.SecurityTokenValidatedEventArgs
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public class SecurityTokenValidatedEventArgs : CancelEventArgs
  {
    private IClaimsPrincipal _claimsPrincipal;

    public SecurityTokenValidatedEventArgs(IClaimsPrincipal claimsPrincipal) => this._claimsPrincipal = claimsPrincipal != null ? claimsPrincipal : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claimsPrincipal));

    public IClaimsPrincipal ClaimsPrincipal
    {
      get => this._claimsPrincipal;
      [SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)] set => this._claimsPrincipal = value;
    }
  }
}
