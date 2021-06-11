// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.ServiceCertificateElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Configuration;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class ServiceCertificateElement : ConfigurationElement
  {
    [ConfigurationProperty("certificateReference", IsRequired = false)]
    public CertificateReferenceElement CertificateReference
    {
      get => (CertificateReferenceElement) this["certificateReference"];
      set => this["certificateReference"] = (object) value;
    }

    internal X509Certificate2 GetCertificate()
    {
      if (!this.IsConfigured)
        return (X509Certificate2) null;
      try
      {
        return X509Util.ResolveCertificate(this.CertificateReference);
      }
      catch (InvalidOperationException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) this, "certificateReference", (Exception) ex);
      }
    }

    public bool IsConfigured => !string.IsNullOrEmpty(this.CertificateReference.FindValue);
  }
}
