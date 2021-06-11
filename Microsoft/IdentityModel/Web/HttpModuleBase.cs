// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.HttpModuleBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Configuration;
using System.Runtime.InteropServices;
using System.Web;

namespace Microsoft.IdentityModel.Web
{
  [ComVisible(true)]
  public abstract class HttpModuleBase : IHttpModule
  {
    private ServiceConfiguration _serviceConfiguration;

    public ServiceConfiguration ServiceConfiguration
    {
      get => this._serviceConfiguration;
      set => this._serviceConfiguration = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public virtual void Dispose()
    {
    }

    public void Init(HttpApplication context)
    {
      this._serviceConfiguration = FederatedAuthentication.ServiceConfiguration;
      this.InitializeModule(context);
    }

    protected abstract void InitializeModule(HttpApplication context);
  }
}
