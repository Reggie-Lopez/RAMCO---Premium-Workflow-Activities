// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.FederatedSecurityTokenProvider
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class FederatedSecurityTokenProvider : IssuedSecurityTokenProvider
  {
    private FederatedClientCredentialsParameters _additionalParameters;
    private Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager _securityTokenHandlerCollectionManager;

    public FederatedSecurityTokenProvider(
      FederatedClientCredentialsParameters federatedClientCredentialsParameters,
      IssuedSecurityTokenProvider federatedSecurityTokenProvider)
      : this(federatedClientCredentialsParameters, federatedSecurityTokenProvider, Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager.CreateDefaultSecurityTokenHandlerCollectionManager())
    {
    }

    public FederatedSecurityTokenProvider(
      FederatedClientCredentialsParameters federatedClientCredentialsParameters,
      IssuedSecurityTokenProvider federatedSecurityTokenProvider,
      Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager)
    {
      if (federatedClientCredentialsParameters == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (federatedClientCredentialsParameters));
      if (federatedSecurityTokenProvider == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (federatedSecurityTokenProvider));
      if (securityTokenHandlerCollectionManager == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenHandlerCollectionManager));
      this._additionalParameters = federatedClientCredentialsParameters;
      this.CloneBase(federatedSecurityTokenProvider);
      this._securityTokenHandlerCollectionManager = securityTokenHandlerCollectionManager;
    }

    internal FederatedClientCredentialsParameters AdditionalParameters
    {
      get => this._additionalParameters;
      set => this._additionalParameters = value;
    }

    internal void CloneBase(
      IssuedSecurityTokenProvider issuedSecurityTokenProvider)
    {
      this.IdentityVerifier = issuedSecurityTokenProvider.IdentityVerifier;
      this.IssuerBinding = issuedSecurityTokenProvider.IssuerBinding;
      this.IssuerAddress = issuedSecurityTokenProvider.IssuerAddress;
      this.TargetAddress = issuedSecurityTokenProvider.TargetAddress;
      this.KeyEntropyMode = issuedSecurityTokenProvider.KeyEntropyMode;
      this.IdentityVerifier = issuedSecurityTokenProvider.IdentityVerifier;
      this.CacheIssuedTokens = issuedSecurityTokenProvider.CacheIssuedTokens;
      this.MaxIssuedTokenCachingTime = issuedSecurityTokenProvider.MaxIssuedTokenCachingTime;
      this.MessageSecurityVersion = issuedSecurityTokenProvider.MessageSecurityVersion;
      this.SecurityTokenSerializer = issuedSecurityTokenProvider.SecurityTokenSerializer;
      this.SecurityAlgorithmSuite = issuedSecurityTokenProvider.SecurityAlgorithmSuite;
      this.IssuedTokenRenewalThresholdPercentage = issuedSecurityTokenProvider.IssuedTokenRenewalThresholdPercentage;
      if (issuedSecurityTokenProvider.IssuerChannelBehaviors != null && issuedSecurityTokenProvider.IssuerChannelBehaviors.Count > 0)
      {
        foreach (IEndpointBehavior issuerChannelBehavior in (Collection<IEndpointBehavior>) issuedSecurityTokenProvider.IssuerChannelBehaviors)
          this.IssuerChannelBehaviors.Add(issuerChannelBehavior);
      }
      if (issuedSecurityTokenProvider.TokenRequestParameters == null || issuedSecurityTokenProvider.TokenRequestParameters.Count <= 0)
        return;
      foreach (XmlElement requestParameter in issuedSecurityTokenProvider.TokenRequestParameters)
        this.TokenRequestParameters.Add(requestParameter);
    }

    protected override IAsyncResult BeginGetTokenCore(
      TimeSpan timeout,
      AsyncCallback callback,
      object state)
    {
      this.SetupParameters();
      return base.BeginGetTokenCore(timeout, callback, state);
    }

    protected override SecurityToken GetTokenCore(TimeSpan timeout)
    {
      this.SetupParameters();
      return base.GetTokenCore(timeout);
    }

    private void SetupParameters()
    {
      if (this.AdditionalParameters.IssuedSecurityToken != null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3024"));
      if (this.AdditionalParameters.OnBehalfOf != null)
      {
        if (this.MessageSecurityVersion.TrustVersion == TrustVersion.WSTrust13)
        {
          if (this.TokenRequestParameterExists("OnBehalfOf", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3266", (object) "OnBehalfOf"));
          this.TokenRequestParameters.Add(this.CreateXmlTokenElement(this.AdditionalParameters.OnBehalfOf, "trust", "OnBehalfOf", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", "OnBehalfOf"));
        }
        else if (this.MessageSecurityVersion.TrustVersion == TrustVersion.WSTrustFeb2005)
        {
          if (this.TokenRequestParameterExists("OnBehalfOf", "http://schemas.xmlsoap.org/ws/2005/02/trust"))
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3266", (object) "OnBehalfOf"));
          this.TokenRequestParameters.Add(this.CreateXmlTokenElement(this.AdditionalParameters.OnBehalfOf, "t", "OnBehalfOf", "http://schemas.xmlsoap.org/ws/2005/02/trust", "OnBehalfOf"));
        }
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3137", (object) this.MessageSecurityVersion.TrustVersion.Namespace)));
      }
      if (this.AdditionalParameters.ActAs == null)
        return;
      if (this.TokenRequestParameterExists("ActAs", "http://docs.oasis-open.org/ws-sx/ws-trust/200802"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3266", (object) "ActAs"));
      this.TokenRequestParameters.Add(this.CreateXmlTokenElement(this.AdditionalParameters.ActAs, "tr", "ActAs", "http://docs.oasis-open.org/ws-sx/ws-trust/200802", "ActAs"));
    }

    private bool TokenRequestParameterExists(string localName, string xmlNamespace)
    {
      foreach (XmlElement requestParameter in this.TokenRequestParameters)
      {
        if (requestParameter.LocalName == localName && requestParameter.NamespaceURI == xmlNamespace)
          return true;
      }
      return false;
    }

    private XmlElement CreateXmlTokenElement(
      SecurityToken token,
      string prefix,
      string name,
      string ns,
      string usage)
    {
      Stream stream = (Stream) new MemoryStream();
      using (XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, false))
      {
        textWriter.WriteStartElement(prefix, name, ns);
        this.WriteToken((XmlWriter) textWriter, token, usage);
        textWriter.WriteEndElement();
        textWriter.Flush();
      }
      stream.Seek(0L, SeekOrigin.Begin);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.PreserveWhitespace = true;
      xmlDocument.Load(XmlReader.Create(stream, new XmlReaderSettings()
      {
        XmlResolver = (XmlResolver) null
      }));
      stream.Close();
      return xmlDocument.DocumentElement;
    }

    private void WriteToken(XmlWriter xmlWriter, SecurityToken token, string usage)
    {
      Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection handlerCollection = !this._securityTokenHandlerCollectionManager.ContainsKey(usage) ? this._securityTokenHandlerCollectionManager[""] : this._securityTokenHandlerCollectionManager[usage];
      if (handlerCollection != null && handlerCollection.CanWriteToken(token))
        handlerCollection.WriteToken(xmlWriter, token);
      else
        this.SecurityTokenSerializer.WriteToken(xmlWriter, token);
    }
  }
}
