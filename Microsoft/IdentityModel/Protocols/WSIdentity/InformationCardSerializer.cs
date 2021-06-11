// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.InformationCardSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Protocols.XmlSignature;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class InformationCardSerializer
  {
    private bool _allowUnknownElements = true;
    private SecurityTokenSerializer _tokenSerializer;
    private WSTrustConstantsAdapter _trustConstants = (WSTrustConstantsAdapter) WSTrustConstantsAdapter.TrustFeb2005;

    public InformationCardSerializer()
      : this((SecurityTokenSerializer) new WSSecurityTokenSerializer(SecurityVersion.WSSecurity11, TrustVersion.WSTrust13, SecureConversationVersion.WSSecureConversation13, false, (SamlSerializer) null, (SecurityStateEncoder) null, (IEnumerable<System.Type>) null, WSSecurityTokenSerializer.DefaultInstance.MaximumKeyDerivationOffset, WSSecurityTokenSerializer.DefaultInstance.MaximumKeyDerivationLabelLength, WSSecurityTokenSerializer.DefaultInstance.MaximumKeyDerivationNonceLength))
    {
    }

    public InformationCardSerializer(SecurityTokenSerializer securityTokenSerializer) => this._tokenSerializer = securityTokenSerializer != null ? securityTokenSerializer : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenSerializer));

    public bool AllowUnknownElements
    {
      get => this._allowUnknownElements;
      set => this._allowUnknownElements = value;
    }

    public void WriteCard(Stream stream, InformationCard card)
    {
      if (stream == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (stream));
      using (XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, false))
        this.WriteCard((XmlWriter) textWriter, card);
    }

    public void WriteCard(XmlWriter writer, InformationCard card)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (card == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (card));
      this.WriteCardProperties(card.SigningCredentials == null ? XmlDictionaryWriter.CreateDictionaryWriter(writer) : (XmlDictionaryWriter) new EnvelopingSignatureWriter(writer, card.SigningCredentials, "_Object_InformationCard", this._tokenSerializer), card);
      writer.Flush();
    }

    public InformationCard ReadCard(Stream stream) => stream != null ? this.ReadCard((XmlReader) XmlDictionaryReader.CreateTextReader(stream, XmlDictionaryReaderQuotas.Max)) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (stream));

    public InformationCard ReadCard(XmlReader reader) => reader != null ? this.ReadCard(reader, Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));

    public InformationCard ReadCard(
      XmlReader reader,
      SecurityTokenResolver signingTokenResolver)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (signingTokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (signingTokenResolver));
      SigningCredentials signingCredentials = (SigningCredentials) null;
      XmlDictionaryReader reader1;
      if (reader.IsStartElement("Signature", "http://www.w3.org/2000/09/xmldsig#"))
      {
        reader1 = (XmlDictionaryReader) new EnvelopingSignatureReader(reader, this._tokenSerializer, signingTokenResolver);
        signingCredentials = ((EnvelopingSignatureReader) reader1).SigningCredentials;
      }
      else
        reader1 = XmlDictionaryReader.CreateDictionaryReader(reader);
      return this.ReadCardProperties(reader1, signingCredentials);
    }

    public SecurityTokenSerializer SecurityTokenSerializer => this._tokenSerializer;

    private static void VerifyPPIDValidBase64String(string ppid)
    {
      if (string.IsNullOrEmpty(ppid))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (ppid));
      try
      {
        Convert.FromBase64String(ppid);
      }
      catch (FormatException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3126", (object) ppid), (Exception) ex));
      }
    }

    private static T GetTypedValue<T>(object value, string contextString)
    {
      if (value == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
      if (string.IsNullOrEmpty(contextString))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(nameof (contextString)));
      return value is T obj ? obj : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3065", (object) value.GetType(), (object) contextString, (object) typeof (T))));
    }

    protected virtual InformationCard CreateCard(
      string issuer,
      SigningCredentials signingCredentials)
    {
      if (issuer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (issuer));
      if (!UriUtil.CanCreateValidUri(issuer, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3042", (object) issuer)));
      return signingCredentials == null ? new InformationCard(issuer) : new InformationCard(signingCredentials, issuer);
    }

    protected virtual void ReadAdditionalElements(XmlDictionaryReader reader, InformationCard card)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      while (reader.IsStartElement())
      {
        if (this.AllowUnknownElements && !StringComparer.Ordinal.Equals(reader.NamespaceURI, "http://schemas.xmlsoap.org/ws/2005/05/identity"))
          reader.Skip();
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3208", (object) reader.Name)));
      }
    }

    protected virtual PrivacyNotice ReadPrivacyNotice(XmlDictionaryReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      PrivacyNotice privacyNotice = (PrivacyNotice) null;
      if (reader.IsStartElement("PrivacyNotice", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
      {
        string attribute = reader.GetAttribute("Version", (string) null);
        long version = 1;
        if (!string.IsNullOrEmpty(attribute))
        {
          version = long.Parse(attribute, (IFormatProvider) CultureInfo.InvariantCulture);
          if (version < 1L || version > (long) uint.MaxValue)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3054", (object) (long) uint.MaxValue)));
        }
        reader.Read();
        int content1 = (int) reader.MoveToContent();
        string str = reader.ReadString();
        if (!UriUtil.CanCreateValidUri(str, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3070", (object) str)));
        int content2 = (int) reader.MoveToContent();
        reader.ReadEndElement();
        privacyNotice = new PrivacyNotice(str, version);
      }
      return privacyNotice;
    }

    protected virtual DisplayClaimCollection ReadSupportedClaimList(
      XmlDictionaryReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      DisplayClaimCollection displayClaimCollection = new DisplayClaimCollection();
      if (reader.IsStartElement("SupportedClaimTypeList", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
      {
        bool isEmptyElement1 = reader.IsEmptyElement;
        reader.ReadStartElement("SupportedClaimTypeList", "http://schemas.xmlsoap.org/ws/2005/05/identity");
        while (reader.IsStartElement("SupportedClaimType", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
        {
          if (displayClaimCollection.Count == 128)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3050", (object) 128)));
          string attribute = reader.GetAttribute("Uri", (string) null);
          DisplayClaim displayClaim = UriUtil.CanCreateValidUri(attribute, UriKind.Absolute) ? new DisplayClaim(attribute) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3051", (object) attribute)));
          bool isEmptyElement2 = reader.IsEmptyElement;
          reader.Read();
          int content = (int) reader.MoveToContent();
          if (!isEmptyElement2)
          {
            if (reader.IsStartElement("DisplayTag", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
            {
              string str = reader.ReadElementContentAsString("DisplayTag", "http://schemas.xmlsoap.org/ws/2005/05/identity");
              displayClaim.DisplayTag = str.Length >= 1 && str.Length <= (int) byte.MaxValue ? str : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3052", (object) str.Length)));
            }
            if (reader.IsStartElement("Description", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
            {
              string str = reader.ReadElementContentAsString("Description", "http://schemas.xmlsoap.org/ws/2005/05/identity");
              displayClaim.Description = str.Length >= 1 && str.Length <= (int) byte.MaxValue ? str : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3053", (object) str.Length)));
            }
            reader.ReadEndElement();
          }
          displayClaimCollection.Add(displayClaim);
        }
        if (!isEmptyElement1)
          reader.ReadEndElement();
      }
      return displayClaimCollection;
    }

    protected virtual TokenServiceCollection ReadTokenServiceList(
      XmlDictionaryReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      TokenServiceCollection serviceCollection = new TokenServiceCollection();
      if (reader.IsStartElement("TokenServiceList", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
      {
        if (reader.IsEmptyElement)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3176")));
        reader.ReadStartElement();
        int num = 0;
        while (reader.IsStartElement("TokenService", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
        {
          ++num;
          if (num > 128)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3045", (object) 128)));
          if (reader.IsEmptyElement)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3178")));
          reader.ReadStartElement("TokenService", "http://schemas.xmlsoap.org/ws/2005/05/identity");
          EndpointAddress endpointAddress = EndpointAddress.ReadFrom(reader);
          if (!reader.IsStartElement("UserCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3177")));
          reader.ReadStartElement("UserCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
          string displayCredentialHint = (string) null;
          if (reader.IsStartElement("DisplayCredentialHint", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
          {
            displayCredentialHint = reader.ReadElementContentAsString("DisplayCredentialHint", "http://schemas.xmlsoap.org/ws/2005/05/identity");
            if (displayCredentialHint.Length <= 0 || displayCredentialHint.Length > 64)
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3068", (object) displayCredentialHint)));
          }
          IUserCredential userCredential = this.ReadUserCredential(reader);
          reader.ReadEndElement();
          reader.ReadEndElement();
          serviceCollection.Add(new TokenService(endpointAddress, userCredential, displayCredentialHint));
        }
        if (num == 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3067")));
        reader.ReadEndElement();
      }
      return serviceCollection;
    }

    protected virtual IUserCredential ReadUnrecognizedUserCredential(
      XmlDictionaryReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3046", (object) reader.LocalName, (object) reader.NamespaceURI)));
    }

    protected virtual IUserCredential ReadX509UserCredential(
      XmlDictionaryReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("X509V3Credential", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
        reader.ReadStartElement("X509V3Credential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
      if (reader.IsEmptyElement)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3181")));
      reader.ReadStartElement();
      if (!reader.IsStartElement("X509Data", "http://www.w3.org/2000/09/xmldsig#"))
        reader.ReadStartElement("X509Data", "http://www.w3.org/2000/09/xmldsig#");
      if (reader.IsEmptyElement)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3186")));
      reader.ReadStartElement();
      IUserCredential userCredential;
      if (reader.IsStartElement("X509Principal", "http://docs.oasis-open.org/imi/ns/identity-200903"))
        userCredential = (IUserCredential) new X509CertificateCredential(this.ReadX509Principal(reader));
      else if (reader.IsStartElement("X509SubjectAndIssuer", "http://docs.oasis-open.org/imi/ns/identity-200903"))
        userCredential = (IUserCredential) new X509CertificateCredential(this.ReadX509SubjectAndIssuer(reader));
      else if (reader.IsStartElement("X509SubjectName", "http://www.w3.org/2000/09/xmldsig#"))
        userCredential = !reader.IsEmptyElement ? (IUserCredential) new X509CertificateCredential(reader.ReadElementString("X509SubjectName", "http://www.w3.org/2000/09/xmldsig#")) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3279")));
      else
        userCredential = (IUserCredential) new X509CertificateCredential(this.ReadX509SecurityKeyIdentifierClause(reader));
      reader.ReadEndElement();
      reader.ReadEndElement();
      return userCredential;
    }

    protected virtual void WriteAdditionalElements(XmlDictionaryWriter writer, InformationCard card)
    {
    }

    protected virtual void WritePrivacyNotice(XmlDictionaryWriter writer, InformationCard card)
    {
      if (card.PrivacyNotice == null)
        return;
      if (card.PrivacyNotice.Version < 1L || card.PrivacyNotice.Version > (long) uint.MaxValue)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3054", (object) (long) uint.MaxValue)));
      if (!UriUtil.CanCreateValidUri(card.PrivacyNotice.Location, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3070", (object) card.PrivacyNotice.Location)));
      writer.WriteStartElement("i", "PrivacyNotice", "http://schemas.xmlsoap.org/ws/2005/05/identity");
      writer.WriteAttributeString("Version", Convert.ToString(card.PrivacyNotice.Version, (IFormatProvider) CultureInfo.InvariantCulture));
      writer.WriteString(card.PrivacyNotice.Location);
      writer.WriteEndElement();
    }

    protected virtual void WriteSupportedClaimTypeList(
      XmlDictionaryWriter writer,
      InformationCard card)
    {
      if (card.SupportedClaimTypeList.Count > 128)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3050", (object) 128)));
      if (card.SupportedClaimTypeList.Count <= 0)
        return;
      writer.WriteStartElement("i", "SupportedClaimTypeList", "http://schemas.xmlsoap.org/ws/2005/05/identity");
      for (int index = 0; index < card.SupportedClaimTypeList.Count; ++index)
      {
        DisplayClaim supportedClaimType = card.SupportedClaimTypeList[index];
        if (!UriUtil.CanCreateValidUri(supportedClaimType.ClaimType, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3051", (object) supportedClaimType.ClaimType)));
        writer.WriteStartElement("i", "SupportedClaimType", "http://schemas.xmlsoap.org/ws/2005/05/identity");
        writer.WriteAttributeString("Uri", supportedClaimType.ClaimType);
        if (!string.IsNullOrEmpty(supportedClaimType.DisplayTag))
        {
          if (supportedClaimType.DisplayTag.Length < 1 || supportedClaimType.DisplayTag.Length > (int) byte.MaxValue)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3052", (object) supportedClaimType.DisplayTag.Length)));
          writer.WriteElementString("i", "DisplayTag", "http://schemas.xmlsoap.org/ws/2005/05/identity", supportedClaimType.DisplayTag);
        }
        if (!string.IsNullOrEmpty(supportedClaimType.Description))
        {
          if (supportedClaimType.Description.Length < 1 || supportedClaimType.Description.Length > (int) byte.MaxValue)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3053", (object) supportedClaimType.Description.Length)));
          writer.WriteElementString("i", "Description", "http://schemas.xmlsoap.org/ws/2005/05/identity", supportedClaimType.Description);
        }
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }

    protected virtual void WriteUnrecognizedUserCredential(
      XmlDictionaryWriter writer,
      IUserCredential credential)
    {
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3062")));
    }

    protected virtual void WriteX509UserCredential(
      XmlDictionaryWriter writer,
      IUserCredential credential)
    {
      writer.WriteStartElement("i", "X509V3Credential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
      X509CertificateCredential typedValue = InformationCardSerializer.GetTypedValue<X509CertificateCredential>((object) credential, "X509V3Credential");
      writer.WriteStartElement("ds", "X509Data", "http://www.w3.org/2000/09/xmldsig#");
      if (typedValue.X509SecurityTokenIdentifierClause != null)
        this.WriteX509SecurityKeyIdentifierClause(writer, typedValue.X509SecurityTokenIdentifierClause);
      else if (!string.IsNullOrEmpty(typedValue.X509SubjectName))
        writer.WriteElementString("ds", "X509SubjectName", "http://www.w3.org/2000/09/xmldsig#", typedValue.X509SubjectName);
      else if (typedValue.X509Principal != null)
        this.WriteX509Principal(writer, typedValue.X509Principal);
      else if (typedValue.X509SubjectAndIssuer != null)
        this.WriteX509SubjectAndIssuer(writer, typedValue.X509SubjectAndIssuer);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    protected virtual void WriteTokenServiceList(XmlDictionaryWriter writer, InformationCard card)
    {
      if (card.TokenServiceList.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3067")));
      if (card.TokenServiceList.Count > 128)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3045", (object) 128)));
      writer.WriteStartElement("i", "TokenServiceList", "http://schemas.xmlsoap.org/ws/2005/05/identity");
      for (int index = 0; index < card.TokenServiceList.Count; ++index)
      {
        TokenService tokenService = card.TokenServiceList[index];
        writer.WriteStartElement("i", "TokenService", "http://schemas.xmlsoap.org/ws/2005/05/identity");
        tokenService.Address.WriteTo(AddressingVersion.WSAddressing10, writer);
        writer.WriteStartElement("i", "UserCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
        if (!string.IsNullOrEmpty(tokenService.DisplayCredentialHint))
        {
          if (tokenService.DisplayCredentialHint.Length > 64)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3068", (object) tokenService.DisplayCredentialHint)));
          writer.WriteElementString("i", "DisplayCredentialHint", "http://schemas.xmlsoap.org/ws/2005/05/identity", tokenService.DisplayCredentialHint);
        }
        this.WriteUserCredential(writer, tokenService.UserCredential);
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }

    private void WriteCardProperties(XmlDictionaryWriter writer, InformationCard card)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (card == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (card));
      writer.WriteStartElement("i", "InformationCard", "http://schemas.xmlsoap.org/ws/2005/05/identity");
      if (string.IsNullOrEmpty(card.Language))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3029", (object) "xml:lang")));
      Microsoft.IdentityModel.XmlUtil.WriteLanguageAttribute((XmlWriter) writer, card.Language);
      if (card.InformationCardReference != null)
      {
        writer.WriteStartElement("i", "InformationCardReference", "http://schemas.xmlsoap.org/ws/2005/05/identity");
        if (!UriUtil.CanCreateValidUri(card.InformationCardReference.CardId, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3040")));
        writer.WriteElementString("i", "CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity", card.InformationCardReference.CardId);
        if (card.InformationCardReference.CardVersion < 1L || card.InformationCardReference.CardVersion > (long) uint.MaxValue)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3066", (object) (long) uint.MaxValue)));
        writer.WriteElementString("i", "CardVersion", "http://schemas.xmlsoap.org/ws/2005/05/identity", Convert.ToString(card.InformationCardReference.CardVersion, (IFormatProvider) CultureInfo.InvariantCulture));
        writer.WriteEndElement();
        if (!string.IsNullOrEmpty(card.CardName))
        {
          if (card.CardName.Length < 1 || card.CardName.Length > (int) byte.MaxValue)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3041")));
          writer.WriteElementString("i", "CardName", "http://schemas.xmlsoap.org/ws/2005/05/identity", card.CardName);
        }
        if (card.CardImage != null)
        {
          if (!CardImage.IsValidMimeType(card.CardImage.MimeType))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3043", (object) card.CardImage.MimeType)));
          if (card.CardImage.GetImage() == null || card.CardImage.GetImage().Length == 0)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3044")));
          string base64String = Convert.ToBase64String(card.CardImage.GetImage());
          if (base64String.Length > 1048576)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3123", (object) 1048576)));
          writer.WriteStartElement("i", "CardImage", "http://schemas.xmlsoap.org/ws/2005/05/identity");
          writer.WriteAttributeString("MimeType", card.CardImage.MimeType);
          writer.WriteString(base64String);
          writer.WriteEndElement();
        }
        if (card.Issuer != null)
        {
          if (!UriUtil.CanCreateValidUri(card.Issuer, UriKind.Absolute))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3042", (object) card.Issuer)));
          writer.WriteElementString("i", "Issuer", "http://schemas.xmlsoap.org/ws/2005/05/identity", card.Issuer);
          if (card.TimeIssued.HasValue)
          {
            writer.WriteElementString("i", "TimeIssued", "http://schemas.xmlsoap.org/ws/2005/05/identity", card.TimeIssued.Value.ToString(DateTimeFormats.Generated, (IFormatProvider) CultureInfo.InvariantCulture));
            if (card.TimeExpires.HasValue)
            {
              if (card.TimeIssued.Value.CompareTo(card.TimeExpires.Value) > 0)
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3125")));
              writer.WriteElementString("i", "TimeExpires", "http://schemas.xmlsoap.org/ws/2005/05/identity", card.TimeExpires.Value.ToString(DateTimeFormats.Generated, (IFormatProvider) CultureInfo.InvariantCulture));
            }
            this.WriteTokenServiceList(writer, card);
            if (card.SupportedTokenTypeList.Count == 0)
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3047")));
            if (card.SupportedTokenTypeList.Count > 32)
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3049", (object) 32)));
            writer.WriteStartElement("i", "SupportedTokenTypeList", "http://schemas.xmlsoap.org/ws/2005/05/identity");
            for (int index = 0; index < card.SupportedTokenTypeList.Count; ++index)
            {
              string supportedTokenType = card.SupportedTokenTypeList[index];
              if (!UriUtil.CanCreateValidUri(supportedTokenType, UriKind.Absolute))
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3209", (object) supportedTokenType)));
              writer.WriteElementString(this._trustConstants.Prefix, this._trustConstants.Elements.TokenType, this._trustConstants.NamespaceURI, supportedTokenType);
            }
            writer.WriteEndElement();
            this.WriteSupportedClaimTypeList(writer, card);
            AppliesToOption? appliesToOption1 = card.AppliesToOption;
            if ((appliesToOption1.GetValueOrDefault() != AppliesToOption.NotAllowed ? 1 : (!appliesToOption1.HasValue ? 1 : 0)) != 0)
            {
              writer.WriteStartElement("i", "RequireAppliesTo", "http://schemas.xmlsoap.org/ws/2005/05/identity");
              XmlDictionaryWriter dictionaryWriter = writer;
              AppliesToOption? appliesToOption2 = card.AppliesToOption;
              string str = (appliesToOption2.GetValueOrDefault() != AppliesToOption.Optional ? 0 : (appliesToOption2.HasValue ? 1 : 0)) != 0 ? "true" : "false";
              dictionaryWriter.WriteAttributeString("Optional", str);
              writer.WriteEndElement();
            }
            this.WritePrivacyNotice(writer, card);
            if (card.IssuerInformation.Count > 0)
            {
              writer.WriteStartElement("ic07", "IssuerInformation", "http://schemas.xmlsoap.org/ws/2007/01/identity");
              foreach (IssuerInformation issuerInformation in card.IssuerInformation)
              {
                if (issuerInformation.Key.Length < 1 || issuerInformation.Key.Length > (int) byte.MaxValue)
                  throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3115", (object) issuerInformation.Key, (object) 1, (object) (int) byte.MaxValue)));
                if (issuerInformation.Value.Length < 1 || issuerInformation.Value.Length > (int) byte.MaxValue)
                  throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3116", (object) issuerInformation.Value, (object) 1, (object) (int) byte.MaxValue)));
                writer.WriteStartElement("ic07", "IssuerInformationEntry", "http://schemas.xmlsoap.org/ws/2007/01/identity");
                writer.WriteElementString("ic07", "EntryName", "http://schemas.xmlsoap.org/ws/2007/01/identity", issuerInformation.Key);
                writer.WriteElementString("ic07", "EntryValue", "http://schemas.xmlsoap.org/ws/2007/01/identity", issuerInformation.Value);
                writer.WriteEndElement();
              }
              writer.WriteEndElement();
            }
            if (card.RequireStrongRecipientIdentity.HasValue && card.RequireStrongRecipientIdentity.Value)
            {
              writer.WriteStartElement("ic07", "RequireStrongRecipientIdentity", "http://schemas.xmlsoap.org/ws/2007/01/identity");
              writer.WriteEndElement();
            }
            if (card.CardType != (Uri) null)
              writer.WriteElementString("ic09", "CardType", "http://docs.oasis-open.org/imi/ns/identity-200903", card.CardType.OriginalString);
            if (!string.IsNullOrEmpty(card.IssuerName))
            {
              if (card.IssuerName.Length < 1 || card.IssuerName.Length > 64)
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3259")));
              writer.WriteElementString("ic09", "IssuerName", "http://docs.oasis-open.org/imi/ns/identity-200903", card.IssuerName);
            }
            this.WriteAdditionalElements(writer, card);
            writer.WriteEndElement();
          }
          else
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3029", (object) "TimeIssued")));
        }
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3029", (object) "Issuer")));
      }
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3029", (object) "InformationCardReference")));
    }

    private void WriteEkuPolicy(XmlDictionaryWriter writer, EkuPolicy ekuPolicy)
    {
      if (ekuPolicy == null)
        return;
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      writer.WriteStartElement("ic09", "EKUPolicy", "http://docs.oasis-open.org/imi/ns/identity-200903");
      foreach (Oid oid in ekuPolicy.Oids)
        writer.WriteElementString("ic09", "OID", "http://docs.oasis-open.org/imi/ns/identity-200903", oid.Value);
      writer.WriteEndElement();
    }

    private void WriteUserCredential(XmlDictionaryWriter writer, IUserCredential credential)
    {
      switch (credential.CredentialType)
      {
        case UserCredentialType.KerberosV5Credential:
          writer.WriteStartElement("i", "KerberosV5Credential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
          writer.WriteEndElement();
          break;
        case UserCredentialType.SelfIssuedCredential:
          string ppid = ((SelfIssuedCredentials) credential).PPID;
          if (string.IsNullOrEmpty(ppid))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3129")));
          InformationCardSerializer.VerifyPPIDValidBase64String(ppid);
          if (ppid.Length > 1024)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3127", (object) ppid, (object) 1024)));
          writer.WriteStartElement("i", "SelfIssuedCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
          writer.WriteElementString("i", "PrivatePersonalIdentifier", "http://schemas.xmlsoap.org/ws/2005/05/identity", ppid);
          writer.WriteEndElement();
          break;
        case UserCredentialType.UserNamePasswordCredential:
          writer.WriteStartElement("i", "UsernamePasswordCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
          UserNamePasswordCredential typedValue = InformationCardSerializer.GetTypedValue<UserNamePasswordCredential>((object) credential, "UsernamePasswordCredential");
          if (!string.IsNullOrEmpty(typedValue.UserName))
          {
            if (typedValue.UserName.Length > (int) byte.MaxValue)
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3069", (object) typedValue.UserName)));
            writer.WriteElementString("i", "Username", "http://schemas.xmlsoap.org/ws/2005/05/identity", typedValue.UserName);
          }
          writer.WriteEndElement();
          break;
        case UserCredentialType.X509V3Credential:
          this.WriteX509UserCredential(writer, credential);
          break;
        default:
          this.WriteUnrecognizedUserCredential(writer, credential);
          break;
      }
    }

    private void WriteX509Principal(XmlDictionaryWriter writer, X509Principal x509Principal)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (x509Principal == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (x509Principal));
      writer.WriteStartElement("ic09", "X509Principal", "http://docs.oasis-open.org/imi/ns/identity-200903");
      writer.WriteElementString("ic09", "PrincipalName", "http://docs.oasis-open.org/imi/ns/identity-200903", x509Principal.PrincipalName);
      this.WriteEkuPolicy(writer, x509Principal.EkuPolicy);
      writer.WriteEndElement();
    }

    private void WriteX509SecurityKeyIdentifierClause(
      XmlDictionaryWriter writer,
      SecurityKeyIdentifierClause keyIdentifierClause)
    {
      switch (keyIdentifierClause)
      {
        case X509ThumbprintKeyIdentifierClause identifierClause:
          writer.WriteStartElement("wsse", "KeyIdentifier", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
          writer.WriteAttributeString("ValueType", "http://docs.oasis-open.org/wss/2004/xx/oasis-2004xx-wss-soap-message-security-1.1#ThumbprintSHA1");
          writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
          byte[] x509Thumbprint = identifierClause.GetX509Thumbprint();
          writer.WriteBase64(x509Thumbprint, 0, x509Thumbprint.Length);
          writer.WriteEndElement();
          break;
        case X509IssuerSerialKeyIdentifierClause identifierClause:
          writer.WriteStartElement("ds", "X509IssuerSerial", "http://www.w3.org/2000/09/xmldsig#");
          writer.WriteElementString("ds", "X509IssuerName", "http://www.w3.org/2000/09/xmldsig#", identifierClause.IssuerName);
          writer.WriteElementString("ds", "X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#", identifierClause.IssuerSerialNumber);
          writer.WriteEndElement();
          break;
        case X509RawDataKeyIdentifierClause identifierClause:
          string base64String1 = Convert.ToBase64String(identifierClause.GetX509RawData());
          writer.WriteElementString("ds", "X509Certificate", "http://www.w3.org/2000/09/xmldsig#", base64String1);
          break;
        case X509SubjectKeyIdentifierClause identifierClause:
          string base64String2 = Convert.ToBase64String(identifierClause.GetX509SubjectKeyIdentifier());
          writer.WriteElementString("ds", "X509SKI", "http://www.w3.org/2000/09/xmldsig#", base64String2);
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3060", (object) keyIdentifierClause.GetType())));
      }
    }

    private void WriteX509SubjectAndIssuer(
      XmlDictionaryWriter writer,
      X509SubjectAndIssuer x509SubjectAndIssuer)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (x509SubjectAndIssuer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (x509SubjectAndIssuer));
      writer.WriteStartElement("ic09", "X509SubjectAndIssuer", "http://docs.oasis-open.org/imi/ns/identity-200903");
      writer.WriteElementString("ic09", "X509Subject", "http://docs.oasis-open.org/imi/ns/identity-200903", x509SubjectAndIssuer.X509Subject);
      writer.WriteElementString("ic09", "X509Issuer", "http://docs.oasis-open.org/imi/ns/identity-200903", x509SubjectAndIssuer.X509Issuer);
      this.WriteEkuPolicy(writer, x509SubjectAndIssuer.EkuPolicy);
      writer.WriteEndElement();
    }

    private InformationCard ReadCardProperties(
      XmlDictionaryReader reader,
      SigningCredentials signingCredentials)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("InformationCard", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3128", (object) "InformationCard", (object) "http://schemas.xmlsoap.org/ws/2005/05/identity")));
      if (reader.IsEmptyElement)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3171", (object) reader.LocalName, (object) reader.NamespaceURI)));
      reader.MoveToStartElement("InformationCard", "http://schemas.xmlsoap.org/ws/2005/05/identity");
      string attribute1 = reader.GetAttribute("xml:lang");
      reader.Read();
      if (!reader.IsStartElement("InformationCardReference", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3172")));
      if (reader.IsEmptyElement)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3173")));
      reader.ReadStartElement("InformationCardReference", "http://schemas.xmlsoap.org/ws/2005/05/identity");
      string str1 = reader.IsStartElement("CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity") ? reader.ReadElementString("CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3174")));
      if (!UriUtil.CanCreateValidUri(str1, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3040")));
      long cardVersion = reader.IsStartElement("CardVersion", "http://schemas.xmlsoap.org/ws/2005/05/identity") ? Convert.ToInt64(reader.ReadElementString("CardVersion", "http://schemas.xmlsoap.org/ws/2005/05/identity"), (IFormatProvider) CultureInfo.InvariantCulture) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3175")));
      if (cardVersion < 1L || cardVersion > (long) uint.MaxValue)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3262", (object) (long) uint.MaxValue)));
      reader.ReadEndElement();
      string str2 = (string) null;
      if (reader.IsStartElement("CardName", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
      {
        str2 = reader.ReadElementString("CardName", "http://schemas.xmlsoap.org/ws/2005/05/identity");
        if (string.IsNullOrEmpty(str2) || str2.Length > (int) byte.MaxValue)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3041")));
      }
      CardImage cardImage = (CardImage) null;
      if (reader.IsStartElement("CardImage", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
      {
        string attribute2 = reader.GetAttribute("MimeType", (string) null);
        if (!CardImage.IsValidMimeType(attribute2))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3043", (object) attribute2)));
        string s = reader.ReadElementContentAsString();
        byte[] image = !string.IsNullOrEmpty(s) ? Convert.FromBase64String(s) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3044")));
        cardImage = image.Length <= 1048576 ? new CardImage(image, attribute2) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3123", (object) 1048576)));
      }
      InformationCard card = reader.IsStartElement("Issuer", "http://schemas.xmlsoap.org/ws/2005/05/identity") ? this.CreateCard(reader.ReadElementString("Issuer", "http://schemas.xmlsoap.org/ws/2005/05/identity"), signingCredentials) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3128", (object) "Issuer", (object) "http://schemas.xmlsoap.org/ws/2005/05/identity")));
      card.InformationCardReference = new InformationCardReference(str1, cardVersion);
      card.CardName = str2;
      card.CardImage = cardImage;
      card.Language = attribute1;
      card.TimeIssued = reader.IsStartElement("TimeIssued", "http://schemas.xmlsoap.org/ws/2005/05/identity") ? new DateTime?(XmlConvert.ToDateTime(reader.ReadElementString("TimeIssued", "http://schemas.xmlsoap.org/ws/2005/05/identity"), XmlDateTimeSerializationMode.Utc)) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3128", (object) "TimeIssued", (object) "http://schemas.xmlsoap.org/ws/2005/05/identity")));
      if (reader.IsStartElement("TimeExpires", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
      {
        card.TimeExpires = new DateTime?(XmlConvert.ToDateTime(reader.ReadElementString("TimeExpires", "http://schemas.xmlsoap.org/ws/2005/05/identity"), XmlDateTimeSerializationMode.Utc));
        if (card.TimeIssued.Value.CompareTo(card.TimeExpires.Value) > 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3125")));
      }
      TokenServiceCollection serviceCollection = this.ReadTokenServiceList(reader);
      if (serviceCollection != null)
      {
        foreach (TokenService tokenService in (Collection<TokenService>) serviceCollection)
          card.TokenServiceList.Add(tokenService);
      }
      if (!reader.IsStartElement("SupportedTokenTypeList", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3128", (object) "SupportedTokenTypeList", (object) "http://schemas.xmlsoap.org/ws/2005/05/identity")));
      reader.ReadStartElement("SupportedTokenTypeList", "http://schemas.xmlsoap.org/ws/2005/05/identity");
      while (reader.IsStartElement(this._trustConstants.Elements.TokenType, this._trustConstants.NamespaceURI))
      {
        if (card.SupportedTokenTypeList.Count == 32)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3049", (object) 32)));
        string uriString = reader.ReadElementContentAsString(this._trustConstants.Elements.TokenType, this._trustConstants.NamespaceURI);
        if (!UriUtil.CanCreateValidUri(uriString, UriKind.Absolute))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3135", (object) this._trustConstants.Elements.TokenType, (object) this._trustConstants.NamespaceURI, (object) uriString)));
        card.SupportedTokenTypeList.Add(uriString);
      }
      if (card.SupportedTokenTypeList.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3047")));
      reader.ReadEndElement();
      DisplayClaimCollection displayClaimCollection = this.ReadSupportedClaimList(reader);
      if (displayClaimCollection != null)
      {
        foreach (DisplayClaim displayClaim in (Collection<DisplayClaim>) displayClaimCollection)
          card.SupportedClaimTypeList.Add(displayClaim);
      }
      if (reader.IsStartElement("RequireAppliesTo", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
      {
        bool isEmptyElement = reader.IsEmptyElement;
        string attribute2 = reader.GetAttribute("Optional", (string) null);
        card.AppliesToOption = string.IsNullOrEmpty(attribute2) ? new AppliesToOption?(AppliesToOption.Required) : new AppliesToOption?(XmlConvert.ToBoolean(attribute2) ? AppliesToOption.Optional : AppliesToOption.Required);
        reader.Read();
        int content = (int) reader.MoveToContent();
        if (!isEmptyElement)
          reader.ReadEndElement();
      }
      else
        card.AppliesToOption = new AppliesToOption?(AppliesToOption.NotAllowed);
      card.PrivacyNotice = this.ReadPrivacyNotice(reader);
      if (reader.IsStartElement("IssuerInformation", "http://schemas.xmlsoap.org/ws/2007/01/identity"))
      {
        reader.ReadStartElement();
        int num = 0;
        while (reader.IsStartElement("IssuerInformationEntry", "http://schemas.xmlsoap.org/ws/2007/01/identity"))
        {
          ++num;
          reader.ReadStartElement("IssuerInformationEntry", "http://schemas.xmlsoap.org/ws/2007/01/identity");
          string key = reader.IsStartElement("EntryName", "http://schemas.xmlsoap.org/ws/2007/01/identity") ? reader.ReadElementContentAsString("EntryName", "http://schemas.xmlsoap.org/ws/2007/01/identity") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3189", (object) "IssuerInformationEntry", (object) "EntryName")));
          if (key.Length < 1 || key.Length > (int) byte.MaxValue)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3115", (object) key, (object) 1, (object) (int) byte.MaxValue)));
          string str3 = reader.IsStartElement("EntryValue", "http://schemas.xmlsoap.org/ws/2007/01/identity") ? reader.ReadElementContentAsString("EntryValue", "http://schemas.xmlsoap.org/ws/2007/01/identity") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3189", (object) "IssuerInformationEntry", (object) "EntryValue")));
          if (str3.Length < 1 || str3.Length > (int) byte.MaxValue)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3116", (object) str3, (object) 1, (object) (int) byte.MaxValue)));
          reader.ReadEndElement();
          card.IssuerInformation.Add(new IssuerInformation(key, str3));
        }
        if (num == 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3117")));
        reader.ReadEndElement();
      }
      if (reader.IsStartElement("RequireStrongRecipientIdentity", "http://schemas.xmlsoap.org/ws/2007/01/identity"))
      {
        bool isEmptyElement = reader.IsEmptyElement;
        reader.ReadStartElement();
        card.RequireStrongRecipientIdentity = new bool?(true);
        if (!isEmptyElement)
          reader.ReadEndElement();
      }
      if (reader.IsStartElement("CardType", "http://docs.oasis-open.org/imi/ns/identity-200903"))
      {
        string uriString = reader.ReadElementContentAsString();
        card.CardType = UriUtil.CanCreateValidUri(uriString, UriKind.Absolute) ? new Uri(uriString) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3185", (object) uriString)));
      }
      if (reader.IsStartElement("IssuerName", "http://docs.oasis-open.org/imi/ns/identity-200903"))
      {
        string str3 = reader.ReadElementString();
        card.IssuerName = !string.IsNullOrEmpty(str3) && str3.Length >= 1 && str3.Length <= 64 ? str3 : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3259")));
      }
      this.ReadAdditionalElements(reader, card);
      reader.ReadEndElement();
      return card;
    }

    private EkuPolicy ReadEkuPolicy(XmlDictionaryReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      Collection<Oid> collection = new Collection<Oid>();
      if (!reader.IsStartElement("EKUPolicy", "http://docs.oasis-open.org/imi/ns/identity-200903"))
        reader.ReadStartElement("EKUPolicy", "http://docs.oasis-open.org/imi/ns/identity-200903");
      if (reader.IsEmptyElement)
      {
        reader.Skip();
        return new EkuPolicy();
      }
      reader.ReadStartElement();
      while (reader.IsStartElement())
      {
        string oid = reader.ReadElementContentAsString("OID", "http://docs.oasis-open.org/imi/ns/identity-200903");
        collection.Add(new Oid(oid));
      }
      reader.ReadEndElement();
      return new EkuPolicy((IEnumerable<Oid>) collection);
    }

    private IUserCredential ReadUserCredential(XmlDictionaryReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      IUserCredential userCredential = (IUserCredential) null;
      if (reader.IsStartElement("SelfIssuedCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
      {
        if (reader.IsEmptyElement)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3179")));
        reader.ReadStartElement();
        string ppid = reader.IsStartElement("PrivatePersonalIdentifier", "http://schemas.xmlsoap.org/ws/2005/05/identity") ? reader.ReadElementString("PrivatePersonalIdentifier", "http://schemas.xmlsoap.org/ws/2005/05/identity") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3180")));
        if (string.IsNullOrEmpty(ppid))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3129")));
        InformationCardSerializer.VerifyPPIDValidBase64String(ppid);
        if (ppid.Length > 1024)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3127", (object) ppid, (object) 1024)));
        reader.ReadEndElement();
        userCredential = (IUserCredential) new SelfIssuedCredentials(ppid);
      }
      else if (reader.IsStartElement("X509V3Credential", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
        userCredential = this.ReadX509UserCredential(reader);
      else if (reader.IsStartElement("KerberosV5Credential", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
      {
        bool isEmptyElement = reader.IsEmptyElement;
        reader.Read();
        int content = (int) reader.MoveToContent();
        if (!isEmptyElement)
          reader.ReadEndElement();
        userCredential = TokenService.DefaultUserCredential;
      }
      else if (reader.IsStartElement("UsernamePasswordCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
      {
        bool isEmptyElement = reader.IsEmptyElement;
        reader.Read();
        if (!isEmptyElement)
        {
          if (reader.IsStartElement("Username", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
          {
            string userName = reader.ReadElementContentAsString("Username", "http://schemas.xmlsoap.org/ws/2005/05/identity");
            if (string.IsNullOrEmpty(userName))
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3124")));
            userCredential = userName.Length <= (int) byte.MaxValue ? (IUserCredential) new UserNamePasswordCredential(userName) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3069", (object) userName)));
          }
          reader.ReadEndElement();
        }
        if (userCredential == null)
          userCredential = (IUserCredential) new UserNamePasswordCredential();
      }
      else
        userCredential = this.ReadUnrecognizedUserCredential(reader);
      return userCredential;
    }

    private X509Principal ReadX509Principal(XmlDictionaryReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("X509Principal", "http://docs.oasis-open.org/imi/ns/identity-200903"))
        reader.ReadStartElement("X509Principal", "http://docs.oasis-open.org/imi/ns/identity-200903");
      if (reader.IsEmptyElement)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3171", (object) reader.LocalName, (object) reader.NamespaceURI)));
      reader.ReadStartElement();
      string principalName = reader.ReadElementContentAsString("PrincipalName", "http://docs.oasis-open.org/imi/ns/identity-200903");
      EkuPolicy ekuPolicy = (EkuPolicy) null;
      if (reader.IsStartElement("EKUPolicy", "http://docs.oasis-open.org/imi/ns/identity-200903"))
        ekuPolicy = this.ReadEkuPolicy(reader);
      reader.ReadEndElement();
      return new X509Principal(principalName, ekuPolicy);
    }

    private SecurityKeyIdentifierClause ReadX509SecurityKeyIdentifierClause(
      XmlDictionaryReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      SecurityKeyIdentifierClause identifierClause = (SecurityKeyIdentifierClause) null;
      if (reader.IsStartElement("KeyIdentifier", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
      {
        string attribute1 = reader.GetAttribute("ValueType", (string) null);
        if (string.IsNullOrEmpty(attribute1) || !StringComparer.Ordinal.Equals(attribute1, "http://docs.oasis-open.org/wss/2004/xx/oasis-2004xx-wss-soap-message-security-1.1#ThumbprintSHA1") && !StringComparer.Ordinal.Equals(attribute1, "http://docs.oasis-open.org/wss/oasis-wss-soap-message-security-1.1#ThumbprintSHA1"))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3079", (object) attribute1)));
        string attribute2 = reader.GetAttribute("EncodingType", (string) null);
        byte[] thumbprint;
        if (string.IsNullOrEmpty(attribute2) || StringComparer.Ordinal.Equals(attribute2, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"))
          thumbprint = reader.ReadElementContentAsBase64();
        else if (StringComparer.Ordinal.Equals(attribute2, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary"))
          thumbprint = SoapHexBinary.Parse(reader.ReadElementContentAsString()).Value;
        else if (StringComparer.Ordinal.Equals(attribute2, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Text"))
          thumbprint = new UTF8Encoding().GetBytes(reader.ReadElementContentAsString());
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3080", (object) attribute2)));
        identifierClause = (SecurityKeyIdentifierClause) new X509ThumbprintKeyIdentifierClause(thumbprint);
      }
      else if (reader.IsStartElement("X509IssuerSerial", "http://www.w3.org/2000/09/xmldsig#"))
      {
        if (reader.IsEmptyElement)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3182")));
        reader.ReadStartElement("X509IssuerSerial", "http://www.w3.org/2000/09/xmldsig#");
        string issuerName = reader.IsStartElement("X509IssuerName", "http://www.w3.org/2000/09/xmldsig#") ? reader.ReadElementContentAsString("X509IssuerName", "http://www.w3.org/2000/09/xmldsig#") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3183")));
        if (string.IsNullOrEmpty(issuerName))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3187")));
        string issuerSerialNumber = reader.IsStartElement("X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#") ? reader.ReadElementContentAsString("X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3184")));
        if (string.IsNullOrEmpty(issuerSerialNumber))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3188")));
        reader.ReadEndElement();
        identifierClause = (SecurityKeyIdentifierClause) new X509IssuerSerialKeyIdentifierClause(issuerName, issuerSerialNumber);
      }
      else if (reader.IsStartElement("X509Certificate", "http://www.w3.org/2000/09/xmldsig#"))
        identifierClause = !reader.IsEmptyElement ? (SecurityKeyIdentifierClause) new X509RawDataKeyIdentifierClause(Convert.FromBase64String(reader.ReadElementString("X509Certificate", "http://www.w3.org/2000/09/xmldsig#"))) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3277")));
      else if (reader.IsStartElement("X509SKI", "http://www.w3.org/2000/09/xmldsig#"))
        identifierClause = !reader.IsEmptyElement ? (SecurityKeyIdentifierClause) new X509SubjectKeyIdentifierClause(Convert.FromBase64String(reader.ReadElementString("X509SKI", "http://www.w3.org/2000/09/xmldsig#"))) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3278")));
      return identifierClause != null ? identifierClause : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3186")));
    }

    private X509SubjectAndIssuer ReadX509SubjectAndIssuer(
      XmlDictionaryReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("X509SubjectAndIssuer", "http://docs.oasis-open.org/imi/ns/identity-200903"))
        reader.ReadStartElement("X509SubjectAndIssuer", "http://docs.oasis-open.org/imi/ns/identity-200903");
      if (reader.IsEmptyElement)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InformationCardException(Microsoft.IdentityModel.SR.GetString("ID3171", (object) reader.LocalName, (object) reader.NamespaceURI)));
      reader.ReadStartElement();
      string x509Subject = reader.ReadElementContentAsString("X509Subject", "http://docs.oasis-open.org/imi/ns/identity-200903");
      string x509Issuer = reader.ReadElementContentAsString("X509Issuer", "http://docs.oasis-open.org/imi/ns/identity-200903");
      EkuPolicy ekuPolicy = (EkuPolicy) null;
      if (reader.IsStartElement("EKUPolicy", "http://docs.oasis-open.org/imi/ns/identity-200903"))
        ekuPolicy = this.ReadEkuPolicy(reader);
      reader.ReadEndElement();
      return new X509SubjectAndIssuer(x509Subject, x509Issuer, ekuPolicy);
    }
  }
}
