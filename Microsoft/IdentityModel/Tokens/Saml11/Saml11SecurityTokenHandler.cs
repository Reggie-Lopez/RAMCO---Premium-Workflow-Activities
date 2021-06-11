// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml11.Saml11SecurityTokenHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Diagnostics;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Protocols.XmlSignature;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens.Saml11
{
  [ComVisible(true)]
  public class Saml11SecurityTokenHandler : Microsoft.IdentityModel.Tokens.SecurityTokenHandler
  {
    public const string Namespace = "urn:oasis:names:tc:SAML:1.0";
    public const string BearerConfirmationMethod = "urn:oasis:names:tc:SAML:1.0:cm:bearer";
    public const string UnspecifiedAuthenticationMethod = "urn:oasis:names:tc:SAML:1.0:am:unspecified";
    public const string Assertion = "urn:oasis:names:tc:SAML:1.0:assertion";
    private const string Attribute = "saml:Attribute";
    private const string Actor = "Actor";
    private static DateTime WCFMinValue = new DateTime(DateTime.MinValue.Ticks + 864000000000L, DateTimeKind.Utc);
    private static DateTime WCFMaxValue = new DateTime(DateTime.MaxValue.Ticks - 864000000000L, DateTimeKind.Utc);
    private static string[] _tokenTypeIdentifiers = new string[2]
    {
      "urn:oasis:names:tc:SAML:1.0:assertion",
      "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV1.1"
    };
    private Microsoft.IdentityModel.Tokens.SamlSecurityTokenRequirement _samlSecurityTokenRequirement;
    private SecurityTokenSerializer _keyInfoSerializer;
    private object _syncObject = new object();

    public Saml11SecurityTokenHandler()
      : this(new Microsoft.IdentityModel.Tokens.SamlSecurityTokenRequirement())
    {
    }

    public Saml11SecurityTokenHandler(
      Microsoft.IdentityModel.Tokens.SamlSecurityTokenRequirement samlSecurityTokenRequirement)
    {
      this._samlSecurityTokenRequirement = samlSecurityTokenRequirement != null ? samlSecurityTokenRequirement : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (samlSecurityTokenRequirement));
    }

    public Saml11SecurityTokenHandler(XmlNodeList customConfigElements)
    {
      List<XmlElement> xmlElementList = customConfigElements != null ? XmlUtil.GetXmlElements(customConfigElements) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (customConfigElements));
      bool flag = false;
      foreach (XmlElement element in xmlElementList)
      {
        if (!(element.LocalName != "samlSecurityTokenRequirement"))
        {
          if (flag)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7026", (object) "samlSecurityTokenRequirement"));
          this._samlSecurityTokenRequirement = new Microsoft.IdentityModel.Tokens.SamlSecurityTokenRequirement(element);
          flag = true;
        }
      }
      if (flag)
        return;
      this._samlSecurityTokenRequirement = new Microsoft.IdentityModel.Tokens.SamlSecurityTokenRequirement();
    }

    public override SecurityToken CreateToken(Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      IEnumerable<SamlStatement> statements = tokenDescriptor != null ? this.CreateStatements(tokenDescriptor) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor));
      SamlConditions conditions = this.CreateConditions(tokenDescriptor.Lifetime, tokenDescriptor.AppliesToAddress, tokenDescriptor);
      SamlAdvice advice = this.CreateAdvice(tokenDescriptor);
      SamlAssertion assertion = this.CreateAssertion(tokenDescriptor.TokenIssuerName, conditions, advice, statements);
      if (assertion == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4013")));
      assertion.SigningCredentials = this.GetSigningCredentials(tokenDescriptor);
      SecurityToken token = (SecurityToken) new SamlSecurityToken(assertion);
      Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = this.GetEncryptingCredentials(tokenDescriptor);
      if (encryptingCredentials != null)
        token = (SecurityToken) new Microsoft.IdentityModel.Tokens.EncryptedSecurityToken(token, encryptingCredentials);
      return token;
    }

    protected virtual Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials GetEncryptingCredentials(
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      if (tokenDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor));
      Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = (Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials) null;
      if (tokenDescriptor.EncryptingCredentials != null)
      {
        encryptingCredentials = tokenDescriptor.EncryptingCredentials;
        if (encryptingCredentials.SecurityKey is AsymmetricSecurityKey)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4178")));
      }
      return encryptingCredentials;
    }

    protected virtual SigningCredentials GetSigningCredentials(
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      return tokenDescriptor != null ? tokenDescriptor.SigningCredentials : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor));
    }

    protected virtual SamlAdvice CreateAdvice(Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor) => (SamlAdvice) null;

    protected virtual SamlAssertion CreateAssertion(
      string issuer,
      SamlConditions conditions,
      SamlAdvice advice,
      IEnumerable<SamlStatement> statements)
    {
      return (SamlAssertion) new Saml11Assertion(Microsoft.IdentityModel.UniqueId.CreateRandomId(), issuer, DateTime.UtcNow, conditions, advice, statements);
    }

    public override SecurityKeyIdentifierClause CreateSecurityTokenReference(
      SecurityToken token,
      bool attached)
    {
      return token != null ? (SecurityKeyIdentifierClause) token.CreateKeyIdentifierClause<SamlAssertionKeyIdentifierClause>() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
    }

    protected virtual SamlConditions CreateConditions(
      Lifetime tokenLifetime,
      string relyingPartyAddress,
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      SamlConditions samlConditions = new SamlConditions();
      if (tokenLifetime != null)
      {
        if (tokenLifetime.Created.HasValue)
          samlConditions.NotBefore = tokenLifetime.Created.Value;
        if (tokenLifetime.Expires.HasValue)
          samlConditions.NotOnOrAfter = tokenLifetime.Expires.Value;
      }
      if (!string.IsNullOrEmpty(relyingPartyAddress))
        samlConditions.Conditions.Add((SamlCondition) new SamlAudienceRestrictionCondition((IEnumerable<Uri>) new Uri[1]
        {
          new Uri(relyingPartyAddress)
        }));
      return samlConditions;
    }

    protected virtual IEnumerable<SamlStatement> CreateStatements(
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      Collection<SamlStatement> collection = new Collection<SamlStatement>();
      SamlSubject samlSubject = this.CreateSamlSubject(tokenDescriptor);
      SamlAttributeStatement attributeStatement = this.CreateAttributeStatement(samlSubject, tokenDescriptor.Subject, tokenDescriptor);
      if (attributeStatement != null)
        collection.Add((SamlStatement) attributeStatement);
      SamlAuthenticationStatement authenticationStatement = this.CreateAuthenticationStatement(samlSubject, tokenDescriptor.AuthenticationInfo, tokenDescriptor);
      if (authenticationStatement != null)
        collection.Add((SamlStatement) authenticationStatement);
      return (IEnumerable<SamlStatement>) collection;
    }

    protected virtual SamlAuthenticationStatement CreateAuthenticationStatement(
      SamlSubject samlSubject,
      AuthenticationInformation authInfo,
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      if (samlSubject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (samlSubject));
      if (tokenDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor));
      if (tokenDescriptor.Subject == null)
        return (SamlAuthenticationStatement) null;
      string normalizedAuthenticationType = (string) null;
      string s = (string) null;
      IEnumerable<Claim> source1 = tokenDescriptor.Subject.Claims.Where<Claim>((Func<Claim, bool>) (c => c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod"));
      if (source1.Count<Claim>() > 0)
        normalizedAuthenticationType = source1.First<Claim>().Value;
      IEnumerable<Claim> source2 = tokenDescriptor.Subject.Claims.Where<Claim>((Func<Claim, bool>) (c => c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant"));
      if (source2.Count<Claim>() > 0)
        s = source2.First<Claim>().Value;
      if (normalizedAuthenticationType == null && s == null)
        return (SamlAuthenticationStatement) null;
      if (normalizedAuthenticationType == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4270", (object) "AuthenticationMethod", (object) "SAML11"));
      if (s == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4270", (object) "AuthenticationInstant", (object) "SAML11"));
      DateTime universalTime = DateTime.ParseExact(s, DateTimeFormats.Accepted, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();
      return authInfo == null ? new SamlAuthenticationStatement(samlSubject, this.DenormalizeAuthenticationType(normalizedAuthenticationType), universalTime, (string) null, (string) null, (IEnumerable<SamlAuthorityBinding>) null) : new SamlAuthenticationStatement(samlSubject, this.DenormalizeAuthenticationType(normalizedAuthenticationType), universalTime, authInfo.DnsName, authInfo.Address, (IEnumerable<SamlAuthorityBinding>) null);
    }

    protected virtual SamlAttributeStatement CreateAttributeStatement(
      SamlSubject samlSubject,
      IClaimsIdentity subject,
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      if (subject == null)
        return (SamlAttributeStatement) null;
      if (samlSubject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (samlSubject));
      if (subject.Claims != null)
      {
        List<SamlAttribute> samlAttributeList = new List<SamlAttribute>(subject.Claims.Count);
        foreach (Claim claim in subject.Claims)
        {
          if (claim != null && claim.ClaimType != "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
          {
            switch (claim.ClaimType)
            {
              case "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant":
              case "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod":
                continue;
              default:
                samlAttributeList.Add(this.CreateAttribute(claim, tokenDescriptor));
                continue;
            }
          }
        }
        this.AddDelegateToAttributes(subject, (ICollection<SamlAttribute>) samlAttributeList, tokenDescriptor);
        ICollection<SamlAttribute> samlAttributes = this.CollectAttributeValues((ICollection<SamlAttribute>) samlAttributeList);
        if (samlAttributes.Count > 0)
          return new SamlAttributeStatement(samlSubject, (IEnumerable<SamlAttribute>) samlAttributes);
      }
      return (SamlAttributeStatement) null;
    }

    protected virtual ICollection<SamlAttribute> CollectAttributeValues(
      ICollection<SamlAttribute> attributes)
    {
      Dictionary<Microsoft.IdentityModel.Tokens.SamlAttributeKeyComparer.AttributeKey, SamlAttribute> dictionary = new Dictionary<Microsoft.IdentityModel.Tokens.SamlAttributeKeyComparer.AttributeKey, SamlAttribute>(attributes.Count, (IEqualityComparer<Microsoft.IdentityModel.Tokens.SamlAttributeKeyComparer.AttributeKey>) new Microsoft.IdentityModel.Tokens.SamlAttributeKeyComparer());
      foreach (SamlAttribute attribute1 in (IEnumerable<SamlAttribute>) attributes)
      {
        if (attribute1 is Saml11Attribute attribute)
        {
          Microsoft.IdentityModel.Tokens.SamlAttributeKeyComparer.AttributeKey key = new Microsoft.IdentityModel.Tokens.SamlAttributeKeyComparer.AttributeKey(attribute);
          if (dictionary.ContainsKey(key))
          {
            foreach (string attributeValue in (IEnumerable<string>) attribute.AttributeValues)
              dictionary[key].AttributeValues.Add(attributeValue);
          }
          else
            dictionary.Add(key, (SamlAttribute) attribute);
        }
      }
      return (ICollection<SamlAttribute>) dictionary.Values;
    }

    protected virtual void AddDelegateToAttributes(
      IClaimsIdentity subject,
      ICollection<SamlAttribute> attributes,
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      if (subject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (subject));
      if (tokenDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor));
      if (subject.Actor == null)
        return;
      List<SamlAttribute> samlAttributeList = new List<SamlAttribute>(subject.Actor.Claims.Count);
      foreach (Claim claim in subject.Actor.Claims)
      {
        if (claim != null)
          samlAttributeList.Add(this.CreateAttribute(claim, tokenDescriptor));
      }
      this.AddDelegateToAttributes(subject.Actor, (ICollection<SamlAttribute>) samlAttributeList, tokenDescriptor);
      ICollection<SamlAttribute> samlAttributes = this.CollectAttributeValues((ICollection<SamlAttribute>) samlAttributeList);
      attributes.Add(this.CreateAttribute(new Claim("http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor", this.CreateXmlStringFromAttributes((IEnumerable<SamlAttribute>) samlAttributes), "http://www.w3.org/2001/XMLSchema#string"), tokenDescriptor));
    }

    protected virtual SamlSubject CreateSamlSubject(
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      if (tokenDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor));
      SamlSubject samlSubject = new SamlSubject();
      Claim claim1 = (Claim) null;
      if (tokenDescriptor.Subject != null && tokenDescriptor.Subject.Claims != null)
      {
        foreach (Claim claim2 in tokenDescriptor.Subject.Claims)
        {
          if (claim2.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
            claim1 = claim1 == null ? claim2 : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4139")));
        }
      }
      if (claim1 != null)
      {
        samlSubject.Name = claim1.Value;
        if (claim1.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format"))
          samlSubject.NameFormat = claim1.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format"];
        if (claim1.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier"))
          samlSubject.NameQualifier = claim1.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier"];
      }
      if (tokenDescriptor.Proof != null)
      {
        samlSubject.KeyIdentifier = tokenDescriptor.Proof.KeyIdentifier;
        samlSubject.ConfirmationMethods.Add(SamlConstants.HolderOfKey);
      }
      else
        samlSubject.ConfirmationMethods.Add("urn:oasis:names:tc:SAML:1.0:cm:bearer");
      return samlSubject;
    }

    protected virtual string CreateXmlStringFromAttributes(IEnumerable<SamlAttribute> attributes)
    {
      bool flag = false;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter((Stream) memoryStream, Encoding.UTF8, false))
        {
          foreach (SamlAttribute attribute in attributes)
          {
            if (attribute != null)
            {
              if (!flag)
              {
                textWriter.WriteStartElement("Actor");
                flag = true;
              }
              this.WriteAttribute((XmlWriter) textWriter, attribute);
            }
          }
          if (flag)
            textWriter.WriteEndElement();
          textWriter.Flush();
        }
        return Encoding.UTF8.GetString(memoryStream.ToArray());
      }
    }

    protected virtual SamlAttribute CreateAttribute(
      Claim claim,
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      int length = claim != null ? claim.ClaimType.LastIndexOf('/') : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claim));
      if (length == 0 || length == -1)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("claimType", Microsoft.IdentityModel.SR.GetString("ID4216", (object) claim.ClaimType));
      if (length == claim.ClaimType.Length - 1)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("claimType", Microsoft.IdentityModel.SR.GetString("ID4216", (object) claim.ClaimType));
      string attributeNamespace = claim.ClaimType.Substring(0, length);
      if (attributeNamespace.EndsWith("/", StringComparison.Ordinal))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (claim), Microsoft.IdentityModel.SR.GetString("ID4213", (object) claim.ClaimType));
      string attributeName = claim.ClaimType.Substring(length + 1, claim.ClaimType.Length - (length + 1));
      Saml11Attribute saml11Attribute = new Saml11Attribute(attributeNamespace, attributeName, (IEnumerable<string>) new string[1]
      {
        claim.Value
      });
      if (!StringComparer.Ordinal.Equals("LOCAL AUTHORITY", claim.OriginalIssuer))
        saml11Attribute.OriginalIssuer = claim.OriginalIssuer;
      saml11Attribute.AttributeValueXsiType = claim.ValueType;
      return (SamlAttribute) saml11Attribute;
    }

    public override bool CanValidateToken => true;

    public X509CertificateValidator CertificateValidator
    {
      get
      {
        if (this._samlSecurityTokenRequirement.CertificateValidator != null)
          return this._samlSecurityTokenRequirement.CertificateValidator;
        return this.Configuration != null ? this.Configuration.CertificateValidator : (X509CertificateValidator) null;
      }
      set => this._samlSecurityTokenRequirement.CertificateValidator = value;
    }

    protected override void DetectReplayedTokens(SecurityToken token)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (!(token is SamlSecurityToken token1))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID1067", (object) token.GetType().ToString()));
      if (token1.SecurityKeys.Count != 0)
        return;
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      if (this.Configuration.TokenReplayCache == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4278"));
      if (string.IsNullOrEmpty(token1.Assertion.AssertionId))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenValidationException(Microsoft.IdentityModel.SR.GetString("ID1063")));
      StringBuilder stringBuilder = new StringBuilder();
      string base64String;
      using (HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.NewSha256())
      {
        if (string.IsNullOrEmpty(token1.Assertion.Issuer))
          stringBuilder.AppendFormat("{0}{1}", (object) token1.Assertion.AssertionId, (object) Saml11SecurityTokenHandler._tokenTypeIdentifiers[0]);
        else
          stringBuilder.AppendFormat("{0}{1}{2}", (object) token1.Assertion.AssertionId, (object) token1.Assertion.Issuer, (object) Saml11SecurityTokenHandler._tokenTypeIdentifiers[0]);
        base64String = Convert.ToBase64String(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(stringBuilder.ToString())));
      }
      if (this.Configuration.TokenReplayCache.TryFind(base64String))
      {
        if (string.IsNullOrEmpty(token1.Assertion.Issuer))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Microsoft.IdentityModel.Tokens.SecurityTokenReplayDetectedException(Microsoft.IdentityModel.SR.GetString("ID1062", (object) typeof (SamlSecurityToken).ToString(), (object) token1.Assertion.AssertionId, (object) "")));
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Microsoft.IdentityModel.Tokens.SecurityTokenReplayDetectedException(Microsoft.IdentityModel.SR.GetString("ID1062", (object) typeof (SamlSecurityToken).ToString(), (object) token1.Assertion.AssertionId, (object) token1.Assertion.Issuer)));
      }
      this.Configuration.TokenReplayCache.TryAdd(base64String, (SecurityToken) null, DateTimeUtil.Add(this.GetCacheExpirationTime(token1), this.Configuration.MaxClockSkew));
    }

    protected virtual DateTime GetCacheExpirationTime(SamlSecurityToken token)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      DateTime t1 = DateTimeUtil.Add(DateTime.UtcNow, this.Configuration.TokenReplayCacheExpirationPeriod);
      return DateTime.Compare(t1, token.ValidTo) < 0 ? t1 : token.ValidTo;
    }

    protected virtual void ValidateConditions(
      SamlConditions conditions,
      bool enforceAudienceRestriction)
    {
      if (conditions != null)
      {
        DateTime utcNow = DateTime.UtcNow;
        DateTime notBefore = conditions.NotBefore;
        if (DateTimeUtil.Add(utcNow, this.Configuration.MaxClockSkew) < conditions.NotBefore)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Microsoft.IdentityModel.Tokens.SecurityTokenNotYetValidException(Microsoft.IdentityModel.SR.GetString("ID4222", (object) conditions.NotBefore, (object) utcNow)));
        DateTime notOnOrAfter = conditions.NotOnOrAfter;
        if (DateTimeUtil.Add(utcNow, this.Configuration.MaxClockSkew.Negate()) >= conditions.NotOnOrAfter)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException(Microsoft.IdentityModel.SR.GetString("ID4223", (object) conditions.NotOnOrAfter, (object) utcNow)));
      }
      if (!enforceAudienceRestriction)
        return;
      if (this.Configuration == null || this.Configuration.AudienceRestriction.AllowedAudienceUris.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID1032")));
      bool flag = false;
      if (conditions != null && conditions.Conditions != null)
      {
        foreach (SamlCondition condition in (IEnumerable<SamlCondition>) conditions.Conditions)
        {
          if (condition is SamlAudienceRestrictionCondition restrictionCondition)
          {
            this._samlSecurityTokenRequirement.ValidateAudienceRestriction((IList<Uri>) this.Configuration.AudienceRestriction.AllowedAudienceUris, restrictionCondition.Audiences);
            flag = true;
          }
        }
      }
      if (!flag)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Microsoft.IdentityModel.Tokens.AudienceUriValidationFailedException(Microsoft.IdentityModel.SR.GetString("ID1035")));
    }

    public override ClaimsIdentityCollection ValidateToken(
      SecurityToken token)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      if (!(token is SamlSecurityToken samlSecurityToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID1033", (object) token.GetType().ToString()));
      if (samlSecurityToken.Assertion == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID1034"));
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Verbose, TraceCode.Diagnostics, Microsoft.IdentityModel.SR.GetString("TraceValidateToken"), (TraceRecord) new TokenTraceRecord(token), (Exception) null);
      Saml11Assertion assertion = samlSecurityToken.Assertion as Saml11Assertion;
      if (samlSecurityToken.Assertion.SigningToken == null && (assertion == null || assertion.IssuerToken == null))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenValidationException(Microsoft.IdentityModel.SR.GetString("ID4220")));
      this.ValidateConditions(samlSecurityToken.Assertion.Conditions, this._samlSecurityTokenRequirement.ShouldEnforceAudienceRestriction(this.Configuration.AudienceRestriction.AudienceMode, (SecurityToken) samlSecurityToken));
      X509SecurityToken x509SecurityToken = (X509SecurityToken) null;
      if (assertion != null && assertion.IssuerToken != null)
        x509SecurityToken = assertion.IssuerToken as X509SecurityToken;
      else if (samlSecurityToken.Assertion.SigningToken != null)
        x509SecurityToken = samlSecurityToken.Assertion.SigningToken as X509SecurityToken;
      if (x509SecurityToken != null)
      {
        try
        {
          this.CertificateValidator.Validate(x509SecurityToken.Certificate);
        }
        catch (SecurityTokenValidationException ex)
        {
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenValidationException(Microsoft.IdentityModel.SR.GetString("ID4257", (object) X509Util.GetCertificateId(x509SecurityToken.Certificate)), (Exception) ex));
        }
      }
      IClaimsIdentity claimsIdentity = this.CreateClaims(samlSecurityToken);
      if (this._samlSecurityTokenRequirement.MapToWindows)
      {
        WindowsClaimsIdentity fromUpn = WindowsClaimsIdentity.CreateFromUpn(this.FindUpn(claimsIdentity), "Federation", this._samlSecurityTokenRequirement.UseWindowsTokenService, this.Configuration.IssuerNameRegistry.GetWindowsIssuerName());
        fromUpn.Claims.CopyRange((IEnumerable<Claim>) claimsIdentity.Claims);
        claimsIdentity = (IClaimsIdentity) fromUpn;
      }
      if (this.Configuration.DetectReplayedTokens)
        this.DetectReplayedTokens((SecurityToken) samlSecurityToken);
      if (this.Configuration.SaveBootstrapTokens)
        claimsIdentity.BootstrapToken = token;
      return new ClaimsIdentityCollection((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
      {
        claimsIdentity
      });
    }

    protected virtual string FindUpn(IClaimsIdentity claimsIdentity) => WindowsMappingOperations.FindUpn(claimsIdentity);

    protected virtual IClaimsIdentity CreateClaims(
      SamlSecurityToken samlSecurityToken)
    {
      if (samlSecurityToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (samlSecurityToken));
      if (samlSecurityToken.Assertion == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (samlSecurityToken), Microsoft.IdentityModel.SR.GetString("ID1034"));
      IClaimsIdentity subject = (IClaimsIdentity) new ClaimsIdentity("Federation", this._samlSecurityTokenRequirement.NameClaimType, this._samlSecurityTokenRequirement.RoleClaimType);
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      if (this.Configuration.IssuerNameRegistry == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4277"));
      string issuerName;
      if (samlSecurityToken.Assertion is Saml11Assertion assertion && assertion.IssuerToken != null)
      {
        issuerName = this.Configuration.IssuerNameRegistry.GetIssuerName(assertion.IssuerToken, assertion.Issuer);
        if (string.IsNullOrEmpty(issuerName))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4175")));
      }
      else
      {
        issuerName = this.Configuration.IssuerNameRegistry.GetIssuerName(samlSecurityToken.Assertion.SigningToken, samlSecurityToken.Assertion.Issuer);
        if (string.IsNullOrEmpty(issuerName))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4175")));
      }
      this.ProcessStatement(samlSecurityToken.Assertion.Statements, subject, issuerName);
      return subject;
    }

    protected virtual string DenormalizeAuthenticationType(string normalizedAuthenticationType) => AuthenticationTypeMaps.Denormalize(normalizedAuthenticationType, AuthenticationTypeMaps.Saml11);

    protected virtual string NormalizeAuthenticationType(string saml11AuthenticationMethod) => AuthenticationTypeMaps.Normalize(saml11AuthenticationMethod, AuthenticationTypeMaps.Saml11);

    protected virtual void ProcessStatement(
      IList<SamlStatement> statements,
      IClaimsIdentity subject,
      string issuer)
    {
      if (statements == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (statements));
      Collection<SamlAuthenticationStatement> collection = new Collection<SamlAuthenticationStatement>();
      this.ValidateStatements(statements);
      foreach (SamlStatement statement in (IEnumerable<SamlStatement>) statements)
      {
        switch (statement)
        {
          case SamlAttributeStatement samlStatement:
            this.ProcessAttributeStatement(samlStatement, subject, issuer);
            continue;
          case SamlAuthenticationStatement authenticationStatement:
            collection.Add(authenticationStatement);
            continue;
          case SamlAuthorizationDecisionStatement samlStatement:
            this.ProcessAuthorizationDecisionStatement(samlStatement, subject, issuer);
            continue;
          default:
            continue;
        }
      }
      foreach (SamlAuthenticationStatement samlStatement in collection)
      {
        if (samlStatement != null)
          this.ProcessAuthenticationStatement(samlStatement, subject, issuer);
      }
    }

    protected virtual void ProcessAttributeStatement(
      SamlAttributeStatement samlStatement,
      IClaimsIdentity subject,
      string issuer)
    {
      if (samlStatement == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (samlStatement));
      if (subject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (subject));
      this.ProcessSamlSubject(samlStatement.SamlSubject, subject, issuer);
      foreach (SamlAttribute attribute in (IEnumerable<SamlAttribute>) samlStatement.Attributes)
      {
        string str;
        if (string.IsNullOrEmpty(attribute.Namespace))
        {
          str = attribute.Name;
        }
        else
        {
          if (StringComparer.Ordinal.Equals(attribute.Name, "NameIdentifier"))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID4094")));
          int num = attribute.Namespace.LastIndexOf('/');
          str = num == -1 || num != attribute.Namespace.Length - 1 ? attribute.Namespace + "/" + attribute.Name : attribute.Namespace + attribute.Name;
        }
        if (str == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor")
        {
          if (subject.Actor != null)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4034"));
          this.SetDelegateFromAttribute(attribute, subject, issuer);
        }
        else
        {
          for (int index = 0; index < attribute.AttributeValues.Count; ++index)
          {
            if (!StringComparer.Ordinal.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", str) || Saml11SecurityTokenHandler.GetClaim(subject, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier") == null)
            {
              string originalIssuer = issuer;
              if (attribute is Saml11Attribute saml11Attribute && saml11Attribute.OriginalIssuer != null)
                originalIssuer = saml11Attribute.OriginalIssuer;
              string valueType = "http://www.w3.org/2001/XMLSchema#string";
              if (saml11Attribute != null)
                valueType = saml11Attribute.AttributeValueXsiType;
              subject.Claims.Add(new Claim(str, attribute.AttributeValues[index], valueType, issuer, originalIssuer));
            }
          }
        }
      }
    }

    private static Claim GetClaim(IClaimsIdentity subject, string claimType)
    {
      foreach (Claim claim in subject.Claims)
      {
        if (StringComparer.Ordinal.Equals(claimType, claim.ClaimType))
          return claim;
      }
      return (Claim) null;
    }

    protected virtual void ProcessSamlSubject(
      SamlSubject samlSubject,
      IClaimsIdentity subject,
      string issuer)
    {
      if (samlSubject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (samlSubject));
      if (Saml11SecurityTokenHandler.GetClaim(subject, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier") != null || string.IsNullOrEmpty(samlSubject.Name))
        return;
      Claim claim = new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", samlSubject.Name, "http://www.w3.org/2001/XMLSchema#string", issuer);
      if (samlSubject.NameFormat != null)
        claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format"] = samlSubject.NameFormat;
      if (samlSubject.NameQualifier != null)
        claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier"] = samlSubject.NameQualifier;
      subject.Claims.Add(claim);
    }

    protected virtual void ProcessAuthenticationStatement(
      SamlAuthenticationStatement samlStatement,
      IClaimsIdentity subject,
      string issuer)
    {
      if (samlStatement == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (samlStatement));
      if (subject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (subject));
      this.ProcessSamlSubject(samlStatement.SamlSubject, subject, issuer);
      subject.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", this.NormalizeAuthenticationType(samlStatement.AuthenticationMethod), "http://www.w3.org/2001/XMLSchema#string", issuer));
      subject.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(samlStatement.AuthenticationInstant.ToUniversalTime(), DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime", issuer));
    }

    protected virtual void ProcessAuthorizationDecisionStatement(
      SamlAuthorizationDecisionStatement samlStatement,
      IClaimsIdentity subject,
      string issuer)
    {
    }

    protected virtual void SetDelegateFromAttribute(
      SamlAttribute attribute,
      IClaimsIdentity subject,
      string issuer)
    {
      if (subject == null || attribute == null || (attribute.AttributeValues == null || attribute.AttributeValues.Count < 1))
        return;
      Collection<Claim> collection = new Collection<Claim>();
      SamlAttribute attribute1 = (SamlAttribute) null;
      foreach (string attributeValue in (IEnumerable<string>) attribute.AttributeValues)
      {
        if (attributeValue != null && attributeValue.Length > 0)
        {
          using (XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(attributeValue), XmlDictionaryReaderQuotas.Max))
          {
            int content = (int) textReader.MoveToContent();
            textReader.ReadStartElement("Actor");
            while (textReader.IsStartElement("saml:Attribute"))
            {
              SamlAttribute samlAttribute = this.ReadAttribute((XmlReader) textReader);
              if (samlAttribute != null)
              {
                string claimType = string.IsNullOrEmpty(samlAttribute.Namespace) ? samlAttribute.Name : samlAttribute.Namespace + "/" + samlAttribute.Name;
                if (claimType == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor")
                {
                  attribute1 = attribute1 == null ? samlAttribute : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4034"));
                }
                else
                {
                  string valueType = "http://www.w3.org/2001/XMLSchema#string";
                  string originalIssuer = (string) null;
                  if (samlAttribute is Saml11Attribute saml11Attribute)
                  {
                    valueType = saml11Attribute.AttributeValueXsiType;
                    originalIssuer = saml11Attribute.OriginalIssuer;
                  }
                  for (int index = 0; index < samlAttribute.AttributeValues.Count; ++index)
                  {
                    Claim claim = !string.IsNullOrEmpty(originalIssuer) ? new Claim(claimType, samlAttribute.AttributeValues[index], valueType, issuer, originalIssuer) : new Claim(claimType, samlAttribute.AttributeValues[index], valueType, issuer);
                    collection.Add(claim);
                  }
                }
              }
            }
            textReader.ReadEndElement();
          }
        }
      }
      subject.Actor = (IClaimsIdentity) new ClaimsIdentity((IEnumerable<Claim>) collection, "Federation");
      this.SetDelegateFromAttribute(attribute1, subject.Actor, issuer);
    }

    public override bool CanReadToken(XmlReader reader) => reader != null && reader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:1.0:assertion");

    public override SecurityToken ReadToken(XmlReader reader)
    {
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      if (this.Configuration.IssuerTokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4275"));
      Saml11Assertion assertion = this.ReadAssertion(reader);
      SecurityToken token;
      this.TryResolveIssuerToken(assertion, this.Configuration.IssuerTokenResolver, out token);
      assertion.IssuerToken = token;
      return (SecurityToken) new SamlSecurityToken((SamlAssertion) assertion);
    }

    protected virtual SamlAction ReadAction(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      string ns = reader.IsStartElement("Action", "urn:oasis:names:tc:SAML:1.0:assertion") ? reader.GetAttribute("Namespace", (string) null) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4065", (object) "Action", (object) "urn:oasis:names:tc:SAML:1.0:assertion", (object) reader.LocalName, (object) reader.NamespaceURI)));
      int content1 = (int) reader.MoveToContent();
      string action = reader.ReadString();
      if (string.IsNullOrEmpty(action))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4073")));
      int content2 = (int) reader.MoveToContent();
      reader.ReadEndElement();
      return new SamlAction(action, ns);
    }

    protected virtual void WriteAction(XmlWriter writer, SamlAction action)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (action == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (action));
      writer.WriteStartElement("saml", "Action", "urn:oasis:names:tc:SAML:1.0:assertion");
      if (!string.IsNullOrEmpty(action.Namespace))
        writer.WriteAttributeString("Namespace", (string) null, action.Namespace);
      writer.WriteString(action.Action);
      writer.WriteEndElement();
    }

    protected virtual SamlAdvice ReadAdvice(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("Advice", "urn:oasis:names:tc:SAML:1.0:assertion"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4065", (object) "Advice", (object) "urn:oasis:names:tc:SAML:1.0:assertion", (object) reader.LocalName, (object) reader.NamespaceURI)));
      if (reader.IsEmptyElement)
      {
        int content = (int) reader.MoveToContent();
        reader.Read();
        return new SamlAdvice();
      }
      int content1 = (int) reader.MoveToContent();
      reader.Read();
      Collection<string> collection1 = new Collection<string>();
      Collection<SamlAssertion> collection2 = new Collection<SamlAssertion>();
      while (reader.IsStartElement())
      {
        if (reader.IsStartElement("AssertionIDReference", "urn:oasis:names:tc:SAML:1.0:assertion"))
        {
          collection1.Add(reader.ReadString());
          reader.ReadEndElement();
        }
        else if (reader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:1.0:assertion"))
        {
          SamlAssertion samlAssertion = (SamlAssertion) this.ReadAssertion(reader);
          collection2.Add(samlAssertion);
        }
        else
        {
          if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
            DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID8005", (object) reader.LocalName, (object) reader.NamespaceURI));
          reader.Skip();
        }
      }
      int content2 = (int) reader.MoveToContent();
      reader.ReadEndElement();
      return new SamlAdvice((IEnumerable<string>) collection1, (IEnumerable<SamlAssertion>) collection2);
    }

    protected virtual void WriteAdvice(XmlWriter writer, SamlAdvice advice)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (advice == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (advice));
      writer.WriteStartElement("saml", "Advice", "urn:oasis:names:tc:SAML:1.0:assertion");
      if (advice.AssertionIdReferences.Count > 0)
      {
        foreach (string assertionIdReference in (IEnumerable<string>) advice.AssertionIdReferences)
        {
          if (string.IsNullOrEmpty(assertionIdReference))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4079")));
          writer.WriteElementString("saml", "AssertionIDReference", "urn:oasis:names:tc:SAML:1.0:assertion", assertionIdReference);
        }
      }
      if (advice.Assertions.Count > 0)
      {
        foreach (SamlAssertion assertion in (IEnumerable<SamlAssertion>) advice.Assertions)
          this.WriteAssertion(writer, assertion);
      }
      writer.WriteEndElement();
    }

    protected virtual Saml11Assertion ReadAssertion(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      if (this.Configuration.IssuerTokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4275"));
      Saml11Assertion saml11Assertion = new Saml11Assertion();
      EnvelopedSignatureReader reader1 = new EnvelopedSignatureReader(reader, (SecurityTokenSerializer) new Saml11SecurityTokenHandler.WrappedSerializer(this, (SamlAssertion) saml11Assertion), this.Configuration.IssuerTokenResolver, false, true, false);
      string s = reader1.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:1.0:assertion") ? reader1.GetAttribute("MajorVersion", (string) null) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4065", (object) "Assertion", (object) "urn:oasis:names:tc:SAML:1.0:assertion", (object) reader1.LocalName, (object) reader1.NamespaceURI)));
      int num1 = !string.IsNullOrEmpty(s) ? XmlConvert.ToInt32(s) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4075", (object) "MajorVersion")));
      string attribute1 = reader1.GetAttribute("MinorVersion", (string) null);
      int num2 = !string.IsNullOrEmpty(attribute1) ? XmlConvert.ToInt32(attribute1) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4075", (object) "MinorVersion")));
      if (num1 != 1 || num2 != 1)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4076", (object) num1, (object) num2, (object) 1, (object) 1)));
      string attribute2 = reader1.GetAttribute("AssertionID", (string) null);
      if (string.IsNullOrEmpty(attribute2))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4075", (object) "AssertionID")));
      saml11Assertion.AssertionId = XmlUtil.IsValidXmlIDValue(attribute2) ? attribute2 : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4077", (object) attribute2)));
      string attribute3 = reader1.GetAttribute("Issuer", (string) null);
      saml11Assertion.Issuer = !string.IsNullOrEmpty(attribute3) ? attribute3 : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4075", (object) "Issuer")));
      string attribute4 = reader1.GetAttribute("IssueInstant", (string) null);
      if (!string.IsNullOrEmpty(attribute4))
        saml11Assertion.IssueInstant = DateTime.ParseExact(attribute4, DateTimeFormats.Accepted, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();
      int content1 = (int) reader1.MoveToContent();
      reader1.Read();
      if (reader1.IsStartElement("Conditions", "urn:oasis:names:tc:SAML:1.0:assertion"))
        saml11Assertion.Conditions = this.ReadConditions((XmlReader) reader1);
      if (reader1.IsStartElement("Advice", "urn:oasis:names:tc:SAML:1.0:assertion"))
        saml11Assertion.Advice = this.ReadAdvice((XmlReader) reader1);
      while (reader1.IsStartElement())
        saml11Assertion.Statements.Add(this.ReadStatement((XmlReader) reader1));
      if (saml11Assertion.Statements.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4078")));
      int content2 = (int) reader1.MoveToContent();
      reader1.ReadEndElement();
      saml11Assertion.SigningCredentials = reader1.SigningCredentials;
      saml11Assertion.CaptureSourceData(reader1);
      return saml11Assertion;
    }

    protected virtual void WriteAssertion(XmlWriter writer, SamlAssertion assertion)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (assertion == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (assertion));
      if (assertion is Saml11Assertion saml11Assertion && saml11Assertion.CanWriteSourceData)
      {
        saml11Assertion.WriteSourceData(writer);
      }
      else
      {
        if (assertion.SigningCredentials != null)
          writer = (XmlWriter) new EnvelopedSignatureWriter(writer, assertion.SigningCredentials, assertion.AssertionId, (SecurityTokenSerializer) new Saml11SecurityTokenHandler.WrappedSerializer(this, assertion));
        writer.WriteStartElement("saml", "Assertion", "urn:oasis:names:tc:SAML:1.0:assertion");
        writer.WriteAttributeString("MajorVersion", (string) null, Convert.ToString(1, (IFormatProvider) CultureInfo.InvariantCulture));
        writer.WriteAttributeString("MinorVersion", (string) null, Convert.ToString(1, (IFormatProvider) CultureInfo.InvariantCulture));
        writer.WriteAttributeString("AssertionID", (string) null, assertion.AssertionId);
        writer.WriteAttributeString("Issuer", (string) null, assertion.Issuer);
        writer.WriteAttributeString("IssueInstant", (string) null, assertion.IssueInstant.ToUniversalTime().ToString(DateTimeFormats.Generated, (IFormatProvider) CultureInfo.InvariantCulture));
        if (assertion.Conditions != null)
          this.WriteConditions(writer, assertion.Conditions);
        if (assertion.Advice != null)
          this.WriteAdvice(writer, assertion.Advice);
        for (int index = 0; index < assertion.Statements.Count; ++index)
          this.WriteStatement(writer, assertion.Statements[index]);
        writer.WriteEndElement();
      }
    }

    protected virtual SamlConditions ReadConditions(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      SamlConditions samlConditions = new SamlConditions();
      string attribute1 = reader.GetAttribute("NotBefore", (string) null);
      if (!string.IsNullOrEmpty(attribute1))
        samlConditions.NotBefore = DateTime.ParseExact(attribute1, DateTimeFormats.Accepted, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();
      string attribute2 = reader.GetAttribute("NotOnOrAfter", (string) null);
      if (!string.IsNullOrEmpty(attribute2))
        samlConditions.NotOnOrAfter = DateTime.ParseExact(attribute2, DateTimeFormats.Accepted, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();
      if (reader.IsEmptyElement)
      {
        int content = (int) reader.MoveToContent();
        reader.Read();
        return samlConditions;
      }
      reader.ReadStartElement();
      while (reader.IsStartElement())
        samlConditions.Conditions.Add(this.ReadCondition(reader));
      reader.ReadEndElement();
      return samlConditions;
    }

    protected virtual void WriteConditions(XmlWriter writer, SamlConditions conditions)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (conditions == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (conditions));
      writer.WriteStartElement("saml", "Conditions", "urn:oasis:names:tc:SAML:1.0:assertion");
      if (conditions.NotBefore != DateTimeUtil.GetMinValue(DateTimeKind.Utc) && conditions.NotBefore != Saml11SecurityTokenHandler.WCFMinValue)
        writer.WriteAttributeString("NotBefore", (string) null, conditions.NotBefore.ToUniversalTime().ToString(DateTimeFormats.Generated, (IFormatProvider) DateTimeFormatInfo.InvariantInfo));
      if (conditions.NotOnOrAfter != DateTimeUtil.GetMaxValue(DateTimeKind.Utc) && conditions.NotOnOrAfter != Saml11SecurityTokenHandler.WCFMaxValue)
        writer.WriteAttributeString("NotOnOrAfter", (string) null, conditions.NotOnOrAfter.ToUniversalTime().ToString(DateTimeFormats.Generated, (IFormatProvider) DateTimeFormatInfo.InvariantInfo));
      for (int index = 0; index < conditions.Conditions.Count; ++index)
        this.WriteCondition(writer, conditions.Conditions[index]);
      writer.WriteEndElement();
    }

    protected virtual SamlCondition ReadCondition(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (reader.IsStartElement("AudienceRestrictionCondition", "urn:oasis:names:tc:SAML:1.0:assertion"))
        return (SamlCondition) this.ReadAudienceRestrictionCondition(reader);
      return reader.IsStartElement("DoNotCacheCondition", "urn:oasis:names:tc:SAML:1.0:assertion") ? (SamlCondition) this.ReadDoNotCacheCondition(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4080", (object) reader.LocalName, (object) reader.NamespaceURI)));
    }

    protected virtual void WriteCondition(XmlWriter writer, SamlCondition condition)
    {
      switch (condition)
      {
        case null:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (condition));
        case SamlAudienceRestrictionCondition condition1:
          this.WriteAudienceRestrictionCondition(writer, condition1);
          break;
        case SamlDoNotCacheCondition condition1:
          this.WriteDoNotCacheCondition(writer, condition1);
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4081", (object) condition.GetType())));
      }
    }

    protected virtual SamlAudienceRestrictionCondition ReadAudienceRestrictionCondition(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("AudienceRestrictionCondition", "urn:oasis:names:tc:SAML:1.0:assertion"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4082", (object) "AudienceRestrictionCondition", (object) "urn:oasis:names:tc:SAML:1.0:assertion", (object) reader.LocalName, (object) reader.NamespaceURI)));
      reader.ReadStartElement();
      SamlAudienceRestrictionCondition restrictionCondition = new SamlAudienceRestrictionCondition();
      while (reader.IsStartElement())
      {
        string uriString = reader.IsStartElement("Audience", "urn:oasis:names:tc:SAML:1.0:assertion") ? reader.ReadString() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4082", (object) "Audience", (object) "urn:oasis:names:tc:SAML:1.0:assertion", (object) reader.LocalName, (object) reader.NamespaceURI)));
        if (string.IsNullOrEmpty(uriString))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4083")));
        restrictionCondition.Audiences.Add(new Uri(uriString, UriKind.RelativeOrAbsolute));
        int content = (int) reader.MoveToContent();
        reader.ReadEndElement();
      }
      if (restrictionCondition.Audiences.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4084")));
      int content1 = (int) reader.MoveToContent();
      reader.ReadEndElement();
      return restrictionCondition;
    }

    protected virtual void WriteAudienceRestrictionCondition(
      XmlWriter writer,
      SamlAudienceRestrictionCondition condition)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (condition == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (condition));
      if (condition.Audiences == null || condition.Audiences.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4269")));
      writer.WriteStartElement("saml", "AudienceRestrictionCondition", "urn:oasis:names:tc:SAML:1.0:assertion");
      for (int index = 0; index < condition.Audiences.Count; ++index)
        writer.WriteElementString("Audience", "urn:oasis:names:tc:SAML:1.0:assertion", condition.Audiences[index].OriginalString);
      writer.WriteEndElement();
    }

    protected virtual SamlDoNotCacheCondition ReadDoNotCacheCondition(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("DoNotCacheCondition", "urn:oasis:names:tc:SAML:1.0:assertion"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4082", (object) "DoNotCacheCondition", (object) "urn:oasis:names:tc:SAML:1.0:assertion", (object) reader.LocalName, (object) reader.NamespaceURI)));
      SamlDoNotCacheCondition notCacheCondition = new SamlDoNotCacheCondition();
      if (reader.IsEmptyElement)
      {
        int content = (int) reader.MoveToContent();
        reader.Read();
        return notCacheCondition;
      }
      int content1 = (int) reader.MoveToContent();
      reader.ReadStartElement();
      reader.ReadEndElement();
      return notCacheCondition;
    }

    protected virtual void WriteDoNotCacheCondition(
      XmlWriter writer,
      SamlDoNotCacheCondition condition)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (condition == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (condition));
      writer.WriteStartElement("saml", "DoNotCacheCondition", "urn:oasis:names:tc:SAML:1.0:assertion");
      writer.WriteEndElement();
    }

    protected virtual SamlStatement ReadStatement(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (reader.IsStartElement("AuthenticationStatement", "urn:oasis:names:tc:SAML:1.0:assertion"))
        return (SamlStatement) this.ReadAuthenticationStatement(reader);
      if (reader.IsStartElement("AttributeStatement", "urn:oasis:names:tc:SAML:1.0:assertion"))
        return (SamlStatement) this.ReadAttributeStatement(reader);
      return reader.IsStartElement("AuthorizationDecisionStatement", "urn:oasis:names:tc:SAML:1.0:assertion") ? (SamlStatement) this.ReadAuthorizationDecisionStatement(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4085", (object) reader.LocalName, (object) reader.NamespaceURI)));
    }

    protected virtual void WriteStatement(XmlWriter writer, SamlStatement statement)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      switch (statement)
      {
        case null:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (statement));
        case SamlAuthenticationStatement statement1:
          this.WriteAuthenticationStatement(writer, statement1);
          break;
        case SamlAuthorizationDecisionStatement statement1:
          this.WriteAuthorizationDecisionStatement(writer, statement1);
          break;
        case SamlAttributeStatement statement1:
          this.WriteAttributeStatement(writer, statement1);
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4086", (object) statement.GetType())));
      }
    }

    protected virtual SamlSubject ReadSubject(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("Subject", "urn:oasis:names:tc:SAML:1.0:assertion"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4082", (object) "Subject", (object) "urn:oasis:names:tc:SAML:1.0:assertion", (object) reader.LocalName, (object) reader.NamespaceURI)));
      SamlSubject samlSubject = new SamlSubject();
      reader.ReadStartElement("Subject", "urn:oasis:names:tc:SAML:1.0:assertion");
      if (reader.IsStartElement("NameIdentifier", "urn:oasis:names:tc:SAML:1.0:assertion"))
      {
        samlSubject.NameFormat = reader.GetAttribute("Format", (string) null);
        samlSubject.NameQualifier = reader.GetAttribute("NameQualifier", (string) null);
        int content = (int) reader.MoveToContent();
        samlSubject.Name = reader.ReadElementString();
        if (string.IsNullOrEmpty(samlSubject.Name))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4087")));
      }
      if (reader.IsStartElement("SubjectConfirmation", "urn:oasis:names:tc:SAML:1.0:assertion"))
      {
        reader.ReadStartElement();
        while (reader.IsStartElement("ConfirmationMethod", "urn:oasis:names:tc:SAML:1.0:assertion"))
        {
          string str = reader.ReadElementString();
          if (string.IsNullOrEmpty(str))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4088")));
          samlSubject.ConfirmationMethods.Add(str);
        }
        if (samlSubject.ConfirmationMethods.Count == 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4088")));
        if (reader.IsStartElement("SubjectConfirmationData", "urn:oasis:names:tc:SAML:1.0:assertion"))
          samlSubject.SubjectConfirmationData = reader.ReadElementString();
        if (reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
        {
          samlSubject.KeyIdentifier = this.ReadSubjectKeyInfo(reader);
          SecurityKey securityKey = this.ResolveSubjectKeyIdentifier(samlSubject.KeyIdentifier);
          samlSubject.Crypto = securityKey == null ? (SecurityKey) new Microsoft.IdentityModel.Tokens.SecurityKeyElement(samlSubject.KeyIdentifier, this.Configuration.ServiceTokenResolver) : securityKey;
        }
        if (samlSubject.ConfirmationMethods.Count == 0 && string.IsNullOrEmpty(samlSubject.Name))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4089")));
        int content = (int) reader.MoveToContent();
        reader.ReadEndElement();
      }
      int content1 = (int) reader.MoveToContent();
      reader.ReadEndElement();
      return samlSubject;
    }

    protected virtual void WriteSubject(XmlWriter writer, SamlSubject subject)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (subject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (subject));
      writer.WriteStartElement("saml", "Subject", "urn:oasis:names:tc:SAML:1.0:assertion");
      if (!string.IsNullOrEmpty(subject.Name))
      {
        writer.WriteStartElement("saml", "NameIdentifier", "urn:oasis:names:tc:SAML:1.0:assertion");
        if (!string.IsNullOrEmpty(subject.NameFormat))
          writer.WriteAttributeString("Format", (string) null, subject.NameFormat);
        if (subject.NameQualifier != null)
          writer.WriteAttributeString("NameQualifier", (string) null, subject.NameQualifier);
        writer.WriteString(subject.Name);
        writer.WriteEndElement();
      }
      if (subject.ConfirmationMethods.Count > 0)
      {
        writer.WriteStartElement("saml", "SubjectConfirmation", "urn:oasis:names:tc:SAML:1.0:assertion");
        foreach (string confirmationMethod in (IEnumerable<string>) subject.ConfirmationMethods)
          writer.WriteElementString("ConfirmationMethod", "urn:oasis:names:tc:SAML:1.0:assertion", confirmationMethod);
        if (!string.IsNullOrEmpty(subject.SubjectConfirmationData))
          writer.WriteElementString("SubjectConfirmationData", "urn:oasis:names:tc:SAML:1.0:assertion", subject.SubjectConfirmationData);
        if (subject.KeyIdentifier != null)
          this.WriteSubjectKeyInfo(writer, subject.KeyIdentifier);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }

    protected virtual SecurityKeyIdentifier ReadSubjectKeyInfo(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      return this.KeyInfoSerializer.CanReadKeyIdentifier(reader) ? this.KeyInfoSerializer.ReadKeyIdentifier(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4090")));
    }

    protected virtual void WriteSubjectKeyInfo(XmlWriter writer, SecurityKeyIdentifier subjectSki)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (subjectSki == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (subjectSki));
      if (this.KeyInfoSerializer.CanWriteKeyIdentifier(subjectSki))
        this.KeyInfoSerializer.WriteKeyIdentifier(writer, subjectSki);
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (subjectSki), Microsoft.IdentityModel.SR.GetString("ID4091", (object) subjectSki.GetType()));
    }

    protected virtual SamlAttributeStatement ReadAttributeStatement(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("AttributeStatement", "urn:oasis:names:tc:SAML:1.0:assertion"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4082", (object) "AttributeStatement", (object) "urn:oasis:names:tc:SAML:1.0:assertion", (object) reader.LocalName, (object) reader.NamespaceURI)));
      reader.ReadStartElement();
      SamlAttributeStatement attributeStatement = new SamlAttributeStatement();
      attributeStatement.SamlSubject = reader.IsStartElement("Subject", "urn:oasis:names:tc:SAML:1.0:assertion") ? this.ReadSubject(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4092")));
      while (reader.IsStartElement() && reader.IsStartElement("Attribute", "urn:oasis:names:tc:SAML:1.0:assertion"))
        attributeStatement.Attributes.Add(this.ReadAttribute(reader));
      if (attributeStatement.Attributes.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4093")));
      int content = (int) reader.MoveToContent();
      reader.ReadEndElement();
      return attributeStatement;
    }

    protected virtual void WriteAttributeStatement(
      XmlWriter writer,
      SamlAttributeStatement statement)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (statement == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (statement));
      writer.WriteStartElement("saml", "AttributeStatement", "urn:oasis:names:tc:SAML:1.0:assertion");
      this.WriteSubject(writer, statement.SamlSubject);
      for (int index = 0; index < statement.Attributes.Count; ++index)
        this.WriteAttribute(writer, statement.Attributes[index]);
      writer.WriteEndElement();
    }

    protected virtual SamlAttribute ReadAttribute(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      Saml11Attribute saml11Attribute = new Saml11Attribute();
      saml11Attribute.Name = reader.GetAttribute("AttributeName", (string) null);
      if (string.IsNullOrEmpty(saml11Attribute.Name))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4094")));
      saml11Attribute.Namespace = reader.GetAttribute("AttributeNamespace", (string) null);
      if (string.IsNullOrEmpty(saml11Attribute.Namespace))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4095")));
      string str1 = reader.GetAttribute("OriginalIssuer", "http://schemas.xmlsoap.org/ws/2009/09/identity/claims") ?? reader.GetAttribute("OriginalIssuer", "http://schemas.microsoft.com/ws/2008/06/identity");
      saml11Attribute.OriginalIssuer = !(str1 == string.Empty) ? str1 : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4252")));
      int content1 = (int) reader.MoveToContent();
      reader.Read();
      while (reader.IsStartElement("AttributeValue", "urn:oasis:names:tc:SAML:1.0:assertion"))
      {
        string str2 = (string) null;
        string str3 = (string) null;
        string attribute = reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
        if (!string.IsNullOrEmpty(attribute))
        {
          if (attribute.IndexOf(":", StringComparison.Ordinal) == -1)
          {
            str2 = reader.LookupNamespace(string.Empty);
            str3 = attribute;
          }
          else if (attribute.IndexOf(":", StringComparison.Ordinal) > 0 && attribute.IndexOf(":", StringComparison.Ordinal) < attribute.Length - 1)
          {
            string prefix = attribute.Substring(0, attribute.IndexOf(":", StringComparison.Ordinal));
            str2 = reader.LookupNamespace(prefix);
            str3 = attribute.Substring(attribute.IndexOf(":", StringComparison.Ordinal) + 1);
          }
        }
        if (str2 != null && str3 != null)
          saml11Attribute.AttributeValueXsiType = str2 + "#" + str3;
        string str4 = this.ReadAttributeValue(reader, (SamlAttribute) saml11Attribute);
        saml11Attribute.AttributeValues.Add(str4);
      }
      if (saml11Attribute.AttributeValues.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4212")));
      int content2 = (int) reader.MoveToContent();
      reader.ReadEndElement();
      return (SamlAttribute) saml11Attribute;
    }

    protected virtual string ReadAttributeValue(XmlReader reader, SamlAttribute attribute) => reader != null ? reader.ReadElementString() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));

    protected virtual void WriteAttribute(XmlWriter writer, SamlAttribute attribute)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (attribute == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (attribute));
      writer.WriteStartElement("saml", "Attribute", "urn:oasis:names:tc:SAML:1.0:assertion");
      writer.WriteAttributeString("AttributeName", (string) null, attribute.Name);
      writer.WriteAttributeString("AttributeNamespace", (string) null, attribute.Namespace);
      if (attribute is Saml11Attribute saml11Attribute && saml11Attribute.OriginalIssuer != null)
        writer.WriteAttributeString("OriginalIssuer", "http://schemas.xmlsoap.org/ws/2009/09/identity/claims", saml11Attribute.OriginalIssuer);
      string str1 = (string) null;
      string str2 = (string) null;
      if (saml11Attribute != null && !StringComparer.Ordinal.Equals(saml11Attribute.AttributeValueXsiType, "http://www.w3.org/2001/XMLSchema#string"))
      {
        int length = saml11Attribute.AttributeValueXsiType.IndexOf('#');
        str1 = saml11Attribute.AttributeValueXsiType.Substring(0, length);
        str2 = saml11Attribute.AttributeValueXsiType.Substring(length + 1);
      }
      for (int index = 0; index < attribute.AttributeValues.Count; ++index)
      {
        if (attribute.AttributeValues[index] == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4096")));
        writer.WriteStartElement("saml", "AttributeValue", "urn:oasis:names:tc:SAML:1.0:assertion");
        if (str1 != null && str2 != null)
        {
          writer.WriteAttributeString("xmlns", "tn", (string) null, str1);
          writer.WriteAttributeString("type", "http://www.w3.org/2001/XMLSchema-instance", "tn:" + str2);
        }
        this.WriteAttributeValue(writer, attribute.AttributeValues[index], attribute);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }

    protected virtual void WriteAttributeValue(
      XmlWriter writer,
      string value,
      SamlAttribute attribute)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      writer.WriteString(value);
    }

    protected virtual SamlAuthenticationStatement ReadAuthenticationStatement(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("AuthenticationStatement", "urn:oasis:names:tc:SAML:1.0:assertion"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4082", (object) "AuthenticationStatement", (object) "urn:oasis:names:tc:SAML:1.0:assertion", (object) reader.LocalName, (object) reader.NamespaceURI)));
      SamlAuthenticationStatement authenticationStatement = new SamlAuthenticationStatement();
      string attribute = reader.GetAttribute("AuthenticationInstant", (string) null);
      if (string.IsNullOrEmpty(attribute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4097")));
      authenticationStatement.AuthenticationInstant = DateTime.ParseExact(attribute, DateTimeFormats.Accepted, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();
      authenticationStatement.AuthenticationMethod = reader.GetAttribute("AuthenticationMethod", (string) null);
      if (string.IsNullOrEmpty(authenticationStatement.AuthenticationMethod))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4098")));
      int content1 = (int) reader.MoveToContent();
      reader.Read();
      authenticationStatement.SamlSubject = reader.IsStartElement("Subject", "urn:oasis:names:tc:SAML:1.0:assertion") ? this.ReadSubject(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4099")));
      if (reader.IsStartElement("SubjectLocality", "urn:oasis:names:tc:SAML:1.0:assertion"))
      {
        authenticationStatement.DnsAddress = reader.GetAttribute("DNSAddress", (string) null);
        authenticationStatement.IPAddress = reader.GetAttribute("IPAddress", (string) null);
        if (reader.IsEmptyElement)
        {
          int content2 = (int) reader.MoveToContent();
          reader.Read();
        }
        else
        {
          int content2 = (int) reader.MoveToContent();
          reader.Read();
          reader.ReadEndElement();
        }
      }
      while (reader.IsStartElement())
      {
        if (reader.IsStartElement("AuthorityBinding", "urn:oasis:names:tc:SAML:1.0:assertion"))
          authenticationStatement.AuthorityBindings.Add(this.ReadAuthorityBinding(reader));
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4082", (object) "AuthorityBinding", (object) "urn:oasis:names:tc:SAML:1.0:assertion", (object) reader.LocalName, (object) reader.NamespaceURI)));
      }
      int content3 = (int) reader.MoveToContent();
      reader.ReadEndElement();
      return authenticationStatement;
    }

    protected virtual void WriteAuthenticationStatement(
      XmlWriter writer,
      SamlAuthenticationStatement statement)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (statement == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (statement));
      writer.WriteStartElement("saml", "AuthenticationStatement", "urn:oasis:names:tc:SAML:1.0:assertion");
      writer.WriteAttributeString("AuthenticationMethod", (string) null, statement.AuthenticationMethod);
      writer.WriteAttributeString("AuthenticationInstant", (string) null, XmlConvert.ToString(statement.AuthenticationInstant.ToUniversalTime(), DateTimeFormats.Generated));
      this.WriteSubject(writer, statement.SamlSubject);
      if (statement.IPAddress != null || statement.DnsAddress != null)
      {
        writer.WriteStartElement("saml", "SubjectLocality", "urn:oasis:names:tc:SAML:1.0:assertion");
        if (statement.IPAddress != null)
          writer.WriteAttributeString("IPAddress", (string) null, statement.IPAddress);
        if (statement.DnsAddress != null)
          writer.WriteAttributeString("DNSAddress", (string) null, statement.DnsAddress);
        writer.WriteEndElement();
      }
      for (int index = 0; index < statement.AuthorityBindings.Count; ++index)
        this.WriteAuthorityBinding(writer, statement.AuthorityBindings[index]);
      writer.WriteEndElement();
    }

    protected virtual SamlAuthorityBinding ReadAuthorityBinding(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      SamlAuthorityBinding authorityBinding = new SamlAuthorityBinding();
      string attribute = reader.GetAttribute("AuthorityKind", (string) null);
      string[] strArray = !string.IsNullOrEmpty(attribute) ? attribute.Split(':') : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4200")));
      if (strArray.Length > 2)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4201", (object) attribute)));
      string empty;
      string name;
      if (strArray.Length == 2)
      {
        empty = strArray[0];
        name = strArray[1];
      }
      else
      {
        empty = string.Empty;
        name = strArray[0];
      }
      string ns = reader.LookupNamespace(empty);
      authorityBinding.AuthorityKind = new XmlQualifiedName(name, ns);
      authorityBinding.Binding = reader.GetAttribute("Binding", (string) null);
      if (string.IsNullOrEmpty(authorityBinding.Binding))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4202")));
      authorityBinding.Location = reader.GetAttribute("Location", (string) null);
      if (string.IsNullOrEmpty(authorityBinding.Location))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4203")));
      if (reader.IsEmptyElement)
      {
        int content = (int) reader.MoveToContent();
        reader.Read();
      }
      else
      {
        int content = (int) reader.MoveToContent();
        reader.Read();
        reader.ReadEndElement();
      }
      return authorityBinding;
    }

    protected virtual void WriteAuthorityBinding(
      XmlWriter writer,
      SamlAuthorityBinding authorityBinding)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (authorityBinding == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("statement");
      writer.WriteStartElement("saml", "AuthorityBinding", "urn:oasis:names:tc:SAML:1.0:assertion");
      string str = (string) null;
      if (!string.IsNullOrEmpty(authorityBinding.AuthorityKind.Namespace))
      {
        writer.WriteAttributeString(string.Empty, "xmlns", (string) null, authorityBinding.AuthorityKind.Namespace);
        str = writer.LookupPrefix(authorityBinding.AuthorityKind.Namespace);
      }
      writer.WriteStartAttribute("AuthorityKind", (string) null);
      if (string.IsNullOrEmpty(str))
        writer.WriteString(authorityBinding.AuthorityKind.Name);
      else
        writer.WriteString(str + ":" + authorityBinding.AuthorityKind.Name);
      writer.WriteEndAttribute();
      writer.WriteAttributeString("Location", (string) null, authorityBinding.Location);
      writer.WriteAttributeString("Binding", (string) null, authorityBinding.Binding);
      writer.WriteEndElement();
    }

    public override bool CanWriteToken => true;

    public override void WriteToken(XmlWriter writer, SecurityToken token)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (!(token is SamlSecurityToken samlSecurityToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4217", (object) token.GetType(), (object) typeof (SamlSecurityToken))));
      this.WriteAssertion(writer, samlSecurityToken.Assertion);
    }

    protected virtual SamlAuthorizationDecisionStatement ReadAuthorizationDecisionStatement(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("AuthorizationDecisionStatement", "urn:oasis:names:tc:SAML:1.0:assertion"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4082", (object) "AuthorizationDecisionStatement", (object) "urn:oasis:names:tc:SAML:1.0:assertion", (object) reader.LocalName, (object) reader.NamespaceURI)));
      SamlAuthorizationDecisionStatement decisionStatement = new SamlAuthorizationDecisionStatement();
      decisionStatement.Resource = reader.GetAttribute("Resource", (string) null);
      if (string.IsNullOrEmpty(decisionStatement.Resource))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4205")));
      string attribute = reader.GetAttribute("Decision", (string) null);
      if (string.IsNullOrEmpty(attribute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4204")));
      decisionStatement.AccessDecision = !attribute.Equals(SamlAccessDecision.Deny.ToString(), StringComparison.OrdinalIgnoreCase) ? (!attribute.Equals(SamlAccessDecision.Permit.ToString(), StringComparison.OrdinalIgnoreCase) ? SamlAccessDecision.Indeterminate : SamlAccessDecision.Permit) : SamlAccessDecision.Deny;
      int content1 = (int) reader.MoveToContent();
      reader.Read();
      decisionStatement.SamlSubject = reader.IsStartElement("Subject", "urn:oasis:names:tc:SAML:1.0:assertion") ? this.ReadSubject(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4206")));
      while (reader.IsStartElement())
      {
        if (reader.IsStartElement("Action", "urn:oasis:names:tc:SAML:1.0:assertion"))
          decisionStatement.SamlActions.Add(this.ReadAction(reader));
        else if (reader.IsStartElement("Evidence", "urn:oasis:names:tc:SAML:1.0:assertion"))
          decisionStatement.Evidence = decisionStatement.Evidence == null ? this.ReadEvidence(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4207")));
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4208", (object) reader.LocalName, (object) reader.NamespaceURI)));
      }
      if (decisionStatement.SamlActions.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4209")));
      int content2 = (int) reader.MoveToContent();
      reader.ReadEndElement();
      return decisionStatement;
    }

    protected virtual void WriteAuthorizationDecisionStatement(
      XmlWriter writer,
      SamlAuthorizationDecisionStatement statement)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (statement == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (statement));
      writer.WriteStartElement("saml", "AuthorizationDecisionStatement", "urn:oasis:names:tc:SAML:1.0:assertion");
      writer.WriteAttributeString("Decision", (string) null, statement.AccessDecision.ToString());
      writer.WriteAttributeString("Resource", (string) null, statement.Resource);
      this.WriteSubject(writer, statement.SamlSubject);
      foreach (SamlAction samlAction in (IEnumerable<SamlAction>) statement.SamlActions)
        this.WriteAction(writer, samlAction);
      if (statement.Evidence != null)
        this.WriteEvidence(writer, statement.Evidence);
      writer.WriteEndElement();
    }

    protected virtual SamlEvidence ReadEvidence(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("Evidence", "urn:oasis:names:tc:SAML:1.0:assertion"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4082", (object) "Evidence", (object) "urn:oasis:names:tc:SAML:1.0:assertion", (object) reader.LocalName, (object) reader.NamespaceURI)));
      SamlEvidence samlEvidence = new SamlEvidence();
      reader.ReadStartElement();
      while (reader.IsStartElement())
      {
        if (reader.IsStartElement("AssertionIDReference", "urn:oasis:names:tc:SAML:1.0:assertion"))
          samlEvidence.AssertionIdReferences.Add(reader.ReadElementString());
        else if (reader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:1.0:assertion"))
          samlEvidence.Assertions.Add((SamlAssertion) this.ReadAssertion(reader));
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4210", (object) reader.LocalName, (object) reader.NamespaceURI)));
      }
      if (samlEvidence.AssertionIdReferences.Count == 0 && samlEvidence.Assertions.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4211")));
      int content = (int) reader.MoveToContent();
      reader.ReadEndElement();
      return samlEvidence;
    }

    protected virtual void WriteEvidence(XmlWriter writer, SamlEvidence evidence)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (evidence == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (evidence));
      writer.WriteStartElement("saml", "Evidence", "urn:oasis:names:tc:SAML:1.0:assertion");
      for (int index = 0; index < evidence.AssertionIdReferences.Count; ++index)
        writer.WriteElementString("saml", "AssertionIDReference", "urn:oasis:names:tc:SAML:1.0:assertion", evidence.AssertionIdReferences[index]);
      for (int index = 0; index < evidence.Assertions.Count; ++index)
        this.WriteAssertion(writer, evidence.Assertions[index]);
      writer.WriteEndElement();
    }

    protected virtual SecurityKey ResolveSubjectKeyIdentifier(
      SecurityKeyIdentifier subjectKeyIdentifier)
    {
      if (subjectKeyIdentifier == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (subjectKeyIdentifier));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      if (this.Configuration.ServiceTokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4276"));
      SecurityKey key = (SecurityKey) null;
      foreach (SecurityKeyIdentifierClause keyIdentifierClause in subjectKeyIdentifier)
      {
        if (this.Configuration.ServiceTokenResolver.TryResolveSecurityKey(keyIdentifierClause, out key))
          return key;
      }
      return subjectKeyIdentifier.CanCreateKey ? subjectKeyIdentifier.CreateKey() : (SecurityKey) null;
    }

    protected virtual SecurityToken ResolveIssuerToken(
      Saml11Assertion assertion,
      SecurityTokenResolver issuerResolver)
    {
      if (assertion == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (assertion));
      if (issuerResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (issuerResolver));
      SecurityToken token;
      if (this.TryResolveIssuerToken(assertion, issuerResolver, out token))
        return token;
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4220")));
    }

    protected virtual bool TryResolveIssuerToken(
      Saml11Assertion assertion,
      SecurityTokenResolver issuerResolver,
      out SecurityToken token)
    {
      if (assertion == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (assertion));
      if (assertion.SigningCredentials != null && assertion.SigningCredentials.SigningKeyIdentifier != null && issuerResolver != null)
      {
        SecurityKeyIdentifier signingKeyIdentifier = assertion.SigningCredentials.SigningKeyIdentifier;
        if (signingKeyIdentifier.Count < 2)
          return issuerResolver.TryResolveToken(signingKeyIdentifier, out token);
        return issuerResolver.TryResolveToken(new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
        {
          signingKeyIdentifier[0]
        }), out token);
      }
      token = (SecurityToken) null;
      return false;
    }

    protected virtual SecurityKeyIdentifier ReadSigningKeyInfo(
      XmlReader reader,
      SamlAssertion assertion)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      SecurityKeyIdentifier securityKeyIdentifier;
      if (this.KeyInfoSerializer.CanReadKeyIdentifier(reader))
      {
        securityKeyIdentifier = this.KeyInfoSerializer.ReadKeyIdentifier(reader);
      }
      else
      {
        KeyInfo keyInfo = new KeyInfo(this.KeyInfoSerializer);
        keyInfo.ReadXml(XmlDictionaryReader.CreateDictionaryReader(reader));
        securityKeyIdentifier = keyInfo.KeyIdentifier;
      }
      if (securityKeyIdentifier.Count != 0)
        return securityKeyIdentifier;
      return new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
      {
        (SecurityKeyIdentifierClause) new Saml11SecurityKeyIdentifierClause(assertion)
      });
    }

    protected virtual void WriteSigningKeyInfo(
      XmlWriter writer,
      SecurityKeyIdentifier signingKeyIdentifier)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (signingKeyIdentifier == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (signingKeyIdentifier));
      if (this.KeyInfoSerializer.CanWriteKeyIdentifier(signingKeyIdentifier))
        this.KeyInfoSerializer.WriteKeyIdentifier(writer, signingKeyIdentifier);
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4221", (object) signingKeyIdentifier));
    }

    private void ValidateStatements(IList<SamlStatement> statements)
    {
      if (statements == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (statements));
      List<SamlSubject> samlSubjectList = new List<SamlSubject>();
      foreach (SamlStatement statement in (IEnumerable<SamlStatement>) statements)
      {
        if (statement is SamlAttributeStatement)
          samlSubjectList.Add((statement as SamlAttributeStatement).SamlSubject);
        if (statement is SamlAuthenticationStatement)
          samlSubjectList.Add((statement as SamlAuthenticationStatement).SamlSubject);
        if (statement is SamlAuthorizationDecisionStatement)
          samlSubjectList.Add((statement as SamlAuthorizationDecisionStatement).SamlSubject);
      }
      if (samlSubjectList.Count == 0)
        return;
      string name = samlSubjectList[0].Name;
      string nameFormat = samlSubjectList[0].NameFormat;
      string nameQualifier = samlSubjectList[0].NameQualifier;
      foreach (SamlSubject samlSubject in samlSubjectList)
      {
        if (!StringComparer.Ordinal.Equals(samlSubject.Name, name) || !StringComparer.Ordinal.Equals(samlSubject.NameFormat, nameFormat) || !StringComparer.Ordinal.Equals(samlSubject.NameQualifier, nameQualifier))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4225", (object) samlSubject));
      }
    }

    public override string[] GetTokenTypeIdentifiers() => Saml11SecurityTokenHandler._tokenTypeIdentifiers;

    public SecurityTokenSerializer KeyInfoSerializer
    {
      get
      {
        if (this._keyInfoSerializer == null)
        {
          lock (this._syncObject)
          {
            if (this._keyInfoSerializer == null)
              this._keyInfoSerializer = (SecurityTokenSerializer) new Microsoft.IdentityModel.Tokens.SecurityTokenSerializerAdapter(this.ContainingCollection != null ? this.ContainingCollection : Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(), SecurityVersion.WSSecurity11, TrustVersion.WSTrust13, SecureConversationVersion.WSSecureConversation13, false, (SamlSerializer) null, (SecurityStateEncoder) null, (IEnumerable<Type>) null);
          }
        }
        return this._keyInfoSerializer;
      }
      set => this._keyInfoSerializer = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public override Type TokenType => typeof (SamlSecurityToken);

    public Microsoft.IdentityModel.Tokens.SamlSecurityTokenRequirement SamlSecurityTokenRequirement
    {
      get => this._samlSecurityTokenRequirement;
      set => this._samlSecurityTokenRequirement = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    private class WrappedSerializer : SecurityTokenSerializer
    {
      private Saml11SecurityTokenHandler _parent;
      private SamlAssertion _assertion;

      public WrappedSerializer(Saml11SecurityTokenHandler parent, SamlAssertion assertion)
      {
        this._parent = parent != null ? parent : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (parent));
        this._assertion = assertion;
      }

      protected override bool CanReadKeyIdentifierClauseCore(XmlReader reader) => false;

      protected override bool CanReadKeyIdentifierCore(XmlReader reader) => true;

      protected override bool CanReadTokenCore(XmlReader reader) => false;

      protected override bool CanWriteKeyIdentifierClauseCore(
        SecurityKeyIdentifierClause keyIdentifierClause)
      {
        return false;
      }

      protected override bool CanWriteKeyIdentifierCore(SecurityKeyIdentifier keyIdentifier) => false;

      protected override bool CanWriteTokenCore(SecurityToken token) => false;

      protected override SecurityKeyIdentifierClause ReadKeyIdentifierClauseCore(
        XmlReader reader)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException());
      }

      protected override SecurityKeyIdentifier ReadKeyIdentifierCore(
        XmlReader reader)
      {
        return this._parent.ReadSigningKeyInfo(reader, this._assertion);
      }

      protected override SecurityToken ReadTokenCore(
        XmlReader reader,
        SecurityTokenResolver tokenResolver)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException());
      }

      protected override void WriteKeyIdentifierClauseCore(
        XmlWriter writer,
        SecurityKeyIdentifierClause keyIdentifierClause)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException());
      }

      protected override void WriteKeyIdentifierCore(
        XmlWriter writer,
        SecurityKeyIdentifier keyIdentifier)
      {
        this._parent.WriteSigningKeyInfo(writer, keyIdentifier);
      }

      protected override void WriteTokenCore(XmlWriter writer, SecurityToken token) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException());
    }
  }
}
