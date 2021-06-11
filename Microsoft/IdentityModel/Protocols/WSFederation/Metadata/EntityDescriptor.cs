// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.EntityDescriptor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class EntityDescriptor : MetadataBase
  {
    private Collection<ContactPerson> _contacts = new Collection<ContactPerson>();
    private EntityId _entityId;
    private string _federationId;
    private Organization _organization;
    private Collection<RoleDescriptor> _roleDescriptors = new Collection<RoleDescriptor>();

    public EntityDescriptor()
      : this((EntityId) null)
    {
    }

    public EntityDescriptor(EntityId entityId) => this._entityId = entityId;

    public ICollection<ContactPerson> Contacts => (ICollection<ContactPerson>) this._contacts;

    public EntityId EntityId
    {
      get => this._entityId;
      set => this._entityId = value;
    }

    public string FederationId
    {
      get => this._federationId;
      set => this._federationId = value;
    }

    public Organization Organization
    {
      get => this._organization;
      set => this._organization = value;
    }

    public ICollection<RoleDescriptor> RoleDescriptors => (ICollection<RoleDescriptor>) this._roleDescriptors;
  }
}
