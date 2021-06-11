// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.RequestClaim
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class RequestClaim
  {
    private string _claimType;
    private bool _isOptional;
    private string _value;

    public RequestClaim(string claimType)
      : this(claimType, false)
    {
    }

    public RequestClaim(string claimType, bool isOptional)
      : this(claimType, isOptional, (string) null)
    {
    }

    public RequestClaim(string claimType, bool isOptional, string value)
    {
      this._claimType = !string.IsNullOrEmpty(claimType) ? claimType : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (claimType)));
      this._isOptional = isOptional;
      this._value = value;
    }

    public string ClaimType => this._claimType;

    public bool IsOptional
    {
      get => this._isOptional;
      set => this._isOptional = value;
    }

    public string Value
    {
      get => this._value;
      set => this._value = value;
    }
  }
}
