// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.TokenReplayDetectionElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class TokenReplayDetectionElement : ConfigurationElement
  {
    [ConfigurationProperty("purgeInterval", IsRequired = false)]
    public string PurgeInterval
    {
      get => (string) this["purgeInterval"];
      set => this["purgeInterval"] = (object) value;
    }

    [ConfigurationProperty("capacity", IsRequired = false)]
    public string Capacity
    {
      get => (string) this["capacity"];
      set => this["capacity"] = (object) value;
    }

    [ConfigurationProperty("enabled", IsRequired = false)]
    public string Enabled
    {
      get => (string) this["enabled"];
      set => this["enabled"] = (object) value;
    }

    [ConfigurationProperty("expirationPeriod", IsRequired = false)]
    public string ExpirationPeriod
    {
      get => (string) this["expirationPeriod"];
      set => this["expirationPeriod"] = (object) value;
    }

    [ConfigurationProperty("replayCache", IsRequired = false)]
    public CustomTypeElement CustomType
    {
      get => (CustomTypeElement) this["replayCache"];
      set => this["replayCache"] = (object) value;
    }

    public bool IsConfigured => !string.IsNullOrEmpty(this.PurgeInterval) || !string.IsNullOrEmpty(this.Capacity) || (!string.IsNullOrEmpty(this.Enabled) || !string.IsNullOrEmpty(this.ExpirationPeriod)) || this.CustomType.IsConfigured;
  }
}
