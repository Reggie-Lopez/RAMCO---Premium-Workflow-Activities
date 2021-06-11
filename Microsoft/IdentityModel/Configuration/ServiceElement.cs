// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.ServiceElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Web.Configuration;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class ServiceElement : ConfigurationElement
  {
    [ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey)]
    public string Name
    {
      get => (string) this["name"];
      set => this["name"] = (object) value;
    }

    [ConfigurationProperty("audienceUris", IsRequired = false)]
    public AudienceUriElementCollection AudienceUriElements
    {
      get => (AudienceUriElementCollection) this["audienceUris"];
      set => this["audienceUris"] = (object) value;
    }

    [ConfigurationProperty("certificateValidation", IsRequired = false)]
    public X509CertificateValidationElement CertificateValidationElement
    {
      get => (X509CertificateValidationElement) this["certificateValidation"];
      set => this["certificateValidation"] = (object) value;
    }

    [ConfigurationProperty("claimsAuthenticationManager", IsRequired = false)]
    public CustomTypeElement ClaimsAuthenticationManager
    {
      get => (CustomTypeElement) this["claimsAuthenticationManager"];
      set => this["claimsAuthenticationManager"] = (object) value;
    }

    [ConfigurationProperty("claimsAuthorizationManager", IsRequired = false)]
    public CustomTypeElement ClaimsAuthorizationManager
    {
      get => (CustomTypeElement) this["claimsAuthorizationManager"];
      set => this["claimsAuthorizationManager"] = (object) value;
    }

    [ConfigurationProperty("federatedAuthentication", IsRequired = false)]
    public FederatedAuthenticationElement FederatedAuthentication
    {
      get => (FederatedAuthenticationElement) this["federatedAuthentication"];
      set => this["federatedAuthentication"] = (object) value;
    }

    [ConfigurationProperty("issuerNameRegistry", IsRequired = false)]
    public CustomTypeElement IssuerNameRegistry
    {
      get => (CustomTypeElement) this["issuerNameRegistry"];
      set => this["issuerNameRegistry"] = (object) value;
    }

    [ConfigurationProperty("issuerTokenResolver", IsRequired = false)]
    public CustomTypeElement IssuerTokenResolver
    {
      get => (CustomTypeElement) this["issuerTokenResolver"];
      set => this["issuerTokenResolver"] = (object) value;
    }

    [ConfigurationProperty("maximumClockSkew", IsRequired = false)]
    public ValueTypeElement MaximumClockSkew
    {
      get => (ValueTypeElement) this["maximumClockSkew"];
      set => this["maximumClockSkew"] = (object) value;
    }

    [ConfigurationProperty("saveBootstrapTokens", IsRequired = false)]
    public string SaveBootstrapTokens
    {
      get => (string) this["saveBootstrapTokens"];
      set => this["saveBootstrapTokens"] = (object) value;
    }

    [ConfigurationProperty("serviceCertificate", IsRequired = false)]
    public ServiceCertificateElement ServiceCertificate
    {
      get => (ServiceCertificateElement) this["serviceCertificate"];
      set => this["serviceCertificate"] = (object) value;
    }

    [ConfigurationProperty("serviceTokenResolver", IsRequired = false)]
    public CustomTypeElement ServiceTokenResolver
    {
      get => (CustomTypeElement) this["serviceTokenResolver"];
      set => this["serviceTokenResolver"] = (object) value;
    }

    [ConfigurationProperty("tokenReplayDetection", IsRequired = false)]
    public TokenReplayDetectionElement TokenReplayDetectionElement
    {
      get => (TokenReplayDetectionElement) this["tokenReplayDetection"];
      set => this["tokenReplayDetection"] = (object) value;
    }

    [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
    public SecurityTokenHandlerSetElementCollection SecurityTokenHandlerSets => (SecurityTokenHandlerSetElementCollection) this[""];

    [ConfigurationProperty("applicationService", IsRequired = false)]
    internal ApplicationServiceConfigurationElement ApplicationService
    {
      get => (ApplicationServiceConfigurationElement) this["applicationService"];
      set => this["applicationService"] = (object) value;
    }

    public bool IsConfigured => !string.IsNullOrEmpty(this.Name) || this.AudienceUriElements.IsConfigured || (this.CertificateValidationElement.IsConfigured || this.ClaimsAuthenticationManager.IsConfigured) || (this.ClaimsAuthorizationManager.IsConfigured || this.FederatedAuthentication.IsConfigured || (this.IssuerNameRegistry.IsConfigured || this.IssuerTokenResolver.IsConfigured)) || (this.MaximumClockSkew.IsConfigured || !string.IsNullOrEmpty(this.SaveBootstrapTokens) || (this.ServiceCertificate.IsConfigured || this.ServiceTokenResolver.IsConfigured) || this.TokenReplayDetectionElement.IsConfigured) || this.SecurityTokenHandlerSets.IsConfigured;
  }
}
