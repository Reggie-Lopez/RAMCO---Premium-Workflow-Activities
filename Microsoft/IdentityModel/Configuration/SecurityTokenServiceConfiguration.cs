// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.SecurityTokenServiceConfiguration
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSTrust;
using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class SecurityTokenServiceConfiguration : ServiceConfiguration
  {
    internal const int DefaultKeySizeInBitsConstant = 256;
    private string _tokenIssuerName;
    private SigningCredentials _signingCredentials;
    private TimeSpan _defaultTokenLifetime = TimeSpan.FromHours(1.0);
    private TimeSpan _maximumTokenLifetime = TimeSpan.FromDays(1.0);
    private string _defaultTokenType = "urn:oasis:names:tc:SAML:1.0:assertion";
    private int _defaultSymmetricKeySizeInBits = 256;
    private int _defaultMaxSymmetricKeySizeInBits = 1024;
    private Collection<ServiceHostEndpointConfiguration> _endpoints = new Collection<ServiceHostEndpointConfiguration>();
    private Type _securityTokenServiceType;
    private WSTrust13RequestSerializer _wsTrust13RequestSerializer = new WSTrust13RequestSerializer();
    private WSTrust13ResponseSerializer _wsTrust13ResponseSerializer = new WSTrust13ResponseSerializer();
    private WSTrustFeb2005RequestSerializer _wsTrustFeb2005RequestSerializer = new WSTrustFeb2005RequestSerializer();
    private WSTrustFeb2005ResponseSerializer _wsTrustFeb2005ResponseSerializer = new WSTrustFeb2005ResponseSerializer();

    public SecurityTokenServiceConfiguration()
      : this((string) null, (SigningCredentials) null)
    {
    }

    public SecurityTokenServiceConfiguration(bool loadConfig)
      : this((string) null, (SigningCredentials) null, loadConfig)
    {
    }

    public SecurityTokenServiceConfiguration(string issuerName)
      : this(issuerName, (SigningCredentials) null)
    {
    }

    public SecurityTokenServiceConfiguration(string issuerName, bool loadConfig)
      : this(issuerName, (SigningCredentials) null, loadConfig)
    {
    }

    public SecurityTokenServiceConfiguration(
      string issuerName,
      SigningCredentials signingCredentials)
    {
      this._tokenIssuerName = issuerName;
      this._signingCredentials = signingCredentials;
    }

    public SecurityTokenServiceConfiguration(
      string issuerName,
      SigningCredentials signingCredentials,
      bool loadConfig)
      : base(loadConfig)
    {
      this._tokenIssuerName = issuerName;
      this._signingCredentials = signingCredentials;
    }

    public SecurityTokenServiceConfiguration(
      string issuerName,
      SigningCredentials signingCredentials,
      string serviceName)
      : base(serviceName)
    {
      this._tokenIssuerName = issuerName;
      this._signingCredentials = signingCredentials;
    }

    public Type SecurityTokenService
    {
      get => this._securityTokenServiceType;
      set
      {
        if ((object) value == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
        this._securityTokenServiceType = typeof (Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService).IsAssignableFrom(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID2069"));
      }
    }

    public virtual Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService CreateSecurityTokenService()
    {
      Type securityTokenService = this.SecurityTokenService;
      if ((object) securityTokenService == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2073"));
      return typeof (Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService).IsAssignableFrom(securityTokenService) ? Activator.CreateInstance(securityTokenService, (object) this) as Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2074", (object) securityTokenService, (object) typeof (Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService)));
    }

    public int DefaultSymmetricKeySizeInBits
    {
      get => this._defaultSymmetricKeySizeInBits;
      set => this._defaultSymmetricKeySizeInBits = value > 0 ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value), Microsoft.IdentityModel.SR.GetString("ID0002"));
    }

    public int DefaultMaxSymmetricKeySizeInBits
    {
      get => this._defaultMaxSymmetricKeySizeInBits;
      set => this._defaultMaxSymmetricKeySizeInBits = value > 0 ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value), Microsoft.IdentityModel.SR.GetString("ID0002"));
    }

    public TimeSpan DefaultTokenLifetime
    {
      get => this._defaultTokenLifetime;
      set => this._defaultTokenLifetime = value;
    }

    public string DefaultTokenType
    {
      get => this._defaultTokenType;
      set
      {
        if (string.IsNullOrEmpty(value))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (value));
        this._defaultTokenType = this.SecurityTokenHandlers[value] != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID2015", (object) value));
      }
    }

    public TimeSpan MaximumTokenLifetime
    {
      get => this._maximumTokenLifetime;
      set => this._maximumTokenLifetime = !(value <= TimeSpan.Zero) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value), Microsoft.IdentityModel.SR.GetString("ID0016"));
    }

    public SigningCredentials SigningCredentials
    {
      get => this._signingCredentials;
      set => this._signingCredentials = value;
    }

    public string TokenIssuerName
    {
      get => this._tokenIssuerName;
      set => this._tokenIssuerName = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public Collection<ServiceHostEndpointConfiguration> TrustEndpoints => this._endpoints;

    public WSTrust13RequestSerializer WSTrust13RequestSerializer
    {
      get => this._wsTrust13RequestSerializer;
      set => this._wsTrust13RequestSerializer = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public WSTrust13ResponseSerializer WSTrust13ResponseSerializer
    {
      get => this._wsTrust13ResponseSerializer;
      set => this._wsTrust13ResponseSerializer = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public WSTrustFeb2005RequestSerializer WSTrustFeb2005RequestSerializer
    {
      get => this._wsTrustFeb2005RequestSerializer;
      set => this._wsTrustFeb2005RequestSerializer = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public WSTrustFeb2005ResponseSerializer WSTrustFeb2005ResponseSerializer
    {
      get => this._wsTrustFeb2005ResponseSerializer;
      set => this._wsTrustFeb2005ResponseSerializer = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }
  }
}
