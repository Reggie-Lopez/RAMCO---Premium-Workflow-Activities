// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.RoleDescriptor
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
  public abstract class RoleDescriptor
  {
    private Collection<ContactPerson> _contacts = new Collection<ContactPerson>();
    private Uri _errorUrl;
    private Collection<KeyDescriptor> _keys = new Collection<KeyDescriptor>();
    private Organization _organization;
    private Collection<Uri> _protocolsSupported = new Collection<Uri>();
    private DateTime _validUntil = DateTime.MaxValue;

    protected RoleDescriptor()
      : this(new Collection<Uri>())
    {
    }

    protected RoleDescriptor(Collection<Uri> protocolsSupported) => this._protocolsSupported = protocolsSupported;

    public ICollection<ContactPerson> Contacts => (ICollection<ContactPerson>) this._contacts;

    public Uri ErrorUrl
    {
      get => this._errorUrl;
      set => this._errorUrl = value;
    }

    public ICollection<KeyDescriptor> Keys => (ICollection<KeyDescriptor>) this._keys;

    public Organization Organization
    {
      get => this._organization;
      set => this._organization = value;
    }

    public ICollection<Uri> ProtocolsSupported => (ICollection<Uri>) this._protocolsSupported;

    public DateTime ValidUntil
    {
      get => this._validUntil;
      set => this._validUntil = value;
    }
  }
}
