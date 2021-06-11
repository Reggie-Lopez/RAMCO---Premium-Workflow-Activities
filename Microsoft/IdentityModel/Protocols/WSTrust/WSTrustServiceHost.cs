// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustServiceHost
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Web.Configuration;
using System.Web.Hosting;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class WSTrustServiceHost : ServiceHost
  {
    private WSTrustServiceContract _serviceContract;

    public WSTrustServiceHost(
      SecurityTokenServiceConfiguration securityTokenServiceConfiguration,
      params Uri[] baseAddresses)
      : this(new WSTrustServiceContract(securityTokenServiceConfiguration), baseAddresses)
    {
    }

    public WSTrustServiceHost(WSTrustServiceContract serviceContract, params Uri[] baseAddresses)
      : base((object) serviceContract, baseAddresses)
    {
      if (serviceContract == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (serviceContract));
      this._serviceContract = serviceContract.SecurityTokenServiceConfiguration != null ? serviceContract : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serviceContract.SecurityTokenServiceConfiguration");
      if (this._serviceContract.SecurityTokenServiceConfiguration.ServiceCertificate != null)
        this.Credentials.ServiceCertificate.Certificate = this._serviceContract.SecurityTokenServiceConfiguration.ServiceCertificate;
      Collection<ServiceHostEndpointConfiguration> trustEndpoints = this._serviceContract.SecurityTokenServiceConfiguration.TrustEndpoints;
      for (int index = 0; index < trustEndpoints.Count; ++index)
      {
        ServiceHostEndpointConfiguration endpointConfiguration = trustEndpoints[index];
        if (string.IsNullOrEmpty(endpointConfiguration.Address))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3098"));
        this.AddServiceEndpoint(endpointConfiguration.Contract, endpointConfiguration.Binding, endpointConfiguration.Address);
      }
    }

    public WSTrustServiceContract ServiceContract => this._serviceContract;

    public SecurityTokenServiceConfiguration SecurityTokenServiceConfiguration => this._serviceContract.SecurityTokenServiceConfiguration;

    protected virtual void ConfigureMetadata()
    {
      if (this.BaseAddresses == null || this.BaseAddresses.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3140"));
      ServiceMetadataBehavior metadataBehavior = this.Description.Behaviors.Find<ServiceMetadataBehavior>();
      if (metadataBehavior == null)
      {
        metadataBehavior = new ServiceMetadataBehavior();
        this.Description.Behaviors.Add((IServiceBehavior) metadataBehavior);
      }
      bool flag = this.Description.Endpoints.Find(typeof (IMetadataExchange)) != null;
      Binding binding = (Binding) null;
      foreach (Uri baseAddress in this.BaseAddresses)
      {
        if (StringComparer.OrdinalIgnoreCase.Equals(baseAddress.Scheme, Uri.UriSchemeHttp))
        {
          metadataBehavior.HttpGetEnabled = true;
          binding = MetadataExchangeBindings.CreateMexHttpBinding();
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(baseAddress.Scheme, Uri.UriSchemeHttps))
        {
          metadataBehavior.HttpsGetEnabled = true;
          binding = MetadataExchangeBindings.CreateMexHttpsBinding();
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(baseAddress.Scheme, Uri.UriSchemeNetTcp))
          binding = MetadataExchangeBindings.CreateMexTcpBinding();
        else if (StringComparer.OrdinalIgnoreCase.Equals(baseAddress.Scheme, Uri.UriSchemeNetPipe))
          binding = MetadataExchangeBindings.CreateMexNamedPipeBinding();
        if (!flag && binding != null)
          this.AddServiceEndpoint("IMetadataExchange", binding, "mex");
        binding = (Binding) null;
      }
    }

    protected override void ApplyConfiguration()
    {
      base.ApplyConfiguration();
      if (((WSTrustServiceContract) this.SingletonInstance).SecurityTokenServiceConfiguration.DisableWsdl)
        return;
      this.ConfigureMetadata();
    }

    protected override void InitializeRuntime()
    {
      if (this.Description.Endpoints.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3097")));
      ServiceDebugBehavior serviceDebugBehavior = this.Description.Behaviors.Find<ServiceDebugBehavior>();
      if (HostingEnvironment.IsHosted)
      {
        foreach (Uri baseAddress in this.BaseAddresses)
        {
          CompilationSection section = (CompilationSection) WebConfigurationManager.OpenWebConfiguration(baseAddress.AbsolutePath).GetSection("system.web/compilation");
          if (section != null && section.Debug)
          {
            if (serviceDebugBehavior == null)
            {
              serviceDebugBehavior = new ServiceDebugBehavior();
              this.Description.Behaviors.Add((IServiceBehavior) serviceDebugBehavior);
            }
            serviceDebugBehavior.IncludeExceptionDetailInFaults = true;
            break;
          }
        }
      }
      this.InitializeSecurityTokenManager();
      base.InitializeRuntime();
    }

    protected virtual void InitializeSecurityTokenManager() => FederatedServiceCredentials.ConfigureServiceHost((ServiceHostBase) this, (Microsoft.IdentityModel.Configuration.ServiceConfiguration) this._serviceContract.SecurityTokenServiceConfiguration);
  }
}
