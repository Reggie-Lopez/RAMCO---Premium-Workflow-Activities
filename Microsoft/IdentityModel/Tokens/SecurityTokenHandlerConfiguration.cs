// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Configuration;
using System;
using System.IdentityModel.Selectors;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class SecurityTokenHandlerConfiguration
  {
    public static readonly bool DefaultDetectReplayedTokens;
    public static readonly IssuerNameRegistry DefaultIssuerNameRegistry = (IssuerNameRegistry) new ConfigurationBasedIssuerNameRegistry();
    public static readonly SecurityTokenResolver DefaultIssuerTokenResolver = (SecurityTokenResolver) Microsoft.IdentityModel.Tokens.IssuerTokenResolver.DefaultInstance;
    public static readonly TimeSpan DefaultMaxClockSkew = new TimeSpan(0, 5, 0);
    public static readonly bool DefaultSaveBootstrapTokens = false;
    public static readonly int DefaultTokenReplayCacheCapacity = 500000;
    public static readonly TimeSpan DefaultTokenReplayCachePurgeInterval = TimeSpan.FromMinutes(1.0);
    public static readonly TimeSpan DefaultTokenReplayCacheExpirationPeriod = TimeSpan.MaxValue;
    public static readonly X509CertificateValidator DefaultCertificateValidator = X509Util.CreateCertificateValidator(ServiceConfiguration.DefaultCertificateValidationMode, ServiceConfiguration.DefaultRevocationMode, ServiceConfiguration.DefaultTrustedStoreLocation);
    private AudienceRestriction _audienceRestriction = new AudienceRestriction();
    private X509CertificateValidator _certificateValidator = SecurityTokenHandlerConfiguration.DefaultCertificateValidator;
    private bool _detectReplayedTokens = SecurityTokenHandlerConfiguration.DefaultDetectReplayedTokens;
    private IssuerNameRegistry _issuerNameRegistry = SecurityTokenHandlerConfiguration.DefaultIssuerNameRegistry;
    private SecurityTokenResolver _issuerTokenResolver = SecurityTokenHandlerConfiguration.DefaultIssuerTokenResolver;
    private TimeSpan _maxClockSkew = SecurityTokenHandlerConfiguration.DefaultMaxClockSkew;
    private bool _saveBootstrapTokens = SecurityTokenHandlerConfiguration.DefaultSaveBootstrapTokens;
    private SecurityTokenResolver _serviceTokenResolver = EmptySecurityTokenResolver.Instance;
    private TokenReplayCache _tokenReplayCache = (TokenReplayCache) new DefaultTokenReplayCache(SecurityTokenHandlerConfiguration.DefaultTokenReplayCacheCapacity, SecurityTokenHandlerConfiguration.DefaultTokenReplayCachePurgeInterval);
    private TimeSpan _tokenReplayCacheExpirationPeriod = SecurityTokenHandlerConfiguration.DefaultTokenReplayCacheExpirationPeriod;

    public AudienceRestriction AudienceRestriction
    {
      get => this._audienceRestriction;
      set => this._audienceRestriction = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public X509CertificateValidator CertificateValidator
    {
      get => this._certificateValidator;
      set => this._certificateValidator = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public bool DetectReplayedTokens
    {
      get => this._detectReplayedTokens;
      set => this._detectReplayedTokens = value;
    }

    public IssuerNameRegistry IssuerNameRegistry
    {
      get => this._issuerNameRegistry;
      set => this._issuerNameRegistry = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public SecurityTokenResolver IssuerTokenResolver
    {
      get => this._issuerTokenResolver;
      set => this._issuerTokenResolver = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public TimeSpan MaxClockSkew
    {
      get => this._maxClockSkew;
      set => this._maxClockSkew = !(value < TimeSpan.Zero) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value), (object) value, Microsoft.IdentityModel.SR.GetString("ID2070"));
    }

    public bool SaveBootstrapTokens
    {
      get => this._saveBootstrapTokens;
      set => this._saveBootstrapTokens = value;
    }

    public SecurityTokenResolver ServiceTokenResolver
    {
      get => this._serviceTokenResolver;
      set => this._serviceTokenResolver = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public TokenReplayCache TokenReplayCache
    {
      get => this._tokenReplayCache;
      set => this._tokenReplayCache = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public TimeSpan TokenReplayCacheExpirationPeriod
    {
      get => this._tokenReplayCacheExpirationPeriod;
      set => this._tokenReplayCacheExpirationPeriod = !(value <= TimeSpan.Zero) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value), (object) value, Microsoft.IdentityModel.SR.GetString("ID0016"));
    }
  }
}
