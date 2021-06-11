// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.WSFederationAuthenticationModule
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Diagnostics;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Web.Configuration;
using Microsoft.IdentityModel.Web.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public class WSFederationAuthenticationModule : HttpModuleBase
  {
    private string _authenticationType;
    private string _freshness;
    private string _homeRealm;
    private string _issuer;
    private string _policy;
    private string _realm;
    private string _reply;
    private string _signOutReply = string.Empty;
    private string _request;
    private string _requestPtr;
    private string _resource;
    private string _signInContext;
    private string _signInQueryString = string.Empty;
    private string _signOutQueryString = string.Empty;
    private bool _passiveRedirectEnabled;
    private bool _persistentCookiesOnPassiveRedirects;
    private bool _requireHttps;
    private XmlDictionaryReaderQuotas _xmlDictionaryReaderQuotas;
    private static readonly string _sessionTokenContextPrefix = "(" + typeof (WSFederationAuthenticationModule).Name + ")";
    private static readonly byte[] _signOutImage = new byte[143]
    {
      (byte) 71,
      (byte) 73,
      (byte) 70,
      (byte) 56,
      (byte) 57,
      (byte) 97,
      (byte) 17,
      (byte) 0,
      (byte) 13,
      (byte) 0,
      (byte) 162,
      (byte) 0,
      (byte) 0,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      (byte) 169,
      (byte) 240,
      (byte) 169,
      (byte) 125,
      (byte) 232,
      (byte) 125,
      (byte) 82,
      (byte) 224,
      (byte) 82,
      (byte) 38,
      (byte) 216,
      (byte) 38,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 33,
      (byte) 249,
      (byte) 4,
      (byte) 5,
      (byte) 0,
      (byte) 0,
      (byte) 5,
      (byte) 0,
      (byte) 44,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 17,
      (byte) 0,
      (byte) 13,
      (byte) 0,
      (byte) 0,
      (byte) 8,
      (byte) 84,
      (byte) 0,
      (byte) 11,
      (byte) 8,
      (byte) 28,
      (byte) 72,
      (byte) 112,
      (byte) 32,
      (byte) 128,
      (byte) 131,
      (byte) 5,
      (byte) 19,
      (byte) 22,
      (byte) 56,
      (byte) 24,
      (byte) 128,
      (byte) 64,
      (byte) 0,
      (byte) 0,
      (byte) 10,
      (byte) 13,
      (byte) 54,
      (byte) 116,
      (byte) 8,
      (byte) 49,
      (byte) 226,
      (byte) 193,
      (byte) 1,
      (byte) 4,
      (byte) 6,
      (byte) 32,
      (byte) 36,
      (byte) 88,
      (byte) 113,
      (byte) 97,
      (byte) 0,
      (byte) 140,
      (byte) 26,
      (byte) 11,
      (byte) 30,
      (byte) 68,
      (byte) 8,
      (byte) 64,
      (byte) 0,
      (byte) 129,
      (byte) 140,
      (byte) 29,
      (byte) 5,
      (byte) 2,
      (byte) 56,
      (byte) 73,
      (byte) 209,
      (byte) 36,
      (byte) 202,
      (byte) 132,
      (byte) 37,
      (byte) 79,
      (byte) 14,
      (byte) 112,
      (byte) 73,
      (byte) 81,
      (byte) 97,
      (byte) 76,
      (byte) 150,
      (byte) 53,
      (byte) 109,
      (byte) 210,
      (byte) 36,
      (byte) 32,
      (byte) 32,
      (byte) 37,
      (byte) 76,
      (byte) 151,
      (byte) 33,
      (byte) 35,
      (byte) 26,
      (byte) 20,
      (byte) 16,
      (byte) 84,
      (byte) 168,
      (byte) 65,
      (byte) 159,
      (byte) 9,
      (byte) 3,
      (byte) 2,
      (byte) 0,
      (byte) 59
    };

    internal static string SessionTokenContextPrefix => WSFederationAuthenticationModule._sessionTokenContextPrefix;

    public event EventHandler<SecurityTokenReceivedEventArgs> SecurityTokenReceived;

    public event EventHandler<SecurityTokenValidatedEventArgs> SecurityTokenValidated;

    public event EventHandler<SessionSecurityTokenCreatedEventArgs> SessionSecurityTokenCreated;

    public event EventHandler SignedIn;

    public event EventHandler SignedOut;

    public event EventHandler<ErrorEventArgs> SignInError;

    public event EventHandler<SigningOutEventArgs> SigningOut;

    public event EventHandler<ErrorEventArgs> SignOutError;

    public event EventHandler<RedirectingToIdentityProviderEventArgs> RedirectingToIdentityProvider;

    public event EventHandler<AuthorizationFailedEventArgs> AuthorizationFailed;

    public string AuthenticationType
    {
      get => this._authenticationType;
      set => this._authenticationType = value;
    }

    public string Freshness
    {
      get => this._freshness;
      set => this._freshness = value;
    }

    public string HomeRealm
    {
      get => this._homeRealm;
      set => this._homeRealm = value;
    }

    public string Issuer
    {
      get => this._issuer;
      set
      {
        if (string.IsNullOrEmpty(value))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (value)));
        this._issuer = UriUtil.CanCreateValidUri(value, UriKind.Absolute) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0014", (object) value), nameof (value)));
      }
    }

    public string Realm
    {
      get => this._realm;
      set
      {
        if (string.IsNullOrEmpty(value))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (value)));
        this._realm = UriUtil.CanCreateValidUri(value, UriKind.Absolute) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0014", (object) value), nameof (value)));
      }
    }

    public string Policy
    {
      get => this._policy;
      set => this._policy = value;
    }

    public string Reply
    {
      get => this._reply;
      set => this._reply = string.IsNullOrEmpty(value) || UriUtil.CanCreateValidUri(value, UriKind.Absolute) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0014", (object) value), nameof (value)));
    }

    public string SignOutReply
    {
      get => this._signOutReply;
      set => this._signOutReply = value;
    }

    public string Request
    {
      get => this._request;
      set => this._request = value;
    }

    public string RequestPtr
    {
      get => this._requestPtr;
      set => this._requestPtr = string.IsNullOrEmpty(value) || UriUtil.CanCreateValidUri(value, UriKind.Absolute) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0014", (object) value), nameof (value)));
    }

    public string Resource
    {
      get => this._resource;
      set => this._resource = value;
    }

    public bool PassiveRedirectEnabled
    {
      get => this._passiveRedirectEnabled;
      set => this._passiveRedirectEnabled = value;
    }

    public bool PersistentCookiesOnPassiveRedirects
    {
      get => this._persistentCookiesOnPassiveRedirects;
      set => this._persistentCookiesOnPassiveRedirects = value;
    }

    public bool RequireHttps
    {
      get => this._requireHttps;
      set => this._requireHttps = value;
    }

    public string SignInContext
    {
      get => this._signInContext;
      set => this._signInContext = value;
    }

    public string SignInQueryString
    {
      get => this._signInQueryString;
      set => this._signInQueryString = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (SignInQueryString));
    }

    public string SignOutQueryString
    {
      get => this._signOutQueryString;
      set => this._signOutQueryString = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (SignOutQueryString));
    }

    public XmlDictionaryReaderQuotas XmlDictionaryReaderQuotas
    {
      get => this._xmlDictionaryReaderQuotas;
      set => this._xmlDictionaryReaderQuotas = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public WSFederationAuthenticationModule() => this._xmlDictionaryReaderQuotas = new XmlDictionaryReaderQuotas()
    {
      MaxArrayLength = 2097152,
      MaxStringContentLength = 2097152
    };

    public bool CanReadSignInResponse(HttpRequest request) => this.CanReadSignInResponse(request, false);

    public virtual bool CanReadSignInResponse(HttpRequest request, bool onPage)
    {
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (request));
      SessionAuthenticationModule authenticationModule = FederatedAuthentication.SessionAuthenticationModule;
      if (string.Equals(request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
      {
        if ((onPage || !authenticationModule.ContainsSessionTokenCookie(request.Cookies)) && this.IsSignInResponse(request))
          return true;
      }
      else
      {
        SignOutCleanupRequestMessage outCleanupMessage = WSFederationAuthenticationModule.GetSignOutCleanupMessage(request);
        if (outCleanupMessage != null)
        {
          if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
            DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, nameof (CanReadSignInResponse), (TraceRecord) new WSFedMessageTraceRecord((WSFederationMessage) outCleanupMessage), (Exception) null);
          this.SignOut(true);
          HttpResponse response = HttpContext.Current.Response;
          if (!string.IsNullOrEmpty(outCleanupMessage.Reply))
          {
            string signOutRedirectUrl = this.GetSignOutRedirectUrl(outCleanupMessage);
            if (onPage)
            {
              response.Redirect(signOutRedirectUrl);
            }
            else
            {
              response.Redirect(signOutRedirectUrl, false);
              HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
          }
          else
          {
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ClearContent();
            response.ContentType = "image/gif";
            response.BinaryWrite(WSFederationAuthenticationModule._signOutImage);
            if (onPage)
              response.End();
            else
              HttpContext.Current.ApplicationInstance.CompleteRequest();
          }
        }
      }
      return false;
    }

    protected virtual string GetSignOutRedirectUrl(SignOutCleanupRequestMessage signOutMessage)
    {
      if (signOutMessage == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (signOutMessage));
      if (string.IsNullOrEmpty(signOutMessage.Reply))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("signOutMessage.Reply", Microsoft.IdentityModel.SR.GetString("ID0022"));
      if (WSFederationAuthenticationModule.IsSignOutReplySafe(new Uri(signOutMessage.Reply, UriKind.RelativeOrAbsolute), new Uri(this.Issuer, UriKind.RelativeOrAbsolute)))
      {
        if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
          DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, Microsoft.IdentityModel.SR.GetString("TraceResponseRedirect", (object) signOutMessage.Reply));
        return signOutMessage.Reply;
      }
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
        DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, Microsoft.IdentityModel.SR.GetString("TraceResponseRedirectNotTrusted", (object) signOutMessage.Reply, (object) this.Issuer));
      return this.Issuer;
    }

    private static bool IsSignOutReplySafe(Uri replyUri, Uri issuerUri)
    {
      bool flag = false;
      if (replyUri.IsAbsoluteUri && replyUri.Scheme == issuerUri.Scheme && replyUri.Port == issuerUri.Port)
      {
        string dnsSafeHost1 = replyUri.DnsSafeHost;
        string dnsSafeHost2 = issuerUri.DnsSafeHost;
        if (StringComparer.OrdinalIgnoreCase.Equals(dnsSafeHost1, dnsSafeHost2) || dnsSafeHost1.EndsWith("." + dnsSafeHost2, StringComparison.OrdinalIgnoreCase))
          flag = true;
      }
      return flag;
    }

    protected virtual string GetReturnUrlFromResponse(HttpRequest request)
    {
      if (!this.PassiveRedirectEnabled)
        return string.Empty;
      WSFederationMessage fromFormPost = WSFederationMessage.CreateFromFormPost(request);
      FederatedPassiveContext federatedPassiveContext = new FederatedPassiveContext(fromFormPost.Context);
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
      {
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, nameof (GetReturnUrlFromResponse), (TraceRecord) new WSFedMessageTraceRecord(fromFormPost), (Exception) null);
        DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, Microsoft.IdentityModel.SR.GetString("TraceGetReturnUrlFromResponse", (object) federatedPassiveContext.ReturnUrl));
      }
      return federatedPassiveContext.ReturnUrl;
    }

    public virtual SecurityToken GetSecurityToken(HttpRequest request) => request != null ? this.GetSecurityToken(this.GetSignInResponseMessage(request)) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentNullException(nameof (request)));

    public void VerifyProperties()
    {
      if (string.IsNullOrEmpty(this.Issuer))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID1047")));
      if (string.IsNullOrEmpty(this.Realm))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID1048")));
      if (!this.RequireHttps)
        return;
      if (!ControlUtil.IsHttps(this.Issuer))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID1056")));
      if (!string.IsNullOrEmpty(this.Reply) && !ControlUtil.IsHttps(this.Reply))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID1057")));
    }

    public virtual void RedirectToIdentityProvider(string uniqueId, string returnUrl, bool persist)
    {
      this.VerifyProperties();
      HttpContext current = HttpContext.Current;
      if (current == null || current.Response == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID1055")));
      RedirectingToIdentityProviderEventArgs e = new RedirectingToIdentityProviderEventArgs(this.CreateSignInRequest(uniqueId, returnUrl, persist));
      this.OnRedirectingToIdentityProvider(e);
      if (e.Cancel)
        return;
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
        DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, Microsoft.IdentityModel.SR.GetString("TraceRedirectArgsSignInRequestMessageRequestUrl", (object) e.SignInRequestMessage.RequestUrl));
      current.Response.Redirect(e.SignInRequestMessage.RequestUrl, false);
      current.ApplicationInstance.CompleteRequest();
    }

    protected override void InitializeModule(HttpApplication context)
    {
      if (FederatedAuthentication.SessionAuthenticationModule == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID1060"));
      context.AuthenticateRequest += new EventHandler(this.OnAuthenticateRequest);
      context.EndRequest += new EventHandler(this.OnEndRequest);
      context.PostAuthenticateRequest += new EventHandler(this.OnPostAuthenticateRequest);
      this.InitializePropertiesFromConfiguration(this.ServiceConfiguration.Name);
    }

    protected virtual void InitializePropertiesFromConfiguration(string serviceName) => this.InitializePropertiesFromConfiguration(MicrosoftIdentityModelSection.Current.ServiceElements.GetElement(serviceName));

    internal void InitializePropertiesFromConfiguration(ServiceElement element)
    {
      if (element == null)
        return;
      WSFederationAuthenticationElement wsFederation = element.FederatedAuthentication.WSFederation;
      wsFederation.Verify();
      this.Issuer = wsFederation.Issuer;
      this.Reply = wsFederation.Reply;
      this.RequireHttps = wsFederation.RequireHttps;
      this.Freshness = wsFederation.Freshness;
      this.AuthenticationType = wsFederation.AuthenticationType;
      this.HomeRealm = wsFederation.HomeRealm;
      this.Policy = wsFederation.Policy;
      this.Realm = wsFederation.Realm;
      this.Reply = wsFederation.Reply;
      this.SignOutReply = wsFederation.SignOutReply;
      this.Request = wsFederation.Request;
      this.RequestPtr = wsFederation.RequestPtr;
      this.Resource = wsFederation.Resource;
      this.SignInQueryString = wsFederation.SignInQueryString;
      this.SignOutQueryString = wsFederation.SignOutQueryString;
      this.PassiveRedirectEnabled = wsFederation.PassiveRedirectEnabled;
      this.PersistentCookiesOnPassiveRedirects = wsFederation.PersistentCookiesOnPassiveRedirects;
    }

    protected virtual void OnAuthenticateRequest(object sender, EventArgs args)
    {
      HttpRequest request = HttpContext.Current.Request;
      if (!this.CanReadSignInResponse(request))
        return;
      try
      {
        this.SignInWithResponseMessage(request);
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
          ErrorEventArgs args1 = new ErrorEventArgs(ex);
          this.OnSignInError(args1);
          if (args1.Cancel)
            return;
          throw;
        }
      }
    }

    private void SignInWithResponseMessage(HttpRequest request)
    {
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (string allKey in request.Form.AllKeys)
          dictionary.Add(allKey, request.Form[allKey]);
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, Microsoft.IdentityModel.SR.GetString("TraceSignInWithResponseMessage"), (TraceRecord) new PassiveMessageTraceRecord((IDictionary<string, string>) dictionary), (Exception) null);
      }
      HttpContext current1 = HttpContext.Current;
      SecurityTokenReceivedEventArgs e1 = new SecurityTokenReceivedEventArgs(this.GetSecurityToken(request));
      if (this.SecurityTokenReceived != null)
        this.SecurityTokenReceived((object) this, e1);
      if (e1.Cancel)
        return;
      TokenReceiver tokenReceiver = new TokenReceiver(this.ServiceConfiguration);
      IClaimsPrincipal claimsPrincipal = tokenReceiver.AuthenticateToken(e1.SecurityToken, true, HttpContext.Current.Request.RawUrl);
      if (claimsPrincipal == null)
        return;
      SecurityTokenValidatedEventArgs e2 = new SecurityTokenValidatedEventArgs(claimsPrincipal);
      if (this.SecurityTokenValidated != null)
        this.SecurityTokenValidated((object) this, e2);
      if (e2.Cancel)
        return;
      SessionAuthenticationModule current2 = SessionAuthenticationModule.Current;
      DateTime validFrom;
      DateTime validTo;
      tokenReceiver.ComputeSessionTokenLifeTime(e1.SecurityToken, out validFrom, out validTo);
      this.SetPrincipalAndWriteSessionToken(current2.CreateSessionSecurityToken(e2.ClaimsPrincipal, this.GetSessionTokenContext(), validFrom, validTo, this.PersistentCookiesOnPassiveRedirects), true);
      this.OnSignedIn(EventArgs.Empty);
      string returnUrlFromResponse = this.GetReturnUrlFromResponse(request);
      if (string.IsNullOrEmpty(returnUrlFromResponse))
        return;
      if (ControlUtil.IsAppRelative(returnUrlFromResponse))
      {
        current1.Response.Redirect(CookieHandler.MatchCookiePath(returnUrlFromResponse), false);
        current1.ApplicationInstance.CompleteRequest();
      }
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelper((Exception) new FederationException(Microsoft.IdentityModel.SR.GetString("ID3206", (object) returnUrlFromResponse)), TraceEventType.Error);
    }

    protected virtual void OnEndRequest(object sender, EventArgs args)
    {
      if (!this.PassiveRedirectEnabled || FederatedAuthentication.WSFederationAuthenticationModule == null || sender == null)
        return;
      HttpApplication httpApplication = (HttpApplication) sender;
      if (httpApplication.Response.StatusCode != 401)
        return;
      string absoluteUri = httpApplication.Request.Url.AbsoluteUri;
      string str = CookieHandler.MatchCookiePath(absoluteUri);
      if (!StringComparer.Ordinal.Equals(absoluteUri, str))
      {
        httpApplication.Response.Redirect(str, false);
        httpApplication.CompleteRequest();
      }
      else
      {
        AuthorizationFailedEventArgs e = new AuthorizationFailedEventArgs();
        this.OnAuthorizationFailed(e);
        if (!e.RedirectToIdentityProvider)
          return;
        if (FederatedAuthentication.SessionAuthenticationModule.CookieHandler.RequireSsl && !ControlUtil.IsHttps(httpApplication.Request.Url))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID1059"));
        if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
          DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, Microsoft.IdentityModel.SR.GetString("TraceOnEndRequestRedirect", (object) httpApplication.Request.RawUrl));
        this.RedirectToIdentityProvider("passive", httpApplication.Request.RawUrl, this.PersistentCookiesOnPassiveRedirects);
      }
    }

    protected virtual void OnPostAuthenticateRequest(object sender, EventArgs e)
    {
      if (HttpContext.Current.User is IClaimsPrincipal)
        return;
      IClaimsPrincipal incomingPrincipal = ClaimsPrincipal.CreateFromHttpContext(HttpContext.Current);
      ClaimsAuthenticationManager authenticationManager = this.ServiceConfiguration.ClaimsAuthenticationManager;
      if (authenticationManager != null && incomingPrincipal != null && incomingPrincipal.Identity != null)
        incomingPrincipal = authenticationManager.Authenticate(HttpContext.Current.Request.Url.AbsoluteUri, incomingPrincipal);
      HttpContext.Current.User = (IPrincipal) incomingPrincipal;
      Thread.CurrentPrincipal = (IPrincipal) incomingPrincipal;
    }

    protected virtual void OnSessionSecurityTokenCreated(SessionSecurityTokenCreatedEventArgs args)
    {
      if (this.SessionSecurityTokenCreated == null)
        return;
      this.SessionSecurityTokenCreated((object) this, args);
    }

    protected virtual void OnSignedIn(EventArgs args)
    {
      if (this.SignedIn == null)
        return;
      this.SignedIn((object) this, args);
    }

    protected virtual void OnSignedOut(EventArgs args)
    {
      if (this.SignedOut == null)
        return;
      this.SignedOut((object) this, args);
    }

    protected virtual void OnSignInError(ErrorEventArgs args)
    {
      if (this.SignInError == null)
        return;
      this.SignInError((object) this, args);
    }

    protected virtual void OnSigningOut(SigningOutEventArgs args)
    {
      if (this.SigningOut == null)
        return;
      this.SigningOut((object) this, args);
    }

    protected virtual void OnSignOutError(ErrorEventArgs args)
    {
      if (this.SignOutError == null)
        return;
      this.SignOutError((object) this, args);
    }

    protected virtual void OnRedirectingToIdentityProvider(RedirectingToIdentityProviderEventArgs e)
    {
      if (this.RedirectingToIdentityProvider == null)
        return;
      this.RedirectingToIdentityProvider((object) this, e);
    }

    protected virtual void OnAuthorizationFailed(AuthorizationFailedEventArgs e)
    {
      e.RedirectToIdentityProvider = !Thread.CurrentPrincipal.Identity.IsAuthenticated;
      if (this.AuthorizationFailed == null)
        return;
      this.AuthorizationFailed((object) this, e);
    }

    public void SetPrincipalAndWriteSessionToken(Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken, bool isSession)
    {
      if (sessionToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sessionToken));
      SessionAuthenticationModule authenticationModule = FederatedAuthentication.SessionAuthenticationModule;
      SessionSecurityTokenCreatedEventArgs args = new SessionSecurityTokenCreatedEventArgs(sessionToken);
      args.WriteSessionCookie = isSession;
      this.OnSessionSecurityTokenCreated(args);
      authenticationModule.AuthenticateSessionSecurityToken(args.SessionToken, args.WriteSessionCookie);
    }

    public virtual void SignOut(bool isIPRequest)
    {
      try
      {
        SessionAuthenticationModule authenticationModule = FederatedAuthentication.SessionAuthenticationModule;
        if (authenticationModule == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID1060")));
        this.OnSigningOut(new SigningOutEventArgs(isIPRequest));
        authenticationModule.DeleteSessionTokenCookie();
        this.OnSignedOut(EventArgs.Empty);
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
            DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID8022", (object) ex));
          ErrorEventArgs args = new ErrorEventArgs(ex);
          this.OnSignOutError(args);
          if (args.Cancel)
            return;
          throw;
        }
      }
    }

    public SignInRequestMessage CreateSignInRequest(
      string uniqueId,
      string returnUrl,
      bool rememberMeSet)
    {
      if (string.IsNullOrEmpty(this.Issuer))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID5002")));
      if (string.IsNullOrEmpty(this.Realm))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID5001")));
      if (!string.IsNullOrEmpty(this.Freshness))
      {
        double result = -1.0;
        if (!double.TryParse(this.Freshness, out result) || result < 0.0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID5020")));
      }
      SignInRequestMessage inRequestMessage = new SignInRequestMessage(new Uri(this.Issuer), this.Realm);
      if (!string.IsNullOrEmpty(this.Freshness))
        inRequestMessage.Freshness = this.Freshness;
      FederatedPassiveContext federatedPassiveContext = new FederatedPassiveContext(uniqueId, this.SignInContext, returnUrl, rememberMeSet);
      inRequestMessage.Context = federatedPassiveContext.WCtx;
      inRequestMessage.CurrentTime = DateTime.UtcNow.ToString("s", (IFormatProvider) CultureInfo.InvariantCulture) + "Z";
      if (!string.IsNullOrEmpty(this.AuthenticationType))
        inRequestMessage.AuthenticationType = this.AuthenticationType;
      if (!string.IsNullOrEmpty(this.HomeRealm))
        inRequestMessage.HomeRealm = this.HomeRealm;
      if (!string.IsNullOrEmpty(this.Policy))
        inRequestMessage.Policy = this.Policy;
      if (!string.IsNullOrEmpty(this.Reply))
        inRequestMessage.Reply = this.Reply;
      if (!string.IsNullOrEmpty(this.Resource))
        inRequestMessage.Resource = this.Resource;
      if (!string.IsNullOrEmpty(this.Request))
        inRequestMessage.Request = this.Request;
      if (!string.IsNullOrEmpty(this.RequestPtr))
        inRequestMessage.RequestPtr = this.RequestPtr;
      NameValueCollection queryString = HttpUtility.ParseQueryString(this.SignInQueryString);
      foreach (string key in queryString.Keys)
      {
        if (!inRequestMessage.Parameters.ContainsKey(key))
          inRequestMessage.Parameters.Add(key, queryString[key]);
      }
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, nameof (CreateSignInRequest), (TraceRecord) new WSFedMessageTraceRecord((WSFederationMessage) inRequestMessage), (Exception) null);
      return inRequestMessage;
    }

    protected virtual string GetReferencedResult(string resultPtr) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3210", (object) "wresultptr")));

    private string GetResultXml(SignInResponseMessage message) => !string.IsNullOrEmpty(message.Result) ? message.Result : this.GetReferencedResult(message.ResultPtr);

    public virtual SecurityToken GetSecurityToken(SignInResponseMessage message)
    {
      if (message == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentNullException(nameof (message)));
      return new TokenReceiver(this.ServiceConfiguration).ReadToken(this.GetXmlTokenFromMessage(message, (WSFederationSerializer) null), this.XmlDictionaryReaderQuotas);
    }

    public virtual SignInResponseMessage GetSignInResponseMessage(
      HttpRequest request)
    {
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentNullException(nameof (request)));
      if (!(WSFederationMessage.CreateFromFormPost(request) is SignInResponseMessage fromFormPost))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID1052"));
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, nameof (GetSignInResponseMessage), (TraceRecord) new WSFedMessageTraceRecord((WSFederationMessage) fromFormPost), (Exception) null);
      return fromFormPost;
    }

    private WSFederationSerializer CreateSerializerForResultXml(
      string resultXml)
    {
      if (string.IsNullOrEmpty(resultXml))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (resultXml));
      using (XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(resultXml), this._xmlDictionaryReaderQuotas))
        return new WSFederationSerializer(textReader);
    }

    public virtual string GetXmlTokenFromMessage(SignInResponseMessage message)
    {
      string resultXml = message != null ? this.GetResultXml(message) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (message));
      return !string.IsNullOrEmpty(resultXml) ? this.GetXmlTokenFromMessage(message, this.CreateSerializerForResultXml(resultXml)) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3001"));
    }

    public virtual string GetXmlTokenFromMessage(
      SignInResponseMessage message,
      WSFederationSerializer federationSerializer)
    {
      if (federationSerializer == null)
        return this.GetXmlTokenFromMessage(message);
      if (message == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (message));
      WSTrustSerializationContext context = new WSTrustSerializationContext(this.ServiceConfiguration.SecurityTokenHandlerCollectionManager);
      return federationSerializer.CreateResponse((WSFederationMessage) message, context).RequestedSecurityToken.SecurityTokenXml.OuterXml;
    }

    public virtual bool IsSignInResponse(HttpRequest request)
    {
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (request));
      if (request.Form == null || !StringComparer.Ordinal.Equals(request.Form["wa"], "wsignin1.0"))
        return false;
      return !string.IsNullOrEmpty(request.Form["wresult"]) || !string.IsNullOrEmpty(request.Form["wresultptr"]);
    }

    internal static SignOutCleanupRequestMessage GetSignOutCleanupMessage(
      HttpRequest request)
    {
      SignOutCleanupRequestMessage cleanupRequestMessage = (SignOutCleanupRequestMessage) null;
      WSFederationMessage fedMsg;
      if (WSFederationMessage.TryCreateFromUri(request.Url, out fedMsg))
        cleanupRequestMessage = fedMsg as SignOutCleanupRequestMessage;
      return cleanupRequestMessage;
    }

    protected virtual string GetSessionTokenContext()
    {
      string issuer = this.Issuer;
      if (string.IsNullOrEmpty(issuer))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID1058")));
      return WSFederationAuthenticationModule.SessionTokenContextPrefix + WSFederationAuthenticationModule.GetFederationPassiveSignOutUrl(issuer, this.SignOutReply, this.SignOutQueryString);
    }

    public static void FederatedSignOut(Uri signOutUrl, Uri replyUrl)
    {
      Uri passiveSignOutUrl = WSFederationAuthenticationModule.GetFederationPassiveSignOutUrl(signOutUrl, replyUrl);
      if (passiveSignOutUrl == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3272"));
      FederatedAuthentication.SessionAuthenticationModule?.DeleteSessionTokenCookie();
      HttpContext.Current.Response.Redirect(passiveSignOutUrl.AbsoluteUri);
    }

    internal static Uri GetFederationPassiveSignOutUrl(Uri signOutUrl, Uri replyUrl)
    {
      if (signOutUrl != (Uri) null && !signOutUrl.IsAbsoluteUri)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (signOutUrl), Microsoft.IdentityModel.SR.GetString("ID0014", (object) signOutUrl.ToString()));
      Uri baseUri = (Uri) null;
      if (signOutUrl == (Uri) null)
      {
        SessionAuthenticationModule authenticationModule = FederatedAuthentication.SessionAuthenticationModule;
        if (authenticationModule != null)
          baseUri = authenticationModule.GetSignOutUrlFromSessionToken();
      }
      else
        baseUri = new Uri(signOutUrl, string.Empty);
      if (replyUrl != (Uri) null)
      {
        if (!replyUrl.IsAbsoluteUri)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (replyUrl), Microsoft.IdentityModel.SR.GetString("ID0014", (object) replyUrl.ToString()));
        NameValueCollection queryString = HttpUtility.ParseQueryString(baseUri.Query);
        queryString["wreply"] = replyUrl.AbsoluteUri;
        SignOutRequestMessage outRequestMessage = new SignOutRequestMessage(new Uri(baseUri, new Uri(baseUri.AbsolutePath, UriKind.Relative)));
        foreach (string key in queryString.Keys)
        {
          if (!outRequestMessage.Parameters.ContainsKey(key))
            outRequestMessage.Parameters.Add(key, queryString[key]);
        }
        baseUri = new Uri(ControlUtil.GetPathAndQuery((WSFederationMessage) outRequestMessage));
      }
      return baseUri;
    }

    public static string GetFederationPassiveSignOutUrl(
      string issuer,
      string signOutReply,
      string signOutQueryString)
    {
      SignOutRequestMessage outRequestMessage = !string.IsNullOrEmpty(issuer) ? new SignOutRequestMessage(new Uri(issuer)) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (issuer));
      if (!string.IsNullOrEmpty(signOutReply))
        outRequestMessage.Reply = signOutReply;
      if (!string.IsNullOrEmpty(signOutQueryString))
      {
        NameValueCollection queryString = HttpUtility.ParseQueryString(signOutQueryString);
        foreach (string key in queryString.Keys)
          outRequestMessage.Parameters.Add(key, queryString[key]);
      }
      return ControlUtil.GetPathAndQuery((WSFederationMessage) outRequestMessage);
    }
  }
}
