// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.MicrosoftIdentityModelSection
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Configuration;
using System.Runtime.InteropServices;
using System.Web.Configuration;
using System.Web.Hosting;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class MicrosoftIdentityModelSection : ConfigurationSection
  {
    public const string SectionName = "microsoft.identityModel";

    public static MicrosoftIdentityModelSection Current
    {
      get
      {
        if (!HostingEnvironment.IsHosted)
          return ConfigurationManager.GetSection("microsoft.identityModel") as MicrosoftIdentityModelSection;
        return HostingEnvironment.ApplicationVirtualPath != null ? WebConfigurationManager.GetSection("microsoft.identityModel", HostingEnvironment.ApplicationVirtualPath) as MicrosoftIdentityModelSection : WebConfigurationManager.GetSection("microsoft.identityModel") as MicrosoftIdentityModelSection;
      }
    }

    public static ServiceElement DefaultServiceElement => MicrosoftIdentityModelSection.Current?.ServiceElements.GetElement("");

    [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
    public ServiceElementCollection ServiceElements => (ServiceElementCollection) this[""];

    public bool IsConfigured => this.ServiceElements.IsConfigured;
  }
}
