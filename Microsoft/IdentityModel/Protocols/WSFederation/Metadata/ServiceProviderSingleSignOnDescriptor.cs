// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.ServiceProviderSingleSignOnDescriptor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class ServiceProviderSingleSignOnDescriptor : SingleSignOnDescriptor
  {
    private bool _authenticationRequestsSigned;
    private bool _wantAssertionsSigned;
    private IndexedProtocolEndpointDictionary _assertionConsumingServices = new IndexedProtocolEndpointDictionary();

    public ServiceProviderSingleSignOnDescriptor()
      : this(new IndexedProtocolEndpointDictionary())
    {
    }

    public ServiceProviderSingleSignOnDescriptor(IndexedProtocolEndpointDictionary collection) => this._assertionConsumingServices = collection;

    public IndexedProtocolEndpointDictionary AssertionConsumerService => this._assertionConsumingServices;

    public bool AuthenticationRequestsSigned
    {
      get => this._authenticationRequestsSigned;
      set => this._authenticationRequestsSigned = value;
    }

    public bool WantAssertionsSigned
    {
      get => this._wantAssertionsSigned;
      set => this._wantAssertionsSigned = value;
    }
  }
}
