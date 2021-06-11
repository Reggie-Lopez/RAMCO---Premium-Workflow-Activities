// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.AudienceRestriction
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Selectors;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class AudienceRestriction
  {
    private AudienceUriMode _audienceMode = AudienceUriMode.Always;
    private Collection<Uri> _audience = new Collection<Uri>();

    public AudienceRestriction()
    {
    }

    public AudienceRestriction(AudienceUriMode audienceMode) => this._audienceMode = audienceMode;

    public AudienceUriMode AudienceMode
    {
      get => this._audienceMode;
      set => this._audienceMode = value;
    }

    public Collection<Uri> AllowedAudienceUris => this._audience;
  }
}
