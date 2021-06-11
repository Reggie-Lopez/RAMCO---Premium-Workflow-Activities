// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.LocalizedName
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class LocalizedName : LocalizedEntry
  {
    private string _name;

    public LocalizedName()
      : this((string) null, (CultureInfo) null)
    {
    }

    public LocalizedName(string name, CultureInfo language)
      : base(language)
      => this._name = name;

    public string Name
    {
      get => this._name;
      set => this._name = value;
    }
  }
}
