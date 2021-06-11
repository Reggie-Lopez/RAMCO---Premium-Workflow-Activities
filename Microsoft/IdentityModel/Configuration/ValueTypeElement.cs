// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.ValueTypeElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class ValueTypeElement : ConfigurationElement
  {
    [ConfigurationProperty("value", IsRequired = true)]
    public string Value
    {
      get => (string) this["value"];
      set => this[nameof (value)] = (object) value;
    }

    public bool IsConfigured => !string.IsNullOrEmpty(this.Value);
  }
}
