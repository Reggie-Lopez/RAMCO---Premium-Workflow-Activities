// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.EntityId
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class EntityId
  {
    private const int MaximumLength = 1024;
    private string _id;

    public EntityId()
      : this((string) null)
    {
    }

    public EntityId(string id) => this._id = id;

    public string Id
    {
      get => this._id;
      set => this._id = value == null || value.ToString().Length <= 1024 ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), SR.GetString("ID3199"));
    }
  }
}
