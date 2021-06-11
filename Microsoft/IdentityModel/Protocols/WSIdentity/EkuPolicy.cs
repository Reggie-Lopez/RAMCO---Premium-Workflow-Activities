// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.EkuPolicy
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class EkuPolicy
  {
    private Collection<Oid> _oids = new Collection<Oid>();

    public EkuPolicy()
    {
    }

    public EkuPolicy(IEnumerable<Oid> oids)
    {
      if (oids == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (oids));
      foreach (Oid oid in oids)
      {
        if (oid == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (oids), SR.GetString("ID3283"));
        this._oids.Add(oid);
      }
    }

    public Collection<Oid> Oids => this._oids;
  }
}
