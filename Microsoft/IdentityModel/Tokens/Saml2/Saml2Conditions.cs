// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2Conditions
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2Conditions
  {
    private Collection<Saml2AudienceRestriction> _audienceRestrictions = new Collection<Saml2AudienceRestriction>();
    private DateTime? _notBefore;
    private DateTime? _notOnOrAfter;
    private bool _oneTimeUse;
    private Saml2ProxyRestriction _proxyRestriction;

    public Collection<Saml2AudienceRestriction> AudienceRestrictions => this._audienceRestrictions;

    public DateTime? NotBefore
    {
      get => this._notBefore;
      set
      {
        value = DateTimeUtil.ToUniversalTime(value);
        if (value.HasValue && this._notOnOrAfter.HasValue && value.Value >= this._notOnOrAfter.Value)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID4116"));
        this._notBefore = value;
      }
    }

    public DateTime? NotOnOrAfter
    {
      get => this._notOnOrAfter;
      set
      {
        value = DateTimeUtil.ToUniversalTime(value);
        if (value.HasValue && this._notBefore.HasValue && value.Value <= this._notBefore.Value)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID4116"));
        this._notOnOrAfter = value;
      }
    }

    public bool OneTimeUse
    {
      get => this._oneTimeUse;
      set => this._oneTimeUse = value;
    }

    public Saml2ProxyRestriction ProxyRestriction
    {
      get => this._proxyRestriction;
      set => this._proxyRestriction = value;
    }
  }
}
