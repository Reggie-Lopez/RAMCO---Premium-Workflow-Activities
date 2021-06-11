// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.AudienceUriElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class AudienceUriElement : ConfigurationElement
  {
    private const string DefaultValue = " ";

    [ConfigurationProperty("value", DefaultValue = " ", IsKey = true, IsRequired = true)]
    [StringValidator(MinLength = 1)]
    public string Value
    {
      get => (string) this["value"];
      set => this[nameof (value)] = (object) value;
    }

    public bool IsConfigured => !string.Equals(this.Value, " ", StringComparison.Ordinal);
  }
}
