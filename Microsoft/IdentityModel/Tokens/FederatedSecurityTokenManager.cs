// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.FederatedSecurityTokenManager
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens.Saml11;
using Microsoft.IdentityModel.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class FederatedSecurityTokenManager : ServiceCredentialsSecurityTokenManager
  {
    private static string ListenUriProperty = "http://schemas.microsoft.com/ws/2006/05/servicemodel/securitytokenrequirement/ListenUri";
    private static Assembly AssemblyName = typeof (TrustVersion).Assembly;
    private static BindingFlags SetPropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty;
    private static BindingFlags GetPropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty;
    private static Type MessageSecurityTokenVersionType = FederatedSecurityTokenManager.AssemblyName.GetType("System.ServiceModel.Security.MessageSecurityTokenVersion");
    private static SecurityTokenCache DefaultTokenCache = (SecurityTokenCache) new MruSecurityTokenCache(10000);
    private static ReadOnlyCollection<CookieTransform> DefaultCookieTransforms = new List<CookieTransform>((IEnumerable<CookieTransform>) new CookieTransform[2]
    {
      (CookieTransform) new DeflateCookieTransform(),
      (CookieTransform) new ProtectedDataCookieTransform()
    }).AsReadOnly();
    private ClaimsAuthenticationManager _claimsAuthenticationManager;
    private ExceptionMapper _exceptionMapper = new ExceptionMapper();
    private SecurityTokenResolver _defaultTokenResolver;
    private SecurityTokenHandlerCollection _securityTokenHandlerCollection;
    private object _syncObject = new object();
    private ReadOnlyCollection<CookieTransform> _cookieTransforms;
    private SecurityTokenCache _tokenCache;

    public FederatedSecurityTokenManager(
      ServiceCredentials parentCredentials,
      SecurityTokenHandlerCollection securityTokenHandlerCollection)
      : this(parentCredentials, securityTokenHandlerCollection, new ClaimsAuthenticationManager())
    {
    }

    public FederatedSecurityTokenManager(
      ServiceCredentials parentCredentials,
      SecurityTokenHandlerCollection securityTokenHandlerCollection,
      ClaimsAuthenticationManager claimsAuthenticationManager)
      : base(parentCredentials)
    {
      if (securityTokenHandlerCollection == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenHandlerCollection));
      if (claimsAuthenticationManager == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claimsAuthenticationManager));
      this._securityTokenHandlerCollection = securityTokenHandlerCollection;
      this._claimsAuthenticationManager = claimsAuthenticationManager;
      this._tokenCache = !(this._securityTokenHandlerCollection[typeof (SessionSecurityToken)] is SessionSecurityTokenHandler securityTokenHandler) ? FederatedSecurityTokenManager.DefaultTokenCache : securityTokenHandler.TokenCache;
      this._cookieTransforms = FederatedSecurityTokenManager.DefaultCookieTransforms;
    }

    public SecurityTokenHandlerCollection SecurityTokenHandlers => this._securityTokenHandlerCollection;

    public ExceptionMapper ExceptionMapper
    {
      get => this._exceptionMapper;
      set => this._exceptionMapper = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public override SecurityTokenAuthenticator CreateSecurityTokenAuthenticator(
      SecurityTokenRequirement tokenRequirement,
      out SecurityTokenResolver outOfBandTokenResolver)
    {
      if (tokenRequirement == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenRequirement));
      outOfBandTokenResolver = (SecurityTokenResolver) null;
      string tokenType = tokenRequirement.TokenType;
      if (string.IsNullOrEmpty(tokenType))
        return this.CreateSamlSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
      SecurityTokenHandler securityTokenHandler = this._securityTokenHandlerCollection[tokenType];
      SecurityTokenAuthenticator tokenAuthenticator;
      if (securityTokenHandler != null && securityTokenHandler.CanValidateToken)
      {
        outOfBandTokenResolver = this.GetDefaultOutOfBandTokenResolver();
        if (StringComparer.Ordinal.Equals(tokenType, "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/UserName"))
        {
          if (!(securityTokenHandler is UserNameSecurityTokenHandler wrappedUserNameSecurityTokenHandler))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4072", (object) securityTokenHandler.GetType(), (object) tokenType, (object) typeof (UserNameSecurityTokenHandler))));
          tokenAuthenticator = (SecurityTokenAuthenticator) new WrappedUserNameSecurityTokenAuthenticator(wrappedUserNameSecurityTokenHandler, tokenRequirement, this._claimsAuthenticationManager, this._exceptionMapper);
        }
        else if (StringComparer.Ordinal.Equals(tokenType, "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/Kerberos"))
          tokenAuthenticator = this.CreateInnerSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
        else if (StringComparer.Ordinal.Equals(tokenType, "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/Rsa"))
        {
          if (!(securityTokenHandler is RsaSecurityTokenHandler wrappedRsaSecurityTokenHandler))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4072", (object) securityTokenHandler.GetType(), (object) tokenType, (object) typeof (RsaSecurityTokenHandler))));
          tokenAuthenticator = (SecurityTokenAuthenticator) new WrappedRsaSecurityTokenAuthenticator(wrappedRsaSecurityTokenHandler, tokenRequirement, this._claimsAuthenticationManager, this._exceptionMapper);
        }
        else if (StringComparer.Ordinal.Equals(tokenType, "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/X509Certificate"))
        {
          if (!(securityTokenHandler is X509SecurityTokenHandler wrappedX509SecurityTokenHandler))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4072", (object) securityTokenHandler.GetType(), (object) tokenType, (object) typeof (X509SecurityTokenHandler))));
          tokenAuthenticator = (SecurityTokenAuthenticator) new WrappedX509SecurityTokenAuthenticator(wrappedX509SecurityTokenHandler, tokenRequirement, this._claimsAuthenticationManager, this._exceptionMapper);
        }
        else if (StringComparer.Ordinal.Equals(tokenType, "urn:oasis:names:tc:SAML:1.0:assertion") || StringComparer.Ordinal.Equals(tokenType, "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV1.1"))
        {
          if (!(securityTokenHandler is Saml11SecurityTokenHandler saml11SecurityTokenHandler))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4072", (object) securityTokenHandler.GetType(), (object) tokenType, (object) typeof (Saml11SecurityTokenHandler))));
          if (saml11SecurityTokenHandler.Configuration == null)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
          tokenAuthenticator = (SecurityTokenAuthenticator) new WrappedSaml11SecurityTokenAuthenticator(saml11SecurityTokenHandler, tokenRequirement, this._claimsAuthenticationManager, this._exceptionMapper);
          outOfBandTokenResolver = saml11SecurityTokenHandler.Configuration.ServiceTokenResolver;
        }
        else if (StringComparer.Ordinal.Equals(tokenType, "urn:oasis:names:tc:SAML:2.0:assertion") || StringComparer.Ordinal.Equals(tokenType, "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0"))
        {
          if (!(securityTokenHandler is Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler saml2SecurityTokenHandler))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4072", (object) securityTokenHandler.GetType(), (object) tokenType, (object) typeof (Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler))));
          if (saml2SecurityTokenHandler.Configuration == null)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
          tokenAuthenticator = (SecurityTokenAuthenticator) new WrappedSaml2SecurityTokenAuthenticator(saml2SecurityTokenHandler, tokenRequirement, this._claimsAuthenticationManager, this._exceptionMapper);
          outOfBandTokenResolver = saml2SecurityTokenHandler.Configuration.ServiceTokenResolver;
        }
        else if (StringComparer.Ordinal.Equals(tokenType, ServiceModelSecurityTokenTypes.SecureConversation))
          tokenAuthenticator = tokenRequirement is RecipientServiceModelSecurityTokenRequirement tokenRequirement1 ? this.SetupSecureConversationWrapper(tokenRequirement1, securityTokenHandler as SessionSecurityTokenHandler, out outOfBandTokenResolver) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4240", (object) tokenRequirement.GetType().ToString()));
        else
          tokenAuthenticator = (SecurityTokenAuthenticator) new SecurityTokenAuthenticatorAdapter(securityTokenHandler, tokenRequirement, this._claimsAuthenticationManager, this._exceptionMapper);
      }
      else if (tokenType == ServiceModelSecurityTokenTypes.SecureConversation || tokenType == ServiceModelSecurityTokenTypes.MutualSslnego || (tokenType == ServiceModelSecurityTokenTypes.AnonymousSslnego || tokenType == ServiceModelSecurityTokenTypes.SecurityContext) || tokenType == ServiceModelSecurityTokenTypes.Spnego)
        tokenAuthenticator = tokenRequirement is RecipientServiceModelSecurityTokenRequirement tokenRequirement1 ? this.SetupSecureConversationWrapper(tokenRequirement1, (SessionSecurityTokenHandler) null, out outOfBandTokenResolver) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4240", (object) tokenRequirement.GetType().ToString()));
      else
        tokenAuthenticator = this.CreateInnerSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
      return tokenAuthenticator;
    }

    private SecurityTokenAuthenticator SetupSecureConversationWrapper(
      RecipientServiceModelSecurityTokenRequirement tokenRequirement,
      SessionSecurityTokenHandler tokenHandler,
      out SecurityTokenResolver outOfBandTokenResolver)
    {
      SecurityTokenAuthenticator tokenAuthenticator = base.CreateSecurityTokenAuthenticator((SecurityTokenRequirement) tokenRequirement, out outOfBandTokenResolver);
      SessionSecurityTokenHandler sessionTokenHandler = tokenHandler;
      bool isSessionMode = true;
      if (tokenRequirement.Properties[ServiceModelSecurityTokenRequirement.SupportSecurityContextCancellationProperty] != null)
        isSessionMode = (bool) tokenRequirement.Properties[ServiceModelSecurityTokenRequirement.SupportSecurityContextCancellationProperty];
      if (tokenHandler == null)
      {
        sessionTokenHandler = new SessionSecurityTokenHandler(this._cookieTransforms, this._tokenCache, SessionSecurityTokenHandler.DefaultTokenLifetime);
        sessionTokenHandler.ContainingCollection = this._securityTokenHandlerCollection;
        sessionTokenHandler.Configuration = this._securityTokenHandlerCollection.Configuration;
      }
      if (this.ServiceCredentials is FederatedServiceCredentials serviceCredentials)
        sessionTokenHandler.Configuration.MaxClockSkew = serviceCredentials.MaxClockSkew;
      SctClaimsHandler sctClaimsHandler = new SctClaimsHandler(this._claimsAuthenticationManager, this._securityTokenHandlerCollection, FederatedSecurityTokenManager.GetNormalizedEndpointId((SecurityTokenRequirement) tokenRequirement));
      WrappedSessionSecurityTokenAuthenticator wssta = new WrappedSessionSecurityTokenAuthenticator(sessionTokenHandler, tokenAuthenticator, sctClaimsHandler, this._exceptionMapper);
      Microsoft.IdentityModel.WrappedTokenCache wrappedTokenCache = new Microsoft.IdentityModel.WrappedTokenCache(this._tokenCache, sctClaimsHandler, isSessionMode);
      FederatedSecurityTokenManager.SetWrappedTokenCache(wrappedTokenCache, tokenAuthenticator, wssta, sctClaimsHandler);
      outOfBandTokenResolver = (SecurityTokenResolver) wrappedTokenCache;
      return (SecurityTokenAuthenticator) wssta;
    }

    private static void SetWrappedTokenCache(
      Microsoft.IdentityModel.WrappedTokenCache wrappedTokenCache,
      SecurityTokenAuthenticator sta,
      WrappedSessionSecurityTokenAuthenticator wssta,
      SctClaimsHandler claimsHandler)
    {
      Type type = (Type) null;
      if (sta.GetType().Name == "SecuritySessionSecurityTokenAuthenticator")
        type = FederatedSecurityTokenManager.AssemblyName.GetType("System.ServiceModel.Security.SecuritySessionSecurityTokenAuthenticator");
      else if (sta.GetType().Name == "AcceleratedTokenAuthenticator")
        type = FederatedSecurityTokenManager.AssemblyName.GetType("System.ServiceModel.Security.AcceleratedTokenAuthenticator");
      else if (sta.GetType().Name == "SpnegoTokenAuthenticator")
        type = FederatedSecurityTokenManager.AssemblyName.GetType("System.ServiceModel.Security.SpnegoTokenAuthenticator");
      else if (sta.GetType().Name == "TlsnegoTokenAuthenticator")
        type = FederatedSecurityTokenManager.AssemblyName.GetType("System.ServiceModel.Security.TlsnegoTokenAuthenticator");
      if ((object) type == null)
        return;
      type.InvokeMember("IssuedTokenCache", FederatedSecurityTokenManager.SetPropertyFlags, (Binder) null, (object) sta, new object[1]
      {
        (object) wrappedTokenCache
      }, CultureInfo.InvariantCulture);
      if (!(sta is IIssuanceSecurityTokenAuthenticator tokenAuthenticator))
        return;
      tokenAuthenticator.IssuedSecurityTokenHandler = new IssuedSecurityTokenHandler(claimsHandler.OnTokenIssued);
      tokenAuthenticator.RenewedSecurityTokenHandler = new RenewedSecurityTokenHandler(claimsHandler.OnTokenRenewed);
    }

    public override SecurityTokenSerializer CreateSecurityTokenSerializer(
      SecurityTokenVersion version)
    {
      if (version == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (version));
      TrustVersion trustVersion = (TrustVersion) null;
      SecureConversationVersion secureConversationVersion = (SecureConversationVersion) null;
      foreach (string securitySpecification in version.GetSecuritySpecifications())
      {
        if (StringComparer.Ordinal.Equals(securitySpecification, "http://schemas.xmlsoap.org/ws/2005/02/trust"))
          trustVersion = TrustVersion.WSTrustFeb2005;
        else if (StringComparer.Ordinal.Equals(securitySpecification, "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
          trustVersion = TrustVersion.WSTrust13;
        else if (StringComparer.Ordinal.Equals(securitySpecification, "http://schemas.xmlsoap.org/ws/2005/02/sc"))
          secureConversationVersion = SecureConversationVersion.WSSecureConversationFeb2005;
        else if (StringComparer.Ordinal.Equals(securitySpecification, "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512"))
          secureConversationVersion = SecureConversationVersion.WSSecureConversation13;
        if (trustVersion != null)
        {
          if (secureConversationVersion != null)
            break;
        }
      }
      if (trustVersion == null)
        trustVersion = TrustVersion.WSTrust13;
      if (secureConversationVersion == null)
        secureConversationVersion = SecureConversationVersion.WSSecureConversation13;
      return (SecurityTokenSerializer) new SecurityTokenSerializerAdapter(this._securityTokenHandlerCollection, FederatedSecurityTokenManager.GetSecurityVersion(version), trustVersion, secureConversationVersion, false, this.ServiceCredentials.IssuedTokenAuthentication.SamlSerializer, this.ServiceCredentials.SecureConversationAuthentication.SecurityStateEncoder, (IEnumerable<Type>) this.ServiceCredentials.SecureConversationAuthentication.SecurityContextClaimTypes)
      {
        MapExceptionsToSoapFaults = true,
        ExceptionMapper = this._exceptionMapper
      };
    }

    protected SecurityTokenResolver GetDefaultOutOfBandTokenResolver()
    {
      if (this._defaultTokenResolver == null)
      {
        lock (this._syncObject)
        {
          if (this._defaultTokenResolver == null)
          {
            List<SecurityToken> securityTokenList = new List<SecurityToken>();
            if (this.ServiceCredentials.ServiceCertificate.Certificate != null)
              securityTokenList.Add((SecurityToken) new X509SecurityToken(this.ServiceCredentials.ServiceCertificate.Certificate));
            if (this.ServiceCredentials.IssuedTokenAuthentication.KnownCertificates != null && this.ServiceCredentials.IssuedTokenAuthentication.KnownCertificates.Count > 0)
            {
              for (int index = 0; index < this.ServiceCredentials.IssuedTokenAuthentication.KnownCertificates.Count; ++index)
                securityTokenList.Add((SecurityToken) new X509SecurityToken(this.ServiceCredentials.IssuedTokenAuthentication.KnownCertificates[index]));
            }
            this._defaultTokenResolver = SecurityTokenResolver.CreateDefaultSecurityTokenResolver(securityTokenList.AsReadOnly(), false);
          }
        }
      }
      return this._defaultTokenResolver;
    }

    internal static SecurityVersion GetSecurityVersion(
      SecurityTokenVersion tokenVersion)
    {
      if (tokenVersion == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenVersion));
      return (object) tokenVersion.GetType() == (object) FederatedSecurityTokenManager.MessageSecurityTokenVersionType ? (FederatedSecurityTokenManager.MessageSecurityTokenVersionType.InvokeMember("SecurityVersion", FederatedSecurityTokenManager.GetPropertyFlags, (Binder) null, (object) tokenVersion, (object[]) null, CultureInfo.InvariantCulture) is SecurityVersion securityVersion ? securityVersion : SecurityVersion.WSSecurity11) : (tokenVersion.GetSecuritySpecifications().Contains("http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd") || !tokenVersion.GetSecuritySpecifications().Contains("http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd") ? SecurityVersion.WSSecurity11 : SecurityVersion.WSSecurity10);
    }

    private SecurityTokenAuthenticator CreateInnerSecurityTokenAuthenticator(
      SecurityTokenRequirement tokenRequirement,
      out SecurityTokenResolver outOfBandTokenResolver)
    {
      SecurityTokenAuthenticator tokenAuthenticator = base.CreateSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
      SctClaimsHandler sctClaimsHandler = new SctClaimsHandler(this._claimsAuthenticationManager, this._securityTokenHandlerCollection, FederatedSecurityTokenManager.GetNormalizedEndpointId(tokenRequirement));
      bool isSessionMode = true;
      if (tokenRequirement.Properties[ServiceModelSecurityTokenRequirement.SupportSecurityContextCancellationProperty] != null)
        isSessionMode = (bool) tokenRequirement.Properties[ServiceModelSecurityTokenRequirement.SupportSecurityContextCancellationProperty];
      FederatedSecurityTokenManager.SetWrappedTokenCache(new Microsoft.IdentityModel.WrappedTokenCache(this._tokenCache, sctClaimsHandler, isSessionMode), tokenAuthenticator, (WrappedSessionSecurityTokenAuthenticator) null, sctClaimsHandler);
      return tokenAuthenticator;
    }

    private SecurityTokenAuthenticator CreateSamlSecurityTokenAuthenticator(
      SecurityTokenRequirement tokenRequirement,
      out SecurityTokenResolver outOfBandTokenResolver)
    {
      outOfBandTokenResolver = (SecurityTokenResolver) null;
      Saml11SecurityTokenHandler securityTokenHandler1 = this._securityTokenHandlerCollection["urn:oasis:names:tc:SAML:1.0:assertion"] as Saml11SecurityTokenHandler;
      Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler securityTokenHandler2 = this._securityTokenHandlerCollection["urn:oasis:names:tc:SAML:2.0:assertion"] as Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler;
      if (securityTokenHandler1 != null && securityTokenHandler1.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      if (securityTokenHandler2 != null && securityTokenHandler2.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      SecurityTokenAuthenticator tokenAuthenticator;
      if (securityTokenHandler1 != null && securityTokenHandler2 != null)
      {
        tokenAuthenticator = (SecurityTokenAuthenticator) new WrappedSamlSecurityTokenAuthenticator(new WrappedSaml11SecurityTokenAuthenticator(securityTokenHandler1, tokenRequirement, this._claimsAuthenticationManager, this._exceptionMapper), new WrappedSaml2SecurityTokenAuthenticator(securityTokenHandler2, tokenRequirement, this._claimsAuthenticationManager, this._exceptionMapper));
        outOfBandTokenResolver = (SecurityTokenResolver) new AggregateTokenResolver((IEnumerable<SecurityTokenResolver>) new List<SecurityTokenResolver>()
        {
          securityTokenHandler1.Configuration.ServiceTokenResolver,
          securityTokenHandler2.Configuration.ServiceTokenResolver
        });
      }
      else if (securityTokenHandler1 == null && securityTokenHandler2 != null)
      {
        tokenAuthenticator = (SecurityTokenAuthenticator) new WrappedSaml2SecurityTokenAuthenticator(securityTokenHandler2, tokenRequirement, this._claimsAuthenticationManager, this._exceptionMapper);
        outOfBandTokenResolver = securityTokenHandler2.Configuration.ServiceTokenResolver;
      }
      else if (securityTokenHandler1 != null && securityTokenHandler2 == null)
      {
        tokenAuthenticator = (SecurityTokenAuthenticator) new WrappedSaml11SecurityTokenAuthenticator(securityTokenHandler1, tokenRequirement, this._claimsAuthenticationManager, this._exceptionMapper);
        outOfBandTokenResolver = securityTokenHandler1.Configuration.ServiceTokenResolver;
      }
      else
        tokenAuthenticator = this.CreateInnerSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
      return tokenAuthenticator;
    }

    public static string GetNormalizedEndpointId(SecurityTokenRequirement tokenRequirement)
    {
      if (tokenRequirement == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenRequirement));
      Uri uri = (Uri) null;
      if (tokenRequirement.Properties.ContainsKey(FederatedSecurityTokenManager.ListenUriProperty))
        uri = tokenRequirement.Properties[FederatedSecurityTokenManager.ListenUriProperty] as Uri;
      if (uri == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4287", (object) tokenRequirement));
      return uri.IsDefaultPort ? string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}://NormalizedHostName{1}", (object) uri.Scheme, (object) uri.AbsolutePath) : string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}://NormalizedHostName:{1}{2}", (object) uri.Scheme, (object) uri.Port, (object) uri.AbsolutePath);
    }
  }
}
