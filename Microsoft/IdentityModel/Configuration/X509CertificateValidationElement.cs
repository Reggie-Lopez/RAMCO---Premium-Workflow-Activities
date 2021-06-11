// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.X509CertificateValidationElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.ComponentModel;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class X509CertificateValidationElement : ConfigurationElement
  {
    private const X509CertificateValidationMode DefaultX509CertificateValidationMode = X509CertificateValidationMode.PeerOrChainTrust;
    private const X509RevocationMode DefaultX509RevocationMode = X509RevocationMode.Online;
    private const StoreLocation DefaultStoreLocation = StoreLocation.LocalMachine;

    [ConfigurationProperty("certificateValidationMode", DefaultValue = X509CertificateValidationMode.PeerOrChainTrust, IsRequired = false)]
    [TypeConverter(typeof (X509CertificateValidationModeConverter))]
    public X509CertificateValidationMode ValidationMode
    {
      get => (X509CertificateValidationMode) this["certificateValidationMode"];
      set => this["certificateValidationMode"] = (object) value;
    }

    [TypeConverter(typeof (X509RevocationModeConverter))]
    [ConfigurationProperty("revocationMode", DefaultValue = X509RevocationMode.Online, IsRequired = false)]
    public X509RevocationMode RevocationMode
    {
      get => (X509RevocationMode) this["revocationMode"];
      set => this["revocationMode"] = (object) value;
    }

    [ConfigurationProperty("trustedStoreLocation", DefaultValue = StoreLocation.LocalMachine, IsRequired = false)]
    [TypeConverter(typeof (StoreLocationConverter))]
    public StoreLocation TrustedStoreLocation
    {
      get => (StoreLocation) this["trustedStoreLocation"];
      set => this["trustedStoreLocation"] = (object) value;
    }

    [ConfigurationProperty("certificateValidator", IsRequired = false)]
    public CustomTypeElement CustomType
    {
      get => (CustomTypeElement) this["certificateValidator"];
      set => this["certificateValidator"] = (object) value;
    }

    public bool IsConfigured => this.ValidationMode != X509CertificateValidationMode.PeerOrChainTrust || this.RevocationMode != X509RevocationMode.Online || this.TrustedStoreLocation != StoreLocation.LocalMachine || this.CustomType.IsConfigured;
  }
}
