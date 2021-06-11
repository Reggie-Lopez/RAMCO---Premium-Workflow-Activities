// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2ProxyRestriction
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2ProxyRestriction
  {
    private Collection<Uri> _audiences = (Collection<Uri>) new AbsoluteUriCollection();
    private int? _count;

    public Collection<Uri> Audiences => this._audiences;

    public int? Count
    {
      get => this._count;
      set
      {
        if (value.HasValue && value.Value < 0)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value), Microsoft.IdentityModel.SR.GetString("ID0002"));
        this._count = value;
      }
    }
  }
}
