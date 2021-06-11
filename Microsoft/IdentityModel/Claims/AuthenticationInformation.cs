// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.AuthenticationInformation
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
  [ComVisible(true)]
  public class AuthenticationInformation
  {
    private string _address;
    private Collection<AuthenticationContext> _authContexts;
    private string _dnsName;
    private DateTime? _notOnOrAfter;
    private string _session;

    public AuthenticationInformation() => this._authContexts = new Collection<AuthenticationContext>();

    public string Address
    {
      get => this._address;
      set => this._address = value;
    }

    public Collection<AuthenticationContext> AuthorizationContexts => this._authContexts;

    public string DnsName
    {
      get => this._dnsName;
      set => this._dnsName = value;
    }

    public DateTime? NotOnOrAfter
    {
      get => this._notOnOrAfter;
      set => this._notOnOrAfter = value;
    }

    public string Session
    {
      get => this._session;
      set => this._session = value;
    }
  }
}
