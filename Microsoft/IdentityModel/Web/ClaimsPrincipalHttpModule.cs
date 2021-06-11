// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.ClaimsPrincipalHttpModule
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Diagnostics;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public class ClaimsPrincipalHttpModule : IHttpModule
  {
    private bool _clientCertificateAuthenticationEnabled;
    private ClaimsAuthenticationManager _authenticationManager;

    public bool ClientCertificateAuthenticationEnabled
    {
      get => this._clientCertificateAuthenticationEnabled;
      set => this._clientCertificateAuthenticationEnabled = value;
    }

    public ClaimsAuthenticationManager AuthenticationManager
    {
      get => this._authenticationManager;
      set => this._authenticationManager = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    protected virtual void OnPostAuthenticateRequest(object sender, EventArgs e)
    {
      if (HttpContext.Current.User is IClaimsPrincipal)
        return;
      IClaimsPrincipal claimsPrincipal = ClaimsPrincipal.CreateFromHttpContext(HttpContext.Current, this._clientCertificateAuthenticationEnabled);
      ClaimsAuthenticationManager authenticationManager = this._authenticationManager;
      if (authenticationManager != null && claimsPrincipal != null && claimsPrincipal.Identity != null)
        claimsPrincipal = authenticationManager.Authenticate(HttpContext.Current.Request.Url.AbsoluteUri, claimsPrincipal);
      HttpContext.Current.User = (IPrincipal) claimsPrincipal;
      Thread.CurrentPrincipal = (IPrincipal) claimsPrincipal;
      if (!DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
        return;
      DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, nameof (OnPostAuthenticateRequest), (TraceRecord) new ClaimsPrincipalTraceRecord(claimsPrincipal), (Exception) null);
    }

    public void Dispose()
    {
      if (!(this._authenticationManager is IDisposable authenticationManager))
        return;
      authenticationManager.Dispose();
    }

    public void Init(HttpApplication context)
    {
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      this._authenticationManager = FederatedAuthentication.ServiceConfiguration.ClaimsAuthenticationManager;
      context.PostAuthenticateRequest += new EventHandler(this.OnPostAuthenticateRequest);
    }
  }
}
