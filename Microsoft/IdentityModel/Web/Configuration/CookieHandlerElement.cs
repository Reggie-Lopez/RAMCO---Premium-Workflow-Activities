// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Configuration.CookieHandlerElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Configuration;
using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web.Configuration
{
  [ComVisible(true)]
  public class CookieHandlerElement : ConfigurationElement
  {
    private const bool DefaultHideFromScript = true;
    private const bool DefaultRequireSsl = true;
    private const string MaxPersistentSessionLifetimeString = "365.0:0:0";
    private const string TimeSpanZeroString = "0:0:0";

    internal CookieHandler GetConfiguredCookieHandler()
    {
      CookieHandlerMode cookieHandlerMode = this.Mode;
      if (cookieHandlerMode == CookieHandlerMode.Default)
        cookieHandlerMode = CookieHandlerMode.Chunked;
      if (this.ChunkedCookieHandler.IsConfigured && CookieHandlerMode.Chunked != cookieHandlerMode)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) this, "mode", Microsoft.IdentityModel.SR.GetString("ID1027", (object) "chunkedCookieHandler", (object) this.Mode));
      if (this.CustomCookieHandler.IsConfigured && CookieHandlerMode.Custom != cookieHandlerMode)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) this, "mode", Microsoft.IdentityModel.SR.GetString("ID1027", (object) "customCookieHandler", (object) this.Mode));
      CookieHandler handler = (CookieHandler) new Microsoft.IdentityModel.Web.ChunkedCookieHandler();
      switch (cookieHandlerMode)
      {
        case CookieHandlerMode.Chunked:
          this.ApplyChunked(ref handler);
          break;
        case CookieHandlerMode.Custom:
          this.ApplyCustom(ref handler);
          break;
      }
      handler.HideFromClientScript = this.HideFromScript;
      handler.RequireSsl = this.RequireSsl;
      if (!string.IsNullOrEmpty(this.Domain))
        handler.Domain = this.Domain;
      if (!string.IsNullOrEmpty(this.Name))
        handler.Name = this.Name;
      if (!string.IsNullOrEmpty(this.Path))
        handler.Path = this.Path;
      if (this.PersistentSessionLifetime > TimeSpan.Zero)
        handler.PersistentSessionLifetime = new TimeSpan?(this.PersistentSessionLifetime);
      return handler;
    }

    private void ApplyChunked(ref CookieHandler handler)
    {
      if (!this.ChunkedCookieHandler.IsConfigured)
        return;
      try
      {
        handler = (CookieHandler) new Microsoft.IdentityModel.Web.ChunkedCookieHandler(this.ChunkedCookieHandler.ChunkSize);
      }
      catch (ArgumentException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) this.ChunkedCookieHandler, "chunkSize", (Exception) ex);
      }
    }

    private void ApplyCustom(ref CookieHandler handler) => handler = this.CustomCookieHandler.IsConfigured ? CustomTypeElement.Resolve<CookieHandler>(this.CustomCookieHandler) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) this, "mode", Microsoft.IdentityModel.SR.GetString("ID1028", (object) "customCookieHandler", (object) this.Mode));

    [ConfigurationProperty("mode", DefaultValue = CookieHandlerMode.Default, IsRequired = false)]
    public CookieHandlerMode Mode
    {
      get => (CookieHandlerMode) this["mode"];
      set => this["mode"] = (object) value;
    }

    [ConfigurationProperty("chunkedCookieHandler", IsRequired = false)]
    public ChunkedCookieHandlerElement ChunkedCookieHandler
    {
      get => (ChunkedCookieHandlerElement) this["chunkedCookieHandler"];
      set => this["chunkedCookieHandler"] = (object) value;
    }

    [ConfigurationProperty("customCookieHandler", IsRequired = false)]
    public CustomTypeElement CustomCookieHandler
    {
      get => (CustomTypeElement) this["customCookieHandler"];
      set => this["customCookieHandler"] = (object) value;
    }

    [ConfigurationProperty("domain", IsRequired = false)]
    public string Domain
    {
      get => (string) this["domain"];
      set => this["domain"] = (object) value;
    }

    [ConfigurationProperty("hideFromScript", DefaultValue = true, IsRequired = false)]
    public bool HideFromScript
    {
      get => (bool) this["hideFromScript"];
      set => this["hideFromScript"] = (object) value;
    }

    [ConfigurationProperty("name", IsRequired = false)]
    public string Name
    {
      get => (string) this["name"];
      set => this["name"] = (object) value;
    }

    [ConfigurationProperty("path", IsRequired = false)]
    public string Path
    {
      get => (string) this["path"];
      set => this["path"] = (object) value;
    }

    [ConfigurationProperty("persistentSessionLifetime", DefaultValue = "0:0:0", IsRequired = false)]
    [TimeSpanValidator(MaxValueString = "365.0:0:0", MinValueString = "0:0:0")]
    public TimeSpan PersistentSessionLifetime
    {
      get => (TimeSpan) this["persistentSessionLifetime"];
      set => this["persistentSessionLifetime"] = (object) value;
    }

    [ConfigurationProperty("requireSsl", DefaultValue = true, IsRequired = false)]
    public bool RequireSsl
    {
      get => (bool) this["requireSsl"];
      set => this["requireSsl"] = (object) value;
    }

    public bool IsConfigured => this.Mode != CookieHandlerMode.Default || this.ChunkedCookieHandler.IsConfigured || (this.CustomCookieHandler.IsConfigured || !string.IsNullOrEmpty(this.Domain)) || (!this.HideFromScript || !string.IsNullOrEmpty(this.Name) || !string.IsNullOrEmpty(this.Path)) || !this.PersistentSessionLifetime.Equals(TimeSpan.Zero) || !this.RequireSsl;
  }
}
