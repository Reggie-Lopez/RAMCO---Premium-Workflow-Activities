// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.ConfigureServiceHostServiceBehavior
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class ConfigureServiceHostServiceBehavior : IServiceBehavior
  {
    private string _serviceName;

    public ConfigureServiceHostServiceBehavior()
    {
    }

    public ConfigureServiceHostServiceBehavior(string serviceName) => this._serviceName = serviceName != null ? serviceName : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (serviceName));

    public void AddBindingParameters(
      ServiceDescription serviceDescription,
      ServiceHostBase serviceHostBase,
      Collection<ServiceEndpoint> endpoints,
      BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyDispatchBehavior(
      ServiceDescription serviceDescription,
      ServiceHostBase serviceHostBase)
    {
    }

    public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
      if (this._serviceName != null && !StringComparer.Ordinal.Equals(this._serviceName, Microsoft.IdentityModel.Configuration.ServiceConfiguration.DefaultServiceName))
        FederatedServiceCredentials.ConfigureServiceHost(serviceHostBase, this._serviceName);
      else
        FederatedServiceCredentials.ConfigureServiceHost(serviceHostBase);
    }
  }
}
