// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.Lifetime
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class Lifetime
  {
    private DateTime? _created;
    private DateTime? _expires;

    public Lifetime(DateTime created, DateTime expires)
      : this(new DateTime?(created), new DateTime?(expires))
    {
    }

    public Lifetime(DateTime? created, DateTime? expires)
    {
      if (created.HasValue && expires.HasValue && expires.Value <= created.Value)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID2000")));
      this._created = DateTimeUtil.ToUniversalTime(created);
      this._expires = DateTimeUtil.ToUniversalTime(expires);
    }

    public DateTime? Created
    {
      get => this._created;
      set => this._created = value;
    }

    public DateTime? Expires
    {
      get => this._expires;
      set => this._expires = value;
    }
  }
}
