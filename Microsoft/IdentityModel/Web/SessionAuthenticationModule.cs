// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.SessionAuthenticationModule
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web.Controls;
using System;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ServiceModel.Security;
using System.Threading;
using System.Web;
using System.Xml;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public class SessionAuthenticationModule : HttpModuleBase
  {
    private CookieHandler _cookieHandler = (CookieHandler) new ChunkedCookieHandler();
    private object _lockObject = new object();
    private bool _isSessionMode;

    public CookieHandler CookieHandler
    {
      get => this._cookieHandler;
      set => this._cookieHandler = value;
    }

    public virtual Microsoft.IdentityModel.Tokens.SessionSecurityToken ContextSessionSecurityToken
    {
      get => (Microsoft.IdentityModel.Tokens.SessionSecurityToken) HttpContext.Current.Items[(object) typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken).AssemblyQualifiedName];
      internal set => HttpContext.Current.Items[(object) typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken).AssemblyQualifiedName] = (object) value;
    }

    public bool IsSessionMode
    {
      get => this._isSessionMode;
      set => this._isSessionMode = value;
    }

    private SessionSecurityTokenCreatedEventArgs RaiseSessionCreatedEvent(
      Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken,
      bool reissueCookie)
    {
      SessionSecurityTokenCreatedEventArgs args = new SessionSecurityTokenCreatedEventArgs(sessionToken);
      args.WriteSessionCookie = reissueCookie;
      this.OnSessionSecurityTokenCreated(args);
      return args;
    }

    internal static SessionAuthenticationModule Current => FederatedAuthentication.SessionAuthenticationModule ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID1060"));

    public virtual void AuthenticateSessionSecurityToken(
      Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken,
      bool writeCookie)
    {
      this.SetPrincipalFromSessionToken(sessionToken);
      if (!writeCookie)
        return;
      this.WriteSessionTokenToCookie(sessionToken);
    }

    public bool ContainsSessionTokenCookie(HttpCookieCollection httpCookieCollection)
    {
      if (httpCookieCollection == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (httpCookieCollection));
      return httpCookieCollection[this.CookieHandler.Name] != null;
    }

    public void DeleteSessionTokenCookie()
    {
      this.CookieHandler.Delete();
      if (this.ContextSessionSecurityToken == null)
        return;
      this.RemoveSessionTokenFromCache(this.ContextSessionSecurityToken);
    }

    internal void RemoveSessionTokenFromCache(Microsoft.IdentityModel.Tokens.SessionSecurityToken token)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (!(this.ServiceConfiguration.SecurityTokenHandlers[typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken)] is Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler securityTokenHandler))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4010", (object) typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken))));
      Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken = token;
      SecurityTokenCacheKey securityTokenCacheKey = new SecurityTokenCacheKey(sessionSecurityToken.EndpointId, sessionSecurityToken.ContextId, sessionSecurityToken.KeyGeneration, this._isSessionMode);
      securityTokenHandler.TokenCache.TryRemoveEntry((object) securityTokenCacheKey);
    }

    internal Uri GetSignOutUrlFromSessionToken()
    {
      Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken = this.ContextSessionSecurityToken;
      if (sessionSecurityToken != null && !string.IsNullOrEmpty(sessionSecurityToken.Context))
      {
        string tokenContextPrefix = WSFederationAuthenticationModule.SessionTokenContextPrefix;
        if (sessionSecurityToken.Context.StartsWith(tokenContextPrefix, StringComparison.Ordinal))
          return new Uri(sessionSecurityToken.Context.Substring(tokenContextPrefix.Length));
      }
      return (Uri) null;
    }

    protected override void InitializeModule(HttpApplication context)
    {
      context.AuthenticateRequest += new EventHandler(this.OnAuthenticateRequest);
      context.PostAuthenticateRequest += new EventHandler(this.OnPostAuthenticateRequest);
      this.InitializePropertiesFromConfiguration(this.ServiceConfiguration.Name);
    }

    protected virtual void InitializePropertiesFromConfiguration(string serviceName)
    {
      ServiceElement element = MicrosoftIdentityModelSection.Current.ServiceElements.GetElement(serviceName);
      if (element == null || element.FederatedAuthentication == null || (element.FederatedAuthentication.CookieHandler == null || !element.FederatedAuthentication.CookieHandler.IsConfigured))
        return;
      this._cookieHandler = element.FederatedAuthentication.CookieHandler.GetConfiguredCookieHandler();
    }

    protected virtual void OnAuthenticateRequest(object sender, EventArgs eventArgs)
    {
      Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken1 = (Microsoft.IdentityModel.Tokens.SessionSecurityToken) null;
      if (!this.TryReadSessionTokenFromCookie(out sessionToken1))
      {
        HttpApplication httpApplication = (HttpApplication) sender;
        if (string.Equals(httpApplication.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
        {
          string absoluteUri = httpApplication.Request.Url.AbsoluteUri;
          string str = CookieHandler.MatchCookiePath(absoluteUri);
          if (!StringComparer.Ordinal.Equals(absoluteUri, str))
          {
            httpApplication.Response.Redirect(str, false);
            httpApplication.CompleteRequest();
          }
        }
      }
      if (sessionToken1 == null)
        return;
      Microsoft.IdentityModel.Tokens.SessionSecurityToken token = sessionToken1;
      SessionSecurityTokenReceivedEventArgs args = new SessionSecurityTokenReceivedEventArgs(sessionToken1);
      this.OnSessionSecurityTokenReceived(args);
      if (args.Cancel)
        return;
      Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken2 = args.SessionToken;
      bool writeCookie = args.ReissueCookie;
      if (writeCookie)
      {
        this.RemoveSessionTokenFromCache(token);
        SessionSecurityTokenCreatedEventArgs createdEventArgs = this.RaiseSessionCreatedEvent(sessionToken2, true);
        sessionToken2 = createdEventArgs.SessionToken;
        writeCookie = createdEventArgs.WriteSessionCookie;
      }
      try
      {
        this.AuthenticateSessionSecurityToken(sessionToken2, writeCookie);
      }
      catch (FederatedAuthenticationSessionEndingException ex)
      {
        if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
          DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, Microsoft.IdentityModel.SR.GetString("ID8021", (object) ex));
        this.SignOut();
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

    public virtual void SignOut()
    {
      try
      {
        this.OnSigningOut(SigningOutEventArgs.RPInitiated);
        this.DeleteSessionTokenCookie();
        this.OnSignedOut(EventArgs.Empty);
      }
      catch (Exception ex)
      {
        if (!DiagnosticUtil.IsFatal(ex))
        {
          if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
            DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID8022", (object) ex));
          ErrorEventArgs e = new ErrorEventArgs(ex);
          this.OnSignOutError(e);
          if (e.Cancel)
            return;
          throw;
        }
        else
          throw;
      }
    }

    protected virtual void OnSessionSecurityTokenCreated(SessionSecurityTokenCreatedEventArgs args)
    {
      if (this.SessionSecurityTokenCreated == null)
        return;
      this.SessionSecurityTokenCreated((object) this, args);
    }

    protected virtual void OnSessionSecurityTokenReceived(SessionSecurityTokenReceivedEventArgs args)
    {
      if (this.SessionSecurityTokenReceived == null)
        return;
      this.SessionSecurityTokenReceived((object) this, args);
    }

    protected virtual void OnSignedOut(EventArgs e)
    {
      if (this.SignedOut == null)
        return;
      this.SignedOut((object) this, e);
    }

    protected virtual void OnSigningOut(SigningOutEventArgs e)
    {
      if (this.SigningOut == null)
        return;
      this.SigningOut((object) this, e);
    }

    protected virtual void OnSignOutError(ErrorEventArgs e)
    {
      if (this.SignOutError == null)
        return;
      this.SignOutError((object) this, e);
    }

    public Microsoft.IdentityModel.Tokens.SessionSecurityToken CreateSessionSecurityToken(
      IClaimsPrincipal principal,
      string context,
      DateTime validFrom,
      DateTime validTo,
      bool isPersistent)
    {
      if (!(this.ServiceConfiguration.SecurityTokenHandlers[typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken)] is Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler securityTokenHandler))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4010", (object) typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken))));
      Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken = securityTokenHandler.CreateSessionSecurityToken(principal, context, this._cookieHandler.Path, validFrom, validTo);
      sessionSecurityToken.IsPersistent = isPersistent;
      sessionSecurityToken.IsSessionMode = this._isSessionMode;
      return sessionSecurityToken;
    }

    public Microsoft.IdentityModel.Tokens.SessionSecurityToken ReadSessionTokenFromCookie(
      byte[] sessionCookie)
    {
      if (!(this.ServiceConfiguration.SecurityTokenHandlers[typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken)] is Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler securityTokenHandler))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4010", (object) typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken))));
      SessionSecurityTokenResolver securityTokenResolver = new SessionSecurityTokenResolver(securityTokenHandler.TokenCache, this.CookieHandler.Path, this._isSessionMode);
      SecurityContextKeyIdentifierClause keyId = this.GetKeyId(sessionCookie);
      SecurityToken token = (SecurityToken) null;
      bool flag = false;
      if (keyId != null)
      {
        if (securityTokenResolver.TryResolveToken((SecurityKeyIdentifierClause) keyId, out token))
          return token as Microsoft.IdentityModel.Tokens.SessionSecurityToken;
        flag = true;
      }
      token = !flag ? securityTokenHandler.ReadToken(sessionCookie, (SecurityTokenResolver) securityTokenResolver) : securityTokenHandler.ReadToken(sessionCookie, Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance);
      if (keyId != null)
        securityTokenHandler.TokenCache.TryAddEntry((object) new SecurityTokenCacheKey(this.CookieHandler.Path, keyId.ContextId, keyId.Generation, this._isSessionMode), token);
      return token as Microsoft.IdentityModel.Tokens.SessionSecurityToken;
    }

    private SecurityContextKeyIdentifierClause GetKeyId(
      byte[] sessionCookie)
    {
      using (XmlReader textReader = (XmlReader) XmlDictionaryReader.CreateTextReader(sessionCookie, XmlDictionaryReaderQuotas.Max))
      {
        System.Xml.UniqueId generation = (System.Xml.UniqueId) null;
        SessionDictionary instance = SessionDictionary.Instance;
        XmlDictionaryReader dictionaryReader = XmlDictionaryReader.CreateDictionaryReader(textReader);
        int content = (int) dictionaryReader.MoveToContent();
        string ns;
        string localname;
        if (dictionaryReader.IsStartElement("SecurityContextToken", "http://schemas.xmlsoap.org/ws/2005/02/sc"))
        {
          ns = "http://schemas.xmlsoap.org/ws/2005/02/sc";
          localname = "Instance";
        }
        else
        {
          if (!dictionaryReader.IsStartElement("SecurityContextToken", "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512"))
            return (SecurityContextKeyIdentifierClause) null;
          ns = "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512";
          localname = "Instance";
        }
        dictionaryReader.ReadFullStartElement();
        System.Xml.UniqueId contextId = dictionaryReader.ReadElementContentAsUniqueId();
        if (contextId == (System.Xml.UniqueId) null || string.IsNullOrEmpty(contextId.ToString()))
          return (SecurityContextKeyIdentifierClause) null;
        if (dictionaryReader.IsStartElement(localname, ns))
          generation = dictionaryReader.ReadElementContentAsUniqueId();
        return !(generation == (System.Xml.UniqueId) null) ? new SecurityContextKeyIdentifierClause(contextId, generation) : new SecurityContextKeyIdentifierClause(contextId);
      }
    }

    protected virtual void SetPrincipalFromSessionToken(Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken)
    {
      IClaimsPrincipal fromIdentities = ClaimsPrincipal.CreateFromIdentities(this.ValidateSessionToken(sessionSecurityToken));
      HttpContext.Current.User = (IPrincipal) fromIdentities;
      Thread.CurrentPrincipal = (IPrincipal) fromIdentities;
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, Microsoft.IdentityModel.SR.GetString("TraceSetPrincipalFromSessionToken"), (TraceRecord) new ClaimsPrincipalTraceRecord(fromIdentities), (Exception) null);
      this.ContextSessionSecurityToken = sessionSecurityToken;
    }

    public bool TryReadSessionTokenFromCookie(out Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken)
    {
      byte[] sessionCookie = this.CookieHandler.Read();
      if (sessionCookie == null)
      {
        sessionToken = (Microsoft.IdentityModel.Tokens.SessionSecurityToken) null;
        return false;
      }
      sessionToken = this.ReadSessionTokenFromCookie(sessionCookie);
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Verbose, TraceCode.Diagnostics, Microsoft.IdentityModel.SR.GetString("TraceValidateToken"), (TraceRecord) new TokenTraceRecord((SecurityToken) sessionToken), (Exception) null);
      return true;
    }

    protected ClaimsIdentityCollection ValidateSessionToken(
      Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken)
    {
      if (!(this.ServiceConfiguration.SecurityTokenHandlers[typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken)] is Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler securityTokenHandler))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4011", (object) typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken)));
      try
      {
        return securityTokenHandler.ValidateToken(sessionSecurityToken, this._cookieHandler.Path);
      }
      catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new FederatedSessionExpiredException(DateTime.UtcNow, sessionSecurityToken.ValidTo, (Exception) ex));
      }
      catch (Microsoft.IdentityModel.Tokens.SecurityTokenNotYetValidException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new FederatedAuthenticationSessionEndingException(Microsoft.IdentityModel.SR.GetString("ID1071", (object) DateTime.UtcNow, (object) sessionSecurityToken.ValidFrom), (Exception) ex));
      }
    }

    public void WriteSessionTokenToCookie(Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken)
    {
      if (sessionToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sessionToken));
      if (!(this.ServiceConfiguration.SecurityTokenHandlers[typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken)] is Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler securityTokenHandler))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4011", (object) typeof (Microsoft.IdentityModel.Tokens.SessionSecurityToken)));
      byte[] numArray = securityTokenHandler.WriteToken(sessionToken);
      SecurityTokenCacheKey securityTokenCacheKey = new SecurityTokenCacheKey(this.CookieHandler.Path, sessionToken.ContextId, sessionToken.KeyGeneration, this._isSessionMode);
      securityTokenHandler.TokenCache.TryAddEntry((object) securityTokenCacheKey, (SecurityToken) sessionToken);
      this.CookieHandler.Write(numArray, sessionToken.IsPersistent, sessionToken.ValidTo);
    }

    public event EventHandler<SessionSecurityTokenCreatedEventArgs> SessionSecurityTokenCreated;

    public event EventHandler<SessionSecurityTokenReceivedEventArgs> SessionSecurityTokenReceived;

    public event EventHandler<SigningOutEventArgs> SigningOut;

    public event EventHandler SignedOut;

    public event EventHandler<ErrorEventArgs> SignOutError;
  }
}
