// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.EntitiesDescriptor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class EntitiesDescriptor : MetadataBase
  {
    private Collection<EntitiesDescriptor> _entityGroupCollection = new Collection<EntitiesDescriptor>();
    private Collection<EntityDescriptor> _entityCollection = new Collection<EntityDescriptor>();
    private string _name;

    public EntitiesDescriptor()
      : this(new Collection<EntityDescriptor>(), new Collection<EntitiesDescriptor>())
    {
    }

    public EntitiesDescriptor(Collection<EntitiesDescriptor> entityGroupList) => this._entityGroupCollection = entityGroupList;

    public EntitiesDescriptor(Collection<EntityDescriptor> entityList) => this._entityCollection = entityList;

    public EntitiesDescriptor(
      Collection<EntityDescriptor> entityList,
      Collection<EntitiesDescriptor> entityGroupList)
    {
      this._entityCollection = entityList;
      this._entityGroupCollection = entityGroupList;
    }

    public ICollection<EntityDescriptor> ChildEntities => (ICollection<EntityDescriptor>) this._entityCollection;

    public ICollection<EntitiesDescriptor> ChildEntityGroups => (ICollection<EntitiesDescriptor>) this._entityGroupCollection;

    public string Name
    {
      get => this._name;
      set => this._name = value;
    }
  }
}
