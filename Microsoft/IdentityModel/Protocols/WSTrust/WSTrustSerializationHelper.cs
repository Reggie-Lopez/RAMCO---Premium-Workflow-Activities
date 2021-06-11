// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustSerializationHelper
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.SecurityTokenService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  internal class WSTrustSerializationHelper
  {
    public static RequestSecurityToken CreateRequest(
      XmlReader reader,
      WSTrustSerializationContext context,
      WSTrustRequestSerializer requestSerializer,
      WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (requestSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestSerializer));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (!reader.IsStartElement(trustConstants.Elements.RequestSecurityToken, trustConstants.NamespaceURI))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3032", (object) reader.LocalName, (object) reader.NamespaceURI, (object) trustConstants.Elements.RequestSecurityToken, (object) trustConstants.NamespaceURI)));
      bool isEmptyElement = reader.IsEmptyElement;
      RequestSecurityToken requestSecurityToken = requestSerializer.CreateRequestSecurityToken();
      requestSecurityToken.Context = reader.GetAttribute(trustConstants.Attributes.Context);
      reader.Read();
      if (!isEmptyElement)
      {
        while (reader.IsStartElement())
          requestSerializer.ReadXmlElement(reader, requestSecurityToken, context);
        reader.ReadEndElement();
      }
      requestSerializer.Validate(requestSecurityToken);
      return requestSecurityToken;
    }

    public static void ReadRSTXml(
      XmlReader reader,
      RequestSecurityToken rst,
      WSTrustSerializationContext context,
      WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (rst == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rst));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (reader.IsStartElement(trustConstants.Elements.TokenType, trustConstants.NamespaceURI))
      {
        rst.TokenType = reader.ReadElementContentAsString();
        if (!UriUtil.CanCreateValidUri(rst.TokenType, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.TokenType, (object) trustConstants.NamespaceURI, (object) rst.TokenType)));
      }
      else if (reader.IsStartElement(trustConstants.Elements.RequestType, trustConstants.NamespaceURI))
        rst.RequestType = WSTrustSerializationHelper.ReadRequestType(reader, trustConstants);
      else if (reader.IsStartElement("AppliesTo", "http://schemas.xmlsoap.org/ws/2004/09/policy"))
        rst.AppliesTo = WSTrustSerializationHelper.ReadAppliesTo(reader, trustConstants);
      else if (reader.IsStartElement(trustConstants.Elements.Issuer, trustConstants.NamespaceURI))
        rst.Issuer = WSTrustSerializationHelper.ReadOnBehalfOfIssuer(reader, trustConstants);
      else if (reader.IsStartElement(trustConstants.Elements.ProofEncryption, trustConstants.NamespaceURI))
      {
        if (!reader.IsEmptyElement)
          rst.ProofEncryption = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(WSTrustSerializationHelper.ReadInnerXml(reader), context.SecurityTokenHandlers);
        if (rst.ProofEncryption == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3218")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.Encryption, trustConstants.NamespaceURI))
      {
        if (!reader.IsEmptyElement)
          rst.Encryption = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(WSTrustSerializationHelper.ReadInnerXml(reader), context.SecurityTokenHandlers);
        if (rst.Encryption == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3268")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.DelegateTo, trustConstants.NamespaceURI))
      {
        if (!reader.IsEmptyElement)
          rst.DelegateTo = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(WSTrustSerializationHelper.ReadInnerXml(reader), context.SecurityTokenHandlers);
        if (rst.DelegateTo == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3219")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.Claims, trustConstants.NamespaceURI))
      {
        rst.Claims.Dialect = reader.GetAttribute(trustConstants.Attributes.Dialect);
        if (rst.Claims.Dialect != null && !UriUtil.CanCreateValidUri(rst.Claims.Dialect, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3136", (object) trustConstants.Attributes.Dialect, (object) reader.LocalName, (object) reader.NamespaceURI, (object) rst.Claims.Dialect)));
        string requestClaimNamespace = WSTrustSerializationHelper.GetRequestClaimNamespace(rst.Claims.Dialect);
        bool isEmptyElement1 = reader.IsEmptyElement;
        reader.ReadStartElement(trustConstants.Elements.Claims, trustConstants.NamespaceURI);
        if (isEmptyElement1)
          return;
        while (reader.IsStartElement("ClaimType", requestClaimNamespace))
        {
          bool isEmptyElement2 = reader.IsEmptyElement;
          string attribute1 = reader.GetAttribute("Uri");
          if (string.IsNullOrEmpty(attribute1))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3009")));
          bool isOptional = false;
          string attribute2 = reader.GetAttribute("Optional");
          if (!string.IsNullOrEmpty(attribute2))
            isOptional = XmlConvert.ToBoolean(attribute2);
          reader.Read();
          int content = (int) reader.MoveToContent();
          string str = (string) null;
          if (!isEmptyElement2)
          {
            if (reader.IsStartElement("Value", requestClaimNamespace))
            {
              if (!StringComparer.Ordinal.Equals(rst.Claims.Dialect, "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims"))
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3258", (object) rst.Claims.Dialect, (object) "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims")));
              str = reader.ReadElementContentAsString("Value", requestClaimNamespace);
            }
            reader.ReadEndElement();
          }
          rst.Claims.Add(new RequestClaim(attribute1, isOptional, str));
        }
        reader.ReadEndElement();
      }
      else if (reader.IsStartElement(trustConstants.Elements.Entropy, trustConstants.NamespaceURI))
      {
        bool isEmptyElement = reader.IsEmptyElement;
        reader.ReadStartElement(trustConstants.Elements.Entropy, trustConstants.NamespaceURI);
        if (!isEmptyElement)
        {
          rst.Entropy = new Entropy(WSTrustSerializationHelper.ReadProtectedKey(reader, context, trustConstants) ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3026"))));
          reader.ReadEndElement();
        }
        if (rst.Entropy == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3026")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.BinaryExchange, trustConstants.NamespaceURI))
        rst.BinaryExchange = WSTrustSerializationHelper.ReadBinaryExchange(reader, trustConstants);
      else if (reader.IsStartElement(trustConstants.Elements.Lifetime, trustConstants.NamespaceURI))
        rst.Lifetime = WSTrustSerializationHelper.ReadLifetime(reader, trustConstants);
      else if (reader.IsStartElement(trustConstants.Elements.RenewTarget, trustConstants.NamespaceURI))
      {
        if (!reader.IsEmptyElement)
          rst.RenewTarget = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(WSTrustSerializationHelper.ReadInnerXml(reader), context.SecurityTokenHandlers);
        if (rst.RenewTarget == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3151")));
      }
      else if (reader.IsStartElement("RequestDisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
      {
        rst.RequestDisplayToken = true;
        while (reader.MoveToNextAttribute())
        {
          if (StringComparer.Ordinal.Equals("xml:lang", reader.Name))
          {
            rst.DisplayTokenLanguage = reader.Value;
            break;
          }
        }
        reader.Skip();
      }
      else if (reader.IsStartElement(trustConstants.Elements.OnBehalfOf, trustConstants.NamespaceURI))
      {
        if (!reader.IsEmptyElement)
        {
          if (!context.SecurityTokenHandlerCollectionManager.ContainsKey("OnBehalfOf"))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3264")));
          rst.OnBehalfOf = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(WSTrustSerializationHelper.ReadInnerXml(reader), context.SecurityTokenHandlerCollectionManager["OnBehalfOf"]);
        }
        if (rst.OnBehalfOf == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3152")));
      }
      else if (reader.IsStartElement("ActAs", "http://docs.oasis-open.org/ws-sx/ws-trust/200802"))
      {
        if (!reader.IsEmptyElement)
        {
          if (!context.SecurityTokenHandlerCollectionManager.ContainsKey("ActAs"))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3265")));
          rst.ActAs = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(WSTrustSerializationHelper.ReadInnerXml(reader), context.SecurityTokenHandlerCollectionManager["ActAs"]);
        }
        if (rst.ActAs == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3153")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.KeyType, trustConstants.NamespaceURI))
        rst.KeyType = WSTrustSerializationHelper.ReadKeyType(reader, trustConstants);
      else if (reader.IsStartElement(trustConstants.Elements.KeySize, trustConstants.NamespaceURI))
      {
        if (!reader.IsEmptyElement)
          rst.KeySizeInBits = new int?(int.Parse(reader.ReadElementContentAsString(), (IFormatProvider) CultureInfo.InvariantCulture));
        if (!rst.KeySizeInBits.HasValue)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3154")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.UseKey, trustConstants.NamespaceURI))
      {
        bool isEmptyElement = reader.IsEmptyElement;
        reader.ReadStartElement();
        if (!isEmptyElement)
        {
          if (!context.SecurityTokenHandlers.CanReadToken(reader))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3165")));
          SecurityToken securityToken = context.SecurityTokenHandlers.ReadToken(reader);
          SecurityKeyIdentifier securityKeyIdentifier = new SecurityKeyIdentifier();
          if (securityToken.CanCreateKeyIdentifierClause<RsaKeyIdentifierClause>())
          {
            securityKeyIdentifier.Add((SecurityKeyIdentifierClause) securityToken.CreateKeyIdentifierClause<RsaKeyIdentifierClause>());
          }
          else
          {
            if (!securityToken.CanCreateKeyIdentifierClause<X509RawDataKeyIdentifierClause>())
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3166")));
            securityKeyIdentifier.Add((SecurityKeyIdentifierClause) securityToken.CreateKeyIdentifierClause<X509RawDataKeyIdentifierClause>());
          }
          SecurityToken token;
          if (!context.UseKeyTokenResolver.TryResolveToken(securityKeyIdentifier, out token))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3092", (object) securityKeyIdentifier)));
          rst.UseKey = new UseKey(securityKeyIdentifier, token);
          reader.ReadEndElement();
        }
        if (rst.UseKey == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3155")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.SignWith, trustConstants.NamespaceURI))
      {
        rst.SignWith = reader.ReadElementContentAsString();
        if (!UriUtil.CanCreateValidUri(rst.SignWith, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.SignWith, (object) trustConstants.NamespaceURI, (object) rst.SignWith)));
      }
      else if (reader.IsStartElement(trustConstants.Elements.EncryptWith, trustConstants.NamespaceURI))
      {
        rst.EncryptWith = reader.ReadElementContentAsString();
        if (!UriUtil.CanCreateValidUri(rst.EncryptWith, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.EncryptWith, (object) trustConstants.NamespaceURI, (object) rst.EncryptWith)));
      }
      else if (reader.IsStartElement(trustConstants.Elements.ComputedKeyAlgorithm, trustConstants.NamespaceURI))
        rst.ComputedKeyAlgorithm = WSTrustSerializationHelper.ReadComputedKeyAlgorithm(reader, trustConstants);
      else if (reader.IsStartElement(trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI))
      {
        rst.AuthenticationType = reader.ReadElementContentAsString(trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI);
        if (!UriUtil.CanCreateValidUri(rst.AuthenticationType, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.AuthenticationType, (object) trustConstants.NamespaceURI, (object) rst.AuthenticationType)));
      }
      else if (reader.IsStartElement(trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI))
      {
        rst.EncryptionAlgorithm = reader.ReadElementContentAsString(trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI);
        if (!UriUtil.CanCreateValidUri(rst.EncryptionAlgorithm, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.EncryptionAlgorithm, (object) trustConstants.NamespaceURI, (object) rst.EncryptionAlgorithm)));
      }
      else if (reader.IsStartElement(trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI))
      {
        rst.CanonicalizationAlgorithm = reader.ReadElementContentAsString(trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI);
        if (!UriUtil.CanCreateValidUri(rst.CanonicalizationAlgorithm, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.CanonicalizationAlgorithm, (object) trustConstants.NamespaceURI, (object) rst.CanonicalizationAlgorithm)));
      }
      else if (reader.IsStartElement(trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI))
      {
        rst.SignatureAlgorithm = reader.ReadElementContentAsString(trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI);
        if (!UriUtil.CanCreateValidUri(rst.SignatureAlgorithm, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.SignatureAlgorithm, (object) trustConstants.NamespaceURI, (object) rst.SignatureAlgorithm)));
      }
      else if (reader.IsStartElement("InformationCardReference", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
      {
        if (!reader.IsEmptyElement)
        {
          reader.ReadStartElement();
          string cardId = reader.ReadElementContentAsString("CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity");
          long cardVersion = reader.ReadElementContentAsLong("CardVersion", "http://schemas.xmlsoap.org/ws/2005/05/identity");
          rst.InformationCardReference = cardVersion >= 1L && cardVersion <= (long) uint.MaxValue ? new InformationCardReference(cardId, cardVersion) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3263", (object) (long) uint.MaxValue)));
          reader.ReadEndElement();
        }
        if (rst.InformationCardReference == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3156")));
      }
      else if (reader.IsStartElement("ClientPseudonym", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
      {
        if (!reader.IsEmptyElement)
        {
          reader.ReadStartElement();
          rst.ClientPseudonym = reader.ReadElementString("PPID", "http://schemas.xmlsoap.org/ws/2005/05/identity");
          reader.ReadEndElement();
        }
        if (string.IsNullOrEmpty(rst.ClientPseudonym))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3157")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.Forwardable, trustConstants.NamespaceURI))
        rst.Forwardable = new bool?(reader.ReadElementContentAsBoolean());
      else if (reader.IsStartElement(trustConstants.Elements.Delegatable, trustConstants.NamespaceURI))
        rst.Delegatable = new bool?(reader.ReadElementContentAsBoolean());
      else if (reader.IsStartElement(trustConstants.Elements.AllowPostdating, trustConstants.NamespaceURI))
      {
        rst.AllowPostdating = true;
        bool isEmptyElement = reader.IsEmptyElement;
        reader.Read();
        int content = (int) reader.MoveToContent();
        if (isEmptyElement)
          return;
        reader.ReadEndElement();
      }
      else if (reader.IsStartElement(trustConstants.Elements.Renewing, trustConstants.NamespaceURI))
      {
        bool isEmptyElement = reader.IsEmptyElement;
        string attribute1 = reader.GetAttribute(trustConstants.Attributes.Allow);
        bool allowRenewal = true;
        bool okForRenewalAfterExpiration = false;
        if (!string.IsNullOrEmpty(attribute1))
          allowRenewal = XmlConvert.ToBoolean(attribute1);
        string attribute2 = reader.GetAttribute(trustConstants.Attributes.OK);
        if (!string.IsNullOrEmpty(attribute2))
          okForRenewalAfterExpiration = XmlConvert.ToBoolean(attribute2);
        rst.Renewing = new Renewing(allowRenewal, okForRenewalAfterExpiration);
        reader.Read();
        int content = (int) reader.MoveToContent();
        if (isEmptyElement)
          return;
        reader.ReadEndElement();
      }
      else if (reader.IsStartElement(trustConstants.Elements.CancelTarget, trustConstants.NamespaceURI))
      {
        if (!reader.IsEmptyElement)
          rst.CancelTarget = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(WSTrustSerializationHelper.ReadInnerXml(reader), context.SecurityTokenHandlers);
        if (rst.CancelTarget == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3220")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.Participants, trustConstants.NamespaceURI))
      {
        EndpointAddress endpointAddress = (EndpointAddress) null;
        List<EndpointAddress> endpointAddressList = new List<EndpointAddress>();
        bool isEmptyElement = reader.IsEmptyElement;
        reader.Read();
        int content = (int) reader.MoveToContent();
        if (isEmptyElement)
          return;
        if (reader.IsStartElement(trustConstants.Elements.Primary, trustConstants.NamespaceURI))
        {
          reader.ReadStartElement(trustConstants.Elements.Primary, trustConstants.NamespaceURI);
          endpointAddress = EndpointAddress.ReadFrom(XmlDictionaryReader.CreateDictionaryReader(reader));
          reader.ReadEndElement();
        }
        while (reader.IsStartElement(trustConstants.Elements.Participant, trustConstants.NamespaceURI))
        {
          reader.ReadStartElement(trustConstants.Elements.Participant, trustConstants.NamespaceURI);
          endpointAddressList.Add(EndpointAddress.ReadFrom(XmlDictionaryReader.CreateDictionaryReader(reader)));
          reader.ReadEndElement();
        }
        if (reader.IsStartElement())
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3223", (object) trustConstants.Elements.Participants, (object) trustConstants.NamespaceURI, (object) reader.LocalName, (object) reader.NamespaceURI)));
        rst.Participants = new Participants();
        rst.Participants.Primary = endpointAddress;
        rst.Participants.Participant.AddRange((IEnumerable<EndpointAddress>) endpointAddressList);
        reader.ReadEndElement();
      }
      else if (reader.IsStartElement("AdditionalContext", "http://docs.oasis-open.org/wsfed/authorization/200706"))
      {
        rst.AdditionalContext = new AdditionalContext();
        bool isEmptyElement = reader.IsEmptyElement;
        reader.Read();
        int content = (int) reader.MoveToContent();
        if (isEmptyElement)
          return;
        while (reader.IsStartElement("ContextItem", "http://docs.oasis-open.org/wsfed/authorization/200706"))
        {
          Uri result1 = (Uri) null;
          Uri result2 = (Uri) null;
          string str = (string) null;
          string attribute1 = reader.GetAttribute("Name");
          if (string.IsNullOrEmpty(attribute1) || !UriUtil.TryCreateValidUri(attribute1, UriKind.Absolute, out result1))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3136", (object) "Name", (object) reader.LocalName, (object) reader.NamespaceURI, (object) attribute1)));
          string attribute2 = reader.GetAttribute("Scope");
          if (!string.IsNullOrEmpty(attribute2) && !UriUtil.TryCreateValidUri(attribute2, UriKind.Absolute, out result2))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3136", (object) "Scope", (object) reader.LocalName, (object) reader.NamespaceURI, (object) attribute2)));
          if (reader.IsEmptyElement)
          {
            reader.Read();
          }
          else
          {
            reader.Read();
            if (reader.IsStartElement("Value", "http://docs.oasis-open.org/wsfed/authorization/200706"))
              str = reader.ReadElementContentAsString("Value", "http://docs.oasis-open.org/wsfed/authorization/200706");
            reader.ReadEndElement();
          }
          rst.AdditionalContext.Items.Add(new ContextItem(result1, str, result2));
        }
        if (reader.IsStartElement())
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3223", (object) "AdditionalContext", (object) "http://docs.oasis-open.org/wsfed/authorization/200706", (object) reader.LocalName, (object) reader.NamespaceURI)));
        reader.ReadEndElement();
      }
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3007", (object) reader.LocalName, (object) reader.NamespaceURI)));
    }

    public static void WriteRequest(
      RequestSecurityToken rst,
      XmlWriter writer,
      WSTrustSerializationContext context,
      WSTrustRequestSerializer requestSerializer,
      WSTrustConstantsAdapter trustConstants)
    {
      if (rst == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rst));
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (requestSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestSerializer));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      requestSerializer.Validate(rst);
      writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestSecurityToken, trustConstants.NamespaceURI);
      if (rst.Context != null)
        writer.WriteAttributeString(trustConstants.Attributes.Context, rst.Context);
      requestSerializer.WriteKnownRequestElement(rst, writer, context);
      foreach (KeyValuePair<string, object> property in rst.Properties)
        requestSerializer.WriteXmlElement(writer, property.Key, property.Value, rst, context);
      writer.WriteEndElement();
    }

    public static void WriteKnownRequestElement(
      RequestSecurityToken rst,
      XmlWriter writer,
      WSTrustSerializationContext context,
      WSTrustRequestSerializer requestSerializer,
      WSTrustConstantsAdapter trustConstants)
    {
      if (rst == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rst));
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (requestSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestSerializer));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (rst.AppliesTo != (EndpointAddress) null)
        requestSerializer.WriteXmlElement(writer, "AppliesTo", (object) rst.AppliesTo, rst, context);
      if (rst.Claims.Count > 0)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Claims, (object) rst.Claims, rst, context);
      if (!string.IsNullOrEmpty(rst.ComputedKeyAlgorithm))
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.ComputedKeyAlgorithm, (object) rst.ComputedKeyAlgorithm, rst, context);
      if (!string.IsNullOrEmpty(rst.SignWith))
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.SignWith, (object) rst.SignWith, rst, context);
      if (!string.IsNullOrEmpty(rst.EncryptWith))
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.EncryptWith, (object) rst.EncryptWith, rst, context);
      if (rst.Entropy != null)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Entropy, (object) rst.Entropy, rst, context);
      if (rst.KeySizeInBits.HasValue)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.KeySize, (object) rst.KeySizeInBits, rst, context);
      if (!string.IsNullOrEmpty(rst.KeyType))
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.KeyType, (object) rst.KeyType, rst, context);
      if (rst.Lifetime != null)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Lifetime, (object) rst.Lifetime, rst, context);
      if (rst.RenewTarget != null)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.RenewTarget, (object) rst.RenewTarget, rst, context);
      if (rst.RequestDisplayToken)
        requestSerializer.WriteXmlElement(writer, "RequestDisplayToken", (object) rst.DisplayTokenLanguage, rst, context);
      if (rst.OnBehalfOf != null)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.OnBehalfOf, (object) rst.OnBehalfOf, rst, context);
      if (rst.ActAs != null)
        requestSerializer.WriteXmlElement(writer, "ActAs", (object) rst.ActAs, rst, context);
      if (!string.IsNullOrEmpty(rst.RequestType))
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestType, (object) rst.RequestType, rst, context);
      if (!string.IsNullOrEmpty(rst.TokenType))
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.TokenType, (object) rst.TokenType, rst, context);
      if (rst.UseKey != null)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.UseKey, (object) rst.UseKey, rst, context);
      if (!string.IsNullOrEmpty(rst.AuthenticationType))
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.AuthenticationType, (object) rst.AuthenticationType, rst, context);
      if (!string.IsNullOrEmpty(rst.EncryptionAlgorithm))
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.EncryptionAlgorithm, (object) rst.EncryptionAlgorithm, rst, context);
      if (!string.IsNullOrEmpty(rst.CanonicalizationAlgorithm))
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.CanonicalizationAlgorithm, (object) rst.CanonicalizationAlgorithm, rst, context);
      if (!string.IsNullOrEmpty(rst.SignatureAlgorithm))
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.SignatureAlgorithm, (object) rst.SignatureAlgorithm, rst, context);
      if (rst.InformationCardReference != null)
        requestSerializer.WriteXmlElement(writer, "InformationCardReference", (object) rst.InformationCardReference, rst, context);
      if (!string.IsNullOrEmpty(rst.ClientPseudonym))
        requestSerializer.WriteXmlElement(writer, "ClientPseudonym", (object) rst.ClientPseudonym, rst, context);
      if (rst.BinaryExchange != null)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.BinaryExchange, (object) rst.BinaryExchange, rst, context);
      if (rst.Issuer != (EndpointAddress) null)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Issuer, (object) rst.Issuer, rst, context);
      if (rst.ProofEncryption != null)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.ProofEncryption, (object) rst.ProofEncryption, rst, context);
      if (rst.Encryption != null)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Encryption, (object) rst.Encryption, rst, context);
      if (rst.DelegateTo != null)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.DelegateTo, (object) rst.DelegateTo, rst, context);
      if (rst.Forwardable.HasValue)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Forwardable, (object) rst.Forwardable.Value, rst, context);
      if (rst.Delegatable.HasValue)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Delegatable, (object) rst.Delegatable.Value, rst, context);
      if (rst.AllowPostdating)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.AllowPostdating, (object) rst.AllowPostdating, rst, context);
      if (rst.Renewing != null)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Renewing, (object) rst.Renewing, rst, context);
      if (rst.CancelTarget != null)
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.CancelTarget, (object) rst.CancelTarget, rst, context);
      if (rst.Participants != null && (rst.Participants.Primary != (EndpointAddress) null || rst.Participants.Participant.Count > 0))
        requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Participants, (object) rst.Participants, rst, context);
      if (rst.AdditionalContext == null)
        return;
      requestSerializer.WriteXmlElement(writer, "AdditionalContext", (object) rst.AdditionalContext, rst, context);
    }

    public static void WriteRSTXml(
      XmlWriter writer,
      string elementName,
      object elementValue,
      WSTrustSerializationContext context,
      WSTrustConstantsAdapter trustConstants)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (string.IsNullOrEmpty(elementName))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (elementName));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (StringComparer.Ordinal.Equals(elementName, "AppliesTo"))
      {
        EndpointAddress appliesTo = elementValue as EndpointAddress;
        WSTrustSerializationHelper.WriteAppliesTo(writer, appliesTo, trustConstants);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Claims))
      {
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Claims, trustConstants.NamespaceURI);
        RequestClaimCollection requestClaimCollection = (RequestClaimCollection) elementValue;
        string ns = requestClaimCollection.Dialect == null || UriUtil.CanCreateValidUri(requestClaimCollection.Dialect, UriKind.Absolute) ? WSTrustSerializationHelper.GetRequestClaimNamespace(requestClaimCollection.Dialect) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3136", (object) trustConstants.Attributes.Dialect, (object) trustConstants.Elements.Claims, (object) trustConstants.NamespaceURI, (object) requestClaimCollection.Dialect)));
        string str = writer.LookupPrefix(ns);
        if (string.IsNullOrEmpty(str))
        {
          str = WSTrustSerializationHelper.GetRequestClaimPrefix(requestClaimCollection.Dialect);
          writer.WriteAttributeString("xmlns", str, (string) null, ns);
        }
        writer.WriteAttributeString(trustConstants.Attributes.Dialect, !string.IsNullOrEmpty(requestClaimCollection.Dialect) ? requestClaimCollection.Dialect : "http://schemas.xmlsoap.org/ws/2005/05/identity");
        foreach (RequestClaim requestClaim in (Collection<RequestClaim>) requestClaimCollection)
        {
          writer.WriteStartElement(str, "ClaimType", ns);
          writer.WriteAttributeString("Uri", requestClaim.ClaimType);
          writer.WriteAttributeString("Optional", requestClaim.IsOptional ? "true" : "false");
          if (requestClaim.Value != null)
          {
            if (StringComparer.Ordinal.Equals(requestClaimCollection.Dialect, "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims"))
              writer.WriteElementString(str, "Value", ns, requestClaim.Value);
            else
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3257", (object) requestClaimCollection.Dialect, (object) "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims")));
          }
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.ComputedKeyAlgorithm))
        WSTrustSerializationHelper.WriteComputedKeyAlgorithm(writer, trustConstants.Elements.ComputedKeyAlgorithm, (string) elementValue, trustConstants);
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.BinaryExchange))
        WSTrustSerializationHelper.WriteBinaryExchange(writer, elementValue as BinaryExchange, trustConstants);
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Issuer))
        WSTrustSerializationHelper.WriteOnBehalfOfIssuer(writer, elementValue as EndpointAddress, trustConstants);
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.SignWith))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.SignWith, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.SignWith, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.EncryptWith))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.EncryptWith, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.EncryptWith, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Entropy))
      {
        if (!(elementValue is Entropy entropy))
          return;
        writer.WriteStartElement(trustConstants.Elements.Entropy, trustConstants.NamespaceURI);
        WSTrustSerializationHelper.WriteProtectedKey(writer, (ProtectedKey) entropy, context, trustConstants);
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.KeySize))
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.KeySize, trustConstants.NamespaceURI, Convert.ToString((int) elementValue, (IFormatProvider) CultureInfo.InvariantCulture));
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.KeyType))
        WSTrustSerializationHelper.WriteKeyType(writer, (string) elementValue, trustConstants);
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Lifetime))
      {
        Lifetime lifetime = (Lifetime) elementValue;
        WSTrustSerializationHelper.WriteLifetime(writer, lifetime, trustConstants);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RenewTarget))
      {
        if (!(elementValue is Microsoft.IdentityModel.Tokens.SecurityTokenElement securityTokenElement))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (elementValue), Microsoft.IdentityModel.SR.GetString("ID3222", (object) trustConstants.Elements.RenewTarget, (object) trustConstants.NamespaceURI, (object) typeof (Microsoft.IdentityModel.Tokens.SecurityTokenElement), elementValue));
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RenewTarget, trustConstants.NamespaceURI);
        if (securityTokenElement.SecurityTokenXml != null)
          securityTokenElement.SecurityTokenXml.WriteTo(writer);
        else
          context.SecurityTokenSerializer.WriteToken(writer, securityTokenElement.GetSecurityToken());
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, "RequestDisplayToken"))
      {
        writer.WriteStartElement("i", "RequestDisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity");
        if (!string.IsNullOrEmpty((string) elementValue))
          Microsoft.IdentityModel.XmlUtil.WriteLanguageAttribute(writer, (string) elementValue);
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.OnBehalfOf))
      {
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.OnBehalfOf, trustConstants.NamespaceURI);
        WSTrustSerializationHelper.WriteTokenElement((Microsoft.IdentityModel.Tokens.SecurityTokenElement) elementValue, "OnBehalfOf", context, writer);
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, "ActAs"))
      {
        writer.WriteStartElement("tr", "ActAs", "http://docs.oasis-open.org/ws-sx/ws-trust/200802");
        WSTrustSerializationHelper.WriteTokenElement((Microsoft.IdentityModel.Tokens.SecurityTokenElement) elementValue, "ActAs", context, writer);
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestType))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.RequestType, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        WSTrustSerializationHelper.WriteRequestType(writer, (string) elementValue, trustConstants);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.TokenType))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.TokenType, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.TokenType, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.UseKey))
      {
        UseKey useKey = (UseKey) elementValue;
        if (useKey.Token == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3012")));
        if (!context.SecurityTokenSerializer.CanWriteToken(useKey.Token))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3017")));
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.UseKey, trustConstants.NamespaceURI);
        context.SecurityTokenSerializer.WriteToken(writer, useKey.Token);
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.AuthenticationType))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.AuthenticationType, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.EncryptionAlgorithm))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.EncryptionAlgorithm, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.CanonicalizationAlgorithm))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.CanonicalizationAlgorithm, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.SignatureAlgorithm))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.SignatureAlgorithm, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, "InformationCardReference"))
      {
        InformationCardReference informationCardReference = (InformationCardReference) elementValue;
        writer.WriteStartElement("i", "InformationCardReference", "http://schemas.xmlsoap.org/ws/2005/05/identity");
        writer.WriteElementString("i", "CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity", informationCardReference.CardId);
        writer.WriteElementString("i", "CardVersion", "http://schemas.xmlsoap.org/ws/2005/05/identity", Convert.ToString(informationCardReference.CardVersion, (IFormatProvider) CultureInfo.InvariantCulture));
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, "ClientPseudonym"))
      {
        string str = (string) elementValue;
        writer.WriteStartElement("i", "ClientPseudonym", "http://schemas.xmlsoap.org/ws/2005/05/identity");
        writer.WriteElementString("i", "PPID", "http://schemas.xmlsoap.org/ws/2005/05/identity", str);
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Encryption))
      {
        Microsoft.IdentityModel.Tokens.SecurityTokenElement securityTokenElement = (Microsoft.IdentityModel.Tokens.SecurityTokenElement) elementValue;
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Encryption, trustConstants.NamespaceURI);
        if (securityTokenElement.SecurityTokenXml != null)
          securityTokenElement.SecurityTokenXml.WriteTo(writer);
        else
          context.SecurityTokenSerializer.WriteToken(writer, securityTokenElement.GetSecurityToken());
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.ProofEncryption))
      {
        Microsoft.IdentityModel.Tokens.SecurityTokenElement securityTokenElement = (Microsoft.IdentityModel.Tokens.SecurityTokenElement) elementValue;
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.ProofEncryption, trustConstants.NamespaceURI);
        if (securityTokenElement.SecurityTokenXml != null)
          securityTokenElement.SecurityTokenXml.WriteTo(writer);
        else
          context.SecurityTokenSerializer.WriteToken(writer, securityTokenElement.GetSecurityToken());
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.DelegateTo))
      {
        Microsoft.IdentityModel.Tokens.SecurityTokenElement securityTokenElement = (Microsoft.IdentityModel.Tokens.SecurityTokenElement) elementValue;
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.DelegateTo, trustConstants.NamespaceURI);
        if (securityTokenElement.SecurityTokenXml != null)
          securityTokenElement.SecurityTokenXml.WriteTo(writer);
        else
          context.SecurityTokenSerializer.WriteToken(writer, securityTokenElement.GetSecurityToken());
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Forwardable))
      {
        if (!(elementValue is bool flag))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (elementValue), Microsoft.IdentityModel.SR.GetString("ID3222", (object) trustConstants.Elements.Forwardable, (object) trustConstants.NamespaceURI, (object) typeof (bool), elementValue));
        writer.WriteStartElement(trustConstants.Elements.Forwardable, trustConstants.NamespaceURI);
        writer.WriteString(XmlConvert.ToString(flag));
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Delegatable))
      {
        if (!(elementValue is bool flag))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (elementValue), Microsoft.IdentityModel.SR.GetString("ID3222", (object) trustConstants.Elements.Delegatable, (object) trustConstants.NamespaceURI, (object) typeof (bool), elementValue));
        writer.WriteStartElement(trustConstants.Elements.Delegatable, trustConstants.NamespaceURI);
        writer.WriteString(XmlConvert.ToString(flag));
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.AllowPostdating))
      {
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.AllowPostdating, trustConstants.NamespaceURI);
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Renewing))
      {
        if (!(elementValue is Renewing renewing))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (elementValue), Microsoft.IdentityModel.SR.GetString("ID3222", (object) trustConstants.Elements.Renewing, (object) trustConstants.NamespaceURI, (object) typeof (Renewing), elementValue));
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Renewing, trustConstants.NamespaceURI);
        writer.WriteAttributeString(trustConstants.Attributes.Allow, XmlConvert.ToString(renewing.AllowRenewal));
        writer.WriteAttributeString(trustConstants.Attributes.OK, XmlConvert.ToString(renewing.OkForRenewalAfterExpiration));
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.CancelTarget))
      {
        if (!(elementValue is Microsoft.IdentityModel.Tokens.SecurityTokenElement securityTokenElement))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (elementValue), Microsoft.IdentityModel.SR.GetString("ID3222", (object) trustConstants.Elements.CancelTarget, (object) trustConstants.NamespaceURI, (object) typeof (Microsoft.IdentityModel.Tokens.SecurityTokenElement), elementValue));
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.CancelTarget, trustConstants.NamespaceURI);
        if (securityTokenElement.SecurityTokenXml != null)
          securityTokenElement.SecurityTokenXml.WriteTo(writer);
        else
          context.SecurityTokenSerializer.WriteToken(writer, securityTokenElement.GetSecurityToken());
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Participants))
      {
        if (!(elementValue is Participants participants))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (elementValue), Microsoft.IdentityModel.SR.GetString("ID3222", (object) trustConstants.Elements.Participant, (object) trustConstants.NamespaceURI, (object) typeof (Participants), elementValue));
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Participants, trustConstants.NamespaceURI);
        if (participants.Primary != (EndpointAddress) null)
        {
          writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Primary, trustConstants.NamespaceURI);
          participants.Primary.WriteTo(AddressingVersion.WSAddressing10, writer);
          writer.WriteEndElement();
        }
        foreach (EndpointAddress endpointAddress in participants.Participant)
        {
          writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Participant, trustConstants.NamespaceURI);
          endpointAddress.WriteTo(AddressingVersion.WSAddressing10, writer);
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, "AdditionalContext"))
      {
        if (!(elementValue is AdditionalContext additionalContext))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (elementValue), Microsoft.IdentityModel.SR.GetString("ID3222", (object) "AdditionalContext", (object) "http://docs.oasis-open.org/wsfed/authorization/200706", (object) typeof (AdditionalContext), elementValue));
        writer.WriteStartElement("auth", "AdditionalContext", "http://docs.oasis-open.org/wsfed/authorization/200706");
        foreach (ContextItem contextItem in (IEnumerable<ContextItem>) additionalContext.Items)
        {
          writer.WriteStartElement("auth", "ContextItem", "http://docs.oasis-open.org/wsfed/authorization/200706");
          writer.WriteAttributeString("Name", contextItem.Name.AbsoluteUri);
          if (contextItem.Scope != (Uri) null)
            writer.WriteAttributeString("Scope", contextItem.Scope.AbsoluteUri);
          if (contextItem.Value != null)
            writer.WriteElementString("Value", "http://docs.oasis-open.org/wsfed/authorization/200706", contextItem.Value);
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3013", (object) elementName, (object) elementValue.GetType())));
    }

    private static void WriteTokenElement(
      Microsoft.IdentityModel.Tokens.SecurityTokenElement tokenElement,
      string usage,
      WSTrustSerializationContext context,
      XmlWriter writer)
    {
      if (tokenElement.SecurityTokenXml != null)
      {
        tokenElement.SecurityTokenXml.WriteTo(writer);
      }
      else
      {
        Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection handlerCollection = !context.SecurityTokenHandlerCollectionManager.ContainsKey(usage) ? context.SecurityTokenHandlers : context.SecurityTokenHandlerCollectionManager[usage];
        SecurityToken securityToken = tokenElement.GetSecurityToken();
        bool flag = false;
        if (handlerCollection != null && handlerCollection.CanWriteToken(securityToken))
        {
          handlerCollection.WriteToken(writer, securityToken);
          flag = true;
        }
        if (flag)
          return;
        context.SecurityTokenSerializer.WriteToken(writer, securityToken);
      }
    }

    public static RequestSecurityTokenResponse CreateResponse(
      XmlReader reader,
      WSTrustSerializationContext context,
      WSTrustResponseSerializer responseSerializer,
      WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (responseSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (responseSerializer));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (!reader.IsStartElement(trustConstants.Elements.RequestSecurityTokenResponse, trustConstants.NamespaceURI))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3032", (object) reader.LocalName, (object) reader.NamespaceURI, (object) trustConstants.Elements.RequestSecurityTokenResponse, (object) trustConstants.NamespaceURI)));
      RequestSecurityTokenResponse instance = responseSerializer.CreateInstance();
      bool isEmptyElement = reader.IsEmptyElement;
      instance.Context = reader.GetAttribute(trustConstants.Attributes.Context);
      reader.Read();
      if (!isEmptyElement)
      {
        while (reader.IsStartElement())
          responseSerializer.ReadXmlElement(reader, instance, context);
        reader.ReadEndElement();
      }
      responseSerializer.Validate(instance);
      return instance;
    }

    public static void ReadRSTRXml(
      XmlReader reader,
      RequestSecurityTokenResponse rstr,
      WSTrustSerializationContext context,
      WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (rstr == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rstr));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (reader.IsStartElement(trustConstants.Elements.Entropy, trustConstants.NamespaceURI))
      {
        if (!reader.IsEmptyElement)
        {
          reader.ReadStartElement(trustConstants.Elements.Entropy, trustConstants.NamespaceURI);
          rstr.Entropy = new Entropy(WSTrustSerializationHelper.ReadProtectedKey(reader, context, trustConstants) ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3026"))));
          reader.ReadEndElement();
        }
        if (rstr.Entropy == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3026")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.KeySize, trustConstants.NamespaceURI))
      {
        if (!reader.IsEmptyElement)
          rstr.KeySizeInBits = new int?(Convert.ToInt32(reader.ReadElementContentAsString(), (IFormatProvider) CultureInfo.InvariantCulture));
        if (!rstr.KeySizeInBits.HasValue)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3154")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.RequestType, trustConstants.NamespaceURI))
        rstr.RequestType = WSTrustSerializationHelper.ReadRequestType(reader, trustConstants);
      else if (reader.IsStartElement(trustConstants.Elements.Lifetime, trustConstants.NamespaceURI))
        rstr.Lifetime = WSTrustSerializationHelper.ReadLifetime(reader, trustConstants);
      else if (reader.IsStartElement(trustConstants.Elements.RequestedSecurityToken, trustConstants.NamespaceURI))
      {
        if (!reader.IsEmptyElement)
          rstr.RequestedSecurityToken = new RequestedSecurityToken(WSTrustSerializationHelper.ReadInnerXml(reader));
        if (rstr.RequestedSecurityToken == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3158")));
      }
      else if (reader.IsStartElement("RequestedDisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
        rstr.RequestedDisplayToken = WSTrustSerializationHelper.ReadRequestedDisplayToken(reader, trustConstants);
      else if (reader.IsStartElement("AppliesTo", "http://schemas.xmlsoap.org/ws/2004/09/policy"))
        rstr.AppliesTo = WSTrustSerializationHelper.ReadAppliesTo(reader, trustConstants);
      else if (reader.IsStartElement(trustConstants.Elements.RequestedProofToken, trustConstants.NamespaceURI))
      {
        if (!reader.IsEmptyElement)
        {
          reader.ReadStartElement();
          if (reader.LocalName == trustConstants.Elements.ComputedKey && reader.NamespaceURI == trustConstants.NamespaceURI)
            rstr.RequestedProofToken = new RequestedProofToken(WSTrustSerializationHelper.ReadComputedKeyAlgorithm(reader, trustConstants));
          else
            rstr.RequestedProofToken = new RequestedProofToken(WSTrustSerializationHelper.ReadProtectedKey(reader, context, trustConstants) ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3025"))));
          reader.ReadEndElement();
        }
        if (rstr.RequestedProofToken == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3025")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.RequestedAttachedReference, trustConstants.NamespaceURI))
      {
        if (!reader.IsEmptyElement)
        {
          reader.ReadStartElement();
          rstr.RequestedAttachedReference = context.SecurityTokenSerializer.ReadKeyIdentifierClause(reader);
          reader.ReadEndElement();
        }
        if (rstr.RequestedAttachedReference == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3159")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.RequestedUnattachedReference, trustConstants.NamespaceURI))
      {
        if (!reader.IsEmptyElement)
        {
          reader.ReadStartElement();
          rstr.RequestedUnattachedReference = context.SecurityTokenSerializer.ReadKeyIdentifierClause(reader);
          reader.ReadEndElement();
        }
        if (rstr.RequestedUnattachedReference == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3160")));
      }
      else if (reader.IsStartElement(trustConstants.Elements.TokenType, trustConstants.NamespaceURI))
      {
        rstr.TokenType = reader.ReadElementContentAsString();
        if (!UriUtil.CanCreateValidUri(rstr.TokenType, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.TokenType, (object) trustConstants.NamespaceURI, (object) rstr.TokenType)));
      }
      else if (reader.IsStartElement(trustConstants.Elements.KeyType, trustConstants.NamespaceURI))
        rstr.KeyType = WSTrustSerializationHelper.ReadKeyType(reader, trustConstants);
      else if (reader.IsStartElement(trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI))
      {
        rstr.AuthenticationType = reader.ReadElementContentAsString(trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI);
        if (!UriUtil.CanCreateValidUri(rstr.AuthenticationType, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.AuthenticationType, (object) trustConstants.NamespaceURI, (object) rstr.AuthenticationType)));
      }
      else if (reader.IsStartElement(trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI))
      {
        rstr.EncryptionAlgorithm = reader.ReadElementContentAsString(trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI);
        if (!UriUtil.CanCreateValidUri(rstr.EncryptionAlgorithm, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.EncryptionAlgorithm, (object) trustConstants.NamespaceURI, (object) rstr.EncryptionAlgorithm)));
      }
      else if (reader.IsStartElement(trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI))
      {
        rstr.CanonicalizationAlgorithm = reader.ReadElementContentAsString(trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI);
        if (!UriUtil.CanCreateValidUri(rstr.CanonicalizationAlgorithm, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.CanonicalizationAlgorithm, (object) trustConstants.NamespaceURI, (object) rstr.CanonicalizationAlgorithm)));
      }
      else if (reader.IsStartElement(trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI))
      {
        rstr.SignatureAlgorithm = reader.ReadElementContentAsString(trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI);
        if (!UriUtil.CanCreateValidUri(rstr.SignatureAlgorithm, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.SignatureAlgorithm, (object) trustConstants.NamespaceURI, (object) rstr.SignatureAlgorithm)));
      }
      else if (reader.IsStartElement(trustConstants.Elements.SignWith, trustConstants.NamespaceURI))
      {
        rstr.SignWith = reader.ReadElementContentAsString();
        if (!UriUtil.CanCreateValidUri(rstr.SignWith, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.SignWith, (object) trustConstants.NamespaceURI, (object) rstr.SignWith)));
      }
      else if (reader.IsStartElement(trustConstants.Elements.EncryptWith, trustConstants.NamespaceURI))
      {
        rstr.EncryptWith = reader.ReadElementContentAsString();
        if (!UriUtil.CanCreateValidUri(rstr.EncryptWith, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.EncryptWith, (object) trustConstants.NamespaceURI, (object) rstr.EncryptWith)));
      }
      else if (reader.IsStartElement(trustConstants.Elements.BinaryExchange, trustConstants.NamespaceURI))
        rstr.BinaryExchange = WSTrustSerializationHelper.ReadBinaryExchange(reader, trustConstants);
      else if (reader.IsStartElement(trustConstants.Elements.Status, trustConstants.NamespaceURI))
        rstr.Status = WSTrustSerializationHelper.ReadStatus(reader, trustConstants);
      else if (reader.IsStartElement(trustConstants.Elements.RequestedTokenCancelled, trustConstants.NamespaceURI))
      {
        rstr.RequestedTokenCancelled = true;
        reader.ReadStartElement();
      }
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3007", (object) reader.LocalName, (object) reader.NamespaceURI)));
    }

    public static void WriteResponse(
      RequestSecurityTokenResponse response,
      XmlWriter writer,
      WSTrustSerializationContext context,
      WSTrustResponseSerializer responseSerializer,
      WSTrustConstantsAdapter trustConstants)
    {
      if (response == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (response));
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (responseSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (responseSerializer));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      responseSerializer.Validate(response);
      writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestSecurityTokenResponse, trustConstants.NamespaceURI);
      if (!string.IsNullOrEmpty(response.Context))
        writer.WriteAttributeString(trustConstants.Attributes.Context, response.Context);
      responseSerializer.WriteKnownResponseElement(response, writer, context);
      foreach (KeyValuePair<string, object> property in response.Properties)
        responseSerializer.WriteXmlElement(writer, property.Key, property.Value, response, context);
      writer.WriteEndElement();
    }

    public static void WriteKnownResponseElement(
      RequestSecurityTokenResponse rstr,
      XmlWriter writer,
      WSTrustSerializationContext context,
      WSTrustResponseSerializer responseSerializer,
      WSTrustConstantsAdapter trustConstants)
    {
      if (rstr == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rstr));
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (responseSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (responseSerializer));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (rstr.Entropy != null)
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.Entropy, (object) rstr.Entropy, rstr, context);
      if (rstr.KeySizeInBits.HasValue)
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.KeySize, (object) rstr.KeySizeInBits, rstr, context);
      if (rstr.Lifetime != null)
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.Lifetime, (object) rstr.Lifetime, rstr, context);
      if (rstr.AppliesTo != (EndpointAddress) null)
        responseSerializer.WriteXmlElement(writer, "AppliesTo", (object) rstr.AppliesTo, rstr, context);
      if (rstr.RequestedSecurityToken != null)
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestedSecurityToken, (object) rstr.RequestedSecurityToken, rstr, context);
      if (rstr.RequestedDisplayToken != null)
        responseSerializer.WriteXmlElement(writer, "RequestedDisplayToken", (object) rstr.RequestedDisplayToken, rstr, context);
      if (rstr.RequestedProofToken != null)
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestedProofToken, (object) rstr.RequestedProofToken, rstr, context);
      if (rstr.RequestedAttachedReference != null)
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestedAttachedReference, (object) rstr.RequestedAttachedReference, rstr, context);
      if (rstr.RequestedUnattachedReference != null)
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestedUnattachedReference, (object) rstr.RequestedUnattachedReference, rstr, context);
      if (!string.IsNullOrEmpty(rstr.SignWith))
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.SignWith, (object) rstr.SignWith, rstr, context);
      if (!string.IsNullOrEmpty(rstr.EncryptWith))
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.EncryptWith, (object) rstr.EncryptWith, rstr, context);
      if (!string.IsNullOrEmpty(rstr.TokenType))
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.TokenType, (object) rstr.TokenType, rstr, context);
      if (!string.IsNullOrEmpty(rstr.RequestType))
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestType, (object) rstr.RequestType, rstr, context);
      if (!string.IsNullOrEmpty(rstr.KeyType))
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.KeyType, (object) rstr.KeyType, rstr, context);
      if (!string.IsNullOrEmpty(rstr.AuthenticationType))
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.AuthenticationType, (object) rstr.AuthenticationType, rstr, context);
      if (!string.IsNullOrEmpty(rstr.EncryptionAlgorithm))
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.EncryptionAlgorithm, (object) rstr.EncryptionAlgorithm, rstr, context);
      if (!string.IsNullOrEmpty(rstr.CanonicalizationAlgorithm))
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.CanonicalizationAlgorithm, (object) rstr.CanonicalizationAlgorithm, rstr, context);
      if (!string.IsNullOrEmpty(rstr.SignatureAlgorithm))
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.SignatureAlgorithm, (object) rstr.SignatureAlgorithm, rstr, context);
      if (rstr.BinaryExchange != null)
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.BinaryExchange, (object) rstr.BinaryExchange, rstr, context);
      if (rstr.Status != null)
        responseSerializer.WriteXmlElement(writer, trustConstants.Elements.Status, (object) rstr.Status, rstr, context);
      if (!rstr.RequestedTokenCancelled)
        return;
      responseSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestedTokenCancelled, (object) rstr.RequestedTokenCancelled, rstr, context);
    }

    public static void WriteRSTRXml(
      XmlWriter writer,
      string elementName,
      object elementValue,
      WSTrustSerializationContext context,
      WSTrustConstantsAdapter trustConstants)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (string.IsNullOrEmpty(elementName))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (elementName));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Entropy))
      {
        if (!(elementValue is Entropy entropy))
          return;
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Entropy, trustConstants.NamespaceURI);
        WSTrustSerializationHelper.WriteProtectedKey(writer, (ProtectedKey) entropy, context, trustConstants);
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.KeySize))
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.KeySize, trustConstants.NamespaceURI, Convert.ToString((int) elementValue, (IFormatProvider) CultureInfo.InvariantCulture));
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Lifetime))
      {
        Lifetime lifetime = (Lifetime) elementValue;
        WSTrustSerializationHelper.WriteLifetime(writer, lifetime, trustConstants);
      }
      else if (StringComparer.Ordinal.Equals(elementName, "AppliesTo"))
      {
        EndpointAddress appliesTo = elementValue as EndpointAddress;
        WSTrustSerializationHelper.WriteAppliesTo(writer, appliesTo, trustConstants);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestedSecurityToken))
      {
        RequestedSecurityToken requestedSecurityToken = (RequestedSecurityToken) elementValue;
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestedSecurityToken, trustConstants.NamespaceURI);
        if (requestedSecurityToken.SecurityTokenXml != null)
          requestedSecurityToken.SecurityTokenXml.WriteTo(writer);
        else
          context.SecurityTokenSerializer.WriteToken(writer, requestedSecurityToken.SecurityToken);
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, "RequestedDisplayToken"))
        WSTrustSerializationHelper.WriteRequestedDisplayToken(writer, (DisplayToken) elementValue, trustConstants);
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestedProofToken))
      {
        RequestedProofToken requestedProofToken = (RequestedProofToken) elementValue;
        if (string.IsNullOrEmpty(requestedProofToken.ComputedKeyAlgorithm) && requestedProofToken.ProtectedKey == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3021")));
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestedProofToken, trustConstants.NamespaceURI);
        if (!string.IsNullOrEmpty(requestedProofToken.ComputedKeyAlgorithm))
          WSTrustSerializationHelper.WriteComputedKeyAlgorithm(writer, trustConstants.Elements.ComputedKey, requestedProofToken.ComputedKeyAlgorithm, trustConstants);
        else
          WSTrustSerializationHelper.WriteProtectedKey(writer, requestedProofToken.ProtectedKey, context, trustConstants);
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestedAttachedReference))
      {
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestedAttachedReference, trustConstants.NamespaceURI);
        context.SecurityTokenSerializer.WriteKeyIdentifierClause(writer, (SecurityKeyIdentifierClause) elementValue);
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestedUnattachedReference))
      {
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestedUnattachedReference, trustConstants.NamespaceURI);
        context.SecurityTokenSerializer.WriteKeyIdentifierClause(writer, (SecurityKeyIdentifierClause) elementValue);
        writer.WriteEndElement();
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.TokenType))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.TokenType, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.TokenType, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestType))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.RequestType, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        WSTrustSerializationHelper.WriteRequestType(writer, (string) elementValue, trustConstants);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.KeyType))
        WSTrustSerializationHelper.WriteKeyType(writer, (string) elementValue, trustConstants);
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.AuthenticationType))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.AuthenticationType, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.EncryptionAlgorithm))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.EncryptionAlgorithm, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.CanonicalizationAlgorithm))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.CanonicalizationAlgorithm, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.SignatureAlgorithm))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.SignatureAlgorithm, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.SignWith))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.SignWith, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.SignWith, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.EncryptWith))
      {
        if (!UriUtil.CanCreateValidUri((string) elementValue, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.EncryptWith, (object) trustConstants.NamespaceURI, (object) (string) elementValue)));
        writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.EncryptWith, trustConstants.NamespaceURI, (string) elementValue);
      }
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.BinaryExchange))
        WSTrustSerializationHelper.WriteBinaryExchange(writer, elementValue as BinaryExchange, trustConstants);
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Status))
        WSTrustSerializationHelper.WriteStatus(writer, elementValue as Status, trustConstants);
      else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestedTokenCancelled))
      {
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestedTokenCancelled, trustConstants.NamespaceURI);
        writer.WriteEndElement();
      }
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3013", (object) elementName, (object) elementValue.GetType())));
    }

    public static string ReadComputedKeyAlgorithm(
      XmlReader reader,
      WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      string str = reader.ReadElementContentAsString();
      if (string.IsNullOrEmpty(str))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3006")));
      if (!UriUtil.CanCreateValidUri(str, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.ComputedKeyAlgorithm, (object) trustConstants.NamespaceURI, (object) str)));
      if (StringComparer.Ordinal.Equals(str, trustConstants.ComputedKeyAlgorithm.Psha1))
        str = "http://schemas.microsoft.com/idfx/computedkeyalgorithm/psha1";
      return str;
    }

    public static void WriteComputedKeyAlgorithm(
      XmlWriter writer,
      string elementName,
      string computedKeyAlgorithm,
      WSTrustConstantsAdapter trustConstants)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (string.IsNullOrEmpty(computedKeyAlgorithm))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (computedKeyAlgorithm));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (!UriUtil.CanCreateValidUri(computedKeyAlgorithm, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) elementName, (object) trustConstants.NamespaceURI, (object) computedKeyAlgorithm)));
      string uriString = !StringComparer.Ordinal.Equals(computedKeyAlgorithm, "http://schemas.microsoft.com/idfx/computedkeyalgorithm/psha1") ? computedKeyAlgorithm : trustConstants.ComputedKeyAlgorithm.Psha1;
      if (!UriUtil.CanCreateValidUri(uriString, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) elementName, (object) trustConstants.NamespaceURI, (object) uriString)));
      writer.WriteElementString(trustConstants.Prefix, elementName, trustConstants.NamespaceURI, uriString);
    }

    public static Status ReadStatus(XmlReader reader, WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (!reader.IsStartElement(trustConstants.Elements.Status, trustConstants.NamespaceURI))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3032", (object) reader.LocalName, (object) reader.NamespaceURI, (object) trustConstants.Elements.Status, (object) trustConstants.NamespaceURI)));
      string reason = (string) null;
      reader.ReadStartElement();
      if (!reader.IsStartElement(trustConstants.Elements.Code, trustConstants.NamespaceURI))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3032", (object) reader.LocalName, (object) reader.NamespaceURI, (object) trustConstants.Elements.Code, (object) trustConstants.NamespaceURI)));
      string code = reader.ReadElementContentAsString(trustConstants.Elements.Code, trustConstants.NamespaceURI);
      if (reader.IsStartElement(trustConstants.Elements.Reason, trustConstants.NamespaceURI))
        reason = reader.ReadElementContentAsString(trustConstants.Elements.Reason, trustConstants.NamespaceURI);
      reader.ReadEndElement();
      return new Status(code, reason);
    }

    public static BinaryExchange ReadBinaryExchange(
      XmlReader reader,
      WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (!reader.IsStartElement(trustConstants.Elements.BinaryExchange, trustConstants.NamespaceURI))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3032", (object) reader.LocalName, (object) reader.NamespaceURI, (object) trustConstants.Elements.BinaryExchange, (object) trustConstants.NamespaceURI)));
      string attribute1 = reader.GetAttribute(trustConstants.Attributes.ValueType);
      if (string.IsNullOrEmpty(attribute1))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID0001", (object) trustConstants.Attributes.ValueType, (object) reader.Name)));
      Uri result1;
      if (!UriUtil.TryCreateValidUri(attribute1, UriKind.Absolute, out result1))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3136", (object) trustConstants.Attributes.ValueType, (object) reader.LocalName, (object) reader.NamespaceURI, (object) attribute1)));
      string attribute2 = reader.GetAttribute(trustConstants.Attributes.EncodingType);
      if (string.IsNullOrEmpty(attribute2))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID0001", (object) trustConstants.Attributes.EncodingType, (object) reader.Name)));
      Uri result2;
      if (!UriUtil.TryCreateValidUri(attribute2, UriKind.Absolute, out result2))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3136", (object) trustConstants.Attributes.EncodingType, (object) reader.LocalName, (object) reader.NamespaceURI, (object) attribute2)));
      byte[] binaryData;
      switch (result2.AbsoluteUri)
      {
        case "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary":
          binaryData = Convert.FromBase64String(reader.ReadElementContentAsString());
          break;
        case "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary":
          binaryData = SoapHexBinary.Parse(reader.ReadElementContentAsString()).Value;
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3215", (object) result2, (object) reader.LocalName, (object) reader.NamespaceURI, (object) string.Format("({0}, {1})", (object) "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary", (object) "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary"))));
      }
      return new BinaryExchange(binaryData, result1, result2);
    }

    public static void WriteBinaryExchange(
      XmlWriter writer,
      BinaryExchange binaryExchange,
      WSTrustConstantsAdapter trustConstants)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (binaryExchange == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (binaryExchange));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      string base64String;
      switch (binaryExchange.EncodingType.AbsoluteUri)
      {
        case "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary":
          base64String = Convert.ToBase64String(binaryExchange.BinaryData);
          break;
        case "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary":
          base64String = new SoapHexBinary(binaryExchange.BinaryData).ToString();
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3217", (object) binaryExchange.EncodingType.AbsoluteUri, (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, "({0}, {1})", (object) "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary", (object) "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary"))));
      }
      writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.BinaryExchange, trustConstants.NamespaceURI);
      writer.WriteAttributeString(trustConstants.Attributes.ValueType, binaryExchange.ValueType.AbsoluteUri);
      writer.WriteAttributeString(trustConstants.Attributes.EncodingType, binaryExchange.EncodingType.AbsoluteUri);
      writer.WriteString(base64String);
      writer.WriteEndElement();
    }

    public static void WriteStatus(
      XmlWriter writer,
      Status status,
      WSTrustConstantsAdapter trustConstants)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (status == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (status));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (status.Code == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("status code");
      writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Status, trustConstants.NamespaceURI);
      writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Code, trustConstants.NamespaceURI);
      writer.WriteString(status.Code);
      writer.WriteEndElement();
      if (status.Reason != null)
      {
        writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Reason, trustConstants.NamespaceURI);
        writer.WriteString(status.Reason);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }

    public static ProtectedKey ReadProtectedKey(
      XmlReader reader,
      WSTrustSerializationContext context,
      WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      ProtectedKey protectedKey = (ProtectedKey) null;
      if (!reader.IsEmptyElement)
      {
        if (reader.IsStartElement(trustConstants.Elements.BinarySecret, trustConstants.NamespaceURI))
          protectedKey = new ProtectedKey(WSTrustSerializationHelper.ReadBinarySecretSecurityToken(reader, trustConstants).GetKeyBytes());
        else if (context.SecurityTokenSerializer.CanReadKeyIdentifierClause(reader) && context.SecurityTokenSerializer.ReadKeyIdentifierClause(reader) is EncryptedKeyIdentifierClause identifierClause)
        {
          SecurityKey key = (SecurityKey) null;
          foreach (SecurityKeyIdentifierClause keyIdentifierClause in identifierClause.EncryptingKeyIdentifier)
          {
            if (context.TokenResolver.TryResolveSecurityKey(keyIdentifierClause, out key))
              break;
          }
          if (key == null)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3027", (object) "the SecurityHeaderTokenResolver or OutOfBandTokenResolver")));
          protectedKey = new ProtectedKey(key.DecryptKey(identifierClause.EncryptionMethod, identifierClause.GetEncryptedKey()), new Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials(key, identifierClause.EncryptingKeyIdentifier, identifierClause.EncryptionMethod));
        }
      }
      return protectedKey;
    }

    public static void WriteProtectedKey(
      XmlWriter writer,
      ProtectedKey protectedKey,
      WSTrustSerializationContext context,
      WSTrustConstantsAdapter trustConstants)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (protectedKey == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (protectedKey));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (protectedKey.WrappingCredentials != null)
      {
        EncryptedKeyIdentifierClause identifierClause = new EncryptedKeyIdentifierClause(protectedKey.WrappingCredentials.SecurityKey.EncryptKey(protectedKey.WrappingCredentials.Algorithm, protectedKey.GetKeyBytes()), protectedKey.WrappingCredentials.Algorithm, protectedKey.WrappingCredentials.SecurityKeyIdentifier);
        context.SecurityTokenSerializer.WriteKeyIdentifierClause(writer, (SecurityKeyIdentifierClause) identifierClause);
      }
      else
      {
        BinarySecretSecurityToken token = new BinarySecretSecurityToken(protectedKey.GetKeyBytes());
        WSTrustSerializationHelper.WriteBinarySecretSecurityToken(writer, token, trustConstants);
      }
    }

    public static string ReadRequestType(XmlReader reader, WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      string uriString = reader.ReadElementContentAsString();
      if (!UriUtil.CanCreateValidUri(uriString, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.RequestType, (object) trustConstants.NamespaceURI, (object) uriString)));
      if (trustConstants.RequestTypes.Issue.Equals(uriString))
        return "http://schemas.microsoft.com/idfx/requesttype/issue";
      if (trustConstants.RequestTypes.Cancel.Equals(uriString))
        return "http://schemas.microsoft.com/idfx/requesttype/cancel";
      if (trustConstants.RequestTypes.Renew.Equals(uriString))
        return "http://schemas.microsoft.com/idfx/requesttype/renew";
      if (trustConstants.RequestTypes.Validate.Equals(uriString))
        return "http://schemas.microsoft.com/idfx/requesttype/validate";
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3011", (object) uriString)));
    }

    public static void WriteRequestType(
      XmlWriter writer,
      string requestType,
      WSTrustConstantsAdapter trustConstants)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (requestType == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestType));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      string str;
      if (StringComparer.Ordinal.Equals(requestType, "http://schemas.microsoft.com/idfx/requesttype/issue") || StringComparer.Ordinal.Equals(requestType, trustConstants.RequestTypes.Issue))
        str = trustConstants.RequestTypes.Issue;
      else if (StringComparer.Ordinal.Equals(requestType, "http://schemas.microsoft.com/idfx/requesttype/renew") || StringComparer.Ordinal.Equals(requestType, trustConstants.RequestTypes.Renew))
        str = trustConstants.RequestTypes.Renew;
      else if (StringComparer.Ordinal.Equals(requestType, "http://schemas.microsoft.com/idfx/requesttype/cancel") || StringComparer.Ordinal.Equals(requestType, trustConstants.RequestTypes.Cancel))
        str = trustConstants.RequestTypes.Cancel;
      else if (StringComparer.Ordinal.Equals(requestType, "http://schemas.microsoft.com/idfx/requesttype/validate") || StringComparer.Ordinal.Equals(requestType, trustConstants.RequestTypes.Validate))
        str = trustConstants.RequestTypes.Validate;
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3011", (object) requestType)));
      writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.RequestType, trustConstants.NamespaceURI, str);
    }

    public static Lifetime ReadLifetime(
      XmlReader reader,
      WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      DateTime? created = new DateTime?();
      DateTime? expires = new DateTime?();
      Lifetime lifetime = (Lifetime) null;
      bool isEmptyElement = reader.IsEmptyElement;
      reader.ReadStartElement();
      if (!isEmptyElement)
      {
        if (reader.IsStartElement("Created", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"))
        {
          reader.ReadStartElement("Created", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
          created = new DateTime?(DateTime.ParseExact(reader.ReadString(), DateTimeFormats.Accepted, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime());
          reader.ReadEndElement();
        }
        if (reader.IsStartElement("Expires", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"))
        {
          reader.ReadStartElement("Expires", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
          expires = new DateTime?(DateTime.ParseExact(reader.ReadString(), DateTimeFormats.Accepted, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime());
          reader.ReadEndElement();
        }
        reader.ReadEndElement();
        lifetime = new Lifetime(created, expires);
      }
      return lifetime != null ? lifetime : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3161")));
    }

    public static void WriteLifetime(
      XmlWriter writer,
      Lifetime lifetime,
      WSTrustConstantsAdapter trustConstants)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (lifetime == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (lifetime));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Lifetime, trustConstants.NamespaceURI);
      if (lifetime.Created.HasValue)
        writer.WriteElementString("wsu", "Created", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", lifetime.Created.Value.ToString(DateTimeFormats.Generated, (IFormatProvider) CultureInfo.InvariantCulture));
      if (lifetime.Expires.HasValue)
        writer.WriteElementString("wsu", "Expires", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", lifetime.Expires.Value.ToString(DateTimeFormats.Generated, (IFormatProvider) CultureInfo.InvariantCulture));
      writer.WriteEndElement();
    }

    public static EndpointAddress ReadOnBehalfOfIssuer(
      XmlReader reader,
      WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (!reader.IsStartElement(trustConstants.Elements.Issuer, trustConstants.NamespaceURI))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3032", (object) reader.LocalName, (object) reader.NamespaceURI, (object) trustConstants.Elements.Issuer, (object) trustConstants.NamespaceURI)));
      EndpointAddress endpointAddress = (EndpointAddress) null;
      if (!reader.IsEmptyElement)
      {
        reader.ReadStartElement();
        endpointAddress = EndpointAddress.ReadFrom(XmlDictionaryReader.CreateDictionaryReader(reader));
        reader.ReadEndElement();
      }
      return !(endpointAddress == (EndpointAddress) null) ? endpointAddress : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3216")));
    }

    public static void WriteOnBehalfOfIssuer(
      XmlWriter writer,
      EndpointAddress issuer,
      WSTrustConstantsAdapter trustConstants)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (issuer == (EndpointAddress) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (issuer));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Issuer, trustConstants.NamespaceURI);
      issuer.WriteTo(AddressingVersion.WSAddressing10, writer);
      writer.WriteEndElement();
    }

    public static EndpointAddress ReadAppliesTo(
      XmlReader reader,
      WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      EndpointAddress endpointAddress = (EndpointAddress) null;
      if (!reader.IsEmptyElement)
      {
        reader.ReadStartElement();
        endpointAddress = EndpointAddress.ReadFrom(XmlDictionaryReader.CreateDictionaryReader(reader));
        reader.ReadEndElement();
      }
      return !(endpointAddress == (EndpointAddress) null) ? endpointAddress : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3162")));
    }

    public static void WriteAppliesTo(
      XmlWriter writer,
      EndpointAddress appliesTo,
      WSTrustConstantsAdapter trustConstants)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (appliesTo == (EndpointAddress) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (appliesTo));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      writer.WriteStartElement("wsp", "AppliesTo", "http://schemas.xmlsoap.org/ws/2004/09/policy");
      appliesTo.WriteTo(AddressingVersion.WSAddressing10, writer);
      writer.WriteEndElement();
    }

    public static string ReadKeyType(XmlReader reader, WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      string uriString = reader.ReadElementContentAsString();
      if (!UriUtil.CanCreateValidUri(uriString, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.KeyType, (object) trustConstants.NamespaceURI, (object) uriString)));
      if (trustConstants.KeyTypes.Symmetric.Equals(uriString))
        return "http://schemas.microsoft.com/idfx/keytype/symmetric";
      if (trustConstants.KeyTypes.Asymmetric.Equals(uriString))
        return "http://schemas.microsoft.com/idfx/keytype/asymmetric";
      if (trustConstants.KeyTypes.Bearer.Equals(uriString))
        return "http://schemas.microsoft.com/idfx/keytype/bearer";
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3020", (object) uriString)));
    }

    public static void WriteKeyType(
      XmlWriter writer,
      string keyType,
      WSTrustConstantsAdapter trustConstants)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (string.IsNullOrEmpty(keyType))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (keyType));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      if (!UriUtil.CanCreateValidUri(keyType, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) trustConstants.Elements.KeyType, (object) trustConstants.NamespaceURI, (object) keyType)));
      string str;
      if (StringComparer.Ordinal.Equals(keyType, "http://schemas.microsoft.com/idfx/keytype/asymmetric") || StringComparer.Ordinal.Equals(keyType, trustConstants.KeyTypes.Asymmetric))
        str = trustConstants.KeyTypes.Asymmetric;
      else if (StringComparer.Ordinal.Equals(keyType, "http://schemas.microsoft.com/idfx/keytype/symmetric") || StringComparer.Ordinal.Equals(keyType, trustConstants.KeyTypes.Symmetric))
        str = trustConstants.KeyTypes.Symmetric;
      else if (StringComparer.Ordinal.Equals(keyType, "http://schemas.microsoft.com/idfx/keytype/bearer") || StringComparer.Ordinal.Equals(keyType, trustConstants.KeyTypes.Bearer))
        str = trustConstants.KeyTypes.Bearer;
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3010", (object) keyType)));
      writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.KeyType, trustConstants.NamespaceURI, str);
    }

    public static DisplayToken ReadRequestedDisplayToken(
      XmlReader reader,
      WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      DisplayToken displayToken = (DisplayToken) null;
      if (!reader.IsEmptyElement)
      {
        reader.ReadStartElement("RequestedDisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity");
        if (!reader.IsStartElement("DisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3030", (object) "DisplayToken", (object) "http://schemas.xmlsoap.org/ws/2005/05/identity")));
        string language = (string) null;
        while (reader.MoveToNextAttribute())
        {
          if (StringComparer.Ordinal.Equals("xml:lang", reader.Name))
          {
            language = reader.Value;
            break;
          }
        }
        if (string.IsNullOrEmpty(language))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3031", (object) "DisplayToken", (object) "xml:lang")));
        reader.Read();
        IList<DisplayClaim> displayClaimList = (IList<DisplayClaim>) new List<DisplayClaim>();
        while (reader.IsStartElement("DisplayClaim", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
        {
          bool isEmptyElement = reader.IsEmptyElement;
          string claimType = (string) null;
          while (reader.MoveToNextAttribute())
          {
            if (StringComparer.Ordinal.Equals("Uri", reader.Name))
            {
              claimType = reader.Value;
              break;
            }
          }
          if (claimType == null)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3031", (object) "DisplayClaim", (object) "Uri")));
          reader.Read();
          int content = (int) reader.MoveToContent();
          DisplayClaim displayClaim = new DisplayClaim(claimType);
          if (!isEmptyElement)
          {
            if (reader.IsStartElement("DisplayTag", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
              displayClaim.DisplayTag = reader.ReadElementContentAsString("DisplayTag", "http://schemas.xmlsoap.org/ws/2005/05/identity");
            if (reader.IsStartElement("Description", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
              displayClaim.Description = reader.ReadElementContentAsString("Description", "http://schemas.xmlsoap.org/ws/2005/05/identity");
            if (reader.IsStartElement("DisplayValue", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
              displayClaim.DisplayValue = reader.ReadElementContentAsString("DisplayValue", "http://schemas.xmlsoap.org/ws/2005/05/identity");
            reader.ReadEndElement();
          }
          displayClaimList.Add(displayClaim);
        }
        if (displayClaimList.Count == 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3030", (object) "DisplayClaim", (object) "http://schemas.xmlsoap.org/ws/2005/05/identity")));
        reader.ReadEndElement();
        displayToken = new DisplayToken(language, (IEnumerable<DisplayClaim>) displayClaimList);
        reader.ReadEndElement();
      }
      return displayToken != null ? displayToken : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3163")));
    }

    public static void WriteRequestedDisplayToken(
      XmlWriter writer,
      DisplayToken requestedDisplayToken,
      WSTrustConstantsAdapter trustConstants)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (requestedDisplayToken == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestedDisplayToken));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      writer.WriteStartElement("i", "RequestedDisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity");
      writer.WriteStartElement("i", "DisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity");
      Microsoft.IdentityModel.XmlUtil.WriteLanguageAttribute(writer, requestedDisplayToken.Language);
      for (int index = 0; index < requestedDisplayToken.DisplayClaims.Count; ++index)
      {
        DisplayClaim displayClaim = requestedDisplayToken.DisplayClaims[index];
        writer.WriteStartElement("i", "DisplayClaim", "http://schemas.xmlsoap.org/ws/2005/05/identity");
        writer.WriteAttributeString("Uri", displayClaim.ClaimType);
        if (!string.IsNullOrEmpty(displayClaim.DisplayTag))
          writer.WriteElementString("DisplayTag", "http://schemas.xmlsoap.org/ws/2005/05/identity", displayClaim.DisplayTag);
        if (!string.IsNullOrEmpty(displayClaim.Description))
          writer.WriteElementString("Description", "http://schemas.xmlsoap.org/ws/2005/05/identity", displayClaim.Description);
        if (!string.IsNullOrEmpty(displayClaim.DisplayValue))
          writer.WriteElementString("DisplayValue", "http://schemas.xmlsoap.org/ws/2005/05/identity", displayClaim.DisplayValue);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    public static XmlElement ReadInnerXml(XmlReader reader) => WSTrustSerializationHelper.ReadInnerXml(reader, false);

    public static XmlElement ReadInnerXml(XmlReader reader, bool onStartElement)
    {
      string str = reader != null ? reader.LocalName : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      string namespaceUri = reader.NamespaceURI;
      if (reader.IsEmptyElement)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3061", (object) str, (object) namespaceUri)));
      if (!onStartElement)
        reader.ReadStartElement();
      int content = (int) reader.MoveToContent();
      XmlElement documentElement;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (XmlWriter textWriter = (XmlWriter) XmlDictionaryWriter.CreateTextWriter((Stream) memoryStream, Encoding.UTF8, false))
        {
          textWriter.WriteNode(reader, true);
          textWriter.Flush();
        }
        memoryStream.Seek(0L, SeekOrigin.Begin);
        if (memoryStream.Length == 0L)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3061", (object) str, (object) namespaceUri)));
        XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader((Stream) memoryStream, Encoding.UTF8, XmlDictionaryReaderQuotas.Max, (OnXmlDictionaryReaderClose) null);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.PreserveWhitespace = true;
        xmlDocument.Load(XmlReader.Create((XmlReader) textReader, new XmlReaderSettings()
        {
          XmlResolver = (XmlResolver) null
        }));
        documentElement = xmlDocument.DocumentElement;
      }
      if (!onStartElement)
        reader.ReadEndElement();
      return documentElement;
    }

    public static BinarySecretSecurityToken ReadBinarySecretSecurityToken(
      XmlReader reader,
      WSTrustConstantsAdapter trustConstants)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      string s = reader.ReadElementContentAsString(trustConstants.Elements.BinarySecret, trustConstants.NamespaceURI);
      return !string.IsNullOrEmpty(s) ? new BinarySecretSecurityToken(Convert.FromBase64String(s)) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3164")));
    }

    public static void WriteBinarySecretSecurityToken(
      XmlWriter writer,
      BinarySecretSecurityToken token,
      WSTrustConstantsAdapter trustConstants)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (token == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (token));
      if (trustConstants == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustConstants));
      byte[] keyBytes = token.GetKeyBytes();
      writer.WriteStartElement(trustConstants.Elements.BinarySecret, trustConstants.NamespaceURI);
      writer.WriteBase64(keyBytes, 0, keyBytes.Length);
      writer.WriteEndElement();
    }

    private static string GetRequestClaimNamespace(string dialect) => StringComparer.Ordinal.Equals(dialect, "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims") ? "http://docs.oasis-open.org/wsfed/authorization/200706" : "http://schemas.xmlsoap.org/ws/2005/05/identity";

    private static string GetRequestClaimPrefix(string dialect) => StringComparer.Ordinal.Equals(dialect, "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims") ? "auth" : "i";
  }
}
