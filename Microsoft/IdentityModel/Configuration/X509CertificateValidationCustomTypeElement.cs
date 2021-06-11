// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.X509CertificateValidationCustomTypeElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class X509CertificateValidationCustomTypeElement : ConfigurationElement
  {
    [ConfigurationProperty("certificateValidator", IsRequired = true)]
    public CustomTypeElement CustomType
    {
      get => (CustomTypeElement) this["certificateValidator"];
      set => this["certificateValidator"] = (object) value;
    }

    public bool IsConfigured => this.CustomType.IsConfigured;
  }
}
