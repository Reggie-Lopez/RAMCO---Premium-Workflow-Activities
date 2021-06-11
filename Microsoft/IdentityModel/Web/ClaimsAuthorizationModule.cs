// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.ClaimsAuthorizationModule
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Diagnostics;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public class ClaimsAuthorizationModule : IHttpModule
  {
    private ClaimsAuthorizationManager _authorizationManager;

    protected virtual bool Authorize()
    {
      bool flag = true;
      HttpRequest request = HttpContext.Current.Request;
      if (!(Thread.CurrentPrincipal is IClaimsPrincipal currentPrincipal))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelper((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID1069")), TraceEventType.Error);
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, Microsoft.IdentityModel.SR.GetString("TraceAuthorize"), (TraceRecord) new AuthorizeTraceRecord(currentPrincipal, request), (Exception) null);
      if (this.ClaimsAuthorizationManager != null)
        flag = this.ClaimsAuthorizationManager.CheckAccess(new AuthorizationContext(currentPrincipal, request.Url.AbsoluteUri, request.HttpMethod));
      return flag;
    }

    public ClaimsAuthorizationManager ClaimsAuthorizationManager
    {
      get => this._authorizationManager;
      set => this._authorizationManager = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    protected virtual void OnAuthorizeRequest(object sender, EventArgs args)
    {
      if (!this.Authorize())
      {
        if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
          DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, Microsoft.IdentityModel.SR.GetString("TraceOnAuthorizeRequestFailed"));
        HttpContext.Current.Response.StatusCode = 401;
        HttpContext.Current.ApplicationInstance.CompleteRequest();
      }
      else
      {
        if (!DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
          return;
        DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, Microsoft.IdentityModel.SR.GetString("TraceOnAuthorizeRequestSucceed"));
      }
    }

    public void Dispose()
    {
      if (!(this._authorizationManager is IDisposable authorizationManager))
        return;
      authorizationManager.Dispose();
    }

    public void Init(HttpApplication context)
    {
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      this._authorizationManager = FederatedAuthentication.ServiceConfiguration.ClaimsAuthorizationManager;
      context.AuthorizeRequest += new EventHandler(this.OnAuthorizeRequest);
    }
  }
}
