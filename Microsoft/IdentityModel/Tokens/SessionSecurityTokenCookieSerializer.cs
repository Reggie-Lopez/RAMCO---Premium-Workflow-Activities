// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SessionSecurityTokenCookieSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class SessionSecurityTokenCookieSerializer
  {
    private const string SupportedVersion = "1";
    private const string WindowsSecurityTokenStubElementName = "WindowsSecurityTokenStub";
    private const int MaxDomainNameMapSize = 50;
    private static Dictionary<string, string> DomainNameMap = new Dictionary<string, string>(50, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private static Random rnd = new Random();
    private SecurityTokenHandlerCollection _bootstrapTokenHandlers;
    private bool _saveBootstrapTokens;
    private bool _useWindowsTokenService;
    private string _windowsIssuerName;

    public SessionSecurityTokenCookieSerializer()
      : this(SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(), true, false, "LOCAL AUTHORITY")
    {
    }

    public SessionSecurityTokenCookieSerializer(
      SecurityTokenHandlerCollection bootstrapTokenHandlers,
      bool saveBootstrapTokens,
      bool useWindowsTokenService,
      string windowsIssuerName)
    {
      this._bootstrapTokenHandlers = bootstrapTokenHandlers;
      this._saveBootstrapTokens = saveBootstrapTokens;
      this._useWindowsTokenService = useWindowsTokenService;
      this._windowsIssuerName = windowsIssuerName;
    }

    public SecurityTokenHandlerCollection BootstrapTokenHandler => this._bootstrapTokenHandlers;

    public virtual byte[] Serialize(SessionSecurityToken sessionToken)
    {
      if (sessionToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sessionToken));
      MemoryStream memoryStream = new MemoryStream();
      Microsoft.IdentityModel.Claims.SessionDictionary instance = Microsoft.IdentityModel.Claims.SessionDictionary.Instance;
      using (XmlDictionaryWriter binaryWriter = XmlDictionaryWriter.CreateBinaryWriter((Stream) memoryStream, (IXmlDictionary) instance))
      {
        if (sessionToken.IsSecurityContextSecurityTokenWrapper)
        {
          binaryWriter.WriteStartElement(instance.SecurityContextToken, instance.EmptyString);
        }
        else
        {
          binaryWriter.WriteStartElement(instance.SessionToken, instance.EmptyString);
          if (sessionToken.IsPersistent)
            binaryWriter.WriteAttributeString(instance.PersistentTrue, instance.EmptyString, "");
          if (sessionToken.IsSessionMode)
            binaryWriter.WriteAttributeString(instance.SessionModeTrue, instance.EmptyString, "");
          if (!string.IsNullOrEmpty(sessionToken.Context))
            binaryWriter.WriteElementString(instance.Context, instance.EmptyString, sessionToken.Context);
        }
        binaryWriter.WriteStartElement(instance.Version, instance.EmptyString);
        binaryWriter.WriteValue("1");
        binaryWriter.WriteEndElement();
        binaryWriter.WriteElementString(instance.SecureConversationVersion, instance.EmptyString, sessionToken.SecureConversationVersion.Namespace.Value);
        binaryWriter.WriteElementString(instance.Id, instance.EmptyString, sessionToken.Id);
        XmlUtil.WriteElementStringAsUniqueId(binaryWriter, instance.ContextId, instance.EmptyString, sessionToken.ContextId.ToString());
        byte[] symmetricKey = ((SymmetricSecurityKey) sessionToken.SecurityKeys[0]).GetSymmetricKey();
        binaryWriter.WriteStartElement(instance.Key, instance.EmptyString);
        binaryWriter.WriteBase64(symmetricKey, 0, symmetricKey.Length);
        binaryWriter.WriteEndElement();
        if (sessionToken.KeyGeneration != (System.Xml.UniqueId) null)
          XmlUtil.WriteElementStringAsUniqueId(binaryWriter, instance.KeyGeneration, instance.EmptyString, sessionToken.KeyGeneration.ToString());
        XmlUtil.WriteElementContentAsInt64(binaryWriter, instance.EffectiveTime, instance.EmptyString, sessionToken.ValidFrom.ToUniversalTime().Ticks);
        XmlUtil.WriteElementContentAsInt64(binaryWriter, instance.ExpiryTime, instance.EmptyString, sessionToken.ValidTo.ToUniversalTime().Ticks);
        XmlUtil.WriteElementContentAsInt64(binaryWriter, instance.KeyEffectiveTime, instance.EmptyString, sessionToken.KeyEffectiveTime.ToUniversalTime().Ticks);
        XmlUtil.WriteElementContentAsInt64(binaryWriter, instance.KeyExpiryTime, instance.EmptyString, sessionToken.KeyExpirationTime.ToUniversalTime().Ticks);
        this.WritePrincipal(binaryWriter, instance, sessionToken.ClaimsPrincipal);
        for (int index = 0; index < sessionToken.SecurityContextSecurityToken.AuthorizationPolicies.Count; ++index)
        {
          if (sessionToken.SecurityContextSecurityToken.AuthorizationPolicies[index] is SctAuthorizationPolicy authorizationPolicy)
          {
            binaryWriter.WriteStartElement(instance.SctAuthorizationPolicy, instance.EmptyString);
            this.SerializeSysClaim(((IAuthorizationPolicy) authorizationPolicy).Issuer[0], binaryWriter);
            binaryWriter.WriteEndElement();
          }
        }
        binaryWriter.WriteElementString(instance.EndpointId, instance.EmptyString, sessionToken.EndpointId);
        binaryWriter.WriteEndElement();
        binaryWriter.Flush();
        return memoryStream.ToArray();
      }
    }

    public virtual SessionSecurityToken Deserialize(byte[] cookie)
    {
      if (cookie == null || cookie.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (cookie));
      Microsoft.IdentityModel.Claims.SessionDictionary instance = Microsoft.IdentityModel.Claims.SessionDictionary.Instance;
      using (XmlDictionaryReader binaryReader = XmlDictionaryReader.CreateBinaryReader(cookie, 0, cookie.Length, (IXmlDictionary) instance, XmlDictionaryReaderQuotas.Max, (XmlBinaryReaderSession) null, (OnXmlDictionaryReaderClose) null))
      {
        bool flag1 = false;
        bool flag2 = true;
        bool flag3 = false;
        string context = string.Empty;
        if (binaryReader.IsStartElement(instance.SecurityContextToken, instance.EmptyString))
          flag1 = true;
        else if (binaryReader.IsStartElement(instance.SessionToken, instance.EmptyString))
        {
          if (binaryReader.GetAttribute(instance.PersistentTrue, instance.EmptyString) == null)
            flag2 = false;
          if (binaryReader.GetAttribute(instance.SessionModeTrue, instance.EmptyString) != null)
            flag3 = true;
          binaryReader.ReadFullStartElement();
          int content = (int) binaryReader.MoveToContent();
          if (binaryReader.IsStartElement(instance.Context, instance.EmptyString))
            context = binaryReader.ReadElementContentAsString();
        }
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4230", (object) instance.SecurityContextToken.Value, (object) binaryReader.Name)));
        string str1 = binaryReader.ReadElementString();
        if (str1 != "1")
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4232", (object) str1, (object) "1")));
        string str2 = binaryReader.ReadElementString();
        SecureConversationVersion version;
        if (str2 == SecureConversationVersion.WSSecureConversationFeb2005.Namespace.Value)
          version = SecureConversationVersion.WSSecureConversationFeb2005;
        else if (str2 == SecureConversationVersion.WSSecureConversation13.Namespace.Value)
          version = SecureConversationVersion.WSSecureConversation13;
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4232", (object) str1, (object) "1")));
        string id = (string) null;
        if (binaryReader.IsStartElement(instance.Id, instance.EmptyString))
          id = binaryReader.ReadElementString();
        if (string.IsNullOrEmpty(id))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4239", (object) instance.Id.Value)));
        if (!binaryReader.IsStartElement(instance.ContextId, instance.EmptyString))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4230", (object) instance.ContextId.Value, (object) binaryReader.Name)));
        System.Xml.UniqueId contextId = binaryReader.ReadElementContentAsUniqueId();
        if (!binaryReader.IsStartElement(instance.Key, instance.EmptyString))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4230", (object) instance.Key.Value, (object) binaryReader.Name)));
        byte[] key = binaryReader.ReadElementContentAsBase64();
        System.Xml.UniqueId keyGeneration = (System.Xml.UniqueId) null;
        if (binaryReader.IsStartElement(instance.KeyGeneration, instance.EmptyString))
          keyGeneration = binaryReader.ReadElementContentAsUniqueId();
        if (!binaryReader.IsStartElement(instance.EffectiveTime, instance.EmptyString))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4230", (object) instance.EffectiveTime.Value, (object) binaryReader.Name)));
        DateTime validFrom = new DateTime(XmlUtil.ReadElementContentAsInt64(binaryReader), DateTimeKind.Utc);
        if (!binaryReader.IsStartElement(instance.ExpiryTime, instance.EmptyString))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4230", (object) instance.ExpiryTime.Value, (object) binaryReader.Name)));
        DateTime validTo = new DateTime(XmlUtil.ReadElementContentAsInt64(binaryReader), DateTimeKind.Utc);
        if (!binaryReader.IsStartElement(instance.KeyEffectiveTime, instance.EmptyString))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4230", (object) instance.KeyEffectiveTime.Value, (object) binaryReader.Name)));
        DateTime keyEffectiveTime = new DateTime(XmlUtil.ReadElementContentAsInt64(binaryReader), DateTimeKind.Utc);
        if (!binaryReader.IsStartElement(instance.KeyExpiryTime, instance.EmptyString))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4230", (object) instance.KeyExpiryTime.Value, (object) binaryReader.Name)));
        DateTime keyExpirationTime = new DateTime(XmlUtil.ReadElementContentAsInt64(binaryReader), DateTimeKind.Utc);
        if (binaryReader.IsStartElement(instance.ClaimsPrincipal, instance.EmptyString))
        {
          List<IAuthorizationPolicy> authorizationPolicyList = new List<IAuthorizationPolicy>();
          IClaimsPrincipal claimsPrincipal = this.ReadPrincipal(binaryReader, instance);
          if (claimsPrincipal != null)
            authorizationPolicyList.Add((IAuthorizationPolicy) new AuthorizationPolicy(claimsPrincipal.Identities));
          if (binaryReader.IsStartElement(instance.SctAuthorizationPolicy, instance.EmptyString))
          {
            binaryReader.ReadStartElement(instance.SctAuthorizationPolicy, instance.EmptyString);
            System.IdentityModel.Claims.Claim claim = this.DeserializeSysClaim(binaryReader);
            binaryReader.ReadEndElement();
            authorizationPolicyList.Add((IAuthorizationPolicy) new SctAuthorizationPolicy(claim));
          }
          string endpointId = (string) null;
          if (binaryReader.IsStartElement(instance.EndpointId, instance.EmptyString))
          {
            endpointId = binaryReader.ReadElementContentAsString();
            authorizationPolicyList.Add((IAuthorizationPolicy) new EndpointAuthorizationPolicy(endpointId));
          }
          binaryReader.ReadEndElement();
          if (flag1)
            return new SessionSecurityToken(new SecurityContextSecurityToken(contextId, id, key, validFrom, validTo, keyGeneration, keyEffectiveTime, keyExpirationTime, authorizationPolicyList.AsReadOnly()), version);
          authorizationPolicyList.Add((IAuthorizationPolicy) new EndpointAuthorizationPolicy(endpointId ?? string.Empty));
          return new SessionSecurityToken(contextId, id, context, key, endpointId, validFrom, validTo, keyGeneration, keyEffectiveTime, keyExpirationTime, authorizationPolicyList.AsReadOnly())
          {
            IsPersistent = flag2,
            IsSessionMode = flag3
          };
        }
      }
      return (SessionSecurityToken) null;
    }

    protected virtual System.IdentityModel.Claims.Claim DeserializeSysClaim(
      XmlDictionaryReader reader)
    {
      Microsoft.IdentityModel.Claims.SessionDictionary instance = Microsoft.IdentityModel.Claims.SessionDictionary.Instance;
      if (reader.IsStartElement(instance.NullValue, instance.EmptyString))
      {
        reader.ReadElementString();
        return (System.IdentityModel.Claims.Claim) null;
      }
      if (reader.IsStartElement(instance.WindowsSidClaim, instance.EmptyString))
      {
        string right = SessionSecurityTokenCookieSerializer.ReadRightAttribute(reader, instance);
        reader.ReadStartElement();
        byte[] binaryForm = reader.ReadContentAsBase64();
        reader.ReadEndElement();
        return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Sid, (object) new SecurityIdentifier(binaryForm, 0), right);
      }
      if (reader.IsStartElement(instance.DenyOnlySidClaim, instance.EmptyString))
      {
        string right = SessionSecurityTokenCookieSerializer.ReadRightAttribute(reader, instance);
        reader.ReadStartElement();
        byte[] binaryForm = reader.ReadContentAsBase64();
        reader.ReadEndElement();
        return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.DenyOnlySid, (object) new SecurityIdentifier(binaryForm, 0), right);
      }
      if (reader.IsStartElement(instance.X500DistinguishedNameClaim, instance.EmptyString))
      {
        string right = SessionSecurityTokenCookieSerializer.ReadRightAttribute(reader, instance);
        reader.ReadStartElement();
        byte[] encodedDistinguishedName = reader.ReadContentAsBase64();
        reader.ReadEndElement();
        return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.X500DistinguishedName, (object) new X500DistinguishedName(encodedDistinguishedName), right);
      }
      if (reader.IsStartElement(instance.X509ThumbprintClaim, instance.EmptyString))
      {
        string right = SessionSecurityTokenCookieSerializer.ReadRightAttribute(reader, instance);
        reader.ReadStartElement();
        byte[] numArray = reader.ReadContentAsBase64();
        reader.ReadEndElement();
        return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Thumbprint, (object) numArray, right);
      }
      if (reader.IsStartElement(instance.NameClaim, instance.EmptyString))
      {
        string right = SessionSecurityTokenCookieSerializer.ReadRightAttribute(reader, instance);
        reader.ReadStartElement();
        string str = reader.ReadString();
        reader.ReadEndElement();
        return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Name, (object) str, right);
      }
      if (reader.IsStartElement(instance.DnsClaim, instance.EmptyString))
      {
        string right = SessionSecurityTokenCookieSerializer.ReadRightAttribute(reader, instance);
        reader.ReadStartElement();
        string str = reader.ReadString();
        reader.ReadEndElement();
        return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Dns, (object) str, right);
      }
      if (reader.IsStartElement(instance.RsaClaim, instance.EmptyString))
      {
        string right = SessionSecurityTokenCookieSerializer.ReadRightAttribute(reader, instance);
        reader.ReadStartElement();
        string xmlString = reader.ReadString();
        reader.ReadEndElement();
        RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
        cryptoServiceProvider.FromXmlString(xmlString);
        return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Rsa, (object) cryptoServiceProvider, right);
      }
      if (reader.IsStartElement(instance.MailAddressClaim, instance.EmptyString))
      {
        string right = SessionSecurityTokenCookieSerializer.ReadRightAttribute(reader, instance);
        reader.ReadStartElement();
        string address = reader.ReadString();
        reader.ReadEndElement();
        return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Email, (object) new MailAddress(address), right);
      }
      if (reader.IsStartElement(instance.SystemClaim, instance.EmptyString))
      {
        reader.ReadElementString();
        return System.IdentityModel.Claims.Claim.System;
      }
      if (reader.IsStartElement(instance.HashClaim, instance.EmptyString))
      {
        string right = SessionSecurityTokenCookieSerializer.ReadRightAttribute(reader, instance);
        reader.ReadStartElement();
        byte[] numArray = reader.ReadContentAsBase64();
        reader.ReadEndElement();
        return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Hash, (object) numArray, right);
      }
      if (reader.IsStartElement(instance.SpnClaim, instance.EmptyString))
      {
        string right = SessionSecurityTokenCookieSerializer.ReadRightAttribute(reader, instance);
        reader.ReadStartElement();
        string str = reader.ReadString();
        reader.ReadEndElement();
        return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Spn, (object) str, right);
      }
      if (reader.IsStartElement(instance.UpnClaim, instance.EmptyString))
      {
        string right = SessionSecurityTokenCookieSerializer.ReadRightAttribute(reader, instance);
        reader.ReadStartElement();
        string str = reader.ReadString();
        reader.ReadEndElement();
        return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Upn, (object) str, right);
      }
      if (reader.IsStartElement(instance.UrlClaim, instance.EmptyString))
      {
        string right = SessionSecurityTokenCookieSerializer.ReadRightAttribute(reader, instance);
        reader.ReadStartElement();
        string uriString = reader.ReadString();
        reader.ReadEndElement();
        return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Uri, (object) new Uri(uriString), right);
      }
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4289", (object) reader.LocalName, (object) reader.NamespaceURI)));
    }

    protected virtual void SerializeSysClaim(System.IdentityModel.Claims.Claim claim, XmlDictionaryWriter writer)
    {
      Microsoft.IdentityModel.Claims.SessionDictionary instance = Microsoft.IdentityModel.Claims.SessionDictionary.Instance;
      if (claim == null)
        writer.WriteElementString(instance.NullValue, instance.EmptyString, string.Empty);
      else if (System.IdentityModel.Claims.ClaimTypes.Sid.Equals(claim.ClaimType))
      {
        writer.WriteStartElement(instance.WindowsSidClaim, instance.EmptyString);
        SessionSecurityTokenCookieSerializer.WriteRightAttribute(claim, instance, writer);
        SessionSecurityTokenCookieSerializer.SerializeSid((SecurityIdentifier) claim.Resource, instance, writer);
        writer.WriteEndElement();
      }
      else if (System.IdentityModel.Claims.ClaimTypes.DenyOnlySid.Equals(claim.ClaimType))
      {
        writer.WriteStartElement(instance.DenyOnlySidClaim, instance.EmptyString);
        SessionSecurityTokenCookieSerializer.WriteRightAttribute(claim, instance, writer);
        SessionSecurityTokenCookieSerializer.SerializeSid((SecurityIdentifier) claim.Resource, instance, writer);
        writer.WriteEndElement();
      }
      else if (System.IdentityModel.Claims.ClaimTypes.X500DistinguishedName.Equals(claim.ClaimType))
      {
        writer.WriteStartElement(instance.X500DistinguishedNameClaim, instance.EmptyString);
        SessionSecurityTokenCookieSerializer.WriteRightAttribute(claim, instance, writer);
        byte[] rawData = ((AsnEncodedData) claim.Resource).RawData;
        writer.WriteBase64(rawData, 0, rawData.Length);
        writer.WriteEndElement();
      }
      else if (System.IdentityModel.Claims.ClaimTypes.Thumbprint.Equals(claim.ClaimType))
      {
        writer.WriteStartElement(instance.X509ThumbprintClaim, instance.EmptyString);
        SessionSecurityTokenCookieSerializer.WriteRightAttribute(claim, instance, writer);
        byte[] resource = (byte[]) claim.Resource;
        writer.WriteBase64(resource, 0, resource.Length);
        writer.WriteEndElement();
      }
      else if (System.IdentityModel.Claims.ClaimTypes.Name.Equals(claim.ClaimType))
      {
        writer.WriteStartElement(instance.NameClaim, instance.EmptyString);
        SessionSecurityTokenCookieSerializer.WriteRightAttribute(claim, instance, writer);
        writer.WriteString((string) claim.Resource);
        writer.WriteEndElement();
      }
      else if (System.IdentityModel.Claims.ClaimTypes.Dns.Equals(claim.ClaimType))
      {
        writer.WriteStartElement(instance.DnsClaim, instance.EmptyString);
        SessionSecurityTokenCookieSerializer.WriteRightAttribute(claim, instance, writer);
        writer.WriteString((string) claim.Resource);
        writer.WriteEndElement();
      }
      else if (System.IdentityModel.Claims.ClaimTypes.Rsa.Equals(claim.ClaimType))
      {
        writer.WriteStartElement(instance.RsaClaim, instance.EmptyString);
        SessionSecurityTokenCookieSerializer.WriteRightAttribute(claim, instance, writer);
        writer.WriteString(((AsymmetricAlgorithm) claim.Resource).ToXmlString(false));
        writer.WriteEndElement();
      }
      else if (System.IdentityModel.Claims.ClaimTypes.Email.Equals(claim.ClaimType))
      {
        writer.WriteStartElement(instance.MailAddressClaim, instance.EmptyString);
        SessionSecurityTokenCookieSerializer.WriteRightAttribute(claim, instance, writer);
        writer.WriteString(((MailAddress) claim.Resource).Address);
        writer.WriteEndElement();
      }
      else if (claim == System.IdentityModel.Claims.Claim.System)
        writer.WriteElementString(instance.SystemClaim, instance.EmptyString, string.Empty);
      else if (System.IdentityModel.Claims.ClaimTypes.Hash.Equals(claim.ClaimType))
      {
        writer.WriteStartElement(instance.HashClaim, instance.EmptyString);
        SessionSecurityTokenCookieSerializer.WriteRightAttribute(claim, instance, writer);
        byte[] resource = (byte[]) claim.Resource;
        writer.WriteBase64(resource, 0, resource.Length);
        writer.WriteEndElement();
      }
      else if (System.IdentityModel.Claims.ClaimTypes.Spn.Equals(claim.ClaimType))
      {
        writer.WriteStartElement(instance.SpnClaim, instance.EmptyString);
        SessionSecurityTokenCookieSerializer.WriteRightAttribute(claim, instance, writer);
        writer.WriteString((string) claim.Resource);
        writer.WriteEndElement();
      }
      else if (System.IdentityModel.Claims.ClaimTypes.Upn.Equals(claim.ClaimType))
      {
        writer.WriteStartElement(instance.UpnClaim, instance.EmptyString);
        SessionSecurityTokenCookieSerializer.WriteRightAttribute(claim, instance, writer);
        writer.WriteString((string) claim.Resource);
        writer.WriteEndElement();
      }
      else if (System.IdentityModel.Claims.ClaimTypes.Uri.Equals(claim.ClaimType))
      {
        writer.WriteStartElement(instance.UrlClaim, instance.EmptyString);
        SessionSecurityTokenCookieSerializer.WriteRightAttribute(claim, instance, writer);
        writer.WriteString(((Uri) claim.Resource).AbsoluteUri);
        writer.WriteEndElement();
      }
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4290", (object) claim)));
    }

    protected virtual SecurityToken ReadBootstrapToken(
      XmlDictionaryReader dictionaryReader,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary,
      IClaimsIdentity identity)
    {
      if (dictionaryReader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionaryReader));
      if (dictionary == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionary));
      if (this._bootstrapTokenHandlers == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4273")));
      SecurityToken securityToken = (SecurityToken) null;
      dictionaryReader.ReadStartElement(dictionary.BootstrapToken, dictionary.EmptyString);
      if (dictionaryReader.IsStartElement("WindowsSecurityTokenStub", string.Empty))
      {
        bool isEmptyElement = dictionaryReader.IsEmptyElement;
        dictionaryReader.ReadStartElement();
        if (!isEmptyElement)
          dictionaryReader.ReadEndElement();
        if (identity is WindowsClaimsIdentity windowsIdentity && SessionSecurityTokenCookieSerializer.IsNonX509Identity(windowsIdentity))
          securityToken = (SecurityToken) new WindowsSecurityToken((WindowsIdentity) windowsIdentity);
      }
      else
      {
        using (StringReader stringReader = new StringReader(dictionaryReader.ReadOuterXml()))
        {
          using (XmlDictionaryReader dictionaryReader1 = (XmlDictionaryReader) new IdentityModelWrappedXmlDictionaryReader((XmlReader) new XmlTextReader((TextReader) stringReader)
          {
            Normalization = false,
            ProhibitDtd = true,
            XmlResolver = (XmlResolver) null
          }, XmlDictionaryReaderQuotas.Max))
          {
            int content = (int) dictionaryReader1.MoveToContent();
            securityToken = this._bootstrapTokenHandlers.ReadToken((XmlReader) dictionaryReader1);
          }
        }
      }
      dictionaryReader.ReadEndElement();
      return securityToken;
    }

    protected virtual IClaimsPrincipal ReadPrincipal(
      XmlDictionaryReader dictionaryReader,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary)
    {
      if (dictionaryReader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionaryReader));
      if (dictionary == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionary));
      IClaimsPrincipal claimsPrincipal = (IClaimsPrincipal) null;
      ClaimsIdentityCollection identities = new ClaimsIdentityCollection();
      int content = (int) dictionaryReader.MoveToContent();
      if (dictionaryReader.IsStartElement(dictionary.ClaimsPrincipal, dictionary.EmptyString))
      {
        dictionaryReader.ReadFullStartElement();
        this.ReadIdentities(dictionaryReader, dictionary, identities);
        dictionaryReader.ReadEndElement();
      }
      identity = (WindowsClaimsIdentity) null;
      foreach (IClaimsIdentity claimsIdentity in identities)
      {
        if (claimsIdentity is WindowsClaimsIdentity identity)
        {
          claimsPrincipal = (IClaimsPrincipal) new WindowsClaimsPrincipal(identity);
          break;
        }
      }
      if (claimsPrincipal != null)
        identities.Remove((IClaimsIdentity) identity);
      else if (identities.Count > 0)
        claimsPrincipal = (IClaimsPrincipal) new ClaimsPrincipal();
      claimsPrincipal?.Identities.AddRange((IEnumerable<IClaimsIdentity>) identities);
      return claimsPrincipal;
    }

    protected virtual void ReadIdentities(
      XmlDictionaryReader dictionaryReader,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary,
      ClaimsIdentityCollection identities)
    {
      if (dictionaryReader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionaryReader));
      if (dictionary == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionary));
      if (identities == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identities));
      int content = (int) dictionaryReader.MoveToContent();
      if (!dictionaryReader.IsStartElement(dictionary.Identities, dictionary.EmptyString))
        return;
      dictionaryReader.ReadFullStartElement();
      while (dictionaryReader.IsStartElement(dictionary.Identity, dictionary.EmptyString))
        identities.Add(this.ReadIdentity(dictionaryReader, dictionary));
      dictionaryReader.ReadEndElement();
    }

    protected virtual IClaimsIdentity ReadIdentity(
      XmlDictionaryReader dictionaryReader,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary)
    {
      if (dictionaryReader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionaryReader));
      if (dictionary == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionary));
      int content = (int) dictionaryReader.MoveToContent();
      if (dictionaryReader.IsStartElement(dictionary.Identity, dictionary.EmptyString))
      {
        string attribute1 = dictionaryReader.GetAttribute(dictionary.WindowsLogonName, dictionary.EmptyString);
        string attribute2 = dictionaryReader.GetAttribute(dictionary.AuthenticationType, dictionary.EmptyString);
        IClaimsIdentity identity = !string.IsNullOrEmpty(attribute1) ? (IClaimsIdentity) WindowsClaimsIdentity.CreateFromUpn(this.GetUpn(attribute1), attribute2, this._useWindowsTokenService, this._windowsIssuerName) : (IClaimsIdentity) new ClaimsIdentity(attribute2);
        identity.Label = dictionaryReader.GetAttribute(dictionary.Label, dictionary.EmptyString);
        identity.NameClaimType = dictionaryReader.GetAttribute(dictionary.NameClaimType, dictionary.EmptyString);
        identity.RoleClaimType = dictionaryReader.GetAttribute(dictionary.RoleClaimType, dictionary.EmptyString);
        dictionaryReader.ReadFullStartElement();
        if (dictionaryReader.IsStartElement(dictionary.ClaimCollection, dictionary.EmptyString))
        {
          dictionaryReader.ReadStartElement();
          this.ReadClaims(dictionaryReader, dictionary, identity.Claims);
          dictionaryReader.ReadEndElement();
        }
        if (dictionaryReader.IsStartElement(dictionary.Actor, dictionary.EmptyString))
        {
          dictionaryReader.ReadStartElement();
          identity.Actor = this.ReadIdentity(dictionaryReader, dictionary);
          dictionaryReader.ReadEndElement();
        }
        if (dictionaryReader.IsStartElement(dictionary.BootstrapToken, dictionary.EmptyString))
        {
          SecurityToken securityToken = this.ReadBootstrapToken(dictionaryReader, dictionary, identity);
          if (this._saveBootstrapTokens)
            identity.BootstrapToken = securityToken;
        }
        dictionaryReader.ReadEndElement();
        return identity;
      }
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID3007", (object) dictionaryReader.LocalName, (object) dictionaryReader.NamespaceURI)));
    }

    protected virtual void ReadClaims(
      XmlDictionaryReader dictionaryReader,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary,
      ClaimCollection claims)
    {
      if (dictionaryReader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionaryReader));
      if (dictionary == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionary));
      if (claims == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claims));
      while (dictionaryReader.IsStartElement(dictionary.Claim, dictionary.EmptyString))
      {
        Microsoft.IdentityModel.Claims.Claim claim = new Microsoft.IdentityModel.Claims.Claim(dictionaryReader.GetAttribute(dictionary.Type, dictionary.EmptyString), dictionaryReader.GetAttribute(dictionary.Value, dictionary.EmptyString), dictionaryReader.GetAttribute(dictionary.ValueType, dictionary.EmptyString), dictionaryReader.GetAttribute(dictionary.Issuer, dictionary.EmptyString), dictionaryReader.GetAttribute(dictionary.OriginalIssuer, dictionary.EmptyString));
        dictionaryReader.ReadFullStartElement();
        if (dictionaryReader.IsStartElement(dictionary.ClaimProperties, dictionary.EmptyString))
          this.ReadClaimProperties(dictionaryReader, dictionary, claim.Properties);
        dictionaryReader.ReadEndElement();
        claims.Add(claim);
      }
    }

    protected virtual void ReadClaimProperties(
      XmlDictionaryReader dictionaryReader,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary,
      IDictionary<string, string> properties)
    {
      if (dictionaryReader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionaryReader));
      if (dictionary == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionary));
      if (properties == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (properties));
      dictionaryReader.ReadStartElement();
      while (dictionaryReader.IsStartElement(dictionary.ClaimProperty, dictionary.EmptyString))
      {
        string attribute1 = dictionaryReader.GetAttribute(dictionary.ClaimPropertyName, dictionary.EmptyString);
        string attribute2 = dictionaryReader.GetAttribute(dictionary.ClaimPropertyValue, dictionary.EmptyString);
        if (string.IsNullOrEmpty(attribute1))
          DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4249")));
        if (string.IsNullOrEmpty(attribute2))
          DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID4250")));
        properties.Add(new KeyValuePair<string, string>(attribute1, attribute2));
        dictionaryReader.ReadFullStartElement();
        dictionaryReader.ReadEndElement();
      }
      dictionaryReader.ReadEndElement();
    }

    protected virtual void WriteBootstrapToken(
      XmlDictionaryWriter dictionaryWriter,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary,
      SecurityToken token)
    {
      if (dictionaryWriter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryReader");
      if (dictionary == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionary));
      if (token == null)
        return;
      dictionaryWriter.WriteStartElement(dictionary.BootstrapToken, dictionary.EmptyString);
      if (token is KerberosReceiverSecurityToken || token is WindowsSecurityToken)
      {
        dictionaryWriter.WriteStartElement("WindowsSecurityTokenStub", string.Empty);
        dictionaryWriter.WriteEndElement();
      }
      else
      {
        Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityToken saml2SecurityToken = this._bootstrapTokenHandlers.CanWriteToken(token) ? token as Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityToken : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4010", (object) token.GetType().ToString()));
        Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = (Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials) null;
        if (saml2SecurityToken != null && saml2SecurityToken.Assertion.EncryptingCredentials != null)
        {
          encryptingCredentials = saml2SecurityToken.Assertion.EncryptingCredentials;
          saml2SecurityToken.Assertion.EncryptingCredentials = (Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials) null;
        }
        this._bootstrapTokenHandlers.WriteToken((XmlWriter) dictionaryWriter, token);
        if (saml2SecurityToken != null && encryptingCredentials != null)
          saml2SecurityToken.Assertion.EncryptingCredentials = encryptingCredentials;
      }
      dictionaryWriter.WriteEndElement();
    }

    protected virtual void WritePrincipal(
      XmlDictionaryWriter dictionaryWriter,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary,
      IClaimsPrincipal principal)
    {
      if (dictionaryWriter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionaryWriter));
      if (dictionary == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionary));
      if (principal == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (principal));
      dictionaryWriter.WriteStartElement(dictionary.ClaimsPrincipal, dictionary.EmptyString);
      if (principal.Identities != null)
        this.WriteIdentities(dictionaryWriter, dictionary, principal.Identities);
      dictionaryWriter.WriteEndElement();
    }

    protected virtual void WriteIdentities(
      XmlDictionaryWriter dictionaryWriter,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary,
      ClaimsIdentityCollection identities)
    {
      if (dictionaryWriter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionaryWriter));
      if (dictionary == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionary));
      if (identities == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identities));
      dictionaryWriter.WriteStartElement(dictionary.Identities, dictionary.EmptyString);
      foreach (IClaimsIdentity identity in identities)
        this.WriteIdentity(dictionaryWriter, dictionary, identity);
      dictionaryWriter.WriteEndElement();
    }

    protected virtual void WriteIdentity(
      XmlDictionaryWriter dictionaryWriter,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary,
      IClaimsIdentity identity)
    {
      if (dictionaryWriter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionaryWriter));
      if (dictionary == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionary));
      if (identity == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identity));
      dictionaryWriter.WriteStartElement(dictionary.Identity, dictionary.EmptyString);
      if (identity is WindowsClaimsIdentity windowsClaimsIdentity)
        dictionaryWriter.WriteAttributeString(dictionary.WindowsLogonName, dictionary.EmptyString, windowsClaimsIdentity.Name);
      if (!string.IsNullOrEmpty(identity.AuthenticationType))
        dictionaryWriter.WriteAttributeString(dictionary.AuthenticationType, dictionary.EmptyString, identity.AuthenticationType);
      if (!string.IsNullOrEmpty(identity.Label))
        dictionaryWriter.WriteAttributeString(dictionary.Label, dictionary.EmptyString, identity.Label);
      if (identity.NameClaimType != null)
        dictionaryWriter.WriteAttributeString(dictionary.NameClaimType, dictionary.EmptyString, identity.NameClaimType);
      if (identity.RoleClaimType != null)
        dictionaryWriter.WriteAttributeString(dictionary.RoleClaimType, dictionary.EmptyString, identity.RoleClaimType);
      if (identity.Claims != null && identity.Claims.Count > 0)
      {
        dictionaryWriter.WriteStartElement(dictionary.ClaimCollection, dictionary.EmptyString);
        this.WriteClaims(dictionaryWriter, dictionary, identity.Claims, windowsClaimsIdentity == null ? (SessionSecurityTokenCookieSerializer.OutboundClaimsFilter) null : (SessionSecurityTokenCookieSerializer.OutboundClaimsFilter) (c => c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid" || c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid" || (c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid" || c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarygroupsid") || c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarysid" || c.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" && c.Issuer == "LOCAL AUTHORITY" && c.ValueType == "http://www.w3.org/2001/XMLSchema#string"));
        dictionaryWriter.WriteEndElement();
      }
      if (identity.Actor != null)
      {
        dictionaryWriter.WriteStartElement(dictionary.Actor, dictionary.EmptyString);
        this.WriteIdentity(dictionaryWriter, dictionary, identity.Actor);
        dictionaryWriter.WriteEndElement();
      }
      if (this._saveBootstrapTokens)
        this.WriteBootstrapToken(dictionaryWriter, dictionary, identity.BootstrapToken);
      dictionaryWriter.WriteEndElement();
    }

    protected virtual void WriteClaims(
      XmlDictionaryWriter dictionaryWriter,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary,
      ClaimCollection claims,
      SessionSecurityTokenCookieSerializer.OutboundClaimsFilter outboundClaimsFilter)
    {
      if (dictionaryWriter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionaryWriter));
      if (dictionary == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionary));
      if (claims == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claims));
      foreach (Microsoft.IdentityModel.Claims.Claim claim in claims)
      {
        if (claim != null && (outboundClaimsFilter == null || !outboundClaimsFilter(claim)))
        {
          dictionaryWriter.WriteStartElement(dictionary.Claim, dictionary.EmptyString);
          if (!string.IsNullOrEmpty(claim.Issuer))
            dictionaryWriter.WriteAttributeString(dictionary.Issuer, dictionary.EmptyString, claim.Issuer);
          if (!string.IsNullOrEmpty(claim.OriginalIssuer))
            dictionaryWriter.WriteAttributeString(dictionary.OriginalIssuer, dictionary.EmptyString, claim.OriginalIssuer);
          dictionaryWriter.WriteAttributeString(dictionary.Type, dictionary.EmptyString, claim.ClaimType);
          dictionaryWriter.WriteAttributeString(dictionary.Value, dictionary.EmptyString, claim.Value);
          dictionaryWriter.WriteAttributeString(dictionary.ValueType, dictionary.EmptyString, claim.ValueType);
          if (claim.Properties != null && claim.Properties.Count > 0)
            this.WriteClaimProperties(dictionaryWriter, dictionary, claim.Properties);
          dictionaryWriter.WriteEndElement();
        }
      }
    }

    protected virtual void WriteClaimProperties(
      XmlDictionaryWriter dictionaryWriter,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary,
      IDictionary<string, string> properties)
    {
      if (dictionaryWriter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionaryWriter));
      if (dictionary == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dictionary));
      if (properties == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (properties));
      if (properties.Count <= 0)
        return;
      dictionaryWriter.WriteStartElement(dictionary.ClaimProperties, dictionary.EmptyString);
      foreach (KeyValuePair<string, string> property in (IEnumerable<KeyValuePair<string, string>>) properties)
      {
        dictionaryWriter.WriteStartElement(dictionary.ClaimProperty, dictionary.EmptyString);
        dictionaryWriter.WriteAttributeString(dictionary.ClaimPropertyName, dictionary.EmptyString, property.Key);
        dictionaryWriter.WriteAttributeString(dictionary.ClaimPropertyValue, dictionary.EmptyString, property.Value);
        dictionaryWriter.WriteEndElement();
      }
      dictionaryWriter.WriteEndElement();
    }

    private static bool IsNonX509Identity(WindowsClaimsIdentity windowsIdentity) => windowsIdentity.Claims.FirstOrDefault<Microsoft.IdentityModel.Claims.Claim>((Func<Microsoft.IdentityModel.Claims.Claim, bool>) (claim => claim.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumbprint")) == null;

    private static void SerializeSid(
      SecurityIdentifier sid,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary,
      XmlDictionaryWriter writer)
    {
      byte[] numArray = new byte[sid.BinaryLength];
      sid.GetBinaryForm(numArray, 0);
      writer.WriteBase64(numArray, 0, numArray.Length);
    }

    private static void WriteRightAttribute(
      System.IdentityModel.Claims.Claim claim,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary,
      XmlDictionaryWriter writer)
    {
      if (Rights.PossessProperty.Equals(claim.Right))
        return;
      writer.WriteAttributeString(dictionary.Right, dictionary.EmptyString, claim.Right);
    }

    private static string ReadRightAttribute(
      XmlDictionaryReader reader,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary)
    {
      string attribute = reader.GetAttribute(dictionary.Right, dictionary.EmptyString);
      return !string.IsNullOrEmpty(attribute) ? attribute : Rights.PossessProperty;
    }

    private static void WriteSidAttribute(
      SecurityIdentifier sid,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary,
      XmlDictionaryWriter writer)
    {
      byte[] numArray = new byte[sid.BinaryLength];
      sid.GetBinaryForm(numArray, 0);
      writer.WriteAttributeString(dictionary.Sid, dictionary.EmptyString, Convert.ToBase64String(numArray));
    }

    private static SecurityIdentifier ReadSidAttribute(
      XmlDictionaryReader reader,
      Microsoft.IdentityModel.Claims.SessionDictionary dictionary)
    {
      return new SecurityIdentifier(Convert.FromBase64String(reader.GetAttribute(dictionary.Sid, dictionary.EmptyString)), 0);
    }

    protected virtual string GetUpn(string windowsLogonName)
    {
      int num1 = !string.IsNullOrEmpty(windowsLogonName) ? windowsLogonName.IndexOf('\\') : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (windowsLogonName));
      if (num1 < 0 || num1 == 0 || num1 == windowsLogonName.Length - 1)
        return SessionSecurityTokenCookieSerializer.IsPossibleUpn(windowsLogonName) ? windowsLogonName : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4248", (object) windowsLogonName)));
      string str1 = windowsLogonName.Substring(0, num1 + 1);
      string str2 = windowsLogonName.Substring(num1 + 1);
      string str3;
      bool flag;
      lock (SessionSecurityTokenCookieSerializer.DomainNameMap)
        flag = SessionSecurityTokenCookieSerializer.DomainNameMap.TryGetValue(str1, out str3);
      if (!flag)
      {
        uint size = 50;
        StringBuilder outputString = new StringBuilder((int) size);
        if (!Microsoft.IdentityModel.NativeMethods.TranslateName(str1, EXTENDED_NAME_FORMAT.NameSamCompatible, EXTENDED_NAME_FORMAT.NameCanonical, outputString, out size))
        {
          int lastWin32Error1 = Marshal.GetLastWin32Error();
          if (lastWin32Error1 == 122)
          {
            outputString = new StringBuilder((int) size);
            if (!Microsoft.IdentityModel.NativeMethods.TranslateName(str1, EXTENDED_NAME_FORMAT.NameSamCompatible, EXTENDED_NAME_FORMAT.NameCanonical, outputString, out size))
            {
              int lastWin32Error2 = Marshal.GetLastWin32Error();
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4248", (object) windowsLogonName), (Exception) new Win32Exception(lastWin32Error2)));
            }
          }
          else
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4248", (object) windowsLogonName), (Exception) new Win32Exception(lastWin32Error1)));
        }
        str3 = outputString.Remove(outputString.Length - 1, 1).ToString();
        lock (SessionSecurityTokenCookieSerializer.DomainNameMap)
        {
          if (SessionSecurityTokenCookieSerializer.DomainNameMap.Count >= 50)
          {
            if (SessionSecurityTokenCookieSerializer.rnd == null)
              SessionSecurityTokenCookieSerializer.rnd = new Random((int) DateTime.Now.Ticks);
            int num2 = SessionSecurityTokenCookieSerializer.rnd.Next() % SessionSecurityTokenCookieSerializer.DomainNameMap.Count;
            foreach (string key in SessionSecurityTokenCookieSerializer.DomainNameMap.Keys)
            {
              if (num2 <= 0)
              {
                SessionSecurityTokenCookieSerializer.DomainNameMap.Remove(key);
                break;
              }
              --num2;
            }
          }
          SessionSecurityTokenCookieSerializer.DomainNameMap[str1] = str3;
        }
      }
      return str2 + "@" + str3;
    }

    private static bool IsPossibleUpn(string name)
    {
      int num = name.IndexOf('@');
      return name.Length >= 3 && num >= 0 && (num != 0 && num != name.Length - 1);
    }

    protected delegate bool OutboundClaimsFilter(Microsoft.IdentityModel.Claims.Claim claim);
  }
}
