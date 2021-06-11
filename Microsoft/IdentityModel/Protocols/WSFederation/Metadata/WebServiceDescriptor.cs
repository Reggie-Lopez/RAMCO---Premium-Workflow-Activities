// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.WebServiceDescriptor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSIdentity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public abstract class WebServiceDescriptor : RoleDescriptor
  {
    private Collection<DisplayClaim> _claimTypesOffered = new Collection<DisplayClaim>();
    private Collection<DisplayClaim> _claimTypesRequested = new Collection<DisplayClaim>();
    private string _serviceDisplayName;
    private string _serviceDescription;
    private Collection<EndpointAddress> _targetScopes = new Collection<EndpointAddress>();
    private Collection<Uri> _tokenTypesOffered = new Collection<Uri>();

    public ICollection<DisplayClaim> ClaimTypesOffered => (ICollection<DisplayClaim>) this._claimTypesOffered;

    public ICollection<DisplayClaim> ClaimTypesRequested => (ICollection<DisplayClaim>) this._claimTypesRequested;

    public string ServiceDescription
    {
      get => this._serviceDescription;
      set => this._serviceDescription = value;
    }

    public string ServiceDisplayName
    {
      get => this._serviceDisplayName;
      set => this._serviceDisplayName = value;
    }

    public ICollection<EndpointAddress> TargetScopes => (ICollection<EndpointAddress>) this._targetScopes;

    public ICollection<Uri> TokenTypesOffered => (ICollection<Uri>) this._tokenTypesOffered;
  }
}
