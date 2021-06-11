// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.FederatedClientCredentialsParameters
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class FederatedClientCredentialsParameters
  {
    private SecurityToken _actAs;
    private SecurityToken _onBehalfOf;
    private SecurityToken _issuedSecurityToken;

    public SecurityToken ActAs
    {
      get => this._actAs;
      set => this._actAs = value;
    }

    public SecurityToken OnBehalfOf
    {
      get => this._onBehalfOf;
      set => this._onBehalfOf = value;
    }

    public SecurityToken IssuedSecurityToken
    {
      get => this._issuedSecurityToken;
      set => this._issuedSecurityToken = value;
    }
  }
}
