// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.FederatedPassiveSecurityTokenServiceOperations
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Diagnostics;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Protocols.WSTrust;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public static class FederatedPassiveSecurityTokenServiceOperations
  {
    public static void ProcessRequest(
      HttpRequest request,
      IPrincipal principal,
      Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService sts,
      HttpResponse response)
    {
      FederatedPassiveSecurityTokenServiceOperations.ProcessRequest(request, principal, sts, response, (WSFederationSerializer) null);
    }

    public static void ProcessRequest(
      HttpRequest request,
      IPrincipal principal,
      Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService sts,
      HttpResponse response,
      WSFederationSerializer federationSerializer)
    {
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (request));
      if (principal == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (principal));
      if (sts == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sts));
      if (response == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (response));
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (string allKey in request.QueryString.AllKeys)
          dictionary.Add(allKey, request.QueryString[allKey]);
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, Microsoft.IdentityModel.SR.GetString("TracePassiveOperationProcessRequest"), (TraceRecord) new PassiveMessageTraceRecord((IDictionary<string, string>) dictionary), (Exception) null);
      }
      string str = request.QueryString["wa"];
      Uri uri = (Uri) null;
      if (string.IsNullOrEmpty(str))
        return;
      try
      {
        if (str == "wsignin1.0")
        {
          Uri requestUri = uri;
          if ((object) requestUri == null)
            requestUri = request.Url;
          SignInRequestMessage fromUri = (SignInRequestMessage) WSFederationMessage.CreateFromUri(requestUri);
          if (!FederatedPassiveSecurityTokenServiceOperations.IsAuthenticatedUser(principal))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new UnauthorizedAccessException());
          FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(fromUri, principal, sts, federationSerializer), response);
        }
        else if (str == "wsignout1.0" || str == "wsignoutcleanup1.0")
        {
          WSFederationMessage fromUri = WSFederationMessage.CreateFromUri(request.Url);
          string reply = (string) null;
          if (str == "wsignout1.0")
            reply = ((SignOutRequestMessage) fromUri).Reply;
          FederatedPassiveSecurityTokenServiceOperations.ProcessSignOutRequest(fromUri, principal, reply, response);
        }
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3000", (object) str)));
      }
      catch (Exception ex)
      {
        if (DiagnosticUtil.IsFatal(ex))
        {
          throw;
        }
        else
        {
          if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
            DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID8020", (object) ex));
          throw;
        }
      }
    }

    public static SignInResponseMessage ProcessSignInRequest(
      SignInRequestMessage requestMessage,
      IPrincipal principal,
      Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService sts)
    {
      return FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, principal, sts, (WSFederationSerializer) null);
    }

    public static SignInResponseMessage ProcessSignInRequest(
      SignInRequestMessage requestMessage,
      IPrincipal principal,
      Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService sts,
      WSFederationSerializer federationSerializer)
    {
      if (requestMessage == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestMessage));
      if (principal == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (principal));
      if (sts == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sts));
      if (string.IsNullOrEmpty(requestMessage.Realm))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID5023")));
      WSTrustSerializationContext context = new WSTrustSerializationContext(sts.SecurityTokenServiceConfiguration.SecurityTokenHandlerCollectionManager, sts.SecurityTokenServiceConfiguration.CreateAggregateTokenResolver(), sts.SecurityTokenServiceConfiguration.IssuerTokenResolver);
      if (federationSerializer == null)
        federationSerializer = new WSFederationSerializer((WSTrustRequestSerializer) sts.SecurityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) sts.SecurityTokenServiceConfiguration.WSTrust13ResponseSerializer);
      RequestSecurityToken request = federationSerializer.CreateRequest((WSFederationMessage) requestMessage, context);
      RequestSecurityTokenResponse response = sts.Issue(ClaimsPrincipal.CreateFromPrincipal(principal), request);
      Uri result = (Uri) null;
      if (!UriUtil.TryCreateValidUri(response.ReplyTo, UriKind.Absolute, out result))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID5024")));
      return new SignInResponseMessage(result, response, federationSerializer, context);
    }

    public static void ProcessSignInResponse(
      SignInResponseMessage signInResponseMessage,
      HttpResponse httpResponse)
    {
      if (signInResponseMessage == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (signInResponseMessage));
      if (httpResponse == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (httpResponse));
      signInResponseMessage.Write(httpResponse.Output);
      httpResponse.Flush();
      httpResponse.End();
    }

    public static void ProcessSignOutRequest(
      WSFederationMessage requestMessage,
      IPrincipal principal,
      string reply,
      HttpResponse httpResponse)
    {
      if (requestMessage == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestMessage));
      if (principal == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (principal));
      if (httpResponse == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (httpResponse));
      switch (requestMessage)
      {
        case SignOutRequestMessage _:
        case SignOutCleanupRequestMessage _:
          if (FederatedPassiveSecurityTokenServiceOperations.IsAuthenticatedUser(principal))
          {
            try
            {
              FormsAuthentication.SignOut();
            }
            finally
            {
              FederatedAuthentication.SessionAuthenticationModule?.DeleteSessionTokenCookie();
            }
          }
          if (reply == null)
            break;
          httpResponse.Redirect(reply);
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation("ProcessSignOutRequest can only be called with a SignOutRequestMessage or SignOutCleanupRequestMessage");
      }
    }

    private static bool IsAuthenticatedUser(IPrincipal principal) => principal != null && principal.Identity != null && principal.Identity.IsAuthenticated;
  }
}
