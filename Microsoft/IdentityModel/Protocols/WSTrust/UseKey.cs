// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.UseKey
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class UseKey
  {
    private SecurityToken _token;
    private SecurityKeyIdentifier _ski;

    public UseKey()
    {
    }

    public UseKey(SecurityKeyIdentifier ski)
      : this(ski, (SecurityToken) null)
    {
    }

    public UseKey(SecurityToken token)
      : this((SecurityKeyIdentifier) null, token)
    {
    }

    public UseKey(SecurityKeyIdentifier ski, SecurityToken token)
    {
      this._ski = ski;
      this._token = token;
    }

    public SecurityToken Token => this._token;

    public SecurityKeyIdentifier SecurityKeyIdentifier => this._ski;
  }
}
