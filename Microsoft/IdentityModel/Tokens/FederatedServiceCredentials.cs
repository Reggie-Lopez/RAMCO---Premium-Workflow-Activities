// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.FederatedServiceCredentials
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class FederatedServiceCredentials : ServiceCredentials
  {
    private bool _saveBootstrapTokenInSession = true;
    private Microsoft.IdentityModel.Configuration.ServiceConfiguration _configuration;

    public FederatedServiceCredentials()
      : this(new ServiceCredentials(), new Microsoft.IdentityModel.Configuration.ServiceConfiguration())
    {
    }

    public FederatedServiceCredentials(ServiceCredentials innerServiceCredentials)
      : this(innerServiceCredentials, new Microsoft.IdentityModel.Configuration.ServiceConfiguration())
    {
    }

    public FederatedServiceCredentials(Microsoft.IdentityModel.Configuration.ServiceConfiguration configuration)
      : this(new ServiceCredentials(), configuration)
    {
    }

    public FederatedServiceCredentials(
      ServiceCredentials innerServiceCredentials,
      Microsoft.IdentityModel.Configuration.ServiceConfiguration configuration)
      : base(innerServiceCredentials)
    {
      this._configuration = configuration;
    }

    protected FederatedServiceCredentials(FederatedServiceCredentials other)
      : base((ServiceCredentials) other)
    {
      this._configuration = other != null ? other._configuration : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (other));
      this._saveBootstrapTokenInSession = other._saveBootstrapTokenInSession;
    }

    protected override ServiceCredentials CloneCore() => (ServiceCredentials) new FederatedServiceCredentials(this);

    public override SecurityTokenManager CreateSecurityTokenManager() => (SecurityTokenManager) new FederatedSecurityTokenManager((ServiceCredentials) this, this._configuration.SecurityTokenHandlers, this._configuration.ClaimsAuthenticationManager)
    {
      ExceptionMapper = this._configuration.ExceptionMapper
    };

    public static void ConfigureServiceHost(ServiceHostBase serviceHost)
    {
      if (serviceHost == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (serviceHost));
      FederatedServiceCredentials.ConfigureServiceHost(serviceHost, new Microsoft.IdentityModel.Configuration.ServiceConfiguration());
    }

    public static void ConfigureServiceHost(ServiceHostBase serviceHost, string serviceName)
    {
      if (serviceHost == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (serviceHost));
      FederatedServiceCredentials.ConfigureServiceHost(serviceHost, new Microsoft.IdentityModel.Configuration.ServiceConfiguration(serviceName));
    }

    public static void ConfigureServiceHost(
      ServiceHostBase serviceHost,
      Microsoft.IdentityModel.Configuration.ServiceConfiguration configuration)
    {
      if (serviceHost == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (serviceHost));
      if (configuration == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (configuration));
      if (serviceHost.State != CommunicationState.Created && serviceHost.State != CommunicationState.Opening)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4041", (object) serviceHost));
      ServiceCredentials innerServiceCredentials = serviceHost.Description.Behaviors.Find<ServiceCredentials>();
      if (!(innerServiceCredentials is FederatedServiceCredentials))
      {
        serviceHost.Description.Behaviors.Remove<ServiceCredentials>();
        FederatedServiceCredentials serviceCredentials = innerServiceCredentials == null ? new FederatedServiceCredentials(new ServiceCredentials(), configuration) : new FederatedServiceCredentials(innerServiceCredentials, configuration);
        if (configuration.ServiceCertificate != null)
          serviceCredentials.ServiceCertificate.Certificate = configuration.ServiceCertificate;
        else if (serviceCredentials.ServiceCertificate != null && serviceCredentials.ServiceCertificate.Certificate != null)
          configuration.ServiceCertificate = serviceCredentials.ServiceCertificate.Certificate;
        if (object.ReferenceEquals((object) configuration.IssuerTokenResolver, (object) SecurityTokenHandlerConfiguration.DefaultIssuerTokenResolver) && serviceCredentials.IssuedTokenAuthentication != null && (serviceCredentials.IssuedTokenAuthentication.KnownCertificates != null && serviceCredentials.IssuedTokenAuthentication.KnownCertificates.Count > 0))
        {
          List<SecurityToken> securityTokenList = new List<SecurityToken>();
          foreach (X509Certificate2 knownCertificate in (IEnumerable<X509Certificate2>) serviceCredentials.IssuedTokenAuthentication.KnownCertificates)
            securityTokenList.Add((SecurityToken) new X509SecurityToken(knownCertificate));
          SecurityTokenResolver securityTokenResolver = SecurityTokenResolver.CreateDefaultSecurityTokenResolver(securityTokenList.AsReadOnly(), false);
          configuration.IssuerTokenResolver = (SecurityTokenResolver) new AggregateTokenResolver((IEnumerable<SecurityTokenResolver>) new SecurityTokenResolver[2]
          {
            securityTokenResolver,
            SecurityTokenHandlerConfiguration.DefaultIssuerTokenResolver
          });
        }
        if (!configuration.IsInitialized)
          configuration.Initialize();
        serviceCredentials.ClaimsAuthenticationManager = configuration.ClaimsAuthenticationManager;
        serviceCredentials.ClaimsAuthorizationManager = configuration.ClaimsAuthorizationManager;
        if (serviceHost.Description.Behaviors.Find<ServiceAuthorizationBehavior>() == null)
        {
          ServiceAuthorizationBehavior authorizationBehavior = new ServiceAuthorizationBehavior();
          serviceHost.Description.Behaviors.Add((IServiceBehavior) authorizationBehavior);
        }
        serviceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
        serviceHost.Description.Behaviors.Add((IServiceBehavior) serviceCredentials);
      }
      if (serviceHost.Authorization.ServiceAuthorizationManager == null)
        serviceHost.Authorization.ServiceAuthorizationManager = (ServiceAuthorizationManager) new IdentityModelServiceAuthorizationManager();
      else if (!(serviceHost.Authorization.ServiceAuthorizationManager is IdentityModelServiceAuthorizationManager))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID4039")));
      if (configuration.SecurityTokenHandlers[typeof (SecurityContextSecurityToken)] == null || serviceHost.Credentials.SecureConversationAuthentication.SecurityStateEncoder != null)
        return;
      serviceHost.Credentials.SecureConversationAuthentication.SecurityStateEncoder = (SecurityStateEncoder) new NoOpSecurityStateEncoder();
    }

    public ClaimsAuthenticationManager ClaimsAuthenticationManager
    {
      get => this._configuration.ClaimsAuthenticationManager;
      set => this._configuration.ClaimsAuthenticationManager = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public Microsoft.IdentityModel.ExceptionMapper ExceptionMapper
    {
      get => this._configuration.ExceptionMapper;
      set => this._configuration.ExceptionMapper = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public ClaimsAuthorizationManager ClaimsAuthorizationManager
    {
      get => this._configuration.ClaimsAuthorizationManager;
      set => this._configuration.ClaimsAuthorizationManager = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public TimeSpan MaxClockSkew => this._configuration.MaxClockSkew;

    public bool SaveBootstrapTokens => this._configuration.SecurityTokenHandlers.Configuration.SaveBootstrapTokens;

    public SecurityTokenHandlerCollectionManager SecurityTokenHandlerCollectionManager => this._configuration.SecurityTokenHandlerCollectionManager;

    public SecurityTokenHandlerCollection SecurityTokenHandlers => this._configuration.SecurityTokenHandlerCollectionManager[""];

    internal Microsoft.IdentityModel.Configuration.ServiceConfiguration ServiceConfiguration => this._configuration;
  }
}
