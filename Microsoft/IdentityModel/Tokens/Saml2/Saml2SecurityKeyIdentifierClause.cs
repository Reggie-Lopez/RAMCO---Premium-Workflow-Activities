// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityKeyIdentifierClause
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2SecurityKeyIdentifierClause : SecurityKeyIdentifierClause
  {
    private Saml2Assertion _assertion;

    public Saml2SecurityKeyIdentifierClause(Saml2Assertion assertion)
      : base(typeof (Saml2SecurityKeyIdentifierClause).ToString())
      => this._assertion = assertion;

    public Saml2Assertion Assertion => this._assertion;
  }
}
