// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.IdentityProviderSingleSignOnDescriptor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Tokens.Saml2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class IdentityProviderSingleSignOnDescriptor : SingleSignOnDescriptor
  {
    private bool _wantAuthenticationRequestsSigned;
    private Collection<ProtocolEndpoint> _singleSignOnServices = new Collection<ProtocolEndpoint>();
    private Collection<Saml2Attribute> _supportedAttributes = new Collection<Saml2Attribute>();

    public ICollection<ProtocolEndpoint> SingleSignOnServices => (ICollection<ProtocolEndpoint>) this._singleSignOnServices;

    public ICollection<Saml2Attribute> SupportedAttributes => (ICollection<Saml2Attribute>) this._supportedAttributes;

    public bool WantAuthenticationRequestsSigned
    {
      get => this._wantAuthenticationRequestsSigned;
      set => this._wantAuthenticationRequestsSigned = value;
    }
  }
}
