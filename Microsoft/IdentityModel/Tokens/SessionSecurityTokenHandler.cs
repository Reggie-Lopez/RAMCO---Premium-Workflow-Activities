// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Diagnostics;
using Microsoft.IdentityModel.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Web.Compilation;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class SessionSecurityTokenHandler : SecurityTokenHandler
  {
    private const string DefaultCookieElementName = "Cookie";
    private const string DefaultCookieNamespace = "http://schemas.microsoft.com/ws/2006/05/security";
    public static readonly TimeSpan DefaultLifetime = TimeSpan.FromHours(10.0);
    public static readonly ReadOnlyCollection<CookieTransform> DefaultCookieTransforms = new List<CookieTransform>((IEnumerable<CookieTransform>) new CookieTransform[2]
    {
      (CookieTransform) new DeflateCookieTransform(),
      (CookieTransform) new ProtectedDataCookieTransform()
    }).AsReadOnly();
    private static SecureConversationVersion DefaultVersion = SecureConversationVersion.WSSecureConversation13;
    private bool _useWindowsTokenService;
    private SecurityTokenCache _tokenCache;
    private TimeSpan _tokenLifetime = SessionSecurityTokenHandler.DefaultLifetime;
    private ReadOnlyCollection<CookieTransform> _transforms = SessionSecurityTokenHandler.DefaultCookieTransforms;

    public SessionSecurityTokenHandler()
      : this(SessionSecurityTokenHandler.DefaultCookieTransforms)
    {
    }

    public SessionSecurityTokenHandler(ReadOnlyCollection<CookieTransform> transforms)
      : this(transforms, (SecurityTokenCache) new MruSecurityTokenCache(), SessionSecurityTokenHandler.DefaultLifetime)
    {
    }

    public SessionSecurityTokenHandler(
      ReadOnlyCollection<CookieTransform> transforms,
      SecurityTokenCache tokenCache,
      TimeSpan tokenLifetime)
    {
      if (transforms == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (transforms));
      if (tokenCache == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenCache));
      if (tokenLifetime <= TimeSpan.Zero)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID0016"));
      this._transforms = transforms;
      this._tokenCache = tokenCache;
      this._tokenLifetime = tokenLifetime;
      this._tokenCache.Owner = (SecurityTokenHandler) this;
    }

    public SessionSecurityTokenHandler(XmlNodeList customConfigElements)
      : this()
    {
      List<XmlElement> xmlElementList = customConfigElements != null ? XmlUtil.GetXmlElements(customConfigElements) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (customConfigElements));
      bool flag = false;
      foreach (XmlElement xmlElement in xmlElementList)
      {
        if (StringComparer.Ordinal.Equals(xmlElement.LocalName, "sessionTokenRequirement"))
        {
          if (flag)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7026", (object) "sessionTokenRequirement"));
          this._tokenLifetime = SessionSecurityTokenHandler.DefaultLifetime;
          SecurityTokenCache securityTokenCache = (SecurityTokenCache) null;
          int maximumSize = -1;
          foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlElement.Attributes)
          {
            if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "securityTokenCacheType"))
            {
              string typeName = attribute.Value;
              securityTokenCache = (!string.IsNullOrEmpty(typeName) ? CustomTypeElement.Resolve<SecurityTokenCache>(new CustomTypeElement(BuildManager.GetType(typeName, true))) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7015"))) ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7015"));
            }
            else if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "useWindowsTokenService"))
            {
              bool result = false;
              if (!bool.TryParse(attribute.Value, out result))
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7021", (object) attribute.Value));
              this._useWindowsTokenService = result;
            }
            else if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "lifetime"))
            {
              TimeSpan result = SessionSecurityTokenHandler.DefaultLifetime;
              if (!TimeSpan.TryParse(attribute.Value, out result))
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7017", (object) attribute.Value));
              this._tokenLifetime = !(result < TimeSpan.Zero) ? result : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7018"));
            }
            else if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "securityTokenCacheSize"))
            {
              int result = -1;
              if (!int.TryParse(attribute.Value, out result))
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7024", (object) attribute.Value));
              maximumSize = result >= 0 ? result : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7025"));
            }
            else
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID7004", (object) attribute.LocalName, (object) xmlElement.LocalName)));
          }
          this._tokenCache = securityTokenCache == null ? (maximumSize == -1 ? (SecurityTokenCache) new MruSecurityTokenCache() : (SecurityTokenCache) new MruSecurityTokenCache(maximumSize)) : securityTokenCache;
          this._transforms = SessionSecurityTokenHandler.DefaultCookieTransforms;
          flag = true;
        }
      }
    }

    public virtual string CookieElementName => "Cookie";

    public virtual string CookieNamespace => "http://schemas.microsoft.com/ws/2006/05/security";

    protected virtual byte[] ApplyTransforms(byte[] cookie, bool outbound)
    {
      byte[] encoded = cookie;
      if (outbound)
      {
        for (int index = 0; index < this._transforms.Count; ++index)
          encoded = this._transforms[index].Encode(encoded);
      }
      else
      {
        for (int count = this._transforms.Count; count > 0; --count)
          encoded = this._transforms[count - 1].Decode(encoded);
      }
      return encoded;
    }

    public override bool CanReadToken(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      return reader.IsStartElement("SecurityContextToken", "http://schemas.xmlsoap.org/ws/2005/02/sc") || reader.IsStartElement("SecurityContextToken", "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512");
    }

    public override bool CanValidateToken => true;

    public override bool CanWriteToken => true;

    public override SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
    {
      if (tokenDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4272"));
      IClaimsPrincipal fromIdentity = ClaimsPrincipal.CreateFromIdentity((IIdentity) tokenDescriptor.Subject);
      if (this.Configuration.SaveBootstrapTokens)
        fromIdentity.Identities[0].BootstrapToken = tokenDescriptor.Token;
      DateTime dateTime1 = tokenDescriptor.Lifetime.Created.HasValue ? tokenDescriptor.Lifetime.Created.Value : DateTime.UtcNow;
      DateTime dateTime2 = tokenDescriptor.Lifetime.Expires.HasValue ? tokenDescriptor.Lifetime.Expires.Value : DateTime.UtcNow + SessionSecurityTokenHandler.DefaultTokenLifetime;
      return (SecurityToken) new SessionSecurityToken(fromIdentity, (string) null, new DateTime?(dateTime1), new DateTime?(dateTime2));
    }

    public virtual SessionSecurityToken CreateSessionSecurityToken(
      IClaimsPrincipal principal,
      string context,
      string endpointId,
      DateTime validFrom,
      DateTime validTo)
    {
      if (principal == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (principal));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4272"));
      return new SessionSecurityToken(principal, context, endpointId, new DateTime?(validFrom), new DateTime?(validTo));
    }

    public static SecureConversationVersion DefaultSecureConversationVersion => SessionSecurityTokenHandler.DefaultVersion;

    public static TimeSpan DefaultTokenLifetime => SessionSecurityTokenHandler.DefaultLifetime;

    public static ReadOnlyCollection<CookieTransform> DefaultTransforms => SessionSecurityTokenHandler.DefaultCookieTransforms;

    public virtual SecurityToken ReadToken(byte[] token) => (SecurityToken) null;

    public virtual SecurityToken ReadToken(
      byte[] token,
      SecurityTokenResolver tokenResolver)
    {
      SecurityToken securityToken = this.ReadToken(token);
      if (securityToken != null)
        return securityToken;
      using (XmlReader textReader = (XmlReader) XmlDictionaryReader.CreateTextReader(token, XmlDictionaryReaderQuotas.Max))
        return this.ReadToken(textReader, tokenResolver);
    }

    public override SecurityToken ReadToken(XmlReader reader) => this.ReadToken(reader, EmptySecurityTokenResolver.Instance);

    public override SecurityToken ReadToken(
      XmlReader reader,
      SecurityTokenResolver tokenResolver)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (tokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenResolver));
      System.Xml.UniqueId generation = (System.Xml.UniqueId) null;
      SessionSecurityToken sessionSecurityToken = (SessionSecurityToken) null;
      SessionDictionary instance = SessionDictionary.Instance;
      XmlDictionaryReader dictionaryReader = XmlDictionaryReader.CreateDictionaryReader(reader);
      SecureConversationVersion version;
      string ns;
      string localname1;
      string localname2;
      if (dictionaryReader.IsStartElement("SecurityContextToken", "http://schemas.xmlsoap.org/ws/2005/02/sc"))
      {
        version = SecureConversationVersion.WSSecureConversationFeb2005;
        ns = "http://schemas.xmlsoap.org/ws/2005/02/sc";
        localname1 = "Identifier";
        localname2 = "Instance";
      }
      else if (dictionaryReader.IsStartElement("SecurityContextToken", "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512"))
      {
        version = SecureConversationVersion.WSSecureConversation13;
        ns = "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512";
        localname1 = "Identifier";
        localname2 = "Instance";
      }
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4230", (object) "SecurityContextToken", (object) dictionaryReader.Name)));
      string attribute = dictionaryReader.GetAttribute("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
      dictionaryReader.ReadFullStartElement();
      if (!dictionaryReader.IsStartElement(localname1, ns))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4230", (object) "Identifier", (object) dictionaryReader.Name)));
      System.Xml.UniqueId contextId = dictionaryReader.ReadElementContentAsUniqueId();
      if (contextId == (System.Xml.UniqueId) null || string.IsNullOrEmpty(contextId.ToString()))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4242")));
      if (dictionaryReader.IsStartElement(localname2, ns))
        generation = dictionaryReader.ReadElementContentAsUniqueId();
      if (dictionaryReader.IsStartElement(this.CookieElementName, this.CookieNamespace))
      {
        SecurityToken token = (SecurityToken) null;
        SecurityContextKeyIdentifierClause identifierClause = !(generation == (System.Xml.UniqueId) null) ? new SecurityContextKeyIdentifierClause(contextId, generation) : new SecurityContextKeyIdentifierClause(contextId);
        tokenResolver.TryResolveToken((SecurityKeyIdentifierClause) identifierClause, out token);
        if (token != null)
        {
          sessionSecurityToken = !(token is SecurityContextSecurityToken securityContextToken) ? token as SessionSecurityToken : new SessionSecurityToken(securityContextToken, version);
          dictionaryReader.Skip();
        }
        else
        {
          byte[] cookie = this.ApplyTransforms(dictionaryReader.ReadElementContentAsBase64() ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4237"))), false);
          sessionSecurityToken = this.CreateCookieSerializer().Deserialize(cookie);
          if (sessionSecurityToken != null && sessionSecurityToken.ContextId != contextId)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4229", (object) sessionSecurityToken.ContextId, (object) contextId)));
          if (sessionSecurityToken != null && sessionSecurityToken.Id != attribute)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4227", (object) sessionSecurityToken.Id, (object) attribute)));
        }
      }
      else
      {
        SecurityToken token = (SecurityToken) null;
        SecurityContextKeyIdentifierClause identifierClause = !(generation == (System.Xml.UniqueId) null) ? new SecurityContextKeyIdentifierClause(contextId, generation) : new SecurityContextKeyIdentifierClause(contextId);
        tokenResolver.TryResolveToken((SecurityKeyIdentifierClause) identifierClause, out token);
        if (token != null)
          sessionSecurityToken = !(token is SecurityContextSecurityToken securityContextToken) ? token as SessionSecurityToken : new SessionSecurityToken(securityContextToken, version);
      }
      dictionaryReader.ReadEndElement();
      return sessionSecurityToken != null ? (SecurityToken) sessionSecurityToken : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4243")));
    }

    public bool UseWindowsTokenService
    {
      get => this._useWindowsTokenService;
      set => this._useWindowsTokenService = value;
    }

    public virtual TimeSpan TokenLifetime
    {
      get => this._tokenLifetime;
      set => this._tokenLifetime = !(value <= TimeSpan.Zero) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID0016"));
    }

    public virtual SessionSecurityTokenCookieSerializer CreateCookieSerializer()
    {
      SecurityTokenHandlerCollection bootstrapTokenHandlers = this.ContainingCollection ?? SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection();
      bool saveBootstrapTokens = false;
      string windowsIssuerName = "LOCAL AUTHORITY";
      if (this.Configuration != null)
      {
        saveBootstrapTokens = this.Configuration.SaveBootstrapTokens;
        windowsIssuerName = this.Configuration.IssuerNameRegistry.GetWindowsIssuerName();
      }
      return new SessionSecurityTokenCookieSerializer(bootstrapTokenHandlers, saveBootstrapTokens, this._useWindowsTokenService, windowsIssuerName);
    }

    public SecurityTokenCache TokenCache
    {
      get => this._tokenCache;
      set
      {
        this._tokenCache = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
        this._tokenCache.Owner = (SecurityTokenHandler) this;
      }
    }

    public override string[] GetTokenTypeIdentifiers() => new string[3]
    {
      ServiceModelSecurityTokenTypes.SecureConversation,
      "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512/sct",
      "http://schemas.xmlsoap.org/ws/2005/02/sc/sct"
    };

    public override Type TokenType => typeof (SessionSecurityToken);

    public ReadOnlyCollection<CookieTransform> Transforms => this._transforms;

    protected void SetTransforms(IEnumerable<CookieTransform> transforms) => this._transforms = new List<CookieTransform>(transforms).AsReadOnly();

    public override ClaimsIdentityCollection ValidateToken(
      SecurityToken token)
    {
      if (!(token is SessionSecurityToken securityToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Verbose, TraceCode.Diagnostics, (string) null, (TraceRecord) new TokenTraceRecord(token), (Exception) null);
      this.ValidateSession(securityToken);
      return securityToken.ClaimsPrincipal.Identities;
    }

    public virtual ClaimsIdentityCollection ValidateToken(
      SessionSecurityToken token,
      string endpointId)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (endpointId == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (endpointId));
      if (!string.IsNullOrEmpty(token.EndpointId) && token.EndpointId != endpointId)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4291", (object) token)));
      return this.ValidateToken((SecurityToken) token);
    }

    protected virtual void ValidateSession(SessionSecurityToken securityToken)
    {
      if (securityToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityToken));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      DateTime utcNow = DateTime.UtcNow;
      DateTime dateTime1 = DateTimeUtil.Add(utcNow, this.Configuration.MaxClockSkew);
      DateTime dateTime2 = DateTimeUtil.Add(utcNow, -this.Configuration.MaxClockSkew);
      if (securityToken.ValidFrom > dateTime1)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenNotYetValidException(Microsoft.IdentityModel.SR.GetString("ID4255", (object) securityToken.ValidTo, (object) securityToken.ValidFrom, (object) utcNow)));
      if (securityToken.ValidTo < dateTime2)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenExpiredException(Microsoft.IdentityModel.SR.GetString("ID4255", (object) securityToken.ValidTo, (object) securityToken.ValidFrom, (object) utcNow)));
    }

    public virtual byte[] WriteToken(SessionSecurityToken sessionToken)
    {
      if (sessionToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sessionToken));
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (XmlWriter writer = XmlWriter.Create((Stream) memoryStream))
        {
          this.WriteToken(writer, (SecurityToken) sessionToken);
          writer.Flush();
        }
        return memoryStream.ToArray();
      }
    }

    public override void WriteToken(XmlWriter writer, SecurityToken token)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (!(token is SessionSecurityToken sessionToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4046", (object) token, (object) this.TokenType));
      string ns;
      string localName1;
      string localName2;
      string localName3;
      if (sessionToken.SecureConversationVersion == SecureConversationVersion.WSSecureConversationFeb2005)
      {
        ns = "http://schemas.xmlsoap.org/ws/2005/02/sc";
        localName1 = "SecurityContextToken";
        localName2 = "Identifier";
        localName3 = "Instance";
      }
      else
      {
        if (sessionToken.SecureConversationVersion != SecureConversationVersion.WSSecureConversation13)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4050"));
        ns = "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512";
        localName1 = "SecurityContextToken";
        localName2 = "Identifier";
        localName3 = "Instance";
      }
      SessionDictionary instance = SessionDictionary.Instance;
      XmlDictionaryWriter dictionaryWriter = !(writer is XmlDictionaryWriter) ? XmlDictionaryWriter.CreateDictionaryWriter(writer) : (XmlDictionaryWriter) writer;
      dictionaryWriter.WriteStartElement(localName1, ns);
      if (sessionToken.Id != null)
        dictionaryWriter.WriteAttributeString("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", sessionToken.Id);
      dictionaryWriter.WriteElementString(localName2, ns, sessionToken.ContextId.ToString());
      if (sessionToken.KeyGeneration != (System.Xml.UniqueId) null)
      {
        dictionaryWriter.WriteStartElement(localName3, ns);
        dictionaryWriter.WriteValue(sessionToken.KeyGeneration);
        dictionaryWriter.WriteEndElement();
      }
      if (!sessionToken.IsSessionMode)
      {
        dictionaryWriter.WriteStartElement(this.CookieElementName, this.CookieNamespace);
        byte[] buffer = this.ApplyTransforms(this.CreateCookieSerializer().Serialize(sessionToken), true);
        dictionaryWriter.WriteBase64(buffer, 0, buffer.Length);
        dictionaryWriter.WriteEndElement();
      }
      dictionaryWriter.WriteEndElement();
      dictionaryWriter.Flush();
    }
  }
}
