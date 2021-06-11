// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Configuration.FederatedAuthenticationElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web.Configuration
{
  [ComVisible(true)]
  public class FederatedAuthenticationElement : ConfigurationElement
  {
    [ConfigurationProperty("wsFederation", IsRequired = false)]
    public WSFederationAuthenticationElement WSFederation
    {
      get => (WSFederationAuthenticationElement) this["wsFederation"];
      set => this["wsFederation"] = (object) value;
    }

    [ConfigurationProperty("cookieHandler", IsRequired = false)]
    public CookieHandlerElement CookieHandler
    {
      get => (CookieHandlerElement) this["cookieHandler"];
      set => this["cookieHandler"] = (object) value;
    }

    public bool IsConfigured => this.WSFederation.IsConfigured || this.CookieHandler.IsConfigured;
  }
}
