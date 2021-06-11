// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Configuration.ServiceHostEndpointConfiguration
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Configuration
{
  [ComVisible(true)]
  public class ServiceHostEndpointConfiguration
  {
    private string _address;
    private Binding _binding;
    private System.Type _contractType;

    public ServiceHostEndpointConfiguration(System.Type contractType, Binding binding, string address)
    {
      if (binding == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (binding));
      if ((object) contractType == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (contractType));
      this._address = !string.IsNullOrEmpty(address) ? address : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (address));
      this._binding = binding;
      this._contractType = contractType;
    }

    public string Address => this._address;

    public Binding Binding => this._binding;

    public System.Type Contract => this._contractType;
  }
}
