// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.SingleSignOnDescriptor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class SingleSignOnDescriptor : RoleDescriptor
  {
    private IndexedProtocolEndpointDictionary _artifactResolutionServices = new IndexedProtocolEndpointDictionary();
    private Collection<ProtocolEndpoint> _singleLogoutServices = new Collection<ProtocolEndpoint>();
    private Collection<Uri> _nameIdFormats = new Collection<Uri>();

    public ICollection<Uri> NameIdentifierFormats => (ICollection<Uri>) this._nameIdFormats;

    public IndexedProtocolEndpointDictionary ArtifactResolutionServices => this._artifactResolutionServices;

    public Collection<ProtocolEndpoint> SingleLogoutServices => this._singleLogoutServices;
  }
}
