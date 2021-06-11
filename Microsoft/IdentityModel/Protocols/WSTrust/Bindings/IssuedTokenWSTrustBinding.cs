// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.Bindings.IssuedTokenWSTrustBinding
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust.Bindings
{
  [ComVisible(true)]
  public class IssuedTokenWSTrustBinding : WSTrustBindingBase
  {
    private SecurityKeyType _keyType;
    private SecurityAlgorithmSuite _algorithmSuite;
    private string _tokenType;
    private Binding _issuerBinding;
    private EndpointAddress _issuerAddress;
    private Collection<System.ServiceModel.Security.Tokens.ClaimTypeRequirement> _claimTypeRequirements = new Collection<System.ServiceModel.Security.Tokens.ClaimTypeRequirement>();
    private EndpointAddress _issuerMetadataAddress;

    public IssuedTokenWSTrustBinding()
      : this((Binding) null, (EndpointAddress) null)
    {
    }

    public IssuedTokenWSTrustBinding(Binding issuerBinding, EndpointAddress issuerAddress)
      : this(issuerBinding, issuerAddress, SecurityMode.Message, TrustVersion.WSTrust13, (EndpointAddress) null)
    {
    }

    public IssuedTokenWSTrustBinding(
      Binding issuerBinding,
      EndpointAddress issuerAddress,
      EndpointAddress issuerMetadataAddress)
      : this(issuerBinding, issuerAddress, SecurityMode.Message, TrustVersion.WSTrust13, issuerMetadataAddress)
    {
    }

    public IssuedTokenWSTrustBinding(
      Binding issuerBinding,
      EndpointAddress issuerAddress,
      SecurityMode mode,
      TrustVersion trustVersion,
      EndpointAddress issuerMetadataAddress)
      : this(issuerBinding, issuerAddress, mode, trustVersion, SecurityKeyType.SymmetricKey, SecurityAlgorithmSuite.Basic256, (string) null, (IEnumerable<System.ServiceModel.Security.Tokens.ClaimTypeRequirement>) null, issuerMetadataAddress)
    {
    }

    public IssuedTokenWSTrustBinding(
      Binding issuerBinding,
      EndpointAddress issuerAddress,
      string tokenType,
      IEnumerable<System.ServiceModel.Security.Tokens.ClaimTypeRequirement> claimTypeRequirements)
      : this(issuerBinding, issuerAddress, SecurityKeyType.SymmetricKey, SecurityAlgorithmSuite.Basic256, tokenType, claimTypeRequirements)
    {
    }

    public IssuedTokenWSTrustBinding(
      Binding issuerBinding,
      EndpointAddress issuerAddress,
      SecurityKeyType keyType,
      SecurityAlgorithmSuite algorithmSuite,
      string tokenType,
      IEnumerable<System.ServiceModel.Security.Tokens.ClaimTypeRequirement> claimTypeRequirements)
      : this(issuerBinding, issuerAddress, SecurityMode.Message, TrustVersion.WSTrust13, keyType, algorithmSuite, tokenType, claimTypeRequirements, (EndpointAddress) null)
    {
    }

    public IssuedTokenWSTrustBinding(
      Binding issuerBinding,
      EndpointAddress issuerAddress,
      SecurityMode mode,
      TrustVersion version,
      SecurityKeyType keyType,
      SecurityAlgorithmSuite algorithmSuite,
      string tokenType,
      IEnumerable<System.ServiceModel.Security.Tokens.ClaimTypeRequirement> claimTypeRequirements,
      EndpointAddress issuerMetadataAddress)
      : base(mode, version)
    {
      if (SecurityMode.Message != mode && SecurityMode.TransportWithMessageCredential != mode)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3226", (object) mode));
      if (this._keyType == SecurityKeyType.BearerKey && version == TrustVersion.WSTrustFeb2005)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3267"));
      this._keyType = keyType;
      this._algorithmSuite = algorithmSuite;
      this._tokenType = tokenType;
      this._issuerBinding = issuerBinding;
      this._issuerAddress = issuerAddress;
      this._issuerMetadataAddress = issuerMetadataAddress;
      if (claimTypeRequirements == null)
        return;
      foreach (System.ServiceModel.Security.Tokens.ClaimTypeRequirement claimTypeRequirement in claimTypeRequirements)
        this._claimTypeRequirements.Add(claimTypeRequirement);
    }

    public Binding IssuerBinding
    {
      get => this._issuerBinding;
      set => this._issuerBinding = value;
    }

    public EndpointAddress IssuerAddress
    {
      get => this._issuerAddress;
      set => this._issuerAddress = value;
    }

    public EndpointAddress IssuerMetadataAddress
    {
      get => this._issuerMetadataAddress;
      set => this._issuerMetadataAddress = value;
    }

    public SecurityKeyType KeyType
    {
      get => this._keyType;
      set => this._keyType = value;
    }

    public SecurityAlgorithmSuite AlgorithmSuite
    {
      get => this._algorithmSuite;
      set => this._algorithmSuite = value;
    }

    public string TokenType
    {
      get => this._tokenType;
      set => this._tokenType = value;
    }

    public Collection<System.ServiceModel.Security.Tokens.ClaimTypeRequirement> ClaimTypeRequirement => this._claimTypeRequirements;

    protected override SecurityBindingElement CreateSecurityBindingElement()
    {
      IssuedSecurityTokenParameters issuedParameters = new IssuedSecurityTokenParameters(this._tokenType, this._issuerAddress, this._issuerBinding);
      issuedParameters.KeyType = this._keyType;
      issuedParameters.IssuerMetadataAddress = this._issuerMetadataAddress;
      issuedParameters.KeySize = this._keyType != SecurityKeyType.SymmetricKey ? 0 : this._algorithmSuite.DefaultSymmetricKeyLength;
      if (this._claimTypeRequirements != null)
      {
        foreach (System.ServiceModel.Security.Tokens.ClaimTypeRequirement claimTypeRequirement in this._claimTypeRequirements)
          issuedParameters.ClaimTypeRequirements.Add(claimTypeRequirement);
      }
      this.AddAlgorithmParameters(this._algorithmSuite, this.TrustVersion, this._keyType, ref issuedParameters);
      SecurityBindingElement securityBindingElement;
      if (SecurityMode.Message == this.SecurityMode)
        securityBindingElement = (SecurityBindingElement) SecurityBindingElement.CreateIssuedTokenForCertificateBindingElement(issuedParameters);
      else if (SecurityMode.TransportWithMessageCredential == this.SecurityMode)
        securityBindingElement = (SecurityBindingElement) SecurityBindingElement.CreateIssuedTokenOverTransportBindingElement(issuedParameters);
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3226", (object) this.SecurityMode));
      securityBindingElement.DefaultAlgorithmSuite = this._algorithmSuite;
      securityBindingElement.IncludeTimestamp = true;
      return securityBindingElement;
    }

    private void AddAlgorithmParameters(
      SecurityAlgorithmSuite algorithmSuite,
      TrustVersion trustVersion,
      SecurityKeyType keyType,
      ref IssuedSecurityTokenParameters issuedParameters)
    {
      issuedParameters.AdditionalRequestParameters.Insert(0, this.CreateEncryptionAlgorithmElement(algorithmSuite.DefaultEncryptionAlgorithm));
      issuedParameters.AdditionalRequestParameters.Insert(0, this.CreateCanonicalizationAlgorithmElement(algorithmSuite.DefaultCanonicalizationAlgorithm));
      string signatureAlgorithm;
      string encryptionAlgorithm;
      switch (keyType)
      {
        case SecurityKeyType.SymmetricKey:
          signatureAlgorithm = algorithmSuite.DefaultSymmetricSignatureAlgorithm;
          encryptionAlgorithm = algorithmSuite.DefaultEncryptionAlgorithm;
          break;
        case SecurityKeyType.AsymmetricKey:
          signatureAlgorithm = algorithmSuite.DefaultAsymmetricSignatureAlgorithm;
          encryptionAlgorithm = algorithmSuite.DefaultAsymmetricKeyWrapAlgorithm;
          break;
        case SecurityKeyType.BearerKey:
          return;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (keyType));
      }
      issuedParameters.AdditionalRequestParameters.Insert(0, this.CreateSignWithElement(signatureAlgorithm));
      issuedParameters.AdditionalRequestParameters.Insert(0, this.CreateEncryptWithElement(encryptionAlgorithm));
      if (trustVersion == TrustVersion.WSTrustFeb2005)
        return;
      issuedParameters.AdditionalRequestParameters.Insert(0, IssuedTokenWSTrustBinding.CreateKeyWrapAlgorithmElement(algorithmSuite.DefaultAsymmetricKeyWrapAlgorithm));
    }

    protected override void ApplyTransportSecurity(HttpTransportBindingElement transport) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3227")));

    private XmlElement CreateSignWithElement(string signatureAlgorithm)
    {
      if (signatureAlgorithm == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (signatureAlgorithm));
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement xmlElement = (XmlElement) null;
      if (this.TrustVersion == TrustVersion.WSTrust13)
        xmlElement = xmlDocument.CreateElement("trust", "SignatureAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
      else if (this.TrustVersion == TrustVersion.WSTrustFeb2005)
        xmlElement = xmlDocument.CreateElement("t", "SignatureAlgorithm", "http://schemas.xmlsoap.org/ws/2005/02/trust");
      xmlElement?.AppendChild((XmlNode) xmlDocument.CreateTextNode(signatureAlgorithm));
      return xmlElement;
    }

    private XmlElement CreateEncryptionAlgorithmElement(string encryptionAlgorithm)
    {
      if (encryptionAlgorithm == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (encryptionAlgorithm));
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement xmlElement = (XmlElement) null;
      if (this.TrustVersion == TrustVersion.WSTrust13)
        xmlElement = xmlDocument.CreateElement("trust", "EncryptionAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
      else if (this.TrustVersion == TrustVersion.WSTrustFeb2005)
        xmlElement = xmlDocument.CreateElement("t", "EncryptionAlgorithm", "http://schemas.xmlsoap.org/ws/2005/02/trust");
      xmlElement?.AppendChild((XmlNode) xmlDocument.CreateTextNode(encryptionAlgorithm));
      return xmlElement;
    }

    private XmlElement CreateCanonicalizationAlgorithmElement(
      string canonicalizationAlgorithm)
    {
      if (canonicalizationAlgorithm == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (canonicalizationAlgorithm));
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement xmlElement = (XmlElement) null;
      if (this.TrustVersion == TrustVersion.WSTrust13)
        xmlElement = xmlDocument.CreateElement("trust", "CanonicalizationAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
      else if (this.TrustVersion == TrustVersion.WSTrustFeb2005)
        xmlElement = xmlDocument.CreateElement("t", "CanonicalizationAlgorithm", "http://schemas.xmlsoap.org/ws/2005/02/trust");
      xmlElement?.AppendChild((XmlNode) xmlDocument.CreateTextNode(canonicalizationAlgorithm));
      return xmlElement;
    }

    private XmlElement CreateEncryptWithElement(string encryptionAlgorithm)
    {
      if (encryptionAlgorithm == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (encryptionAlgorithm));
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement xmlElement = (XmlElement) null;
      if (this.TrustVersion == TrustVersion.WSTrust13)
        xmlElement = xmlDocument.CreateElement("trust", "EncryptWith", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
      else if (this.TrustVersion == TrustVersion.WSTrustFeb2005)
        xmlElement = xmlDocument.CreateElement("t", "EncryptWith", "http://schemas.xmlsoap.org/ws/2005/02/trust");
      xmlElement?.AppendChild((XmlNode) xmlDocument.CreateTextNode(encryptionAlgorithm));
      return xmlElement;
    }

    private static XmlElement CreateKeyWrapAlgorithmElement(string keyWrapAlgorithm)
    {
      if (keyWrapAlgorithm == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyWrapAlgorithm));
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement element = xmlDocument.CreateElement("trust", "KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
      element.AppendChild((XmlNode) xmlDocument.CreateTextNode(keyWrapAlgorithm));
      return element;
    }
  }
}
