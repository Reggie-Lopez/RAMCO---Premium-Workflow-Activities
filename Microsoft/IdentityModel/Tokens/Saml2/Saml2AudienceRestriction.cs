// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2AudienceRestriction
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2AudienceRestriction
  {
    private Collection<Uri> _audiences = new Collection<Uri>();

    public Saml2AudienceRestriction()
    {
    }

    public Saml2AudienceRestriction(Uri audience)
      : this((IEnumerable<Uri>) new Uri[1]
      {
        audience
      })
    {
    }

    public Saml2AudienceRestriction(IEnumerable<Uri> audiences)
    {
      if (audiences == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (audiences));
      foreach (Uri audience in audiences)
      {
        if ((Uri) null == audience)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (audiences));
        this._audiences.Add(audience);
      }
    }

    public Collection<Uri> Audiences => this._audiences;
  }
}
