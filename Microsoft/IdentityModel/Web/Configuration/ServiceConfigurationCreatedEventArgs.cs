// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.Configuration.ServiceConfigurationCreatedEventArgs
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Configuration;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web.Configuration
{
  [ComVisible(true)]
  public class ServiceConfigurationCreatedEventArgs : EventArgs
  {
    private ServiceConfiguration _serviceConfiguration;

    public ServiceConfigurationCreatedEventArgs(ServiceConfiguration config) => this._serviceConfiguration = config;

    public ServiceConfiguration ServiceConfiguration
    {
      get => this._serviceConfiguration;
      set => this._serviceConfiguration = value;
    }
  }
}
