// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.TokenServiceEndpoint
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class TokenServiceEndpoint
  {
    private EndpointAddress _endpointAddress;
    private UserCredentialType _userCredentialType;
    private string _displayCredentialHint;
    private SecurityKeyIdentifier _encryptingCredentials;
    private MetadataSet _mexSet;
    private MetadataReference _metadataReference;
    private X509Certificate2Collection _certificates;

    public TokenServiceEndpoint(string address)
      : this(new EndpointAddress(address))
    {
    }

    public TokenServiceEndpoint(EndpointAddress endpointAddress)
      : this(endpointAddress, TokenService.DefaultUserCredentialType)
    {
    }

    public TokenServiceEndpoint(
      EndpointAddress endpointAddress,
      UserCredentialType userCredentialType)
      : this(endpointAddress, userCredentialType, (string) null)
    {
    }

    public TokenServiceEndpoint(
      EndpointAddress endpointAddress,
      UserCredentialType userCredentialType,
      string displayCredentialHint)
    {
      this._endpointAddress = !(endpointAddress == (EndpointAddress) null) ? endpointAddress : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (endpointAddress));
      this._userCredentialType = userCredentialType;
      this._displayCredentialHint = displayCredentialHint;
      this._certificates = !(endpointAddress.Identity is X509CertificateEndpointIdentity identity) ? new X509Certificate2Collection() : identity.Certificates;
      XmlDictionaryReader readerAtMetadata = this._endpointAddress.GetReaderAtMetadata();
      if (readerAtMetadata == null)
        return;
      MetadataSet metadataSet = new MetadataSet();
      ((IXmlSerializable) metadataSet).ReadXml((XmlReader) readerAtMetadata);
      if (metadataSet.MetadataSections == null || metadataSet.MetadataSections.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("endpoint", Microsoft.IdentityModel.SR.GetString("ID2049"));
      for (int index = 0; index < metadataSet.MetadataSections.Count; ++index)
      {
        MetadataSection metadataSection = metadataSet.MetadataSections[index];
        if (metadataSection.Metadata == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("endpoint", Microsoft.IdentityModel.SR.GetString("ID2049"));
        if (metadataSection.Metadata is MetadataReference metadata)
        {
          this._metadataReference = metadata;
          break;
        }
      }
      this._mexSet = metadataSet;
    }

    public TokenServiceEndpoint(string address, string mexAddress)
      : this(address, mexAddress, TokenService.DefaultUserCredentialType)
    {
    }

    public TokenServiceEndpoint(string address, string mexAddress, X509Certificate2 certificate)
      : this(address, mexAddress, TokenService.DefaultUserCredentialType, certificate)
    {
    }

    public TokenServiceEndpoint(
      string address,
      string mexAddress,
      UserCredentialType userCredentialType)
      : this(address, mexAddress, userCredentialType, (X509Certificate2) null)
    {
    }

    public TokenServiceEndpoint(
      string address,
      string mexAddress,
      UserCredentialType userCredentialType,
      X509Certificate2 certificate)
      : this(address, mexAddress, userCredentialType, certificate, (string) null)
    {
    }

    public TokenServiceEndpoint(
      string address,
      string mexAddress,
      UserCredentialType userCredentialType,
      X509Certificate2 certificate,
      string displayCredentialHint)
    {
      if (string.IsNullOrEmpty(address))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (address)));
      this._userCredentialType = userCredentialType;
      this._displayCredentialHint = displayCredentialHint;
      this._certificates = new X509Certificate2Collection();
      if (certificate != null)
        this._certificates.Add(certificate);
      if (!string.IsNullOrEmpty(mexAddress))
      {
        MetadataReference metadataReference = new MetadataReference();
        metadataReference.Address = new EndpointAddress(mexAddress);
        metadataReference.AddressVersion = AddressingVersion.WSAddressing10;
        MetadataSection metadataSection = new MetadataSection();
        metadataSection.Dialect = "http://schemas.xmlsoap.org/ws/2004/09/mex";
        metadataSection.Metadata = (object) metadataReference;
        this._mexSet = new MetadataSet();
        this._mexSet.MetadataSections.Add(metadataSection);
        MemoryStream memoryStream = new MemoryStream();
        using (XmlTextWriter xmlTextWriter = new XmlTextWriter((Stream) memoryStream, Encoding.UTF8))
        {
          this._mexSet.WriteTo((XmlWriter) xmlTextWriter);
          xmlTextWriter.Flush();
          memoryStream.Seek(0L, SeekOrigin.Begin);
          XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader((Stream) memoryStream, XmlDictionaryReaderQuotas.Max);
          this._endpointAddress = new EndpointAddress(new Uri(address), certificate != null ? (EndpointIdentity) new X509CertificateEndpointIdentity(certificate) : (EndpointIdentity) null, (AddressHeaderCollection) null, textReader, (XmlDictionaryReader) null);
        }
        this._metadataReference = metadataReference;
      }
      else
        this._endpointAddress = new EndpointAddress(new Uri(address), certificate != null ? (EndpointIdentity) new X509CertificateEndpointIdentity(certificate) : (EndpointIdentity) null, new AddressHeader[0]);
    }

    internal X509Certificate2Collection Certificates => this._certificates;

    public string DisplayCredentialHint
    {
      get => this._displayCredentialHint;
      set => this._displayCredentialHint = value;
    }

    public EndpointAddress Address => this._endpointAddress;

    public MetadataSet MetadataSet => this._mexSet;

    public MetadataReference MetadataReference => this._metadataReference;

    public SecurityKeyIdentifier EncryptingCredentials
    {
      get => this._encryptingCredentials;
      set => this._encryptingCredentials = value;
    }

    public UserCredentialType UserCredentialType => this._userCredentialType;
  }
}
