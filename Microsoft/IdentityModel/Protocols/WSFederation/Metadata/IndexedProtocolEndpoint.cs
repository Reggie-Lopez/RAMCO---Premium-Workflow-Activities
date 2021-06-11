// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.IndexedProtocolEndpoint
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class IndexedProtocolEndpoint : ProtocolEndpoint
  {
    private int _index;
    private bool? _isDefault = new bool?();

    public IndexedProtocolEndpoint()
    {
    }

    public IndexedProtocolEndpoint(int index, Uri binding, Uri location)
      : base(binding, location)
      => this._index = index;

    public int Index
    {
      get => this._index;
      set => this._index = value;
    }

    public bool? IsDefault
    {
      get => this._isDefault;
      set => this._isDefault = value;
    }
  }
}
