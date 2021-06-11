// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.AudienceUriElementCollection
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.ComponentModel;
using System.Configuration;
using System.IdentityModel.Selectors;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
  [ConfigurationCollection(typeof (AudienceUriElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
  [ComVisible(true)]
  public class AudienceUriElementCollection : ConfigurationElementCollection
  {
    private const AudienceUriMode DefaultAudienceUriMode = AudienceUriMode.Always;

    protected override void Init() => base.Init();

    protected override ConfigurationElement CreateNewElement() => (ConfigurationElement) new AudienceUriElement();

    protected override object GetElementKey(ConfigurationElement element) => (object) ((AudienceUriElement) element).Value;

    [TypeConverter(typeof (AudienceUriModeConverter))]
    [ConfigurationProperty("mode", DefaultValue = AudienceUriMode.Always, IsRequired = false)]
    public AudienceUriMode Mode
    {
      get => (AudienceUriMode) this["mode"];
      set => this["mode"] = (object) value;
    }

    public bool IsConfigured => this.Mode != AudienceUriMode.Always || this.Count > 0;
  }
}
