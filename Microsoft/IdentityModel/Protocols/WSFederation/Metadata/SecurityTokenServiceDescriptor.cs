// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.SecurityTokenServiceDescriptor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class SecurityTokenServiceDescriptor : WebServiceDescriptor
  {
    private Collection<EndpointAddress> _securityTokenServiceEndpoints = new Collection<EndpointAddress>();
    private Collection<EndpointAddress> _passiveRequestorEndpoints = new Collection<EndpointAddress>();

    public Collection<EndpointAddress> SecurityTokenServiceEndpoints => this._securityTokenServiceEndpoints;

    public Collection<EndpointAddress> PassiveRequestorEndpoints => this._passiveRequestorEndpoints;
  }
}
