// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Configuration.WSFederationAuthenticationElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web.Configuration
{
  [ComVisible(true)]
  public class WSFederationAuthenticationElement : ConfigurationElement
  {
    private const bool DefaultPassiveRedirectEnabled = true;
    private const bool DefaultPersistentCookiesOnPassiveRedirects = false;
    private const bool DefaultRequireHttps = true;

    internal void Verify()
    {
      if (!this.IsConfigured)
        return;
      if (string.IsNullOrEmpty(this.Issuer))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) this, "issuer", Microsoft.IdentityModel.SR.GetString("ID1045"));
      Uri result1;
      if (this.RequireHttps && UriUtil.TryCreateValidUri(this.Issuer, UriKind.Absolute, out result1) && result1.Scheme != Uri.UriSchemeHttps)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) this, "issuer", Microsoft.IdentityModel.SR.GetString("ID1056"));
      if (string.IsNullOrEmpty(this.Realm))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) this, "realm", Microsoft.IdentityModel.SR.GetString("ID1046"));
      if (string.IsNullOrEmpty(this.Freshness))
        return;
      double result2 = -1.0;
      if (!double.TryParse(this.Freshness, out result2) || result2 < 0.0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) this, "freshness", Microsoft.IdentityModel.SR.GetString("ID1050"));
    }

    [ConfigurationProperty("authenticationType", IsRequired = false)]
    public string AuthenticationType => (string) this["authenticationType"];

    [ConfigurationProperty("freshness", IsRequired = false)]
    public string Freshness => (string) this["freshness"];

    [ConfigurationProperty("homeRealm", IsRequired = false)]
    public string HomeRealm => (string) this["homeRealm"];

    [ConfigurationProperty("issuer", IsRequired = true)]
    public string Issuer => (string) this["issuer"];

    [ConfigurationProperty("policy", IsRequired = false)]
    public string Policy => (string) this["policy"];

    [ConfigurationProperty("realm", IsRequired = true)]
    public string Realm => (string) this["realm"];

    [ConfigurationProperty("reply", IsRequired = false)]
    public string Reply => (string) this["reply"];

    [ConfigurationProperty("request", IsRequired = false)]
    public string Request => (string) this["request"];

    [ConfigurationProperty("requestPtr", IsRequired = false)]
    public string RequestPtr => (string) this["requestPtr"];

    [ConfigurationProperty("resource", IsRequired = false)]
    public string Resource => (string) this["resource"];

    [ConfigurationProperty("signInQueryString", IsRequired = false)]
    public string SignInQueryString => (string) this["signInQueryString"];

    [ConfigurationProperty("signOutQueryString", IsRequired = false)]
    public string SignOutQueryString => (string) this["signOutQueryString"];

    [ConfigurationProperty("signOutReply", IsRequired = false)]
    public string SignOutReply => (string) this["signOutReply"];

    [ConfigurationProperty("passiveRedirectEnabled", DefaultValue = true, IsRequired = false)]
    public bool PassiveRedirectEnabled => (bool) this["passiveRedirectEnabled"];

    [ConfigurationProperty("persistentCookiesOnPassiveRedirects", DefaultValue = false, IsRequired = false)]
    public bool PersistentCookiesOnPassiveRedirects => (bool) this["persistentCookiesOnPassiveRedirects"];

    [ConfigurationProperty("requireHttps", DefaultValue = true, IsRequired = false)]
    public bool RequireHttps => (bool) this["requireHttps"];

    public bool IsConfigured => !string.IsNullOrEmpty(this.AuthenticationType) || !string.IsNullOrEmpty(this.Freshness) || (!string.IsNullOrEmpty(this.HomeRealm) || !string.IsNullOrEmpty(this.Issuer)) || (!string.IsNullOrEmpty(this.Policy) || !string.IsNullOrEmpty(this.Realm) || (!string.IsNullOrEmpty(this.Reply) || !string.IsNullOrEmpty(this.Request))) || (!string.IsNullOrEmpty(this.RequestPtr) || !string.IsNullOrEmpty(this.Resource) || (!string.IsNullOrEmpty(this.SignInQueryString) || !string.IsNullOrEmpty(this.SignOutQueryString)) || (!string.IsNullOrEmpty(this.SignOutReply) || !this.PassiveRedirectEnabled || this.PersistentCookiesOnPassiveRedirects)) || !this.RequireHttps;
  }
}
