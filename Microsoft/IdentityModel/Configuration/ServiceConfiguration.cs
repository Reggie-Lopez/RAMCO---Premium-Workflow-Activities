// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.ServiceConfiguration
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Xml;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class ServiceConfiguration
  {
    internal const string ServiceConfigurationKey = "ServiceConfiguration";
    public static readonly string DefaultServiceName = "";
    public static readonly TimeSpan DefaultMaxClockSkew = new TimeSpan(0, 5, 0);
    public static readonly X509CertificateValidationMode DefaultCertificateValidationMode = X509CertificateValidationMode.PeerOrChainTrust;
    public static readonly X509RevocationMode DefaultRevocationMode = X509RevocationMode.Online;
    public static readonly StoreLocation DefaultTrustedStoreLocation = StoreLocation.LocalMachine;
    private X509CertificateValidationMode _certificateValidationMode = ServiceConfiguration.DefaultCertificateValidationMode;
    private ClaimsAuthenticationManager _claimsAuthenticationManager = new ClaimsAuthenticationManager();
    private ClaimsAuthorizationManager _claimsAuthorizationManager = new ClaimsAuthorizationManager();
    private bool _disableWsdl;
    private Microsoft.IdentityModel.ExceptionMapper _exceptionMapper = new Microsoft.IdentityModel.ExceptionMapper();
    private bool _isInitialized;
    private X509RevocationMode _revocationMode = ServiceConfiguration.DefaultRevocationMode;
    private Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager _securityTokenHandlerCollectionManager;
    private string _serviceName = ServiceConfiguration.DefaultServiceName;
    private X509Certificate2 _serviceCertificate;
    private TimeSpan _serviceMaxClockSkew = ServiceConfiguration.DefaultMaxClockSkew;
    private Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration _serviceHandlerConfiguration;
    private StoreLocation _trustedStoreLocation = ServiceConfiguration.DefaultTrustedStoreLocation;

    public ServiceConfiguration() => this.LoadConfiguration(MicrosoftIdentityModelSection.Current?.ServiceElements.GetElement(ServiceConfiguration.DefaultServiceName));

    public ServiceConfiguration(bool loadConfig)
    {
      if (loadConfig)
        this.LoadConfiguration((MicrosoftIdentityModelSection.Current ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7027"))).ServiceElements.GetElement(ServiceConfiguration.DefaultServiceName));
      else
        this.LoadConfiguration((ServiceElement) null);
    }

    public ServiceConfiguration(string serviceConfigurationName)
    {
      if (serviceConfigurationName == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (serviceConfigurationName));
      MicrosoftIdentityModelSection current = MicrosoftIdentityModelSection.Current;
      if (current == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7027"));
      this._serviceName = serviceConfigurationName;
      this.LoadConfiguration(current.ServiceElements.GetElement(serviceConfigurationName));
    }

    internal static ServiceConfiguration GetCurrent()
    {
      if (OperationContext.Current == null)
        return FederatedAuthentication.ServiceConfiguration;
      return OperationContext.Current.IncomingMessageProperties.ContainsKey(nameof (ServiceConfiguration)) && OperationContext.Current.IncomingMessageProperties[nameof (ServiceConfiguration)] is ServiceConfiguration incomingMessageProperty ? incomingMessageProperty : new ServiceConfiguration();
    }

    public Microsoft.IdentityModel.Tokens.AudienceRestriction AudienceRestriction
    {
      get => this._serviceHandlerConfiguration.AudienceRestriction;
      set => this._serviceHandlerConfiguration.AudienceRestriction = value;
    }

    public X509CertificateValidationMode CertificateValidationMode
    {
      get => this._certificateValidationMode;
      set => this._certificateValidationMode = value;
    }

    public X509CertificateValidator CertificateValidator
    {
      get => this._serviceHandlerConfiguration.CertificateValidator;
      set => this._serviceHandlerConfiguration.CertificateValidator = value;
    }

    public ClaimsAuthenticationManager ClaimsAuthenticationManager
    {
      get => this._claimsAuthenticationManager;
      set => this._claimsAuthenticationManager = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public ClaimsAuthorizationManager ClaimsAuthorizationManager
    {
      get => this._claimsAuthorizationManager;
      set => this._claimsAuthorizationManager = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public bool DetectReplayedTokens
    {
      get => this._serviceHandlerConfiguration.DetectReplayedTokens;
      set => this._serviceHandlerConfiguration.DetectReplayedTokens = value;
    }

    public bool DisableWsdl
    {
      get => this._disableWsdl;
      set => this._disableWsdl = value;
    }

    public virtual bool IsInitialized
    {
      get => this._isInitialized;
      protected set => this._isInitialized = value;
    }

    private static SecurityTokenResolver GetServiceTokenResolver(
      ServiceElement element)
    {
      try
      {
        return CustomTypeElement.Resolve<SecurityTokenResolver>(element.ServiceTokenResolver);
      }
      catch (ArgumentException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) element, "serviceTokenResolver", (Exception) ex);
      }
    }

    private static SecurityTokenResolver GetIssuerTokenResolver(
      ServiceElement element)
    {
      try
      {
        return CustomTypeElement.Resolve<SecurityTokenResolver>(element.IssuerTokenResolver);
      }
      catch (ArgumentException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) element, "issuerTokenResolver", (Exception) ex);
      }
    }

    private static ClaimsAuthenticationManager GetClaimsAuthenticationManager(
      ServiceElement element)
    {
      try
      {
        return CustomTypeElement.Resolve<ClaimsAuthenticationManager>(element.ClaimsAuthenticationManager);
      }
      catch (ArgumentException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) element, "claimsAuthenticationManager", (Exception) ex);
      }
    }

    private static Microsoft.IdentityModel.Tokens.IssuerNameRegistry GetIssuerNameRegistry(
      ServiceElement element)
    {
      try
      {
        return CustomTypeElement.Resolve<Microsoft.IdentityModel.Tokens.IssuerNameRegistry>(element.IssuerNameRegistry);
      }
      catch (ArgumentException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) element, "issuerNameRegistry", (Exception) ex);
      }
    }

    private static X509Certificate2 GetServiceCertificate(ServiceElement element)
    {
      try
      {
        X509Certificate2 certificate = element.ServiceCertificate.GetCertificate();
        if (certificate != null)
          X509Util.EnsureAndGetPrivateRSAKey(certificate);
        return certificate;
      }
      catch (ArgumentException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) element, "serviceCertificate", (Exception) ex);
      }
    }

    public SecurityTokenResolver CreateAggregateTokenResolver()
    {
      List<SecurityTokenResolver> securityTokenResolverList = new List<SecurityTokenResolver>();
      if (this._serviceCertificate != null)
        securityTokenResolverList.Add(SecurityTokenResolver.CreateDefaultSecurityTokenResolver(new List<SecurityToken>(1)
        {
          (SecurityToken) new X509SecurityToken(this._serviceCertificate)
        }.AsReadOnly(), false));
      if (this._serviceHandlerConfiguration != null && this._serviceHandlerConfiguration.ServiceTokenResolver != null && !object.ReferenceEquals((object) this._serviceHandlerConfiguration.ServiceTokenResolver, (object) Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance))
        securityTokenResolverList.Add(this._serviceHandlerConfiguration.ServiceTokenResolver);
      if (securityTokenResolverList.Count == 1)
        return securityTokenResolverList[0];
      return securityTokenResolverList.Count > 1 ? (SecurityTokenResolver) new Microsoft.IdentityModel.Tokens.AggregateTokenResolver((IEnumerable<SecurityTokenResolver>) securityTokenResolverList) : Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance;
    }

    public virtual void Initialize()
    {
      if (this.IsInitialized)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID7009"));
      Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection securityTokenHandlers = this.SecurityTokenHandlers;
      if (!object.ReferenceEquals((object) this._serviceHandlerConfiguration, (object) securityTokenHandlers.Configuration))
      {
        DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, Microsoft.IdentityModel.SR.GetString("ID4283"));
        this.IsInitialized = true;
      }
      else
      {
        securityTokenHandlers.Configuration.ServiceTokenResolver = this.CreateAggregateTokenResolver();
        if (this.CertificateValidationMode != X509CertificateValidationMode.Custom)
          securityTokenHandlers.Configuration.CertificateValidator = X509Util.CreateCertificateValidator(this.CertificateValidationMode, this.RevocationMode, this.TrustedStoreLocation);
        else if (object.ReferenceEquals((object) securityTokenHandlers.Configuration.CertificateValidator, (object) Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration.DefaultCertificateValidator))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4280")));
        this.IsInitialized = true;
      }
    }

    protected void LoadConfiguration(ServiceElement element)
    {
      if (element != null)
      {
        if (element.ClaimsAuthenticationManager.IsConfigured)
          this._claimsAuthenticationManager = ServiceConfiguration.GetClaimsAuthenticationManager(element);
        if (element.ClaimsAuthorizationManager.IsConfigured)
          this._claimsAuthorizationManager = CustomTypeElement.Resolve<ClaimsAuthorizationManager>(element.ClaimsAuthorizationManager);
        if (this._serviceCertificate == null && element.ServiceCertificate.IsConfigured)
          this._serviceCertificate = ServiceConfiguration.GetServiceCertificate(element);
        if (element.CertificateValidationElement.IsConfigured)
        {
          this._revocationMode = element.CertificateValidationElement.RevocationMode;
          this._certificateValidationMode = element.CertificateValidationElement.ValidationMode;
          this._trustedStoreLocation = element.CertificateValidationElement.TrustedStoreLocation;
        }
        this._serviceHandlerConfiguration = this.LoadHandlerConfiguration(element);
      }
      this._securityTokenHandlerCollectionManager = this.LoadHandlers(element);
    }

    protected Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager LoadHandlers(
      ServiceElement serviceElement)
    {
      Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager collectionManager = Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager.CreateEmptySecurityTokenHandlerCollectionManager();
      if (serviceElement != null)
      {
        if (serviceElement.SecurityTokenHandlerSets.Count > 0)
        {
          foreach (SecurityTokenHandlerElementCollection securityTokenHandlerSet in (ConfigurationElementCollection) serviceElement.SecurityTokenHandlerSets)
          {
            try
            {
              Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration configuration;
              if (string.IsNullOrEmpty(securityTokenHandlerSet.Name) || StringComparer.Ordinal.Equals(securityTokenHandlerSet.Name, ""))
              {
                if (securityTokenHandlerSet.HandlerConfiguration.IsConfigured)
                {
                  this._serviceHandlerConfiguration = this.LoadHandlerConfiguration(serviceElement);
                  configuration = this.LoadHandlerConfiguration(this._serviceHandlerConfiguration, securityTokenHandlerSet.HandlerConfiguration);
                }
                else
                  configuration = this.LoadHandlerConfiguration(serviceElement);
                this._serviceHandlerConfiguration = configuration;
              }
              else
                configuration = !securityTokenHandlerSet.HandlerConfiguration.IsConfigured ? new Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration() : this.LoadHandlerConfiguration((Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration) null, securityTokenHandlerSet.HandlerConfiguration);
              Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection handlerCollection = new Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection(configuration);
              collectionManager[securityTokenHandlerSet.Name] = handlerCollection;
              foreach (CustomTypeElement customTypeElement in (ConfigurationElementCollection) securityTokenHandlerSet)
                handlerCollection.Add(CustomTypeElement.Resolve<Microsoft.IdentityModel.Tokens.SecurityTokenHandler>(customTypeElement));
            }
            catch (ArgumentException ex)
            {
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) serviceElement, securityTokenHandlerSet.Name, (Exception) ex);
            }
          }
        }
        if (!collectionManager.ContainsKey(""))
          collectionManager[""] = Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(this._serviceHandlerConfiguration);
      }
      else
      {
        this._serviceHandlerConfiguration = new Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration();
        this._serviceHandlerConfiguration.MaxClockSkew = this._serviceMaxClockSkew;
        if (!collectionManager.ContainsKey(""))
          collectionManager[""] = Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(this._serviceHandlerConfiguration);
      }
      return collectionManager;
    }

    protected Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration LoadHandlerConfiguration(
      ServiceElement element)
    {
      Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration handlerConfiguration = new Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration();
      try
      {
        handlerConfiguration.MaxClockSkew = !(this._serviceMaxClockSkew == ServiceConfiguration.DefaultMaxClockSkew) || !element.MaximumClockSkew.IsConfigured ? this._serviceMaxClockSkew : TimeSpan.Parse(element.MaximumClockSkew.Value);
      }
      catch (ArgumentException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) element, "maximumClockSkew", (Exception) ex);
      }
      if (element.AudienceUriElements.IsConfigured)
      {
        handlerConfiguration.AudienceRestriction.AudienceMode = element.AudienceUriElements.Mode;
        foreach (AudienceUriElement audienceUriElement in (ConfigurationElementCollection) element.AudienceUriElements)
          handlerConfiguration.AudienceRestriction.AllowedAudienceUris.Add(new Uri(audienceUriElement.Value, UriKind.RelativeOrAbsolute));
      }
      if (element.CertificateValidationElement.IsConfigured && element.CertificateValidationElement.CustomType.IsConfigured)
        handlerConfiguration.CertificateValidator = CustomTypeElement.Resolve<X509CertificateValidator>(element.CertificateValidationElement.CustomType);
      if (element.IssuerNameRegistry.IsConfigured)
        handlerConfiguration.IssuerNameRegistry = ServiceConfiguration.GetIssuerNameRegistry(element);
      if (element.IssuerTokenResolver.IsConfigured)
        handlerConfiguration.IssuerTokenResolver = ServiceConfiguration.GetIssuerTokenResolver(element);
      if (!string.IsNullOrEmpty(element.SaveBootstrapTokens))
      {
        try
        {
          handlerConfiguration.SaveBootstrapTokens = XmlConvert.ToBoolean(element.SaveBootstrapTokens.ToLowerInvariant());
        }
        catch (FormatException ex)
        {
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) element, "saveBootstrapTokens", (Exception) ex);
        }
      }
      if (element.ServiceTokenResolver.IsConfigured)
        handlerConfiguration.ServiceTokenResolver = ServiceConfiguration.GetServiceTokenResolver(element);
      if (element.TokenReplayDetectionElement.IsConfigured)
      {
        if (element.TokenReplayDetectionElement.CustomType.IsConfigured)
          handlerConfiguration.TokenReplayCache = CustomTypeElement.Resolve<Microsoft.IdentityModel.Tokens.TokenReplayCache>(element.TokenReplayDetectionElement.CustomType);
        if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.Enabled))
        {
          try
          {
            handlerConfiguration.DetectReplayedTokens = XmlConvert.ToBoolean(element.TokenReplayDetectionElement.Enabled.ToLowerInvariant());
          }
          catch (FormatException ex)
          {
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) element, "saveBootstrapTokens", (Exception) ex);
          }
        }
        if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.ExpirationPeriod))
          handlerConfiguration.TokenReplayCacheExpirationPeriod = TimeSpan.Parse(element.TokenReplayDetectionElement.ExpirationPeriod);
        if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.PurgeInterval))
          handlerConfiguration.TokenReplayCache.PurgeInterval = TimeSpan.Parse(element.TokenReplayDetectionElement.PurgeInterval);
        if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.Capacity))
          handlerConfiguration.TokenReplayCache.Capacity = int.Parse(element.TokenReplayDetectionElement.Capacity, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      return handlerConfiguration;
    }

    protected Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration LoadHandlerConfiguration(
      Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration baseConfiguration,
      SecurityTokenHandlerConfigurationElement element)
    {
      Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration handlerConfiguration = baseConfiguration == null ? new Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration() : baseConfiguration;
      if (element.AudienceUriElements.IsConfigured)
      {
        handlerConfiguration.AudienceRestriction.AudienceMode = AudienceUriMode.Always;
        handlerConfiguration.AudienceRestriction.AllowedAudienceUris.Clear();
        handlerConfiguration.AudienceRestriction.AudienceMode = element.AudienceUriElements.Mode;
        foreach (AudienceUriElement audienceUriElement in (ConfigurationElementCollection) element.AudienceUriElements)
          handlerConfiguration.AudienceRestriction.AllowedAudienceUris.Add(new Uri(audienceUriElement.Value, UriKind.RelativeOrAbsolute));
      }
      if (element.CertificateValidationElement.IsConfigured && element.CertificateValidationElement.CustomType.IsConfigured)
      {
        if (object.ReferenceEquals((object) baseConfiguration, (object) this._serviceHandlerConfiguration))
        {
          this.RevocationMode = ServiceConfiguration.DefaultRevocationMode;
          this.TrustedStoreLocation = ServiceConfiguration.DefaultTrustedStoreLocation;
          this.CertificateValidationMode = X509CertificateValidationMode.Custom;
        }
        handlerConfiguration.CertificateValidator = CustomTypeElement.Resolve<X509CertificateValidator>(element.CertificateValidationElement.CustomType);
      }
      if (element.IssuerNameRegistry.IsConfigured)
        handlerConfiguration.IssuerNameRegistry = CustomTypeElement.Resolve<Microsoft.IdentityModel.Tokens.IssuerNameRegistry>(element.IssuerNameRegistry);
      if (element.IssuerTokenResolver.IsConfigured)
        handlerConfiguration.IssuerTokenResolver = CustomTypeElement.Resolve<SecurityTokenResolver>(element.IssuerTokenResolver);
      try
      {
        if (element.MaximumClockSkew.IsConfigured)
          handlerConfiguration.MaxClockSkew = TimeSpan.Parse(element.MaximumClockSkew.Value);
      }
      catch (ArgumentException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) element, "maximumClockSkew", (Exception) ex);
      }
      if (!string.IsNullOrEmpty(element.SaveBootstrapTokens))
      {
        try
        {
          handlerConfiguration.SaveBootstrapTokens = XmlConvert.ToBoolean(element.SaveBootstrapTokens.ToLowerInvariant());
        }
        catch (FormatException ex)
        {
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) element, "saveBootstrapTokens", (Exception) ex);
        }
      }
      if (element.ServiceTokenResolver.IsConfigured)
        handlerConfiguration.ServiceTokenResolver = CustomTypeElement.Resolve<SecurityTokenResolver>(element.ServiceTokenResolver);
      if (element.TokenReplayDetectionElement.IsConfigured)
      {
        if (element.TokenReplayDetectionElement.CustomType.IsConfigured)
          handlerConfiguration.TokenReplayCache = CustomTypeElement.Resolve<Microsoft.IdentityModel.Tokens.TokenReplayCache>(element.TokenReplayDetectionElement.CustomType);
        if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.Enabled))
        {
          try
          {
            handlerConfiguration.DetectReplayedTokens = XmlConvert.ToBoolean(element.TokenReplayDetectionElement.Enabled.ToLowerInvariant());
          }
          catch (FormatException ex)
          {
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError((ConfigurationElement) element, "saveBootstrapTokens", (Exception) ex);
          }
        }
        if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.ExpirationPeriod))
          handlerConfiguration.TokenReplayCacheExpirationPeriod = TimeSpan.Parse(element.TokenReplayDetectionElement.ExpirationPeriod);
        if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.PurgeInterval))
          handlerConfiguration.TokenReplayCache.PurgeInterval = TimeSpan.Parse(element.TokenReplayDetectionElement.PurgeInterval);
        if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.Capacity))
          handlerConfiguration.TokenReplayCache.Capacity = int.Parse(element.TokenReplayDetectionElement.Capacity, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      return handlerConfiguration;
    }

    public TimeSpan MaxClockSkew
    {
      get => this._serviceHandlerConfiguration.MaxClockSkew;
      set => this._serviceHandlerConfiguration.MaxClockSkew = value;
    }

    public string Name => this._serviceName;

    public Microsoft.IdentityModel.Tokens.IssuerNameRegistry IssuerNameRegistry
    {
      get => this._serviceHandlerConfiguration.IssuerNameRegistry;
      set => this._serviceHandlerConfiguration.IssuerNameRegistry = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public Microsoft.IdentityModel.ExceptionMapper ExceptionMapper
    {
      get => this._exceptionMapper;
      set => this._exceptionMapper = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public SecurityTokenResolver IssuerTokenResolver
    {
      get => this._serviceHandlerConfiguration.IssuerTokenResolver;
      set => this._serviceHandlerConfiguration.IssuerTokenResolver = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public X509RevocationMode RevocationMode
    {
      get => this._revocationMode;
      set => this._revocationMode = value;
    }

    public X509Certificate2 ServiceCertificate
    {
      get => this._serviceCertificate;
      set => this._serviceCertificate = value;
    }

    public SecurityTokenResolver ServiceTokenResolver
    {
      get => this._serviceHandlerConfiguration.ServiceTokenResolver;
      set => this._serviceHandlerConfiguration.ServiceTokenResolver = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public bool SaveBootstrapTokens
    {
      get => this._serviceHandlerConfiguration.SaveBootstrapTokens;
      set => this._serviceHandlerConfiguration.SaveBootstrapTokens = value;
    }

    public Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager SecurityTokenHandlerCollectionManager => this._securityTokenHandlerCollectionManager;

    public Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection SecurityTokenHandlers => this._securityTokenHandlerCollectionManager[""];

    public Microsoft.IdentityModel.Tokens.TokenReplayCache TokenReplayCache
    {
      get => this._serviceHandlerConfiguration.TokenReplayCache;
      set => this._serviceHandlerConfiguration.TokenReplayCache = value;
    }

    public TimeSpan TokenReplayCacheExpirationPeriod
    {
      get => this._serviceHandlerConfiguration.TokenReplayCacheExpirationPeriod;
      set => this._serviceHandlerConfiguration.TokenReplayCacheExpirationPeriod = value;
    }

    public StoreLocation TrustedStoreLocation
    {
      get => this._trustedStoreLocation;
      set => this._trustedStoreLocation = value;
    }
  }
}
