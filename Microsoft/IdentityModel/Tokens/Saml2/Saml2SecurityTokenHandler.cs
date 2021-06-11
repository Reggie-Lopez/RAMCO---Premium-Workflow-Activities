// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Diagnostics;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Protocols.XmlEncryption;
using Microsoft.IdentityModel.Protocols.XmlSignature;
using Microsoft.IdentityModel.Web;
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

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2SecurityTokenHandler : Microsoft.IdentityModel.Tokens.SecurityTokenHandler
  {
    public const string TokenProfile11ValueType = "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID";
    private const string Actor = "Actor";
    private const string Attribute = "Attribute";
    private static string[] _tokenTypeIdentifiers = new string[2]
    {
      "urn:oasis:names:tc:SAML:2.0:assertion",
      "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0"
    };
    private Microsoft.IdentityModel.Tokens.SamlSecurityTokenRequirement _samlSecurityTokenRequirement;
    private SecurityTokenSerializer _keyInfoSerializer;
    private object _syncObject = new object();

    public Saml2SecurityTokenHandler()
      : this(new Microsoft.IdentityModel.Tokens.SamlSecurityTokenRequirement())
    {
    }

    public Saml2SecurityTokenHandler(
      Microsoft.IdentityModel.Tokens.SamlSecurityTokenRequirement samlSecurityTokenRequirement)
    {
      this._samlSecurityTokenRequirement = samlSecurityTokenRequirement != null ? samlSecurityTokenRequirement : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (samlSecurityTokenRequirement));
    }

    public Saml2SecurityTokenHandler(XmlNodeList customConfigElements)
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

    public override SecurityKeyIdentifierClause CreateSecurityTokenReference(
      SecurityToken token,
      bool attached)
    {
      return token != null ? (SecurityKeyIdentifierClause) token.CreateKeyIdentifierClause<Saml2AssertionKeyIdentifierClause>() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
    }

    protected virtual Saml2Conditions CreateConditions(
      Lifetime tokenLifetime,
      string relyingPartyAddress,
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      bool flag1 = null != tokenLifetime;
      bool flag2 = !string.IsNullOrEmpty(relyingPartyAddress);
      if (!flag1 && !flag2)
        return (Saml2Conditions) null;
      Saml2Conditions saml2Conditions = new Saml2Conditions();
      if (flag1)
      {
        saml2Conditions.NotBefore = tokenLifetime.Created;
        saml2Conditions.NotOnOrAfter = tokenLifetime.Expires;
      }
      if (flag2)
        saml2Conditions.AudienceRestrictions.Add(new Saml2AudienceRestriction(new Uri(relyingPartyAddress)));
      return saml2Conditions;
    }

    protected virtual Saml2Advice CreateAdvice(Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor) => (Saml2Advice) null;

    protected virtual Saml2NameIdentifier CreateIssuerNameIdentifier(
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      string name = tokenDescriptor != null ? tokenDescriptor.TokenIssuerName : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor));
      return !string.IsNullOrEmpty(name) ? new Saml2NameIdentifier(name) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4138")));
    }

    protected virtual Saml2Attribute CreateAttribute(
      Claim claim,
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      if (claim == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claim));
      Saml2Attribute saml2Attribute = new Saml2Attribute(claim.ClaimType, claim.Value);
      if (!StringComparer.Ordinal.Equals("LOCAL AUTHORITY", claim.OriginalIssuer))
        saml2Attribute.OriginalIssuer = claim.OriginalIssuer;
      saml2Attribute.AttributeValueXsiType = claim.ValueType;
      if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/attributename"))
      {
        string property = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/attributename"];
        saml2Attribute.NameFormat = UriUtil.CanCreateValidUri(property, UriKind.Absolute) ? new Uri(property) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("nameFormat", Microsoft.IdentityModel.SR.GetString("ID0013"));
      }
      if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/displayname"))
        saml2Attribute.FriendlyName = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/displayname"];
      return saml2Attribute;
    }

    protected virtual Saml2AttributeStatement CreateAttributeStatement(
      IClaimsIdentity subject,
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      if (subject == null)
        return (Saml2AttributeStatement) null;
      if (subject.Claims != null)
      {
        List<Saml2Attribute> saml2AttributeList = new List<Saml2Attribute>(subject.Claims.Count);
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
                saml2AttributeList.Add(this.CreateAttribute(claim, tokenDescriptor));
                continue;
            }
          }
        }
        this.AddDelegateToAttributes(subject, (ICollection<Saml2Attribute>) saml2AttributeList, tokenDescriptor);
        ICollection<Saml2Attribute> saml2Attributes = this.CollectAttributeValues((ICollection<Saml2Attribute>) saml2AttributeList);
        if (saml2Attributes.Count > 0)
          return new Saml2AttributeStatement((IEnumerable<Saml2Attribute>) saml2Attributes);
      }
      return (Saml2AttributeStatement) null;
    }

    protected virtual ICollection<Saml2Attribute> CollectAttributeValues(
      ICollection<Saml2Attribute> attributes)
    {
      Dictionary<Microsoft.IdentityModel.Tokens.SamlAttributeKeyComparer.AttributeKey, Saml2Attribute> dictionary = new Dictionary<Microsoft.IdentityModel.Tokens.SamlAttributeKeyComparer.AttributeKey, Saml2Attribute>(attributes.Count, (IEqualityComparer<Microsoft.IdentityModel.Tokens.SamlAttributeKeyComparer.AttributeKey>) new Microsoft.IdentityModel.Tokens.SamlAttributeKeyComparer());
      foreach (Saml2Attribute attribute in (IEnumerable<Saml2Attribute>) attributes)
      {
        if (attribute != null)
        {
          Microsoft.IdentityModel.Tokens.SamlAttributeKeyComparer.AttributeKey key = new Microsoft.IdentityModel.Tokens.SamlAttributeKeyComparer.AttributeKey(attribute);
          if (dictionary.ContainsKey(key))
          {
            foreach (string str in attribute.Values)
              dictionary[key].Values.Add(str);
          }
          else
            dictionary.Add(key, attribute);
        }
      }
      return (ICollection<Saml2Attribute>) dictionary.Values;
    }

    protected virtual void AddDelegateToAttributes(
      IClaimsIdentity subject,
      ICollection<Saml2Attribute> attributes,
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      if (subject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (subject));
      if (tokenDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor));
      if (subject.Actor == null)
        return;
      List<Saml2Attribute> saml2AttributeList = new List<Saml2Attribute>(subject.Actor.Claims.Count);
      foreach (Claim claim in subject.Actor.Claims)
      {
        if (claim != null)
          saml2AttributeList.Add(this.CreateAttribute(claim, tokenDescriptor));
      }
      this.AddDelegateToAttributes(subject.Actor, (ICollection<Saml2Attribute>) saml2AttributeList, tokenDescriptor);
      ICollection<Saml2Attribute> saml2Attributes = this.CollectAttributeValues((ICollection<Saml2Attribute>) saml2AttributeList);
      attributes.Add(this.CreateAttribute(new Claim("http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor", this.CreateXmlStringFromAttributes((IEnumerable<Saml2Attribute>) saml2Attributes), "http://www.w3.org/2001/XMLSchema#string"), tokenDescriptor));
    }

    protected virtual string CreateXmlStringFromAttributes(IEnumerable<Saml2Attribute> attributes)
    {
      bool flag = false;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter((Stream) memoryStream, Encoding.UTF8, false))
        {
          foreach (Saml2Attribute attribute in attributes)
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

    protected virtual IEnumerable<Saml2Statement> CreateStatements(
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      if (tokenDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor));
      Collection<Saml2Statement> collection = new Collection<Saml2Statement>();
      Saml2AttributeStatement attributeStatement = this.CreateAttributeStatement(tokenDescriptor.Subject, tokenDescriptor);
      if (attributeStatement != null)
        collection.Add((Saml2Statement) attributeStatement);
      Saml2AuthenticationStatement authenticationStatement = this.CreateAuthenticationStatement(tokenDescriptor.AuthenticationInfo, tokenDescriptor);
      if (authenticationStatement != null)
        collection.Add((Saml2Statement) authenticationStatement);
      return (IEnumerable<Saml2Statement>) collection;
    }

    protected virtual Saml2AuthenticationStatement CreateAuthenticationStatement(
      AuthenticationInformation authInfo,
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      if (tokenDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor));
      if (tokenDescriptor.Subject == null)
        return (Saml2AuthenticationStatement) null;
      string normalizedAuthenticationType = (string) null;
      string s = (string) null;
      IEnumerable<Claim> source1 = tokenDescriptor.Subject.Claims.Where<Claim>((Func<Claim, bool>) (c => c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod"));
      if (source1.Count<Claim>() > 0)
        normalizedAuthenticationType = source1.First<Claim>().Value;
      IEnumerable<Claim> source2 = tokenDescriptor.Subject.Claims.Where<Claim>((Func<Claim, bool>) (c => c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant"));
      if (source2.Count<Claim>() > 0)
        s = source2.First<Claim>().Value;
      if (normalizedAuthenticationType == null && s == null)
        return (Saml2AuthenticationStatement) null;
      if (normalizedAuthenticationType == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4270", (object) "AuthenticationMethod", (object) "SAML2"));
      if (s == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4270", (object) "AuthenticationInstant", (object) "SAML2"));
      Uri result;
      if (!UriUtil.TryCreateValidUri(this.DenormalizeAuthenticationType(normalizedAuthenticationType), UriKind.Absolute, out result))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4185", (object) normalizedAuthenticationType));
      Saml2AuthenticationStatement authenticationStatement = new Saml2AuthenticationStatement(new Saml2AuthenticationContext(result), DateTime.ParseExact(s, DateTimeFormats.Accepted, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime());
      if (authInfo != null)
      {
        if (!string.IsNullOrEmpty(authInfo.DnsName) || !string.IsNullOrEmpty(authInfo.Address))
          authenticationStatement.SubjectLocality = new Saml2SubjectLocality(authInfo.Address, authInfo.DnsName);
        if (!string.IsNullOrEmpty(authInfo.Session))
          authenticationStatement.SessionIndex = authInfo.Session;
        authenticationStatement.SessionNotOnOrAfter = authInfo.NotOnOrAfter;
      }
      return authenticationStatement;
    }

    protected virtual Saml2Subject CreateSamlSubject(
      Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      if (tokenDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor));
      Saml2Subject saml2Subject = new Saml2Subject();
      string name = (string) null;
      string uriString = (string) null;
      string str1 = (string) null;
      string str2 = (string) null;
      string str3 = (string) null;
      if (tokenDescriptor.Subject != null && tokenDescriptor.Subject.Claims != null)
      {
        foreach (Claim claim in tokenDescriptor.Subject.Claims)
        {
          if (claim.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
          {
            name = name == null ? claim.Value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4139")));
            if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format"))
              uriString = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format"];
            if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier"))
              str1 = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier"];
            if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spnamequalifier"))
              str3 = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spnamequalifier"];
            if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spprovidedid"))
              str2 = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spprovidedid"];
          }
        }
      }
      if (name != null)
      {
        Saml2NameIdentifier saml2NameIdentifier = new Saml2NameIdentifier(name);
        if (uriString != null && UriUtil.CanCreateValidUri(uriString, UriKind.Absolute))
          saml2NameIdentifier.Format = new Uri(uriString);
        saml2NameIdentifier.NameQualifier = str1;
        saml2NameIdentifier.SPNameQualifier = str3;
        saml2NameIdentifier.SPProvidedId = str2;
        saml2Subject.NameId = saml2NameIdentifier;
      }
      Saml2SubjectConfirmation subjectConfirmation;
      if (tokenDescriptor.Proof == null)
      {
        subjectConfirmation = new Saml2SubjectConfirmation(Saml2Constants.ConfirmationMethods.Bearer);
      }
      else
      {
        subjectConfirmation = new Saml2SubjectConfirmation(Saml2Constants.ConfirmationMethods.HolderOfKey, new Saml2SubjectConfirmationData());
        subjectConfirmation.SubjectConfirmationData.KeyIdentifiers.Add(tokenDescriptor.Proof.KeyIdentifier);
      }
      saml2Subject.SubjectConfirmations.Add(subjectConfirmation);
      return saml2Subject;
    }

    public override SecurityToken CreateToken(Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
    {
      Saml2Assertion assertion = tokenDescriptor != null ? new Saml2Assertion(this.CreateIssuerNameIdentifier(tokenDescriptor)) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenDescriptor));
      assertion.Subject = this.CreateSamlSubject(tokenDescriptor);
      assertion.SigningCredentials = this.GetSigningCredentials(tokenDescriptor);
      assertion.Conditions = this.CreateConditions(tokenDescriptor.Lifetime, tokenDescriptor.AppliesToAddress, tokenDescriptor);
      assertion.Advice = this.CreateAdvice(tokenDescriptor);
      IEnumerable<Saml2Statement> statements = this.CreateStatements(tokenDescriptor);
      if (statements != null)
      {
        foreach (Saml2Statement saml2Statement in statements)
          assertion.Statements.Add(saml2Statement);
      }
      assertion.EncryptingCredentials = this.GetEncryptingCredentials(tokenDescriptor);
      return (SecurityToken) new Saml2SecurityToken(assertion);
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

    public override bool CanValidateToken => true;

    public override Type TokenType => typeof (Saml2SecurityToken);

    public override string[] GetTokenTypeIdentifiers() => Saml2SecurityTokenHandler._tokenTypeIdentifiers;

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

    protected virtual void ValidateConditions(
      Saml2Conditions conditions,
      bool enforceAudienceRestriction)
    {
      if (conditions != null)
      {
        DateTime utcNow = DateTime.UtcNow;
        if (conditions.NotBefore.HasValue && DateTimeUtil.Add(utcNow, this.Configuration.MaxClockSkew) < conditions.NotBefore.Value)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Microsoft.IdentityModel.Tokens.SecurityTokenNotYetValidException(Microsoft.IdentityModel.SR.GetString("ID4147", (object) conditions.NotBefore.Value, (object) utcNow)));
        if (conditions.NotOnOrAfter.HasValue && DateTimeUtil.Add(utcNow, this.Configuration.MaxClockSkew.Negate()) >= conditions.NotOnOrAfter.Value)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException(Microsoft.IdentityModel.SR.GetString("ID4148", (object) conditions.NotOnOrAfter.Value, (object) utcNow)));
        if (conditions.OneTimeUse)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenValidationException(Microsoft.IdentityModel.SR.GetString("ID4149")));
        if (conditions.ProxyRestriction != null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenValidationException(Microsoft.IdentityModel.SR.GetString("ID4150")));
      }
      if (!enforceAudienceRestriction)
        return;
      if (this.Configuration == null || this.Configuration.AudienceRestriction.AllowedAudienceUris.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID1032")));
      if (conditions == null || conditions.AudienceRestrictions.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Microsoft.IdentityModel.Tokens.AudienceUriValidationFailedException(Microsoft.IdentityModel.SR.GetString("ID1035")));
      foreach (Saml2AudienceRestriction audienceRestriction in conditions.AudienceRestrictions)
        this.SamlSecurityTokenRequirement.ValidateAudienceRestriction((IList<Uri>) this.Configuration.AudienceRestriction.AllowedAudienceUris, (IList<Uri>) audienceRestriction.Audiences);
    }

    public override ClaimsIdentityCollection ValidateToken(
      SecurityToken token)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (!(token is Saml2SecurityToken samlToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID4151"));
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Verbose, TraceCode.Diagnostics, Microsoft.IdentityModel.SR.GetString("TraceValidateToken"), (TraceRecord) new TokenTraceRecord(token), (Exception) null);
      if (samlToken.IssuerToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenValidationException(Microsoft.IdentityModel.SR.GetString("ID4152")));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      Saml2Assertion assertion = samlToken.Assertion;
      this.ValidateConditions(assertion.Conditions, this.SamlSecurityTokenRequirement.ShouldEnforceAudienceRestriction(this.Configuration.AudienceRestriction.AudienceMode, (SecurityToken) samlToken));
      Saml2SubjectConfirmation subjectConfirmation = assertion.Subject.SubjectConfirmations[0];
      if (subjectConfirmation.SubjectConfirmationData != null)
        this.ValidateConfirmationData(subjectConfirmation.SubjectConfirmationData);
      if (samlToken.IssuerToken is X509SecurityToken issuerToken)
        this.CertificateValidator.Validate(issuerToken.Certificate);
      IClaimsIdentity claimsIdentity = this.CreateClaims(samlToken);
      if (this._samlSecurityTokenRequirement.MapToWindows)
      {
        WindowsClaimsIdentity fromUpn = WindowsClaimsIdentity.CreateFromUpn(this.FindUpn(claimsIdentity), "Federation", this._samlSecurityTokenRequirement.UseWindowsTokenService, this.Configuration.IssuerNameRegistry.GetWindowsIssuerName());
        fromUpn.Claims.CopyRange((IEnumerable<Claim>) claimsIdentity.Claims);
        claimsIdentity = (IClaimsIdentity) fromUpn;
      }
      if (this.Configuration.DetectReplayedTokens)
        this.DetectReplayedTokens((SecurityToken) samlToken);
      if (this.Configuration.SaveBootstrapTokens)
        claimsIdentity.BootstrapToken = token;
      return new ClaimsIdentityCollection((IEnumerable<IClaimsIdentity>) new IClaimsIdentity[1]
      {
        claimsIdentity
      });
    }

    protected virtual string FindUpn(IClaimsIdentity claimsIdentity) => WindowsMappingOperations.FindUpn(claimsIdentity);

    protected virtual string DenormalizeAuthenticationType(string normalizedAuthenticationType) => AuthenticationTypeMaps.Denormalize(normalizedAuthenticationType, AuthenticationTypeMaps.Saml2);

    protected override void DetectReplayedTokens(SecurityToken token)
    {
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (!(token is Saml2SecurityToken token1))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID1064", (object) token.GetType().ToString()));
      if (token1.SecurityKeys.Count != 0)
        return;
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      if (this.Configuration.TokenReplayCache == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4278"));
      if (string.IsNullOrEmpty(token1.Assertion.Id.Value))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenValidationException(Microsoft.IdentityModel.SR.GetString("ID1065")));
      StringBuilder stringBuilder = new StringBuilder();
      string base64String;
      using (HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.NewSha256())
      {
        if (string.IsNullOrEmpty(token1.Assertion.Issuer.Value))
          stringBuilder.AppendFormat("{0}{1}", (object) token1.Assertion.Id.Value, (object) Saml2SecurityTokenHandler._tokenTypeIdentifiers[0]);
        else
          stringBuilder.AppendFormat("{0}{1}{2}", (object) token1.Assertion.Id.Value, (object) token1.Assertion.Issuer.Value, (object) Saml2SecurityTokenHandler._tokenTypeIdentifiers[0]);
        base64String = Convert.ToBase64String(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(stringBuilder.ToString())));
      }
      if (this.Configuration.TokenReplayCache.TryFind(base64String))
      {
        string str = token1.Assertion.Issuer.Value != null ? token1.Assertion.Issuer.Value : string.Empty;
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Microsoft.IdentityModel.Tokens.SecurityTokenReplayDetectedException(Microsoft.IdentityModel.SR.GetString("ID1066", (object) typeof (Saml2SecurityToken).ToString(), (object) token1.Assertion.Id.Value, (object) str)));
      }
      this.Configuration.TokenReplayCache.TryAdd(base64String, (SecurityToken) null, DateTimeUtil.Add(this.GetCacheExpirationTime(token1), this.Configuration.MaxClockSkew));
    }

    protected virtual DateTime GetCacheExpirationTime(Saml2SecurityToken token)
    {
      DateTime t2 = DateTime.MaxValue;
      Saml2Assertion saml2Assertion = token != null ? token.Assertion : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (saml2Assertion != null)
      {
        if (saml2Assertion.Conditions != null && saml2Assertion.Conditions.NotOnOrAfter.HasValue)
          t2 = saml2Assertion.Conditions.NotOnOrAfter.Value;
        else if (saml2Assertion.Subject != null && saml2Assertion.Subject.SubjectConfirmations != null && (saml2Assertion.Subject.SubjectConfirmations.Count != 0 && saml2Assertion.Subject.SubjectConfirmations[0].SubjectConfirmationData != null) && saml2Assertion.Subject.SubjectConfirmations[0].SubjectConfirmationData.NotOnOrAfter.HasValue)
          t2 = saml2Assertion.Subject.SubjectConfirmations[0].SubjectConfirmationData.NotOnOrAfter.Value;
      }
      DateTime t1 = DateTimeUtil.Add(DateTime.UtcNow, this.Configuration.TokenReplayCacheExpirationPeriod);
      if (DateTime.Compare(t1, t2) < 0)
        t2 = t1;
      return t2;
    }

    protected virtual string NormalizeAuthenticationContextClassReference(
      string saml2AuthenticationContextClassReference)
    {
      return AuthenticationTypeMaps.Normalize(saml2AuthenticationContextClassReference, AuthenticationTypeMaps.Saml2);
    }

    protected virtual void ProcessSamlSubject(
      Saml2Subject assertionSubject,
      IClaimsIdentity subject,
      string issuer)
    {
      if (assertionSubject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (assertionSubject));
      if (subject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (subject));
      Saml2NameIdentifier nameId = assertionSubject.NameId;
      if (nameId == null)
        return;
      Claim claim = new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", nameId.Value, "http://www.w3.org/2001/XMLSchema#string", issuer);
      if (nameId.Format != (Uri) null)
        claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format"] = nameId.Format.AbsoluteUri;
      if (nameId.NameQualifier != null)
        claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier"] = nameId.NameQualifier;
      if (nameId.SPNameQualifier != null)
        claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spnamequalifier"] = nameId.SPNameQualifier;
      if (nameId.SPProvidedId != null)
        claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spprovidedid"] = nameId.SPProvidedId;
      subject.Claims.Add(claim);
    }

    protected virtual void ProcessAttributeStatement(
      Saml2AttributeStatement statement,
      IClaimsIdentity subject,
      string issuer)
    {
      if (statement == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (statement));
      if (subject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (subject));
      foreach (Saml2Attribute attribute in statement.Attributes)
      {
        if (StringComparer.Ordinal.Equals(attribute.Name, "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor"))
        {
          if (subject.Actor != null)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4218"));
          this.SetDelegateFromAttribute(attribute, subject, issuer);
        }
        else
        {
          foreach (string str in attribute.Values)
          {
            if (str != null)
            {
              string originalIssuer = issuer;
              if (attribute.OriginalIssuer != null)
                originalIssuer = attribute.OriginalIssuer;
              Claim claim = new Claim(attribute.Name, str, attribute.AttributeValueXsiType, issuer, originalIssuer);
              if (attribute.NameFormat != (Uri) null)
                claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/attributename"] = attribute.NameFormat.AbsoluteUri;
              if (attribute.FriendlyName != null)
                claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/displayname"] = attribute.FriendlyName;
              subject.Claims.Add(claim);
            }
          }
        }
      }
    }

    protected virtual void SetDelegateFromAttribute(
      Saml2Attribute attribute,
      IClaimsIdentity subject,
      string issuer)
    {
      if (subject == null || attribute == null || (attribute.Values == null || attribute.Values.Count < 1))
        return;
      Saml2Attribute attribute1 = (Saml2Attribute) null;
      Collection<Claim> collection = new Collection<Claim>();
      foreach (string s in attribute.Values)
      {
        if (s != null)
        {
          using (XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(s), XmlDictionaryReaderQuotas.Max))
          {
            int content = (int) textReader.MoveToContent();
            textReader.ReadStartElement("Actor");
            while (textReader.IsStartElement("Attribute"))
            {
              Saml2Attribute saml2Attribute = this.ReadAttribute((XmlReader) textReader);
              if (saml2Attribute != null)
              {
                if (saml2Attribute.Name == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor")
                {
                  attribute1 = attribute1 == null ? saml2Attribute : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4218"));
                }
                else
                {
                  string originalIssuer = saml2Attribute.OriginalIssuer;
                  for (int index = 0; index < saml2Attribute.Values.Count; ++index)
                  {
                    Claim claim = !string.IsNullOrEmpty(originalIssuer) ? new Claim(saml2Attribute.Name, saml2Attribute.Values[index], saml2Attribute.AttributeValueXsiType, issuer, originalIssuer) : new Claim(saml2Attribute.Name, saml2Attribute.Values[index], saml2Attribute.AttributeValueXsiType, issuer);
                    if (saml2Attribute.NameFormat != (Uri) null)
                      claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/attributename"] = saml2Attribute.NameFormat.AbsoluteUri;
                    if (saml2Attribute.FriendlyName != null)
                      claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/displayname"] = saml2Attribute.FriendlyName;
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

    protected virtual void ProcessAuthenticationStatement(
      Saml2AuthenticationStatement statement,
      IClaimsIdentity subject,
      string issuer)
    {
      if (subject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (subject));
      if (statement.AuthenticationContext.DeclarationReference != (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4180"));
      if (statement.AuthenticationContext.ClassReference != (Uri) null)
        subject.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", this.NormalizeAuthenticationContextClassReference(statement.AuthenticationContext.ClassReference.AbsoluteUri), "http://www.w3.org/2001/XMLSchema#string", issuer));
      subject.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(statement.AuthenticationInstant.ToUniversalTime(), DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime", issuer));
    }

    protected virtual void ProcessAuthorizationDecisionStatement(
      Saml2AuthorizationDecisionStatement statement,
      IClaimsIdentity subject,
      string issuer)
    {
    }

    protected virtual void ProcessStatement(
      Collection<Saml2Statement> statements,
      IClaimsIdentity subject,
      string issuer)
    {
      Collection<Saml2AuthenticationStatement> collection = new Collection<Saml2AuthenticationStatement>();
      foreach (Saml2Statement statement1 in statements)
      {
        switch (statement1)
        {
          case Saml2AttributeStatement statement:
            this.ProcessAttributeStatement(statement, subject, issuer);
            continue;
          case Saml2AuthenticationStatement authenticationStatement:
            collection.Add(authenticationStatement);
            continue;
          case Saml2AuthorizationDecisionStatement statement:
            this.ProcessAuthorizationDecisionStatement(statement, subject, issuer);
            continue;
          default:
            continue;
        }
      }
      foreach (Saml2AuthenticationStatement statement in collection)
      {
        if (statement != null)
          this.ProcessAuthenticationStatement(statement, subject, issuer);
      }
    }

    protected virtual IClaimsIdentity CreateClaims(Saml2SecurityToken samlToken)
    {
      if (samlToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (samlToken));
      IClaimsIdentity subject = (IClaimsIdentity) new ClaimsIdentity("Federation", this.SamlSecurityTokenRequirement.NameClaimType, this.SamlSecurityTokenRequirement.RoleClaimType);
      Saml2Assertion assertion = samlToken.Assertion;
      if (assertion == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (samlToken), Microsoft.IdentityModel.SR.GetString("ID1034"));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      if (this.Configuration.IssuerNameRegistry == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4277"));
      string issuerName = this.Configuration.IssuerNameRegistry.GetIssuerName(samlToken.IssuerToken, assertion.Issuer.Value);
      if (string.IsNullOrEmpty(issuerName))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4175")));
      this.ProcessSamlSubject(assertion.Subject, subject, issuerName);
      this.ProcessStatement(assertion.Statements, subject, issuerName);
      return subject;
    }

    public override bool CanWriteToken => true;

    public override void WriteToken(XmlWriter writer, SecurityToken token)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (!(token is Saml2SecurityToken saml2SecurityToken))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), Microsoft.IdentityModel.SR.GetString("ID4160"));
      this.WriteAssertion(writer, saml2SecurityToken.Assertion);
    }

    public override bool CanReadToken(XmlReader reader)
    {
      if (reader == null)
        return false;
      return reader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion") || reader.IsStartElement("EncryptedAssertion", "urn:oasis:names:tc:SAML:2.0:assertion");
    }

    public override bool CanReadKeyIdentifierClause(XmlReader reader) => Saml2SecurityTokenHandler.IsSaml2KeyIdentifierClause(reader);

    public override bool CanWriteKeyIdentifierClause(
      SecurityKeyIdentifierClause securityKeyIdentifierClause)
    {
      return securityKeyIdentifierClause is Saml2AssertionKeyIdentifierClause || securityKeyIdentifierClause is WrappedSaml2AssertionKeyIdentifierClause;
    }

    internal static bool IsSaml2KeyIdentifierClause(XmlReader reader)
    {
      if (!reader.IsStartElement("SecurityTokenReference", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
        return false;
      string attribute = reader.GetAttribute("TokenType", "http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd");
      return ((IEnumerable<string>) Saml2SecurityTokenHandler._tokenTypeIdentifiers).Contains<string>(attribute);
    }

    internal static bool IsSaml2Assertion(XmlReader reader) => reader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion") || reader.IsStartElement("EncryptedAssertion", "urn:oasis:names:tc:SAML:2.0:assertion");

    public override SecurityKeyIdentifierClause ReadKeyIdentifierClause(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!Saml2SecurityTokenHandler.IsSaml2KeyIdentifierClause(reader))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4161"));
      if (reader.IsEmptyElement)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID3061", (object) "SecurityTokenReference", (object) "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"));
      try
      {
        byte[] derivationNonce = (byte[]) null;
        int derivationLength = 0;
        string attribute1 = reader.GetAttribute("Nonce", "http://schemas.xmlsoap.org/ws/2005/02/sc");
        if (!string.IsNullOrEmpty(attribute1))
        {
          derivationNonce = Convert.FromBase64String(attribute1);
          string attribute2 = reader.GetAttribute("Length", "http://schemas.xmlsoap.org/ws/2005/02/sc");
          derivationLength = string.IsNullOrEmpty(attribute2) ? 32 : XmlConvert.ToInt32(attribute2);
        }
        if (derivationNonce == null)
        {
          string attribute2 = reader.GetAttribute("Nonce", "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512");
          if (!string.IsNullOrEmpty(attribute2))
          {
            derivationNonce = Convert.FromBase64String(attribute2);
            string attribute3 = reader.GetAttribute("Length", "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512");
            derivationLength = string.IsNullOrEmpty(attribute3) ? 32 : XmlConvert.ToInt32(attribute3);
          }
        }
        reader.Read();
        if (reader.IsStartElement("Reference", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4126"));
        if (!reader.IsStartElement("KeyIdentifier", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
          reader.ReadStartElement("KeyIdentifier", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
        string attribute4 = reader.GetAttribute("ValueType");
        if (string.IsNullOrEmpty(attribute4))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0001", (object) "ValueType", (object) "KeyIdentifier"));
        if (!StringComparer.Ordinal.Equals("http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID", attribute4))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4127", (object) attribute4));
        string id = reader.ReadElementString();
        reader.ReadEndElement();
        return (SecurityKeyIdentifierClause) new Saml2AssertionKeyIdentifierClause(id, derivationNonce, derivationLength);
      }
      catch (Exception ex)
      {
        switch (ex)
        {
          case FormatException _:
          case ArgumentException _:
          case InvalidOperationException _:
          case OverflowException _:
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4125"), ex);
          default:
            throw;
        }
      }
    }

    protected virtual void ValidateConfirmationData(Saml2SubjectConfirmationData confirmationData)
    {
      if (confirmationData == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (confirmationData));
      if (confirmationData.Address != null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4153")));
      if (confirmationData.InResponseTo != null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4154")));
      if ((Uri) null != confirmationData.Recipient)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4157")));
      DateTime utcNow = DateTime.UtcNow;
      if (confirmationData.NotBefore.HasValue && DateTimeUtil.Add(utcNow, this.Configuration.MaxClockSkew) < confirmationData.NotBefore.Value)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4176", (object) confirmationData.NotBefore.Value, (object) utcNow)));
      if (confirmationData.NotOnOrAfter.HasValue && DateTimeUtil.Add(utcNow, this.Configuration.MaxClockSkew.Negate()) >= confirmationData.NotOnOrAfter.Value)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4177", (object) confirmationData.NotOnOrAfter.Value, (object) utcNow)));
    }

    protected virtual ReadOnlyCollection<SecurityKey> ResolveSecurityKeys(
      Saml2Assertion assertion,
      SecurityTokenResolver resolver)
    {
      Saml2Subject saml2Subject = assertion != null ? assertion.Subject : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (assertion));
      if (saml2Subject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4130")));
      if (saml2Subject.SubjectConfirmations.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4131")));
      if (saml2Subject.SubjectConfirmations.Count > 1)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4132")));
      Saml2SubjectConfirmation subjectConfirmation = saml2Subject.SubjectConfirmations[0];
      if (Saml2Constants.ConfirmationMethods.Bearer == subjectConfirmation.Method)
      {
        if (subjectConfirmation.SubjectConfirmationData != null && subjectConfirmation.SubjectConfirmationData.KeyIdentifiers.Count != 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4133")));
        return EmptyReadOnlyCollection<SecurityKey>.Instance;
      }
      if (Saml2Constants.ConfirmationMethods.HolderOfKey == subjectConfirmation.Method)
      {
        if (subjectConfirmation.SubjectConfirmationData == null || subjectConfirmation.SubjectConfirmationData.KeyIdentifiers.Count == 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4134")));
        List<SecurityKey> securityKeyList = new List<SecurityKey>();
        foreach (SecurityKeyIdentifier keyIdentifier in subjectConfirmation.SubjectConfirmationData.KeyIdentifiers)
        {
          SecurityKey key1 = (SecurityKey) null;
          foreach (SecurityKeyIdentifierClause keyIdentifierClause in keyIdentifier)
          {
            if (resolver != null && resolver.TryResolveSecurityKey(keyIdentifierClause, out key1))
            {
              securityKeyList.Add(key1);
              break;
            }
          }
          if (key1 == null)
          {
            if (keyIdentifier.CanCreateKey)
            {
              SecurityKey key2 = keyIdentifier.CreateKey();
              securityKeyList.Add(key2);
            }
            else
              securityKeyList.Add((SecurityKey) new Microsoft.IdentityModel.Tokens.SecurityKeyElement(keyIdentifier, resolver));
          }
        }
        return securityKeyList.AsReadOnly();
      }
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4136", (object) subjectConfirmation.Method)));
    }

    protected virtual SecurityToken ResolveIssuerToken(
      Saml2Assertion assertion,
      SecurityTokenResolver issuerResolver)
    {
      if (assertion == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (assertion));
      SecurityToken token;
      if (this.TryResolveIssuerToken(assertion, issuerResolver, out token))
        return token;
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString(assertion.SigningCredentials == null ? "ID4141" : "ID4142")));
    }

    protected virtual bool TryResolveIssuerToken(
      Saml2Assertion assertion,
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

    public override SecurityToken ReadToken(XmlReader reader)
    {
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      if (this.Configuration.IssuerTokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4275"));
      if (this.Configuration.ServiceTokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4276"));
      Saml2Assertion assertion = this.ReadAssertion(reader);
      ReadOnlyCollection<SecurityKey> keys = this.ResolveSecurityKeys(assertion, this.Configuration.ServiceTokenResolver);
      SecurityToken token;
      this.TryResolveIssuerToken(assertion, this.Configuration.IssuerTokenResolver, out token);
      return (SecurityToken) new Saml2SecurityToken(assertion, keys, token);
    }

    public Microsoft.IdentityModel.Tokens.SamlSecurityTokenRequirement SamlSecurityTokenRequirement
    {
      get => this._samlSecurityTokenRequirement;
      set => this._samlSecurityTokenRequirement = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public override void WriteKeyIdentifierClause(
      XmlWriter writer,
      SecurityKeyIdentifierClause securityKeyIdentifierClause)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      Saml2AssertionKeyIdentifierClause identifierClause1 = securityKeyIdentifierClause != null ? securityKeyIdentifierClause as Saml2AssertionKeyIdentifierClause : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifierClause");
      if (securityKeyIdentifierClause is WrappedSaml2AssertionKeyIdentifierClause identifierClause2)
        identifierClause1 = identifierClause2.WrappedClause;
      if (identifierClause1 == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("keyIdentifierClause", Microsoft.IdentityModel.SR.GetString("ID4162"));
      writer.WriteStartElement("SecurityTokenReference", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
      byte[] derivationNonce = identifierClause1.GetDerivationNonce();
      if (derivationNonce != null)
      {
        writer.WriteAttributeString("Nonce", "http://schemas.xmlsoap.org/ws/2005/02/sc", Convert.ToBase64String(derivationNonce));
        switch (identifierClause1.DerivationLength)
        {
          case 0:
          case 32:
            break;
          default:
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4129")));
        }
      }
      writer.WriteAttributeString("TokenType", "http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd", "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0");
      writer.WriteStartElement("KeyIdentifier", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
      writer.WriteAttributeString("ValueType", "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID");
      writer.WriteString(identifierClause1.AssertionId);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    private static void ReadEmptyContentElement(XmlReader reader)
    {
      bool isEmptyElement = reader.IsEmptyElement;
      reader.Read();
      if (isEmptyElement)
        return;
      reader.ReadEndElement();
    }

    private static Saml2Id ReadSimpleNCNameElement(XmlReader reader)
    {
      try
      {
        if (reader.IsEmptyElement)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID3061", (object) reader.LocalName, (object) reader.NamespaceURI));
        XmlUtil.ValidateXsiType(reader, "NCName", "http://www.w3.org/2001/XMLSchema");
        reader.MoveToElement();
        return new Saml2Id(reader.ReadElementContentAsString());
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    private static Uri ReadSimpleUriElement(XmlReader reader) => Saml2SecurityTokenHandler.ReadSimpleUriElement(reader, UriKind.Absolute);

    private static Uri ReadSimpleUriElement(XmlReader reader, UriKind kind) => Saml2SecurityTokenHandler.ReadSimpleUriElement(reader, kind, false);

    private static Uri ReadSimpleUriElement(
      XmlReader reader,
      UriKind kind,
      bool allowLaxReading)
    {
      try
      {
        if (reader.IsEmptyElement)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID3061", (object) reader.LocalName, (object) reader.NamespaceURI));
        XmlUtil.ValidateXsiType(reader, "anyURI", "http://www.w3.org/2001/XMLSchema");
        reader.MoveToElement();
        string uriString = reader.ReadElementContentAsString();
        if (string.IsNullOrEmpty(uriString))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0022"));
        if (!allowLaxReading && !UriUtil.CanCreateValidUri(uriString, kind))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString(kind == UriKind.RelativeOrAbsolute ? "ID0019" : "ID0013"));
        return new Uri(uriString, kind);
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual Saml2NameIdentifier ReadSubjectID(
      XmlReader reader,
      string parentElement)
    {
      if (reader.IsStartElement("NameID", "urn:oasis:names:tc:SAML:2.0:assertion"))
        return this.ReadNameID(reader);
      if (reader.IsStartElement("EncryptedID", "urn:oasis:names:tc:SAML:2.0:assertion"))
        return this.ReadEncryptedId(reader);
      if (!reader.IsStartElement("BaseID", "urn:oasis:names:tc:SAML:2.0:assertion"))
        return (Saml2NameIdentifier) null;
      XmlQualifiedName xsiType = XmlUtil.GetXsiType(reader);
      if ((XmlQualifiedName) null == xsiType || XmlUtil.EqualsQName(xsiType, "BaseIDAbstractType", "urn:oasis:names:tc:SAML:2.0:assertion"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4104", (object) reader.LocalName, (object) reader.NamespaceURI));
      if (XmlUtil.EqualsQName(xsiType, "NameIDType", "urn:oasis:names:tc:SAML:2.0:assertion"))
        return this.ReadNameIDType(reader);
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4110", (object) parentElement, (object) xsiType.Name, (object) xsiType.Namespace));
    }

    private static Exception TryWrapReadException(XmlReader reader, Exception inner)
    {
      switch (inner)
      {
        case FormatException _:
        case ArgumentException _:
        case InvalidOperationException _:
        case OverflowException _:
          return DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4125"), inner);
        default:
          return (Exception) null;
      }
    }

    protected virtual Saml2Action ReadAction(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("Action", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("Action", "urn:oasis:names:tc:SAML:2.0:assertion");
      if (reader.IsEmptyElement)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID3061", (object) "Action", (object) "urn:oasis:names:tc:SAML:2.0:assertion"));
      try
      {
        XmlUtil.ValidateXsiType(reader, "ActionType", "urn:oasis:names:tc:SAML:2.0:assertion");
        string attribute = reader.GetAttribute("Namespace");
        if (string.IsNullOrEmpty(attribute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0001", (object) "Namespace", (object) "Action"));
        Uri actionNamespace = UriUtil.CanCreateValidUri(attribute, UriKind.Absolute) ? new Uri(attribute) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0011", (object) "Namespace", (object) "Action"));
        return new Saml2Action(reader.ReadElementString(), actionNamespace);
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteAction(XmlWriter writer, Saml2Action data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      if ((Uri) null == data.Namespace)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data.Namespace");
      if (string.IsNullOrEmpty(data.Namespace.ToString()))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("data.Namespace");
      writer.WriteStartElement("Action", "urn:oasis:names:tc:SAML:2.0:assertion");
      writer.WriteAttributeString("Namespace", data.Namespace.AbsoluteUri);
      writer.WriteString(data.Value);
      writer.WriteEndElement();
    }

    protected virtual Saml2Advice ReadAdvice(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("Advice", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("Advice", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        Saml2Advice saml2Advice = new Saml2Advice();
        bool isEmptyElement = reader.IsEmptyElement;
        XmlUtil.ValidateXsiType(reader, "AdviceType", "urn:oasis:names:tc:SAML:2.0:assertion");
        reader.Read();
        if (!isEmptyElement)
        {
          while (reader.IsStartElement())
          {
            if (reader.IsStartElement("AssertionIDRef", "urn:oasis:names:tc:SAML:2.0:assertion"))
              saml2Advice.AssertionIdReferences.Add(Saml2SecurityTokenHandler.ReadSimpleNCNameElement(reader));
            else if (reader.IsStartElement("AssertionURIRef", "urn:oasis:names:tc:SAML:2.0:assertion"))
              saml2Advice.AssertionUriReferences.Add(Saml2SecurityTokenHandler.ReadSimpleUriElement(reader));
            else if (reader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
              saml2Advice.Assertions.Add(this.ReadAssertion(reader));
            else if (reader.IsStartElement("EncryptedAssertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
            {
              saml2Advice.Assertions.Add(this.ReadAssertion(reader));
            }
            else
            {
              if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
                DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID8006", (object) reader.LocalName, (object) reader.NamespaceURI));
              reader.Skip();
            }
          }
          reader.ReadEndElement();
        }
        return saml2Advice;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteAdvice(XmlWriter writer, Saml2Advice data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      writer.WriteStartElement("Advice", "urn:oasis:names:tc:SAML:2.0:assertion");
      foreach (Saml2Id assertionIdReference in data.AssertionIdReferences)
        writer.WriteElementString("AssertionIDRef", "urn:oasis:names:tc:SAML:2.0:assertion", assertionIdReference.Value);
      foreach (Uri assertionUriReference in data.AssertionUriReferences)
        writer.WriteElementString("AssertionURIRef", "urn:oasis:names:tc:SAML:2.0:assertion", assertionUriReference.AbsoluteUri);
      foreach (Saml2Assertion assertion in data.Assertions)
        this.WriteAssertion(writer, assertion);
      writer.WriteEndElement();
    }

    private static XmlDictionaryReader CreatePlaintextReaderFromEncryptedData(
      XmlDictionaryReader reader,
      SecurityTokenResolver serviceTokenResolver,
      SecurityTokenSerializer keyInfoSerializer,
      Collection<EncryptedKeyIdentifierClause> clauses,
      out Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials)
    {
      int num = reader != null ? (int) reader.MoveToContent() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (reader.IsEmptyElement)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader, Microsoft.IdentityModel.SR.GetString("ID3061", (object) reader.LocalName, (object) reader.NamespaceURI));
      encryptingCredentials = (Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials) null;
      XmlUtil.ValidateXsiType((XmlReader) reader, "EncryptedElementType", "urn:oasis:names:tc:SAML:2.0:assertion");
      reader.ReadStartElement();
      EncryptedDataElement encryptedDataElement = new EncryptedDataElement(keyInfoSerializer);
      encryptedDataElement.ReadXml(reader);
      int content = (int) reader.MoveToContent();
      while (reader.IsStartElement("EncryptedKey", "http://www.w3.org/2001/04/xmlenc#"))
      {
        SecurityKeyIdentifierClause identifierClause1;
        if (keyInfoSerializer.CanReadKeyIdentifierClause((XmlReader) reader))
        {
          identifierClause1 = keyInfoSerializer.ReadKeyIdentifierClause((XmlReader) reader);
        }
        else
        {
          EncryptedKeyElement encryptedKeyElement = new EncryptedKeyElement(keyInfoSerializer);
          encryptedKeyElement.ReadXml(reader);
          identifierClause1 = (SecurityKeyIdentifierClause) encryptedKeyElement.GetClause();
        }
        if (!(identifierClause1 is EncryptedKeyIdentifierClause identifierClause))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader, Microsoft.IdentityModel.SR.GetString("ID4172"));
        clauses.Add(identifierClause);
      }
      reader.ReadEndElement();
      SecurityKey key = (SecurityKey) null;
      SecurityKeyIdentifierClause identifierClause3 = (SecurityKeyIdentifierClause) null;
      foreach (SecurityKeyIdentifierClause keyIdentifierClause in encryptedDataElement.KeyIdentifier)
      {
        if (serviceTokenResolver.TryResolveSecurityKey(keyIdentifierClause, out key))
        {
          identifierClause3 = keyIdentifierClause;
          break;
        }
      }
      if (key == null)
      {
        foreach (SecurityKeyIdentifierClause clause in clauses)
        {
          if (serviceTokenResolver.TryResolveSecurityKey(clause, out key))
          {
            identifierClause3 = clause;
            break;
          }
        }
      }
      if (key == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new Microsoft.IdentityModel.Tokens.EncryptedTokenDecryptionFailedException());
      if (!(key is SymmetricSecurityKey symmetricSecurityKey))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4023")));
      SymmetricAlgorithm symmetricAlgorithm = symmetricSecurityKey.GetSymmetricAlgorithm(encryptedDataElement.Algorithm);
      byte[] buffer = encryptedDataElement.Decrypt(symmetricAlgorithm);
      encryptingCredentials = (Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials) new Saml2SecurityTokenHandler.ReceivedEncryptingCredentials(key, new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
      {
        identifierClause3
      }), encryptedDataElement.Algorithm);
      return XmlDictionaryReader.CreateTextReader(buffer, reader.Quotas);
    }

    protected virtual Saml2Assertion ReadAssertion(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (this.Configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4274"));
      if (this.Configuration.IssuerTokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4275"));
      if (this.Configuration.ServiceTokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4276"));
      XmlDictionaryReader reader1 = XmlDictionaryReader.CreateDictionaryReader(reader);
      Saml2Assertion assertion = new Saml2Assertion(new Saml2NameIdentifier("__TemporaryIssuer__"));
      if (reader.IsStartElement("EncryptedAssertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
      {
        Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = (Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials) null;
        reader1 = Saml2SecurityTokenHandler.CreatePlaintextReaderFromEncryptedData(reader1, this.Configuration.ServiceTokenResolver, this.KeyInfoSerializer, assertion.ExternalEncryptedKeys, out encryptingCredentials);
        assertion.EncryptingCredentials = encryptingCredentials;
      }
      if (!reader1.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader1.ReadStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion");
      if (reader1.IsEmptyElement)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader1, Microsoft.IdentityModel.SR.GetString("ID3061", (object) reader1.LocalName, (object) reader1.NamespaceURI));
      Saml2SecurityTokenHandler.WrappedSerializer wrappedSerializer = new Saml2SecurityTokenHandler.WrappedSerializer(this, assertion);
      EnvelopedSignatureReader reader2 = new EnvelopedSignatureReader((XmlReader) reader1, (SecurityTokenSerializer) wrappedSerializer, this.Configuration.IssuerTokenResolver, false, false, false);
      try
      {
        XmlUtil.ValidateXsiType((XmlReader) reader2, "AssertionType", "urn:oasis:names:tc:SAML:2.0:assertion");
        string attribute1 = reader2.GetAttribute("Version");
        if (string.IsNullOrEmpty(attribute1))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader2, Microsoft.IdentityModel.SR.GetString("ID0001", (object) "Version", (object) "Assertion"));
        if (!StringComparer.Ordinal.Equals(assertion.Version, attribute1))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader2, Microsoft.IdentityModel.SR.GetString("ID4100", (object) attribute1));
        string attribute2 = reader2.GetAttribute("ID");
        assertion.Id = !string.IsNullOrEmpty(attribute2) ? new Saml2Id(attribute2) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader2, Microsoft.IdentityModel.SR.GetString("ID0001", (object) "ID", (object) "Assertion"));
        string attribute3 = reader2.GetAttribute("IssueInstant");
        assertion.IssueInstant = !string.IsNullOrEmpty(attribute3) ? XmlConvert.ToDateTime(attribute3, DateTimeFormats.Accepted) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader2, Microsoft.IdentityModel.SR.GetString("ID0001", (object) "IssueInstant", (object) "Assertion"));
        reader2.Read();
        assertion.Issuer = this.ReadIssuer((XmlReader) reader2);
        reader2.TryReadSignature();
        if (reader2.IsStartElement("Subject", "urn:oasis:names:tc:SAML:2.0:assertion"))
          assertion.Subject = this.ReadSubject((XmlReader) reader2);
        if (reader2.IsStartElement("Conditions", "urn:oasis:names:tc:SAML:2.0:assertion"))
          assertion.Conditions = this.ReadConditions((XmlReader) reader2);
        if (reader2.IsStartElement("Advice", "urn:oasis:names:tc:SAML:2.0:assertion"))
          assertion.Advice = this.ReadAdvice((XmlReader) reader2);
        while (reader2.IsStartElement())
        {
          Saml2Statement saml2Statement;
          if (reader2.IsStartElement("Statement", "urn:oasis:names:tc:SAML:2.0:assertion"))
            saml2Statement = this.ReadStatement((XmlReader) reader2);
          else if (reader2.IsStartElement("AttributeStatement", "urn:oasis:names:tc:SAML:2.0:assertion"))
            saml2Statement = (Saml2Statement) this.ReadAttributeStatement((XmlReader) reader2);
          else if (reader2.IsStartElement("AuthnStatement", "urn:oasis:names:tc:SAML:2.0:assertion"))
            saml2Statement = (Saml2Statement) this.ReadAuthenticationStatement((XmlReader) reader2);
          else if (reader2.IsStartElement("AuthzDecisionStatement", "urn:oasis:names:tc:SAML:2.0:assertion"))
            saml2Statement = (Saml2Statement) this.ReadAuthorizationDecisionStatement((XmlReader) reader2);
          else
            break;
          assertion.Statements.Add(saml2Statement);
        }
        reader2.ReadEndElement();
        if (assertion.Subject == null)
        {
          if (assertion.Statements.Count == 0)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4106"));
          foreach (Saml2Statement statement in assertion.Statements)
          {
            switch (statement)
            {
              case Saml2AuthenticationStatement _:
              case Saml2AttributeStatement _:
              case Saml2AuthorizationDecisionStatement _:
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4119"));
              default:
                continue;
            }
          }
        }
        assertion.SigningCredentials = reader2.SigningCredentials;
        assertion.CaptureSourceData(reader2);
        return assertion;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException((XmlReader) reader2, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteAssertion(XmlWriter writer, Saml2Assertion data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      XmlWriter writer1 = writer;
      MemoryStream memoryStream = (MemoryStream) null;
      XmlDictionaryWriter dictionaryWriter = (XmlDictionaryWriter) null;
      if (data.EncryptingCredentials != null && !(data.EncryptingCredentials is Saml2SecurityTokenHandler.ReceivedEncryptingCredentials))
      {
        memoryStream = new MemoryStream();
        writer = (XmlWriter) (dictionaryWriter = XmlDictionaryWriter.CreateTextWriter((Stream) memoryStream, Encoding.UTF8, false));
      }
      else if (data.ExternalEncryptedKeys == null || data.ExternalEncryptedKeys.Count > 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4173")));
      if (data.CanWriteSourceData)
      {
        data.WriteSourceData(writer);
      }
      else
      {
        EnvelopedSignatureWriter envelopedSignatureWriter = (EnvelopedSignatureWriter) null;
        if (data.SigningCredentials != null)
          writer = (XmlWriter) (envelopedSignatureWriter = new EnvelopedSignatureWriter(writer, data.SigningCredentials, data.Id.Value, (SecurityTokenSerializer) new Saml2SecurityTokenHandler.WrappedSerializer(this, data)));
        if (data.Subject == null)
        {
          if (data.Statements == null || data.Statements.Count == 0)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4106")));
          foreach (Saml2Statement statement in data.Statements)
          {
            switch (statement)
            {
              case Saml2AuthenticationStatement _:
              case Saml2AttributeStatement _:
              case Saml2AuthorizationDecisionStatement _:
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4119")));
              default:
                continue;
            }
          }
        }
        writer.WriteStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion");
        writer.WriteAttributeString("ID", data.Id.Value);
        writer.WriteAttributeString("IssueInstant", XmlConvert.ToString(data.IssueInstant.ToUniversalTime(), DateTimeFormats.Generated));
        writer.WriteAttributeString("Version", data.Version);
        this.WriteIssuer(writer, data.Issuer);
        envelopedSignatureWriter?.WriteSignature();
        if (data.Subject != null)
          this.WriteSubject(writer, data.Subject);
        if (data.Conditions != null)
          this.WriteConditions(writer, data.Conditions);
        if (data.Advice != null)
          this.WriteAdvice(writer, data.Advice);
        foreach (Saml2Statement statement in data.Statements)
          this.WriteStatement(writer, statement);
        writer.WriteEndElement();
      }
      if (dictionaryWriter == null)
        return;
      dictionaryWriter.Dispose();
      EncryptedDataElement encryptedDataElement = new EncryptedDataElement();
      encryptedDataElement.Type = "http://www.w3.org/2001/04/xmlenc#Element";
      encryptedDataElement.Algorithm = data.EncryptingCredentials.Algorithm;
      encryptedDataElement.KeyIdentifier = data.EncryptingCredentials.SecurityKeyIdentifier;
      if (!(data.EncryptingCredentials.SecurityKey is SymmetricSecurityKey securityKey))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID3064")));
      SymmetricAlgorithm symmetricAlgorithm = securityKey.GetSymmetricAlgorithm(data.EncryptingCredentials.Algorithm);
      encryptedDataElement.Encrypt(symmetricAlgorithm, memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
      memoryStream.Dispose();
      writer1.WriteStartElement("EncryptedAssertion", "urn:oasis:names:tc:SAML:2.0:assertion");
      encryptedDataElement.WriteXml(writer1, this.KeyInfoSerializer);
      foreach (EncryptedKeyIdentifierClause externalEncryptedKey in data.ExternalEncryptedKeys)
        this.KeyInfoSerializer.WriteKeyIdentifierClause(writer1, (SecurityKeyIdentifierClause) externalEncryptedKey);
      writer1.WriteEndElement();
    }

    protected virtual Saml2Attribute ReadAttribute(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        bool isEmptyElement1 = reader.IsEmptyElement;
        XmlUtil.ValidateXsiType(reader, "AttributeType", "urn:oasis:names:tc:SAML:2.0:assertion");
        string attribute1 = reader.GetAttribute("Name");
        Saml2Attribute attribute2 = !string.IsNullOrEmpty(attribute1) ? new Saml2Attribute(attribute1) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0001", (object) "Name", (object) "Attribute"));
        string attribute3 = reader.GetAttribute("NameFormat");
        if (!string.IsNullOrEmpty(attribute3))
          attribute2.NameFormat = UriUtil.CanCreateValidUri(attribute3, UriKind.Absolute) ? new Uri(attribute3) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0011", (object) "Namespace", (object) "Action"));
        attribute2.FriendlyName = reader.GetAttribute("FriendlyName");
        string str1 = reader.GetAttribute("OriginalIssuer", "http://schemas.xmlsoap.org/ws/2009/09/identity/claims") ?? reader.GetAttribute("OriginalIssuer", "http://schemas.microsoft.com/ws/2008/06/identity");
        attribute2.OriginalIssuer = !(str1 == string.Empty) ? str1 : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID4252")));
        reader.Read();
        if (!isEmptyElement1)
        {
          while (reader.IsStartElement("AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion"))
          {
            bool isEmptyElement2 = reader.IsEmptyElement;
            bool flag = XmlUtil.IsNil(reader);
            string str2 = (string) null;
            string str3 = (string) null;
            string attribute4 = reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
            if (!string.IsNullOrEmpty(attribute4))
            {
              if (attribute4.IndexOf(":", StringComparison.Ordinal) == -1)
              {
                str2 = reader.LookupNamespace(string.Empty);
                str3 = attribute4;
              }
              else if (attribute4.IndexOf(":", StringComparison.Ordinal) > 0 && attribute4.IndexOf(":", StringComparison.Ordinal) < attribute4.Length - 1)
              {
                string prefix = attribute4.Substring(0, attribute4.IndexOf(":", StringComparison.Ordinal));
                str2 = reader.LookupNamespace(prefix);
                str3 = attribute4.Substring(attribute4.IndexOf(":", StringComparison.Ordinal) + 1);
              }
            }
            if (str2 != null && str3 != null)
              attribute2.AttributeValueXsiType = str2 + "#" + str3;
            if (flag)
            {
              reader.Read();
              if (!isEmptyElement2)
                reader.ReadEndElement();
              attribute2.Values.Add((string) null);
            }
            else if (isEmptyElement2)
            {
              reader.Read();
              attribute2.Values.Add("");
            }
            else
              attribute2.Values.Add(this.ReadAttributeValue(reader, attribute2));
          }
          reader.ReadEndElement();
        }
        return attribute2;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual string ReadAttributeValue(XmlReader reader, Saml2Attribute attribute)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      string str = string.Empty;
      reader.ReadStartElement("AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion");
      int content1 = (int) reader.MoveToContent();
      if (reader.NodeType == XmlNodeType.Element)
      {
        while (reader.NodeType == XmlNodeType.Element)
        {
          str += reader.ReadOuterXml();
          int content2 = (int) reader.MoveToContent();
        }
      }
      else
        str = reader.ReadContentAsString();
      reader.ReadEndElement();
      return str;
    }

    protected virtual void WriteAttribute(XmlWriter writer, Saml2Attribute data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      writer.WriteStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion");
      writer.WriteAttributeString("Name", data.Name);
      if ((Uri) null != data.NameFormat)
        writer.WriteAttributeString("NameFormat", data.NameFormat.AbsoluteUri);
      if (data.FriendlyName != null)
        writer.WriteAttributeString("FriendlyName", data.FriendlyName);
      if (data.OriginalIssuer != null)
        writer.WriteAttributeString("OriginalIssuer", "http://schemas.xmlsoap.org/ws/2009/09/identity/claims", data.OriginalIssuer);
      string str1 = (string) null;
      string str2 = (string) null;
      if (!StringComparer.Ordinal.Equals(data.AttributeValueXsiType, "http://www.w3.org/2001/XMLSchema#string"))
      {
        int length = data.AttributeValueXsiType.IndexOf('#');
        str1 = data.AttributeValueXsiType.Substring(0, length);
        str2 = data.AttributeValueXsiType.Substring(length + 1);
      }
      foreach (string str3 in data.Values)
      {
        writer.WriteStartElement("AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion");
        if (str3 == null)
          writer.WriteAttributeString("nil", "http://www.w3.org/2001/XMLSchema-instance", XmlConvert.ToString(true));
        else if (str3.Length > 0)
        {
          if (str1 != null && str2 != null)
          {
            writer.WriteAttributeString("xmlns", "tn", (string) null, str1);
            writer.WriteAttributeString("type", "http://www.w3.org/2001/XMLSchema-instance", "tn:" + str2);
          }
          this.WriteAttributeValue(writer, str3, data);
        }
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }

    protected virtual void WriteAttributeValue(
      XmlWriter writer,
      string value,
      Saml2Attribute attribute)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      writer.WriteString(value);
    }

    protected virtual Saml2AttributeStatement ReadAttributeStatement(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      bool requireDeclaration = false;
      if (reader.IsStartElement("Statement", "urn:oasis:names:tc:SAML:2.0:assertion"))
        requireDeclaration = true;
      else if (!reader.IsStartElement("AttributeStatement", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("AttributeStatement", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        bool isEmptyElement = reader.IsEmptyElement;
        XmlUtil.ValidateXsiType(reader, "AttributeStatementType", "urn:oasis:names:tc:SAML:2.0:assertion", requireDeclaration);
        if (isEmptyElement)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID3061", (object) "AttributeStatement", (object) "urn:oasis:names:tc:SAML:2.0:assertion"));
        Saml2AttributeStatement attributeStatement = new Saml2AttributeStatement();
        reader.Read();
        while (reader.IsStartElement())
        {
          if (reader.IsStartElement("EncryptedAttribute", "urn:oasis:names:tc:SAML:2.0:assertion"))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4158"));
          if (reader.IsStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion"))
            attributeStatement.Attributes.Add(this.ReadAttribute(reader));
          else
            break;
        }
        if (attributeStatement.Attributes.Count == 0)
          reader.ReadStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion");
        reader.ReadEndElement();
        return attributeStatement;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteAttributeStatement(XmlWriter writer, Saml2AttributeStatement data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      if (data.Attributes == null || data.Attributes.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4124")));
      writer.WriteStartElement("AttributeStatement", "urn:oasis:names:tc:SAML:2.0:assertion");
      foreach (Saml2Attribute attribute in data.Attributes)
        this.WriteAttribute(writer, attribute);
      writer.WriteEndElement();
    }

    protected virtual Saml2AudienceRestriction ReadAudienceRestriction(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      bool requireDeclaration = false;
      if (reader.IsStartElement("Condition", "urn:oasis:names:tc:SAML:2.0:assertion"))
        requireDeclaration = true;
      else if (!reader.IsStartElement("AudienceRestriction", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("AudienceRestriction", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        bool isEmptyElement = reader.IsEmptyElement;
        XmlUtil.ValidateXsiType(reader, "AudienceRestrictionType", "urn:oasis:names:tc:SAML:2.0:assertion", requireDeclaration);
        if (isEmptyElement)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID3061", (object) reader.LocalName, (object) reader.NamespaceURI));
        reader.Read();
        if (!reader.IsStartElement("Audience", "urn:oasis:names:tc:SAML:2.0:assertion"))
          reader.ReadStartElement("Audience", "urn:oasis:names:tc:SAML:2.0:assertion");
        Saml2AudienceRestriction audienceRestriction = new Saml2AudienceRestriction(Saml2SecurityTokenHandler.ReadSimpleUriElement(reader, UriKind.RelativeOrAbsolute, true));
        while (reader.IsStartElement("Audience", "urn:oasis:names:tc:SAML:2.0:assertion"))
          audienceRestriction.Audiences.Add(Saml2SecurityTokenHandler.ReadSimpleUriElement(reader, UriKind.RelativeOrAbsolute, true));
        reader.ReadEndElement();
        return audienceRestriction;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteAudienceRestriction(XmlWriter writer, Saml2AudienceRestriction data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      if (data.Audiences == null || data.Audiences.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4159")));
      writer.WriteStartElement("AudienceRestriction", "urn:oasis:names:tc:SAML:2.0:assertion");
      foreach (Uri audience in data.Audiences)
        writer.WriteElementString("Audience", "urn:oasis:names:tc:SAML:2.0:assertion", audience.OriginalString);
      writer.WriteEndElement();
    }

    protected virtual Saml2AuthenticationContext ReadAuthenticationContext(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("AuthnContext", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("AuthnContext", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        if (reader.IsEmptyElement)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID3061", (object) "AuthnContext", (object) "urn:oasis:names:tc:SAML:2.0:assertion"));
        XmlUtil.ValidateXsiType(reader, "AuthnContextType", "urn:oasis:names:tc:SAML:2.0:assertion");
        reader.ReadStartElement();
        Uri classReference = (Uri) null;
        Uri declarationReference = (Uri) null;
        if (reader.IsStartElement("AuthnContextClassRef", "urn:oasis:names:tc:SAML:2.0:assertion"))
          classReference = Saml2SecurityTokenHandler.ReadSimpleUriElement(reader);
        if (reader.IsStartElement("AuthnContextDecl", "urn:oasis:names:tc:SAML:2.0:assertion"))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4118"));
        if (reader.IsStartElement("AuthnContextDeclRef", "urn:oasis:names:tc:SAML:2.0:assertion"))
          declarationReference = Saml2SecurityTokenHandler.ReadSimpleUriElement(reader);
        else if ((Uri) null == classReference)
          reader.ReadStartElement("AuthnContextDeclRef", "urn:oasis:names:tc:SAML:2.0:assertion");
        Saml2AuthenticationContext authenticationContext = new Saml2AuthenticationContext(classReference, declarationReference);
        while (reader.IsStartElement("AuthenticatingAuthority", "urn:oasis:names:tc:SAML:2.0:assertion"))
          authenticationContext.AuthenticatingAuthorities.Add(Saml2SecurityTokenHandler.ReadSimpleUriElement(reader));
        reader.ReadEndElement();
        return authenticationContext;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteAuthenticationContext(
      XmlWriter writer,
      Saml2AuthenticationContext data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      if ((Uri) null == data.ClassReference && (Uri) null == data.DeclarationReference)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4117")));
      writer.WriteStartElement("AuthnContext", "urn:oasis:names:tc:SAML:2.0:assertion");
      if ((Uri) null != data.ClassReference)
        writer.WriteElementString("AuthnContextClassRef", "urn:oasis:names:tc:SAML:2.0:assertion", data.ClassReference.AbsoluteUri);
      if ((Uri) null != data.DeclarationReference)
        writer.WriteElementString("AuthnContextDeclRef", "urn:oasis:names:tc:SAML:2.0:assertion", data.DeclarationReference.AbsoluteUri);
      foreach (Uri authenticatingAuthority in data.AuthenticatingAuthorities)
        writer.WriteElementString("AuthenticatingAuthority", "urn:oasis:names:tc:SAML:2.0:assertion", authenticatingAuthority.AbsoluteUri);
      writer.WriteEndElement();
    }

    protected virtual Saml2AuthenticationStatement ReadAuthenticationStatement(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      bool requireDeclaration = false;
      if (reader.IsStartElement("Statement", "urn:oasis:names:tc:SAML:2.0:assertion"))
        requireDeclaration = true;
      else if (!reader.IsStartElement("AuthnStatement", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("AuthnStatement", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        DateTime? nullable = new DateTime?();
        Saml2SubjectLocality saml2SubjectLocality = (Saml2SubjectLocality) null;
        bool isEmptyElement = reader.IsEmptyElement;
        XmlUtil.ValidateXsiType(reader, "AuthnStatementType", "urn:oasis:names:tc:SAML:2.0:assertion", requireDeclaration);
        if (isEmptyElement)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID3061", (object) "AuthnStatement", (object) "urn:oasis:names:tc:SAML:2.0:assertion"));
        string attribute1 = reader.GetAttribute("AuthnInstant");
        DateTime authenticationInstant = !string.IsNullOrEmpty(attribute1) ? XmlConvert.ToDateTime(attribute1, DateTimeFormats.Accepted) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0001", (object) "AuthnInstant", (object) "AuthnStatement"));
        string attribute2 = reader.GetAttribute("SessionIndex");
        string attribute3 = reader.GetAttribute("SessionNotOnOrAfter");
        if (!string.IsNullOrEmpty(attribute3))
          nullable = new DateTime?(XmlConvert.ToDateTime(attribute3, DateTimeFormats.Accepted));
        reader.Read();
        if (reader.IsStartElement("SubjectLocality", "urn:oasis:names:tc:SAML:2.0:assertion"))
          saml2SubjectLocality = this.ReadSubjectLocality(reader);
        Saml2AuthenticationContext authenticationContext = this.ReadAuthenticationContext(reader);
        reader.ReadEndElement();
        return new Saml2AuthenticationStatement(authenticationContext, authenticationInstant)
        {
          SessionIndex = attribute2,
          SessionNotOnOrAfter = nullable,
          SubjectLocality = saml2SubjectLocality
        };
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteAuthenticationStatement(
      XmlWriter writer,
      Saml2AuthenticationStatement data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      writer.WriteStartElement("AuthnStatement", "urn:oasis:names:tc:SAML:2.0:assertion");
      writer.WriteAttributeString("AuthnInstant", XmlConvert.ToString(data.AuthenticationInstant.ToUniversalTime(), DateTimeFormats.Generated));
      if (data.SessionIndex != null)
        writer.WriteAttributeString("SessionIndex", data.SessionIndex);
      if (data.SessionNotOnOrAfter.HasValue)
        writer.WriteAttributeString("SessionNotOnOrAfter", XmlConvert.ToString(data.SessionNotOnOrAfter.Value.ToUniversalTime(), DateTimeFormats.Generated));
      if (data.SubjectLocality != null)
        this.WriteSubjectLocality(writer, data.SubjectLocality);
      this.WriteAuthenticationContext(writer, data.AuthenticationContext);
      writer.WriteEndElement();
    }

    protected virtual Saml2AuthorizationDecisionStatement ReadAuthorizationDecisionStatement(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      bool requireDeclaration = false;
      if (reader.IsStartElement("Statement", "urn:oasis:names:tc:SAML:2.0:assertion"))
        requireDeclaration = true;
      else if (!reader.IsStartElement("AuthzDecisionStatement", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("AuthzDecisionStatement", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        bool isEmptyElement = reader.IsEmptyElement;
        XmlUtil.ValidateXsiType(reader, "AuthzDecisionStatementType", "urn:oasis:names:tc:SAML:2.0:assertion", requireDeclaration);
        if (isEmptyElement)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID3061", (object) "AuthzDecisionStatement", (object) "urn:oasis:names:tc:SAML:2.0:assertion"));
        string attribute1 = reader.GetAttribute("Decision");
        if (string.IsNullOrEmpty(attribute1))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0001", (object) "Decision", (object) "AuthzDecisionStatement"));
        SamlAccessDecision decision;
        if (StringComparer.Ordinal.Equals(SamlAccessDecision.Permit.ToString(), attribute1))
          decision = SamlAccessDecision.Permit;
        else if (StringComparer.Ordinal.Equals(SamlAccessDecision.Deny.ToString(), attribute1))
          decision = SamlAccessDecision.Deny;
        else if (StringComparer.Ordinal.Equals(SamlAccessDecision.Indeterminate.ToString(), attribute1))
          decision = SamlAccessDecision.Indeterminate;
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4123", (object) attribute1));
        string attribute2 = reader.GetAttribute("Resource");
        if (attribute2 == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0001", (object) "Resource", (object) "AuthzDecisionStatement"));
        Uri resource;
        if (attribute2.Length == 0)
          resource = Saml2AuthorizationDecisionStatement.EmptyResource;
        else
          resource = UriUtil.CanCreateValidUri(attribute2, UriKind.Absolute) ? new Uri(attribute2) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4121"));
        Saml2AuthorizationDecisionStatement decisionStatement = new Saml2AuthorizationDecisionStatement(resource, decision);
        reader.Read();
        do
        {
          decisionStatement.Actions.Add(this.ReadAction(reader));
        }
        while (reader.IsStartElement("Action", "urn:oasis:names:tc:SAML:2.0:assertion"));
        if (reader.IsStartElement("Evidence", "urn:oasis:names:tc:SAML:2.0:assertion"))
          decisionStatement.Evidence = this.ReadEvidence(reader);
        reader.ReadEndElement();
        return decisionStatement;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteAuthorizationDecisionStatement(
      XmlWriter writer,
      Saml2AuthorizationDecisionStatement data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      if (data.Actions.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4122")));
      writer.WriteStartElement("AuthzDecisionStatement", "urn:oasis:names:tc:SAML:2.0:assertion");
      writer.WriteAttributeString("Decision", data.Decision.ToString());
      writer.WriteAttributeString("Resource", data.Resource.Equals((object) Saml2AuthorizationDecisionStatement.EmptyResource) ? data.Resource.ToString() : data.Resource.AbsoluteUri);
      foreach (Saml2Action action in data.Actions)
        this.WriteAction(writer, action);
      if (data.Evidence != null)
        this.WriteEvidence(writer, data.Evidence);
      writer.WriteEndElement();
    }

    protected virtual Saml2Conditions ReadConditions(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("Conditions", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("Conditions", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        Saml2Conditions saml2Conditions = new Saml2Conditions();
        bool isEmptyElement = reader.IsEmptyElement;
        XmlUtil.ValidateXsiType(reader, "ConditionsType", "urn:oasis:names:tc:SAML:2.0:assertion");
        string attribute1 = reader.GetAttribute("NotBefore");
        if (!string.IsNullOrEmpty(attribute1))
          saml2Conditions.NotBefore = new DateTime?(XmlConvert.ToDateTime(attribute1, DateTimeFormats.Accepted));
        string attribute2 = reader.GetAttribute("NotOnOrAfter");
        if (!string.IsNullOrEmpty(attribute2))
          saml2Conditions.NotOnOrAfter = new DateTime?(XmlConvert.ToDateTime(attribute2, DateTimeFormats.Accepted));
        reader.ReadStartElement();
        if (!isEmptyElement)
        {
          while (reader.IsStartElement())
          {
            if (reader.IsStartElement("Condition", "urn:oasis:names:tc:SAML:2.0:assertion"))
            {
              XmlQualifiedName xsiType = XmlUtil.GetXsiType(reader);
              if ((XmlQualifiedName) null == xsiType || XmlUtil.EqualsQName(xsiType, "ConditionAbstractType", "urn:oasis:names:tc:SAML:2.0:assertion"))
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4104", (object) reader.LocalName, (object) reader.NamespaceURI));
              if (XmlUtil.EqualsQName(xsiType, "AudienceRestrictionType", "urn:oasis:names:tc:SAML:2.0:assertion"))
                saml2Conditions.AudienceRestrictions.Add(this.ReadAudienceRestriction(reader));
              else if (XmlUtil.EqualsQName(xsiType, "OneTimeUseType", "urn:oasis:names:tc:SAML:2.0:assertion"))
              {
                if (saml2Conditions.OneTimeUse)
                  throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4115", (object) "OneTimeUse"));
                Saml2SecurityTokenHandler.ReadEmptyContentElement(reader);
                saml2Conditions.OneTimeUse = true;
              }
              else
              {
                if (!XmlUtil.EqualsQName(xsiType, "ProxyRestrictionType", "urn:oasis:names:tc:SAML:2.0:assertion"))
                  throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4113"));
                saml2Conditions.ProxyRestriction = saml2Conditions.ProxyRestriction == null ? this.ReadProxyRestriction(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4115", (object) "ProxyRestriction"));
              }
            }
            else if (reader.IsStartElement("AudienceRestriction", "urn:oasis:names:tc:SAML:2.0:assertion"))
              saml2Conditions.AudienceRestrictions.Add(this.ReadAudienceRestriction(reader));
            else if (reader.IsStartElement("OneTimeUse", "urn:oasis:names:tc:SAML:2.0:assertion"))
            {
              if (saml2Conditions.OneTimeUse)
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4115", (object) "OneTimeUse"));
              Saml2SecurityTokenHandler.ReadEmptyContentElement(reader);
              saml2Conditions.OneTimeUse = true;
            }
            else if (reader.IsStartElement("ProxyRestriction", "urn:oasis:names:tc:SAML:2.0:assertion"))
              saml2Conditions.ProxyRestriction = saml2Conditions.ProxyRestriction == null ? this.ReadProxyRestriction(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4115", (object) "ProxyRestriction"));
            else
              break;
          }
          reader.ReadEndElement();
        }
        return saml2Conditions;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteConditions(XmlWriter writer, Saml2Conditions data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      writer.WriteStartElement("Conditions", "urn:oasis:names:tc:SAML:2.0:assertion");
      if (data.NotBefore.HasValue)
        writer.WriteAttributeString("NotBefore", XmlConvert.ToString(data.NotBefore.Value.ToUniversalTime(), DateTimeFormats.Generated));
      if (data.NotOnOrAfter.HasValue)
        writer.WriteAttributeString("NotOnOrAfter", XmlConvert.ToString(data.NotOnOrAfter.Value.ToUniversalTime(), DateTimeFormats.Generated));
      foreach (Saml2AudienceRestriction audienceRestriction in data.AudienceRestrictions)
        this.WriteAudienceRestriction(writer, audienceRestriction);
      if (data.OneTimeUse)
      {
        writer.WriteStartElement("OneTimeUse", "urn:oasis:names:tc:SAML:2.0:assertion");
        writer.WriteEndElement();
      }
      if (data.ProxyRestriction != null)
        this.WriteProxyRestriction(writer, data.ProxyRestriction);
      writer.WriteEndElement();
    }

    protected virtual Saml2Evidence ReadEvidence(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("Evidence", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("Evidence", "urn:oasis:names:tc:SAML:2.0:assertion");
      if (reader.IsEmptyElement)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID3061", (object) "Evidence", (object) "urn:oasis:names:tc:SAML:2.0:assertion"));
      try
      {
        Saml2Evidence saml2Evidence = new Saml2Evidence();
        XmlUtil.ValidateXsiType(reader, "EvidenceType", "urn:oasis:names:tc:SAML:2.0:assertion");
        reader.Read();
        while (reader.IsStartElement())
        {
          if (reader.IsStartElement("AssertionIDRef", "urn:oasis:names:tc:SAML:2.0:assertion"))
            saml2Evidence.AssertionIdReferences.Add(Saml2SecurityTokenHandler.ReadSimpleNCNameElement(reader));
          else if (reader.IsStartElement("AssertionURIRef", "urn:oasis:names:tc:SAML:2.0:assertion"))
            saml2Evidence.AssertionUriReferences.Add(Saml2SecurityTokenHandler.ReadSimpleUriElement(reader));
          else if (reader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
            saml2Evidence.Assertions.Add(this.ReadAssertion(reader));
          else if (reader.IsStartElement("EncryptedAssertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
            saml2Evidence.Assertions.Add(this.ReadAssertion(reader));
        }
        if (saml2Evidence.AssertionIdReferences.Count == 0 && saml2Evidence.Assertions.Count == 0 && saml2Evidence.AssertionUriReferences.Count == 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4120"));
        reader.ReadEndElement();
        return saml2Evidence;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteEvidence(XmlWriter writer, Saml2Evidence data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      if ((data.AssertionIdReferences == null || data.AssertionIdReferences.Count == 0) && (data.Assertions == null || data.Assertions.Count == 0) && (data.AssertionUriReferences == null || data.AssertionUriReferences.Count == 0))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4120")));
      writer.WriteStartElement("Evidence", "urn:oasis:names:tc:SAML:2.0:assertion");
      foreach (Saml2Id assertionIdReference in data.AssertionIdReferences)
        writer.WriteElementString("AssertionIDRef", "urn:oasis:names:tc:SAML:2.0:assertion", assertionIdReference.Value);
      foreach (Uri assertionUriReference in data.AssertionUriReferences)
        writer.WriteElementString("AssertionURIRef", "urn:oasis:names:tc:SAML:2.0:assertion", assertionUriReference.AbsoluteUri);
      foreach (Saml2Assertion assertion in data.Assertions)
        this.WriteAssertion(writer, assertion);
      writer.WriteEndElement();
    }

    protected virtual Saml2NameIdentifier ReadIssuer(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("Issuer", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
      return this.ReadNameIDType(reader);
    }

    protected virtual void WriteIssuer(XmlWriter writer, Saml2NameIdentifier data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      writer.WriteStartElement("Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
      this.WriteNameIDType(writer, data);
      writer.WriteEndElement();
    }

    protected virtual SecurityKeyIdentifier ReadSubjectKeyInfo(XmlReader reader) => reader != null ? this.KeyInfoSerializer.ReadKeyIdentifier(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));

    protected virtual SecurityKeyIdentifier ReadSigningKeyInfo(
      XmlReader reader,
      Saml2Assertion assertion)
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
        (SecurityKeyIdentifierClause) new Saml2SecurityKeyIdentifierClause(assertion)
      });
    }

    protected virtual void WriteSubjectKeyInfo(XmlWriter writer, SecurityKeyIdentifier data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      this.KeyInfoSerializer.WriteKeyIdentifier(writer, data);
    }

    protected virtual void WriteSigningKeyInfo(
      XmlWriter writer,
      SecurityKeyIdentifier signingKeyIdentifier)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (signingKeyIdentifier == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
      if (this.KeyInfoSerializer.CanWriteKeyIdentifier(signingKeyIdentifier))
        this.KeyInfoSerializer.WriteKeyIdentifier(writer, signingKeyIdentifier);
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4221", (object) signingKeyIdentifier));
    }

    protected virtual Saml2NameIdentifier ReadNameID(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("NameID", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("NameID", "urn:oasis:names:tc:SAML:2.0:assertion");
      return this.ReadNameIDType(reader);
    }

    protected virtual void WriteNameID(XmlWriter writer, Saml2NameIdentifier data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      if (data.EncryptingCredentials != null)
      {
        Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = data.EncryptingCredentials;
        if (!(encryptingCredentials.SecurityKey is SymmetricSecurityKey securityKey))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID3284")));
        MemoryStream memoryStream = (MemoryStream) null;
        try
        {
          memoryStream = new MemoryStream();
          using (XmlWriter textWriter = (XmlWriter) XmlDictionaryWriter.CreateTextWriter((Stream) memoryStream, Encoding.UTF8, false))
          {
            textWriter.WriteStartElement("NameID", "urn:oasis:names:tc:SAML:2.0:assertion");
            this.WriteNameIDType(textWriter, data);
            textWriter.WriteEndElement();
          }
          EncryptedDataElement encryptedDataElement = new EncryptedDataElement();
          encryptedDataElement.Type = "http://www.w3.org/2001/04/xmlenc#Element";
          encryptedDataElement.Algorithm = encryptingCredentials.Algorithm;
          encryptedDataElement.KeyIdentifier = encryptingCredentials.SecurityKeyIdentifier;
          SymmetricAlgorithm symmetricAlgorithm = securityKey.GetSymmetricAlgorithm(encryptingCredentials.Algorithm);
          encryptedDataElement.Encrypt(symmetricAlgorithm, memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
          memoryStream.Dispose();
          writer.WriteStartElement("EncryptedID", "urn:oasis:names:tc:SAML:2.0:assertion");
          encryptedDataElement.WriteXml(writer, this.KeyInfoSerializer);
          foreach (EncryptedKeyIdentifierClause externalEncryptedKey in data.ExternalEncryptedKeys)
            this.KeyInfoSerializer.WriteKeyIdentifierClause(writer, (SecurityKeyIdentifierClause) externalEncryptedKey);
          writer.WriteEndElement();
        }
        finally
        {
          memoryStream?.Dispose();
        }
      }
      else
      {
        writer.WriteStartElement("NameID", "urn:oasis:names:tc:SAML:2.0:assertion");
        this.WriteNameIDType(writer, data);
        writer.WriteEndElement();
      }
    }

    protected virtual Saml2NameIdentifier ReadNameIDType(XmlReader reader)
    {
      try
      {
        int content = (int) reader.MoveToContent();
        Saml2NameIdentifier saml2NameIdentifier = new Saml2NameIdentifier("__TemporaryName__");
        XmlUtil.ValidateXsiType(reader, "NameIDType", "urn:oasis:names:tc:SAML:2.0:assertion");
        string attribute1 = reader.GetAttribute("Format");
        if (!string.IsNullOrEmpty(attribute1))
          saml2NameIdentifier.Format = UriUtil.CanCreateValidUri(attribute1, UriKind.Absolute) ? new Uri(attribute1) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0011", (object) "Format", (object) "NameID"));
        string attribute2 = reader.GetAttribute("NameQualifier");
        if (!string.IsNullOrEmpty(attribute2))
          saml2NameIdentifier.NameQualifier = attribute2;
        string attribute3 = reader.GetAttribute("SPNameQualifier");
        if (!string.IsNullOrEmpty(attribute3))
          saml2NameIdentifier.SPNameQualifier = attribute3;
        string attribute4 = reader.GetAttribute("SPProvidedID");
        if (!string.IsNullOrEmpty(attribute4))
          saml2NameIdentifier.SPProvidedId = attribute4;
        saml2NameIdentifier.Value = reader.ReadElementString();
        if (saml2NameIdentifier.Format != (Uri) null && StringComparer.Ordinal.Equals(saml2NameIdentifier.Format.AbsoluteUri, Saml2Constants.NameIdentifierFormats.Entity.AbsoluteUri))
        {
          if (!UriUtil.CanCreateValidUri(saml2NameIdentifier.Value, UriKind.Absolute))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4262", (object) saml2NameIdentifier.Value, (object) Saml2Constants.NameIdentifierFormats.Entity.AbsoluteUri));
          if (!string.IsNullOrEmpty(saml2NameIdentifier.NameQualifier) || !string.IsNullOrEmpty(saml2NameIdentifier.SPNameQualifier) || !string.IsNullOrEmpty(saml2NameIdentifier.SPProvidedId))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4263", (object) saml2NameIdentifier.Value, (object) Saml2Constants.NameIdentifierFormats.Entity.AbsoluteUri));
        }
        return saml2NameIdentifier;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual Saml2NameIdentifier ReadEncryptedId(XmlReader reader)
    {
      int num = reader != null ? (int) reader.MoveToContent() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("EncryptedID", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("EncryptedID", "urn:oasis:names:tc:SAML:2.0:assertion");
      Collection<EncryptedKeyIdentifierClause> clauses = new Collection<EncryptedKeyIdentifierClause>();
      Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = (Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials) null;
      Saml2NameIdentifier saml2NameIdentifier = (Saml2NameIdentifier) null;
      using (StringReader stringReader1 = new StringReader(reader.ReadOuterXml()))
      {
        StringReader stringReader2 = stringReader1;
        XmlReaderSettings settings = new XmlReaderSettings()
        {
          XmlResolver = (XmlResolver) null
        };
        using (XmlDictionaryReader reader1 = (XmlDictionaryReader) new IdentityModelWrappedXmlDictionaryReader(XmlReader.Create((TextReader) stringReader2, settings), XmlDictionaryReaderQuotas.Max))
        {
          saml2NameIdentifier = this.ReadNameIDType((XmlReader) Saml2SecurityTokenHandler.CreatePlaintextReaderFromEncryptedData(reader1, this.Configuration.ServiceTokenResolver, this.KeyInfoSerializer, clauses, out encryptingCredentials));
          saml2NameIdentifier.EncryptingCredentials = encryptingCredentials;
          foreach (EncryptedKeyIdentifierClause identifierClause in clauses)
            saml2NameIdentifier.ExternalEncryptedKeys.Add(identifierClause);
        }
      }
      return saml2NameIdentifier;
    }

    protected virtual void WriteNameIDType(XmlWriter writer, Saml2NameIdentifier data)
    {
      if ((Uri) null != data.Format)
        writer.WriteAttributeString("Format", data.Format.AbsoluteUri);
      if (!string.IsNullOrEmpty(data.NameQualifier))
        writer.WriteAttributeString("NameQualifier", data.NameQualifier);
      if (!string.IsNullOrEmpty(data.SPNameQualifier))
        writer.WriteAttributeString("SPNameQualifier", data.SPNameQualifier);
      if (!string.IsNullOrEmpty(data.SPProvidedId))
        writer.WriteAttributeString("SPProvidedID", data.SPProvidedId);
      writer.WriteString(data.Value);
    }

    protected virtual Saml2ProxyRestriction ReadProxyRestriction(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      bool requireDeclaration = false;
      if (reader.IsStartElement("Condition", "urn:oasis:names:tc:SAML:2.0:assertion"))
        requireDeclaration = true;
      else if (!reader.IsStartElement("ProxyRestriction", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("ProxyRestriction", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        Saml2ProxyRestriction proxyRestriction = new Saml2ProxyRestriction();
        bool isEmptyElement = reader.IsEmptyElement;
        XmlUtil.ValidateXsiType(reader, "ProxyRestrictionType", "urn:oasis:names:tc:SAML:2.0:assertion", requireDeclaration);
        string attribute = reader.GetAttribute("Count");
        if (!string.IsNullOrEmpty(attribute))
          proxyRestriction.Count = new int?(XmlConvert.ToInt32(attribute));
        reader.Read();
        if (!isEmptyElement)
        {
          while (reader.IsStartElement("Audience", "urn:oasis:names:tc:SAML:2.0:assertion"))
            proxyRestriction.Audiences.Add(Saml2SecurityTokenHandler.ReadSimpleUriElement(reader));
          reader.ReadEndElement();
        }
        return proxyRestriction;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteProxyRestriction(XmlWriter writer, Saml2ProxyRestriction data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      writer.WriteStartElement("ProxyRestriction", "urn:oasis:names:tc:SAML:2.0:assertion");
      if (data.Count.HasValue)
        writer.WriteAttributeString("Count", XmlConvert.ToString(data.Count.Value));
      foreach (Uri audience in data.Audiences)
        writer.WriteElementString("Audience", audience.AbsoluteUri);
      writer.WriteEndElement();
    }

    protected virtual Saml2Statement ReadStatement(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("Statement", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("Statement", "urn:oasis:names:tc:SAML:2.0:assertion");
      XmlQualifiedName xsiType = XmlUtil.GetXsiType(reader);
      if ((XmlQualifiedName) null == xsiType || XmlUtil.EqualsQName(xsiType, "StatementAbstractType", "urn:oasis:names:tc:SAML:2.0:assertion"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4104", (object) reader.LocalName, (object) reader.NamespaceURI));
      if (XmlUtil.EqualsQName(xsiType, "AttributeStatementType", "urn:oasis:names:tc:SAML:2.0:assertion"))
        return (Saml2Statement) this.ReadAttributeStatement(reader);
      if (XmlUtil.EqualsQName(xsiType, "AuthnStatementType", "urn:oasis:names:tc:SAML:2.0:assertion"))
        return (Saml2Statement) this.ReadAuthenticationStatement(reader);
      if (XmlUtil.EqualsQName(xsiType, "AuthzDecisionStatementType", "urn:oasis:names:tc:SAML:2.0:assertion"))
        return (Saml2Statement) this.ReadAuthorizationDecisionStatement(reader);
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4105", (object) xsiType.Name, (object) xsiType.Namespace));
    }

    protected virtual void WriteStatement(XmlWriter writer, Saml2Statement data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      switch (data)
      {
        case null:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
        case Saml2AttributeStatement data1:
          this.WriteAttributeStatement(writer, data1);
          break;
        case Saml2AuthenticationStatement data1:
          this.WriteAuthenticationStatement(writer, data1);
          break;
        case Saml2AuthorizationDecisionStatement data1:
          this.WriteAuthorizationDecisionStatement(writer, data1);
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4107", (object) data.GetType().AssemblyQualifiedName)));
      }
    }

    protected virtual Saml2Subject ReadSubject(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("Subject", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("Subject", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        if (reader.IsEmptyElement)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID3061", (object) reader.LocalName, (object) reader.NamespaceURI));
        XmlUtil.ValidateXsiType(reader, "SubjectType", "urn:oasis:names:tc:SAML:2.0:assertion");
        Saml2Subject saml2Subject = new Saml2Subject();
        reader.Read();
        saml2Subject.NameId = this.ReadSubjectID(reader, "Subject");
        while (reader.IsStartElement("SubjectConfirmation", "urn:oasis:names:tc:SAML:2.0:assertion"))
          saml2Subject.SubjectConfirmations.Add(this.ReadSubjectConfirmation(reader));
        reader.ReadEndElement();
        if (saml2Subject.NameId == null && saml2Subject.SubjectConfirmations.Count == 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4108"));
        return saml2Subject;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteSubject(XmlWriter writer, Saml2Subject data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      if (data.NameId == null && data.SubjectConfirmations.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4108")));
      writer.WriteStartElement("Subject", "urn:oasis:names:tc:SAML:2.0:assertion");
      if (data.NameId != null)
        this.WriteNameID(writer, data.NameId);
      foreach (Saml2SubjectConfirmation subjectConfirmation in data.SubjectConfirmations)
        this.WriteSubjectConfirmation(writer, subjectConfirmation);
      writer.WriteEndElement();
    }

    protected virtual Saml2SubjectConfirmation ReadSubjectConfirmation(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("SubjectConfirmation", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("SubjectConfirmation", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        bool isEmptyElement = reader.IsEmptyElement;
        XmlUtil.ValidateXsiType(reader, "SubjectConfirmationType", "urn:oasis:names:tc:SAML:2.0:assertion");
        string attribute = reader.GetAttribute("Method");
        if (string.IsNullOrEmpty(attribute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0001", (object) "Method", (object) "SubjectConfirmation"));
        Saml2SubjectConfirmation subjectConfirmation = UriUtil.CanCreateValidUri(attribute, UriKind.Absolute) ? new Saml2SubjectConfirmation(new Uri(attribute)) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0011", (object) "Method", (object) "SubjectConfirmation"));
        reader.Read();
        if (!isEmptyElement)
        {
          subjectConfirmation.NameIdentifier = this.ReadSubjectID(reader, "SubjectConfirmation");
          if (reader.IsStartElement("SubjectConfirmationData", "urn:oasis:names:tc:SAML:2.0:assertion"))
            subjectConfirmation.SubjectConfirmationData = this.ReadSubjectConfirmationData(reader);
          reader.ReadEndElement();
        }
        return subjectConfirmation;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteSubjectConfirmation(XmlWriter writer, Saml2SubjectConfirmation data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      if ((Uri) null == data.Method)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data.Method");
      if (string.IsNullOrEmpty(data.Method.ToString()))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("data.Method");
      writer.WriteStartElement("SubjectConfirmation", "urn:oasis:names:tc:SAML:2.0:assertion");
      writer.WriteAttributeString("Method", data.Method.AbsoluteUri);
      if (data.NameIdentifier != null)
        this.WriteNameID(writer, data.NameIdentifier);
      if (data.SubjectConfirmationData != null)
        this.WriteSubjectConfirmationData(writer, data.SubjectConfirmationData);
      writer.WriteEndElement();
    }

    protected virtual Saml2SubjectConfirmationData ReadSubjectConfirmationData(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("SubjectConfirmationData", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("SubjectConfirmationData", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        Saml2SubjectConfirmationData confirmationData = new Saml2SubjectConfirmationData();
        bool isEmptyElement = reader.IsEmptyElement;
        bool flag = false;
        XmlQualifiedName xsiType = XmlUtil.GetXsiType(reader);
        if ((XmlQualifiedName) null != xsiType)
        {
          if (XmlUtil.EqualsQName(xsiType, "KeyInfoConfirmationDataType", "urn:oasis:names:tc:SAML:2.0:assertion"))
            flag = true;
          else if (!XmlUtil.EqualsQName(xsiType, "SubjectConfirmationDataType", "urn:oasis:names:tc:SAML:2.0:assertion"))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4112", (object) xsiType.Name, (object) xsiType.Namespace));
        }
        if (flag && isEmptyElement)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString(Microsoft.IdentityModel.SR.GetString("ID4111")));
        string attribute1 = reader.GetAttribute("Address");
        if (!string.IsNullOrEmpty(attribute1))
          confirmationData.Address = attribute1;
        string attribute2 = reader.GetAttribute("InResponseTo");
        if (!string.IsNullOrEmpty(attribute2))
          confirmationData.InResponseTo = new Saml2Id(attribute2);
        string attribute3 = reader.GetAttribute("NotBefore");
        if (!string.IsNullOrEmpty(attribute3))
          confirmationData.NotBefore = new DateTime?(XmlConvert.ToDateTime(attribute3, DateTimeFormats.Accepted));
        string attribute4 = reader.GetAttribute("NotOnOrAfter");
        if (!string.IsNullOrEmpty(attribute4))
          confirmationData.NotOnOrAfter = new DateTime?(XmlConvert.ToDateTime(attribute4, DateTimeFormats.Accepted));
        string attribute5 = reader.GetAttribute("Recipient");
        if (!string.IsNullOrEmpty(attribute5))
          confirmationData.Recipient = UriUtil.CanCreateValidUri(attribute5, UriKind.Absolute) ? new Uri(attribute5) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0011", (object) "Recipient", (object) "SubjectConfirmationData"));
        reader.Read();
        if (!isEmptyElement)
        {
          if (flag)
            confirmationData.KeyIdentifiers.Add(this.ReadSubjectKeyInfo(reader));
          while (reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
            confirmationData.KeyIdentifiers.Add(this.ReadSubjectKeyInfo(reader));
          if (!flag && XmlNodeType.EndElement != reader.NodeType)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID4114", (object) "SubjectConfirmationData"));
          reader.ReadEndElement();
        }
        return confirmationData;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteSubjectConfirmationData(
      XmlWriter writer,
      Saml2SubjectConfirmationData data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      writer.WriteStartElement("SubjectConfirmationData", "urn:oasis:names:tc:SAML:2.0:assertion");
      if (data.KeyIdentifiers != null && data.KeyIdentifiers.Count > 0)
        writer.WriteAttributeString("type", "http://www.w3.org/2001/XMLSchema-instance", "KeyInfoConfirmationDataType");
      if (!string.IsNullOrEmpty(data.Address))
        writer.WriteAttributeString("Address", data.Address);
      if (data.InResponseTo != null)
        writer.WriteAttributeString("InResponseTo", data.InResponseTo.Value);
      if (data.NotBefore.HasValue)
        writer.WriteAttributeString("NotBefore", XmlConvert.ToString(data.NotBefore.Value.ToUniversalTime(), DateTimeFormats.Generated));
      if (data.NotOnOrAfter.HasValue)
        writer.WriteAttributeString("NotOnOrAfter", XmlConvert.ToString(data.NotOnOrAfter.Value.ToUniversalTime(), DateTimeFormats.Generated));
      if ((Uri) null != data.Recipient)
        writer.WriteAttributeString("Recipient", data.Recipient.OriginalString);
      foreach (SecurityKeyIdentifier keyIdentifier in data.KeyIdentifiers)
        this.WriteSubjectKeyInfo(writer, keyIdentifier);
      writer.WriteEndElement();
    }

    protected virtual Saml2SubjectLocality ReadSubjectLocality(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("SubjectLocality", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("SubjectLocality", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        Saml2SubjectLocality saml2SubjectLocality = new Saml2SubjectLocality();
        bool isEmptyElement = reader.IsEmptyElement;
        XmlUtil.ValidateXsiType(reader, "SubjectLocalityType", "urn:oasis:names:tc:SAML:2.0:assertion");
        saml2SubjectLocality.Address = reader.GetAttribute("Address");
        saml2SubjectLocality.DnsName = reader.GetAttribute("DNSName");
        reader.Read();
        if (!isEmptyElement)
          reader.ReadEndElement();
        return saml2SubjectLocality;
      }
      catch (Exception ex)
      {
        Exception exception = Saml2SecurityTokenHandler.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteSubjectLocality(XmlWriter writer, Saml2SubjectLocality data)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (data == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (data));
      writer.WriteStartElement("SubjectLocality", "urn:oasis:names:tc:SAML:2.0:assertion");
      if (data.Address != null)
        writer.WriteAttributeString("Address", data.Address);
      if (data.DnsName != null)
        writer.WriteAttributeString("DNSName", data.DnsName);
      writer.WriteEndElement();
    }

    private class WrappedSerializer : SecurityTokenSerializer
    {
      private Saml2SecurityTokenHandler _parent;
      private Saml2Assertion _assertion;

      public WrappedSerializer(Saml2SecurityTokenHandler parent, Saml2Assertion assertion)
      {
        this._assertion = assertion;
        this._parent = parent;
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

    private class ReceivedEncryptingCredentials : Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials
    {
      public ReceivedEncryptingCredentials(
        SecurityKey key,
        SecurityKeyIdentifier keyIdentifier,
        string algorithm)
        : base(key, keyIdentifier, algorithm)
      {
      }
    }
  }
}
