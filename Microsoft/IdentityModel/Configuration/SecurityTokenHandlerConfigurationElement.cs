// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.SecurityTokenHandlerConfigurationElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class SecurityTokenHandlerConfigurationElement : ConfigurationElement
  {
    protected override void Init() => this.Name = "";

    [ConfigurationProperty("audienceUris", IsRequired = false)]
    public AudienceUriElementCollection AudienceUriElements
    {
      get => (AudienceUriElementCollection) this["audienceUris"];
      set => this["audienceUris"] = (object) value;
    }

    [ConfigurationProperty("certificateValidation", IsRequired = false)]
    public X509CertificateValidationCustomTypeElement CertificateValidationElement
    {
      get => (X509CertificateValidationCustomTypeElement) this["certificateValidation"];
      set => this["certificateValidation"] = (object) value;
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

    [ConfigurationProperty("name", IsRequired = false, Options = ConfigurationPropertyOptions.IsKey)]
    public string Name
    {
      get => (string) this["name"];
      set => this["name"] = (object) value;
    }

    [ConfigurationProperty("saveBootstrapTokens", IsRequired = false)]
    public string SaveBootstrapTokens
    {
      get => (string) this["saveBootstrapTokens"];
      set => this["saveBootstrapTokens"] = (object) value;
    }

    [ConfigurationProperty("maximumClockSkew", IsRequired = false)]
    public ValueTypeElement MaximumClockSkew
    {
      get => (ValueTypeElement) this["maximumClockSkew"];
      set => this["maximumClockSkew"] = (object) value;
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

    public bool IsConfigured => this.AudienceUriElements.IsConfigured || this.CertificateValidationElement.IsConfigured || (this.IssuerNameRegistry.IsConfigured || this.IssuerTokenResolver.IsConfigured) || (!string.IsNullOrEmpty(this.Name) || !string.IsNullOrEmpty(this.SaveBootstrapTokens) || (this.MaximumClockSkew.IsConfigured || this.ServiceTokenResolver.IsConfigured)) || this.TokenReplayDetectionElement.IsConfigured;
  }
}
