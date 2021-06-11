// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.SecurityTokenHandlerElementCollection
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml11;
using Microsoft.IdentityModel.Tokens.Saml2;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
  [ConfigurationCollection(typeof (CustomTypeElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
  [ComVisible(true)]
  public class SecurityTokenHandlerElementCollection : ConfigurationElementCollection
  {
    protected override ConfigurationElement CreateNewElement() => (ConfigurationElement) new CustomTypeElement();

    protected override object GetElementKey(ConfigurationElement element) => (object) ((CustomTypeElement) element).TypeName;

    protected override void Init()
    {
      this.BaseAdd((ConfigurationElement) new CustomTypeElement(typeof (Saml11SecurityTokenHandler)));
      this.BaseAdd((ConfigurationElement) new CustomTypeElement(typeof (Saml2SecurityTokenHandler)));
      this.BaseAdd((ConfigurationElement) new CustomTypeElement(typeof (WindowsUserNameSecurityTokenHandler)));
      this.BaseAdd((ConfigurationElement) new CustomTypeElement(typeof (X509SecurityTokenHandler)));
      this.BaseAdd((ConfigurationElement) new CustomTypeElement(typeof (KerberosSecurityTokenHandler)));
      this.BaseAdd((ConfigurationElement) new CustomTypeElement(typeof (RsaSecurityTokenHandler)));
      this.BaseAdd((ConfigurationElement) new CustomTypeElement(typeof (SessionSecurityTokenHandler)));
      this.BaseAdd((ConfigurationElement) new CustomTypeElement(typeof (EncryptedSecurityTokenHandler)));
    }

    [ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey)]
    public string Name
    {
      get => (string) this["name"];
      set => this["name"] = (object) value;
    }

    [ConfigurationProperty("securityTokenHandlerConfiguration", IsRequired = false)]
    public SecurityTokenHandlerConfigurationElement HandlerConfiguration
    {
      get => (SecurityTokenHandlerConfigurationElement) this["securityTokenHandlerConfiguration"];
      set => this["securityTokenHandlerConfiguration"] = (object) value;
    }

    public bool IsConfigured => !string.IsNullOrEmpty(this.Name) || this.HandlerConfiguration.IsConfigured || this.Count > 0;
  }
}
