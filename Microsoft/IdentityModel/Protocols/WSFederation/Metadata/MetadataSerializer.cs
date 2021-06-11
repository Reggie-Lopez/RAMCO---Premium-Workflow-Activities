// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.MetadataSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.Protocols.XmlSignature;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class MetadataSerializer
  {
    public const string LanguagePrefix = "xml";
    public const string LanguageLocalname = "lang";
    public const string LanguageAttribute = "xml:lang";
    public const string LanguageNamespaceUri = "http://www.w3.org/XML/1998/namespace";
    private const string _uriReference = "_metadata";
    private SecurityTokenSerializer _tokenSerializer;

    public MetadataSerializer()
      : this((SecurityTokenSerializer) new WSSecurityTokenSerializer(SecurityVersion.WSSecurity11, TrustVersion.WSTrust13, SecureConversationVersion.WSSecureConversation13, false, (SamlSerializer) null, (SecurityStateEncoder) null, (IEnumerable<System.Type>) null, WSSecurityTokenSerializer.DefaultInstance.MaximumKeyDerivationOffset, WSSecurityTokenSerializer.DefaultInstance.MaximumKeyDerivationLabelLength, WSSecurityTokenSerializer.DefaultInstance.MaximumKeyDerivationNonceLength))
    {
    }

    public MetadataSerializer(SecurityTokenSerializer tokenSerializer) => this._tokenSerializer = tokenSerializer != null ? tokenSerializer : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenSerializer));

    protected virtual ApplicationServiceDescriptor CreateApplicationServiceInstance() => new ApplicationServiceDescriptor();

    protected virtual ContactPerson CreateContactPersonInstance() => new ContactPerson();

    protected virtual ProtocolEndpoint CreateProtocolEndpointInstance() => new ProtocolEndpoint();

    protected virtual EntitiesDescriptor CreateEntitiesDescriptorInstance() => new EntitiesDescriptor();

    protected virtual EntityDescriptor CreateEntityDescriptorInstance() => new EntityDescriptor();

    protected virtual IdentityProviderSingleSignOnDescriptor CreateIdentityProviderSingleSignOnDescriptorInstance() => new IdentityProviderSingleSignOnDescriptor();

    protected virtual IndexedProtocolEndpoint CreateIndexedProtocolEndpointInstance() => new IndexedProtocolEndpoint();

    protected virtual KeyDescriptor CreateKeyDescriptorInstance() => new KeyDescriptor();

    protected virtual LocalizedName CreateLocalizedNameInstance() => new LocalizedName();

    protected virtual LocalizedUri CreateLocalizedUriInstance() => new LocalizedUri();

    protected virtual Organization CreateOrganizationInstance() => new Organization();

    protected virtual SecurityTokenServiceDescriptor CreateSecurityTokenServiceDescriptorInstance() => new SecurityTokenServiceDescriptor();

    protected virtual ServiceProviderSingleSignOnDescriptor CreateServiceProviderSingleSignOnDescriptorInstance() => new ServiceProviderSingleSignOnDescriptor();

    protected virtual AddressingVersion GetAddressingVersion(string namespaceUri)
    {
      if (string.IsNullOrEmpty(namespaceUri))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (namespaceUri));
      if (StringComparer.Ordinal.Equals(namespaceUri, "http://www.w3.org/2005/08/addressing"))
        return AddressingVersion.WSAddressing10;
      return StringComparer.Ordinal.Equals(namespaceUri, "http://schemas.xmlsoap.org/ws/2004/08/addressing") ? AddressingVersion.WSAddressingAugust2004 : (AddressingVersion) null;
    }

    private static ContactType GetContactPersonType(string conactType, out bool found)
    {
      if (conactType == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (conactType));
      found = true;
      if (StringComparer.Ordinal.Equals(conactType, "unspecified"))
        return ContactType.Unspecified;
      if (StringComparer.Ordinal.Equals(conactType, "administrative"))
        return ContactType.Administrative;
      if (StringComparer.Ordinal.Equals(conactType, "billing"))
        return ContactType.Billing;
      if (StringComparer.Ordinal.Equals(conactType, "other"))
        return ContactType.Other;
      if (StringComparer.Ordinal.Equals(conactType, "support"))
        return ContactType.Support;
      if (StringComparer.Ordinal.Equals(conactType, "technical"))
        return ContactType.Technical;
      found = false;
      return ContactType.Unspecified;
    }

    private static KeyType GetKeyDescriptorType(string keyType)
    {
      if (keyType == null)
        return KeyType.Unspecified;
      if (StringComparer.Ordinal.Equals(keyType, "encryption"))
        return KeyType.Encryption;
      if (StringComparer.Ordinal.Equals(keyType, "signing"))
        return KeyType.Signing;
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "use", (object) keyType)));
    }

    protected virtual ApplicationServiceDescriptor ReadApplicationServiceDescriptor(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      ApplicationServiceDescriptor applicationServiceInstance = this.CreateApplicationServiceInstance();
      this.ReadWebServiceDescriptorAttributes(reader, (WebServiceDescriptor) applicationServiceInstance);
      this.ReadCustomAttributes<ApplicationServiceDescriptor>(reader, applicationServiceInstance);
      bool isEmptyElement1 = reader.IsEmptyElement;
      reader.ReadStartElement();
      if (!isEmptyElement1)
      {
        while (reader.IsStartElement())
        {
          if (reader.IsStartElement("ApplicationServiceEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706"))
          {
            bool isEmptyElement2 = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement2 && reader.IsStartElement())
            {
              EndpointAddress endpointAddress = EndpointAddress.ReadFrom(this.GetAddressingVersion(reader.NamespaceURI), reader);
              applicationServiceInstance.Endpoints.Add(endpointAddress);
              reader.ReadEndElement();
            }
          }
          else if (reader.IsStartElement("PassiveRequestorEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706"))
          {
            bool isEmptyElement2 = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement2 && reader.IsStartElement())
            {
              EndpointAddress endpointAddress = EndpointAddress.ReadFrom(this.GetAddressingVersion(reader.NamespaceURI), reader);
              applicationServiceInstance.PassiveRequestorEndpoints.Add(endpointAddress);
              reader.ReadEndElement();
            }
          }
          else if (!this.ReadWebServiceDescriptorElement(reader, (WebServiceDescriptor) applicationServiceInstance) && !this.ReadCustomElement<ApplicationServiceDescriptor>(reader, applicationServiceInstance))
            reader.Skip();
        }
        reader.ReadEndElement();
      }
      return applicationServiceInstance;
    }

    protected virtual ContactPerson ReadContactPerson(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      ContactPerson contactPersonInstance = this.CreateContactPersonInstance();
      string attribute = reader.GetAttribute("contactType", (string) null);
      bool found = false;
      contactPersonInstance.Type = MetadataSerializer.GetContactPersonType(attribute, out found);
      if (!found)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3201", (object) typeof (ContactType), (object) attribute)));
      this.ReadCustomAttributes<ContactPerson>(reader, contactPersonInstance);
      bool isEmptyElement = reader.IsEmptyElement;
      reader.ReadStartElement();
      if (!isEmptyElement)
      {
        while (reader.IsStartElement())
        {
          if (reader.IsStartElement("Company", "urn:oasis:names:tc:SAML:2.0:metadata"))
            contactPersonInstance.Company = reader.ReadElementContentAsString("Company", "urn:oasis:names:tc:SAML:2.0:metadata");
          else if (reader.IsStartElement("GivenName", "urn:oasis:names:tc:SAML:2.0:metadata"))
            contactPersonInstance.GivenName = reader.ReadElementContentAsString("GivenName", "urn:oasis:names:tc:SAML:2.0:metadata");
          else if (reader.IsStartElement("SurName", "urn:oasis:names:tc:SAML:2.0:metadata"))
            contactPersonInstance.Surname = reader.ReadElementContentAsString("SurName", "urn:oasis:names:tc:SAML:2.0:metadata");
          else if (reader.IsStartElement("EmailAddress", "urn:oasis:names:tc:SAML:2.0:metadata"))
          {
            string str = reader.ReadElementContentAsString("EmailAddress", "urn:oasis:names:tc:SAML:2.0:metadata");
            if (!string.IsNullOrEmpty(str))
              contactPersonInstance.EmailAddresses.Add(str);
          }
          else if (reader.IsStartElement("TelephoneNumber", "urn:oasis:names:tc:SAML:2.0:metadata"))
          {
            string str = reader.ReadElementContentAsString("TelephoneNumber", "urn:oasis:names:tc:SAML:2.0:metadata");
            if (!string.IsNullOrEmpty(str))
              contactPersonInstance.TelephoneNumbers.Add(str);
          }
          else if (!this.ReadCustomElement<ContactPerson>(reader, contactPersonInstance))
            reader.Skip();
        }
        reader.ReadEndElement();
      }
      return contactPersonInstance;
    }

    protected virtual void ReadCustomAttributes<T>(XmlReader reader, T target)
    {
    }

    protected virtual bool ReadCustomElement<T>(XmlReader reader, T target) => false;

    protected virtual void ReadCustomRoleDescriptor(
      string xsiType,
      XmlReader reader,
      EntityDescriptor entityDescriptor)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, Microsoft.IdentityModel.SR.GetString("ID3274", (object) xsiType));
      reader.Skip();
    }

    protected virtual DisplayClaim ReadDisplayClaim(XmlReader reader)
    {
      string str = reader != null ? reader.GetAttribute("Uri", (string) null) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      DisplayClaim displayClaim = UriUtil.CanCreateValidUri(str, UriKind.Absolute) ? new DisplayClaim(str) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "ClaimType", (object) str)));
      bool flag = true;
      string attribute = reader.GetAttribute("Optional");
      if (!string.IsNullOrEmpty(attribute))
      {
        try
        {
          flag = XmlConvert.ToBoolean(attribute.ToLowerInvariant());
        }
        catch (FormatException ex)
        {
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "Optional", (object) attribute)));
        }
      }
      displayClaim.Optional = flag;
      bool isEmptyElement = reader.IsEmptyElement;
      reader.ReadStartElement();
      if (!isEmptyElement)
      {
        while (reader.IsStartElement())
        {
          if (reader.IsStartElement("DisplayName", "http://docs.oasis-open.org/wsfed/authorization/200706"))
            displayClaim.DisplayTag = reader.ReadElementContentAsString("DisplayName", "http://docs.oasis-open.org/wsfed/authorization/200706");
          else if (reader.IsStartElement("Description", "http://docs.oasis-open.org/wsfed/authorization/200706"))
            displayClaim.Description = reader.ReadElementContentAsString("Description", "http://docs.oasis-open.org/wsfed/authorization/200706");
          else
            reader.Skip();
        }
        reader.ReadEndElement();
      }
      return displayClaim;
    }

    protected virtual EntitiesDescriptor ReadEntitiesDescriptor(
      XmlReader reader,
      SecurityTokenResolver tokenResolver)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      EntitiesDescriptor descriptorInstance = this.CreateEntitiesDescriptorInstance();
      EnvelopedSignatureReader envelopedSignatureReader = new EnvelopedSignatureReader(reader, this.SecurityTokenSerializer, tokenResolver, false, false, true);
      string attribute = envelopedSignatureReader.GetAttribute("Name", (string) null);
      if (!string.IsNullOrEmpty(attribute))
        descriptorInstance.Name = attribute;
      this.ReadCustomAttributes<EntitiesDescriptor>((XmlReader) envelopedSignatureReader, descriptorInstance);
      bool isEmptyElement = envelopedSignatureReader.IsEmptyElement;
      envelopedSignatureReader.ReadStartElement();
      if (!isEmptyElement)
      {
        while (envelopedSignatureReader.IsStartElement())
        {
          if (envelopedSignatureReader.IsStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
            descriptorInstance.ChildEntities.Add(this.ReadEntityDescriptor((XmlReader) envelopedSignatureReader, tokenResolver));
          else if (envelopedSignatureReader.IsStartElement("EntitiesDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
            descriptorInstance.ChildEntityGroups.Add(this.ReadEntitiesDescriptor((XmlReader) envelopedSignatureReader, tokenResolver));
          else if (!envelopedSignatureReader.TryReadSignature() && !this.ReadCustomElement<EntitiesDescriptor>((XmlReader) envelopedSignatureReader, descriptorInstance))
            envelopedSignatureReader.Skip();
        }
        envelopedSignatureReader.ReadEndElement();
      }
      descriptorInstance.SigningCredentials = envelopedSignatureReader.SigningCredentials;
      if (descriptorInstance.ChildEntityGroups.Count == 0 && descriptorInstance.ChildEntities.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3200", (object) "EntityDescriptor")));
      foreach (EntityDescriptor childEntity in (IEnumerable<EntityDescriptor>) descriptorInstance.ChildEntities)
      {
        if (!string.IsNullOrEmpty(childEntity.FederationId) && !StringComparer.Ordinal.Equals(childEntity.FederationId, descriptorInstance.Name))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "FederationID", (object) childEntity.FederationId)));
      }
      return descriptorInstance;
    }

    protected virtual EntityDescriptor ReadEntityDescriptor(
      XmlReader inputReader,
      SecurityTokenResolver tokenResolver)
    {
      if (inputReader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (inputReader));
      EntityDescriptor descriptorInstance = this.CreateEntityDescriptorInstance();
      EnvelopedSignatureReader envelopedSignatureReader = new EnvelopedSignatureReader(inputReader, this.SecurityTokenSerializer, tokenResolver, false, false, true);
      string attribute1 = envelopedSignatureReader.GetAttribute("entityID", (string) null);
      if (!string.IsNullOrEmpty(attribute1))
        descriptorInstance.EntityId = new EntityId(attribute1);
      string attribute2 = envelopedSignatureReader.GetAttribute("FederationID", "http://docs.oasis-open.org/wsfed/federation/200706");
      if (!string.IsNullOrEmpty(attribute2))
        descriptorInstance.FederationId = attribute2;
      this.ReadCustomAttributes<EntityDescriptor>((XmlReader) envelopedSignatureReader, descriptorInstance);
      bool isEmptyElement = envelopedSignatureReader.IsEmptyElement;
      envelopedSignatureReader.ReadStartElement();
      if (!isEmptyElement)
      {
        while (envelopedSignatureReader.IsStartElement())
        {
          if (envelopedSignatureReader.IsStartElement("SPSSODescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
            descriptorInstance.RoleDescriptors.Add((RoleDescriptor) this.ReadServiceProviderSingleSignOnDescriptor((XmlReader) envelopedSignatureReader));
          else if (envelopedSignatureReader.IsStartElement("IDPSSODescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
            descriptorInstance.RoleDescriptors.Add((RoleDescriptor) this.ReadIdentityProviderSingleSignOnDescriptor((XmlReader) envelopedSignatureReader));
          else if (envelopedSignatureReader.IsStartElement("RoleDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
          {
            string attribute3 = envelopedSignatureReader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
            int length = !string.IsNullOrEmpty(attribute3) ? attribute3.IndexOf(":", 0, StringComparison.Ordinal) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID0001", (object) "xsi:type", (object) "RoleDescriptor")));
            string prefix = length >= 0 ? attribute3.Substring(0, length) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3207", (object) "xsi:type", (object) "RoleDescriptor", (object) attribute3)));
            string x = envelopedSignatureReader.LookupNamespace(prefix);
            if (string.IsNullOrEmpty(x))
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) prefix, (object) x)));
            if (!StringComparer.Ordinal.Equals(x, "http://docs.oasis-open.org/wsfed/federation/200706"))
              this.ReadCustomRoleDescriptor(attribute3, (XmlReader) envelopedSignatureReader, descriptorInstance);
            else if (StringComparer.Ordinal.Equals(attribute3, prefix + ":ApplicationServiceType"))
              descriptorInstance.RoleDescriptors.Add((RoleDescriptor) this.ReadApplicationServiceDescriptor((XmlReader) envelopedSignatureReader));
            else if (StringComparer.Ordinal.Equals(attribute3, prefix + ":SecurityTokenServiceType"))
              descriptorInstance.RoleDescriptors.Add((RoleDescriptor) this.ReadSecurityTokenServiceDescriptor((XmlReader) envelopedSignatureReader));
            else
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3207", (object) "xsi:type", (object) "RoleDescriptor", (object) attribute3)));
          }
          else if (envelopedSignatureReader.IsStartElement("Organization", "urn:oasis:names:tc:SAML:2.0:metadata"))
            descriptorInstance.Organization = this.ReadOrganization((XmlReader) envelopedSignatureReader);
          else if (envelopedSignatureReader.IsStartElement("ContactPerson", "urn:oasis:names:tc:SAML:2.0:metadata"))
            descriptorInstance.Contacts.Add(this.ReadContactPerson((XmlReader) envelopedSignatureReader));
          else if (!envelopedSignatureReader.TryReadSignature() && !this.ReadCustomElement<EntityDescriptor>((XmlReader) envelopedSignatureReader, descriptorInstance))
            envelopedSignatureReader.Skip();
        }
        envelopedSignatureReader.ReadEndElement();
      }
      descriptorInstance.SigningCredentials = envelopedSignatureReader.SigningCredentials;
      return descriptorInstance;
    }

    protected virtual IdentityProviderSingleSignOnDescriptor ReadIdentityProviderSingleSignOnDescriptor(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      IdentityProviderSingleSignOnDescriptor descriptorInstance = this.CreateIdentityProviderSingleSignOnDescriptorInstance();
      this.ReadSingleSignOnDescriptorAttributes(reader, (SingleSignOnDescriptor) descriptorInstance);
      this.ReadCustomAttributes<IdentityProviderSingleSignOnDescriptor>(reader, descriptorInstance);
      string attribute = reader.GetAttribute("WantAuthnRequestsSigned");
      if (!string.IsNullOrEmpty(attribute))
      {
        try
        {
          descriptorInstance.WantAuthenticationRequestsSigned = XmlConvert.ToBoolean(attribute.ToLowerInvariant());
        }
        catch (FormatException ex)
        {
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "WantAuthnRequestsSigned", (object) attribute)));
        }
      }
      bool isEmptyElement = reader.IsEmptyElement;
      reader.ReadStartElement();
      if (!isEmptyElement)
      {
        while (reader.IsStartElement())
        {
          if (reader.IsStartElement("SingleSignOnService", "urn:oasis:names:tc:SAML:2.0:metadata"))
          {
            ProtocolEndpoint protocolEndpoint = this.ReadProtocolEndpoint(reader);
            descriptorInstance.SingleSignOnServices.Add(protocolEndpoint);
          }
          else if (reader.IsStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion"))
            descriptorInstance.SupportedAttributes.Add(this.ReadAttribute(reader));
          else if (!this.ReadSingleSignOnDescriptorElement(reader, (SingleSignOnDescriptor) descriptorInstance) && !this.ReadCustomElement<IdentityProviderSingleSignOnDescriptor>(reader, descriptorInstance))
            reader.Skip();
        }
        reader.ReadEndElement();
      }
      return descriptorInstance;
    }

    protected virtual IndexedProtocolEndpoint ReadIndexedProtocolEndpoint(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      IndexedProtocolEndpoint endpointInstance = this.CreateIndexedProtocolEndpointInstance();
      string attribute1 = reader.GetAttribute("Binding", (string) null);
      Uri result1;
      if (!UriUtil.TryCreateValidUri(attribute1, UriKind.RelativeOrAbsolute, out result1))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "Binding", (object) attribute1)));
      endpointInstance.Binding = result1;
      string attribute2 = reader.GetAttribute("Location", (string) null);
      Uri result2;
      if (!UriUtil.TryCreateValidUri(attribute2, UriKind.RelativeOrAbsolute, out result2))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "Location", (object) attribute2)));
      endpointInstance.Location = result2;
      string attribute3 = reader.GetAttribute("index", (string) null);
      int result3;
      if (!int.TryParse(attribute3, out result3))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "index", (object) attribute3)));
      endpointInstance.Index = result3;
      string attribute4 = reader.GetAttribute("ResponseLocation", (string) null);
      if (!string.IsNullOrEmpty(attribute4))
      {
        Uri result4;
        if (!UriUtil.TryCreateValidUri(attribute4, UriKind.RelativeOrAbsolute, out result4))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "ResponseLocation", (object) attribute4)));
        endpointInstance.ResponseLocation = result4;
      }
      string attribute5 = reader.GetAttribute("isDefault", (string) null);
      if (!string.IsNullOrEmpty(attribute5))
      {
        try
        {
          endpointInstance.IsDefault = new bool?(XmlConvert.ToBoolean(attribute5.ToLowerInvariant()));
        }
        catch (FormatException ex)
        {
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "isDefault", (object) attribute5)));
        }
      }
      this.ReadCustomAttributes<IndexedProtocolEndpoint>(reader, endpointInstance);
      bool isEmptyElement = reader.IsEmptyElement;
      reader.ReadStartElement();
      if (!isEmptyElement)
      {
        while (reader.IsStartElement())
        {
          if (!this.ReadCustomElement<IndexedProtocolEndpoint>(reader, endpointInstance))
            reader.Skip();
        }
        reader.ReadEndElement();
      }
      return endpointInstance;
    }

    protected virtual KeyDescriptor ReadKeyDescriptor(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      KeyDescriptor descriptorInstance = this.CreateKeyDescriptorInstance();
      string attribute1 = reader.GetAttribute("use", (string) null);
      if (!string.IsNullOrEmpty(attribute1))
        descriptorInstance.Use = MetadataSerializer.GetKeyDescriptorType(attribute1);
      this.ReadCustomAttributes<KeyDescriptor>(reader, descriptorInstance);
      bool isEmptyElement1 = reader.IsEmptyElement;
      reader.ReadStartElement();
      if (!isEmptyElement1)
      {
        while (reader.IsStartElement())
        {
          if (reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
            descriptorInstance.KeyInfo = this.SecurityTokenSerializer.ReadKeyIdentifier(reader);
          else if (reader.IsStartElement("EncryptionMethod", "urn:oasis:names:tc:SAML:2.0:metadata"))
          {
            string attribute2 = reader.GetAttribute("Algorithm");
            if (!string.IsNullOrEmpty(attribute2) && UriUtil.CanCreateValidUri(attribute2, UriKind.Absolute))
              descriptorInstance.EncryptionMethods.Add(new EncryptionMethod(new Uri(attribute2)));
            bool isEmptyElement2 = reader.IsEmptyElement;
            reader.ReadStartElement("EncryptionMethod", "urn:oasis:names:tc:SAML:2.0:metadata");
            if (!isEmptyElement2)
            {
              while (reader.IsStartElement())
              {
                if (!this.ReadCustomElement<KeyDescriptor>(reader, descriptorInstance))
                  reader.Skip();
              }
              reader.ReadEndElement();
            }
          }
          else if (!this.ReadCustomElement<KeyDescriptor>(reader, descriptorInstance))
            reader.Skip();
        }
        reader.ReadEndElement();
      }
      return descriptorInstance.KeyInfo != null ? descriptorInstance : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3200", (object) "KeyInfo")));
    }

    protected virtual LocalizedName ReadLocalizedName(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      LocalizedName localizedNameInstance = this.CreateLocalizedNameInstance();
      string attribute = reader.GetAttribute("lang", "http://www.w3.org/XML/1998/namespace");
      try
      {
        localizedNameInstance.Language = CultureInfo.GetCultureInfo(attribute);
      }
      catch (ArgumentNullException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "lang", (object) "null")));
      }
      catch (ArgumentException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "lang", (object) attribute)));
      }
      this.ReadCustomAttributes<LocalizedName>(reader, localizedNameInstance);
      bool isEmptyElement = reader.IsEmptyElement;
      string name = reader.Name;
      reader.ReadStartElement();
      if (!isEmptyElement)
      {
        localizedNameInstance.Name = reader.ReadContentAsString();
        while (reader.IsStartElement())
        {
          if (!this.ReadCustomElement<LocalizedName>(reader, localizedNameInstance))
            reader.Skip();
        }
        reader.ReadEndElement();
      }
      return !string.IsNullOrEmpty(localizedNameInstance.Name) ? localizedNameInstance : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3200", (object) name)));
    }

    protected virtual LocalizedUri ReadLocalizedUri(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      LocalizedUri localizedUriInstance = this.CreateLocalizedUriInstance();
      string attribute = reader.GetAttribute("lang", "http://www.w3.org/XML/1998/namespace");
      try
      {
        localizedUriInstance.Language = CultureInfo.GetCultureInfo(attribute);
      }
      catch (ArgumentNullException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "lang", (object) "null")));
      }
      catch (ArgumentException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "lang", (object) attribute)));
      }
      this.ReadCustomAttributes<LocalizedUri>(reader, localizedUriInstance);
      bool isEmptyElement = reader.IsEmptyElement;
      string name = reader.Name;
      reader.ReadStartElement();
      if (!isEmptyElement)
      {
        string uriString = reader.ReadContentAsString();
        Uri result;
        if (!UriUtil.TryCreateValidUri(uriString, UriKind.RelativeOrAbsolute, out result))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) name, (object) uriString)));
        localizedUriInstance.Uri = result;
        while (reader.IsStartElement())
        {
          if (!this.ReadCustomElement<LocalizedUri>(reader, localizedUriInstance))
            reader.Skip();
        }
        reader.ReadEndElement();
      }
      return !(localizedUriInstance.Uri == (Uri) null) ? localizedUriInstance : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3200", (object) name)));
    }

    public MetadataBase ReadMetadata(Stream stream) => stream != null ? this.ReadMetadata((XmlReader) XmlDictionaryReader.CreateTextReader(stream, XmlDictionaryReaderQuotas.Max)) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (stream));

    public MetadataBase ReadMetadata(XmlReader reader) => this.ReadMetadata(reader, Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance);

    public MetadataBase ReadMetadata(
      XmlReader reader,
      SecurityTokenResolver tokenResolver)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (tokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenResolver));
      if (!(reader is XmlDictionaryReader))
        reader = (XmlReader) XmlDictionaryReader.CreateDictionaryReader(reader);
      return this.ReadMetadataCore(reader, tokenResolver);
    }

    protected virtual MetadataBase ReadMetadataCore(
      XmlReader reader,
      SecurityTokenResolver tokenResolver)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (tokenResolver == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenResolver));
      if (reader.IsStartElement("EntitiesDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
        return (MetadataBase) this.ReadEntitiesDescriptor(reader, tokenResolver);
      return reader.IsStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata") ? (MetadataBase) this.ReadEntityDescriptor(reader, tokenResolver) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3260")));
    }

    protected virtual Organization ReadOrganization(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      Organization organizationInstance = this.CreateOrganizationInstance();
      this.ReadCustomAttributes<Organization>(reader, organizationInstance);
      bool isEmptyElement = reader.IsEmptyElement;
      reader.ReadStartElement();
      if (!isEmptyElement)
      {
        while (reader.IsStartElement())
        {
          if (reader.IsStartElement("OrganizationName", "urn:oasis:names:tc:SAML:2.0:metadata"))
            organizationInstance.Names.Add(this.ReadLocalizedName(reader));
          else if (reader.IsStartElement("OrganizationDisplayName", "urn:oasis:names:tc:SAML:2.0:metadata"))
            organizationInstance.DisplayNames.Add(this.ReadLocalizedName(reader));
          else if (reader.IsStartElement("OrganizationURL", "urn:oasis:names:tc:SAML:2.0:metadata"))
            organizationInstance.Urls.Add(this.ReadLocalizedUri(reader));
          else if (!this.ReadCustomElement<Organization>(reader, organizationInstance))
            reader.Skip();
        }
        reader.ReadEndElement();
      }
      return organizationInstance;
    }

    protected virtual ProtocolEndpoint ReadProtocolEndpoint(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      ProtocolEndpoint endpointInstance = this.CreateProtocolEndpointInstance();
      string attribute1 = reader.GetAttribute("Binding", (string) null);
      Uri result1;
      if (!UriUtil.TryCreateValidUri(attribute1, UriKind.RelativeOrAbsolute, out result1))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "Binding", (object) attribute1)));
      endpointInstance.Binding = result1;
      string attribute2 = reader.GetAttribute("Location", (string) null);
      Uri result2;
      if (!UriUtil.TryCreateValidUri(attribute2, UriKind.RelativeOrAbsolute, out result2))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "Location", (object) attribute2)));
      endpointInstance.Location = result2;
      string attribute3 = reader.GetAttribute("ResponseLocation", (string) null);
      if (!string.IsNullOrEmpty(attribute3))
      {
        Uri result3;
        if (!UriUtil.TryCreateValidUri(attribute3, UriKind.RelativeOrAbsolute, out result3))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "ResponseLocation", (object) attribute3)));
        endpointInstance.ResponseLocation = result3;
      }
      this.ReadCustomAttributes<ProtocolEndpoint>(reader, endpointInstance);
      bool isEmptyElement = reader.IsEmptyElement;
      reader.ReadStartElement();
      if (!isEmptyElement)
      {
        while (reader.IsStartElement())
        {
          if (!this.ReadCustomElement<ProtocolEndpoint>(reader, endpointInstance))
            reader.Skip();
        }
        reader.ReadEndElement();
      }
      return endpointInstance;
    }

    protected virtual void ReadRoleDescriptorAttributes(
      XmlReader reader,
      RoleDescriptor roleDescriptor)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (roleDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (roleDescriptor));
      if (roleDescriptor.ProtocolsSupported == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.ProtocolsSupported");
      string attribute1 = reader.GetAttribute("validUntil", (string) null);
      if (!string.IsNullOrEmpty(attribute1))
      {
        DateTime result;
        if (!DateTime.TryParse(attribute1, out result))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "validUntil", (object) attribute1)));
        roleDescriptor.ValidUntil = result;
      }
      string attribute2 = reader.GetAttribute("errorURL", (string) null);
      if (!string.IsNullOrEmpty(attribute2))
      {
        Uri result;
        if (!UriUtil.TryCreateValidUri(attribute2, UriKind.RelativeOrAbsolute, out result))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "errorURL", (object) attribute2)));
        roleDescriptor.ErrorUrl = result;
      }
      string attribute3 = reader.GetAttribute("protocolSupportEnumeration", (string) null);
      string str1 = !string.IsNullOrEmpty(attribute3) ? attribute3 : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "protocolSupportEnumeration", (object) attribute3)));
      char[] chArray = new char[1]{ ' ' };
      foreach (string str2 in str1.Split(chArray))
      {
        string uriString = str2.Trim();
        if (!string.IsNullOrEmpty(uriString))
          roleDescriptor.ProtocolsSupported.Add(new Uri(uriString));
      }
      this.ReadCustomAttributes<RoleDescriptor>(reader, roleDescriptor);
    }

    protected virtual bool ReadRoleDescriptorElement(
      XmlReader reader,
      RoleDescriptor roleDescriptor)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (roleDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (roleDescriptor));
      if (roleDescriptor.Contacts == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.Contacts");
      if (roleDescriptor.Keys == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.Keys");
      if (reader.IsStartElement("Organization", "urn:oasis:names:tc:SAML:2.0:metadata"))
      {
        roleDescriptor.Organization = this.ReadOrganization(reader);
        return true;
      }
      if (reader.IsStartElement("KeyDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
      {
        roleDescriptor.Keys.Add(this.ReadKeyDescriptor(reader));
        return true;
      }
      if (!reader.IsStartElement("ContactPerson", "urn:oasis:names:tc:SAML:2.0:metadata"))
        return this.ReadCustomElement<RoleDescriptor>(reader, roleDescriptor);
      roleDescriptor.Contacts.Add(this.ReadContactPerson(reader));
      return true;
    }

    protected virtual SecurityTokenServiceDescriptor ReadSecurityTokenServiceDescriptor(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      SecurityTokenServiceDescriptor descriptorInstance = this.CreateSecurityTokenServiceDescriptorInstance();
      this.ReadWebServiceDescriptorAttributes(reader, (WebServiceDescriptor) descriptorInstance);
      this.ReadCustomAttributes<SecurityTokenServiceDescriptor>(reader, descriptorInstance);
      bool isEmptyElement1 = reader.IsEmptyElement;
      reader.ReadStartElement();
      if (!isEmptyElement1)
      {
        while (reader.IsStartElement())
        {
          if (reader.IsStartElement("SecurityTokenServiceEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706"))
          {
            bool isEmptyElement2 = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement2 && reader.IsStartElement())
            {
              EndpointAddress endpointAddress = EndpointAddress.ReadFrom(this.GetAddressingVersion(reader.NamespaceURI), reader);
              descriptorInstance.SecurityTokenServiceEndpoints.Add(endpointAddress);
              reader.ReadEndElement();
            }
          }
          else if (reader.IsStartElement("PassiveRequestorEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706"))
          {
            bool isEmptyElement2 = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement2 && reader.IsStartElement())
            {
              EndpointAddress endpointAddress = EndpointAddress.ReadFrom(this.GetAddressingVersion(reader.NamespaceURI), reader);
              descriptorInstance.PassiveRequestorEndpoints.Add(endpointAddress);
              reader.ReadEndElement();
            }
          }
          else if (!this.ReadWebServiceDescriptorElement(reader, (WebServiceDescriptor) descriptorInstance) && !this.ReadCustomElement<SecurityTokenServiceDescriptor>(reader, descriptorInstance))
            reader.Skip();
        }
        reader.ReadEndElement();
      }
      return descriptorInstance;
    }

    protected virtual ServiceProviderSingleSignOnDescriptor ReadServiceProviderSingleSignOnDescriptor(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      ServiceProviderSingleSignOnDescriptor descriptorInstance = this.CreateServiceProviderSingleSignOnDescriptorInstance();
      string attribute1 = reader.GetAttribute("AuthnRequestsSigned");
      if (!string.IsNullOrEmpty(attribute1))
      {
        try
        {
          descriptorInstance.AuthenticationRequestsSigned = XmlConvert.ToBoolean(attribute1.ToLowerInvariant());
        }
        catch (FormatException ex)
        {
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "AuthnRequestsSigned", (object) attribute1)));
        }
      }
      string attribute2 = reader.GetAttribute("WantAssertionsSigned");
      if (!string.IsNullOrEmpty(attribute2))
      {
        try
        {
          descriptorInstance.WantAssertionsSigned = XmlConvert.ToBoolean(attribute2.ToLowerInvariant());
        }
        catch (FormatException ex)
        {
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "WantAssertionsSigned", (object) attribute2)));
        }
      }
      this.ReadSingleSignOnDescriptorAttributes(reader, (SingleSignOnDescriptor) descriptorInstance);
      this.ReadCustomAttributes<ServiceProviderSingleSignOnDescriptor>(reader, descriptorInstance);
      bool isEmptyElement = reader.IsEmptyElement;
      reader.ReadStartElement();
      if (!isEmptyElement)
      {
        while (reader.IsStartElement())
        {
          if (reader.IsStartElement("AssertionConsumerService", "urn:oasis:names:tc:SAML:2.0:metadata"))
          {
            IndexedProtocolEndpoint protocolEndpoint = this.ReadIndexedProtocolEndpoint(reader);
            descriptorInstance.AssertionConsumerService.Add(protocolEndpoint.Index, protocolEndpoint);
          }
          else if (!this.ReadSingleSignOnDescriptorElement(reader, (SingleSignOnDescriptor) descriptorInstance) && !this.ReadCustomElement<ServiceProviderSingleSignOnDescriptor>(reader, descriptorInstance))
            reader.Skip();
        }
        reader.ReadEndElement();
      }
      return descriptorInstance;
    }

    protected virtual void ReadSingleSignOnDescriptorAttributes(
      XmlReader reader,
      SingleSignOnDescriptor roleDescriptor)
    {
      this.ReadRoleDescriptorAttributes(reader, (RoleDescriptor) roleDescriptor);
      this.ReadCustomAttributes<SingleSignOnDescriptor>(reader, roleDescriptor);
    }

    protected virtual bool ReadSingleSignOnDescriptorElement(
      XmlReader reader,
      SingleSignOnDescriptor ssoDescriptor)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (ssoDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (ssoDescriptor));
      if (this.ReadRoleDescriptorElement(reader, (RoleDescriptor) ssoDescriptor))
        return true;
      if (reader.IsStartElement("ArtifactResolutionService", "urn:oasis:names:tc:SAML:2.0:metadata"))
      {
        IndexedProtocolEndpoint protocolEndpoint = this.ReadIndexedProtocolEndpoint(reader);
        ssoDescriptor.ArtifactResolutionServices.Add(protocolEndpoint.Index, protocolEndpoint);
        return true;
      }
      if (reader.IsStartElement("SingleLogoutService", "urn:oasis:names:tc:SAML:2.0:metadata"))
      {
        ssoDescriptor.SingleLogoutServices.Add(this.ReadProtocolEndpoint(reader));
        return true;
      }
      if (!reader.IsStartElement("NameIDFormat", "urn:oasis:names:tc:SAML:2.0:metadata"))
        return this.ReadCustomElement<SingleSignOnDescriptor>(reader, ssoDescriptor);
      string uriString = reader.ReadElementContentAsString("NameIDFormat", "urn:oasis:names:tc:SAML:2.0:metadata");
      if (!UriUtil.CanCreateValidUri(uriString, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID0014", (object) "NameIDFormat")));
      ssoDescriptor.NameIdentifierFormats.Add(new Uri(uriString));
      return true;
    }

    protected virtual void ReadWebServiceDescriptorAttributes(
      XmlReader reader,
      WebServiceDescriptor roleDescriptor)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (roleDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (roleDescriptor));
      this.ReadRoleDescriptorAttributes(reader, (RoleDescriptor) roleDescriptor);
      string attribute1 = reader.GetAttribute("ServiceDisplayName", (string) null);
      if (!string.IsNullOrEmpty(attribute1))
        roleDescriptor.ServiceDisplayName = attribute1;
      string attribute2 = reader.GetAttribute("ServiceDescription", (string) null);
      if (!string.IsNullOrEmpty(attribute2))
        roleDescriptor.ServiceDescription = attribute2;
      this.ReadCustomAttributes<WebServiceDescriptor>(reader, roleDescriptor);
    }

    public virtual bool ReadWebServiceDescriptorElement(
      XmlReader reader,
      WebServiceDescriptor roleDescriptor)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (roleDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (roleDescriptor));
      if (roleDescriptor.TargetScopes == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.TargetScopes");
      if (roleDescriptor.ClaimTypesOffered == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.TargetScopes");
      if (roleDescriptor.TokenTypesOffered == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.TokenTypesOffered");
      if (this.ReadRoleDescriptorElement(reader, (RoleDescriptor) roleDescriptor))
        return true;
      if (reader.IsStartElement("TargetScopes", "http://docs.oasis-open.org/wsfed/federation/200706"))
      {
        bool isEmptyElement = reader.IsEmptyElement;
        reader.ReadStartElement();
        if (!isEmptyElement)
        {
          while (reader.IsStartElement())
            roleDescriptor.TargetScopes.Add(EndpointAddress.ReadFrom(this.GetAddressingVersion(reader.NamespaceURI), reader));
          reader.ReadEndElement();
        }
        return true;
      }
      if (reader.IsStartElement("ClaimTypesOffered", "http://docs.oasis-open.org/wsfed/federation/200706"))
      {
        bool isEmptyElement = reader.IsEmptyElement;
        reader.ReadStartElement();
        if (!isEmptyElement)
        {
          while (reader.IsStartElement())
          {
            if (reader.IsStartElement("ClaimType", "http://docs.oasis-open.org/wsfed/authorization/200706"))
              roleDescriptor.ClaimTypesOffered.Add(this.ReadDisplayClaim(reader));
            else
              reader.Skip();
          }
          reader.ReadEndElement();
        }
        return true;
      }
      if (reader.IsStartElement("ClaimTypesRequested", "http://docs.oasis-open.org/wsfed/federation/200706"))
      {
        bool isEmptyElement = reader.IsEmptyElement;
        reader.ReadStartElement();
        if (!isEmptyElement)
        {
          while (reader.IsStartElement())
          {
            if (reader.IsStartElement("ClaimType", "http://docs.oasis-open.org/wsfed/authorization/200706"))
              roleDescriptor.ClaimTypesRequested.Add(this.ReadDisplayClaim(reader));
            else
              reader.Skip();
          }
          reader.ReadEndElement();
        }
        return true;
      }
      if (!reader.IsStartElement("TokenTypesOffered", "http://docs.oasis-open.org/wsfed/federation/200706"))
        return this.ReadCustomElement<WebServiceDescriptor>(reader, roleDescriptor);
      bool isEmptyElement1 = reader.IsEmptyElement;
      reader.ReadStartElement("TokenTypesOffered", "http://docs.oasis-open.org/wsfed/federation/200706");
      if (!isEmptyElement1)
      {
        while (reader.IsStartElement())
        {
          if (reader.IsStartElement("TokenType", "http://docs.oasis-open.org/wsfed/federation/200706"))
          {
            string attribute = reader.GetAttribute("Uri", (string) null);
            Uri result;
            if (!UriUtil.TryCreateValidUri(attribute, UriKind.Absolute, out result))
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3202", (object) "TokenType", (object) attribute)));
            roleDescriptor.TokenTypesOffered.Add(result);
            bool isEmptyElement2 = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement2)
              reader.ReadEndElement();
          }
          else
            reader.Skip();
        }
        reader.ReadEndElement();
      }
      return true;
    }

    public SecurityTokenSerializer SecurityTokenSerializer => this._tokenSerializer;

    protected virtual void WriteApplicationServiceDescriptor(
      XmlWriter writer,
      ApplicationServiceDescriptor appService)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (appService == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (appService));
      if (appService.Endpoints == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("appService.Endpoints");
      if (appService.PassiveRequestorEndpoints == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("appService.PassiveRequestorEndpoints");
      writer.WriteStartElement("RoleDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
      writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "fed:ApplicationServiceType");
      writer.WriteAttributeString("xmlns", "fed", (string) null, "http://docs.oasis-open.org/wsfed/federation/200706");
      this.WriteWebServiceDescriptorAttributes(writer, (WebServiceDescriptor) appService);
      this.WriteCustomAttributes<ApplicationServiceDescriptor>(writer, appService);
      this.WriteWebServiceDescriptorElements(writer, (WebServiceDescriptor) appService);
      foreach (EndpointAddress endpoint in (IEnumerable<EndpointAddress>) appService.Endpoints)
      {
        writer.WriteStartElement("ApplicationServiceEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706");
        endpoint.WriteTo(AddressingVersion.WSAddressing10, writer);
        writer.WriteEndElement();
      }
      foreach (EndpointAddress requestorEndpoint in (IEnumerable<EndpointAddress>) appService.PassiveRequestorEndpoints)
      {
        writer.WriteStartElement("PassiveRequestorEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706");
        requestorEndpoint.WriteTo(AddressingVersion.WSAddressing10, writer);
        writer.WriteEndElement();
      }
      this.WriteCustomElements<ApplicationServiceDescriptor>(writer, appService);
      writer.WriteEndElement();
    }

    protected virtual void WriteContactPerson(XmlWriter writer, ContactPerson contactPerson)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (contactPerson == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (contactPerson));
      if (contactPerson.EmailAddresses == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("contactPerson.EmailAddresses");
      if (contactPerson.TelephoneNumbers == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("contactPerson.TelephoneNumbers");
      writer.WriteStartElement("ContactPerson", "urn:oasis:names:tc:SAML:2.0:metadata");
      if (contactPerson.Type == ContactType.Unspecified)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "contactType")));
      writer.WriteAttributeString("contactType", (string) null, contactPerson.Type.ToString().ToLowerInvariant());
      this.WriteCustomAttributes<ContactPerson>(writer, contactPerson);
      if (!string.IsNullOrEmpty(contactPerson.Company))
        writer.WriteElementString("Company", "urn:oasis:names:tc:SAML:2.0:metadata", contactPerson.Company);
      if (!string.IsNullOrEmpty(contactPerson.GivenName))
        writer.WriteElementString("GivenName", "urn:oasis:names:tc:SAML:2.0:metadata", contactPerson.GivenName);
      if (!string.IsNullOrEmpty(contactPerson.Surname))
        writer.WriteElementString("SurName", "urn:oasis:names:tc:SAML:2.0:metadata", contactPerson.Surname);
      foreach (string emailAddress in (IEnumerable<string>) contactPerson.EmailAddresses)
        writer.WriteElementString("EmailAddress", "urn:oasis:names:tc:SAML:2.0:metadata", emailAddress);
      foreach (string telephoneNumber in (IEnumerable<string>) contactPerson.TelephoneNumbers)
        writer.WriteElementString("TelephoneNumber", "urn:oasis:names:tc:SAML:2.0:metadata", telephoneNumber);
      this.WriteCustomElements<ContactPerson>(writer, contactPerson);
      writer.WriteEndElement();
    }

    protected virtual void WriteCustomAttributes<T>(XmlWriter writer, T source)
    {
    }

    protected virtual void WriteCustomElements<T>(XmlWriter writer, T source)
    {
    }

    protected virtual void WriteProtocolEndpoint(
      XmlWriter writer,
      ProtocolEndpoint endpoint,
      XmlQualifiedName element)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (endpoint == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (endpoint));
      if (element == (XmlQualifiedName) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (element));
      writer.WriteStartElement(element.Name, element.Namespace);
      if (endpoint.Binding == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "Binding")));
      writer.WriteAttributeString("Binding", (string) null, endpoint.Binding.IsAbsoluteUri ? endpoint.Binding.AbsoluteUri : endpoint.Binding.ToString());
      if (endpoint.Location == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "Location")));
      writer.WriteAttributeString("Location", (string) null, endpoint.Location.IsAbsoluteUri ? endpoint.Location.AbsoluteUri : endpoint.Location.ToString());
      if (endpoint.ResponseLocation != (Uri) null)
        writer.WriteAttributeString("ResponseLocation", (string) null, endpoint.ResponseLocation.IsAbsoluteUri ? endpoint.ResponseLocation.AbsoluteUri : endpoint.ResponseLocation.ToString());
      this.WriteCustomAttributes<ProtocolEndpoint>(writer, endpoint);
      this.WriteCustomElements<ProtocolEndpoint>(writer, endpoint);
      writer.WriteEndElement();
    }

    protected virtual void WriteDisplayClaim(XmlWriter writer, DisplayClaim claim)
    {
      writer.WriteStartElement("auth", "ClaimType", "http://docs.oasis-open.org/wsfed/authorization/200706");
      if (string.IsNullOrEmpty(claim.ClaimType))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "ClaimType")));
      if (!UriUtil.CanCreateValidUri(claim.ClaimType, UriKind.Absolute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID0014", (object) claim.ClaimType)));
      writer.WriteAttributeString("Uri", claim.ClaimType);
      writer.WriteAttributeString("Optional", XmlConvert.ToString(claim.Optional));
      if (!string.IsNullOrEmpty(claim.DisplayTag))
        writer.WriteElementString("auth", "DisplayName", "http://docs.oasis-open.org/wsfed/authorization/200706", claim.DisplayTag);
      if (!string.IsNullOrEmpty(claim.Description))
        writer.WriteElementString("auth", "Description", "http://docs.oasis-open.org/wsfed/authorization/200706", claim.Description);
      writer.WriteEndElement();
    }

    protected virtual void WriteEntitiesDescriptor(
      XmlWriter inputWriter,
      EntitiesDescriptor entitiesDescriptor)
    {
      if (inputWriter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (inputWriter));
      if (entitiesDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (entitiesDescriptor));
      if (entitiesDescriptor.ChildEntities == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("entitiesDescriptor.ChildEntities");
      if (entitiesDescriptor.ChildEntityGroups == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("entitiesDescriptor.ChildEntityGroups");
      string referenceId = "_" + Guid.NewGuid().ToString();
      XmlWriter xmlWriter = inputWriter;
      EnvelopedSignatureWriter envelopedSignatureWriter = (EnvelopedSignatureWriter) null;
      if (entitiesDescriptor.SigningCredentials != null)
      {
        envelopedSignatureWriter = new EnvelopedSignatureWriter(inputWriter, entitiesDescriptor.SigningCredentials, referenceId, this.SecurityTokenSerializer);
        xmlWriter = (XmlWriter) envelopedSignatureWriter;
      }
      xmlWriter.WriteStartElement("EntitiesDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
      xmlWriter.WriteAttributeString("ID", (string) null, referenceId);
      if (entitiesDescriptor.ChildEntities.Count == 0 && entitiesDescriptor.ChildEntityGroups.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "EntitiesDescriptor")));
      foreach (EntityDescriptor childEntity in (IEnumerable<EntityDescriptor>) entitiesDescriptor.ChildEntities)
      {
        if (!string.IsNullOrEmpty(childEntity.FederationId) && !StringComparer.Ordinal.Equals(childEntity.FederationId, entitiesDescriptor.Name))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "FederationID")));
      }
      if (!string.IsNullOrEmpty(entitiesDescriptor.Name))
        xmlWriter.WriteAttributeString("Name", (string) null, entitiesDescriptor.Name);
      this.WriteCustomAttributes<EntitiesDescriptor>(xmlWriter, entitiesDescriptor);
      envelopedSignatureWriter?.WriteSignature();
      foreach (EntityDescriptor childEntity in (IEnumerable<EntityDescriptor>) entitiesDescriptor.ChildEntities)
        this.WriteEntityDescriptor(xmlWriter, childEntity);
      foreach (EntitiesDescriptor childEntityGroup in (IEnumerable<EntitiesDescriptor>) entitiesDescriptor.ChildEntityGroups)
        this.WriteEntitiesDescriptor(xmlWriter, childEntityGroup);
      this.WriteCustomElements<EntitiesDescriptor>(xmlWriter, entitiesDescriptor);
      xmlWriter.WriteEndElement();
    }

    protected virtual void WriteEntityDescriptor(
      XmlWriter inputWriter,
      EntityDescriptor entityDescriptor)
    {
      if (inputWriter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (inputWriter));
      if (entityDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (entityDescriptor));
      if (entityDescriptor.Contacts == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("entityDescriptor.Contacts");
      if (entityDescriptor.RoleDescriptors == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("entityDescriptor.RoleDescriptors");
      string referenceId = "_" + Guid.NewGuid().ToString();
      XmlWriter writer = inputWriter;
      EnvelopedSignatureWriter envelopedSignatureWriter = (EnvelopedSignatureWriter) null;
      if (entityDescriptor.SigningCredentials != null)
      {
        envelopedSignatureWriter = new EnvelopedSignatureWriter(inputWriter, entityDescriptor.SigningCredentials, referenceId, this.SecurityTokenSerializer);
        writer = (XmlWriter) envelopedSignatureWriter;
      }
      writer.WriteStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
      writer.WriteAttributeString("ID", (string) null, referenceId);
      if (entityDescriptor.EntityId == null || entityDescriptor.EntityId.Id == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "entityID")));
      writer.WriteAttributeString("entityID", (string) null, entityDescriptor.EntityId.Id);
      if (!string.IsNullOrEmpty(entityDescriptor.FederationId))
        writer.WriteAttributeString("FederationID", "http://docs.oasis-open.org/wsfed/federation/200706", entityDescriptor.FederationId);
      this.WriteCustomAttributes<EntityDescriptor>(writer, entityDescriptor);
      envelopedSignatureWriter?.WriteSignature();
      if (entityDescriptor.RoleDescriptors.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "RoleDescriptor")));
      foreach (RoleDescriptor roleDescriptor in (IEnumerable<RoleDescriptor>) entityDescriptor.RoleDescriptors)
      {
        if (roleDescriptor is ServiceProviderSingleSignOnDescriptor spssoDescriptor)
          this.WriteServiceProviderSingleSignOnDescriptor(writer, spssoDescriptor);
        if (roleDescriptor is IdentityProviderSingleSignOnDescriptor idpssoDescriptor)
          this.WriteIdentityProviderSingleSignOnDescriptor(writer, idpssoDescriptor);
        if (roleDescriptor is ApplicationServiceDescriptor appService)
          this.WriteApplicationServiceDescriptor(writer, appService);
        if (roleDescriptor is SecurityTokenServiceDescriptor securityTokenServiceDescriptor)
          this.WriteSecurityTokenServiceDescriptor(writer, securityTokenServiceDescriptor);
      }
      if (entityDescriptor.Organization != null)
        this.WriteOrganization(writer, entityDescriptor.Organization);
      foreach (ContactPerson contact in (IEnumerable<ContactPerson>) entityDescriptor.Contacts)
        this.WriteContactPerson(writer, contact);
      this.WriteCustomElements<EntityDescriptor>(writer, entityDescriptor);
      writer.WriteEndElement();
    }

    protected virtual void WriteIdentityProviderSingleSignOnDescriptor(
      XmlWriter writer,
      IdentityProviderSingleSignOnDescriptor idpssoDescriptor)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (idpssoDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (idpssoDescriptor));
      if (idpssoDescriptor.SupportedAttributes == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("idpssoDescriptor.SupportedAttributes");
      if (idpssoDescriptor.SingleSignOnServices == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("idpssoDescriptor.SingleSignOnServices");
      writer.WriteStartElement("IDPSSODescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
      if (idpssoDescriptor.WantAuthenticationRequestsSigned)
        writer.WriteAttributeString("WantAuthnRequestsSigned", (string) null, XmlConvert.ToString(idpssoDescriptor.WantAuthenticationRequestsSigned));
      this.WriteSingleSignOnDescriptorAttributes(writer, (SingleSignOnDescriptor) idpssoDescriptor);
      this.WriteCustomAttributes<IdentityProviderSingleSignOnDescriptor>(writer, idpssoDescriptor);
      this.WriteSingleSignOnDescriptorElements(writer, (SingleSignOnDescriptor) idpssoDescriptor);
      if (idpssoDescriptor.SingleSignOnServices.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "SingleSignOnService")));
      foreach (ProtocolEndpoint singleSignOnService in (IEnumerable<ProtocolEndpoint>) idpssoDescriptor.SingleSignOnServices)
      {
        if (singleSignOnService.ResponseLocation != (Uri) null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3249", (object) "ResponseLocation")));
        XmlQualifiedName element = new XmlQualifiedName("SingleSignOnService", "urn:oasis:names:tc:SAML:2.0:metadata");
        this.WriteProtocolEndpoint(writer, singleSignOnService, element);
      }
      foreach (Microsoft.IdentityModel.Tokens.Saml2.Saml2Attribute supportedAttribute in (IEnumerable<Microsoft.IdentityModel.Tokens.Saml2.Saml2Attribute>) idpssoDescriptor.SupportedAttributes)
        this.WriteAttribute(writer, supportedAttribute);
      this.WriteCustomElements<IdentityProviderSingleSignOnDescriptor>(writer, idpssoDescriptor);
      writer.WriteEndElement();
    }

    protected virtual void WriteIndexedProtocolEndpoint(
      XmlWriter writer,
      IndexedProtocolEndpoint indexedEP,
      XmlQualifiedName element)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (indexedEP == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (indexedEP));
      if (element == (XmlQualifiedName) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (element));
      writer.WriteStartElement(element.Name, element.Namespace);
      if (indexedEP.Binding == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "Binding")));
      writer.WriteAttributeString("Binding", (string) null, indexedEP.Binding.IsAbsoluteUri ? indexedEP.Binding.AbsoluteUri : indexedEP.Binding.ToString());
      if (indexedEP.Location == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "Location")));
      writer.WriteAttributeString("Location", (string) null, indexedEP.Location.IsAbsoluteUri ? indexedEP.Location.AbsoluteUri : indexedEP.Location.ToString());
      if (indexedEP.Index < 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "index")));
      writer.WriteAttributeString("index", (string) null, indexedEP.Index.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      if (indexedEP.ResponseLocation != (Uri) null)
        writer.WriteAttributeString("ResponseLocation", (string) null, indexedEP.ResponseLocation.IsAbsoluteUri ? indexedEP.ResponseLocation.AbsoluteUri : indexedEP.ResponseLocation.ToString());
      if (indexedEP.IsDefault.HasValue)
        writer.WriteAttributeString("isDefault", (string) null, XmlConvert.ToString(indexedEP.IsDefault.Value));
      this.WriteCustomAttributes<IndexedProtocolEndpoint>(writer, indexedEP);
      this.WriteCustomElements<IndexedProtocolEndpoint>(writer, indexedEP);
      writer.WriteEndElement();
    }

    protected virtual void WriteKeyDescriptor(XmlWriter writer, KeyDescriptor keyDescriptor)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (keyDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyDescriptor));
      writer.WriteStartElement("KeyDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
      if (keyDescriptor.Use == KeyType.Encryption || keyDescriptor.Use == KeyType.Signing)
        writer.WriteAttributeString("use", (string) null, keyDescriptor.Use.ToString().ToLowerInvariant());
      this.WriteCustomAttributes<KeyDescriptor>(writer, keyDescriptor);
      if (keyDescriptor.KeyInfo == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "KeyInfo")));
      this.SecurityTokenSerializer.WriteKeyIdentifier(writer, keyDescriptor.KeyInfo);
      if (keyDescriptor.EncryptionMethods != null && keyDescriptor.EncryptionMethods.Count > 0)
      {
        foreach (EncryptionMethod encryptionMethod in (IEnumerable<EncryptionMethod>) keyDescriptor.EncryptionMethods)
        {
          if (encryptionMethod.Algorithm == (Uri) null)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "Algorithm")));
          if (!encryptionMethod.Algorithm.IsAbsoluteUri)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID0014", (object) "Algorithm")));
          writer.WriteStartElement("EncryptionMethod", "urn:oasis:names:tc:SAML:2.0:metadata");
          writer.WriteAttributeString("Algorithm", (string) null, encryptionMethod.Algorithm.AbsoluteUri);
          writer.WriteEndElement();
        }
      }
      this.WriteCustomElements<KeyDescriptor>(writer, keyDescriptor);
      writer.WriteEndElement();
    }

    protected virtual void WriteLocalizedName(
      XmlWriter writer,
      LocalizedName name,
      XmlQualifiedName element)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (name == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (name));
      if (element == (XmlQualifiedName) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (element));
      if (name.Name == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("name.Name");
      writer.WriteStartElement(element.Name, element.Namespace);
      if (name.Language == null || string.IsNullOrEmpty(name.Name))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "lang")));
      writer.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", name.Language.Name);
      this.WriteCustomAttributes<LocalizedName>(writer, name);
      writer.WriteString(name.Name);
      this.WriteCustomElements<LocalizedName>(writer, name);
      writer.WriteEndElement();
    }

    protected virtual void WriteLocalizedUri(
      XmlWriter writer,
      LocalizedUri uri,
      XmlQualifiedName element)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (uri == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (uri));
      if (element == (XmlQualifiedName) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (element));
      writer.WriteStartElement(element.Name, element.Namespace);
      if (uri.Language == null || uri.Uri == (Uri) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "lang")));
      writer.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", uri.Language.Name);
      this.WriteCustomAttributes<LocalizedUri>(writer, uri);
      writer.WriteString(uri.Uri.IsAbsoluteUri ? uri.Uri.AbsoluteUri : uri.Uri.ToString());
      this.WriteCustomElements<LocalizedUri>(writer, uri);
      writer.WriteEndElement();
    }

    public void WriteMetadata(Stream stream, MetadataBase metadata)
    {
      if (stream == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (stream));
      if (metadata == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (metadata));
      using (XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, false))
        this.WriteMetadata((XmlWriter) textWriter, metadata);
    }

    public void WriteMetadata(XmlWriter writer, MetadataBase metadata)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (metadata == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (metadata));
      this.WriteMetadataCore(writer, metadata);
    }

    protected virtual void WriteMetadataCore(XmlWriter writer, MetadataBase metadataBase)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      switch (metadataBase)
      {
        case null:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (metadataBase));
        case EntitiesDescriptor entitiesDescriptor:
          this.WriteEntitiesDescriptor(writer, entitiesDescriptor);
          break;
        case EntityDescriptor entityDescriptor:
          this.WriteEntityDescriptor(writer, entityDescriptor);
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "EntitiesDescriptor")));
      }
    }

    protected virtual void WriteOrganization(XmlWriter writer, Organization organization)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (organization == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (organization));
      if (organization.DisplayNames == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("organization.DisplayNames");
      if (organization.Names == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("organization.Names");
      if (organization.Urls == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("organization.Urls");
      writer.WriteStartElement("Organization", "urn:oasis:names:tc:SAML:2.0:metadata");
      if (organization.Names.Count < 1)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "OrganizationName")));
      foreach (LocalizedName name in (Collection<LocalizedName>) organization.Names)
      {
        XmlQualifiedName element = new XmlQualifiedName("OrganizationName", "urn:oasis:names:tc:SAML:2.0:metadata");
        this.WriteLocalizedName(writer, name, element);
      }
      if (organization.DisplayNames.Count < 1)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "OrganizationDisplayName")));
      foreach (LocalizedName displayName in (Collection<LocalizedName>) organization.DisplayNames)
      {
        XmlQualifiedName element = new XmlQualifiedName("OrganizationDisplayName", "urn:oasis:names:tc:SAML:2.0:metadata");
        this.WriteLocalizedName(writer, displayName, element);
      }
      if (organization.Urls.Count < 1)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "OrganizationURL")));
      foreach (LocalizedUri url in (Collection<LocalizedUri>) organization.Urls)
      {
        XmlQualifiedName element = new XmlQualifiedName("OrganizationURL", "urn:oasis:names:tc:SAML:2.0:metadata");
        this.WriteLocalizedUri(writer, url, element);
      }
      this.WriteCustomAttributes<Organization>(writer, organization);
      this.WriteCustomElements<Organization>(writer, organization);
      writer.WriteEndElement();
    }

    protected virtual void WriteRoleDescriptorAttributes(
      XmlWriter writer,
      RoleDescriptor roleDescriptor)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (roleDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (roleDescriptor));
      DateTime dateTime = roleDescriptor.ProtocolsSupported != null ? roleDescriptor.ValidUntil : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.ProtocolsSupported");
      if (roleDescriptor.ValidUntil != DateTime.MaxValue)
        writer.WriteAttributeString("validUntil", (string) null, roleDescriptor.ValidUntil.ToString("s", (IFormatProvider) CultureInfo.InvariantCulture));
      if (roleDescriptor.ErrorUrl != (Uri) null)
        writer.WriteAttributeString("errorURL", (string) null, roleDescriptor.ErrorUrl.IsAbsoluteUri ? roleDescriptor.ErrorUrl.AbsoluteUri : roleDescriptor.ErrorUrl.ToString());
      if (roleDescriptor.ProtocolsSupported.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "protocolSupportEnumeration")));
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Uri uri in (IEnumerable<Uri>) roleDescriptor.ProtocolsSupported)
        stringBuilder.AppendFormat("{0} ", uri.IsAbsoluteUri ? (object) uri.AbsoluteUri : (object) uri.ToString());
      string str = stringBuilder.ToString();
      writer.WriteAttributeString("protocolSupportEnumeration", (string) null, str.Trim());
      this.WriteCustomAttributes<RoleDescriptor>(writer, roleDescriptor);
    }

    protected virtual void WriteRoleDescriptorElements(
      XmlWriter writer,
      RoleDescriptor roleDescriptor)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (roleDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (roleDescriptor));
      if (roleDescriptor.Contacts == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.Contacts");
      if (roleDescriptor.Keys == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.Keys");
      if (roleDescriptor.Organization != null)
        this.WriteOrganization(writer, roleDescriptor.Organization);
      foreach (KeyDescriptor key in (IEnumerable<KeyDescriptor>) roleDescriptor.Keys)
        this.WriteKeyDescriptor(writer, key);
      foreach (ContactPerson contact in (IEnumerable<ContactPerson>) roleDescriptor.Contacts)
        this.WriteContactPerson(writer, contact);
      this.WriteCustomElements<RoleDescriptor>(writer, roleDescriptor);
    }

    protected virtual void WriteSecurityTokenServiceDescriptor(
      XmlWriter writer,
      SecurityTokenServiceDescriptor securityTokenServiceDescriptor)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (securityTokenServiceDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenServiceDescriptor));
      if (securityTokenServiceDescriptor.SecurityTokenServiceEndpoints == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenServiceDescriptor.Endpoints");
      if (securityTokenServiceDescriptor.PassiveRequestorEndpoints == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenServiceDescriptor.PassiveRequestorEndpoints");
      writer.WriteStartElement("RoleDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
      writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "fed:SecurityTokenServiceType");
      writer.WriteAttributeString("xmlns", "fed", (string) null, "http://docs.oasis-open.org/wsfed/federation/200706");
      this.WriteWebServiceDescriptorAttributes(writer, (WebServiceDescriptor) securityTokenServiceDescriptor);
      this.WriteCustomAttributes<SecurityTokenServiceDescriptor>(writer, securityTokenServiceDescriptor);
      this.WriteWebServiceDescriptorElements(writer, (WebServiceDescriptor) securityTokenServiceDescriptor);
      if (securityTokenServiceDescriptor.SecurityTokenServiceEndpoints.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "SecurityTokenServiceEndpoint")));
      foreach (EndpointAddress tokenServiceEndpoint in securityTokenServiceDescriptor.SecurityTokenServiceEndpoints)
      {
        writer.WriteStartElement("SecurityTokenServiceEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706");
        tokenServiceEndpoint.WriteTo(AddressingVersion.WSAddressing10, writer);
        writer.WriteEndElement();
      }
      foreach (EndpointAddress requestorEndpoint in securityTokenServiceDescriptor.PassiveRequestorEndpoints)
      {
        writer.WriteStartElement("PassiveRequestorEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706");
        requestorEndpoint.WriteTo(AddressingVersion.WSAddressing10, writer);
        writer.WriteEndElement();
      }
      this.WriteCustomElements<SecurityTokenServiceDescriptor>(writer, securityTokenServiceDescriptor);
      writer.WriteEndElement();
    }

    protected virtual void WriteServiceProviderSingleSignOnDescriptor(
      XmlWriter writer,
      ServiceProviderSingleSignOnDescriptor spssoDescriptor)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (spssoDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (spssoDescriptor));
      if (spssoDescriptor.AssertionConsumerService == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("spssoDescriptor.AssertionConsumerService");
      writer.WriteStartElement("SPSSODescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
      if (spssoDescriptor.AuthenticationRequestsSigned)
        writer.WriteAttributeString("AuthnRequestsSigned", (string) null, XmlConvert.ToString(spssoDescriptor.AuthenticationRequestsSigned));
      if (spssoDescriptor.WantAssertionsSigned)
        writer.WriteAttributeString("WantAssertionsSigned", (string) null, XmlConvert.ToString(spssoDescriptor.WantAssertionsSigned));
      this.WriteSingleSignOnDescriptorAttributes(writer, (SingleSignOnDescriptor) spssoDescriptor);
      this.WriteCustomAttributes<ServiceProviderSingleSignOnDescriptor>(writer, spssoDescriptor);
      this.WriteSingleSignOnDescriptorElements(writer, (SingleSignOnDescriptor) spssoDescriptor);
      if (spssoDescriptor.AssertionConsumerService.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "AssertionConsumerService")));
      foreach (IndexedProtocolEndpoint indexedEP in (IEnumerable<IndexedProtocolEndpoint>) spssoDescriptor.AssertionConsumerService.Values)
      {
        XmlQualifiedName element = new XmlQualifiedName("AssertionConsumerService", "urn:oasis:names:tc:SAML:2.0:metadata");
        this.WriteIndexedProtocolEndpoint(writer, indexedEP, element);
      }
      this.WriteCustomElements<ServiceProviderSingleSignOnDescriptor>(writer, spssoDescriptor);
      writer.WriteEndElement();
    }

    protected virtual void WriteSingleSignOnDescriptorAttributes(
      XmlWriter writer,
      SingleSignOnDescriptor ssoDescriptor)
    {
      this.WriteRoleDescriptorAttributes(writer, (RoleDescriptor) ssoDescriptor);
      this.WriteCustomAttributes<SingleSignOnDescriptor>(writer, ssoDescriptor);
    }

    protected virtual void WriteSingleSignOnDescriptorElements(
      XmlWriter writer,
      SingleSignOnDescriptor ssoDescriptor)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (ssoDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (ssoDescriptor));
      this.WriteRoleDescriptorElements(writer, (RoleDescriptor) ssoDescriptor);
      if (ssoDescriptor.ArtifactResolutionServices != null && ssoDescriptor.ArtifactResolutionServices.Count > 0)
      {
        foreach (IndexedProtocolEndpoint indexedEP in (IEnumerable<IndexedProtocolEndpoint>) ssoDescriptor.ArtifactResolutionServices.Values)
        {
          if (indexedEP.ResponseLocation != (Uri) null)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3249", (object) "ResponseLocation")));
          XmlQualifiedName element = new XmlQualifiedName("ArtifactResolutionService", "urn:oasis:names:tc:SAML:2.0:metadata");
          this.WriteIndexedProtocolEndpoint(writer, indexedEP, element);
        }
      }
      if (ssoDescriptor.SingleLogoutServices != null && ssoDescriptor.SingleLogoutServices.Count > 0)
      {
        foreach (ProtocolEndpoint singleLogoutService in ssoDescriptor.SingleLogoutServices)
        {
          XmlQualifiedName element = new XmlQualifiedName("SingleLogoutService", "urn:oasis:names:tc:SAML:2.0:metadata");
          this.WriteProtocolEndpoint(writer, singleLogoutService, element);
        }
      }
      if (ssoDescriptor.NameIdentifierFormats != null && ssoDescriptor.NameIdentifierFormats.Count > 0)
      {
        foreach (Uri identifierFormat in (IEnumerable<Uri>) ssoDescriptor.NameIdentifierFormats)
        {
          if (!identifierFormat.IsAbsoluteUri)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID0014", (object) "NameIDFormat")));
          writer.WriteStartElement("NameIDFormat", "urn:oasis:names:tc:SAML:2.0:metadata");
          writer.WriteString(identifierFormat.AbsoluteUri);
          writer.WriteEndElement();
        }
      }
      this.WriteCustomElements<SingleSignOnDescriptor>(writer, ssoDescriptor);
    }

    protected virtual void WriteWebServiceDescriptorAttributes(
      XmlWriter writer,
      WebServiceDescriptor wsDescriptor)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (wsDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (wsDescriptor));
      this.WriteRoleDescriptorAttributes(writer, (RoleDescriptor) wsDescriptor);
      if (!string.IsNullOrEmpty(wsDescriptor.ServiceDisplayName))
        writer.WriteAttributeString("ServiceDisplayName", (string) null, wsDescriptor.ServiceDisplayName);
      if (!string.IsNullOrEmpty(wsDescriptor.ServiceDescription))
        writer.WriteAttributeString("ServiceDescription", (string) null, wsDescriptor.ServiceDescription);
      this.WriteCustomAttributes<WebServiceDescriptor>(writer, wsDescriptor);
    }

    protected virtual void WriteWebServiceDescriptorElements(
      XmlWriter writer,
      WebServiceDescriptor wsDescriptor)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (wsDescriptor == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (wsDescriptor));
      if (wsDescriptor.TargetScopes == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wsDescriptor.TargetScopes");
      if (wsDescriptor.ClaimTypesOffered == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wsDescriptor.ClaimTypesOffered");
      if (wsDescriptor.TokenTypesOffered == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wsDescriptor.TokenTypesOffered");
      this.WriteRoleDescriptorElements(writer, (RoleDescriptor) wsDescriptor);
      if (wsDescriptor.TokenTypesOffered.Count > 0)
      {
        writer.WriteStartElement("TokenTypesOffered", "http://docs.oasis-open.org/wsfed/federation/200706");
        foreach (Uri uri in (IEnumerable<Uri>) wsDescriptor.TokenTypesOffered)
        {
          writer.WriteStartElement("TokenType", "http://docs.oasis-open.org/wsfed/federation/200706");
          if (!uri.IsAbsoluteUri)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new MetadataSerializationException(Microsoft.IdentityModel.SR.GetString("ID3203", (object) "ClaimType")));
          writer.WriteAttributeString("Uri", uri.AbsoluteUri);
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      if (wsDescriptor.ClaimTypesOffered.Count > 0)
      {
        writer.WriteStartElement("ClaimTypesOffered", "http://docs.oasis-open.org/wsfed/federation/200706");
        foreach (DisplayClaim claim in (IEnumerable<DisplayClaim>) wsDescriptor.ClaimTypesOffered)
          this.WriteDisplayClaim(writer, claim);
        writer.WriteEndElement();
      }
      if (wsDescriptor.ClaimTypesRequested.Count > 0)
      {
        writer.WriteStartElement("ClaimTypesRequested", "http://docs.oasis-open.org/wsfed/federation/200706");
        foreach (DisplayClaim claim in (IEnumerable<DisplayClaim>) wsDescriptor.ClaimTypesRequested)
          this.WriteDisplayClaim(writer, claim);
        writer.WriteEndElement();
      }
      if (wsDescriptor.TargetScopes.Count > 0)
      {
        writer.WriteStartElement("TargetScopes", "http://docs.oasis-open.org/wsfed/federation/200706");
        foreach (EndpointAddress targetScope in (IEnumerable<EndpointAddress>) wsDescriptor.TargetScopes)
          targetScope.WriteTo(AddressingVersion.WSAddressing10, writer);
        writer.WriteEndElement();
      }
      this.WriteCustomElements<WebServiceDescriptor>(writer, wsDescriptor);
    }

    protected virtual Microsoft.IdentityModel.Tokens.Saml2.Saml2Attribute ReadAttribute(
      XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion"))
        reader.ReadStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion");
      try
      {
        bool isEmptyElement1 = reader.IsEmptyElement;
        Microsoft.IdentityModel.XmlUtil.ValidateXsiType(reader, "AttributeType", "urn:oasis:names:tc:SAML:2.0:assertion");
        string attribute1 = reader.GetAttribute("Name");
        Microsoft.IdentityModel.Tokens.Saml2.Saml2Attribute saml2Attribute = !string.IsNullOrEmpty(attribute1) ? new Microsoft.IdentityModel.Tokens.Saml2.Saml2Attribute(attribute1) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0001", (object) "Name", (object) "Attribute"));
        string attribute2 = reader.GetAttribute("NameFormat");
        if (!string.IsNullOrEmpty(attribute2))
          saml2Attribute.NameFormat = UriUtil.CanCreateValidUri(attribute2, UriKind.Absolute) ? new Uri(attribute2) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, Microsoft.IdentityModel.SR.GetString("ID0011", (object) "Namespace", (object) "Action"));
        saml2Attribute.FriendlyName = reader.GetAttribute("FriendlyName");
        reader.Read();
        if (!isEmptyElement1)
        {
          while (reader.IsStartElement("AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion"))
          {
            bool isEmptyElement2 = reader.IsEmptyElement;
            bool flag = Microsoft.IdentityModel.XmlUtil.IsNil(reader);
            Microsoft.IdentityModel.XmlUtil.ValidateXsiType(reader, "string", "http://www.w3.org/2001/XMLSchema");
            if (flag)
            {
              reader.Read();
              if (!isEmptyElement2)
                reader.ReadEndElement();
              saml2Attribute.Values.Add((string) null);
            }
            else if (isEmptyElement2)
            {
              reader.Read();
              saml2Attribute.Values.Add("");
            }
            else
              saml2Attribute.Values.Add(reader.ReadElementString());
          }
          reader.ReadEndElement();
        }
        return saml2Attribute;
      }
      catch (Exception ex)
      {
        Exception exception = MetadataSerializer.TryWrapReadException(reader, ex);
        if (exception != null)
          throw exception;
        throw;
      }
    }

    protected virtual void WriteAttribute(XmlWriter writer, Microsoft.IdentityModel.Tokens.Saml2.Saml2Attribute data)
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
      foreach (string text in data.Values)
      {
        writer.WriteStartElement("AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion");
        if (text == null)
          writer.WriteAttributeString("nil", "http://www.w3.org/2001/XMLSchema-instance", XmlConvert.ToString(true));
        else if (text.Length > 0)
          writer.WriteString(text);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
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
  }
}
